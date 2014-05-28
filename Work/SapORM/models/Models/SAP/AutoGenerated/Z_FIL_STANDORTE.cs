using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_FIL_STANDORTE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_FIL_STANDORTE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_FIL_STANDORTE).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_STANDORTE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZKBA1 { get; set; }

			public string PRCTR { get; set; }

			public string ZSTAT { get; set; }

			public string BUTXT { get; set; }

			public string TYP { get; set; }

			public string KAT { get; set; }

			public string ZKFZKZ { get; set; }

			public string ZKREIS { get; set; }

			public string STRAS { get; set; }

			public string PSTLZ { get; set; }

			public string ORT01 { get; set; }

			public string TELF1 { get; set; }

			public string TELFX { get; set; }

			public string ORT02 { get; set; }

			public string ZEMAIL { get; set; }

			public string MOAB1 { get; set; }

			public string MOBI1 { get; set; }

			public string MOAB2 { get; set; }

			public string MOBI2 { get; set; }

			public string DIAB1 { get; set; }

			public string DIBI1 { get; set; }

			public string DIAB2 { get; set; }

			public string DIBI2 { get; set; }

			public string MIAB1 { get; set; }

			public string MIBI1 { get; set; }

			public string MIAB2 { get; set; }

			public string MIBI2 { get; set; }

			public string DOAB1 { get; set; }

			public string DOBI1 { get; set; }

			public string DOAB2 { get; set; }

			public string DOBI2 { get; set; }

			public string FRAB1 { get; set; }

			public string FRBI1 { get; set; }

			public string FRAB2 { get; set; }

			public string FRBI2 { get; set; }

			public string SAAB1 { get; set; }

			public string SABI1 { get; set; }

			public string SAAB2 { get; set; }

			public string SABI2 { get; set; }

			public string SOAB11 { get; set; }

			public string SOAB12 { get; set; }

			public string SOAB13 { get; set; }

			public string SOAB21 { get; set; }

			public string SOAB22 { get; set; }

			public string SOAB23 { get; set; }

			public string SOBI11 { get; set; }

			public string SOBI12 { get; set; }

			public string SOBI13 { get; set; }

			public string SOBI21 { get; set; }

			public string SOBI22 { get; set; }

			public string SOBI23 { get; set; }

			public Int32? FREQUENZ1 { get; set; }

			public Int32? FREQUENZ2 { get; set; }

			public Int32? FREQUENZ3 { get; set; }

			public string WOCHENTAG1 { get; set; }

			public string WOCHENTAG2 { get; set; }

			public string WOCHENTAG3 { get; set; }

			public string GUELTIGKEIT1 { get; set; }

			public string GUELTIGKEIT2 { get; set; }

			public string GUELTIGKEIT3 { get; set; }

			public static GT_STANDORTE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_STANDORTE
				{
					ZKBA1 = (string)row["ZKBA1"],
					PRCTR = (string)row["PRCTR"],
					ZSTAT = (string)row["ZSTAT"],
					BUTXT = (string)row["BUTXT"],
					TYP = (string)row["TYP"],
					KAT = (string)row["KAT"],
					ZKFZKZ = (string)row["ZKFZKZ"],
					ZKREIS = (string)row["ZKREIS"],
					STRAS = (string)row["STRAS"],
					PSTLZ = (string)row["PSTLZ"],
					ORT01 = (string)row["ORT01"],
					TELF1 = (string)row["TELF1"],
					TELFX = (string)row["TELFX"],
					ORT02 = (string)row["ORT02"],
					ZEMAIL = (string)row["ZEMAIL"],
					MOAB1 = (string)row["MOAB1"],
					MOBI1 = (string)row["MOBI1"],
					MOAB2 = (string)row["MOAB2"],
					MOBI2 = (string)row["MOBI2"],
					DIAB1 = (string)row["DIAB1"],
					DIBI1 = (string)row["DIBI1"],
					DIAB2 = (string)row["DIAB2"],
					DIBI2 = (string)row["DIBI2"],
					MIAB1 = (string)row["MIAB1"],
					MIBI1 = (string)row["MIBI1"],
					MIAB2 = (string)row["MIAB2"],
					MIBI2 = (string)row["MIBI2"],
					DOAB1 = (string)row["DOAB1"],
					DOBI1 = (string)row["DOBI1"],
					DOAB2 = (string)row["DOAB2"],
					DOBI2 = (string)row["DOBI2"],
					FRAB1 = (string)row["FRAB1"],
					FRBI1 = (string)row["FRBI1"],
					FRAB2 = (string)row["FRAB2"],
					FRBI2 = (string)row["FRBI2"],
					SAAB1 = (string)row["SAAB1"],
					SABI1 = (string)row["SABI1"],
					SAAB2 = (string)row["SAAB2"],
					SABI2 = (string)row["SABI2"],
					SOAB11 = (string)row["SOAB11"],
					SOAB12 = (string)row["SOAB12"],
					SOAB13 = (string)row["SOAB13"],
					SOAB21 = (string)row["SOAB21"],
					SOAB22 = (string)row["SOAB22"],
					SOAB23 = (string)row["SOAB23"],
					SOBI11 = (string)row["SOBI11"],
					SOBI12 = (string)row["SOBI12"],
					SOBI13 = (string)row["SOBI13"],
					SOBI21 = (string)row["SOBI21"],
					SOBI22 = (string)row["SOBI22"],
					SOBI23 = (string)row["SOBI23"],
					FREQUENZ1 = (string.IsNullOrEmpty(row["FREQUENZ1"].ToString())) ? null : (Int32?)Convert.ToInt32(row["FREQUENZ1"]),
					FREQUENZ2 = (string.IsNullOrEmpty(row["FREQUENZ2"].ToString())) ? null : (Int32?)Convert.ToInt32(row["FREQUENZ2"]),
					FREQUENZ3 = (string.IsNullOrEmpty(row["FREQUENZ3"].ToString())) ? null : (Int32?)Convert.ToInt32(row["FREQUENZ3"]),
					WOCHENTAG1 = (string)row["WOCHENTAG1"],
					WOCHENTAG2 = (string)row["WOCHENTAG2"],
					WOCHENTAG3 = (string)row["WOCHENTAG3"],
					GUELTIGKEIT1 = (string)row["GUELTIGKEIT1"],
					GUELTIGKEIT2 = (string)row["GUELTIGKEIT2"],
					GUELTIGKEIT3 = (string)row["GUELTIGKEIT3"],

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

			public static IEnumerable<GT_STANDORTE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_STANDORTE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToList();
			}

			public static IEnumerable<GT_STANDORTE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_STANDORTE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_STANDORTE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToList();
			}

			public static List<GT_STANDORTE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_STANDORTE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_STANDORTE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_FIL_STANDORTE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_STANDORTE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_STANDORTE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_STANDORTE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_STANDORTE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_STANDORTE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_STANDORTE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_FIL_STANDORTE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}

			public static List<GT_STANDORTE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_STANDORTE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_FIL_STANDORTE.GT_STANDORTE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_FIL_STANDORTE.GT_STANDORTE> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
