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
	public partial class Z_ALL_DEBI_VORERFASSUNG_WEB
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ALL_DEBI_VORERFASSUNG_WEB).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ALL_DEBI_VORERFASSUNG_WEB).Name, inputParameterKeys, inputParameterValues);
		}


		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public string GetExportParameter_E_VKUNNR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_VKUNNR");
		}

		public partial class GS_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BEDIEN { get; set; }

			public string BUKRS { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public string KALKS { get; set; }

			public string EZERM { get; set; }

			public string TITLE { get; set; }

			public string BRSCH { get; set; }

			public string BRSCH_FREITXT { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string NAME4 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string CITY1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string LAND1 { get; set; }

			public string STCEG { get; set; }

			public string AP_NAMEV { get; set; }

			public string AP_NAME1 { get; set; }

			public string AP_PAFKT { get; set; }

			public string AP_TEL_NUMBER { get; set; }

			public string AP_MOB_NUMBER { get; set; }

			public string AP_FAX_NUMBER { get; set; }

			public string AP_SMTP_ADDR { get; set; }

			public string QUELLE { get; set; }

			public string ERNAM { get; set; }

			public string BANKS { get; set; }

			public string BANKL { get; set; }

			public string BNKLZ { get; set; }

			public string BANKN { get; set; }

			public string SWIFT { get; set; }

			public string IBAN { get; set; }

			public string GRUPPE_T { get; set; }

			public decimal? UMS_P_MON { get; set; }

			public string GEB_M_UST { get; set; }

			public string KREDITVS { get; set; }

			public string AUSKUNFT { get; set; }

			public string BEMERKUNG { get; set; }

			public static GS_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GS_IN
				{
					BEDIEN = (string)row["BEDIEN"],
					BUKRS = (string)row["BUKRS"],
					VKORG = (string)row["VKORG"],
					VKBUR = (string)row["VKBUR"],
					KALKS = (string)row["KALKS"],
					EZERM = (string)row["EZERM"],
					TITLE = (string)row["TITLE"],
					BRSCH = (string)row["BRSCH"],
					BRSCH_FREITXT = (string)row["BRSCH_FREITXT"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					NAME4 = (string)row["NAME4"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					CITY1 = (string)row["CITY1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					LAND1 = (string)row["LAND1"],
					STCEG = (string)row["STCEG"],
					AP_NAMEV = (string)row["AP_NAMEV"],
					AP_NAME1 = (string)row["AP_NAME1"],
					AP_PAFKT = (string)row["AP_PAFKT"],
					AP_TEL_NUMBER = (string)row["AP_TEL_NUMBER"],
					AP_MOB_NUMBER = (string)row["AP_MOB_NUMBER"],
					AP_FAX_NUMBER = (string)row["AP_FAX_NUMBER"],
					AP_SMTP_ADDR = (string)row["AP_SMTP_ADDR"],
					QUELLE = (string)row["QUELLE"],
					ERNAM = (string)row["ERNAM"],
					BANKS = (string)row["BANKS"],
					BANKL = (string)row["BANKL"],
					BNKLZ = (string)row["BNKLZ"],
					BANKN = (string)row["BANKN"],
					SWIFT = (string)row["SWIFT"],
					IBAN = (string)row["IBAN"],
					GRUPPE_T = (string)row["GRUPPE_T"],
					UMS_P_MON = string.IsNullOrEmpty(row["UMS_P_MON"].ToString()) ? null : (decimal?)row["UMS_P_MON"],
					GEB_M_UST = (string)row["GEB_M_UST"],
					KREDITVS = (string)row["KREDITVS"],
					AUSKUNFT = (string)row["AUSKUNFT"],
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

			public static IEnumerable<GS_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static IEnumerable<GS_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GS_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GS_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GS_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ALL_DEBI_VORERFASSUNG_WEB", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GS_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GS_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ALL_DEBI_VORERFASSUNG_WEB.GS_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
