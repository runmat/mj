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
	public partial class Z_ZLD_EXPORT_INFOPOOL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_INFOPOOL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_INFOPOOL).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KREISKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KREISKZ", value);
		}

		public partial class GT_EX_ZUSTLIEF : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KREISKZ_DIREKT { get; set; }

			public string MATNR { get; set; }

			public string LIFNR { get; set; }

			public string ADRNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string VERKF { get; set; }

			public string TEL_NUMBER { get; set; }

			public string TEL_EXTENS { get; set; }

			public string FAX_NUMBER { get; set; }

			public string FAX_EXTENS { get; set; }

			public string SMTP_ADDR { get; set; }

			public string ZTXT1 { get; set; }

			public string ZTXT2 { get; set; }

			public string ZTXT3 { get; set; }

			public string REMARK { get; set; }

			public decimal? KBETR { get; set; }

			public string KNUMH { get; set; }

			public string KREISKZ { get; set; }

			public string KBANR { get; set; }

			public static GT_EX_ZUSTLIEF Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_EX_ZUSTLIEF
				{
					KREISKZ_DIREKT = (string)row["KREISKZ_DIREKT"],
					MATNR = (string)row["MATNR"],
					LIFNR = (string)row["LIFNR"],
					ADRNR = (string)row["ADRNR"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					VERKF = (string)row["VERKF"],
					TEL_NUMBER = (string)row["TEL_NUMBER"],
					TEL_EXTENS = (string)row["TEL_EXTENS"],
					FAX_NUMBER = (string)row["FAX_NUMBER"],
					FAX_EXTENS = (string)row["FAX_EXTENS"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],
					ZTXT1 = (string)row["ZTXT1"],
					ZTXT2 = (string)row["ZTXT2"],
					ZTXT3 = (string)row["ZTXT3"],
					REMARK = (string)row["REMARK"],
					KBETR = string.IsNullOrEmpty(row["KBETR"].ToString()) ? null : (decimal?)row["KBETR"],
					KNUMH = (string)row["KNUMH"],
					KREISKZ = (string)row["KREISKZ"],
					KBANR = (string)row["KBANR"],

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

			public static IEnumerable<GT_EX_ZUSTLIEF> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EX_ZUSTLIEF> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EX_ZUSTLIEF> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EX_ZUSTLIEF).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EX_ZUSTLIEF> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUSTLIEF> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EX_ZUSTLIEF> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUSTLIEF>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_INFOPOOL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUSTLIEF> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUSTLIEF>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUSTLIEF> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUSTLIEF>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUSTLIEF> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUSTLIEF>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_INFOPOOL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ZUSTLIEF> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ZUSTLIEF>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_INFOPOOL.GT_EX_ZUSTLIEF> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
