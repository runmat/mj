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
	public partial class Z_M_PARTNER_AUS_KNVP_LESEN
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_PARTNER_AUS_KNVP_LESEN).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_PARTNER_AUS_KNVP_LESEN).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_EIKTO(ISapDataService sap, string value)
		{
			sap.SetImportParameter("EIKTO", value);
		}

		public static void SetImportParameter_GRUPPE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("GRUPPE", value);
		}

		public static void SetImportParameter_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("KUNNR", value);
		}

		public partial class AUSGABE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string PARVW { get; set; }

			public string KUNNR { get; set; }

			public string NAME1 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string TEL_NUMBER { get; set; }

			public string NAME2 { get; set; }

			public string TELF2 { get; set; }

			public string TELFX { get; set; }

			public string DEFPA { get; set; }

			public string NICK_NAME { get; set; }

			public static AUSGABE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new AUSGABE
				{
					PARVW = (string)row["PARVW"],
					KUNNR = (string)row["KUNNR"],
					NAME1 = (string)row["NAME1"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					TEL_NUMBER = (string)row["TEL_NUMBER"],
					NAME2 = (string)row["NAME2"],
					TELF2 = (string)row["TELF2"],
					TELFX = (string)row["TELFX"],
					DEFPA = (string)row["DEFPA"],
					NICK_NAME = (string)row["NICK_NAME"],

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

			public static IEnumerable<AUSGABE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<AUSGABE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<AUSGABE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(AUSGABE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<AUSGABE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<AUSGABE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_PARTNER_AUS_KNVP_LESEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_PARTNER_AUS_KNVP_LESEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<AUSGABE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<AUSGABE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
