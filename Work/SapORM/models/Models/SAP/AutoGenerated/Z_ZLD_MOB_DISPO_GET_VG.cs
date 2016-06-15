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
	public partial class Z_ZLD_MOB_DISPO_GET_VG
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_MOB_DISPO_GET_VG).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_MOB_DISPO_GET_VG).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_ALLE_AEMTER(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ALLE_AEMTER", value);
		}

		public static void SetImportParameter_I_FUNCTION(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FUNCTION", value);
		}

		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static void SetImportParameter_I_VKORG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKORG", value);
		}

		public static void SetImportParameter_I_ZZZLDAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZZLDAT", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_VGANZ : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AMT { get; set; }

			public string KREISBEZ { get; set; }

			public int? VG_ANZ { get; set; }

			public string MOBUSER { get; set; }

			public string NAME { get; set; }

			public string MOB_AKTIV { get; set; }

			public string NO_MOB_AKTIV { get; set; }

			public decimal? GEB_AMT { get; set; }

			public string HINWEIS { get; set; }

			public string VORSCHUSS { get; set; }

			public decimal? VORSCHUSS_BETRAG { get; set; }

			public string WAERS { get; set; }

			public int? SUBRC { get; set; }

			public string MESSAGE { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_VGANZ Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_VGANZ o;

				try
				{
					o = new GT_VGANZ
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						AMT = (string)row["AMT"],
						KREISBEZ = (string)row["KREISBEZ"],
						VG_ANZ = string.IsNullOrEmpty(row["VG_ANZ"].ToString()) ? null : (int?)row["VG_ANZ"],
						MOBUSER = (string)row["MOBUSER"],
						NAME = (string)row["NAME"],
						MOB_AKTIV = (string)row["MOB_AKTIV"],
						NO_MOB_AKTIV = (string)row["NO_MOB_AKTIV"],
						GEB_AMT = string.IsNullOrEmpty(row["GEB_AMT"].ToString()) ? null : (decimal?)row["GEB_AMT"],
						HINWEIS = (string)row["HINWEIS"],
						VORSCHUSS = (string)row["VORSCHUSS"],
						VORSCHUSS_BETRAG = string.IsNullOrEmpty(row["VORSCHUSS_BETRAG"].ToString()) ? null : (decimal?)row["VORSCHUSS_BETRAG"],
						WAERS = (string)row["WAERS"],
						SUBRC = string.IsNullOrEmpty(row["SUBRC"].ToString()) ? null : (int?)row["SUBRC"],
						MESSAGE = (string)row["MESSAGE"],
					};
				}
				catch(Exception e)
				{
					o = new GT_VGANZ
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

			public static IEnumerable<GT_VGANZ> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_VGANZ> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_VGANZ> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_VGANZ).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_VGANZ> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_VGANZ> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_VGANZ> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VGANZ>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_MOB_DISPO_GET_VG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VGANZ> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VGANZ>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VGANZ> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VGANZ>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VGANZ> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_VGANZ>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_MOB_DISPO_GET_VG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_VGANZ> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_VGANZ>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_MOB_DISPO_GET_VG.GT_VGANZ> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
