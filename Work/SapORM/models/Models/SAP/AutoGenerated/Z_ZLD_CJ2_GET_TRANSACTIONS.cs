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
	public partial class Z_ZLD_CJ2_GET_TRANSACTIONS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_TRANSACTIONS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_TRANSACTIONS).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_EIN_AUS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EIN_AUS", value);
		}

		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_TRANSACTIONS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BUKRS { get; set; }

			public string TRANSACT_NUMBER { get; set; }

			public string TRANSACT_NAME { get; set; }

			public string TRANSACT_TYPE { get; set; }

			public string TAX_CODE { get; set; }

			public string ZUONR_SPERR { get; set; }

			public string ZUONR_VBL { get; set; }

			public string TEXT_SPERR { get; set; }

			public string TEXT_VBL { get; set; }

			public static GT_TRANSACTIONS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TRANSACTIONS
				{
					BUKRS = (string)row["BUKRS"],
					TRANSACT_NUMBER = (string)row["TRANSACT_NUMBER"],
					TRANSACT_NAME = (string)row["TRANSACT_NAME"],
					TRANSACT_TYPE = (string)row["TRANSACT_TYPE"],
					TAX_CODE = (string)row["TAX_CODE"],
					ZUONR_SPERR = (string)row["ZUONR_SPERR"],
					ZUONR_VBL = (string)row["ZUONR_VBL"],
					TEXT_SPERR = (string)row["TEXT_SPERR"],
					TEXT_VBL = (string)row["TEXT_VBL"],

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

			public static IEnumerable<GT_TRANSACTIONS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TRANSACTIONS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TRANSACTIONS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TRANSACTIONS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TRANSACTIONS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TRANSACTIONS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TRANSACTIONS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TRANSACTIONS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_CJ2_GET_TRANSACTIONS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TRANSACTIONS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TRANSACTIONS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TRANSACTIONS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TRANSACTIONS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TRANSACTIONS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TRANSACTIONS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_CJ2_GET_TRANSACTIONS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TRANSACTIONS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TRANSACTIONS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_CJ2_GET_TRANSACTIONS.GT_TRANSACTIONS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
