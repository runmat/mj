// ReSharper disable RedundantUsingDirective
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Services;
using SapORM.Contracts;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Insurance.ViewModels
{
    public class VersEventsViewModel : CkgBaseViewModel
    {
        private const string LanguageKey = "DE";
        private const int GutachtenTerminStatusID = 2;

        [XmlIgnore]
        public IVersEventsDataService DataService { get { return CacheGet<IVersEventsDataService>(); } }

        public List<SelectItem> Versicherungen
        {
            get { return PropertyCacheGet(() => DataService.Versicherungen); }
        }

        public Func<IEnumerable> FilteredObjectsCurrent
        {
            get { return PropertyCacheGet(() => (Func<IEnumerable>)(() => VersEventsFiltered)); }
            set { PropertyCacheSet(value); }
        }

        public bool InsertMode { get; set; }


        public void DataInit()
        {
            VersEventsLoad();
        }

        public void DataInitTest()
        {
            VersEventCurrent = VersEvents.FirstOrDefault(item => item.ID == 18);
            VersEventOrteLoad();

            VersEventOrtCurrent = VersEventOrte.FirstOrDefault(item => item.ID == 11);
            VersEventOrtBoxenLoad();

            BoxCurrent = VersEventOrtBoxGet(19);
        }

        public void DataMarkForRefresh()
        {
            DataMarkForRefreshEvents();
            DataMarkForRefreshOrte();
            DataMarkForRefreshOrtBoxen();

            DataMarkForRefreshSchadenfaelle();
            DataMarkForRefreshTermine();
        }

        void DataMarkForRefreshEvents()
        {
            PropertyCacheClear(this, m => m.VersEventsFiltered);
        }

        public void DataMarkForRefreshOrte()
        {
            PropertyCacheClear(this, m => m.VersEventOrteAlle);
            PropertyCacheClear(this, m => m.VersEventOrteFiltered);
            VersEvents.ForEach(e => e.OrteReset());

            DataMarkForRefreshOrtBoxen();
        }

        public void DataMarkForRefreshOrtBoxen()
        {
            PropertyCacheClear(this, m => m.VersEventOrtBoxenFiltered);
        }

        public void DataMarkForRefreshSchadenfaelle()
        {
            //Schadenfall.Laender = DataService.Laender;
            //Schadenfall.Versicherungen = DataService.Versicherungen;

            PropertyCacheClear(this, m => m.Schadenfaelle);
            PropertyCacheClear(this, m => m.SchadenfaelleFiltered);
        }

        public void DataMarkForRefreshTermine()
        {
            PropertyCacheClear(this, m => m.AlleTermine);
            PropertyCacheClear(this, m => m.AlleTermineFiltered);

            PropertyCacheClear(this, m => m.Termine);
            PropertyCacheClear(this, m => m.TermineFiltered);

            PropertyCacheClear(this, m => m.BoxBlockerTermine);
            PropertyCacheClear(this, m => m.BoxBlockerTermineFiltered);
        }


        #region Schadenfall Status Arten

        [XmlIgnore]
        public List<SchadenfallStatusArt> SchadenfallStatusArten
        {
            get { return PropertyCacheGet(() => DataService.SchadenfallStatusArtenGet(LanguageKey)); }
        }

        #endregion


        #region Schadenfall Status (Werte)

        int? SchadenfallCurrentID { get { return SchadenfallCurrent == null ? null : (int?)SchadenfallCurrent.ID; } }

        [XmlIgnore]
        public List<SchadenfallStatus> SchadenfallStatusWerte
        {
            get { return PropertyCacheGet(() => DataService.SchadenfallStatusWerteGet(LanguageKey, SchadenfallCurrentID)); }
        }

        [XmlIgnore]
        public List<SchadenfallStatus> SchadenfallStatusWerteWithNulls
        {
            get 
            { 
                return PropertyCacheGet(() => DataService.SchadenfallStatusArtenGet(LanguageKey)
                                                         .OrderBy(art => art.Sort)
                                                         .Select(art =>
                                                             {
                                                                 var existingStatus = SchadenfallStatusWerte.FirstOrDefault(status => status.StatusArtID == art.ArtID);
                                                                 if (existingStatus == null)
                                                                     return new SchadenfallStatus
                                                                         {
                                                                             VersSchadenfallID = SchadenfallCurrentID.GetValueOrDefault(),
                                                                             StatusArtID = art.ArtID,
                                                                             Bezeichnung = art.Bezeichnung,
                                                                             Sort = art.Sort,
                                                                         };

                                                                 return existingStatus;
                                                             }).ToList()); 
            }
        }

        public List<string> SchadenfallStatusWertSave(int itemID, bool toggleDisabled = false)
        {
            var errorList = new List<string>();

            var item = SchadenfallStatusWerteWithNulls.FirstOrDefault(s => s.StatusArtID == itemID);
            if (item == null)
                return errorList;

            if (toggleDisabled && item.User.IsNotNullOrEmpty())
                return errorList;

            if (item.User.IsNullOrEmpty())
            {
                item.User = LogonContext.UserName;
                item.Datum = DateTime.Now;
                item.Zeit = DateTime.Now.ToString("HH:mm");
            }
            else
            {
                item.User = null;
                item.Datum = null;
                item.Zeit = null;
            }

            DataService.SchadenfallStatusWertSave(item, (key, error) => errorList.Add(error));
            PropertyCacheClear(this, m => m.SchadenfallStatusWerte);
            PropertyCacheClear(this, m => m.SchadenfallStatusWerteWithNulls);

            return errorList;
        }

        #endregion


        #region Schadenfaelle

        [XmlIgnore]
        public List<Schadenfall> Schadenfaelle
        {
            get { return PropertyCacheGet(() => DataService.SchadenfaelleGet()); }
        }

        [XmlIgnore]
        public List<Schadenfall> SchadenfaelleFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => SchadenfaelleFiltered;
                return PropertyCacheGet(() => Schadenfaelle);
            }
            private set { PropertyCacheSet(value); }
        }

        public Schadenfall SchadenfallCurrent { get; private set; }

        public void SchadenfaelleFilter(string filterValue, string filterProperties)
        {
            SchadenfaelleFiltered = Schadenfaelle.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public Schadenfall SchadenfallGet(int id)
        {
            var item = Schadenfaelle.FirstOrDefault(c => c.ID == id);

            return item;
        }

        public Schadenfall SchadenfallCreate()
        {
            var item = new Schadenfall
            {
                KundenNr = LogonContext.KundenNr,
                AnlageUser = LogonContext.UserName,
                AnlageDatum = DateTime.Now,
            };

            return item;
        }

        public Schadenfall SchadenfallAdd(Schadenfall newItem, Action<string, string> addModelError)
        {
            newItem = DataService.SchadenfallAdd(newItem, addModelError);
            Schadenfaelle.Add(newItem);
            DataMarkForRefreshSchadenfaelle();
            return newItem;
        }

        public void SchadenfallDelete(int id)
        {
            Schadenfaelle.Remove(SchadenfallGet(id));
            DataService.SchadenfallDelete(id);

            DataMarkForRefreshSchadenfaelle();
        }

        public Schadenfall SchadenfallSave(Schadenfall item, Action<string, string> addModelError)
        {
            if (InsertMode)
                return SchadenfallAdd(item, addModelError);

            var savedItem = DataService.SchadenfallSave(item, addModelError);
            DataMarkForRefreshSchadenfaelle();
            return savedItem;
        }

        public void SchadenfallValidate(Schadenfall model, Action<string, string> addModelError)
        {
            var dublette = Schadenfaelle.FirstOrDefault(s => s.EventID == model.EventID && s.Kennzeichen.ToUpper() == model.Kennzeichen.ToUpper() && s.ID != model.ID);
            if (dublette != null)
                addModelError("", Localize.DamageCaseAlreadyExists);
        }

        public void SchadenfallCurrentDisable()
        {
            LoadSchadenfall(0);
        }

        public void LoadSchadenfall(int schadenfallID)
        {
            SchadenfallCurrent = Schadenfaelle.FirstOrDefault(fall => fall.ID == schadenfallID);
            PropertyCacheClear(this, m => m.SchadenfallStatusWerte);
            PropertyCacheClear(this, m => m.SchadenfallStatusWerteWithNulls);

            DataMarkForRefreshTermine();
        }

        #endregion


        #region Alle Termine

        [XmlIgnore]
        public List<TerminSchadenfall> AlleTermine
        {
            get { return PropertyCacheGet(() => DataService.TermineGet()); }
        }

        [XmlIgnore]
        public List<TerminSchadenfall> AlleTermineFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => AlleTermineFiltered;
                return PropertyCacheGet(() => AlleTermine);
            }
            private set { PropertyCacheSet(value); }
        }

        public void AlleTermineFilter(string filterValue, string filterProperties)
        {
            AlleTermineFiltered = AlleTermine.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


        #region Blocker Termine

        public VersEventOrtBox BoxCurrent { get; private set; }

        [XmlIgnore]
        public List<TerminSchadenfall> BoxBlockerTermine
        {
            get { return PropertyCacheGet(() => DataService.TermineGet(null, BoxCurrent == null ? -1 : BoxCurrent.ID)); }
        }

        [XmlIgnore]
        public List<TerminSchadenfall> BoxBlockerTermineFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => BoxBlockerTermineFiltered;
                return PropertyCacheGet(() => BoxBlockerTermine);
            }
            private set { PropertyCacheSet(value); }
        }

        public void BoxBlockerTermineFilter(string filterValue, string filterProperties)
        {
            BoxBlockerTermineFiltered = BoxBlockerTermine.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public TerminSchadenfall BoxBlockerTerminGet(int id)
        {
            var item = BoxBlockerTermine.FirstOrDefault(c => c.ID == id);
            TerminCurrent = item;

            return item;
        }

        public TerminSchadenfall BoxBlockerTerminCreate()
        {
            if (BoxCurrent == null)
                return new TerminSchadenfall();

            var termin = TerminCreate(BoxCurrent.ID, 0);
            termin.VersOrtID = BoxCurrent.VersOrtID;

            return termin;
        }

        public void BoxBlockerTerminDelete(int id)
        {
            BoxBlockerTermine.Remove(TerminGet(id));
            DataService.TerminDelete(id);

            DataMarkForRefreshTermine();
        }

        #endregion 

        
        #region Termine

        [XmlIgnore]
        public List<TerminSchadenfall> Termine
        {
            get { return PropertyCacheGet(() => 
                (SchadenfallCurrent == null 
                    ? new List<TerminSchadenfall>() 
                    : AlleTermine.Where(termin => termin.VersSchadenfallID == SchadenfallCurrent.ID).ToList())); }
        }

        [XmlIgnore]
        public List<TerminSchadenfall> TermineFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => TermineFiltered;
                return PropertyCacheGet(() => Termine);
            }
            private set { PropertyCacheSet(value); }
        }

        public TerminSchadenfall TerminCurrent { get; private set; }

        public List<TerminSchadenfall> GetTermineEinerBoxAllerSchadenFaelle(int versBoxID)
        {
            return DataService.TermineGet(null, versBoxID);
        }


        public void TermineFilter(string filterValue, string filterProperties)
        {
            TermineFiltered = Termine.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public TerminSchadenfall TerminGet(int id)
        {
            var item = Termine.FirstOrDefault(c => c.ID == id);
            TerminCurrent = item;

            return item;
        }

        public TerminSchadenfall TerminCreate()
        {
            return TerminCreate(-1, SchadenfallCurrent == null ? -1 : SchadenfallCurrent.ID);
        }

        public TerminSchadenfall TerminCreate(int versBoxID, int schadenfallID)
        {
            var item = new TerminSchadenfall
            {
                VersBoxID = versBoxID,
                VersSchadenfallID = schadenfallID,
                Datum = DateTime.Today,
                KundenNr = LogonContext.KundenNr,
                AnlageUser = LogonContext.UserName,
                AnlageDatum = DateTime.Now,
                BoxArtGewuenscht = "GU"
            };

            TerminCurrent = item;

            return item;
        }

        TerminSchadenfall TerminAdd(TerminSchadenfall newTermin, Action<string, string> addModelError, List<TerminSchadenfall> termine)
        {
            if (!newTermin.Validate(addModelError))
                return newTermin;

            newTermin = DataService.TerminAdd(newTermin, addModelError);
            termine.Add(newTermin);
            TerminCurrent = newTermin;

            if (TerminCurrent.GetCachedBoxArt() == "GU")
                // Status "GA Termin vergeben" in Schadenakte speichern
                SchadenfallStatusWertSave(GutachtenTerminStatusID, true);
            
            DataMarkForRefreshTermine();

            return newTermin;
        }

        TerminSchadenfall TerminSave(TerminSchadenfall termin, Action<string, string> addModelError)
        {
            if (!termin.Validate(addModelError))
                return termin;

            var savedTermin = DataService.TerminSave(termin, addModelError);

            var cachedItem = AlleTermine.FirstOrDefault(i => i.ID == termin.ID);
            if (cachedItem != null)
                ModelMapping.Copy(savedTermin, cachedItem);

            TerminCurrent = savedTermin;

            DataMarkForRefreshTermine();

            return savedTermin;
        }

        public void TerminDelete(int id)
        {
            Termine.Remove(TerminGet(id));
            DataService.TerminDelete(id);

            DataMarkForRefreshTermine();
        }

        public void TerminCurrentPrepare(string boxArt, string key, string startDateString, int startTimeHours, int startTimeMinutes, int endTimeHours, int endTimeMinutes)
        {
            var termin = TerminCurrent;

            if (boxArt == "RE")
            {
                termin.VersBoxID = key.ToInt();
            }

            termin.Datum = DateTime.Parse(startDateString);
            termin.ZeitVon = string.Format("{0:00}:{1:00}", startTimeHours, startTimeMinutes);
            termin.ZeitBis = string.Format("{0:00}:{1:00}", endTimeHours, endTimeMinutes);
        }

        public TerminSchadenfall TerminCurrentSave(Action<string, string> addModelError, List<TerminSchadenfall> termine)
        {
            var termin = TerminCurrent;
            
            if (InsertMode)
                return TerminAdd(termin, addModelError, termine);

            return TerminSave(termin, addModelError);
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

        public VersEvent VersEventCurrent { get; private set; }


        public void VersEventsLoad()
        {
            VersEvents = DataService.VersEventsGet();
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
                KundenNr = LogonContext.KundenNr.ToSapKunnr(),
                AnlageUser = LogonContext.UserName,
                AnlageDatum = DateTime.Now,
                StartDatum = DateTime.Today,
                EndDatum = DateTime.Today.AddMonths(3)
            };
            
            VersEventCurrent = item;
            VersEventOrteLoad();
            
            return item;
        }

        public VersEvent VersEventAdd(VersEvent newVersEvent, Action<string, string> addModelError)
        {
            newVersEvent = DataService.VersEventAdd(newVersEvent, addModelError);
            VersEvents.Add(newVersEvent);
            
            VersEventCurrent = newVersEvent;
            VersEventOrteLoad();

            return newVersEvent;
        }

        public void VersEventDelete(int id)
        {
            VersEvents.Remove(VersEventGet(id));
            DataService.VersEventDelete(id);

            //DataMarkForRefresh();
        }

        public VersEvent VersEventSave(VersEvent item, Action<string, string> addModelError)
        {
            var savedVersEvent = DataService.VersEventSave(item, addModelError);
            var cachedItem = VersEvents.FirstOrDefault(i => i.ID == item.ID);
            if (cachedItem != null)
                ModelMapping.Copy(savedVersEvent, cachedItem);

            VersEventCurrent = savedVersEvent;
            
            //DataMarkForRefresh();
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

        public VersEventOrt VersEventOrtAdd(VersEventOrt newVersEventOrt, Action<string, string> addModelError)
        {
            newVersEventOrt = DataService.VersEventOrtAdd(newVersEventOrt, addModelError);
            VersEventOrte.Add(newVersEventOrt);
            
            VersEventOrtCurrent = newVersEventOrt;
            VersEventOrtBoxenLoad();
            
            return newVersEventOrt;
        }

        public void VersEventOrtDelete(int id)
        {
            VersEventOrte.Remove(VersEventOrtGet(id));
            DataService.VersEventOrtDelete(id);

            //DataMarkForRefresh();
        }

        public VersEventOrt VersEventOrtSave(VersEventOrt item, Action<string, string> addModelError)
        {
            var savedVersEventOrt = DataService.VersEventOrtSave(item, addModelError);
            var cachedItem = VersEventOrte.FirstOrDefault(i => i.ID == item.ID);
            if (cachedItem != null)
                ModelMapping.Copy(savedVersEventOrt, cachedItem);

            VersEventOrtCurrent = savedVersEventOrt;

            DataMarkForRefreshOrte();

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

        public VersEventOrtBox VersEventOrtBoxAdd(VersEventOrtBox newVersEventOrtBox, Action<string, string> addModelError)
        {
            newVersEventOrtBox = DataService.VersEventOrtBoxAdd(newVersEventOrtBox, addModelError);
            VersEventOrtBoxen.Add(newVersEventOrtBox);
            return newVersEventOrtBox;
        }

        public void VersEventOrtBoxDelete(int id)
        {
            VersEventOrtBoxen.Remove(VersEventOrtBoxGet(id));
            DataService.VersEventOrtBoxDelete(id);

            //DataMarkForRefresh();
        }

        public VersEventOrtBox VersEventOrtBoxSave(VersEventOrtBox item, Action<string, string> addModelError)
        {
            var savedVersEventOrtBox = DataService.VersEventOrtBoxSave(item, addModelError);
            var cachedItem = VersEventOrtBoxen.FirstOrDefault(i => i.ID == item.ID);
            if (cachedItem != null)
                ModelMapping.Copy(savedVersEventOrtBox, cachedItem);

            //DataMarkForRefresh();
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
