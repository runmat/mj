using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_EXP_GBA_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_EXP_GBA_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_EXP_GBA_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VBELN { get; set; }

			public DateTime? VDATU { get; set; }

			public string ZVERT_ART { get; set; }

			public string KONTONR_C { get; set; }

			public string CIN_C { get; set; }

			public string PAID_C { get; set; }

			public string NAME_KRED { get; set; }

			public DateTime? ZZBISDATUM { get; set; }

			public string NAME_KRED_BEST { get; set; }

			public string MAKTX { get; set; }

			public DateTime? DAT_RE_EING { get; set; }

			public string NETWR_C { get; set; }

			public string GEBUEHREN_C { get; set; }

			public string BEM_ZEILE1 { get; set; }

			public string BEM_ZEILE2 { get; set; }

			public string ZZPROTOKOLL { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					VBELN = (string)row["VBELN"],
					VDATU = (string.IsNullOrEmpty(row["VDATU"].ToString())) ? null : (DateTime?)row["VDATU"],
					ZVERT_ART = (string)row["ZVERT_ART"],
					KONTONR_C = (string)row["KONTONR_C"],
					CIN_C = (string)row["CIN_C"],
					PAID_C = (string)row["PAID_C"],
					NAME_KRED = (string)row["NAME_KRED"],
					ZZBISDATUM = (string.IsNullOrEmpty(row["ZZBISDATUM"].ToString())) ? null : (DateTime?)row["ZZBISDATUM"],
					NAME_KRED_BEST = (string)row["NAME_KRED_BEST"],
					MAKTX = (string)row["MAKTX"],
					DAT_RE_EING = (string.IsNullOrEmpty(row["DAT_RE_EING"].ToString())) ? null : (DateTime?)row["DAT_RE_EING"],
					NETWR_C = (string)row["NETWR_C"],
					GEBUEHREN_C = (string)row["GEBUEHREN_C"],
					BEM_ZEILE1 = (string)row["BEM_ZEILE1"],
					BEM_ZEILE2 = (string)row["BEM_ZEILE2"],
					ZZPROTOKOLL = (string)row["ZZPROTOKOLL"],

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

			public static IEnumerable<GT_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EXP_GBA_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EXP_GBA_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_EXP_GBA_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_EXP_GBA_01.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
