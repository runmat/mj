using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.ZldPartner.Contracts;
using CkgDomainLogic.ZldPartner.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.ZldPartner.ViewModels
{
    public class ZldPartnerZulassungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IZldPartnerZulassungenDataService DataService { get { return CacheGet<IZldPartnerZulassungenDataService>(); } }

        [XmlIgnore]
        public List<StornoGrund> Gruende { get { return DataService.Gruende; } }

        [XmlIgnore]
        public List<Material> Materialien { get { return DataService.Materialien; } }

        [XmlIgnore]
        public List<OffeneZulassung> OffeneZulassungen
        {
            get { return PropertyCacheGet(() => new List<OffeneZulassung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<OffeneZulassung> OffeneZulassungenToSave
        {
            get { return PropertyCacheGet(() => new List<OffeneZulassung>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<OffeneZulassung> OffeneZulassungenGridItems
        {
            get { return (EditMode ? OffeneZulassungen : OffeneZulassungenToSave); }
        }

        [XmlIgnore]
        public List<DurchgefuehrteZulassung> DurchgefuehrteZulassungen
        {
            get { return PropertyCacheGet(() => new List<DurchgefuehrteZulassung>()); }
            private set { PropertyCacheSet(value); }
        }

        public DurchgefuehrteZulassungenSuchparameter DurchgefuehrteZulassungenSelektor
        {
            get { return PropertyCacheGet(() => new DurchgefuehrteZulassungenSuchparameter { Kunde = "", Auswahl = "A" }); }
            set { PropertyCacheSet(value); }
        }

        public bool EditMode { get; set; }

        private const string KennzeichenRegexString = @"^[A-ZÄÖÜ]{1,3}-[A-Z]{1,2}\d{1,4}H?$";

        public string StatusWerte { get { return ",;EGG,eingegangen;IA,in Arbeit;DGF,durchgeführt;STO,storniert;FGS,fehlgeschlagen"; } }

        public bool SendingEnabled { get { return (OffeneZulassungen.Any(z => z.Status == "DGF" || z.Status == "STO")); } }

        public void DataInit()
        {
            EditMode = true;

            DataService.LoadStammdaten();
        }

        public void LoadOffeneZulassungen()
        {
            PropertyCacheClear(this, m => m.OffeneZulassungen);
            PropertyCacheClear(this, m => m.OffeneZulassungenToSave);
            PropertyCacheClear(this, m => m.OffeneZulassungenGridItemsFiltered);

            OffeneZulassungen = DataService.LoadOffeneZulassungen();
        }

        private bool ValidateOffeneZulassungen(bool nurSpeichern)
        {
            foreach (var item in OffeneZulassungen)
            {
                item.ValidationErrorList.Clear();

                var gebuehrInvalid = false;

                if (item.Gebuehrenrelevant && !String.IsNullOrEmpty(item.Gebuehr) && !item.Gebuehr.IsDecimal())
                {
                    item.ValidationErrorList.Add(new ValidationResult(Localize.FeeInvalid, new[] { "Gebuehr" }));
                    gebuehrInvalid = true;
                }

                if (item.Hauptposition)
                {
                    if (!String.IsNullOrEmpty(item.ZulassungsDatum) && !item.ZulassungsDatum.IsDate())
                        item.ValidationErrorList.Add(new ValidationResult(Localize.RegistrationDateInvalid, new[] { "ZulassungsDatum" }));

                    if (item.Status == "DGF" && !nurSpeichern)
                    {
                        if (!Regex.IsMatch(item.Kennzeichen, KennzeichenRegexString))
                            item.ValidationErrorList.Add(new ValidationResult(Localize.LicenseNoInvalid, new[] { "Kennzeichen" }));

                        if (!gebuehrInvalid && item.Gebuehr.ToDouble(0) < 0.02)
                            item.ValidationErrorList.Add(new ValidationResult("Gebühr darf nicht 0,00 EUR oder 0,01 EUR sein", new[] { "Gebuehr" }));
                    }

                    if (item.Status.In("FGS,STO") && string.IsNullOrEmpty(item.StornoGrundId))
                        item.ValidationErrorList.Add(new ValidationResult(string.Format("{0} {1}", Localize.Reason, Localize.Required.NotNullOrEmpty().ToLower()), new[] { "StornoGrundId" }));
                }
            }

            return (OffeneZulassungen.All(z => z.ValidationOk));
        }

        public OffeneZulassung GetOffeneZulassungById(string id)
        {
            return OffeneZulassungen.Find(z => z.DatensatzId == id);
        }

        public StornoModel GetStornoModel(string datensatzId, string status)
        {
            return new StornoModel { DatensatzId = datensatzId, Status = status };
        }

        public AddPositionModel GetAddPositionModel(string belegNr)
        {
            var item = OffeneZulassungen.First(z => z.BelegNr == belegNr && z.Hauptposition);

            return new AddPositionModel { BelegNr = belegNr, Werk = item.Werk };
        }

        public bool CheckGrundBemerkung(string grundId)
        {
            var grund = Gruende.FirstOrDefault(g => g.GrundId == grundId);

            return (grund != null && grund.MitBemerkung);
        }

        public bool CheckMaterialPreis(string materialNr)
        {
            var material = Materialien.FirstOrDefault(m => m.MaterialNr == materialNr);

            return (material != null && material.PreisEingebbar);
        }

        public void TryAddPosition(AddPositionModel model, ModelStateDictionary state)
        {
            if (OffeneZulassungen.Any(z => z.BelegNr == model.BelegNr && z.MaterialNr == model.MaterialNr))
            {
                state.AddModelError("", Localize.ServiceAlreadyExists);
                return;
            }

            var vorhandenePositionen = OffeneZulassungen.Where(z => z.BelegNr == model.BelegNr).ToList();
            vorhandenePositionen.ForEach(z => z.IsChanged = true);

            var newItem = ModelMapping.Copy(vorhandenePositionen.First());

            newItem.NeuePosition = true;
            newItem.BelegPosition = (vorhandenePositionen.Select(vz => vz.BelegPosition.ToInt(0)).Max() + 10).ToString();
            newItem.Hauptposition = false;
            newItem.MaterialNr = model.MaterialNr;
            newItem.Gebuehr = null;
            newItem.Gebuehrenrelevant = false;
            newItem.IsChanged = true;
            newItem.MaterialText = model.Material.MaterialText;
            newItem.Preis = model.Preis;

            OffeneZulassungen.Add(newItem);

            PropertyCacheClear(this, m => m.OffeneZulassungenToSave);
            PropertyCacheClear(this, m => m.OffeneZulassungenGridItemsFiltered);
        }

        public void ApplyChangedData(string datensatzId, string property, string value, string grundId = "", string grundBemerkung = "")
        {
            var zul = OffeneZulassungen.FirstOrDefault(z => z.DatensatzId == datensatzId);
            if (zul != null)
            {
                var sonstigePositionen = OffeneZulassungen.Where(z => z.BelegNr == zul.BelegNr && z.BelegPosition != zul.BelegPosition).ToList();

                switch (property)
                {
                    case "ZulassungsDatum":
                        if (zul.ZulassungsDatum != value)
                        {
                            zul.ZulassungsDatum = value;
                            zul.LieferDatum = value;
                            zul.IsChanged = true;

                            sonstigePositionen.ForEach(z => z.IsChanged = true);
                        }
                        break;
                    case "Kennzeichen":
                        var kennz = value.NotNullOrEmpty().ToUpper();
                        if (zul.Kennzeichen != kennz)
                        {
                            zul.Kennzeichen = kennz;
                            zul.IsChanged = true;

                            sonstigePositionen.ForEach(z => z.IsChanged = true);
                        }
                        break;
                    case "Gebuehr":
                        if (zul.Gebuehr != value)
                        {
                            zul.Gebuehr = value;
                            zul.IsChanged = true;

                            sonstigePositionen.ForEach(z => z.IsChanged = true);
                        }
                        break;
                    case "Status":
                        if (zul.Status != value)
                        {
                            zul.Status = value;
                            zul.StornoGrundId = grundId;
                            zul.StornoBemerkung = grundBemerkung;
                            zul.IsChanged = true;

                            sonstigePositionen.ForEach(z =>
                            {
                                z.Status = value;
                                z.IsChanged = true;
                            });
                        }
                        break;
                }
            }
        }

        public void SaveOffeneZulassungen(bool nurSpeichern, ModelStateDictionary state)
        {
            if (!ValidateOffeneZulassungen(nurSpeichern))
            {
                state.AddModelError("", Localize.DataWithErrorsOccurred);
                return;
            }

            PropertyCacheClear(this, m => m.OffeneZulassungenToSave);
            PropertyCacheClear(this, m => m.OffeneZulassungenGridItemsFiltered);

            if (nurSpeichern)
                OffeneZulassungenToSave = OffeneZulassungen.Where(z => z.IsChanged).ToList();
            else
                OffeneZulassungenToSave = OffeneZulassungen.Where(z => z.Status == "DGF" || z.Status == "STO").ToList();

            if (OffeneZulassungenToSave.None())
            {
                state.AddModelError("", Localize.NoDataChanged);
                return;
            }

            var savedItems = DataService.SaveOffeneZulassungen(nurSpeichern, OffeneZulassungenToSave);

            foreach (var zulItem in OffeneZulassungenToSave)
            {
                var saveItem = savedItems.FirstOrDefault(z => z.DatensatzId == zulItem.DatensatzId);
                if (saveItem != null)
                {
                    zulItem.SaveMessage = saveItem.SaveMessage;
                    if (zulItem.SaveOk)
                        zulItem.IsChanged = false;
                }
            }

            if (!nurSpeichern)
                EditMode = false;
        }

        public void LoadDurchgefuehrteZulassungen(ModelStateDictionary state)
        {
            PropertyCacheClear(this, m => m.DurchgefuehrteZulassungen);
            PropertyCacheClear(this, m => m.DurchgefuehrteZulassungenFiltered);

            List<DurchgefuehrteZulassung> tmpZulassungen;
            var errMessage = DataService.LoadDurchgefuehrteZulassungen(DurchgefuehrteZulassungenSelektor, out tmpZulassungen);
            DurchgefuehrteZulassungen = tmpZulassungen;

            if (!string.IsNullOrEmpty(errMessage))
                state.AddModelError("", errMessage);
        }

        #region Filter

        [XmlIgnore]
        public List<OffeneZulassung> OffeneZulassungenGridItemsFiltered
        {
            get { return PropertyCacheGet(() => OffeneZulassungenGridItems); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterOffeneZulassungenGridItems(string filterValue, string filterProperties)
        {
            OffeneZulassungenGridItemsFiltered = OffeneZulassungenGridItems.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        [XmlIgnore]
        public List<DurchgefuehrteZulassung> DurchgefuehrteZulassungenFiltered
        {
            get { return PropertyCacheGet(() => DurchgefuehrteZulassungen); }
            private set { PropertyCacheSet(value); }
        }

        public void FilterDurchgefuehrteZulassungen(string filterValue, string filterProperties)
        {
            DurchgefuehrteZulassungenFiltered = DurchgefuehrteZulassungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion
    }
}
