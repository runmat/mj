using System;
using WMDQryCln;
using System.IO;

namespace QuickEasy
{
    /// <summary>
    /// EasyArchiv Dokumenten Klasse
    /// </summary>
    public class Documents
    {

       private string mQueryString {get; set;}
       private string mRemoteHosts {get; set;}
       private int mEasyRequestTimeout { get; set; }
       private string mEasySessionId {get; set;}
       private string mEasyBlobPathLocal { get; set; }
       private string mEasyBlobPathRemote { get; set; }
       private string mEasyUser { get; set; }
       private string mEasyPwdClear { get; set; }
       private string mEasyLogPath { get; set; }
       private string mEasyLagerortName { get; set; }
       private string mEasyArchivName { get; set; }
       private string mEasyQueryIndexName { get; set; }
       private string mPath = string.Empty;
       private string[] mPaths = new string[0];
       private int mStatus = 0;
       //private string[] mDokumentenarten;

         public string path
        {
            get { return mPath;}
        }

         public string[] Paths 
         {
             get { return mPaths; }
         }

         //public string[] Dokumentenarten
         //{
         //    get { return mDokumentenarten; }
         //}
         /// <summary>
         /// 0 = Es konnte kein Dokument ermittelt werden
         /// 1 = Es ist ein Fehler aufgetreten
         /// 2 = Erfolg
         /// </summary>
         public int ReturnStatus
         {
             get { return mStatus; }
         }

        /// <summary>
        /// Erzeugt ein neues Documents-Objekt
        /// </summary>
        /// <param name="QueryString">Abfrage-String</param>
         /// <param name="RemoteHosts">RemoteHosts</param>
        /// <param name="EasyRequestTimeout">Timeoutzeit</param>
        /// <param name="EasySessionId">SessionID</param>
        /// <param name="EasyBlobPathLocal">Downloadpfad lokal</param>
         /// <param name="EasyBlobPathRemote">Downloadpfad Remote</param>
        /// <param name="EasyUser">Anmeldename</param>
        /// <param name="EasyPwdClear">Passwort in Klarschrift</param>
        /// <param name="EasyLogPath">Log-Pfad</param>
        /// <param name="EasyLagerortName">Lagerort</param>
        /// <param name="EasyArchivName">Archiv</param>
        /// <param name="EasyQueryIndexName">Index</param>
       public Documents(string QueryString,
                        string RemoteHosts,
                        int EasyRequestTimeout,
                        string EasySessionId,
                        string EasyBlobPathLocal,
                        string EasyBlobPathRemote,
                        string EasyUser,
                        string EasyPwdClear,
                        string EasyLogPath,
                        string EasyLagerortName,
                        string EasyArchivName,
                        string EasyQueryIndexName)
        {
             mQueryString = QueryString;
             mRemoteHosts = RemoteHosts;
             mEasyRequestTimeout = EasyRequestTimeout;
             mEasySessionId = EasySessionId;
             mEasyBlobPathLocal = EasyBlobPathLocal;
             mEasyBlobPathRemote = EasyBlobPathRemote;
             mEasyUser = EasyUser;
             mEasyPwdClear = EasyPwdClear;
             mEasyLogPath = EasyLogPath;
             mEasyLagerortName = EasyLagerortName;
             mEasyArchivName = EasyArchivName;
             mEasyQueryIndexName = EasyQueryIndexName;
        }

