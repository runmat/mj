Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Data.SqlClient.SqlDataReader
Imports System.Web.UI.WebControls.Label
Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Structure Appl
    Dim Name As String
    Dim FriendlyName As String
    Dim Id As Integer
End Structure

'---------------------------
'Anforderungsnummer 1066
'---------------------------
Public Class Change16
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
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtNummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHDNummer As System.Web.UI.WebControls.Label
    Protected WithEvents trHaendlernummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblOrt As System.Web.UI.WebControls.Label
    Protected WithEvents trOrt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDistrikt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblShowDistrikt As System.Web.UI.WebControls.Label
    Protected WithEvents lblDistrikt As System.Web.UI.WebControls.Label
    Protected WithEvents lblAuswahl As System.Web.UI.WebControls.Label
    Protected WithEvents trHdAuswahl As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmbHaendler As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DistrictRow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents DistrictDropDown As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdDistrikt As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents cmdDel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trInfo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblError As System.Web.UI.WebControls.Label

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


        '----- Autorisieren?
        If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
                        (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
            Try
                Aut = True
                Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(objApp.Connectionstring, CInt(Session("AuthorizationID")), "objDistrikt")
                If OutPutStream Is Nothing Then
                    lblError.Text = "Keine Daten für den Vorgang vorhanden."
                Else
                    Dim formatter As New BinaryFormatter()
                    objDistrikt = New FFD_Bank_Distrikt(m_User, objApp, Session("AppID").ToString, Session.SessionID.ToString)
                    objDistrikt = DirectCast(formatter.Deserialize(OutPutStream), FFD_Bank_Distrikt)
                    formatter = New BinaryFormatter()
                    OutPutStream = GiveAuthorizationDetails(objApp.Connectionstring, CInt(Session("AuthorizationID")), "objSuche")
                    objSuche = DirectCast(formatter.Deserialize(OutPutStream), Search)
                    Session("Treffer") = 1
                    DoSubmit2()
                    Exit Sub
                End If
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try

        End If
        '----- Autorisieren?--- End
        Dim sInputFiliale As String = ""
        Dim DistricCount As Integer = 0
        If Not IsPostBack Then
            DistricCount = ReadDistricts()
        ElseIf Not Session("DistrictCount") Is Nothing Then
            DistricCount = Session("DistrictCount")
        End If
        'If DistricCount > 1 Then
        '    'DistrictRow.Visible = True
        '    'trDistrikt.Visible = False
        '    'trOrt.Visible = False
        '    'trName.Visible = False
        '    'trHdAuswahl.Visible = False
        '    'trHaendlernummer.Visible = False
        '    'cmdSelect.Visible = True
        '    'ElseIf DistricCount = 1 Then
        '    'sInputFiliale = Session("App_DistriktID")
        'Else
        '    sInputFiliale = m_User.Organization.OrganizationReference()
        'End If

        If Not IsPostBack Then

            'If Not IsNothing(Session("App_DistriktID")) Then

            'End If
            'If sInputFiliale.Length > 0 Then
            Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString, "", "ALL")
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
            End If
            trInfo.Visible = True
            'End If
        Else
            If Not cmbHaendler.SelectedItem Is Nothing Then
                If txtNummer.Text.Length + txtName.Text.Length + txtCity.Text.Length > 0 Then
                    objSuche.HaendlerReferenzNummer = txtNummer.Text
                    objSuche.HaendlerName = txtName.Text
                    objSuche.HaendlerOrt = txtCity.Text
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
                        txtCity.Text = ""
                        Session("Treffer") = tmpIntValue
                        cmdReset.Visible = True
                        Session("objSuche") = objSuche
                        lblMessage.Text = "Ihre Suche ergab mehrere Treffer. Bitte wählen Sie aus."
                        trInfo.Visible = True
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
                            cmdBack.Visible = True
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
                        cmdBack.Visible = True
                        Session("objSuche") = objSuche
                        Session("Treffer") = 1
                        trInfo.Visible = False
                    End If
                End If
            End If
        End If
    End Sub


    Private Sub DoSubmit()
        Dim strInitiator As String = ""
        Dim intAuthorizationID As Int32
        Dim iDistrictID As Integer

        Try
            'If Not DistrictDropDown.SelectedItem Is Nothing Then
            '    Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString, "", DistrictDropDown.SelectedItem.Value)
            '    If tmpIntValue < 0 Then
            '        lblMessage.CssClass = "TextError"
            '        lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
            '    ElseIf tmpIntValue = 0 Then
            '        lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
            '    Else
            '        cmbHaendler.DataSource = objSuche.Haendler '####
            '        Session("objSuche") = objSuche
            '        If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "REFERENZ"
            '        cmbHaendler.DataTextField = "DISPLAY"
            '        cmbHaendler.DataValueField = "REFERENZ"
            '        cmbHaendler.DataBind()
            '        cmbHaendler.SelectedIndex = 0
            '        cmbHaendler.Visible = True
            '        lblAuswahl.Visible = True
            '        cmdSelect.Visible = True
            '        DistrictRow.Visible = False
            '        trDistrikt.Visible = True
            '        trOrt.Visible = True
            '        trName.Visible = True
            '        trHdAuswahl.Visible = True
            '        trHaendlernummer.Visible = True
            '        trInfo.Visible = True
            '        Session("objSuche") = objSuche
            '        Session("App_DistriktID") = DistrictDropDown.SelectedItem.Value
            '        DistrictDropDown.Items.Clear()
            '    End If
            'Else
            If Not cmbHaendler.SelectedItem Is Nothing And CType(Session("Treffer"), Integer) = 1 Then
                If CType(Session("App_DistriktID"), Integer) > 0 Then
                    iDistrictID = CType(Session("App_DistriktID"), Integer)
                Else
                    iDistrictID = m_User.Organization.OrganizationId
                End If

                objApp.CheckForPendingAuthorization(CInt(Session("AppID")), iDistrictID, objSuche.REFERENZ, "", m_User.IsTestUser, strInitiator, intAuthorizationID)
                If Not strInitiator.Length = 0 Then
                    'Seite gesperrt aufgerufen, da Händlerdaten in Autorisierung

                    LoadAuthorizatioData(intAuthorizationID)
                    lblError.Text = "Die Angaben zum Händler " & Session("SelectedDealer") & " wurden vom Benutzer """ & strInitiator & """ geändert.<br>&nbsp;&nbsp;Die Autorisierung steht noch aus!"
                Else
                    If Not cmbHaendler.SelectedItem Is Nothing And Session("Treffer") = 1 Then
                        Session("SelectedDealer") = cmbHaendler.SelectedItem.Value
                        objDistrikt = New FFD_Bank_Distrikt(m_User, objApp, Session("AppID").ToString, Session.SessionID.ToString)
                        objDistrikt.Haendler = Session("SelectedDealer")
                        objDistrikt.Show(Session("AppID").ToString, Session.SessionID, Me)
                        If objDistrikt.Status = 0 Then
                            lblAuswahl.Visible = True
                            cmdSelect.Visible = True

                            trHaendlernummer.Visible = True
                            lblHDNummer.Visible = True
                            txtNummer.Visible = False
                            lblHDNummer.Text = objSuche.REFERENZ

                            trName.Visible = True
                            txtName.Visible = False
                            lblName.Visible = True
                            lblName.Text = objSuche.NAME

                            trOrt.Visible = True
                            txtCity.Visible = False
                            lblOrt.Visible = True
                            lblOrt.Text = objSuche.CITY

                            lblShowDistrikt.Visible = True
                            lblDistrikt.Visible = True
                            lblDistrikt.Text = objDistrikt.Distrikt
                            trDistrikt.Visible = True

                            trHdAuswahl.Visible = False
                            With DistrictDropDown
                                .Items.Clear()
                                'dropdown füllen:
                                .DataSource = objDistrikt.Districts
                                .DataTextField = "NAME1"
                                .DataValueField = "Kunnr_Di"
                                .DataBind()
                                'vorbelegten distrikt suchen

                                Dim li As ListItem = .Items.FindByText(objDistrikt.Distrikt)
                                If Not li Is Nothing Then
                                    If Not .SelectedItem Is Nothing Then
                                        .SelectedItem.Selected = False ' falls schon ein anderer selektiert, dann deselektieren.
                                    End If
                                    li.Selected = True
                                End If
                            End With
                            Session("App_Distrikte") = objDistrikt.Districts
                            DistrictRow.Visible = True
                            cmdDistrikt.Visible = True
                            cmdSelect.Visible = False
                            lblMessage.Text = ""
                        ElseIf objDistrikt.Status = "-9999" Then

                        End If
                    End If
                End If
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
        Response.Redirect("Change16.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        trDistrikt.Visible = False
        txtNummer.Visible = True
        lblHDNummer.Visible = False
        txtName.Visible = True
        lblName.Visible = False
        txtCity.Visible = True
        lblOrt.Visible = False
        lblShowDistrikt.Visible = True
        lblDistrikt.Visible = False
        trHdAuswahl.Visible = True
        DistrictRow.Visible = False
        cmdDistrikt.Visible = False
        cmbHaendler.Enabled = True
        cmdSelect.Visible = True
        cmdBack.Visible = False
        lblMessage.Text = ""
        If Not (Session("objSuche") Is Nothing) Then
            objSuche = Session("objSuche")
            cmbHaendler.DataSource = objSuche.Haendler
            cmbHaendler.DataTextField = "DISPLAY"
            cmbHaendler.DataValueField = "REFERENZ"
            cmbHaendler.DataBind()
            cmbHaendler.SelectedIndex = 0
            txtNummer.Text = ""
            txtName.Text = ""
            txtCity.Text = ""
        End If
        If Aut = True Then
            Try
                Response.Redirect("Change14.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")) & "&Aut=@!", False)
            Catch
            End Try
        End If

    End Sub

    Private Sub cmdDistrikt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDistrikt.Click

        Dim intAuthorizationID As Int32
        Dim iDistrictID As Integer

        lblMessage.Text = ""
        If Not DistrictDropDown.SelectedItem Is Nothing Then
            Try
                Dim logApp As New Base.Kernel.Logging.Trace(objApp.Connectionstring, objApp.SaveLogAccessSAP, objApp.LogLevel)

                objDistrikt = New FFD_Bank_Distrikt(m_User, objApp, Session("AppID").ToString, Session.SessionID.ToString)
                objDistrikt.Haendler = Session("SelectedDealer")
                objDistrikt.sDistriktID = DistrictDropDown.SelectedItem.Value
                objDistrikt.Districts = Session("App_Distrikte")
                objDistrikt.Distrikt = lblDistrikt.Text
                If Not Session("App_DistriktID") = Nothing Then
                    iDistrictID = Session("App_DistriktID")
                Else
                    iDistrictID = m_User.Organization.OrganizationId
                End If

                If Aut = False Then

                    Dim DetailArray(2, 2) As Object
                    Dim ms As MemoryStream
                    Dim formatter As BinaryFormatter
                    Dim b() As Byte

                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, objDistrikt)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(0, 0) = ms
                    DetailArray(0, 1) = "objDistrikt"

                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, objSuche)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(1, 0) = ms
                    DetailArray(1, 1) = "objSuche"


                    intAuthorizationID = WriteAuthorization(objApp.Connectionstring, CInt(Session("AppID")), m_User.UserName, iDistrictID, objSuche.REFERENZ, "", "", "", m_User.IsTestUser, DetailArray)
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(Session("SelectedDealer").ToString, 5), "Distriktzuordnung für Händler " & objSuche.REFERENZ & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                    lblMessage.Text = "Die Änderung des Regionalbüros wurde initiiert und liegt zur Autorisierung vor!"
                    cmdDistrikt.Visible = False
                    cmdReset.Visible = False
                    cmbHaendler.Enabled = False
                    DistrictDropDown.Enabled = False
                Else
                    objDistrikt.Change(Session("AppID").ToString, Session.SessionID, Me)
                    DeleteAuthorizationEntry(objApp.Connectionstring, Session("AuthorizationID"))
                    DoSubmit2()
                    lblMessage.Text = "Die Änderung des Regionalbüros des Händlers " & Session("SelectedDealer").ToString & " war erfolgreich!"
                    DistrictDropDown.Enabled = False
                    cmdDistrikt.Visible = False
                    cmdReset.Visible = False
                    cmbHaendler.Enabled = False
                    DistrictDropDown.Enabled = False
                    cmdDel.Visible = False
                End If
            Catch ex As Exception
                lblError.Text = "Fehler in der Zuordnung. " & ex.Message
            End Try

        End If
    End Sub

    Private Sub LoadAuthorizatioData(ByVal AuthorizationID As Int32)
        Dim OutPutStream As System.IO.MemoryStream = GiveAuthorizationDetails(objApp.Connectionstring, AuthorizationID, "objDistrikt")
        Dim formatter As New BinaryFormatter()
        objDistrikt = New FFD_Bank_Distrikt(m_User, objApp, Session("AppID").ToString, Session.SessionID.ToString)
        objDistrikt = DirectCast(formatter.Deserialize(OutPutStream), FFD_Bank_Distrikt)
        formatter = New BinaryFormatter()
        OutPutStream = GiveAuthorizationDetails(objApp.Connectionstring, AuthorizationID, "objSuche")
        objSuche = DirectCast(formatter.Deserialize(OutPutStream), Search)
        DoSubmit2()
        cmdDistrikt.Visible = False
        cmdReset.Visible = False
        cmbHaendler.Enabled = False
        DistrictDropDown.Enabled = False
    End Sub

    Private Sub DoSubmit2()
        Try
            If objDistrikt.Status = 0 Then

                trDistrikt.Visible = True
                txtNummer.Visible = False
                lblHDNummer.Visible = True
                lblHDNummer.Text = objSuche.REFERENZ
                Session("SelectedDealer") = objSuche.REFERENZ

                txtName.Visible = False
                lblName.Visible = True
                lblName.Text = objSuche.NAME

                txtCity.Visible = False
                lblOrt.Visible = True
                txtCity.Visible = False

                lblOrt.Visible = True
                lblOrt.Text = objSuche.CITY

                lblShowDistrikt.Visible = True
                lblDistrikt.Visible = True
                lblDistrikt.Text = objDistrikt.Distrikt

                trHdAuswahl.Visible = False
                With DistrictDropDown
                    .Items.Clear()
                    'dropdown füllen:
                    .DataSource = objDistrikt.Districts
                    .DataTextField = "NAME1"
                    .DataValueField = "Kunnr_Di"
                    .DataBind()
                    'vorbelegten distrikt suchen

                    Dim li As ListItem = .Items.FindByValue(objDistrikt.sDistriktID)
                    If Not li Is Nothing Then
                        If Not .SelectedItem Is Nothing Then
                            .SelectedItem.Selected = False ' falls schon ein anderer selektiert, dann deselektieren.
                        End If
                        li.Selected = True
                    End If
                End With
                Session("App_Distrikte") = objDistrikt.Districts
                DistrictRow.Visible = True
                cmdDistrikt.Text = "Autorisieren"
                DistrictDropDown.Enabled = False
                cmdDistrikt.Visible = True
                cmdSelect.Visible = False
                cmdBack.Visible = True
                cmdDel.Visible = True
                lblMessage.Text = ""
            ElseIf objDistrikt.Status = "-9999" Then

            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


    Private Sub cmdDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDel.Click

        Try
            DeleteAuthorizationEntry(objApp.Connectionstring, Session("AuthorizationID"))
            DoSubmit2()
            lblMessage.Text = "Die Änderung des Regionalbüros des Händlers " & Session("SelectedDealer").ToString & " wurde gelöscht!"
            DistrictDropDown.Enabled = False
            cmdDistrikt.Visible = False
            cmdReset.Visible = False
            cmbHaendler.Enabled = False
            cmdDel.Visible = False
            DistrictDropDown.Enabled = False
        Catch ex As Exception
            lblMessage.Text = "Fehler beim Löschen des Vorgangs! " & ex.Message
        End Try
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
                        'If districtCount = 1 Then
                        Session("App_DistriktID") = li.Value
                        'End If
                    End If
                    Exit For ' nach dem ersten aussteigen, da nur einer selektiert sein darf!!!
                Next
            End With
        End If
        Session("DistrictCount") = districtCount
        Return districtCount
    End Function
End Class

' ************************************************
' $History: Change16.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 19.06.09   Time: 8:35
' Updated in $/CKAG/Applications/appffd/Forms
' Change16 - FFD_Bank_Distrikt New AppID und SessionID getauscht
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 20.05.09   Time: 14:30
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 21  *****************
' User: Rudolpho     Date: 10.03.08   Time: 10:10
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 28.02.08   Time: 10:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA: 1737
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 27.02.08   Time: 18:11
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA: 1737
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 27.02.08   Time: 10:33
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA: 1737
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 18.02.08   Time: 17:19
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA: 1689
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 13.02.08   Time: 14:21
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 23.11.07   Time: 15:55
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA: 1372 OR
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 14.11.07   Time: 17:34
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 7.11.07    Time: 15:50
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 7.11.07    Time: 14:24
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA:1374
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 12.07.07   Time: 13:19
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 21.06.07   Time: 16:45
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 20.06.07   Time: 10:06
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 20.06.07   Time: 9:31
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 20.06.07   Time: 9:00
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 19.06.07   Time: 14:29
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 19.06.07   Time: 14:27
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 19.06.07   Time: 8:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:35
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
