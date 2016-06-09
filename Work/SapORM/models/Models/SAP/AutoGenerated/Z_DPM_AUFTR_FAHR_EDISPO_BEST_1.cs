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
	public partial class Z_DPM_AUFTR_FAHR_EDISPO_BEST_1
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_AUFTR_FAHR_EDISPO_BEST_1).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_AUFTR_FAHR_EDISPO_BEST_1).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_FAHRER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRER", value);
		}

		public static void SetImportParameter_I_VBELN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VBELN", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