        /// <summary>
        /// Liefert das erste Dokument zu den Suchkriterien
        /// </summary>
       public void GetDocument()
       {

           try
           {

                WMDQryCln.clsQuery Weblink = new WMDQryCln.clsQuery();
              
                Weblink.RemoteHosts = mRemoteHosts;
                Weblink.lngRequestTimeout = mEasyRequestTimeout;
                Weblink.strSessionID = mEasySessionId;
                Weblink.strBlobPathLocal = mEasyBlobPathLocal;
                Weblink.strBlobPathRemote = mEasyBlobPathRemote;
                Weblink.strEASYUser = mEasyUser;
                Weblink.strEASYPwd = mEasyPwdClear;
                Weblink.strDebugLogPath = mEasyLogPath;
                Weblink.Init(false, false);

                int index;
                object total_hits = null;
                object status = null;

                object obj1 = null;
                object obj2 = null;
                object obj3 = null;
                object obj4 = null;


                object resulthits = Weblink.EASYQueryArchiveInit(mEasyLagerortName, mEasyArchivName, mQueryString, 100, 100, mEasyQueryIndexName,obj1 ,ref obj2,ref obj3,ref total_hits,ref obj4,ref status);


                Int32 int_resulthits = Convert.ToInt32(resulthits);

                string[,] strResults = new string[21, 2];
                strResults = new string[21, int_resulthits];

                object doc_id = null;
                object doc_ver = null;
                object tblHeader = null;

                object EasyLagerortName = (object)mEasyLagerortName;
                object EasyArchivName = (object)mEasyArchivName;

                tblHeader = Weblink.EASYQueryArchiveNext(ref EasyLagerortName, ref EasyArchivName, ref doc_id, ref doc_ver);
                //Erste Zeile ist Header

                //Zeilen lesen
                string[] strCols = null;
                object tblRow = null;
                int typ = 0;
                Int32 stat = default(Int32);
                string strfilename = string.Empty;


                if (int_resulthits > 1)
                {
               tblRow = Weblink.EASYQueryArchiveNext(ref EasyLagerortName, ref EasyArchivName, ref doc_id, ref doc_ver);
               //Nächste Zeile              

               string StringRow = Convert.ToString(tblRow);


               StringRow = StringRow.Replace("^", "");

               //Indexdatei-Felder sammeln
               strCols = StringRow.Split(',');

               //0	.Mappe
               //1	.Version
               //2	.Archivdatum
               //3	.Änderungsdatum
               //4	.Felder gesamt
               //5	.Textfelder
               //6	.Bilder
               //7	.Titel
               //8	.Typ
               //9	.FileType
               //10	.FormName
               //11	.WorkBoxName
               //12	.BatchName
               //13	Fahrgestellnummer   
               //14	Leasingvertragsnumme
               //15	Kennzeichen
               //16	Zulassungsdatum

               for (int j = 0; j <= strCols.GetUpperBound(0); j++)
               {
                   strResults[j, 0] = strCols[j];
               }
           }
           else
           {
               return;
           }



           //for (int i = 1; i <= int_resulthits - 1; i++)
           //{
           //    tblRow = Weblink.EASYQueryArchiveNext(ref EasyLagerortName, ref EasyArchivName, ref doc_id, ref doc_ver);
           //    //Nächste Zeile              

           //    string StringRow = Convert.ToString(tblRow);


           //    StringRow = StringRow.Replace("^", "");

           //    //Indexdatei-Felder sammeln
           //    strCols = StringRow.Split(',');

           //    //0	.Mappe
           //    //1	.Version
           //    //2	.Archivdatum
           //    //3	.Änderungsdatum
           //    //4	.Felder gesamt
           //    //5	.Textfelder
           //    //6	.Bilder
           //    //7	.Titel
           //    //8	.Typ
           //    //9	.FileType
           //    //10	.FormName
           //    //11	.WorkBoxName
           //    //12	.BatchName
           //    //13	Fahrgestellnummer   
           //    //14	Leasingvertragsnumme
           //    //15	Kennzeichen
           //    //16	Zulassungsdatum

           //    for (int j = 0; j <= strCols.GetUpperBound(0); j++)
           //    {
           //        strResults[j, i - 1] = strCols[j];
           //    }

             

           //}

           int e = 0;


           if (!string.IsNullOrEmpty(strResults[0, e])) {
	        
	        object resultfields = Weblink.EASYGetSegmentDescriptionInit(EasyLagerortName, EasyArchivName, doc_id, doc_ver,obj1 ,ref obj2 ,ref status);

            object Extension = null;
            object Name = null;
            object obj6 = null;
            object obj7 = null;
            object obj8 = null;
            object objTyp = (object)typ;
            object objStat = (object)stat;

	        Int32 counter = 0;


	        //Feldnamen abfragen    'Einzelne Felder durchgehen und nach Bildern suchen...
	        for (index = 0; index <= Convert.ToInt32(resultfields) - 1; index++) {
                Weblink.EASYGetSegmentDescriptionNext(ref obj1, ref obj2, ref obj3, ref obj4, ref Extension, ref objTyp, ref obj6, ref Name, ref objStat, ref obj7, ref obj8);
		        //Bildtyp: größer 512
		        if ((Convert.ToInt32(objTyp) >= 512)) {
			        if (Extension.ToString().ToUpper() != "TIF") {
				        break; // TODO: might not be correct. Was : Exit For
			        } else {
				        counter += 1;
			        }

		        }

	        }


            object objFilename = null;
            object objfLength = null;
            object objIndex = (object)index;
           
            object intStatus;


            if (Extension.ToString().ToUpper() != "TIF")
            {
                intStatus = Weblink.EASYGetSegmentData(EasyLagerortName, EasyArchivName, doc_id, doc_ver, index, ref objFilename, ref objfLength, ref status);
	        } else {
                intStatus = Weblink.EASYGetSegmentMultiTIF(EasyLagerortName, EasyArchivName, doc_id, doc_ver, "1-" + counter.ToString(), ref objFilename, ref objfLength,ref status);
	        }


	        string str_filename = mEasyBlobPathLocal + Convert.ToString(objFilename);

               if (File.Exists(str_filename))
               {
                   File.Delete(str_filename);
                   
               }

	        //Dateien schreiben
            Weblink.EASYTransferBLOB(str_filename, objfLength, ref status);

            mPath = str_filename;
            mStatus = 2;

        }


           }
           catch (Exception)
           {

               mStatus = 1;
           }


       }

