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
	public partial class Z_ZLD_CHECK_48H
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_CHECK_48H).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_CHECK_48H).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_DATUM_AUSFUEHRUNG(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM_AUSFUEHRUNG", value);
		}

		public static void SetImportParameter_I_DATUM_BEAUFTRAGUNG(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATUM_BEAUFTRAGUNG", value);
		}

		public static void SetImportParameter_I_KREISKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KREISKZ", value);
		}

		public static void SetImportParameter_I_LIFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIFNR", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class ES_VERSAND_48H : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STREET { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string LIFUHRBIS { get; set; }

			public string NACHREICH { get; set; }

			public string Z48H { get; set; }

			public string ABW_ADR_GENERELL { get; set; }

			public string IST_48H { get; set; }

			public static ES_VERSAND_48H Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ES_VERSAND_48H
				{
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STREET = (string)row["STREET"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					LIFUHRBIS = (string)row["LIFUHRBIS"],
					NACHREICH = (string)row["NACHREICH"],
					Z48H = (string)row["Z48H"],
					ABW_ADR_GENERELL = (string)row["ABW_ADR_GENERELL"],
					IST_48H = (string)row["IST_48H"],

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

			public static IEnumerable<ES_VERSAND_48H> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ES_VERSAND_48H> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ES_VERSAND_48H> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ES_VERSAND_48H).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ES_VERSAND_48H> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ES_VERSAND_48H> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ES_VERSAND_48H> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ES_VERSAND_48H>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_CHECK_48H", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_VERSAND_48H> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_VERSAND_48H>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_VERSAND_48H> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_VERSAND_48H>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class ET_MESSAGE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public int? SUBRC { get; set; }

			public string MESSAGE { get; set; }

			public string GRUND { get; set; }

			public static ET_MESSAGE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ET_MESSAGE
				{
					SUBRC = string.IsNullOrEmpty(row["SUBRC"].ToString()) ? null : (int?)row["SUBRC"],
					MESSAGE = (string)row["MESSAGE"],
					GRUND = (string)row["GRUND"],

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

			public static IEnumerable<ET_MESSAGE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_MESSAGE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ET_MESSAGE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_MESSAGE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_MESSAGE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ET_MESSAGE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_MESSAGE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_MESSAGE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_CHECK_48H", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_MESSAGE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_MESSAGE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_MESSAGE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_MESSAGE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_MESSAGE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_MESSAGE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_CHECK_48H", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_MESSAGE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_MESSAGE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_CHECK_48H.ES_VERSAND_48H> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_CHECK_48H.ET_MESSAGE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
