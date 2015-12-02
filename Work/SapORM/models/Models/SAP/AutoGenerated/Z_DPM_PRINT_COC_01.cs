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
	public partial class Z_DPM_PRINT_COC_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_PRINT_COC_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_PRINT_COC_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_PDF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PDF", value);
		}

		public static void SetImportParameter_I_SAP_PREVIEW(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SAP_PREVIEW", value);
		}

		public static void SetImportParameter_I_VKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKZ", value);
		}

		public static void SetImportParameter_I_VORG_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORG_NR", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static byte[] GetExportParameter_E_PDF(ISapDataService sap)
		{
			return sap.GetExportParameter<byte[]>("E_PDF");
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class I_OUTPARMS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string DEVICE { get; set; }

			public string NODIALOG { get; set; }

			public string PREVIEW { get; set; }

			public string GETPDF { get; set; }

			public string GETPDL { get; set; }

			public string GETXML { get; set; }

			public string CONNECTION { get; set; }

			public string ADSTRLEVEL { get; set; }

			public string JOB_PROFILE { get; set; }

			public string BUMODE { get; set; }

			public string ASSEMBLE { get; set; }

			public string PARALLEL { get; set; }

			public string PDFVERSION { get; set; }

			public string PDFTAGGED { get; set; }

			public string PDFCHANGESRESTRICTED { get; set; }

			public string PDFNORM { get; set; }

			public string DEST { get; set; }

			public string REQNEW { get; set; }

			public string REQIMM { get; set; }

			public string REQDEL { get; set; }

			public string REQFINAL { get; set; }

			public int? SPOOLID { get; set; }

			public DateTime? SENDDATE { get; set; }

			public string SENDTIME { get; set; }

			public string SCHEDULE { get; set; }

			public string COPIES { get; set; }

			public string DATASET { get; set; }

			public string SUFFIX1 { get; set; }

			public string SUFFIX2 { get; set; }

			public string COVTITLE { get; set; }

			public string COVER { get; set; }

			public string RECEIVER { get; set; }

			public string DIVISION { get; set; }

			public string LIFETIME { get; set; }

			public string AUTHORITY { get; set; }

			public string RQPOSNAME { get; set; }

			public string PDLTYPE { get; set; }

			public string XDCNAME { get; set; }

			public string XDCOWNER { get; set; }

			public string NOPDF { get; set; }

			public string SPONUMIV { get; set; }

			public string PRINTTICKET { get; set; }

			public string ARCMODE { get; set; }

			public string NOARMCH { get; set; }

			public string TITLE { get; set; }

			public string NOPREVIEW { get; set; }

			public string NOPRINT { get; set; }

			public string NOARCHIVE { get; set; }

			public string IMMEXIT { get; set; }

			public string NOPRIBUTT { get; set; }

			public string XFP { get; set; }

			public string XFPTYPE { get; set; }

			public string XFPOUTDEV { get; set; }

			public static I_OUTPARMS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new I_OUTPARMS
				{
					DEVICE = (string)row["DEVICE"],
					NODIALOG = (string)row["NODIALOG"],
					PREVIEW = (string)row["PREVIEW"],
					GETPDF = (string)row["GETPDF"],
					GETPDL = (string)row["GETPDL"],
					GETXML = (string)row["GETXML"],
					CONNECTION = (string)row["CONNECTION"],
					ADSTRLEVEL = (string)row["ADSTRLEVEL"],
					JOB_PROFILE = (string)row["JOB_PROFILE"],
					BUMODE = (string)row["BUMODE"],
					ASSEMBLE = (string)row["ASSEMBLE"],
					PARALLEL = (string)row["PARALLEL"],
					PDFVERSION = (string)row["PDFVERSION"],
					PDFTAGGED = (string)row["PDFTAGGED"],
					PDFCHANGESRESTRICTED = (string)row["PDFCHANGESRESTRICTED"],
					PDFNORM = (string)row["PDFNORM"],
					DEST = (string)row["DEST"],
					REQNEW = (string)row["REQNEW"],
					REQIMM = (string)row["REQIMM"],
					REQDEL = (string)row["REQDEL"],
					REQFINAL = (string)row["REQFINAL"],
					SPOOLID = string.IsNullOrEmpty(row["SPOOLID"].ToString()) ? null : (int?)row["SPOOLID"],
					SENDDATE = string.IsNullOrEmpty(row["SENDDATE"].ToString()) ? null : (DateTime?)row["SENDDATE"],
					SENDTIME = (string)row["SENDTIME"],
					SCHEDULE = (string)row["SCHEDULE"],
					COPIES = (string)row["COPIES"],
					DATASET = (string)row["DATASET"],
					SUFFIX1 = (string)row["SUFFIX1"],
					SUFFIX2 = (string)row["SUFFIX2"],
					COVTITLE = (string)row["COVTITLE"],
					COVER = (string)row["COVER"],
					RECEIVER = (string)row["RECEIVER"],
					DIVISION = (string)row["DIVISION"],
					LIFETIME = (string)row["LIFETIME"],
					AUTHORITY = (string)row["AUTHORITY"],
					RQPOSNAME = (string)row["RQPOSNAME"],
					PDLTYPE = (string)row["PDLTYPE"],
					XDCNAME = (string)row["XDCNAME"],
					XDCOWNER = (string)row["XDCOWNER"],
					NOPDF = (string)row["NOPDF"],
					SPONUMIV = (string)row["SPONUMIV"],
					PRINTTICKET = (string)row["PRINTTICKET"],
					ARCMODE = (string)row["ARCMODE"],
					NOARMCH = (string)row["NOARMCH"],
					TITLE = (string)row["TITLE"],
					NOPREVIEW = (string)row["NOPREVIEW"],
					NOPRINT = (string)row["NOPRINT"],
					NOARCHIVE = (string)row["NOARCHIVE"],
					IMMEXIT = (string)row["IMMEXIT"],
					NOPRIBUTT = (string)row["NOPRIBUTT"],
					XFP = (string)row["XFP"],
					XFPTYPE = (string)row["XFPTYPE"],
					XFPOUTDEV = (string)row["XFPOUTDEV"],

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

			public static IEnumerable<I_OUTPARMS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static IEnumerable<I_OUTPARMS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(I_OUTPARMS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<I_OUTPARMS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<I_OUTPARMS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_PRINT_COC_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<I_OUTPARMS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<I_OUTPARMS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_PRINT_COC_01.I_OUTPARMS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