        /// <summary>
        /// Liefert alle Dokumente zu den Suchkriterien
        /// </summary>
       public void GetDocuments()
       {

           try
           {

               WMDQryCln.clsQuery Weblink = new WMDQryCln.clsQuery();

               Weblink.RemoteHosts = mRemoteHosts;
               Weblink.lngRequestTimeout = mEasyRequestTimeout;
               Weblink.strSessionID = mEasySessionId;
               Weblink.strBlobPathLocal = mEasyBlobPathLocal;
               Weblink.strBlobPathRemote = mEasyBlobPathRemote;
               Weblink.strEASYUser = mEasyUser;
               Weblink.strEASYPwd = mEasyPwdClear;
               Weblink.strDebugLogPath = mEasyLogPath;
               Weblink.Init(false, false);

               int index;
               object total_hits = null;
               object status = null;

               object obj1 = null;
               object obj2 = null;
               object obj3 = null;
               object obj4 = null;


               object resulthits = Weblink.EASYQueryArchiveInit(mEasyLagerortName, mEasyArchivName, mQueryString, 100, 100, mEasyQueryIndexName, obj1, ref obj2, ref obj3, ref total_hits, ref obj4, ref status);

               Int32 int_resulthits = Convert.ToInt32(resulthits);

               string[,] strResults = new string[21, 2];
               strResults = new string[21, int_resulthits];

               object doc_id = null;
               object doc_ver = null;
               object tblHeader = null;

               object EasyLagerortName = (object)mEasyLagerortName;
               object EasyArchivName = (object)mEasyArchivName;

               tblHeader = Weblink.EASYQueryArchiveNext(ref EasyLagerortName, ref EasyArchivName, ref doc_id, ref doc_ver);
               //Erste Zeile ist Header

               //Zeilen lesen
               string[] strCols = null;
               object tblRow = null;
               //int typ = 0;
               //Int32 stat = default(Int32);
               string strfilename = string.Empty;

               string[,] docIDs = new string[int_resulthits-1,2];               

               if (int_resulthits > 1)
               {
                   for (int i = 1; i <= strResults.GetUpperBound(1); i++)
                   {
                       tblRow = Weblink.EASYQueryArchiveNext(ref EasyLagerortName, ref EasyArchivName, ref doc_id, ref doc_ver);                      
                       //Nächste Zeile              

                       docIDs[i - 1, 0] = (string)doc_id;
                       docIDs[i - 1, 1] = (string)doc_ver;

                       string StringRow = Convert.ToString(tblRow);

                       StringRow = StringRow.Replace("^", "");

                       //Indexdatei-Felder sammeln
                       strCols = StringRow.Split(',');

                       //0	.Mappe
                       //1	.Version
                       //2	.Archivdatum
                       //3	.Änderungsdatum
                       //4	.Felder gesamt
                       //5	.Textfelder
                       //6	.Bilder
                       //7	.Titel
                       //8	.Typ
                       //9	.FileType
                       //10	.FormName
                       //11	.WorkBoxName
                       //12	.BatchName
                       //13	Fahrgestellnummer   
                       //14	Leasingvertragsnumme
                       //15	Kennzeichen
                       //16	Zulassungsdatum

                       for (int j = 0; j <= strCols.GetUpperBound(0); j++)
                       {
                           strResults[j, i-1] = strCols[j];
                       }

                   }
               }
               else
               {
                   return;
               }

               
               if (!string.IsNullOrEmpty(strResults[0, 0])) 
               {
                  //mDokumentenarten = new string[docIDs.GetLength(0)];
                  mPaths = new string[docIDs.GetLength(0)];

                  for (int i = 0; i <= docIDs.GetUpperBound(0); i++)
                   {
                       doc_id = docIDs[i, 0];
                       doc_ver = docIDs[i, 1];

                       obj1 = null;
                       obj2 = null;
                       status = null;

                       object resultfields = Weblink.EASYGetSegmentDescriptionInit(EasyLagerortName, EasyArchivName, doc_id, doc_ver, obj1, ref obj2, ref status);

                       object Extension = null;
                       object Name = null;
                       object Dokumentenart = null;
                       object obj7 = null;
                       object obj8 = null;
                       object objTyp = null; // (object)typ;
                       object objStat = null; //(object)stat;

                       //Int32 counter = 0;

                       bool bFoundDokArt = false;
                       bool bFoundExtension = false;

                       //Feldnamen abfragen    'Einzelne Felder durchgehen und nach Bildern suchen...
                      for (index = 0; index <= Convert.ToInt32(resultfields) - 1; index++)
                      {
                           object Description = Weblink.EASYGetSegmentDescriptionNext(ref obj1, ref obj2, ref obj3, ref obj4, ref Extension, ref objTyp, ref Dokumentenart, ref Name, ref objStat, ref obj7, ref obj8);

                           if (!bFoundDokArt)
                           {
                               if (Dokumentenart != null && (docIDs[i,3] == null || docIDs[i,3] == ""))
                               {
                                   docIDs[i,3] = Convert.ToString(Dokumentenart);
                                   bFoundDokArt = true;
                               }
                           }

                           if (!bFoundExtension)
                           {
                                //Bildtyp: größer 512
                                if ((Convert.ToInt32(objTyp) >= 512))
                                {
                                    docIDs[i, 2] = (string)Extension;
                                    bFoundExtension = true;
                                }
                               //if (Extension.ToString().ToUpper() != "TIF")
                               //{
                               //    break; // TODO: might not be correct. Was : Exit For
                               //}
                               //else
                               //{
                               //    counter += 1;
                               //}
                           }

                           if (bFoundDokArt && bFoundExtension)
                           {
                                /* Extension gefüllt und Dokumentenart gefunden -> Schleife verlassen */
                                break;
                           }
                       }

                   object objFilename = null;
                   object objfLength = null;
                   object objIndex = null; //(object)index;

                   object intStatus;



                    if (Extension != null && Extension.ToString() != "")
                    {

                        if (Extension.ToString().ToUpper() != "TIF")
                        {
                           intStatus = Weblink.EASYGetSegmentData(EasyLagerortName, EasyArchivName, doc_id, doc_ver, objIndex, ref objFilename, ref objfLength, ref status);
                        }
                        else
                        {
                           intStatus = Weblink.EASYGetSegmentMultiTIF(EasyLagerortName, EasyArchivName, doc_id, doc_ver, objIndex, ref objFilename, ref objfLength, ref status);
                        }
                        //"1-" + counter.ToString()

                        string str_filename = mEasyBlobPathLocal + Convert.ToString(objFilename);
                        //Dateien schreiben
                        Weblink.EASYTransferBLOB(str_filename, objfLength, ref status);

                        mPaths[i] = str_filename;
                        mStatus = 2;
                        }
                   }
               }

           }
           catch (Exception)
           {

               mStatus = 1;
           }


       }

