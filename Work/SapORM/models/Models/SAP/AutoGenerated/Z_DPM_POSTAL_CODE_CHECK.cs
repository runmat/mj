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
	public partial class Z_DPM_POSTAL_CODE_CHECK
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_POSTAL_CODE_CHECK).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_POSTAL_CODE_CHECK).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_COUNTRY(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_COUNTRY", value);
		}

		public static void SetImportParameter_I_POSTAL_CODE_CITY(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_POSTAL_CODE_CITY", value);
		}

		public static void SetImportParameter_I_POSTAL_CODE_COMPANY(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_POSTAL_CODE_COMPANY", value);
		}

		public static void SetImportParameter_I_POSTAL_CODE_PO_BOX(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_POSTAL_CODE_PO_BOX", value);
		}

		public static void SetImportParameter_I_PO_BOX(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PO_BOX", value);
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
