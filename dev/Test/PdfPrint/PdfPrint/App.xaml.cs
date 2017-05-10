// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GeneralTools.Models;

namespace PdfPrint
{
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var list = GpsLogImport(@"C:\Backup\GpsLog_t.dat");
            GpxLogExport(list.Where(g => g.DateTime.Date == new DateTime(2017, 05, 10).Date), @"C:\Backup\Matz_t.gpx");

            Process.GetCurrentProcess().Kill();
            base.OnStartup(e);
        }

        static void GpxLogExport(IEnumerable<ActivityItem> list, string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

//            var item = list.ToArray()[336];
//            var gpsLocation = $"{FormatGpsDouble(item.Latitude)},{FormatGpsDouble(item.Longditude)}";
//            Clipboard.SetText(gpsLocation);

            var f = File.CreateText(fileName);
            f.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>");
            f.WriteLine("<gpx version=\"1.1\" creator=\"Matzi\">");

            f.WriteLine("<trk>");
            f.WriteLine("<name>Matzis Aktivität</name>");
            f.WriteLine("<desc>(Diese Aktivität wurde aufgezeichnet mit Matzis GpsLog App)</desc>");
            f.WriteLine("<trkseg>");

            foreach (var activityItem in list)
            {
                f.WriteLine($"<trkpt lat=\"{FormatGpsDouble(activityItem.Latitude)}\" lon=\"{FormatGpsDouble(activityItem.Longditude)}\">");
                f.WriteLine("<ele>0.0</ele>");
                f.WriteLine($"<time>{FormatGpsDateTime(activityItem.DateTime)}</time>");
                f.WriteLine("</trkpt>");
            }

            f.WriteLine("</trkseg>");
            f.WriteLine("</trk>");

            f.WriteLine("</gpx>");

            f.Close();
        }

        static string FormatGpsDouble(double val)
        {
            return val.ToString().Replace(",", ".");
        }

        static string FormatGpsDateTime(DateTime val)
        {
            return val.ToString("yyyy-MM-dd.THH:mm:ssZ");
        }

        static IEnumerable<ActivityItem> GpsLogImport(string fileName)
        {
            var list = new List<ActivityItem>();

            const int dataItemLength = 25;

            var f = new FileStream(fileName, FileMode.Open);
            var br = new BinaryReader(f);

            var dataItem = br.ReadBytes(dataItemLength);
            while (dataItem.Length == dataItemLength)
            {
                list.Add(ProcessDataItem(dataItem));

                dataItem = br.ReadBytes(dataItemLength);
            }

            br.Close();
            f.Close();

            return list;
        }

        static ActivityItem ProcessDataItem(byte[] dataItem)
        {
            var dateTime = JavaDateTimeBytesToDateTime(dataItem, 0);
            var activityType = dataItem[8];

            var locationLatitude = JavaDoubleBytesToDouble(dataItem, 9);
            var locationLongditude = JavaDoubleBytesToDouble(dataItem, 17);

            return new ActivityItem
            {
                DateTime = dateTime,
                ActiviyType = activityType,
                Latitude = locationLatitude,
                Longditude = locationLongditude
            };
        }


        #region Java to C# Conversion Helper

        private static readonly Int64Converter Int64Converter = new Int64Converter();

        [StructLayout(LayoutKind.Explicit)]
        private struct Double2Ulong
        {
            [FieldOffset(0)] public double d;
            [FieldOffset(0)] public ulong ul;
        }

        public static DateTime JavaDateTimeBytesToDateTime(byte[] bytes, int startIndex)
        {
            var dtDateTime = DateTime.MinValue;

            try
            {
                var hexString = "0x" + BitConverter.ToString(bytes, startIndex).SubstringTry(0, 23).Replace("-", "");
                var val = Int64Converter.ConvertFromString(hexString);
                if (val == null)
                    return dtDateTime;

                var dateTimeLong = (long) val;

                // Java timestamp is milliseconds past epoch
                dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddMilliseconds(dateTimeLong).ToLocalTime();
            }
            catch
            {
                return dtDateTime;
            }

            return dtDateTime;
        }

        public static double JavaDoubleBytesToDouble(byte[] bytes, int startIndex)
        {
            try
            {
                var hexString = "" + BitConverter.ToString(bytes, startIndex).SubstringTry(0, 23).Replace("-", "");

                var d2Ul = new Double2Ulong();
                var parsed = ulong.Parse(hexString, NumberStyles.AllowHexSpecifier);
                d2Ul.ul = parsed;
                var dbl = d2Ul.d;

                return dbl;
            }
            catch // ignored
            {
                return 0;
            }
        }

        #endregion

    }
}
