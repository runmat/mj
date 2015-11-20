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
	public partial class Z_M_EC_AVM_STATUS_EINSTEUERUNG
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EC_AVM_STATUS_EINSTEUERUNG).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EC_AVM_STATUS_EINSTEUERUNG).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZCARPORT { get; set; }

			public string ZNAME1 { get; set; }

			public string ZZHERST { get; set; }

			public string ZKLTXT { get; set; }

			public string ZZMODELL { get; set; }

			public string ZZBEZEI { get; set; }

			public string FZG_EING_GES { get; set; }

			public string FZG_AUS_VJ { get; set; }

			public string ZUL_VM { get; set; }

			public string ZUL_LFD_M { get; set; }

			public string ZUL_GES_M { get; set; }

			public string ZUL_PZ_LFD_M { get; set; }

			public string ZUL_PZ_FM { get; set; }

			public string FZG_BEST { get; set; }

			public string FZG_AUSGER { get; set; }

			public string FZG_M_BRIEF { get; set; }

			public string FZG_ZUL_BER { get; set; }

			public string FZG_OHNE_UNIT { get; set; }

			public string ZFZG_GROUP { get; set; }

			public string ZSIPP_CODE { get; set; }

			public string FZG_GESP { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					ZZCARPORT = (string)row["ZZCARPORT"],
					ZNAME1 = (string)row["ZNAME1"],
					ZZHERST = (string)row["ZZHERST"],
					ZKLTXT = (string)row["ZKLTXT"],
					ZZMODELL = (string)row["ZZMODELL"],
					ZZBEZEI = (string)row["ZZBEZEI"],
					FZG_EING_GES = (string)row["FZG_EING_GES"],
					FZG_AUS_VJ = (string)row["FZG_AUS_VJ"],
					ZUL_VM = (string)row["ZUL_VM"],
					ZUL_LFD_M = (string)row["ZUL_LFD_M"],
					ZUL_GES_M = (string)row["ZUL_GES_M"],
					ZUL_PZ_LFD_M = (string)row["ZUL_PZ_LFD_M"],
					ZUL_PZ_FM = (string)row["ZUL_PZ_FM"],
					FZG_BEST = (string)row["FZG_BEST"],
					FZG_AUSGER = (string)row["FZG_AUSGER"],
					FZG_M_BRIEF = (string)row["FZG_M_BRIEF"],
					FZG_ZUL_BER = (string)row["FZG_ZUL_BER"],
					FZG_OHNE_UNIT = (string)row["FZG_OHNE_UNIT"],
					ZFZG_GROUP = (string)row["ZFZG_GROUP"],
					ZSIPP_CODE = (string)row["ZSIPP_CODE"],
					FZG_GESP = (string)row["FZG_GESP"],

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
				return Select(dt, sapConnection).ToListOrEmptyList();
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
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_STATUS_EINSTEUERUNG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_STATUS_EINSTEUERUNG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_STATUS_EINSTEUERUNG.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
