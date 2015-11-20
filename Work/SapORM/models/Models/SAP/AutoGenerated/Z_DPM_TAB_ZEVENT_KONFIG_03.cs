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
	public partial class Z_DPM_TAB_ZEVENT_KONFIG_03
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_TAB_ZEVENT_KONFIG_03).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_TAB_ZEVENT_KONFIG_03).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AKTION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AKTION", value);
		}

		public void SetImportParameter_I_EVENT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EVENT", value);
		}

		public void SetImportParameter_I_EVENT_ORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EVENT_ORT", value);
		}

		public void SetImportParameter_I_EVENT_ORT_BOX(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EVENT_ORT_BOX", value);
		}

		public void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public partial class GT_BOX : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EVENT_ORTBOX { get; set; }

			public string EVENT_ORT { get; set; }

			public string EVENT { get; set; }

			public string BOXNR { get; set; }

			public string BOXNAME { get; set; }

			public string BOXART { get; set; }

			public string VERSICH_ID { get; set; }

			public string TECHNIKERNAME { get; set; }

			public string TAKTUNGMINUTEN { get; set; }

			public DateTime? LOESCHDATUM { get; set; }

			public string LOESCHUSER { get; set; }

			public string ERROR { get; set; }

			public static GT_BOX Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_BOX
				{
					EVENT_ORTBOX = (string)row["EVENT_ORTBOX"],
					EVENT_ORT = (string)row["EVENT_ORT"],
					EVENT = (string)row["EVENT"],
					BOXNR = (string)row["BOXNR"],
					BOXNAME = (string)row["BOXNAME"],
					BOXART = (string)row["BOXART"],
					VERSICH_ID = (string)row["VERSICH_ID"],
					TECHNIKERNAME = (string)row["TECHNIKERNAME"],
					TAKTUNGMINUTEN = (string)row["TAKTUNGMINUTEN"],
					LOESCHDATUM = string.IsNullOrEmpty(row["LOESCHDATUM"].ToString()) ? null : (DateTime?)row["LOESCHDATUM"],
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

			public static IEnumerable<GT_BOX> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BOX> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BOX> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BOX).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BOX> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BOX> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BOX> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BOX>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_TAB_ZEVENT_KONFIG_03", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BOX> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BOX>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BOX> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BOX>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BOX> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BOX>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_TAB_ZEVENT_KONFIG_03", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BOX> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BOX>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_TAB_ZEVENT_KONFIG_03.GT_BOX> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
