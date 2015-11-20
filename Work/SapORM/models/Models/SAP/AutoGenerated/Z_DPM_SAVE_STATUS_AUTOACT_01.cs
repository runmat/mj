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
	public partial class Z_DPM_SAVE_STATUS_AUTOACT_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_SAVE_STATUS_AUTOACT_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_SAVE_STATUS_AUTOACT_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AUTOACT_ID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AUTOACT_ID", value);
		}

		public void SetImportParameter_I_BELEGNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BELEGNR", value);
		}

		public void SetImportParameter_I_RUECK_AUTOACT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_RUECK_AUTOACT", value);
		}

		public void SetImportParameter_I_STATUS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STATUS", value);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
