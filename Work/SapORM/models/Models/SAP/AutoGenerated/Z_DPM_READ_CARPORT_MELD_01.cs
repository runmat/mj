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
	public partial class Z_DPM_READ_CARPORT_MELD_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_CARPORT_MELD_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_CARPORT_MELD_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AUFTRAGS_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AUFTRAGS_NR", value);
		}

		public static void SetImportParameter_I_CARPORT_ID_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CARPORT_ID_AG", value);
		}

		public static void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public static void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public static void SetImportParameter_I_LICENSE_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LICENSE_NUM", value);
		}

		public static void SetImportParameter_I_MVA_NUMMER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MVA_NUMMER", value);
		}

		public static void SetImportParameter_I_NUR_OFF_NL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NUR_OFF_NL", value);
		}

		public static void SetImportParameter_I_ORGANISATION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ORGANISATION", value);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR_AG { get; set; }

			public string CARPORT_ID_AG { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string AUFTRAGS_NR { get; set; }

			public string MVA_NUMMER { get; set; }

			public string BARCODE { get; set; }

			public string ANZ_KENNZ_CPL { get; set; }

			public string VORLAGE_ZB1_CPL { get; set; }

			public string ZB2_VORH { get; set; }

			public string COC_VORH { get; set; }

			public string SERVICEH_VORH { get; set; }

			public string HU_AU_BER_VORH { get; set; }

			public string ZWEITSCHLUE_VORH { get; set; }

			public string NAVI_CD_VORH { get; set; }

			public string ORGANISATION { get; set; }

			public DateTime? DAT_DEMONT { get; set; }

			public string FZG_ABGEMELDET { get; set; }

			public string WEB_USER { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					KUNNR_AG = (string)row["KUNNR_AG"],
					CARPORT_ID_AG = (string)row["CARPORT_ID_AG"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					AUFTRAGS_NR = (string)row["AUFTRAGS_NR"],
					MVA_NUMMER = (string)row["MVA_NUMMER"],
					BARCODE = (string)row["BARCODE"],
					ANZ_KENNZ_CPL = (string)row["ANZ_KENNZ_CPL"],
					VORLAGE_ZB1_CPL = (string)row["VORLAGE_ZB1_CPL"],
					ZB2_VORH = (string)row["ZB2_VORH"],
					COC_VORH = (string)row["COC_VORH"],
					SERVICEH_VORH = (string)row["SERVICEH_VORH"],
					HU_AU_BER_VORH = (string)row["HU_AU_BER_VORH"],
					ZWEITSCHLUE_VORH = (string)row["ZWEITSCHLUE_VORH"],
					NAVI_CD_VORH = (string)row["NAVI_CD_VORH"],
					ORGANISATION = (string)row["ORGANISATION"],
					DAT_DEMONT = string.IsNullOrEmpty(row["DAT_DEMONT"].ToString()) ? null : (DateTime?)row["DAT_DEMONT"],
					FZG_ABGEMELDET = (string)row["FZG_ABGEMELDET"],
					WEB_USER = (string)row["WEB_USER"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_CARPORT_MELD_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_CARPORT_MELD_01", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_CARPORT_MELD_01.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
