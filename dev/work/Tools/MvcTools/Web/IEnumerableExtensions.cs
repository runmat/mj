using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using GeneralTools.Models;

namespace MvcTools.Web
{
    public static class IEnumerableExtensions
    {
        public static SelectList ToSelectList(this IEnumerable<object> items, string keyAndTextField)
        {
            return new SelectList(items, keyAndTextField, keyAndTextField);
        }

        public static SelectList ToSelectList(this IEnumerable<object> items, string keyField, string textField)
        {
            return new SelectList(items, keyField, textField);
        }

        public static SelectList ToSelectList(this IEnumerable<string> items)
        {
            return new SelectList(items.Select(i => new { Key = TryGetSplitOptionOrAll(i, 0), Value = TryGetSplitOptionOrAll(i, 1) }), "Key", "Value");
        }

        public static MultiSelectList ToMultiSelectList(this IEnumerable<string> items, IEnumerable<string> preselectionListItems = null)
        {
            return new MultiSelectList(items.Select(i => new { Key = TryGetSplitOptionOrAll(i, 0), Value = TryGetSplitOptionOrAll(i, 1) }), "Key", "Value", preselectionListItems);
        }

        static string TryGetSplitOptionOrAll(string option, int index)
        {
            if (!option.NotNullOrEmpty().Contains(","))
                return option;

            return option.Split(',')[index].Trim();
        }

        public static SelectList ToSelectList<T>(this IEnumerable<T> items, bool addEmptyOption = false)
        {
            if (items == null)
                return new SelectList("", "");

            var properties = typeof (T).GetProperties();
            var keyProperty = properties.FirstOrDefault(property => property.GetCustomAttributes(typeof(SelectListKeyAttribute), true).Any());
            var valueProperty = properties.FirstOrDefault(property => property.GetCustomAttributes(typeof(SelectListTextAttribute), true).Any());
            if (keyProperty == null || valueProperty == null)
                return new SelectList("", "");

            var innerItems = items.Select(i => new {Key = keyProperty.GetValue(i, null), Value = valueProperty.GetValue(i, null)});
            
            if (!addEmptyOption)
                return new SelectList(innerItems, "Key", "Value");

            var emptyOption = new List<dynamic> {new {Key = (object) null, Value = (object) ""}};
            return new SelectList(emptyOption.Concat(innerItems), "Key", "Value");
        }

        #region IDictionary object, string

        private static IEnumerable<string> ToCommaSeparatedStringItems(this IDictionary<object, string> dict)
        {
            return dict.Select((entry, val) => entry.Key.ToString() + "," + entry.Value);
        }
        public static SelectList ToSelectList(this IDictionary<object, string> dict)
        {
            return dict.ToCommaSeparatedStringItems().ToSelectList();
        }
        public static MultiSelectList ToMultiSelectList(this IDictionary<object, string> dict)
        {
            return dict.ToCommaSeparatedStringItems().ToMultiSelectList();
        }

        #endregion

        #region IDictionary int, string

        private static IEnumerable<string> ToCommaSeparatedStringItems(this IDictionary<int, string> dict)
        {
            return dict.Select((entry, val) => entry.Key.ToString() + "," + entry.Value);
        }
        public static SelectList ToSelectList(this IDictionary<int, string> dict)
        {
            return dict.ToCommaSeparatedStringItems().ToSelectList();
        }
        public static MultiSelectList ToMultiSelectList(this IDictionary<int, string> dict)
        {
            return dict.ToCommaSeparatedStringItems().ToMultiSelectList();
        }

        #endregion

        #region IDictionary string, string

        private static IEnumerable<string> ToCommaSeparatedStringItems(this IDictionary<string, string> dict)
        {
            return dict.Select((entry, val) => entry.Key.ToString() + "," + entry.Value);
        }
        public static SelectList ToSelectList(this IDictionary<string, string> dict)
        {
            return dict.ToCommaSeparatedStringItems().ToSelectList();
        }
        public static MultiSelectList ToMultiSelectList(this IDictionary<string, string> dict)
        {
            return dict.ToCommaSeparatedStringItems().ToMultiSelectList();
        }

        #endregion
    }

    public static class NameValueCollectionExtension
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection sourceCollection)
        {
            return sourceCollection.Cast<string>()
                     .Select(i => new { Key = i, Value = sourceCollection[i] })
                     .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
