using System;
using System.Collections.Generic;
using AutoAct.Enums;
using Newtonsoft.Json;
using SapORM.Models;

namespace AutoAct.Entities
{
    public class Vehicle
    {
        private readonly List<string> _status = new List<string> {"R", "S"};

        public Vehicle(Z_DPM_READ_AUTOACT_01.GT_OUT fahrzeug)
        {
            AutoComplete = new AutoComplete();
            Attachments = new List<Attachment>();

            FirstRegistrationDate = fahrzeug.ERSTZULDAT.Value;

            //fahrzeug
            Type = VehicleType.Car;
            Vin = fahrzeug.CHASSIS_NUM;
            NumberOfPreviousOwners = byte.Parse(fahrzeug.ANZ_HALTER);
            if (NumberOfPreviousOwners == 0)
            {
                NumberOfPreviousOwners = 1;
            }

            // Identification des Fahrzeugs
            SellerReferenceId = fahrzeug.REFERENZ;  // Referenz bei dem Kunden des DADs, also welche Belegnummer das das Fahrzeug im System des Kunden
            Belegnummer = fahrzeug.BELEGNR;         // Belegnummer in SAP für das Fahrzeug in den Datenbeständen des DADs

            if (_status.Contains(fahrzeug.TEXTART))
            {
                if (fahrzeug.TEXTART == "R")
                {
                    Condition = VehicleCondition.repaired;
                }

                if (fahrzeug.TEXTART == "S")
                {
                    Condition = VehicleCondition.damaged;
                }

                DamageDescription = fahrzeug.REP_SCHADENTEXT; 
            }

            TypeOfCarFleet = TypeOfCarFleet.LEASING; // Test, warte darauf dass aus SAP die ART_FZGTYP übertragen wird
            Lifetime = VehicleLifetime.USED;
            Variant = fahrzeug.ZZFABRIKNAME;

            if (string.IsNullOrEmpty(fahrzeug.KM_STAND) == false)
            {
                Mileage = int.Parse(fahrzeug.KM_STAND);
            }
        }

        public Vehicle()
        {
        }

        [JsonIgnore]
        public string Belegnummer { get; set; }

        public Int64 Id { get; set; }

        public string Vin { get; set; }

        public VehicleType Type { get; set; }
        
        public DateTime? FirstRegistrationDate { get; set; }
        
        public byte NumberOfPreviousOwners { get; set; }

        public Auction Auction { get; set; }

        public SellerContact SellerContact { get; set; }

        public AutoComplete AutoComplete { get; set; }

        public string SellerReferenceId { get; set; }

        public VehicleCondition Condition { get; set; }

        public string DamageDescription { get; set; }

        public TypeOfCarFleet TypeOfCarFleet { get; set; }

        public VehicleLifetime Lifetime { get; set; }

        public int Mileage { get; set; }

        // TODO Diese Werte müssen noch aus SAP übernommen werden

        public decimal? RepairedDamageCosts { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Variant { get; set; }

        public List<Attachment> Attachments { get; set; } 

    }
}
