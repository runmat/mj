using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_BRIEFBESTAND_002
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_BRIEFBESTAND_002).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_BRIEFBESTAND_002).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_DATEN : IModelMappingApplied
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

			public string ABCKZ { get; set; }

			public string MSGRP { get; set; }

			public string STORT { get; set; }

			public string ZZVGRUND { get; set; }

			public DateTime? DATAB { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public DateTime? PICKDAT { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public string NAME1_ZL { get; set; }

			public string NAME2_ZL { get; set; }

			public string STREET_ZL { get; set; }

			public string HOUSE_NUM1_ZL { get; set; }

			public string POST_CODE1_ZL { get; set; }

			public string CITY1_ZL { get; set; }

			public DateTime? DAT_VERTR_BEG { get; set; }

			public DateTime? DAT_VERTR_END { get; set; }

			public string VERTRAGS_STAT { get; set; }

			public string VERSGRU_TXT { get; set; }

			public static GT_DATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DATEN
				{
					EQUNR = (string)row["EQUNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					LIZNR = (string)row["LIZNR"],
					TIDNR = (string)row["TIDNR"],
					ABCKZ = (string)row["ABCKZ"],
					MSGRP = (string)row["MSGRP"],
					STORT = (string)row["STORT"],
					ZZVGRUND = (string)row["ZZVGRUND"],
					DATAB = (string.IsNullOrEmpty(row["DATAB"].ToString())) ? null : (DateTime?)row["DATAB"],
					ZZTMPDT = (string.IsNullOrEmpty(row["ZZTMPDT"].ToString())) ? null : (DateTime?)row["ZZTMPDT"],
					EXPIRY_DATE = (string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString())) ? null : (DateTime?)row["EXPIRY_DATE"],
					PICKDAT = (string.IsNullOrEmpty(row["PICKDAT"].ToString())) ? null : (DateTime?)row["PICKDAT"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
					NAME1_ZL = (string)row["NAME1_ZL"],
					NAME2_ZL = (string)row["NAME2_ZL"],
					STREET_ZL = (string)row["STREET_ZL"],
					HOUSE_NUM1_ZL = (string)row["HOUSE_NUM1_ZL"],
					POST_CODE1_ZL = (string)row["POST_CODE1_ZL"],
					CITY1_ZL = (string)row["CITY1_ZL"],
					DAT_VERTR_BEG = (string.IsNullOrEmpty(row["DAT_VERTR_BEG"].ToString())) ? null : (DateTime?)row["DAT_VERTR_BEG"],
					DAT_VERTR_END = (string.IsNullOrEmpty(row["DAT_VERTR_END"].ToString())) ? null : (DateTime?)row["DAT_VERTR_END"],
					VERTRAGS_STAT = (string)row["VERTRAGS_STAT"],
					VERSGRU_TXT = (string)row["VERSGRU_TXT"],

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

			public static IEnumerable<GT_DATEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DATEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_DATEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DATEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DATEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_DATEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DATEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_BRIEFBESTAND_002", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DATEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DATEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_BRIEFBESTAND_002", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_BRIEFBESTAND_002.GT_DATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_BRIEFBESTAND_002.GT_DATEN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
