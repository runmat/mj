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
	public partial class Z_DPM_READ_DOK_ARCHIV_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_DOK_ARCHIV_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_DOK_ARCHIV_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_ARC_ID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ARC_ID", value);
		}

		public void SetImportParameter_I_DOC_ID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_DOC_ID", value);
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public byte[] GetExportParameter_E_XSTRING(ISapDataService sap)
		{
			return sap.GetExportParameter<byte[]>("E_XSTRING");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
