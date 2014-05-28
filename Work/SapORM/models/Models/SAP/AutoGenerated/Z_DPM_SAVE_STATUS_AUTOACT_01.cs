using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_SAVE_STATUS_AUTOACT_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_SAVE_STATUS_AUTOACT_01).Name);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
