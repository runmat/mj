using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_AH_CRE_CHG_FZG_AKT_BEST
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_AH_CRE_CHG_FZG_AKT_BEST).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_AH_CRE_CHG_FZG_AKT_BEST).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_IN_IMP : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN_ID { get; set; }

			public string FIN { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string KAEUFER { get; set; }

			public string HALTER { get; set; }

			public string BRIEFBESTAND { get; set; }

			public string LGORT { get; set; }

			public string STANDORT { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public DateTime? AKTZULDAT { get; set; }

			public DateTime? ABMDAT { get; set; }

			public string KENNZ { get; set; }

			public string BRIEFNR { get; set; }

			public string COCVORHANDEN { get; set; }

			public string BEMERKUNG { get; set; }

			public static GT_IN_IMP Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IN_IMP
				{
					FIN_ID = (string)row["FIN_ID"],
					FIN = (string)row["FIN"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
					ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					KAEUFER = (string)row["KAEUFER"],
					HALTER = (string)row["HALTER"],
					BRIEFBESTAND = (string)row["BRIEFBESTAND"],
					LGORT = (string)row["LGORT"],
					STANDORT = (string)row["STANDORT"],
					ERSTZULDAT = (string.IsNullOrEmpty(row["ERSTZULDAT"].ToString())) ? null : (DateTime?)row["ERSTZULDAT"],
					AKTZULDAT = (string.IsNullOrEmpty(row["AKTZULDAT"].ToString())) ? null : (DateTime?)row["AKTZULDAT"],
					ABMDAT = (string.IsNullOrEmpty(row["ABMDAT"].ToString())) ? null : (DateTime?)row["ABMDAT"],
					KENNZ = (string)row["KENNZ"],
					BRIEFNR = (string)row["BRIEFNR"],
					COCVORHANDEN = (string)row["COCVORHANDEN"],
					BEMERKUNG = (string)row["BEMERKUNG"],

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

			public static IEnumerable<GT_IN_IMP> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IN_IMP> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_IN_IMP> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IN_IMP).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IN_IMP> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_IN_IMP> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IN_IMP> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN_IMP>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_AH_CRE_CHG_FZG_AKT_BEST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN_IMP> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN_IMP>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN_IMP> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN_IMP>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN_IMP> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN_IMP>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_AH_CRE_CHG_FZG_AKT_BEST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN_IMP> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN_IMP>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_AH_CRE_CHG_FZG_AKT_BEST.GT_IN_IMP> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_AH_CRE_CHG_FZG_AKT_BEST.GT_IN_IMP> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
