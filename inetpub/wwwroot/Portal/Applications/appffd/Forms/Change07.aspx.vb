Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System
Imports System.IO


Public Class Change07
    Inherits System.Web.UI.Page

    Private objSuche As Search
    Private m_User As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Private m_strHeadline As String
    Private AppName As String
    Private m_strRedirectUrl As String
    Private objDistrikt As FFD_Bank_Distrikt
    Dim Aut As Boolean = False

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSelect As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdReset As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblAuswahl As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trHaendlernummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHdAuswahl As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtNummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHDNummer As System.Web.UI.WebControls.Label
    Protected WithEvents cmbHaendler As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DistrictDropDown As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents trName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trInfo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents DistrictRow As System.Web.UI.HtmlControls.HtmlTableRow

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        objApp = New Base.Kernel.Security.App(m_User)

        If Request.QueryString("AppID").Length > 0 Then
            Session("AppID") = Request.QueryString("AppID").ToString
        End If
        ucStyles.TitleText = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        objSuche = New Search(objApp, m_User, Session.SessionID.ToString, Session("AppID").ToString)


        Dim sInputFiliale As String = ""
        Dim DistricCount As Integer = 0
        If Not IsPostBack Then
            DistricCount = ReadDistricts()
        ElseIf Not Session("DistrictCount") Is Nothing Then
            DistricCount = Session("DistrictCount")
        End If
        If DistricCount > 1 Then
            DistrictRow.Visible = True
            trName.Visible = False
            trHdAuswahl.Visible = False
            trHaendlernummer.Visible = False
            cmdSelect.Visible = True
        ElseIf DistricCount = 1 Then
            sInputFiliale = Session("App_DistriktID")
        Else
            sInputFiliale = m_User.Organization.OrganizationReference()
        End If
        If Not IsPostBack Then
            If sInputFiliale.Length > 0 Then
                Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString, "", sInputFiliale)
                If tmpIntValue < 0 Then
                    lblMessage.CssClass = "TextError"
                    lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
                ElseIf tmpIntValue = 0 Then
                    lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
                Else
                    cmbHaendler.DataSource = objSuche.Haendler '####
                    Session("objSuche") = objSuche
                    If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "REFERENZ"
                    cmbHaendler.DataTextField = "DISPLAY"
                    cmbHaendler.DataValueField = "REFERENZ"
                    cmbHaendler.DataBind()
                    cmbHaendler.SelectedIndex = 0
                    cmbHaendler.Visible = True
                    lblAuswahl.Visible = True
                    cmdSelect.Visible = True
                    Session("objSuche") = objSuche
                    trInfo.Visible = True
                End If
            End If
        Else
            If Not cmbHaendler.SelectedItem Is Nothing Then
                If txtNummer.Text.Length + txtName.Text.Length > 0 Then
                    objSuche.HaendlerReferenzNummer = txtNummer.Text
                    objSuche.HaendlerName = txtName.Text
                    Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString, objSuche.HaendlerReferenzNummer, sInputFiliale)
                    If tmpIntValue < 0 Then
                        'lblMessage.CssClass = "TextError"
                        lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
                        Session("Treffer") = tmpIntValue
                    ElseIf tmpIntValue = 0 Then
                        lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
                        Session("Treffer") = tmpIntValue
                    ElseIf tmpIntValue > 1 Then
                        cmbHaendler.DataSource = objSuche.Haendler
                        cmbHaendler.DataTextField = "DISPLAY"
                        cmbHaendler.DataValueField = "REFERENZ"
                        cmbHaendler.DataBind()
                        cmbHaendler.SelectedIndex = 0
                        txtNummer.Text = ""
                        txtName.Text = ""
                        Session("Treffer") = tmpIntValue
                        cmdReset.Visible = True
                        Session("objSuche") = objSuche
                        trInfo.Visible = True
                        lblMessage.Text = "Ihre Suche ergab mehrere Treffer. Bitte wählen Sie aus."
                    ElseIf tmpIntValue = 1 Then
                        cmbHaendler.DataSource = objSuche.Haendler
                        cmbHaendler.DataTextField = "DISPLAY"
                        cmbHaendler.DataValueField = "REFERENZ"
                        cmbHaendler.DataBind()
                        cmbHaendler.SelectedIndex = 0
                        Session("Treffer") = tmpIntValue
                        Dim tmpbValue As Boolean = objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, cmbHaendler.SelectedItem.Value)
                        If tmpbValue = False Then
                            lblMessage.Text = "Keine Daten zum angegebenen Händler gefunden."
                        ElseIf tmpbValue = True Then
                            cmdReset.Visible = True
                            Session("objSuche") = objSuche
                            Session("Treffer") = 1
                        End If
                        trInfo.Visible = False

                    End If
                Else
                    Dim tmpbValue As Boolean = objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, cmbHaendler.SelectedItem.Value)
                    If tmpbValue = False Then
                        lblMessage.Text = "Keine Daten zum angegebenen Händler gefunden."
                    ElseIf tmpbValue = True Then
                        cmdReset.Visible = True
                        Session("objSuche") = objSuche
                        Session("Treffer") = 1
                    End If
                End If
            End If
        End If
    End Sub


    Private Sub DoSubmit()

        Try
            If Not DistrictDropDown.SelectedItem Is Nothing Then
                Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString, "", DistrictDropDown.SelectedItem.Value)
                If tmpIntValue < 0 Then
                    lblMessage.CssClass = "TextError"
                    lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
                ElseIf tmpIntValue = 0 Then
                    lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
                Else
                    cmbHaendler.DataSource = objSuche.Haendler '####
                    Session("objSuche") = objSuche
                    If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "REFERENZ"
                    cmbHaendler.DataTextField = "DISPLAY"
                    cmbHaendler.DataValueField = "REFERENZ"
                    cmbHaendler.DataBind()
                    cmbHaendler.SelectedIndex = 0
                    cmbHaendler.Visible = True
                    lblAuswahl.Visible = True
                    cmdSelect.Visible = True
                    DistrictRow.Visible = False
                    trName.Visible = True
                    trHdAuswahl.Visible = True
                    trHaendlernummer.Visible = True
                    trInfo.Visible = True
                    Session("objSuche") = objSuche
                    Session("App_DistriktID") = DistrictDropDown.SelectedItem.Value
                    DistrictDropDown.Items.Clear()
                End If
            ElseIf Not cmbHaendler.SelectedItem Is Nothing And CType(Session("Treffer"), Integer) = 1 Then
                Session("objSuche") = objSuche
                Response.Redirect("Change07_2.aspx?AppID=" & Session("AppID").ToString)

            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        DoSubmit()
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Session("Treffer") = Nothing
        Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


    Private Function ReadDistricts() As Integer
        'diesen ganzen block nur einmal, da die dropdown sich eigentlich die werte über einen postback hinaus merken sollte.
        Dim districtCount As Integer

        'Hier Zugriff auf neue BAPI....
        Dim appId As Integer = CInt(Session("AppID"))
        districtCount = objSuche.ReadDistrictSAP(appId, Session.SessionID)
        If districtCount > 0 Then
            With DistrictDropDown
                .Items.Clear()
                'dropdown füllen:
                .DataSource = objSuche.District
                .DataTextField = "NAME1"
                .DataValueField = "DISTRIKT"
                .DataBind()
                'vorbelegten distrikt suchen
                objSuche.District.RowFilter = "VORBELEGT='1'"
                Dim drv As DataRowView
                For Each drv In objSuche.District
                    Dim li As ListItem = .Items.FindByValue(drv("DISTRIKT").ToString)
                    If Not li Is Nothing Then
                        If Not .SelectedItem Is Nothing Then
                            .SelectedItem.Selected = False ' falls schon ein anderer selektiert, dann deselektieren.
                        End If
                        li.Selected = True
                        If districtCount = 1 Then
                            Session("App_DistriktID") = li.Value
                        End If
                    End If
                    Exit For ' nach dem ersten aussteigen, da nur einer selektiert sein darf!!!
                Next
            End With
        End If
        Session("DistrictCount") = districtCount
        Return districtCount
    End Function

End Class
