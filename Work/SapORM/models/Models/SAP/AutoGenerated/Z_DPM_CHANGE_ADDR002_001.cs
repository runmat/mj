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
	public partial class Z_DPM_CHANGE_ADDR002_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_CHANGE_ADDR002_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_CHANGE_ADDR002_001).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_ADDRTYP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ADDRTYP", value);
		}

		public void SetImportParameter_I_CITY1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CITY1", value);
		}

		public void SetImportParameter_I_COUNTRY(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_COUNTRY", value);
		}

		public void SetImportParameter_I_EX_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EX_KUNNR", value);
		}

		public void SetImportParameter_I_FAXNUMBER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAXNUMBER", value);
		}

		public void SetImportParameter_I_HOUSENUM1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_HOUSENUM1", value);
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

		public void SetImportParameter_I_POSTCODE1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_POSTCODE1", value);
		}

		public void SetImportParameter_I_SMTPADDR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SMTPADDR", value);
		}

		public void SetImportParameter_I_STREET(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STREET", value);
		}

		public void SetImportParameter_I_TELNUMBER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TELNUMBER", value);
		}

		public void SetImportParameter_I_TYPE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TYPE", value);
		}

		public void SetImportParameter_I_WEBUSER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_WEBUSER", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string KUNNR_AG { get; set; }

			public string ADDRTYP { get; set; }

			public string EX_KUNNR { get; set; }

			public string SORTL { get; set; }

			public string ANREDE { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string NAME3 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string DISTRIKT { get; set; }

			public string COUNTRY { get; set; }

			public string TEL_NUMBER { get; set; }

			public string FAX_NUMBER { get; set; }

			public string SMTP_ADDR { get; set; }

			public string VTEH { get; set; }

			public string MC_NAME1 { get; set; }

			public string MC_CITY1 { get; set; }

			public string MC_STREET { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERNAM { get; set; }

			public DateTime? AEDAT { get; set; }

			public string AENAM { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					MANDT = (string)row["MANDT"],
					KUNNR_AG = (string)row["KUNNR_AG"],
					ADDRTYP = (string)row["ADDRTYP"],
					EX_KUNNR = (string)row["EX_KUNNR"],
					SORTL = (string)row["SORTL"],
					ANREDE = (string)row["ANREDE"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					NAME3 = (string)row["NAME3"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					DISTRIKT = (string)row["DISTRIKT"],
					COUNTRY = (string)row["COUNTRY"],
					TEL_NUMBER = (string)row["TEL_NUMBER"],
					FAX_NUMBER = (string)row["FAX_NUMBER"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],
					VTEH = (string)row["VTEH"],
					MC_NAME1 = (string)row["MC_NAME1"],
					MC_CITY1 = (string)row["MC_CITY1"],
					MC_STREET = (string)row["MC_STREET"],
					ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
					ERNAM = (string)row["ERNAM"],
					AEDAT = string.IsNullOrEmpty(row["AEDAT"].ToString()) ? null : (DateTime?)row["AEDAT"],
					AENAM = (string)row["AENAM"],

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

			public static IEnumerable<GT_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_CHANGE_ADDR002_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_CHANGE_ADDR002_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_CHANGE_ADDR002_001.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
