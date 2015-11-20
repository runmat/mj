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
	public partial class Z_M_VERSAUFTR_FEHLERHAFTE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_VERSAUFTR_FEHLERHAFTE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_VERSAUFTR_FEHLERHAFTE).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_FLAG_VERS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("FLAG_VERS", value);
		}

		public void SetImportParameter_FLAG_VERS_SPERR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("FLAG_VERS_SPERR", value);
		}

		public void SetImportParameter_I_BEAUFTR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BEAUFTR", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_TREUGEBER_VERS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TREUGEBER_VERS", value);
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

			public string LICENSE_NUM { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string ZZKUNNR_ZS { get; set; }

			public string ZZADRNR_ZS { get; set; }

			public string ZZKONZS_ZS { get; set; }

			public string ZZNAME1_ZS { get; set; }

			public string ZZNAME2_ZS { get; set; }

			public string ZZSTRAS_ZS { get; set; }

			public string ZZHAUSNR_ZS { get; set; }

			public string ZZPSTLZ_ZS { get; set; }

			public string ZZORT01_ZS { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ZZFCODE { get; set; }

			public string ZZFCOD_TEXT { get; set; }

			public string ZZNAME1_TG { get; set; }

			public string ZZBRFVERS { get; set; }

			public string ZZSCHLVERS { get; set; }

			public string ABCKZ { get; set; }

			public string KENNZEICHNG { get; set; }

			public string KUNNR_TG { get; set; }

			public string IDNRK { get; set; }

			public string MAKTX { get; set; }

			public string ZANF_NR { get; set; }

			public string LIZNR { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					KUNNR = (string)row["KUNNR"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					ZZKUNNR_ZS = (string)row["ZZKUNNR_ZS"],
					ZZADRNR_ZS = (string)row["ZZADRNR_ZS"],
					ZZKONZS_ZS = (string)row["ZZKONZS_ZS"],
					ZZNAME1_ZS = (string)row["ZZNAME1_ZS"],
					ZZNAME2_ZS = (string)row["ZZNAME2_ZS"],
					ZZSTRAS_ZS = (string)row["ZZSTRAS_ZS"],
					ZZHAUSNR_ZS = (string)row["ZZHAUSNR_ZS"],
					ZZPSTLZ_ZS = (string)row["ZZPSTLZ_ZS"],
					ZZORT01_ZS = (string)row["ZZORT01_ZS"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ZZFCODE = (string)row["ZZFCODE"],
					ZZFCOD_TEXT = (string)row["ZZFCOD_TEXT"],
					ZZNAME1_TG = (string)row["ZZNAME1_TG"],
					ZZBRFVERS = (string)row["ZZBRFVERS"],
					ZZSCHLVERS = (string)row["ZZSCHLVERS"],
					ABCKZ = (string)row["ABCKZ"],
					KENNZEICHNG = (string)row["KENNZEICHNG"],
					KUNNR_TG = (string)row["KUNNR_TG"],
					IDNRK = (string)row["IDNRK"],
					MAKTX = (string)row["MAKTX"],
					ZANF_NR = (string)row["ZANF_NR"],
					LIZNR = (string)row["LIZNR"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_VERSAUFTR_FEHLERHAFTE", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_VERSAUFTR_FEHLERHAFTE", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_M_VERSAUFTR_FEHLERHAFTE.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
