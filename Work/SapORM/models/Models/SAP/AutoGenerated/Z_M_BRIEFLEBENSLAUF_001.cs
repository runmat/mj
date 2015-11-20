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
	public partial class Z_M_BRIEFLEBENSLAUF_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_BRIEFLEBENSLAUF_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_BRIEFLEBENSLAUF_001).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_EQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQUNR", value);
		}

		public void SetImportParameter_I_FCE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FCE", value);
		}

		public void SetImportParameter_I_HAENDLER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_HAENDLER", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_SPRAS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SPRAS", value);
		}

		public void SetImportParameter_I_ZZBRIEF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZBRIEF", value);
		}

		public void SetImportParameter_I_ZZFAHRG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZFAHRG", value);
		}

		public void SetImportParameter_I_ZZKENN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZKENN", value);
		}

		public void SetImportParameter_I_ZZREF1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREF1", value);
		}

		public partial class GT_ADDR : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ADDRTYP { get; set; }

			public string ADDRTYP_TEXT { get; set; }

			public string EX_KUNNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string COUNTRY { get; set; }

			public string TEL_NUMBER { get; set; }

			public string FAX_NUMBER { get; set; }

			public string SMTP_ADDR { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? AEDAT { get; set; }

			public static GT_ADDR Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ADDR
				{
					ADDRTYP = (string)row["ADDRTYP"],
					ADDRTYP_TEXT = (string)row["ADDRTYP_TEXT"],
					EX_KUNNR = (string)row["EX_KUNNR"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					COUNTRY = (string)row["COUNTRY"],
					TEL_NUMBER = (string)row["TEL_NUMBER"],
					FAX_NUMBER = (string)row["FAX_NUMBER"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					AEDAT = string.IsNullOrEmpty(row["AEDAT"].ToString()) ? null : (DateTime?)row["AEDAT"],

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

			public static IEnumerable<GT_ADDR> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ADDR> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ADDR> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ADDR).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ADDR> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ADDR> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADDR> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADDR>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_EQUI : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LIZNR { get; set; }

			public string TIDNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string ABCKZ { get; set; }

			public string EX_KUNNR { get; set; }

			public string OBJNR { get; set; }

			public string ZZFINART_TXT { get; set; }

			public string ZZVGRUND { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public static GT_EQUI Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EQUI
				{
					EQUNR = (string)row["EQUNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					LIZNR = (string)row["LIZNR"],
					TIDNR = (string)row["TIDNR"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
					ABCKZ = (string)row["ABCKZ"],
					EX_KUNNR = (string)row["EX_KUNNR"],
					OBJNR = (string)row["OBJNR"],
					ZZFINART_TXT = (string)row["ZZFINART_TXT"],
					ZZVGRUND = (string)row["ZZVGRUND"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],

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

			public static IEnumerable<GT_EQUI> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EQUI> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EQUI> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EQUI).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EQUI> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EQUI> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUI> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUI>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_QMEL : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string QMNUM { get; set; }

			public string ERNAM { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? STRMN { get; set; }

			public string KURZTEXT { get; set; }

			public string QMNAM { get; set; }

			public string KUNNR_Z5 { get; set; }

			public string ADRNR_Z5 { get; set; }

			public string NAME1_Z5 { get; set; }

			public string NAME2_Z5 { get; set; }

			public string STREET_Z5 { get; set; }

			public string HOUSE_NUM1_Z5 { get; set; }

			public string POST_CODE1_Z5 { get; set; }

			public string CITY1_Z5 { get; set; }

			public string LANDX_Z5 { get; set; }

			public string ZZDIEN1 { get; set; }

			public string QMGRP { get; set; }

			public string LICENSE_NUM { get; set; }

			public string ZZULDAT { get; set; }

			public string LIZNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public static GT_QMEL Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_QMEL
				{
					QMNUM = (string)row["QMNUM"],
					ERNAM = (string)row["ERNAM"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					STRMN = string.IsNullOrEmpty(row["STRMN"].ToString()) ? null : (DateTime?)row["STRMN"],
					KURZTEXT = (string)row["KURZTEXT"],
					QMNAM = (string)row["QMNAM"],
					KUNNR_Z5 = (string)row["KUNNR_Z5"],
					ADRNR_Z5 = (string)row["ADRNR_Z5"],
					NAME1_Z5 = (string)row["NAME1_Z5"],
					NAME2_Z5 = (string)row["NAME2_Z5"],
					STREET_Z5 = (string)row["STREET_Z5"],
					HOUSE_NUM1_Z5 = (string)row["HOUSE_NUM1_Z5"],
					POST_CODE1_Z5 = (string)row["POST_CODE1_Z5"],
					CITY1_Z5 = (string)row["CITY1_Z5"],
					LANDX_Z5 = (string)row["LANDX_Z5"],
					ZZDIEN1 = (string)row["ZZDIEN1"],
					QMGRP = (string)row["QMGRP"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ZZULDAT = (string)row["ZZULDAT"],
					LIZNR = (string)row["LIZNR"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],

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

			public static IEnumerable<GT_QMEL> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_QMEL> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_QMEL> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_QMEL).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_QMEL> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_QMEL> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMEL> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMEL>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_QMMA : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string QMNUM { get; set; }

			public string MANUM { get; set; }

			public string MNCOD { get; set; }

			public string MATXT { get; set; }

			public DateTime? AEDAT { get; set; }

			public string AEZEIT { get; set; }

			public DateTime? ZZUEBER { get; set; }

			public DateTime? PSTER { get; set; }

			public static GT_QMMA Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_QMMA
				{
					QMNUM = (string)row["QMNUM"],
					MANUM = (string)row["MANUM"],
					MNCOD = (string)row["MNCOD"],
					MATXT = (string)row["MATXT"],
					AEDAT = string.IsNullOrEmpty(row["AEDAT"].ToString()) ? null : (DateTime?)row["AEDAT"],
					AEZEIT = (string)row["AEZEIT"],
					ZZUEBER = string.IsNullOrEmpty(row["ZZUEBER"].ToString()) ? null : (DateTime?)row["ZZUEBER"],
					PSTER = string.IsNullOrEmpty(row["PSTER"].ToString()) ? null : (DateTime?)row["PSTER"],

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

			public static IEnumerable<GT_QMMA> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_QMMA> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_QMMA> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_QMMA).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_QMMA> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_QMMA> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_QMMA> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_QMMA>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_TEXT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string TDLINE { get; set; }

			public static GT_TEXT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TEXT
				{
					EQUNR = (string)row["EQUNR"],
					TDLINE = (string)row["TDLINE"],

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

			public static IEnumerable<GT_TEXT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TEXT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TEXT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TEXT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TEXT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TEXT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string NAME1_AG { get; set; }

			public string ORT01_AG { get; set; }

			public string QMNUM { get; set; }

			public string EQUNR { get; set; }

			public string ZZKENN { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZKENN_OLD { get; set; }

			public string ZZBRIEF_OLD { get; set; }

			public string ZZBRIEF_A { get; set; }

			public DateTime? UDATE { get; set; }

			public string ZZFAHRG { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public string ZZSTATUS_ZUL { get; set; }

			public string ZZSTATUS_ABG { get; set; }

			public string ZZSTATUS_BAG { get; set; }

			public string ZZSTATUS_OZU { get; set; }

			public string ZZSTATUS_ZUB { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string SCHILDER_PHY { get; set; }

			public string SCHEIN_PHY { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? MINDBIS { get; set; }

			public string DADPDI { get; set; }

			public string KUNPDI { get; set; }

			public string DADPDI_NAME1 { get; set; }

			public string ZZCODE { get; set; }

			public string ZZBEZEI { get; set; }

			public string HERST_K { get; set; }

			public string ZZSIPP3 { get; set; }

			public string ZZAUSF { get; set; }

			public string ZZANTR { get; set; }

			public string ZZREIFEN { get; set; }

			public string ZZNAVI { get; set; }

			public DateTime? ZZDAT_EIN { get; set; }

			public string ZZKLEBE { get; set; }

			public string ZZAKTSPERRE { get; set; }

			public DateTime? ZZCARPORT_EING { get; set; }

			public DateTime? ZZKENN_EING { get; set; }

			public string ADRNR_VS { get; set; }

			public string NAME1_VS { get; set; }

			public string NAME2_VS { get; set; }

			public string ORT01_VS { get; set; }

			public string PSTLZ_VS { get; set; }

			public string STRAS_VS { get; set; }

			public string HSNR_VS { get; set; }

			public string KUNNR_ZH { get; set; }

			public string ADRNR_ZH { get; set; }

			public string NAME1_ZH { get; set; }

			public string NAME2_ZH { get; set; }

			public string ORT01_ZH { get; set; }

			public string PSTLZ_ZH { get; set; }

			public string STRAS_ZH { get; set; }

			public string HSNR_ZH { get; set; }

			public string ZZREF1 { get; set; }

			public string ZZREF2 { get; set; }

			public decimal? ENGINE_POWER { get; set; }

			public DateTime? CHECK_IN { get; set; }

			public string ABCKZ { get; set; }

			public string ZZCOCKZ { get; set; }

			public string ZZVGRUND { get; set; }

			public string ZZCO2KOMBI { get; set; }

			public DateTime? ZZDAT_BER { get; set; }

			public string KUNNR_ZF { get; set; }

			public string ADRNR_ZF { get; set; }

			public string NAME1_ZF { get; set; }

			public string NAME2_ZF { get; set; }

			public string ORT01_ZF { get; set; }

			public string PSTLZ_ZF { get; set; }

			public string STRAS_ZF { get; set; }

			public string HSNR_ZF { get; set; }

			public string KUNNR_ZL { get; set; }

			public string ADRNR_ZL { get; set; }

			public string NAME1_ZL { get; set; }

			public string NAME2_ZL { get; set; }

			public string ORT01_ZL { get; set; }

			public string PSTLZ_ZL { get; set; }

			public string STRAS_ZL { get; set; }

			public string HSNR_ZL { get; set; }

			public string KONZS_ZK { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZFARBE { get; set; }

			public string KUNNR_ZA { get; set; }

			public string ADRNR_ZA { get; set; }

			public string NAME1_ZA { get; set; }

			public string NAME2_ZA { get; set; }

			public string STRAS_ZA { get; set; }

			public string HSNR_ZA { get; set; }

			public string PSTLZ_ZA { get; set; }

			public string ORT01_ZA { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZLABEL { get; set; }

			public string BANK { get; set; }

			public string BRANDING { get; set; }

			public string BEZEICHNUNG_1 { get; set; }

			public string BEZEICHNUNG_2 { get; set; }

			public string BEZEICHNUNG_3 { get; set; }

			public string DADPDI_ORT { get; set; }

			public string ZZFINART_TXT { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					KUNNR = (string)row["KUNNR"],
					NAME1_AG = (string)row["NAME1_AG"],
					ORT01_AG = (string)row["ORT01_AG"],
					QMNUM = (string)row["QMNUM"],
					EQUNR = (string)row["EQUNR"],
					ZZKENN = (string)row["ZZKENN"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					ZZKENN_OLD = (string)row["ZZKENN_OLD"],
					ZZBRIEF_OLD = (string)row["ZZBRIEF_OLD"],
					ZZBRIEF_A = (string)row["ZZBRIEF_A"],
					UDATE = string.IsNullOrEmpty(row["UDATE"].ToString()) ? null : (DateTime?)row["UDATE"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					EXPIRY_DATE = string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString()) ? null : (DateTime?)row["EXPIRY_DATE"],
					ZZSTATUS_ZUL = (string)row["ZZSTATUS_ZUL"],
					ZZSTATUS_ABG = (string)row["ZZSTATUS_ABG"],
					ZZSTATUS_BAG = (string)row["ZZSTATUS_BAG"],
					ZZSTATUS_OZU = (string)row["ZZSTATUS_OZU"],
					ZZSTATUS_ZUB = (string)row["ZZSTATUS_ZUB"],
					ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
					SCHILDER_PHY = (string)row["SCHILDER_PHY"],
					SCHEIN_PHY = (string)row["SCHEIN_PHY"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					MINDBIS = string.IsNullOrEmpty(row["MINDBIS"].ToString()) ? null : (DateTime?)row["MINDBIS"],
					DADPDI = (string)row["DADPDI"],
					KUNPDI = (string)row["KUNPDI"],
					DADPDI_NAME1 = (string)row["DADPDI_NAME1"],
					ZZCODE = (string)row["ZZCODE"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					HERST_K = (string)row["HERST_K"],
					ZZSIPP3 = (string)row["ZZSIPP3"],
					ZZAUSF = (string)row["ZZAUSF"],
					ZZANTR = (string)row["ZZANTR"],
					ZZREIFEN = (string)row["ZZREIFEN"],
					ZZNAVI = (string)row["ZZNAVI"],
					ZZDAT_EIN = string.IsNullOrEmpty(row["ZZDAT_EIN"].ToString()) ? null : (DateTime?)row["ZZDAT_EIN"],
					ZZKLEBE = (string)row["ZZKLEBE"],
					ZZAKTSPERRE = (string)row["ZZAKTSPERRE"],
					ZZCARPORT_EING = string.IsNullOrEmpty(row["ZZCARPORT_EING"].ToString()) ? null : (DateTime?)row["ZZCARPORT_EING"],
					ZZKENN_EING = string.IsNullOrEmpty(row["ZZKENN_EING"].ToString()) ? null : (DateTime?)row["ZZKENN_EING"],
					ADRNR_VS = (string)row["ADRNR_VS"],
					NAME1_VS = (string)row["NAME1_VS"],
					NAME2_VS = (string)row["NAME2_VS"],
					ORT01_VS = (string)row["ORT01_VS"],
					PSTLZ_VS = (string)row["PSTLZ_VS"],
					STRAS_VS = (string)row["STRAS_VS"],
					HSNR_VS = (string)row["HSNR_VS"],
					KUNNR_ZH = (string)row["KUNNR_ZH"],
					ADRNR_ZH = (string)row["ADRNR_ZH"],
					NAME1_ZH = (string)row["NAME1_ZH"],
					NAME2_ZH = (string)row["NAME2_ZH"],
					ORT01_ZH = (string)row["ORT01_ZH"],
					PSTLZ_ZH = (string)row["PSTLZ_ZH"],
					STRAS_ZH = (string)row["STRAS_ZH"],
					HSNR_ZH = (string)row["HSNR_ZH"],
					ZZREF1 = (string)row["ZZREF1"],
					ZZREF2 = (string)row["ZZREF2"],
					ENGINE_POWER = string.IsNullOrEmpty(row["ENGINE_POWER"].ToString()) ? null : (decimal?)row["ENGINE_POWER"],
					CHECK_IN = string.IsNullOrEmpty(row["CHECK_IN"].ToString()) ? null : (DateTime?)row["CHECK_IN"],
					ABCKZ = (string)row["ABCKZ"],
					ZZCOCKZ = (string)row["ZZCOCKZ"],
					ZZVGRUND = (string)row["ZZVGRUND"],
					ZZCO2KOMBI = (string)row["ZZCO2KOMBI"],
					ZZDAT_BER = string.IsNullOrEmpty(row["ZZDAT_BER"].ToString()) ? null : (DateTime?)row["ZZDAT_BER"],
					KUNNR_ZF = (string)row["KUNNR_ZF"],
					ADRNR_ZF = (string)row["ADRNR_ZF"],
					NAME1_ZF = (string)row["NAME1_ZF"],
					NAME2_ZF = (string)row["NAME2_ZF"],
					ORT01_ZF = (string)row["ORT01_ZF"],
					PSTLZ_ZF = (string)row["PSTLZ_ZF"],
					STRAS_ZF = (string)row["STRAS_ZF"],
					HSNR_ZF = (string)row["HSNR_ZF"],
					KUNNR_ZL = (string)row["KUNNR_ZL"],
					ADRNR_ZL = (string)row["ADRNR_ZL"],
					NAME1_ZL = (string)row["NAME1_ZL"],
					NAME2_ZL = (string)row["NAME2_ZL"],
					ORT01_ZL = (string)row["ORT01_ZL"],
					PSTLZ_ZL = (string)row["PSTLZ_ZL"],
					STRAS_ZL = (string)row["STRAS_ZL"],
					HSNR_ZL = (string)row["HSNR_ZL"],
					KONZS_ZK = (string)row["KONZS_ZK"],
					ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZFARBE = (string)row["ZFARBE"],
					KUNNR_ZA = (string)row["KUNNR_ZA"],
					ADRNR_ZA = (string)row["ADRNR_ZA"],
					NAME1_ZA = (string)row["NAME1_ZA"],
					NAME2_ZA = (string)row["NAME2_ZA"],
					STRAS_ZA = (string)row["STRAS_ZA"],
					HSNR_ZA = (string)row["HSNR_ZA"],
					PSTLZ_ZA = (string)row["PSTLZ_ZA"],
					ORT01_ZA = (string)row["ORT01_ZA"],
					ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ZZLABEL = (string)row["ZZLABEL"],
					BANK = (string)row["BANK"],
					BRANDING = (string)row["BRANDING"],
					BEZEICHNUNG_1 = (string)row["BEZEICHNUNG_1"],
					BEZEICHNUNG_2 = (string)row["BEZEICHNUNG_2"],
					BEZEICHNUNG_3 = (string)row["BEZEICHNUNG_3"],
					DADPDI_ORT = (string)row["DADPDI_ORT"],
					ZZFINART_TXT = (string)row["ZZFINART_TXT"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],

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

			public static IEnumerable<GT_WEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_BRIEFLEBENSLAUF_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_BRIEFLEBENSLAUF_001.GT_ADDR> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_BRIEFLEBENSLAUF_001.GT_EQUI> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_BRIEFLEBENSLAUF_001.GT_QMEL> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_BRIEFLEBENSLAUF_001.GT_QMMA> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_BRIEFLEBENSLAUF_001.GT_TEXT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_BRIEFLEBENSLAUF_001.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
