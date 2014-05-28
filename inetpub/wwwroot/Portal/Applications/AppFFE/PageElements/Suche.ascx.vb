Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common


Partial Public Class Suche
    Inherits System.Web.UI.UserControl

    Private objSuche As FFE_Search
    Private m_objUser As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Private m_strHeadline As String
    Private AppName As String
    Private m_strRedirectUrl As String
    Private m_blnAllowEmptySearch As Boolean
    Private m_blnNurFilialSuche As Boolean
    Private m_blnAlleFilialen As Boolean

#Region "Properties"
    Public Property AlleFilialen() As Boolean
        Get
            Return m_blnAlleFilialen
        End Get
        Set(ByVal Value As Boolean)
            m_blnAlleFilialen = Value
        End Set
    End Property

    Public Property NurFilialSuche() As Boolean
        Get
            Return m_blnNurFilialSuche
        End Get
        Set(ByVal Value As Boolean)
            m_blnNurFilialSuche = Value
        End Set
    End Property

    Public Property AllowEmptySearch() As Boolean
        Get
            Return m_blnAllowEmptySearch
        End Get
        Set(ByVal Value As Boolean)
            m_blnAllowEmptySearch = Value
        End Set
    End Property

    Public Property Headline() As String
        Get
            Return m_strHeadline
        End Get
        Set(ByVal Value As String)
            m_strHeadline = Value
            lblHeadline.Text = m_strHeadline
        End Set
    End Property

    Public Property User() As Base.Kernel.Security.User
        Get
            Return m_objUser
        End Get
        Set(ByVal Value As Base.Kernel.Security.User)
            m_objUser = Value
        End Set
    End Property

    Public Property RedirectUrl() As String
        Get
            Return m_strRedirectUrl
        End Get
        Set(ByVal Value As String)
            m_strRedirectUrl = Value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("ShowLink") = "True"

        AppName = Me.Page.Request.Url.LocalPath
        AppName = Left(AppName, InStrRev(AppName, ".") - 1)
        AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
        If AppName = "Report12" Then
            tr_Report.Visible = True
        Else
            tr_Report.Visible = False
        End If

        lblTask.Text = "Händlersuche"

        objApp = New Base.Kernel.Security.App(m_objUser)

        Dim districtCount As Integer
        ''########### O.Rudolph ITA:946 Distriktstruktur 13.04.2007
        If Not IsPostBack Then
            'cmbFilialen.Enabled = True
            If Not Request.QueryString("Back") = Nothing Then
                objSuche = CType(Session("objSuche"), FFE_Search)
                districtCount = ReadDistricts()
                Dim i As Integer
                For i = 0 To DistrictDropDown.Items.Count - 1
                    If DistrictDropDown.Items(i).Value = Session("SelectedDistrict") Then
                        DistrictDropDown.Items(i).Selected = True
                        Exit For
                    End If
                Next
                Session("objSuche") = objSuche
            Else
                objSuche = New FFE_Search(objApp, m_objUser, Session.SessionID.ToString, Session("AppID").ToString)
                districtCount = ReadDistricts()
                Session("objSuche") = objSuche

            End If

        Else
            objSuche = CType(Session("objSuche"), FFE_Search)
            districtCount = CInt(Session("DistrictCount"))
            Session("objSuche") = objSuche
        End If

        If districtCount = 0 Then
            '####################   parallel bei der Filialstruktur bleiben
            If Not IsPostBack Then
                'cmbFilialen.Enabled = True

                If Not Request.QueryString("Back") = Nothing Then
                    objSuche = CType(Session("objSuche"), FFE_Search)
                    Dim sFiliale As String
                    Dim i As Integer
                    sFiliale = objSuche.HaendlerFiliale
                    FilialenLesen()
                    For i = 0 To cmbFilialen.Items.Count - 1
                        If cmbFilialen.Items(i).Value = sFiliale Then
                            cmbFilialen.Items(i).Selected = True
                            Exit For
                        End If
                    Next
                    cmbFilialen.Visible = False
                    lblFiliale.Text = cmbFilialen.SelectedItem.Text
                    lblFiliale.Visible = True
                    lbl_FilialeShow.Visible = True
                    DoSubmit1()
                Else
                    objSuche = New FFE_Search(objApp, m_objUser, Session.SessionID.ToString, Session("AppID").ToString)
                    FilialenLesen()
                    Session("objSuche") = objSuche
                    'DoSubmit1()
                End If
            Else
                objSuche = CType(Session("objSuche"), FFE_Search)
                FilialenLesen(True)
                Session("objSuche") = objSuche
            End If

            If Not m_objUser.Reference.Length = 0 Then
                objSuche.HaendlerReferenzNummer = m_objUser.Reference
                Session("ShowLink") = "False"
                DoSubmit1()
            End If
            '########################
            tr_DistrictRow.Visible = False
            tr_SubsidiaryRow.Visible = True
            'cmdSearch.Visible = False
            'cmdSelect.Visible = False
            'cmdReset.Visible = False
            'trHaendlernummer.Visible = False
            'trName.Visible = False
            'trOrt.Visible = False
            'lblMessage.Text = "Ihnen wurde bisher noch kein Distrikt zugeordnet!" & vbCrLf & _
            '"Bitte wenden Sie sich an Ihren Administrator!"
            'FilialenLesen(IsPostBack) ' true wenn postback, false wenn nicht.
        End If
        If Not m_objUser.Reference.Length = 0 And Not m_objUser.Organization.AllOrganizations Then
            objSuche.HaendlerReferenzNummer = m_objUser.Reference
            Session("ShowLink") = "False"
            DoSubmit1()
        ElseIf districtCount > 0 Then
            Dim sSession As String
            sSession = Session("AppID")
            'If Not Session("AppID") = "98" AndAlso Not Session("AppID") = "6" And _
            '    Not Session("AppID") = "608" And Not Session("AppID") = "134" And Not Session("AppID") = "627" Then
            '    m_blnAllowEmptySearch = True
            'End If
            'trHaendlernummer.Visible = False
            'trName.Visible = False
            'trOrt.Visible = False
            'cmdSearch.Visible = False
            'cmdSelect.Visible = True
            tr_DistrictRow.Visible = True
            tr_SubsidiaryRow.Visible = False
            If districtCount >= 1 And Not IsPostBack Then
                DoSubmit1()
            End If
        End If
        'If Not m_objUser.Reference.Length = 0 Then
        '    objSuche.HaendlerReferenzNummer = m_objUser.Reference
        '    Session("ShowLink") = "False"
        '    DoSubmit1()
        'End If
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
                objSuche.District.RowFilter = "VORBELEGT='X'"
                Dim drv As DataRowView
                For Each drv In objSuche.District
                    Dim li As ListItem = .Items.FindByValue(drv("DISTRIKT").ToString)
                    If Not li Is Nothing Then
                        If Not .SelectedItem Is Nothing Then
                            .SelectedItem.Selected = False ' falls schon ein anderer selektiert, dann deselektieren.
                        End If
                        li.Selected = True
                    End If
                    Exit For ' nach dem ersten aussteigen, da nur einer selektiert sein darf!!!
                Next
            End With

            '########################
            'Rudolph 02.05.2007 wenn nicht nach einer Händlernummer etc. gesucht werden soll
            If m_blnNurFilialSuche Then
                'lblTask.Text = "Filialsuche"
                cmbHaendler.Visible = False
                tr_Haendlernummer.Visible = False
                tr_Name.Visible = False
                tr_Ort.Visible = False
                'Seidel, 8.10.2004: Hab nicht verstanden, wieso jetzt der Reset-Button
                'zum Auswählen-Button werden soll. Hab es rausgenommen und jetzt funktioniert es.
                'cmdReset.Text = "Auswählen"
                'cmdReset.Visible = True
                cmdSearch.Visible = False
                cmdSelect.Visible = True 'Seidel, 8.10.2004: s.o.
            End If
        End If
        Session("DistrictCount") = districtCount
        Return districtCount
    End Function

    Private Sub FilialenLesen(Optional ByVal blnUseComboInput As Boolean = False)
        If blnUseComboInput Then
            objSuche.HaendlerFiliale = cmbFilialen.SelectedItem.Value
        Else
            If m_objUser.Organization.AllOrganizations Or m_blnAlleFilialen Then
                objSuche.HaendlerFiliale = ""
                '§§§JVE 23.09.2005 <begin>
                'Problem: Wenn Flag "Zeige ALLE Organisationen" gesetzt ist, springt er nicht in den Redirect-Zweig!
                'Das muß er aber, damit die Reports "Überzogene Kontingente" laufen, andernfalls bleibt er auf der Seite "Report29.aspx" hängen,
                'was er nicht soll. Das hier ist eine Notlösung, die mit dem Reoportnamen arbeitet (sollte konstant bleiben), aber
                'das ist natürlich nicht sauber. Die Auswirkung des Falgs wurde an dieser Stelle nicht gesehen, leider...
                If IsSpecialRedirectURL(m_strRedirectUrl) Then
                    objSuche.HaendlerFiliale = m_objUser.Organization.OrganizationReference
                End If
                '§§§JVE 23.09.2005 <end>
            Else
                If m_objUser.Organization.OrganizationReference.Trim(" "c).Trim("0"c).Length = 0 Then
                    objSuche.HaendlerFiliale = "00"
                Else
                    objSuche.HaendlerFiliale = m_objUser.Organization.OrganizationReference
                End If
            End If
        End If

        If objSuche.LeseFilialenSAP() > 0 Then
            Session("objSuche") = objSuche
            lbl_FilialeShow.Visible = True
            lblMessage.CssClass = ""
            cmbFilialen.DataSource = objSuche.Filialen
            cmbFilialen.DataValueField = "FILIALE"
            cmbFilialen.DataTextField = "DISPLAY_FILIALE"
            cmbFilialen.DataBind()
            'cmbFilialen.SelectedIndex = 0
            '
            'Seidel, 16.10.2004:
            'Wenn Benutzer nicht Filialen aussuchen darf, dann auf eigene stellen.
            '*********************************************************************
            If Not m_objUser.Organization.AllOrganizations AndAlso Not m_blnAlleFilialen Then
                Dim _li As ListItem
                For Each _li In cmbFilialen.Items
                    If _li.Value = m_objUser.Organization.OrganizationReference Then
                        _li.Selected = True
                        Exit For
                    End If
                Next
            End If
            '*********************************************************************          
            If objSuche.Filialen.Count = 1 Then 'Seidel,15.10.2004: Hab ich selbst wieder rausgenommen: OrElse Not m_objUser.Organization.AllOrganizations Then
                cmbFilialen.Visible = False
                lblFiliale.Text = cmbFilialen.SelectedItem.Text
                lblFiliale.Visible = True
                If m_blnNurFilialSuche Then
                    Try
                        Response.Redirect(m_strRedirectUrl)
                    Catch
                    End Try
                End If
            Else
                If m_blnNurFilialSuche Then
                    lblTask.Text = "Filialsuche"
                    cmbHaendler.Visible = False
                    tr_Haendlernummer.Visible = False
                    tr_Name.Visible = False
                    tr_Ort.Visible = False
                    'Seidel, 8.10.2004: Hab nicht verstanden, wieso jetzt der Reset-Button
                    'zum Auswählen-Button werden soll. Hab es rausgenommen und jetzt funktioniert es.
                    'cmdReset.Text = "Auswählen"
                    'cmdReset.Visible = True
                    cmdSearch.Visible = False
                    cmdSelect.Visible = True 'Seidel, 8.10.2004: s.o.
                End If
                cmbFilialen.Visible = True
            End If
            'Seidel, 15.10.2004:
            'Hier versuche ich das Problem wieder in den Griff zu kriegen.
            'Der Benutzer soll weitergeleitet werden, wenn er keine Filialen aussuchen darf.
            '*********************************************************************************
            If Not m_objUser.Organization.AllOrganizations Then
                If m_blnNurFilialSuche Then
                    'Weiterleiten
                    Try
                        Response.Redirect(m_strRedirectUrl)
                    Catch
                    End Try
                ElseIf Not m_blnAlleFilialen Then
                    'Filialauswahl disablen
                    cmbFilialen.Visible = False
                    lblFiliale.Text = cmbFilialen.SelectedItem.Text
                    lblFiliale.Visible = True
                End If
                '§§§JVE 23.09.2005 <begin>
                'Problem wie oben: Wenn Flag "Zeige ALLE Organisationen" gesetzt ist,...
            Else
                If IsSpecialRedirectURL(m_strRedirectUrl) Then
                    Response.Redirect(m_strRedirectUrl)
                End If
                '§§§JVE 23.09.2005 <end>
            End If
            '*********************************************************************************
        Else
            lbl_FilialeShow.Visible = False
            lblFiliale.Visible = False
            cmbFilialen.Visible = False
            lblMessage.CssClass = "TextError"
            lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
            cmdSearch.Enabled = False
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit1()
    End Sub

    Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        Dim districtCount As String = Session("DistrictCount")
        If districtCount > 0 Then
            If Not cmbHaendler.SelectedItem Is Nothing Then
                DoSubmit2()
            Else
                DoSubmit1()
            End If
        Else : DoSubmit2()
        End If
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        If cmdSearch.Text = "&#149;&nbsp;Neue Suche" Then
            DoSubmit2()
        Else
            DoSubmit1()
        End If
    End Sub

    Private Sub DoSubmit1()
        Dim districtCount As Integer = CInt(Session("DistrictCount"))

        If districtCount > 0 Then '#################### Distriktstruktur anwenden
            If m_objUser.Reference.Length = 0 Then
                objSuche.HaendlerReferenzNummer = txtNummer.Text
                objSuche.HaendlerName = txtName.Text
                objSuche.HaendlerOrt = txtCity.Text
                If objSuche.HaendlerFiliale.Length = 0 Then
                    objSuche.HaendlerFiliale = DistrictDropDown.SelectedItem.Value
                End If
            Else
                objSuche.HaendlerReferenzNummer = m_objUser.Reference
            End If
            If districtCount = 0 Then
                'If objSuche.HaendlerFiliale.Length = 0 AndAlso cmbFilialen.Visible = False Then
                '    objSuche.HaendlerFiliale = cmbFilialen.SelectedItem.Value
                'End If
                If objSuche.HaendlerFiliale.Length = 0 Then
                    objSuche.HaendlerFiliale = DistrictDropDown.SelectedItem.Value
                End If
            Else
                DistrictLabel.Text = DistrictDropDown.SelectedItem.Text
                DistrictLabel.Visible = True
                DistrictDropDown.Visible = False
                objSuche.HaendlerFiliale = DistrictDropDown.SelectedItem.Value
                Session("SelectedDistrict") = DistrictDropDown.SelectedItem.Value
                If objSuche.HaendlerFiliale.Length = 0 Then
                    objSuche.HaendlerFiliale = DistrictDropDown.SelectedItem.Value
                End If
            End If

            cmbHaendler.Visible = False
            tr_HdAuswahl.Visible = False
            lbl_Auswahl.Visible = False

            cmdSelect.Visible = False
            lblMessage.Text = ""
            lblMessage.CssClass = ""
            cmdSearch.Text = "  Suchen  "

            Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString)

            If tmpIntValue < 0 Then
                lblMessage.CssClass = "TextError"
                lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
            ElseIf tmpIntValue = 0 AndAlso districtCount = 1 Then
                lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
            Else
                'objSuche.Haendler.Sort = "REFERENZ"
                cmbHaendler.DataSource = objSuche.Haendler '####
                If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "REFERENZ"
                cmbHaendler.DataTextField = "DISPLAY"
                cmbHaendler.DataValueField = "REFERENZ"
                cmbHaendler.DataBind()
                cmbHaendler.SelectedIndex = 0
                'cmbHaendler.Items.
                If tmpIntValue > 1 Then
                    If m_blnAllowEmptySearch And _
                    (objSuche.HaendlerReferenzNummer.Length + objSuche.HaendlerName.Length + objSuche.HaendlerOrt.Length = 0) Then
                        If districtCount > 1 And Not IsPostBack Then
                            VisibleControls()
                        Else
                            Try
                                Response.Redirect(m_strRedirectUrl)
                            Catch
                            End Try
                        End If
                    ElseIf districtCount > 1 And Not IsPostBack Then
                        VisibleControls()
                        If Not Session("SelectedDealer") = Nothing Then
                            DistrictLabel.Text = DistrictDropDown.SelectedItem.Text
                            DistrictLabel.Visible = True
                            DistrictDropDown.Visible = False
                            cmbHaendler.Visible = True
                            tr_HdAuswahl.Visible = True
                            lbl_Auswahl.Visible = True
                            cmdSelect.Visible = True
                            cmdSearch.Visible = False
                            cmdReset.Visible = True
                        End If
                    Else
                        If lblFiliale.Visible = False Then
                            cmbFilialen.Enabled = False
                        End If
                        cmbHaendler.Visible = True
                        tr_HdAuswahl.Visible = True
                        lbl_Auswahl.Visible = True
                        cmdSelect.Visible = True
                        cmdSearch.Visible = False
                        cmdReset.Visible = True
                    End If
                ElseIf districtCount > 1 Then
                    If Not cmbHaendler.SelectedItem Is Nothing Then
                        Session("SelectedDealer") = cmbHaendler.SelectedItem.Value
                        Session("objSuche") = objSuche
                        Try
                            Response.Redirect(m_strRedirectUrl)
                        Catch
                        End Try
                    Else
                        If m_blnNurFilialSuche Then
                            cmdSearch.Visible = False
                            cmdSelect.Visible = True
                        End If
                        ' cmdReset.Visible = True
                        DistrictLabel.Visible = False
                        DistrictLabel.Text = ""
                        DistrictDropDown.Visible = True
                        If IsPostBack Then
                            lblMessage.Text = "Für Ihre Auswahl wurden keine Händler gefunden!"
                            cmdSelect.Visible = False
                            cmdReset.Visible = True
                        End If
                    End If


                ElseIf Not cmbHaendler.SelectedItem Is Nothing Then
                    Session("SelectedDealer") = cmbHaendler.SelectedItem.Value
                    Session("objSuche") = objSuche
                    Try
                        Response.Redirect(m_strRedirectUrl)
                    Catch
                    End Try
                End If
            End If

            Session("objSuche") = objSuche
        Else '####################   parallel bei der Filialstruktur bleiben
            tr_DistrictRow.Visible = False
            tr_SubsidiaryRow.Visible = True
            If m_objUser.Reference.Length = 0 Then
                objSuche.HaendlerReferenzNummer = txtNummer.Text
                objSuche.HaendlerName = txtName.Text
                objSuche.HaendlerOrt = txtCity.Text
                If objSuche.HaendlerFiliale.Length = 0 And cmbFilialen.Visible = False Then
                    objSuche.HaendlerFiliale = cmbFilialen.SelectedItem.Value
                End If
            Else
                objSuche.HaendlerReferenzNummer = m_objUser.Reference
                If objSuche.HaendlerFiliale.Length = 0 And cmbFilialen.Visible = False Then


                    objSuche.HaendlerFiliale = cmbFilialen.SelectedItem.Value
                End If


            End If
            tr_HdAuswahl.Visible = False
            cmbHaendler.Visible = False

            lbl_Auswahl.Visible = False

            cmdSelect.Visible = False
            lblMessage.Text = ""
            lblMessage.CssClass = ""
            cmdSearch.Text = "  Suchen  "

            Dim tmpIntValue As Int32 = objSuche.LeseHaendlerSAP(Session("AppID").ToString, Session.SessionID.ToString)

            If tmpIntValue < 0 Then
                lblMessage.CssClass = "TextError"
                lblMessage.Text = "Fehler: " & objSuche.ErrorMessage
            ElseIf tmpIntValue = 0 Then
                cmdReset.Visible = True
                lblMessage.Text = "Ihre Suche hat keine Treffer ergeben."
            Else
                If Not IsNothing(objSuche.Haendler) Then objSuche.Haendler.Sort = "REFERENZ"
                cmbHaendler.DataSource = objSuche.Haendler
                cmbHaendler.DataTextField = "DISPLAY"
                cmbHaendler.DataValueField = "REFERENZ"
                cmbHaendler.DataBind()
                cmbHaendler.SelectedIndex = 0

                If tmpIntValue > 1 Then
                    If m_blnAllowEmptySearch And (objSuche.HaendlerReferenzNummer.Length + objSuche.HaendlerName.Length + objSuche.HaendlerOrt.Length = 0) Then
                        Session("SelectedDealer") = ""
                        Try
                            Response.Redirect(m_strRedirectUrl)
                        Catch
                        End Try

                    Else
                        If lblFiliale.Visible = False Then
                            cmbFilialen.Enabled = False
                        End If
                        cmbHaendler.Visible = True
                        tr_HdAuswahl.Visible = True
                        lbl_Auswahl.Visible = True
                        cmdSelect.Visible = True

                        lblMessage.Text = "Ihre Suche ergab mehrere Treffer.<br>Bitte wählen Sie aus."
                        lblMessage.Font.Bold = True
                        'cmdSearch.Text = "&#149;&nbsp;Neue Suche"
                        cmdSearch.Visible = False
                        cmdReset.Visible = True
                    End If
                Else
                    Session("SelectedDealer") = cmbHaendler.SelectedItem.Value
                    Session("objSuche") = objSuche
                    Try
                        Response.Redirect(m_strRedirectUrl)
                    Catch
                    End Try
                End If
            End If

            Session("objSuche") = objSuche

        End If
    End Sub
    Private Sub VisibleControls()
        cmdSearch.Visible = True
        DistrictLabel.Visible = False
        DistrictLabel.Text = ""
        DistrictDropDown.Visible = True
        cmbHaendler.Visible = False
        tr_HdAuswahl.Visible = False
    End Sub
    Private Sub DoSubmit2()
        If Not cmbHaendler.SelectedItem Is Nothing Then
            Session("SelectedDealer") = cmbHaendler.SelectedItem.Value
        End If
        Session("objSuche") = objSuche
        Try
            Response.Redirect(m_strRedirectUrl)
        Catch
        End Try
    End Sub

    Public Function IsDateWithoutPoint(ByRef strInput As String) As Boolean
        Dim strTemp As String
        If Not Len(strInput) = 8 Then
            Return False
        End If
        strTemp = Left(strInput, 2) & "." & Mid(strInput, 3, 2) & "." & Right(strInput, 4)
        If IsDate(strTemp) Then
            strInput = strTemp
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsStandardDate(ByRef strInput As String) As Boolean
        Dim strTemp As String
        If Not Len(strInput) = 8 Then
            Return False
        End If
        strTemp = Left(strInput, 2) & "." & Mid(strInput, 3, 2) & "." & Right(strInput, 4)
        If IsDate(strTemp) Then
            strInput = strTemp
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Session("SelectedDealer") = Nothing
        objSuche = New FFE_Search(objApp, m_objUser, Session.SessionID.ToString, Session("AppID").ToString)

        Dim districtCount As Integer = ReadDistricts()
        lblMessage.Text = ""
        If districtCount > 0 Then
            'distrikte anzeigen
            tr_DistrictRow.Visible = True
            tr_SubsidiaryRow.Visible = False
            VisibleControls()
            DistrictLabel.Text = String.Empty
            DistrictLabel.Visible = False
            cmdReset.Visible = False
            cmdSelect.Visible = False
        Else
            ''########### O.Rudolph ITA:946 Distriktstruktur 13.04.2007
            'tr_DistrictRow.Visible = False
            'tr_SubsidiaryRow.Visible = False
            'cmdSearch.Visible = False
            'cmdSelect.Visible = False
            'cmdReset.Visible = False
            'trHaendlernummer.Visible = False
            'trName.Visible = False
            'trOrt.Visible = False
            'cmdSearch.Visible = False
            'lblMessage.Text = "Ihnen wurde bisher noch kein Distrikt zugeordnet!" & vbCrLf & _
            ' "Bitte wenden Sie sich an Ihren Administrator!"

            '####################   parallel bei der Filialstruktur bleiben

            'filialen anzeigen
            tr_DistrictRow.Visible = False
            tr_SubsidiaryRow.Visible = True

            'wie bisher:
            '============   
            cmbFilialen.Visible = True
            lblFiliale.Visible = False
            'Seidel, 8.10.2004: s.o. in FilialenLesen
            'If m_blnNurFilialSuche Then
            '    FilialenLesen(True)
            'Else
            FilialenLesen()
            'End If

            cmbHaendler.Items.Clear()
            cmbHaendler.Visible = False
            tr_HdAuswahl.Visible = False
            lbl_Auswahl.Visible = False
            cmdReset.Visible = False
            cmdSearch.Visible = True
            cmdSelect.Visible = cmbFilialen.Visible AndAlso m_blnNurFilialSuche 'Seidel, 15.10.2004: nur ausblenden, wenn nicht ausschliesslich Filialsuche und/oder cmbFilialen unsichtbar
            cmbFilialen.Enabled = True 'Seidel, 15.10.2004: Neu; wurde vielleicht vergessen. Reset muss auch Filialen entsperren.
        End If

        cmdSearch.Text = "  Suchen  "

        Session("objSuche") = objSuche
    End Sub

    Private Function IsSpecialRedirectURL(ByVal aUrl As String) As Boolean
        If (aUrl.IndexOf("Report30_01.aspx") >= 0 OrElse aUrl.IndexOf("Report29_22.aspx") >= 0) Then
            Return True
        End If
        Return False
    End Function
End Class

