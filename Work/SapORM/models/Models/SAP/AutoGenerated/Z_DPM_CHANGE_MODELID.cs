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
	public partial class Z_DPM_CHANGE_MODELID
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_CHANGE_MODELID).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_CHANGE_MODELID).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AHK(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AHK", value);
		}

		public void SetImportParameter_I_ANTR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ANTR", value);
		}

		public void SetImportParameter_I_BLUETOOTH(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BLUETOOTH", value);
		}

		public void SetImportParameter_I_GESAMT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_GESAMT", value);
		}

		public void SetImportParameter_I_HERST(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_HERST", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_LEASING(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LEASING", value);
		}

		public void SetImportParameter_I_LKW(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LKW", value);
		}

		public void SetImportParameter_I_MODELID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MODELID", value);
		}

		public void SetImportParameter_I_NAVI_VORH(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NAVI_VORH", value);
		}

		public void SetImportParameter_I_SECU_FLEET(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SECU_FLEET", value);
		}

		public void SetImportParameter_I_TEST(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TEST", value);
		}

		public void SetImportParameter_I_UNAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_UNAME", value);
		}

		public void SetImportParameter_I_VERKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VERKZ", value);
		}

		public void SetImportParameter_I_WINTERREIFEN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_WINTERREIFEN", value);
		}

		public void SetImportParameter_I_ZLAUFZEIT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZLAUFZEIT", value);
		}

		public void SetImportParameter_I_ZLZBINDUNG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZLZBINDUNG", value);
		}

		public void SetImportParameter_I_ZSIPP_CODE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZSIPP_CODE", value);
		}

		public void SetImportParameter_I_ZZBEZEI(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZBEZEI", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public string GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_SUBRC");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
