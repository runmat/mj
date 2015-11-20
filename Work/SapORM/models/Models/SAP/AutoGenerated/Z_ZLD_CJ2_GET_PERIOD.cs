using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_ZLD_CJ2_GET_PERIOD
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_PERIOD).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_PERIOD).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_COMP_CODE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_COMP_CODE", value);
		}

		public static void SetImportParameter_I_DATUM(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM", value);
		}

		public static DateTime? GetExportParameter_E_FDAY(ISapDataService sap)
		{
			return sap.GetExportParameter<DateTime?>("E_FDAY");
		}

		public static DateTime? GetExportParameter_E_LDAY(ISapDataService sap)
		{
			return sap.GetExportParameter<DateTime?>("E_LDAY");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
