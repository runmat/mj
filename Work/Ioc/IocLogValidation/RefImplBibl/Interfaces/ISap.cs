using System.Linq;
using RefImplBibl.Models;

namespace RefImplBibl.Interfaces
{
    public interface ISap
    {
        IQueryable<EQUI> Z_DPM_EQUI_GET();
        int Z_DPM_INSERT_EQUI(EQUI equi);
        int Z_DPM_UPDATE_EQUI(EQUI equi);
    }
}
