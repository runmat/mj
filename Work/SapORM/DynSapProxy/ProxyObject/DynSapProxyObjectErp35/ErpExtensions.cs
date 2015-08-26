﻿using System.Data;
using System.Globalization;
using ERPConnect;

namespace SapORM.Services
{
    public static class ErpExtensions
    {
        public static DataTable ToADOTableLocaleDe(this RFCTable rfct)
        {
            var tmpTable = rfct.ToADOTable();
            tmpTable.Locale = new CultureInfo("de-DE");
            return tmpTable;
        }
    }
}
