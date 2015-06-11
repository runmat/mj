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
	public partial class Z_M_EC_AVM_KENNZ_SERIE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_EC_AVM_KENNZ_SERIE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_EC_AVM_KENNZ_SERIE).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_WEB : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string KUNNR { get; set; }

			public string KBANR { get; set; }

			public string ART { get; set; }

			public string ACTIVE { get; set; }

			public string ORTKENNZ { get; set; }

			public string MINLETTER { get; set; }

			public string MINNUMBER { get; set; }

			public string MAXLETTER { get; set; }

			public string MAXNUMBER { get; set; }

			public string NEXTLETTER { get; set; }

			public string NEXTNUMBER { get; set; }

			public string SONDERSERIE { get; set; }

			public string SWLETTER { get; set; }

			public string SWNUMBER { get; set; }

			public DateTime? DATUM { get; set; }

			public string SMTP_ADDR { get; set; }

			public string VKORG { get; set; }

			public string BESTBISKNZ { get; set; }

			public DateTime? BESTDATUM { get; set; }

			public string BESTUSER { get; set; }

			public string BESTANDKNZ { get; set; }

			public string GRENZWERT { get; set; }

			public string INTERVALL_VON { get; set; }

			public string INTERVALL_BIS { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					MANDT = (string)row["MANDT"],
					KUNNR = (string)row["KUNNR"],
					KBANR = (string)row["KBANR"],
					ART = (string)row["ART"],
					ACTIVE = (string)row["ACTIVE"],
					ORTKENNZ = (string)row["ORTKENNZ"],
					MINLETTER = (string)row["MINLETTER"],
					MINNUMBER = (string)row["MINNUMBER"],
					MAXLETTER = (string)row["MAXLETTER"],
					MAXNUMBER = (string)row["MAXNUMBER"],
					NEXTLETTER = (string)row["NEXTLETTER"],
					NEXTNUMBER = (string)row["NEXTNUMBER"],
					SONDERSERIE = (string)row["SONDERSERIE"],
					SWLETTER = (string)row["SWLETTER"],
					SWNUMBER = (string)row["SWNUMBER"],
					DATUM = (string.IsNullOrEmpty(row["DATUM"].ToString())) ? null : (DateTime?)row["DATUM"],
					SMTP_ADDR = (string)row["SMTP_ADDR"],
					VKORG = (string)row["VKORG"],
					BESTBISKNZ = (string)row["BESTBISKNZ"],
					BESTDATUM = (string.IsNullOrEmpty(row["BESTDATUM"].ToString())) ? null : (DateTime?)row["BESTDATUM"],
					BESTUSER = (string)row["BESTUSER"],
					BESTANDKNZ = (string)row["BESTANDKNZ"],
					GRENZWERT = (string)row["GRENZWERT"],
					INTERVALL_VON = (string)row["INTERVALL_VON"],
					INTERVALL_BIS = (string)row["INTERVALL_BIS"],

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

			public static IEnumerable<GT_WEB> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_WEB> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_WEB> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_WEB).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_WEB> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_WEB> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_M_EC_AVM_KENNZ_SERIE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_M_EC_AVM_KENNZ_SERIE", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_WEB> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_WEB>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_M_EC_AVM_KENNZ_SERIE.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_M_EC_AVM_KENNZ_SERIE.GT_WEB> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
