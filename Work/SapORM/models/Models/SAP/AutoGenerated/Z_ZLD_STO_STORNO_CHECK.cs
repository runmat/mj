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
	public partial class Z_ZLD_STO_STORNO_CHECK
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_STO_STORNO_CHECK).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_STO_STORNO_CHECK).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_VBELN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VBELN", value);
		}

		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static void SetImportParameter_I_VKORG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKORG", value);
		}

		public static void SetImportParameter_I_ZULBELN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZULBELN", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public static string GetExportParameter_E_ZULBELN(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_ZULBELN").NotNullOrEmpty().Trim();
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
