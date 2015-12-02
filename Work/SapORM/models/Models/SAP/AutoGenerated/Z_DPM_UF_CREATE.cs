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
	public partial class Z_DPM_UF_CREATE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_UF_CREATE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_UF_CREATE).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_EQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQUNR", value);
		}

		public static void SetImportParameter_I_ERNAM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ERNAM", value);
		}

		public static void SetImportParameter_I_REST_PREIS(ISapDataService sap, decimal? value)
		{
			sap.SetImportParameter("I_REST_PREIS", value);
		}

		public static void SetImportParameter_I_STANDORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STANDORT", value);
		}

		public static void SetImportParameter_I_STATION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STATION", value);
		}

		public static void SetImportParameter_I_VORG_ART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORG_ART", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public static string GetExportParameter_E_UNFALL_NR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_UNFALL_NR").NotNullOrEmpty().Trim();
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
