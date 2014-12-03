﻿using System;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Vorgang
    {
        [LocalizedDisplay(LocalizeConstants.ReceiptNo)]
        public string BelegNr { get; set; }

        public string VkOrg { get; set; }

        public string VkBur { get; set; }

        public string Vorerfasser { get; set; }

        public string VorgangsStatus { get; set; }

        public Rechnungsdaten Rechnungsdaten { get; set; }

        public BankAdressdaten BankAdressdaten { get; set; }

        public Fahrzeugdaten Fahrzeugdaten { get; set; }

        public Adresse Halterdaten { get; set; }

        public string Halter
        {
            get
            {
                if (Halterdaten != null)
                {
                    return String.Format("{0} {1}", Halterdaten.Name1, Halterdaten.Name2);
                }

                return "";
            }
        }

        public Zulassungsdaten Zulassungsdaten { get; set; }

        public OptionenDienstleistungen OptionenDienstleistungen { get; set; }

        public byte[] KundenformularPdf { get; set; }

        public Vorgang()
        {
            Rechnungsdaten = new Rechnungsdaten();
            BankAdressdaten = new BankAdressdaten();
            Fahrzeugdaten = new Fahrzeugdaten();
            Halterdaten = new Adresse { Land = "DE", Kennung = "HALTER" };
            Zulassungsdaten = new Zulassungsdaten();
            OptionenDienstleistungen = new OptionenDienstleistungen();
        }
    }
}