       ///// <summary>
       ///// Liefert alle Dokumente zu den Suchkriterien
       ///// </summary>
       //public void GetDocumentsByType(string DocType)
       //{

       //    try
       //    {

       //        WMDQryCln.clsQuery Weblink = new WMDQryCln.clsQuery();

       //        Weblink.RemoteHosts = mRemoteHosts;
       //        Weblink.lngRequestTimeout = mEasyRequestTimeout;
       //        Weblink.strSessionID = mEasySessionId;
       //        Weblink.strBlobPathLocal = mEasyBlobPathLocal;
       //        Weblink.strBlobPathRemote = mEasyBlobPathRemote;
       //        Weblink.strEASYUser = mEasyUser;
       //        Weblink.strEASYPwd = mEasyPwdClear;
       //        Weblink.strDebugLogPath = mEasyLogPath;
       //        Weblink.Init(false, false);

               
       //        object total_hits = null;
       //        object status = null;

       //        object obj1 = null;
       //        object obj2 = null;
       //        object obj3 = null;
       //        object obj4 = null;


       //        object resulthits = Weblink.EASYQueryArchiveInit(mEasyLagerortName, mEasyArchivName, mQueryString, 100, 100, mEasyQueryIndexName, obj1, ref obj2, ref obj3, ref total_hits, ref obj4, ref status);

