using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_Eingabe_Lvnummer_Sixtleas
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_Eingabe_Lvnummer_Sixtleas).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_Eingabe_Lvnummer_Sixtleas).Name, inputParameterKeys, inputParameterValues);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
