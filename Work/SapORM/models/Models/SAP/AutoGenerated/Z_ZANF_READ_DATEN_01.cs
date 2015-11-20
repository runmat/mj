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
	public partial class Z_ZANF_READ_DATEN_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZANF_READ_DATEN_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZANF_READ_DATEN_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_ADATUM_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ADATUM_BIS", value);
		}

		public void SetImportParameter_I_ADATUM_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ADATUM_VON", value);
		}

		public void SetImportParameter_I_AKTION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AKTION", value);
		}

		public void SetImportParameter_I_ERDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ERDAT_BIS", value);
		}

		public void SetImportParameter_I_ERDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ERDAT_VON", value);
		}

		public void SetImportParameter_I_ERL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ERL", value);
		}

		public void SetImportParameter_I_FAHRG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRG", value);
		}

		public void SetImportParameter_I_KLAERF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KLAERF", value);
		}

		public void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public void SetImportParameter_I_OFFEN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_OFFEN", value);
		}

		public void SetImportParameter_I_REFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_REFNR", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public partial class GT_ADRESS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ORDERID { get; set; }

			public string VBELN { get; set; }

			public string PARVW { get; set; }

			public string KUNNR { get; set; }

			public string XCPDK { get; set; }

			public string TITLE { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string NAME4 { get; set; }

			public string CITY1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string COUNTRY { get; set; }

			public string TEL_NUMBER { get; set; }

			public string TEL_NUMBER2 { get; set; }

			public string FAX_NUMBER { get; set; }

			public string SMTP_ADDR { get; set; }

			public static GT_ADRESS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ADRESS
				{
					ORDERID = (string)row["ORDERID"],
					VBELN = (string)row["VBELN"],
					PARVW = (string)row["PARVW"],
					KUNNR = (string)row["KUNNR"],
					XCPDK = (string)row["XCPDK"],
					TITLE = (string)row["TITLE"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					NAME4 = (string)row["NAME4"],
					CITY1 = (string)row["CITY1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					COUNTRY = (string)row["COUNTRY"],
					TEL_NUMBER = (string)row["TEL_NUMBER"],
					TEL_NUMBER2 = (string)row["TEL_NUMBER2"],
					FAX_NUMBER = (string)row["FAX_NUMBER"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],

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

			public static IEnumerable<GT_ADRESS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ADRESS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ADRESS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ADRESS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ADRESS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ADRESS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZANF_READ_DATEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZANF_READ_DATEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRESS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRESS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_DATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ORDERID { get; set; }

			public string VBELN { get; set; }

			public string AKTION { get; set; }

			public string KTEXT { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZREFNR { get; set; }

			public string ZZKENN { get; set; }

			public DateTime? ERDAT { get; set; }

			public DateTime? ADATUM { get; set; }

			public string PSTATUS { get; set; }

			public string PKTEXT { get; set; }

			public string EQUNR { get; set; }

			public string ZUSER { get; set; }

			public string KUNNR_ZH_OLD { get; set; }

			public string NAME1_ZH_OLD { get; set; }

			public string NAME2_ZH_OLD { get; set; }

			public string CITY1_ZH_OLD { get; set; }

			public string POST_CODE1_ZH_OLD { get; set; }

			public string STREET_ZH_OLD { get; set; }

			public string HOUSE_NUM1_ZH_OLD { get; set; }

			public static GT_DATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DATEN
				{
					ORDERID = (string)row["ORDERID"],
					VBELN = (string)row["VBELN"],
					AKTION = (string)row["AKTION"],
					KTEXT = (string)row["KTEXT"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					ZZREFNR = (string)row["ZZREFNR"],
					ZZKENN = (string)row["ZZKENN"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ADATUM = string.IsNullOrEmpty(row["ADATUM"].ToString()) ? null : (DateTime?)row["ADATUM"],
					PSTATUS = (string)row["PSTATUS"],
					PKTEXT = (string)row["PKTEXT"],
					EQUNR = (string)row["EQUNR"],
					ZUSER = (string)row["ZUSER"],
					KUNNR_ZH_OLD = (string)row["KUNNR_ZH_OLD"],
					NAME1_ZH_OLD = (string)row["NAME1_ZH_OLD"],
					NAME2_ZH_OLD = (string)row["NAME2_ZH_OLD"],
					CITY1_ZH_OLD = (string)row["CITY1_ZH_OLD"],
					POST_CODE1_ZH_OLD = (string)row["POST_CODE1_ZH_OLD"],
					STREET_ZH_OLD = (string)row["STREET_ZH_OLD"],
					HOUSE_NUM1_ZH_OLD = (string)row["HOUSE_NUM1_ZH_OLD"],

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

			public static IEnumerable<GT_DATEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DATEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_DATEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DATEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DATEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DATEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZANF_READ_DATEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZANF_READ_DATEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_KLAERFALLTEXT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ORDERID { get; set; }

			public string VBELN { get; set; }

			public string ZEILENNR { get; set; }

			public string BEMERKUNG { get; set; }

			public static GT_KLAERFALLTEXT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_KLAERFALLTEXT
				{
					ORDERID = (string)row["ORDERID"],
					VBELN = (string)row["VBELN"],
					ZEILENNR = (string)row["ZEILENNR"],
					BEMERKUNG = (string)row["BEMERKUNG"],

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

			public static IEnumerable<GT_KLAERFALLTEXT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_KLAERFALLTEXT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_KLAERFALLTEXT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_KLAERFALLTEXT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_KLAERFALLTEXT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_KLAERFALLTEXT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_KLAERFALLTEXT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_KLAERFALLTEXT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZANF_READ_DATEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_KLAERFALLTEXT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_KLAERFALLTEXT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_KLAERFALLTEXT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_KLAERFALLTEXT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_KLAERFALLTEXT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_KLAERFALLTEXT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZANF_READ_DATEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_KLAERFALLTEXT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_KLAERFALLTEXT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZANF_READ_DATEN_01.GT_ADRESS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZANF_READ_DATEN_01.GT_DATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZANF_READ_DATEN_01.GT_KLAERFALLTEXT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
