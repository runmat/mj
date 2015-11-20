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
	public partial class Z_V_UEBERF_AUFTR_PROTOKOLL_AB
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_PROTOKOLL_AB).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_V_UEBERF_AUFTR_PROTOKOLL_AB).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_ABHOL_DAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("ABHOL_DAT", value);
		}

		public static void SetImportParameter_ABHOL_ZEIT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ABHOL_ZEIT", value);
		}

		public static void SetImportParameter_AUFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("AUFNR", value);
		}

		public static void SetImportParameter_FAHRTNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("FAHRTNR", value);
		}

		public static void SetImportParameter_IUG_ZEIT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IUG_ZEIT", value);
		}

		public static void SetImportParameter_KMSTAND(ISapDataService sap, string value)
		{
			sap.SetImportParameter("KMSTAND", value);
		}

		public static void SetImportParameter_UEBERGABE_DAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("UEBERGABE_DAT", value);
		}

		public static void SetImportParameter_UNTERSCHR_VORH(ISapDataService sap, string value)
		{
			sap.SetImportParameter("UNTERSCHR_VORH", value);
		}

		public static void SetImportParameter_VERARBEITUNG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("VERARBEITUNG", value);
		}

		public static void SetImportParameter_WADAT_IST(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("WADAT_IST", value);
		}

		public static void SetImportParameter_ZZKATEGORIE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("ZZKATEGORIE", value);
		}

		public static string GetExportParameter_UPDATE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("UPDATE").NotNullOrEmpty().Trim();
		}

		public partial class GT_IMP_QM : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string GRFEHL { get; set; }

			public string COFEHL { get; set; }

			public string I_MASS_TEXT { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public static GT_IMP_QM Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IMP_QM
				{
					GRFEHL = (string)row["GRFEHL"],
					COFEHL = (string)row["COFEHL"],
					I_MASS_TEXT = (string)row["I_MASS_TEXT"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],

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

			public static IEnumerable<GT_IMP_QM> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IMP_QM> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_IMP_QM> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IMP_QM).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IMP_QM> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_QM> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IMP_QM> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_QM>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_UEBERF_AUFTR_PROTOKOLL_AB", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_QM> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_QM>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_QM> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_QM>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_QM> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_QM>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_UEBERF_AUFTR_PROTOKOLL_AB", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IMP_QM> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IMP_QM>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_V_UEBERF_AUFTR_PROTOKOLL_AB.GT_IMP_QM> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
