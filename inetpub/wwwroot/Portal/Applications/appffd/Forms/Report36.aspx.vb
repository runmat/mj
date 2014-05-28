Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report36
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
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblFiliale As System.Web.UI.WebControls.Label
    Protected WithEvents ddlFiliale As System.Web.UI.WebControls.DropDownList
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
                        If Not m_User.Organization.AllOrganizations Then DoSubmit()
                    ElseIf districtCount = 0 Then
                        DistriktRow.Visible = False
                        cmdCreate.Visible = False
                        lblError.Text = "Ihne wurde bisher noch kein Distrikt zu geordnet!" & vbCrLf & _
                         "Bitte wenden Sie sich an Ihren Administrator!"
                    End If
                Else 'parallel alte Filialstruktur beibehalten 18.04.2007
                    FilialRow.Visible = True
                    DistriktRow.Visible = False
                    lblFiliale.Text = m_User.Organization.OrganizationName
                    objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    objSuche.LeseFilialenSAP()
                    With ddlFiliale
                        .Items.Clear()
                        Dim dv As DataView = objSuche.Filialen
                        dv.Sort = "DISPLAY_FILIALE"
                        .DataSource = dv
                        .DataTextField = "DISPLAY_FILIALE"
                        .DataValueField = "FILIALE"
                        .DataBind()
                        If IsNumeric(m_User.Organization.OrganizationReference) Then
                            Dim _li As ListItem = .Items.FindByValue(m_User.Organization.OrganizationReference.ToString)
                            If Not (_li Is Nothing) Then
                                _li.Selected = True
                            End If
                        End If
                    End With
                    ddlFiliale.Visible = m_User.Organization.AllOrganizations OrElse m_User.HighestAdminLevel > Security.AdminLevel.Customer
                    lblFiliale.Visible = Not ddlFiliale.Visible
                End If
            End If
            'parallel alte Filialstruktur beibehalten 18.04.2007
            If Not m_User.Organization.AllOrganizations AndAlso districtCount = 0 Then DoSubmit()

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
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

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFiliale As String = ""
            ''########### O.Rudolph ITA:946 Distriktstruktur 13.04.2007
            Dim Districtcount As String = Session("DistrictCount")
            If Districtcount > 0 Then
                If Not cmbDistrikte.SelectedItem Is Nothing Then
                    strFiliale = cmbDistrikte.SelectedItem.Value
                End If
            Else 'parallel alte Filialstruktur beibehalten 18.04.2007
                If Not ddlFiliale.SelectedItem Is Nothing Then
                    strFiliale = ddlFiliale.SelectedItem.Value
                End If
            End If

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New FFD_Bank_FehlCOC(m_User, m_App, strFiliale, strFileName)

            lblError.Text = ""

            If lblError.Text.Length = 0 Then
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
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report36.aspx.vb $
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
