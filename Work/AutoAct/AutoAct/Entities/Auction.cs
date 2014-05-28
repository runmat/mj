using System;
using System.Collections.Generic;
using AutoAct.Enums;
using SapORM.Models;

namespace AutoAct.Entities
{
    public class Auction
    {
        public Auction(Z_DPM_READ_AUTOACT_01.GT_OUT quelle)
        {
            // Ich erhalte aus SAP Datum und Uhrzeit getrennt, muss beide zusammenbringen
            StartDate = quelle.STARTDATUM.Value;
            EndDate = quelle.ENDDATUM.Value;

            StartDate = StartDate.AddTicks(ConvertSapTimeInTimeSpan(quelle.STARTUHRZEIT).Ticks);                

            EndDate = EndDate.AddTicks(ConvertSapTimeInTimeSpan(quelle.ENDUHRZEIT).Ticks);                

            decimal grossClearancePrice;
            if (decimal.TryParse(quelle.FREIGABEPREIS_C, out grossClearancePrice))
            {
                GrossClearancePrice = grossClearancePrice;
            }

            decimal grossStartingPrice;
            if (decimal.TryParse(quelle.AUSRUFPREIS_C, out grossStartingPrice))
            {
                GrossStartingPrice = grossStartingPrice;    
            }

            switch (quelle.ANGEBOTSART)
            {
                case "1":
                    Format = AdFormat.BIDDING;
                    break;

                case "2":
                    Format = AdFormat.AUCTION;
                    break;

                case "3":
                    Format = AdFormat.FIX_PRICE;
                    break;
            }
        }

        /// <summary>
        /// Parameterloser Constructor für die Deserialisierung
        /// </summary>
        public Auction()
        {
            
        }

        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Dauer der Auktion in Stunden
        /// </summary>
        // public int Duration { get; set; } // Test, entweder Begin und Ende oder Begin und Dauer

        public decimal GrossStartingPrice { get; set; }

        public decimal GrossClearancePrice { get; set; }

        public AdFormat Format { get; set; }

        private static TimeSpan ConvertSapTimeInTimeSpan(string sapTime)
        {
            if (string.IsNullOrEmpty(sapTime))
            {
                return new TimeSpan();
            }
            var charsOfSapTime = new List<char>(sapTime.ToCharArray());
            charsOfSapTime.Insert(2, ":".ToCharArray()[0]);
            charsOfSapTime.Insert(5, ":".ToCharArray()[0]);
            return TimeSpan.Parse(new string(charsOfSapTime.ToArray()));
        }
    }
}