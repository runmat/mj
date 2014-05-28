Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report03 ' Datenübernahme ohne Fahrzeugbriefe
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As FFE_Search

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        NoDealer(Me, m_User)


        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            Dim districtCount As Integer

            If Not IsPostBack Then
                ''########### O.Rudolph ITA:946 Distriktstruktur 13.04.2007
                objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                districtCount = ReadDistricts()

                If districtCount > 0 Then
                    Session("objSuche") = objSuche
                    cmdCreate.Visible = True
                    If districtCount > 1 Then
                        DistriktRow.Visible = True
                    ElseIf districtCount = 1 Then
                        DistriktRow.Visible = False
                    ElseIf districtCount = 0 Then
                        DistriktRow.Visible = False
                        cmdCreate.Visible = False
                        lblError.Text = "Ihnen wurde bisher noch kein Distrikt zugeordnet!" & vbCrLf & _
                         "Bitte wenden Sie sich an Ihren Administrator!"
                    End If
                Else 'parallel alte Filialstruktur beibehalten
                    FilialeRow.Visible = True
                    DistriktRow.Visible = False
                    objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    FilialenLesen()
                End If
                '########### alte Filialstruktur
                'objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                'FilialenLesen()
                ''    Session("objSuche") = objSuche
                ''Else
                ''    objSuche = CType(Session("objSuche"), DealerSearch.Search)
                '##########
            End If

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New FFE_Bank_Briefe(m_User, m_App, strFileName)
            Dim Distriktcount As String = Session("DistrictCount")
            ''########### O.Rudolph ITA:946 Distriktstruktur 13.04.2007
            If Distriktcount > 0 Then
                m_Report.Filiale = cmbDistrikte.SelectedItem.Value
            Else 'parallel alte Filialstruktur beibehalten 18.04.2007
                m_Report.Filiale = cmbFilialen.SelectedItem.Value
            End If
            '§§§ JVE 13.01.2006: neue Methode "FillData" in DatenimportBase eingefügt, um Fehler ".FILL - Methode nicht gefunden (bisher nicht erklärbar) zu entschärfen.
            'm_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)
            m_Report.FillData(Session("AppID").ToString, Session.SessionID.ToString)

            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else

                    Dim objExcelExport As New Excel.ExcelExport()
                    Try
                        Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    Catch
                    End Try
                    Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try
    End Sub
    Private Function ReadDistricts() As Integer
        'diesen ganzen block nur einmal, da die dropdown sich eigentlich die werte über einen postback hinaus merken sollte.
        Dim districtCount As Integer

        'Hier Zugriff auf neue BAPI....
        Dim appId As Integer = CInt(Session("AppID"))
        districtCount = objSuche.ReadDistrictSAP(Me.Page, appId, Session.SessionID)
        If districtCount > 0 Then
            With cmbDistrikte
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
        End If
        Session("DistrictCount") = districtCount
        Return districtCount
    End Function
    Private Sub FilialenLesen(Optional ByVal blnUseComboInput As Boolean = False)
        If blnUseComboInput Then
            objSuche.HaendlerFiliale = cmbFilialen.SelectedItem.Value
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
            lblFilialeShow.Visible = True
            cmbFilialen.DataSource = objSuche.Filialen
            cmbFilialen.DataValueField = "FILIALE"
            cmbFilialen.DataTextField = "DISPLAY_FILIALE"
            cmbFilialen.DataBind()
            cmbFilialen.SelectedIndex = 0
            If objSuche.Filialen.Count = 1 Then
                cmbFilialen.Visible = False
                lblFiliale.Text = cmbFilialen.SelectedItem.Text
                lblFiliale.Visible = True
                DoSubmit()
            Else
                cmdCreate.Visible = True
                cmbFilialen.Visible = True
                lblFiliale.Visible = False
            End If
            'Seidel, 15.10.2004
            '*********************************************************************************
            If Not m_User.Organization.AllOrganizations Then
                Dim _li As ListItem
                For Each _li In cmbFilialen.Items
                    If _li.Value = m_User.Organization.OrganizationReference Then
                        _li.Selected = True
                        Exit For
                    End If
                Next
                cmbFilialen.Visible = False
                lblFiliale.Text = m_User.Organization.OrganizationName
                lblFiliale.Visible = True
                If Not cmbFilialen.SelectedItem Is Nothing Then
                    DoSubmit()
                End If
            End If
            '*********************************************************************************
        Else
            lblFilialeShow.Visible = False
            lblFiliale.Visible = False
            cmbFilialen.Visible = False
            lblError.Text = "Fehler: " & objSuche.ErrorMessage
            cmdCreate.Enabled = False
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report03.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 10.03.10   Time: 14:25
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 29.07.08   Time: 15:40
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 28.07.08   Time: 11:45
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
