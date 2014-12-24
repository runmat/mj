using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleDirections.Contracts
{
    public interface IRoute
    {
        PredicateAddressCaching RouteCalculationCachingLoad { get; set; }
        ActionAddressCaching RouteCalculationCachingSave { get; set; }

        IAddress StartAddress { get; set; }
        IAddress EndAddress { get; set; }

        int Weight { get; set; }

        double DistanceKm { get; set; }
        bool RouteCalculationOk { get; set; }

        void CalculateRoute();
        void CalculationReset();
    }
}
