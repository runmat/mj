using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GeneralTools.Services;

namespace GeneralTools.Models
{
    public static class IEnumerableExtensions
    {
        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            return !source.Any();
        }

        public static List<TSource> ToListOrEmptyList<TSource>(this List<TSource> source)
        {
            return source != null && source.Any() ? source.ToList() : new List<TSource>();
        }

        public static List<TSource> ToListOrEmptyList<TSource>(this IEnumerable<TSource> source)
        {
            return source.Any() ? source.ToList() : new List<TSource>();
        }

        //public static TValue FirstOrDefault<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Expression<Func<TSource, TValue>> defaultExpression) 
        //    where TSource : class
        //{
        //    var item = source.FirstOrDefault(predicate);
        //    if (item == null)
        //        return default(TValue);

        //    var propertyName = defaultExpression.GetPropertyName();
        //    var modelType = typeof(TSource);
        //    var propertyValue = (TValue)modelType.GetProperty(propertyName).GetValue(item, null);
            
        //    return propertyValue;
        //}

        public static TSource[] ToArrayOrEmptyArray<TSource>(this IEnumerable<TSource> source)
        {
            return source != null && source.Any() ? source.ToArray() : new TSource[0];
        }

        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return !source.Any(predicate);
        }
        
        public static List<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static List<TSource> Copy<TSource>(this List<TSource> source, Action<TSource, TSource> onInit = null) where TSource : class, new()
        {
            return ModelMapping.Copy<TSource, TSource>(source, onInit: onInit).ToList();
        }

        public static List<TSource> CopyAndInsertAtTop<TSource>(this List<TSource> source, TSource itemToInsert) where TSource : class, new()
        {
            var copiedList = source.Copy();
            copiedList.Insert(0, itemToInsert);
            return copiedList;
        }
    }

    public static class ListExtensions
    {
        public static bool AnyAndNotNull<TSource>(this List<TSource> source)
        {
            if (source == null)
                return false;

            return source.Any();
        }

        public static bool AnyAndNotNull<TSource>(this List<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                return false;

            return source.Any(predicate);
        }

        public static bool NoneOrNotNull<TSource>(this List<TSource> source)
        {
            if (source == null)
                return true;

            return source.None();
        }

        public static bool NoneOrNotNull<TSource>(this List<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                return true;

            return source.None(predicate);
        }

        public static string GetSqlMultiselectCondition<T>(this List<T> items, string sql, string columnName)
        {
            if (items.NoneOrNotNull())
                return "";

            sql = string.Format("{0} {1} in ({2})",
                                    sql.GetSqlKeyWordWhereAnd(),
                                    columnName,
                                    string.Join(",", items.Select(item => string.Format("'{0}'", item)).ToArray()));

            return sql;
        }
    }

    public static class DateRangeExtensions
    {
        public static string GetSqlDateRangeCondition(this DateRange dateRange, string sql, string columnName)
        {
            if (!dateRange.IsSelected)
                return "";

            return string.Format("{0} ({1} >= '{2:yyyy-MM-dd}' and {1} <= '{3:yyyy-MM-dd 23:59:59}')",
                                    sql.GetSqlKeyWordWhereAnd(),
                                    columnName, dateRange.StartDate, dateRange.EndDate);
        }
    }

    public static class StringExtensions
    {
        public static string Crop(this string s, int len)
        {
            if (len == 0)
                return s;

            if (string.IsNullOrEmpty(s))
                return "";
            if (s.Length < len)
                return s;

            return s.Substring(0, len) + "..";
        }

        public static string SubstringTry(this string s, int start, int len)
        {
            var val = s.NotNullOrEmpty();

            if (val.Length - 1 < start)
                return "";

            if (val.Length - 1 < start + len)
                return val.Substring(start);

            return val.Substring(start, len);
        }

        public static string PrependIfNotNull(this string s, string prepend)
        {
            if (s.IsNullOrEmpty() || prepend.IsNullOrEmpty())
                return "";

            return string.Format("{0}{1}", prepend, s);
        }

        public static string AppendIfNotNull(this string s, string append)
        {
            if (s.IsNullOrEmpty() || append.IsNullOrEmpty())
                return "";

            return string.Format("{0}{1}", s, append);
        }

        public static string ReplaceIfNotNull(this string s, string replace)
        {
            if (s.IsNullOrEmpty() || replace.IsNullOrEmpty())
                return "";

            return replace;
        }

        public static string FormatIfNotNull(this string s, string format, params object[] param)
        {
            if (s.IsNullOrEmpty() || format.IsNullOrEmpty())
                return "";

            return string.Format(format.Replace("{this}", s), param);
        }

        public static string FormatPropertyParams<T>(this string s, params Expression<Func<T, object>>[] param)
        {
            if (param == null || param.Length == 0)
                return "";

            var propertyNameArray = param.Select(m => m.GetPropertyName()).ToArray();
            for (var i = 0; i < propertyNameArray.Length; i++)
                s = s.Replace("{" + i + "}", propertyNameArray[i]);

            return s;
        }

        public static string JoinIfNotNull(this IEnumerable<string> enumerable, string separator)
        {
            var list = enumerable.ToListOrEmptyList();
            list.RemoveAll(item => item.IsNullOrEmpty());

            return string.Join(separator, list.ToListOrEmptyList().ToArray());
        }

        public static string ToLowerFirstUpper(this string s)
        {
            s = s.NotNullOrEmpty();
            if (s.Length <= 1)
                return s.ToUpper();

            return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }

        public static string ToLowerAndHyphens(this string s)
        {
            var ret = "";
            s.NotNullOrEmpty().ToArray().ToList().ForEach(c =>
                {
                    if (c >= 'A' && c <= 'Z' && ret != "")
                        ret += "-";

                    ret += c.ToString().ToLower();
                });

            return ret;
        }

        public static string NotNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) ? "" : s;
        }

        public static string NotNullOrEmptyOrNullString(this string s, string additionalNullstring=null)
        {
            return IsNullOrEmptyOrNullString(s, additionalNullstring) ? "" : s;
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrEmptyOrNullString(this string s, string additionalNullstring = null)
        {
            var s2 = s.NotNullOrEmpty();
            return s2 == "" || s2 == additionalNullstring || s2.ToLower() == "null";
        }

        public static bool IsNotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s) && s.ToLower() != "null";
        }

        public static string RemovePropertyName<T>(this string s, Expression<Func<T>> expression, string replaceWith)
        {
            return s.Replace(expression.GetPropertyName(), replaceWith);
        }

        public static List<string> RemovePropertyName<T>(this List<string> list, Expression<Func<T>> expression)
        {
            list.RemoveAll(item => item == expression.GetPropertyName());
            return list;
        }

        public static string GetPartEnclosedBy(this string s, char partEnclosingCharacter)
        {
            if (!s.NotNullOrEmpty().Contains(partEnclosingCharacter))
                return "";

            var parts = s.Split(partEnclosingCharacter);
            if (parts.Length <= 2)
                return "";

            return parts[1];
        }

        public static string ToCommaSeparatedstring(this List<string> list)
        {
            if (list == null || list.Count == 0)
                return "";

            return string.Join(",", list.ToArray());
        }

        public static List<string> AddIfNotNull(this List<string> list, string value)
        {
            if (value.IsNullOrEmpty())
                return list;

            list.Add(value);

            return list;
        }

        public static bool IsNumeric(this string stringValue)
        {
            int tmp;
            return Int32.TryParse(stringValue.NotNullOrEmpty(), out tmp);
        }

        public static int ToInt(this string stringValue, int defaultValue = -1)
        {
            int tmp;
            if (!Int32.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return defaultValue;
            return tmp;
        }

        public static int? ToNullableInt(this string stringValue)
        {
            int tmp;
            if (!Int32.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return null;
            return tmp;
        }

        public static DateTime? ToNullableDateTime(this string stringValue, string format = null)
        {
            DateTime tmp;
            if (!String.IsNullOrEmpty(format))
            {
                if (!DateTime.TryParseExact(stringValue.NotNullOrEmpty(), format, CultureInfo.CurrentCulture, DateTimeStyles.None, out tmp))
                    return null;
            }
            else
            {
                if (!DateTime.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                    return null;
            }
            return tmp;
        }

        public static bool XToBool(this string stringValue)
        {
            return (stringValue.NotNullOrEmpty().ToUpper() == "X");
        }

        public static Int64 ToLong(this string stringValue, Int64 defaultValue = -1)
        {
            Int64 tmp;
            if (!Int64.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return defaultValue;
            return tmp;
        }

        public static string ToEmailWithoutAtSymbol(this string email)
        {
            return email.NotNullOrEmpty().Replace("@", "(at)");
        }
    
        public static string ToJavascriptString(this string s)
        {
            var translationTable = new Dictionary<string, string>
                {
                    {"ä", "e4"},
                    {"ö", "f6"},
                    {"ü", "fc"},
                    {"Ä", "c4"},
                    {"Ö", "d6"},
                    {"Ü", "dc"},
                    {"ß", "df"}
                };

            foreach (var keyValuePair in translationTable)
                s = s.Replace(keyValuePair.Key, string.Format(@"\u00{0}", keyValuePair.Value));

            return s;
        }

        public static string ToWebPath(this string s)
        {
            return s.NotNullOrEmpty().Replace('\\', '/');
        }

        public static string[] SplitSeparators(this string s)
        {
            return s.Replace(Environment.NewLine, "|").Replace("\t", "|").Replace(";", "|").Replace(",", "|").Split('|');
        }

        public static string GetSqlKeyWordWhereAnd(this string sqlString)
        {
            return sqlString.NotNullOrEmpty().ToLower().Contains(" where") ? " and" : " where";
        }
    }

    public static class ExpressionExtensions
    {
        public static string GetPropertyName(this LambdaExpression lambda)
        {
            MemberExpression memberExpr = null;

            switch (lambda.Body.NodeType)
            {
                case ExpressionType.Convert:
                    memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
                    break;
                case ExpressionType.MemberAccess:
                    memberExpr = lambda.Body as MemberExpression;
                    break;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr.Member.Name;
        }
    }

    public static class DictionaryExtensions
    {
        public static string TryGetValue(this IDictionary<int, string> dict, int key, string defaultValue = "")
        {
            string val;
            if (!dict.TryGetValue(key, out val))
                return defaultValue;

            return val;
        }
    }

    public static class ObjectExtensions
    {
        public static string GetPropertyDisplayName(this object model, string propertyName)
        {
            var type = model.GetType();

            var attribute = type.GetProperty(propertyName).GetCustomAttributes(true).FirstOrDefault(attr => attr is DisplayNameAttribute);
            var displayAttribute = (attribute as DisplayNameAttribute);
            if (displayAttribute == null)
                return propertyName;

            return displayAttribute.DisplayName;
        }

        public static bool IsPropertyRequired(this object model, string propertyName)
        {
            var type = model.GetType();

            var attribute = type.GetProperty(propertyName).GetCustomAttributes(true).FirstOrDefault(attr => attr is RequiredAttribute);

            return (attribute as RequiredAttribute) != null;
        }

        public static IDictionary<string, object> DisabledIf(this object model, bool disabledCondition)
        {
            var dict = new Dictionary<string, object>();
            model.GetType().GetProperties().ToList().ForEach(p => dict.Add(p.Name, p.GetValue(model, null)));

            if (disabledCondition)
                dict.Add("disabled", "");

            return dict;
        }

        public static IDictionary<string, object> ReadonlyIf(this object model, bool readonlyCondition)
        {
            var dict = new Dictionary<string, object>();
            model.GetType().GetProperties().ToList().ForEach(p => dict.Add(p.Name, p.GetValue(model, null)));

            if (readonlyCondition)
                dict.Add("readonly", "readonly");

            return dict;
        }

        public static IDictionary<string, object> MergePropertiesStrictly(this object model, object model2)
        {
            return TypeMerger.StrictlyMergeProperties(model, model2);
        }

        public static IDictionary<string, object> ToHtmlDictionary(this object model)
        {
            return TypeMerger.ToHtmlDictionary(model);
        }
    }

    public static class TypeExtensions
    {
        public static string GetFullTypeName(this Type type)
        {
            var typeFullName = type.AssemblyQualifiedName;
            if (typeFullName != null)
                return string.Join(",", typeFullName.Split(',').Take(2).ToArray());

            return null;
        }

        public static IEnumerable<string> GetScaffoldPropertyNames(this Type type)
        {
            return type.GetScaffoldProperties().Select(property => property.Name);
        }

        public static IEnumerable<PropertyInfo> GetScaffoldProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(property =>
            {
                if (property == null) return false;
                var scaffoldAttribute = property.GetCustomAttributes(true).OfType<ScaffoldColumnAttribute>().FirstOrDefault();
                if (scaffoldAttribute != null && !scaffoldAttribute.Scaffold)
                    return false;

                return true;
            });
        }
    }

    public static class DateTimeExtensions
    {
        public static string NotNullOrEmptyToString(this DateTime? dt)
        {
            return dt == null ? null : dt.GetValueOrDefault().ToString("d");
        }

        public static string NotNullOrEmptyToString(this DateTime? dt, string formatString)
        {
            return dt == null ? null : dt.GetValueOrDefault().ToString(formatString);
        }

        public static DateTime MoveToFirstDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime MoveToLastDay(this DateTime dt)
        {
            return dt.AddMonths(1).MoveToFirstDay().AddDays(-1);
        }

        public static string ToJsonDateTimeString(this DateTime dt)
        {
            return dt.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss");
        }

        public static string ToJsonDateString(this DateTime dt)
        {
            return dt.ToLocalTime().ToString("yyyy-MM-dd");
        }

        public static string ToShortDateTimeString(this DateTime dt)
        {
            return dt.ToString("dd.MM.yy HH:mm");
        }
    }

    public static class DataTableExtensions
    {
        public static DataTable DeleteEmptyRows(this DataTable dataTable)
        {
            var rowsToDelete = new List<DataRow>();
            foreach (DataRow row in dataTable.Rows)
            {
                var rowIsEmpty = true;
                for (var c = 0; c < dataTable.Columns.Count; c++)
                {
                    var val = row[c];
                    if (val != null && val != DBNull.Value && val.ToString().Trim(new[] { '\0', ' ' }) != "")
                        rowIsEmpty = false;
                }

                if (rowIsEmpty)
                    rowsToDelete.Add(row);

            }
            if (rowsToDelete.Any())
            {
                rowsToDelete.ForEach(r => dataTable.Rows.Remove(r));
                dataTable.AcceptChanges();
            }

            return dataTable;
        }
    }

    public static class ReflectionExtentions
    {
        public static T GetAttributeFrom<T>(this Type type, string propertyName) 
        {
            var attrType = typeof(T);
            var property = type.GetProperty(propertyName);
            return (T)property.GetCustomAttributes(attrType, false).FirstOrDefault();
        }
    }

    public static class NullableIntExtensions
    {
        public static string ToStringNotNull(this int? intValue, string defaultValue = "")
        {
            return (intValue.HasValue ? intValue.Value.ToString() : defaultValue);
        }
    }

    public static class BoolExtensions
    {
        public static string ToCustomString(this bool boolValue, string trueString, string falseString)
        {
            return (boolValue ? trueString : falseString);
        }

        public static string BoolToX(this bool boolValue)
        {
            return boolValue.ToCustomString("X", "");
        }
    }
}
