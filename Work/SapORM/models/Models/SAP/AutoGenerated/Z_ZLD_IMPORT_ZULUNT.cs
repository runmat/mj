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
	public partial class Z_ZLD_IMPORT_ZULUNT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_IMPORT_ZULUNT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_IMPORT_ZULUNT).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GS_WEB : IModelMappingApplied
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

			public DateTime? AEDAT { get; set; }

			public string AENAM { get; set; }

			public string PZUL_BRIEF { get; set; }

			public string PUMSCHR_BRIEF { get; set; }

			public string PUMK_BRIEF { get; set; }

			public string PERS_BRIEF { get; set; }

			public string UZUL_BRIEF { get; set; }

			public string UUMSCHR_BRIEF { get; set; }

			public string UUMK_BRIEF { get; set; }

			public string UERS_BRIEF { get; set; }

			public string PZUL_SCHEIN { get; set; }

			public string PUMSCHR_SCHEIN { get; set; }

			public string PUMK_SCHEIN { get; set; }

			public string PERS_SCHEIN { get; set; }

			public string UZUL_SCHEIN { get; set; }

			public string UUMSCHR_SCHEIN { get; set; }

			public string UUMK_SCHEIN { get; set; }

			public string UERS_SCHEIN { get; set; }

			public string PZUL_COC { get; set; }

			public string PUMSCHR_COC { get; set; }

			public string PUMK_COC { get; set; }

			public string PERS_COC { get; set; }

			public string UZUL_COC { get; set; }

			public string UUMSCHR_COC { get; set; }

			public string UUMK_COC { get; set; }

			public string UERS_COC { get; set; }

			public string PZUL_DECK { get; set; }

			public string PUMSCHR_DECK { get; set; }

			public string PUMK_DECK { get; set; }

			public string PERS_DECK { get; set; }

			public string UZUL_DECK { get; set; }

			public string UUMSCHR_DECK { get; set; }

			public string UUMK_DECK { get; set; }

			public string UERS_DECK { get; set; }

			public string PZUL_VOLLM { get; set; }

			public string PUMSCHR_VOLLM { get; set; }

			public string PUMK_VOLLM { get; set; }

			public string PERS_VOLLM { get; set; }

			public string UZUL_VOLLM { get; set; }

			public string UUMSCHR_VOLLM { get; set; }

			public string UUMK_VOLLM { get; set; }

			public string UERS_VOLLM { get; set; }

			public string PZUL_AUSW { get; set; }

			public string PUMSCHR_AUSW { get; set; }

			public string PUMK_AUSW { get; set; }

			public string PERS_AUSW { get; set; }

			public string UZUL_AUSW { get; set; }

			public string UUMSCHR_AUSW { get; set; }

			public string UUMK_AUSW { get; set; }

			public string UERS_AUSW { get; set; }

			public string PZUL_GEWERB { get; set; }

			public string PUMSCHR_GEWERB { get; set; }

			public string PUMK_GEWERB { get; set; }

			public string PERS_GEWERB { get; set; }

			public string UZUL_GEWERB { get; set; }

			public string UUMSCHR_GEWERB { get; set; }

			public string UUMK_GEWERB { get; set; }

			public string UERS_GEWERB { get; set; }

			public string PZUL_HANDEL { get; set; }

			public string PUMSCHR_HANDEL { get; set; }

			public string PUMK_HANDEL { get; set; }

			public string PERS_HANDEL { get; set; }

			public string UZUL_HANDEL { get; set; }

			public string UUMSCHR_HANDEL { get; set; }

			public string UUMK_HANDEL { get; set; }

			public string UERS_HANDEL { get; set; }

			public string PZUL_LAST { get; set; }

			public string PUMSCHR_LAST { get; set; }

			public string PUMK_LAST { get; set; }

			public string PERS_LAST { get; set; }

			public string UZUL_LAST { get; set; }

			public string UUMSCHR_LAST { get; set; }

			public string UUMK_LAST { get; set; }

			public string UERS_LAST { get; set; }

			public string PZUL_BEM { get; set; }

			public string PUMSCHR_BEM { get; set; }

			public string PUMK_BEM { get; set; }

			public string PERS_BEM { get; set; }

			public string UZUL_BEM { get; set; }

			public string UUMSCHR_BEM { get; set; }

			public string UUMK_BEM { get; set; }

			public string UERS_BEM { get; set; }

			public string ZKFZKZ { get; set; }

			public string STVALN { get; set; }

			public string STVALNFORM { get; set; }

			public string STVALNGEB { get; set; }

			public string URL { get; set; }

			public static GS_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GS_WEB
				{
					MANDT = (string)row["MANDT"],
					ZKBA1 = (string)row["ZKBA1"],
					ZKBA2 = (string)row["ZKBA2"],
					AEDAT = (string.IsNullOrEmpty(row["AEDAT"].ToString())) ? null : (DateTime?)row["AEDAT"],
					AENAM = (string)row["AENAM"],
					PZUL_BRIEF = (string)row["PZUL_BRIEF"],
					PUMSCHR_BRIEF = (string)row["PUMSCHR_BRIEF"],
					PUMK_BRIEF = (string)row["PUMK_BRIEF"],
					PERS_BRIEF = (string)row["PERS_BRIEF"],
					UZUL_BRIEF = (string)row["UZUL_BRIEF"],
					UUMSCHR_BRIEF = (string)row["UUMSCHR_BRIEF"],
					UUMK_BRIEF = (string)row["UUMK_BRIEF"],
					UERS_BRIEF = (string)row["UERS_BRIEF"],
					PZUL_SCHEIN = (string)row["PZUL_SCHEIN"],
					PUMSCHR_SCHEIN = (string)row["PUMSCHR_SCHEIN"],
					PUMK_SCHEIN = (string)row["PUMK_SCHEIN"],
					PERS_SCHEIN = (string)row["PERS_SCHEIN"],
					UZUL_SCHEIN = (string)row["UZUL_SCHEIN"],
					UUMSCHR_SCHEIN = (string)row["UUMSCHR_SCHEIN"],
					UUMK_SCHEIN = (string)row["UUMK_SCHEIN"],
					UERS_SCHEIN = (string)row["UERS_SCHEIN"],
					PZUL_COC = (string)row["PZUL_COC"],
					PUMSCHR_COC = (string)row["PUMSCHR_COC"],
					PUMK_COC = (string)row["PUMK_COC"],
					PERS_COC = (string)row["PERS_COC"],
					UZUL_COC = (string)row["UZUL_COC"],
					UUMSCHR_COC = (string)row["UUMSCHR_COC"],
					UUMK_COC = (string)row["UUMK_COC"],
					UERS_COC = (string)row["UERS_COC"],
					PZUL_DECK = (string)row["PZUL_DECK"],
					PUMSCHR_DECK = (string)row["PUMSCHR_DECK"],
					PUMK_DECK = (string)row["PUMK_DECK"],
					PERS_DECK = (string)row["PERS_DECK"],
					UZUL_DECK = (string)row["UZUL_DECK"],
					UUMSCHR_DECK = (string)row["UUMSCHR_DECK"],
					UUMK_DECK = (string)row["UUMK_DECK"],
					UERS_DECK = (string)row["UERS_DECK"],
					PZUL_VOLLM = (string)row["PZUL_VOLLM"],
					PUMSCHR_VOLLM = (string)row["PUMSCHR_VOLLM"],
					PUMK_VOLLM = (string)row["PUMK_VOLLM"],
					PERS_VOLLM = (string)row["PERS_VOLLM"],
					UZUL_VOLLM = (string)row["UZUL_VOLLM"],
					UUMSCHR_VOLLM = (string)row["UUMSCHR_VOLLM"],
					UUMK_VOLLM = (string)row["UUMK_VOLLM"],
					UERS_VOLLM = (string)row["UERS_VOLLM"],
					PZUL_AUSW = (string)row["PZUL_AUSW"],
					PUMSCHR_AUSW = (string)row["PUMSCHR_AUSW"],
					PUMK_AUSW = (string)row["PUMK_AUSW"],
					PERS_AUSW = (string)row["PERS_AUSW"],
					UZUL_AUSW = (string)row["UZUL_AUSW"],
					UUMSCHR_AUSW = (string)row["UUMSCHR_AUSW"],
					UUMK_AUSW = (string)row["UUMK_AUSW"],
					UERS_AUSW = (string)row["UERS_AUSW"],
					PZUL_GEWERB = (string)row["PZUL_GEWERB"],
					PUMSCHR_GEWERB = (string)row["PUMSCHR_GEWERB"],
					PUMK_GEWERB = (string)row["PUMK_GEWERB"],
					PERS_GEWERB = (string)row["PERS_GEWERB"],
					UZUL_GEWERB = (string)row["UZUL_GEWERB"],
					UUMSCHR_GEWERB = (string)row["UUMSCHR_GEWERB"],
					UUMK_GEWERB = (string)row["UUMK_GEWERB"],
					UERS_GEWERB = (string)row["UERS_GEWERB"],
					PZUL_HANDEL = (string)row["PZUL_HANDEL"],
					PUMSCHR_HANDEL = (string)row["PUMSCHR_HANDEL"],
					PUMK_HANDEL = (string)row["PUMK_HANDEL"],
					PERS_HANDEL = (string)row["PERS_HANDEL"],
					UZUL_HANDEL = (string)row["UZUL_HANDEL"],
					UUMSCHR_HANDEL = (string)row["UUMSCHR_HANDEL"],
					UUMK_HANDEL = (string)row["UUMK_HANDEL"],
					UERS_HANDEL = (string)row["UERS_HANDEL"],
					PZUL_LAST = (string)row["PZUL_LAST"],
					PUMSCHR_LAST = (string)row["PUMSCHR_LAST"],
					PUMK_LAST = (string)row["PUMK_LAST"],
					PERS_LAST = (string)row["PERS_LAST"],
					UZUL_LAST = (string)row["UZUL_LAST"],
					UUMSCHR_LAST = (string)row["UUMSCHR_LAST"],
					UUMK_LAST = (string)row["UUMK_LAST"],
					UERS_LAST = (string)row["UERS_LAST"],
					PZUL_BEM = (string)row["PZUL_BEM"],
					PUMSCHR_BEM = (string)row["PUMSCHR_BEM"],
					PUMK_BEM = (string)row["PUMK_BEM"],
					PERS_BEM = (string)row["PERS_BEM"],
					UZUL_BEM = (string)row["UZUL_BEM"],
					UUMSCHR_BEM = (string)row["UUMSCHR_BEM"],
					UUMK_BEM = (string)row["UUMK_BEM"],
					UERS_BEM = (string)row["UERS_BEM"],
					ZKFZKZ = (string)row["ZKFZKZ"],
					STVALN = (string)row["STVALN"],
					STVALNFORM = (string)row["STVALNFORM"],
					STVALNGEB = (string)row["STVALNGEB"],
					URL = (string)row["URL"],

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

			public static IEnumerable<GS_WEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GS_WEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GS_WEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GS_WEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GS_WEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GS_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GS_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GS_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_IMPORT_ZULUNT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GS_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GS_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GS_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GS_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GS_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GS_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_IMPORT_ZULUNT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GS_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GS_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_IMPORT_ZULUNT.GS_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_IMPORT_ZULUNT.GS_WEB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
