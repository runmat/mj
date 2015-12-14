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
	public partial class Z_WFM_WRITE_TODO_02
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_WFM_WRITE_TODO_02).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_WFM_WRITE_TODO_02).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_ANMERKUNG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ANMERKUNG", value);
		}

		public static void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public static void SetImportParameter_I_FUNKTIONSNAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FUNKTIONSNAME", value);
		}

		public static void SetImportParameter_I_VORG_NR_ABM_AUF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORG_NR_ABM_AUF", value);
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
