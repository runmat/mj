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
	public partial class Z_DPM_FAHRZEUGHISTORIE_AVM
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_FAHRZEUGHISTORIE_AVM).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_FAHRZEUGHISTORIE_AVM).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public void SetImportParameter_I_EQUNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQUNR", value);
		}

		public void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public void SetImportParameter_I_LICENSE_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LICENSE_NUM", value);
		}

		public void SetImportParameter_I_LIZNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIZNR", value);
		}

		public void SetImportParameter_I_QMNUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_QMNUM", value);
		}

		public void SetImportParameter_I_REFERENZ1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_REFERENZ1", value);
		}

		public void SetImportParameter_I_TIDNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TIDNR", value);
		}

		public int? GetExportParameter_E_COUNTER(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_COUNTER");
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_AUSST : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public DateTime? ZZABMDAT { get; set; }

			public DateTime? ERDAT_ABMK { get; set; }

			public DateTime? DATUM_ABMZ { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public string UNFALLFZG { get; set; }

			public DateTime? ERDAT_ZCARPK { get; set; }

			public string CARPORT { get; set; }

			public string PDINAME { get; set; }

			public string PSCHILD { get; set; }

			public string PSCHEIN { get; set; }

			public static GT_AUSST Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_AUSST
				{
					ZZABMDAT = string.IsNullOrEmpty(row["ZZABMDAT"].ToString()) ? null : (DateTime?)row["ZZABMDAT"],
					ERDAT_ABMK = string.IsNullOrEmpty(row["ERDAT_ABMK"].ToString()) ? null : (DateTime?)row["ERDAT_ABMK"],
					DATUM_ABMZ = string.IsNullOrEmpty(row["DATUM_ABMZ"].ToString()) ? null : (DateTime?)row["DATUM_ABMZ"],
					EXPIRY_DATE = string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString()) ? null : (DateTime?)row["EXPIRY_DATE"],
					UNFALLFZG = (string)row["UNFALLFZG"],
					ERDAT_ZCARPK = string.IsNullOrEmpty(row["ERDAT_ZCARPK"].ToString()) ? null : (DateTime?)row["ERDAT_ZCARPK"],
					CARPORT = (string)row["CARPORT"],
					PDINAME = (string)row["PDINAME"],
					PSCHILD = (string)row["PSCHILD"],
					PSCHEIN = (string)row["PSCHEIN"],

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

			public static IEnumerable<GT_AUSST> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_AUSST> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_AUSST> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_AUSST).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_AUSST> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_AUSST> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_AUSST> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_AUSST>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_EINST : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public DateTime? EINGANG_ZB2 { get; set; }

			public string KUNNR_ZP { get; set; }

			public string NAME1_ZP { get; set; }

			public string NAME2_ZP { get; set; }

			public string STREET_ZP { get; set; }

			public string HOUSE_NUM1_ZP { get; set; }

			public string POST_CODE1_ZP { get; set; }

			public string CITY1_ZP { get; set; }

			public string KUNPDI { get; set; }

			public string PDINAME { get; set; }

			public string PDIORT { get; set; }

			public DateTime? ZZDAT_EIN { get; set; }

			public DateTime? ZZDAT_BER { get; set; }

			public string ZZMODELL { get; set; }

			public string ZZBEZEI { get; set; }

			public DateTime? QMDAT { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public string KUNNR_ZH { get; set; }

			public string NAME1_ZH { get; set; }

			public string NAME2_ZH { get; set; }

			public string STREET_ZH { get; set; }

			public string HOUSE_NUM1_ZH { get; set; }

			public string POST_CODE1_ZH { get; set; }

			public string CITY1_ZH { get; set; }

			public string LICENSE_NUM { get; set; }

			public string ZZAKTSPERRE { get; set; }

			public string ZBEMERKUNG { get; set; }

			public DateTime? EINGANG_EQT { get; set; }

			public static GT_EINST Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EINST
				{
					EINGANG_ZB2 = string.IsNullOrEmpty(row["EINGANG_ZB2"].ToString()) ? null : (DateTime?)row["EINGANG_ZB2"],
					KUNNR_ZP = (string)row["KUNNR_ZP"],
					NAME1_ZP = (string)row["NAME1_ZP"],
					NAME2_ZP = (string)row["NAME2_ZP"],
					STREET_ZP = (string)row["STREET_ZP"],
					HOUSE_NUM1_ZP = (string)row["HOUSE_NUM1_ZP"],
					POST_CODE1_ZP = (string)row["POST_CODE1_ZP"],
					CITY1_ZP = (string)row["CITY1_ZP"],
					KUNPDI = (string)row["KUNPDI"],
					PDINAME = (string)row["PDINAME"],
					PDIORT = (string)row["PDIORT"],
					ZZDAT_EIN = string.IsNullOrEmpty(row["ZZDAT_EIN"].ToString()) ? null : (DateTime?)row["ZZDAT_EIN"],
					ZZDAT_BER = string.IsNullOrEmpty(row["ZZDAT_BER"].ToString()) ? null : (DateTime?)row["ZZDAT_BER"],
					ZZMODELL = (string)row["ZZMODELL"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					QMDAT = string.IsNullOrEmpty(row["QMDAT"].ToString()) ? null : (DateTime?)row["QMDAT"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					KUNNR_ZH = (string)row["KUNNR_ZH"],
					NAME1_ZH = (string)row["NAME1_ZH"],
					NAME2_ZH = (string)row["NAME2_ZH"],
					STREET_ZH = (string)row["STREET_ZH"],
					HOUSE_NUM1_ZH = (string)row["HOUSE_NUM1_ZH"],
					POST_CODE1_ZH = (string)row["POST_CODE1_ZH"],
					CITY1_ZH = (string)row["CITY1_ZH"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ZZAKTSPERRE = (string)row["ZZAKTSPERRE"],
					ZBEMERKUNG = (string)row["ZBEMERKUNG"],
					EINGANG_EQT = string.IsNullOrEmpty(row["EINGANG_EQT"].ToString()) ? null : (DateTime?)row["EINGANG_EQT"],

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

			public static IEnumerable<GT_EINST> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EINST> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EINST> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EINST).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EINST> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EINST> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EINST> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EINST>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EINST> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EINST>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EINST> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EINST>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EINST> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EINST>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EINST> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EINST>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_EQUIS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string TIDNR { get; set; }

			public string LIZNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string EQUNR { get; set; }

			public string QMNUM { get; set; }

			public string HERKUNFT { get; set; }

			public static GT_EQUIS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EQUIS
				{
					KUNNR = (string)row["KUNNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					TIDNR = (string)row["TIDNR"],
					LIZNR = (string)row["LIZNR"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					EQUNR = (string)row["EQUNR"],
					QMNUM = (string)row["QMNUM"],
					HERKUNFT = (string)row["HERKUNFT"],

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

			public static IEnumerable<GT_EQUIS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EQUIS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EQUIS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EQUIS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EQUIS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EQUIS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_LLSCH : IModelMappingApplied
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

			public string ZZDIEN1 { get; set; }

			public string QMGRP { get; set; }

			public string LICENSE_NUM { get; set; }

			public string ZZULDAT { get; set; }

			public string QMCOD { get; set; }

			public string NAME1_ZB1 { get; set; }

			public string NAME2_ZB1 { get; set; }

			public string STREET_ZB1 { get; set; }

			public string HOUSE_NUM1_ZB1 { get; set; }

			public string POST_CODE1_ZB1 { get; set; }

			public string CITY1_ZB1 { get; set; }

			public string COUNTRY_ZB1 { get; set; }

			public string BAUTL { get; set; }

			public string MAKTX { get; set; }

			public static GT_LLSCH Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_LLSCH
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
					ZZDIEN1 = (string)row["ZZDIEN1"],
					QMGRP = (string)row["QMGRP"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ZZULDAT = (string)row["ZZULDAT"],
					QMCOD = (string)row["QMCOD"],
					NAME1_ZB1 = (string)row["NAME1_ZB1"],
					NAME2_ZB1 = (string)row["NAME2_ZB1"],
					STREET_ZB1 = (string)row["STREET_ZB1"],
					HOUSE_NUM1_ZB1 = (string)row["HOUSE_NUM1_ZB1"],
					POST_CODE1_ZB1 = (string)row["POST_CODE1_ZB1"],
					CITY1_ZB1 = (string)row["CITY1_ZB1"],
					COUNTRY_ZB1 = (string)row["COUNTRY_ZB1"],
					BAUTL = (string)row["BAUTL"],
					MAKTX = (string)row["MAKTX"],

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

			public static IEnumerable<GT_LLSCH> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_LLSCH> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_LLSCH> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_LLSCH).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_LLSCH> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLSCH> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_LLSCH> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LLSCH>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLSCH> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LLSCH>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLSCH> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LLSCH>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLSCH> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LLSCH>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLSCH> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LLSCH>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_LLZB2 : IModelMappingApplied
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

			public string ZZDIEN1 { get; set; }

			public string QMGRP { get; set; }

			public string LICENSE_NUM { get; set; }

			public string ZZULDAT { get; set; }

			public string QMCOD { get; set; }

			public string NAME1_ZB1 { get; set; }

			public string NAME2_ZB1 { get; set; }

			public string STREET_ZB1 { get; set; }

			public string HOUSE_NUM1_ZB1 { get; set; }

			public string POST_CODE1_ZB1 { get; set; }

			public string CITY1_ZB1 { get; set; }

			public string COUNTRY_ZB1 { get; set; }

			public string BAUTL { get; set; }

			public string MAKTX { get; set; }

			public static GT_LLZB2 Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_LLZB2
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
					ZZDIEN1 = (string)row["ZZDIEN1"],
					QMGRP = (string)row["QMGRP"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ZZULDAT = (string)row["ZZULDAT"],
					QMCOD = (string)row["QMCOD"],
					NAME1_ZB1 = (string)row["NAME1_ZB1"],
					NAME2_ZB1 = (string)row["NAME2_ZB1"],
					STREET_ZB1 = (string)row["STREET_ZB1"],
					HOUSE_NUM1_ZB1 = (string)row["HOUSE_NUM1_ZB1"],
					POST_CODE1_ZB1 = (string)row["POST_CODE1_ZB1"],
					CITY1_ZB1 = (string)row["CITY1_ZB1"],
					COUNTRY_ZB1 = (string)row["COUNTRY_ZB1"],
					BAUTL = (string)row["BAUTL"],
					MAKTX = (string)row["MAKTX"],

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

			public static IEnumerable<GT_LLZB2> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_LLZB2> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_LLZB2> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_LLZB2).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_LLZB2> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLZB2> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_LLZB2> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LLZB2>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLZB2> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LLZB2>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLZB2> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LLZB2>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLZB2> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_LLZB2>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_LLZB2> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_LLZB2>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_TKOMP : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string STLKN { get; set; }

			public string ZAKT_NR { get; set; }

			public string IDNRK { get; set; }

			public string MAKTX { get; set; }

			public string AKTI { get; set; }

			public string AKTI_BEZEI { get; set; }

			public DateTime? DATUM { get; set; }

			public string UZEIT { get; set; }

			public string UNAME { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string COUNTRY { get; set; }

			public static GT_TKOMP Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TKOMP
				{
					STLKN = (string)row["STLKN"],
					ZAKT_NR = (string)row["ZAKT_NR"],
					IDNRK = (string)row["IDNRK"],
					MAKTX = (string)row["MAKTX"],
					AKTI = (string)row["AKTI"],
					AKTI_BEZEI = (string)row["AKTI_BEZEI"],
					DATUM = string.IsNullOrEmpty(row["DATUM"].ToString()) ? null : (DateTime?)row["DATUM"],
					UZEIT = (string)row["UZEIT"],
					UNAME = (string)row["UNAME"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					COUNTRY = (string)row["COUNTRY"],

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

			public static IEnumerable<GT_TKOMP> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TKOMP> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TKOMP> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TKOMP).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TKOMP> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TKOMP> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TKOMP> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TKOMP>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TKOMP> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TKOMP>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TKOMP> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TKOMP>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TKOMP> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TKOMP>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TKOMP> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TKOMP>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_TUETE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ATBEZ { get; set; }

			public string ANZAHL { get; set; }

			public static GT_TUETE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TUETE
				{
					ATBEZ = (string)row["ATBEZ"],
					ANZAHL = (string)row["ANZAHL"],

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

			public static IEnumerable<GT_TUETE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TUETE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TUETE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TUETE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TUETE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TUETE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TUETE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TUETE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TUETE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TUETE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TUETE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TUETE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TUETE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TUETE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TUETE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TUETE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_TYPEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZUNGUELTIG { get; set; }

			public string ZZRESERVE { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string ZZFAHRZEUGKLASSE { get; set; }

			public string ZZCODE_AUFBAU { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZKLARTEXT_TYP { get; set; }

			public string ZZVARIANTE { get; set; }

			public string ZZVERSION { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZZFHRZKLASSE_TXT { get; set; }

			public string ZZTEXT_AUFBAU { get; set; }

			public string ZZABGASRICHTL_TG { get; set; }

			public string ZZNATIONALE_EMIK { get; set; }

			public string ZZKRAFTSTOFF_TXT { get; set; }

			public string ZZCODE_KRAFTSTOF { get; set; }

			public string ZZSLD { get; set; }

			public string ZZHUBRAUM { get; set; }

			public string ZZANZACHS { get; set; }

			public string ZZANTRIEBSACHS { get; set; }

			public string ZZNENNLEISTUNG { get; set; }

			public string ZZBEIUMDREH { get; set; }

			public string ZZHOECHSTGESCHW { get; set; }

			public string ZZFASSVERMOEGEN { get; set; }

			public string ZZANZSITZE { get; set; }

			public string ZZANZSTEHPLAETZE { get; set; }

			public string ZZMASSEFAHRBMIN { get; set; }

			public string ZZMASSEFAHRBMAX { get; set; }

			public string ZZZULGESGEW { get; set; }

			public string ZZZULGESGEWSTAAT { get; set; }

			public string ZZACHSLST_ACHSE1 { get; set; }

			public string ZZACHSLST_ACHSE2 { get; set; }

			public string ZZACHSLST_ACHSE3 { get; set; }

			public string ZZACHSL_A1_STA { get; set; }

			public string ZZACHSL_A2_STA { get; set; }

			public string ZZACHSL_A3_STA { get; set; }

			public string ZZCO2KOMBI { get; set; }

			public string ZZSTANDGERAEUSCH { get; set; }

			public string ZZDREHZSTANDGER { get; set; }

			public string ZZFAHRGERAEUSCH { get; set; }

			public string ZZANHLAST_GEBR { get; set; }

			public string ZZANHLAST_UNGEBR { get; set; }

			public string ZZLEISTUNGSGEW { get; set; }

			public string ZZLAENGEMIN { get; set; }

			public string ZZLAENGEMAX { get; set; }

			public string ZZBREITEMIN { get; set; }

			public string ZZBREITEMAX { get; set; }

			public string ZZHOEHEMIN { get; set; }

			public string ZZHOEHEMAX { get; set; }

			public string ZZSTUETZLAST { get; set; }

			public string ZZBEREIFACHSE1 { get; set; }

			public string ZZBEREIFACHSE2 { get; set; }

			public string ZZBEREIFACHSE3 { get; set; }

			public string ZZGENEHMIGNR { get; set; }

			public string ZZGENEHMIGDAT { get; set; }

			public string ZZBEMER1 { get; set; }

			public string ZZBEMER2 { get; set; }

			public string ZZBEMER3 { get; set; }

			public string ZZBEMER4 { get; set; }

			public string ZZBEMER5 { get; set; }

			public string ZZBEMER6 { get; set; }

			public string ZZBEMER7 { get; set; }

			public string ZZBEMER8 { get; set; }

			public string ZZBEMER9 { get; set; }

			public string ZZBEMER10 { get; set; }

			public string ZZBEMER11 { get; set; }

			public string ZZBEMER12 { get; set; }

			public string ZZBEMER13 { get; set; }

			public string ZZBEMER14 { get; set; }

			public static GT_TYPEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TYPEN
				{
					ZZUNGUELTIG = (string)row["ZZUNGUELTIG"],
					ZZRESERVE = (string)row["ZZRESERVE"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
					ZZFAHRZEUGKLASSE = (string)row["ZZFAHRZEUGKLASSE"],
					ZZCODE_AUFBAU = (string)row["ZZCODE_AUFBAU"],
					ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
					ZZKLARTEXT_TYP = (string)row["ZZKLARTEXT_TYP"],
					ZZVARIANTE = (string)row["ZZVARIANTE"],
					ZZVERSION = (string)row["ZZVERSION"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
					ZZFHRZKLASSE_TXT = (string)row["ZZFHRZKLASSE_TXT"],
					ZZTEXT_AUFBAU = (string)row["ZZTEXT_AUFBAU"],
					ZZABGASRICHTL_TG = (string)row["ZZABGASRICHTL_TG"],
					ZZNATIONALE_EMIK = (string)row["ZZNATIONALE_EMIK"],
					ZZKRAFTSTOFF_TXT = (string)row["ZZKRAFTSTOFF_TXT"],
					ZZCODE_KRAFTSTOF = (string)row["ZZCODE_KRAFTSTOF"],
					ZZSLD = (string)row["ZZSLD"],
					ZZHUBRAUM = (string)row["ZZHUBRAUM"],
					ZZANZACHS = (string)row["ZZANZACHS"],
					ZZANTRIEBSACHS = (string)row["ZZANTRIEBSACHS"],
					ZZNENNLEISTUNG = (string)row["ZZNENNLEISTUNG"],
					ZZBEIUMDREH = (string)row["ZZBEIUMDREH"],
					ZZHOECHSTGESCHW = (string)row["ZZHOECHSTGESCHW"],
					ZZFASSVERMOEGEN = (string)row["ZZFASSVERMOEGEN"],
					ZZANZSITZE = (string)row["ZZANZSITZE"],
					ZZANZSTEHPLAETZE = (string)row["ZZANZSTEHPLAETZE"],
					ZZMASSEFAHRBMIN = (string)row["ZZMASSEFAHRBMIN"],
					ZZMASSEFAHRBMAX = (string)row["ZZMASSEFAHRBMAX"],
					ZZZULGESGEW = (string)row["ZZZULGESGEW"],
					ZZZULGESGEWSTAAT = (string)row["ZZZULGESGEWSTAAT"],
					ZZACHSLST_ACHSE1 = (string)row["ZZACHSLST_ACHSE1"],
					ZZACHSLST_ACHSE2 = (string)row["ZZACHSLST_ACHSE2"],
					ZZACHSLST_ACHSE3 = (string)row["ZZACHSLST_ACHSE3"],
					ZZACHSL_A1_STA = (string)row["ZZACHSL_A1_STA"],
					ZZACHSL_A2_STA = (string)row["ZZACHSL_A2_STA"],
					ZZACHSL_A3_STA = (string)row["ZZACHSL_A3_STA"],
					ZZCO2KOMBI = (string)row["ZZCO2KOMBI"],
					ZZSTANDGERAEUSCH = (string)row["ZZSTANDGERAEUSCH"],
					ZZDREHZSTANDGER = (string)row["ZZDREHZSTANDGER"],
					ZZFAHRGERAEUSCH = (string)row["ZZFAHRGERAEUSCH"],
					ZZANHLAST_GEBR = (string)row["ZZANHLAST_GEBR"],
					ZZANHLAST_UNGEBR = (string)row["ZZANHLAST_UNGEBR"],
					ZZLEISTUNGSGEW = (string)row["ZZLEISTUNGSGEW"],
					ZZLAENGEMIN = (string)row["ZZLAENGEMIN"],
					ZZLAENGEMAX = (string)row["ZZLAENGEMAX"],
					ZZBREITEMIN = (string)row["ZZBREITEMIN"],
					ZZBREITEMAX = (string)row["ZZBREITEMAX"],
					ZZHOEHEMIN = (string)row["ZZHOEHEMIN"],
					ZZHOEHEMAX = (string)row["ZZHOEHEMAX"],
					ZZSTUETZLAST = (string)row["ZZSTUETZLAST"],
					ZZBEREIFACHSE1 = (string)row["ZZBEREIFACHSE1"],
					ZZBEREIFACHSE2 = (string)row["ZZBEREIFACHSE2"],
					ZZBEREIFACHSE3 = (string)row["ZZBEREIFACHSE3"],
					ZZGENEHMIGNR = (string)row["ZZGENEHMIGNR"],
					ZZGENEHMIGDAT = (string)row["ZZGENEHMIGDAT"],
					ZZBEMER1 = (string)row["ZZBEMER1"],
					ZZBEMER2 = (string)row["ZZBEMER2"],
					ZZBEMER3 = (string)row["ZZBEMER3"],
					ZZBEMER4 = (string)row["ZZBEMER4"],
					ZZBEMER5 = (string)row["ZZBEMER5"],
					ZZBEMER6 = (string)row["ZZBEMER6"],
					ZZBEMER7 = (string)row["ZZBEMER7"],
					ZZBEMER8 = (string)row["ZZBEMER8"],
					ZZBEMER9 = (string)row["ZZBEMER9"],
					ZZBEMER10 = (string)row["ZZBEMER10"],
					ZZBEMER11 = (string)row["ZZBEMER11"],
					ZZBEMER12 = (string)row["ZZBEMER12"],
					ZZBEMER13 = (string)row["ZZBEMER13"],
					ZZBEMER14 = (string)row["ZZBEMER14"],

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

			public static IEnumerable<GT_TYPEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TYPEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TYPEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TYPEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TYPEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TYPEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TYPEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TYPEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TYPEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TYPEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TYPEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TYPEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TYPEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TYPEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TYPEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TYPEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_UEBER : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZZMODELL { get; set; }

			public string ZFARBE { get; set; }

			public string ZFARBTXT { get; set; }

			public string TIDNR { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public string LIZNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public static GT_UEBER Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_UEBER
				{
					ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
					ZZMODELL = (string)row["ZZMODELL"],
					ZFARBE = (string)row["ZFARBE"],
					ZFARBTXT = (string)row["ZFARBTXT"],
					TIDNR = (string)row["TIDNR"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					EXPIRY_DATE = string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString()) ? null : (DateTime?)row["EXPIRY_DATE"],
					LIZNR = (string)row["LIZNR"],
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

			public static IEnumerable<GT_UEBER> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_UEBER> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_UEBER> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_UEBER).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_UEBER> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_UEBER> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_UEBER> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_UEBER>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_UEBER> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_UEBER>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_UEBER> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_UEBER>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_UEBER> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_UEBER>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_FAHRZEUGHISTORIE_AVM", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_UEBER> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_UEBER>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_AUSST> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EINST> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EQUIS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLSCH> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLZB2> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TKOMP> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TUETE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TYPEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_FAHRZEUGHISTORIE_AVM.GT_UEBER> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
