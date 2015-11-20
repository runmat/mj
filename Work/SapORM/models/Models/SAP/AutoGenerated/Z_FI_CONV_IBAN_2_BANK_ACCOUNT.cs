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
	public partial class Z_FI_CONV_IBAN_2_BANK_ACCOUNT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_FI_CONV_IBAN_2_BANK_ACCOUNT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_FI_CONV_IBAN_2_BANK_ACCOUNT).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_IBAN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_IBAN", value);
		}

		public string GetExportParameter_E_BANKA(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_BANKA");
		}

		public string GetExportParameter_E_BANK_ACCOUNT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_BANK_ACCOUNT");
		}

		public string GetExportParameter_E_BANK_CONTROL_KEY(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_BANK_CONTROL_KEY");
		}

		public string GetExportParameter_E_BANK_COUNTRY(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_BANK_COUNTRY");
		}

		public string GetExportParameter_E_BANK_NUMBER(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_BANK_NUMBER");
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public string GetExportParameter_E_ORT01(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_ORT01");
		}

		public string GetExportParameter_E_PROVZ(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_PROVZ");
		}

		public string GetExportParameter_E_STRAS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_STRAS");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public string GetExportParameter_E_SWIFT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_SWIFT");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
