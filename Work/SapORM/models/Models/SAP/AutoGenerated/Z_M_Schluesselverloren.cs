using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_Schluesselverloren
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_Schluesselverloren).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_Schluesselverloren).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_WEB_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string KUNNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string EQUNR { get; set; }

			public string EQTYP { get; set; }

			public string EQFNR { get; set; }

			public string LICENSE_NUM { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public string TIDNR { get; set; }

			public string LIZNR { get; set; }

			public string ABCKZ { get; set; }

			public string KUNPDI { get; set; }

			public string FLAG { get; set; }

			public string ERSSCHLUESSEL { get; set; }

			public string CARPASS { get; set; }

			public string RADIOCODEKARTE { get; set; }

			public string NAVICD { get; set; }

			public string CHIPKARTE { get; set; }

			public string COCBESCH { get; set; }

			public string NAVICODEKARTE { get; set; }

			public string WFSCODEKARTE { get; set; }

			public string SH_ERS_FB { get; set; }

			public string PRUEFBUCH_LKW { get; set; }

			public string EMPTY { get; set; }

			public string OBJNR { get; set; }

			public string AUSNAHME { get; set; }

			public DateTime? MELDDAT { get; set; }

			public DateTime? MAHN_1 { get; set; }

			public DateTime? MAHN_2 { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public static GT_WEB_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB_IN
				{
					MANDT = (string)row["MANDT"],
					KUNNR = (string)row["KUNNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					EQUNR = (string)row["EQUNR"],
					EQTYP = (string)row["EQTYP"],
					EQFNR = (string)row["EQFNR"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					REPLA_DATE = (string.IsNullOrEmpty(row["REPLA_DATE"].ToString())) ? null : (DateTime?)row["REPLA_DATE"],
					TIDNR = (string)row["TIDNR"],
					LIZNR = (string)row["LIZNR"],
					ABCKZ = (string)row["ABCKZ"],
					KUNPDI = (string)row["KUNPDI"],
					FLAG = (string)row["FLAG"],
					ERSSCHLUESSEL = (string)row["ERSSCHLUESSEL"],
					CARPASS = (string)row["CARPASS"],
					RADIOCODEKARTE = (string)row["RADIOCODEKARTE"],
					NAVICD = (string)row["NAVICD"],
					CHIPKARTE = (string)row["CHIPKARTE"],
					COCBESCH = (string)row["COCBESCH"],
					NAVICODEKARTE = (string)row["NAVICODEKARTE"],
					WFSCODEKARTE = (string)row["WFSCODEKARTE"],
					SH_ERS_FB = (string)row["SH_ERS_FB"],
					PRUEFBUCH_LKW = (string)row["PRUEFBUCH_LKW"],
					EMPTY = (string)row["EMPTY"],
					OBJNR = (string)row["OBJNR"],
					AUSNAHME = (string)row["AUSNAHME"],
					MELDDAT = (string.IsNullOrEmpty(row["MELDDAT"].ToString())) ? null : (DateTime?)row["MELDDAT"],
					MAHN_1 = (string.IsNullOrEmpty(row["MAHN_1"].ToString())) ? null : (DateTime?)row["MAHN_1"],
					MAHN_2 = (string.IsNullOrEmpty(row["MAHN_2"].ToString())) ? null : (DateTime?)row["MAHN_2"],
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

			public static IEnumerable<GT_WEB_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB_IN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_WEB_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB_IN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_WEB_IN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB_IN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_Schluesselverloren", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB_IN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB_IN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_Schluesselverloren", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_Schluesselverloren.GT_WEB_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_Schluesselverloren.GT_WEB_IN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