       //        Int32 int_resulthits = Convert.ToInt32(resulthits);

       //        string[,] strResults = new string[21, 2];
       //        strResults = new string[21, int_resulthits];

       //        object doc_id = null;
       //        object doc_ver = null;
       //        object tblHeader = null;

       //        object EasyLagerortName = (object)mEasyLagerortName;
       //        object EasyArchivName = (object)mEasyArchivName;

       //        tblHeader = Weblink.EASYQueryArchiveNext(ref EasyLagerortName, ref EasyArchivName, ref doc_id, ref doc_ver);
       //        //Erste Zeile ist Header

       //        //Zeilen lesen
       //        string[] strCols = null;
       //        object tblRow = null;
       //        //int typ = 0;
       //        //Int32 stat = default(Int32);
       //        string strfilename = string.Empty;

       //        object[,] docIDs = new string[int_resulthits - 1, 2];

       //        if (int_resulthits > 1)
       //        {
       //            for (int i = 1; i <= strResults.GetUpperBound(1); i++)
       //            {
       //                tblRow = Weblink.EASYQueryArchiveNext(ref EasyLagerortName, ref EasyArchivName, ref doc_id, ref doc_ver);
       //                //Nächste Zeile              

       //                docIDs[i - 1, 0] = doc_id;
       //                docIDs[i - 1, 1] = doc_ver;

       //                string StringRow = Convert.ToString(tblRow);

       //                StringRow = StringRow.Replace("^", "");

       //                //Indexdatei-Felder sammeln
       //                strCols = StringRow.Split(',');

       //                //0	.Mappe
       //                //1	.Version
       //                //2	.Archivdatum
       //                //3	.Änderungsdatum
       //                //4	.Felder gesamt
       //                //5	.Textfelder
       //                //6	.Bilder
       //                //7	.Titel
       //                //8	.Typ
       //                //9	.FileType
       //                //10	.FormName
       //                //11	.WorkBoxName
       //                //12	.BatchName
       //                //13	Fahrgestellnummer   
       //                //14	Leasingvertragsnumme
       //                //15	Kennzeichen
       //                //16	Zulassungsdatum

       //                for (int j = 0; j <= strCols.GetUpperBound(0); j++)
       //                {
       //                    strResults[j, i - 1] = strCols[j];
       //                }

       //            }
       //        }
       //        else
       //        {
       //            return;
       //        }


       //         if (!string.IsNullOrEmpty(strResults[0, 0]))
       //         {
       //             mDokumentenarten = new string[docIDs.GetLength(0)];
       //             mPaths = new string[docIDs.GetLength(0)];

              
       //             for (int i = 0; i <= docIDs.GetUpperBound(0); i++)
       //             {
       //                 DetailHelper(docIDs, i, ref Weblink, ref EasyLagerortName, ref EasyArchivName, ref DocType);
       //             }
       //        }

