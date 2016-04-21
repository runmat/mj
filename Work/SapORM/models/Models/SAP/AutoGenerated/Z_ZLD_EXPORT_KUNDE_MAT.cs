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
	public partial class Z_ZLD_EXPORT_KUNDE_MAT
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_KUNDE_MAT).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_KUNDE_MAT).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static void SetImportParameter_I_VKORG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKORG", value);
		}

		public partial class GT_EX_KUNDE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public string KUNNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string CITY1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string COUNTRY { get; set; }

			public string ZZPAUSCHAL { get; set; }

			public string OHNEUST { get; set; }

			public string XCPDK { get; set; }

			public string XCPDEIN { get; set; }

			public string KUNNR_LF { get; set; }

			public string KREISKZ_DIREKT { get; set; }

			public string EXTENSION1 { get; set; }

			public string BARKUNDE { get; set; }

			public string INAKTIV { get; set; }

			public string SOFORT_ABR { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_EX_KUNDE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_EX_KUNDE o;

				try
				{
					o = new GT_EX_KUNDE
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						VKORG = (string)row["VKORG"],
						VKBUR = (string)row["VKBUR"],
						KUNNR = (string)row["KUNNR"],
						NAME1 = (string)row["NAME1"],
						NAME2 = (string)row["NAME2"],
						CITY1 = (string)row["CITY1"],
						POST_CODE1 = (string)row["POST_CODE1"],
						STREET = (string)row["STREET"],
						HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
						COUNTRY = (string)row["COUNTRY"],
						ZZPAUSCHAL = (string)row["ZZPAUSCHAL"],
						OHNEUST = (string)row["OHNEUST"],
						XCPDK = (string)row["XCPDK"],
						XCPDEIN = (string)row["XCPDEIN"],
						KUNNR_LF = (string)row["KUNNR_LF"],
						KREISKZ_DIREKT = (string)row["KREISKZ_DIREKT"],
						EXTENSION1 = (string)row["EXTENSION1"],
						BARKUNDE = (string)row["BARKUNDE"],
						INAKTIV = (string)row["INAKTIV"],
						SOFORT_ABR = (string)row["SOFORT_ABR"],
					};
				}
				catch(Exception e)
				{
					o = new GT_EX_KUNDE
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

			public static IEnumerable<GT_EX_KUNDE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EX_KUNDE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EX_KUNDE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EX_KUNDE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EX_KUNDE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_KUNDE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EX_KUNDE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_KUNDE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_KUNDE_MAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_KUNDE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_KUNDE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_KUNDE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_KUNDE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_KUNDE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_KUNDE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_KUNDE_MAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_KUNDE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_KUNDE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_EX_MATERIAL : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public string KENNZREL { get; set; }

			public string ZZGEBPFLICHT { get; set; }

			public string GEBMAT { get; set; }

			public string GMAKTX { get; set; }

			public string GBAUST { get; set; }

			public string GUMAKTX { get; set; }

			public string KENNZMAT { get; set; }

			public string NULLPREIS_OK { get; set; }

			public string INAKTIV { get; set; }

			public string MENGE_ERL { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_EX_MATERIAL Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_EX_MATERIAL o;

				try
				{
					o = new GT_EX_MATERIAL
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						VKORG = (string)row["VKORG"],
						VKBUR = (string)row["VKBUR"],
						MATNR = (string)row["MATNR"],
						MAKTX = (string)row["MAKTX"],
						KENNZREL = (string)row["KENNZREL"],
						ZZGEBPFLICHT = (string)row["ZZGEBPFLICHT"],
						GEBMAT = (string)row["GEBMAT"],
						GMAKTX = (string)row["GMAKTX"],
						GBAUST = (string)row["GBAUST"],
						GUMAKTX = (string)row["GUMAKTX"],
						KENNZMAT = (string)row["KENNZMAT"],
						NULLPREIS_OK = (string)row["NULLPREIS_OK"],
						INAKTIV = (string)row["INAKTIV"],
						MENGE_ERL = (string)row["MENGE_ERL"],
					};
				}
				catch(Exception e)
				{
					o = new GT_EX_MATERIAL
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

			public static IEnumerable<GT_EX_MATERIAL> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_EX_MATERIAL> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_EX_MATERIAL> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_EX_MATERIAL).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_EX_MATERIAL> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_MATERIAL> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_EX_MATERIAL> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_MATERIAL>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_KUNDE_MAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_MATERIAL> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_MATERIAL>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_MATERIAL> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_MATERIAL>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_MATERIAL> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_EX_MATERIAL>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_KUNDE_MAT", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_EX_MATERIAL> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_EX_MATERIAL>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_KUNDE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_MATERIAL> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
