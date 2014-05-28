using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CKG.Components.Zulassung.DAL
{
    public class VehicleSearchCriterias : IEquatable<VehicleSearchCriterias>
    {
        private string chassisNumber = string.Empty;
        private string zb2No = string.Empty;
        private string contract = string.Empty;
        private string licensePlate1 = string.Empty;
        private string licensePlate2 = string.Empty;
        private string licensePlate3 = string.Empty;
        private string reservationNo = string.Empty;
        private string reservationName = string.Empty;

        public string ChassisNumber
        {
            get { return chassisNumber; }
            set { chassisNumber = value; }
        }

        public string ZB2No
        {
            get { return zb2No; }
            set { zb2No = value; }
        }

        public string Contract
        {
            get { return contract; }
            set { contract = value; }
        }        

        public string LicensePlate1
        {
            get { return licensePlate1; }
            set { licensePlate1 = value; }
        }        

        public string LicensePlate2
        {
            get { return licensePlate2; }
            set { licensePlate2 = value; }
        }        

        public string LicensePlate3
        {
            get { return licensePlate3; }
            set { licensePlate3 = value; }
        }        

        public string ReservationNo
        {
            get { return reservationNo; }
            set { reservationNo = value; }
        }        

        public string ReservationName
        {
            get { return reservationName; }
            set { reservationName = value; }
        }

        public bool Equals(VehicleSearchCriterias other)
        {
            return chassisNumber.Equals(other.chassisNumber, StringComparison.OrdinalIgnoreCase)
                && zb2No.Equals(other.zb2No, StringComparison.OrdinalIgnoreCase)
                && contract.Equals(other.contract, StringComparison.OrdinalIgnoreCase)
                && licensePlate1.Equals(other.licensePlate1, StringComparison.OrdinalIgnoreCase)
                && licensePlate2.Equals(other.licensePlate2, StringComparison.OrdinalIgnoreCase)
                && licensePlate3.Equals(other.licensePlate3, StringComparison.OrdinalIgnoreCase)
                && reservationNo.Equals(other.reservationNo, StringComparison.OrdinalIgnoreCase)
                && reservationName.Equals(other.reservationName, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return string.Concat(chassisNumber, zb2No, contract, licensePlate1, licensePlate2, licensePlate3, reservationNo, reservationName);
        }
    }
}