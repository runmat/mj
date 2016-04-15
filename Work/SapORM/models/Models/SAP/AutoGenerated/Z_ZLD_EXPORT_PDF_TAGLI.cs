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
	public partial class Z_ZLD_EXPORT_PDF_TAGLI
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_PDF_TAGLI).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_PDF_TAGLI).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AUSGABE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AUSGABE", value);
		}

		public static void SetImportParameter_I_KTEXT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KTEXT", value);
		}

		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static string GetExportParameter_E_FILENAME(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_FILENAME").NotNullOrEmpty().Trim();
		}

		public static byte[] GetExportParameter_E_PDF(ISapDataService sap)
		{
			return sap.GetExportParameter<byte[]>("E_PDF");
		}

		public partial class IS_FIL_ADRS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string TEL_NUMBER { get; set; }

			public string TEL_EXTENS { get; set; }

			public string FAX_NUMBER { get; set; }

			public string FAX_EXTENS { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static IS_FIL_ADRS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				IS_FIL_ADRS o;

				try
				{
					o = new IS_FIL_ADRS
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						VKORG = (string)row["VKORG"],
						VKBUR = (string)row["VKBUR"],
						NAME1 = (string)row["NAME1"],
						NAME2 = (string)row["NAME2"],
						POST_CODE1 = (string)row["POST_CODE1"],
						CITY1 = (string)row["CITY1"],
						STREET = (string)row["STREET"],
						HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
						TEL_NUMBER = (string)row["TEL_NUMBER"],
						TEL_EXTENS = (string)row["TEL_EXTENS"],
						FAX_NUMBER = (string)row["FAX_NUMBER"],
						FAX_EXTENS = (string)row["FAX_EXTENS"],
					};
				}
				catch(Exception e)
				{
					o = new IS_FIL_ADRS
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,
					};
					o.OnMappingError(e, row, false);
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

			public static IEnumerable<IS_FIL_ADRS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static IEnumerable<IS_FIL_ADRS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(IS_FIL_ADRS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<IS_FIL_ADRS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IS_FIL_ADRS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_PDF_TAGLI", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IS_FIL_ADRS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IS_FIL_ADRS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_TAGLI_BEM : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KREISKZ { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZULBELN { get; set; }

			public string BEMERKUNG { get; set; }

			public string TEXT { get; set; }

			public string EINGABEFELD { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_TAGLI_BEM Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_TAGLI_BEM o;

				try
				{
					o = new GT_TAGLI_BEM
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KREISKZ = (string)row["KREISKZ"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						ZULBELN = (string)row["ZULBELN"],
						BEMERKUNG = (string)row["BEMERKUNG"],
						TEXT = (string)row["TEXT"],
						EINGABEFELD = (string)row["EINGABEFELD"],
					};
				}
				catch(Exception e)
				{
					o = new GT_TAGLI_BEM
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

			public static IEnumerable<GT_TAGLI_BEM> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TAGLI_BEM> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TAGLI_BEM> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TAGLI_BEM).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TAGLI_BEM> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_BEM> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TAGLI_BEM> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_BEM>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_PDF_TAGLI", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_BEM> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_BEM>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_BEM> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_BEM>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_BEM> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_BEM>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_PDF_TAGLI", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_BEM> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_BEM>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_TAGLI_K : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KREISKZ { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string KREISBEZ { get; set; }

			public decimal? PREIS { get; set; }

			public string DRUKZ { get; set; }

			public decimal? KENNZANZ { get; set; }

			public int? POS_COUNT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_TAGLI_K Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_TAGLI_K o;

				try
				{
					o = new GT_TAGLI_K
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KREISKZ = (string)row["KREISKZ"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						KREISBEZ = (string)row["KREISBEZ"],
						PREIS = string.IsNullOrEmpty(row["PREIS"].ToString()) ? null : (decimal?)row["PREIS"],
						DRUKZ = (string)row["DRUKZ"],
						KENNZANZ = string.IsNullOrEmpty(row["KENNZANZ"].ToString()) ? null : (decimal?)row["KENNZANZ"],
						POS_COUNT = string.IsNullOrEmpty(row["POS_COUNT"].ToString()) ? null : (int?)row["POS_COUNT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_TAGLI_K
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

			public static IEnumerable<GT_TAGLI_K> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TAGLI_K> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TAGLI_K> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TAGLI_K).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TAGLI_K> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_K> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TAGLI_K> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_K>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_PDF_TAGLI", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_K> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_K>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_K> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_K>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_K> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_K>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_PDF_TAGLI", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_K> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_K>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_TAGLI_P : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KREISKZ { get; set; }

			public string BLTYP { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZULBELN { get; set; }

			public string ZULPOSNR { get; set; }

			public string KUNNR { get; set; }

			public string NAME1 { get; set; }

			public string ZZREFNR1 { get; set; }

			public string ZZREFNR2 { get; set; }

			public decimal? KENNZANZ { get; set; }

			public string KENNZFORM { get; set; }

			public string ZZKENN { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public string RESWUNSCH { get; set; }

			public string WUNSCHKENN_JN { get; set; }

			public string RESERVKENN_JN { get; set; }

			public string RESERVKENN { get; set; }

			public string FEINSTAUBAMT { get; set; }

			public decimal? PREIS { get; set; }

			public string EC_JN { get; set; }

			public string BAR_JN { get; set; }

			public string FLAG { get; set; }

			public string WAERS { get; set; }

			public string BEMERKUNG { get; set; }

			public string VGTYP { get; set; }

			public string SAISON { get; set; }

			public string SAISON_ZEITRAUM { get; set; }

			public string VKBUR { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_TAGLI_P Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_TAGLI_P o;

				try
				{
					o = new GT_TAGLI_P
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KREISKZ = (string)row["KREISKZ"],
						BLTYP = (string)row["BLTYP"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						ZULBELN = (string)row["ZULBELN"],
						ZULPOSNR = (string)row["ZULPOSNR"],
						KUNNR = (string)row["KUNNR"],
						NAME1 = (string)row["NAME1"],
						ZZREFNR1 = (string)row["ZZREFNR1"],
						ZZREFNR2 = (string)row["ZZREFNR2"],
						KENNZANZ = string.IsNullOrEmpty(row["KENNZANZ"].ToString()) ? null : (decimal?)row["KENNZANZ"],
						KENNZFORM = (string)row["KENNZFORM"],
						ZZKENN = (string)row["ZZKENN"],
						MATNR = (string)row["MATNR"],
						MAKTX = (string)row["MAKTX"],
						RESWUNSCH = (string)row["RESWUNSCH"],
						WUNSCHKENN_JN = (string)row["WUNSCHKENN_JN"],
						RESERVKENN_JN = (string)row["RESERVKENN_JN"],
						RESERVKENN = (string)row["RESERVKENN"],
						FEINSTAUBAMT = (string)row["FEINSTAUBAMT"],
						PREIS = string.IsNullOrEmpty(row["PREIS"].ToString()) ? null : (decimal?)row["PREIS"],
						EC_JN = (string)row["EC_JN"],
						BAR_JN = (string)row["BAR_JN"],
						FLAG = (string)row["FLAG"],
						WAERS = (string)row["WAERS"],
						BEMERKUNG = (string)row["BEMERKUNG"],
						VGTYP = (string)row["VGTYP"],
						SAISON = (string)row["SAISON"],
						SAISON_ZEITRAUM = (string)row["SAISON_ZEITRAUM"],
						VKBUR = (string)row["VKBUR"],
					};
				}
				catch(Exception e)
				{
					o = new GT_TAGLI_P
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

			public static IEnumerable<GT_TAGLI_P> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TAGLI_P> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TAGLI_P> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TAGLI_P).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TAGLI_P> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_P> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TAGLI_P> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_P>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_PDF_TAGLI", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_P> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_P>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_P> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_P>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_P> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_P>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_PDF_TAGLI", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_P> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_P>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_TAGLI_SDL : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KREISKZ { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZULBELN { get; set; }

			public string STRING1 { get; set; }

			public string STRING2 { get; set; }

			public string STRING3 { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_TAGLI_SDL Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_TAGLI_SDL o;

				try
				{
					o = new GT_TAGLI_SDL
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KREISKZ = (string)row["KREISKZ"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						ZULBELN = (string)row["ZULBELN"],
						STRING1 = (string)row["STRING1"],
						STRING2 = (string)row["STRING2"],
						STRING3 = (string)row["STRING3"],
					};
				}
				catch(Exception e)
				{
					o = new GT_TAGLI_SDL
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

			public static IEnumerable<GT_TAGLI_SDL> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TAGLI_SDL> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TAGLI_SDL> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TAGLI_SDL).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TAGLI_SDL> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_SDL> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TAGLI_SDL> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_SDL>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_PDF_TAGLI", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_SDL> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_SDL>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_SDL> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_SDL>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_SDL> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_SDL>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_PDF_TAGLI", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TAGLI_SDL> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TAGLI_SDL>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_PDF_TAGLI.IS_FIL_ADRS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_PDF_TAGLI.GT_TAGLI_BEM> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_PDF_TAGLI.GT_TAGLI_K> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_PDF_TAGLI.GT_TAGLI_P> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_PDF_TAGLI.GT_TAGLI_SDL> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
