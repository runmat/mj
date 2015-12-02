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
	public partial class Z_M_EC_AVM_ZULASSUNGSSPERRE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EC_AVM_ZULASSUNGSSPERRE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EC_AVM_ZULASSUNGSSPERRE).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public static void SetImportParameter_I_ZZAKTSPERRE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZAKTSPERRE", value);
		}

		public static void SetImportParameter_I_ZZCARPORT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZCARPORT", value);
		}

		public static void SetImportParameter_I_ZZDATBEM(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZDATBEM", value);
		}

		public static void SetImportParameter_I_ZZFAHRG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZFAHRG", value);
		}

		public static string GetExportParameter_E_EQUNR(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_EQUNR").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_QMNUM(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_QMNUM").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_TEXTE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string TDFORMAT { get; set; }

			public string TDLINE { get; set; }

			public static GT_TEXTE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_TEXTE
				{
					TDFORMAT = (string)row["TDFORMAT"],
					TDLINE = (string)row["TDLINE"],

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

			public static IEnumerable<GT_TEXTE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_TEXTE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_TEXTE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_TEXTE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_TEXTE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXTE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_TEXTE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TEXTE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_ZULASSUNGSSPERRE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXTE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TEXTE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXTE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TEXTE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXTE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_TEXTE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_ZULASSUNGSSPERRE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_TEXTE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_TEXTE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_ZULASSUNGSSPERRE.GT_TEXTE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
