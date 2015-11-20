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
	public partial class Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KOSTL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KOSTL", value);
		}

		public static void SetImportParameter_I_LIFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIFNR", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_PO_K : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BSTNR { get; set; }

			public string LIFNR { get; set; }

			public string NAME1 { get; set; }

			public DateTime? BEDAT { get; set; }

			public string EKORG { get; set; }

			public string BUKRS { get; set; }

			public string KOSTL { get; set; }

			public string EKGRP { get; set; }

			public string EBELN { get; set; }

			public string LIEFERSNR { get; set; }

			public string MVT_BELNR { get; set; }

			public DateTime? EEIND { get; set; }

			public static GT_PO_K Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_PO_K
				{
					BSTNR = (string)row["BSTNR"],
					LIFNR = (string)row["LIFNR"],
					NAME1 = (string)row["NAME1"],
					BEDAT = string.IsNullOrEmpty(row["BEDAT"].ToString()) ? null : (DateTime?)row["BEDAT"],
					EKORG = (string)row["EKORG"],
					BUKRS = (string)row["BUKRS"],
					KOSTL = (string)row["KOSTL"],
					EKGRP = (string)row["EKGRP"],
					EBELN = (string)row["EBELN"],
					LIEFERSNR = (string)row["LIEFERSNR"],
					MVT_BELNR = (string)row["MVT_BELNR"],
					EEIND = string.IsNullOrEmpty(row["EEIND"].ToString()) ? null : (DateTime?)row["EEIND"],

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

			public static IEnumerable<GT_PO_K> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_PO_K> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_PO_K> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_PO_K).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_PO_K> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_K> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_PO_K> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PO_K>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_K> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PO_K>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_K> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PO_K>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_K> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PO_K>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_K> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PO_K>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_PO_P : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BSTNR { get; set; }

			public string EBELP { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public string ARTLIF { get; set; }

			public decimal? MENGE { get; set; }

			public DateTime? EEIND { get; set; }

			public string WERKS { get; set; }

			public string LGORT { get; set; }

			public decimal? PREIS { get; set; }

			public string LTEXT_NR { get; set; }

			public static GT_PO_P Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_PO_P
				{
					BSTNR = (string)row["BSTNR"],
					EBELP = (string)row["EBELP"],
					MATNR = (string)row["MATNR"],
					MAKTX = (string)row["MAKTX"],
					ARTLIF = (string)row["ARTLIF"],
					MENGE = string.IsNullOrEmpty(row["MENGE"].ToString()) ? null : (decimal?)row["MENGE"],
					EEIND = string.IsNullOrEmpty(row["EEIND"].ToString()) ? null : (DateTime?)row["EEIND"],
					WERKS = (string)row["WERKS"],
					LGORT = (string)row["LGORT"],
					PREIS = string.IsNullOrEmpty(row["PREIS"].ToString()) ? null : (decimal?)row["PREIS"],
					LTEXT_NR = (string)row["LTEXT_NR"],

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

			public static IEnumerable<GT_PO_P> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_PO_P> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_PO_P> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_PO_P).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_PO_P> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_P> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_PO_P> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PO_P>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_P> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PO_P>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_P> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PO_P>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_P> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PO_P>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_PO_P> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PO_P>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST.GT_PO_K> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST.GT_PO_P> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
