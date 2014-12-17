using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace CkgServerTasks
{
    public class DeleteBatch
    {
        public void DeleteOldLogfileEntries()
        {
            try
            {
                FileInfo protocol = new FileInfo("log.txt");

                if (protocol.Exists)
                {
                    string inhalt;

                    using (StreamReader reader = new StreamReader(protocol.FullName))
                    {
                        inhalt = reader.ReadToEnd();
                        reader.Close();
                    }

                    string[] zeilen = inhalt.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    string inhaltNeu = "";

                    foreach (string zeile in zeilen)
                    {
                        if (zeile.Length > 10)
                        {
                            string strDat = zeile.Substring(0, 10);
                            DateTime tmpDat;
                            if (DateTime.TryParse(strDat, out tmpDat))
                            {
                                if (tmpDat > DateTime.Today.AddDays(-21))
                                {
                                    inhaltNeu += zeile + Environment.NewLine;
                                }
                            }
                        }
                    }

                    using (StreamWriter writer = new StreamWriter(protocol.FullName))
                    {
                        writer.Write(inhaltNeu);
                        writer.WriteLine(DateTime.Now.ToString() + " Einträge in der log.txt gelöscht!");
                        writer.Close();
                    }
                }
            }
            catch (Exception)
            {
                // NOP
            }
        }

        public void DeleteFilesXml()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Config.xml");

                if (doc.DocumentElement != null)
                {
                    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                    {
                        if (node.Name == "Paths" && node.Attributes != null)
                        {
                            string pfad = node.Attributes.GetNamedItem("Path").Value;
                            int zeitraumTage = Int32.Parse(node.Attributes.GetNamedItem("DaysBack").Value);
                            string mitUnterverzeichnissen = node.Attributes.GetNamedItem("MitUnterverzeichnis").Value;
                            string ordnerLoeschen = node.Attributes.GetNamedItem("MitOrdner").Value;

                            DirectoryInfo verzeichnis = new DirectoryInfo(pfad);

                            foreach (XmlNode patternNode in node.ChildNodes)
                            {
                                if (patternNode.Attributes != null)
                                {
                                    string pattern = patternNode.Attributes.GetNamedItem("Pattern").Value;

                                    if (mitUnterverzeichnissen == "Ja")
                                    {
                                        DirectoryInfo[] unterverzeichnisse = verzeichnis.GetDirectories();

                                        foreach (DirectoryInfo unterverzeichnis in unterverzeichnisse)
                                        {
                                            DirectoryInfo[] unterverzeichnisse2 = unterverzeichnis.GetDirectories();

                                            foreach (DirectoryInfo unterverzeichnis2 in unterverzeichnisse2)
                                            {
                                                DeleteFiles(unterverzeichnis2.FullName, pattern, zeitraumTage, ordnerLoeschen);
                                            }

                                            DeleteFiles(unterverzeichnis.FullName, pattern, zeitraumTage, ordnerLoeschen);
                                        }
                                    }

                                    DeleteFiles(verzeichnis.FullName, pattern, zeitraumTage, ordnerLoeschen);
                                }
                            }

                            Common.WriteLogEntry("Success", DateTime.Now.ToString() + " - Ordner " + verzeichnis.FullName + " wurde bereinigt!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteLogEntry("Fehler", ex.Message);
                Common.SendErrorMail(ex.Message);
            }
        }

        private void DeleteFiles(string pfad, string pattern, int zeitraumTage, string ordnerLoeschen)
        {
            try
            {
                DateTime loeschDatum = DateTime.Now.AddDays(-zeitraumTage);

                DirectoryInfo verzeichnis = new DirectoryInfo(pfad);

                if (verzeichnis.Exists)
                {
                    FileInfo[] dateien = verzeichnis.GetFiles(pattern);

                    foreach (FileInfo datei in dateien)
                    {
                        Common.WriteLogEntry("DEBUG", datei.FullName + " found");

                        if (datei.LastWriteTime < loeschDatum)
                        {
                            try
                            {
                                datei.Delete();
                            }
                            catch (Exception innerEx)
                            {
                                Common.WriteLogEntry("Fehler", datei.FullName + ": " + innerEx.Message);
                            }
                        }
                    }

                    if (ordnerLoeschen == "Ja")
                    {
                        if (verzeichnis.GetDirectories().Length == 0 && verzeichnis.GetFiles().Length == 0)
                        {
                            verzeichnis.Delete();
                            Common.WriteLogEntry("Hinweis:", "Verzeichnis " + verzeichnis.FullName + " wurde gelöscht.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteLogEntry("Fehler", ex.Message + ": " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                Common.SendErrorMail(ex.Message);
            }
        }

        public void ExecuteConfigDelLog()
        {
            try
            {
                string runOnlyOn = ConfigurationManager.AppSettings["RunUserLockOnlyOn"];
                bool runToday = true;

                if (!String.IsNullOrEmpty(runOnlyOn))
                {
                    DayOfWeek wochentag = DateTime.Today.DayOfWeek;

                    switch (runOnlyOn)
                    {
                        case "1":
                            runToday = (wochentag == DayOfWeek.Monday);
                            break;

                        case "2":
                            runToday = (wochentag == DayOfWeek.Tuesday);
                            break;

                        case "3":
                            runToday = (wochentag == DayOfWeek.Wednesday);
                            break;

                        case "4":
                            runToday = (wochentag == DayOfWeek.Thursday);
                            break;

                        case "5":
                            runToday = (wochentag == DayOfWeek.Friday);
                            break;

                        case "6":
                            runToday = (wochentag == DayOfWeek.Saturday);
                            break;

                        case "7":
                            runToday = (wochentag == DayOfWeek.Sunday);
                            break;
                    }
                }

                XmlDocument doc = new XmlDocument();
                doc.Load("ConfigDelLog.xml");

                if (doc.DocumentElement != null)
                {
                    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                    {
                        if (node.Name == "Connections")
                        {
                            if (node.Attributes != null)
                            {
                                string connString = node.Attributes.GetNamedItem("Conn").Value;

                                foreach (XmlNode connNode in node.ChildNodes)
                                {
                                    if (connNode.Attributes != null)
                                    {
                                        string queryString;

                                        switch (connNode.Name)
                                        {
                                            case "Queries":
                                                queryString = connNode.Attributes.GetNamedItem("qString").Value;
                                                DelLog(connString, queryString);
                                                break;

                                            case "UserLock":
                                                if (runToday)
                                                {
                                                    queryString = connNode.Attributes.GetNamedItem("qString").Value;
                                                    Regelprozess(connString, queryString);
                                                }
                                                else
                                                {
                                                    Common.WriteLogEntry("Hinweis", "Regelprozess geplant übersprungen.");
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                    
            }
            catch (Exception ex)
            {
                Common.WriteLogEntry("Fehler", ex.Message + ": " + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        private void Regelprozess(string connString, string queryString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = queryString;

                    try
                    {
                        conn.Open();

                        cmd.ExecuteNonQuery();

                        Common.WriteLogEntry("Success", "Regelprozess erfolgreich ausgeführt.");
                    }
                    catch (Exception ex)
                    {
                        Common.WriteLogEntry("Fehler", ex.Message + ": " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                        Common.SendErrorMail(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                Common.WriteLogEntry("Fehler", "Der SQL-Connectionstring konnte nicht ausgelesen werden!");
            }
        }

        private void DelLog(string connString, string queryString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = queryString;
                    cmd.CommandTimeout = 300;

                    try
                    {
                        conn.Open();

                        cmd.ExecuteNonQuery();

                        string strTemp = queryString.Substring(12);
                        string[] tableNames = strTemp.Split(' ');
                        string tableName = (tableNames.Length > 0 ? tableNames[0] : "");

                        Common.WriteLogEntry("Delete", DateTime.Now.ToString() + " - Einträge aus Datenbank: " + conn.Database + ", Tabelle: " + tableName + ", Server:" + conn.DataSource + " gelöscht.");
                    }
                    catch (Exception ex)
                    {
                        Common.WriteLogEntry("Fehler", ex.Message + ": " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                        Common.SendErrorMail(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                Common.WriteLogEntry("Fehler", "Der SQL-Connectionstring konnte nicht ausgelesen werden!");
            }
        }
    }
}
