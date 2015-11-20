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
	public partial class Z_ZLD_AH_2015_ETIKETT_SEL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_AH_2015_ETIKETT_SEL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_AH_2015_ETIKETT_SEL).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_DELTA_LISTE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_DELTA_LISTE", value);
		}

		public static void SetImportParameter_I_ID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ID", value);
		}

		public static void SetImportParameter_I_KENNZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KENNZ", value);
		}

		public static void SetImportParameter_I_ZLDAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZLDAT", value);
		}

		public partial class ET_BAK : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZKENN { get; set; }

			public string ZZREFNR1 { get; set; }

			public string FZGTYP { get; set; }

			public string FARBE { get; set; }

			public string ZZREFNR2 { get; set; }

			public string KUNNR { get; set; }

			public string NAME { get; set; }

			public string ZULBELN { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public static ET_BAK Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ET_BAK
				{
					ZZKENN = (string)row["ZZKENN"],
					ZZREFNR1 = (string)row["ZZREFNR1"],
					FZGTYP = (string)row["FZGTYP"],
					FARBE = (string)row["FARBE"],
					ZZREFNR2 = (string)row["ZZREFNR2"],
					KUNNR = (string)row["KUNNR"],
					NAME = (string)row["NAME"],
					ZULBELN = (string)row["ZULBELN"],
					ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],

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

			public static IEnumerable<ET_BAK> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_BAK> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ET_BAK> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_BAK).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_BAK> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ET_BAK> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_BAK> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_BAK>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_2015_ETIKETT_SEL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_BAK> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_BAK>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_BAK> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_BAK>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_BAK> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_BAK>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_2015_ETIKETT_SEL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_BAK> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_BAK>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_2015_ETIKETT_SEL.ET_BAK> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
