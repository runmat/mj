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
	public partial class Z_DPM_READ_SENDTAB_03
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_SENDTAB_03).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_SENDTAB_03).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public static void SetImportParameter_I_CHECK_TRACK(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHECK_TRACK", value);
		}

		public static void SetImportParameter_I_POOLGROUP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_POOLGROUP", value);
		}

		public static void SetImportParameter_I_POOLNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_POOLNR", value);
		}

		public static void SetImportParameter_I_TRACK(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TRACK", value);
		}

		public static void SetImportParameter_I_ZZBRIEF(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZBRIEF", value);
		}

		public static void SetImportParameter_I_ZZKENN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZKENN", value);
		}

		public static void SetImportParameter_I_ZZLSDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZLSDAT_BIS", value);
		}

		public static void SetImportParameter_I_ZZLSDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZLSDAT_VON", value);
		}

		public static void SetImportParameter_I_ZZREFERENZ1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFERENZ1", value);
		}

		public static void SetImportParameter_I_ZZREFERENZ2(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFERENZ2", value);
		}

		public static void SetImportParameter_I_ZZREFNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFNR", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZZFAHRG { get; set; }

			public string ZZKENN { get; set; }

			public DateTime? ZZLSDAT { get; set; }

			public string VERSANDWEG { get; set; }

			public string ZZTRACK { get; set; }

			public string STATUS_CODE { get; set; }

			public string VBELN { get; set; }

			public string POOLNR { get; set; }

			public string ZZBRIEF { get; set; }

			public string ZZREFNR { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STRAS { get; set; }

			public string HSNM1 { get; set; }

			public string PSTLZ { get; set; }

			public string CITY1 { get; set; }

			public string VERSANDVALUE { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public string IDNRK { get; set; }

			public string MAKTX { get; set; }

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

						ZZFAHRG = (string)row["ZZFAHRG"],
						ZZKENN = (string)row["ZZKENN"],
						ZZLSDAT = string.IsNullOrEmpty(row["ZZLSDAT"].ToString()) ? null : (DateTime?)row["ZZLSDAT"],
						VERSANDWEG = (string)row["VERSANDWEG"],
						ZZTRACK = (string)row["ZZTRACK"],
						STATUS_CODE = (string)row["STATUS_CODE"],
						VBELN = (string)row["VBELN"],
						POOLNR = (string)row["POOLNR"],
						ZZBRIEF = (string)row["ZZBRIEF"],
						ZZREFNR = (string)row["ZZREFNR"],
						NAME1 = (string)row["NAME1"],
						NAME2 = (string)row["NAME2"],
						STRAS = (string)row["STRAS"],
						HSNM1 = (string)row["HSNM1"],
						PSTLZ = (string)row["PSTLZ"],
						CITY1 = (string)row["CITY1"],
						VERSANDVALUE = (string)row["VERSANDVALUE"],
						ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
						ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
						IDNRK = (string)row["IDNRK"],
						MAKTX = (string)row["MAKTX"],
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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_SENDTAB_03", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_SENDTAB_03", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_SENDTAB_03.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
