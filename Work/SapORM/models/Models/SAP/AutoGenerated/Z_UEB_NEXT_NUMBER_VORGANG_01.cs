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
	public partial class Z_UEB_NEXT_NUMBER_VORGANG_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_UEB_NEXT_NUMBER_VORGANG_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_UEB_NEXT_NUMBER_VORGANG_01).Name, inputParameterKeys, inputParameterValues);
		}


		public string GetExportParameter_E_VORGANG(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_VORGANG");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
