using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
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
            return (source != null && source.Any()) ? source.ToList() : new List<TSource>();
        }

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

        public static IEnumerable<T> InsertAtTop<T>(this IEnumerable<T> source, T itemToInsert)
        {
            var list = source.ToListOrEmptyList();
            list.Insert(0, itemToInsert);
            return list;
        }

        public static IDictionary<T1, T2> InsertAtTop<T1, T2>(this IDictionary<T1, T2> source, T1 key, T2 value)
        {
            var list = source.ToList();
            list.Insert(0, new KeyValuePair<T1, T2>(key, value));
            return list.ToDictionary(s => s.Key, s => s.Value);
        }

        public static IDictionary<T1, T2> InsertAtTop<T1, T2>(this IEnumerable<KeyValuePair<T1, T2>> source, T1 key, T2 value)
        {
            var list = source.ToList();
            list.Insert(0, new KeyValuePair<T1, T2>(key, value));
            return list.ToDictionary(s => s.Key, s => s.Value);
        }

        public static DataTable ToExcelExportDataTable<T>(this IList<T> source)
        {
            return source.ToDataTable(pi => pi.GetCustomAttributes(true).None(p => p.GetType() == typeof (GridExportIgnoreAttribute)));
        }

        public static DataTable ToDataTable<T>(this IList<T> source, Func<PropertyInfo, bool> propertySelectorFunc = null)
        {
            var dt = new DataTable();
            var properties = TypeDescriptor.GetProperties(typeof(T));
            
            // Column Headers
            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var propType = property.PropertyType;
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof (Nullable<>))
                    propType = propType.GetGenericArguments()[0];

                var propertyInfo = property.ComponentType.GetProperty(property.Name);
                if (propertySelectorFunc == null || (propertyInfo != null && propertySelectorFunc(propertyInfo)))
                    dt.Columns.Add(property.Name, propType);
            }

            // Data
            var values = new object[dt.Columns.Count];

            foreach (var item in source)
            {
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    var columnName = dt.Columns[i].ColumnName;
                    values[i] = properties[columnName].GetValue(item);
                }
                dt.Rows.Add(values);
            }

            return dt;
        }

        public static Type GetItemType(this IEnumerable someCollection)
        {
            var type = someCollection.GetType();
            var ienum = type.GetInterface(typeof(IEnumerable<>).Name);
            return ienum != null ? ienum.GetGenericArguments()[0] : null;
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> source, T defaultValue) where T : class
        {
            var item = source.FirstOrDefault();
            return (item ?? defaultValue);
    }

        public static T FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T defaultValue) where T : class
        {
            var item = source.FirstOrDefault(predicate);
            return (item ?? defaultValue);
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

        public static string GetListAsString<T>(this List<T> items)
        {
            var erg = "";

            if (items != null && items.Any())
                items.ForEach(i => erg += (i as object).GetObjectAsString() + Environment.NewLine);

            return erg;
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
        public static string ReplaceHtmlTags(this string s, bool replaceBrWithCrLf = true)
        {
            var brRelevantTags = @"<p>|</p>|<br>|<br />|<br/>";

            if (!replaceBrWithCrLf)
                s = Regex.Replace(s, brRelevantTags, "@br@", RegexOptions.IgnoreCase);
            else
                s = Regex.Replace(s, brRelevantTags, "\r\n", RegexOptions.IgnoreCase);

            s = Regex.Replace(s, @"<.+?>", string.Empty);

            if (!replaceBrWithCrLf)
                s = s.Replace("@br@", "<br />");

            return s;
        }

        public static string Crop(this string s, int len, string appendText = "..")
        {
            if (len == 0)
                return s;

            if (string.IsNullOrEmpty(s))
                return "";
            if (s.Length < len)
                return s;

            return s.Substring(0, len) + appendText;
        }

        public static string CropExactly(this string s, int len)
        {
            return s.Crop(len, "");
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

        public static string SubstringTry(this string s, int start)
        {
            return s.SubstringTry(start, 99999999);
        }

        public static string PrependIfNotNull(this string s, string prepend)
        {
            if (s.IsNullOrEmpty() || prepend.IsNullOrEmpty())
                return "";

            return string.Format("{0}{1}", prepend, s);
        }

        public static string PrependIfNotNullElse(this string s, string prepend, string elseString)
        {
            return s.IsNotNullOrEmpty() ? s.PrependIfNotNull(prepend) : elseString;
        }

        public static string AppendIfNotNull(this string s, string append)
        {
            if (s.IsNullOrEmpty() || append.IsNullOrEmpty())
                return "";

            return string.Format("{0}{1}", s, append);
        }

        public static string AppendIfNotNullAndNot(this string s, string what, string append)
        {
            if (s.IsNullOrEmpty() || append.IsNullOrEmpty())
                return "";

            if (what == s)
                return s;

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

        public static string ToLowerFirstUpperWithFragments(this string s, char fragmentSourceSeparator = '_', char fragmentDestinationSeparator = '-')
        {
            if (!s.Contains(fragmentSourceSeparator))
                return s.ToLowerFirstUpper();

            var fragments = s.Split(fragmentSourceSeparator);
            return string.Join(fragmentDestinationSeparator.ToString(), fragments.Select(f => f.ToLowerFirstUpper()).ToArray());
        }

        public static string ToLowerFirstUpper(this string s)
        {
            s = s.NotNullOrEmpty();
            if (s.Length <= 1)
                return s.ToUpper();

            return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }

        public static string ToFirstLower(this string s)
        {
            s = s.NotNullOrEmpty();
            if (s.Length <= 1)
                return s;

            return s.Substring(0, 1).ToLower() + s.Substring(1);
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

        public static string RemoveDigits(this string s)
        {
            var ret = "";
            s.NotNullOrEmpty().ToArray().ToList().ForEach(c =>
                {
                    if (c >= '0' && c <= '9')
                        return;

                ret += c.ToString();
            });

            return ret;
        }

        public static bool LastCharIs(this string s, char lastChar)
        {
            return !s.IsNullOrEmpty() && (s[s.Length - 1] == lastChar);
        }

        public static string NotNullOrEmpty(this string s, string defaultValue)
        {
            return string.IsNullOrEmpty(s) ? defaultValue : s;
        }

        public static string NotNullOrEmpty(this string s)
        {
            return s.NotNullOrEmpty("");
        }

        public static string ToLowerAndNotEmpty(this string s)
        {
            return s.NotNullOrEmpty().ToLower();
        }

        public static string NotNullOr(this string s, string alternativeValue)
        {
            return s.IsNotNullOrEmpty() ? s : alternativeValue;
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

        public static string NullIfNullOrEmpty(this string s)
        {
            return (s.IsNullOrEmpty() ? null : s);
        }

        public static decimal? NullIf0(this decimal? val)
        {
            return (val.GetValueOrDefault() == 0 ? null : val);
        }

        public static string SlashToBackslash(this string s)
        {
            return s.NotNullOrEmpty().Replace('/', '\\');
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
            decimal tmp;
            return decimal.TryParse(stringValue.NotNullOrEmpty(), out tmp);
        }

        public static int ToInt(this string stringValue, int defaultValue = -1)
        {
            decimal tmp;
            if (!decimal.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return defaultValue;
            return (int)tmp;
        }

        public static decimal ToDecimal(this string stringValue, decimal defaultValue = -1)
        {
            decimal tmp;
            if (!Decimal.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return defaultValue;
            return tmp;
        }

        public static decimal? ToNullableDecimal(this string stringValue)
        {
            decimal tmp;
            if (!Decimal.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return null;
            return tmp;
        }

        public static int? ToNullableInt(this string stringValue)
        {
            decimal tmp;
            if (!decimal.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return null;
            return (int)tmp;
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

        public static object ToDateTimeOrDbNull(this string stringValue, string format = null)
        {
            DateTime tmp;
            if (!String.IsNullOrEmpty(format))
            {
                if (!DateTime.TryParseExact(stringValue.NotNullOrEmpty(), format, CultureInfo.CurrentCulture, DateTimeStyles.None, out tmp))
                    return DBNull.Value;
            }
            else
            {
                if (!DateTime.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                    return DBNull.Value;
            }
            return tmp;
        }

        public static DateTime ToFirstDayOfWeek(this DateTime? dateValue)
        {
            var date = dateValue.GetValueOrDefault();

            return date.AddDays(date.DayOfWeek.ToString("d").ToInt() * -1);
        }

        public static DateTime ToFirstDayOfMonth(this DateTime? dateValue)
        {
            var date = dateValue.GetValueOrDefault();

            return date.AddDays((date.Day * -1) + 1);
        }

        public static int GetWeekNumber(this DateTime? dateValue)
        {
            return dateValue.GetValueOrDefault().GetWeekNumber();
        }

        public static int GetWeekNumber(this DateTime dateValue)
        {
            var cul = CultureInfo.CurrentCulture;
            var weekNum = cul.Calendar.GetWeekOfYear(dateValue, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return weekNum;
        }

        public static string FormatYearAndWeek(this DateTime? dateValue, string yearFormat = "yyyy")
        {
            return dateValue.GetValueOrDefault().FormatYearAndWeek(yearFormat);
        }

        public static string FormatYearAndWeek(this DateTime dateValue, string yearFormat = "yyyy")
        {
            return string.Format("{0}{1}", dateValue.ToString(yearFormat), dateValue.GetWeekNumber().ToString("00"));
        }

        public static double ToJsonTicks(this DateTime dateValue)
        {
            var d1 = new DateTime(1970, 1, 1);
            var d2 = dateValue.ToUniversalTime();
            var ts = new TimeSpan(d2.Ticks - d1.Ticks);

            return Math.Round(ts.TotalMilliseconds, 0);
        }

        public static bool XToBool(this string stringValue)
        {
            return (stringValue.NotNullOrEmpty().ToUpper() == "X");
        }

        public static bool XToBoolInverse(this string stringValue)
        {
            return (stringValue.NotNullOrEmpty().ToUpper() != "X");
        }

        public static Int64 ToLong(this string stringValue, Int64 defaultValue = -1)
        {
            decimal tmp;
            if (!decimal.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return defaultValue;
            return (Int64)tmp;
        }

        public static Int64? ToNullableLong(this string stringValue)
        {
            Int64 tmp;
            if (!Int64.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return null;
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

        public static bool ToBool(this string stringValue)
        {
            var strUppercaseText = stringValue.NotNullOrEmpty().ToUpper();

            return (strUppercaseText == "TRUE" || strUppercaseText == "JA" || strUppercaseText == "X");
        }

        public static bool IsInteger(this string stringValue)
        {
            int tmp;
            return Int32.TryParse(stringValue.NotNullOrEmpty(), out tmp);
        }

        public static double ToDouble(this string stringValue, double defaultValue = -1)
        {
            double tmp;
            if (!double.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return defaultValue;
            return tmp;
        }

        public static double? ToNullableDouble(this string stringValue)
        {
            double tmp;
            if (!double.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return null;
            return tmp;
        }

        public static float ToFloat(this string stringValue, float defaultValue = -1)
        {
            float tmp;
            if (!float.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return defaultValue;
            return tmp;
        }

        public static float? ToNullableFloat(this string stringValue)
        {
            float tmp;
            if (!float.TryParse(stringValue.NotNullOrEmpty(), out tmp))
                return null;
            return tmp;
        }

        public static bool IsDecimal(this string stringValue)
        {
            decimal tmp;
            return Decimal.TryParse(stringValue.NotNullOrEmpty(), out tmp);
        }

        public static bool IsDate(this string stringValue)
        {
            DateTime tmp;
            return DateTime.TryParse(stringValue.NotNullOrEmpty(), out tmp);
        }

        public static object TryConvertToDestinationType(this string stringValue, PropertyInfo propertyDst, bool useDbNullInsteadOfNullForEmptyDateTimeValues = false)
        {
            if (propertyDst.PropertyType == typeof (string))
                return stringValue;

            if (propertyDst.PropertyType == typeof (int))
                return stringValue.ToInt(0);

            if (propertyDst.PropertyType == typeof (int?))
                return stringValue.ToNullableInt();

            if (propertyDst.PropertyType == typeof (long))
                return stringValue.ToLong(0);

            if (propertyDst.PropertyType == typeof (long?))
                return stringValue.ToNullableLong();

            if (propertyDst.PropertyType == typeof (decimal))
                return stringValue.ToDecimal(0);

            if (propertyDst.PropertyType == typeof (decimal?))
                return stringValue.ToNullableDecimal();

            if (propertyDst.PropertyType == typeof(float))
                return stringValue.ToFloat(0);

            if (propertyDst.PropertyType == typeof(float?))
                return stringValue.ToNullableFloat();

            if (propertyDst.PropertyType == typeof (double))
                return stringValue.ToDouble(0);

            if (propertyDst.PropertyType == typeof (double?))
                return stringValue.ToNullableDouble();

            if (propertyDst.PropertyType == typeof (DateTime))
                return stringValue.ToNullableDateTime() ?? DateTime.MinValue;

            if (propertyDst.PropertyType == typeof (DateTime?))
            {
                if (useDbNullInsteadOfNullForEmptyDateTimeValues)
                    return stringValue.ToDateTimeOrDbNull();

                return stringValue.ToNullableDateTime();
            }

            if (propertyDst.PropertyType == typeof (bool))
                return stringValue.ToBool();

            if (propertyDst.PropertyType == typeof(bool?))
                return (bool?)stringValue.ToBool();

            return null;
        }


        public static bool In(this string stringValue, IEnumerable<string> liste)
        {
            return ( stringValue != null && liste.Contains(stringValue));
        }
        public static bool In(this string stringValue, string listeAsCommaSeparatedString)
        {
            return (stringValue != null && !string.IsNullOrEmpty(listeAsCommaSeparatedString) && listeAsCommaSeparatedString.Split(',').Contains(stringValue));
        }

        public static bool NotIn(this string stringValue, IEnumerable<string> liste)
        {
            return (stringValue == null || !liste.Contains(stringValue));
        }

        public static bool NotIn(this string stringValue, string listeAsCommaSeparatedString)
        {
            return (stringValue == null || string.IsNullOrEmpty(listeAsCommaSeparatedString) || !listeAsCommaSeparatedString.Split(',').Contains(stringValue));
        }
    }
    public static class ExpressionExtensions
    {
        public static string GetPropertyName(this LambdaExpression lambda, bool forGridBinding = false)
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

            var memberExprExpr = memberExpr.Expression.ToString();
            var memberExprMember = memberExpr.Member.Name;

            if (forGridBinding && memberExprExpr.Contains("."))
                return string.Format("{0}.{1}", memberExprExpr.Split('.').Last(), memberExprMember);

            return memberExprMember;
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

        public static string GetObjectAsString(this object model)
        {
            var erg = "";

            if (model != null)
            {
                erg += "[";
                foreach (var prop in model.GetType().GetProperties())
                {
                    erg += String.Format("{0}: {1}|", prop.Name, prop.GetValue(model, null));
                }
                if (erg.EndsWith("|")) erg = erg.Substring(0, erg.Length - 1);
                erg += "]";
            }

            return erg;
        }

        public static TValue GetPropertyValueIfIs<TModel, TValue>(this object o, Expression<Func<TModel, TValue>> expression, TValue defaultValue = default (TValue))
        {
            if (!(o is TModel))
                return defaultValue;

            return expression.Compile().Invoke((TModel)o);
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

        public static IEnumerable<string> GetScaffoldPropertyLowerNames(this Type type)
        {
            return type.GetScaffoldProperties().Select(property => property.Name.ToLower());
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


        /// <summary>
        /// Alle Properties von Typ "type" mit Property-Klassen die ein Attribut "attributeType" besitzen
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(this Type type, Type attributeType)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetCustomAttributes(true)
                    .Any(p => p.GetType() == attributeType));
    }

        public static PropertyInfo GetPropertyWithAttribute(this Type type, Type attributeType)
    {
            return type.GetPropertiesWithAttribute(attributeType).FirstOrDefault();
        }

        /// <summary>
        /// Alle Properties von Typ "type" mit Property-Klassen die ein Klassen-Attribut "classAttributeType" besitzen
        /// </summary>
        /// <param name="type"></param>
        /// <param name="classAttributeType"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertiesOfClassWithAttribute(this Type type, Type classAttributeType)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.PropertyType.GetCustomAttributes(true).Any(attr => attr.GetType() == classAttributeType));
        }

        public static PropertyInfo GetPropertyOfClassWithAttribute(this Type type, Type propertyClassAttributeType)
        {
            return type.GetPropertiesOfClassWithAttribute(propertyClassAttributeType).FirstOrDefault();
        }

        /// <summary>
        /// Alle Methoden von Typ "type" mit Property-Klassen die ein Attribut "attributeType" besitzen
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(this Type type, Type attributeType)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetCustomAttributes(true).Any(attr => attr.GetType() == attributeType));
        }

        public static MethodInfo GetMethodWithAttribute(this Type type, Type attributeType)
        {
            return type.GetMethodsWithAttribute(attributeType).FirstOrDefault();
        }

        public static IEnumerable<MethodInfo> GetMethodsWithAttribute<T>(this Type type, Predicate<T> filterAttributeFunc) where T : Attribute
        {
            var attributeType = typeof (T);
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.GetCustomAttributes(true).Any(attr => attr.GetType() == attributeType && filterAttributeFunc((T)attr)));
        }

        public static MethodInfo GetMethodWithAttribute<T>(this Type type, Predicate<T> filterAttributeFunc) where T : Attribute
        {
            return type.GetMethodsWithAttribute<T>(filterAttributeFunc).FirstOrDefault();
        }
    }

    public static class DateTimeExtensions
    {
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

    public static class NullableDateTimeExtensions
    {
        public static string NotNullOrEmptyToString(this DateTime? dt)
        {
            return dt == null ? null : dt.GetValueOrDefault().ToString("d");
        }

        public static string NotNullOrEmptyToString(this DateTime? dt, string formatString)
        {
            return dt == null ? null : dt.GetValueOrDefault().ToString(formatString);
        }

        public static string ToString(this DateTime? dt, string formatString)
        {
            return (dt.HasValue ? dt.Value.ToString(formatString) : "");
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

        public static string GetTableAsString(this DataTable dataTable)
        {
            var erg = "";

            if (dataTable != null)
            {
                // Header
                erg += dataTable.TableName + Environment.NewLine;
                erg += "[";
                for (var i = 0; i < dataTable.Columns.Count; i++)
                {
                    erg += dataTable.Columns[i].ColumnName + "|";
                }
                if (erg.EndsWith("|")) erg = erg.Substring(0, erg.Length - 1);
                erg += "]" + Environment.NewLine;

                // Daten
                for (var j = 0; j < dataTable.Rows.Count; j++)
                {
                    erg += "[";
                    for (var k = 0; k < dataTable.Columns.Count; k++)
                    {
                        erg += dataTable.Rows[j][dataTable.Columns[k]].ToString() + "|";
                    }
                    if (erg.EndsWith("|")) erg = erg.Substring(0, erg.Length - 1);
                    erg += "]" + Environment.NewLine;
                }
            }

            return erg;
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

        public static string BoolToXInverse(this bool boolValue)
        {
            return boolValue.ToCustomString("", "X");
        }
    }

    public static class NullableBoolExtensions
    {
        public static string BoolToX(this bool? boolValue)
        {
            return (boolValue == true).ToCustomString("X", "");
}

        public static bool IsTrue(this bool? boolValue)
        {
            return (boolValue == true);
        }
    }

    public static class NullableDecimalExtensions
    {
        public static string ToString(this decimal? decimalValue, string format)
        {
            if (!decimalValue.HasValue)
                return "";

            return decimalValue.Value.ToString(format);
        }
    }
}
