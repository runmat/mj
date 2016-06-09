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
	public partial class Z_ZLD_AH_VZ_SAVE2
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_AH_VZ_SAVE2).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_AH_VZ_SAVE2).Name, inputParameterKeys, inputParameterValues);
		}


		public partial class GT_ADRS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string PARVW { get; set; }

			public string KUNNR { get; set; }

			public string LI_NAME1 { get; set; }

			public string LI_NAME2 { get; set; }

			public string LI_PLZ { get; set; }

			public string LI_CITY1 { get; set; }

			public string LI_STREET { get; set; }

			public string LOEKZ { get; set; }

			public string BEMERKUNG { get; set; }

			public string LAND1 { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_ADRS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_ADRS o;

				try
				{
					o = new GT_ADRS
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						ZULBELN = (string)row["ZULBELN"],
						PARVW = (string)row["PARVW"],
						KUNNR = (string)row["KUNNR"],
						LI_NAME1 = (string)row["LI_NAME1"],
						LI_NAME2 = (string)row["LI_NAME2"],
						LI_PLZ = (string)row["LI_PLZ"],
						LI_CITY1 = (string)row["LI_CITY1"],
						LI_STREET = (string)row["LI_STREET"],
						LOEKZ = (string)row["LOEKZ"],
						BEMERKUNG = (string)row["BEMERKUNG"],
						LAND1 = (string)row["LAND1"],
					};
				}
				catch(Exception e)
				{
					o = new GT_ADRS
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

			public static IEnumerable<GT_ADRS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ADRS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ADRS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ADRS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ADRS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ADRS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_BAK : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string VBELN { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public DateTime? VE_ERDAT { get; set; }

			public string VE_ERNAM { get; set; }

			public string VE_ERZEIT { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERNAM { get; set; }

			public string STATUS { get; set; }

			public string BLTYP { get; set; }

			public string VZB_STATUS { get; set; }

			public string VZD_VKBUR { get; set; }

			public DateTime? VZERDAT { get; set; }

			public string BARCODE { get; set; }

			public string KUNNR { get; set; }

			public string ZZREFNR1 { get; set; }

			public string ZZREFNR2 { get; set; }

			public string KREISKZ { get; set; }

			public string KREISBEZ { get; set; }

			public string WUNSCHKENN_JN { get; set; }

			public string RESERVKENN_JN { get; set; }

			public string RESERVKENN { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZKENN { get; set; }

			public string KENNZFORM { get; set; }

			public string KENNZANZ { get; set; }

			public string EINKENN_JN { get; set; }

			public string BEMERKUNG { get; set; }

			public string EC_JN { get; set; }

			public string BAR_JN { get; set; }

			public string RE_JN { get; set; }

			public string ZL_LIFNR { get; set; }

			public string KUNDEBAR_JN { get; set; }

			public string LOEKZ { get; set; }

			public string KSTATUS { get; set; }

			public string ERROR_TEXT { get; set; }

			public string PRALI_PRINT { get; set; }

			public string FLIEGER { get; set; }

			public string BEB_STATUS { get; set; }

			public string INFO_TEXT { get; set; }

			public string ZL_RL_FRBNR_HIN { get; set; }

			public string ZL_RL_FRBNR_ZUR { get; set; }

			public string NACHBEARBEITEN { get; set; }

			public string LTEXT_NR { get; set; }

			public string MOBUSER { get; set; }

			public string ZZEVB { get; set; }

			public string ZAHLART { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_BAK Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_BAK o;

				try
				{
					o = new GT_BAK
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						ZULBELN = (string)row["ZULBELN"],
						VBELN = (string)row["VBELN"],
						VKORG = (string)row["VKORG"],
						VKBUR = (string)row["VKBUR"],
						VE_ERDAT = string.IsNullOrEmpty(row["VE_ERDAT"].ToString()) ? null : (DateTime?)row["VE_ERDAT"],
						VE_ERNAM = (string)row["VE_ERNAM"],
						VE_ERZEIT = (string)row["VE_ERZEIT"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						ERNAM = (string)row["ERNAM"],
						STATUS = (string)row["STATUS"],
						BLTYP = (string)row["BLTYP"],
						VZB_STATUS = (string)row["VZB_STATUS"],
						VZD_VKBUR = (string)row["VZD_VKBUR"],
						VZERDAT = string.IsNullOrEmpty(row["VZERDAT"].ToString()) ? null : (DateTime?)row["VZERDAT"],
						BARCODE = (string)row["BARCODE"],
						KUNNR = (string)row["KUNNR"],
						ZZREFNR1 = (string)row["ZZREFNR1"],
						ZZREFNR2 = (string)row["ZZREFNR2"],
						KREISKZ = (string)row["KREISKZ"],
						KREISBEZ = (string)row["KREISBEZ"],
						WUNSCHKENN_JN = (string)row["WUNSCHKENN_JN"],
						RESERVKENN_JN = (string)row["RESERVKENN_JN"],
						RESERVKENN = (string)row["RESERVKENN"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						ZZKENN = (string)row["ZZKENN"],
						KENNZFORM = (string)row["KENNZFORM"],
						KENNZANZ = (string)row["KENNZANZ"],
						EINKENN_JN = (string)row["EINKENN_JN"],
						BEMERKUNG = (string)row["BEMERKUNG"],
						EC_JN = (string)row["EC_JN"],
						BAR_JN = (string)row["BAR_JN"],
						RE_JN = (string)row["RE_JN"],
						ZL_LIFNR = (string)row["ZL_LIFNR"],
						KUNDEBAR_JN = (string)row["KUNDEBAR_JN"],
						LOEKZ = (string)row["LOEKZ"],
						KSTATUS = (string)row["KSTATUS"],
						ERROR_TEXT = (string)row["ERROR_TEXT"],
						PRALI_PRINT = (string)row["PRALI_PRINT"],
						FLIEGER = (string)row["FLIEGER"],
						BEB_STATUS = (string)row["BEB_STATUS"],
						INFO_TEXT = (string)row["INFO_TEXT"],
						ZL_RL_FRBNR_HIN = (string)row["ZL_RL_FRBNR_HIN"],
						ZL_RL_FRBNR_ZUR = (string)row["ZL_RL_FRBNR_ZUR"],
						NACHBEARBEITEN = (string)row["NACHBEARBEITEN"],
						LTEXT_NR = (string)row["LTEXT_NR"],
						MOBUSER = (string)row["MOBUSER"],
						ZZEVB = (string)row["ZZEVB"],
						ZAHLART = (string)row["ZAHLART"],
					};
				}
				catch(Exception e)
				{
					o = new GT_BAK
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

			public static IEnumerable<GT_BAK> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BAK> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BAK> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BAK).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BAK> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BAK> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BAK>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BAK>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BAK>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BAK>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BAK> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BAK>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_BANK : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string PARVW { get; set; }

			public string BANKL { get; set; }

			public string BANKN { get; set; }

			public string EBPP_ACCNAME { get; set; }

			public string KOINH { get; set; }

			public string EINZ_JN { get; set; }

			public string RECH_JN { get; set; }

			public string SWIFT { get; set; }

			public string IBAN { get; set; }

			public string LOEKZ { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_BANK Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_BANK o;

				try
				{
					o = new GT_BANK
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						ZULBELN = (string)row["ZULBELN"],
						PARVW = (string)row["PARVW"],
						BANKL = (string)row["BANKL"],
						BANKN = (string)row["BANKN"],
						EBPP_ACCNAME = (string)row["EBPP_ACCNAME"],
						KOINH = (string)row["KOINH"],
						EINZ_JN = (string)row["EINZ_JN"],
						RECH_JN = (string)row["RECH_JN"],
						SWIFT = (string)row["SWIFT"],
						IBAN = (string)row["IBAN"],
						LOEKZ = (string)row["LOEKZ"],
					};
				}
				catch(Exception e)
				{
					o = new GT_BANK
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

			public static IEnumerable<GT_BANK> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_BANK> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_BANK> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_BANK).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_BANK> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_BANK> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_BANK> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BANK>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BANK> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BANK>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BANK> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BANK>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BANK> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_BANK>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_BANK> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_BANK>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_EX_ERRORS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string ZULPOSNR { get; set; }

			public string ERROR_TEXT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_EX_ERRORS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_EX_ERRORS o;

				try
				{
					o = new GT_EX_ERRORS
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						ZULBELN = (string)row["ZULBELN"],
						ZULPOSNR = (string)row["ZULPOSNR"],
						ERROR_TEXT = (string)row["ERROR_TEXT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_EX_ERRORS
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

			public static IEnumerable<GT_EX_ERRORS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EX_ERRORS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EX_ERRORS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EX_ERRORS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EX_ERRORS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EX_ERRORS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_ERRORS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_ERRORS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_POS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string ZULPOSNR { get; set; }

			public string UEPOS { get; set; }

			public string LOEKZ { get; set; }

			public decimal? MENGE { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public decimal? PREIS { get; set; }

			public decimal? GEB_AMT { get; set; }

			public decimal? GEB_AMT_ADD { get; set; }

			public string WEBMTART { get; set; }

			public string SD_REL { get; set; }

			public string NULLPREIS_OK { get; set; }

			public string GBPAK { get; set; }

			public decimal? UPREIS { get; set; }

			public decimal? DIFF { get; set; }

			public string KONDTAB { get; set; }

			public string KSCHL { get; set; }

			public DateTime? CALCDAT { get; set; }

			public string WEB_STATUS { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_POS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_POS o;

				try
				{
					o = new GT_POS
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						ZULBELN = (string)row["ZULBELN"],
						ZULPOSNR = (string)row["ZULPOSNR"],
						UEPOS = (string)row["UEPOS"],
						LOEKZ = (string)row["LOEKZ"],
						MENGE = string.IsNullOrEmpty(row["MENGE"].ToString()) ? null : (decimal?)row["MENGE"],
						MATNR = (string)row["MATNR"],
						MAKTX = (string)row["MAKTX"],
						PREIS = string.IsNullOrEmpty(row["PREIS"].ToString()) ? null : (decimal?)row["PREIS"],
						GEB_AMT = string.IsNullOrEmpty(row["GEB_AMT"].ToString()) ? null : (decimal?)row["GEB_AMT"],
						GEB_AMT_ADD = string.IsNullOrEmpty(row["GEB_AMT_ADD"].ToString()) ? null : (decimal?)row["GEB_AMT_ADD"],
						WEBMTART = (string)row["WEBMTART"],
						SD_REL = (string)row["SD_REL"],
						NULLPREIS_OK = (string)row["NULLPREIS_OK"],
						GBPAK = (string)row["GBPAK"],
						UPREIS = string.IsNullOrEmpty(row["UPREIS"].ToString()) ? null : (decimal?)row["UPREIS"],
						DIFF = string.IsNullOrEmpty(row["DIFF"].ToString()) ? null : (decimal?)row["DIFF"],
						KONDTAB = (string)row["KONDTAB"],
						KSCHL = (string)row["KSCHL"],
						CALCDAT = string.IsNullOrEmpty(row["CALCDAT"].ToString()) ? null : (DateTime?)row["CALCDAT"],
						WEB_STATUS = (string)row["WEB_STATUS"],
					};
				}
				catch(Exception e)
				{
					o = new GT_POS
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

			public static IEnumerable<GT_POS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_POS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_POS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_POS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_POS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_POS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_VZ_SAVE2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_VZ_SAVE2.GT_ADRS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_VZ_SAVE2.GT_BAK> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_VZ_SAVE2.GT_BANK> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_VZ_SAVE2.GT_EX_ERRORS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_VZ_SAVE2.GT_POS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
