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
	public partial class Z_ZLD_MOB_DISPO_SET_USER
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_MOB_DISPO_SET_USER).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_MOB_DISPO_SET_USER).Name, inputParameterKeys, inputParameterValues);
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

			public static GT_VGANZ Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_VGANZ
				{
					AMT = (string)row["AMT"],
					KREISBEZ = (string)row["KREISBEZ"],
					VG_ANZ = string.IsNullOrEmpty(row["VG_ANZ"].ToString()) ? null : (int?)row["VG_ANZ"],
					MOBUSER = (string)row["MOBUSER"],
					NAME = (string)row["NAME"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_MOB_DISPO_SET_USER", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_MOB_DISPO_SET_USER", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_ZLD_MOB_DISPO_SET_USER.GT_VGANZ> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
