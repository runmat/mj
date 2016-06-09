using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Archive.Contracts;
using CkgDomainLogic.Archive.Models;
using EasyAccess40;
using GeneralTools.Models;

namespace CkgDomainLogic.Archive.Services
{
    public class EasyAccessDataService : CkgGeneralDataService, IEasyAccessDataService
    {
        private EasyAccess m_easyAccess;

        public EasyAccessSuchparameter Suchparameter { get { return PropertyCacheGet(() => LoadSuchparameter()); } }
        public DataTable Documents { get { return PropertyCacheGet(() => LoadDocuments()); } }

        public bool HasErrors { get { return (!String.IsNullOrEmpty(ErrorMessage)); } }
        public string ErrorMessage { get; set; }

        public EasyAccessDataService()
        {
            m_easyAccess = new EasyAccess(LogonContext);
        }

        public void MarkForRefreshSuchparameter()
        {
            PropertyCacheClear(this, m => m.Suchparameter);
        }

        public void MarkForRefreshDocuments()
        {
            PropertyCacheClear(this, m => m.Documents);
        }

        public void ApplySuchparameter(EasyAccessSuchparameter suchparameter)
        {
            // Archivtyp
            Suchparameter.Archivtyp = suchparameter.Archivtyp;

            // Archivauswahl
            foreach (var arc in suchparameter.ArchivesOfType)
            {
                Suchparameter.ArchivesOfType.Find(a => a.Name == arc.Name).Selected = arc.Selected;
            }

            // Suchfelder
            foreach (var field in suchparameter.SearchFields)
            {
                Suchparameter.SearchFields.Find(s => s.Id == field.Id).FieldValue = field.FieldValue;
            }
        }

        private EasyAccessSuchparameter LoadSuchparameter()
        {
            ErrorMessage = "";

            EasyAccessSuchparameter erg = new EasyAccessSuchparameter();

            // LogonContext nochmal explizit setzen, da nicht zwangsläufig schon im Konstruktor komplett
            m_easyAccess.setUser(LogonContext);

            LoadArchives(erg);
            LoadArchiveTypeSpecificData(erg, true);       

            return erg;
        }

        public void UpdateSuchparameter(string archiveType)
        {
            ErrorMessage = "";

            Suchparameter.Archivtyp = archiveType;  
            LoadArchiveTypeSpecificData(Suchparameter);
        }

        private void LoadArchives(EasyAccessSuchparameter suchparameter)
        {
            // Archive
            suchparameter.Archives.Clear();
            EasyArchive archives = m_easyAccess.getArchives(); 
            archives.resetCounter();
            while (archives.hasNext())
            {
                EasyAccess40.Archive arc = archives.nextArchive();
                suchparameter.Archives.Add(new EasyAccessArchive
                    {
                        ArchiveType = arc.Type, 
                        DefaultQuery = arc.DefaultQuery, 
                        Id = arc.Id,
                        Index = arc.Index,
                        Location = arc.Location,
                        IndexName = arc.IndexName,
                        Name = arc.Name,
                        Selected = false,
                        TitleName = arc.TitleName
                    });
            }

            string status = "";
            m_easyAccess.init(ref status);
            ErrorMessage = status;

            if (!HasErrors)
            {
                // Archivtypen
                suchparameter.ArchiveTypes = (from a in suchparameter.Archives
                                    orderby a.ArchiveType
                                    select a.ArchiveType).Distinct().ToList();

                if (suchparameter.ArchiveTypes.Any())
                    suchparameter.Archivtyp = suchparameter.ArchiveTypes.FirstOrDefault();
            }
            else
            {
                suchparameter.ArchiveTypes.Clear();
            }
        }

