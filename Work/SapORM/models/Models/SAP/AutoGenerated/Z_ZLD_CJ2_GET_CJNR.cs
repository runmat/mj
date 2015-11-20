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
	public partial class Z_ZLD_CJ2_GET_CJNR
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_CJNR).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_CJNR).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static string GetExportParameter_E_BUKRS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_BUKRS").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_CAJO_NUMBER(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_CAJO_NUMBER").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public static string GetExportParameter_E_WAERS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_WAERS").NotNullOrEmpty().Trim();
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
