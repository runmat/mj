// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using GeneralTools.Models;

namespace PdfPrint
{
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            GpsLogImport();

            Process.GetCurrentProcess().Kill();
                    
            base.OnStartup(e);
        }

        static void GpsLogImport()
        {
            const string fileName = @"C:\Backup\GpsLog.dat";
            const int dataItemLength = 25;

            var f = new FileStream(fileName, FileMode.Open);
            var br = new BinaryReader(f);

            var dataItem = br.ReadBytes(dataItemLength);
            while (dataItem.Length == dataItemLength)
            {
                ProcessDataItem(dataItem);

                dataItem = br.ReadBytes(dataItemLength);
            }

            br.Close();
            f.Close();
        }

        static void ProcessDataItem(byte[] dataItem)
        {
            var dateTime = JavaDateTimeBytesToDateTime(dataItem, 0);
            var activityType = dataItem[8];

            var locationLatitudeBin = JavaDoubleBytesToDouble(dataItem, 9);
            var locationLongditudeBin = JavaDoubleBytesToDouble(dataItem, 17);
        }


        static readonly Int64Converter Int64Converter = new Int64Converter();
        static readonly DoubleConverter DoubleConverter = new DoubleConverter();

        public static DateTime JavaDateTimeBytesToDateTime(byte[] bytes, int startIndex)
        {
            var hexString = "0x" + BitConverter.ToString(bytes, startIndex).SubstringTry(0, 23).Replace("-", "");
            var val = Int64Converter.ConvertFromString(hexString);
            if (val == null)
                return DateTime.MinValue;

            var dateTimeLong = (long)val;

            // Java timestamp is milliseconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(dateTimeLong).ToLocalTime();
            return dtDateTime;
        }

        public static double JavaDoubleBytesToDouble(byte[] bytes, int startIndex)
        {
            var hexString = "0x" + BitConverter.ToString(bytes, startIndex).SubstringTry(0, 23).Replace("-", "");
//            var val = DoubleConverter.ConvertFrom(hexString);
//            if (val == null)
                return 0;
//
//            return (double)val;
        }
    }
}
