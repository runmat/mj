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
	public partial class Z_DPM_GET_ZZSEND2
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_GET_ZZSEND2).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_GET_ZZSEND2).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_AUART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("AUART", value);
		}

		public static void SetImportParameter_AUGRU(ISapDataService sap, string value)
		{
			sap.SetImportParameter("AUGRU", value);
		}

		public static void SetImportParameter_CHECK_SEND2(ISapDataService sap, string value)
		{
			sap.SetImportParameter("CHECK_SEND2", value);
		}

		public static void SetImportParameter_ERDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("ERDAT_BIS", value);
		}

		public static void SetImportParameter_ERDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("ERDAT_VON", value);
		}

		public static void SetImportParameter_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("KUNNR_AG", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VBELN { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZKENN { get; set; }

			public DateTime? VDATU { get; set; }

			public string ZZSEND2 { get; set; }

			public string ZZREFNR { get; set; }

			public string ZZDIEN2 { get; set; }

			public string FRACHRFUEHRER { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_WEB o;

				try
				{
					o = new GT_WEB
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						VBELN = (string)row["VBELN"],
						ERDAT = string.IsNullOrEmpty(row["ERDAT"].ToString()) ? null : (DateTime?)row["ERDAT"],
						ZZFAHRG = (string)row["ZZFAHRG"],
						ZZKENN = (string)row["ZZKENN"],
						VDATU = string.IsNullOrEmpty(row["VDATU"].ToString()) ? null : (DateTime?)row["VDATU"],
						ZZSEND2 = (string)row["ZZSEND2"],
						ZZREFNR = (string)row["ZZREFNR"],
						ZZDIEN2 = (string)row["ZZDIEN2"],
						FRACHRFUEHRER = (string)row["FRACHRFUEHRER"],
					};
				}
				catch(Exception e)
				{
					o = new GT_WEB
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

			public static IEnumerable<GT_WEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_GET_ZZSEND2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_GET_ZZSEND2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_GET_ZZSEND2.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
