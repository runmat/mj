using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_READ_EQUI_003
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_EQUI_003).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_EQUI_003).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string EQTYP { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LIZNR { get; set; }

			public string TIDNR { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZHERST_TEXT { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ABCKZ { get; set; }

			public string MSGRP { get; set; }

			public string ZZMASSEFAHRBMAX { get; set; }

			public string ZZZULGESGEW { get; set; }

			public string ZZFAHRZEUGKLASSE { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					EQUNR = (string)row["EQUNR"],
					EQTYP = (string)row["EQTYP"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					LIZNR = (string)row["LIZNR"],
					TIDNR = (string)row["TIDNR"],
					ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
					ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
					ZZHERST_TEXT = (string)row["ZZHERST_TEXT"],
					ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
					ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
					ZZTMPDT = (string.IsNullOrEmpty(row["ZZTMPDT"].ToString())) ? null : (DateTime?)row["ZZTMPDT"],
					EXPIRY_DATE = (string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString())) ? null : (DateTime?)row["EXPIRY_DATE"],
					REPLA_DATE = (string.IsNullOrEmpty(row["REPLA_DATE"].ToString())) ? null : (DateTime?)row["REPLA_DATE"],
					ZZZLDAT = (string.IsNullOrEmpty(row["ZZZLDAT"].ToString())) ? null : (DateTime?)row["ZZZLDAT"],
					ABCKZ = (string)row["ABCKZ"],
					MSGRP = (string)row["MSGRP"],
					ZZMASSEFAHRBMAX = (string)row["ZZMASSEFAHRBMAX"],
					ZZZULGESGEW = (string)row["ZZZULGESGEW"],
					ZZFAHRZEUGKLASSE = (string)row["ZZFAHRZEUGKLASSE"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_EQUI_003", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_EQUI_003", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_EQUI_003.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_READ_EQUI_003.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
