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
	public partial class Z_ZLD_CJ2_CALC_MWST
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_CJ2_CALC_MWST).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_CJ2_CALC_MWST).Name, inputParameterKeys, inputParameterValues);
		}


		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class IS_DOCS_K : IModelMappingApplied
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

			public string WAERS { get; set; }

			public DateTime? BUDAT { get; set; }

			public string DOCUMENT_NUMBER { get; set; }

			public string STATUS { get; set; }

			public string ASTATUS { get; set; }

			public static IS_DOCS_K Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new IS_DOCS_K
				{
					BUKRS = (string)row["BUKRS"],
					CAJO_NUMBER = (string)row["CAJO_NUMBER"],
					POSTING_NUMBER = (string)row["POSTING_NUMBER"],
					WAERS = (string)row["WAERS"],
					BUDAT = string.IsNullOrEmpty(row["BUDAT"].ToString()) ? null : (DateTime?)row["BUDAT"],
					DOCUMENT_NUMBER = (string)row["DOCUMENT_NUMBER"],
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

			public static IEnumerable<IS_DOCS_K> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static IEnumerable<IS_DOCS_K> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(IS_DOCS_K).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<IS_DOCS_K> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IS_DOCS_K>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_CJ2_CALC_MWST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IS_DOCS_K> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IS_DOCS_K>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class ES_DOCS_K : IModelMappingApplied
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

			public string WAERS { get; set; }

			public DateTime? BUDAT { get; set; }

			public string DOCUMENT_NUMBER { get; set; }

			public string STATUS { get; set; }

			public string ASTATUS { get; set; }

			public static ES_DOCS_K Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ES_DOCS_K
				{
					BUKRS = (string)row["BUKRS"],
					CAJO_NUMBER = (string)row["CAJO_NUMBER"],
					POSTING_NUMBER = (string)row["POSTING_NUMBER"],
					WAERS = (string)row["WAERS"],
					BUDAT = string.IsNullOrEmpty(row["BUDAT"].ToString()) ? null : (DateTime?)row["BUDAT"],
					DOCUMENT_NUMBER = (string)row["DOCUMENT_NUMBER"],
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

			public static IEnumerable<ES_DOCS_K> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ES_DOCS_K> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ES_DOCS_K> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ES_DOCS_K).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ES_DOCS_K> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ES_DOCS_K> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ES_DOCS_K> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ES_DOCS_K>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_CJ2_CALC_MWST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_DOCS_K> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_DOCS_K>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_DOCS_K> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_DOCS_K>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_POS : IModelMappingApplied
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

			public static GT_POS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_POS
				{
					BUKRS = (string)row["BUKRS"],
					CAJO_NUMBER = (string)row["CAJO_NUMBER"],
					POSTING_NUMBER = (string)row["POSTING_NUMBER"],
					POSITION_NUMBER = (string)row["POSITION_NUMBER"],
					TRANSACT_NUMBER = (string)row["TRANSACT_NUMBER"],
					TRANSACT_NAME = (string)row["TRANSACT_NAME"],
					P_RECEIPTS = string.IsNullOrEmpty(row["P_RECEIPTS"].ToString()) ? null : (decimal?)row["P_RECEIPTS"],
					P_PAYMENTS = string.IsNullOrEmpty(row["P_PAYMENTS"].ToString()) ? null : (decimal?)row["P_PAYMENTS"],
					P_NET_AMOUNT = string.IsNullOrEmpty(row["P_NET_AMOUNT"].ToString()) ? null : (decimal?)row["P_NET_AMOUNT"],
					P_TAX_AMOUNT = string.IsNullOrEmpty(row["P_TAX_AMOUNT"].ToString()) ? null : (decimal?)row["P_TAX_AMOUNT"],
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

			public static IEnumerable<GT_POS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_POS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_POS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_POS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_POS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_POS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_CJ2_CALC_MWST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_CJ2_CALC_MWST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_CJ2_CALC_MWST.IS_DOCS_K> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_CJ2_CALC_MWST.ES_DOCS_K> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_CJ2_CALC_MWST.GT_POS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
