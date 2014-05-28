// ReSharper disable RedundantUsingDirective
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.VersEvents.Contracts;
using CkgDomainLogic.VersEvents.Models;
using CkgDomainLogic.VersEvents.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Services;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.VersEvents.ViewModels
{
    public class VersEventsViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IVersEventsDataService DataService { get { return CacheGet<IVersEventsDataService>(); } }

        public Func<IEnumerable> FilteredObjectsCurrent
        {
            get { return PropertyCacheGet(() => (Func<IEnumerable>)(() => VersEventsFiltered)); }
            set { PropertyCacheSet(value); }
        }

        public bool InsertMode { get; set; }


        public void DataInit()
        {
            VersEventsLoad();
            DataMarkForRefreshVorgaenge();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.VersEventSelector);
            PropertyCacheClear(this, m => m.VersEventsFiltered);
            DataMarkForRefreshOrte();
        }

        public void DataMarkForRefreshOrte()
        {
            PropertyCacheClear(this, m => m.VersEventOrteAlle);
            PropertyCacheClear(this, m => m.VersEventOrteFiltered);
            DataMarkForRefreshOrtBoxen();
        }

        public void DataMarkForRefreshOrtBoxen()
        {
            PropertyCacheClear(this, m => m.VersEventOrtBoxenFiltered);
        }

        public void DataMarkForRefreshVorgaenge()
        {
            Vorgang.Laender = DataService.Laender;
            Vorgang.Versicherungen = DataService.Versicherungen;

            PropertyCacheClear(this, m => m.Vorgaenge);
            PropertyCacheClear(this, m => m.VorgaengeFiltered);
        }

        public void DataMarkForRefreshTermine()
        {
            PropertyCacheClear(this, m => m.Termine);
            PropertyCacheClear(this, m => m.TermineFiltered);
        }

        public void Validate(Action<string, string> addModelError)
        {
        }


        #region Vorgaenge

        [XmlIgnore]
        public List<Vorgang> Vorgaenge
        {
            get { return PropertyCacheGet(() => DataService.VorgaengeGet()); }
        }

        [XmlIgnore]
        public List<Vorgang> VorgaengeFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => VorgaengeFiltered;
                return PropertyCacheGet(() => Vorgaenge);
            }
            private set { PropertyCacheSet(value); }
        }

        public void VorgaengeFilter(string filterValue, string filterProperties)
        {
            VorgaengeFiltered = Vorgaenge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public Vorgang VorgangGet(int id)
        {
            var item = Vorgaenge.FirstOrDefault(c => c.ID == id);

            return item;
        }

        public Vorgang VorgangCreate()
        {
            var item = new Vorgang
            {
                KundenNr = LogonContext.KundenNr.ToInt(),
                AnlageUser = LogonContext.UserName,
                AnlageDatum = DateTime.Now,
            };

            return item;
        }

        public Vorgang VorgangAdd(Vorgang newVorgang)
        {
            newVorgang = DataService.VorgangAdd(newVorgang);
            Vorgaenge.Add(newVorgang);
            return newVorgang;
        }

        public void VorgangDelete(int id)
        {
            Vorgaenge.Remove(VorgangGet(id));
            DataService.VorgangDelete(id);

            DataMarkForRefreshVorgaenge();
        }

        public Vorgang VorgangSave(Vorgang item, Action<string, string> addModelError)
        {
            var savedVorgang = DataService.VorgangSave(item, addModelError);
            DataMarkForRefreshVorgaenge();
            return savedVorgang;
        }

        public void VorgangValidate(Vorgang model, bool insertMode, Action<Expression<Func<Vorgang, object>>, string> addModelError)
        {
        }

        #endregion


        #region Termine

        [XmlIgnore]
        public List<VorgangTermin> Termine
        {
            get { return PropertyCacheGet(() => DataService.TermineGet(null)); }
        }

        [XmlIgnore]
        public List<VorgangTermin> TermineFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => TermineFiltered;
                return PropertyCacheGet(() => Termine);
            }
            private set { PropertyCacheSet(value); }
        }

        public VorgangTermin TerminCurrent { get; private set; }


        public void TermineFilter(string filterValue, string filterProperties)
        {
            TermineFiltered = Termine.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public VorgangTermin TerminGet(int id)
        {
            var item = Termine.FirstOrDefault(c => c.ID == id);
            TerminCurrent = item;

            return item;
        }

        public VorgangTermin TerminCreate(int versBoxID, int vorgangID)
        {
            var item = new VorgangTermin
            {
                VersBoxID = versBoxID,
                VorgangID = vorgangID,
                Datum = DateTime.Today,
                DauerMinuten = 60,
                KundenNr = LogonContext.KundenNr.ToInt(),
                AnlageUser = LogonContext.UserName,
                AnlageDatum = DateTime.Now,
            };

            return item;
        }

        public VorgangTermin TerminAdd(VorgangTermin newTermin)
        {
            newTermin = DataService.TerminAdd(newTermin);
            Termine.Add(newTermin);
            TerminCurrent = newTermin;
            return newTermin;
        }

        public void TerminDelete(int id)
        {
            Termine.Remove(TerminGet(id));
            DataService.TerminDelete(id);

            DataMarkForRefreshTermine();
        }

        public VorgangTermin TerminSave(VorgangTermin item, Action<string, string> addModelError)
        {
            var savedTermin = DataService.TerminSave(item, addModelError);
            DataMarkForRefreshTermine();
            return savedTermin;
        }

        public void TerminValidate(VorgangTermin model, Action<Expression<Func<VorgangTermin, object>>, string> addModelError)
        {
        }

        #endregion


        #region VersEvent

        [XmlIgnore]
        public List<VersEvent> VersEvents
        {
            get { return PropertyCacheGet(() => new List<VersEvent>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<VersEvent> VersEventsFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => VersEventsFiltered;
                return PropertyCacheGet(() => VersEvents);
            }
            private set { PropertyCacheSet(value); }
        }

        public VersEventSelector VersEventSelector
        {
            get { return PropertyCacheGet(() => new VersEventSelector()); }
            set { PropertyCacheSet(value); }
        }

        public VersEvent VersEventCurrent { get; private set; }


        public void VersEventsLoad()
        {
            DataMarkForRefresh();
            VersEvents = DataService.VersEventsGet(VersEventSelector);
            DataMarkForRefresh();
        }

        public void VersEventsFilter(string filterValue, string filterProperties)
        {
            VersEventsFiltered = VersEvents.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public VersEvent VersEventGet(int id)
        {
            var item = VersEvents.FirstOrDefault(c => c.ID == id);
            
            VersEventCurrent = item;
            VersEventOrteLoad();
            
            return item;
        }

        public VersEvent VersEventCreate()
        {
            var item = new VersEvent
            {
                EventName = "",
                KundenNr = LogonContext.KundenNr.ToInt(),
                AnlageUser = LogonContext.UserName,
                AnlageDatum = DateTime.Now,
                StartDatum = DateTime.Today,
                EndDatum = DateTime.Today.AddMonths(3)
            };
            
            VersEventCurrent = item;
            VersEventOrteLoad();
            
            return item;
        }

        public VersEvent VersEventAdd(VersEvent newVersEvent)
        {
            newVersEvent = DataService.VersEventAdd(newVersEvent);
            VersEvents.Add(newVersEvent);
            return newVersEvent;
        }

        public void VersEventDelete(int id)
        {
            VersEvents.Remove(VersEventGet(id));
            DataService.VersEventDelete(id);

            DataMarkForRefresh();
        }

        public VersEvent VersEventSave(VersEvent item, Action<string, string> addModelError)
        {
            var savedVersEvent = DataService.VersEventSave(item, addModelError);
            DataMarkForRefresh();
            return savedVersEvent;
        }

        public void VersEventValidate(VersEvent model, Action<Expression<Func<VersEvent, object>>, string> addModelError)
        {
        }

        #endregion


        #region VersEventOrt

        [XmlIgnore]
        public List<VersEventOrt> VersEventOrteAlle
        {
            get { return PropertyCacheGet(() => DataService.VersEventOrteGet()); }
        }

        [XmlIgnore]
        public List<VersEventOrt> VersEventOrte
        {
            get { return PropertyCacheGet(() => new List<VersEventOrt>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<VersEventOrt> VersEventOrteFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => VersEventOrteFiltered;
                return PropertyCacheGet(() => VersEventOrte);
            }
            private set { PropertyCacheSet(value); }
        }

        public VersEventOrt VersEventOrtCurrent { get; private set; }

        public void VersEventOrteLoad()
        {
            VersEventOrt.Laender = DataService.Laender;

            VersEventOrte = VersEventOrteAlle.Where(o => o.VersEventID == VersEventCurrent.ID).ToList();
            DataMarkForRefreshOrte();
        }

        public void VersEventOrteFilter(string filterValue, string filterProperties)
        {
            VersEventOrteFiltered = VersEventOrte.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public VersEventOrt VersEventOrtGet(int id)
        {
            var item = VersEventOrte.FirstOrDefault(c => c.ID == id);

            VersEventOrtCurrent = item;
            VersEventOrtBoxenLoad();

            return item;
        }

        public VersEventOrt VersEventOrtCreate()
        {
            var item = new VersEventOrt
                {
                    OrtName = "",
                    VersEventID = VersEventCurrent.ID,
                    Land = "DE",
                };

            VersEventOrtCurrent = item;
            VersEventOrtBoxenLoad();

            return item;
        }

        public VersEventOrt VersEventOrtAdd(VersEventOrt newVersEventOrt)
        {
            newVersEventOrt = DataService.VersEventOrtAdd(newVersEventOrt);
            VersEventOrte.Add(newVersEventOrt);
            return newVersEventOrt;
        }

        public void VersEventOrtDelete(int id)
        {
            VersEventOrte.Remove(VersEventOrtGet(id));
            DataService.VersEventOrtDelete(id);

            DataMarkForRefresh();
        }

        public VersEventOrt VersEventOrtSave(VersEventOrt item, Action<string, string> addModelError)
        {
            var savedVersEventOrt = DataService.VersEventOrtSave(item, addModelError);
            DataMarkForRefresh();
            return savedVersEventOrt;
        }

        public void VersEventOrtValidate(VersEventOrt model, Action<Expression<Func<VersEventOrt, object>>, string> addModelError)
        {
        }

        #endregion


        #region VersEventOrtBox

        [XmlIgnore]
        public List<VersEventOrtBox> VersEventOrtBoxen
        {
            get { return PropertyCacheGet(() => new List<VersEventOrtBox>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<VersEventOrtBox> VersEventOrtBoxenFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => VersEventOrtBoxenFiltered;
                return PropertyCacheGet(() => VersEventOrtBoxen);
            }
            private set { PropertyCacheSet(value); }
        }

        public void VersEventOrtBoxenLoad()
        {
            VersEventOrtBox.Versicherungen = DataService.Versicherungen;

            VersEventOrtBoxen = DataService.VersEventOrtBoxenGet(VersEventOrtCurrent);
            DataMarkForRefreshOrtBoxen();
        }

        public void VersEventOrtBoxenFilter(string filterValue, string filterProperties)
        {
            VersEventOrtBoxenFiltered = VersEventOrtBoxen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public VersEventOrtBox VersEventOrtBoxGet(int id)
        {
            return VersEventOrtBoxen.FirstOrDefault(c => c.ID == id);
        }

        public VersEventOrtBox VersEventOrtBoxCreate()
        {
            return new VersEventOrtBox
                {
                    BoxName = "",
                    BoxArt = "GU",
                    VersOrtID = VersEventOrtCurrent.ID
                };
        }

        public VersEventOrtBox VersEventOrtBoxAdd(VersEventOrtBox newVersEventOrtBox)
        {
            newVersEventOrtBox = DataService.VersEventOrtBoxAdd(newVersEventOrtBox);
            VersEventOrtBoxen.Add(newVersEventOrtBox);
            return newVersEventOrtBox;
        }

        public void VersEventOrtBoxDelete(int id)
        {
            VersEventOrtBoxen.Remove(VersEventOrtBoxGet(id));
            DataService.VersEventOrtBoxDelete(id);

            DataMarkForRefresh();
        }

        public VersEventOrtBox VersEventOrtBoxSave(VersEventOrtBox item, Action<string, string> addModelError)
        {
            var savedVersEventOrtBox = DataService.VersEventOrtBoxSave(item, addModelError);
            DataMarkForRefresh();
            return savedVersEventOrtBox;
        }

        public void VersEventOrtBoxValidate(VersEventOrtBox model, Action<Expression<Func<VersEventOrtBox, object>>, string> addModelError)
        {
            if (model.BoxArt == "RE")
            {
                // bei BoxArt "Techniker" die Versicherung "löschen"
                model.VersicherungID = null;

                if (model.TechnikerName.IsNullOrEmpty())
                {
                    addModelError(m => m.TechnikerName, "Bitte geben Sie einen Techniker an");
                    return;
                }
            }

            if (model.BoxArt == "GU")
            {
                if (model.TaktungMinuten == 0)
                {
                    addModelError(m => m.TaktungMinuten, "Bitte geben Sie eine Taktung in Minuten an");
                    return;
                }

                // bei BoxArt "Versicherung" den Techniker "löschen"
                model.TechnikerName = null;

                if (model.VersicherungID.IsNullOrEmpty())
                {
                    addModelError(m => m.VersicherungID, "Bitte geben Sie eine Versicherung an");
                    return;
                }

                var insuranceName = DataService.Versicherungen.First(v => v.Key == model.VersicherungID).Text;
                var otherBoxWithSeparateMinuteRaster = VersEventOrtBoxen
                    .FirstOrDefault(box =>
                        box.VersOrtID == model.VersOrtID &&
                        box.VersicherungID == model.VersicherungID &&
                        box.ID != model.ID && 
                        box.TaktungMinuten != model.TaktungMinuten
                );

                if (otherBoxWithSeparateMinuteRaster != null)
                    addModelError(m => m.TaktungMinuten,
                        string.Format("Die Taktung muss {0} Minuten betragen, da diese Taktung bereits durch Box '{1}' mit Versicherung '{2}' vorgegeben wurde",
                            otherBoxWithSeparateMinuteRaster.TaktungMinuten, otherBoxWithSeparateMinuteRaster.BoxName, insuranceName));
            }
        }

        #endregion
    }
}