        private void LoadArchiveTypeSpecificData(EasyAccessSuchparameter suchparameter, bool initial = false)
        {
            if (!HasErrors)
            {
                // Archive des gewählten Typs
                suchparameter.ArchivesOfType =
                    suchparameter.Archives.FindAll(a => a.ArchiveType == suchparameter.Archivtyp)
                                 .OrderByDescending(a => a.Name)
                                 .ToList();

                if (suchparameter.ArchivesOfType.Any())
                {
                    var firstArchive = suchparameter.ArchivesOfType.First();

                    // initial die ersten 2 Archive des Typs selektieren
                    if (initial)
                    {
                        firstArchive.Selected = true;

                        if (suchparameter.ArchivesOfType.Count > 1)
                            suchparameter.ArchivesOfType[1].Selected = true;
                    }

                    // Suchfelder
                    suchparameter.SearchFields.Clear();
                    string strSearchFields = "";
                    string status = "";
                    EasyAccess40.Archive selEasyArchive = GetEasyAccess40Archive(firstArchive);
                    List<EasyResultField> tmpListe = m_easyAccess.getSearchFields(selEasyArchive, ref strSearchFields, ref status);
                    ErrorMessage = status;

                    foreach (EasyResultField item in tmpListe)
                    {
                        suchparameter.SearchFields.Add(new EasyAccessResultField
                            {
                                FieldValue = "",
                                Id = item.Id,
                                Index = item.Index,
                                Name = item.Name
                            });
                    }
                }
            }
            else
            {
                suchparameter.ArchivesOfType.Clear();
                suchparameter.SearchFields.Clear();
            }
        }

        private DataTable LoadDocuments()
        {
            ErrorMessage = "";

            DataTable erg = new DataTable();

            string queryString = "";
            foreach (var field in Suchparameter.SearchFields)
            {
                if (!String.IsNullOrEmpty(field.FieldValue))
                {
                    queryString += "." + field.Name + "=" + field.FieldValue + " & ";
                }
            }
            queryString = queryString.TrimEnd(' ', '&');

            m_easyAccess.getResult().hitTblHeader = getColumnsToshow();

            string status = "";
            List<EasyAccess40.Archive> tmpListe = new List<EasyAccess40.Archive>();
            foreach (EasyAccessArchive arc in Suchparameter.ArchivesOfType.FindAll(a => a.Selected))
            {
                tmpListe.Add(GetEasyAccess40Archive(arc));
            }
            if (tmpListe.Count > 0)
            {
                m_easyAccess.query(tmpListe, queryString, ref status);
                ErrorMessage = status;
            }
            else
            {
                ErrorMessage = "Kein Archiv ausgewählt";
            }

            if (!HasErrors)
            {
                erg = m_easyAccess.getResult().getHitTable();
                // Grid hat Probleme mit Spaltennamen, die Sonderzeichen enthalten
                foreach (DataColumn col in erg.Columns)
                {
                    col.ColumnName = col.ColumnName.Replace(".", "").Replace("-", "").Replace("'", "").Replace("\"", "").Replace(",", "").Replace(";", "").Replace("/", "").Replace("\\", "").Replace(" ", "");
                }
            }

            return erg;
        }

        private EasyAccess40.Archive GetEasyAccess40Archive(EasyAccessArchive arc)
        {
            long arcId;
            long.TryParse(arc.Id, out arcId);
            int arcIdx;
            int.TryParse(arc.Index, out arcIdx);
            return new EasyAccess40.Archive(arcId, arc.Location, arc.Name, arcIdx, arc.IndexName, 
                arc.TitleName, arc.DefaultQuery, arc.ArchiveType);

        }

