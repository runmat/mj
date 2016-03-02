using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using SapORM.Models;
using UniversalFileBasedLogging;
using WMDQryCln;

namespace EasyExportGeneralTask
{
    public static class clsQueryClassExtensions
    {
        public static void Configure(this clsQueryClass cls, TaskKonfiguration taskConfig)
        {
            cls.RemoteHosts = Konfiguration.easyRemoteHosts; // IP-Adresse WebLink-Server (NEU)
            int tmpTimeout;
            if (Int32.TryParse(Konfiguration.easyRequestTimeout, out tmpTimeout))
            {
                cls.lngRequestTimeout = tmpTimeout; // Timeout - Zeit
            }
            cls.strSessionID = Konfiguration.easySessionId;
            cls.strBlobPathLocal = taskConfig.easyBlobPathLocal; // Pfad auf dem WebLink-Server (zur Dateiablage)
            cls.strBlobPathRemote = Konfiguration.easyBlobPathRemote; // Lokaler Pfad
            cls.strEASYUser = Konfiguration.easyUser;
            cls.strEASYPwd = Konfiguration.easyPwd;
            cls.strDebugLogPath = Konfiguration.easyLogPath;
            cls.Init(false, false);
        }

        /// <summary>
        /// Daten aus angegebenem Archiv holen
        /// </summary>
        /// <param name="cls"></param>
        /// <param name="archiveName"></param>
        /// <param name="queryExpression"></param>
        /// <param name="totalHits"></param>
        /// <param name="result"></param>
        /// <param name="taskConfig"></param>
        /// <returns></returns>
        public static string QueryArchive(this clsQueryClass cls, string archiveName, string queryExpression, ref object totalHits, ref EasyResult result, TaskKonfiguration taskConfig)
        {
            object dummyLayoutFieldId = null;
            object dummyLayoutId = null;
            object dummyHitlistMultiTiff = null;
            object dummyChannel = null;
            object dummyArchive = null;

            object doc_id = "";
            object doc_ver = "";
            object status = "";
            object strLocFound = "";
            object strArcFound = "";

            int startIndex = result.hitCounter; // alten hitCounter merken

            // Hier kann nicht immer arcLocation als QueryFormat (Parameter 6) gewählt werden, weil z.B. bei Ford je nach Kunde (CSC,FFD) die Sichten NICHT Ford heißen!!!
            var tmpCounter = cls.EASYQueryArchiveInit(taskConfig.easyLocation, archiveName, queryExpression, 8000, 8000, Konfiguration.easyQueryIndexName,
                                                      null, ref dummyLayoutFieldId, ref dummyLayoutId, ref totalHits, ref dummyHitlistMultiTiff, ref status);

            result.hitCounter = Int32.Parse(tmpCounter.ToString()) - 1; // Header abziehen

            if (result.hitCounter == 0)
            {
                status = "Keine Daten gefunden.";
            }

            if (String.IsNullOrEmpty((string)status))
            {
                string HeaderFields = (string)cls.EASYQueryArchiveNext(ref dummyChannel, ref dummyArchive, ref doc_id, ref doc_ver); // Erste Zeile ist Header

                List<string> feldliste = result.CreateHeaderFieldlist(HeaderFields);

                // Hier Tabelle zusammenbauen
                result.AddColumnsToResultTable(feldliste); // Spalten zur Treffertabelle hinzufügen

                for (int i = startIndex; i < (startIndex + result.hitCounter); i++)
                {
                    string tblRow = (string)cls.EASYQueryArchiveNext(ref strLocFound, ref strArcFound, ref doc_id, ref doc_ver); // Nächste Zeile              
                    result.addRowToResultTable(tblRow, feldliste, (string)strLocFound, (string)strArcFound, (string)doc_id, (string)doc_ver);
                }
            }

            return (string)status;
        }

