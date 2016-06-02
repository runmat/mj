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
	public partial class Z_DPM_CD_ABGL_ABMELDUNGEN
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_CD_ABGL_ABMELDUNGEN).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_CD_ABGL_ABMELDUNGEN).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_ANLAGEDATUM(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ANLAGEDATUM", value);
		}

		public partial class ET_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR_AG { get; set; }

			public DateTime? ANLAGEDATUM { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static ET_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				ET_OUT o;

				try
				{
					o = new ET_OUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						KUNNR_AG = (string)row["KUNNR_AG"],
						ANLAGEDATUM = string.IsNullOrEmpty(row["ANLAGEDATUM"].ToString()) ? null : (DateTime?)row["ANLAGEDATUM"],
						CHASSIS_NUM = (string)row["CHASSIS_NUM"],
						LICENSE_NUM = (string)row["LICENSE_NUM"],
					};
				}
				catch(Exception e)
				{
					o = new ET_OUT
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

			public static IEnumerable<ET_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ET_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_CD_ABGL_ABMELDUNGEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_CD_ABGL_ABMELDUNGEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_CD_ABGL_ABMELDUNGEN.ET_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
