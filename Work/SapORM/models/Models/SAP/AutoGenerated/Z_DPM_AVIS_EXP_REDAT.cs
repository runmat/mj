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
	public partial class Z_DPM_AVIS_EXP_REDAT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_AVIS_EXP_REDAT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_AVIS_EXP_REDAT).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_ABGEARB(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ABGEARB", value);
		}

		public void SetImportParameter_I_FAHRGNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRGNR", value);
		}

		public void SetImportParameter_I_LEIST_CODE_BIS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LEIST_CODE_BIS", value);
		}

		public void SetImportParameter_I_LEIST_CODE_VON(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LEIST_CODE_VON", value);
		}

		public void SetImportParameter_I_LEIST_DATE_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_LEIST_DATE_BIS", value);
		}

		public void SetImportParameter_I_LEIST_DATE_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_LEIST_DATE_VON", value);
		}

		public void SetImportParameter_I_NUR_OHNE_GRUNDDAT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NUR_OHNE_GRUNDDAT", value);
		}

		public void SetImportParameter_I_RECH_NR_BIS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_RECH_NR_BIS", value);
		}

		public void SetImportParameter_I_RECH_NR_VON(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_RECH_NR_VON", value);
		}

		public void SetImportParameter_I_REDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_REDAT_BIS", value);
		}

		public void SetImportParameter_I_REDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_REDAT_VON", value);
		}

		public void SetImportParameter_I_SEL_ART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SEL_ART", value);
		}

		public void SetImportParameter_I_SPEDI(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SPEDI", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
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

			public string ID { get; set; }

			public string AG { get; set; }

			public DateTime? ERDAT { get; set; }

			public string SPEDI { get; set; }

			public string RECH_NR { get; set; }

			public DateTime? REDAT { get; set; }

			public decimal? ANZAHL { get; set; }

			public string FAHRGNR { get; set; }

			public string LEIST_CODE { get; set; }

			public decimal? BETRAG { get; set; }

			public DateTime? LEIST_DATE { get; set; }

			public string LEIST_TEXT { get; set; }

			public string CARPORT { get; set; }

			public string PSTLZ_VON { get; set; }

			public string ORT_VON { get; set; }

			public string PLZ_NACH { get; set; }

			public string ORT_NACH { get; set; }

			public string SAP_SD { get; set; }

			public string HERST { get; set; }

			public string VERTRAG { get; set; }

			public string CARPORT_BEZ { get; set; }

			public string LEASING { get; set; }

			public string HERST_NUMMER { get; set; }

			public string MAKE_CODE { get; set; }

			public string AUFBAUART { get; set; }

			public string MVA_NUMMER { get; set; }

			public string OWNER_CODE { get; set; }

			public string LIEFERANT { get; set; }

			public string KENNZEICHEN { get; set; }

			public DateTime? ZULASS_DAT { get; set; }

			public DateTime? DAT_AUSS_BETR { get; set; }

			public string DEPARTMENT { get; set; }

			public string ABGEARB_FLAG { get; set; }

			public string ABGEARB_USER { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					ID = (string)row["ID"],
					AG = (string)row["AG"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					SPEDI = (string)row["SPEDI"],
					RECH_NR = (string)row["RECH_NR"],
					REDAT = string.IsNullOrEmpty(row["REDAT"].ToString()) ? null : (DateTime?)row["REDAT"],
					ANZAHL = string.IsNullOrEmpty(row["ANZAHL"].ToString()) ? null : (decimal?)row["ANZAHL"],
					FAHRGNR = (string)row["FAHRGNR"],
					LEIST_CODE = (string)row["LEIST_CODE"],
					BETRAG = string.IsNullOrEmpty(row["BETRAG"].ToString()) ? null : (decimal?)row["BETRAG"],
					LEIST_DATE = string.IsNullOrEmpty(row["LEIST_DATE"].ToString()) ? null : (DateTime?)row["LEIST_DATE"],
					LEIST_TEXT = (string)row["LEIST_TEXT"],
					CARPORT = (string)row["CARPORT"],
					PSTLZ_VON = (string)row["PSTLZ_VON"],
					ORT_VON = (string)row["ORT_VON"],
					PLZ_NACH = (string)row["PLZ_NACH"],
					ORT_NACH = (string)row["ORT_NACH"],
					SAP_SD = (string)row["SAP_SD"],
					HERST = (string)row["HERST"],
					VERTRAG = (string)row["VERTRAG"],
					CARPORT_BEZ = (string)row["CARPORT_BEZ"],
					LEASING = (string)row["LEASING"],
					HERST_NUMMER = (string)row["HERST_NUMMER"],
					MAKE_CODE = (string)row["MAKE_CODE"],
					AUFBAUART = (string)row["AUFBAUART"],
					MVA_NUMMER = (string)row["MVA_NUMMER"],
					OWNER_CODE = (string)row["OWNER_CODE"],
					LIEFERANT = (string)row["LIEFERANT"],
					KENNZEICHEN = (string)row["KENNZEICHEN"],
					ZULASS_DAT = string.IsNullOrEmpty(row["ZULASS_DAT"].ToString()) ? null : (DateTime?)row["ZULASS_DAT"],
					DAT_AUSS_BETR = string.IsNullOrEmpty(row["DAT_AUSS_BETR"].ToString()) ? null : (DateTime?)row["DAT_AUSS_BETR"],
					DEPARTMENT = (string)row["DEPARTMENT"],
					ABGEARB_FLAG = (string)row["ABGEARB_FLAG"],
					ABGEARB_USER = (string)row["ABGEARB_USER"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_AVIS_EXP_REDAT", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_AVIS_EXP_REDAT", inputParameterKeys, inputParameterValues);
				 
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

		public partial class GT_SPEDITEUR : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string SPEDITEUR { get; set; }

			public static GT_SPEDITEUR Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SPEDITEUR
				{
					SPEDITEUR = (string)row["SPEDITEUR"],

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

			public static IEnumerable<GT_SPEDITEUR> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SPEDITEUR> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SPEDITEUR> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SPEDITEUR).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SPEDITEUR> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SPEDITEUR> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SPEDITEUR> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SPEDITEUR>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_AVIS_EXP_REDAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SPEDITEUR> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SPEDITEUR>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SPEDITEUR> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SPEDITEUR>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SPEDITEUR> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SPEDITEUR>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_AVIS_EXP_REDAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SPEDITEUR> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SPEDITEUR>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_AVIS_EXP_REDAT.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_AVIS_EXP_REDAT.GT_SPEDITEUR> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
