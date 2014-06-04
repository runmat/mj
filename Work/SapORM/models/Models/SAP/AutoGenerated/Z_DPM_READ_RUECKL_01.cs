using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_READ_RUECKL_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_RUECKL_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_RUECKL_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VERTRAGSNR_HLA { get; set; }

			public string CHASSIS_NUM { get; set; }

			public static GT_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IN
				{
					VERTRAGSNR_HLA = (string)row["VERTRAGSNR_HLA"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],

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

			public static IEnumerable<GT_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_IN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_RUECKL_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_RUECKL_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VERTRAGSNR_HLA { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string STANDORT { get; set; }

			public string RUECKGAB_OPTION { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? BEST_ABH_TERMIN { get; set; }

			public DateTime? IUG_DAT { get; set; }

			public DateTime? PROT_EING_O { get; set; }

			public DateTime? IUN_DAT { get; set; }

			public DateTime? DAT_GUTA_BEAUFTRAGT { get; set; }

			public DateTime? PROT_EING_D { get; set; }

			public DateTime? ABMELDEDATUM { get; set; }

			public DateTime? FZG_BEREIT_BLG_SGS { get; set; }

			public DateTime? DAT_GUTA_ERHALT { get; set; }

			public string VERW_ZULETZT_AKTUALISIERT { get; set; }

			public DateTime? DAT_INSERAT { get; set; }

			public DateTime? EING_AUFBER_AUFTR { get; set; }

			public DateTime? AUFBER_FERTIG { get; set; }

			public string SERVICE_LEVEL_2_3 { get; set; }

			public string SERVICE_LEVEL_5_7 { get; set; }

			public string SERVICE_LEVEL_7_14 { get; set; }

			public string SERVICE_LEVEL_8_16 { get; set; }

			public string SERVICE_LEVEL_2_7 { get; set; }

			public string SERVICE_LEVEL_19_21 { get; set; }

			public DateTime? FE_BLG { get; set; }

			public DateTime? FB_GUTA { get; set; }

			public string VORGANGS_ID { get; set; }

			public string GUTA_ERSTELL_1 { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					VERTRAGSNR_HLA = (string)row["VERTRAGSNR_HLA"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					STANDORT = (string)row["STANDORT"],
					RUECKGAB_OPTION = (string)row["RUECKGAB_OPTION"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					BEST_ABH_TERMIN = (string.IsNullOrEmpty(row["BEST_ABH_TERMIN"].ToString())) ? null : (DateTime?)row["BEST_ABH_TERMIN"],
					IUG_DAT = (string.IsNullOrEmpty(row["IUG_DAT"].ToString())) ? null : (DateTime?)row["IUG_DAT"],
					PROT_EING_O = (string.IsNullOrEmpty(row["PROT_EING_O"].ToString())) ? null : (DateTime?)row["PROT_EING_O"],
					IUN_DAT = (string.IsNullOrEmpty(row["IUN_DAT"].ToString())) ? null : (DateTime?)row["IUN_DAT"],
					DAT_GUTA_BEAUFTRAGT = (string.IsNullOrEmpty(row["DAT_GUTA_BEAUFTRAGT"].ToString())) ? null : (DateTime?)row["DAT_GUTA_BEAUFTRAGT"],
					PROT_EING_D = (string.IsNullOrEmpty(row["PROT_EING_D"].ToString())) ? null : (DateTime?)row["PROT_EING_D"],
					ABMELDEDATUM = (string.IsNullOrEmpty(row["ABMELDEDATUM"].ToString())) ? null : (DateTime?)row["ABMELDEDATUM"],
					FZG_BEREIT_BLG_SGS = (string.IsNullOrEmpty(row["FZG_BEREIT_BLG_SGS"].ToString())) ? null : (DateTime?)row["FZG_BEREIT_BLG_SGS"],
					DAT_GUTA_ERHALT = (string.IsNullOrEmpty(row["DAT_GUTA_ERHALT"].ToString())) ? null : (DateTime?)row["DAT_GUTA_ERHALT"],
					VERW_ZULETZT_AKTUALISIERT = (string)row["VERW_ZULETZT_AKTUALISIERT"],
					DAT_INSERAT = (string.IsNullOrEmpty(row["DAT_INSERAT"].ToString())) ? null : (DateTime?)row["DAT_INSERAT"],
					EING_AUFBER_AUFTR = (string.IsNullOrEmpty(row["EING_AUFBER_AUFTR"].ToString())) ? null : (DateTime?)row["EING_AUFBER_AUFTR"],
					AUFBER_FERTIG = (string.IsNullOrEmpty(row["AUFBER_FERTIG"].ToString())) ? null : (DateTime?)row["AUFBER_FERTIG"],
					SERVICE_LEVEL_2_3 = (string)row["SERVICE_LEVEL_2_3"],
					SERVICE_LEVEL_5_7 = (string)row["SERVICE_LEVEL_5_7"],
					SERVICE_LEVEL_7_14 = (string)row["SERVICE_LEVEL_7_14"],
					SERVICE_LEVEL_8_16 = (string)row["SERVICE_LEVEL_8_16"],
					SERVICE_LEVEL_2_7 = (string)row["SERVICE_LEVEL_2_7"],
					SERVICE_LEVEL_19_21 = (string)row["SERVICE_LEVEL_19_21"],
					FE_BLG = (string.IsNullOrEmpty(row["FE_BLG"].ToString())) ? null : (DateTime?)row["FE_BLG"],
					FB_GUTA = (string.IsNullOrEmpty(row["FB_GUTA"].ToString())) ? null : (DateTime?)row["FB_GUTA"],
					VORGANGS_ID = (string)row["VORGANGS_ID"],
					GUTA_ERSTELL_1 = (string)row["GUTA_ERSTELL_1"],

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
				return Select(dt, sapConnection).ToList();
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
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_RUECKL_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_RUECKL_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_RUECKL_01.GT_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_READ_RUECKL_01.GT_IN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_RUECKL_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_READ_RUECKL_01.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
