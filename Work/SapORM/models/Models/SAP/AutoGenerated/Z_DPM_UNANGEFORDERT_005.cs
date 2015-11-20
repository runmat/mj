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
	public partial class Z_DPM_UNANGEFORDERT_005
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_UNANGEFORDERT_005).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_UNANGEFORDERT_005).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public void SetImportParameter_I_EQTYP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQTYP", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_LICENSE_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LICENSE_NUM", value);
		}

		public void SetImportParameter_I_LIZNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIZNR", value);
		}

		public void SetImportParameter_I_TIDNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TIDNR", value);
		}

		public void SetImportParameter_I_ZZREFERENZ1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFERENZ1", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_GRU : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string SPRAS { get; set; }

			public string ZZVGRUND { get; set; }

			public string TEXT50 { get; set; }

			public static GT_GRU Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_GRU
				{
					MANDT = (string)row["MANDT"],
					SPRAS = (string)row["SPRAS"],
					ZZVGRUND = (string)row["ZZVGRUND"],
					TEXT50 = (string)row["TEXT50"],

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

			public static IEnumerable<GT_GRU> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_GRU> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_GRU> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_GRU).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_GRU> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRU> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_GRU> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_GRU>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_UNANGEFORDERT_005", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRU> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_GRU>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRU> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_GRU>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRU> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_GRU>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_UNANGEFORDERT_005", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_GRU> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_GRU>();
				 
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

			public string MANDT { get; set; }

			public string KONZS { get; set; }

			public string KNRZE { get; set; }

			public string KUNNR { get; set; }

			public string ZZVBELN { get; set; }

			public DateTime? ERDAT { get; set; }

			public string EQUNR { get; set; }

			public string EQTYP { get; set; }

			public string ZZCOCKZ { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LIZNR { get; set; }

			public string TIDNR { get; set; }

			public string ZZKKBER { get; set; }

			public string ZZBEZAHLT { get; set; }

			public string CMGST { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public DateTime? ZZANFDT { get; set; }

			public string ZZANFZT { get; set; }

			public DateTime? ZZFAEDT { get; set; }

			public DateTime? ZZABRDT { get; set; }

			public DateTime? ZZFREIDT { get; set; }

			public string ZZFREIZT { get; set; }

			public string ZZMAHNA { get; set; }

			public string ABCKZ { get; set; }

			public string MSGRP { get; set; }

			public string STORT { get; set; }

			public string ZZFINART { get; set; }

			public string ZZLABEL { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public string ADRNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string CITY1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string HINWEIS { get; set; }

			public DateTime? VERDT1 { get; set; }

			public DateTime? VERDT2 { get; set; }

			public DateTime? VERDT3 { get; set; }

			public DateTime? WIEDT1 { get; set; }

			public DateTime? WIEDT2 { get; set; }

			public DateTime? WIEDT3 { get; set; }

			public DateTime? COCDT1 { get; set; }

			public string CRBLB { get; set; }

			public string ZZSPERR_DAD { get; set; }

			public string ZZINSOLVENZ { get; set; }

			public string TEXT50 { get; set; }

			public string TEXT200 { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZSTATUS_ZUG { get; set; }

			public string ZZSTATUS_IABG { get; set; }

			public string ZZSTATUS_ABG { get; set; }

			public string ZZVGRUND { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					MANDT = (string)row["MANDT"],
					KONZS = (string)row["KONZS"],
					KNRZE = (string)row["KNRZE"],
					KUNNR = (string)row["KUNNR"],
					ZZVBELN = (string)row["ZZVBELN"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					EQUNR = (string)row["EQUNR"],
					EQTYP = (string)row["EQTYP"],
					ZZCOCKZ = (string)row["ZZCOCKZ"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					LIZNR = (string)row["LIZNR"],
					TIDNR = (string)row["TIDNR"],
					ZZKKBER = (string)row["ZZKKBER"],
					ZZBEZAHLT = (string)row["ZZBEZAHLT"],
					CMGST = (string)row["CMGST"],
					ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
					ZZANFDT = string.IsNullOrEmpty(row["ZZANFDT"].ToString()) ? null : (DateTime?)row["ZZANFDT"],
					ZZANFZT = (string)row["ZZANFZT"],
					ZZFAEDT = string.IsNullOrEmpty(row["ZZFAEDT"].ToString()) ? null : (DateTime?)row["ZZFAEDT"],
					ZZABRDT = string.IsNullOrEmpty(row["ZZABRDT"].ToString()) ? null : (DateTime?)row["ZZABRDT"],
					ZZFREIDT = string.IsNullOrEmpty(row["ZZFREIDT"].ToString()) ? null : (DateTime?)row["ZZFREIDT"],
					ZZFREIZT = (string)row["ZZFREIZT"],
					ZZMAHNA = (string)row["ZZMAHNA"],
					ABCKZ = (string)row["ABCKZ"],
					MSGRP = (string)row["MSGRP"],
					STORT = (string)row["STORT"],
					ZZFINART = (string)row["ZZFINART"],
					ZZLABEL = (string)row["ZZLABEL"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
					ADRNR = (string)row["ADRNR"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					CITY1 = (string)row["CITY1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					HINWEIS = (string)row["HINWEIS"],
					VERDT1 = string.IsNullOrEmpty(row["VERDT1"].ToString()) ? null : (DateTime?)row["VERDT1"],
					VERDT2 = string.IsNullOrEmpty(row["VERDT2"].ToString()) ? null : (DateTime?)row["VERDT2"],
					VERDT3 = string.IsNullOrEmpty(row["VERDT3"].ToString()) ? null : (DateTime?)row["VERDT3"],
					WIEDT1 = string.IsNullOrEmpty(row["WIEDT1"].ToString()) ? null : (DateTime?)row["WIEDT1"],
					WIEDT2 = string.IsNullOrEmpty(row["WIEDT2"].ToString()) ? null : (DateTime?)row["WIEDT2"],
					WIEDT3 = string.IsNullOrEmpty(row["WIEDT3"].ToString()) ? null : (DateTime?)row["WIEDT3"],
					COCDT1 = string.IsNullOrEmpty(row["COCDT1"].ToString()) ? null : (DateTime?)row["COCDT1"],
					CRBLB = (string)row["CRBLB"],
					ZZSPERR_DAD = (string)row["ZZSPERR_DAD"],
					ZZINSOLVENZ = (string)row["ZZINSOLVENZ"],
					TEXT50 = (string)row["TEXT50"],
					TEXT200 = (string)row["TEXT200"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					EXPIRY_DATE = string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString()) ? null : (DateTime?)row["EXPIRY_DATE"],
					ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
					ZZSTATUS_ZUG = (string)row["ZZSTATUS_ZUG"],
					ZZSTATUS_IABG = (string)row["ZZSTATUS_IABG"],
					ZZSTATUS_ABG = (string)row["ZZSTATUS_ABG"],
					ZZVGRUND = (string)row["ZZVGRUND"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_UNANGEFORDERT_005", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_UNANGEFORDERT_005", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_UNANGEFORDERT_005.GT_GRU> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_UNANGEFORDERT_005.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
