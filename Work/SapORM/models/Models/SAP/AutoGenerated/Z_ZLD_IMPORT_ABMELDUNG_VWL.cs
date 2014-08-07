using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_ZLD_IMPORT_ABMELDUNG_VWL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_IMPORT_ABMELDUNG_VWL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_IMPORT_ABMELDUNG_VWL).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_IN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ORDER_NUMBER { get; set; }

			public string CONTRACT { get; set; }

			public string COMMISSION { get; set; }

			public string ZZKENN { get; set; }

			public string ORGID { get; set; }

			public string VS_NAME1 { get; set; }

			public string VS_NAME2 { get; set; }

			public string VS_STREET { get; set; }

			public string VS_STATE { get; set; }

			public string VS_CITY { get; set; }

			public string VS_ZIP { get; set; }

			public string ZFAHRG { get; set; }

			public string COLCO { get; set; }

			public string DESCRIPTION { get; set; }

			public string EMAIL { get; set; }

			public string AH_NAME1 { get; set; }

			public string AH_NAME2 { get; set; }

			public string AH_STREET { get; set; }

			public string AH_STATE { get; set; }

			public string AH_CITY { get; set; }

			public string AH_ZIP { get; set; }

			public string AH_EMAIL { get; set; }

			public string AH_PARTNER { get; set; }

			public string AH_TELEFON { get; set; }

			public string RE_EMPF { get; set; }

			public DateTime? VERTRAGSBEGINN { get; set; }

			public DateTime? VERTRAGSENDE { get; set; }

			public DateTime? ANLAGE_EXCEL { get; set; }

			public string BEM { get; set; }

			public string BILL_ORGID { get; set; }

			public string HERKUNFT { get; set; }

			public DateTime? ERST_ZUL_DAT { get; set; }

			public static GT_IN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_IN
				{
					ORDER_NUMBER = (string)row["ORDER_NUMBER"],
					CONTRACT = (string)row["CONTRACT"],
					COMMISSION = (string)row["COMMISSION"],
					ZZKENN = (string)row["ZZKENN"],
					ORGID = (string)row["ORGID"],
					VS_NAME1 = (string)row["VS_NAME1"],
					VS_NAME2 = (string)row["VS_NAME2"],
					VS_STREET = (string)row["VS_STREET"],
					VS_STATE = (string)row["VS_STATE"],
					VS_CITY = (string)row["VS_CITY"],
					VS_ZIP = (string)row["VS_ZIP"],
					ZFAHRG = (string)row["ZFAHRG"],
					COLCO = (string)row["COLCO"],
					DESCRIPTION = (string)row["DESCRIPTION"],
					EMAIL = (string)row["EMAIL"],
					AH_NAME1 = (string)row["AH_NAME1"],
					AH_NAME2 = (string)row["AH_NAME2"],
					AH_STREET = (string)row["AH_STREET"],
					AH_STATE = (string)row["AH_STATE"],
					AH_CITY = (string)row["AH_CITY"],
					AH_ZIP = (string)row["AH_ZIP"],
					AH_EMAIL = (string)row["AH_EMAIL"],
					AH_PARTNER = (string)row["AH_PARTNER"],
					AH_TELEFON = (string)row["AH_TELEFON"],
					RE_EMPF = (string)row["RE_EMPF"],
					VERTRAGSBEGINN = (string.IsNullOrEmpty(row["VERTRAGSBEGINN"].ToString())) ? null : (DateTime?)row["VERTRAGSBEGINN"],
					VERTRAGSENDE = (string.IsNullOrEmpty(row["VERTRAGSENDE"].ToString())) ? null : (DateTime?)row["VERTRAGSENDE"],
					ANLAGE_EXCEL = (string.IsNullOrEmpty(row["ANLAGE_EXCEL"].ToString())) ? null : (DateTime?)row["ANLAGE_EXCEL"],
					BEM = (string)row["BEM"],
					BILL_ORGID = (string)row["BILL_ORGID"],
					HERKUNFT = (string)row["HERKUNFT"],
					ERST_ZUL_DAT = (string.IsNullOrEmpty(row["ERST_ZUL_DAT"].ToString())) ? null : (DateTime?)row["ERST_ZUL_DAT"],

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

			public static IEnumerable<GT_IN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_IN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_IN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_IN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_IN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_IN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_IN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_IMPORT_ABMELDUNG_VWL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_IMPORT_ABMELDUNG_VWL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_IN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_IN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_IMPORT_ABMELDUNG_VWL.GT_IN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_IMPORT_ABMELDUNG_VWL.GT_IN> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
