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
	public partial class Z_DPM_EVENT_READ_SCHAD_STAT_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_EVENT_READ_SCHAD_STAT_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_EVENT_READ_SCHAD_STAT_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public static void SetImportParameter_I_PROZESSNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PROZESSNR", value);
		}

		public static void SetImportParameter_I_SCHADEN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SCHADEN", value);
		}

		public static void SetImportParameter_I_SPRAS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_SPRAS", value);
		}

		public partial class GT_STATART : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string STATUSART { get; set; }

			public string PROZESSNR { get; set; }

			public string NAME { get; set; }

			public string REIHENFOLGE { get; set; }

			public string OPTIONAL { get; set; }

			public string TEXT { get; set; }

			public string FARBE_STATUS { get; set; }

			public static GT_STATART Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_STATART
				{
					STATUSART = (string)row["STATUSART"],
					PROZESSNR = (string)row["PROZESSNR"],
					NAME = (string)row["NAME"],
					REIHENFOLGE = (string)row["REIHENFOLGE"],
					OPTIONAL = (string)row["OPTIONAL"],
					TEXT = (string)row["TEXT"],
					FARBE_STATUS = (string)row["FARBE_STATUS"],

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

			public static IEnumerable<GT_STATART> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_STATART> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_STATART> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_STATART).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_STATART> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATART> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_STATART> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_STATART>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EVENT_READ_SCHAD_STAT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATART> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_STATART>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATART> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_STATART>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATART> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_STATART>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EVENT_READ_SCHAD_STAT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATART> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_STATART>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_STATUS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EVENT_SCHADEN { get; set; }

			public string REIHENFOLGE { get; set; }

			public string STATUSART { get; set; }

			public DateTime? DATUM { get; set; }

			public string UZEIT { get; set; }

			public string BENUTZER { get; set; }

			public string TEXT { get; set; }

			public string FARBE_STATUS { get; set; }

			public static GT_STATUS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_STATUS
				{
					EVENT_SCHADEN = (string)row["EVENT_SCHADEN"],
					REIHENFOLGE = (string)row["REIHENFOLGE"],
					STATUSART = (string)row["STATUSART"],
					DATUM = string.IsNullOrEmpty(row["DATUM"].ToString()) ? null : (DateTime?)row["DATUM"],
					UZEIT = (string)row["UZEIT"],
					BENUTZER = (string)row["BENUTZER"],
					TEXT = (string)row["TEXT"],
					FARBE_STATUS = (string)row["FARBE_STATUS"],

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

			public static IEnumerable<GT_STATUS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_STATUS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_STATUS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_STATUS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_STATUS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATUS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_STATUS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_STATUS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EVENT_READ_SCHAD_STAT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATUS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_STATUS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATUS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_STATUS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATUS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_STATUS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EVENT_READ_SCHAD_STAT_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_STATUS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_STATUS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_EVENT_READ_SCHAD_STAT_01.GT_STATART> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DPM_EVENT_READ_SCHAD_STAT_01.GT_STATUS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