        /// <summary>
        /// Bild aus dem Archiv ziehen zum angegebenen Eintrag
        /// </summary>
        /// <param name="cls"></param>
        /// <param name="result"></param>
        /// <param name="LC"></param>
        /// <param name="logDS"></param>
        /// <param name="logCustomer"></param>
        /// <param name="taskConfig"></param>
        /// <param name="logFiles"></param>
        /// <param name="hitListIndex"></param>
        /// <param name="blnFehlerhafteSaetze"></param>
        /// <param name="additionalData"></param>
        /// <returns></returns>
        public static string QueryPicture(this clsQueryClass cls, ref EasyResult result, ref LoggingClass LC, LogDataset logDS, LogCustomer logCustomer, TaskKonfiguration taskConfig, ref List<LogFile> logFiles, int hitListIndex, bool blnFehlerhafteSaetze = false, object[] additionalData = null)
        {
            object dummyLngNotes = null;
            object dummyFeldId = null;
            object dummyTextLaenge = null;
            object dummyTrefferZahl = null;
            object dummyNciLaenge = null;
            object dummyFeldtitel = null;
            object dummyDateiname = null;
            object dummyStatus = null;
            object dummyLayoutId = null;
            object dummyThumbnailSize = null;

            string returnMessage = "";
            object status = "";
            string strFahrgestellnummer = "";
            string strKennzeichen = "";
            string strTitel = "";
            LogFile lfile;
            object strFilename = "";
            string strBetreff = "";

            try
            {
                // Bilder analysieren
                object ext = "";
                object typ = null;
                int resultfields;
                object fLength = null;
                object iStatus;

                if (result.hitList.Rows.Count <= hitListIndex)
                {
                    return ("HitList enthält keine Daten an Position " + hitListIndex.ToString());
                }

                DataRow row = result.hitList.Rows[hitListIndex];

                string rowLocation = row["DOC_Location"].ToString();
                string rowArchive = row["DOC_Archive"].ToString();
                string rowDocId = row["DOC_ID"].ToString();
                string rowDocVersion = row["DOC_VERSION"].ToString();

                var tmpCounter = cls.EASYGetSegmentDescriptionInit(rowLocation, rowArchive, rowDocId, rowDocVersion, null, ref dummyLngNotes, ref status);
                resultfields = Int32.Parse(tmpCounter.ToString());

                int counter = 0;
                int iIndex;
                for (iIndex = 0; iIndex < resultfields; iIndex++)
                {
                    cls.EASYGetSegmentDescriptionNext(ref dummyFeldId, ref dummyTextLaenge, ref dummyTrefferZahl, ref dummyNciLaenge, ref ext, ref typ,
                        ref dummyFeldtitel, ref dummyDateiname, ref dummyStatus, ref dummyLayoutId, ref dummyThumbnailSize);
                    if (Int32.Parse(typ.ToString()) >= 512) // Bildtyp: größer 512
                    {
                        if (ext.ToString().ToUpper() != "TIF")
                        {
                            break;
                        }

                        counter++;
                    }
                }

                // Bild übertragen (als eine Datei)
                if (ext.ToString().ToUpper() != "TIF")
                {
                    iStatus = cls.EASYGetSegmentData(rowLocation, rowArchive, rowDocId, rowDocVersion, iIndex, ref strFilename, ref fLength, ref status);
                }
                else
                {
                    iStatus = cls.EASYGetSegmentMultiTIF(rowLocation, rowArchive, rowDocId, rowDocVersion, "1-" + counter, ref strFilename, ref fLength, ref status);
                }

                if (iStatus.ToString() == "1")
                {
                    //Bild speichern
                    row["Filepath"] = taskConfig.easyBlobPathLocal + "\\" + (string)strFilename;
                    row["File"] = strFilename;
                    row["FileLength"] = Int32.Parse(fLength.ToString());

                    switch (taskConfig.Ablauf)
                    {
                        case AblaufTyp.Athlon:
                            strFahrgestellnummer = row["FAHRGESTELLNR"].ToString();
                            strTitel = row[".TITEL"].ToString();
                            cls.SavePictureAthlon(ref LC, logDS, ref row, taskConfig, blnFehlerhafteSaetze);
                            break;

                        case AblaufTyp.EuropaService:
                            strFahrgestellnummer = row["FAHRGESTELLNR"].ToString();
                            cls.SavePictureEuropaService(ref LC, logDS, ref row, taskConfig, blnFehlerhafteSaetze);
                            break;

                        case AblaufTyp.StarCar:
                            strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();
                            if (additionalData != null)
                            {
                                strKennzeichen = (additionalData[0] as string);
                            }
                            cls.SavePictureStarCar(ref LC, logDS, ref row, taskConfig, blnFehlerhafteSaetze, strKennzeichen);
                            break;

                        case AblaufTyp.XLeasing:
                            strFahrgestellnummer = row["FIN"].ToString();
                            strTitel = row[".TITEL"].ToString();
                            cls.SavePictureXLeasing(ref LC, logDS, ref row, taskConfig);
                            break;

                        case AblaufTyp.Alphabet:
                            strFahrgestellnummer = row["FAHRGESTELLNR"].ToString();
                            cls.SavePictureAlphabet(ref LC, logDS, ref row, taskConfig);
                            break;

                        case AblaufTyp.LeasePlan:
                            strFahrgestellnummer = row["FAHRGESTELLNR"].ToString();
                            strBetreff = "";
                            if (additionalData != null)
                            {
                                strBetreff = (additionalData[0] as string);
                            }
                            cls.SavePictureLeasePlan(ref LC, logDS, ref row, taskConfig, strBetreff);
                            break;

                        case AblaufTyp.XLCheck:
                            if (additionalData != null)
                            {
                                strTitel = (additionalData[0] as string);
                            }
                            cls.SavePictureXLCheck(ref LC, logDS, ref row, taskConfig, strTitel);
                            break;

                        case AblaufTyp.CharterWay_All:
                            strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();
                            string strMvaNummer = "";
                            if (additionalData != null)
                            {
                                strKennzeichen = (additionalData[0] as string);
                                strMvaNummer = (additionalData[1] as string);
                            }
                            cls.SavePictureCharterWay_All(ref LC, logDS, ref row, taskConfig, strKennzeichen, strMvaNummer);
                            break;

                        case AblaufTyp.CharterWay_Single:
                            strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();
                            if (additionalData != null)
                            {
                                strTitel = (additionalData[0] as string);
                            }
                            cls.SavePictureCharterWay_Single(ref LC, logDS, ref row, taskConfig, strTitel);
                            break;

                        case AblaufTyp.DCBank:
                            strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();
                            strKennzeichen = row["KENNZEICHEN"].ToString();
                            cls.SavePictureDCBank(ref LC, logDS, ref row, taskConfig);
                            break;

                        case AblaufTyp.DaimlerFleet:
                            strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();
                            strKennzeichen = row["KENNZEICHEN"].ToString();
                            cls.SavePictureDaimlerFleet(ref LC, logDS, ref row, taskConfig);
                            break;

                        case AblaufTyp.SixtMobility:
                            strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();
                            strKennzeichen = row["KENNZEICHEN"].ToString();
                            Z_M_EXPORTAENDERUNG_01.GT_WEB dataObjSixtMobility = null;
                            if (additionalData != null)
                            {
                                dataObjSixtMobility = (additionalData[0] as Z_M_EXPORTAENDERUNG_01.GT_WEB);
                            }
                            cls.SavePictureSixtMobility(ref LC, logDS, ref row, taskConfig, dataObjSixtMobility);
                            break;

                        case AblaufTyp.Autoinvest:
                            strFahrgestellnummer = row["FIN"].ToString();
                            cls.SavePictureAutoinvest(ref LC, logDS, ref row, taskConfig);
                            break;

                        case AblaufTyp.Europcar:
                            strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();
                            Z_DPM_AVM_DOKUMENT_MAIL.GT_WEB dataObjEuropcar = null;
                            if (additionalData != null)
                            {
                                dataObjEuropcar = (additionalData[0] as Z_DPM_AVM_DOKUMENT_MAIL.GT_WEB);
                            }
                            cls.SavePictureEuropcar(ref LC, logDS, ref row, taskConfig, dataObjEuropcar);
                            break;

                        case AblaufTyp.WKDA:
                        case AblaufTyp.WKDA_Selbstabmelder:
                            strFahrgestellnummer = row["FIN"].ToString();
                            Z_WFM_UEBERMITTLUNG_STAT_01.GT_OUT dataObjWkda = null;
                            if (additionalData != null)
                            {
                                dataObjWkda = (additionalData[0] as Z_WFM_UEBERMITTLUNG_STAT_01.GT_OUT);
                            }
                            cls.SavePictureWKDA(ref LC, logDS, ref row, taskConfig, dataObjWkda);
                            break;

                        case AblaufTyp.StarCar2:
                            strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();
                            cls.SavePictureStarCar2(ref LC, logDS, ref row, taskConfig);
                            break;
                    }
                }
                else
                {
                    throw new Exception("Fehlerstatus " + iStatus.ToString() + " bei EASYGetSegmentMultiTIF (" + status + ")");
                }

                // Zwangspause damit EasyArchiv genug Zeit hat einen neuen Filename zu generieren
                Thread.Sleep(1000);

                if (blnFehlerhafteSaetze)
                {
                    // Log aktualisieren bei Erfolg
                    lfile = logDS.FindFile(strFahrgestellnummer, strKennzeichen, strTitel);
                    lfile.Finished = true;
                    LC.DeleteXmlEntry(lfile);
                    LC.WriteToXml();
                }
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;

                lfile = logDS.FindFile(strFahrgestellnummer, strKennzeichen, strTitel);
                string ErrorStatus;

                if (ex.GetType() == typeof(IOException))
                {
                    ErrorStatus = "FileAlreadyExist";
                }
                else
                {
                    ErrorStatus = "Unbekannter Fehler";
                }

                if (lfile == null)
                {
                    lfile = new LogFile(strFahrgestellnummer, strKennzeichen, strTitel, logCustomer);
                    lfile.Filename = strFilename.ToString();
                }

                lfile.Trys++;

                string sEntry = "";
                if (!String.IsNullOrEmpty(ex.Message))
                {
                    sEntry += ex.Message + ": ";
                }

                if (ex.InnerException != null)
                {
                    sEntry += ex.InnerException.ToString();
                }

                if (lfile.Trys > 2)
                {
                    logFiles.Add(lfile);
                    sEntry += " Es wurde eine E-Mail an die IT-Entwicklung versandt!";
                }

                // Logeintrag schreiben
                LogEntry newEntry = new LogEntry(ErrorStatus, sEntry, lfile);
                LC.NewXmlEntry(lfile);
                LC.NewXmlEntry(newEntry);
                LC.WriteToXml();
            }

            return returnMessage;
        }

