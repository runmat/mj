Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report45
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lbSuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbSelektionZurueckSetzen As System.Web.UI.WebControls.LinkButton

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As CKG.Components.ComCommon.Finance.Search

    Dim m_report As fin_09
    Protected WithEvents SucheHaendler1 As SucheHaendler
    'Dim strErrorText As String

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

        FormAuth(Me, m_User, True)
        ucHeader.InitUser(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text


        If Not IsPostBack Then
            m_report = New fin_09(m_User, m_App, CStr(Session("AppID")), Session.SessionID, "")
            m_report.SessionID = Session.SessionID
            m_report.AppID = CStr(Session("AppID"))
            Session.Add("m_report", m_report)

            If Not m_User.Reference = String.Empty AndAlso Not m_User.Reference = " " Then
                'wenn ein Händler den report aufruft, müssen auch alle Felder ausgeblendet werden
                'da wenn ein fehler Auftritt oder keine Ergebnisse gefunden werden, auf der Seite 
                'die fehlermeldung ausgegeben werden soll. JJ20071217
                lbSuche.Visible = False
                SucheHaendler1.Visible = False
                automaticRedirect()

            Else
                'wenn die bank den Report aufruft
                objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                Session("objSearch") = objSuche

            End If
        Else
            If m_report Is Nothing Then
                m_report = CType(Session("m_report"), fin_09)
            End If
            If objSuche Is Nothing Then
                objSuche = CType(Session("objSearch"), Finance.Search)
            End If
        End If

    End Sub

    Private Sub lbSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbSuche.Click
        'Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"


        Dim tmpHaendlernummer As String
        tmpHaendlernummer = SucheHaendler1.giveHaendlernummer
        If tmpHaendlernummer Is Nothing Then
            lbSelektionZurueckSetzen.Visible = True
            lblError.Text = "Wählen Sie einen Händler aus"
            Exit Sub
        Else
            m_report.personenennummer = tmpHaendlernummer
        End If

        m_report.Report()

        If Not m_report.Fahrzeuge Is Nothing AndAlso m_report.Fahrzeuge.Rows.Count > 0 Then
            Session("m_report") = m_report

            Response.Redirect("Report45_1.aspx?AppID=" & Session("AppID").ToString)
        Else
            lblError.Text = "Zu den gewählten Kriterien wurden keine Fahrzeuge gefunden."
            lblError.Visible = True
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub automaticRedirect()
        'Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        m_report.personenennummer = m_User.Reference

        m_report.Report()
        Session("m_report") = m_report

        If Not m_report.Fahrzeuge Is Nothing AndAlso m_report.Fahrzeuge.Rows.Count > 0 Then
            'Dim objExcelExport As New Excel.ExcelExport()
            'objExcelExport.WriteExcel(m_report.FahrzeugeExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
            'Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
            Dim dvAppLinks As DataView = m_User.Applications.DefaultView
            dvAppLinks.RowFilter = "APPURL='../Components/ComCommon/Finance/Report45.aspx'"

            If dvAppLinks.Count = 1 Then
                Dim strParameter As String = ""
                HelpProcedures.getAppParameters(dvAppLinks.Item(0).Item("AppID"), strParameter, ConfigurationManager.AppSettings("Connectionstring"))
                Response.Redirect("Report45_1.aspx?AppID=" & dvAppLinks.Item(0).Item("AppID") & strParameter)
            Else
                lblError.Text = "Fehler bei der Weiterleitung zur Statusänderung"
                lblError.Visible = True
            End If
        Else
            lblError.Text = "es wurden keine unbezahlten Fahrzeuge gefunden."
        End If
    End Sub

    Private Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSelektionZurueckSetzen.Click
        SucheHaendler1.SelektionZuruecksetzen()
        lbSelektionZurueckSetzen.Visible = False
    End Sub


End Class


' ************************************************
' $History: Report45.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Dittbernerc  Date: 18.06.09   Time: 15:39
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 - abschaltung .net connector
' 
' BAPIS:
' 
' Z_M_Haendlerbestand
' Z_M_Faellige_Fahrzdok
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.07.08   Time: 13:56
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.04.08   Time: 14:43
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 2.04.08    Time: 13:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1758
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 20.03.08   Time: 16:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1758
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 4.03.08    Time: 18:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF ANPASSUNGEN ITA 1733
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 4.03.08    Time: 10:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733, 1667, 1738 
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 1.02.08    Time: 14:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 14.01.08   Time: 11:46
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 8.01.08    Time: 10:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 20.12.07   Time: 10:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.12.07   Time: 15:55
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' BugFix Report 44/45
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 18.12.07   Time: 8:25
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1464/1498  hinzugefügt, Proxy neu erstellt + Bapi Händlerbestand
' 
' ************************************************


