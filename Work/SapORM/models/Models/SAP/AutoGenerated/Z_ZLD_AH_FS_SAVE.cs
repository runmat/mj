using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_ZLD_AH_FS_SAVE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_AH_FS_SAVE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_AH_FS_SAVE).Name, inputParameterKeys, inputParameterValues);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