        public static void SavePictureAthlon(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, bool blnFehlerhafteSaetze)
        {
            object iStatus;
            object status = "";

            Console.WriteLine("Wait... for " + row["FAHRGESTELLNR"] + " " + row[".TITEL"]);

            if (blnFehlerhafteSaetze)
            {
                if (row["found"] != DBNull.Value) // Nur Fehlerdateien neu laden
                {
                    if (File.Exists(row["Filepath"].ToString()))
                    {
                        Console.WriteLine(" " + row["File"] + " wird neu geholt.");
                        File.Delete(row["Filepath"].ToString());
                    }

                    Console.WriteLine(" " + row["File"]);

                    // Datei speichern
                    iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

                    if (iStatus.ToString() == "1")
                    {
                        LogFile lfile2 = logDS.FindFile(row["FAHRGESTELLNR"].ToString(), "", row[".TITEL"].ToString());
                        if (lfile2 != null)
                        {
                            lfile2.Finished = true;
                            LC.DeleteXmlEntry(lfile2);
                            LC.WriteToXml();
                        }
                    }
                    else
                    {
                        throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
                    }
                }
                else
                {
                    Console.WriteLine(" übersprungen.");
                }
            }
            else
            {
                if (File.Exists(row["Filepath"].ToString()))
                {
                    Console.WriteLine(" " + row["File"] + " existiert bereits.");
                    throw new IOException(row["File"] + " existiert bereits.");
                }

                // Datei speichern
                iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

                if (iStatus.ToString() != "1")
                {
                    throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
                }
            } 
        }

