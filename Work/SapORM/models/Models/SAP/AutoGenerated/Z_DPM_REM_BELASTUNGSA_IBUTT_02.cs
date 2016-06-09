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
	public partial class Z_DPM_REM_BELASTUNGSA_IBUTT_02
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_REM_BELASTUNGSA_IBUTT_02).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_REM_BELASTUNGSA_IBUTT_02).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_FIN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN", value);
		}

		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
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

			public string FAHRGNR { get; set; }

			public string LFDNR { get; set; }

			public DateTime? INDATUM { get; set; }

			public string GUTA { get; set; }

			public string GUTAID { get; set; }

			public DateTime? GUTADAT { get; set; }

			public string SCHADKZ { get; set; }

			public decimal? SCHADBETR { get; set; }

			public decimal? SCHADBETR_AV { get; set; }

			public string REPKZ { get; set; }

			public decimal? AUFBETR { get; set; }

			public decimal? AUFBETR_AV { get; set; }

			public decimal? WRTMBETR { get; set; }

			public decimal? WRTMBETR_AV { get; set; }

			public decimal? FEHLTBETR { get; set; }

			public decimal? FEHLTBETR_AV { get; set; }

			public decimal? RESTWERT { get; set; }

			public decimal? MAEWERT_AV { get; set; }

			public decimal? OPTWRTAV { get; set; }

			public decimal? MINWERT_AV { get; set; }

			public decimal? BESCHAED_AV { get; set; }

			public decimal? VORSCHAED_REP_AV { get; set; }

			public decimal? SCHAED_UNREP_AV { get; set; }

			public decimal? VORSCHAED_WERTMIND { get; set; }

			public decimal? SCHAED_UNREP_WMIND { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_OUT o;

				try
				{
					o = new GT_OUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FAHRGNR = (string)row["FAHRGNR"],
						LFDNR = (string)row["LFDNR"],
						INDATUM = string.IsNullOrEmpty(row["INDATUM"].ToString()) ? null : (DateTime?)row["INDATUM"],
						GUTA = (string)row["GUTA"],
						GUTAID = (string)row["GUTAID"],
						GUTADAT = string.IsNullOrEmpty(row["GUTADAT"].ToString()) ? null : (DateTime?)row["GUTADAT"],
						SCHADKZ = (string)row["SCHADKZ"],
						SCHADBETR = string.IsNullOrEmpty(row["SCHADBETR"].ToString()) ? null : (decimal?)row["SCHADBETR"],
						SCHADBETR_AV = string.IsNullOrEmpty(row["SCHADBETR_AV"].ToString()) ? null : (decimal?)row["SCHADBETR_AV"],
						REPKZ = (string)row["REPKZ"],
						AUFBETR = string.IsNullOrEmpty(row["AUFBETR"].ToString()) ? null : (decimal?)row["AUFBETR"],
						AUFBETR_AV = string.IsNullOrEmpty(row["AUFBETR_AV"].ToString()) ? null : (decimal?)row["AUFBETR_AV"],
						WRTMBETR = string.IsNullOrEmpty(row["WRTMBETR"].ToString()) ? null : (decimal?)row["WRTMBETR"],
						WRTMBETR_AV = string.IsNullOrEmpty(row["WRTMBETR_AV"].ToString()) ? null : (decimal?)row["WRTMBETR_AV"],
						FEHLTBETR = string.IsNullOrEmpty(row["FEHLTBETR"].ToString()) ? null : (decimal?)row["FEHLTBETR"],
						FEHLTBETR_AV = string.IsNullOrEmpty(row["FEHLTBETR_AV"].ToString()) ? null : (decimal?)row["FEHLTBETR_AV"],
						RESTWERT = string.IsNullOrEmpty(row["RESTWERT"].ToString()) ? null : (decimal?)row["RESTWERT"],
						MAEWERT_AV = string.IsNullOrEmpty(row["MAEWERT_AV"].ToString()) ? null : (decimal?)row["MAEWERT_AV"],
						OPTWRTAV = string.IsNullOrEmpty(row["OPTWRTAV"].ToString()) ? null : (decimal?)row["OPTWRTAV"],
						MINWERT_AV = string.IsNullOrEmpty(row["MINWERT_AV"].ToString()) ? null : (decimal?)row["MINWERT_AV"],
						BESCHAED_AV = string.IsNullOrEmpty(row["BESCHAED_AV"].ToString()) ? null : (decimal?)row["BESCHAED_AV"],
						VORSCHAED_REP_AV = string.IsNullOrEmpty(row["VORSCHAED_REP_AV"].ToString()) ? null : (decimal?)row["VORSCHAED_REP_AV"],
						SCHAED_UNREP_AV = string.IsNullOrEmpty(row["SCHAED_UNREP_AV"].ToString()) ? null : (decimal?)row["SCHAED_UNREP_AV"],
						VORSCHAED_WERTMIND = string.IsNullOrEmpty(row["VORSCHAED_WERTMIND"].ToString()) ? null : (decimal?)row["VORSCHAED_WERTMIND"],
						SCHAED_UNREP_WMIND = string.IsNullOrEmpty(row["SCHAED_UNREP_WMIND"].ToString()) ? null : (decimal?)row["SCHAED_UNREP_WMIND"],
					};
				}
				catch(Exception e)
				{
					o = new GT_OUT
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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_BELASTUNGSA_IBUTT_02", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_BELASTUNGSA_IBUTT_02", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_BELASTUNGSA_IBUTT_02.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
