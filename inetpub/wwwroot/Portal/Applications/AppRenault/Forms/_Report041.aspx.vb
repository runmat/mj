Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
'Imports AppDCL.Ueberfuehrung

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
    Protected WithEvents ucStyles As Styles
    Protected WithEvents litScript As System.Web.UI.WebControls.Literal
    'Private easy As EasyAccess.EasyAccess
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKfall As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Private arc_id As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        ucHeader.Visible = False
        m_App = New Base.Kernel.Security.App(m_User)

        'Dim doc_id As String
        'Dim doc_ver As String
        arc_id = CInt(Request.QueryString.Item("ARC_ID"))


        Try
            If Not IsPostBack Then
                If Request.QueryString.Item("DOC_ID") Is Nothing Then
                    'easy = New EasyAccess.EasyAccess(m_User)
                    'Session.Add("EasyAccess", easy)
                    fillForm()
                Else
                    'doc_id = Request.QueryString.Item("DOC_ID").ToString
                    'doc_ver = Request.QueryString.Item("DOC_VER").ToString
                    'loadPic(doc_id, doc_ver)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    'Private Sub loadPic(ByVal doc_id, ByVal doc_ver)
    '    Dim easy As EasyAccess.EasyAccess
    '    Dim status As String
    '    Dim lnkRet As String = ""

    '    easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)  'Aus Session holen...
    '    easy.getPicImg(easy.getCurrentArchive, doc_id, doc_ver, status, lnkRet)
    '    lnkRet = lnkRet.Replace("\", "/")
    '    litScript.Text = "<script language=""Javascript"">window.open(""" & lnkRet & """, ""Ansicht"", ""left=0,top=0,scroll=YES"");</script>"
    'End Sub

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
        '§§§ JVE 21.02.2006 neues Feld zur Darstellung
        Dim ansprechpartner As String
        '-----------------------------
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
        'btnShowProtokoll.Visible = False
        'btnShowPics.Visible = False
        'If (fotos <> String.Empty) Then
        '    btnShowPics.Visible = True
        'End If
        'If (protokolle <> String.Empty) Then
        '    btnShowProtokoll.Visible = True
        'End If
    End Sub

    'Private Sub showThumbs(ByVal thumbs As ArrayList)
    '    'Thumbnails anzeigen

    '    Dim lnk As HyperLink

    '    Dim lit As Literal
    '    Dim pic As System.Web.UI.HtmlControls.HtmlImage
    '    Dim i As Integer
    '    Dim img As String
    '    Dim thumbFile As String
    '    Dim thumbFileLarge As String
    '    Dim doc_id As String
    '    Dim doc_ver As String

    '    lblError.Text = String.Empty

    '    For i = 0 To thumbs.Count - 1
    '        'Bild einfügen
    '        thumbFile = "THUMB_" & thumbs(i).ToString
    '        thumbFileLarge = thumbs(i).ToString
    '        'thumbFileLarge = Right(thumbFileLarge, thumbFileLarge.Length - thumbFileLarge.IndexOf("THUMB_") - 1) & ".JPG"

    '        'Bild
    '        pic = New System.Web.UI.HtmlControls.HtmlImage()
    '        pic.Src = ConfigurationSettings.AppSettings("PathView") & thumbFile
    '        pic.Border = 1
    '        'tCell.Controls.Add(pic)
    '        'Lupe einfügen
    '        lnk = New HyperLink()
    '        lnk.Text = "Originalgröße"
    '        lnk.ImageUrl = "/Portal/Images/lupe.gif"
    '        lnk.NavigateUrl = ConfigurationSettings.AppSettings("PathView") & thumbFileLarge
    '        lnk.Target = "_blank"
    '        'tCell.Controls.Add(lnk)

    '        'Leerzeichen einfügen
    '        lit = New Literal()
    '        lit.Text = "</br></br>"
    '        'tCell.Controls.Add(lit)
    '    Next
    'End Sub

    'Private Sub showDetails(ByVal type As String)
    '    Dim thumbs As ArrayList
    '    Dim auftrag As String
    '    Dim fahrt As String
    '    Dim auftragCheck As String
    '    Dim fahrtCheck As String
    '    Dim files As String()
    '    Dim i As Integer
    '    Dim fname As String
    '    Dim pattern As String

    '    auftragCheck = Label1.Text
    '    fahrtCheck = Label2.Text
    '    thumbs = New ArrayList()

    '    If type = "Bild" Then
    '        pattern = "THUMB_(1|2|3|4|5|6|7|8|9)\d{0,9}" & fileDelimiter & "\d{4}" & fileDelimiter & "\d{4}" & fileDelimiter & "(1|2|3|4|5|6|7|8|9)" & fileExt
    '    End If
    '    If type = "Protokoll" Then
    '        pattern = "THUMB_(0|1|2|3|4|5|6|7|8|9)\d{0,9}" & AppDCL.Ueberfuehrung.fileDelimiter & "\d{4}" & AppDCL.Ueberfuehrung.fileDelimiter & "\d{4}" & AppDCL.Ueberfuehrung.fileDelimiter & "(0|1|2|3|4|5|6|7|8|9)P" & AppDCL.Ueberfuehrung.fileExt
    '    End If

    '    files = System.IO.Directory.GetFiles(ConfigurationSettings.AppSettings("UploadPathSambaShow"), "*")

    '    For i = 0 To files.Length - 1
    '        fname = Right(files(i), files(i).Length - files(i).LastIndexOf("\") - 1)
    '        If checkFilename(fname, pattern) Then
    '            If fname.IndexOf("THUMB_") >= 0 Then      'Zunächst nur Thumbs laden
    '                fname = Right(fname, fname.Length - fname.IndexOf("_") - 1)
    '                auftrag = getAuftragFromFilename(fname)
    '                fahrt = getTourFromFilename(fname)
    '                If (auftrag = auftragCheck) And (fahrt = fahrtCheck) Then
    '                    If (type = "Protokoll") And (fname.IndexOf("P.") >= 0) Then
    '                        thumbs.Add(fname)
    '                    End If
    '                    If (type = "Bild") And (fname.IndexOf("P.") < 0) Then
    '                        thumbs.Add(fname)
    '                    End If
    '                End If
    '            End If
    '        End If

    '    Next
    '    If thumbs.Count > 0 Then
    '        showThumbs(thumbs)
    '    Else
    '        lblError.Text = "Keine Dokumente vorhanden."
    '    End If
    'End Sub

    Private Sub showAbmeldung()
        '§§§ JVE 12.12.2005 auskommentiert, da z.Z. nicht gewünscht!
        'Dim easy As EasyAccess.EasyAccess
        'Dim resultList As ArrayList
        'Dim status As String

        'arc_id = CInt(Request.QueryString.Item("ARC_ID"))

        'easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)
        'easy.getArchives.getArchive(arc_id)     'Archiv holen

        'easy.query(easy.getCurrentArchive, "", resultList, status)
    End Sub

    Private Sub btnShowProtokoll_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'showDetails("Protokoll")
    End Sub

    Private Sub btnShowPics_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'showDetails("Bild")
    End Sub

    Private Sub btnShowAbmeldung_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        showAbmeldung()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: _Report041.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:45
' Updated in $/CKAG/Applications/AppRenault/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 12:55
' Created in $/CKAG/Applications/AppRenault/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 13:36
' Updated in $/CKG/Applications/AppRenault/AppRenaultWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 12.03.07   Time: 17:14
' Updated in $/CKG/Applications/AppRenault/AppRenaultWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 11:34
' Updated in $/CKG/Applications/AppRenault/AppRenaultWeb/Forms
' 
' ************************************************
