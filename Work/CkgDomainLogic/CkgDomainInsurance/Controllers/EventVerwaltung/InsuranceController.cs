using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Web.Mvc;
using CkgDomainLogic.Insurance.Models;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Insurance.ViewModels;
using MvcTools.Web;
using Telerik.Web.Mvc;
using GeneralTools.Models;

namespace ServicesMvc.Controllers
{
    public partial class InsuranceController 
    {
        public VersEventsViewModel EventsViewModel { get { return GetViewModel<VersEventsViewModel>(); } }

        public string UserCulture { get { return SessionHelper.GetSessionValue("UserCulture", "de-DE"); } }


        [CkgApplication]
        public ActionResult Konfigurator()
        {
            EventsViewModel.DataInit(UserCulture);

            return View(EventsViewModel);
        }

        public ActionResult KonfiguratorTest()
        {
            EventsViewModel.DataInit(UserCulture);
            EventsViewModel.DataInitTest();

            return View(EventsViewModel);
        }

        [CkgApplication]
        public ActionResult Termine()
        {
            EventsViewModel.DataInit(UserCulture);

            return View(EventsViewModel);
        }

        [CkgApplication]
        public ActionResult SchadenStatusAlle()
        {
            EventsViewModel.DataInit(UserCulture);

            return View(EventsViewModel);
        }

        [CkgApplication]
        public ActionResult SchadenDokumenteAlle()
        {
            EventsViewModel.DataInit(UserCulture);

            return View(EventsViewModel);
        }

        [CkgApplication]
        public ActionResult Schadenverwaltung()
        {
            EventsViewModel.DataInit(UserCulture);

            return View(EventsViewModel);
        }

        [CkgApplication]
        public ActionResult TerminUebersichtBoxen()
        {
            EventsViewModel.DataInit(UserCulture);

            return View(EventsViewModel);
        }
        
        [CkgApplication]
        public ActionResult Calendar()
        {
            EventsViewModel.DataInit(UserCulture);

            return View(EventsViewModel);
        }

        void InitModelStatics()
        {
            VersEvent.GetViewModel = GetViewModel<VersEventsViewModel>;
            VersEventOrt.GetViewModel = GetViewModel<VersEventsViewModel>;
            VersEventOrtBox.GetViewModel = GetViewModel<VersEventsViewModel>;
            TerminSchadenfall.GetViewModel = GetViewModel<VersEventsViewModel>;
            Schadenfall.GetViewModel = GetViewModel<VersEventsViewModel>;
        }

        
        #region Schadenfall Status (Werte)

        [HttpPost]
        public ActionResult SchadenfallStatusUpdate()
        {
            return PartialView("Schadenakte/Partial/Status/StatusListe", EventsViewModel.SchadenfallCurrentStatusWerteWithNulls);
        }

        [GridAction]
        public ActionResult SchadenfallStatusAjaxSelect()
        {
            return View(new GridModel(EventsViewModel.SchadenfallCurrentStatusWerteWithNulls));
        }

        [HttpPost]
        [GridAction]
        public ActionResult SchadenfallStatusAjaxUpdate(int id)
        {
            var itemToUpdate = EventsViewModel.SchadenfallCurrentStatusWerteWithNulls.FirstOrDefault(p => p.StatusArtID == id);
            var itemCloned = ModelMapping.Copy(itemToUpdate);
            if (TryUpdateModel(itemCloned))
            {
                ModelMapping.Copy(itemCloned, itemToUpdate); 
                EventsViewModel.SchadenfallStatusWertSave(itemToUpdate, ModelState.AddModelError);
            }

            return View(new GridModel(EventsViewModel.SchadenfallCurrentStatusWerteWithNulls));
        }
        
        [GridAction]
        public ActionResult SchadenfallStatusAlleAjaxBinding()
        {
            return View(new GridModel(EventsViewModel.SchadenStatusAlleFiltered));
        }

