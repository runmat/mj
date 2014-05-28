Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Components.ComCommon.Finance

Public Class Report05
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Suche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_Haendlernummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Protected WithEvents lbl_Haendler As System.Web.UI.WebControls.Label
    Protected WithEvents ddl_Haendler As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txt_Personennummer As System.Web.UI.WebControls.TextBox
    Dim m_report As fin_09

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

        m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte 
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

        FormAuth(Me, m_User)
        ucHeader.InitUser(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        If Not IsPostBack Then
            m_report = New fin_09(m_User, m_App, CStr(Session("AppID")), Session.SessionID, "")
            m_report.SessionID = Session.SessionID
            m_report.AppID = CStr(Session("AppID"))
            Session.Add("m_report", m_report)
            objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Session("objSearch") = objSuche
            fillDDL()
        Else
            If m_report Is Nothing Then
                m_report = CType(Session("m_report"), fin_09)
            End If
            If objSuche Is Nothing Then
                objSuche = CType(Session("objSearch"), Search)
            End If

        End If

    End Sub

    Private Sub fillDDL()

        Try
            objSuche.LeseHaendlerSAP(CStr(Session("AppID")), Session.SessionID)
            If Not objSuche.ErrorOccured Then
                objSuche.Haendler.Sort = "NAME"
                ddl_Haendler.DataSource = objSuche.Haendler
                ddl_Haendler.DataTextField = "DISPLAY"
                ddl_Haendler.DataValueField = "REFERENZ"
                ddl_Haendler.DataBind()
            End If
        Catch ex As Exception
            lblError.Text = "Fehler!: " + ex.Message
        End Try

    End Sub

    Private Sub lb_Suche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Suche.Click
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        
        m_report.personenennummer = txt_Personennummer.Text

        If txt_Personennummer.Text = " " OrElse txt_Personennummer.Text Is String.Empty Then
            If ddl_Haendler.SelectedItem IsNot Nothing And Not ddl_Haendler.SelectedIndex = 0 Then
                'Session.Add("SelectedDealer", ddl_Haendler.SelectedItem.Value)
                m_report.personenennummer = ddl_Haendler.SelectedItem.Value
            Else
                m_report.personenennummer = ""
            End If
        End If

        m_report.Report()

        If Not m_report.Fahrzeuge Is Nothing AndAlso m_report.Fahrzeuge.Rows.Count > 0 Then
            Session("m_report") = m_report

            'Dim objExcelExport As New Excel.ExcelExport()

            Excel.ExcelExport.WriteExcel(m_report.FahrzeugeExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
            Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
            Response.Redirect("Report05_02.aspx?AppID=" & Session("AppID").ToString)
        Else
            lblError.Text = "Zu den gewählten Kriterien wurden keine unbezahlten Fahrzeuge gefunden."
            lblError.Visible = True
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    'Private Sub automaticRedirect()
    '    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
    '    m_report.personenennummer = m_User.Reference

    '    m_report.Report()
    '    Session("m_report") = m_report

    '    If Not m_report.Fahrzeuge Is Nothing AndAlso m_report.Fahrzeuge.Rows.Count > 0 Then
    '        Dim objExcelExport As New Excel.ExcelExport()
    '        Excel.ExcelExport.WriteExcel(m_report.FahrzeugeExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
    '        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
    '        Response.Redirect("Report05_02.aspx?AppID=" & Session("AppID").ToString)
    '    Else
    '        lblError.Text = "es wurden keine unbezahlten Fahrzeuge gefunden."
    '    End If

    'End Sub

End Class
' ************************************************
' $History: Report05.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 5  *****************
' User: Dittbernerc  Date: 18.06.09   Time: 15:39
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 - abschaltung .net connector
' 
' BAPIS:
' 
' Z_M_Haendlerbestand
' Z_M_Faellige_Fahrzdok
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 4.03.08    Time: 18:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF ANPASSUNGEN ITA 1733
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 4.03.08    Time: 10:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733, 1667, 1738 
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.01.08   Time: 13:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' doku
' 
' ************************************************
