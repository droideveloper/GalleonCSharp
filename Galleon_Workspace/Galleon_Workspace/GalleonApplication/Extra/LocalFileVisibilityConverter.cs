﻿using GalleonApplication.App_Data;
using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GalleonApplication.Extra {
    
    public class LocalFileVisibilityConverter : IValueConverter {

        private DbManager dbManager;

        public LocalFileVisibilityConverter() {
            dbManager = DependencyInjector.Get<DbManager>();
            //force load
            dbManager.Syncables.Count();
        }
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            DocumentEntity entity = value as DocumentEntity;
            if(!entity.IsNullOrEmpty()) {
                Syncable sync = dbManager.Syncables.FirstOrDefault(x => x.RemoteID == entity.DocumentID);
                return !sync.IsNullOrEmpty();
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return false;
        }
    }
}
