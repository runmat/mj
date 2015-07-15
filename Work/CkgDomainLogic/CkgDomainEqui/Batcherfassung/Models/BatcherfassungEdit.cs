using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.FzgModelle.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class BatcherfassungEdit : Store, IValidatableObject
    {
        public Batcherfassung Batch { get; set; }
       
        [XmlIgnore]
        public List<Fahrzeughersteller> HerstellerList { get { return GetViewModel == null ? new List<Fahrzeughersteller>() : GetViewModel().FahrzeugHersteller; } }

        [XmlIgnore]
        public List<SelectItem> AntriebeList
        {
            get
            {
                return PropertyCacheGet(() => new List<SelectItem>
                    {
                        new SelectItem ("", Localize.DropdownDefaultOptionNotSpecified),
                        new SelectItem ("B", Localize.EngineGasoline),
                        new SelectItem ("D", Localize.EngineDiesel),
                        new SelectItem ("K", Localize.EngineCompressor),
                    });
            }
        }

        [XmlIgnore]
        public List<Auftragsnummer> AuftragsnummerList { get { return GetViewModel == null ? new List<Auftragsnummer>() : GetViewModel().Auftragsnummern; } }

        [XmlIgnore]
        public List<ModelHersteller> ModelList { get { return GetViewModel == null ? new List<ModelHersteller>() : GetViewModel().ModelList; } }

        [GridHidden, NotMapped]
        public bool InsertModeTmp { get; set; }
      
        public BatcherfassungEdit SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }

        [LocalizedDisplay(LocalizeConstants.Error)]
        public string ValidationError { get; set; }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<BatcherfassungViewModel> GetViewModel { get; set; }

        [XmlIgnore, NotMapped, GridExportIgnore]
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Aktion { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Batch.UnitnummerVon.IsNullOrEmpty() && Batch.UnitnummerBis.IsNullOrEmpty() && Batch.Unitnummern.IsNullOrEmpty())
                yield return new ValidationResult(Localize.UnitNumbersRequired);

            if (Batch.AuftragsnummerVon.IsNotNullOrEmpty() && Batch.AuftragsnummerBis.IsNullOrEmpty())
                yield return new ValidationResult(Localize.InvalidOrderNoSelection);
        }
    }
}
