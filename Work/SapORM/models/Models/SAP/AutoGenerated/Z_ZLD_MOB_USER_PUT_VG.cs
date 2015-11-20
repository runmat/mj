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
	public partial class Z_ZLD_MOB_USER_PUT_VG
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_MOB_USER_PUT_VG).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_MOB_USER_PUT_VG).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_MOBUSER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MOBUSER", value);
		}

		public void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public void SetImportParameter_I_VKORG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKORG", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_VG_KOPF : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BLTYP { get; set; }

			public string ZULBELN { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public string VZD_VKBUR { get; set; }

			public string KUNNR { get; set; }

			public string KUNDENNAME { get; set; }

			public string REFERENZ1 { get; set; }

			public string REFERENZ2 { get; set; }

			public string KREISKZ { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZKENN { get; set; }

			public string BEB_STATUS { get; set; }

			public string EC_JN { get; set; }

			public string BAR_JN { get; set; }

			public string RE_JN { get; set; }

			public string INFO_TEXT { get; set; }

			public string NACHBEARBEITEN { get; set; }

			public string WUNSCHKENN_JN { get; set; }

			public string RESERVKENN_JN { get; set; }

			public string EINKENN_JN { get; set; }

			public string KENNZFORM { get; set; }

			public string KENNZANZ { get; set; }

			public string BEMERKUNG { get; set; }

			public string TEL_NUMBER { get; set; }

			public string TEL_EXTENS { get; set; }

			public string VE_ERNAM { get; set; }

			public string SAISON_KNZ { get; set; }

			public string SAISON_BEG { get; set; }

			public string SAISON_END { get; set; }

			public static GT_VG_KOPF Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_VG_KOPF
				{
					BLTYP = (string)row["BLTYP"],
					ZULBELN = (string)row["ZULBELN"],
					VKORG = (string)row["VKORG"],
					VKBUR = (string)row["VKBUR"],
					VZD_VKBUR = (string)row["VZD_VKBUR"],
					KUNNR = (string)row["KUNNR"],
					KUNDENNAME = (string)row["KUNDENNAME"],
					REFERENZ1 = (string)row["REFERENZ1"],
					REFERENZ2 = (string)row["REFERENZ2"],
					KREISKZ = (string)row["KREISKZ"],
					ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
					ZZKENN = (string)row["ZZKENN"],
					BEB_STATUS = (string)row["BEB_STATUS"],
					EC_JN = (string)row["EC_JN"],
					BAR_JN = (string)row["BAR_JN"],
					RE_JN = (string)row["RE_JN"],
					INFO_TEXT = (string)row["INFO_TEXT"],
					NACHBEARBEITEN = (string)row["NACHBEARBEITEN"],
					WUNSCHKENN_JN = (string)row["WUNSCHKENN_JN"],
					RESERVKENN_JN = (string)row["RESERVKENN_JN"],
					EINKENN_JN = (string)row["EINKENN_JN"],
					KENNZFORM = (string)row["KENNZFORM"],
					KENNZANZ = (string)row["KENNZANZ"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					TEL_NUMBER = (string)row["TEL_NUMBER"],
					TEL_EXTENS = (string)row["TEL_EXTENS"],
					VE_ERNAM = (string)row["VE_ERNAM"],
					SAISON_KNZ = (string)row["SAISON_KNZ"],
					SAISON_BEG = (string)row["SAISON_BEG"],
					SAISON_END = (string)row["SAISON_END"],

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

			public static IEnumerable<GT_VG_KOPF> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_VG_KOPF> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_VG_KOPF> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_VG_KOPF).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_VG_KOPF> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_KOPF> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_VG_KOPF> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VG_KOPF>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_MOB_USER_PUT_VG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_KOPF> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VG_KOPF>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_KOPF> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VG_KOPF>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_KOPF> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VG_KOPF>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_MOB_USER_PUT_VG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_KOPF> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VG_KOPF>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_VG_POS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string ZULPOSNR { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public decimal? GEB_AMT { get; set; }

			public string GEBMAT { get; set; }

			public static GT_VG_POS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_VG_POS
				{
					ZULBELN = (string)row["ZULBELN"],
					ZULPOSNR = (string)row["ZULPOSNR"],
					MATNR = (string)row["MATNR"],
					MAKTX = (string)row["MAKTX"],
					GEB_AMT = string.IsNullOrEmpty(row["GEB_AMT"].ToString()) ? null : (decimal?)row["GEB_AMT"],
					GEBMAT = (string)row["GEBMAT"],

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

			public static IEnumerable<GT_VG_POS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_VG_POS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_VG_POS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_VG_POS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_VG_POS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_POS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_VG_POS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VG_POS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_MOB_USER_PUT_VG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_POS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VG_POS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_POS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VG_POS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_POS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VG_POS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_MOB_USER_PUT_VG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VG_POS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VG_POS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_MOB_USER_PUT_VG.GT_VG_KOPF> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_MOB_USER_PUT_VG.GT_VG_POS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
