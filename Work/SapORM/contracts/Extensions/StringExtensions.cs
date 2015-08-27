using System;
using GeneralTools.Models;
using Microsoft.VisualBasic;

namespace SapORM.Contracts
{
    public static class StringExtensions
    { 
        public static bool IsValid(this string s)
        {
            return !s.IsEmpty();
        }

        public static bool IsNotEmpty(this string s)
        {
            return s.IsValid();
        }

        public static bool IsEmpty(this string s)
        { 
            return string.IsNullOrEmpty(s);
        }

        public static string NotEmptyOrNull(this string s)
        {
            return string.IsNullOrEmpty(s) ? null : s;
        }

        public static object NotEmptyOrDbNull(this string s)
        {
            return string.IsNullOrEmpty(s) ? (object)System.DBNull.Value : s;
        }

        public static object ToDateTimeOrNull(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            DateTime dt;
            if (!DateTime.TryParse(s, out dt))
                return null;

            return dt;
        }

        public static object ToDateTimeOrNull(this object o)
        {
            if (o == null || o == DBNull.Value) return null;
            
            if (o is string) return (o as string).ToDateTimeOrNull();

            return null;
        }

        public static string PadLeftChar(this string str, int length, char padChar)
        {
            if (string.IsNullOrEmpty(str))
                str = "";

            str = str.PadLeft(length, padChar);
            if (str.Length < length)
                return str;

            return Strings.Left(str, length);
        }

        public static string PadLeft0(this string str, int length)
        {
            return str.PadLeftChar(length, '0');
        }

        /// <summary>
        /// SAP Kunden-Nr 10-stellig, ggfls. links mit "0" Zeichen auffüllen
        /// </summary>
        public static string ToSapKunnr(this string str)
        {
            return str.PadLeft0(10);
        }

        /// <summary>
        /// SAP Wert 10-stellig, ggfls. links mit "0" Zeichen auffüllen
        /// </summary>
        public static string PadLeft10(this string str)
        {
            return str.ToSapKunnr();
        }

        public static string ToNumC(this bool b)
        {
            return b ? "1" : "0";
        }

        public static string ToNumC(this string s)
        {
            return s.IsNotNullOrEmpty() ? "1" : "0";
        }

        public static string CStr(string str)
        {
            return (string.IsNullOrEmpty(str) ? "" : str);
        }

        public static string Left(this string str, int length)
        {
            str = CStr(str);
            return (str.Length < length ? str : str.Substring(0, length));
        }

        /// <summary>
        /// returns TT.MM.JJJJ
        /// </summary>
        public static string TruncateToShortDateString(this string str)
        {
            return str.Left(10);
        }

        /// <summary>
        /// returns HH:mm:ss or HH:mm
        /// </summary>
        public static string ToTimeString(this string str)
        {
            str = CStr(str);

            if (str.Length < 4)
                return str;

            if (str.Length < 6)
                return string.Format("{0}:{1}", str.Left(2), str.Substring(2, 2));

            return string.Format("{0}:{1}:{2}", str.Left(2), str.Substring(2, 2), str.Substring(4, 2));
        }
    }
}
