// ReSharper disable ConvertClosureToMethodGroup
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.ViewModels;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using System.Web.Mvc;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using SapORM.Contracts;

namespace CkgDomainLogic.Insurance.ViewModels
{
    public class VersEventsViewModel : CustomerDocumentViewModel
    {
        private string _languageKey;
        private const int GutachtenTerminStatusID = 2;

        [XmlIgnore]
        public IVersEventsDataService EventsDataService { get { return CacheGet<IVersEventsDataService>(); } }

        [XmlIgnore]
        public ISchadenakteDataService SchadenDataService { get { return CacheGet<ISchadenakteDataService>(); } }


        public List<SelectItem> Versicherungen
        {
            get { return PropertyCacheGet(() => EventsDataService.Versicherungen); }
        }

        public Func<IEnumerable> FilteredObjectsCurrent
        {
            get { return PropertyCacheGet(() => (Func<IEnumerable>)(() => VersEventsFiltered)); }
            set { PropertyCacheSet(value); }
        }

        public bool InsertMode { get; set; }


        public void DataInit(string userCulture)
        {
            _languageKey = (userCulture.IsNullOrEmpty() ? "DE" : userCulture.SubstringTry(0,2).ToUpper());

            AlleTermineFetchAllDetails = false;
            VersEventsLoad();
        }

        public void DataInitTest()
        {
            VersEventCurrent = VersEvents.FirstOrDefault(item => item.ID == 18);
            VersEventOrteLoad();

            VersEventOrtCurrent = VersEventOrte.FirstOrDefault(item => item.ID == 11);
            VersEventOrtBoxenLoad();

            BoxCurrent = VersEventOrtBoxGet(19);
            DataMarkForRefreshTermine();
        }

        public void DataMarkForRefresh()
        {
            DataMarkForRefreshEvents();
            DataMarkForRefreshOrte();
            DataMarkForRefreshOrtBoxen();

            DataMarkForRefreshSchadenfaelle();
            DataMarkForRefreshTermine();

            DataMarkForRefreshSchadenfallStatusWerte();
            DataMarkForRefreshSchadenfallStatusAlle();
            DataMarkForRefreshSchadenfallDokumenteAlle();
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
            PropertyCacheClear(this, m => m.AlleSchadenfaelleTermineFiltered);

            PropertyCacheClear(this, m => m.Termine);
            PropertyCacheClear(this, m => m.TermineFiltered);

            DataMarkForRefreshBlockerTermine();
        }

        public void DataMarkForRefreshBlockerTermine()
        {
            PropertyCacheClear(this, m => m.BoxBlockerTermine);
            PropertyCacheClear(this, m => m.BoxBlockerTermineFiltered);
        }

        public void DataMarkForRefreshSchadenfallStatusWerte()
        {
            PropertyCacheClear(this, m => m.AlleSchadenfallStatusWerte);
            PropertyCacheClear(this, m => m.SchadenfallCurrentStatusWerteWithNulls);
        }

        public void DataMarkForRefreshSchadenfallStatusAlle()
        {
			PropertyCacheClear(this, m => m.SchadenfallStatusArten);
            PropertyCacheClear(this, m => m.SchadenStatusAlle);
            PropertyCacheClear(this, m => m.SchadenStatusAlleFiltered);
        }

        public void DataMarkForRefreshSchadenfallDokumenteAlle()
        {
            DataService.ApplicationKey = "Schadenakte";
            PropertyCacheClear(this, m => m.CategoriesFiltered);
            PropertyCacheClear(this, m => m.AllDocumentsFiltered); 

            PropertyCacheClear(this, m => m.AlleSchadenfallDokumente);
            PropertyCacheClear(this, m => m.SchadenDokumenteAlle);
            PropertyCacheClear(this, m => m.SchadenDokumenteAlleFiltered);
        }

        int? SchadenfallCurrentID { get { return SchadenfallCurrent == null ? null : (int?)SchadenfallCurrent.ID; } }

        #region Schadenfall Status Arten

