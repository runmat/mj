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
	public partial class Z_ZLD_AH_FS_CHECK
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_AH_FS_CHECK).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_AH_FS_CHECK).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_CODE_AUFBAU(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CODE_AUFBAU", value);
		}

		public void SetImportParameter_I_CODE_KRAFTSTOFF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CODE_KRAFTSTOFF", value);
		}

		public void SetImportParameter_I_FAHRZEUGKLASSE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRZEUGKLASSE", value);
		}

		public void SetImportParameter_I_SLD2(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SLD2", value);
		}

		public string GetExportParameter_E_PLAKART(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_PLAKART");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
