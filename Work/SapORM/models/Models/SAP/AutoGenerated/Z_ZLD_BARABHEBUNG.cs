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
	public partial class Z_ZLD_BARABHEBUNG
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_BARABHEBUNG).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_BARABHEBUNG).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_AUSGABE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("AUSGABE", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public byte[] GetExportParameter_E_PDF(ISapDataService sap)
		{
			return sap.GetExportParameter<byte[]>("E_PDF");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class IS_BARABHEBUNG : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VKBUR { get; set; }

			public string NAME { get; set; }

			public string EC_KARTE_NR { get; set; }

			public DateTime? DATUM { get; set; }

			public string UZEIT { get; set; }

			public string ORT { get; set; }

			public decimal? BETRAG { get; set; }

			public string WAERS { get; set; }

			public static IS_BARABHEBUNG Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new IS_BARABHEBUNG
				{
					VKBUR = (string)row["VKBUR"],
					NAME = (string)row["NAME"],
					EC_KARTE_NR = (string)row["EC_KARTE_NR"],
					DATUM = string.IsNullOrEmpty(row["DATUM"].ToString()) ? null : (DateTime?)row["DATUM"],
					UZEIT = (string)row["UZEIT"],
					ORT = (string)row["ORT"],
					BETRAG = string.IsNullOrEmpty(row["BETRAG"].ToString()) ? null : (decimal?)row["BETRAG"],
					WAERS = (string)row["WAERS"],

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

			public static IEnumerable<IS_BARABHEBUNG> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static IEnumerable<IS_BARABHEBUNG> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(IS_BARABHEBUNG).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<IS_BARABHEBUNG> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IS_BARABHEBUNG>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_BARABHEBUNG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IS_BARABHEBUNG> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IS_BARABHEBUNG>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_BARABHEBUNG.IS_BARABHEBUNG> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