       //    }
       //    catch (Exception)
       //    {

       //        mStatus = 1;
       //    }


       //}

       /// <summary>
       /// Ruft die Feldbezeichnungen und strukturierten Daten eines Archives ab. 
       /// </summary>
       /// <param name="docIDs">Dokumenten ID</param>
       /// <param name="ind">index</param>
       /// <param name="Weblink">Schnittestellen Objekt</param>
       /// <param name="EasyLagerortName">EasyLagerort</param>
       /// <param name="EasyArchivName">EasyArchiv</param>
       /// <param name="DocType">zu filternde Dokumentenart</param>
       private void DetailHelper(string[,] docIDs, int ind, ref clsQuery Weblink, ref object EasyLagerortName, ref object EasyArchivName, ref string DocType)
       {
           object doc_id = docIDs[ind, 0];
           object doc_ver = docIDs[ind, 1];

           object obj1 = null;
           object obj2 = null;
           object obj3 = null;
           object obj4 = null;
           object status = null;
           object Extension = null;
           object Name = null;
           object Dokumentenart = null;
           object obj7 = null;
           object obj8 = null;
           object objTyp = null;
           object objStat = null;

           object resultfields = Weblink.EASYGetSegmentDescriptionInit(EasyLagerortName, EasyArchivName, doc_id, doc_ver, obj1, ref obj2, ref status);
           
           bool bFoundDokArt = false;
           bool bFoundExtension = false;

           int index;
           //Feldnamen abfragen    'Einzelne Felder durchgehen und nach Bildern suchen...
           for (index = 0; index <= Convert.ToInt32(resultfields) - 1; index++)
           {
               object Description = Weblink.EASYGetSegmentDescriptionNext(ref obj1, ref obj2, ref obj3, ref obj4, ref Extension, ref objTyp, ref Dokumentenart, ref Name, ref objStat, ref obj7, ref obj8);

               if (!bFoundDokArt)
               {
                   if (Dokumentenart != null && (docIDs[ind, 3] == null || docIDs[ind, 3] == ""))
                   {
                       docIDs[ind, 3] = Convert.ToString(Dokumentenart);
                       bFoundDokArt = true;
                   }
               }

               if (!bFoundExtension)
               {
                   //Bildtyp: größer 512
                   if ((Convert.ToInt32(objTyp) >= 512))
                   {
                       docIDs[ind, 2] = (string)Extension;
                       bFoundExtension = true;
                   }
               }

               if (bFoundDokArt && bFoundExtension)
               {
                   if (docIDs[ind, 3] != DocType)
                   {
                       return;
                   }

                   /* Extension gefüllt und Dokumentenart gefunden -> Schleife verlassen */
                   break;
               }
           }

           object objFilename = null;
           object objfLength = null;
           object objIndex = null;
           object intStatus = null;


           if (Extension != null && Extension.ToString() != "")
           {

               if (Extension.ToString().ToUpper() != "TIF")
               {
                   intStatus = Weblink.EASYGetSegmentData(EasyLagerortName, EasyArchivName, doc_id, doc_ver, objIndex, ref objFilename, ref objfLength, ref status);
               }
               else
               {
                   intStatus = Weblink.EASYGetSegmentMultiTIF(EasyLagerortName, EasyArchivName, doc_id, doc_ver, objIndex, ref objFilename, ref objfLength, ref status);
               }

               string str_filename = mEasyBlobPathLocal + Convert.ToString(objFilename);
               //Dateien schreiben
               Weblink.EASYTransferBLOB(str_filename, objfLength, ref status);

               mPaths[ind] = str_filename;
               mStatus = 2;
           }
       }

