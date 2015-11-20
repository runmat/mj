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
	public partial class Z_ZLD_MOB_SET_VG_STATUS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_MOB_SET_VG_STATUS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_MOB_SET_VG_STATUS).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_STATUS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STATUS", value);
		}

		public void SetImportParameter_I_ZULBELN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZULBELN", value);
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
