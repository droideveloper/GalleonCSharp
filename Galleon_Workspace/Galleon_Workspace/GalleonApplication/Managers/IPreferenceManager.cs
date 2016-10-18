using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Managers {
    
    public interface IPreferenceManager {

        /// <summary>
        /// checks if key exits
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool hasKey(string key);

        /// <summary>
        /// puts specified value for key or creates it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void putValue<T>(string key, T value);

        /// <summary>
        /// gets sepecified value or default if not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T getValue<T>(string key, T defaultValue);

        /// <summary>
        /// reads from file system
        /// </summary>
        void read();

        /// <summary>
        /// writes into file system
        /// </summary>
        void write();

        /// <summary>
        /// loads defaults properties
        /// </summary>
        void loadDefaults();
    }
}