        [HttpPost]
        public ActionResult FilterSchadenStatusAlleGrid(string filterValue, string filterColumns)
        {
            EventsViewModel.AlleSchadenStatusFilter(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion

        #region Schadenfall Dokumente

        [GridAction]
        public ActionResult SchadenfallDokumenteAlleAjaxBinding()
        {
            return View(new GridModel(EventsViewModel.SchadenDokumenteAlleFiltered));
        }

        [HttpPost]
        public ActionResult FilterSchadenDokumenteAlleGrid(string filterValue, string filterColumns)
        {
            EventsViewModel.AlleSchadenDokumenteFilter(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion


        #region Schadenfaelle

        [GridAction]
        public ActionResult SchadenfaelleAjaxBinding()
        {
            return View(new GridModel(EventsViewModel.SchadenfaelleFiltered));
        }

        [HttpPost]
        public ActionResult FilterSchadenfallGrid(string filterValue, string filterColumns)
        {
            EventsViewModel.SchadenfaelleFilter(filterValue, filterColumns);

            return new EmptyResult();
        }

        private void SchadenfaelleSetViewBagData()
        {
            ViewBag.AuswahlEvents = EventsViewModel.VersEvents.ToSelectList();
            ViewBag.AuswahlVersicherungen = EventsViewModel.EventsDataService.Versicherungen.ToSelectList();
        }

        [HttpPost]
        public ActionResult SchadenfallCreate()
        {
            EventsViewModel.InsertMode = true;
            ModelState.Clear();
            
            SchadenfaelleSetViewBagData();
            return PartialView("Schadenfaelle/SchadenfallDetailsForm", EventsViewModel.SchadenfallCreate().SetInsertMode(EventsViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult SchadenfallEdit(int id)
        {
            EventsViewModel.InsertMode = false;
            ModelState.Clear();
            
            SchadenfaelleSetViewBagData();
            return PartialView("Schadenfaelle/SchadenfallDetailsForm", EventsViewModel.SchadenfallGet(id).SetInsertMode(EventsViewModel.InsertMode));
        }

        [HttpPost]
        public ActionResult SchadenfallDelete(int id)
        {
            EventsViewModel.SchadenfallDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SchadenfallGridSelect()
        {
            EventsViewModel.DataMarkForRefreshSchadenfaelle();

            return PartialView("Schadenfaelle/SchadenfallGrid");
        }

        [HttpPost]
        public ActionResult SchadenfallDetailsFormSave(Schadenfall model)
        {
            // Avoid ModelState clearing on saving 
            // => because automatic model validation (via data annotations) would be omitted !!!
            // ModelState.Clear();
            ModelState.SetModelValue("ID", null);

            EventsViewModel.SchadenfallValidate(model, ModelState.AddModelError);

            if (ModelState.IsValid)
                model = EventsViewModel.SchadenfallSave(model, ModelState.AddModelError);

            model.InsertModeTmp = EventsViewModel.InsertMode;

            SchadenfaelleSetViewBagData();
            return PartialView("Schadenfaelle/SchadenfallDetailsForm", model);
        }

        #endregion


        #region Alle Termine

        [GridAction]
        public ActionResult AlleTermineAjaxBinding()
        {
            return View(new GridModel(EventsViewModel.AlleSchadenfaelleTermineFiltered));
        }

        [HttpPost]
        public ActionResult FilterAlleTermineGrid(string filterValue, string filterColumns)
        {
            EventsViewModel.AlleSchadenfaelleTermineFilter(filterValue, filterColumns);

            return new EmptyResult();
        }

        #endregion


        #region Blocker Termine

        [GridAction]
        public ActionResult BoxBlockerTermineAjaxBinding()
        {
            return View(new GridModel(EventsViewModel.BoxBlockerTermineFiltered));
        }

        [HttpPost]
        public ActionResult FilterBoxBlockerTermineGrid(string filterValue, string filterColumns)
        {
            EventsViewModel.BoxBlockerTermineFilter(filterValue, filterColumns);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult BoxBlockerTerminCreate()
        {
            EventsViewModel.InsertMode = true;
            EventsViewModel.SchadenfallCurrentDisable();

            ModelState.Clear();
            return PartialView("Konfigurator/BoxBlockerTerminDetailsForm", EventsViewModel.BoxBlockerTerminCreate());
        }


        [HttpPost]
        public ActionResult BoxBlockerTerminEdit(int id)
        {
            EventsViewModel.InsertMode = false;
            EventsViewModel.SchadenfallCurrentDisable();

            ModelState.Clear();
            return PartialView("Konfigurator/BoxBlockerTerminDetailsForm", EventsViewModel.BoxBlockerTerminGet(id));
        }

        [HttpPost]
        public ActionResult BoxBlockerTerminDelete(int id)
        {
            EventsViewModel.BoxBlockerTerminDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult BoxBlockerTerminDetailsFormSave(TerminSchadenfall termin)
        {
            return TerminGeneralSave(termin, EventsViewModel.BoxBlockerTermine, "Konfigurator/BoxBlockerTerminDetailsForm");
        }

        #endregion 


        #region Termine

        [HttpPost]
        public ActionResult TerminSchadenfallCreate()
        {
            EventsViewModel.InsertMode = true;
            ModelState.Clear();

            SchadenfaelleSetViewBagData();
            return PartialView("Schadenakte/Partial/Termine/TerminAnlage", EventsViewModel.TerminCreate());
        }

        public ActionResult TerminSchadenfallCreateFinish(int? ortID)
        {
            if (ortID == null)
                return new EmptyResult();

            EventsViewModel.TerminCurrent.VersOrtID = ortID.GetValueOrDefault();

            return PartialView("Schadenakte/Partial/Termine/TerminKalender", EventsViewModel.TerminCurrent);
        }

        public ActionResult ReTerminVorschlaegeSearch(DateTime? datum, string uhrzeit, int dauer)
        {
            var errorMessage = EventsViewModel.ReTerminVorschlaegeSearch(datum, uhrzeit, dauer);
            if (errorMessage.IsNotNullOrEmpty())
                return Json(new { errorMessage });

            return PartialView("Schadenakte/Partial/Termine/ReTerminVorschlaegeResults", EventsViewModel);
        }

        [HttpPost]
        public ActionResult TerminSchadenfallMove(int id)
        {
            EventsViewModel.InsertMode = false;
            ModelState.Clear();

            SchadenfaelleSetViewBagData();
            return PartialView("Schadenakte/Partial/Termine/TerminKalender", EventsViewModel.TerminGet(id));
        }

        [HttpPost]
        public ActionResult TerminSchadenfallDelete(int id)
        {
            EventsViewModel.TerminDelete(id);

            return new EmptyResult();
        }
        
        [GridAction]
        public ActionResult TerminSchadenfallAjaxBinding()
        {
            return View(new GridModel(EventsViewModel.TermineFiltered));
        }

        [HttpPost]
        public ActionResult FilterTerminSchadenfallGrid(string filterValue, string filterColumns)
        {
            EventsViewModel.TermineFilter(filterValue, filterColumns);

            return new EmptyResult();
        }

        public ActionResult TerminSchadenfallCreateOnBoxArtChange(string boxArt)
        {
            EventsViewModel.TerminCurrent.BoxArtGewuenscht = boxArt;

            return Json(EventsViewModel.TerminCurrent.GetValidOrte().Select(ort => new { val = ort.ID, text = ort.OrtName }));
        }

        [HttpPost]
        public ActionResult TermineSchadenfaelleKalenderAjaxSelect(string boxID, string startDate, string endDate)
        {
            var dateStart = DateTime.ParseExact(startDate, "dd.MM.yyyy", new CultureInfo("de-DE"));
            var dateEnd = DateTime.ParseExact(endDate, "dd.MM.yyyy", new CultureInfo("de-DE"));

            var termin = EventsViewModel.TerminCurrent;
            var boxArt = termin.GetCachedBoxArt();

            var boxen = termin.GetValidBoxen();
            var firstBox = boxen.FirstOrDefault();
            if (firstBox == null)
                return new EmptyResult();

            var termineForValidBoxen = termin.GetTermineForValidBoxen(t => t.Datum >= dateStart && t.Datum <= dateEnd);

            if (boxArt == "RE")
                return Json(termineForValidBoxen.Select(t => new TerminEntity
                    {
                        key = t.VersBoxID.ToString(),
                        boxArt = boxArt,
                        title = (t.IsBlockerDummyTermin ? "Blocker" : string.Format("{0} {1}", t.SchadenfallKennzeichen, t.Schadenfall.Nachname)),
                        isBlocker = t.IsBlockerDummyTermin,
                        isCurrentEditing = (!EventsViewModel.InsertMode && t.ID == termin.ID),

                        startDateString = t.Datum.ToJsonDateTimeString(),
                        start = t.Datum.ToJsonDateString(),
                        startTimeHours = t.ZeitVon.Split(':')[0].ToInt(),
                        startTimeMinutes = t.ZeitVon.Split(':')[1].ToInt(),
                        
                        end = t.Datum.ToJsonDateString(),
                        endTimeHours = t.ZeitBis.Split(':')[0].ToInt(),
                        endTimeMinutes = t.ZeitBis.Split(':')[1].ToInt(),
                    }).OrderBy(t => t.key).ToArray());

            
            // Für BoxArt "GU" nur 1 Kalender anzeigen und zwar über alle Boxen gruppiert / kumuliert:
            // Es werden für alle Zeitscheiben pro Tag Termine vorbelegt, 
            // - für die "freien" Zeitscheiben werden "grüne" Dummy Termine angelegt.
            // - für komplett belegte Zeitscheiben werden "rote" Dummy Termine angelegt.

            var ort = termin.Ort;
            var groupedTerminList = new List<TerminEntity>();
            for (var dt = dateStart; dt <= dateEnd; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                var zeitscheibeTaktungMinuten = firstBox.TaktungMinuten;

                DateTime thisDayTimeStart, thisDayTimeEnd;
                ort.GetDayTimeRangeForDate(dt, out thisDayTimeStart, out thisDayTimeEnd);

                for (var timeThisDay = thisDayTimeStart; timeThisDay < thisDayTimeEnd; timeThisDay = timeThisDay.AddMinutes(zeitscheibeTaktungMinuten))
                {
                    var timeThisDayEnd = timeThisDay.AddMinutes(zeitscheibeTaktungMinuten);

                    var termineInThisTimeGrid = termineForValidBoxen.Where(t => t.DatumZeitVon == timeThisDay);
                    var normalTermine = termineInThisTimeGrid.Where(t => !t.IsBlockerDummyTermin).ToList();
                    var blockerTermine = termineInThisTimeGrid.Where(t => t.IsBlockerDummyTermin).ToList();

                    var title = normalTermine.None() ? "" : string.Format("{0} Termin{1}", normalTermine.Count, normalTermine.Count == 1 ? "" : "e");
                    if (blockerTermine.Any())
                        title += (title.IsNotNullOrEmpty() ? ", " : "") + string.Format("{0} Blocker", blockerTermine.Count);

                    groupedTerminList.Add(new TerminEntity
                        {
                            key = timeThisDay.ToShortDateString(),
                            boxArt = boxArt,
                            title = title,
                            isCurrentEditing = (!EventsViewModel.InsertMode && normalTermine.Any(t => t.ID == termin.ID)),

                            startDateString = timeThisDay.ToJsonDateString(),
                            start = timeThisDay.ToJsonDateString(),
                            startTimeHours = timeThisDay.Hour,
                            startTimeMinutes = timeThisDay.Minute,

                            end = timeThisDayEnd.ToJsonDateString(),
                            endTimeHours = timeThisDayEnd.Hour,
                            endTimeMinutes = timeThisDayEnd.Minute,

                            boxenTotal = boxen.Count,
                            boxenOccupied = termineInThisTimeGrid.Count(),
                        });
                }
            }

            return Json(groupedTerminList.ToArray());
        }

        [HttpPost]
        public ActionResult TerminSchadenfallKalenderEditElement(string boxArt, string key, string startDateString, int startTimeHours, int startTimeMinutes, int endTimeHours, int endTimeMinutes)
        {
            EventsViewModel.TerminCurrentPrepare(boxArt, key, startDateString, startTimeHours, startTimeMinutes, endTimeHours, endTimeMinutes);

            return PartialView("Schadenakte/Partial/Termine/TerminEditDetailsForm", EventsViewModel.TerminCurrent);
        }
        
        [HttpPost]
        public ActionResult TerminSchadenfallKalenderEditElementFromSchadenfallId(int id)
        {
            EventsViewModel.TerminCurrentPrepareFromSchadenfallId(id);

            return PartialView("Schadenakte/Partial/Termine/TerminEditDetailsForm", EventsViewModel.TerminCurrent);
        }

        public ActionResult ReTerminVorschlagSelect(int terminID)
        {
            EventsViewModel.ReTerminVorschlaegeSelect(terminID);

            return PartialView("Schadenakte/Partial/Termine/TerminEditDetailsForm", EventsViewModel.TerminCurrent);
        }

        [HttpPost]
        public ActionResult TerminSchadenfallEditFormSave(TerminSchadenfall termin)
        {
            return TerminGeneralSave(termin, EventsViewModel.Termine, "Schadenakte/Partial/Termine/TerminEditDetailsForm");
        }

        ActionResult TerminGeneralSave(TerminSchadenfall termin, List<TerminSchadenfall> termine, string partialViewName)
        {
            //ModelState.Clear();
            if (ModelState.IsValid)
            {
                var terminOldData = new TerminSchadenfall();
                TerminCopyProperties(EventsViewModel.TerminCurrent, terminOldData);
                TerminCopyProperties(termin, EventsViewModel.TerminCurrent);

                EventsViewModel.TerminCurrentSave(ModelState, termine);

                if (!ModelState.IsValid)
                    TerminCopyProperties(terminOldData, EventsViewModel.TerminCurrent);
                else
                    EventsViewModel.InsertMode = false;
            }

            return PartialView(partialViewName, EventsViewModel.TerminCurrent);
        }

        static void TerminCopyProperties(TerminSchadenfall src, TerminSchadenfall dst)
        {
            dst.VersOrtID = src.VersOrtID;
            dst.VersBoxID = src.VersBoxID;
            dst.Datum = src.Datum;
            dst.DatumTmpBlockerSerieBis = src.DatumTmpBlockerSerieBis;
            dst.ZeitVon = src.ZeitVon;
            dst.ZeitBis = src.ZeitBis;
            dst.Bemerkung = src.Bemerkung;
        }

        #endregion


        #region Termin Übersicht

        public ActionResult TerminUebersichtGetOrte(int eventID)
        {
            if (eventID == 0)
                return new EmptyResult();

            EventsViewModel.VersEventGet(eventID);

            return Json(EventsViewModel.VersEventOrte.Select(ort => new { val = ort.ID, text = ort.OrtName }));
        }

        [HttpPost]
        public ActionResult TerminUebersichtRequestShow(int eventID, int ortID)
        {
            EventsViewModel.TerminCurrentPrepareForTerminUebersicht(eventID, ortID);

            ViewData["HideReTerminVorschlaege"] = true;
            return PartialView("Schadenakte/Partial/Termine/TerminKalender", EventsViewModel.TerminCurrent);
        }

        #endregion


        #region VersEvents

        [GridAction]
        public ActionResult VersEventAjaxBinding()
        {
            return View(new GridModel(EventsViewModel.VersEventsFiltered));
        }

        [HttpPost]
        public ActionResult FilterVersEventGrid(string filterValue, string filterColumns)
        {
            EventsViewModel.VersEventsFilter(filterValue, filterColumns);

            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult VersEventCreate()
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventDetailsForm", EventsViewModel.VersEventAdd(EventsViewModel.VersEventCreate(), ModelState.AddModelError));
        }


        [HttpPost]
        public ActionResult VersEventEdit(int id)
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventDetailsForm", EventsViewModel.VersEventGet(id));
        }

        [HttpPost]
        public ActionResult VersEventDelete(int id)
        {
            EventsViewModel.VersEventDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult VersEventDetailsFormSave(VersEvent model)
        {
            EventsViewModel.VersEventValidate(model, ModelState.AddModelError);

            if (ModelState.IsValid)
                model = EventsViewModel.VersEventSave(model, ModelState.AddModelError);

            return PartialView("Konfigurator/VersEventDetailsForm", model);
        }

        #endregion


        #region VersEventOrte

        [GridAction]
        public ActionResult VersEventOrtAjaxBinding()
        {
            return View(new GridModel(EventsViewModel.VersEventOrteFiltered));
        }

        [HttpPost]
        public ActionResult FilterVersEventOrtGrid(string filterValue, string filterColumns)
        {
            EventsViewModel.VersEventOrteFilter(filterValue, filterColumns);

            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult VersEventOrtCreate()
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventOrtDetailsForm", EventsViewModel.VersEventOrtAdd(EventsViewModel.VersEventOrtCreate(), ModelState.AddModelError));
        }


        [HttpPost]
        public ActionResult VersEventOrtEdit(int id)
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventOrtDetailsForm", EventsViewModel.VersEventOrtGet(id));
        }

        [HttpPost]
        public ActionResult VersEventOrtDelete(int id)
        {
            EventsViewModel.VersEventOrtDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult VersEventOrtDetailsFormSave(VersEventOrt model)
        {
            EventsViewModel.VersEventOrtValidate(model, ModelState.AddModelError);

            if (ModelState.IsValid)
                model = EventsViewModel.VersEventOrtSave(model, ModelState.AddModelError);

            return PartialView("Konfigurator/VersEventOrtDetailsForm", model);
        }

        #endregion


        #region VersEventOrtBoxen

        [GridAction]
        public ActionResult VersEventOrtBoxAjaxBinding()
        {
            return View(new GridModel(EventsViewModel.VersEventOrtBoxenFiltered));
        }

        [HttpPost]
        public ActionResult FilterVersEventOrtBoxGrid(string filterValue, string filterColumns)
        {
            EventsViewModel.VersEventOrtBoxenFilter(filterValue, filterColumns);

            return new EmptyResult();
        }


        [HttpPost]
        public ActionResult VersEventOrtBoxCreate()
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventOrtBoxDetailsForm", EventsViewModel.VersEventOrtBoxAdd(EventsViewModel.VersEventOrtBoxCreate(), ModelState.AddModelError));
        }


        [HttpPost]
        public ActionResult VersEventOrtBoxEdit(int id)
        {
            ModelState.Clear();
            return PartialView("Konfigurator/VersEventOrtBoxDetailsForm", EventsViewModel.VersEventOrtBoxGet(id));
        }

        [HttpPost]
        public ActionResult VersEventOrtBoxDelete(int id)
        {
            EventsViewModel.VersEventOrtBoxDelete(id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult VersEventOrtBoxDetailsFormSave(VersEventOrtBox model)
        {
            EventsViewModel.VersEventOrtBoxValidate(model, ModelState.AddModelError);

            if (ModelState.IsValid)
                model = EventsViewModel.VersEventOrtBoxSave(model, ModelState.AddModelError);

            return PartialView("Konfigurator/VersEventOrtBoxDetailsForm", model);
        }

        #endregion


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            return EventsViewModel.FilteredObjectsCurrent();
        }

        #endregion
    }
}
