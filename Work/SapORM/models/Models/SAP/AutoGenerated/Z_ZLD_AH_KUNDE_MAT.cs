using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_ZLD_AH_KUNDE_MAT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_AH_KUNDE_MAT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_AH_KUNDE_MAT).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_DEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public string GRUPPE { get; set; }

			public string KUNNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string CITY1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string EXTENSION1 { get; set; }

			public string ZZPAUSCHAL { get; set; }

			public string OHNEUST { get; set; }

			public string XCPDK { get; set; }

			public string XCPDEIN { get; set; }

			public string KUNNR_LF { get; set; }

			public string BARKUNDE { get; set; }

			public string ZULUPFLICHT { get; set; }

			public string REF_NAME_01 { get; set; }

			public string REF_NAME_02 { get; set; }

			public string REF_NAME_03 { get; set; }

			public string REF_NAME_04 { get; set; }

			public string HALTER { get; set; }

			public string DAUER_EVB { get; set; }

			public static GT_DEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DEB
				{
					VKORG = (string)row["VKORG"],
					VKBUR = (string)row["VKBUR"],
					GRUPPE = (string)row["GRUPPE"],
					KUNNR = (string)row["KUNNR"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					CITY1 = (string)row["CITY1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					EXTENSION1 = (string)row["EXTENSION1"],
					ZZPAUSCHAL = (string)row["ZZPAUSCHAL"],
					OHNEUST = (string)row["OHNEUST"],
					XCPDK = (string)row["XCPDK"],
					XCPDEIN = (string)row["XCPDEIN"],
					KUNNR_LF = (string)row["KUNNR_LF"],
					BARKUNDE = (string)row["BARKUNDE"],
					ZULUPFLICHT = (string)row["ZULUPFLICHT"],
					REF_NAME_01 = (string)row["REF_NAME_01"],
					REF_NAME_02 = (string)row["REF_NAME_02"],
					REF_NAME_03 = (string)row["REF_NAME_03"],
					REF_NAME_04 = (string)row["REF_NAME_04"],
					HALTER = (string)row["HALTER"],
					DAUER_EVB = (string)row["DAUER_EVB"],

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

			public static IEnumerable<GT_DEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_DEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_DEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_KUNDE_MAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_KUNDE_MAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_DEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}

		public partial class GT_MAT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public string MATNR { get; set; }

			public string ZUONR { get; set; }

			public string BLTYP { get; set; }

			public string ZDEFAULT { get; set; }

			public string MAKTX { get; set; }

			public string DOKZUORD { get; set; }

			public static GT_MAT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_MAT
				{
					VKORG = (string)row["VKORG"],
					VKBUR = (string)row["VKBUR"],
					MATNR = (string)row["MATNR"],
					ZUONR = (string)row["ZUONR"],
					BLTYP = (string)row["BLTYP"],
					ZDEFAULT = (string)row["ZDEFAULT"],
					MAKTX = (string)row["MAKTX"],
					DOKZUORD = (string)row["DOKZUORD"],

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

			public static IEnumerable<GT_MAT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_MAT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_MAT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_MAT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_MAT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_MAT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_MAT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_MAT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_KUNDE_MAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_MAT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_MAT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_MAT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_MAT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_MAT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_MAT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_KUNDE_MAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_MAT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_MAT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_KUNDE_MAT.GT_DEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_AH_KUNDE_MAT.GT_DEB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_KUNDE_MAT.GT_MAT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_AH_KUNDE_MAT.GT_MAT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
