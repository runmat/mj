using System;
using System.Collections.Generic;
using System.Data;

namespace EasyExportGeneralTask
{
    public class EasyResult
    {
        /// <summary>
        /// Trefferzeilen-Zähler
        /// </summary>
        public int hitCounter { get; set; }

        /// <summary>
        /// Liste der Treffer
        /// </summary>
        public DataTable hitList { get; set; }

        public EasyResult()
        {
            clear();
        }

        /// <summary>
        /// Setzt die Result Tabelle zurück
        /// </summary>
        public void clear()
        {
            hitList = new DataTable {TableName = "HitList"};

            // Grundspalten ergänzen
            hitList.Columns.Add("DOC_Location", typeof(String));
            hitList.Columns.Add("DOC_Archive", typeof(String));
            hitList.Columns.Add("DOC_ID", typeof(String));
            hitList.Columns.Add("DOC_VERSION", typeof(String));
            hitList.Columns.Add("Bilder", typeof(String));
            hitList.Columns.Add("Filepath", typeof(String));
            hitList.Columns.Add("File", typeof(String));
            hitList.Columns.Add("FileLength", typeof(Int32));
        }

        /// <summary>
        /// Fügt eine Reihe von Spalten zur Ergebnis-Tabelle hinzu falls diese noch nicht existieren
        /// </summary>
        /// <param name="lst">Liste der Spaltennamen</param>
        public void AddColumnsToResultTable(List<string> lst)
        {
            foreach (string item in lst)
            {
                if (!hitList.Columns.Contains(item))
                {
                    hitList.Columns.Add(item);
                }
            }
        }

        /// <summary>
        /// Erzeugt aus einem EasyArchiv Header-String eine Liste der vorhandenen Spalten
        /// </summary>
        /// <param name="header">unformatierter Header-String aus EasyArchiv-Abfrage</param>
        /// <returns>Liste von Spaltennamen in Easyarchiv-Reihenfolge</returns>
        public List<string> CreateHeaderFieldlist(string header)
        {
            string[] headerSplit = header.Replace("^", "").ToUpper().Split(',');

            return new List<string>(headerSplit);
        }

        /// <summary>
        /// Fügt der Ergebnistabelle eine neue Row hinzu und füllt die angegebenen Spalten mit Werten aus dem Row-String
        /// </summary>
        /// <param name="row">Easy-Archiv DatenRow als String</param>
        /// <param name="columnlist">Spaltenschema der Row als Liste</param>
        /// <param name="strLocFound">EasyArchiv Location</param>
        /// <param name="strArcFound">EasyArchiv Archivname</param>
        /// <param name="doc_id">EasyArchiv DocumentID</param>
        /// <param name="doc_version">EasyArchiv Documentversion</param>
        public void addRowToResultTable(string row, List<string> columnlist, 
            string strLocFound, string strArcFound, string doc_id, string doc_version)
        {
            string[] SplittedRow = row.Replace("^,^", ";").Replace("^", "").Split(';');

            DataRow nRow = hitList.NewRow();

            nRow["DOC_Location"] = strLocFound;
            nRow["DOC_Archive"] = strArcFound;
            nRow["DOC_ID"] = doc_id;
            nRow["DOC_VERSION"] = doc_version;
            nRow["Bilder"] = null;

            for (int i = 0; i < SplittedRow.GetLength(0); i++)
            {
                nRow[columnlist[i]] = SplittedRow[i];
            }

            hitList.Rows.Add(nRow);
            hitList.AcceptChanges();
        }
    }
}
