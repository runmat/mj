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
	public partial class Z_M_UNZUGELASSENE_FZGE_ARVAL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_UNZUGELASSENE_FZGE_ARVAL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_UNZUGELASSENE_FZGE_ARVAL).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public partial class T_DATA : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string ZZLVNR { get; set; }

			public string NAME1_HAENDLER { get; set; }

			public string ORT_HAENDLER { get; set; }

			public DateTime? ZBRIEFEINGANG { get; set; }

			public DateTime? ZDATBEAUFTRAGUNG { get; set; }

			public DateTime? VDATU { get; set; }

			public string ZZKENN { get; set; }

			public string ZULORT { get; set; }

			public string NAME1_ZH { get; set; }

			public string ORT01_ZH { get; set; }

			public string NAME1_ZA { get; set; }

			public string ORT01_ZA { get; set; }

			public string NAME1_ZE { get; set; }

			public string ORT01_ZE { get; set; }

			public string EQUNR { get; set; }

			public string ZZCOCKZ { get; set; }

			public string NAME1_ZP { get; set; }

			public string STREET_ZP { get; set; }

			public string HAUSNR_ZP { get; set; }

			public string POSTLZ_ZP { get; set; }

			public string ORT01_ZP { get; set; }

			public string ZEVBNR { get; set; }

			public string ZEVBTXT { get; set; }

			public string KUNDENNAME { get; set; }

			public string NUTZER { get; set; }

			public DateTime? WUNSCH_LIEF_DAT { get; set; }

			public string STATUS { get; set; }

			public string LTEXT_EQUI { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static T_DATA Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				T_DATA o;

				try
				{
					o = new T_DATA
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						CHASSIS_NUM = (string)row["CHASSIS_NUM"],
						ZZLVNR = (string)row["ZZLVNR"],
						NAME1_HAENDLER = (string)row["NAME1_HAENDLER"],
						ORT_HAENDLER = (string)row["ORT_HAENDLER"],
						ZBRIEFEINGANG = string.IsNullOrEmpty(row["ZBRIEFEINGANG"].ToString()) ? null : (DateTime?)row["ZBRIEFEINGANG"],
						ZDATBEAUFTRAGUNG = string.IsNullOrEmpty(row["ZDATBEAUFTRAGUNG"].ToString()) ? null : (DateTime?)row["ZDATBEAUFTRAGUNG"],
						VDATU = string.IsNullOrEmpty(row["VDATU"].ToString()) ? null : (DateTime?)row["VDATU"],
						ZZKENN = (string)row["ZZKENN"],
						ZULORT = (string)row["ZULORT"],
						NAME1_ZH = (string)row["NAME1_ZH"],
						ORT01_ZH = (string)row["ORT01_ZH"],
						NAME1_ZA = (string)row["NAME1_ZA"],
						ORT01_ZA = (string)row["ORT01_ZA"],
						NAME1_ZE = (string)row["NAME1_ZE"],
						ORT01_ZE = (string)row["ORT01_ZE"],
						EQUNR = (string)row["EQUNR"],
						ZZCOCKZ = (string)row["ZZCOCKZ"],
						NAME1_ZP = (string)row["NAME1_ZP"],
						STREET_ZP = (string)row["STREET_ZP"],
						HAUSNR_ZP = (string)row["HAUSNR_ZP"],
						POSTLZ_ZP = (string)row["POSTLZ_ZP"],
						ORT01_ZP = (string)row["ORT01_ZP"],
						ZEVBNR = (string)row["ZEVBNR"],
						ZEVBTXT = (string)row["ZEVBTXT"],
						KUNDENNAME = (string)row["KUNDENNAME"],
						NUTZER = (string)row["NUTZER"],
						WUNSCH_LIEF_DAT = string.IsNullOrEmpty(row["WUNSCH_LIEF_DAT"].ToString()) ? null : (DateTime?)row["WUNSCH_LIEF_DAT"],
						STATUS = (string)row["STATUS"],
						LTEXT_EQUI = (string)row["LTEXT_EQUI"],
					};
				}
				catch(Exception e)
				{
					o = new T_DATA
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

			public static IEnumerable<T_DATA> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<T_DATA> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<T_DATA> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(T_DATA).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<T_DATA> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<T_DATA> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<T_DATA> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_DATA>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_UNZUGELASSENE_FZGE_ARVAL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_DATA> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_DATA>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_DATA> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_DATA>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_DATA> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<T_DATA>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_UNZUGELASSENE_FZGE_ARVAL", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<T_DATA> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<T_DATA>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_UNZUGELASSENE_FZGE_ARVAL.T_DATA> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
