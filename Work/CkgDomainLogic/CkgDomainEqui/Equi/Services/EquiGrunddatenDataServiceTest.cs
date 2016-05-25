using System.Collections.Generic;
using System.IO;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Services;
using System.Linq;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.Services
{
    public class EquiGrunddatenDataServiceTest : CkgGeneralDataServiceTest, IEquiGrunddatenDataService
    {
        public List<SelectItem> GetZielorte()
        {
            return new List<SelectItem>();
        }

        public List<SelectItem> GetStandorte()
        {
            return new List<SelectItem>();
        }

        public List<SelectItem> GetBetriebsnummern()
        {
            return new List<SelectItem>();
        }

        public List<EquiGrunddaten> GetEquis(EquiGrunddatenSelektor suchparameter)
        {
            var list = XmlService.XmlDeserializeFromFile<List<EquiGrunddaten>>(Path.Combine(AppSettings.DataPath, @"GrunddatenEquis_02.xml"));


            if (suchparameter.Standorte.AnyAndNotNull())
                list = list.Where(e => suchparameter.Standorte.Any(f => f == e.Standort)).ToList();

            if (suchparameter.Betriebsnummern.AnyAndNotNull())
                list = list.Where(e => suchparameter.Betriebsnummern.Any(f => f == e.Betrieb)).ToList();

            if (suchparameter.Zielorte.AnyAndNotNull())
                list = list.Where(e => suchparameter.Zielorte.Any(f => f == e.Zielort)).ToList();


            if (suchparameter.Fahrgestellnummern.AnyAndNotNull())
                list = list.Where(e => suchparameter.Fahrgestellnummern.Any(f => f.FIN.Trim() == e.Fahrgestellnummer.Trim())).ToList();

            if (suchparameter.Fahrgestellnummern10.AnyAndNotNull())
                list = list.Where(e => suchparameter.Fahrgestellnummern10.Any(f => f.FIN.Trim() == e.Fahrgestellnummer10.Trim())).ToList();


            if (suchparameter.ErfassungsDatumRange.IsSelected)
                list = list.Where(e => e.Erfassungsdatum.GetValueOrDefault() >= suchparameter.ErfassungsDatumRange.StartDate && e.Erfassungsdatum.GetValueOrDefault() <= suchparameter.AbmeldeDatumRange.EndDate).ToList();

            if (suchparameter.AbmeldeDatumRange.IsSelected)
                list = list.Where(e => e.Abmeldedatum.GetValueOrDefault() >= suchparameter.AbmeldeDatumRange.StartDate && e.Abmeldedatum.GetValueOrDefault() <= suchparameter.AbmeldeDatumRange.EndDate).ToList();

            return list;
        }
    }
}
