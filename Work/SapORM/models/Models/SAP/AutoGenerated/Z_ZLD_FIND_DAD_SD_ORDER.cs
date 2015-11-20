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
	public partial class Z_ZLD_FIND_DAD_SD_ORDER
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_FIND_DAD_SD_ORDER).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_FIND_DAD_SD_ORDER).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_BRIEF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BRIEF", value);
		}

		public void SetImportParameter_I_FAHRG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FAHRG", value);
		}

		public void SetImportParameter_I_VBELN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VBELN", value);
		}

		public void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public void SetImportParameter_I_VKORG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKORG", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class E_VBAK : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VBELN { get; set; }

			public string VKORG { get; set; }

			public string ZZSEND2 { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZKENN { get; set; }

			public string ZZREFNR { get; set; }

			public string ZZFAHRG { get; set; }

			public string EBELN { get; set; }

			public DateTime? VDATU { get; set; }

			public static E_VBAK Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new E_VBAK
				{
					VBELN = (string)row["VBELN"],
					VKORG = (string)row["VKORG"],
					ZZSEND2 = (string)row["ZZSEND2"],
					ZZBRIEF = (string)row["ZZBRIEF"],
					ZZKENN = (string)row["ZZKENN"],
					ZZREFNR = (string)row["ZZREFNR"],
					ZZFAHRG = (string)row["ZZFAHRG"],
					EBELN = (string)row["EBELN"],
					VDATU = string.IsNullOrEmpty(row["VDATU"].ToString()) ? null : (DateTime?)row["VDATU"],

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

			public static IEnumerable<E_VBAK> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<E_VBAK> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<E_VBAK> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(E_VBAK).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<E_VBAK> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<E_VBAK> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<E_VBAK> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<E_VBAK>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_FIND_DAD_SD_ORDER", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<E_VBAK> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<E_VBAK>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<E_VBAK> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<E_VBAK>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_FIND_DAD_SD_ORDER.E_VBAK> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
