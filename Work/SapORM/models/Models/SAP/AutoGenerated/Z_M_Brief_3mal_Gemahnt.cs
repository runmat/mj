using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_Brief_3mal_Gemahnt
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_Brief_3mal_Gemahnt).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_Brief_3mal_Gemahnt).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class EXP_BRIEFE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string CITY1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string LIZNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string KNRZE { get; set; }

			public string ERNAM { get; set; }

			public string ZZVGRUND { get; set; }

			public string ZZVGRUND_TXT { get; set; }

			public string ZZFINART_TXT { get; set; }

			public string EQUNR { get; set; }

			public static EXP_BRIEFE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new EXP_BRIEFE
				{
					KUNNR = (string)row["KUNNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ZZTMPDT = (string.IsNullOrEmpty(row["ZZTMPDT"].ToString())) ? null : (DateTime?)row["ZZTMPDT"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					CITY1 = (string)row["CITY1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					LIZNR = (string)row["LIZNR"],
					ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
					KNRZE = (string)row["KNRZE"],
					ERNAM = (string)row["ERNAM"],
					ZZVGRUND = (string)row["ZZVGRUND"],
					ZZVGRUND_TXT = (string)row["ZZVGRUND_TXT"],
					ZZFINART_TXT = (string)row["ZZFINART_TXT"],
					EQUNR = (string)row["EQUNR"],

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

			public static IEnumerable<EXP_BRIEFE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<EXP_BRIEFE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<EXP_BRIEFE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(EXP_BRIEFE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<EXP_BRIEFE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<EXP_BRIEFE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<EXP_BRIEFE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EXP_BRIEFE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_Brief_3mal_Gemahnt", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EXP_BRIEFE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EXP_BRIEFE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EXP_BRIEFE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EXP_BRIEFE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EXP_BRIEFE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EXP_BRIEFE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_Brief_3mal_Gemahnt", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<EXP_BRIEFE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EXP_BRIEFE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_TEXT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string TDLINE { get; set; }

			public static GT_TEXT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TEXT
				{
					EQUNR = (string)row["EQUNR"],
					TDLINE = (string)row["TDLINE"],

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

			public static IEnumerable<GT_TEXT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TEXT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_TEXT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TEXT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TEXT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_TEXT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TEXT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_Brief_3mal_Gemahnt", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_TEXT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_TEXT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_TEXT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_Brief_3mal_Gemahnt", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_TEXT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TEXT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_Brief_3mal_Gemahnt.EXP_BRIEFE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_Brief_3mal_Gemahnt.EXP_BRIEFE> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_M_Brief_3mal_Gemahnt.GT_TEXT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_Brief_3mal_Gemahnt.GT_TEXT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
