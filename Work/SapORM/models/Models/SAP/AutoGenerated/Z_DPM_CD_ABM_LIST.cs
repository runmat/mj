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
	public partial class Z_DPM_CD_ABM_LIST
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_CD_ABM_LIST).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_CD_ABM_LIST).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_ABTEILUNG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ABTEILUNG", value);
		}

		public static void SetImportParameter_I_ABT_LEITER_NAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ABT_LEITER_NAME", value);
		}

		public static void SetImportParameter_I_ABT_LEITER_VNAME(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ABT_LEITER_VNAME", value);
		}

		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_BETRIEB(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BETRIEB", value);
		}

		public static void SetImportParameter_I_DAT_ABM_AUFTR_AB(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DAT_ABM_AUFTR_AB", value);
		}

		public static void SetImportParameter_I_DAT_ABM_AUFTR_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DAT_ABM_AUFTR_BIS", value);
		}

		public static void SetImportParameter_I_EXPIRY_DATE_AB(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_EXPIRY_DATE_AB", value);
		}

		public static void SetImportParameter_I_EXPIRY_DATE_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_EXPIRY_DATE_BIS", value);
		}

		public static void SetImportParameter_I_FIN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN", value);
		}

		public static void SetImportParameter_I_FIN_10(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN_10", value);
		}

		public static void SetImportParameter_I_KOSTST(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KOSTST", value);
		}

		public static void SetImportParameter_I_STATUS_NUR_KF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STATUS_NUR_KF", value);
		}

		public static void SetImportParameter_I_ZIELORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZIELORT", value);
		}

		public partial class ET_ABM_LIST : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN { get; set; }

			public string FIN_10 { get; set; }

			public string STATUS { get; set; }

			public DateTime? DAT_ABM_AUFTR { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public string ZIELORT { get; set; }

			public string BETRIEB { get; set; }

			public string KOSTST { get; set; }

			public string ABTEILUNG { get; set; }

			public string ABT_LEITER_NAME { get; set; }

			public string ABT_LEITER_VNAME { get; set; }

			public string TODO_TXT { get; set; }

			public string BEMERKUNG_TXT { get; set; }

			public string STATUS_BEZ { get; set; }

			public DateTime? DAT_ABM_REP { get; set; }

			public string ORT_ABM_REP { get; set; }

			public string MODELL { get; set; }

			public string KM { get; set; }

			public static ET_ABM_LIST Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ET_ABM_LIST
				{
					FIN = (string)row["FIN"],
					FIN_10 = (string)row["FIN_10"],
					STATUS = (string)row["STATUS"],
					DAT_ABM_AUFTR = string.IsNullOrEmpty(row["DAT_ABM_AUFTR"].ToString()) ? null : (DateTime?)row["DAT_ABM_AUFTR"],
					EXPIRY_DATE = string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString()) ? null : (DateTime?)row["EXPIRY_DATE"],
					ZIELORT = (string)row["ZIELORT"],
					BETRIEB = (string)row["BETRIEB"],
					KOSTST = (string)row["KOSTST"],
					ABTEILUNG = (string)row["ABTEILUNG"],
					ABT_LEITER_NAME = (string)row["ABT_LEITER_NAME"],
					ABT_LEITER_VNAME = (string)row["ABT_LEITER_VNAME"],
					TODO_TXT = (string)row["TODO_TXT"],
					BEMERKUNG_TXT = (string)row["BEMERKUNG_TXT"],
					STATUS_BEZ = (string)row["STATUS_BEZ"],
					DAT_ABM_REP = string.IsNullOrEmpty(row["DAT_ABM_REP"].ToString()) ? null : (DateTime?)row["DAT_ABM_REP"],
					ORT_ABM_REP = (string)row["ORT_ABM_REP"],
					MODELL = (string)row["MODELL"],
					KM = (string)row["KM"],

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

			public static IEnumerable<ET_ABM_LIST> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_ABM_LIST> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ET_ABM_LIST> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_ABM_LIST).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_ABM_LIST> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ET_ABM_LIST> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_ABM_LIST> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_LIST>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_CD_ABM_LIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ABM_LIST> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_LIST>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ABM_LIST> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_LIST>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ABM_LIST> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_LIST>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_CD_ABM_LIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ABM_LIST> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ABM_LIST>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class IT_STATUS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string STATUS { get; set; }

			public static IT_STATUS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new IT_STATUS
				{
					STATUS = (string)row["STATUS"],

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

			public static IEnumerable<IT_STATUS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<IT_STATUS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<IT_STATUS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(IT_STATUS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<IT_STATUS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<IT_STATUS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<IT_STATUS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IT_STATUS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_CD_ABM_LIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IT_STATUS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_STATUS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IT_STATUS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_STATUS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IT_STATUS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IT_STATUS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_CD_ABM_LIST", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IT_STATUS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_STATUS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_CD_ABM_LIST.ET_ABM_LIST> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_CD_ABM_LIST.IT_STATUS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
