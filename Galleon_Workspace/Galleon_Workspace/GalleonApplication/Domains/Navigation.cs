using System;
using System.Collections.Generic;
using GalleonApplication.Extra;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Domains {
    
    public class Navigation : IPropertyChanged {

        private const string TITLE_STR          = "Title";
        private const string CONTENT_VIEW_STR   = "ContentView";
        private const string IS_SELECTED_STR    = "IsSelected";

        private bool mIsSelected;
        private string mTitle;
        private object mContentView;

        public bool IsSelected {
            get {
                return mIsSelected;
            }
            set {
                setProperty(ref mIsSelected, value, IS_SELECTED_STR);
            }
        }

        public string Title {
            get {
                return mTitle;
            }
            set {
                setProperty(ref mTitle, value, TITLE_STR);
            }
        }

        public object ContentView {
            get {
                return mContentView;
            }
            set {
                setProperty(ref mContentView, value, CONTENT_VIEW_STR);
            }
        }
    }
}
