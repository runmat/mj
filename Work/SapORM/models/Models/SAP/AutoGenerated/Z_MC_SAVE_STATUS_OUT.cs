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
	public partial class Z_MC_SAVE_STATUS_OUT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_MC_SAVE_STATUS_OUT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_MC_SAVE_STATUS_OUT).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_BD_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BD_NR", value);
		}

		public static void SetImportParameter_I_LFDNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LFDNR", value);
		}

		public static void SetImportParameter_I_STATUS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STATUS", value);
		}

		public static void SetImportParameter_I_VORGID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORGID", value);
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