        public static void SavePictureEuropaService(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, bool blnFehlerhafteSaetze)
        {
            object iStatus;
            object status = "";

            string strFahrgestellnummer = row["FAHRGESTELLNR"].ToString();

            Console.WriteLine("Wait... for " + strFahrgestellnummer);

            if (blnFehlerhafteSaetze)
            {
                if (row["found"] != DBNull.Value) // Nur Fehlerdateien neu laden
                {
                    if (File.Exists(row["Filepath"].ToString()))
                    {
                        Console.WriteLine(" " + row["File"] + " wird neu geholt.");
                        File.Delete(row["Filepath"].ToString());
                    }

                    Console.WriteLine(" " + row["File"]);

                    // Datei speichern
                    iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

                    if (iStatus.ToString() == "1")
                    {
                        LogFile lfile2 = logDS.FindFile(strFahrgestellnummer, "", "");
                        if (lfile2 != null)
                        {
                            lfile2.Finished = true;
                            LC.DeleteXmlEntry(lfile2);
                            LC.WriteToXml();
                        }
                    }
                    else
                    {
                        throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
                    }
                }
                else
                {
                    Console.WriteLine(" übersprungen.");
                    return;
                }
            }
            else
            {
                if (File.Exists(row["Filepath"].ToString()))
                {
                    Console.WriteLine(" " + row["File"] + " existiert bereits.");
                    throw new IOException(row["File"] + " existiert bereits.");
                }

                // Datei speichern
                iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

                if (iStatus.ToString() != "1")
                {
                    throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
                }
            }

            // neuen Namen für Datei vergeben
            string dateValue = DateTime.Today.ToString("yyyyMMdd");

            if (!taskConfig.AbfrageNachDatum)
            {
                string strDatum = row[".ARCHIVDATUM"].ToString();

                dateValue = strDatum.Substring(6, 4) + strDatum.Substring(3, 2) + strDatum.Substring(0, 2);
            }

            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + row[".TITEL"] + "_" + strFahrgestellnummer + "_" + dateValue + ".pdf";
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(row["Filepath"].ToString(), newFilePath);
        }

