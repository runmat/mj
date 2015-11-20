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
	public partial class Z_ZLD_SET_GRUPPE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_SET_GRUPPE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_SET_GRUPPE).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_BEZEI(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BEZEI", value);
		}

		public void SetImportParameter_I_FUNC(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FUNC", value);
		}

		public void SetImportParameter_I_GRUPART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_GRUPART", value);
		}

		public void SetImportParameter_I_GRUPPE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_GRUPPE", value);
		}

		public void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
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
