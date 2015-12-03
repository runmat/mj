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
	public partial class Z_M_TH_BESTAND
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_TH_BESTAND).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_TH_BESTAND).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_EQTYP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQTYP", value);
		}

		public static void SetImportParameter_I_SPD_TAGESDAT_MINUS_TAGE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SPD_TAGESDAT_MINUS_TAGE", value);
		}

		public static void SetImportParameter_I_SPERRDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_SPERRDAT_BIS", value);
		}

		public static void SetImportParameter_I_SPERRDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_SPERRDAT_VON", value);
		}

		public static void SetImportParameter_I_TG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TG", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_BESTAND : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AG { get; set; }

			public string TG { get; set; }

			public string AG_NAME1 { get; set; }

			public string TG_NAME1 { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string ABCKZ { get; set; }

			public string ABCTX { get; set; }

			public string TIDNR { get; set; }

			public string LIZNR { get; set; }

			public string NAME1 { get; set; }

			public string STREET { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string COUNTRY { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public string TG_NAME2 { get; set; }

			public string TG_STREET { get; set; }

			public string TG_POST_CODE1 { get; set; }

			public string TG_CITY1 { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public DateTime? SPERRDAT { get; set; }

			public static GT_BESTAND Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_BESTAND
				{
					AG = (string)row["AG"],
					TG = (string)row["TG"],
					AG_NAME1 = (string)row["AG_NAME1"],
					TG_NAME1 = (string)row["TG_NAME1"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ABCKZ = (string)row["ABCKZ"],
					ABCTX = (string)row["ABCTX"],
					TIDNR = (string)row["TIDNR"],
					LIZNR = (string)row["LIZNR"],
					NAME1 = (string)row["NAME1"],
					STREET = (string)row["STREET"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					COUNTRY = (string)row["COUNTRY"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
					TG_NAME2 = (string)row["TG_NAME2"],
					TG_STREET = (string)row["TG_STREET"],
					TG_POST_CODE1 = (string)row["TG_POST_CODE1"],
					TG_CITY1 = (string)row["TG_CITY1"],
					ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					SPERRDAT = string.IsNullOrEmpty(row["SPERRDAT"].ToString()) ? null : (DateTime?)row["SPERRDAT"],

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

			public static IEnumerable<GT_BESTAND> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BESTAND> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BESTAND> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BESTAND).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BESTAND> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTAND> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BESTAND> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BESTAND>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_TH_BESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTAND> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTAND>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTAND> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTAND>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTAND> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BESTAND>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_TH_BESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTAND> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTAND>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_TH_BESTAND.GT_BESTAND> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
