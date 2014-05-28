// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.VersEvents.Contracts;
using CkgDomainLogic.VersEvents.Models;
using GeneralTools.Models;
using MvcTools.Web;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.VersEvents.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.VersEvents.Services
{
    public class VersEventsDataServiceSQL : CkgGeneralDataServiceTest, IVersEventsDataService
    {
        public VersEventSqlDbContext DbContext
        {
            get { return SessionStoreAutoCreate<VersEventSqlDbContext>.Model; }
        }

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


        #region Vorgänge

        public List<Vorgang> VorgaengeGet()
        {
            var kundenNr = LogonContext.KundenNr.ToInt();
            return DbContext.Vorgaenge.Where(e => e.KundenNr == kundenNr && e.LoeschDatum == null).ToList();
        }

        public Vorgang VorgangAdd(Vorgang item)
        {
            DbContext.Vorgaenge.Add(item);
            DbContext.SaveChanges();

            return item;
        }

        public bool VorgangDelete(int id)
        {
            //DbContext.Vorgaenge.Remove(DbContext.Vorgaenge.First(e => e.ID == id));
            //DbContext.SaveChanges();
            
            var savedItem = DbContext.Vorgaenge.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            DbContext.SaveChanges();

            return true;
        }

        public Vorgang VorgangSave(Vorgang item, Action<string, string> addModelError)
        {
            var savedItem = DbContext.Vorgaenge.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            DbContext.SaveChanges();

            return savedItem;
        }

        #endregion


        #region Termine

        public List<VorgangTermin> TermineGet(VorgangTerminSelector selector = null)
        {
            var kundenNr = LogonContext.KundenNr.ToInt();
            return DbContext.VorgangTermine.Where(e => e.KundenNr == kundenNr && e.LoeschDatum == null).ToList();
        }

        public VorgangTermin TerminAdd(VorgangTermin item)
        {
            DbContext.VorgangTermine.Add(item);
            DbContext.SaveChanges();

            return item;
        }

        public bool TerminDelete(int id)
        {
            //DbContext.VorgangTermine.Remove(DbContext.VorgangTermine.First(e => e.ID == id));
            //DbContext.SaveChanges();

            var savedItem = DbContext.VorgangTermine.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            DbContext.SaveChanges();

            return true;
        }

        public VorgangTermin TerminSave(VorgangTermin item, Action<string, string> addModelError)
        {
            var savedItem = DbContext.VorgangTermine.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            DbContext.SaveChanges();

            return savedItem;
        }

        #endregion


        #region VersEvents

        public List<VersEvent> VersEventsGet(VersEventSelector selector = null)
        {
            var kundenNr = LogonContext.KundenNr.ToInt();
            return DbContext.VersEvents.Where(e => e.KundenNr == kundenNr && e.LoeschDatum == null).ToList();
        }

        public VersEvent VersEventAdd(VersEvent item)
        {
            DbContext.VersEvents.Add(item);
            DbContext.SaveChanges();

            return item;
        }

        public bool VersEventDelete(int id)
        {
            //DbContext.VersEvents.Remove(DbContext.VersEvents.First(e => e.ID == id));
            //DbContext.SaveChanges();

            var savedItem = DbContext.VersEvents.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            DbContext.SaveChanges();

            return true;
        }

        public VersEvent VersEventSave(VersEvent item, Action<string, string> addModelError)
        {
            var savedItem = DbContext.VersEvents.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            DbContext.SaveChanges();

            return savedItem;
        }

        #endregion


        #region VersEventOrte

        public List<VersEventOrt> VersEventOrteGet(VersEvent versEvent = null)
        {
            var query = DbContext.VersEventOrte.AsQueryable();
            if (versEvent != null)
                query = query.Where(e => e.VersEventID == versEvent.ID);
            
            return query.Where(e => e.LoeschDatum == null).ToList();
        }

        public VersEventOrt VersEventOrtAdd(VersEventOrt item)
        {
            DbContext.VersEventOrte.Add(item);
            DbContext.SaveChanges();

            return item;
        }

        public bool VersEventOrtDelete(int id)
        {
            //DbContext.VersEventOrte.Remove(DbContext.VersEventOrte.First(e => e.ID == id));
            //DbContext.SaveChanges();

            var savedItem = DbContext.VersEventOrte.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            DbContext.SaveChanges();

            return true;
        }

        public VersEventOrt VersEventOrtSave(VersEventOrt item, Action<string, string> addModelError)
        {
            var savedItem = DbContext.VersEventOrte.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            DbContext.SaveChanges();

            return savedItem;
        }

        #endregion


        #region VersEventOrtBoxen

        public List<VersEventOrtBox> VersEventOrtBoxenGet(VersEventOrt versEventOrt)
        {
            return DbContext.VersEventOrtBoxen.Where(e => e.VersOrtID == versEventOrt.ID && e.LoeschDatum == null).ToList();
        }

        public VersEventOrtBox VersEventOrtBoxAdd(VersEventOrtBox item)
        {
            DbContext.VersEventOrtBoxen.Add(item);
            DbContext.SaveChanges();

            return item;
        }

        public bool VersEventOrtBoxDelete(int id)
        {
            //DbContext.VersEventOrtBoxen.Remove(DbContext.VersEventOrtBoxen.First(e => e.ID == id));
            //DbContext.SaveChanges();

            var savedItem = DbContext.VersEventOrtBoxen.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            DbContext.SaveChanges();

            return true;
        }

        public VersEventOrtBox VersEventOrtBoxSave(VersEventOrtBox item, Action<string, string> addModelError)
        {
            var savedItem = DbContext.VersEventOrtBoxen.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            DbContext.SaveChanges();

            return savedItem;
        }

        #endregion
    }
}
