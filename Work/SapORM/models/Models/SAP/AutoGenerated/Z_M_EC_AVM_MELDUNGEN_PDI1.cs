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
	public partial class Z_M_EC_AVM_MELDUNGEN_PDI1
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EC_AVM_MELDUNGEN_PDI1).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EC_AVM_MELDUNGEN_PDI1).Name, inputParameterKeys, inputParameterValues);
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

			public string QMNUM { get; set; }

			public string EQUNR { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ZZKENN { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZFAHRG { get; set; }

			public string DADPDI { get; set; }

			public string KUNPDI { get; set; }

			public string DADPDI_NAME1 { get; set; }

			public string ZZCODE { get; set; }

			public string ZZMODELL { get; set; }

			public string ZZBEZEI { get; set; }

			public string HERST_K { get; set; }

			public string HERST_T { get; set; }

			public string ZZSIPP1 { get; set; }

			public string ZZSIPP2 { get; set; }

			public string ZZSIPP3 { get; set; }

			public string ZZSIPP4 { get; set; }

			public string ZZAUSF { get; set; }

			public string ZZANTR { get; set; }

			public string ZZREIFEN { get; set; }

			public string ZZNAVI { get; set; }

			public DateTime? ZZDAT_EIN { get; set; }

			public string ZZKLEBE { get; set; }

			public string ZZAKTSPERRE { get; set; }

			public string ANZAHL_ZUL { get; set; }

			public string ANZAHL_GSP { get; set; }

			public string KUNNR_ZP { get; set; }

			public string NAME1_ZP { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? VDATU { get; set; }

			public string ZZREF1 { get; set; }

			public string ZZREF2 { get; set; }

			public DateTime? ERDAT_LOW { get; set; }

			public DateTime? ERDAT_HIGH { get; set; }

			public string FLEET_VIN { get; set; }

			public string KUNNR_ZK { get; set; }

			public DateTime? ZZDATBEM { get; set; }

			public string ZZFARBE { get; set; }

			public string ZZVERWENDUNG { get; set; }

			public string ZNAVI { get; set; }

			public string ZAHK { get; set; }

			public string ZFZG_GROUP { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					KUNNR = (string)row["KUNNR"],
					QMNUM = (string)row["QMNUM"],
					EQUNR = (string)row["EQUNR"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					ZZKENN = (string)row["ZZKENN"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					DADPDI = (string)row["DADPDI"],
					KUNPDI = (string)row["KUNPDI"],
					DADPDI_NAME1 = (string)row["DADPDI_NAME1"],
					ZZCODE = (string)row["ZZCODE"],
					ZZMODELL = (string)row["ZZMODELL"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					HERST_K = (string)row["HERST_K"],
					HERST_T = (string)row["HERST_T"],
					ZZSIPP1 = (string)row["ZZSIPP1"],
					ZZSIPP2 = (string)row["ZZSIPP2"],
					ZZSIPP3 = (string)row["ZZSIPP3"],
					ZZSIPP4 = (string)row["ZZSIPP4"],
					ZZAUSF = (string)row["ZZAUSF"],
					ZZANTR = (string)row["ZZANTR"],
					ZZREIFEN = (string)row["ZZREIFEN"],
					ZZNAVI = (string)row["ZZNAVI"],
					ZZDAT_EIN = (string.IsNullOrEmpty(row["ZZDAT_EIN"].ToString())) ? null : (DateTime?)row["ZZDAT_EIN"],
					ZZKLEBE = (string)row["ZZKLEBE"],
					ZZAKTSPERRE = (string)row["ZZAKTSPERRE"],
					ANZAHL_ZUL = (string)row["ANZAHL_ZUL"],
					ANZAHL_GSP = (string)row["ANZAHL_GSP"],
					KUNNR_ZP = (string)row["KUNNR_ZP"],
					NAME1_ZP = (string)row["NAME1_ZP"],
					REPLA_DATE = (string.IsNullOrEmpty(row["REPLA_DATE"].ToString())) ? null : (DateTime?)row["REPLA_DATE"],
					VDATU = (string.IsNullOrEmpty(row["VDATU"].ToString())) ? null : (DateTime?)row["VDATU"],
					ZZREF1 = (string)row["ZZREF1"],
					ZZREF2 = (string)row["ZZREF2"],
					ERDAT_LOW = (string.IsNullOrEmpty(row["ERDAT_LOW"].ToString())) ? null : (DateTime?)row["ERDAT_LOW"],
					ERDAT_HIGH = (string.IsNullOrEmpty(row["ERDAT_HIGH"].ToString())) ? null : (DateTime?)row["ERDAT_HIGH"],
					FLEET_VIN = (string)row["FLEET_VIN"],
					KUNNR_ZK = (string)row["KUNNR_ZK"],
					ZZDATBEM = (string.IsNullOrEmpty(row["ZZDATBEM"].ToString())) ? null : (DateTime?)row["ZZDATBEM"],
					ZZFARBE = (string)row["ZZFARBE"],
					ZZVERWENDUNG = (string)row["ZZVERWENDUNG"],
					ZNAVI = (string)row["ZNAVI"],
					ZAHK = (string)row["ZAHK"],
					ZFZG_GROUP = (string)row["ZFZG_GROUP"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_MELDUNGEN_PDI1", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_MELDUNGEN_PDI1", inputParameterKeys, inputParameterValues);
				 
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

		public partial class GT_TXT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string QMNUM { get; set; }

			public string LFDNUM { get; set; }

			public string TDFORMAT { get; set; }

			public string TDLINE { get; set; }

			public static GT_TXT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TXT
				{
					KUNNR = (string)row["KUNNR"],
					QMNUM = (string)row["QMNUM"],
					LFDNUM = (string)row["LFDNUM"],
					TDFORMAT = (string)row["TDFORMAT"],
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

			public static IEnumerable<GT_TXT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TXT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TXT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TXT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TXT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TXT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TXT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TXT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_MELDUNGEN_PDI1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TXT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TXT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TXT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TXT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TXT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TXT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_MELDUNGEN_PDI1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TXT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TXT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_MELDUNGEN_PDI1.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_EC_AVM_MELDUNGEN_PDI1.GT_WEB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_MELDUNGEN_PDI1.GT_TXT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_EC_AVM_MELDUNGEN_PDI1.GT_TXT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
