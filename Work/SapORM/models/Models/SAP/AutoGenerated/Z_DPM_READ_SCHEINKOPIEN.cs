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
	public partial class Z_DPM_READ_SCHEINKOPIEN
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_SCHEINKOPIEN).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_SCHEINKOPIEN).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_DATUM_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM_BIS", value);
		}

		public static void SetImportParameter_I_DATUM_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM_VON", value);
		}

		public static void SetImportParameter_I_KLAERFALL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KLAERFALL", value);
		}

		public static void SetImportParameter_I_KUNDE_OTHER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNDE_OTHER", value);
		}

		public static void SetImportParameter_I_KUNDE_PG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNDE_PG", value);
		}

		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public DateTime? ERDAT { get; set; }

			public string LIZNR { get; set; }

			public string EQUNR { get; set; }

			public string LICENSE_NUM { get; set; }

			public string CHASSIS_NUM { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public string KOPIE_FLG { get; set; }

			public string HALTERNAME { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					KUNNR = (string)row["KUNNR"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					LIZNR = (string)row["LIZNR"],
					EQUNR = (string)row["EQUNR"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					KOPIE_FLG = (string)row["KOPIE_FLG"],
					HALTERNAME = (string)row["HALTERNAME"],

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
				return Select(dt, sapConnection).ToListOrEmptyList();
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
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_SCHEINKOPIEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_SCHEINKOPIEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_SCHEINKOPIEN.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
