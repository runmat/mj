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
	public partial class Z_M_EC_AVM_BATCH_SELECT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EC_AVM_BATCH_SELECT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EC_AVM_BATCH_SELECT).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZBATCH_ID_VON { get; set; }

			public string ZBATCH_ID_BIS { get; set; }

			public string ZUNIT_NR_VON { get; set; }

			public string ZUNIT_NR_BIS { get; set; }

			public string ZMODEL_ID_VON { get; set; }

			public string ZMODEL_ID_BIS { get; set; }

			public string ZPURCH_MTH_VON { get; set; }

			public string ZPURCH_MTH_BIS { get; set; }

			public DateTime? ERDAT_VON { get; set; }

			public DateTime? ERDAT_BIS { get; set; }

			public string ZERNAM { get; set; }

			public string ZSIPP_CODE { get; set; }

			public string ZAUFNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string ZLOEVM { get; set; }

			public static GT_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IN
				{
					ZBATCH_ID_VON = (string)row["ZBATCH_ID_VON"],
					ZBATCH_ID_BIS = (string)row["ZBATCH_ID_BIS"],
					ZUNIT_NR_VON = (string)row["ZUNIT_NR_VON"],
					ZUNIT_NR_BIS = (string)row["ZUNIT_NR_BIS"],
					ZMODEL_ID_VON = (string)row["ZMODEL_ID_VON"],
					ZMODEL_ID_BIS = (string)row["ZMODEL_ID_BIS"],
					ZPURCH_MTH_VON = (string)row["ZPURCH_MTH_VON"],
					ZPURCH_MTH_BIS = (string)row["ZPURCH_MTH_BIS"],
					ERDAT_VON = (string.IsNullOrEmpty(row["ERDAT_VON"].ToString())) ? null : (DateTime?)row["ERDAT_VON"],
					ERDAT_BIS = (string.IsNullOrEmpty(row["ERDAT_BIS"].ToString())) ? null : (DateTime?)row["ERDAT_BIS"],
					ZERNAM = (string)row["ZERNAM"],
					ZSIPP_CODE = (string)row["ZSIPP_CODE"],
					ZAUFNR = (string)row["ZAUFNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					ZLOEVM = (string)row["ZLOEVM"],

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

			public static IEnumerable<GT_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_BATCH_SELECT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_BATCH_SELECT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
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

			public string ZMODEL_ID { get; set; }

			public string ZMOD_DESCR { get; set; }

			public string ZSIPP_CODE { get; set; }

			public string ZMAKE { get; set; }

			public string ZPURCH_MTH { get; set; }

			public string ZANZAHL { get; set; }

			public string ZUNIT_NR_VON { get; set; }

			public string ZUNIT_NR_BIS { get; set; }

			public string ZFZG_GROUP { get; set; }

			public string ZSONDERSERIE { get; set; }

			public string ZNAVI { get; set; }

			public string ZAHK { get; set; }

			public string ZLAUFZEIT { get; set; }

			public string ZLZBINDUNG { get; set; }

			public string ZAUFNR_VON { get; set; }

			public string ZAUFNR_BIS { get; set; }

			public string ZMS_REIFEN { get; set; }

			public string ZSECU_FLEET { get; set; }

			public string ZLEASING { get; set; }

			public string ZVERWENDUNG { get; set; }

			public string ZBEMERKUNG { get; set; }

			public string ZERNAM { get; set; }

			public DateTime? ERDAT { get; set; }

			public string AENAM { get; set; }

			public DateTime? AEDAT { get; set; }

			public string ZLOEVM_BATCH_ID { get; set; }

			public string ZUNITVERGEBEN { get; set; }

			public string STATUS { get; set; }

			public string ANZ_FZG_ZULAUF { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					ZBATCH_ID = (string)row["ZBATCH_ID"],
					ZMODEL_ID = (string)row["ZMODEL_ID"],
					ZMOD_DESCR = (string)row["ZMOD_DESCR"],
					ZSIPP_CODE = (string)row["ZSIPP_CODE"],
					ZMAKE = (string)row["ZMAKE"],
					ZPURCH_MTH = (string)row["ZPURCH_MTH"],
					ZANZAHL = (string)row["ZANZAHL"],
					ZUNIT_NR_VON = (string)row["ZUNIT_NR_VON"],
					ZUNIT_NR_BIS = (string)row["ZUNIT_NR_BIS"],
					ZFZG_GROUP = (string)row["ZFZG_GROUP"],
					ZSONDERSERIE = (string)row["ZSONDERSERIE"],
					ZNAVI = (string)row["ZNAVI"],
					ZAHK = (string)row["ZAHK"],
					ZLAUFZEIT = (string)row["ZLAUFZEIT"],
					ZLZBINDUNG = (string)row["ZLZBINDUNG"],
					ZAUFNR_VON = (string)row["ZAUFNR_VON"],
					ZAUFNR_BIS = (string)row["ZAUFNR_BIS"],
					ZMS_REIFEN = (string)row["ZMS_REIFEN"],
					ZSECU_FLEET = (string)row["ZSECU_FLEET"],
					ZLEASING = (string)row["ZLEASING"],
					ZVERWENDUNG = (string)row["ZVERWENDUNG"],
					ZBEMERKUNG = (string)row["ZBEMERKUNG"],
					ZERNAM = (string)row["ZERNAM"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					AENAM = (string)row["AENAM"],
					AEDAT = (string.IsNullOrEmpty(row["AEDAT"].ToString())) ? null : (DateTime?)row["AEDAT"],
					ZLOEVM_BATCH_ID = (string)row["ZLOEVM_BATCH_ID"],
					ZUNITVERGEBEN = (string)row["ZUNITVERGEBEN"],
					STATUS = (string)row["STATUS"],
					ANZ_FZG_ZULAUF = (string)row["ANZ_FZG_ZULAUF"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_BATCH_SELECT", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_BATCH_SELECT", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_BATCH_SELECT.GT_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_EC_AVM_BATCH_SELECT.GT_IN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_BATCH_SELECT.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_EC_AVM_BATCH_SELECT.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
