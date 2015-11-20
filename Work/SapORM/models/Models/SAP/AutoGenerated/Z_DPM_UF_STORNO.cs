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
	public partial class Z_DPM_UF_STORNO
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_UF_STORNO).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_UF_STORNO).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_STORNOBEM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STORNOBEM", value);
		}

		public static void SetImportParameter_I_STORNONAM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STORNONAM", value);
		}

		public static void SetImportParameter_I_UNFALL_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_UNFALL_NR", value);
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
