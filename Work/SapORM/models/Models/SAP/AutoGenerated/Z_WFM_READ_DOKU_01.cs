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
	public partial class Z_WFM_READ_DOKU_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_WFM_READ_DOKU_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_WFM_READ_DOKU_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_AR_OBJECT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AR_OBJECT", value);
		}

		public static void SetImportParameter_I_DATUM_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM_BIS", value);
		}

		public static void SetImportParameter_I_DATUM_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM_VON", value);
		}

		public static void SetImportParameter_I_OBJECT_ID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_OBJECT_ID", value);
		}

		public static void SetImportParameter_I_VORG_NR_ABM_AUF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORG_NR_ABM_AUF", value);
		}

		public static byte[] GetExportParameter_E_DOC(ISapDataService sap)
		{
			return sap.GetExportParameter<byte[]>("E_DOC");
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class ES_DOKUMENT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string VORG_NR_ABM_AUF { get; set; }

			public string LFD_NR { get; set; }

			public DateTime? DATUM { get; set; }

			public string AR_OBJECT { get; set; }

			public string ARCHIV_ID { get; set; }

			public string AR_JAHR { get; set; }

			public string DATEINAME { get; set; }

			public string ERR { get; set; }

			public static ES_DOKUMENT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ES_DOKUMENT
				{
					KUNNR = (string)row["KUNNR"],
					VORG_NR_ABM_AUF = (string)row["VORG_NR_ABM_AUF"],
					LFD_NR = (string)row["LFD_NR"],
					DATUM = string.IsNullOrEmpty(row["DATUM"].ToString()) ? null : (DateTime?)row["DATUM"],
					AR_OBJECT = (string)row["AR_OBJECT"],
					ARCHIV_ID = (string)row["ARCHIV_ID"],
					AR_JAHR = (string)row["AR_JAHR"],
					DATEINAME = (string)row["DATEINAME"],
					ERR = (string)row["ERR"],

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

			public static IEnumerable<ES_DOKUMENT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ES_DOKUMENT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ES_DOKUMENT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ES_DOKUMENT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ES_DOKUMENT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ES_DOKUMENT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ES_DOKUMENT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ES_DOKUMENT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_DOKU_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_DOKUMENT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_DOKUMENT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_DOKUMENT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_DOKUMENT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_DOKU_01.ES_DOKUMENT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
