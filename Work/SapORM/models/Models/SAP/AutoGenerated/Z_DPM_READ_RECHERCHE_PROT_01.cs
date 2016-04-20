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
	public partial class Z_DPM_READ_RECHERCHE_PROT_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_RECHERCHE_PROT_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_RECHERCHE_PROT_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public static void SetImportParameter_I_VORG_NR_ABM_AUF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORG_NR_ABM_AUF", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VORG_NR_ABM_AUF { get; set; }

			public string KENNUNG_AP { get; set; }

			public string NAME { get; set; }

			public string TELNR { get; set; }

			public DateTime? DATUM_1 { get; set; }

			public string USER_1 { get; set; }

			public string ZB1_KNZ_1 { get; set; }

			public DateTime? DATUM_2 { get; set; }

			public string USER_2 { get; set; }

			public string ZB1_KNZ_2 { get; set; }

			public string VERMERK { get; set; }

			public string VERMERK_2 { get; set; }

			public string VE_VORH_1 { get; set; }

			public string VE_VORH_2 { get; set; }

			public string KENNZ_VORH_1 { get; set; }

			public string KENNZ_VORH_2 { get; set; }

			public string NICHT_ERREICHT_1 { get; set; }

			public string NICHT_ERREICHT_2 { get; set; }

			public string PRUE_MELD_SICH_1 { get; set; }

			public string PRUE_MELD_SICH_2 { get; set; }

			public string GA_HAT_UNTERLAG_1 { get; set; }

			public string GA_HAT_UNTERLAG_2 { get; set; }

			public string TEL_NICHT_VERG { get; set; }

			public string EMAIL { get; set; }

			public string AP_PARTNER { get; set; }

			public string ZB2_KNZ_1 { get; set; }

			public string ZB2_KNZ_2 { get; set; }

			public string KENNZ_VORH_H1 { get; set; }

			public string KENNZ_VORH_H2 { get; set; }

			public string KENNZ_ENTWERTET_1 { get; set; }

			public string KENNZ_ENTWERTET_2 { get; set; }

			public string KENNZ_DIEBSTAHL_1 { get; set; }

			public string KENNZ_DIEBSTAHL_2 { get; set; }

			public DateTime? IMPORTDAT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_OUT o;

				try
				{
					o = new GT_OUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						VORG_NR_ABM_AUF = (string)row["VORG_NR_ABM_AUF"],
						KENNUNG_AP = (string)row["KENNUNG_AP"],
						NAME = (string)row["NAME"],
						TELNR = (string)row["TELNR"],
						DATUM_1 = string.IsNullOrEmpty(row["DATUM_1"].ToString()) ? null : (DateTime?)row["DATUM_1"],
						USER_1 = (string)row["USER_1"],
						ZB1_KNZ_1 = (string)row["ZB1_KNZ_1"],
						DATUM_2 = string.IsNullOrEmpty(row["DATUM_2"].ToString()) ? null : (DateTime?)row["DATUM_2"],
						USER_2 = (string)row["USER_2"],
						ZB1_KNZ_2 = (string)row["ZB1_KNZ_2"],
						VERMERK = (string)row["VERMERK"],
						VERMERK_2 = (string)row["VERMERK_2"],
						VE_VORH_1 = (string)row["VE_VORH_1"],
						VE_VORH_2 = (string)row["VE_VORH_2"],
						KENNZ_VORH_1 = (string)row["KENNZ_VORH_1"],
						KENNZ_VORH_2 = (string)row["KENNZ_VORH_2"],
						NICHT_ERREICHT_1 = (string)row["NICHT_ERREICHT_1"],
						NICHT_ERREICHT_2 = (string)row["NICHT_ERREICHT_2"],
						PRUE_MELD_SICH_1 = (string)row["PRUE_MELD_SICH_1"],
						PRUE_MELD_SICH_2 = (string)row["PRUE_MELD_SICH_2"],
						GA_HAT_UNTERLAG_1 = (string)row["GA_HAT_UNTERLAG_1"],
						GA_HAT_UNTERLAG_2 = (string)row["GA_HAT_UNTERLAG_2"],
						TEL_NICHT_VERG = (string)row["TEL_NICHT_VERG"],
						EMAIL = (string)row["EMAIL"],
						AP_PARTNER = (string)row["AP_PARTNER"],
						ZB2_KNZ_1 = (string)row["ZB2_KNZ_1"],
						ZB2_KNZ_2 = (string)row["ZB2_KNZ_2"],
						KENNZ_VORH_H1 = (string)row["KENNZ_VORH_H1"],
						KENNZ_VORH_H2 = (string)row["KENNZ_VORH_H2"],
						KENNZ_ENTWERTET_1 = (string)row["KENNZ_ENTWERTET_1"],
						KENNZ_ENTWERTET_2 = (string)row["KENNZ_ENTWERTET_2"],
						KENNZ_DIEBSTAHL_1 = (string)row["KENNZ_DIEBSTAHL_1"],
						KENNZ_DIEBSTAHL_2 = (string)row["KENNZ_DIEBSTAHL_2"],
						IMPORTDAT = string.IsNullOrEmpty(row["IMPORTDAT"].ToString()) ? null : (DateTime?)row["IMPORTDAT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_OUT
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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_RECHERCHE_PROT_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_RECHERCHE_PROT_01", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_RECHERCHE_PROT_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
