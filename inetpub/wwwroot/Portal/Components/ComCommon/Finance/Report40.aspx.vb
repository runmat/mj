Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report40
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lnkBack As HyperLink

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lblError As System.Web.UI.WebControls.Label

    Dim m_report As fin_01


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


#Region " Construktor "
    Public Sub New()

    End Sub
#End Region

#Region " methoden "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        If Page.IsPostBack = False Then
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            m_report = New fin_01(m_User, m_App, strFileName)
            Session.Add("objReport", m_report)
            m_report.SessionID = Session.SessionID
            m_report.AppID = CStr(Session("AppID"))

            doSubmit()
        End If


    End Sub

    Private Sub doSubmit()
        
        m_report.Fill(Session("AppID").ToString, Session.SessionID)
        If m_report.Status < 0 And Not m_report.Status = -12 Then '-12=no-data
            lblError.Text = "Fehler: " & m_report.Message
        Else
            If m_report.Result Is Nothing OrElse m_report.Result.Rows.Count = 0 Then
                lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
                lnkBack.Visible = True
            Else
                Excel.ExcelExport.WriteExcel(m_report.Result, ConfigurationManager.AppSettings("ExcelPath") & m_report.FileName)

                Session("ApplblInfoText") = "LZT = Langzeittest, HEV = Händlereigene Vorführwagen, WHS = sonstige wholesale Fahrzeuge"
                Session("ResultTable") = m_report.Result
                Session("lnkExcel") = "/Portal/Temp/Excel/" & m_report.FileName
                Response.Redirect("Report40_02.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region
End Class

' ************************************************
' $History: Report40.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Dittbernerc  Date: 17.06.09   Time: 15:56
' Updated in $/CKAG/Components/ComCommon/Finance
' .Net Connector Umstellung
' 
' Bapis:
' 
' Z_M_Daten_Ohne_Brief
' Z_M_Daten_Ohne_Brief_Del
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 29.05.08   Time: 8:36
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1945
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
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
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.02.08    Time: 10:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1677
' 
' *****************  Version 4  *****************
' User: Uha          Date: 12.12.07   Time: 10:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Kosmetik im Bereich Finance
' 
' *****************  Version 3  *****************
' User: Uha          Date: 11.12.07   Time: 16:31
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1468/1500 Testversion
' 
' *****************  Version 2  *****************
' User: Uha          Date: 11.12.07   Time: 15:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1470 bzw. 1473/1497 ASPX-Seite und Lib hinzugefügt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.12.07   Time: 13:56
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1468/1500 ASPX-Seite und Lib hinzugefügt
' 
' ************************************************