        public static void SavePictureStarCar(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, bool blnFehlerhafteSaetze, string kennzeichen)
        {
            object iStatus;
            object status = "";

            string strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();

            Console.WriteLine("Wait... for " + kennzeichen);

            if (blnFehlerhafteSaetze)
            {
                if (row["found"] != DBNull.Value) // Nur Fehlerdateien neu laden
                {
                    if (File.Exists(row["Filepath"].ToString()))
                    {
                        Console.WriteLine(" " + row["File"] + " wird neu geholt.");
                        File.Delete(row["Filepath"].ToString());
                    }

                    Console.WriteLine(" " + row["File"]);

                    // Datei speichern
                    iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

                    if (iStatus.ToString() == "1")
                    {
                        LogFile lfile2 = logDS.FindFile(strFahrgestellnummer, kennzeichen, "");
                        if (lfile2 != null)
                        {
                            lfile2.Finished = true;
                            LC.DeleteXmlEntry(lfile2);
                            LC.WriteToXml();
                        }
                    }
                    else
                    {
                        throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
                    }
                }
                else
                {
                    Console.WriteLine(" übersprungen.");
                    return;
                }
            }
            else
            {
                if (File.Exists(row["Filepath"].ToString()))
                {
                    Console.WriteLine(" " + row["File"] + " existiert bereits.");
                    throw new IOException(row["File"] + " existiert bereits.");
                }

                // Datei speichern
                iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

                if (iStatus.ToString() != "1")
                {
                    throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
                }
            }

            // neuen Namen für Datei vergeben
            string dateValue = DateTime.Now.ToString("yyyyMMddHHmmss");

            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + kennzeichen + "_" + strFahrgestellnummer + "_" + dateValue + ".pdf";

            if (File.Exists(newFilePath))
            {
                DateTime newDate = new DateTime(DateTime.Now.Ticks + (1 * 10 ^ 7));
                newFilePath = taskConfig.easyBlobPathLocal + "\\" + kennzeichen + "_" + strFahrgestellnummer + "_" + newDate.ToString("yyyyMMddHHmmss") + ".pdf";
            }

            File.Move(row["Filepath"].ToString(), newFilePath);
        }