        [XmlIgnore]
        public List<SchadenfallStatusArt> SchadenfallStatusArten
        {
            get { return PropertyCacheGet(() => SchadenDataService.SchadenfallStatusArtenGet(_languageKey)); }
        }

        #endregion


        #region Schadenfall Status (Werte)

        [XmlIgnore]
        public List<SchadenfallStatus> AlleSchadenfallStatusWerte
        {
            get
            {
                return PropertyCacheGet(() => SchadenDataService.SchadenfallStatusWerteGet(_languageKey));
            }
        }

        [XmlIgnore]
        public List<SchadenfallStatus> SchadenfallCurrentStatusWerteWithNulls
        {
            get { return PropertyCacheGet(() => GetSchadenfallStatusWerteWithNulls(SchadenfallCurrentID.GetValueOrDefault())); }
        }

        public List<SchadenfallStatus> GetSchadenfallStatusWerteWithNulls(int schadenfallID)
        {
            return SchadenfallStatusArten
                                .OrderBy(art => art.Sort).ThenBy(art => art.ArtID)
                                    .Select(art =>
                                        {
                                            var item = AlleSchadenfallStatusWerte
                                                           .FirstOrDefault(
                                                               status =>
                                                               status.VersSchadenfallID == schadenfallID &&
                                                               status.StatusArtID == art.ArtID)
                                                       ?? new SchadenfallStatus
                                                           {
                                                               VersSchadenfallID = schadenfallID,
                                                               StatusArtID = art.ArtID,
                                                               Sort = art.Sort,
                                                           };
                                            item.Bezeichnung = art.Bezeichnung;
                                            return item;
                                        }).ToList();
        }

        public void SchadenfallStatusWertSave(int itemID, DateTime? saveDate)
        {
            var item = SchadenfallCurrentStatusWerteWithNulls.FirstOrDefault(s => s.StatusArtID == itemID);
            if (item == null)
                return;

            item.Datum = saveDate.GetValueOrDefault();

            SchadenfallStatusWertSave(item);
        }

        static bool SchadenfallStatusResetPermission { get { return ConfigurationManager.AppSettings["SchadenfallStatusResetPermission"].NotNullOrEmpty().ToLower() == "true"; } }

        public void SchadenfallStatusWertSave(SchadenfallStatus itemToUpdate)
        {
            if (itemToUpdate.Datum == null)
            {
                if (!SchadenfallStatusResetPermission)
                    throw new Exception("Keine ausreichende Berechtigung für Status-Reset!");

                itemToUpdate.Kommentar = null;
                itemToUpdate.User = null;
                itemToUpdate.Zeit = null;
            }
            else
            {
                itemToUpdate.User = LogonContext.UserName;
                if (itemToUpdate.Zeit.IsNullOrEmpty())
                    itemToUpdate.Zeit = DateTime.Now.ToString("HH:mm");
            }

            var errorList = new List<string>();
            SchadenDataService.SchadenfallStatusWertSave(itemToUpdate, (key, error) => errorList.Add(error));
            if (errorList.Any())
                throw new Exception(string.Join(", ", errorList));

            DataMarkForRefreshSchadenfallStatusWerte();
        }

        public string SchadenfallStatusAlleGetHeaderText(int index)
        {
            var zeroBasedIndex = index - 1;
            var statusArtArray = SchadenfallStatusArten.OrderBy(art => art.Sort).ThenBy(art => art.ArtID).ToArray();
            return (zeroBasedIndex >= statusArtArray.Length ? "" : statusArtArray[zeroBasedIndex].Bezeichnung);
        }

        public bool SchadenfallStatusAlleIsStatusVisible(int index)
        {
            return SchadenfallStatusAlleGetHeaderText(index).IsNotNullOrEmpty();
        }

        [XmlIgnore]
        public List<SchadenfallStatusAlle> SchadenStatusAlle
        {
            get { return PropertyCacheGet(() => GetSchadenStatusAlle()); }
        }

