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
	public partial class Z_DPM_SET_EQUI_REFERENZ
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_SET_EQUI_REFERENZ).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_SET_EQUI_REFERENZ).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_IMP_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IMP_AG", value);
		}

		public static void SetImportParameter_IMP_EQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IMP_EQUNR", value);
		}

		public static void SetImportParameter_IMP_FIN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IMP_FIN", value);
		}

		public static void SetImportParameter_IMP_REFERENZ1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IMP_REFERENZ1", value);
		}

		public static void SetImportParameter_IMP_REFERENZ2(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IMP_REFERENZ2", value);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