        public string ViewDocument(string docId)
        {
            string docLink = "";

            ErrorMessage = "";

            try
            {
                DataRow[] rows = Documents.Select("DOC_ID='" + docId + "'");

                if (rows.Length > 0)
                {
                    DataRow dRow = rows[0];

                    string status = "";
                    m_easyAccess.getPics(dRow["DOC_Location"].ToString(), dRow["DOC_Archive"].ToString(), dRow["DOC_ID"].ToString(),
                        dRow["DOC_Version"].ToString(), ref status, ref docLink);
                    ErrorMessage = status;
                }
                else
                {
                    ErrorMessage = "Dokument wurde nicht gefunden";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Fehler: " + ex.Message;
            }

            return docLink;
        }

        private List<EasyResultField> getColumnsToshow()
        {
            List<EasyResultField> liste = new List<EasyResultField>();

            foreach (EasyAccessResultField sPar in Suchparameter.SearchFields)
            {
                int intId;
                Int32.TryParse(sPar.Id, out intId);
                int intIdx;
                Int32.TryParse(sPar.Index, out intIdx);
                liste.Add(new EasyResultField(sPar.Name, intId, intIdx));
            }

            return liste;
        }

        public EasyAccessDetail GetDocumentDetail(string docId)
        {
            EasyAccessDetail erg = null;

            DataRow[] rows = Documents.Select("DOC_ID='" + docId + "'");

            if (rows.Length > 0)
            {
                DataRow doc = rows[0];

                int tmpLaenge = 0;
                string tmpErstelldatum = "";
                string tmpAenderungsdatum  = "";
                string tmpTitel = "";
                int tmpAnzFelderGesamt = 0;
                int tmpAnzFelderText = 0;
                int tmpAnzFelderBild = 0;
                int tmpAnzFelderBlob = 0;
                string tmpStatus = "";

                m_easyAccess.getDocumentInfo(doc["DOC_LOCATION"].ToString(), doc["DOC_ARCHIVE"].ToString(), docId, doc["DOC_VERSION"].ToString(), 
                    ref tmpLaenge, ref tmpErstelldatum, ref tmpAenderungsdatum, ref tmpTitel, ref tmpAnzFelderGesamt, ref tmpAnzFelderText, 
                    ref tmpAnzFelderBild, ref tmpAnzFelderBlob, ref tmpStatus);

                erg = new EasyAccessDetail{ Aenderungsdatum = tmpAenderungsdatum,
                                            AnzFelderBild = tmpAnzFelderBild,
                                            AnzFelderBlob = tmpAnzFelderBlob, 
                                            AnzFelderGesamt = tmpAnzFelderGesamt, 
                                            AnzFelderText = tmpAnzFelderText, 
                                            DocId = docId,
                                            Erstelldatum = tmpErstelldatum, 
                                            Laenge = tmpLaenge, 
                                            Status = tmpStatus, 
                                            Titel = tmpTitel 
                };
            }

            return erg;
        }

        public List<string> GetDocuments(EasyAccessArchiveDefinition archiveToSearch, string query)
        {
            return GetDocuments(new List<EasyAccessArchiveDefinition> { archiveToSearch }, query);
        }

        public List<string> GetDocuments(List<EasyAccessArchiveDefinition> archivesToSearch, string query)
        {
            // LogonContext nochmal explizit setzen, da nicht zwangsläufig schon im Konstruktor komplett
            m_easyAccess.setUser(LogonContext);

            var docList = new List<string>();

            var status = "";

            if (archivesToSearch.None())
                ErrorMessage = "Kein Archiv ausgewählt";

            if (!HasErrors)
            {
                m_easyAccess.init(ref status);
                ErrorMessage = status;
            }

            if (!HasErrors)
            {
                var archiveList = new List<EasyAccess40.Archive>();
                archivesToSearch.ForEach(a => archiveList.Add(new EasyAccess40.Archive(0, a.Location, a.Name, 0, a.IndexName, "", null, "")));

                m_easyAccess.query(archiveList, query, ref status);
                ErrorMessage = status;
            }

            if (!HasErrors)
            {
                var tblResults = m_easyAccess.getResult().getHitTable();

                foreach (DataRow row in tblResults.Rows)
                {
                    var docStatus = "";
                    var docPath = "";
                    m_easyAccess.getPics(row["DOC_Location"].ToString(), row["DOC_Archive"].ToString(), row["DOC_ID"].ToString(),
                        row["DOC_Version"].ToString(), ref docStatus, ref docPath);

                    if (string.IsNullOrEmpty(docStatus))
                        docList.Add(docPath);
                }
            }

            return docList;
        }

        public List<string> GetDocuments(string archiveLocation, string archiveName, string archiveIndex, string query)
        {
            return GetDocuments(new List<EasyAccessArchiveDefinition> { new EasyAccessArchiveDefinition { Location = archiveLocation, IndexName = archiveIndex, Name = archiveName } }, query);
        }
    }
}
