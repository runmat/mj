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
	public partial class Z_FIL_CHECK_OPEN_002
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_FIL_CHECK_OPEN_002).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_FIL_CHECK_OPEN_002).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_DATUM(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM", value);
		}

		public void SetImportParameter_I_KREISKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KREISKZ", value);
		}

		public void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public void SetImportParameter_I_VKORG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKORG", value);
		}

		public DateTime? GetExportParameter_E_DATUM(ISapDataService sap)
		{
			return sap.GetExportParameter<DateTime?>("E_DATUM");
		}

		public string GetExportParameter_E_GRUND(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_GRUND");
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
