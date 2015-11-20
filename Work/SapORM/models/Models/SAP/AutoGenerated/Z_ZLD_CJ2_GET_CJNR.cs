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


		public void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public string GetExportParameter_E_BUKRS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_BUKRS");
		}

		public string GetExportParameter_E_CAJO_NUMBER(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_CAJO_NUMBER");
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public string GetExportParameter_E_WAERS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_WAERS");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
