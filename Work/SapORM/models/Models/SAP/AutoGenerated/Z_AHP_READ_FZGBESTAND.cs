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
	public partial class Z_AHP_READ_FZGBESTAND
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_AHP_READ_FZGBESTAND).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_AHP_READ_FZGBESTAND).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_ABMDAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ABMDAT", value);
		}

		public static void SetImportParameter_I_AKTZULDAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_AKTZULDAT", value);
		}

		public static void SetImportParameter_I_BRIEFBESTAND(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BRIEFBESTAND", value);
		}

		public static void SetImportParameter_I_BRIEFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BRIEFNR", value);
		}

		public static void SetImportParameter_I_COCVORHANDEN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_COCVORHANDEN", value);
		}

		public static void SetImportParameter_I_ERSTZULDAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ERSTZULDAT", value);
		}

		public static void SetImportParameter_I_FIN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN", value);
		}

		public static void SetImportParameter_I_FIN_ID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN_ID", value);
		}

		public static void SetImportParameter_I_HALTER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_HALTER", value);
		}

		public static void SetImportParameter_I_KAEUFER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KAEUFER", value);
		}

		public static void SetImportParameter_I_KD_REF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KD_REF", value);
		}

		public static void SetImportParameter_I_KENNZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KENNZ", value);
		}

		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public static void SetImportParameter_I_LGORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LGORT", value);
		}

		public static void SetImportParameter_I_STANDORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STANDORT", value);
		}

		public partial class GT_WEBOUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN_ID { get; set; }

			public string FIN { get; set; }

			public string KUNDENREFERENZ { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERF_USER { get; set; }

			public DateTime? AENDAT { get; set; }

			public string AEN_USER { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZVVS_SCHLUESSEL { get; set; }

			public string ZZTYP_VVS_PRUEF { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZHANDELSNAME { get; set; }

			public string KUNNR { get; set; }

			public string KAEUFER { get; set; }

			public string HALTER { get; set; }

			public string BRIEFBESTAND { get; set; }

			public string LGORT { get; set; }

			public string STANDORT { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public DateTime? AKTZULDAT { get; set; }

			public DateTime? ABMDAT { get; set; }

			public string KENNZ { get; set; }

			public string BRIEFNR { get; set; }

			public string COCVORHANDEN { get; set; }

			public string BEMERKUNG { get; set; }

			public string FZGART { get; set; }

			public string VKSPARTE { get; set; }

			public string FZGNR { get; set; }

			public string AUFNR { get; set; }

			public string FAREF1 { get; set; }

			public string FAREF2 { get; set; }

			public string KOSTL { get; set; }

			public string KONTOINHABER { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_WEBOUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEBOUT o;

				try
				{
					o = new GT_WEBOUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FIN_ID = (string)row["FIN_ID"],
						FIN = (string)row["FIN"],
						KUNDENREFERENZ = (string)row["KUNDENREFERENZ"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						ERF_USER = (string)row["ERF_USER"],
						AENDAT = string.IsNullOrEmpty(row["AENDAT"].ToString()) ? null : (DateTime?)row["AENDAT"],
						AEN_USER = (string)row["AEN_USER"],
						ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
						ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
						ZZVVS_SCHLUESSEL = (string)row["ZZVVS_SCHLUESSEL"],
						ZZTYP_VVS_PRUEF = (string)row["ZZTYP_VVS_PRUEF"],
						ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
						ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
						KUNNR = (string)row["KUNNR"],
						KAEUFER = (string)row["KAEUFER"],
						HALTER = (string)row["HALTER"],
						BRIEFBESTAND = (string)row["BRIEFBESTAND"],
						LGORT = (string)row["LGORT"],
						STANDORT = (string)row["STANDORT"],
						ERSTZULDAT = string.IsNullOrEmpty(row["ERSTZULDAT"].ToString()) ? null : (DateTime?)row["ERSTZULDAT"],
						AKTZULDAT = string.IsNullOrEmpty(row["AKTZULDAT"].ToString()) ? null : (DateTime?)row["AKTZULDAT"],
						ABMDAT = string.IsNullOrEmpty(row["ABMDAT"].ToString()) ? null : (DateTime?)row["ABMDAT"],
						KENNZ = (string)row["KENNZ"],
						BRIEFNR = (string)row["BRIEFNR"],
						COCVORHANDEN = (string)row["COCVORHANDEN"],
						BEMERKUNG = (string)row["BEMERKUNG"],
						FZGART = (string)row["FZGART"],
						VKSPARTE = (string)row["VKSPARTE"],
						FZGNR = (string)row["FZGNR"],
						AUFNR = (string)row["AUFNR"],
						FAREF1 = (string)row["FAREF1"],
						FAREF2 = (string)row["FAREF2"],
						KOSTL = (string)row["KOSTL"],
						KONTOINHABER = (string)row["KONTOINHABER"],
					};
				}
				catch(Exception e)
				{
					o = new GT_WEBOUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,
					};
					o.OnMappingError(e, row, true);
					if (!o.MappingErrorProcessed)
						throw;
				}

				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnMappingError(Exception e, DataRow row, bool isExport);

			partial void OnInitFromExtern();

			public void OnModelMappingApplied()
			{
				OnInitFromExtern();
			}

			public static IEnumerable<GT_WEBOUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEBOUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEBOUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEBOUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEBOUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEBOUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEBOUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEBOUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_READ_FZGBESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEBOUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEBOUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEBOUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEBOUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEBOUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEBOUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_READ_FZGBESTAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEBOUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEBOUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_AHP_READ_FZGBESTAND.GT_WEBOUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
