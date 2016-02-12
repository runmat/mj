using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using GeneralTools.Models;
using System.Linq;
using GeneralTools.Services;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    public class StatusverfolgungZulassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore, ScriptIgnore]
        public IZulassungDataService DataService
        {
            get { return CacheGet<IZulassungDataService>(); }
        }

        public StatusverfolgungZulassungSelektor Selektor
        {
            get { return PropertyCacheGet(() => new StatusverfolgungZulassungSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<ZulassungsReportModel> Items
        {
            get { return PropertyCacheGet(() => new List<ZulassungsReportModel>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<ZulassungsReportModel> ItemsFiltered
        {
            get { return PropertyCacheGet(() => Items); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<FahrzeugStatus> FahrzeugStatusWerte
        {
            get { return PropertyCacheGet(() => DataService.FahrzeugStatusWerte); }
        }

        [XmlIgnore, ScriptIgnore]
        public List<Kunde> Kunden
        {
            get { return PropertyCacheGet(() => DataService.Kunden); }
        }

        public StatusverfolgungDetails StatusverfolgungDetails { get; set; }

        public void DataInit()
        {
            DataMarkForRefresh(true);
        }

        public void DataMarkForRefresh(bool refreshStammdaten = false)
        {
            PropertyCacheClear(this, m => m.ItemsFiltered);

            if (refreshStammdaten)
            {
                PropertyCacheClear(this, m => m.FahrzeugStatusWerte);
                PropertyCacheClear(this, m => m.Kunden);
            }
        }

        public void LoadData(Action<string, string> addModelError)
        {
            Items = DataService.GetZulassungsReportItems(Selektor, Kunden, addModelError);

            if (Items.None())
                addModelError("", Localize.NoDataFound);

            DataMarkForRefresh();
        }

        public void FilterData(string filterValue, string filterProperties)
        {
            ItemsFiltered = Items.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }

        public void LoadStatusverfolgungDetails(string belegNr, Action<string, string> addModelError)
        {
            var zulDaten = Items.FirstOrDefault(z => z.BelegNummer == belegNr);

            var itemsRaw = DataService.GetStatusverfolgungItems(belegNr);

            StatusverfolgungDetails = new StatusverfolgungDetails
            {
                Zulassungsdaten = ModelMapping.Copy(zulDaten),
                AktuellerStatusCode = (itemsRaw.Any() ? itemsRaw.Last().Status : ""),
                AktuellerStatusDatumUhrzeit = (itemsRaw.Any() ? itemsRaw.Last().StatusDatumUhrzeit : null)
            };

            if (itemsRaw.None())
            {
                addModelError("", Localize.NoStatusInformationFoundForSelectedRecord);
                return;
            }

            var aktiverVorgang = StatusverfolgungDetails.AktuellerStatusCode.NotIn("S,L");

            // Status 1 (beauftragt)
            var status1 = new StatusverfolgungStatusItem { StatusNr = 1 };

            var itemsStatus1 = itemsRaw.Where(s => s.Status.In("1,4,5,A")).ToList();

            if (aktiverVorgang && itemsStatus1.Any())
            {
                status1.StatusDatumUhrzeit = itemsStatus1.Last().StatusDatumUhrzeit;
                status1.IsCompleted = true;
            }

            // Status 2 (unterwegs)
            var status2 = new StatusverfolgungStatusItem { StatusNr = 2 };

            var itemsStatus2 = itemsRaw.Where(s => s.Status.In("1,4,5,A")).ToList();

            if (aktiverVorgang && itemsStatus2.Any())
            {
                status2.StatusDatumUhrzeit = itemsStatus2.Last().StatusDatumUhrzeit;
                status2.IsCompleted = true;

                itemsStatus2.ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item.TrackingId))
                    {
                        status2.TrackingInfos.Add(new StatusverfolgungTrackingInfo
                        {
                            VersandDienstleister = item.VersandDienstleister,
                            VersandDienstleisterUrl = GeneralConfiguration.GetConfigValue("Sendungsverfolgung", string.Format("URL_{0}", item.VersandDienstleister)),
                            PartnerRolle = item.PartnerRolle,
                            TrackingId = item.TrackingId,
                            Bemerkung = item.Bemerkung
                        });
                    }
                });
            }

            // Status 3 (Zulassung)
            var status3 = new StatusverfolgungStatusItem { StatusNr = 3 };

            var itemsStatus3 = itemsRaw.Where(s => s.Status.In("2,F")).ToList();

            if (aktiverVorgang && itemsStatus3.Any())
            {
                status3.StatusDatumUhrzeit = itemsStatus3.Last().StatusDatumUhrzeit;
                status3.IsCompleted = true;
                status3.IsFailed = itemsStatus3.Any(i => i.Status == "F");
            }

            // Status 4 (unterwegs zurück)
            var status4 = new StatusverfolgungStatusItem { StatusNr = 4 };

            var itemsStatus4 = itemsRaw.Where(s => s.Status.In("2,F")).ToList();

            if (aktiverVorgang && itemsStatus4.Any() && itemsStatus4.None(i => i.Status == "F"))
            {
                status4.StatusDatumUhrzeit = itemsStatus4.Last().StatusDatumUhrzeit;
                status4.IsCompleted = true;

                itemsStatus4.ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item.TrackingId))
                    {
                        status1.TrackingInfos.Add(new StatusverfolgungTrackingInfo
                        {
                            VersandDienstleister = item.VersandDienstleister,
                            VersandDienstleisterUrl = GeneralConfiguration.GetConfigValue("Sendungsverfolgung", string.Format("URL_{0}", item.VersandDienstleister)),
                            PartnerRolle = item.PartnerRolle,
                            TrackingId = item.TrackingId,
                            Bemerkung = item.Bemerkung
                        });
                    }
                });
            }

            // Status 5 (abgeschlossen)
            var status5 = new StatusverfolgungStatusItem { StatusNr = 5 };

            var itemsStatus5 = itemsRaw.Where(s => s.Status == "7").ToList();

            if (aktiverVorgang && itemsStatus5.Any())
            {
                status5.StatusDatumUhrzeit = itemsStatus5.Last().StatusDatumUhrzeit;
                status5.IsCompleted = true;
            }

            StatusverfolgungDetails.Statusverfolgungsdaten = new List<StatusverfolgungStatusItem>
            {
                status1,
                status2,
                status3,
                status4,
                status5
            };
        }
    }
}
