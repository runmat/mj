using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_ASSIST_READ_BESTAND_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_ASSIST_READ_BESTAND_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_ASSIST_READ_BESTAND_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public DateTime? EINGANG { get; set; }

			public string NAME1 { get; set; }

			public string STREET { get; set; }

			public string PSTLZ { get; set; }

			public string ORT { get; set; }

			public string VUNUMMER { get; set; }

			public string VERSNUMMER { get; set; }

			public DateTime? VERTRAGSBEGINN { get; set; }

			public DateTime? VERTRAGSABLAUF { get; set; }

			public string VERTRAGSSTATUS { get; set; }

			public string BEDINGUNGEN { get; set; }

			public string PRODUKTTYP { get; set; }

			public string ANZ_RISIKEN { get; set; }

			public string KENNZEICHEN { get; set; }

			public string HERSTNAME { get; set; }

			public string FIN { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public string MEHRFZKLAUSEL { get; set; }

			public string WAGNISZIFFER { get; set; }

			public string GELTUNGSBEREICH { get; set; }

			public string KVVERSICHERUNG { get; set; }

			public string EURO_VTRG { get; set; }

			public string WKS_NAME { get; set; }

			public string WKS_STRASSE { get; set; }

			public string WKS_PLZ { get; set; }

			public string WKS_ORT { get; set; }

			public string WKS_ZEIT { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					EINGANG = (string.IsNullOrEmpty(row["EINGANG"].ToString())) ? null : (DateTime?)row["EINGANG"],
					NAME1 = (string)row["NAME1"],
					STREET = (string)row["STREET"],
					PSTLZ = (string)row["PSTLZ"],
					ORT = (string)row["ORT"],
					VUNUMMER = (string)row["VUNUMMER"],
					VERSNUMMER = (string)row["VERSNUMMER"],
					VERTRAGSBEGINN = (string.IsNullOrEmpty(row["VERTRAGSBEGINN"].ToString())) ? null : (DateTime?)row["VERTRAGSBEGINN"],
					VERTRAGSABLAUF = (string.IsNullOrEmpty(row["VERTRAGSABLAUF"].ToString())) ? null : (DateTime?)row["VERTRAGSABLAUF"],
					VERTRAGSSTATUS = (string)row["VERTRAGSSTATUS"],
					BEDINGUNGEN = (string)row["BEDINGUNGEN"],
					PRODUKTTYP = (string)row["PRODUKTTYP"],
					ANZ_RISIKEN = (string)row["ANZ_RISIKEN"],
					KENNZEICHEN = (string)row["KENNZEICHEN"],
					HERSTNAME = (string)row["HERSTNAME"],
					FIN = (string)row["FIN"],
					ERSTZULDAT = (string.IsNullOrEmpty(row["ERSTZULDAT"].ToString())) ? null : (DateTime?)row["ERSTZULDAT"],
					MEHRFZKLAUSEL = (string)row["MEHRFZKLAUSEL"],
					WAGNISZIFFER = (string)row["WAGNISZIFFER"],
					GELTUNGSBEREICH = (string)row["GELTUNGSBEREICH"],
					KVVERSICHERUNG = (string)row["KVVERSICHERUNG"],
					EURO_VTRG = (string)row["EURO_VTRG"],
					WKS_NAME = (string)row["WKS_NAME"],
					WKS_STRASSE = (string)row["WKS_STRASSE"],
					WKS_PLZ = (string)row["WKS_PLZ"],
					WKS_ORT = (string)row["WKS_ORT"],
					WKS_ZEIT = (string)row["WKS_ZEIT"],

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
				return Select(dt, sapConnection).ToList();
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
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_ASSIST_READ_BESTAND_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_ASSIST_READ_BESTAND_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_ASSIST_READ_BESTAND_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_ASSIST_READ_BESTAND_01.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
