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
	public partial class Z_ZLD_CJ2_GET_ALL_DOCS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_ALL_DOCS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_ALL_DOCS).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_DOCS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BUKRS { get; set; }

			public string CAJO_NUMBER { get; set; }

			public string POSTING_NUMBER { get; set; }

			public string TRANSACT_NUMBER { get; set; }

			public string TRANSACT_NAME { get; set; }

			public string WAERS { get; set; }

			public DateTime? BUDAT { get; set; }

			public decimal? H_RECEIPTS { get; set; }

			public decimal? H_PAYMENTS { get; set; }

			public decimal? H_NET_AMOUNT { get; set; }

			public decimal? H_TAX_AMOUNT { get; set; }

			public string TAX_CODE { get; set; }

			public string KOKRS { get; set; }

			public string KOSTL { get; set; }

			public string ORDERID { get; set; }

			public string ZUONR { get; set; }

			public string SGTXT { get; set; }

			public string DOCUMENT_NUMBER { get; set; }

			public string LIFNR { get; set; }

			public string KUNNR { get; set; }

			public string GL_ACCOUNT { get; set; }

			public string STATUS { get; set; }

			public string ASTATUS { get; set; }

			public static GT_DOCS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DOCS
				{
					BUKRS = (string)row["BUKRS"],
					CAJO_NUMBER = (string)row["CAJO_NUMBER"],
					POSTING_NUMBER = (string)row["POSTING_NUMBER"],
					TRANSACT_NUMBER = (string)row["TRANSACT_NUMBER"],
					TRANSACT_NAME = (string)row["TRANSACT_NAME"],
					WAERS = (string)row["WAERS"],
					BUDAT = (string.IsNullOrEmpty(row["BUDAT"].ToString())) ? null : (DateTime?)row["BUDAT"],
					H_RECEIPTS = (decimal?)row["H_RECEIPTS"],
					H_PAYMENTS = (decimal?)row["H_PAYMENTS"],
					H_NET_AMOUNT = (decimal?)row["H_NET_AMOUNT"],
					H_TAX_AMOUNT = (decimal?)row["H_TAX_AMOUNT"],
					TAX_CODE = (string)row["TAX_CODE"],
					KOKRS = (string)row["KOKRS"],
					KOSTL = (string)row["KOSTL"],
					ORDERID = (string)row["ORDERID"],
					ZUONR = (string)row["ZUONR"],
					SGTXT = (string)row["SGTXT"],
					DOCUMENT_NUMBER = (string)row["DOCUMENT_NUMBER"],
					LIFNR = (string)row["LIFNR"],
					KUNNR = (string)row["KUNNR"],
					GL_ACCOUNT = (string)row["GL_ACCOUNT"],
					STATUS = (string)row["STATUS"],
					ASTATUS = (string)row["ASTATUS"],

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

			public static IEnumerable<GT_DOCS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DOCS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_DOCS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DOCS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DOCS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DOCS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_CJ2_GET_ALL_DOCS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_CJ2_GET_ALL_DOCS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_DOCS_P : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BUKRS { get; set; }

			public string CAJO_NUMBER { get; set; }

			public string POSTING_NUMBER { get; set; }

			public string POSITION_NUMBER { get; set; }

			public string TRANSACT_NUMBER { get; set; }

			public string TRANSACT_NAME { get; set; }

			public decimal? P_RECEIPTS { get; set; }

			public decimal? P_PAYMENTS { get; set; }

			public decimal? P_NET_AMOUNT { get; set; }

			public decimal? P_TAX_AMOUNT { get; set; }

			public string TAX_CODE { get; set; }

			public string KOKRS { get; set; }

			public string KOSTL { get; set; }

			public string ORDERID { get; set; }

			public string SGTXT { get; set; }

			public string LIFNR { get; set; }

			public string KUNNR { get; set; }

			public string GL_ACCOUNT { get; set; }

			public string ALLOC_NMBR { get; set; }

			public static GT_DOCS_P Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DOCS_P
				{
					BUKRS = (string)row["BUKRS"],
					CAJO_NUMBER = (string)row["CAJO_NUMBER"],
					POSTING_NUMBER = (string)row["POSTING_NUMBER"],
					POSITION_NUMBER = (string)row["POSITION_NUMBER"],
					TRANSACT_NUMBER = (string)row["TRANSACT_NUMBER"],
					TRANSACT_NAME = (string)row["TRANSACT_NAME"],
					P_RECEIPTS = (decimal?)row["P_RECEIPTS"],
					P_PAYMENTS = (decimal?)row["P_PAYMENTS"],
					P_NET_AMOUNT = (decimal?)row["P_NET_AMOUNT"],
					P_TAX_AMOUNT = (decimal?)row["P_TAX_AMOUNT"],
					TAX_CODE = (string)row["TAX_CODE"],
					KOKRS = (string)row["KOKRS"],
					KOSTL = (string)row["KOSTL"],
					ORDERID = (string)row["ORDERID"],
					SGTXT = (string)row["SGTXT"],
					LIFNR = (string)row["LIFNR"],
					KUNNR = (string)row["KUNNR"],
					GL_ACCOUNT = (string)row["GL_ACCOUNT"],
					ALLOC_NMBR = (string)row["ALLOC_NMBR"],

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

			public static IEnumerable<GT_DOCS_P> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DOCS_P> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_DOCS_P> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DOCS_P).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DOCS_P> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS_P> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DOCS_P> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS_P>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_CJ2_GET_ALL_DOCS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS_P> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS_P>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS_P> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS_P>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS_P> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS_P>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_CJ2_GET_ALL_DOCS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOCS_P> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOCS_P>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_CJ2_GET_ALL_DOCS", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_CJ2_GET_ALL_DOCS", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_ZLD_CJ2_GET_ALL_DOCS.GT_DOCS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_CJ2_GET_ALL_DOCS.GT_DOCS> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_CJ2_GET_ALL_DOCS.GT_DOCS_P> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_CJ2_GET_ALL_DOCS.GT_DOCS_P> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_CJ2_GET_ALL_DOCS.GT_TRANSACTIONS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_CJ2_GET_ALL_DOCS.GT_TRANSACTIONS> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
