using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_Blocken_Farben
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_Blocken_Farben).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_Blocken_Farben).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class FARBE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string DOMNAME { get; set; }

			public string DDLANGUAGE { get; set; }

			public string AS4LOCAL { get; set; }

			public string VALPOS { get; set; }

			public string AS4VERS { get; set; }

			public string DDTEXT { get; set; }

			public string DOMVAL_LD { get; set; }

			public string DOMVAL_HD { get; set; }

			public string DOMVALUE_L { get; set; }

			public static FARBE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new FARBE
				{
					DOMNAME = (string)row["DOMNAME"],
					DDLANGUAGE = (string)row["DDLANGUAGE"],
					AS4LOCAL = (string)row["AS4LOCAL"],
					VALPOS = (string)row["VALPOS"],
					AS4VERS = (string)row["AS4VERS"],
					DDTEXT = (string)row["DDTEXT"],
					DOMVAL_LD = (string)row["DOMVAL_LD"],
					DOMVAL_HD = (string)row["DOMVAL_HD"],
					DOMVALUE_L = (string)row["DOMVALUE_L"],

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

			public static IEnumerable<FARBE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<FARBE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<FARBE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(FARBE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<FARBE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<FARBE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<FARBE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<FARBE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_Blocken_Farben", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<FARBE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<FARBE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<FARBE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<FARBE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<FARBE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<FARBE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_Blocken_Farben", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<FARBE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<FARBE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_Blocken_Farben.FARBE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_Blocken_Farben.FARBE> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
