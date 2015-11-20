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
	public partial class Z_M_ABMBEREIT_LAUFAEN
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_ABMBEREIT_LAUFAEN).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_ABMBEREIT_LAUFAEN).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("KUNNR", value);
		}

		public static void SetImportParameter_KUNPDI(ISapDataService sap, string value)
		{
			sap.SetImportParameter("KUNPDI", value);
		}

		public static void SetImportParameter_ZZFAHRG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZFAHRG", value);
		}

		public static void SetImportParameter_ZZKENN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZKENN", value);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
