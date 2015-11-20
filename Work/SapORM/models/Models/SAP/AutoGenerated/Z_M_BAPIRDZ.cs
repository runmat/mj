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
	public partial class Z_M_BAPIRDZ
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_BAPIRDZ).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_BAPIRDZ).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_INAME1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("INAME1", value);
		}

		public void SetImportParameter_IPOST_CODE1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IPOST_CODE1", value);
		}

		public void SetImportParameter_IREMARK(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IREMARK", value);
		}

		public void SetImportParameter_IZKFZKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("IZKFZKZ", value);
		}

		public partial class ITAB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public int? ID { get; set; }

			public string LIFNR { get; set; }

			public string ADRNR { get; set; }

			public string MATNR { get; set; }

			public string AFNAM { get; set; }

			public string REMARK { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string VERKF { get; set; }

			public string ZKREIS { get; set; }

			public string ZKFZKZ { get; set; }

			public string ZTXT1 { get; set; }

			public string ZTXT2 { get; set; }

			public string ZTXT3 { get; set; }

			public string TELE1 { get; set; }

			public string TELE2 { get; set; }

			public string TELE3 { get; set; }

			public string SMTP_ADDR { get; set; }

			public string FAX_NUMBER { get; set; }

			public string ANSPRECHPARTNER { get; set; }

			public string BEMERKUNG { get; set; }

			public string Z48H_NAME1 { get; set; }

			public string Z48H_NAME2 { get; set; }

			public string Z48H_STREET { get; set; }

			public string Z48H_POST_CODE1 { get; set; }

			public string Z48H_CITY1 { get; set; }

			public string LIFUHRBIS { get; set; }

			public string NACHREICH { get; set; }

			public string Z48H { get; set; }

			public string ABW_ADR_GENERELL { get; set; }

			public static ITAB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ITAB
				{
					ID = string.IsNullOrEmpty(row["ID"].ToString()) ? null : (int?)row["ID"],
					LIFNR = (string)row["LIFNR"],
					ADRNR = (string)row["ADRNR"],
					MATNR = (string)row["MATNR"],
					AFNAM = (string)row["AFNAM"],
					REMARK = (string)row["REMARK"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					VERKF = (string)row["VERKF"],
					ZKREIS = (string)row["ZKREIS"],
					ZKFZKZ = (string)row["ZKFZKZ"],
					ZTXT1 = (string)row["ZTXT1"],
					ZTXT2 = (string)row["ZTXT2"],
					ZTXT3 = (string)row["ZTXT3"],
					TELE1 = (string)row["TELE1"],
					TELE2 = (string)row["TELE2"],
					TELE3 = (string)row["TELE3"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],
					FAX_NUMBER = (string)row["FAX_NUMBER"],
					ANSPRECHPARTNER = (string)row["ANSPRECHPARTNER"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					Z48H_NAME1 = (string)row["Z48H_NAME1"],
					Z48H_NAME2 = (string)row["Z48H_NAME2"],
					Z48H_STREET = (string)row["Z48H_STREET"],
					Z48H_POST_CODE1 = (string)row["Z48H_POST_CODE1"],
					Z48H_CITY1 = (string)row["Z48H_CITY1"],
					LIFUHRBIS = (string)row["LIFUHRBIS"],
					NACHREICH = (string)row["NACHREICH"],
					Z48H = (string)row["Z48H"],
					ABW_ADR_GENERELL = (string)row["ABW_ADR_GENERELL"],

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

			public static IEnumerable<ITAB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ITAB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ITAB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ITAB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ITAB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ITAB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ITAB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ITAB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_BAPIRDZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ITAB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ITAB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ITAB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ITAB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ITAB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ITAB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_BAPIRDZ", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ITAB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ITAB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_BAPIRDZ.ITAB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
