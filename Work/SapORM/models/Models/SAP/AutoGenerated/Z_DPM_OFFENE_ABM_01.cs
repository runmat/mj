using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_OFFENE_ABM_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_OFFENE_ABM_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_OFFENE_ABM_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZKENN { get; set; }

			public string ZFAHRG { get; set; }

			public DateTime? ABMDAT { get; set; }

			public string ABMORT { get; set; }

			public string ABNAME { get; set; }

			public string KBANR { get; set; }

			public string KUNNR { get; set; }

			public string NAMEK { get; set; }

			public string ZTUEV { get; set; }

			public string ZASU { get; set; }

			public string HANAME1 { get; set; }

			public string HANAME2 { get; set; }

			public string HAPSTLZ { get; set; }

			public string HAORT01 { get; set; }

			public string HASTRAS { get; set; }

			public string HAUSNR { get; set; }

			public string STATUS { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERZET { get; set; }

			public DateTime? PICKDAT { get; set; }

			public string PICKZET { get; set; }

			public string LIZNR { get; set; }

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

			public string ZB2_FEHLT { get; set; }

			public string SCHEIN_FEHLT { get; set; }

			public string SCHILD_FEHLT { get; set; }

			public DateTime? EINGDAT_SUNDS { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					ZZKENN = (string)row["ZZKENN"],
					ZFAHRG = (string)row["ZFAHRG"],
					ABMDAT = (string.IsNullOrEmpty(row["ABMDAT"].ToString())) ? null : (DateTime?)row["ABMDAT"],
					ABMORT = (string)row["ABMORT"],
					ABNAME = (string)row["ABNAME"],
					KBANR = (string)row["KBANR"],
					KUNNR = (string)row["KUNNR"],
					NAMEK = (string)row["NAMEK"],
					ZTUEV = (string)row["ZTUEV"],
					ZASU = (string)row["ZASU"],
					HANAME1 = (string)row["HANAME1"],
					HANAME2 = (string)row["HANAME2"],
					HAPSTLZ = (string)row["HAPSTLZ"],
					HAORT01 = (string)row["HAORT01"],
					HASTRAS = (string)row["HASTRAS"],
					HAUSNR = (string)row["HAUSNR"],
					STATUS = (string)row["STATUS"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					ERZET = (string)row["ERZET"],
					PICKDAT = (string.IsNullOrEmpty(row["PICKDAT"].ToString())) ? null : (DateTime?)row["PICKDAT"],
					PICKZET = (string)row["PICKZET"],
					LIZNR = (string)row["LIZNR"],
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
					ZB2_FEHLT = (string)row["ZB2_FEHLT"],
					SCHEIN_FEHLT = (string)row["SCHEIN_FEHLT"],
					SCHILD_FEHLT = (string)row["SCHILD_FEHLT"],
					EINGDAT_SUNDS = (string.IsNullOrEmpty(row["EINGDAT_SUNDS"].ToString())) ? null : (DateTime?)row["EINGDAT_SUNDS"],

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
				return Select(dt, sapConnection).ToList();
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
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_OFFENE_ABM_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_OFFENE_ABM_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_OFFENE_ABM_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_DPM_OFFENE_ABM_01.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
