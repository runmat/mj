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
	public partial class Z_ZLD_GET_NICKNAME
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_GET_NICKNAME).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_GET_NICKNAME).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static string GetExportParameter_E_EXTENSION1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_EXTENSION1").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_NAME1(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_NAME1").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_NAME2(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_NAME2").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_NICK_NAME(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_NICK_NAME").NotNullOrEmpty().Trim();
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
