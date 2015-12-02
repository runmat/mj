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
	public partial class Z_DPM_UF_MELDUNGS_SUCHE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_UF_MELDUNGS_SUCHE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_UF_MELDUNGS_SUCHE).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_ABMDT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ABMDT_BIS", value);
		}

		public static void SetImportParameter_I_ABMDT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ABMDT_VON", value);
		}

		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public static void SetImportParameter_I_ERDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ERDAT_BIS", value);
		}

		public static void SetImportParameter_I_ERDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ERDAT_VON", value);
		}

		public static void SetImportParameter_I_ERNAM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ERNAM", value);
		}

		public static void SetImportParameter_I_LICENSE_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LICENSE_NUM", value);
		}

		public static void SetImportParameter_I_MAHNSTUFE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MAHNSTUFE", value);
		}

		public static void SetImportParameter_I_MIT_ABM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MIT_ABM", value);
		}

		public static void SetImportParameter_I_OHNE_ABM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_OHNE_ABM", value);
		}

		public static void SetImportParameter_I_STATION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STATION", value);
		}

		public static void SetImportParameter_I_STORNO(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STORNO", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_UF : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string UNFALL_NR { get; set; }

			public string STATUS { get; set; }

			public string AG { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERNAM { get; set; }

			public string EQUNR { get; set; }

			public string LICENSE_NUM { get; set; }

			public string CHASSIS_NUM { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public string MODELL_BEZ { get; set; }

			public decimal? REST_PREIS { get; set; }

			public string WAERS { get; set; }

			public string STATION { get; set; }

			public string STANDORT { get; set; }

			public DateTime? VSDAT { get; set; }

			public string MAHNSTUFE { get; set; }

			public DateTime? MAHNDAT1 { get; set; }

			public DateTime? MAHNDAT2 { get; set; }

			public DateTime? MAHNDAT3 { get; set; }

			public DateTime? EG_KENNZ { get; set; }

			public DateTime? ABMDT { get; set; }

			public DateTime? VSZB2DAT { get; set; }

			public DateTime? STORNODAT { get; set; }

			public string STORNOBEM { get; set; }

			public string STORNONAM { get; set; }

			public static GT_UF Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_UF
				{
					UNFALL_NR = (string)row["UNFALL_NR"],
					STATUS = (string)row["STATUS"],
					AG = (string)row["AG"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ERNAM = (string)row["ERNAM"],
					EQUNR = (string)row["EQUNR"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					ERSTZULDAT = string.IsNullOrEmpty(row["ERSTZULDAT"].ToString()) ? null : (DateTime?)row["ERSTZULDAT"],
					MODELL_BEZ = (string)row["MODELL_BEZ"],
					REST_PREIS = string.IsNullOrEmpty(row["REST_PREIS"].ToString()) ? null : (decimal?)row["REST_PREIS"],
					WAERS = (string)row["WAERS"],
					STATION = (string)row["STATION"],
					STANDORT = (string)row["STANDORT"],
					VSDAT = string.IsNullOrEmpty(row["VSDAT"].ToString()) ? null : (DateTime?)row["VSDAT"],
					MAHNSTUFE = (string)row["MAHNSTUFE"],
					MAHNDAT1 = string.IsNullOrEmpty(row["MAHNDAT1"].ToString()) ? null : (DateTime?)row["MAHNDAT1"],
					MAHNDAT2 = string.IsNullOrEmpty(row["MAHNDAT2"].ToString()) ? null : (DateTime?)row["MAHNDAT2"],
					MAHNDAT3 = string.IsNullOrEmpty(row["MAHNDAT3"].ToString()) ? null : (DateTime?)row["MAHNDAT3"],
					EG_KENNZ = string.IsNullOrEmpty(row["EG_KENNZ"].ToString()) ? null : (DateTime?)row["EG_KENNZ"],
					ABMDT = string.IsNullOrEmpty(row["ABMDT"].ToString()) ? null : (DateTime?)row["ABMDT"],
					VSZB2DAT = string.IsNullOrEmpty(row["VSZB2DAT"].ToString()) ? null : (DateTime?)row["VSZB2DAT"],
					STORNODAT = string.IsNullOrEmpty(row["STORNODAT"].ToString()) ? null : (DateTime?)row["STORNODAT"],
					STORNOBEM = (string)row["STORNOBEM"],
					STORNONAM = (string)row["STORNONAM"],

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

			public static IEnumerable<GT_UF> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_UF> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_UF> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_UF).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_UF> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_UF> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_UF> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_UF>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_UF_MELDUNGS_SUCHE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_UF> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_UF>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_UF> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_UF>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_UF> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_UF>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_UF_MELDUNGS_SUCHE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_UF> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_UF>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_UF_MELDUNGS_SUCHE.GT_UF> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
