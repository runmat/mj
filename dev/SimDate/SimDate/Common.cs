using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimDate
{
    class Common
    {
    }

    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

        /// <summary>  
        /// System.DateTime?  
        /// </summary>  
        /// <param name="time">System.DateTime?</param>  
        public void FromDateTime(DateTime time)
        {
            wYear = (ushort)time.Year;
            wMonth = (ushort)time.Month;
            wDayOfWeek = (ushort)time.DayOfWeek;
            wDay = (ushort)time.Day;
            wHour = (ushort)time.Hour;
            wMinute = (ushort)time.Minute;
            wSecond = (ushort)time.Second;
            wMilliseconds = (ushort)time.Millisecond;
        }
        /// <summary>  
        /// System.DateTime?  
        /// </summary>  
        /// <returns></returns>  
        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
        }
        /// <summary>  
        /// ?System.DateTime?  
        /// </summary>  
        /// <param name="time">SYSTEMTIME?</param>  
        /// <returns></returns>  
        public static DateTime ToDateTime(SystemTime time)
        {
            return time.ToDateTime();
        }
    }
}
