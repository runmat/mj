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
	public partial class Z_ZLD_CJ2_CASHJOURNALDOC_CRE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_CJ2_CASHJOURNALDOC_CRE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_CJ2_CASHJOURNALDOC_CRE).Name, inputParameterKeys, inputParameterValues);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
