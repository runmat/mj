using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_M_SET_FAHRER_AUFTRAGS_STATUS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_M_SET_FAHRER_AUFTRAGS_STATUS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_M_SET_FAHRER_AUFTRAGS_STATUS).Name, inputParameterKeys, inputParameterValues);
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
