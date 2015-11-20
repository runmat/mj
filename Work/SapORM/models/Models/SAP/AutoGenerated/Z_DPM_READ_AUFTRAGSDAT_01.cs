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
	public partial class Z_DPM_READ_AUFTRAGSDAT_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_AUFTRAGSDAT_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_AUFTRAGSDAT_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public void SetImportParameter_I_AUGRU(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AUGRU", value);
		}

		public void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public void SetImportParameter_I_LIZNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIZNR", value);
		}

		public void SetImportParameter_I_NUR_KLAERFAELLE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NUR_KLAERFAELLE", value);
		}

		public void SetImportParameter_I_NUR_OFFENE_UK(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NUR_OFFENE_UK", value);
		}

		public void SetImportParameter_I_VDATU_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_VDATU_BIS", value);
		}

		public void SetImportParameter_I_VDATU_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_VDATU_VON", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string AG { get; set; }

			public string NAME1_AG { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LIZNR { get; set; }

			public string LICENSE_NUM_ALT { get; set; }

			public string LICENSE_NUM_NEU { get; set; }

			public DateTime? VDATU { get; set; }

			public string CODE_STO { get; set; }

			public string NAME1_STO { get; set; }

			public string NAME2_STO { get; set; }

			public string ANSPP_STO { get; set; }

			public string STRAS_STO { get; set; }

			public string PSTLZ_STO { get; set; }

			public string ORT01_STO { get; set; }

			public string TEL_STO { get; set; }

			public string FAX_STO { get; set; }

			public string EMAIL_STO { get; set; }

			public string STATUS { get; set; }

			public string KLAERFALL { get; set; }

			public string NAME1_ZH { get; set; }

			public string NAME2_ZH { get; set; }

			public string STRAS_ZH { get; set; }

			public string HOUSE_NUM1_ZH { get; set; }

			public string PSTLZ_ZH { get; set; }

			public string ORT01_ZH { get; set; }

			public string NAME1_ZE { get; set; }

			public string NAME2_ZE { get; set; }

			public string STRAS_ZE { get; set; }

			public string HOUSE_NUM1_ZE { get; set; }

			public string PSTLZ_ZE { get; set; }

			public string ORT01_ZE { get; set; }

			public string KOMMENTAR { get; set; }

			public string ERLEDIGT { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					AG = (string)row["AG"],
					NAME1_AG = (string)row["NAME1_AG"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LIZNR = (string)row["LIZNR"],
					LICENSE_NUM_ALT = (string)row["LICENSE_NUM_ALT"],
					LICENSE_NUM_NEU = (string)row["LICENSE_NUM_NEU"],
					VDATU = string.IsNullOrEmpty(row["VDATU"].ToString()) ? null : (DateTime?)row["VDATU"],
					CODE_STO = (string)row["CODE_STO"],
					NAME1_STO = (string)row["NAME1_STO"],
					NAME2_STO = (string)row["NAME2_STO"],
					ANSPP_STO = (string)row["ANSPP_STO"],
					STRAS_STO = (string)row["STRAS_STO"],
					PSTLZ_STO = (string)row["PSTLZ_STO"],
					ORT01_STO = (string)row["ORT01_STO"],
					TEL_STO = (string)row["TEL_STO"],
					FAX_STO = (string)row["FAX_STO"],
					EMAIL_STO = (string)row["EMAIL_STO"],
					STATUS = (string)row["STATUS"],
					KLAERFALL = (string)row["KLAERFALL"],
					NAME1_ZH = (string)row["NAME1_ZH"],
					NAME2_ZH = (string)row["NAME2_ZH"],
					STRAS_ZH = (string)row["STRAS_ZH"],
					HOUSE_NUM1_ZH = (string)row["HOUSE_NUM1_ZH"],
					PSTLZ_ZH = (string)row["PSTLZ_ZH"],
					ORT01_ZH = (string)row["ORT01_ZH"],
					NAME1_ZE = (string)row["NAME1_ZE"],
					NAME2_ZE = (string)row["NAME2_ZE"],
					STRAS_ZE = (string)row["STRAS_ZE"],
					HOUSE_NUM1_ZE = (string)row["HOUSE_NUM1_ZE"],
					PSTLZ_ZE = (string)row["PSTLZ_ZE"],
					ORT01_ZE = (string)row["ORT01_ZE"],
					KOMMENTAR = (string)row["KOMMENTAR"],
					ERLEDIGT = (string)row["ERLEDIGT"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_AUFTRAGSDAT_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_AUFTRAGSDAT_01", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_AUFTRAGSDAT_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
