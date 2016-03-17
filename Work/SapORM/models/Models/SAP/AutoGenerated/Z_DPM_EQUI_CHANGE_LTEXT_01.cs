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
	public partial class Z_DPM_EQUI_CHANGE_LTEXT_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_EQUI_CHANGE_LTEXT_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_EQUI_CHANGE_LTEXT_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_EQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQUNR", value);
		}

		public static void SetImportParameter_I_LTEXT_EQUI(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LTEXT_EQUI", value);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
