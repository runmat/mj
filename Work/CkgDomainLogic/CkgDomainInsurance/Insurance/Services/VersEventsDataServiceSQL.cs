// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;
using MvcTools.Web;
using SapORM.Contracts;
using AppModelMappings = CkgDomainLogic.Insurance.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Insurance.Services
{
    public class VersEventsDataServiceSQL : CkgGeneralDataServiceTest, IVersEventsDataService
    {
        public List<SelectItem> Versicherungen
        {
            get
            {
                return new List<SelectItem>
                    {
                        new SelectItem("", "(Bitte wählen)"),
                        new SelectItem("ARAG", "ARAG Versicherung"),
                        new SelectItem("DEVK", "DEVK Versicherungen"),
                        new SelectItem("ERGO", "ERGO Versicherung"),
                    };
            }
        }


        private static VersEventsSqlDbContext CreateDbContext()
        {
            return new VersEventsSqlDbContext();
        }


        #region Schadenfall Status Arten

        public List<SchadenfallStatusArt> SchadenfallStatusArtenGet(string languageKey)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Schadenfall Status (Werte)

        public List<SchadenfallStatus> SchadenfallStatusWerteGet(string languageKey, int? schadenfallID = null)
        {
            throw new NotImplementedException();
        }

        public bool SchadenfallStatusWertSave(SchadenfallStatus schadenfallStatus, Action<string, string> addModelError)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Schadenfaelle

        public List<Schadenfall> SchadenfaelleGet()
        {
            var ct = CreateDbContext();
            var kundenNr = LogonContext.KundenNr;
            return ct.Schadenfaelle.Where(e => e.KundenNr == kundenNr && e.LoeschDatum == null).ToList();
        }

        public Schadenfall SchadenfallAdd(Schadenfall item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            ct.Schadenfaelle.Add(item);
            ct.SaveChanges();

            return item;
        }

        public bool SchadenfallDelete(int id)
        {
            var ct = CreateDbContext();
            var savedItem = ct.Schadenfaelle.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            ct.SaveChanges();

            return true;
        }

        public Schadenfall SchadenfallSave(Schadenfall item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            var savedItem = ct.Schadenfaelle.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            ct.SaveChanges();

            return savedItem;
        }

        #endregion


        #region Termine

        public List<TerminSchadenfall> TermineGet(Schadenfall schadenfall = null, int boxID = -1)
        {
            var ct = CreateDbContext();
            var kundenNr = LogonContext.KundenNr;
            var faelle = ct.VersEventTermine.Where(e => e.KundenNr == kundenNr && e.LoeschDatum == null);

            if (schadenfall != null)
                faelle = faelle.Where(fall => fall.VersSchadenfallID == schadenfall.ID);

            if (boxID != -1)
                faelle = faelle.Where(fall => fall.VersBoxID == boxID);

            return faelle.ToList();
        }

        public TerminSchadenfall TerminAdd(TerminSchadenfall item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            ct.VersEventTermine.Add(item);
            ct.SaveChanges();

            return item;
        }

        public bool TerminDelete(int id)
        {
            //ct.VersEventTermine.Remove(ct.VersEventTermine.First(e => e.ID == id));
            //ct.SaveChanges();

            var ct = CreateDbContext();
            var savedItem = ct.VersEventTermine.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            ct.SaveChanges();

            return true;
        }

        public TerminSchadenfall TerminSave(TerminSchadenfall item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            var savedItem = ct.VersEventTermine.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            ct.SaveChanges();

            return savedItem;
        }

        #endregion


        #region VersEvents

        public List<VersEvent> VersEventsGet()
        {
            var ct = CreateDbContext();
            var kundenNr = LogonContext.KundenNr.ToSapKunnr();
            return ct.VersEvents.Where(e => e.KundenNr == kundenNr && e.LoeschDatum == null).ToList();
        }

        public VersEvent VersEventAdd(VersEvent item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            ct.VersEvents.Add(item);
            ct.SaveChanges();

            return item;
        }

        public bool VersEventDelete(int id)
        {
            var ct = CreateDbContext();
            var savedItem = ct.VersEvents.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            ct.SaveChanges();

            return true;
        }

        public VersEvent VersEventSave(VersEvent item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            var savedItem = ct.VersEvents.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            ct.SaveChanges();

            return savedItem;
        }

        #endregion


        #region VersEventOrte

        public List<VersEventOrt> VersEventOrteGet(VersEvent versEvent = null)
        {
            var ct = CreateDbContext();
            var query = ct.VersEventOrte.AsQueryable();
            if (versEvent != null)
                query = query.Where(e => e.VersEventID == versEvent.ID);
            
            return query.Where(e => e.LoeschDatum == null).ToList();
        }

        public VersEventOrt VersEventOrtAdd(VersEventOrt item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            ct.VersEventOrte.Add(item);
            ct.SaveChanges();

            return item;
        }

        public bool VersEventOrtDelete(int id)
        {
            var ct = CreateDbContext();
            var savedItem = ct.VersEventOrte.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            ct.SaveChanges();

            return true;
        }

        public VersEventOrt VersEventOrtSave(VersEventOrt item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            var savedItem = ct.VersEventOrte.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            ct.SaveChanges();

            return savedItem;
        }

        #endregion


        #region VersEventOrtBoxen

        public List<VersEventOrtBox> VersEventOrtBoxenGet(VersEventOrt versEventOrt)
        {
            var ct = CreateDbContext();
            return ct.VersEventOrtBoxen.Where(e => e.VersOrtID == versEventOrt.ID && e.LoeschDatum == null).ToList();
        }

        public VersEventOrtBox VersEventOrtBoxAdd(VersEventOrtBox item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            ct.VersEventOrtBoxen.Add(item);
            ct.SaveChanges();

            return item;
        }

        public bool VersEventOrtBoxDelete(int id)
        {
            var ct = CreateDbContext();
            var savedItem = ct.VersEventOrtBoxen.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            ct.SaveChanges();

            return true;
        }

        public VersEventOrtBox VersEventOrtBoxSave(VersEventOrtBox item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            var savedItem = ct.VersEventOrtBoxen.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            ct.SaveChanges();

            return savedItem;
        }

        #endregion
    }
}
