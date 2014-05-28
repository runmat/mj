Imports System.Configuration
Imports System.Collections
Imports System.Data
Imports System.Web.UI
Imports CKG.Base.Kernel
Imports System.IO
Imports WMDQryCln

Public Class EasyAccess

    Private weblink As Object = New WMDQryCln.clsQuery 'WMDWebLink32Client.clsWebLink()
    Private result As New EasyResult()
    Private user As Base.Kernel.Security.User
    Private archive As EasyArchive
    Private srchFields As New ArrayList()
    Private int_total_hits As Int32

    Public ReadOnly Property remotehost() As String
        Get
            Return weblink.strremotehost
        End Get
    End Property

    Public ReadOnly Property remoteport() As Long
        Get
            Return weblink.lngremoteport
        End Get
    End Property

    Public ReadOnly Property requesttimeout() As Long
        Get
            Return weblink.lngrequesttimeout
        End Get
    End Property

    Public ReadOnly Property SessionId() As String
        Get
            Return weblink.strSessionId
        End Get
    End Property

    Public ReadOnly Property blobpathlocal() As String
        Get
            Return weblink.strblobpathlocal
        End Get
    End Property

    Public ReadOnly Property blobpathremote() As String
        Get
            Return weblink.strblobpathremote
        End Get
    End Property

    Public ReadOnly Property EasyUser() As String
        Get
            Return weblink.strEasyUser
        End Get
    End Property

    Public ReadOnly Property EasyPwd() As String
        Get
            Return weblink.strEasyPwd
        End Get
    End Property

    Public ReadOnly Property total_hits() As Int32
        Get
            Return int_total_hits
        End Get
    End Property

    Public ReadOnly Property searchFields() As ArrayList
        Get
            Return srchFields
        End Get
    End Property

    Public Sub New(ByVal aUser As Base.Kernel.Security.User)
        user = aUser                                   'Benutzer merken
        getArcs()
    End Sub

    Private Sub getArcs()
        'Anhand der user.GroupID die Archive aus der DB lesen...
        Dim status As String = ""

        archive = New EasyArchive(user)
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
        Try
            With weblink
                .strremotehost = ConfigurationManager.AppSettings("EasyRemoteHost")                    'IP-Adresse WebLink-Server 
                .lngremoteport = ConfigurationManager.AppSettings("EasyRemotePort")                    'Kommunikationsport
                .lngrequesttimeout = ConfigurationManager.AppSettings("EasyRequestTimeout")            'Timeout - Zeit
                .strSessionId = ConfigurationManager.AppSettings("EasySessionId")
                .strblobpathlocal = ConfigurationManager.AppSettings("EasyBlobPathLocal")              'Pfad auf dem WebLink-Server (zur Dateiablage)
                .strblobpathremote = ConfigurationManager.AppSettings("EasyBlobPathRemote")            'Lokaler Pfad            
                .strEasyUser = ConfigurationManager.AppSettings("EasyUser")
                .strEasyPwd = ConfigurationManager.AppSettings("EasyPwd")
                .init(False)                                                                            'Initialisierung
            End With
        Catch e As Exception
            status = "init():" & e.Message
        End Try
    End Sub

    Public Function getSearchFields(ByVal arc As archive, ByRef strSearchFields As String, ByRef status As String) As ArrayList
        Dim arcLocation As String
        Dim arcName As String
        Dim i As Integer
        Dim num As Long
        Dim descr As String
        Dim fieldID As Integer

        arcLocation = arc.getLocation
        arcName = arc.getName
        strSearchFields = String.Empty
        srchFields.Clear()

        num = weblink.easygetarchivedescriptioninit(arcLocation, arcName, , status)

        If (status = String.Empty) Then
            For i = 1 To num    'Alle Felder durchgehen
                descr = weblink.easygetarchivedescriptionnext(fieldID, , , , , )
                strSearchFields &= "^" & descr & "^,"
                If Not (descr.IndexOf(".") = 0) Then
                    srchFields.Add(New EasyResultField(descr, fieldID, i))
                End If
            Next
            If strSearchFields.Substring(strSearchFields.Length - 1, 1) = "," Then
                strSearchFields = strSearchFields.Substring(0, strSearchFields.Length - 1)
            End If
        End If

        Return srchFields
    End Function

    Public Sub query(ByVal arc As archive, ByVal queryexpression As String, ByRef status As String)
        Dim doc_id As String = ""
        Dim doc_ver As String = ""
        Dim tblHeader As String
        Dim tblRow As String
        Dim arcLocation As String
        Dim arcName As String
        Dim arcMask As String
        Dim i As Integer
        Dim pics As New ArrayList()
        Dim resultList As New ArrayList()

        result.clear()                              'Ergebnismenge löschen
        'Problem: Je nach Archiv ist Wildcardsuche möglich/nicht möglich!!!
        arcLocation = arc.getLocation
        arcName = arc.getName
        arcMask = arc.getIndexName
        status = String.Empty

        'Hier kann nicht immer arcLocation als QueryFormat (Parameter 6) gewählt werden, weil z.B. bei Ford je nach Kunde (CSC,FFD) die Sichten NICHT Ford heißen!!!
        result.hits = weblink.easyqueryarchiveinit(arcLocation, arcName, queryexpression, 1000, 1000, arcMask, , , , int_total_hits, , status)
        If (result.hits = 0) Then
            status = "Keine Daten gefunden."
            Exit Sub
        End If
        If (status = String.Empty) Then
            tblHeader = weblink.easyqueryarchivenext(arcLocation, arcName, doc_id, doc_ver) 'Erste Zeile ist Header
            'Hier Tabelle zusammenbauen
            result.addHitTableHeader(tblHeader) 'Spalten zur Treffertabelle hinzufügen

            For i = 1 To result.hits - 1 'Zeilen durchgehen
                tblRow = weblink.easyqueryarchivenext(arcLocation, arcName, doc_id, doc_ver)    'Nächste Zeile              
                result.addHitTableRow(tblRow, doc_id, doc_ver)
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

    Public Function getThumbnails(ByVal arc As archive, ByVal doc_id As String, ByVal doc_ver As String, ByRef status As String, Optional ByVal check As Boolean = True) As ArrayList
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
        Dim thmbLink As String
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
                weblink.easytransferblob(thumbFileName, thumbFileSize, status)
                thmbLink = ConfigurationManager.AppSettings("EasyBlobPathWebserver") & thumbFileName
            Else
                thmbLink = "../Images/leer.gif"
            End If

            thumbArray.Add(thmbLink)
        Next

        Return thumbArray
    End Function


    Public Sub setPics(ByVal arc As archive, ByVal sessionID As String)

        Dim strFieldIDs As String
        Dim strFieldTypes As String
        Dim strFieldContents As String
        Dim fileName As String
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

        'intStatus = weblink.CreateImportFile(ConfigurationSettings.AppSettings("EasyBlobPathLocal") & "\" & fileName, "DEMO", "DEMOZUL", "WebEdit", strFieldTypes, strFieldIDs, strFieldContents, , status)
        intStatus = weblink.EASYUploadFile(fileName, status)    'Datei Uploaden
        intStatus = weblink.EASYUploadFile("pic1.jpg", status)  'Anhang Uploaden

        intStatus = weblink.EASYCreateDocument(ConfigurationManager.AppSettings("EasyBlobPathRemote") & "\" & fileName, strDocIdVer, status)
        strDocIdVer = strDocIdVer

    End Sub

    Public Sub getPicImg(ByVal arc As archive, ByVal doc_id As String, ByVal doc_ver As String, ByRef status As String, ByRef lnkRef As String)
        Dim index As Integer
        Dim counter As Integer = 0
        Dim typ As Integer
        Dim thumbFiles As New ArrayList()
        Dim thumbArray As New ArrayList()
        Dim strfilename As String = ""
        Dim lngThumbSize As Int32
        Dim htmlLink As New Web.UI.WebControls.HyperLink()
        Dim stat As Int32

        result.fields = weblink.EASYGetSegmentDescriptionInit(arc.getLocation, arc.getName, doc_id, doc_ver, ConfigurationManager.AppSettings("EasyBlobPathRemote").ToString, , status)
        counter = 0
        For index = 0 To result.fields - 1  'Feldnamen abfragen    'Einzelne Felder durchgehen und nach Bildern suchen...
            'weblink.EASYGetSegmentDescriptionNext(, , , , ext, typ, title, strfilename, , , lngThumbSize)
            weblink.EASYGetSegmentDescriptionNext(, , , , , typ, , , stat, , )
            If (typ >= 512) Then   'Bildtyp: größer 512
                counter = index         'Index der Bilddatei holen
                'thumbFiles.Add(strfilename & ";" & lngThumbSize)
            End If
        Next

        weblink.easygetsegmentdata(arc.getLocation, arc.getName, doc_id, doc_ver, counter, strfilename, lngThumbSize, status)
        weblink.easytransferblob(strfilename, lngThumbSize, status)

        lnkRef = ConfigurationManager.AppSettings("EasyBlobPathWebserver") & strfilename

        'If (check = True) Then
        '    row = result.getHitTable.Select("DOC_ID = " & doc_id & " AND DOC_VERSION = " & doc_ver)
        'End If

        'thmbLink = String.Empty
        'thumbArray.Clear()

        'For counter = 0 To thumbFiles.Count - 1
        '    thumbFileName = Left(thumbFiles(counter).ToString, thumbFiles(counter).ToString.IndexOf(";"))
        '    thumbFileName = "__" & Left(thumbFileName, thumbFileName.Length - 4) & "." & ext  'Nicht immer .JPG!!!?
        '    thumbFileSize = Right(thumbFiles(counter).ToString, thumbFiles(counter).ToString.Length - thumbFiles(counter).ToString.IndexOf(";") - 1)

        '    If CInt(thumbFileSize) > 0 Then
        '        weblink.easytransferblob(thumbFileName, thumbFileSize, status)
        '        thmbLink = ConfigurationSettings.AppSettings("EasyBlobPathWebserver") & thumbFileName
        '    Else
        '        thmbLink = "../Images/leer.gif"
        '    End If

        '    thumbArray.Add(thmbLink)
        'Next

        'Return thumbArray
    End Sub


    Public Sub getPics(ByVal arc As archive, ByVal doc_id As String, ByVal doc_ver As String, ByRef status As String, Optional ByRef linkRet As String = Nothing)
        ''Eigentlich in EasyResult!?
        Dim index As Integer
        Dim counter As Integer = 0
        Dim typ As Integer
        Dim link As String
        Dim row As DataRow()

        Dim strfilename As String = ""
        Dim fLength As Int32
        Dim stat As Int32
        Dim htmlLink As New Web.UI.WebControls.HyperLink()

        result.fields = weblink.EASYGetSegmentDescriptionInit(arc.getLocation, arc.getName, doc_id, doc_ver, , status)

        For index = 0 To result.fields - 1  'Feldnamen abfragen    'Einzelne Felder durchgehen und nach Bildern suchen...
            weblink.EASYGetSegmentDescriptionNext(, , , , , typ, , , stat, , )
            If (typ >= 512) Then    'Bildtyp: größer 512
                counter += 1
            End If
        Next

        'Bild übertragen (als eine Datei)
        weblink.easygetsegmentmultitif(arc.getLocation, arc.getName, doc_id, doc_ver, "1-" & counter, strfilename, fLength, status)
        Dim fi As New FileInfo(ConfigurationManager.AppSettings("EasyBlobPathLocal") & "\" & strfilename)
        If fi.Exists Then
            fi.Delete()
        End If
        weblink.easytransferblob(strfilename, fLength, status)
        Randomize(CDbl(doc_id))
        Dim strnewfilename As String = user.UserID.ToString & "_" & Format(Now, "yyyyMMddhhmmssfffffff") & "_" & CInt(Int(1000 * Rnd())).ToString.PadLeft(3, "0"c) & Right(strfilename, 4)
        fi = New FileInfo(ConfigurationManager.AppSettings("EasyBlobPathLocal") & "\" & strfilename)
        fi.MoveTo(ConfigurationManager.AppSettings("EasyBlobPathLocal") & "\" & strnewfilename)
        link = ConfigurationManager.AppSettings("EasyBlobPathWebserver") & strnewfilename

        '§§§JVE 17.11.2005 <Wenn optionaler Paramter linkRet <> Nothing, link zurückgeben (für externen Aufruf!)
        If (linkRet Is Nothing) Then
            row = result.getHitTable.Select("DOC_ID = " & doc_id & " AND DOC_VERSION = " & doc_ver)
            row(0)("Bilder") = link
            row(0)("Link") = "1 - " & counter

            result.getHitTable.AcceptChanges()
        Else
            linkRet = link
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
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/EasyAccess_ALDBP/EasyAccess_ALDBP
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:21
' Created in $/CKAG/EasyAccess_ALDBP/EasyAccess_ALDBP
' 
' *****************  Version 7  *****************
' User: Uha          Date: 13.09.07   Time: 11:18
' Updated in $/CKG/Applications/AppALD/EasyAccess_ALDBP
' Fremde Scans sind zusehen. => Vermeide doppelte Dateinamen.
' 
' *****************  Version 6  *****************
' User: Uha          Date: 7.08.07    Time: 12:41
' Updated in $/CKG/Applications/AppALD/EasyAccess_ALDBP
' Finalize (mit weblink = Nothing) in EasyAccess.vb eigefügt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/EasyAccess_ALDBP
' 
' ************************************************
