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
	public partial class Z_BC_LTEXT_DELETE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_BC_LTEXT_DELETE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_BC_LTEXT_DELETE).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_LTEXT_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LTEXT_NR", value);
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
