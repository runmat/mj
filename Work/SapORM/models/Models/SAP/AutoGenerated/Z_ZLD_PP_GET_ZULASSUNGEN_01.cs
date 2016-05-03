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
	public partial class Z_ZLD_PP_GET_ZULASSUNGEN_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_PP_GET_ZULASSUNGEN_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_PP_GET_ZULASSUNGEN_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AUSWAHL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AUSWAHL", value);
		}

		public static void SetImportParameter_I_HALTER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_HALTER", value);
		}

		public static void SetImportParameter_I_KUNDE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNDE", value);
		}

		public static void SetImportParameter_I_LIFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIFNR", value);
		}

		public static void SetImportParameter_I_ZZKENN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZKENN", value);
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

		public partial class GT_BESTELL_LISTE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNDE { get; set; }

			public string EBELN_SORT { get; set; }

			public string EBELP_SORT { get; set; }

			public string EBELN { get; set; }

			public string EBELP { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public DateTime? EINDT { get; set; }

			public string ZH_NAME1 { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZBRIEF { get; set; }

			public string KREISKZ { get; set; }

			public string ZZKENN { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public decimal? GEBUEHR { get; set; }

			public decimal? DL_PREIS { get; set; }

			public string PP_STATUS { get; set; }

			public string GEB_RELEVANT { get; set; }

			public string HERK { get; set; }

			public string ABRECHNUNG_ERSTELLT { get; set; }

			public string EXPRESS { get; set; }

			public string VBELN { get; set; }

			public string VBELP { get; set; }

			public string BUKRS { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_BESTELL_LISTE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_BESTELL_LISTE o;

				try
				{
					o = new GT_BESTELL_LISTE
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KUNDE = (string)row["KUNDE"],
						EBELN_SORT = (string)row["EBELN_SORT"],
						EBELP_SORT = (string)row["EBELP_SORT"],
						EBELN = (string)row["EBELN"],
						EBELP = (string)row["EBELP"],
						MATNR = (string)row["MATNR"],
						MAKTX = (string)row["MAKTX"],
						EINDT = string.IsNullOrEmpty(row["EINDT"].ToString()) ? null : (DateTime?)row["EINDT"],
						ZH_NAME1 = (string)row["ZH_NAME1"],
						ZZFAHRG = (string)row["ZZFAHRG"],
						ZZBRIEF = (string)row["ZZBRIEF"],
						KREISKZ = (string)row["KREISKZ"],
						ZZKENN = (string)row["ZZKENN"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						GEBUEHR = string.IsNullOrEmpty(row["GEBUEHR"].ToString()) ? null : (decimal?)row["GEBUEHR"],
						DL_PREIS = string.IsNullOrEmpty(row["DL_PREIS"].ToString()) ? null : (decimal?)row["DL_PREIS"],
						PP_STATUS = (string)row["PP_STATUS"],
						GEB_RELEVANT = (string)row["GEB_RELEVANT"],
						HERK = (string)row["HERK"],
						ABRECHNUNG_ERSTELLT = (string)row["ABRECHNUNG_ERSTELLT"],
						EXPRESS = (string)row["EXPRESS"],
						VBELN = (string)row["VBELN"],
						VBELP = (string)row["VBELP"],
						BUKRS = (string)row["BUKRS"],
					};
				}
				catch(Exception e)
				{
					o = new GT_BESTELL_LISTE
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,
					};
					o.OnMappingError(e, row, true);
					if (!o.MappingErrorProcessed)
						throw;
				}

				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnMappingError(Exception e, DataRow row, bool isExport);

			partial void OnInitFromExtern();

			public void OnModelMappingApplied()
			{
				OnInitFromExtern();
			}

			public static IEnumerable<GT_BESTELL_LISTE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BESTELL_LISTE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BESTELL_LISTE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BESTELL_LISTE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BESTELL_LISTE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELL_LISTE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BESTELL_LISTE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELL_LISTE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_PP_GET_ZULASSUNGEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELL_LISTE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELL_LISTE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELL_LISTE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELL_LISTE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELL_LISTE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELL_LISTE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_PP_GET_ZULASSUNGEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELL_LISTE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELL_LISTE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_PP_GET_ZULASSUNGEN_01.GT_BESTELL_LISTE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