        public static void SavePictureXLeasing(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig)
        {
            object iStatus;
            object status = "";

            object dummyLayoutFieldId = null;
            object dummyLayoutId = null;
            object dummyTotalHits = null;
            object dummyHitlistMultiTiff = null;

            string strFahrgestellnummer = row["FIN"].ToString();
            string strTyp = row[".TITEL"].ToString();

            if (strTyp.ToUpper() == "COC")
            {
                return;
            }

            if (strTyp.ToUpper() == "ZB2")
            {
                // Nur Wiedereingänge sollen erzeugt werden
                string querystring = ".1001=" + strFahrgestellnummer + " & .110=" + strTyp;

                var tmpCounter = cls.EASYQueryArchiveInit(taskConfig.easyLocation, taskConfig.easyArchiveNameStandard, querystring, 1000, 1000, Konfiguration.easyQueryIndexName,
                    null, ref dummyLayoutFieldId, ref dummyLayoutId, ref dummyTotalHits, ref dummyHitlistMultiTiff, ref status);
                int resulthits = Int32.Parse(tmpCounter.ToString());

                if (resulthits < 3)
                {
                    return;
                }
            }

            Console.WriteLine("Wait... for " + strFahrgestellnummer + " " + strTyp);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            // neuen Namen für Datei vergeben
            string dateValue = DateTime.Now.ToString("yyyyMMddHHmmss");

            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + dateValue + "_" + row["OBJEKTNUMMER"] + "_" + strTyp + ".pdf";
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(row["Filepath"].ToString(), newFilePath);
        }

        public static void SavePictureAlphabet(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig)
        {
            object iStatus;
            object status = "";

            Console.WriteLine("Wait... for " + row["FAHRGESTELLNR"]);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            string strLeasingvertragsnummer = row[0].ToString();
            string strSubj = "";

            switch (row["MNCOD"].ToString())
            {
                case "ZB2N":
                    strSubj = "N - " + strLeasingvertragsnummer;
                    break;
                case "COC":
                    strSubj = "C - " + strLeasingvertragsnummer;
                    break;
                case "ZB2A":
                    strSubj = "A - " + strLeasingvertragsnummer;
                    break;
                case "ZB1L":
                    strSubj = "L - " + strLeasingvertragsnummer;
                    break;
            }

            Thread.Sleep(2000);

            if (taskConfig.MailsSenden)
            {
                Helper.SendEMail(strSubj, "", row["EMAIL"].ToString(), row["Filepath"].ToString());
                Thread.Sleep(2000);
            }
        }

        public static void SavePictureLeasePlan(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, string betreff)
        {
            object iStatus;
            object status = "";

            Console.WriteLine("Wait... for " + row["FAHRGESTELLNR"]);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            Thread.Sleep(2000);

            if (taskConfig.MailsSenden)
            {
                Helper.SendEMail(betreff, "", taskConfig.MailEmpfaenger, row["Filepath"].ToString());
                Thread.Sleep(2000);
            }
        }

