using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleDirections.Contracts;

namespace GoogleDirections.Services
{
    public static class RoutingInstanceHelper
    {
        public static void CreateRoutes(IRoutingInstance ri)
        {
            ri.RouteHomeToCustomerStart = ri.CreateRouteObject();
            ri.RouteHomeToCustomerStart.StartAddress = ri.HomeAddress;
            ri.RouteHomeToCustomerStart.EndAddress = ri.CustomerStartAddress;

            ri.RouteCustomerStartToEnd = ri.CreateRouteObject();
            ri.RouteCustomerStartToEnd.StartAddress = ri.CustomerStartAddress;
            ri.RouteCustomerStartToEnd.EndAddress = ri.CustomerEndAddress;
        }

        #region Extensions for "IRoutingInstance"

        public static bool RoutingInstanceRoutesValid(this IRoutingInstance ri)
        {
            return (ri.CustomerStartAddress.Valid && ri.CustomerEndAddress.Valid && (!ri.HomeAddressAvailable || ri.HomeAddress.Valid));
        }

        public static bool RoutingInstanceRoutesCalculate(this IRoutingInstance ri)
        {
            if (!ri.RoutesValid)
                return false;

            if (ri.HomeAddressAvailable)
                ri.RouteHomeToCustomerStart.CalculateRoute();
            ri.RouteCustomerStartToEnd.CalculateRoute();

            return true;
        }

        public static bool RoutingInstanceRouteCalculationsValid(this IRoutingInstance ri)
        {
            var valid = false;

            if (ri.HomeAddressAvailable)
                valid = ri.RouteHomeToCustomerStart.RouteCalculationOk;
            if (valid)
                valid = ri.RouteCustomerStartToEnd.RouteCalculationOk;

            return valid;
        }

        public static void RoutingInstanceRoutesReset(this IRoutingInstance ri)
        {
            ri.RouteHomeToCustomerStart.CalculationReset();
            ri.RouteCustomerStartToEnd.CalculationReset();
        }

        #endregion

        #region Extensions for "IAddress"

        public static bool AddressIsValid(this IAddress a)
        {
            if (a.UseGeoLocation)
            {
                a.City = "";
                a.Street = "";
                return (!string.IsNullOrEmpty(a.GeoLocation));
            }

            a.GeoLocation = "";
            return (!string.IsNullOrEmpty(a.City));
        }

        public static string AddressAsString(this IAddress a)
        {
            if (a.UseGeoLocation)
                return a.GeoLocation.Replace("(", "").Replace(")", "").Replace(" ", "");
            
            var postCodeSeparator = (string.IsNullOrEmpty(a.PostCode) ? "" : " ");
            var streetSeparator = (string.IsNullOrEmpty(a.Street) ? "" : ", ");

            return string.Format("{0}{1}{2}{3}{4}", a.PostCode, postCodeSeparator, a.City, streetSeparator, a.Street);
        }

        public static string AddressAsFriendlyString(this IAddress a)
        {
            if (a.UseGeoLocation)
                return "[Meine aktuelle GPS Position]";

            return a.AddressAsString();
        }

        #endregion
    }
}
