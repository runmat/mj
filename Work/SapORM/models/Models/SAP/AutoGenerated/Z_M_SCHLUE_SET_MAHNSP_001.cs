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
	public partial class Z_M_SCHLUE_SET_MAHNSP_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_SCHLUE_SET_MAHNSP_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_SCHLUE_SET_MAHNSP_001).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_EQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQUNR", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_ZZMANSP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZMANSP", value);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
