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
	public partial class Z_DPM_READ_VERSAND_SPERR_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_VERSAND_SPERR_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_VERSAND_SPERR_001).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_AKTION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AKTION", value);
		}

		public static void SetImportParameter_I_EMAIL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EMAIL", value);
		}

		public static void SetImportParameter_I_FREIGABEDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_FREIGABEDAT_BIS", value);
		}

		public static void SetImportParameter_I_FREIGABEDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_FREIGABEDAT_VON", value);
		}

		public static void SetImportParameter_I_NAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NAME", value);
		}

		public static void SetImportParameter_I_TREU(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TREU", value);
		}

		public static void SetImportParameter_I_VORNA(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORNA", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BELNR { get; set; }

			public string ZZKUNNR_AG { get; set; }

			public string NAME1_AG { get; set; }

			public string NAME2_AG { get; set; }

			public string STRASSE_AG { get; set; }

			public string PLZ_AG { get; set; }

			public string ORT_AG { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string TIDNR { get; set; }

			public string LICENSE_NUM { get; set; }

			public string VERSART { get; set; }

			public string NAME1_ZS { get; set; }

			public string NAME2_ZS { get; set; }

			public string STRASSE_ZS { get; set; }

			public string PLZ_ZS { get; set; }

			public string ORT_ZS { get; set; }

			public string ERNAM { get; set; }

			public DateTime? ERDAT { get; set; }

			public string SPERRSTATUS { get; set; }

			public DateTime? FREIGABEDAT { get; set; }

			public string FREIGABEUHRZEIT { get; set; }

			public string FREIGABEUSER { get; set; }

			public string NICHT_FREIG_GRU { get; set; }

			public string KUNNR_TG { get; set; }

			public string NAME1_TG { get; set; }

			public string LIZNR { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					BELNR = (string)row["BELNR"],
					ZZKUNNR_AG = (string)row["ZZKUNNR_AG"],
					NAME1_AG = (string)row["NAME1_AG"],
					NAME2_AG = (string)row["NAME2_AG"],
					STRASSE_AG = (string)row["STRASSE_AG"],
					PLZ_AG = (string)row["PLZ_AG"],
					ORT_AG = (string)row["ORT_AG"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					TIDNR = (string)row["TIDNR"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					VERSART = (string)row["VERSART"],
					NAME1_ZS = (string)row["NAME1_ZS"],
					NAME2_ZS = (string)row["NAME2_ZS"],
					STRASSE_ZS = (string)row["STRASSE_ZS"],
					PLZ_ZS = (string)row["PLZ_ZS"],
					ORT_ZS = (string)row["ORT_ZS"],
					ERNAM = (string)row["ERNAM"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					SPERRSTATUS = (string)row["SPERRSTATUS"],
					FREIGABEDAT = string.IsNullOrEmpty(row["FREIGABEDAT"].ToString()) ? null : (DateTime?)row["FREIGABEDAT"],
					FREIGABEUHRZEIT = (string)row["FREIGABEUHRZEIT"],
					FREIGABEUSER = (string)row["FREIGABEUSER"],
					NICHT_FREIG_GRU = (string)row["NICHT_FREIG_GRU"],
					KUNNR_TG = (string)row["KUNNR_TG"],
					NAME1_TG = (string)row["NAME1_TG"],
					LIZNR = (string)row["LIZNR"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],

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

			public static IEnumerable<GT_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_VERSAND_SPERR_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_VERSAND_SPERR_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_VERSAND_SPERR_001.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
