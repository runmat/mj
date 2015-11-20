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
	public partial class Z_M_VERSAUFTR_FEHLERHAFTE_DEL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_VERSAUFTR_FEHLERHAFTE_DEL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_VERSAUFTR_FEHLERHAFTE_DEL).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public void SetImportParameter_I_IDNRK(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_IDNRK", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_LICENSE_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LICENSE_NUM", value);
		}

		public void SetImportParameter_I_LOENAM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LOENAM", value);
		}

		public void SetImportParameter_I_ZANF_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZANF_NR", value);
		}

		public void SetImportParameter_I_ZZBRFVERS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZBRFVERS", value);
		}

		public void SetImportParameter_I_ZZSCHLVERS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZSCHLVERS", value);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
