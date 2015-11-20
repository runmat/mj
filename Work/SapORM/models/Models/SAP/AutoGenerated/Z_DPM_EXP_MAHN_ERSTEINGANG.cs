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
	public partial class Z_DPM_EXP_MAHN_ERSTEINGANG
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_EXP_MAHN_ERSTEINGANG).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_EXP_MAHN_ERSTEINGANG).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_MAHNSTUFE1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MAHNSTUFE1", value);
		}

		public void SetImportParameter_I_MAHNSTUFE2(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MAHNSTUFE2", value);
		}

		public void SetImportParameter_I_MAHNSTUFE3(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MAHNSTUFE3", value);
		}

		public void SetImportParameter_I_MASPER_GES(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_MASPER_GES", value);
		}

		public void SetImportParameter_I_ZVERT_ART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZVERT_ART", value);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KONTONR { get; set; }

			public string CIN { get; set; }

			public string PAID { get; set; }

			public string ZVERT_ART { get; set; }

			public string MAKTX { get; set; }

			public string ZZMAHNS { get; set; }

			public string ZZ_MAHNA { get; set; }

			public string MANSP { get; set; }

			public DateTime? MASPB { get; set; }

			public DateTime? MADAT { get; set; }

			public DateTime? MNDAT { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					KONTONR = (string)row["KONTONR"],
					CIN = (string)row["CIN"],
					PAID = (string)row["PAID"],
					ZVERT_ART = (string)row["ZVERT_ART"],
					MAKTX = (string)row["MAKTX"],
					ZZMAHNS = (string)row["ZZMAHNS"],
					ZZ_MAHNA = (string)row["ZZ_MAHNA"],
					MANSP = (string)row["MANSP"],
					MASPB = string.IsNullOrEmpty(row["MASPB"].ToString()) ? null : (DateTime?)row["MASPB"],
					MADAT = string.IsNullOrEmpty(row["MADAT"].ToString()) ? null : (DateTime?)row["MADAT"],
					MNDAT = string.IsNullOrEmpty(row["MNDAT"].ToString()) ? null : (DateTime?)row["MNDAT"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_EXP_MAHN_ERSTEINGANG", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_EXP_MAHN_ERSTEINGANG", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_EXP_MAHN_ERSTEINGANG.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
