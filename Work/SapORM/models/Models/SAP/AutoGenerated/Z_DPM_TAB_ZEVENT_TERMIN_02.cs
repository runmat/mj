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
	public partial class Z_DPM_TAB_ZEVENT_TERMIN_02
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_TAB_ZEVENT_TERMIN_02).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_TAB_ZEVENT_TERMIN_02).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_DATUM_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM_BIS", value);
		}

		public void SetImportParameter_I_DATUM_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM_VON", value);
		}

		public void SetImportParameter_I_EVENT_ORT_BOX(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EVENT_ORT_BOX", value);
		}

		public void SetImportParameter_I_EVENT_SCHADEN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EVENT_SCHADEN", value);
		}

		public void SetImportParameter_I_EVENT_TERMIN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EVENT_TERMIN", value);
		}

		public void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public partial class GT_TERMIN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public DateTime? DATUM_VON { get; set; }

			public string ZEIT_VON { get; set; }

			public DateTime? DATUM_BIS { get; set; }

			public string ZEIT_BIS { get; set; }

			public string TEXT { get; set; }

			public DateTime? ANLAGEDATUM { get; set; }

			public string ANLAGEUSER { get; set; }

			public string EVENT_NAME { get; set; }

			public string NAME1 { get; set; }

			public string CITY1 { get; set; }

			public string BOXNAME { get; set; }

			public string VERSICH_ID { get; set; }

			public string ANDERE { get; set; }

			public string LICENSE_NUM { get; set; }

			public string NAME_FIRST { get; set; }

			public string NAME_LAST { get; set; }

			public string EVENT_TERMIN { get; set; }

			public string EVENT_ORT { get; set; }

			public string EVENT_ORTBOX { get; set; }

			public string EVENT_SCHADEN { get; set; }

			public string NACHNAME { get; set; }

			public DateTime? LOESCHDATUM { get; set; }

			public string LOESCHUSER { get; set; }

			public static GT_TERMIN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TERMIN
				{
					DATUM_VON = string.IsNullOrEmpty(row["DATUM_VON"].ToString()) ? null : (DateTime?)row["DATUM_VON"],
					ZEIT_VON = (string)row["ZEIT_VON"],
					DATUM_BIS = string.IsNullOrEmpty(row["DATUM_BIS"].ToString()) ? null : (DateTime?)row["DATUM_BIS"],
					ZEIT_BIS = (string)row["ZEIT_BIS"],
					TEXT = (string)row["TEXT"],
					ANLAGEDATUM = string.IsNullOrEmpty(row["ANLAGEDATUM"].ToString()) ? null : (DateTime?)row["ANLAGEDATUM"],
					ANLAGEUSER = (string)row["ANLAGEUSER"],
					EVENT_NAME = (string)row["EVENT_NAME"],
					NAME1 = (string)row["NAME1"],
					CITY1 = (string)row["CITY1"],
					BOXNAME = (string)row["BOXNAME"],
					VERSICH_ID = (string)row["VERSICH_ID"],
					ANDERE = (string)row["ANDERE"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					NAME_FIRST = (string)row["NAME_FIRST"],
					NAME_LAST = (string)row["NAME_LAST"],
					EVENT_TERMIN = (string)row["EVENT_TERMIN"],
					EVENT_ORT = (string)row["EVENT_ORT"],
					EVENT_ORTBOX = (string)row["EVENT_ORTBOX"],
					EVENT_SCHADEN = (string)row["EVENT_SCHADEN"],
					NACHNAME = (string)row["NACHNAME"],
					LOESCHDATUM = string.IsNullOrEmpty(row["LOESCHDATUM"].ToString()) ? null : (DateTime?)row["LOESCHDATUM"],
					LOESCHUSER = (string)row["LOESCHUSER"],

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

			public static IEnumerable<GT_TERMIN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TERMIN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TERMIN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TERMIN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TERMIN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TERMIN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TERMIN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TERMIN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_TAB_ZEVENT_TERMIN_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TERMIN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TERMIN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TERMIN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TERMIN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TERMIN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TERMIN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_TAB_ZEVENT_TERMIN_02", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TERMIN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TERMIN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_TAB_ZEVENT_TERMIN_02.GT_TERMIN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
