Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Security



Public Class change01
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

    Private clsQMErfassung As QMErfassung
    Private m_User As User
    Private m_App As App
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents cmdAbsenden As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtAnzPos As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtMeldedatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents rbKundenrekla As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents txtKundennr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKundenname As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddProzess As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtAnsprechpartner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKontakdaten As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddFehler As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtVerursacherFirma As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVerursacherName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKlaerungsVerantwortlName As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddStatus As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtFehlerbeschreibung As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtReferenz As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdAbsenden.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New App(m_User)

            If IsPostBack = False Then

                FillDropdowns()


                If (Not Request.QueryString("ID") Is Nothing) Then
                    FillRekla(Request.QueryString("ID"))
                End If

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillDropdowns()

        Dim ds As New DataSet()
        Dim QmGetData As New QMBase(m_User, m_App, String.Empty)
        Dim dr As DataRow

        'DataSet für die Dropdowns füllen
        ds = QmGetData.getTablesForDropdowns


        'Dropdown Prozess füllen

        With ddProzess
            .Items.Add(New ListItem("Bitte auswählen", "00"))

            For Each dr In ds.Tables("dtProzess").Rows()
                .Items.Add(New ListItem(dr.Item("Prozesstext"), dr.Item("Prozessnr")))
            Next

        End With

        'Dropdown Fehler füllen

        dr = ds.Tables("dtFehler").NewRow()

        With ddFehler
            .Items.Add(New ListItem("Bitte auswählen", "00"))

            For Each dr In ds.Tables("dtFehler").Rows
                .Items.Add(New ListItem(dr.Item("Fehlertext"), dr.Item("Fehlernr")))
            Next

        End With

        'Dropdown Status füllen

        dr = ds.Tables("dtStatus").NewRow()

        With ddStatus
            .Items.Add(New ListItem("Bitte auswählen", "00"))

            For Each dr In ds.Tables("dtStatus").Rows
                .Items.Add(New ListItem(dr.Item("Statustext"), dr.Item("Statusnr")))
            Next

        End With


    End Sub

    Private Sub FillRekla(ByVal ID As String)
        Dim QmGetData As New QMBase(m_User, m_App, String.Empty)
        Dim tblTemp As New DataTable()


        tblTemp = QmGetData.FILL_QM_AuswertungSingle(ID, Session("AppID").ToString, Session.SessionID.ToString)

        With tblTemp.Rows(0)
            Me.txtAnsprechpartner.Text = .Item("Ansprechpartner")
            Me.txtAnzPos.Text = .Item("AnzPos")
            Me.txtFehlerbeschreibung.Text = .Item("Fehlerbeschreibung")
            Me.txtKlaerungsVerantwortlName.Text = .Item("Klaerungsverantwortlicher")
            Me.txtKontakdaten.Text = .Item("Kontaktdaten")
            Me.txtKundenname.Text = .Item("Kundenname")
            Me.txtKundennr.Text = .Item("Kundennr")
            Me.txtMeldedatum.Text = Left(.Item("Meldedatum"), 2) & Mid(.Item("Meldedatum"), 4, 2) & Mid(.Item("Meldedatum"), 9, 2)
            Me.txtReferenz.Text = .Item("Referenz")
            Me.txtVerursacherFirma.Text = .Item("FehlerverursacherFirma")
            Me.txtVerursacherName.Text = .Item("FehlerverursacherName")

            Me.rbKundenrekla.SelectedItem.Value = CBool(.Item("Kundenreklamation"))

            If Len(.Item("Prozess")) > 0 Then
                Me.ddProzess.SelectedItem.Text = .Item("Prozess")
                Me.ddProzess.SelectedItem.Selected = True
            End If

            If Len(.Item("Fehler")) > 0 Then
                Me.ddFehler.SelectedItem.Text = .Item("Fehler")
                Me.ddFehler.SelectedItem.Selected = True
            End If

            If Len(.Item("Status")) > 0 Then
                Me.ddStatus.SelectedItem.Text = .Item("Status")
                Me.ddStatus.SelectedItem.Selected = True
            End If

        End With


    End Sub



    Private Sub cmdAbsenden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbsenden.Click

        Dim strErr As String
        Dim booErr As Boolean
        Dim QMBase As New QMBase(m_User, m_App, String.Empty)

        If Session("QMErfassung") Is Nothing Then
            If clsQMErfassung Is Nothing Then
                clsQMErfassung = New QMErfassung()
                Session.Add("QMErfassung", clsQMErfassung)
            Else
                Session.Add("QMErfassung", clsQMErfassung)
            End If
        Else
            clsQMErfassung = Session("QMErfassung")
        End If



        'Wurden die Pflichtfelder korrekt gefüllt?
        strErr = "Bitte füllen Sie alle Pflichtfelder aus: <br>"

        If txtReferenz.Text.Length < 1 Then
            strErr = strErr & "Referenz. <br>"
            booErr = True
        End If


        If txtKundennr.Text.Length < 1 Then
            strErr = strErr & "Kundennr. <br>"
            booErr = True
        End If

        If ddProzess.SelectedItem.Text = "Bitte auswählen" Then
            strErr = strErr & "Prozess. <br>"
            booErr = True
        End If

        If ddFehler.SelectedItem.Text = "Bitte auswählen" Then
            strErr = strErr & "Fehler. <br>"
            booErr = True
        End If

        If ddStatus.SelectedItem.Value = "Bitte auswählen" Then
            strErr = strErr & "Status. <br>"
            booErr = True
        End If

        If IsNumeric(txtAnzPos.Text) = False Then
            booErr = True
            strErr = strErr & "Bitte geben Sie numerische Werte für Anz.Pos ein. <br>"
        End If

        If IsDate(Left(txtMeldedatum.Text, 2) & "." & Mid(txtMeldedatum.Text, 3, 2) & ".20" & Right(txtMeldedatum.Text, 2)) = False Then
            strErr = strErr & "Bitte geben Sie ein gültiges Meldedatum ein. <br>"
            booErr = True
        End If


        If Trim(txtFehlerbeschreibung.Text) = String.Empty And ddFehler.SelectedItem.Value <> "00" Then
            If QMBase.ErrDescPflicht(ddFehler.SelectedItem.Value) = False Then
                strErr = strErr & "Bitte geben Sie eine Fehlerbeschreibung ein. <br>"
                booErr = True
            End If
        End If



        'Daten aus den Feldern in die Klasse schreiben

        With clsQMErfassung
            .Ansprechpartner = txtAnsprechpartner.Text
            .AnzPos = txtAnzPos.Text

            .Erfasser = m_User.UserName
            .ErfasstAm = Now
            .Fehler = ddFehler.SelectedItem.Text
            .Fehlerbeschreibung = txtFehlerbeschreibung.Text
            .FehlerKey = ddFehler.SelectedItem.Value
            .FehlerverursacherFirma = txtVerursacherFirma.Text
            .FehlerverursacherName = txtVerursacherName.Text
            .KlaerungsverantwortlicherName = txtKlaerungsVerantwortlName.Text
            .Kontaktdaten = txtKontakdaten.Text
            .Kundenname = txtKundenname.Text
            .Kundenreklamation = CBool(rbKundenrekla.SelectedItem.Value)
            .Kunnr = txtKundennr.Text
            .Meldedatum = txtMeldedatum.Text
            .Prozess = ddProzess.SelectedItem.Text
            .ProzessKey = ddProzess.SelectedItem.Value
            .Referenz = txtReferenz.Text
            .Status = ddStatus.SelectedItem.Text
            .StatusKey = ddStatus.SelectedItem.Value

            If m_User.Reference.Length = 8 Then
                .VKORG = Left(m_User.Reference, 4)
                .VKBUR = Right(m_User.Reference, 4)
            End If

        End With

        If booErr = True Then
            lblError.Text = strErr
        Else

            If (Not Request.QueryString("ID") Is Nothing) Then
                QMBase.Updatesql(Request.QueryString("ID"), clsQMErfassung)
            Else
                QMBase.dbWrite(clsQMErfassung)
            End If

            DisableControls()
            lblError.Text = "Die Daten wurden gespeichert."
        End If

        Session("QMErfassung") = clsQMErfassung

    End Sub

    Private Sub DisableControls()

        txtAnsprechpartner.Enabled = False
        txtAnzPos.Enabled = False
        txtFehlerbeschreibung.Enabled = False
        txtKlaerungsVerantwortlName.Enabled = False
        txtKontakdaten.Enabled = False
        txtKundenname.Enabled = False
        txtKundennr.Enabled = False
        txtMeldedatum.Enabled = False
        txtReferenz.Enabled = False
        txtVerursacherFirma.Enabled = False
        txtVerursacherName.Enabled = False
        ddFehler.Enabled = False
        ddProzess.Enabled = False
        ddStatus.Enabled = False
        rbKundenrekla.Enabled = False
        cmdAbsenden.Visible = False

        If (Request.QueryString("ID") Is Nothing) Then
            cmdNew.Visible = True
        End If

    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Session("QMErfassung") = Nothing
        clsQMErfassung = Nothing
        cmdNew.Visible = False
        cmdAbsenden.Visible = True
        Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)

    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click

        If (Not Request.QueryString("ID") Is Nothing) Then
            Response.Redirect("Report01_2.aspx?AppID=" & Session("AppID").ToString)
        Else
            Response.Redirect("../../../Start/Selection.aspx")
        End If


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
