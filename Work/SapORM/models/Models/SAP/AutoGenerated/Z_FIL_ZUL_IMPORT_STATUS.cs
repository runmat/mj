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
	public partial class Z_FIL_ZUL_IMPORT_STATUS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_FIL_ZUL_IMPORT_STATUS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_FIL_ZUL_IMPORT_STATUS).Name, inputParameterKeys, inputParameterValues);
		}


		public partial class GT_ZFIL_ZUL_K : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ID { get; set; }

			public string VKBUR { get; set; }

			public string KUNNR { get; set; }

			public string NAME1 { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string REFERENZ { get; set; }

			public string ZZKENN { get; set; }

			public string ORDERID { get; set; }

			public string HPPOS { get; set; }

			public string STATUS { get; set; }

			public string POSNR { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public decimal? GEBUEHR { get; set; }

			public string GEB_POS { get; set; }

			public static GT_ZFIL_ZUL_K Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ZFIL_ZUL_K
				{
					ID = (string)row["ID"],
					VKBUR = (string)row["VKBUR"],
					KUNNR = (string)row["KUNNR"],
					NAME1 = (string)row["NAME1"],
					ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
					REFERENZ = (string)row["REFERENZ"],
					ZZKENN = (string)row["ZZKENN"],
					ORDERID = (string)row["ORDERID"],
					HPPOS = (string)row["HPPOS"],
					STATUS = (string)row["STATUS"],
					POSNR = (string)row["POSNR"],
					MATNR = (string)row["MATNR"],
					MAKTX = (string)row["MAKTX"],
					GEBUEHR = string.IsNullOrEmpty(row["GEBUEHR"].ToString()) ? null : (decimal?)row["GEBUEHR"],
					GEB_POS = (string)row["GEB_POS"],

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

			public static IEnumerable<GT_ZFIL_ZUL_K> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ZFIL_ZUL_K> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ZFIL_ZUL_K> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ZFIL_ZUL_K).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ZFIL_ZUL_K> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZFIL_ZUL_K> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ZFIL_ZUL_K> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ZFIL_ZUL_K>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_FIL_ZUL_IMPORT_STATUS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZFIL_ZUL_K> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ZFIL_ZUL_K>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZFIL_ZUL_K> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ZFIL_ZUL_K>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZFIL_ZUL_K> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ZFIL_ZUL_K>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_FIL_ZUL_IMPORT_STATUS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZFIL_ZUL_K> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ZFIL_ZUL_K>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_FIL_ZUL_IMPORT_STATUS.GT_ZFIL_ZUL_K> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
