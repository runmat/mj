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
	public partial class Z_DPM_READ_TEMP_VERS_EQUI_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_TEMP_VERS_EQUI_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_TEMP_VERS_EQUI_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public void SetImportParameter_I_EQTYP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_EQTYP", value);
		}

		public void SetImportParameter_I_LICENSE_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LICENSE_NUM", value);
		}

		public void SetImportParameter_I_LIZNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LIZNR", value);
		}

		public void SetImportParameter_I_TIDNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_TIDNR", value);
		}

		public partial class GT_WEB : IModelMappingApplied
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

			public DateTime? ZZTMPDT { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string IDNRK { get; set; }

			public string MAKTX { get; set; }

			public string STLKN { get; set; }

			public string VERS_ID { get; set; }

			public string ZZMANSP { get; set; }

			public DateTime? ZZMANSP_DATBI { get; set; }

			public string KONTONR { get; set; }

			public string CIN { get; set; }

			public static GT_WEB Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_WEB
				{
					EQUNR = (string)row["EQUNR"],
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					LICENSE_NUM = (string)row["LICENSE_NUM"],
					LIZNR = (string)row["LIZNR"],
					TIDNR = (string)row["TIDNR"],
					ZZTMPDT = string.IsNullOrEmpty(row["ZZTMPDT"].ToString()) ? null : (DateTime?)row["ZZTMPDT"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					IDNRK = (string)row["IDNRK"],
					MAKTX = (string)row["MAKTX"],
					STLKN = (string)row["STLKN"],
					VERS_ID = (string)row["VERS_ID"],
					ZZMANSP = (string)row["ZZMANSP"],
					ZZMANSP_DATBI = string.IsNullOrEmpty(row["ZZMANSP_DATBI"].ToString()) ? null : (DateTime?)row["ZZMANSP_DATBI"],
					KONTONR = (string)row["KONTONR"],
					CIN = (string)row["CIN"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_TEMP_VERS_EQUI_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_TEMP_VERS_EQUI_01", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_TEMP_VERS_EQUI_01.GT_WEB> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
