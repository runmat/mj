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
	public partial class Z_ZLD_GET_BARQ_FROM_EASY
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_GET_BARQ_FROM_EASY).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_GET_BARQ_FROM_EASY).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_BARQ_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BARQ_NR", value);
		}

		public void SetImportParameter_I_OBJECT_ID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_OBJECT_ID", value);
		}

		public string GetExportParameter_E_FILENAME(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_FILENAME");
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
