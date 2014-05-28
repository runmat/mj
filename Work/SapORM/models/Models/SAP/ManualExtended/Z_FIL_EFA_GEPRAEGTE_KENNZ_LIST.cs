//using SapORM.Services;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST
	{
	    public partial class GT_PO_P
	    {
            [SapIgnore]
            public string LTEXT 
            { 
                get
                {
                    if (SAPConnection == null)
                        return "";

                    //return new LongStringToSap(SAPConnection, DynSapProxyFactory).ReadString(LTEXT_NR);
                    return LTEXT_NR;
                } 
            }

            partial void OnInitFromSap()
            {
            }
        }
	}
}
