using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_IMP_AUFTRDAT_007
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_IMP_AUFTRDAT_007).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_IMP_AUFTRDAT_007).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string KUNNR { get; set; }

			public string KENNUNG { get; set; }

			public string POS_KURZTEXT { get; set; }

			public string POS_TEXT { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STRAS { get; set; }

			public string PSTLZ { get; set; }

			public string ORT01 { get; set; }

			public string EMAIL { get; set; }

			public string LAND1 { get; set; }

			public string TELNR { get; set; }

			public string FAXNR { get; set; }

			public string INTNR { get; set; }

			public string SAPNR { get; set; }

			public string LOEVM { get; set; }

			public DateTime? AENDT { get; set; }

			public string AENUS { get; set; }

			public string AD_PRUEF { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					MANDT = (string)row["MANDT"],
					KUNNR = (string)row["KUNNR"],
					KENNUNG = (string)row["KENNUNG"],
					POS_KURZTEXT = (string)row["POS_KURZTEXT"],
					POS_TEXT = (string)row["POS_TEXT"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STRAS = (string)row["STRAS"],
					PSTLZ = (string)row["PSTLZ"],
					ORT01 = (string)row["ORT01"],
					EMAIL = (string)row["EMAIL"],
					LAND1 = (string)row["LAND1"],
					TELNR = (string)row["TELNR"],
					FAXNR = (string)row["FAXNR"],
					INTNR = (string)row["INTNR"],
					SAPNR = (string)row["SAPNR"],
					LOEVM = (string)row["LOEVM"],
					AENDT = (string.IsNullOrEmpty(row["AENDT"].ToString())) ? null : (DateTime?)row["AENDT"],
					AENUS = (string)row["AENUS"],
					AD_PRUEF = (string)row["AD_PRUEF"],

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

			public static IEnumerable<GT_WEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_WEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_IMP_AUFTRDAT_007", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_IMP_AUFTRDAT_007", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_IMP_AUFTRDAT_007.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_IMP_AUFTRDAT_007.GT_WEB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
