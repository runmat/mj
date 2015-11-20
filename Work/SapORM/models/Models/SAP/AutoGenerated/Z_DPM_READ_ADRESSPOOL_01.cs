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
	public partial class Z_DPM_READ_ADRESSPOOL_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_ADRESSPOOL_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_ADRESSPOOL_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_EQTYP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQTYP", value);
		}

		public void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public void SetImportParameter_I_NAME1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NAME1", value);
		}

		public void SetImportParameter_I_NAME2(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NAME2", value);
		}

		public void SetImportParameter_I_ORT01(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ORT01", value);
		}

		public void SetImportParameter_I_POS_TEXT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_POS_TEXT", value);
		}

		public void SetImportParameter_I_PSTLZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PSTLZ", value);
		}

		public void SetImportParameter_I_STRAS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STRAS", value);
		}

		public partial class GT_ADRS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string IDENT { get; set; }

			public string KUNNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string COUNTRY { get; set; }

			public string TELEFON { get; set; }

			public string FAX { get; set; }

			public string EMAIL { get; set; }

			public string KONZS { get; set; }

			public string KENNUNG { get; set; }

			public string POS_TEXT { get; set; }

			public string SAPNR { get; set; }

			public static GT_ADRS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ADRS
				{
					IDENT = (string)row["IDENT"],
					KUNNR = (string)row["KUNNR"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					COUNTRY = (string)row["COUNTRY"],
					TELEFON = (string)row["TELEFON"],
					FAX = (string)row["FAX"],
					EMAIL = (string)row["EMAIL"],
					KONZS = (string)row["KONZS"],
					KENNUNG = (string)row["KENNUNG"],
					POS_TEXT = (string)row["POS_TEXT"],
					SAPNR = (string)row["SAPNR"],

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

			public static IEnumerable<GT_ADRS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ADRS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ADRS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ADRS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ADRS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ADRS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_ADRESSPOOL_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_ADRESSPOOL_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_ZULAST : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string LIFNR { get; set; }

			public string ZKFZKZ { get; set; }

			public string ORT01 { get; set; }

			public string PSTLZ { get; set; }

			public string STRAS { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public static GT_ZULAST Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ZULAST
				{
					LIFNR = (string)row["LIFNR"],
					ZKFZKZ = (string)row["ZKFZKZ"],
					ORT01 = (string)row["ORT01"],
					PSTLZ = (string)row["PSTLZ"],
					STRAS = (string)row["STRAS"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],

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

			public static IEnumerable<GT_ZULAST> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ZULAST> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ZULAST> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ZULAST).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ZULAST> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZULAST> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ZULAST> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ZULAST>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_ADRESSPOOL_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZULAST> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ZULAST>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZULAST> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ZULAST>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZULAST> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ZULAST>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_ADRESSPOOL_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ZULAST> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ZULAST>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_ADRESSPOOL_01.GT_ADRS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_ADRESSPOOL_01.GT_ZULAST> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
