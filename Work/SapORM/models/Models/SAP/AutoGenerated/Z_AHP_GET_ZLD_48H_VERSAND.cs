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
	public partial class Z_AHP_GET_ZLD_48H_VERSAND
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_AHP_GET_ZLD_48H_VERSAND).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_AHP_GET_ZLD_48H_VERSAND).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KBANR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KBANR", value);
		}

		public static void SetImportParameter_I_KREISKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KREISKZ", value);
		}

		public static string GetExportParameter_E_MELDUNG(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MELDUNG").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_RC(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_RC").NotNullOrEmpty().Trim();
		}

		public partial class ET_ZLD_48H : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KBANR { get; set; }

			public string LIFNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STREET { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string LIFUHRBIS { get; set; }

			public string NACHREICH { get; set; }

			public string Z48H { get; set; }

			public string ABW_ADR_GENERELL { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static ET_ZLD_48H Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				ET_ZLD_48H o;

				try
				{
					o = new ET_ZLD_48H
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KBANR = (string)row["KBANR"],
						LIFNR = (string)row["LIFNR"],
						NAME1 = (string)row["NAME1"],
						NAME2 = (string)row["NAME2"],
						STREET = (string)row["STREET"],
						POST_CODE1 = (string)row["POST_CODE1"],
						CITY1 = (string)row["CITY1"],
						LIFUHRBIS = (string)row["LIFUHRBIS"],
						NACHREICH = (string)row["NACHREICH"],
						Z48H = (string)row["Z48H"],
						ABW_ADR_GENERELL = (string)row["ABW_ADR_GENERELL"],
					};
				}
				catch(Exception e)
				{
					o = new ET_ZLD_48H
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

			public static IEnumerable<ET_ZLD_48H> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_ZLD_48H> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ET_ZLD_48H> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_ZLD_48H).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_ZLD_48H> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZLD_48H> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_ZLD_48H> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_ZLD_48H>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_GET_ZLD_48H_VERSAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZLD_48H> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ZLD_48H>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZLD_48H> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ZLD_48H>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZLD_48H> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_ZLD_48H>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_GET_ZLD_48H_VERSAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZLD_48H> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ZLD_48H>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class ET_ZULST : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KBANR { get; set; }

			public string AFNAM { get; set; }

			public string ZKREIS { get; set; }

			public string BLAND { get; set; }

			public string LIFNR { get; set; }

			public string ZVKGU { get; set; }

			public string ZVKGK { get; set; }

			public string ZKFZKZ { get; set; }

			public string KREISKZ { get; set; }

			public string ORT01 { get; set; }

			public string ORT02 { get; set; }

			public string PFACH { get; set; }

			public string PSTL2 { get; set; }

			public string PSTLZ { get; set; }

			public string STRAS { get; set; }

			public string TELF1 { get; set; }

			public string TELF2 { get; set; }

			public string TELFX { get; set; }

			public string ZEMAIL { get; set; }

			public string ZTXT1 { get; set; }

			public string ZTXT2 { get; set; }

			public string ZTXT3 { get; set; }

			public string ZHPAGE { get; set; }

			public decimal? ZKFZBST { get; set; }

			public decimal? ZMARKTPTL { get; set; }

			public decimal? ZABSATZ { get; set; }

			public string ZLSSTATUS { get; set; }

			public DateTime? ZLSDATUM { get; set; }

			public string LFB { get; set; }

			public string LIFNR_ABW { get; set; }

			public decimal? ZEINWOHNER { get; set; }

			public string FABKL { get; set; }

			public string KENNZ_FORMAT { get; set; }

			public string KENNZ2_FORMAT { get; set; }

			public string ZZGEOX { get; set; }

			public string ZZGEOY { get; set; }

			public DateTime? ZZGEODAT { get; set; }

			public string EMAIL_DAD { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static ET_ZULST Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				ET_ZULST o;

				try
				{
					o = new ET_ZULST
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KBANR = (string)row["KBANR"],
						AFNAM = (string)row["AFNAM"],
						ZKREIS = (string)row["ZKREIS"],
						BLAND = (string)row["BLAND"],
						LIFNR = (string)row["LIFNR"],
						ZVKGU = (string)row["ZVKGU"],
						ZVKGK = (string)row["ZVKGK"],
						ZKFZKZ = (string)row["ZKFZKZ"],
						KREISKZ = (string)row["KREISKZ"],
						ORT01 = (string)row["ORT01"],
						ORT02 = (string)row["ORT02"],
						PFACH = (string)row["PFACH"],
						PSTL2 = (string)row["PSTL2"],
						PSTLZ = (string)row["PSTLZ"],
						STRAS = (string)row["STRAS"],
						TELF1 = (string)row["TELF1"],
						TELF2 = (string)row["TELF2"],
						TELFX = (string)row["TELFX"],
						ZEMAIL = (string)row["ZEMAIL"],
						ZTXT1 = (string)row["ZTXT1"],
						ZTXT2 = (string)row["ZTXT2"],
						ZTXT3 = (string)row["ZTXT3"],
						ZHPAGE = (string)row["ZHPAGE"],
						ZKFZBST = string.IsNullOrEmpty(row["ZKFZBST"].ToString()) ? null : (decimal?)row["ZKFZBST"],
						ZMARKTPTL = string.IsNullOrEmpty(row["ZMARKTPTL"].ToString()) ? null : (decimal?)row["ZMARKTPTL"],
						ZABSATZ = string.IsNullOrEmpty(row["ZABSATZ"].ToString()) ? null : (decimal?)row["ZABSATZ"],
						ZLSSTATUS = (string)row["ZLSSTATUS"],
						ZLSDATUM = string.IsNullOrEmpty(row["ZLSDATUM"].ToString()) ? null : (DateTime?)row["ZLSDATUM"],
						LFB = (string)row["LFB"],
						LIFNR_ABW = (string)row["LIFNR_ABW"],
						ZEINWOHNER = string.IsNullOrEmpty(row["ZEINWOHNER"].ToString()) ? null : (decimal?)row["ZEINWOHNER"],
						FABKL = (string)row["FABKL"],
						KENNZ_FORMAT = (string)row["KENNZ_FORMAT"],
						KENNZ2_FORMAT = (string)row["KENNZ2_FORMAT"],
						ZZGEOX = (string)row["ZZGEOX"],
						ZZGEOY = (string)row["ZZGEOY"],
						ZZGEODAT = string.IsNullOrEmpty(row["ZZGEODAT"].ToString()) ? null : (DateTime?)row["ZZGEODAT"],
						EMAIL_DAD = (string)row["EMAIL_DAD"],
					};
				}
				catch(Exception e)
				{
					o = new ET_ZULST
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

			public static IEnumerable<ET_ZULST> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_ZULST> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ET_ZULST> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_ZULST).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_ZULST> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZULST> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_ZULST> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_ZULST>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_GET_ZLD_48H_VERSAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZULST> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ZULST>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZULST> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ZULST>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZULST> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_ZULST>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_GET_ZLD_48H_VERSAND", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_ZULST> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_ZULST>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_AHP_GET_ZLD_48H_VERSAND.ET_ZLD_48H> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_AHP_GET_ZLD_48H_VERSAND.ET_ZULST> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
