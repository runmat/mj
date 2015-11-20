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
	public partial class Z_DPM_PRUEF_FIN_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_PRUEF_FIN_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_PRUEF_FIN_001).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_FGNU(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FGNU", value);
		}

		public void SetImportParameter_I_FGPZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FGPZ", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public string GetExportParameter_E_STATUS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_STATUS");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