        [XmlIgnore]
        public List<SchadenfallStatusAlle> SchadenStatusAlleFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => SchadenStatusAlleFiltered;
                return PropertyCacheGet(() => SchadenStatusAlle);
            }
            private set { PropertyCacheSet(value); }
        }

        public void AlleSchadenStatusFilter(string filterValue, string filterProperties)
        {
            SchadenStatusAlleFiltered = SchadenStatusAlle.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        List<SchadenfallStatusAlle> GetSchadenStatusAlle()
        {
            var statusAlleProperties = typeof(SchadenfallStatusAlle).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return Schadenfaelle
                    .Where(schadenfall => ( LogonContext.Organization == null || 
                                            LogonContext.Organization.AllOrganizations || 
                                            LogonContext.Organization.OrganizationReference.IsNullOrEmpty()) ||
                                            LogonContext.Organization.OrganizationReference.ToSapKunnr() == schadenfall.VersicherungID.ToSapKunnr() )
                        .Select(schadenfall =>
                        {
                            var statusAlle = new SchadenfallStatusAlle
                                {
                                    VersSchadenfallID = schadenfall.ID,
                                    EventName = schadenfall.EventName,
                                    Kennzeichen = schadenfall.Kennzeichen,
                                    VersicherungName = schadenfall.VersicherungName,
                                    Referenznummer = schadenfall.Referenznummer,
                                };

                            var thisStatusWerteWithNulls = GetSchadenfallStatusWerteWithNulls(schadenfall.ID)
                                .Where(statusWerte => statusWerte.VersSchadenfallID == schadenfall.ID)
                                    .OrderBy(art => art.Sort).ThenBy(art => art.StatusArtID).ToArray();

                            for (var i = 0; i < thisStatusWerteWithNulls.Length; i++)
                            {
                                var contentProperty = statusAlleProperties.FirstOrDefault(p => p.Name == string.Format("Status{0}", (i+1)));
                                if (contentProperty == null)
                                    continue;
                                contentProperty.SetValue(statusAlle, thisStatusWerteWithNulls[i].Datum, null);
                            }

                            return statusAlle;
                        }).ToList();
        }

        #endregion


        #region Schadenfall Dokument Arten

        [XmlIgnore]
        public List<CustomerDocumentCategory> SchadenfallDokumentArten
        {
            get { return PropertyCacheGet(() => DataService.Categories); }
        }

        #endregion


        #region Schadenfall Dokumente

        [XmlIgnore]
        public List<CustomerDocument> AlleSchadenfallDokumente
        {
            get
            {
                return PropertyCacheGet(() => DataService.AllDocuments);
            }
        }

        public List<CustomerDocument> GetSchadenfallDokumenteWithNulls(int schadenfallID)
        {
            return SchadenfallDokumentArten
                                .Select(art =>
                                {
                                    var item = AlleSchadenfallDokumente
                                                    .FirstOrDefault(
                                                        status =>
                                                        status.ReferenceField == schadenfallID.ToString() &&
                                                        status.CategoryID == art.ID)
                                                ?? new CustomerDocument
                                                {
                                                    ReferenceField = schadenfallID.ToString(),
                                                    CategoryID = art.ID,
                                                    Category = art.CategoryName
                                                };
                                    return item;
                                }).ToList();
        }

        public string SchadenfallDokumenteAlleGetHeaderText(int index)
        {
            var zeroBasedIndex = index - 1;
            var dokumentArtArray = SchadenfallDokumentArten.OrderBy(art => art.CategoryName).ThenBy(art => art.ID).ToArray();
            return (zeroBasedIndex >= dokumentArtArray.Length ? "" : dokumentArtArray[zeroBasedIndex].CategoryName);
        }

        public bool SchadenfallDokumenteAlleIsDokumentVisible(int index)
        {
            return SchadenfallDokumenteAlleGetHeaderText(index).IsNotNullOrEmpty();
        }

        [XmlIgnore]
        public List<SchadenfallDokumenteAlle> SchadenDokumenteAlle
        {
            get { return PropertyCacheGet(() => GetSchadenDokumenteAlle()); }
        }

        [XmlIgnore]
        public List<SchadenfallDokumenteAlle> SchadenDokumenteAlleFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => SchadenDokumenteAlleFiltered;
                return PropertyCacheGet(() => SchadenDokumenteAlle);
            }
            private set { PropertyCacheSet(value); }
        }

        public void AlleSchadenDokumenteFilter(string filterValue, string filterProperties)
        {
            SchadenDokumenteAlleFiltered = SchadenDokumenteAlle.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        List<SchadenfallDokumenteAlle> GetSchadenDokumenteAlle()
        {
            var dokumenteAlleProperties = typeof(SchadenfallDokumenteAlle).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return Schadenfaelle
                    .Where(schadenfall => (LogonContext.Organization == null ||
                                            LogonContext.Organization.AllOrganizations ||
                                            LogonContext.Organization.OrganizationReference.IsNullOrEmpty()) ||
                                            LogonContext.Organization.OrganizationReference.ToSapKunnr() == schadenfall.VersicherungID.ToSapKunnr())
                        .Select(schadenfall =>
                        {
                            var dokumenteAlle = new SchadenfallDokumenteAlle
                            {
                                VersSchadenfallID = schadenfall.ID,
                                EventName = schadenfall.EventName,
                                Kennzeichen = schadenfall.Kennzeichen,
                                VersicherungName = schadenfall.VersicherungName,
                                Referenznummer = schadenfall.Referenznummer,
                            };

                            var thisDokumenteWithNulls = GetSchadenfallDokumenteWithNulls(schadenfall.ID)
                                .Where(dok => dok.ReferenceField == schadenfall.ID.ToString())
                                    .OrderBy(dok => dok.Category).ThenBy(dok => dok.CategoryID).ToArray();

                            for (var i = 0; i < thisDokumenteWithNulls.Length; i++)
                            {
                                var contentProperty = dokumenteAlleProperties.FirstOrDefault(p => p.Name == string.Format("Dokument{0}", (i + 1)));
                                if (contentProperty == null)
                                    continue;
                                contentProperty.SetValue(dokumenteAlle, (String.IsNullOrEmpty(thisDokumenteWithNulls[i].ApplicationKey) ? "" : "X"), null);
                            }

                            return dokumenteAlle;
                        }).ToList();
        }

        #endregion


        #region Schadenfaelle

        [XmlIgnore]
        public List<Schadenfall> Schadenfaelle
        {
            get { return PropertyCacheGet(() => SchadenDataService.SchadenfaelleGet()); }
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
            newItem = SchadenDataService.SchadenfallAdd(newItem, addModelError);
            Schadenfaelle.Add(newItem);
            DataMarkForRefreshSchadenfaelle();
            return newItem;
        }

        public void SchadenfallDelete(int id)
        {
            Schadenfaelle.Remove(SchadenfallGet(id));
            SchadenDataService.SchadenfallDelete(id);

            DataMarkForRefreshSchadenfaelle();
        }

        public Schadenfall SchadenfallSave(Schadenfall item, Action<string, string> addModelError)
        {
            if (InsertMode)
                return SchadenfallAdd(item, addModelError);

            var savedItem = SchadenDataService.SchadenfallSave(item, addModelError);
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
            DataMarkForRefreshSchadenfallStatusWerte();

            DataMarkForRefreshTermine();
        }

        #endregion


        #region Alle Termine

        public bool AlleTermineFetchAllDetails { get; set; }

        [XmlIgnore]
        public List<TerminSchadenfall> AlleTermine
        {
            get { return PropertyCacheGet(() =>
                {
                    var list = EventsDataService.TermineGet();

                    if (AlleTermineFetchAllDetails)
                        list.ForEach(termin =>
                            {
                                termin.EventAsTextTmp = (termin.VersSchadenfallID == 0 ? "" : termin.EventAsText);
                                termin.OrtAsTextTmp = termin.OrtAsText;
                                termin.BoxAsTextTmp = termin.BoxAsText;
                            });

                    return list;
                }); }
        }

        #endregion 


        #region AlleSchadenfaelleTermine

        private List<TerminSchadenfall> AlleSchadenfaelleTermine { get { return AlleTermine.Where(t => !t.IsBlockerDummyTermin).ToList(); } }

        [XmlIgnore]
        public List<TerminSchadenfall> AlleSchadenfaelleTermineFiltered
        {
            get
            {
                FilteredObjectsCurrent = () => AlleSchadenfaelleTermineFiltered;
                return PropertyCacheGet(() => AlleSchadenfaelleTermine.Where(t => !t.IsBlockerDummyTermin).ToList());
            }
            private set { PropertyCacheSet(value); }
        }

        public void AlleSchadenfaelleTermineFilter(string filterValue, string filterProperties)
        {
            AlleSchadenfaelleTermineFiltered = AlleSchadenfaelleTermine.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        #endregion


        #region Blocker Termine

        public VersEventOrtBox BoxCurrent { get; private set; }

        [XmlIgnore]
        public List<TerminSchadenfall> BoxBlockerTermine
        {
            get { return PropertyCacheGet(() => EventsDataService.TermineGet(null, BoxCurrent == null ? 0 : BoxCurrent.ID).Where(t => t.IsBlockerDummyTermin).ToList()); }
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
            EventsDataService.TerminDelete(id);

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
            return EventsDataService.TermineGet(null, versBoxID);
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
            return TerminCreate(0, SchadenfallCurrent == null ? 0 : SchadenfallCurrent.ID);
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

            newTermin = EventsDataService.TerminAdd(newTermin, addModelError);
            termine.Add(newTermin);
            TerminCurrent = newTermin;

            if (TerminCurrent.GetCachedBoxArt() == "GU")
                // Status "GA Termin vergeben" in Schadenakte speichern
                SchadenfallStatusWertSave(GutachtenTerminStatusID, DateTime.Now);
            
            DataMarkForRefreshTermine();

            return newTermin;
        }

        TerminSchadenfall TerminSave(TerminSchadenfall termin, Action<string, string> addModelError)
        {
            if (!termin.Validate(addModelError))
                return termin;

            var savedTermin = EventsDataService.TerminSave(termin, addModelError);

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
            EventsDataService.TerminDelete(id);

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

        public TerminSchadenfall TerminCurrentSave(ModelStateDictionary modelState, List<TerminSchadenfall> termine)
        {
            var termin = TerminCurrent;

            if (InsertMode)
            {
                // Serien Termine nur bei InsertMode zulassen:
                if (termin.DatumTmpBlockerSerieBis != null)
                    return TerminSerieAdd(termin, modelState, termine);

                return TerminAdd(termin, modelState.AddModelError, termine);
            }

            return TerminSave(termin, modelState.AddModelError);
        }

        private TerminSchadenfall TerminSerieAdd(TerminSchadenfall termin, ModelStateDictionary modelState, List<TerminSchadenfall> termine)
        {
            var serienTermine = new List<TerminSchadenfall>();
            var serienDatum = termin.Datum;
            while (serienDatum <= termin.DatumTmpBlockerSerieBis)
            {
                var serienTermin = ModelMapping.Copy(termin, (src, dst) =>
                {
                    dst.Datum = serienDatum;
                    dst.DatumTmpBlockerSerieBis = null;
                });

                serienTermin.Validate(modelState.AddModelError);
                serienTermine.Add(serienTermin);

                serienDatum = serienDatum.AddDays(1);
                if (serienDatum.DayOfWeek == DayOfWeek.Saturday)
                    serienDatum = serienDatum.AddDays(2);
                if (serienDatum.DayOfWeek == DayOfWeek.Sunday)
                    serienDatum = serienDatum.AddDays(1);
            }

            if (modelState.IsValid)
                // Ok, keine Validierungsfehler für jeden einzelnen Serientermin vorhanden
                // ==> erst jetzt alle Serientermine dem Datenspeicher zum "Insert" übergeben:
                serienTermine.ForEach(t => TerminAdd(t, modelState.AddModelError, termine));

            return termin;
        }

        #endregion


        #region Termin Übersicht

        public void TerminCurrentPrepareForTerminUebersicht(int eventID, int ortID)
        {
            var termin = new TerminSchadenfall
                {
                    BoxArtGewuenscht = "RE",
                    VersOrtID = ortID
                };

            TerminCurrent = termin;
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

        [LocalizedDisplay(LocalizeConstants.EventName)]
        public int TmpVersEventCurrentID { get; set; }


        public void VersEventsLoad()
        {
            VersEvents = EventsDataService.VersEventsGet();
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
            newVersEvent = EventsDataService.VersEventAdd(newVersEvent, addModelError);
            VersEvents.Add(newVersEvent);
            
            VersEventCurrent = newVersEvent;
            VersEventOrteLoad();

            return newVersEvent;
        }

        public void VersEventDelete(int id)
        {
            VersEvents.Remove(VersEventGet(id));
            EventsDataService.VersEventDelete(id);

            //DataMarkForRefresh();
        }

        public VersEvent VersEventSave(VersEvent item, Action<string, string> addModelError)
        {
            var savedVersEvent = EventsDataService.VersEventSave(item, addModelError);
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
            get { return PropertyCacheGet(() => EventsDataService.VersEventOrteGet()); }
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
        
        [LocalizedDisplay(LocalizeConstants.VersEventLocation)]
        public int TmpVersEventOrtCurrentID { get; set; }

        public void VersEventOrteLoad()
        {
            VersEventOrt.Laender = EventsDataService.Laender;

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
            newVersEventOrt = EventsDataService.VersEventOrtAdd(newVersEventOrt, addModelError);
            VersEventOrte.Add(newVersEventOrt);
            
            VersEventOrtCurrent = newVersEventOrt;
            VersEventOrtBoxenLoad();
            
            return newVersEventOrt;
        }

        public void VersEventOrtDelete(int id)
        {
            VersEventOrte.Remove(VersEventOrtGet(id));
            EventsDataService.VersEventOrtDelete(id);

            //DataMarkForRefresh();
        }

        public VersEventOrt VersEventOrtSave(VersEventOrt item, Action<string, string> addModelError)
        {
            var savedVersEventOrt = EventsDataService.VersEventOrtSave(item, addModelError);
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
            VersEventOrtBox.Versicherungen = EventsDataService.Versicherungen;

            VersEventOrtBoxen = EventsDataService.VersEventOrtBoxenGet(VersEventOrtCurrent);
            DataMarkForRefreshOrtBoxen();
        }

        public void VersEventOrtBoxenFilter(string filterValue, string filterProperties)
        {
            VersEventOrtBoxenFiltered = VersEventOrtBoxen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public VersEventOrtBox VersEventOrtBoxGet(int id)
        {
            var item = VersEventOrtBoxen.FirstOrDefault(c => c.ID == id);
            BoxCurrent = item;
            DataMarkForRefreshBlockerTermine();

            return item;
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
            newVersEventOrtBox = EventsDataService.VersEventOrtBoxAdd(newVersEventOrtBox, addModelError);
            VersEventOrtBoxen.Add(newVersEventOrtBox);
            
            BoxCurrent = newVersEventOrtBox;
            DataMarkForRefreshBlockerTermine();

            return newVersEventOrtBox;
        }

        public void VersEventOrtBoxDelete(int id)
        {
            VersEventOrtBoxen.Remove(VersEventOrtBoxGet(id));
            EventsDataService.VersEventOrtBoxDelete(id);

            //DataMarkForRefresh();
        }

        public VersEventOrtBox VersEventOrtBoxSave(VersEventOrtBox item, Action<string, string> addModelError)
        {
            var savedVersEventOrtBox = EventsDataService.VersEventOrtBoxSave(item, addModelError);
            var cachedItem = VersEventOrtBoxen.FirstOrDefault(i => i.ID == item.ID);
            if (cachedItem != null)
                ModelMapping.Copy(savedVersEventOrtBox, cachedItem);

            BoxCurrent = savedVersEventOrtBox;
            DataMarkForRefreshBlockerTermine();

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

                var insuranceName = EventsDataService.Versicherungen.First(v => v.Key == model.VersicherungID).Text;
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
