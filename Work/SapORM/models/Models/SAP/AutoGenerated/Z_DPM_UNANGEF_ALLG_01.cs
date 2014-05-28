using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_UNANGEF_ALLG_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_UNANGEF_ALLG_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_UNANGEF_ALLG_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LIZNR { get; set; }

			public string TIDNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public static GT_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IN
				{
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					LIZNR = (string)row["LIZNR"],
					TIDNR = (string)row["TIDNR"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
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

			public static IEnumerable<GT_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_IN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_UNANGEF_ALLG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_UNANGEF_ALLG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_ABRUFBAR : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string EQTYP { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LIZNR { get; set; }

			public string TIDNR { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZSTATUS_ZUG { get; set; }

			public string ZZSTATUS_IABG { get; set; }

			public string ZZSTATUS_ABG { get; set; }

			public string ABCKZ { get; set; }

			public string MSGRP { get; set; }

			public string PARVW_2 { get; set; }

			public string KUNNR_2 { get; set; }

			public string TREUH_YA { get; set; }

			public string TREUH_YZ { get; set; }

			public string TREUH_YE { get; set; }

			public string TREUH_YT { get; set; }

			public string KUNNR_YA { get; set; }

			public string KUNNR_YZ { get; set; }

			public string KUNNR_YE { get; set; }

			public string KUNNR_YT { get; set; }

			public string NAME1_ZS { get; set; }

			public string NAME2_ZS { get; set; }

			public string STREET_ZS { get; set; }

			public string HOUSE_NUM1_ZS { get; set; }

			public string POST_CODE1_ZS { get; set; }

			public string CITY1_ZS { get; set; }

			public string COUNTRY_ZS { get; set; }

			public string TEXT { get; set; }

			public string FEHLERTEXT { get; set; }

			public string ZZCOCKZ { get; set; }

			public string KUNNR_ZP { get; set; }

			public DateTime? ZZABMDAT { get; set; }

			public string RESTLAUFZEIT { get; set; }

			public DateTime? ABMAUF { get; set; }

			public string NAME1_ZP { get; set; }

			public static GT_ABRUFBAR Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ABRUFBAR
				{
					EQUNR = (string)row["EQUNR"],
					EQTYP = (string)row["EQTYP"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					LIZNR = (string)row["LIZNR"],
					TIDNR = (string)row["TIDNR"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
					ZZTMPDT = (string.IsNullOrEmpty(row["ZZTMPDT"].ToString())) ? null : (DateTime?)row["ZZTMPDT"],
					EXPIRY_DATE = (string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString())) ? null : (DateTime?)row["EXPIRY_DATE"],
					REPLA_DATE = (string.IsNullOrEmpty(row["REPLA_DATE"].ToString())) ? null : (DateTime?)row["REPLA_DATE"],
					ZZZLDAT = (string.IsNullOrEmpty(row["ZZZLDAT"].ToString())) ? null : (DateTime?)row["ZZZLDAT"],
					ZZSTATUS_ZUG = (string)row["ZZSTATUS_ZUG"],
					ZZSTATUS_IABG = (string)row["ZZSTATUS_IABG"],
					ZZSTATUS_ABG = (string)row["ZZSTATUS_ABG"],
					ABCKZ = (string)row["ABCKZ"],
					MSGRP = (string)row["MSGRP"],
					PARVW_2 = (string)row["PARVW_2"],
					KUNNR_2 = (string)row["KUNNR_2"],
					TREUH_YA = (string)row["TREUH_YA"],
					TREUH_YZ = (string)row["TREUH_YZ"],
					TREUH_YE = (string)row["TREUH_YE"],
					TREUH_YT = (string)row["TREUH_YT"],
					KUNNR_YA = (string)row["KUNNR_YA"],
					KUNNR_YZ = (string)row["KUNNR_YZ"],
					KUNNR_YE = (string)row["KUNNR_YE"],
					KUNNR_YT = (string)row["KUNNR_YT"],
					NAME1_ZS = (string)row["NAME1_ZS"],
					NAME2_ZS = (string)row["NAME2_ZS"],
					STREET_ZS = (string)row["STREET_ZS"],
					HOUSE_NUM1_ZS = (string)row["HOUSE_NUM1_ZS"],
					POST_CODE1_ZS = (string)row["POST_CODE1_ZS"],
					CITY1_ZS = (string)row["CITY1_ZS"],
					COUNTRY_ZS = (string)row["COUNTRY_ZS"],
					TEXT = (string)row["TEXT"],
					FEHLERTEXT = (string)row["FEHLERTEXT"],
					ZZCOCKZ = (string)row["ZZCOCKZ"],
					KUNNR_ZP = (string)row["KUNNR_ZP"],
					ZZABMDAT = (string.IsNullOrEmpty(row["ZZABMDAT"].ToString())) ? null : (DateTime?)row["ZZABMDAT"],
					RESTLAUFZEIT = (string)row["RESTLAUFZEIT"],
					ABMAUF = (string.IsNullOrEmpty(row["ABMAUF"].ToString())) ? null : (DateTime?)row["ABMAUF"],
					NAME1_ZP = (string)row["NAME1_ZP"],

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

			public static IEnumerable<GT_ABRUFBAR> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ABRUFBAR> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_ABRUFBAR> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ABRUFBAR).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ABRUFBAR> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_ABRUFBAR> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ABRUFBAR> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ABRUFBAR>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_UNANGEF_ALLG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_ABRUFBAR> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ABRUFBAR>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_ABRUFBAR> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ABRUFBAR>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_ABRUFBAR> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ABRUFBAR>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_UNANGEF_ALLG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_ABRUFBAR> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ABRUFBAR>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_FEHLER : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string EQTYP { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LIZNR { get; set; }

			public string TIDNR { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZSTATUS_ZUG { get; set; }

			public string ZZSTATUS_IABG { get; set; }

			public string ZZSTATUS_ABG { get; set; }

			public string ABCKZ { get; set; }

			public string MSGRP { get; set; }

			public string PARVW_2 { get; set; }

			public string KUNNR_2 { get; set; }

			public string TREUH_YA { get; set; }

			public string TREUH_YZ { get; set; }

			public string TREUH_YE { get; set; }

			public string TREUH_YT { get; set; }

			public string KUNNR_YA { get; set; }

			public string KUNNR_YZ { get; set; }

			public string KUNNR_YE { get; set; }

			public string KUNNR_YT { get; set; }

			public string NAME1_ZS { get; set; }

			public string NAME2_ZS { get; set; }

			public string STREET_ZS { get; set; }

			public string HOUSE_NUM1_ZS { get; set; }

			public string POST_CODE1_ZS { get; set; }

			public string CITY1_ZS { get; set; }

			public string COUNTRY_ZS { get; set; }

			public string TEXT { get; set; }

			public string FEHLERTEXT { get; set; }

			public string ZZCOCKZ { get; set; }

			public string KUNNR_ZP { get; set; }

			public DateTime? ZZABMDAT { get; set; }

			public string RESTLAUFZEIT { get; set; }

			public DateTime? ABMAUF { get; set; }

			public string NAME1_ZP { get; set; }

			public static GT_FEHLER Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_FEHLER
				{
					EQUNR = (string)row["EQUNR"],
					EQTYP = (string)row["EQTYP"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					LIZNR = (string)row["LIZNR"],
					TIDNR = (string)row["TIDNR"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
					ZZTMPDT = (string.IsNullOrEmpty(row["ZZTMPDT"].ToString())) ? null : (DateTime?)row["ZZTMPDT"],
					EXPIRY_DATE = (string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString())) ? null : (DateTime?)row["EXPIRY_DATE"],
					REPLA_DATE = (string.IsNullOrEmpty(row["REPLA_DATE"].ToString())) ? null : (DateTime?)row["REPLA_DATE"],
					ZZZLDAT = (string.IsNullOrEmpty(row["ZZZLDAT"].ToString())) ? null : (DateTime?)row["ZZZLDAT"],
					ZZSTATUS_ZUG = (string)row["ZZSTATUS_ZUG"],
					ZZSTATUS_IABG = (string)row["ZZSTATUS_IABG"],
					ZZSTATUS_ABG = (string)row["ZZSTATUS_ABG"],
					ABCKZ = (string)row["ABCKZ"],
					MSGRP = (string)row["MSGRP"],
					PARVW_2 = (string)row["PARVW_2"],
					KUNNR_2 = (string)row["KUNNR_2"],
					TREUH_YA = (string)row["TREUH_YA"],
					TREUH_YZ = (string)row["TREUH_YZ"],
					TREUH_YE = (string)row["TREUH_YE"],
					TREUH_YT = (string)row["TREUH_YT"],
					KUNNR_YA = (string)row["KUNNR_YA"],
					KUNNR_YZ = (string)row["KUNNR_YZ"],
					KUNNR_YE = (string)row["KUNNR_YE"],
					KUNNR_YT = (string)row["KUNNR_YT"],
					NAME1_ZS = (string)row["NAME1_ZS"],
					NAME2_ZS = (string)row["NAME2_ZS"],
					STREET_ZS = (string)row["STREET_ZS"],
					HOUSE_NUM1_ZS = (string)row["HOUSE_NUM1_ZS"],
					POST_CODE1_ZS = (string)row["POST_CODE1_ZS"],
					CITY1_ZS = (string)row["CITY1_ZS"],
					COUNTRY_ZS = (string)row["COUNTRY_ZS"],
					TEXT = (string)row["TEXT"],
					FEHLERTEXT = (string)row["FEHLERTEXT"],
					ZZCOCKZ = (string)row["ZZCOCKZ"],
					KUNNR_ZP = (string)row["KUNNR_ZP"],
					ZZABMDAT = (string.IsNullOrEmpty(row["ZZABMDAT"].ToString())) ? null : (DateTime?)row["ZZABMDAT"],
					RESTLAUFZEIT = (string)row["RESTLAUFZEIT"],
					ABMAUF = (string.IsNullOrEmpty(row["ABMAUF"].ToString())) ? null : (DateTime?)row["ABMAUF"],
					NAME1_ZP = (string)row["NAME1_ZP"],

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

			public static IEnumerable<GT_FEHLER> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_FEHLER> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_FEHLER> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_FEHLER).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_FEHLER> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_FEHLER> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_FEHLER> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FEHLER>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_UNANGEF_ALLG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_FEHLER> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FEHLER>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_FEHLER> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FEHLER>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_FEHLER> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FEHLER>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_UNANGEF_ALLG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_FEHLER> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FEHLER>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_UNANGEF_ALLG_01.GT_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_UNANGEF_ALLG_01.GT_IN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_UNANGEF_ALLG_01.GT_FEHLER> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_UNANGEF_ALLG_01.GT_FEHLER> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
