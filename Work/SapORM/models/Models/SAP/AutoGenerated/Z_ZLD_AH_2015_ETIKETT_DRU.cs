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
	public partial class Z_ZLD_AH_2015_ETIKETT_DRU
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_AH_2015_ETIKETT_DRU).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_AH_2015_ETIKETT_DRU).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_COMMIT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_COMMIT", value);
		}

		public static void SetImportParameter_I_DRUCK_KENNZEICHNEN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_DRUCK_KENNZEICHNEN", value);
		}

		public static void SetImportParameter_I_DRU_POS_SPALTE(ISapDataService sap, int? value)
		{
			sap.SetImportParameter("I_DRU_POS_SPALTE", value);
		}

		public static void SetImportParameter_I_DRU_POS_ZEILE(ISapDataService sap, int? value)
		{
			sap.SetImportParameter("I_DRU_POS_ZEILE", value);
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

		public partial class IT_BELN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public static IT_BELN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new IT_BELN
				{
					ZULBELN = (string)row["ZULBELN"],

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

			public static IEnumerable<IT_BELN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<IT_BELN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<IT_BELN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(IT_BELN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<IT_BELN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<IT_BELN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<IT_BELN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IT_BELN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_2015_ETIKETT_DRU", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IT_BELN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_BELN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IT_BELN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_BELN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IT_BELN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IT_BELN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_2015_ETIKETT_DRU", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IT_BELN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_BELN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_2015_ETIKETT_DRU.IT_BELN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
