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
	public partial class Z_M_EC_AVM_HERST_VWZWECK_MODID
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EC_AVM_HERST_VWZWECK_MODID).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EC_AVM_HERST_VWZWECK_MODID).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_HERST : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VALPOS { get; set; }

			public string ZHERST { get; set; }

			public static GT_HERST Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_HERST
				{
					VALPOS = (string)row["VALPOS"],
					ZHERST = (string)row["ZHERST"],

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

			public static IEnumerable<GT_HERST> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_HERST> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_HERST> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_HERST).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_HERST> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_HERST> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_HERST> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_HERST>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_HERST_VWZWECK_MODID", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_HERST> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_HERST>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_HERST> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_HERST>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_HERST> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_HERST>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_HERST_VWZWECK_MODID", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_HERST> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_HERST>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_VERW : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string DOMVALUE_L { get; set; }

			public string ZVERWENDUNG { get; set; }

			public static GT_VERW Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_VERW
				{
					DOMVALUE_L = (string)row["DOMVALUE_L"],
					ZVERWENDUNG = (string)row["ZVERWENDUNG"],

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

			public static IEnumerable<GT_VERW> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_VERW> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_VERW> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_VERW).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_VERW> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERW> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_VERW> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VERW>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_HERST_VWZWECK_MODID", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERW> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VERW>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERW> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VERW>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERW> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VERW>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_HERST_VWZWECK_MODID", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VERW> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VERW>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_MODELID : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string CODE { get; set; }

			public string KUNNR { get; set; }

			public string BEZEI { get; set; }

			public string SIPP1 { get; set; }

			public string SIPP2 { get; set; }

			public string SIPP3 { get; set; }

			public string SIPP4 { get; set; }

			public string HERST { get; set; }

			public string ANZTUEREN { get; set; }

			public string BEMER { get; set; }

			public string AUSF { get; set; }

			public string ANTR { get; set; }

			public string REIFEN { get; set; }

			public string NAVI { get; set; }

			public string MODEL_CODE { get; set; }

			public string ZLAUFZEIT { get; set; }

			public string ZLZBINDUNG { get; set; }

			public DateTime? AENDAT { get; set; }

			public string AENUS { get; set; }

			public string LAENGE { get; set; }

			public string HOEHE { get; set; }

			public string BREITE { get; set; }

			public string DURCHFAHRTHOEHE { get; set; }

			public string MODEL { get; set; }

			public string LKW { get; set; }

			public string WINTERREIFEN { get; set; }

			public string AHK { get; set; }

			public string NAVI_VORH { get; set; }

			public string SECU_FLEET { get; set; }

			public string LEASING { get; set; }

			public string BLUETOOTH { get; set; }

			public static GT_MODELID Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_MODELID
				{
					MANDT = (string)row["MANDT"],
					CODE = (string)row["CODE"],
					KUNNR = (string)row["KUNNR"],
					BEZEI = (string)row["BEZEI"],
					SIPP1 = (string)row["SIPP1"],
					SIPP2 = (string)row["SIPP2"],
					SIPP3 = (string)row["SIPP3"],
					SIPP4 = (string)row["SIPP4"],
					HERST = (string)row["HERST"],
					ANZTUEREN = (string)row["ANZTUEREN"],
					BEMER = (string)row["BEMER"],
					AUSF = (string)row["AUSF"],
					ANTR = (string)row["ANTR"],
					REIFEN = (string)row["REIFEN"],
					NAVI = (string)row["NAVI"],
					MODEL_CODE = (string)row["MODEL_CODE"],
					ZLAUFZEIT = (string)row["ZLAUFZEIT"],
					ZLZBINDUNG = (string)row["ZLZBINDUNG"],
					AENDAT = (string.IsNullOrEmpty(row["AENDAT"].ToString())) ? null : (DateTime?)row["AENDAT"],
					AENUS = (string)row["AENUS"],
					LAENGE = (string)row["LAENGE"],
					HOEHE = (string)row["HOEHE"],
					BREITE = (string)row["BREITE"],
					DURCHFAHRTHOEHE = (string)row["DURCHFAHRTHOEHE"],
					MODEL = (string)row["MODEL"],
					LKW = (string)row["LKW"],
					WINTERREIFEN = (string)row["WINTERREIFEN"],
					AHK = (string)row["AHK"],
					NAVI_VORH = (string)row["NAVI_VORH"],
					SECU_FLEET = (string)row["SECU_FLEET"],
					LEASING = (string)row["LEASING"],
					BLUETOOTH = (string)row["BLUETOOTH"],

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

			public static IEnumerable<GT_MODELID> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_MODELID> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_MODELID> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_MODELID).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_MODELID> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_MODELID> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_MODELID> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_MODELID>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_HERST_VWZWECK_MODID", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_MODELID> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_MODELID>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_MODELID> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_MODELID>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_MODELID> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_MODELID>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_HERST_VWZWECK_MODID", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_MODELID> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_MODELID>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_HERST_VWZWECK_MODID.GT_HERST> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_EC_AVM_HERST_VWZWECK_MODID.GT_HERST> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_HERST_VWZWECK_MODID.GT_VERW> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_EC_AVM_HERST_VWZWECK_MODID.GT_VERW> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_HERST_VWZWECK_MODID.GT_MODELID> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_EC_AVM_HERST_VWZWECK_MODID.GT_MODELID> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
