Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System
Imports System.IO

Partial Public Class Change07
    Inherits System.Web.UI.Page

    Private objSuche As FFE_Search
    Private m_User As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Private m_strHeadline As String
    Private AppName As String
    Private m_strRedirectUrl As String
    Private objDistrikt As FFE_Bank_Distrikt
    Dim Aut As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        objSuche = New FFE_Search(objApp, m_User, Session.SessionID.ToString, Session("AppID").ToString)


        Dim sInputFiliale As String = ""
        Dim DistricCount As Integer = 0
        Dim FilialCount As Integer = 0
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
        ElseIf Session("App_FilialCount") Is Nothing Then '##### parallel alte  Filialstruktur beibehalten
            FilialRow.Visible = True
            DistrictRow.Visible = False
            objSuche = New FFE_Search(objApp, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            FilialCount = FilialenLesen()
            Session("App_FilialCount") = FilialCount
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
            ElseIf Not ddl_Filiale.SelectedItem Is Nothing Then
                Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString, "", ddl_Filiale.SelectedItem.Value)
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
                    FilialRow.Visible = False
                    trName.Visible = True
                    trHdAuswahl.Visible = True
                    trHaendlernummer.Visible = True
                    trInfo.Visible = True
                    Session("objSuche") = objSuche
                    Session("App_InputFiliale") = ddl_Filiale.SelectedItem.Value
                    ddl_Filiale.Items.Clear()
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
        districtCount = objSuche.ReadDistrictSAP(Me.Page, appId, Session.SessionID)
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
    Private Function FilialenLesen(Optional ByVal blnUseComboInput As Boolean = False) As Integer
        If blnUseComboInput Then
            objSuche.HaendlerFiliale = ddl_Filiale.SelectedItem.Value
        Else
            If m_User.Organization.AllOrganizations Then
                objSuche.HaendlerFiliale = ""
            Else
                If m_User.Organization.OrganizationReference.Trim(" "c).Trim("0"c).Length = 0 Then
                    objSuche.HaendlerFiliale = "00"
                Else
                    objSuche.HaendlerFiliale = m_User.Organization.OrganizationReference
                End If
            End If
        End If

        If objSuche.LeseFilialenSAP() > 0 Then
            Session("objSuche") = objSuche
            ddl_Filiale.DataSource = objSuche.Filialen
            ddl_Filiale.DataValueField = "FILIALE"
            ddl_Filiale.DataTextField = "DISPLAY_FILIALE"
            ddl_Filiale.DataBind()
            If objSuche.Filialen.Count = 1 Then
                ddl_Filiale.SelectedIndex = 0
                ddl_Filiale.Visible = False
                DoSubmit()
            Else
                If Not m_User.Organization.AllOrganizations Then
                    Dim _li As ListItem
                    For Each _li In ddl_Filiale.Items
                        If _li.Value = m_User.Organization.OrganizationReference Then
                            _li.Selected = True
                            Exit For
                        End If
                    Next
                    ddl_Filiale.Visible = False
                    If Not ddl_Filiale.SelectedItem Is Nothing Then
                        DoSubmit()
                    End If
                Else
                    ddl_Filiale.SelectedIndex = 0
                    ddl_Filiale.Visible = True
                    trName.Visible = False
                    trHdAuswahl.Visible = False
                    trHaendlernummer.Visible = False
                    cmdSelect.Visible = True
                    Return objSuche.Filialen.Count
                End If
            End If
        Else
            ddl_Filiale.Visible = False
            lblError.Text = "Fehler: " & objSuche.ErrorMessage
        End If
    End Function
End Class
' ************************************************
' $History: Change07.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 10.03.10   Time: 14:25
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 16.07.08   Time: 11:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
