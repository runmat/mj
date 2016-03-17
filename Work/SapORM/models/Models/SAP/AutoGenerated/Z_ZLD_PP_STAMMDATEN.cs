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
	public partial class Z_ZLD_PP_STAMMDATEN
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_PP_STAMMDATEN).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_PP_STAMMDATEN).Name, inputParameterKeys, inputParameterValues);
		}


		public partial class EXP_GRUENDE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string PP_STATUS { get; set; }

			public string GRUND_KEY { get; set; }

			public string GRUND { get; set; }

			public string GR_LANGTEXT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static EXP_GRUENDE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				EXP_GRUENDE o;

				try
				{
					o = new EXP_GRUENDE
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						PP_STATUS = (string)row["PP_STATUS"],
						GRUND_KEY = (string)row["GRUND_KEY"],
						GRUND = (string)row["GRUND"],
						GR_LANGTEXT = (string)row["GR_LANGTEXT"],
					};
				}
				catch(Exception e)
				{
					o = new EXP_GRUENDE
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

			public static IEnumerable<EXP_GRUENDE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<EXP_GRUENDE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<EXP_GRUENDE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(EXP_GRUENDE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<EXP_GRUENDE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<EXP_GRUENDE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<EXP_GRUENDE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EXP_GRUENDE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_PP_STAMMDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<EXP_GRUENDE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EXP_GRUENDE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<EXP_GRUENDE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EXP_GRUENDE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<EXP_GRUENDE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EXP_GRUENDE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_PP_STAMMDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<EXP_GRUENDE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EXP_GRUENDE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class EXP_MATERIAL : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public string ZZMAT_1010 { get; set; }

			public string ZZMAT_1510 { get; set; }

			public string ZZMAT_PREIS { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static EXP_MATERIAL Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				EXP_MATERIAL o;

				try
				{
					o = new EXP_MATERIAL
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						MATNR = (string)row["MATNR"],
						MAKTX = (string)row["MAKTX"],
						ZZMAT_1010 = (string)row["ZZMAT_1010"],
						ZZMAT_1510 = (string)row["ZZMAT_1510"],
						ZZMAT_PREIS = (string)row["ZZMAT_PREIS"],
					};
				}
				catch(Exception e)
				{
					o = new EXP_MATERIAL
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

			public static IEnumerable<EXP_MATERIAL> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<EXP_MATERIAL> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<EXP_MATERIAL> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(EXP_MATERIAL).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<EXP_MATERIAL> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<EXP_MATERIAL> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<EXP_MATERIAL> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EXP_MATERIAL>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_PP_STAMMDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<EXP_MATERIAL> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EXP_MATERIAL>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<EXP_MATERIAL> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EXP_MATERIAL>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<EXP_MATERIAL> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<EXP_MATERIAL>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_PP_STAMMDATEN", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<EXP_MATERIAL> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<EXP_MATERIAL>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_PP_STAMMDATEN.EXP_GRUENDE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_PP_STAMMDATEN.EXP_MATERIAL> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
