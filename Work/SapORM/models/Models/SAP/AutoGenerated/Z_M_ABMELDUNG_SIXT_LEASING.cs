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
	public partial class Z_M_ABMELDUNG_SIXT_LEASING
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_ABMELDUNG_SIXT_LEASING).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_ABMELDUNG_SIXT_LEASING).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public partial class I_AUF : IModelMappingApplied
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

			public static I_AUF Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new I_AUF
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

			public static IEnumerable<I_AUF> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<I_AUF> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<I_AUF> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(I_AUF).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<I_AUF> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<I_AUF> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<I_AUF> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<I_AUF>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_ABMELDUNG_SIXT_LEASING", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<I_AUF> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<I_AUF>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<I_AUF> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<I_AUF>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<I_AUF> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<I_AUF>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_ABMELDUNG_SIXT_LEASING", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<I_AUF> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<I_AUF>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_ABMELDUNG_SIXT_LEASING.I_AUF> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
