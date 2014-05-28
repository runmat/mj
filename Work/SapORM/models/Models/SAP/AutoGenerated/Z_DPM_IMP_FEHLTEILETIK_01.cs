using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_IMP_FEHLTEILETIK_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_IMP_FEHLTEILETIK_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_IMP_FEHLTEILETIK_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_DATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string UEBERSCHRIFT_1 { get; set; }

			public string INHALT_1 { get; set; }

			public string UEBERSCHRIFT_2 { get; set; }

			public string INHALT_2 { get; set; }

			public string UEBERSCHRIFT_3 { get; set; }

			public string INHALT_3 { get; set; }

			public string UEBERSCHRIFT_4 { get; set; }

			public string INHALT_4 { get; set; }

			public string UEBERSCHRIFT_5 { get; set; }

			public string INHALT_5 { get; set; }

			public string UEBERSCHRIFT_6 { get; set; }

			public string INHALT_6 { get; set; }

			public string UEBERSCHRIFT_7 { get; set; }

			public string INHALT_7 { get; set; }

			public string UEBERSCHRIFT_8 { get; set; }

			public string INHALT_8 { get; set; }

			public string UEBERSCHRIFT_9 { get; set; }

			public string INHALT_9 { get; set; }

			public string UEBERSCHRIFT_10 { get; set; }

			public string INHALT_10 { get; set; }

			public static GT_DATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DATEN
				{
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					UEBERSCHRIFT_1 = (string)row["UEBERSCHRIFT_1"],
					INHALT_1 = (string)row["INHALT_1"],
					UEBERSCHRIFT_2 = (string)row["UEBERSCHRIFT_2"],
					INHALT_2 = (string)row["INHALT_2"],
					UEBERSCHRIFT_3 = (string)row["UEBERSCHRIFT_3"],
					INHALT_3 = (string)row["INHALT_3"],
					UEBERSCHRIFT_4 = (string)row["UEBERSCHRIFT_4"],
					INHALT_4 = (string)row["INHALT_4"],
					UEBERSCHRIFT_5 = (string)row["UEBERSCHRIFT_5"],
					INHALT_5 = (string)row["INHALT_5"],
					UEBERSCHRIFT_6 = (string)row["UEBERSCHRIFT_6"],
					INHALT_6 = (string)row["INHALT_6"],
					UEBERSCHRIFT_7 = (string)row["UEBERSCHRIFT_7"],
					INHALT_7 = (string)row["INHALT_7"],
					UEBERSCHRIFT_8 = (string)row["UEBERSCHRIFT_8"],
					INHALT_8 = (string)row["INHALT_8"],
					UEBERSCHRIFT_9 = (string)row["UEBERSCHRIFT_9"],
					INHALT_9 = (string)row["INHALT_9"],
					UEBERSCHRIFT_10 = (string)row["UEBERSCHRIFT_10"],
					INHALT_10 = (string)row["INHALT_10"],

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

			public static IEnumerable<GT_DATEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DATEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_DATEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DATEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DATEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_DATEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DATEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_IMP_FEHLTEILETIK_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DATEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DATEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_IMP_FEHLTEILETIK_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_IMP_FEHLTEILETIK_01.GT_DATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_IMP_FEHLTEILETIK_01.GT_DATEN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
