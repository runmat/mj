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
	public partial class Z_Massenzulassung
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_Massenzulassung).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_Massenzulassung).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class INTERNTAB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string I_KUNNR_AG { get; set; }

			public string I_ZZFAHRG { get; set; }

			public string I_EDATU { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERZET { get; set; }

			public string I_KUNNR_ZV { get; set; }

			public string I_ZZKENNZ { get; set; }

			public string I_KUNNR_ZH { get; set; }

			public string I_KUNNR_ZA { get; set; }

			public string I_ZZSONDER { get; set; }

			public string I_KBANR { get; set; }

			public string I_ZZCARPORT { get; set; }

			public string EQUNR { get; set; }

			public string VBELN { get; set; }

			public Int32? SUBRC { get; set; }

			public string LABO_ID { get; set; }

			public string ZZVORGANGSSTATUS { get; set; }

			public DateTime? ZZSTATUSDATUM { get; set; }

			public string ZZSTATUSUHRZEIT { get; set; }

			public decimal? GEBAUSL { get; set; }

			public string ZERNAM { get; set; }

			public static INTERNTAB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new INTERNTAB
				{
					MANDT = (string)row["MANDT"],
					I_KUNNR_AG = (string)row["I_KUNNR_AG"],
					I_ZZFAHRG = (string)row["I_ZZFAHRG"],
					I_EDATU = (string)row["I_EDATU"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					ERZET = (string)row["ERZET"],
					I_KUNNR_ZV = (string)row["I_KUNNR_ZV"],
					I_ZZKENNZ = (string)row["I_ZZKENNZ"],
					I_KUNNR_ZH = (string)row["I_KUNNR_ZH"],
					I_KUNNR_ZA = (string)row["I_KUNNR_ZA"],
					I_ZZSONDER = (string)row["I_ZZSONDER"],
					I_KBANR = (string)row["I_KBANR"],
					I_ZZCARPORT = (string)row["I_ZZCARPORT"],
					EQUNR = (string)row["EQUNR"],
					VBELN = (string)row["VBELN"],
					SUBRC = (string.IsNullOrEmpty(row["SUBRC"].ToString())) ? null : (Int32?)Convert.ToInt32(row["SUBRC"]),
					LABO_ID = (string)row["LABO_ID"],
					ZZVORGANGSSTATUS = (string)row["ZZVORGANGSSTATUS"],
					ZZSTATUSDATUM = (string.IsNullOrEmpty(row["ZZSTATUSDATUM"].ToString())) ? null : (DateTime?)row["ZZSTATUSDATUM"],
					ZZSTATUSUHRZEIT = (string)row["ZZSTATUSUHRZEIT"],
					GEBAUSL = (decimal?)row["GEBAUSL"],
					ZERNAM = (string)row["ZERNAM"],

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

			public static IEnumerable<INTERNTAB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<INTERNTAB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<INTERNTAB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(INTERNTAB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<INTERNTAB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<INTERNTAB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<INTERNTAB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<INTERNTAB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_Massenzulassung", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<INTERNTAB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<INTERNTAB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<INTERNTAB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<INTERNTAB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<INTERNTAB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<INTERNTAB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_Massenzulassung", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<INTERNTAB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<INTERNTAB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class OUTPUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string TYPE { get; set; }

			public string ID { get; set; }

			public string NUMMER { get; set; }

			public string MESSAGE { get; set; }

			public static OUTPUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new OUTPUT
				{
					TYPE = (string)row["TYPE"],
					ID = (string)row["ID"],
					NUMMER = (string)row["NUMMER"],
					MESSAGE = (string)row["MESSAGE"],

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

			public static IEnumerable<OUTPUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<OUTPUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<OUTPUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(OUTPUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<OUTPUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<OUTPUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<OUTPUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<OUTPUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_Massenzulassung", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<OUTPUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<OUTPUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<OUTPUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<OUTPUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<OUTPUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<OUTPUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_Massenzulassung", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<OUTPUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<OUTPUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_Massenzulassung.INTERNTAB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_Massenzulassung.INTERNTAB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_Massenzulassung.OUTPUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_Massenzulassung.OUTPUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
