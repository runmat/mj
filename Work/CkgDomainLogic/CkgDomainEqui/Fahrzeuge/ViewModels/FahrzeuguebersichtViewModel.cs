﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using GeneralTools.Services;
using System.IO;
using System.Data;
using DocumentTools.Services;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class FahrzeuguebersichtViewModel : CkgBaseViewModel
    {

        [XmlIgnore]
        public IFahrzeuguebersichtDataService DataService { get { return CacheGet<IFahrzeuguebersichtDataService>(); } }

        [XmlIgnore]
        public IFahrzeugeDataService DataServiceHersteller { get { return CacheGet<IFahrzeugeDataService>(); } }       

        [XmlIgnore]
        public List<Fahrzeughersteller> FahrzeugHersteller
        {
            get
            {
                return DataServiceHersteller.GetFahrzeugHersteller().Concat(new List<Fahrzeughersteller>
            {
                new Fahrzeughersteller { HerstellerKey = String.Empty, HerstellerName = Localize.DropdownDefaultOptionAll, ShowAllToken = true}
                                        }).OrderBy(w => w.HerstellerName).ToList();
            }
        }

        [XmlIgnore]
        public List<FahrzeuguebersichtPDI> PDIStandorte
        {
            get
            {
                return DataService.GetPDIStandorte().Concat(new List<FahrzeuguebersichtPDI>
            {
                new FahrzeuguebersichtPDI { PDIKey = String.Empty, PDIText = Localize.DropdownDefaultOptionAll }
                                        }).OrderBy(w => w.PDIText).ToList();
            }
        }
       
        [XmlIgnore]
        public List<FahrzeuguebersichtStatus> FahrzeugStatus
        {
            get
            {
                return DataService.GetFahrzeugStatus().Concat(new List<FahrzeuguebersichtStatus>
            {
                new FahrzeuguebersichtStatus { StatusKey = String.Empty, StatusText = Localize.DropdownDefaultOptionAll }
                                        }).OrderBy(w => w.StatusText).ToList();
            }
        }
                           
        [XmlIgnore]
        public List<Fahrzeuguebersicht> Fahrzeuguebersichts
        {
            get { return PropertyCacheGet(() => new List<Fahrzeuguebersicht>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<Fahrzeuguebersicht> FahrzeuguebersichtsFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuguebersichts); }
            private set { PropertyCacheSet(value); }
        }


        public FahrzeuguebersichtSelektor FahrzeuguebersichtSelektor
        {
            get { return PropertyCacheGet(() => new FahrzeuguebersichtSelektor()); }
            set { PropertyCacheSet(value); }
        }

        public void Init()
        {
            FahrzeuguebersichtSelektor.Akion = "manuell";
            FahrzeuguebersichtSelektor.Herstellerkennung = string.Empty;
            FahrzeuguebersichtSelektor.Statuskennung = string.Empty;
            FahrzeuguebersichtSelektor.PDIkennung = string.Empty;            
        }

        public void DataInit()
        {          
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeuguebersichtsFiltered);
        }

       
        public void LoadFahrzeuguebersicht()
        {
            
            Fahrzeuguebersichts = DataService.GetFahrzeuguebersicht(FahrzeuguebersichtSelektor);

            #region custom selector post load filter

            if (FahrzeuguebersichtSelektor.Akion == "manuell")
            {
                UploadItems = null;
                CsvUploadFileName = String.Empty;

                var customList = Fahrzeuguebersichts.Select(x => x).ToList();

                if (FahrzeuguebersichtSelektor.Kennzeichen.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Kennzeichen == FahrzeuguebersichtSelektor.Kennzeichen).ToList();

                if (FahrzeuguebersichtSelektor.Unitnummer.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Unitnummer == FahrzeuguebersichtSelektor.Unitnummer).ToList();

                if (FahrzeuguebersichtSelektor.Auftragsnummer.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Auftragsnummer == FahrzeuguebersichtSelektor.Auftragsnummer).ToList();

                if (FahrzeuguebersichtSelektor.BatchId.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.BatchId == FahrzeuguebersichtSelektor.BatchId).ToList();

                if (FahrzeuguebersichtSelektor.SIPPCode.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.SIPPCode == FahrzeuguebersichtSelektor.SIPPCode).ToList();

                if (FahrzeuguebersichtSelektor.ModelID.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.ModelID == FahrzeuguebersichtSelektor.ModelID).ToList();

                if (FahrzeuguebersichtSelektor.Zb2Nummer.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Zb2Nummer == FahrzeuguebersichtSelektor.Zb2Nummer).ToList();

                if (FahrzeuguebersichtSelektor.Herstellerkennung.IsNotNullOrEmpty())
                    customList = customList.Where(x => x.Hersteller.Contains(FahrzeuguebersichtSelektor.Herstellerkennung)).ToList();

                Fahrzeuguebersichts = customList;
            }
                      
            #endregion

            #region custom excel upload filter

            if (FahrzeuguebersichtSelektor.Akion == "upload" && UploadItems != null && UploadItems.Count > 0)
            {
                                
                var filterList = Fahrzeuguebersichts.Intersect(UploadItems, new KeyEqualityComparer<Fahrzeuguebersicht>(s => s.Fahrgestellnummer));

                filterList = filterList.Intersect(UploadItems, new KeyEqualityComparer<Fahrzeuguebersicht>(s => s.Kennzeichen));

                filterList = filterList.Intersect(UploadItems, new KeyEqualityComparer<Fahrzeuguebersicht>(s => s.ModelID));

                // ...  TODO -> testen, ob additiv

                Fahrzeuguebersichts = filterList.ToList();
                
            }
            #endregion
                                  
            DataMarkForRefresh();

            //XmlService.XmlSerializeToFile(Fahrzeuguebersichts, Path.Combine(AppSettings.DataPath, @"Fahrzeuguebersichts.xml"));
        }

        public void FilterFahrzeuguebersicht(string filterValue, string filterProperties)
        {
            FahrzeuguebersichtsFiltered = Fahrzeuguebersichts.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }


        public List<Fahrzeuguebersicht> UploadItems { get; private set; }

        public string CsvUploadFileName { get; private set; }
        public string CsvUploadServerFileName { get; private set; }


        public bool CsvUploadFileSave(string fileName, Func<string, bool> fileSaveAction)
        {
            UploadItems = null;
                
            CsvUploadFileName = fileName;
            CsvUploadServerFileName = Path.Combine(AppSettings.TempPath, Guid.NewGuid() + ".xls");

            if (!fileSaveAction(CsvUploadServerFileName))
                return false;

            IEnumerable<Fahrzeuguebersicht> list = new ExcelDocumentFactory().ReadToDataTable<Fahrzeuguebersicht>(CsvUploadServerFileName,
                                                                                            true, "", CreateInstanceFromDatarow, ',', false, false).ToList();
            
            FileService.TryFileDelete(CsvUploadServerFileName);
            if (list.None())
                return false;

            UploadItems = list.ToList();
           
            return true;
        }

        static Fahrzeuguebersicht CreateInstanceFromDatarow(DataRow row)
        {
            var item = new Fahrzeuguebersicht
            {                                
                Fahrgestellnummer = row[0].ToString(),
                Kennzeichen = row[1].ToString(),
                Zb2Nummer = row[2].ToString(),
                ModelID = row[3].ToString(),
                Unitnummer = row[4].ToString(),                
                Auftragsnummer = row[5].ToString(),
                BatchId = row[6].ToString(),
                SIPPCode = row[7].ToString(),
            };
            return item;
        }

    }

    class KeyEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, object> keyExtractor;

        public KeyEqualityComparer(Func<T, object> keyExtractor)
        {
            this.keyExtractor = keyExtractor;
        }

        public bool Equals(T x, T y)
        {
            return this.keyExtractor(x).Equals(this.keyExtractor(y));
        }

        public int GetHashCode(T obj)
        {
            return this.keyExtractor(obj).GetHashCode();
        }
    }
}