       /// <summary>
       /// Ruft die Feldbezeichnungen und strukturierten Daten eines Archives ab. 
       /// </summary>
       /// <param name="docIDs">Dokumenten ID</param>
       /// <param name="ind">index</param>
       /// <param name="Weblink">Schnittestellen Objekt</param>
       /// <param name="EasyLagerortName">EasyLagerort</param>
       /// <param name="EasyArchivName">EasyArchiv</param>
       private void DetailHelper(string[,] docIDs, int ind, ref clsQuery Weblink, ref object EasyLagerortName, ref object EasyArchivName)
       {
           object doc_id = docIDs[ind, 0];
           object doc_ver = docIDs[ind, 1];

           object obj1 = null;
           object obj2 = null;
           object obj3 = null;
           object obj4 = null;
           object status = null;
           object Extension = null;
           object Name = null;
           object Dokumentenart = null;
           object obj7 = null;
           object obj8 = null;
           object objTyp = null;
           object objStat = null;

           object resultfields = Weblink.EASYGetSegmentDescriptionInit(EasyLagerortName, EasyArchivName, doc_id, doc_ver, obj1, ref obj2, ref status);

           bool bFoundDokArt = false;
           bool bFoundExtension = false;

           int index;
           //Feldnamen abfragen    'Einzelne Felder durchgehen und nach Bildern suchen...
           for (index = 0; index <= Convert.ToInt32(resultfields) - 1; index++)
           {
               object Description = Weblink.EASYGetSegmentDescriptionNext(ref obj1, ref obj2, ref obj3, ref obj4, ref Extension, ref objTyp, ref Dokumentenart, ref Name, ref objStat, ref obj7, ref obj8);

               if (!bFoundDokArt)
               {
                   if (Dokumentenart != null && (docIDs[ind, 3] == null || docIDs[ind, 3] == ""))
                   {
                       docIDs[ind, 3] = Convert.ToString(Dokumentenart);
                       bFoundDokArt = true;
                   }
               }

               if (!bFoundExtension)
               {
                   //Bildtyp: größer 512
                   if ((Convert.ToInt32(objTyp) >= 512))
                   {
                       docIDs[ind, 2] = (string)Extension;
                       bFoundExtension = true;
                   }
               }

               if (bFoundDokArt && bFoundExtension)
               {
                   //if (mDokumentenarten[ind] != DocType)
                   //{
                   //    return;
                   //}

                   /* Extension gefüllt und Dokumentenart gefunden -> Schleife verlassen */
                   break;
               }
           }
       }

        /// <summary>
        /// Lädt ein Dokument mit den angegebenen Parametern aus dem EasyArchiv herunter und gibt den Downloadpfad zurück
        /// </summary>
        /// <param name="Extension">Dateityp</param>
        /// <param name="strDoc_id">Dokumenten ID</param>
        /// <param name="strDoc_ver">Dokumenten Version</param>
        /// <returns>Downloadpath</returns>
       public string DownloadSingleFile(string Extension,string strDoc_id,string strDoc_ver)
       {
            try
            {

                WMDQryCln.clsQuery Weblink = new WMDQryCln.clsQuery();

                Weblink.RemoteHosts = mRemoteHosts;
                Weblink.lngRequestTimeout = mEasyRequestTimeout;
                Weblink.strSessionID = mEasySessionId;
                Weblink.strBlobPathLocal = mEasyBlobPathLocal;
                Weblink.strBlobPathRemote = mEasyBlobPathRemote;
                Weblink.strEASYUser = mEasyUser;
                Weblink.strEASYPwd = mEasyPwdClear;
                Weblink.strDebugLogPath = mEasyLogPath;
                Weblink.Init(false, false);

                object objFilename = null;
                object objfLength = null;
                object objIndex = null;
                object intStatus = null;
                object EasyLagerortName = (object)mEasyLagerortName;
                object EasyArchivName = (object)mEasyArchivName;
                object doc_id = (object)strDoc_id;
                object doc_ver = (object)strDoc_ver;
                object status = null;

                if (Extension != null && Extension.ToString() != "")
                {

                    if (Extension.ToString().ToUpper() != "TIF")
                    {
                        intStatus = Weblink.EASYGetSegmentData(EasyLagerortName, EasyArchivName, doc_id, doc_ver, objIndex, ref objFilename, ref objfLength, ref status);
                    }
                    else
                    {
                        intStatus = Weblink.EASYGetSegmentMultiTIF(EasyLagerortName, EasyArchivName, doc_id, doc_ver, objIndex, ref objFilename, ref objfLength, ref status);
                    }

                    string str_filename = mEasyBlobPathLocal + Convert.ToString(objFilename);
                    //Dateien schreiben
                    Weblink.EASYTransferBLOB(str_filename, objfLength, ref status);

                    mStatus = 2;
                    return str_filename;
                }
            }
            catch(Exception ex)
            {
           
            }
            return string.Empty;
       }

