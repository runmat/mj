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
	public partial class Z_M_EC_AVM_BATCH_UNIT_SELECT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EC_AVM_BATCH_UNIT_SELECT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EC_AVM_BATCH_UNIT_SELECT).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_BATCH_ID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BATCH_ID", value);
		}

		public static void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZBATCH_ID { get; set; }

			public string ZUNIT_NR { get; set; }

			public string ZEQUNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string TIDNR { get; set; }

			public DateTime? ERDAT_EQUI { get; set; }

			public string LIZNR { get; set; }

			public DateTime? ZZDAT_EIN { get; set; }

			public DateTime? ZZDAT_BER { get; set; }

			public string LICENSE_NUM { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public string ZLOEVM { get; set; }

			public string BLUETOOTH { get; set; }

			public string ANTR { get; set; }

			public string ZBEM_SPERR { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					ZBATCH_ID = (string)row["ZBATCH_ID"],
					ZUNIT_NR = (string)row["ZUNIT_NR"],
					ZEQUNR = (string)row["ZEQUNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					TIDNR = (string)row["TIDNR"],
					ERDAT_EQUI = string.IsNullOrEmpty(row["ERDAT_EQUI"].ToString()) ? null : (DateTime?)row["ERDAT_EQUI"],
					LIZNR = (string)row["LIZNR"],
					ZZDAT_EIN = string.IsNullOrEmpty(row["ZZDAT_EIN"].ToString()) ? null : (DateTime?)row["ZZDAT_EIN"],
					ZZDAT_BER = string.IsNullOrEmpty(row["ZZDAT_BER"].ToString()) ? null : (DateTime?)row["ZZDAT_BER"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
					ZLOEVM = (string)row["ZLOEVM"],
					BLUETOOTH = (string)row["BLUETOOTH"],
					ANTR = (string)row["ANTR"],
					ZBEM_SPERR = (string)row["ZBEM_SPERR"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_BATCH_UNIT_SELECT", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_BATCH_UNIT_SELECT", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_BATCH_UNIT_SELECT.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
