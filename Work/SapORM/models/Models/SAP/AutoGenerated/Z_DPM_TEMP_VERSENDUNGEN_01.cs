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
	public partial class Z_DPM_TEMP_VERSENDUNGEN_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_TEMP_VERSENDUNGEN_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_TEMP_VERSENDUNGEN_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public static void SetImportParameter_I_LICENSE_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LICENSE_NUM", value);
		}

		public static void SetImportParameter_I_LIZNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIZNR", value);
		}

		public static void SetImportParameter_I_ZZREFERENZ1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFERENZ1", value);
		}

		public static void SetImportParameter_I_ZZTMPDT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZTMPDT_BIS", value);
		}

		public static void SetImportParameter_I_ZZTMPDT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZTMPDT_VON", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LIZNR { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_OUT o;

				try
				{
					o = new GT_OUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						EQUNR = (string)row["EQUNR"],
						CHASSIS_NUM = (string)row["CHASSIS_NUM"],
						LICENSE_NUM = (string)row["LICENSE_NUM"],
						LIZNR = (string)row["LIZNR"],
						ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
						ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_OUT
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

			public static IEnumerable<GT_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_TEMP_VERSENDUNGEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_TEMP_VERSENDUNGEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_TEMP_VERSENDUNGEN_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
