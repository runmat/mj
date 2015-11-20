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
	public partial class Z_M_SET_FAHRER_AUFTRAGS_STATUS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_SET_FAHRER_AUFTRAGS_STATUS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_SET_FAHRER_AUFTRAGS_STATUS).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_FAHRER_STATUS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRER_STATUS", value);
		}

		public void SetImportParameter_I_VBELN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VBELN", value);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