        public static void SavePictureXLCheck(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, string titel)
        {
            object iStatus;
            object status = "";

            Console.WriteLine("Wait... for " + titel);
        
            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            // neuen Namen für Datei vergeben
            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + titel + ".pdf";
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(row["Filepath"].ToString(), newFilePath);
        }

        public static void SavePictureCharterWay_All(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, string kennzeichen, string mvanummer)
        {
            object iStatus;
            object status = "";

            string strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            // neuen Namen für Datei vergeben
            string newFilePath;

            Console.WriteLine("Wait... for " + kennzeichen);

            if (!String.IsNullOrEmpty(mvanummer))
            {
                newFilePath = taskConfig.easyBlobPathLocal + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + kennzeichen + "_" + strFahrgestellnummer + "_" + mvanummer + "_" + row[".TITEL"] + ".pdf";

                if (File.Exists(newFilePath))
                {
                    DateTime newDate = new DateTime(DateTime.Now.Ticks + (1 * 10 ^ 7));
                    newFilePath = taskConfig.easyBlobPathLocal + "\\" + newDate.ToString("yyyyMMddHHmmss") + "_" + kennzeichen + "_" + strFahrgestellnummer + "_" + mvanummer + "_" + row[".TITEL"] + ".pdf";
                }
            }
            else
            {
                newFilePath = taskConfig.easyBlobPathLocal + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + kennzeichen + "_" + strFahrgestellnummer + "_" + row[".TITEL"] + ".pdf";

                if (File.Exists(newFilePath))
                {
                    DateTime newDate = new DateTime(DateTime.Now.Ticks + (1 * 10 ^ 7));
                    newFilePath = taskConfig.easyBlobPathLocal + "\\" + newDate.ToString("yyyyMMddHHmmss") + "_" + kennzeichen + "_" + strFahrgestellnummer + "_" + row[".TITEL"] + ".pdf";
                }
            }

            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(row["Filepath"].ToString(), newFilePath);
        }

        public static void SavePictureCharterWay_Single(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, string titel)
        {
            object iStatus;
            object status = "";

            Console.WriteLine("Wait... for " + row["FAHRGESTELLNUMMER"]);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            // neuen Namen für Datei vergeben
            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + row["FAHRGESTELLNUMMER"] + "_" + titel + ".pdf";

            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(row["Filepath"].ToString(), newFilePath);
        }

        public static void SavePictureDCBank(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig)
        {
            object iStatus;
            object status = "";

            string strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();

            Console.WriteLine("Wait... for " + strFahrgestellnummer);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            // neuen Namen für Datei vergeben
            string dateValue = DateTime.Today.ToString("yyyyMMdd");

            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + strFahrgestellnummer + "_" + row[".TITEL"] + "_" + dateValue + ".pdf";
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(row["Filepath"].ToString(), newFilePath);
        }

        public static void SavePictureDaimlerFleet(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig)
        {
            object iStatus;
            object status = "";

            string strFahrgestellnummer = row["FAHRGESTELLNUMMER"].ToString();

            Console.WriteLine("Wait... for " + strFahrgestellnummer);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            // neuen Namen für Datei vergeben
            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + strFahrgestellnummer + ".pdf";
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(row["Filepath"].ToString(), newFilePath);
        }

        public static void SavePictureSixtMobility(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, Z_M_EXPORTAENDERUNG_01.GT_WEB item)
        {
            object iStatus;
            object status = "";

            Console.WriteLine("Wait... for " + row["FAHRGESTELLNUMMER"]);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            // neuen Namen für Datei vergeben
            var newFileName = String.Format("DAD {0} Kopie {1} {2} {3}", (item.MNCOD == "COC" ? "COC" : "ZBII"), item.ZZFABRIKNAME, item.ZZFAHRG, DateTime.Now.ToShortDateString());
            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + newFileName + ".pdf";
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(row["Filepath"].ToString(), newFilePath);

            Thread.Sleep(2000);

            if (taskConfig.MailsSenden)
            {
                var mailBetreff = String.Format("DAD {0} Kopie {1} {2}", (item.MNCOD == "COC" ? "COC" : "ZBII"), item.ZZFABRIKNAME, DateTime.Now.ToShortDateString());
                var mailText = String.Join(Environment.NewLine, new[]
                    {
                        "Sehr geehrte Damen und Herren,",
                        "anbei erhalten Sie optisch archivierte Dokumente Ihrer Fahrzeuge.",
                        "Rückfragen richten Sie bitte an den Sie betreuenden Zulassungsdienst."
                    });

                Helper.SendEMail(mailBetreff, mailText, taskConfig.MailEmpfaenger, newFilePath);
                Thread.Sleep(2000);
            }
        }

