using GalleonApplication.App_Data;
using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GalleonApplication.Extra {

    public static class Collections {

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) {
            return collection == null || collection.Count <= 0;
        }

        public static bool IsNullOrEmpty<T>(this T obj) {
            return obj == null || (obj is string || obj is String) ? string.IsNullOrEmpty(obj as string) : false;  
        }

        public static bool IsNullOrEmpty<T>(this T obj, Func<T, bool> func) {
            if(obj.IsNullOrEmpty()) {
                return true;
            }    
            return func(obj);
        }

        public static ICollection<T> Filter<T>(this ICollection<T> collection, Func<T, bool> filter) {
            ICollection<T> newCollection = new List<T>();
            if(IsNullOrEmpty(collection)) {
                throw new ArgumentNullException("collection is null or empty");
            }
            if(filter == null) {
                throw new ArgumentNullException("filter is null");
            }
            for(int i = 0, z = collection.Count; i < z; i++) {
                T item = collection.ElementAt(i);
                if(filter(item)) {
                    newCollection.Add(item.Copy());
                }
            }
            return newCollection;
        }

        public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> filter) {
            if(items == null || items.Count() == 0)
                throw new ArgumentNullException("items is null or empty");
            if(filter == null)
                throw new ArgumentNullException("filter is null");
            for(int index = 0, z = items.Count(); index < z; index++) {
                T item = items.ElementAt(index);
                if(filter(item))
                    return index;
            }
            return -1;
        }

        public static int IndexOf<T>(this IEnumerable<T> items, T item) {
            return items.IndexOf(i => EqualityComparer<T>.Default.Equals(item, i));
        }

        public static T Copy<T>(this T obj) {
            if(obj == null) {
                throw new ArgumentNullException("object to clone is null");
            }
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binary = new BinaryFormatter();
            binary.Serialize(stream, obj);
            stream.Position = 0;
            return (T) binary.Deserialize(stream);
        }

        public static bool IsEquals<T>(this T o, T other) {
            return EqualityComparer<T>.Default.Equals(other);
        }

        public static T ToEnum<T>(this string str) {
            return (T) Enum.Parse(typeof(T), str, true);
        }

        public static IObservable<Response<K>> ToResponse<K>(this IObservable<HttpResponseMessage> current) {
            if(current.IsNullOrEmpty()) {
                throw new NullReferenceException("reference is null");
            }
            return current.SelectMany(x => {
                string json = x.Content.ReadAsStringAsync().Result;
                return Observable.Return(Response<K>.Deserialize(json));
            });   
        }

        public static IObservable<Stream> ToStream(this IObservable<HttpResponseMessage> source) {
            if(source.IsNullOrEmpty()) { return Observable.Empty<Stream>(); }
            return source.SelectMany(x => {
                return Observable.Return(x.Content.ReadAsStreamAsync().Result);
            });
        }

        public static void Each<T>(this IEnumerable<T> e, Action<int, T> forEach) {
            if(e.IsNullOrEmpty()) { return; }
            e.ToList()
             .ForEach(item => {
                 forEach(e.IndexOf(item), item);
             });
        }

        public static void EachNotifyLast<T>(this IEnumerable<T> e, Action<bool, T> forEachNotifyLast) {
            if(e.IsNullOrEmpty()) { return; }
            e.ToList()
             .ForEach(item => {
                 forEachNotifyLast(e.IndexOf(item) == e.Count() - 1, item);
             });
        }

        public static bool IsFileAccessible(this FileInfo f) {
            if(f.IsNullOrEmpty()) { return false; }
            FileStream stream = null;
            try {
                stream = f.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            } catch(IOException) {
                return false;
            } finally {
                if(!stream.IsNullOrEmpty()) {
                    stream.Close();
                }
            }
            return true;
        }

        public static string ToFormat(this string formatStr, params object[] args) {
            return string.Format(formatStr, args);
        }

        public static FileInfo ToCompress(this FileInfo source) { 
            if(source.IsNullOrEmpty()) { return null; }
            if(!source.IsFileAccessible()) { return null; }
            //open destination file
            using(FileStream inp = source.OpenRead()) {
                //create output
                FileInfo output = new FileInfo(source.FullName + ".gz");
                //delete if exits
                if(output.Exists) { output.Delete(); }
                //create new file
                using(FileStream outp = File.Create(output.FullName))
                using(GZipStream compress = new GZipStream(outp, CompressionMode.Compress)) {
                    //copy it
                    inp.CopyTo(compress); 
                    //return compressed output as stream
                    return output;
                }
            }
        }

        public static IObservable<FileInfo> ToCompress(this IObservable<FileInfo> source) {
            if(source.IsNullOrEmpty()) { return Observable.Never<FileInfo>(); }
            return source.SelectMany(f => {
                //file is not used
                if(!f.IsFileAccessible()) { return Observable.Never<FileInfo>(); }
                //open destination file
                using(FileStream inp = f.OpenRead()) {
                    //create output
                    FileInfo output = new FileInfo(f.FullName + ".gz");
                    //delete if exits
                    if(output.Exists) { output.Delete(); }
                    //create new file
                    using(FileStream outp = File.Create(output.FullName))                    
                    using(GZipStream compress = new GZipStream(outp, CompressionMode.Compress)){
                        //copy it
                        inp.CopyTo(compress);
                        //closing is esential
                        compress.Close();//close it
                        outp.Close();//close it
                        inp.Close();//close it
                        //return compressed output as stream
                        return Observable.Return<FileInfo>(output);
                    }
                }
            });
        }

        public static IObservable<FileInfo> ToFile(this IObservable<Stream> source, Syncable syncable) {
            if(source.IsNullOrEmpty()) { return Observable.Never<FileInfo>(); }
            return source.SelectMany(inp => {
                //creates directory for file path if the directory is not exists.
                string directory = Path.GetDirectoryName(syncable.LocalPath);
                if(!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }
                //check file 
                FileInfo outp = new FileInfo(syncable.LocalPath);
                //delete if there is any such file
                if(outp.Exists) { outp.Delete(); }
                using(FileStream output = File.Create(outp.FullName))
                using(GZipStream compress = new GZipStream(inp, CompressionMode.Decompress)) {
                    //create stream and copy
                    compress.CopyTo(output);
                    //close compression
                    compress.Close();
                    output.Close();
                    //retrun observable
                    return Observable.Return(new FileInfo(syncable.LocalPath));
                }
            });
        }

        public static IObservable<string> ToFileString(this IObservable<DocumentEntity> source, CustomerEntity customer, string syncPath) {
            if(source.IsNullOrEmpty()) { return Observable.Empty<string>(); }
            return source.SelectMany(x => {
                return Observable.Return(Path.Combine(syncPath, 
                                                      customer.CustomerID.ToString(),
                                                      x.DocumentName)); 
            });
        }

        public static IObservable<T> ThoretteEvent<T>(this T item, TimeSpan delay) where T : IEvent {
            if(item.IsNullOrEmpty()) { return Observable.Empty<T>(); }
            return Observable.Return(item)
                             .Throttle(delay);   
        }

        //best extension method to use
        public static void PersistException(this Exception error) {
            if(error.IsNullOrEmpty()) { return; }
            //if those are not null, I can persist ya.
            DbManager dbManager = DependencyInjector.Get<DbManager>();
            LogException parentException = new LogException() {
                Message = error.Message,
                StackTrace = error.StackTrace,
                CreateDate = DateTime.Now
            };
            dbManager.LogExceptions.Add(parentException);
            dbManager.SaveChanges();

            if(!error.InnerException.IsNullOrEmpty()) {
                LogException childException = new LogException() {
                    ParentLogExceptionID = parentException.LogExceptionID,
                    Message = error.InnerException.Message,
                    StackTrace = error.InnerException.StackTrace,
                    CreateDate = DateTime.Now
                };

                dbManager.LogExceptions.Add(childException);
                dbManager.SaveChanges();
            }            
        }

        //we gone perist error here
        public static void PersistResponseError<T>(this Response<T> errorResponse) {
            if(errorResponse.IsNullOrEmpty()) { return; }
            DbManager dbManager = DependencyInjector.Get<DbManager>();
            //create entity log
            string errorFormat = "Message: {0} Code: {1}";
            LogException error = new LogException() {
                Message = errorResponse.Message,
                StackTrace = errorFormat.ToFormat(errorResponse.Message, errorResponse.Code),
                CreateDate = DateTime.Now
            };
            //store object
            dbManager.LogExceptions.Add(error);
            dbManager.SaveChanges();
        }

        public static IObservable<List<FileInfo>> ToFileList(this List<string> source) {
            if(source.IsNullOrEmpty()) { return Observable.Never<List<FileInfo>>(); }
            return Observable.Return(source.Select(x => {
                return new FileInfo(x);
            }).ToList());
        }

        public static List<string> ToFileList(this DragEventArgs e) {
            if(e.HaveFiles()) {
                string[] array = e.Data.GetData(DataFormats.FileDrop) as string[];
                return array.ToList();
            }
            return new List<string>();
        }

        public static bool HaveFiles(this DragEventArgs e) {
            return e.IsNullOrEmpty() ? false : e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        //Non extension helpers
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> property) {
            MemberExpression expression = property.Body as MemberExpression;
            if(expression.IsNullOrEmpty()) { 
                throw new ArgumentNullException(string.Format("Expression '{0}' refers to a method, not a property", property.ToString())); 
            }
            PropertyInfo propertyInfo = expression.Member as PropertyInfo;
            if(propertyInfo.IsNullOrEmpty()) { 
                throw new ArgumentNullException(string.Format("Expression '{0}' refers to a field, not a property", property.ToString())); 
            }
            return propertyInfo;
        }

        public static string NameOfProperty<T>(Expression<Func<T>> property) {
            PropertyInfo propertyInfo = GetPropertyInfo<T>(property);
            return propertyInfo.Name;
        }
    }
}
