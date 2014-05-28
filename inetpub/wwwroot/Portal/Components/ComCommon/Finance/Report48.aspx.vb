Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report48

    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)


        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        DoSubmit()
       
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As New fin_16(m_User, m_App, strFileName)

        lblError.Text = ""

        If lblError.Text.Length = 0 Then
            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Daten vorhanden."
                Else
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Klärfallreport")
                    Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
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
' $History: Report48.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.06.09   Time: 9:26
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918, Z_M_Klaerfaelle_001
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 10.04.08   Time: 12:52
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 21.01.08   Time: 11:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 21.01.08   Time: 11:34
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.01.08   Time: 14:21
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1504
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 10.01.08   Time: 10:23
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1504
' 
' ************************************************