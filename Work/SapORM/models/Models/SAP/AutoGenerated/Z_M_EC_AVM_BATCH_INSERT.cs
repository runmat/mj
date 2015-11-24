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
	public partial class Z_M_EC_AVM_BATCH_INSERT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EC_AVM_BATCH_INSERT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EC_AVM_BATCH_INSERT).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_WEB_USER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("WEB_USER", value);
		}

		public partial class ZBATCH_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string ZUNIT_NR { get; set; }

			public string ZPZ_UNIT { get; set; }

			public string ZBATCH_ID { get; set; }

			public string ZMODEL_ID { get; set; }

			public string ZSIPP_CODE { get; set; }

			public string ZMAKE { get; set; }

			public string ZMOD_DESCR { get; set; }

			public string ZPURCH_MTH { get; set; }

			public string ZANZAHL { get; set; }

			public string ZUNIT_NR_VON { get; set; }

			public string ZPF_UNIT_VON { get; set; }

			public string ZUNIT_NR_BIS { get; set; }

			public string ZPF_UNIT_BIS { get; set; }

			public string ZFZG_GROUP { get; set; }

			public string ZLAUFZEIT { get; set; }

			public string ZLZBINDUNG { get; set; }

			public string ZAUFNR_VON { get; set; }

			public string ZAUFNR_BIS { get; set; }

			public string ZMS_REIFEN { get; set; }

			public string ZSECU_FLEET { get; set; }

			public string ZVERWENDUNG { get; set; }

			public string ZBEMERKUNG { get; set; }

			public string ZLOEVM { get; set; }

			public string ZERNAM { get; set; }

			public string ZEQUNR { get; set; }

			public DateTime? ZGW_DATUM { get; set; }

			public string ZGW_ZEIT { get; set; }

			public DateTime? ZVERGDAT { get; set; }

			public DateTime? ZGW_DATUM_ZUL { get; set; }

			public string ZGW_ZEIT_ZUL { get; set; }

			public string ZLEASING { get; set; }

			public string ZVERGZEIT { get; set; }

			public string ZUSER_SPERR { get; set; }

			public DateTime? ZDAT_SPERR { get; set; }

			public string ZBEM_SPERR { get; set; }

			public string ZSONDERSERIE { get; set; }

			public string ZNAVI { get; set; }

			public string ZAHK { get; set; }

			public DateTime? DAT_UMSETZ { get; set; }

			public DateTime? ERDAT { get; set; }

			public string AENAM { get; set; }

			public DateTime? AEDAT { get; set; }

			public string BLUETOOTH { get; set; }

			public string ANTR { get; set; }

			public DateTime? ZFREISETZUNG { get; set; }

			public DateTime? ZGW_DAT_FREIS { get; set; }

			public string ZGW_ZEIT_FREIS { get; set; }

			public DateTime? ZUKZ { get; set; }

			public DateTime? ZGW_DAT_UKZ { get; set; }

			public string ZGW_ZEIT_UKZ { get; set; }

			public static ZBATCH_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ZBATCH_IN
				{
					MANDT = (string)row["MANDT"],
					ZUNIT_NR = (string)row["ZUNIT_NR"],
					ZPZ_UNIT = (string)row["ZPZ_UNIT"],
					ZBATCH_ID = (string)row["ZBATCH_ID"],
					ZMODEL_ID = (string)row["ZMODEL_ID"],
					ZSIPP_CODE = (string)row["ZSIPP_CODE"],
					ZMAKE = (string)row["ZMAKE"],
					ZMOD_DESCR = (string)row["ZMOD_DESCR"],
					ZPURCH_MTH = (string)row["ZPURCH_MTH"],
					ZANZAHL = (string)row["ZANZAHL"],
					ZUNIT_NR_VON = (string)row["ZUNIT_NR_VON"],
					ZPF_UNIT_VON = (string)row["ZPF_UNIT_VON"],
					ZUNIT_NR_BIS = (string)row["ZUNIT_NR_BIS"],
					ZPF_UNIT_BIS = (string)row["ZPF_UNIT_BIS"],
					ZFZG_GROUP = (string)row["ZFZG_GROUP"],
					ZLAUFZEIT = (string)row["ZLAUFZEIT"],
					ZLZBINDUNG = (string)row["ZLZBINDUNG"],
					ZAUFNR_VON = (string)row["ZAUFNR_VON"],
					ZAUFNR_BIS = (string)row["ZAUFNR_BIS"],
					ZMS_REIFEN = (string)row["ZMS_REIFEN"],
					ZSECU_FLEET = (string)row["ZSECU_FLEET"],
					ZVERWENDUNG = (string)row["ZVERWENDUNG"],
					ZBEMERKUNG = (string)row["ZBEMERKUNG"],
					ZLOEVM = (string)row["ZLOEVM"],
					ZERNAM = (string)row["ZERNAM"],
					ZEQUNR = (string)row["ZEQUNR"],
					ZGW_DATUM = string.IsNullOrEmpty(row["ZGW_DATUM"].ToString()) ? null : (DateTime?)row["ZGW_DATUM"],
					ZGW_ZEIT = (string)row["ZGW_ZEIT"],
					ZVERGDAT = string.IsNullOrEmpty(row["ZVERGDAT"].ToString()) ? null : (DateTime?)row["ZVERGDAT"],
					ZGW_DATUM_ZUL = string.IsNullOrEmpty(row["ZGW_DATUM_ZUL"].ToString()) ? null : (DateTime?)row["ZGW_DATUM_ZUL"],
					ZGW_ZEIT_ZUL = (string)row["ZGW_ZEIT_ZUL"],
					ZLEASING = (string)row["ZLEASING"],
					ZVERGZEIT = (string)row["ZVERGZEIT"],
					ZUSER_SPERR = (string)row["ZUSER_SPERR"],
					ZDAT_SPERR = string.IsNullOrEmpty(row["ZDAT_SPERR"].ToString()) ? null : (DateTime?)row["ZDAT_SPERR"],
					ZBEM_SPERR = (string)row["ZBEM_SPERR"],
					ZSONDERSERIE = (string)row["ZSONDERSERIE"],
					ZNAVI = (string)row["ZNAVI"],
					ZAHK = (string)row["ZAHK"],
					DAT_UMSETZ = string.IsNullOrEmpty(row["DAT_UMSETZ"].ToString()) ? null : (DateTime?)row["DAT_UMSETZ"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					AENAM = (string)row["AENAM"],
					AEDAT = string.IsNullOrEmpty(row["AEDAT"].ToString()) ? null : (DateTime?)row["AEDAT"],
					BLUETOOTH = (string)row["BLUETOOTH"],
					ANTR = (string)row["ANTR"],
					ZFREISETZUNG = string.IsNullOrEmpty(row["ZFREISETZUNG"].ToString()) ? null : (DateTime?)row["ZFREISETZUNG"],
					ZGW_DAT_FREIS = string.IsNullOrEmpty(row["ZGW_DAT_FREIS"].ToString()) ? null : (DateTime?)row["ZGW_DAT_FREIS"],
					ZGW_ZEIT_FREIS = (string)row["ZGW_ZEIT_FREIS"],
					ZUKZ = string.IsNullOrEmpty(row["ZUKZ"].ToString()) ? null : (DateTime?)row["ZUKZ"],
					ZGW_DAT_UKZ = string.IsNullOrEmpty(row["ZGW_DAT_UKZ"].ToString()) ? null : (DateTime?)row["ZGW_DAT_UKZ"],
					ZGW_ZEIT_UKZ = (string)row["ZGW_ZEIT_UKZ"],

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

			public static IEnumerable<ZBATCH_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static IEnumerable<ZBATCH_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ZBATCH_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ZBATCH_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ZBATCH_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_BATCH_INSERT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ZBATCH_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ZBATCH_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZUNIT_NR { get; set; }

			public static GT_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IN
				{
					ZUNIT_NR = (string)row["ZUNIT_NR"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_BATCH_INSERT", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_BATCH_INSERT", inputParameterKeys, inputParameterValues);
				 
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
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_BATCH_INSERT.ZBATCH_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_BATCH_INSERT.GT_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
