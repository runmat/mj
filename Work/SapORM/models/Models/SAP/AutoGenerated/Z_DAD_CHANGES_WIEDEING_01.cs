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
	public partial class Z_DAD_CHANGES_WIEDEING_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DAD_CHANGES_WIEDEING_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DAD_CHANGES_WIEDEING_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_QMART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_QMART", value);
		}

		public partial class IT_RG_ERDAT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string SIGN { get; set; }

			public string OPTION { get; set; }

			public DateTime? LOW { get; set; }

			public DateTime? HIGH { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static IT_RG_ERDAT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				IT_RG_ERDAT o;

				try
				{
					o = new IT_RG_ERDAT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						SIGN = (string)row["SIGN"],
						OPTION = (string)row["OPTION"],
						LOW = string.IsNullOrEmpty(row["LOW"].ToString()) ? null : (DateTime?)row["LOW"],
						HIGH = string.IsNullOrEmpty(row["HIGH"].ToString()) ? null : (DateTime?)row["HIGH"],
					};
				}
				catch(Exception e)
				{
					o = new IT_RG_ERDAT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,
					};
					o.OnMappingError(e, row, false);
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

			public static IEnumerable<IT_RG_ERDAT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static IEnumerable<IT_RG_ERDAT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(IT_RG_ERDAT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<IT_RG_ERDAT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IT_RG_ERDAT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DAD_CHANGES_WIEDEING_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IT_RG_ERDAT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IT_RG_ERDAT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class ET_CHG : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FIN { get; set; }

			public string LVNUMMER { get; set; }

			public string KZ_OLD { get; set; }

			public string KZ_NEW { get; set; }

			public string PARTNERNR_O { get; set; }

			public string NAME1_O { get; set; }

			public string ORT_O { get; set; }

			public string PARTNERNR_N { get; set; }

			public string NAME1_N { get; set; }

			public string ORT_N { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string EQUNR { get; set; }

			public string QMNUM_OLD { get; set; }

			public string QMNUM_NEW { get; set; }

			public string OBJNR_MELD_OLD { get; set; }

			public string OBJNR_MELD_NEW { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static ET_CHG Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				ET_CHG o;

				try
				{
					o = new ET_CHG
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FIN = (string)row["FIN"],
						LVNUMMER = (string)row["LVNUMMER"],
						KZ_OLD = (string)row["KZ_OLD"],
						KZ_NEW = (string)row["KZ_NEW"],
						PARTNERNR_O = (string)row["PARTNERNR_O"],
						NAME1_O = (string)row["NAME1_O"],
						ORT_O = (string)row["ORT_O"],
						PARTNERNR_N = (string)row["PARTNERNR_N"],
						NAME1_N = (string)row["NAME1_N"],
						ORT_N = (string)row["ORT_N"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						EQUNR = (string)row["EQUNR"],
						QMNUM_OLD = (string)row["QMNUM_OLD"],
						QMNUM_NEW = (string)row["QMNUM_NEW"],
						OBJNR_MELD_OLD = (string)row["OBJNR_MELD_OLD"],
						OBJNR_MELD_NEW = (string)row["OBJNR_MELD_NEW"],
					};
				}
				catch(Exception e)
				{
					o = new ET_CHG
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

			public static IEnumerable<ET_CHG> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ET_CHG> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ET_CHG> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ET_CHG).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ET_CHG> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ET_CHG> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ET_CHG> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_CHG>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DAD_CHANGES_WIEDEING_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_CHG> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_CHG>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_CHG> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_CHG>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_CHG> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ET_CHG>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DAD_CHANGES_WIEDEING_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ET_CHG> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ET_CHG>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DAD_CHANGES_WIEDEING_01.IT_RG_ERDAT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_DAD_CHANGES_WIEDEING_01.ET_CHG> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
