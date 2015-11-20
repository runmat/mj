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
	public partial class Z_ZLD_AH_2015_HOLUNDBRING_PDF
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_AH_2015_HOLUNDBRING_PDF).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_AH_2015_HOLUNDBRING_PDF).Name, inputParameterKeys, inputParameterValues);
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

		public partial class IS_DATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AUFTRAGSERSTELLER { get; set; }

			public string AUFTRAGERSTELLERTEL { get; set; }

			public string BETRIEBNAME { get; set; }

			public string BETRIEBSTRASSE { get; set; }

			public string BETRIEBHAUSNR { get; set; }

			public string BETRIEBPLZ { get; set; }

			public string BETRIEBORT { get; set; }

			public string REPCO { get; set; }

			public string ANSPRECHPARTNER { get; set; }

			public string ANSPRECHPARTNERTEL { get; set; }

			public string KUNDETEL { get; set; }

			public string KENNNZEICHEN { get; set; }

			public string FAHRZEUGART { get; set; }

			public string ABHOLUNGKUNDE { get; set; }

			public string ABHOLUNGSTRASSEHAUSNR { get; set; }

			public string ABHOLUNGPLZ { get; set; }

			public string ABHOLUNGORT { get; set; }

			public string ABHOLUNGANSPRECHPARTNER { get; set; }

			public string ABHOLUNGTEL { get; set; }

			public string ABHOLUNGDATETIME { get; set; }

			public string ABHOLUNGMOBILITAETSFAHRZEUG { get; set; }

			public string ABHOLUNGHINWEIS { get; set; }

			public string ANLIEFERUNGKUNDE { get; set; }

			public string ANLIEFERUNGSTRASSEHAUSNR { get; set; }

			public string ANLIEFERUNGPLZ { get; set; }

			public string ANLIEFERUNGORT { get; set; }

			public string ANLIEFERUNGANSPRECHPARTNER { get; set; }

			public string ANLIEFERUNGTEL { get; set; }

			public string ANLIEFERUNGABHOLUNGABDT { get; set; }

			public string ANLIEFERUNGANLIEFERUNGBISDT { get; set; }

			public string ANLIEFERUNGMOBILITAETSFAHRZEUG { get; set; }

			public string ANLIEFERUNGHINWEIS { get; set; }

			public static IS_DATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new IS_DATEN
				{
					AUFTRAGSERSTELLER = (string)row["AUFTRAGSERSTELLER"],
					AUFTRAGERSTELLERTEL = (string)row["AUFTRAGERSTELLERTEL"],
					BETRIEBNAME = (string)row["BETRIEBNAME"],
					BETRIEBSTRASSE = (string)row["BETRIEBSTRASSE"],
					BETRIEBHAUSNR = (string)row["BETRIEBHAUSNR"],
					BETRIEBPLZ = (string)row["BETRIEBPLZ"],
					BETRIEBORT = (string)row["BETRIEBORT"],
					REPCO = (string)row["REPCO"],
					ANSPRECHPARTNER = (string)row["ANSPRECHPARTNER"],
					ANSPRECHPARTNERTEL = (string)row["ANSPRECHPARTNERTEL"],
					KUNDETEL = (string)row["KUNDETEL"],
					KENNNZEICHEN = (string)row["KENNNZEICHEN"],
					FAHRZEUGART = (string)row["FAHRZEUGART"],
					ABHOLUNGKUNDE = (string)row["ABHOLUNGKUNDE"],
					ABHOLUNGSTRASSEHAUSNR = (string)row["ABHOLUNGSTRASSEHAUSNR"],
					ABHOLUNGPLZ = (string)row["ABHOLUNGPLZ"],
					ABHOLUNGORT = (string)row["ABHOLUNGORT"],
					ABHOLUNGANSPRECHPARTNER = (string)row["ABHOLUNGANSPRECHPARTNER"],
					ABHOLUNGTEL = (string)row["ABHOLUNGTEL"],
					ABHOLUNGDATETIME = (string)row["ABHOLUNGDATETIME"],
					ABHOLUNGMOBILITAETSFAHRZEUG = (string)row["ABHOLUNGMOBILITAETSFAHRZEUG"],
					ABHOLUNGHINWEIS = (string)row["ABHOLUNGHINWEIS"],
					ANLIEFERUNGKUNDE = (string)row["ANLIEFERUNGKUNDE"],
					ANLIEFERUNGSTRASSEHAUSNR = (string)row["ANLIEFERUNGSTRASSEHAUSNR"],
					ANLIEFERUNGPLZ = (string)row["ANLIEFERUNGPLZ"],
					ANLIEFERUNGORT = (string)row["ANLIEFERUNGORT"],
					ANLIEFERUNGANSPRECHPARTNER = (string)row["ANLIEFERUNGANSPRECHPARTNER"],
					ANLIEFERUNGTEL = (string)row["ANLIEFERUNGTEL"],
					ANLIEFERUNGABHOLUNGABDT = (string)row["ANLIEFERUNGABHOLUNGABDT"],
					ANLIEFERUNGANLIEFERUNGBISDT = (string)row["ANLIEFERUNGANLIEFERUNGBISDT"],
					ANLIEFERUNGMOBILITAETSFAHRZEUG = (string)row["ANLIEFERUNGMOBILITAETSFAHRZEUG"],
					ANLIEFERUNGHINWEIS = (string)row["ANLIEFERUNGHINWEIS"],

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

			public static IEnumerable<IS_DATEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static IEnumerable<IS_DATEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(IS_DATEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<IS_DATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IS_DATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_AH_2015_HOLUNDBRING_PDF", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IS_DATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IS_DATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_AH_2015_HOLUNDBRING_PDF.IS_DATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
