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
	public partial class Z_DPM_READ_ZULDOK_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_ZULDOK_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_ZULDOK_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public void SetImportParameter_I_HALTER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_HALTER", value);
		}

		public void SetImportParameter_I_NAME_HALTER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NAME_HALTER", value);
		}

		public void SetImportParameter_I_NUR_UNVOLLST(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NUR_UNVOLLST", value);
		}

		public void SetImportParameter_I_NUR_VOLLST(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NUR_VOLLST", value);
		}

		public void SetImportParameter_I_STANDORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STANDORT", value);
		}

		public void SetImportParameter_I_ZKUNNR_EXT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZKUNNR_EXT", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_DATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string HALTER { get; set; }

			public string STANDORT { get; set; }

			public string VOLLM { get; set; }

			public string REGISTER { get; set; }

			public string PERSO { get; set; }

			public string GEWERBE { get; set; }

			public string EINZUG { get; set; }

			public string VOLLST { get; set; }

			public string EVB_NUM { get; set; }

			public DateTime? EVB_VON { get; set; }

			public DateTime? EVB_BIS { get; set; }

			public string BEMERKUNG { get; set; }

			public string KZ_ZLS { get; set; }

			public string SCHLNR_ZLS { get; set; }

			public string KZ_EVBNR { get; set; }

			public string KZ_GEWERBE { get; set; }

			public string KZ_HANDREG { get; set; }

			public string KZ_EZERM { get; set; }

			public string KZ_FSPLAK { get; set; }

			public string KZ_KZVERST { get; set; }

			public string WKENNZ { get; set; }

			public string SONST_SH { get; set; }

			public string BEVOLLM1 { get; set; }

			public DateTime? GUELTIG_BIS1 { get; set; }

			public string BEVOLLM2 { get; set; }

			public DateTime? GUELTIG_BIS2 { get; set; }

			public string BEVOLLM3 { get; set; }

			public DateTime? GUELTIG_BIS3 { get; set; }

			public string ZULKREIS { get; set; }

			public string PERSO_FEHLT { get; set; }

			public string VOLLMACHT_FEHLT { get; set; }

			public DateTime? DAT_GW_ANMELD { get; set; }

			public string EZERM_KFZBEZOGEN { get; set; }

			public string ZKUNNR_EXT { get; set; }

			public DateTime? DAT_LOE { get; set; }

			public string WEB_USER { get; set; }

			public string BARCODE { get; set; }

			public DateTime? VOLLMACHT_VON { get; set; }

			public DateTime? VOLLMACHT_BIS { get; set; }

			public DateTime? HREGDAT_VON { get; set; }

			public DateTime? HREGDAT_BIS { get; set; }

			public DateTime? GA_VON { get; set; }

			public DateTime? GA_BIS { get; set; }

			public string BEST_ORIG_VM { get; set; }

			public string OBJECT_ID { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERNAM { get; set; }

			public DateTime? AENDAT { get; set; }

			public string AENAM { get; set; }

			public string NAME1_HALTER { get; set; }

			public string ORT01_HALTER { get; set; }

			public static GT_DATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DATEN
				{
					KUNNR = (string)row["KUNNR"],
					HALTER = (string)row["HALTER"],
					STANDORT = (string)row["STANDORT"],
					VOLLM = (string)row["VOLLM"],
					REGISTER = (string)row["REGISTER"],
					PERSO = (string)row["PERSO"],
					GEWERBE = (string)row["GEWERBE"],
					EINZUG = (string)row["EINZUG"],
					VOLLST = (string)row["VOLLST"],
					EVB_NUM = (string)row["EVB_NUM"],
					EVB_VON = string.IsNullOrEmpty(row["EVB_VON"].ToString()) ? null : (DateTime?)row["EVB_VON"],
					EVB_BIS = string.IsNullOrEmpty(row["EVB_BIS"].ToString()) ? null : (DateTime?)row["EVB_BIS"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					KZ_ZLS = (string)row["KZ_ZLS"],
					SCHLNR_ZLS = (string)row["SCHLNR_ZLS"],
					KZ_EVBNR = (string)row["KZ_EVBNR"],
					KZ_GEWERBE = (string)row["KZ_GEWERBE"],
					KZ_HANDREG = (string)row["KZ_HANDREG"],
					KZ_EZERM = (string)row["KZ_EZERM"],
					KZ_FSPLAK = (string)row["KZ_FSPLAK"],
					KZ_KZVERST = (string)row["KZ_KZVERST"],
					WKENNZ = (string)row["WKENNZ"],
					SONST_SH = (string)row["SONST_SH"],
					BEVOLLM1 = (string)row["BEVOLLM1"],
					GUELTIG_BIS1 = string.IsNullOrEmpty(row["GUELTIG_BIS1"].ToString()) ? null : (DateTime?)row["GUELTIG_BIS1"],
					BEVOLLM2 = (string)row["BEVOLLM2"],
					GUELTIG_BIS2 = string.IsNullOrEmpty(row["GUELTIG_BIS2"].ToString()) ? null : (DateTime?)row["GUELTIG_BIS2"],
					BEVOLLM3 = (string)row["BEVOLLM3"],
					GUELTIG_BIS3 = string.IsNullOrEmpty(row["GUELTIG_BIS3"].ToString()) ? null : (DateTime?)row["GUELTIG_BIS3"],
					ZULKREIS = (string)row["ZULKREIS"],
					PERSO_FEHLT = (string)row["PERSO_FEHLT"],
					VOLLMACHT_FEHLT = (string)row["VOLLMACHT_FEHLT"],
					DAT_GW_ANMELD = string.IsNullOrEmpty(row["DAT_GW_ANMELD"].ToString()) ? null : (DateTime?)row["DAT_GW_ANMELD"],
					EZERM_KFZBEZOGEN = (string)row["EZERM_KFZBEZOGEN"],
					ZKUNNR_EXT = (string)row["ZKUNNR_EXT"],
					DAT_LOE = string.IsNullOrEmpty(row["DAT_LOE"].ToString()) ? null : (DateTime?)row["DAT_LOE"],
					WEB_USER = (string)row["WEB_USER"],
					BARCODE = (string)row["BARCODE"],
					VOLLMACHT_VON = string.IsNullOrEmpty(row["VOLLMACHT_VON"].ToString()) ? null : (DateTime?)row["VOLLMACHT_VON"],
					VOLLMACHT_BIS = string.IsNullOrEmpty(row["VOLLMACHT_BIS"].ToString()) ? null : (DateTime?)row["VOLLMACHT_BIS"],
					HREGDAT_VON = string.IsNullOrEmpty(row["HREGDAT_VON"].ToString()) ? null : (DateTime?)row["HREGDAT_VON"],
					HREGDAT_BIS = string.IsNullOrEmpty(row["HREGDAT_BIS"].ToString()) ? null : (DateTime?)row["HREGDAT_BIS"],
					GA_VON = string.IsNullOrEmpty(row["GA_VON"].ToString()) ? null : (DateTime?)row["GA_VON"],
					GA_BIS = string.IsNullOrEmpty(row["GA_BIS"].ToString()) ? null : (DateTime?)row["GA_BIS"],
					BEST_ORIG_VM = (string)row["BEST_ORIG_VM"],
					OBJECT_ID = (string)row["OBJECT_ID"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ERNAM = (string)row["ERNAM"],
					AENDAT = string.IsNullOrEmpty(row["AENDAT"].ToString()) ? null : (DateTime?)row["AENDAT"],
					AENAM = (string)row["AENAM"],
					NAME1_HALTER = (string)row["NAME1_HALTER"],
					ORT01_HALTER = (string)row["ORT01_HALTER"],

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
				return Select(dt, sapConnection).ToListOrEmptyList();
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
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DATEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_ZULDOK_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_ZULDOK_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_ZULDOK_01.GT_DATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
