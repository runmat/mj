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
	public partial class Z_DPM_CD_STRAFZETTEL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_CD_STRAFZETTEL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_CD_STRAFZETTEL).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_DATBEHO_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATBEHO_BIS", value);
		}

		public void SetImportParameter_I_DATBEHO_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_DATBEHO_VON", value);
		}

		public void SetImportParameter_I_EINGDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_EINGDAT_BIS", value);
		}

		public void SetImportParameter_I_EINGDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_EINGDAT_VON", value);
		}

		public void SetImportParameter_I_FIN10(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN10", value);
		}

		public void SetImportParameter_I_FIN17(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN17", value);
		}

		public void SetImportParameter_I_KENNZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KENNZ", value);
		}

		public void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public void SetImportParameter_I_NAME1_AMT(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NAME1_AMT", value);
		}

		public void SetImportParameter_I_PLZCODE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_PLZCODE", value);
		}

		public void SetImportParameter_I_VERTRAGS_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VERTRAGS_NR", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VERTRAGS_NR { get; set; }

			public string AKTENZEICHEN { get; set; }

			public string LICENSE_NUM { get; set; }

			public DateTime? EIGDA { get; set; }

			public DateTime? DATUM_BEHOERDE { get; set; }

			public string NAME1_AMT { get; set; }

			public string POST_CODE1_AMT { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					VERTRAGS_NR = (string)row["VERTRAGS_NR"],
					AKTENZEICHEN = (string)row["AKTENZEICHEN"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					EIGDA = string.IsNullOrEmpty(row["EIGDA"].ToString()) ? null : (DateTime?)row["EIGDA"],
					DATUM_BEHOERDE = string.IsNullOrEmpty(row["DATUM_BEHOERDE"].ToString()) ? null : (DateTime?)row["DATUM_BEHOERDE"],
					NAME1_AMT = (string)row["NAME1_AMT"],
					POST_CODE1_AMT = (string)row["POST_CODE1_AMT"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_CD_STRAFZETTEL", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_CD_STRAFZETTEL", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_CD_STRAFZETTEL.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