        public static void SavePictureAutoinvest(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig)
        {
            object iStatus;
            object status = "";

            string strFahrgestellnummer = row["FIN"].ToString();
            string strDokumententyp = row[".TITEL"].ToString();

            Console.WriteLine("Wait... for " + strFahrgestellnummer);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            // neuen Namen für Datei vergeben
            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + strFahrgestellnummer + "-" + strDokumententyp + ".pdf";
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            File.Move(row["Filepath"].ToString(), newFilePath);

            Thread.Sleep(2000);

            if (taskConfig.MailsSenden)
            {
                var mailBetreff = String.Format("{0} - {1}", strFahrgestellnummer, strDokumententyp);
                var mailText = String.Join(Environment.NewLine, new[]
                    {
                        "Sehr geehrte Damen und Herren,",
                        "anbei erhalten Sie wie besprochen die in der Anlage enthaltenen Dokumente."
                    });

                Helper.SendEMail(mailBetreff, mailText, taskConfig.MailEmpfaenger, newFilePath);
                Thread.Sleep(2000);
            }
        }

        public static void SavePictureEuropcar(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, Z_DPM_AVM_DOKUMENT_MAIL.GT_WEB item)
        {
            object iStatus;
            object status = "";

            Console.WriteLine("Wait... for " + row["FAHRGESTELLNUMMER"]);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
            {
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");
            }

            string strSubj = String.Format("Dokumentenkopie {0} {1}", item.DOK_TYP, item.CHASSIS_NUM);

            Thread.Sleep(2000);

            if (taskConfig.MailsSenden)
            {
                Helper.SendEMail(strSubj, "", item.EMAIL, row["Filepath"].ToString());
                Thread.Sleep(2000);
            }
        }

        public static void SavePictureWKDA(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig, Z_WFM_UEBERMITTLUNG_STAT_01.GT_OUT item)
        {
            object iStatus;
            object status = "";

            Console.WriteLine("Wait... for " + row["FIN"]);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");

            // neuen Namen für Datei vergeben
            string newFilePath = taskConfig.easyBlobPathLocal + "\\" + item.REFERENZ1 + "_" + DateTime.Now.ToShortDateString() + "_" + item.NAME1 + ".pdf";
            if (File.Exists(newFilePath))
                File.Delete(newFilePath);

            File.Move(row["Filepath"].ToString(), newFilePath);

            Thread.Sleep(2000);

            // ursprüngliche Datei löschen
            File.Delete(row["Filepath"].ToString());
        }

        public static void SavePictureStarCar2(this clsQueryClass cls, ref LoggingClass LC, LogDataset logDS, ref DataRow row, TaskKonfiguration taskConfig)
        {
            object iStatus;
            object status = "";

            Console.WriteLine("Wait... for " + row["FAHRGESTELLNUMMER"]);

            if (File.Exists(row["Filepath"].ToString()))
            {
                Console.WriteLine(" " + row["File"] + " existiert bereits.");
                throw new IOException(row["File"] + " existiert bereits.");
            }

            // Datei speichern
            iStatus = cls.EASYTransferBLOB(row["File"], row["FileLength"], ref status);

            if (iStatus.ToString() != "1")
                throw new Exception("Fehlerstatus " + iStatus + " bei Dateidownload aus EasyArchiv (" + status + ")");

            Thread.Sleep(2000);
        }
    }
}
