using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcCourse.Helper
{
    public static class Extensions
    {
        // File extension methods.

        public static bool HasFiles(this IEnumerable<HttpPostedFileBase> files)
        {
            var first = files!= null ? files.FirstOrDefault() : default(HttpPostedFileBase);
            return first != null && first.ContentLength > 0;
        }

        public static bool ContainsImages(this IEnumerable<HttpPostedFileBase> files)
        {
            return files.Any(file => file.IsImage());
        }

        public static bool IsImage(this HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            var formats = new[] { ".jpg", ".png", ".gif", ".jpeg" };
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        // Marco Parameter extension methods

        public static T GetValue<T>(this IDictionary<string, object> dictionary, string key)
        {
            return dictionary.GetValue(key, default(T));
        }

        public static T GetValue<T>(this IDictionary<string, object> dictionary, string key, T defaultValue)
        {
            if (!dictionary.ContainsKey(key) || string.IsNullOrEmpty(dictionary[key].ToString())) return defaultValue;
            return (T)Convert.ChangeType(dictionary[key], typeof(T));
        }
    }
}
