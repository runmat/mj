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
	public partial class Z_DPM_READ_PROT_TELEFONATE_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_PROT_TELEFONATE_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_PROT_TELEFONATE_01).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AG", value);
		}

		public static void SetImportParameter_I_ANRUFART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ANRUFART", value);
		}

		public static void SetImportParameter_I_ANRUFDATUM_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ANRUFDATUM_BIS", value);
		}

		public static void SetImportParameter_I_ANRUFDATUM_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ANRUFDATUM_VON", value);
		}

		public static void SetImportParameter_I_ZVERT_ART(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZVERT_ART", value);
		}

		public static string GetExportParameter_E_FOUND(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_FOUND").NotNullOrEmpty().Trim();
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR_AG { get; set; }

			public string KONTONR { get; set; }

			public string CIN { get; set; }

			public string PAID { get; set; }

			public string ZVERT_ART { get; set; }

			public string TEL_NUMBER { get; set; }

			public string ANRUFART { get; set; }

			public DateTime? ANRUFDATUM { get; set; }

			public string UZEIT_VON { get; set; }

			public string UZEIT_BIS { get; set; }

			public string NAME_ANRUFER { get; set; }

			public string TEXT_ANRUFGRUND { get; set; }

			public string FREITEXT_GRUND { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					KUNNR_AG = (string)row["KUNNR_AG"],
					KONTONR = (string)row["KONTONR"],
					CIN = (string)row["CIN"],
					PAID = (string)row["PAID"],
					ZVERT_ART = (string)row["ZVERT_ART"],
					TEL_NUMBER = (string)row["TEL_NUMBER"],
					ANRUFART = (string)row["ANRUFART"],
					ANRUFDATUM = string.IsNullOrEmpty(row["ANRUFDATUM"].ToString()) ? null : (DateTime?)row["ANRUFDATUM"],
					UZEIT_VON = (string)row["UZEIT_VON"],
					UZEIT_BIS = (string)row["UZEIT_BIS"],
					NAME_ANRUFER = (string)row["NAME_ANRUFER"],
					TEXT_ANRUFGRUND = (string)row["TEXT_ANRUFGRUND"],
					FREITEXT_GRUND = (string)row["FREITEXT_GRUND"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_PROT_TELEFONATE_01", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_PROT_TELEFONATE_01", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_PROT_TELEFONATE_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
