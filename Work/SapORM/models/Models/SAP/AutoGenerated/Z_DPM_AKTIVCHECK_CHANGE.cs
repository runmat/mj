using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_AKTIVCHECK_CHANGE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_AKTIVCHECK_CHANGE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_AKTIVCHECK_CHANGE).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class IT_TREFF : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string VORGANGS_ID { get; set; }

			public string AG { get; set; }

			public string FIN { get; set; }

			public string ZB2 { get; set; }

			public string VERTRAGNR { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? PRUEFDAT { get; set; }

			public string KOLLISION { get; set; }

			public string KLASSIFIZIER { get; set; }

			public string BEARBEITET { get; set; }

			public string EQUNR { get; set; }

			public string BESTAND_AG { get; set; }

			public string TEXT { get; set; }

			public string AG_KOL { get; set; }

			public string FIN_KOL { get; set; }

			public string ZB2_KOL { get; set; }

			public string VERTRAGNR_KOL { get; set; }

			public static IT_TREFF Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new IT_TREFF
				{
					MANDT = (string)row["MANDT"],
					VORGANGS_ID = (string)row["VORGANGS_ID"],
					AG = (string)row["AG"],
					FIN = (string)row["FIN"],
					ZB2 = (string)row["ZB2"],
					VERTRAGNR = (string)row["VERTRAGNR"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					PRUEFDAT = (string.IsNullOrEmpty(row["PRUEFDAT"].ToString())) ? null : (DateTime?)row["PRUEFDAT"],
					KOLLISION = (string)row["KOLLISION"],
					KLASSIFIZIER = (string)row["KLASSIFIZIER"],
					BEARBEITET = (string)row["BEARBEITET"],
					EQUNR = (string)row["EQUNR"],
					BESTAND_AG = (string)row["BESTAND_AG"],
					TEXT = (string)row["TEXT"],
					AG_KOL = (string)row["AG_KOL"],
					FIN_KOL = (string)row["FIN_KOL"],
					ZB2_KOL = (string)row["ZB2_KOL"],
					VERTRAGNR_KOL = (string)row["VERTRAGNR_KOL"],

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

			public static IEnumerable<IT_TREFF> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<IT_TREFF> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<IT_TREFF> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(IT_TREFF).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<IT_TREFF> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<IT_TREFF> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<IT_TREFF> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IT_TREFF>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_AKTIVCHECK_CHANGE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<IT_TREFF> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_TREFF>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<IT_TREFF> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_TREFF>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<IT_TREFF> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IT_TREFF>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_AKTIVCHECK_CHANGE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<IT_TREFF> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_TREFF>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_AKTIVCHECK_CHANGE.IT_TREFF> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_AKTIVCHECK_CHANGE.IT_TREFF> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