        /// <summary>
        /// Liefert eine Liste der im Archiv befindlichen Dateien
        /// </summary>
        /// <returns></returns>
       public string[,] GetFileList()
       {
           string[,] docIDs = null;

           try
           {

               WMDQryCln.clsQuery Weblink = new WMDQryCln.clsQuery();

               Weblink.RemoteHosts = mRemoteHosts;
               Weblink.lngRequestTimeout = mEasyRequestTimeout;
               Weblink.strSessionID = mEasySessionId;
               Weblink.strBlobPathLocal = mEasyBlobPathLocal;
               Weblink.strBlobPathRemote = mEasyBlobPathRemote;
               Weblink.strEASYUser = mEasyUser;
               Weblink.strEASYPwd = mEasyPwdClear;
               Weblink.strDebugLogPath = mEasyLogPath;
               Weblink.Init(false, false);


               object total_hits = null;
               object status = null;

               object obj1 = null;
               object obj2 = null;
               object obj3 = null;
               object obj4 = null;


               object resulthits = Weblink.EASYQueryArchiveInit(mEasyLagerortName, mEasyArchivName, mQueryString, 100, 100, mEasyQueryIndexName, obj1, ref obj2, ref obj3, ref total_hits, ref obj4, ref status);

               Int32 int_resulthits = Convert.ToInt32(resulthits);

               string[,] strResults = new string[21, 2];
               strResults = new string[21, int_resulthits];

               object doc_id = null;
               object doc_ver = null;
               object tblHeader = null;

               object EasyLagerortName = (object)mEasyLagerortName;
               object EasyArchivName = (object)mEasyArchivName;

               tblHeader = Weblink.EASYQueryArchiveNext(ref EasyLagerortName, ref EasyArchivName, ref doc_id, ref doc_ver);
               //Erste Zeile ist Header

               //Zeilen lesen
               string[] strCols = null;
               object tblRow = null;               
               string strfilename = string.Empty;

               // doc_id, doc_ver, Extension, DocType
               docIDs = new string[int_resulthits - 1, 4];

               if (int_resulthits > 1)
               {
                   for (int i = 1; i <= strResults.GetUpperBound(1); i++)
                   {
                       tblRow = Weblink.EASYQueryArchiveNext(ref EasyLagerortName, ref EasyArchivName, ref doc_id, ref doc_ver);
                       //Nächste Zeile              

                       docIDs[i - 1, 0] = (string)doc_id;
                       docIDs[i - 1, 1] = (string)doc_ver;

                       string StringRow = Convert.ToString(tblRow);

                       StringRow = StringRow.Replace("^", "");

                       //Indexdatei-Felder sammeln
                       strCols = StringRow.Split(',');

                       //0	.Mappe
                       //1	.Version
                       //2	.Archivdatum
                       //3	.Änderungsdatum
                       //4	.Felder gesamt
                       //5	.Textfelder
                       //6	.Bilder
                       //7	.Titel
                       //8	.Typ
                       //9	.FileType
                       //10	.FormName
                       //11	.WorkBoxName
                       //12	.BatchName
                       //13	Fahrgestellnummer   
                       //14	Leasingvertragsnumme
                       //15	Kennzeichen
                       //16	Zulassungsdatum

                       for (int j = 0; j <= strCols.GetUpperBound(0); j++)
                       {
                           strResults[j, i - 1] = strCols[j];
                       }

                   }
               }
               else
               {
                   return null;
               }


               if (!string.IsNullOrEmpty(strResults[0, 0]))
               {
                   for (int i = 0; i <= docIDs.GetUpperBound(0); i++)
                   {
                       DetailHelper(docIDs, i, ref Weblink, ref EasyLagerortName, ref EasyArchivName);
                   }
               }

           }
           catch (Exception)
           {

               mStatus = 1;
           }

           return docIDs;
       }
    }
}
