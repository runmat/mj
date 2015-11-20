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
	public partial class Z_M_EINGABE_LVNUMMER_SIXTLEAS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EINGABE_LVNUMMER_SIXTLEAS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EINGABE_LVNUMMER_SIXTLEAS).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_LF_EQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("LF_EQUNR", value);
		}

		public void SetImportParameter_LF_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("LF_KUNNR", value);
		}

		public void SetImportParameter_LF_LIZNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("LF_LIZNR", value);
		}

		public int? GetExportParameter_LF_RETURN(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("LF_RETURN");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
