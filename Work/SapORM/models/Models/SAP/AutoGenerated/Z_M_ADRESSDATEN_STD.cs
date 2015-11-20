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
	public partial class Z_M_ADRESSDATEN_STD
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_ADRESSDATEN_STD).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_ADRESSDATEN_STD).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public void SetImportParameter_I_ALL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ALL", value);
		}

		public void SetImportParameter_I_HAENDLER_EX(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_HAENDLER_EX", value);
		}

		public void SetImportParameter_I_HDGRP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_HDGRP", value);
		}

		public void SetImportParameter_I_KONTINGENT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KONTINGENT", value);
		}

		public void SetImportParameter_I_MAX(ISapDataService sap, int? value)
		{
			sap.SetImportParameter("I_MAX", value);
		}

		public void SetImportParameter_I_NAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NAME", value);
		}

		public void SetImportParameter_I_ORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ORT", value);
		}

		public void SetImportParameter_I_PSTLZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PSTLZ", value);
		}

		public int? GetExportParameter_E_REC_ANZ(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_REC_ANZ");
		}

		public partial class GT_ADRS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AG { get; set; }

			public string HDGRP_EX { get; set; }

			public string HDGRP { get; set; }

			public string HAENDLER_EX { get; set; }

			public string HAENDLER { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STRAS { get; set; }

			public string PSTLZ { get; set; }

			public string ORT01 { get; set; }

			public string LAND1 { get; set; }

			public static GT_ADRS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ADRS
				{
					AG = (string)row["AG"],
					HDGRP_EX = (string)row["HDGRP_EX"],
					HDGRP = (string)row["HDGRP"],
					HAENDLER_EX = (string)row["HAENDLER_EX"],
					HAENDLER = (string)row["HAENDLER"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STRAS = (string)row["STRAS"],
					PSTLZ = (string)row["PSTLZ"],
					ORT01 = (string)row["ORT01"],
					LAND1 = (string)row["LAND1"],

					SAPConnection = sapConnection,
					DynSapProxyFactory = dynSapProxyFactory,
				};
				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnInitFromExtern();

			public void OnModelMappingApplied()
			{
				OnInitFromExtern();
			}

			public static IEnumerable<GT_ADRS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ADRS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ADRS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ADRS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ADRS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ADRS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_ADRESSDATEN_STD", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_ADRESSDATEN_STD", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_ADRESSDATEN_STD.GT_ADRS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
