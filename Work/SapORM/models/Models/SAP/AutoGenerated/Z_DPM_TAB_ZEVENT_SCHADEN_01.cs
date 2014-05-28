using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_TAB_ZEVENT_SCHADEN_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_TAB_ZEVENT_SCHADEN_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_TAB_ZEVENT_SCHADEN_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_SCHADEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EVENT_SCHADEN { get; set; }

			public string EVENT { get; set; }

			public string LICENSE_NUM { get; set; }

			public string NAME_FIRST { get; set; }

			public string NAME_LAST { get; set; }

			public string SMTP_ADDR { get; set; }

			public string TELNR_LONG { get; set; }

			public string FZGMARKE { get; set; }

			public string FZGTYP { get; set; }

			public string VERSICH_ID { get; set; }

			public string SBHOEHE { get; set; }

			public string REFNR { get; set; }

			public string SAMMELBES { get; set; }

			public DateTime? ANLAGEDATUM { get; set; }

			public string ANLAGEUSER { get; set; }

			public DateTime? LOESCHDATUM { get; set; }

			public string LOESCHUSER { get; set; }

			public string ERROR { get; set; }

			public static GT_SCHADEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_SCHADEN
				{
					EVENT_SCHADEN = (string)row["EVENT_SCHADEN"],
					EVENT = (string)row["EVENT"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					NAME_FIRST = (string)row["NAME_FIRST"],
					NAME_LAST = (string)row["NAME_LAST"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],
					TELNR_LONG = (string)row["TELNR_LONG"],
					FZGMARKE = (string)row["FZGMARKE"],
					FZGTYP = (string)row["FZGTYP"],
					VERSICH_ID = (string)row["VERSICH_ID"],
					SBHOEHE = (string)row["SBHOEHE"],
					REFNR = (string)row["REFNR"],
					SAMMELBES = (string)row["SAMMELBES"],
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

			public static IEnumerable<GT_SCHADEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_SCHADEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_SCHADEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_SCHADEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_SCHADEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_SCHADEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_SCHADEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_TAB_ZEVENT_SCHADEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_SCHADEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_SCHADEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_SCHADEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_TAB_ZEVENT_SCHADEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_SCHADEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_SCHADEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
