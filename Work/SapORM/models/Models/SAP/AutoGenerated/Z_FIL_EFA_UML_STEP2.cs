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
	public partial class Z_FIL_EFA_UML_STEP2
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_FIL_EFA_UML_STEP2).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_FIL_EFA_UML_STEP2).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_BELNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BELNR", value);
		}

		public static void SetImportParameter_I_BUDAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_BUDAT", value);
		}

		public static void SetImportParameter_I_KOSTL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KOSTL", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_OFF_UML_POS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BELNR { get; set; }

			public string POSNR { get; set; }

			public string MATNR { get; set; }

			public decimal? MENGE { get; set; }

			public string MAKTX { get; set; }

			public DateTime? BUDAT { get; set; }

			public string EAN11 { get; set; }

			public string TEXT { get; set; }

			public string LTEXT_NR { get; set; }

			public string KENNZFORM { get; set; }

			public static GT_OFF_UML_POS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OFF_UML_POS
				{
					BELNR = (string)row["BELNR"],
					POSNR = (string)row["POSNR"],
					MATNR = (string)row["MATNR"],
					MENGE = string.IsNullOrEmpty(row["MENGE"].ToString()) ? null : (decimal?)row["MENGE"],
					MAKTX = (string)row["MAKTX"],
					BUDAT = string.IsNullOrEmpty(row["BUDAT"].ToString()) ? null : (DateTime?)row["BUDAT"],
					EAN11 = (string)row["EAN11"],
					TEXT = (string)row["TEXT"],
					LTEXT_NR = (string)row["LTEXT_NR"],
					KENNZFORM = (string)row["KENNZFORM"],

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

			public static IEnumerable<GT_OFF_UML_POS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OFF_UML_POS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_OFF_UML_POS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OFF_UML_POS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OFF_UML_POS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OFF_UML_POS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OFF_UML_POS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OFF_UML_POS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_FIL_EFA_UML_STEP2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OFF_UML_POS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OFF_UML_POS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OFF_UML_POS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OFF_UML_POS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OFF_UML_POS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OFF_UML_POS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_FIL_EFA_UML_STEP2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OFF_UML_POS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OFF_UML_POS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_FIL_EFA_UML_STEP2.GT_OFF_UML_POS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
