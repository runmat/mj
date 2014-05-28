using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_TAB_ZEVENT_KONFIG_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_TAB_ZEVENT_KONFIG_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_TAB_ZEVENT_KONFIG_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_EVENT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EVENT { get; set; }

			public string EVENT_NAME { get; set; }

			public string REGION { get; set; }

			public string KATEGORIE { get; set; }

			public string FAHRZEUGVOLUMEN { get; set; }

			public string BESCHREIBUNG { get; set; }

			public DateTime? STARTDATUM { get; set; }

			public DateTime? ENDDATUM { get; set; }

			public DateTime? ANLAGEDATUM { get; set; }

			public string ANLAGEUSER { get; set; }

			public DateTime? LOESCHDATUM { get; set; }

			public string LOESCHUSER { get; set; }

			public string ERROR { get; set; }

			public static GT_EVENT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EVENT
				{
					EVENT = (string)row["EVENT"],
					EVENT_NAME = (string)row["EVENT_NAME"],
					REGION = (string)row["REGION"],
					KATEGORIE = (string)row["KATEGORIE"],
					FAHRZEUGVOLUMEN = (string)row["FAHRZEUGVOLUMEN"],
					BESCHREIBUNG = (string)row["BESCHREIBUNG"],
					STARTDATUM = (string.IsNullOrEmpty(row["STARTDATUM"].ToString())) ? null : (DateTime?)row["STARTDATUM"],
					ENDDATUM = (string.IsNullOrEmpty(row["ENDDATUM"].ToString())) ? null : (DateTime?)row["ENDDATUM"],
					ANLAGEDATUM = (string.IsNullOrEmpty(row["ANLAGEDATUM"].ToString())) ? null : (DateTime?)row["ANLAGEDATUM"],
					ANLAGEUSER = (string)row["ANLAGEUSER"],
					LOESCHDATUM = (string.IsNullOrEmpty(row["LOESCHDATUM"].ToString())) ? null : (DateTime?)row["LOESCHDATUM"],
					LOESCHUSER = (string)row["LOESCHUSER"],
					ERROR = (string)row["ERROR"],

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

			public static IEnumerable<GT_EVENT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EVENT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_EVENT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EVENT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EVENT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_EVENT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EVENT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EVENT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_TAB_ZEVENT_KONFIG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_EVENT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EVENT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_EVENT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EVENT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_EVENT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EVENT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_TAB_ZEVENT_KONFIG_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_EVENT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EVENT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
