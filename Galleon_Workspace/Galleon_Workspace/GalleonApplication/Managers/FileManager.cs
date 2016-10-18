using GalleonApplication.App_Data;
using GalleonApplication.Extra;
using GalleonApplication.Extra.Net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GalleonApplication.Managers {
    
    public class FileManager : IFileManager {

        private const string ANY_FILE = "*.*";
        private const long   INTERVAL = 60;//1 min or 60 secs       

        private IDisposable        subscription;
        private IPreferenceManager preferenceManager;
        private IEndpointClient    endpointClient;
        private IFileClient        fileClient;
        private DbManager          dbManager;

        public FileManager(DbManager dbManager, IPreferenceManager preferenceManager, IEndpointClient endpointClient, IFileClient fileClient) {
            this.dbManager = dbManager;
            this.preferenceManager = preferenceManager;
            this.endpointClient = endpointClient;
            this.fileClient = fileClient;
        }

        public void start() {
            subscription = OnStart;
        }

        public void stop() {
            if(!subscription.IsNullOrEmpty()) {
                subscription.Dispose();
                subscription = null;
            }
        }

        //TODO test Operation Download, Upload and None operation, 
        //60 secs may not be big enough for large sync so we need to 
        //do is just check the existance of those files with proper 
        //aspects and those aspects will remain in proper spec
        protected IDisposable OnStart {
            get {
                return Observable.Interval(TimeSpan.FromSeconds(INTERVAL))
                                 .SelectMany(x => {
                                     bool autoSync = preferenceManager.getValue(PreferenceManager.KEY_SYNC_ALLOWED, false);
                                     if(autoSync) {
                                         return Observable.Return(dbManager.Syncables.ToList());
                                     }
                                     return Observable.Never<List<Syncable>>();
                                 })
                                 .SelectMany(x => x)
                                 .SelectMany(x => {
                                     string syncDirectory = preferenceManager.getValue(PreferenceManager.KEY_SYNC_FOLDER, string.Empty);
                                     if(syncDirectory.IsNullOrEmpty()) { return Observable.Never<Match>(); }
                                     if(Directory.Exists(syncDirectory)) {
                                         DirectoryInfo dir = new DirectoryInfo(syncDirectory);
                                         return Observable.Return(dir.GetFiles(ANY_FILE, SearchOption.AllDirectories)
                                                                     .Where(y => y.FullName.Equals(x.LocalPath))
                                                                     .Select(y => new Match() {
                                                                         Operation = x.LastModified != y.LastWriteTime ? Operation.UPLOAD : Operation.NONE,
                                                                         File = y,
                                                                         Sync = x
                                                                     })
                                                                     .FirstOrDefault());
                                     }
                                     return Observable.Never<Match>();
                                 })
                                 .SelectMany(x => {
                                     if(x.Operation.Equals(Operation.NONE)) {
                                         return endpointClient.queryDocument(x.Sync.RemoteID)
                                                              .ToResponse<DocumentEntity>()
                                                              .SelectMany(y => {
                                                                  if(y.Code == ResponseCode.OK) {
                                                                      DocumentEntity docx = y.Data;
                                                                      if(docx.IsNullOrEmpty()) {//no remote existance there so we gone delete it 
                                                                          return Observable.Return(new Match() {
                                                                              Operation = Operation.DELETE,
                                                                              File = x.File,
                                                                              Sync = x.Sync,
                                                                              Docx = docx
                                                                          });
                                                                      } else {
                                                                          return Observable.Return(new Match() {//this is download newer one in the remote server
                                                                              Operation = docx.UpdateDate != x.Sync.LastModified ? Operation.DOWNLOAD : Operation.NONE,
                                                                              File = x.File,
                                                                              Sync = x.Sync,
                                                                              Docx = docx
                                                                          });
                                                                      }
                                                                  } else {
                                                                      y.PersistResponseError();
                                                                  }
                                                                  return Observable.Return(x);
                                                              });
                                     }
                                     return Observable.Return(x);
                                 })
                                 .SelectMany(x => {
                                     if(x.Operation.Equals(Operation.DELETE)) {
                                         dbManager.Syncables.Remove(x.Sync);
                                         dbManager.SaveChanges();
                                         if(x.File.Exists) {
                                             x.File.Delete();//we removed it from file system in local
                                         }
                                         return Observable.Return(new Pair() {
                                             Match = x,
                                             State = true
                                         });
                                     } else if(x.Operation.Equals(Operation.DOWNLOAD)) { 
                                         if(x.File.Exists) {
                                            x.File.Delete();
                                         }
                                         return fileClient.downloadContent(x.Sync.RemoteID)
                                                          .ToStream()
                                                          .ToFile(x.Sync)
                                                          .SelectMany(y => {
                                                              y.LastWriteTime = x.Docx.UpdateDate;//important part that we change data on the file set with this manipulation, store in file system
                                                              x.Sync.LastModified = x.Docx.UpdateDate;//this is acess date thing
                                                              x.Sync.LocalPath = y.FullName;//path is reset
                                                              x.Sync.RemoteID = x.Docx.DocumentID;//might be changed I don't know
                                                              dbManager.Entry(x.Sync).State = EntityState.Modified;
                                                              dbManager.SaveChanges();
                                                              return Observable.Return(new Pair() {
                                                                  Match = new Match() {
                                                                      File = y,
                                                                      Sync = x.Sync,
                                                                      Operation = x.Operation,
                                                                      Docx = x.Docx
                                                                  },
                                                                  State = true
                                                              });
                                                          });
                                     } else if(x.Operation.Equals(Operation.UPLOAD)) {
                                         FileInfo compressed = x.File.ToCompress();
                                         if(compressed.IsNullOrEmpty()) {
                                             return Observable.Never<Pair>();//failed in io I guess
                                         } else { 
                                            x.Docx.UpdateDate = x.File.LastWriteTime;
                                            x.Docx.ContentLength = x.File.Length;
                                            return endpointClient.updateDocument(x.Docx)
                                                                 .ToResponse<DocumentEntity>()
                                                                 .SelectMany(y => {
                                                                     if(y.Code == ResponseCode.OK) {
                                                                         return Observable.Return(y.Data);
                                                                     } else {
                                                                         y.PersistResponseError();
                                                                         return Observable.Never<DocumentEntity>();
                                                                     }
                                                                 })
                                                                 .SelectMany(y => {
                                                                     return fileClient.updateContent(y.DocumentID, compressed.OpenRead())
                                                                                      .ToResponse<bool>();
                                                                 })
                                                                 .SelectMany(y => {
                                                                     if(compressed.Exists) {
                                                                         compressed.Delete();//clean up the uploaded compressed file
                                                                     }
                                                                     if(y.Code != ResponseCode.OK) {
                                                                         y.PersistResponseError();//log error
                                                                     }
                                                                     return Observable.Return(new Pair() {
                                                                         Match = x,
                                                                         State = y.Data
                                                                     });
                                                                 });
                                         }
                                     } else {
                                         return Observable.Return(new Pair() {
                                             Match = x,
                                             State = false//no operation done state false is acceptable
                                         });
                                     }
                                 })
                                 .SubscribeOn(ThreadPoolScheduler.Instance)
                                 .ObserveOnDispatcher(DispatcherPriority.Background)
                                 .Subscribe(x => {
                                     if(x.State) {
                                         new Response<Pair>() {
                                             Code = 0x01,
                                             Message = "Sync of FileManager Logs",
                                             Data = x
                                         }.PersistResponseError();//not error but want to persist it
                                     }
                                 }, error => {
                                     error.PersistException();
                                 });
            }
        }
    }

    internal class Pair {
        public Match Match { get; set; }
        public bool  State { get; set; }
    }

    internal class Match {
        public Operation        Operation { get; set; }
        public FileInfo         File      { get; set; }
        public Syncable         Sync      { get; set; }
        public DocumentEntity   Docx      { get; set; }
    }

    internal enum Operation { 
        UPLOAD,
        DOWNLOAD,
        DELETE,
        NONE
    }
}
