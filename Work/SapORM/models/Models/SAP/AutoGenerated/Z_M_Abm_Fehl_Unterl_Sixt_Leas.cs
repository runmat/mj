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
	public partial class Z_M_ABM_FEHL_UNTERL_SIXT_LEAS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_ABM_FEHL_UNTERL_SIXT_LEAS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_ABM_FEHL_UNTERL_SIXT_LEAS).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("KUNNR", value);
		}

		public partial class AUSGABE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZKENN { get; set; }

			public string LIZNR { get; set; }

			public string TIDNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public DateTime? KZINDAT { get; set; }

			public string FREIGABE { get; set; }

			public string FEHL_AUF_ANL { get; set; }

			public string SCHILD { get; set; }

			public string SCHEIN { get; set; }

			public string ABMAUF { get; set; }

			public string CARPP { get; set; }

			public string BRIEF { get; set; }

			public static AUSGABE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new AUSGABE
				{
					ZZKENN = (string)row["ZZKENN"],
					LIZNR = (string)row["LIZNR"],
					TIDNR = (string)row["TIDNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					KZINDAT = string.IsNullOrEmpty(row["KZINDAT"].ToString()) ? null : (DateTime?)row["KZINDAT"],
					FREIGABE = (string)row["FREIGABE"],
					FEHL_AUF_ANL = (string)row["FEHL_AUF_ANL"],
					SCHILD = (string)row["SCHILD"],
					SCHEIN = (string)row["SCHEIN"],
					ABMAUF = (string)row["ABMAUF"],
					CARPP = (string)row["CARPP"],
					BRIEF = (string)row["BRIEF"],

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

			public static IEnumerable<AUSGABE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<AUSGABE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<AUSGABE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(AUSGABE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<AUSGABE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<AUSGABE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_ABM_FEHL_UNTERL_SIXT_LEAS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_ABM_FEHL_UNTERL_SIXT_LEAS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_ABM_FEHL_UNTERL_SIXT_LEAS.AUSGABE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
