using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleDirections.Contracts
{
    public interface IRoutingInstance
    {
        bool HomeAddressAvailable { get; set; }
        IAddress HomeAddress { get; set; }
        IAddress CustomerStartAddress { get; set; }
        IAddress CustomerEndAddress { get; set; }

        IRoute RouteHomeToCustomerStart { get; set; }
        IRoute RouteCustomerStartToEnd { get; set; }

        bool RoutesValid { get; }

        IRoute CreateRouteObject();

        bool CalculateRoutes();
        void ResetRoutes();
    }
}
