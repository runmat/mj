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
	public partial class ISA_ADDR_POSTAL_CODE_CHECK
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(ISA_ADDR_POSTAL_CODE_CHECK).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(ISA_ADDR_POSTAL_CODE_CHECK).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_COUNTRY(ISapDataService sap, string value)
		{
			sap.SetImportParameter("COUNTRY", value);
		}

		public void SetImportParameter_POSTAL_CODE_CITY(ISapDataService sap, string value)
		{
			sap.SetImportParameter("POSTAL_CODE_CITY", value);
		}

		public void SetImportParameter_POSTAL_CODE_COMPANY(ISapDataService sap, string value)
		{
			sap.SetImportParameter("POSTAL_CODE_COMPANY", value);
		}

		public void SetImportParameter_POSTAL_CODE_PO_BOX(ISapDataService sap, string value)
		{
			sap.SetImportParameter("POSTAL_CODE_PO_BOX", value);
		}

		public void SetImportParameter_PO_BOX(ISapDataService sap, string value)
		{
			sap.SetImportParameter("PO_BOX", value);
		}

		public void SetImportParameter_REGION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("REGION", value);
		}

		public partial class POSTAL_ADDRESS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string NAME_CO { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string HOUSE_NUM2 { get; set; }

			public string STR_SUPPL1 { get; set; }

			public string STR_SUPPL2 { get; set; }

			public string STR_SUPPL3 { get; set; }

			public string CITY1 { get; set; }

			public string CITY2 { get; set; }

			public string HOME_CITY { get; set; }

			public string POST_CODE1 { get; set; }

			public string POST_CODE2 { get; set; }

			public string POST_CODE3 { get; set; }

			public string PCODE1_EXT { get; set; }

			public string PCODE2_EXT { get; set; }

			public string PCODE3_EXT { get; set; }

			public string PO_BOX { get; set; }

			public string PO_BOX_NUM { get; set; }

			public string PO_BOX_LOC { get; set; }

			public string PO_BOX_REG { get; set; }

			public string PO_BOX_CTY { get; set; }

			public string LOCATION { get; set; }

			public string REGION { get; set; }

			public string COUNTRY { get; set; }

			public string PO_BOX_LOBBY { get; set; }

			public string DELI_SERV_TYPE { get; set; }

			public string DELI_SERV_NUMBER { get; set; }

			public string COUNTY { get; set; }

			public string TOWNSHIP { get; set; }

			public static POSTAL_ADDRESS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new POSTAL_ADDRESS
				{
					NAME_CO = (string)row["NAME_CO"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					HOUSE_NUM2 = (string)row["HOUSE_NUM2"],
					STR_SUPPL1 = (string)row["STR_SUPPL1"],
					STR_SUPPL2 = (string)row["STR_SUPPL2"],
					STR_SUPPL3 = (string)row["STR_SUPPL3"],
					CITY1 = (string)row["CITY1"],
					CITY2 = (string)row["CITY2"],
					HOME_CITY = (string)row["HOME_CITY"],
					POST_CODE1 = (string)row["POST_CODE1"],
					POST_CODE2 = (string)row["POST_CODE2"],
					POST_CODE3 = (string)row["POST_CODE3"],
					PCODE1_EXT = (string)row["PCODE1_EXT"],
					PCODE2_EXT = (string)row["PCODE2_EXT"],
					PCODE3_EXT = (string)row["PCODE3_EXT"],
					PO_BOX = (string)row["PO_BOX"],
					PO_BOX_NUM = (string)row["PO_BOX_NUM"],
					PO_BOX_LOC = (string)row["PO_BOX_LOC"],
					PO_BOX_REG = (string)row["PO_BOX_REG"],
					PO_BOX_CTY = (string)row["PO_BOX_CTY"],
					LOCATION = (string)row["LOCATION"],
					REGION = (string)row["REGION"],
					COUNTRY = (string)row["COUNTRY"],
					PO_BOX_LOBBY = (string)row["PO_BOX_LOBBY"],
					DELI_SERV_TYPE = (string)row["DELI_SERV_TYPE"],
					DELI_SERV_NUMBER = (string)row["DELI_SERV_NUMBER"],
					COUNTY = (string)row["COUNTY"],
					TOWNSHIP = (string)row["TOWNSHIP"],

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

			public static IEnumerable<POSTAL_ADDRESS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static IEnumerable<POSTAL_ADDRESS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(POSTAL_ADDRESS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<POSTAL_ADDRESS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<POSTAL_ADDRESS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("ISA_ADDR_POSTAL_CODE_CHECK", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<POSTAL_ADDRESS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<POSTAL_ADDRESS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class RETURN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string TYPE { get; set; }

			public string ID { get; set; }

			public string NUMBER { get; set; }

			public string MESSAGE { get; set; }

			public string LOG_NO { get; set; }

			public string LOG_MSG_NO { get; set; }

			public string MESSAGE_V1 { get; set; }

			public string MESSAGE_V2 { get; set; }

			public string MESSAGE_V3 { get; set; }

			public string MESSAGE_V4 { get; set; }

			public static RETURN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new RETURN
				{
					TYPE = (string)row["TYPE"],
					ID = (string)row["ID"],
					NUMBER = (string)row["NUMBER"],
					MESSAGE = (string)row["MESSAGE"],
					LOG_NO = (string)row["LOG_NO"],
					LOG_MSG_NO = (string)row["LOG_MSG_NO"],
					MESSAGE_V1 = (string)row["MESSAGE_V1"],
					MESSAGE_V2 = (string)row["MESSAGE_V2"],
					MESSAGE_V3 = (string)row["MESSAGE_V3"],
					MESSAGE_V4 = (string)row["MESSAGE_V4"],

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

			public static IEnumerable<RETURN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<RETURN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<RETURN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(RETURN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<RETURN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<RETURN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<RETURN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<RETURN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("ISA_ADDR_POSTAL_CODE_CHECK", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<RETURN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<RETURN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<RETURN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<RETURN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class T005_WA : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string LAND1 { get; set; }

			public string LANDK { get; set; }

			public string LNPLZ { get; set; }

			public string PRPLZ { get; set; }

			public string ADDRS { get; set; }

			public string XPLZS { get; set; }

			public string XPLPF { get; set; }

			public string SPRAS { get; set; }

			public string XLAND { get; set; }

			public string XADDR { get; set; }

			public string NMFMT { get; set; }

			public string XREGS { get; set; }

			public string XPLST { get; set; }

			public string INTCA { get; set; }

			public string INTCA3 { get; set; }

			public string INTCN3 { get; set; }

			public string XEGLD { get; set; }

			public string XSKFN { get; set; }

			public string XMWSN { get; set; }

			public string LNBKN { get; set; }

			public string PRBKN { get; set; }

			public string LNBLZ { get; set; }

			public string PRBLZ { get; set; }

			public string LNPSK { get; set; }

			public string PRPSK { get; set; }

			public string XPRBK { get; set; }

			public string BNKEY { get; set; }

			public string LNBKS { get; set; }

			public string PRBKS { get; set; }

			public string XPRSO { get; set; }

			public string PRUIN { get; set; }

			public string UINLN { get; set; }

			public string LNST1 { get; set; }

			public string PRST1 { get; set; }

			public string LNST2 { get; set; }

			public string PRST2 { get; set; }

			public string LNST3 { get; set; }

			public string PRST3 { get; set; }

			public string LNST4 { get; set; }

			public string PRST4 { get; set; }

			public string LNST5 { get; set; }

			public string PRST5 { get; set; }

			public string LANDD { get; set; }

			public string KALSM { get; set; }

			public string LANDA { get; set; }

			public string WECHF { get; set; }

			public string LKVRZ { get; set; }

			public string INTCN { get; set; }

			public string XDEZP { get; set; }

			public string DATFM { get; set; }

			public string CURIN { get; set; }

			public string CURHA { get; set; }

			public string WAERS { get; set; }

			public string KURST { get; set; }

			public string AFAPL { get; set; }

			public decimal? GWGWRT { get; set; }

			public decimal? UMRWRT { get; set; }

			public string KZRBWB { get; set; }

			public string XANZUM { get; set; }

			public string CTNCONCEPT { get; set; }

			public string KZSRV { get; set; }

			public string XXINVE { get; set; }

			public string SUREG { get; set; }

			public string LANDGRP_VP { get; set; }

			public static T005_WA Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T005_WA
				{
					MANDT = (string)row["MANDT"],
					LAND1 = (string)row["LAND1"],
					LANDK = (string)row["LANDK"],
					LNPLZ = (string)row["LNPLZ"],
					PRPLZ = (string)row["PRPLZ"],
					ADDRS = (string)row["ADDRS"],
					XPLZS = (string)row["XPLZS"],
					XPLPF = (string)row["XPLPF"],
					SPRAS = (string)row["SPRAS"],
					XLAND = (string)row["XLAND"],
					XADDR = (string)row["XADDR"],
					NMFMT = (string)row["NMFMT"],
					XREGS = (string)row["XREGS"],
					XPLST = (string)row["XPLST"],
					INTCA = (string)row["INTCA"],
					INTCA3 = (string)row["INTCA3"],
					INTCN3 = (string)row["INTCN3"],
					XEGLD = (string)row["XEGLD"],
					XSKFN = (string)row["XSKFN"],
					XMWSN = (string)row["XMWSN"],
					LNBKN = (string)row["LNBKN"],
					PRBKN = (string)row["PRBKN"],
					LNBLZ = (string)row["LNBLZ"],
					PRBLZ = (string)row["PRBLZ"],
					LNPSK = (string)row["LNPSK"],
					PRPSK = (string)row["PRPSK"],
					XPRBK = (string)row["XPRBK"],
					BNKEY = (string)row["BNKEY"],
					LNBKS = (string)row["LNBKS"],
					PRBKS = (string)row["PRBKS"],
					XPRSO = (string)row["XPRSO"],
					PRUIN = (string)row["PRUIN"],
					UINLN = (string)row["UINLN"],
					LNST1 = (string)row["LNST1"],
					PRST1 = (string)row["PRST1"],
					LNST2 = (string)row["LNST2"],
					PRST2 = (string)row["PRST2"],
					LNST3 = (string)row["LNST3"],
					PRST3 = (string)row["PRST3"],
					LNST4 = (string)row["LNST4"],
					PRST4 = (string)row["PRST4"],
					LNST5 = (string)row["LNST5"],
					PRST5 = (string)row["PRST5"],
					LANDD = (string)row["LANDD"],
					KALSM = (string)row["KALSM"],
					LANDA = (string)row["LANDA"],
					WECHF = (string)row["WECHF"],
					LKVRZ = (string)row["LKVRZ"],
					INTCN = (string)row["INTCN"],
					XDEZP = (string)row["XDEZP"],
					DATFM = (string)row["DATFM"],
					CURIN = (string)row["CURIN"],
					CURHA = (string)row["CURHA"],
					WAERS = (string)row["WAERS"],
					KURST = (string)row["KURST"],
					AFAPL = (string)row["AFAPL"],
					GWGWRT = string.IsNullOrEmpty(row["GWGWRT"].ToString()) ? null : (decimal?)row["GWGWRT"],
					UMRWRT = string.IsNullOrEmpty(row["UMRWRT"].ToString()) ? null : (decimal?)row["UMRWRT"],
					KZRBWB = (string)row["KZRBWB"],
					XANZUM = (string)row["XANZUM"],
					CTNCONCEPT = (string)row["CTNCONCEPT"],
					KZSRV = (string)row["KZSRV"],
					XXINVE = (string)row["XXINVE"],
					SUREG = (string)row["SUREG"],
					LANDGRP_VP = (string)row["LANDGRP_VP"],

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

			public static IEnumerable<T005_WA> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T005_WA> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<T005_WA> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T005_WA).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T005_WA> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<T005_WA> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T005_WA> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T005_WA>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("ISA_ADDR_POSTAL_CODE_CHECK", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T005_WA> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T005_WA>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T005_WA> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T005_WA>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class T005_WA_PO_BOX : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string LAND1 { get; set; }

			public string LANDK { get; set; }

			public string LNPLZ { get; set; }

			public string PRPLZ { get; set; }

			public string ADDRS { get; set; }

			public string XPLZS { get; set; }

			public string XPLPF { get; set; }

			public string SPRAS { get; set; }

			public string XLAND { get; set; }

			public string XADDR { get; set; }

			public string NMFMT { get; set; }

			public string XREGS { get; set; }

			public string XPLST { get; set; }

			public string INTCA { get; set; }

			public string INTCA3 { get; set; }

			public string INTCN3 { get; set; }

			public string XEGLD { get; set; }

			public string XSKFN { get; set; }

			public string XMWSN { get; set; }

			public string LNBKN { get; set; }

			public string PRBKN { get; set; }

			public string LNBLZ { get; set; }

			public string PRBLZ { get; set; }

			public string LNPSK { get; set; }

			public string PRPSK { get; set; }

			public string XPRBK { get; set; }

			public string BNKEY { get; set; }

			public string LNBKS { get; set; }

			public string PRBKS { get; set; }

			public string XPRSO { get; set; }

			public string PRUIN { get; set; }

			public string UINLN { get; set; }

			public string LNST1 { get; set; }

			public string PRST1 { get; set; }

			public string LNST2 { get; set; }

			public string PRST2 { get; set; }

			public string LNST3 { get; set; }

			public string PRST3 { get; set; }

			public string LNST4 { get; set; }

			public string PRST4 { get; set; }

			public string LNST5 { get; set; }

			public string PRST5 { get; set; }

			public string LANDD { get; set; }

			public string KALSM { get; set; }

			public string LANDA { get; set; }

			public string WECHF { get; set; }

			public string LKVRZ { get; set; }

			public string INTCN { get; set; }

			public string XDEZP { get; set; }

			public string DATFM { get; set; }

			public string CURIN { get; set; }

			public string CURHA { get; set; }

			public string WAERS { get; set; }

			public string KURST { get; set; }

			public string AFAPL { get; set; }

			public decimal? GWGWRT { get; set; }

			public decimal? UMRWRT { get; set; }

			public string KZRBWB { get; set; }

			public string XANZUM { get; set; }

			public string CTNCONCEPT { get; set; }

			public string KZSRV { get; set; }

			public string XXINVE { get; set; }

			public string SUREG { get; set; }

			public string LANDGRP_VP { get; set; }

			public static T005_WA_PO_BOX Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new T005_WA_PO_BOX
				{
					MANDT = (string)row["MANDT"],
					LAND1 = (string)row["LAND1"],
					LANDK = (string)row["LANDK"],
					LNPLZ = (string)row["LNPLZ"],
					PRPLZ = (string)row["PRPLZ"],
					ADDRS = (string)row["ADDRS"],
					XPLZS = (string)row["XPLZS"],
					XPLPF = (string)row["XPLPF"],
					SPRAS = (string)row["SPRAS"],
					XLAND = (string)row["XLAND"],
					XADDR = (string)row["XADDR"],
					NMFMT = (string)row["NMFMT"],
					XREGS = (string)row["XREGS"],
					XPLST = (string)row["XPLST"],
					INTCA = (string)row["INTCA"],
					INTCA3 = (string)row["INTCA3"],
					INTCN3 = (string)row["INTCN3"],
					XEGLD = (string)row["XEGLD"],
					XSKFN = (string)row["XSKFN"],
					XMWSN = (string)row["XMWSN"],
					LNBKN = (string)row["LNBKN"],
					PRBKN = (string)row["PRBKN"],
					LNBLZ = (string)row["LNBLZ"],
					PRBLZ = (string)row["PRBLZ"],
					LNPSK = (string)row["LNPSK"],
					PRPSK = (string)row["PRPSK"],
					XPRBK = (string)row["XPRBK"],
					BNKEY = (string)row["BNKEY"],
					LNBKS = (string)row["LNBKS"],
					PRBKS = (string)row["PRBKS"],
					XPRSO = (string)row["XPRSO"],
					PRUIN = (string)row["PRUIN"],
					UINLN = (string)row["UINLN"],
					LNST1 = (string)row["LNST1"],
					PRST1 = (string)row["PRST1"],
					LNST2 = (string)row["LNST2"],
					PRST2 = (string)row["PRST2"],
					LNST3 = (string)row["LNST3"],
					PRST3 = (string)row["PRST3"],
					LNST4 = (string)row["LNST4"],
					PRST4 = (string)row["PRST4"],
					LNST5 = (string)row["LNST5"],
					PRST5 = (string)row["PRST5"],
					LANDD = (string)row["LANDD"],
					KALSM = (string)row["KALSM"],
					LANDA = (string)row["LANDA"],
					WECHF = (string)row["WECHF"],
					LKVRZ = (string)row["LKVRZ"],
					INTCN = (string)row["INTCN"],
					XDEZP = (string)row["XDEZP"],
					DATFM = (string)row["DATFM"],
					CURIN = (string)row["CURIN"],
					CURHA = (string)row["CURHA"],
					WAERS = (string)row["WAERS"],
					KURST = (string)row["KURST"],
					AFAPL = (string)row["AFAPL"],
					GWGWRT = string.IsNullOrEmpty(row["GWGWRT"].ToString()) ? null : (decimal?)row["GWGWRT"],
					UMRWRT = string.IsNullOrEmpty(row["UMRWRT"].ToString()) ? null : (decimal?)row["UMRWRT"],
					KZRBWB = (string)row["KZRBWB"],
					XANZUM = (string)row["XANZUM"],
					CTNCONCEPT = (string)row["CTNCONCEPT"],
					KZSRV = (string)row["KZSRV"],
					XXINVE = (string)row["XXINVE"],
					SUREG = (string)row["SUREG"],
					LANDGRP_VP = (string)row["LANDGRP_VP"],

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

			public static IEnumerable<T005_WA_PO_BOX> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T005_WA_PO_BOX> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<T005_WA_PO_BOX> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T005_WA_PO_BOX).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T005_WA_PO_BOX> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<T005_WA_PO_BOX> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T005_WA_PO_BOX> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T005_WA_PO_BOX>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("ISA_ADDR_POSTAL_CODE_CHECK", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T005_WA_PO_BOX> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T005_WA_PO_BOX>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T005_WA_PO_BOX> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T005_WA_PO_BOX>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<ISA_ADDR_POSTAL_CODE_CHECK.POSTAL_ADDRESS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<ISA_ADDR_POSTAL_CODE_CHECK.RETURN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<ISA_ADDR_POSTAL_CODE_CHECK.T005_WA> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<ISA_ADDR_POSTAL_CODE_CHECK.T005_WA_PO_BOX> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
