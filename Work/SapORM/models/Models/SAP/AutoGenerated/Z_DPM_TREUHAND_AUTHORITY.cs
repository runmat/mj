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
	public partial class Z_DPM_TREUHAND_AUTHORITY
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_TREUHAND_AUTHORITY).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_TREUHAND_AUTHORITY).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_EMAIL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EMAIL", value);
		}

		public static void SetImportParameter_I_KUNNR_TG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_TG", value);
		}

		public static void SetImportParameter_I_KUNNR_TN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_TN", value);
		}

		public static void SetImportParameter_I_NAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NAME", value);
		}

		public static void SetImportParameter_I_VORNA(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORNA", value);
		}

		public static string GetExportParameter_E_ENTSPERREN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_ENTSPERREN").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_FREIGABE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_FREIGABE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_SPERREN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_SPERREN").NotNullOrEmpty().Trim();
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
