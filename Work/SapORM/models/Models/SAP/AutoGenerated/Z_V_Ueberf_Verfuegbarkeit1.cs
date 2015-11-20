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
	public partial class Z_V_UEBERF_VERFUEGBARKEIT1
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_V_UEBERF_VERFUEGBARKEIT1).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_V_UEBERF_VERFUEGBARKEIT1).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_BISDAT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BISDAT", value);
		}

		public void SetImportParameter_I_FAHRER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRER", value);
		}

		public void SetImportParameter_I_VONDAT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VONDAT", value);
		}

		public string GetExportParameter_O_MELDUNG(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("O_MELDUNG");
		}

		public partial class T_VERFUEG1 : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public DateTime? VERFDAT { get; set; }

			public string ANZ_FAHRER { get; set; }

			public string LIFNR { get; set; }

			public string BEMERKUNG { get; set; }

			public static T_VERFUEG1 Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T_VERFUEG1
				{
					VERFDAT = string.IsNullOrEmpty(row["VERFDAT"].ToString()) ? null : (DateTime?)row["VERFDAT"],
					ANZ_FAHRER = (string)row["ANZ_FAHRER"],
					LIFNR = (string)row["LIFNR"],
					BEMERKUNG = (string)row["BEMERKUNG"],

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

			public static IEnumerable<T_VERFUEG1> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T_VERFUEG1> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<T_VERFUEG1> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T_VERFUEG1).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T_VERFUEG1> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<T_VERFUEG1> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T_VERFUEG1> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_V_UEBERF_VERFUEGBARKEIT1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_VERFUEG1> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_VERFUEG1> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_VERFUEG1> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_V_UEBERF_VERFUEGBARKEIT1", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_VERFUEG1> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_VERFUEG1>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_V_UEBERF_VERFUEGBARKEIT1.T_VERFUEG1> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
