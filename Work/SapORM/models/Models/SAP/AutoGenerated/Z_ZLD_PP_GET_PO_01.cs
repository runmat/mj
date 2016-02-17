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
	public partial class Z_ZLD_PP_GET_PO_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_PP_GET_PO_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_PP_GET_PO_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_LIFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIFNR", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_BESTELLUNGEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EBELN { get; set; }

			public string EBELP { get; set; }

			public string HAUPT_POSITION { get; set; }

			public string LIFNR { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public DateTime? EINDT { get; set; }

			public string EKORG { get; set; }

			public string BUKRS { get; set; }

			public string WERKS { get; set; }

			public string LGORT { get; set; }

			public string BEZ_WERK_LGORT { get; set; }

			public string ZH_NAME1 { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZBRIEF { get; set; }

			public string KREISKZ { get; set; }

			public string ZZKENN { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string VBELN { get; set; }

			public string VBELP { get; set; }

			public string KUNNR { get; set; }

			public decimal? GEBUEHR { get; set; }

			public decimal? DL_PREIS { get; set; }

			public string PP_STATUS { get; set; }

			public string GEB_RELEVANT { get; set; }

			public string HERK { get; set; }

			public string GEB_EBELP { get; set; }

			public string MESSAGE { get; set; }

			public string TELF1 { get; set; }

			public string SMTP_ADDR { get; set; }

			public string GRUND_KEY { get; set; }

			public string LTEXT_NR { get; set; }

			public string ERNAM { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_BESTELLUNGEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_BESTELLUNGEN o;

				try
				{
					o = new GT_BESTELLUNGEN
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						EBELN = (string)row["EBELN"],
						EBELP = (string)row["EBELP"],
						HAUPT_POSITION = (string)row["HAUPT_POSITION"],
						LIFNR = (string)row["LIFNR"],
						MATNR = (string)row["MATNR"],
						MAKTX = (string)row["MAKTX"],
						EINDT = string.IsNullOrEmpty(row["EINDT"].ToString()) ? null : (DateTime?)row["EINDT"],
						EKORG = (string)row["EKORG"],
						BUKRS = (string)row["BUKRS"],
						WERKS = (string)row["WERKS"],
						LGORT = (string)row["LGORT"],
						BEZ_WERK_LGORT = (string)row["BEZ_WERK_LGORT"],
						ZH_NAME1 = (string)row["ZH_NAME1"],
						ZZFAHRG = (string)row["ZZFAHRG"],
						ZZBRIEF = (string)row["ZZBRIEF"],
						KREISKZ = (string)row["KREISKZ"],
						ZZKENN = (string)row["ZZKENN"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						VBELN = (string)row["VBELN"],
						VBELP = (string)row["VBELP"],
						KUNNR = (string)row["KUNNR"],
						GEBUEHR = string.IsNullOrEmpty(row["GEBUEHR"].ToString()) ? null : (decimal?)row["GEBUEHR"],
						DL_PREIS = string.IsNullOrEmpty(row["DL_PREIS"].ToString()) ? null : (decimal?)row["DL_PREIS"],
						PP_STATUS = (string)row["PP_STATUS"],
						GEB_RELEVANT = (string)row["GEB_RELEVANT"],
						HERK = (string)row["HERK"],
						GEB_EBELP = (string)row["GEB_EBELP"],
						MESSAGE = (string)row["MESSAGE"],
						TELF1 = (string)row["TELF1"],
						SMTP_ADDR = (string)row["SMTP_ADDR"],
						GRUND_KEY = (string)row["GRUND_KEY"],
						LTEXT_NR = (string)row["LTEXT_NR"],
						ERNAM = (string)row["ERNAM"],
					};
				}
				catch(Exception e)
				{
					o = new GT_BESTELLUNGEN
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

			public static IEnumerable<GT_BESTELLUNGEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BESTELLUNGEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BESTELLUNGEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BESTELLUNGEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BESTELLUNGEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BESTELLUNGEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_PP_GET_PO_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_PP_GET_PO_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_PP_GET_PO_01.GT_BESTELLUNGEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
