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
	public partial class Z_DPM_REM_BELASTUNGSANZEIGE_02
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_REM_BELASTUNGSANZEIGE_02).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_REM_BELASTUNGSANZEIGE_02).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AVNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AVNR", value);
		}

		public static void SetImportParameter_I_ERDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ERDAT_BIS", value);
		}

		public static void SetImportParameter_I_ERDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ERDAT_VON", value);
		}

		public static void SetImportParameter_I_FIN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_FIN", value);
		}

		public static void SetImportParameter_I_HC(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_HC", value);
		}

		public static void SetImportParameter_I_INVENTAR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_INVENTAR", value);
		}

		public static void SetImportParameter_I_KFZKZ(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KFZKZ", value);
		}

		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public static void SetImportParameter_I_NO_NULL(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_NO_NULL", value);
		}

		public static void SetImportParameter_I_RENNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_RENNR", value);
		}

		public static void SetImportParameter_I_STATU(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_STATU", value);
		}

		public static void SetImportParameter_I_VJAHR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VJAHR", value);
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string FAHRGNR { get; set; }

			public string KENNZ { get; set; }

			public string INVENTAR { get; set; }

			public string AVNAME { get; set; }

			public string HCNAME { get; set; }

			public DateTime? HCEINGDAT { get; set; }

			public DateTime? GUTAUFTRAGDAT { get; set; }

			public decimal? SUMME { get; set; }

			public string AZGUT { get; set; }

			public string STATU { get; set; }

			public string DDTEXT { get; set; }

			public DateTime? GUTADAT { get; set; }

			public DateTime? SOLLFREI { get; set; }

			public string RENNR { get; set; }

			public string GUTA { get; set; }

			public decimal? MINWERT_AV { get; set; }

			public string FLAG_MIETFZG { get; set; }

			public string REPKALK { get; set; }

			public DateTime? REPKALKDAT { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_OUT o;

				try
				{
					o = new GT_OUT
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						FAHRGNR = (string)row["FAHRGNR"],
						KENNZ = (string)row["KENNZ"],
						INVENTAR = (string)row["INVENTAR"],
						AVNAME = (string)row["AVNAME"],
						HCNAME = (string)row["HCNAME"],
						HCEINGDAT = string.IsNullOrEmpty(row["HCEINGDAT"].ToString()) ? null : (DateTime?)row["HCEINGDAT"],
						GUTAUFTRAGDAT = string.IsNullOrEmpty(row["GUTAUFTRAGDAT"].ToString()) ? null : (DateTime?)row["GUTAUFTRAGDAT"],
						SUMME = string.IsNullOrEmpty(row["SUMME"].ToString()) ? null : (decimal?)row["SUMME"],
						AZGUT = (string)row["AZGUT"],
						STATU = (string)row["STATU"],
						DDTEXT = (string)row["DDTEXT"],
						GUTADAT = string.IsNullOrEmpty(row["GUTADAT"].ToString()) ? null : (DateTime?)row["GUTADAT"],
						SOLLFREI = string.IsNullOrEmpty(row["SOLLFREI"].ToString()) ? null : (DateTime?)row["SOLLFREI"],
						RENNR = (string)row["RENNR"],
						GUTA = (string)row["GUTA"],
						MINWERT_AV = string.IsNullOrEmpty(row["MINWERT_AV"].ToString()) ? null : (decimal?)row["MINWERT_AV"],
						FLAG_MIETFZG = (string)row["FLAG_MIETFZG"],
						REPKALK = (string)row["REPKALK"],
						REPKALKDAT = string.IsNullOrEmpty(row["REPKALKDAT"].ToString()) ? null : (DateTime?)row["REPKALKDAT"],
					};
				}
				catch(Exception e)
				{
					o = new GT_OUT
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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_REM_BELASTUNGSANZEIGE_02", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_REM_BELASTUNGSANZEIGE_02", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_REM_BELASTUNGSANZEIGE_02.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
