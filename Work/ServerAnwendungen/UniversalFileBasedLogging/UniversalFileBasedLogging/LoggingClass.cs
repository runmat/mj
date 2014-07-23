using System;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace UniversalFileBasedLogging
{
    /// <summary>
    /// Verwaltungsklasse zum Lesen und Schreiben in ein Log-File
    /// </summary>
    public class LoggingClass
    {
        // Pfade für XML oder Text Log-Dateien
        static string _pathText;
        static string _pathXml;

        static XmlDocument _xDoc;    // XML-Dokument in das geloggt wird
        static XmlReader _xRead;     // globaler Reader 
        static XmlWriter _xWrite;    // globaler Writer

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logpath">Pfad in dem die Log-Datei liegt oder angelegt werden soll</param>
        /// <param name="logName">Name des Log-Files auf das zugegriffen werden soll</param>
        public LoggingClass(string logpath, string logName)
        { 
            if(!logpath.EndsWith("\\"))
            {
                logpath += "\\";
            }

            if (logName != "")
            {
                _pathText = logpath + logName + "_" + DateTime.Today.Month.ToString() + DateTime.Today.Year + ".txt";
                _pathXml = logpath + logName + "_" + DateTime.Today.Month.ToString() + DateTime.Today.Year + ".xml";
            }
            else
            {
                _pathText = logpath + "Log_" + DateTime.Today.Month.ToString() + DateTime.Today.Year + ".txt";
                _pathXml = logpath + "Log_" + DateTime.Today.Month.ToString() + DateTime.Today.Year + ".xml";
            }

            try
            {
                // Erstellt das Grundgerüst des Log-Files
                FileStream fs;

                if (!File.Exists(_pathXml))
                {
                    // Datei erzeugen
                    fs = new FileStream(_pathXml, FileMode.OpenOrCreate);
                    
                    // Optionen für Dateizugriff festlegen
                    var xwSettings = new XmlWriterSettings {CloseOutput = true};
                    // Writer erzeugen
                    _xWrite = XmlWriter.Create(fs, xwSettings);
                    _xWrite.WriteStartDocument();
                    _xWrite.WriteElementString("Log", "");
                    _xWrite.WriteEndDocument();

                    _xWrite.Flush();
                    _xWrite.Close();                    
                }
               
                // Liest das angelegte XML-File aus und erzeugt daraus ein XML-Dokument-Objekt.
                // Das Objekt wird global für Änderungen verwendet.
                fs = new FileStream(_pathXml, FileMode.Open);
                var xrSettings = new XmlReaderSettings {CloseInput = true};
                _xRead = XmlReader.Create(fs, xrSettings);
                _xDoc = new XmlDocument();
                _xDoc.Load(_xRead);

                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("UniversalFileBasedLogging", "Es ist ein XML-Fehler aufgetreten: " + ex.ToString(), EventLogEntryType.Warning);
            }
        }

        public string LogfilePathText
        {
            get { return _pathText; }
            set { _pathText = value; }
        }

        public string LogfilePathXml
        {
            get { return _pathXml; }
            set { _pathXml = value; }
        }

        public void WriteToXml()
        {
            var xwset = new XmlWriterSettings {CloseOutput = true, Indent = true};
            _xWrite = XmlWriter.Create(_pathXml, xwset);
            _xDoc.Save(_xWrite);
            _xWrite.Close();
        }

        /// <summary>
        /// Schreibt einen neuen Logeintrag in die angebene XML-Datei.
        /// </summary>
        /// <param name="data">Datensätze als Liste</param>
        public void NewXmlEntry(LogDataset data)
        {
            foreach (LogCustomer custom in data)
            {
                //NewXMLEntry(custom);
                foreach(LogFile file in custom)
                {
                    foreach (LogEntry entry in file)
                    {
                        NewXmlEntry(entry);
                    }
                }
            }

        }

        /// <summary>
        /// Legt einen neuen Eintrag für ein Datei-Objekt an
        /// </summary>
        /// <param name="file">Datei-Objekt</param>
        /// <returns>der neu angelegte XML-Knoten</returns>
        public XmlNode NewXmlEntry(LogFile file)
        {
            // Prüfen ob File bereits im Log angelegt
            XmlNode filenode = hasFile(file);

            if (filenode == null)
            {
                // Prüfen ob File bereits im Log angelegt
                XmlNode customnode = hasCustomer(file.Customer) ?? NewXmlEntry(file.Customer);

                //Neuen Eintragknoten anlegen
                filenode = _xDoc.CreateElement("Datei");

                // Attribute erzeugen
                XmlAttribute atr0 = _xDoc.CreateAttribute("Filename");
                XmlAttribute atr1 = _xDoc.CreateAttribute("NameEasy");
                XmlAttribute atr2 = _xDoc.CreateAttribute("Kennzeichen");
                XmlAttribute atr3 = _xDoc.CreateAttribute("FIN");
                XmlAttribute atr4 = _xDoc.CreateAttribute("strTyp");
                XmlAttribute atr5 = _xDoc.CreateAttribute("Try");
                XmlAttribute atr6 = _xDoc.CreateAttribute("Abgeschlossen");
                XmlAttribute atr7 = _xDoc.CreateAttribute("Titel");
                // Werte der Attribute festlegen
                atr0.Value = file.Filename;
                atr1.Value = file.NameEasy;
                atr2.Value = file.Kennzeichen;
                atr3.Value = file.FIN;
                atr4.Value = file.strTyp;
                atr5.Value = file.Trys.ToString();
                atr6.Value = file.Finished.ToString();
                atr7.Value = file.Titel;
                // Attribute an den Knoten hängen
                filenode.Attributes.Append(atr0);
                filenode.Attributes.Append(atr1);
                filenode.Attributes.Append(atr2);
                filenode.Attributes.Append(atr3);
                filenode.Attributes.Append(atr4);
                filenode.Attributes.Append(atr5);
                filenode.Attributes.Append(atr6);
                filenode.Attributes.Append(atr7);
                
                // Eintrag zuordnen
                customnode.AppendChild(filenode);
            }
            else 
            { 
                // Update der Daten

                foreach (XmlAttribute atr in filenode.Attributes)
                {
                    switch (atr.Name)
                    {
                        case "Filename":
                            atr.Value = file.Filename;
                            break;
                        case "NameEasy":
                            atr.Value = file.NameEasy;
                            break;
                        case "strTyp":
                            atr.Value = file.strTyp;
                            break;
                        case "Try":
                            atr.Value = file.Trys.ToString();
                            break;
                        case "Abgeschlossen":
                            atr.Value = file.Finished.ToString();
                            break;
                        case"Titel":
                             atr.Value = file.Titel;
                             break;
                    }
                }
            }

            return filenode; 
        }

        /// <summary>
        /// Legt einen neuen Eintrag im Log an
        /// </summary>
        /// <param name="entry">Eintragsobjekt</param>
        /// <returns>der neu angelegte XML-Knoten</returns>
        public XmlNode NewXmlEntry(LogEntry entry)
        {
            // Prüfen ob File bereits im Log angelegt
            XmlNode filenode = hasFile(entry.File) ?? NewXmlEntry(entry.File);

            //Neuen Eintragknoten anlegen
            XmlElement entrynode = _xDoc.CreateElement("Eintrag");
            
            // Attribute erzeugen
            XmlAttribute atr1 = _xDoc.CreateAttribute("Datum");
            XmlAttribute atr2 = _xDoc.CreateAttribute("Zeit");
            XmlAttribute atr3 = _xDoc.CreateAttribute("Status");
            // Werte der Attribute festlegen
            atr1.Value = entry.LogDate;
            atr2.Value = entry.LogTime;
            atr3.Value = entry.Status;
            // Attribute an den Knoten hängen
            entrynode.Attributes.Append(atr1);
            entrynode.Attributes.Append(atr2);
            entrynode.Attributes.Append(atr3);
            // Eintragswert festlegen
            entrynode.AppendChild(_xDoc.CreateTextNode(entry.Entry));
            // Eintrag zuordnen
            filenode.AppendChild(entrynode);

            return entrynode;
        }

        /// <summary>
        /// Legt einen neuen Kundeneintrag im Log an
        /// </summary>
        /// <param name="logCustomer">Kunden-Objekt</param>
        /// <returns>der neu angelegte XML-Knoten</returns>
        public XmlNode NewXmlEntry(LogCustomer logCustomer)
        {
            XmlNode node = hasCustomer(logCustomer);

            if (node == null)
            {
                // Knoten anlegen
                node = _xDoc.CreateElement("Kunde");
                // Attribute erzeugen
                XmlAttribute atr1 = _xDoc.CreateAttribute("Name");
                XmlAttribute atr2 = _xDoc.CreateAttribute("Archivname");
                // Werte der Attribute festlegen
                atr1.Value = logCustomer.Name;
                atr2.Value = logCustomer.Archivname;
                // Attribute an den Knoten hängen
                node.Attributes.Append(atr1);
                node.Attributes.Append(atr2);

                XmlNode rootnode = _xDoc.SelectSingleNode("/Log");
                rootnode.AppendChild(node);
            }

            return node;
        }

        /// <summary>
        /// Löscht einen Knoten und alle seine Unterknoten ausdem Log
        /// </summary>
        /// <param name="file">der zu löschende Eintrag</param>
        /// <returns>True bei Erfolg sonst False</returns>
        public bool DeleteXmlEntry(LogFile file)
        {
            bool bSuccess = false;

            // Prüfen ob File bereits im Log angelegt
            XmlNode filenode = hasFile(file);
            
            if (filenode != null)
            {
                XmlNode parentnode = filenode.ParentNode;
                if(parentnode != null)
                {
                    //filenode.RemoveAll();
                    parentnode.RemoveChild(filenode);
                    bSuccess = true;
                }
            }

            return bSuccess;
        }

        /// <summary>
        /// Prüft ob ein gegebener Kunde bereits exisitert
        /// </summary>
        /// <param name="customer">Kunde</param>
        /// <returns>Der gefundene oder neu angelegte Kunde</returns>
        private XmlNode hasCustomer(LogCustomer customer)
        {
            XmlNode node = _xDoc.SelectSingleNode("//Kunde [@Name='" + customer.Name + "']");

            return node;
        }

        /// <summary>
        /// Prüft ob eine Datei bereits im Log existiert
        /// </summary>
        /// <param name="file">Datei</param>
        /// <returns>Die gefundene oder neu angelegte Datei</returns>
        private static XmlNode hasFile(LogFile file)
        {
            string customer = file.Customer.Name;
            string xpath = "//Kunde [@Name='" + customer + "'] /Datei[@Kennzeichen='" + file.Kennzeichen + "' and @FIN='" + file.FIN + "']";

            XmlNode node = _xDoc.SelectSingleNode(xpath);

            return node;
        }

        /// <summary>
        /// Liest das aktuelle Log aus und erzeugt daraus ein Dataset
        /// </summary>
        /// <param name="blnTitelStattKennz">muss z.B. für Athlon gesetzt werden</param>
        /// <returns>Das ausgelesene Dataset</returns>
        public LogDataset ReadLog(bool blnTitelStattKennz)
        {
            var lds = new LogDataset();
            XmlNodeList nList = _xDoc.SelectNodes("//Kunde");

            // Liste der Kunden ziehen
            foreach (XmlElement node in nList)
            {
                new LogCustomer(node.GetAttribute("Name"), node.GetAttribute("Archivname"), lds);     
            }

            // Liste der Kunden durchlaufen
            foreach(LogCustomer custom in lds)
            {
                // Liste der Dateien pro Kunde ziehen
                nList = _xDoc.SelectNodes("//Kunde[@Name='" + custom.Name + "'] /Datei");

                // Dateien erzeugen und dem Kunden zuordnen
                foreach (XmlElement node in nList)
                {
                    new LogFile(
                            node.GetAttribute("FIN"),
                            (blnTitelStattKennz ? null : node.GetAttribute("Kennzeichen")),
                            (blnTitelStattKennz ? node.GetAttribute("Titel") : null),
                            custom
                        )
                        {
                            Filename = node.GetAttribute("Filename"),
                            NameEasy = node.GetAttribute("NameEasy"),
                            Trys = int.Parse(node.GetAttribute("Try")),
                            strTyp = node.GetAttribute("strTyp"),
                            Finished = bool.Parse(node.GetAttribute("Abgeschlossen"))
                        };
                }

                // Liste der Dateien durchlaufen
                foreach (LogFile file in custom)
                { 
                    // Einträge zur Datei suchen
                    if (blnTitelStattKennz)
                    {
                        nList = _xDoc.SelectNodes("//Kunde[@Name='" + custom.Name + "'] /Datei[@Titel='" + file.Titel + "' and @FIN='" + file.FIN + "'] /Eintrag");
                    }
                    else
                    {
                        nList = _xDoc.SelectNodes("//Kunde[@Name='" + custom.Name + "'] /Datei[@Kennzeichen='" + file.Kennzeichen + "' and @FIN='" + file.FIN + "'] /Eintrag");
                    }  
                    // Einträge erzeugen und der Datei zuordnen
                    foreach(XmlNode node in nList)
                    {
                        var elm = (XmlElement)node; //.Attributes.GetNamedItem("Status");
                        new LogEntry(elm.GetAttribute("Status"),node.Value,file)
                            {
                                LogTime = elm.GetAttribute("Zeit"),
                                LogDate = elm.GetAttribute("Datum")
                            };
                    }
                }
            }

            // Komplettes Dataset zurück geben
            return lds;
        }
    }
}
