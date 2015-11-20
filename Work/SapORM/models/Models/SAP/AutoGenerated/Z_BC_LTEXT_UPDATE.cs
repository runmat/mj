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
	public partial class Z_BC_LTEXT_UPDATE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_BC_LTEXT_UPDATE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_BC_LTEXT_UPDATE).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_LTEXT_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LTEXT_NR", value);
		}

		public static void SetImportParameter_I_STRING(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STRING", value);
		}

		public static void SetImportParameter_I_UNAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_UNAME", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
