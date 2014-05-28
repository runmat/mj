using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_FIL_EFA_PLATARTIKEL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_FIL_EFA_PLATARTIKEL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_FIL_EFA_PLATARTIKEL).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_PLATART : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string LIFNR { get; set; }

			public string REITER { get; set; }

			public string TOPSELLER { get; set; }

			public string POS { get; set; }

			public string ARTBEZ { get; set; }

			public string ARTLIF { get; set; }

			public string MATNR { get; set; }

			public string VMEINS { get; set; }

			public string ZUSINFO { get; set; }

			public decimal? UMREZ { get; set; }

			public decimal? UMREN { get; set; }

			public string PREISPFLICHT { get; set; }

			public string TEXTPFLICHT { get; set; }

			public static GT_PLATART Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_PLATART
				{
					LIFNR = (string)row["LIFNR"],
					REITER = (string)row["REITER"],
					TOPSELLER = (string)row["TOPSELLER"],
					POS = (string)row["POS"],
					ARTBEZ = (string)row["ARTBEZ"],
					ARTLIF = (string)row["ARTLIF"],
					MATNR = (string)row["MATNR"],
					VMEINS = (string)row["VMEINS"],
					ZUSINFO = (string)row["ZUSINFO"],
					UMREZ = (decimal?)row["UMREZ"],
					UMREN = (decimal?)row["UMREN"],
					PREISPFLICHT = (string)row["PREISPFLICHT"],
					TEXTPFLICHT = (string)row["TEXTPFLICHT"],

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

			public static IEnumerable<GT_PLATART> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_PLATART> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_PLATART> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_PLATART).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_PLATART> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_PLATART> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_PLATART> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PLATART>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_FIL_EFA_PLATARTIKEL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PLATART> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PLATART>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PLATART> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PLATART>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PLATART> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PLATART>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_FIL_EFA_PLATARTIKEL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PLATART> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PLATART>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_PLATRTR : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string REITER { get; set; }

			public string BEZ { get; set; }

			public static GT_PLATRTR Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_PLATRTR
				{
					REITER = (string)row["REITER"],
					BEZ = (string)row["BEZ"],

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

			public static IEnumerable<GT_PLATRTR> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_PLATRTR> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_PLATRTR> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_PLATRTR).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_PLATRTR> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_PLATRTR> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_PLATRTR> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PLATRTR>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_FIL_EFA_PLATARTIKEL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PLATRTR> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PLATRTR>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PLATRTR> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PLATRTR>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PLATRTR> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_PLATRTR>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_FIL_EFA_PLATARTIKEL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_PLATRTR> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_PLATRTR>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_FIL_EFA_PLATARTIKEL.GT_PLATART> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_FIL_EFA_PLATARTIKEL.GT_PLATART> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_FIL_EFA_PLATARTIKEL.GT_PLATRTR> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_FIL_EFA_PLATARTIKEL.GT_PLATRTR> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
