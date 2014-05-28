Imports System.Configuration
Imports System.Collections
Imports System.Data
Imports System.Web.UI


Imports System.IO

Public Class EasyAccess

    Private weblink As Object 

    Private result As New EasyResult()
    Private user As CKG.base.kernel.Security.User
    Private archive As EasyArchive
    Private srchFields As New ArrayList()
    Private total_hits As Int32
    Private strEasySystem As String
    Private DadArchive As String = String.Empty

    Public ReadOnly Property totalHits() As Int32
        Get
            Return total_hits
        End Get
    End Property

    Public ReadOnly Property searchFields() As Object
        Get
            Return srchFields
        End Get
    End Property

    'Public ReadOnly Property searchIDsRaw()
    '    Get
    '        Return searchIDs
    '    End Get
    'End Property

    Public Sub New(ByVal aUser As CKG.Base.Kernel.Security.User, Optional ByVal strDadArchiv As String = "")
        user = aUser                              'Benutzer merken
        DadArchive = strDadArchiv
        getArcs()
    End Sub

    Private Sub getArcs()
        'Anhand der user.GroupID die Archive aus der DB lesen...
        Dim status As String = ""

        archive = New EasyArchive(user, DadArchive)
        archive.getArchives(status)
    End Sub

    Public Function getArchives() As EasyArchive
        Return archive
    End Function

    Public Function getResult() As EasyResult
        Return result
    End Function

    Public Function getCurrentArchive() As archive
        Return archive.getCurrentArchive
    End Function

    Public Sub init(ByRef status As String)
        strEasySystem = ConfigurationManager.AppSettings("EasySystem") 'Welches System?

        Try
            If strEasySystem = "OLD" Then
                weblink = New WMDWebLink32Client.clsWebLink()

                With weblink
                    .strREMOTEHOST = ConfigurationManager.AppSettings("EasyRemoteHost")                    'IP-Adresse WebLink-Server 
                    .lngREMOTEPORT = ConfigurationManager.AppSettings("EasyRemotePort")                    'Kommunikationsport
                    .lngREQUESTTIMEOUT = ConfigurationManager.AppSettings("EasyRequestTimeout")            'Timeout - Zeit
                    .strSessionID = ConfigurationManager.AppSettings("EasySessionId")
                    .strBlobPathLocal = ConfigurationManager.AppSettings("EasyBlobPathLocal")              'Pfad auf dem WebLink-Server (zur Dateiablage)
                    .strBlobPathRemote = ConfigurationManager.AppSettings("EasyBlobPathRemote")            'Lokaler Pfad            
                    .strEASYUser = ConfigurationManager.AppSettings("EasyUser")
                    .strEASYPwd = ConfigurationManager.AppSettings("EasyPwd") 'CKAG
                    .Init(False)
                End With
            Else
                weblink = New WMDQryCln.clsQuery()

                With weblink
                    .remotehosts = ConfigurationManager.AppSettings("EasyRemoteHosts")                    'IP-Adresse WebLink-Server (NEU)
                    .lngrequesttimeout = ConfigurationManager.AppSettings("EasyRequestTimeout")            'Timeout - Zeit
                    .strSessionId = ConfigurationManager.AppSettings("EasySessionId")
                    .strblobpathlocal = ConfigurationManager.AppSettings("EasyBlobPathLocal")              'Pfad auf dem WebLink-Server (zur Dateiablage)
                    .strblobpathremote = ConfigurationManager.AppSettings("EasyBlobPathRemote")            'Lokaler Pfad            
                    .strEasyUser = ConfigurationManager.AppSettings("EasyUser")
                    .strEasyPwd = ConfigurationManager.AppSettings("EasyPwdClear")
                    .strDebugLogPath = ConfigurationManager.AppSettings("EasyLogPath")
                    .init(False, False)
                End With
            End If
        Catch e As Exception
            status = "init():" & e.Message
        End Try
    End Sub

    Public Function getSearchFields(ByVal arc As Archive, ByRef strSearchFields As String, ByRef status As String) As ArrayList
        Dim arcLocation As String = ""
        Dim arcName As String = ""
        Dim arcMask As String = ""
        Dim i As Integer
        Dim descr As String = ""
        Dim fieldID As Integer

        arcLocation = arc.getLocation
        arcName = arc.getName
        arcMask = arc.getIndexName
        strSearchFields = String.Empty
        srchFields.Clear()

        '######################################################################################
        '             ALT: Felder werden aus HITLISTE bestimmt
        '######################################################################################
        'num = weblink.easygetarchivedescriptioninit(arcLocation, arcName, , status)

        'If (status = String.Empty) Then
        '    For i = 1 To num    'Alle Felder durchgehen
        '        descr = weblink.easygetarchivedescriptionnext(fieldID, , , , , )
        '        strSearchFields &= "^" & descr & "^,"
        '        If Not (descr.IndexOf(".") = 0) Or (descr.ToUpper = ".TITEL") Then  '§§§ JVE 27.02.2006 Titel mit einfügen!
        '            srchFields.Add(New EasyResultField(descr, fieldID, i))
        '            'strFileIDs &= "." & fieldID & ","
        '        End If
        '    Next
        '    If strSearchFields.Substring(strSearchFields.Length - 1, 1) = "," Then
        '        strSearchFields = strSearchFields.Substring(0, strSearchFields.Length - 1)
        '    End If
        '    'If strFileIDs.Substring(strFileIDs.Length - 1, 1) = "," Then
        '    '    strFileIDs = strFileIDs.Substring(0, strFileIDs.Length - 1)
        '    'End If
        '    'searchFields = strSearchFields
        'End If
        '######################################################################################

        '######################################################################################
        '            NEU: Felder werden aus SUCHMASKE bestimmt
        '            STANDARD-Maske wird offensichtlich NICHT ausgelesen!!!
        '######################################################################################
        Dim lngTotal As Long = weblink.EASYGetMasksInit(arcLocation, arcName, status)
        If CLng(lngTotal) > -1 Then
            Dim lngCount As Long

            Dim lngFields As Long = 0
            Dim strMaskName As String = String.Empty

            While (strMaskName <> arcMask) And (lngFields >= 0)
                lngFields = weblink.EASYGetMasksNext(strMaskName)
            End While

            If strMaskName <> arcMask Then
                Throw New System.Exception("Archivfehler: Suchmaske nicht definiert.")
            End If

            For lngCount = 1 To lngFields
                fieldID = weblink.EASYGetMasksNextField(, , , descr, , , , )
                'Dim lngFieldID As Long = weblink.EASYGetMasksNextField(lngTabIndex, lngWidth, strAlign, strName, blnMandatory, strInitText, blnListOnly, strList)

                strSearchFields &= "^" & descr & "^,"
                srchFields.Add(New EasyResultField(descr, fieldID, i))

            Next
            If strSearchFields.Substring(strSearchFields.Length - 1, 1) = "," Then
                strSearchFields = strSearchFields.Substring(0, strSearchFields.Length - 1)
            End If
        End If
        '######################################################################################

        Return srchFields
    End Function

    Public Sub query(ByVal arc As Archive, ByVal queryexpression As String, ByRef resultList As ArrayList, ByRef status As String)
        Dim doc_id As String = ""
        Dim doc_ver As String = ""
        Dim tblHeader As String
        Dim tblRow As String
        Dim arcLocation As String
        Dim arcName As String
        Dim arcMask As String
        Dim i As Integer
        Dim pics As New ArrayList()

        'Dim resulttable As DataTable
        Dim resultField As EasyResultField

        resultList = New ArrayList()

        result.clear()                              'Ergebnismenge löschen
        'Problem: Je nach Archiv ist Wildcardsuche möglich/nicht möglich!!!
        arcLocation = arc.getLocation
        arcName = arc.getName
        arcMask = arc.getIndexName
        status = String.Empty

        'Hier kann nicht immer arcLocation als QueryFormat (Parameter 6) gewählt werden, weil z.B. bei Ford je nach Kunde (CSC,FFD) die Sichten NICHT Ford heißen!!!
        result.hits = weblink.EASYQueryArchiveInit(arcLocation, arcName, queryexpression, 100, 100, arcMask, , , , total_hits, , status)
        'result.hits = weblink.easyqueryarchiveinit(arcLocation, arcName, queryexpression, 100, 100, , , , , total_hits, , status)
        If (result.hits = 0) Then
            status = "Keine Daten gefunden."
            Exit Sub
        End If
        If (status = String.Empty) Then
            tblHeader = weblink.EASYQueryArchiveNext(arcLocation, arcName, doc_id, doc_ver) 'Erste Zeile ist Header
            'Hier Tabelle zusammenbauen
            'result.addHitTableHeader(tblHeader) 'Spalten zur Treffertabelle hinzufügen

            For i = 1 To result.hits - 1 'Zeilen durchgehen
                tblRow = weblink.EASYQueryArchiveNext(arcLocation, arcName, doc_id, doc_ver)    'Nächste Zeile              
                resultField = New EasyResultField(doc_id & "." & doc_ver, doc_id, doc_ver)
                resultList.Add(resultField)
                'result.addHitTableRow(tblRow, doc_id, doc_ver)
            Next
        End If
    End Sub


    Public Sub query(ByVal objArchiveList As ArrayList, ByVal queryexpression As String, ByRef status As String)
        Dim doc_id As String = ""
        Dim doc_ver As String = ""
        Dim tblHeader As String = ""
        Dim tblRow As String = ""
        Dim arcLocation As String = ""
        Dim arcName As String = ""
        Dim arcMask As String = ""
        Dim i As Integer
        Dim pics As New ArrayList()
        'Dim resulttable As DataTable
        Dim resultList As New ArrayList()
        Dim intCounter As Integer
        Dim strLocFound As String = ""
        Dim strArcFound As String = ""

        result.clear()                              'Ergebnismenge löschen

        '§§§ JVE 11.05.06. Multivarchivsuche: Lagerorte und Archive als kommaseparierten String zusammenbasteln
        arcLocation = String.Empty
        arcName = String.Empty
        arcMask = String.Empty

        For intCounter = 0 To objArchiveList.Count - 1
            arcLocation &= CType(objArchiveList(intCounter), Archive).getLocation
            arcName &= CType(objArchiveList(intCounter), Archive).getName
            If intCounter < objArchiveList.Count - 1 Then
                arcLocation &= ","
                arcName &= ","
            End If
        Next

        arcMask = CType(objArchiveList(0), Archive).getIndexName
        status = String.Empty

        'Hier kann nicht immer arcLocation als QueryFormat (Parameter 6) gewählt werden, weil z.B. bei Ford je nach Kunde (CSC,FFD) die Sichten NICHT Ford heißen!!!
        result.hits = weblink.EASYQueryArchiveInit(arcLocation, arcName, queryexpression, 100, 100, arcMask, , , , total_hits, , status)
        'result.hits = weblink.easyqueryarchiveinit(arcLocation, arcName, queryexpression, 100, 100, , , , , total_hits, , status)
        If (result.hits = 0) Then
            status = "Keine Daten gefunden."
            Exit Sub
        End If
        If (status = String.Empty) Then
            'tblHeader = weblink.easyqueryarchivenext(arcLocation, arcName, doc_id, doc_ver) 'Erste Zeile ist Header
            tblHeader = weblink.EASYQueryArchiveNext(, , doc_id, doc_ver) 'Erste Zeile ist Header
            'Hier Tabelle zusammenbauen
            result.addHitTableHeader(tblHeader) 'Spalten zur Treffertabelle hinzufügen

            For i = 1 To result.hits - 1 'Zeilen durchgehen
                'tblRow = weblink.easyqueryarchivenext(arcLocation, arcName, doc_id, doc_ver)    'Nächste Zeile              
                tblRow = weblink.EASYQueryArchiveNext(strLocFound, strArcFound, doc_id, doc_ver)    'Nächste Zeile              
                result.addHitTableRow(tblRow, strLocFound, strArcFound, doc_id, doc_ver)
            Next
        End If
    End Sub

    Public Sub malkeResultTable(ByVal arc As Archive, ByVal resultList As ArrayList, ByRef status As String)
        Dim row As String
        Dim i As Integer
        Dim lng As Long
        Dim lngCount As Long
        Dim fieldName As String
        Dim fieldID As Int32
        Dim doc_id As String
        Dim doc_ver As String
        Dim arcLocation As String
        Dim arcName As String
        'Dim resultFieldIndex As ArrayList
        Dim typ As Integer

        'Aus dem ersten Eintrag der Ergebnismenge die darzustellenden Felder extrahieren...

        'For i = 0 To resultList.Count - 1
        row = resultList(i)
        doc_id = row.Substring(0, row.IndexOf("."))
        doc_ver = row.Substring(row.IndexOf(".") + 1, row.IndexOf(":") - row.IndexOf(".") - 1)
        'Next

        arcLocation = arc.getLocation
        arcName = arc.getName
        status = String.Empty


        lng = weblink.EASYGetSegmentDescriptionInit(arcLocation, arcName, doc_id, doc_ver, , status)

        For lngCount = 1 To lng
            fieldName = weblink.EASYGetSegmentDescriptionNext(fieldID, , , , , , , , typ, , status)
        Next
    End Sub

    Public Function getThumbnails(ByVal arc As Archive, ByVal doc_id As String, ByVal doc_ver As String, ByRef status As String, Optional ByVal check As Boolean = True) As ArrayList
        '§§§JVE 08.11.2005

        Dim index As Integer
        Dim counter As Integer = 0
        Dim typ As Integer
        Dim thumbFileName As String
        Dim thumbFileSize As String
        Dim row As DataRow()
        Dim thumbFiles As New ArrayList()
        Dim thumbArray As New ArrayList()
        Dim strfilename As String = ""
        Dim lngThumbSize As Int32
        Dim htmlLink As New Web.UI.WebControls.HyperLink()
        Dim thmbLink As String = ""
        Dim ext As String = ""
        Dim title As String = ""


        result.fields = weblink.EASYGetSegmentDescriptionInit(arc.getLocation, arc.getName, doc_id, doc_ver, ConfigurationManager.AppSettings("EasyBlobPathRemote").ToString, , status)
        counter = 0
        For index = 0 To result.fields - 1  'Feldnamen abfragen    'Einzelne Felder durchgehen und nach Bildern suchen...
            weblink.EASYGetSegmentDescriptionNext(, , , , ext, typ, title, strfilename, , , lngThumbSize)
            'weblink.EASYGetSegmentDescriptionNext(, , , , , typ, , , stat, , )
            If (typ >= 512) Then   'Bildtyp: größer 512
                counter += 1
                thumbFiles.Add(strfilename & ";" & lngThumbSize)
            End If
        Next
        'weblink.easygetsegmentdata(arc.getLocation, arc.getName, doc_id, doc_ver, 4, strfilename, lngThumbSize, status)
        'weblink.easytransferblob(strfilename, lngThumbSize, status)

        If (check = True) Then
            row = result.getHitTable.Select("DOC_ID = " & doc_id & " AND DOC_VERSION = " & doc_ver)
        End If

        thmbLink = String.Empty
        thumbArray.Clear()

        For counter = 0 To thumbFiles.Count - 1
            thumbFileName = Left(thumbFiles(counter).ToString, thumbFiles(counter).ToString.IndexOf(";"))
            thumbFileName = "__" & Left(thumbFileName, thumbFileName.Length - 4) & "." & ext  'Nicht immer .JPG!!!?
            thumbFileSize = Right(thumbFiles(counter).ToString, thumbFiles(counter).ToString.Length - thumbFiles(counter).ToString.IndexOf(";") - 1)

            If CInt(thumbFileSize) > 0 Then
                weblink.EASYTransferBLOB(thumbFileName, thumbFileSize, status)
                thmbLink = ConfigurationManager.AppSettings("EasyBlobPathWebserver") & thumbFileName
            Else
                thmbLink = "../Images/leer.gif"
            End If

            thumbArray.Add(thmbLink)
        Next

        Return thumbArray
    End Function


    Public Sub setPics(ByVal arc As Archive, ByVal sessionID As String)
        Dim strFieldIDs As String = ""
        Dim strFieldTypes As String = ""
        Dim strFieldContents As String = ""
        Dim fileName As String = ""
        Dim intStatus As Int32
        Dim status As String = ""
        Dim strDocIdVer As String = ""

        fileName = "001.out"

        'sFields = srchFields
        'strFieldIDs = String.Empty
        'strFieldTypes = String.Empty
        'strFieldContents = String.Empty

        'For index = 0 To sFields.Count - 1
        '    aField = CType(sFields(index), EasyResultField)
        '    strFieldIDs &= aField.Id & ","
        '    strFieldTypes &= "FI,"      'Textfelder
        '    strFieldContents &= "^" & aField.Name & "Inhalt^,"
        'Next

        'If Right(strFieldIDs, 1) = "," Then
        '    strFieldIDs = Left(strFieldIDs, strFieldIDs.Length - 1)
        'End If
        'If Right(strFieldTypes, 1) = "," Then
        '    strFieldTypes = Left(strFieldTypes, strFieldTypes.Length - 1)
        'End If
        'If Right(strFieldContents, 1) = "," Then
        '    strFieldContents = Left(strFieldContents, strFieldContents.Length - 1)
        'End If

        strFieldIDs = "1001,1002,1003,2001"
        strFieldTypes = "FI,FI,FI,BI"
        strFieldContents = "^KundeInhalt^,^OrtInhalt^,^DokumentartInhalt^,^pic1.jpg^"

        'intStatus = weblink.CreateImportFile(ConfigurationManager.AppSettings("EasyBlobPathLocal") & "\" & fileName, "DEMO", "DEMOZUL", "WebEdit", strFieldTypes, strFieldIDs, strFieldContents, , status)
        intStatus = weblink.EASYUploadFile(fileName, status)    'Datei Uploaden
        intStatus = weblink.EASYUploadFile("pic1.jpg", status)  'Anhang Uploaden

        intStatus = weblink.EASYCreateDocument(ConfigurationManager.AppSettings("EasyBlobPathRemote") & "\" & fileName, strDocIdVer, status)
        strDocIdVer = strDocIdVer

    End Sub

    Public Sub getPicImg(ByVal arc As Archive, ByVal doc_id As String, ByVal doc_ver As String, ByRef status As String, ByRef lnkRef As String)
        Dim index As Integer
        Dim counter As Integer = 0
        Dim typ As Integer
        Dim thumbFiles As New ArrayList()
        Dim thumbArray As New ArrayList()
        Dim strfilename As String = ""
        Dim lngThumbSize As Int32
        Dim htmlLink As New Web.UI.WebControls.HyperLink()
        Dim thmbLink As String = ""
        Dim title As String = ""
        Dim stat As Int32

        result.fields = weblink.EASYGetSegmentDescriptionInit(arc.getLocation, arc.getName, doc_id, doc_ver, ConfigurationManager.AppSettings("EasyBlobPathRemote").ToString, , status)
        counter = 0
        For index = 0 To result.fields - 1  'Feldnamen abfragen    'Einzelne Felder durchgehen und nach Bildern suchen...
            weblink.EASYGetSegmentDescriptionNext(, , , , , typ, , , stat, , )
            If (typ >= 512) Then   'Bildtyp: größer 512
                counter = index     'Index der Bilddatei holen
            End If
        Next

        weblink.EASYGetSegmentData(arc.getLocation, arc.getName, doc_id, doc_ver, counter, strfilename, lngThumbSize, status)
        weblink.EASYTransferBLOB(strfilename, lngThumbSize, status)

        lnkRef = ConfigurationManager.AppSettings("EasyBlobPathWebserver") & strfilename
    End Sub


    Public Sub getPics(ByVal arcLoc As String, ByVal arcArc As String, ByVal doc_id As String, ByVal doc_ver As String, ByRef status As String, Optional ByRef linkRet As String = Nothing)
        ''Eigentlich in EasyResult!?
        Dim index As Integer
        Dim counter As Integer = 0
        Dim typ As Integer
        Dim link As String = ""
        Dim row As DataRow()
        Dim intStatus As Int32

        Dim strfilename As String = ""
        Dim fLength As Int32
        Dim stat As Int32
        Dim htmlLink As New Web.UI.WebControls.HyperLink()

        If strEasySystem = "OLD" Then  '§§§ JVE 23.10.2006
            result.fields = weblink.EASYGetSegmentDescriptionInit(arcLoc, arcArc, doc_id, doc_ver, , status)
        Else
            result.fields = weblink.EASYGetSegmentDescriptionInit(arcLoc, arcArc, doc_id, doc_ver, , , status)
        End If
        Dim Extension As String = ""
        Dim Name As String = ""
        For index = 0 To result.fields - 1  'Feldnamen abfragen    'Einzelne Felder durchgehen und nach Bildern suchen...
            weblink.EASYGetSegmentDescriptionNext(, , , , Extension, typ, , Name, stat, , )
            If (typ >= 512) Then    'Bildtyp: größer 512
                If Extension.ToUpper <> "TIF" Then
                    Exit For
                Else
                    counter += 1
                End If

            End If

        Next
        'Bild übertragen (als eine Datei)
        If Extension.ToUpper <> "TIF" Then
            intStatus = weblink.EASYGetSegmentData(arcLoc, arcArc, doc_id, doc_ver, index, strfilename, fLength, status)
        Else
            intStatus = weblink.EASYGetSegmentMultiTIF(arcLoc, arcArc, doc_id, doc_ver, "1-" & counter, strfilename, fLength, status)
        End If


        If intStatus <> 1 Then   'Fehler
            If status = String.Empty Then
                status = "Dokument konnte nicht geladen werden."
            End If
            Exit Sub
        End If

        Dim fi As New FileInfo(ConfigurationManager.AppSettings("EasyBlobPathLocal") & "\" & strfilename)
        If fi.Exists Then
            fi.Delete()
        End If
        intStatus = weblink.EASYTransferBLOB(strfilename, fLength, status)

        If intStatus <> 1 Then   'Fehler
            If status = String.Empty Then
                status = "Dokument konnte nicht geladen werden."
            End If
            Exit Sub
        End If

        Randomize(CDbl(doc_id))
        Dim strnewfilename As String = user.UserID.ToString & "_" & Format(Now, "yyyyMMddhhmmssfffffff") & "_" & CInt(Int(1000 * Rnd())).ToString.PadLeft(3, "0"c) & Right(strfilename, 4)
        fi = New FileInfo(ConfigurationManager.AppSettings("EasyBlobPathLocal") & "\" & strfilename)
        fi.MoveTo(ConfigurationManager.AppSettings("EasyBlobPathLocal") & "\" & strnewfilename)
        link = ConfigurationManager.AppSettings("EasyBlobPathWebserver") & strnewfilename

        '§§§JVE 17.11.2005 <Wenn optionaler Paramter linkRet <> Nothing, link zurückgeben (für externen Aufruf!)
        If (linkRet Is Nothing) Then
            row = result.getHitTable.Select("DOC_Location = '" & arcLoc & "' AND DOC_Archive = '" & arcArc & "' AND DOC_ID = " & doc_id & " AND DOC_VERSION = " & doc_ver)
            row(0)("Bilder") = link
            row(0)("Link") = "1 - " & counter

            result.getHitTable.AcceptChanges()
        Else
            linkRet = link
        End If
    End Sub

    Public Sub getDocumentInfo(ByVal strArcLocation As String, ByVal strArcName As String, ByVal doc_id As String, ByVal doc_ver As String, ByRef lngLaenge As Int32, ByRef strErstelldat As String, ByRef strAenderDat As String, ByRef strTitel As String, ByRef intFelderGes As Integer, ByRef intTextFelder As Integer, ByRef intBildFelder As Integer, ByRef intBlobFelder As Integer, ByRef status As String)
        '§§§ JVE 19.06.2006 Neue Funktion Dokument-Details!
        Dim intStatus As Int32
        status = String.Empty

        intStatus = weblink.EASYGetDocumentInfo(strArcLocation, strArcName, doc_id, doc_ver, lngLaenge, strErstelldat, strAenderDat, strTitel, intFelderGes, intTextFelder, intBildFelder, intBlobFelder, status)

        If intStatus <> 1 Then   'Fehler
            If status = String.Empty Then
                status = "Dokument konnte nicht geladen werden."
            End If
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        weblink = Nothing
    End Sub
End Class

' ************************************************
' $History: EasyAccess.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 9.09.09    Time: 16:59
' Updated in $/CKAG/EasyAccess
' 
' *****************  Version 4  *****************
' User: Dittbernerc  Date: 30.07.09   Time: 17:17
' Updated in $/CKAG/EasyAccess
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 23.01.09   Time: 15:21
' Updated in $/CKAG/EasyAccess
' ITA: 2162
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/EasyAccess
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:09
' Created in $/CKAG/EasyAccess
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:03
' Created in $/CKAG/Components/ComArchive/Work/CKG/EasyAccess
' 
' *****************  Version 23  *****************
' User: Fassbenders  Date: 9.10.07    Time: 18:12
' Updated in $/CKG/EasyAccess
' 
' *****************  Version 22  *****************
' User: Uha          Date: 13.09.07   Time: 11:13
' Updated in $/CKG/EasyAccess
' Fremde Scans sind zusehen. => Vermeide doppelte Dateinamen.
' 
' *****************  Version 21  *****************
' User: Uha          Date: 7.08.07    Time: 12:37
' Updated in $/CKG/EasyAccess
' Finalize (mit weblink = Nothing) in EasyAccess.vb eigefügt
' 
' *****************  Version 20  *****************
' User: Uha          Date: 1.03.07    Time: 16:55
' Updated in $/CKG/EasyAccess
' 
' ************************************************