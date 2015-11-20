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
	public partial class Z_GET_ZULST_BY_PLZ
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_GET_ZULST_BY_PLZ).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_GET_ZULST_BY_PLZ).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_ORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ORT", value);
		}

		public void SetImportParameter_I_PLZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PLZ", value);
		}

		public partial class T_ORTE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string ID { get; set; }

			public string PLZ { get; set; }

			public string ORTSNAME { get; set; }

			public string ORTSZUSATZ { get; set; }

			public string KGS { get; set; }

			public string KGS_KURZ { get; set; }

			public static T_ORTE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T_ORTE
				{
					MANDT = (string)row["MANDT"],
					ID = (string)row["ID"],
					PLZ = (string)row["PLZ"],
					ORTSNAME = (string)row["ORTSNAME"],
					ORTSZUSATZ = (string)row["ORTSZUSATZ"],
					KGS = (string)row["KGS"],
					KGS_KURZ = (string)row["KGS_KURZ"],

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

			public static IEnumerable<T_ORTE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T_ORTE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<T_ORTE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T_ORTE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T_ORTE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<T_ORTE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T_ORTE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_ORTE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_GET_ZULST_BY_PLZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_ORTE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_ORTE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_ORTE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_ORTE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_ORTE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_ORTE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_GET_ZULST_BY_PLZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_ORTE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_ORTE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class T_ZULST : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string ZKBA1 { get; set; }

			public string ZKBA2 { get; set; }

			public string KBANR { get; set; }

			public string AFNAM { get; set; }

			public string ZKREIS { get; set; }

			public string BLAND { get; set; }

			public string LIFNR { get; set; }

			public string ZVKGU { get; set; }

			public string ZVKGK { get; set; }

			public string ZKFZKZ { get; set; }

			public string ORT01 { get; set; }

			public string ORT02 { get; set; }

			public string PFACH { get; set; }

			public string PSTL2 { get; set; }

			public string PSTLZ { get; set; }

			public string STRAS { get; set; }

			public string TELF1 { get; set; }

			public string TELF2 { get; set; }

			public string TELFX { get; set; }

			public string ZEMAIL { get; set; }

			public string ZTXT1 { get; set; }

			public string ZTXT2 { get; set; }

			public string ZTXT3 { get; set; }

			public string ZHPAGE { get; set; }

			public decimal? ZKFZBST { get; set; }

			public decimal? ZMARKTPTL { get; set; }

			public decimal? ZABSATZ { get; set; }

			public string ZLSSTATUS { get; set; }

			public DateTime? ZLSDATUM { get; set; }

			public string LFB { get; set; }

			public string LIFNR_ABW { get; set; }

			public decimal? ZEINWOHNER { get; set; }

			public static T_ZULST Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T_ZULST
				{
					MANDT = (string)row["MANDT"],
					ZKBA1 = (string)row["ZKBA1"],
					ZKBA2 = (string)row["ZKBA2"],
					KBANR = (string)row["KBANR"],
					AFNAM = (string)row["AFNAM"],
					ZKREIS = (string)row["ZKREIS"],
					BLAND = (string)row["BLAND"],
					LIFNR = (string)row["LIFNR"],
					ZVKGU = (string)row["ZVKGU"],
					ZVKGK = (string)row["ZVKGK"],
					ZKFZKZ = (string)row["ZKFZKZ"],
					ORT01 = (string)row["ORT01"],
					ORT02 = (string)row["ORT02"],
					PFACH = (string)row["PFACH"],
					PSTL2 = (string)row["PSTL2"],
					PSTLZ = (string)row["PSTLZ"],
					STRAS = (string)row["STRAS"],
					TELF1 = (string)row["TELF1"],
					TELF2 = (string)row["TELF2"],
					TELFX = (string)row["TELFX"],
					ZEMAIL = (string)row["ZEMAIL"],
					ZTXT1 = (string)row["ZTXT1"],
					ZTXT2 = (string)row["ZTXT2"],
					ZTXT3 = (string)row["ZTXT3"],
					ZHPAGE = (string)row["ZHPAGE"],
					ZKFZBST = string.IsNullOrEmpty(row["ZKFZBST"].ToString()) ? null : (decimal?)row["ZKFZBST"],
					ZMARKTPTL = string.IsNullOrEmpty(row["ZMARKTPTL"].ToString()) ? null : (decimal?)row["ZMARKTPTL"],
					ZABSATZ = string.IsNullOrEmpty(row["ZABSATZ"].ToString()) ? null : (decimal?)row["ZABSATZ"],
					ZLSSTATUS = (string)row["ZLSSTATUS"],
					ZLSDATUM = string.IsNullOrEmpty(row["ZLSDATUM"].ToString()) ? null : (DateTime?)row["ZLSDATUM"],
					LFB = (string)row["LFB"],
					LIFNR_ABW = (string)row["LIFNR_ABW"],
					ZEINWOHNER = string.IsNullOrEmpty(row["ZEINWOHNER"].ToString()) ? null : (decimal?)row["ZEINWOHNER"],

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

			public static IEnumerable<T_ZULST> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T_ZULST> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<T_ZULST> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T_ZULST).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T_ZULST> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<T_ZULST> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T_ZULST> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_ZULST>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_GET_ZULST_BY_PLZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_ZULST> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_ZULST>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_ZULST> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_ZULST>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_ZULST> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_ZULST>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_GET_ZULST_BY_PLZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_ZULST> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_ZULST>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_GET_ZULST_BY_PLZ.T_ORTE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_GET_ZULST_BY_PLZ.T_ZULST> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
