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
	public partial class Z_ZLD_AH_ZULLISTE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_AH_ZULLISTE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_AH_ZULLISTE).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_ERDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ERDAT_BIS", value);
		}

		public static void SetImportParameter_I_ERDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ERDAT_VON", value);
		}

		public static void SetImportParameter_I_GRUPPE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_GRUPPE", value);
		}

		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public static void SetImportParameter_I_LISTE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LISTE", value);
		}

		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static void SetImportParameter_I_VKORG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKORG", value);
		}

		public static void SetImportParameter_I_ZZKENN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZKENN", value);
		}

		public static void SetImportParameter_I_ZZREFNR1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFNR1", value);
		}

		public static void SetImportParameter_I_ZZREFNR2(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFNR2", value);
		}

		public static void SetImportParameter_I_ZZREFNR3(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFNR3", value);
		}

		public static void SetImportParameter_I_ZZREFNR4(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFNR4", value);
		}

		public static void SetImportParameter_I_ZZZLDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZZLDAT_BIS", value);
		}

		public static void SetImportParameter_I_ZZZLDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZZLDAT_VON", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_KUN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string NAME1 { get; set; }

			public string EXTENSION1 { get; set; }

			public string REFNAME1 { get; set; }

			public string REFNAME2 { get; set; }

			public string REFNAME3 { get; set; }

			public string REFNAME4 { get; set; }

			public static GT_KUN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_KUN
				{
					KUNNR = (string)row["KUNNR"],
					NAME1 = (string)row["NAME1"],
					EXTENSION1 = (string)row["EXTENSION1"],
					REFNAME1 = (string)row["REFNAME1"],
					REFNAME2 = (string)row["REFNAME2"],
					REFNAME3 = (string)row["REFNAME3"],
					REFNAME4 = (string)row["REFNAME4"],

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

			public static IEnumerable<GT_KUN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_KUN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_KUN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_KUN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_KUN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_KUN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_KUN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_KUN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_ZULLISTE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_KUN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_KUN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_KUN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_KUN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_KUN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_KUN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_ZULLISTE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_KUN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_KUN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
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

			public string KUNNR { get; set; }

			public string ZULBELN { get; set; }

			public string VBELN { get; set; }

			public DateTime? VE_ERDAT { get; set; }

			public string VE_ERZEIT { get; set; }

			public string VE_ERNAM { get; set; }

			public string ZZREFNR1 { get; set; }

			public string ZZREFNR2 { get; set; }

			public string ZZREFNR3 { get; set; }

			public string ZZREFNR4 { get; set; }

			public string KREISKZ { get; set; }

			public string FEINSTAUBAMT { get; set; }

			public string RESWUNSCH { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZKENN { get; set; }

			public string VK_KUERZEL { get; set; }

			public string KUNDEN_REF { get; set; }

			public string KUNDEN_NOTIZ { get; set; }

			public string BEB_STATUS { get; set; }

			public string ZULPOSNR { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public decimal? PREIS_DL { get; set; }

			public decimal? PREIS_GB { get; set; }

			public decimal? PREIS_ST { get; set; }

			public decimal? PREIS_KZ { get; set; }

			public decimal? MENGE_DL { get; set; }

			public string AH_DOKNAME { get; set; }

			public string ZZEVB { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					KUNNR = (string)row["KUNNR"],
					ZULBELN = (string)row["ZULBELN"],
					VBELN = (string)row["VBELN"],
					VE_ERDAT = string.IsNullOrEmpty(row["VE_ERDAT"].ToString()) ? null : (DateTime?)row["VE_ERDAT"],
					VE_ERZEIT = (string)row["VE_ERZEIT"],
					VE_ERNAM = (string)row["VE_ERNAM"],
					ZZREFNR1 = (string)row["ZZREFNR1"],
					ZZREFNR2 = (string)row["ZZREFNR2"],
					ZZREFNR3 = (string)row["ZZREFNR3"],
					ZZREFNR4 = (string)row["ZZREFNR4"],
					KREISKZ = (string)row["KREISKZ"],
					FEINSTAUBAMT = (string)row["FEINSTAUBAMT"],
					RESWUNSCH = (string)row["RESWUNSCH"],
					ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
					ZZKENN = (string)row["ZZKENN"],
					VK_KUERZEL = (string)row["VK_KUERZEL"],
					KUNDEN_REF = (string)row["KUNDEN_REF"],
					KUNDEN_NOTIZ = (string)row["KUNDEN_NOTIZ"],
					BEB_STATUS = (string)row["BEB_STATUS"],
					ZULPOSNR = (string)row["ZULPOSNR"],
					MATNR = (string)row["MATNR"],
					MAKTX = (string)row["MAKTX"],
					PREIS_DL = string.IsNullOrEmpty(row["PREIS_DL"].ToString()) ? null : (decimal?)row["PREIS_DL"],
					PREIS_GB = string.IsNullOrEmpty(row["PREIS_GB"].ToString()) ? null : (decimal?)row["PREIS_GB"],
					PREIS_ST = string.IsNullOrEmpty(row["PREIS_ST"].ToString()) ? null : (decimal?)row["PREIS_ST"],
					PREIS_KZ = string.IsNullOrEmpty(row["PREIS_KZ"].ToString()) ? null : (decimal?)row["PREIS_KZ"],
					MENGE_DL = string.IsNullOrEmpty(row["MENGE_DL"].ToString()) ? null : (decimal?)row["MENGE_DL"],
					AH_DOKNAME = (string)row["AH_DOKNAME"],
					ZZEVB = (string)row["ZZEVB"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_ZULLISTE", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_ZULLISTE", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_ZULLISTE.GT_KUN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_ZULLISTE.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
