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
	public partial class Z_M_FAELLIGE_EQUI_UPDATE_LP
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_FAELLIGE_EQUI_UPDATE_LP).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_FAELLIGE_EQUI_UPDATE_LP).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_EQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQUNR", value);
		}

		public void SetImportParameter_I_FALLIG_VERLAENGERN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FALLIG_VERLAENGERN", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_TEXT200(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TEXT200", value);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
