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
	public partial class Z_DPM_UF_EQUI_SUCHE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_UF_EQUI_SUCHE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_UF_EQUI_SUCHE).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public static void SetImportParameter_I_LICENSE_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LICENSE_NUM", value);
		}

		public static void SetImportParameter_I_TIDNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TIDNR", value);
		}

		public static void SetImportParameter_I_VORG_ART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORG_ART", value);
		}

		public static void SetImportParameter_I_ZZREFERENZ1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFERENZ1", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_EQUIS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string TIDNR { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public string ZZBEZEI { get; set; }

			public static GT_EQUIS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EQUIS
				{
					EQUNR = (string)row["EQUNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					TIDNR = (string)row["TIDNR"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					ZZBEZEI = (string)row["ZZBEZEI"],

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

			public static IEnumerable<GT_EQUIS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EQUIS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EQUIS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EQUIS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EQUIS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EQUIS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_UF_EQUI_SUCHE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_UF_EQUI_SUCHE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EQUIS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EQUIS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_UF_EQUI_SUCHE.GT_EQUIS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
