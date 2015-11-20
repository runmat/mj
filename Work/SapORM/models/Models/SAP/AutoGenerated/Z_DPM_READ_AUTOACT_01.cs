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
	public partial class Z_DPM_READ_AUTOACT_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_AUTOACT_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_AUTOACT_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public void SetImportParameter_I_STATUS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STATUS", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string BELEGNR { get; set; }

			public string VKORG { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string REFERENZ { get; set; }

			public string EQUNR { get; set; }

			public string STATUS { get; set; }

			public string LICENSE_NUM { get; set; }

			public DateTime? ERSTZULDAT { get; set; }

			public string ANZ_HALTER { get; set; }

			public string AUSRUFPREIS_C { get; set; }

			public string FREIGABEPREIS_C { get; set; }

			public DateTime? STARTDATUM { get; set; }

			public DateTime? ENDDATUM { get; set; }

			public string STARTUHRZEIT { get; set; }

			public string ENDUHRZEIT { get; set; }

			public string REP_SCHADENTEXT { get; set; }

			public string TEXTART { get; set; }

			public string ZUSTANDSBERICHT { get; set; }

			public string UNTERLAGEN1 { get; set; }

			public string UNTERLAGEN2 { get; set; }

			public string UNTERLAGEN3 { get; set; }

			public string BILDERPFAD { get; set; }

			public DateTime? DAT_ERST { get; set; }

			public DateTime? DAT_INSERAT { get; set; }

			public string KUNNR_AG { get; set; }

			public string AUTOACT_ID { get; set; }

			public string RUECK_AUTOACT { get; set; }

			public string ANGEBOTSART { get; set; }

			public string ANGEBOTSART_TXT { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ART_FZGTYP { get; set; }

			public string KM_STAND { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					BELEGNR = (string)row["BELEGNR"],
					VKORG = (string)row["VKORG"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					REFERENZ = (string)row["REFERENZ"],
					EQUNR = (string)row["EQUNR"],
					STATUS = (string)row["STATUS"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					ERSTZULDAT = string.IsNullOrEmpty(row["ERSTZULDAT"].ToString()) ? null : (DateTime?)row["ERSTZULDAT"],
					ANZ_HALTER = (string)row["ANZ_HALTER"],
					AUSRUFPREIS_C = (string)row["AUSRUFPREIS_C"],
					FREIGABEPREIS_C = (string)row["FREIGABEPREIS_C"],
					STARTDATUM = string.IsNullOrEmpty(row["STARTDATUM"].ToString()) ? null : (DateTime?)row["STARTDATUM"],
					ENDDATUM = string.IsNullOrEmpty(row["ENDDATUM"].ToString()) ? null : (DateTime?)row["ENDDATUM"],
					STARTUHRZEIT = (string)row["STARTUHRZEIT"],
					ENDUHRZEIT = (string)row["ENDUHRZEIT"],
					REP_SCHADENTEXT = (string)row["REP_SCHADENTEXT"],
					TEXTART = (string)row["TEXTART"],
					ZUSTANDSBERICHT = (string)row["ZUSTANDSBERICHT"],
					UNTERLAGEN1 = (string)row["UNTERLAGEN1"],
					UNTERLAGEN2 = (string)row["UNTERLAGEN2"],
					UNTERLAGEN3 = (string)row["UNTERLAGEN3"],
					BILDERPFAD = (string)row["BILDERPFAD"],
					DAT_ERST = string.IsNullOrEmpty(row["DAT_ERST"].ToString()) ? null : (DateTime?)row["DAT_ERST"],
					DAT_INSERAT = string.IsNullOrEmpty(row["DAT_INSERAT"].ToString()) ? null : (DateTime?)row["DAT_INSERAT"],
					KUNNR_AG = (string)row["KUNNR_AG"],
					AUTOACT_ID = (string)row["AUTOACT_ID"],
					RUECK_AUTOACT = (string)row["RUECK_AUTOACT"],
					ANGEBOTSART = (string)row["ANGEBOTSART"],
					ANGEBOTSART_TXT = (string)row["ANGEBOTSART_TXT"],
					ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
					ART_FZGTYP = (string)row["ART_FZGTYP"],
					KM_STAND = (string)row["KM_STAND"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_AUTOACT_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_AUTOACT_01", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_AUTOACT_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
