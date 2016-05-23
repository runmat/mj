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
	public partial class Z_DPM_BRIEFBESTAND_001
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_BRIEFBESTAND_001).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_BRIEFBESTAND_001).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_BESTAND(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BESTAND", value);
		}

		public static void SetImportParameter_I_EQTYP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQTYP", value);
		}

		public static void SetImportParameter_I_KUNNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR", value);
		}

		public static void SetImportParameter_I_TEMPVERS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TEMPVERS", value);
		}

		public static void SetImportParameter_I_ZZREFERENZ1(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZREFERENZ1", value);
		}

		public partial class GT_DATEN : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string EQUNR { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string LICENSE_NUM { get; set; }

			public string LIZNR { get; set; }

			public string TIDNR { get; set; }

			public string ABCKZ { get; set; }

			public string MSGRP { get; set; }

			public string STORT { get; set; }

			public string ZZVGRUND { get; set; }

			public DateTime? DATAB { get; set; }

			public DateTime? ZZTMPDT { get; set; }

			public DateTime? EXPIRY_DATE { get; set; }

			public DateTime? PICKDAT { get; set; }

			public string ZZREFERENZ1 { get; set; }

			public string ZZREFERENZ2 { get; set; }

			public string SWERK { get; set; }

			public string TEXT_STO { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public DateTime? REPLA_DATE { get; set; }

			public string ZZHERSTELLER_SCH { get; set; }

			public string ZZTYP_SCHL { get; set; }

			public string ZZFABRIKNAME { get; set; }

			public string ZZHANDELSNAME { get; set; }

			private bool MappingErrorProcessed { get; set; }

			public static GT_DATEN Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				GT_DATEN o;

				try
				{
					o = new GT_DATEN
					{
						SAPConnection = sapConnection,
						DynSapProxyFactory = dynSapProxyFactory,

						EQUNR = (string)row["EQUNR"],
						CHASSIS_NUM = (string)row["CHASSIS_NUM"],
						LICENSE_NUM = (string)row["LICENSE_NUM"],
						LIZNR = (string)row["LIZNR"],
						TIDNR = (string)row["TIDNR"],
						ABCKZ = (string)row["ABCKZ"],
						MSGRP = (string)row["MSGRP"],
						STORT = (string)row["STORT"],
						ZZVGRUND = (string)row["ZZVGRUND"],
						DATAB = string.IsNullOrEmpty(row["DATAB"].ToString()) ? null : (DateTime?)row["DATAB"],
						ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
						EXPIRY_DATE = string.IsNullOrEmpty(row["EXPIRY_DATE"].ToString()) ? null : (DateTime?)row["EXPIRY_DATE"],
						PICKDAT = string.IsNullOrEmpty(row["PICKDAT"].ToString()) ? null : (DateTime?)row["PICKDAT"],
						ZZREFERENZ1 = (string)row["ZZREFERENZ1"],
						ZZREFERENZ2 = (string)row["ZZREFERENZ2"],
						SWERK = (string)row["SWERK"],
						TEXT_STO = (string)row["TEXT_STO"],
						ZZZLDAT = string.IsNullOrEmpty(row["ZZZLDAT"].ToString()) ? null : (DateTime?)row["ZZZLDAT"],
						REPLA_DATE = string.IsNullOrEmpty(row["REPLA_DATE"].ToString()) ? null : (DateTime?)row["REPLA_DATE"],
						ZZHERSTELLER_SCH = (string)row["ZZHERSTELLER_SCH"],
						ZZTYP_SCHL = (string)row["ZZTYP_SCHL"],
						ZZFABRIKNAME = (string)row["ZZFABRIKNAME"],
						ZZHANDELSNAME = (string)row["ZZHANDELSNAME"],
					};
				}
				catch(Exception e)
				{
					o = new GT_DATEN
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

			public static IEnumerable<GT_DATEN> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DATEN> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_DATEN> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DATEN).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DATEN> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DATEN> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_BRIEFBESTAND_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_BRIEFBESTAND_001", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DATEN> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DATEN>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_BRIEFBESTAND_001.GT_DATEN> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
