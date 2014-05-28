Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report02
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
    Private objSuche As Search

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblFiliale As System.Web.UI.WebControls.Label
    Protected WithEvents cmbFilialen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblFilialeShow As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblDistriktShow As System.Web.UI.WebControls.Label
    Protected WithEvents cmbDistrikte As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblDistrikt As System.Web.UI.WebControls.Label
    Protected WithEvents DistriktRow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents FilialRow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
                objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                districtCount = ReadDistricts()
                Session("objSuche") = objSuche
                cmdCreate.Visible = True
                If districtCount > 0 Then
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
                Else '##### parallel alte  Filialstruktur beibehalten
                    FilialRow.Visible = True
                    DistriktRow.Visible = False
                    objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    FilialenLesen()
                End If
                '########### alte Filialstruktur
                'objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                'FilialenLesen()
                '    Session("objSuche") = objSuche
                'Else
                '    objSuche = CType(Session("objSuche"), DealerSearch.Search)
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
            Dim m_Report As New FFD_Bank_Briefe(m_User, m_App, strFileName)
            ''########### O.Rudolph ITA:946 Distriktstruktur 13.04.2007
            Dim Distriktcount As String = Session("DistrictCount")
            If Distriktcount > 0 Then
                m_Report.Filiale = cmbDistrikte.SelectedItem.Value
            Else 'parallel alte Filialstruktur beibehalten 18.04.2007
                m_Report.Filiale = cmbFilialen.SelectedItem.Value
            End If


            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else


                    Try
                        Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch
                    End Try
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try
    End Sub

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
            If objSuche.Filialen.Count = 1 Then
                cmbFilialen.SelectedIndex = 0
                cmbFilialen.Visible = False
                lblFiliale.Text = cmbFilialen.SelectedItem.Text
                lblFiliale.Visible = True
                DoSubmit()
            Else
                If Not m_User.Organization.AllOrganizations Then
                    Dim _li As ListItem
                    Dim intIchWeisNichtWieso As Int32 = 0
                    For Each _li In cmbFilialen.Items
                        If _li.Value = m_User.Organization.OrganizationReference Then
                            cmbFilialen.SelectedIndex = intIchWeisNichtWieso
                            '_li.Selected = True
                            Exit For
                        End If
                        intIchWeisNichtWieso += 1
                    Next
                    cmbFilialen.Visible = False
                    lblFiliale.Text = m_User.Organization.OrganizationName
                    lblFiliale.Visible = True
                    If Not cmbFilialen.SelectedItem Is Nothing Then
                        DoSubmit()
                    End If
                Else
                    cmbFilialen.SelectedIndex = 0
                    cmdCreate.Visible = True
                    cmbFilialen.Visible = True
                    lblFiliale.Visible = False
                End If
            End If
        Else
            lblFilialeShow.Visible = False
            lblFiliale.Visible = False
            cmbFilialen.Visible = False
            lblError.Text = "Fehler: " & objSuche.ErrorMessage
            cmdCreate.Enabled = False
        End If
    End Sub
    Private Function ReadDistricts() As Integer
        'diesen ganzen block nur einmal, da die dropdown sich eigentlich die werte über einen postback hinaus merken sollte.
        Dim districtCount As Integer

        'Hier Zugriff auf neue BAPI....
        Dim appId As Integer = CInt(Session("AppID"))
        districtCount = objSuche.ReadDistrictSAP(appId, Session.SessionID)
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report02.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
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
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
