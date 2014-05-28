//using SapORM.Services;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_DPM_AKTIVCHECK_READ
	{
	    public partial class ET_TREFF
	    {
            [SapIgnore]
            public string Klassifizierungstext { get; set; }
        }
	}
}
