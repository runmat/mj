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
	public partial class Z_M_SAVE_ZABWVERSGRUND
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_SAVE_ZABWVERSGRUND).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_SAVE_ZABWVERSGRUND).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_IMP_DATAUS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("IMP_DATAUS", value);
		}

		public void SetImportParameter_IMP_EQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IMP_EQUNR", value);
		}

		public void SetImportParameter_IMP_ERLEDIGT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IMP_ERLEDIGT", value);
		}

		public void SetImportParameter_IMP_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IMP_KUNNR", value);
		}

		public void SetImportParameter_IMP_MEMO(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IMP_MEMO", value);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
