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
	public partial class Z_DPM_READ_MAHN_EQSTL_02
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_MAHN_EQSTL_02).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_MAHN_EQSTL_02).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_KONTONR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KONTONR", value);
		}

		public static void SetImportParameter_I_LICENSE_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LICENSE_NUM", value);
		}

		public static void SetImportParameter_I_MAHNSTUFEN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MAHNSTUFEN", value);
		}

		public static void SetImportParameter_I_MATNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MATNR", value);
		}

		public static void SetImportParameter_I_MIT_MAHNSPERRE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MIT_MAHNSPERRE", value);
		}

		public static void SetImportParameter_I_NUR_OFFENE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NUR_OFFENE", value);
		}

		public static void SetImportParameter_I_OHNE_GESPERRTE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_OHNE_GESPERRTE", value);
		}

		public static void SetImportParameter_I_PAID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PAID", value);
		}

		public static void SetImportParameter_I_STOPDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_STOPDAT_BIS", value);
		}

		public static void SetImportParameter_I_STOPDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_STOPDAT_VON", value);
		}

		public static void SetImportParameter_I_ZULDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZULDAT_BIS", value);
		}

		public static void SetImportParameter_I_ZULDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZULDAT_VON", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR_AG { get; set; }

			public string VORG_ART { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string EQUNR { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public string ZZMAHNS { get; set; }

			public DateTime? ZZMADAT_1 { get; set; }

			public DateTime? ZZMADAT_2 { get; set; }

			public DateTime? ZZMADAT_3 { get; set; }

			public string MAHNART { get; set; }

			public string NAME1_ME { get; set; }

			public string NAME2_ME { get; set; }

			public string STREET_ME { get; set; }

			public string HOUSE_NUM1_ME { get; set; }

			public string POST_CODE1_ME { get; set; }

			public string CITY1_ME { get; set; }

			public string COUNTRY_ME { get; set; }

			public string SMTP_ADDR_ME { get; set; }

			public DateTime? EINGDAT_IST { get; set; }

			public DateTime? ZULDAT { get; set; }

			public string STATUS_GEN { get; set; }

			public DateTime? ANL_MAHNDAT_AM { get; set; }

			public string ANL_MAHNDAT_US { get; set; }

			public DateTime? MAHNSP_GES_AM { get; set; }

			public string MAHNSP_GES_US { get; set; }

			public DateTime? MAHNSP_ENTF_AM { get; set; }

			public string MAHNSP_ENTF_US { get; set; }

			public DateTime? MAHNDATUM_AB { get; set; }

			public string BEM { get; set; }

			public string KONTONR { get; set; }

			public DateTime? BEZUG_DAT { get; set; }

			public DateTime? MAHND_AB_CHG_AM { get; set; }

			public string CODE_STORT { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					KUNNR_AG = (string)row["KUNNR_AG"],
					VORG_ART = (string)row["VORG_ART"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					EQUNR = (string)row["EQUNR"],
					MATNR = (string)row["MATNR"],
					MAKTX = (string)row["MAKTX"],
					ZZMAHNS = (string)row["ZZMAHNS"],
					ZZMADAT_1 = string.IsNullOrEmpty(row["ZZMADAT_1"].ToString()) ? null : (DateTime?)row["ZZMADAT_1"],
					ZZMADAT_2 = string.IsNullOrEmpty(row["ZZMADAT_2"].ToString()) ? null : (DateTime?)row["ZZMADAT_2"],
					ZZMADAT_3 = string.IsNullOrEmpty(row["ZZMADAT_3"].ToString()) ? null : (DateTime?)row["ZZMADAT_3"],
					MAHNART = (string)row["MAHNART"],
					NAME1_ME = (string)row["NAME1_ME"],
					NAME2_ME = (string)row["NAME2_ME"],
					STREET_ME = (string)row["STREET_ME"],
					HOUSE_NUM1_ME = (string)row["HOUSE_NUM1_ME"],
					POST_CODE1_ME = (string)row["POST_CODE1_ME"],
					CITY1_ME = (string)row["CITY1_ME"],
					COUNTRY_ME = (string)row["COUNTRY_ME"],
					SMTP_ADDR_ME = (string)row["SMTP_ADDR_ME"],
					EINGDAT_IST = string.IsNullOrEmpty(row["EINGDAT_IST"].ToString()) ? null : (DateTime?)row["EINGDAT_IST"],
					ZULDAT = string.IsNullOrEmpty(row["ZULDAT"].ToString()) ? null : (DateTime?)row["ZULDAT"],
					STATUS_GEN = (string)row["STATUS_GEN"],
					ANL_MAHNDAT_AM = string.IsNullOrEmpty(row["ANL_MAHNDAT_AM"].ToString()) ? null : (DateTime?)row["ANL_MAHNDAT_AM"],
					ANL_MAHNDAT_US = (string)row["ANL_MAHNDAT_US"],
					MAHNSP_GES_AM = string.IsNullOrEmpty(row["MAHNSP_GES_AM"].ToString()) ? null : (DateTime?)row["MAHNSP_GES_AM"],
					MAHNSP_GES_US = (string)row["MAHNSP_GES_US"],
					MAHNSP_ENTF_AM = string.IsNullOrEmpty(row["MAHNSP_ENTF_AM"].ToString()) ? null : (DateTime?)row["MAHNSP_ENTF_AM"],
					MAHNSP_ENTF_US = (string)row["MAHNSP_ENTF_US"],
					MAHNDATUM_AB = string.IsNullOrEmpty(row["MAHNDATUM_AB"].ToString()) ? null : (DateTime?)row["MAHNDATUM_AB"],
					BEM = (string)row["BEM"],
					KONTONR = (string)row["KONTONR"],
					BEZUG_DAT = string.IsNullOrEmpty(row["BEZUG_DAT"].ToString()) ? null : (DateTime?)row["BEZUG_DAT"],
					MAHND_AB_CHG_AM = string.IsNullOrEmpty(row["MAHND_AB_CHG_AM"].ToString()) ? null : (DateTime?)row["MAHND_AB_CHG_AM"],
					CODE_STORT = (string)row["CODE_STORT"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_MAHN_EQSTL_02", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_MAHN_EQSTL_02", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_MAHN_EQSTL_02.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
