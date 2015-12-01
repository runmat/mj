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
	public partial class Z_WFM_STORNO_AUFTRAG_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_WFM_STORNO_AUFTRAG_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_WFM_STORNO_AUFTRAG_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_STORNODATUM(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_STORNODATUM", value);
		}

		public static void SetImportParameter_I_VORG_NR_ABM_AUF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORG_NR_ABM_AUF", value);
		}

		public static string GetExportParameter_E_ERF_VERS_ADR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_ERF_VERS_ADR").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_FAHRG(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_FAHRG").NotNullOrEmpty().Trim();
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
