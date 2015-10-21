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
	public partial class Z_ZLD_PP_GET_PO_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_PP_GET_PO_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_PP_GET_PO_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_BESTELLUNGEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EBELN { get; set; }

			public string EBELP { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public DateTime? EINDT { get; set; }

			public string ZH_NAME1 { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZBRIEF { get; set; }

			public string KREISKZ { get; set; }

			public string ZZKENN { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string VBELN { get; set; }

			public string VBELP { get; set; }

			public decimal? GEBUEHR { get; set; }

			public decimal? DL_PREIS { get; set; }

			public string PP_STATUS { get; set; }

			public string GEB_RELEVANT { get; set; }

			public string HERK { get; set; }

			public string GEB_EBELP { get; set; }

			public string MESSAGE { get; set; }

			public static GT_BESTELLUNGEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_BESTELLUNGEN
				{
					EBELN = (string)row["EBELN"],
					EBELP = (string)row["EBELP"],
					MATNR = (string)row["MATNR"],
					MAKTX = (string)row["MAKTX"],
					EINDT = (string.IsNullOrEmpty(row["EINDT"].ToString())) ? null : (DateTime?)row["EINDT"],
					ZH_NAME1 = (string)row["ZH_NAME1"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					KREISKZ = (string)row["KREISKZ"],
					ZZKENN = (string)row["ZZKENN"],
					ZZZLDAT = (string.IsNullOrEmpty(row["ZZZLDAT"].ToString())) ? null : (DateTime?)row["ZZZLDAT"],
					VBELN = (string)row["VBELN"],
					VBELP = (string)row["VBELP"],
					GEBUEHR = (decimal?)row["GEBUEHR"],
					DL_PREIS = (decimal?)row["DL_PREIS"],
					PP_STATUS = (string)row["PP_STATUS"],
					GEB_RELEVANT = (string)row["GEB_RELEVANT"],
					HERK = (string)row["HERK"],
					GEB_EBELP = (string)row["GEB_EBELP"],
					MESSAGE = (string)row["MESSAGE"],

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

			public static IEnumerable<GT_BESTELLUNGEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BESTELLUNGEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BESTELLUNGEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BESTELLUNGEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BESTELLUNGEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BESTELLUNGEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_PP_GET_PO_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_PP_GET_PO_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BESTELLUNGEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BESTELLUNGEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_PP_GET_PO_01.GT_BESTELLUNGEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_PP_GET_PO_01.GT_BESTELLUNGEN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
