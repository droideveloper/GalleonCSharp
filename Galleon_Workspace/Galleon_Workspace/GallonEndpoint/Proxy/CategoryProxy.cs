using GalleonEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GalleonEndpoint.Proxy {
   
    [DataContract]
    public class CategoryProxy {

        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public string CategoryName { get; set; }

        public CategoryProxy(Category category) {
            CategoryID = category.CategoryID;
            CategoryName = category.CategoryName;
        }

        public static List<CategoryProxy> FromCategories(List<Category> categories) {
            return categories.IsNullOrEmpty() ? null : categories.Select(c => new CategoryProxy(c)).ToList();
        }
    }
}