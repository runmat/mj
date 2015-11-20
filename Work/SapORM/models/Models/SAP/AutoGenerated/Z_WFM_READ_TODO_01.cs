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
	public partial class Z_WFM_READ_TODO_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_WFM_READ_TODO_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_WFM_READ_TODO_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public void SetImportParameter_I_ANZAHL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ANZAHL", value);
		}

		public void SetImportParameter_I_LFD_NR_BIS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LFD_NR_BIS", value);
		}

		public void SetImportParameter_I_LFD_NR_VON(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LFD_NR_VON", value);
		}

		public void SetImportParameter_I_SOLL_DATUM_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_SOLL_DATUM_BIS", value);
		}

		public void SetImportParameter_I_SOLL_DATUM_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_SOLL_DATUM_VON", value);
		}

		public void SetImportParameter_I_SOLL_ZEIT_BIS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SOLL_ZEIT_BIS", value);
		}

		public void SetImportParameter_I_SOLL_ZEIT_VON(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SOLL_ZEIT_VON", value);
		}

		public void SetImportParameter_I_STARTDATUM_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_STARTDATUM_BIS", value);
		}

		public void SetImportParameter_I_STARTDATUM_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_STARTDATUM_VON", value);
		}

		public void SetImportParameter_I_TODO_WER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TODO_WER", value);
		}

		public void SetImportParameter_I_VORG_NR_ABM_AUF_BIS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORG_NR_ABM_AUF_BIS", value);
		}

		public void SetImportParameter_I_VORG_NR_ABM_AUF_VON(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORG_NR_ABM_AUF_VON", value);
		}

		public void SetImportParameter_I_WFSTATUS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_WFSTATUS", value);
		}

		public void SetImportParameter_I_ZUSER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZUSER", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_DATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string VORG_NR_ABM_AUF { get; set; }

			public string LFD_NR { get; set; }

			public string AUFGABE { get; set; }

			public string TASK_ID { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string TODO_WER { get; set; }

			public DateTime? STARTDATUM { get; set; }

			public string STARTZEIT { get; set; }

			public DateTime? SOLL_DATUM { get; set; }

			public string SOLL_ZEIT { get; set; }

			public DateTime? IST_DATUM { get; set; }

			public string IST_ZEIT { get; set; }

			public string ZUSER { get; set; }

			public string ANMERKUNG { get; set; }

			public string STATUS { get; set; }

			public string FOLGE_TASK_ID { get; set; }

			public string AUFGABE_FOLGE_TASK { get; set; }

			public string BESTAETIGEN { get; set; }

			public string MA_AUFGABE { get; set; }

			public static GT_DATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DATEN
				{
					KUNNR = (string)row["KUNNR"],
					VORG_NR_ABM_AUF = (string)row["VORG_NR_ABM_AUF"],
					LFD_NR = (string)row["LFD_NR"],
					AUFGABE = (string)row["AUFGABE"],
					TASK_ID = (string)row["TASK_ID"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					TODO_WER = (string)row["TODO_WER"],
					STARTDATUM = string.IsNullOrEmpty(row["STARTDATUM"].ToString()) ? null : (DateTime?)row["STARTDATUM"],
					STARTZEIT = (string)row["STARTZEIT"],
					SOLL_DATUM = string.IsNullOrEmpty(row["SOLL_DATUM"].ToString()) ? null : (DateTime?)row["SOLL_DATUM"],
					SOLL_ZEIT = (string)row["SOLL_ZEIT"],
					IST_DATUM = string.IsNullOrEmpty(row["IST_DATUM"].ToString()) ? null : (DateTime?)row["IST_DATUM"],
					IST_ZEIT = (string)row["IST_ZEIT"],
					ZUSER = (string)row["ZUSER"],
					ANMERKUNG = (string)row["ANMERKUNG"],
					STATUS = (string)row["STATUS"],
					FOLGE_TASK_ID = (string)row["FOLGE_TASK_ID"],
					AUFGABE_FOLGE_TASK = (string)row["AUFGABE_FOLGE_TASK"],
					BESTAETIGEN = (string)row["BESTAETIGEN"],
					MA_AUFGABE = (string)row["MA_AUFGABE"],

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

			public static IEnumerable<GT_DATEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DATEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_DATEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DATEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DATEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DATEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_TODO_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_TODO_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_SEL : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string STATUS { get; set; }

			public static GT_SEL Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SEL
				{
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
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

			public static IEnumerable<GT_SEL> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SEL> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_SEL> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SEL).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SEL> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SEL> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_READ_TODO_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SEL>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_READ_TODO_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_SEL> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SEL>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_TODO_01.GT_DATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_WFM_READ_TODO_01.GT_SEL> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
