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
	public partial class Z_DPM_DRUCK_FEHLTEILETIK
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_DRUCK_FEHLTEILETIK).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_DRUCK_FEHLTEILETIK).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_POSITION(ISapDataService sap, int? value)
		{
			sap.SetImportParameter("I_POSITION", value);
		}

		public static void SetImportParameter_I_PREVIEW(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PREVIEW", value);
		}

		public static void SetImportParameter_I_VERART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VERART", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static byte[] GetExportParameter_E_PDF(ISapDataService sap)
		{
			return sap.GetExportParameter<byte[]>("E_PDF");
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_ETIKETT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

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

			public static GT_ETIKETT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ETIKETT
				{
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
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

			public static IEnumerable<GT_ETIKETT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ETIKETT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ETIKETT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ETIKETT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ETIKETT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ETIKETT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ETIKETT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ETIKETT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_DRUCK_FEHLTEILETIK", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ETIKETT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ETIKETT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ETIKETT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ETIKETT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ETIKETT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ETIKETT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_DRUCK_FEHLTEILETIK", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ETIKETT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ETIKETT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
