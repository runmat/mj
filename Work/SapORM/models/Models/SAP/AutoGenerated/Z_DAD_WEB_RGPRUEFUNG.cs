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
	public partial class Z_DAD_WEB_RGPRUEFUNG
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DAD_WEB_RGPRUEFUNG).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DAD_WEB_RGPRUEFUNG).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AUSLIDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_AUSLIDAT_BIS", value);
		}

		public void SetImportParameter_I_AUSLIDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_AUSLIDAT_VON", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_LIZNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIZNR", value);
		}

		public void SetImportParameter_I_RECHNUNGSNUMMERN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_RECHNUNGSNUMMERN", value);
		}

		public void SetImportParameter_I_ZZREFERENZ1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFERENZ1", value);
		}

		public partial class GT_RECHNUNGEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZMODID { get; set; }

			public DateTime? ZZABRDT { get; set; }

			public DateTime? ZZFAEDT { get; set; }

			public DateTime? AULDT { get; set; }

			public string LIZNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public static GT_RECHNUNGEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_RECHNUNGEN
				{
					ZZMODID = (string)row["ZZMODID"],
					ZZABRDT = string.IsNullOrEmpty(row["ZZABRDT"].ToString()) ? null : (DateTime?)row["ZZABRDT"],
					ZZFAEDT = string.IsNullOrEmpty(row["ZZFAEDT"].ToString()) ? null : (DateTime?)row["ZZFAEDT"],
					AULDT = string.IsNullOrEmpty(row["AULDT"].ToString()) ? null : (DateTime?)row["AULDT"],
					LIZNR = (string)row["LIZNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],

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

			public static IEnumerable<GT_RECHNUNGEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_RECHNUNGEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_RECHNUNGEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_RECHNUNGEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_RECHNUNGEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNUNGEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_RECHNUNGEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNUNGEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DAD_WEB_RGPRUEFUNG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNUNGEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNUNGEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNUNGEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNUNGEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNUNGEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNUNGEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DAD_WEB_RGPRUEFUNG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_RECHNUNGEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_RECHNUNGEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DAD_WEB_RGPRUEFUNG.GT_RECHNUNGEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
