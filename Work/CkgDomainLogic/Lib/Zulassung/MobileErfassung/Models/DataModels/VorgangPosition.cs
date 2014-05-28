﻿using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// ZLD-Positionssatz mit den für die mobile Erfassung relevanten Daten
    /// </summary>
    public class VorgangPosition
    {
        [Display(Name = "ID des Kopfsatzes")]
        public string KopfId { get; set; }

        [Display(Name = "Position")]
        public string PosNr { get; set; }

        [Display(Name = "Dienstleistung-Id")]
        public string DienstleistungId { get; set; }

        [Display(Name = "Dienstleistung")]
        public string DienstleistungBez { get; set; }

        [Display(Name = "Gebühr Amt")]
        public decimal Gebuehr { get; set; }

        public VorgangPosition()
        {
            this.KopfId = "";
            this.PosNr = "0";
            this.DienstleistungId = "";
            this.DienstleistungBez = "";
            this.Gebuehr = 0;
        }

        public VorgangPosition(string kopfid, string posNr, string dlId, string dlBez, decimal gebuehr)
        {
            this.KopfId = kopfid;
            this.PosNr = posNr;
            this.DienstleistungId = dlId;
            this.DienstleistungBez = dlBez;
            this.Gebuehr = gebuehr;
        }
    }
}
