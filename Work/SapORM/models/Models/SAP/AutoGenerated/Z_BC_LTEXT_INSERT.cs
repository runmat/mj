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
	public partial class Z_BC_LTEXT_INSERT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_BC_LTEXT_INSERT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_BC_LTEXT_INSERT).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_LTEXT_ID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LTEXT_ID", value);
		}

		public void SetImportParameter_I_STRING(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STRING", value);
		}

		public void SetImportParameter_I_UNAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_UNAME", value);
		}

		public string GetExportParameter_E_LTEXT_NR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_LTEXT_NR");
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
