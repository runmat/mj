Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG.EasyAccess
Imports AppUeberf.Ueberfuehrung

<CLSCompliant(False)> Public Class _Report041

    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objPDIs As Base.Business.ABEDaten

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents tCell As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents ucStyles As Styles
    Protected WithEvents litScript As System.Web.UI.WebControls.Literal
    Protected WithEvents btnShowProtokoll As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnShowPics As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnShowAbmeldung As System.Web.UI.WebControls.ImageButton
    Private easy As EasyAccess
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKfall As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents txtHidden As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents txtType As System.Web.UI.HtmlControls.HtmlInputHidden
    Private arc_id As Integer

    Protected WithEvents lbl_TextLG As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_DataLN As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_TextLN As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_DataLG As System.Web.UI.WebControls.Label

    Private Const fileNameDelimiter As Char = "-"c
    Private Const fileNameDelimiterNew As Char = "_"c
    Private Const fileExtension As String = ".JPG"
    Private Const strThumbPrefix As String = "THUMB_"
    Private Const strPattern_02 As String = "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])" & fileExtension
    Private Const strPattern_03 As String = "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])P" & fileExtension
    Private Const strPattern_PDF As String = "\d{10}" & fileNameDelimiterNew & "\d{10}" & fileNameDelimiterNew & "[A-Za-z]*" & fileNameDelimiterNew & "[A-Za-z\-]*" &
                                            fileNameDelimiterNew & "([hHrR]\.[pP][dD][fF]){1}"
    Private type As String

    Dim strLeasingkunde As String
    Dim strLeasinggesellschaft As String



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        ucHeader.Visible = False
        m_App = New Base.Kernel.Security.App(m_User)

        Try
            If Not IsPostBack Then
                fillForm()
            Else
                If txtHidden.Value <> String.Empty Then
                    type = txtType.Value
                    showDetails()
                    loadOrigin()
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub fillForm()
        Dim table As DataTable
        Dim key As String
        Dim row As DataRow

        Dim auftrag As String
        Dim fahrt As String
        Dim referenz As String
        Dim kennzeichen As String
        Dim fahrgestellnr As String
        Dim typ As String                   'Fahrzeugtyp
        Dim von As String
        Dim nach As String
        Dim wadat As String                 'Warenausgangsdatum
        Dim wadatist As String              'Ist-Warenbewegungsdatum
        Dim termdat As String               'Datum Terminvereinbarung
        Dim abholdat As String
        Dim km As String
        Dim name1 As String
        Dim telefon As String
        Dim email As String
        Dim vdatum As String
        Dim kfall As String
        Dim ansprechpartner As String
        Dim fotos As String
        Dim protokolle As String

        table = CType(Session("ResultTable"), DataTable)
        key = Request.QueryString.Item("REF")

        row = table.Select("Counter = '" & key & "'")(0)          'Zeile holen

        fotos = row("Zzfoto").ToString
        protokolle = row("Zzprotokoll").ToString

        auftrag = row("Aufnr").ToString
        fahrt = row("Fahrtnr").ToString
        referenz = row("Zzrefnr").ToString
        kennzeichen = row("Zzkenn").ToString
        fahrgestellnr = row("Zzfahrg").ToString
        typ = row("Zzbezei").ToString                 'Fahrzeugtyp
        von = row("Fahrtvon").ToString
        nach = row("Fahrtnach").ToString
        km = row("Gef_Km").ToString
        name1 = row("Name1").ToString
        telefon = row("Telnr_Long").ToString
        email = row("Smtp_Addr").ToString
        vdatum = row("VDATU").ToString
        kfall = row("KFTEXT").ToString

        wadat = row("Wadat").ToString.TrimStart("0c")                   'Warenausgangsdatum
        wadatist = row("Wadat_Ist").ToString.TrimStart("0c")            'Ist-Warenbewegungsdatum
        termdat = row("Dat_Term").ToString.TrimStart("0c")              'Datum Terminvereinbarung
        abholdat = row("Dat_Abhol").ToString.TrimStart("0c")            'Datum Abholung

        ansprechpartner = row("EXTENSION2").ToString.Trim               'Name Ansprechpartner

        strLeasinggesellschaft = row("Name_LG")
        strLeasingkunde = row("Name_LN")

        Label1.Text = auftrag
        Label2.Text = fahrt
        Label3.Text = referenz
        Label4.Text = kennzeichen
        If (abholdat <> String.Empty) Then
            Label5.Text = "-" & abholdat
        End If
        Label6.Text = typ
        Label7.Text = wadat
        Label8.Text = von
        Label9.Text = nach
        Label10.Text = wadatist
        Label11.Text = termdat
        Label12.Text = vdatum
        Label13.Text = km
        Label14.Text = name1
        Label15.Text = telefon
        Label16.Text = email
        Label17.Text = kfall

        lbl_DataLG.Text = strLeasinggesellschaft
        lbl_DataLN.Text = strLeasingkunde


        If (ansprechpartner <> String.Empty) Then
            Label18.Text = "/" & ansprechpartner
        End If
        If termdat <> String.Empty Then
            Label12.Visible = True
            Label5.Visible = True
        End If

        If kfall <> String.Empty Then
            lblKfall.Visible = True
        End If

        btnShowProtokoll.Visible = False
        btnShowPics.Visible = False

        btnShowPics.Visible = True
        btnShowProtokoll.Visible = True

    End Sub

    Private Sub showThumbs(ByVal thumbs As ArrayList)
        'Thumbnails anzeigen

        Dim lnkButton As ImageButton
        Dim lit As Literal
        Dim pic As System.Web.UI.HtmlControls.HtmlImage
        Dim i As Integer
        Dim thumbFile As String = ""
        Dim thumbFileLarge As String = ""


        lblError.Text = String.Empty

        For i = 0 To thumbs.Count - 1
            'Bild einfügen
            thumbFile = "THUMB_" & thumbs(i).ToString
            thumbFileLarge = thumbs(i).ToString

            'Bild
            pic = New System.Web.UI.HtmlControls.HtmlImage()

            If type = "Bild" Then
                pic.Src = ConfigurationManager.AppSettings("PathView") & thumbFile
            Else
                pic.Src = ConfigurationManager.AppSettings("PathThumb")
            End If

            pic.Border = 1
            tCell.Controls.Add(pic)

            ' Button statt Hyperlink. die Bilder in Originalgröße sollen nur auf Anforderung übertragen werden.

            lnkButton = New ImageButton()
            lnkButton.ImageUrl = "../../../Images/arrow_left.gif"
            lnkButton.Attributes.Add("OnClick", "SetKey('" & thumbFileLarge & "');")
            tCell.Controls.Add(lnkButton)

            'Leerzeichen einfügen
            lit = New Literal()
            lit.Text = "</br></br>"
            tCell.Controls.Add(lit)
        Next
    End Sub

    Private Sub showDetails()
        Dim thumbs As ArrayList
        Dim auftrag As String = ""
        Dim fahrt As String = ""
        Dim auftragNr As String = ""
        Dim fahrtNr As String = ""
        Dim files As String()
        Dim i As Integer
        Dim fname As String = ""
        Dim pattern As String = ""
        Dim fileSourceThumb As System.IO.FileInfo
        Dim fileTargetThumb As System.IO.FileInfo
        Dim fileSourcePath As String = ""
        Dim fileTargetPath As String = ""
        Dim fnameThumb As String = ""

        auftragNr = Label1.Text
        fahrtNr = Label2.Text
        thumbs = New ArrayList()

        If type = "Bild" Then
            pattern = strThumbPrefix & strPattern_02
        End If
        If type = "Protokoll" Then
            pattern = strThumbPrefix & strPattern_03
        End If

        fileSourcePath = CType(ConfigurationManager.AppSettings("UploadPathSambaArchive"), String) & Right("0000000000" & Session("Kunnr"), 10) & "\" & Right("0000000000" & auftragNr, 10) & "\"
        fileTargetPath = CType(ConfigurationManager.AppSettings("UploadPathSambaShow"), String)

        Try
            If IO.Directory.Exists(fileSourcePath) Then

                files = System.IO.Directory.GetFiles(fileSourcePath & "\", "*" & auftragNr & "*")

                For i = 0 To files.Length - 1
                    fname = Right(files(i), files(i).Length - files(i).LastIndexOf("\") - 1)

                    fnameThumb = Right(files(i), files(i).Length - files(i).LastIndexOf("\") - 1)

                    If checkFilename(fname, pattern) And type = "Bild" Then
                        If fname.IndexOf("THUMB_") >= 0 Then      'Zunächst nur Thumbs laden
                            fname = Right(fname, fname.Length - fname.IndexOf("_") - 1)
                            If fname.Split(fileNameDelimiterNew).Length < 3 Then
                                auftrag = getAuftragFromFilename(fname)
                                fahrt = getTourFromFilename(fname)
                            Else
                                auftrag = getAuftragFromFilename2(fname)
                                fahrt = getTourFromFilename2(fname)
                            End If

                            If (auftrag = auftragNr) And (fahrt = fahrtNr) Then
                                If (fname.IndexOf("P.") < 0) Then
                                    thumbs.Add(fname)

                                    ' Kopieren nach neuer Verzeichnisstruktur...
                                    fileSourceThumb = New System.IO.FileInfo(fileSourcePath & "\" & fnameThumb)
                                    fileTargetThumb = New System.IO.FileInfo(fileTargetPath & fnameThumb)

                                    If Not fileTargetThumb.Exists Then
                                        Try
                                            fileSourceThumb.CopyTo(fileTargetPath & fnameThumb, True)
                                        Catch ex As Exception
                                            lblError.Text = "Datei(en) konnte(n) nicht übertragen werden."
                                        End Try
                                    End If
                                End If
                            End If
                        End If
                    Else
                        If Left(fname, 6) <> "THUMB_" Then
                            If checkFilename(fname, strPattern_PDF) Then
                                ' Sonderfall neue Dokumente mit Fahrtindex H und R statt 1 und 2
                                Dim split As String() = fname.Split("_")

                                auftrag = split(1)
                                Do While auftrag.StartsWith("0")
                                    auftrag = auftrag.Remove(0, 1)
                                Loop

                                fahrt = Left(split(4), 1)

                                Dim fahrtNr2 As String = ""

                                If fahrtNr = 1 Then
                                    fahrtNr2 = "H"
                                ElseIf fahrtNr = 2 Then
                                    fahrtNr2 = "R"
                                End If

                                If (auftrag = auftragNr) And (fahrt = fahrtNr2) Then
                                    If (type = "Protokoll") Then
                                        thumbs.Add(fname)

                                        ' Kopieren nach neuer Verzeichnisstruktur...
                                        fileSourceThumb = New System.IO.FileInfo(fileSourcePath & "\" & fnameThumb)
                                        fileTargetThumb = New System.IO.FileInfo(fileTargetPath & fnameThumb)

                                        If Not fileTargetThumb.Exists Then
                                            Try
                                                fileSourceThumb.CopyTo(fileTargetPath & fnameThumb, True)
                                            Catch ex As Exception
                                                lblError.Text = "Datei(en) konnte(n) nicht übertragen werden."
                                            End Try
                                        End If
                                    End If
                                End If
                            Else
                                ' altes Protokollformat
                                If fname.Split(fileNameDelimiterNew).Length < 3 Then
                                    auftrag = getAuftragFromFilename(fname)
                                    fahrt = getTourFromFilename(fname)
                                Else
                                    auftrag = getAuftragFromFilename2(fname)
                                    fahrt = getTourFromFilename2(fname)
                                End If

                                If (auftrag = auftragNr) And (fahrt = fahrtNr) Then
                                    If (type = "Protokoll") And (fname.IndexOf("P.") >= 0) Then
                                        thumbs.Add(fname)

                                        ' Kopieren nach neuer Verzeichnisstruktur...
                                        fileSourceThumb = New System.IO.FileInfo(fileSourcePath & "\" & fnameThumb)
                                        fileTargetThumb = New System.IO.FileInfo(fileTargetPath & fnameThumb)

                                        If Not fileTargetThumb.Exists Then
                                            Try
                                                fileSourceThumb.CopyTo(fileTargetPath & fnameThumb, True)
                                            Catch ex As Exception
                                                lblError.Text = "Datei(en) konnte(n) nicht übertragen werden."
                                            End Try
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next

                If thumbs.Count > 0 Then
                    showThumbs(thumbs)
                Else
                    lblError.Text = "Keine Dokumente vorhanden."
                End If
            Else : lblError.Text = "Dokumente noch nicht vorhanden."
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Dateien."
        End Try
    End Sub

    Private Sub loadOrigin()
        Dim fname As String
        Dim fileSource As System.IO.FileInfo
        Dim fileTarget As System.IO.FileInfo
        Dim fileSourcePath As String
        Dim fileTargetPath As String
        Dim strLink As String

        fileSourcePath = CType(ConfigurationManager.AppSettings("UploadPathSambaArchive"), String) & Right("0000000000" & Session("Kunnr"), 10) & "\" & Right("0000000000" & Label1.Text, 10) & "\"
        fileTargetPath = CType(ConfigurationManager.AppSettings("UploadPathSambaShow"), String)

        fname = txtHidden.Value

        fileSource = New System.IO.FileInfo(fileSourcePath & "\" & fname)
        fileTarget = New System.IO.FileInfo(fileTargetPath & fname)

        If Not fileTarget.Exists Then
            Try
                fileSource.CopyTo(fileTargetPath & fname, True)
            Catch ex As Exception
                lblError.Text = "Datei(en) konnte(n) nicht übertragen werden."
            End Try
        End If

        strLink = CType(ConfigurationManager.AppSettings("PathView"), String) & fname

        lblScript.Text = "						<script language=""Javascript"">" & vbCrLf
        lblScript.Text &= "						  <!-- //" & vbCrLf
        lblScript.Text &= "                          window.open(""" & Replace(strLink, "\", "/") & """, ""_blank"", ""width=640,height=480,left=0,top=0,scrollbars=YES,resizable=YES"");" & vbCrLf
        lblScript.Text &= "						  //-->" & vbCrLf
        lblScript.Text &= "						</script>" & vbCrLf

        txtHidden.Value = String.Empty
    End Sub

    Private Sub btnShowProtokoll_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnShowProtokoll.Click
        type = "Protokoll"
        showDetails()
        txtType.Value = "Protokoll"
        txtHidden.Value = String.Empty
        lblScript.Text = String.Empty
    End Sub

    Private Sub btnShowPics_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnShowPics.Click
        type = "Bild"
        showDetails()
        txtType.Value = "Bild"
        txtHidden.Value = String.Empty
        lblScript.Text = String.Empty
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: _Report041.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 8.07.09    Time: 11:51
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 12.11.07   Time: 17:27
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 8.11.07    Time: 13:04
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 8.11.07    Time: 10:17
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 7.11.07    Time: 10:37
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 26.06.07   Time: 11:23
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Fehlerbehandlung wieder aktiviert (AppUeberf, _Report041.aspx.vb)
' 
' *****************  Version 5  *****************
' User: Uha          Date: 26.06.07   Time: 8:35
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Bildpfad fuer Pfeil korrigiert. (AppUeberf, _Report041.aspx)
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 25.06.07   Time: 21:29
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 3.05.07    Time: 15:45
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen -
' _Report041.aspx nachgezogen
' 
' ************************************************