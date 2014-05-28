Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change51
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lbSuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtHaendlerNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents SucheHaendler1 As SucheHaendler
    Protected WithEvents lbSelektionZurueckSetzen As System.Web.UI.WebControls.LinkButton

    Private objSuche As Finance.Search
    'Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    'Private objBank As BankBaseCredit
    Private m_change As fin_13



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
        lbSuche.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

    End Sub

    Private Sub lbSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbSuche.Click
        doSubmit()
    End Sub

    Private Sub doSubmit()

        If SucheHaendler1.isInUse = False Then
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            m_change = New fin_13(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            m_change.AppID = Session("AppID").ToString
            m_change.CreditControlArea = "ZDAD"

            Dim strTemp As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, strTemp, ConfigurationManager.AppSettings("ConnectionString"))
            Session.Add("m_change", m_change)
            objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Session("objSuche") = objSuche
            Response.Redirect("Change51_1.aspx?AppID=" & Session("AppID").ToString & strTemp)
        Else
            Dim tmpHaendlernummer As String = ""
            tmpHaendlernummer = SucheHaendler1.giveHaendlernummer
            lbSelektionZurueckSetzen.Visible = True

            If Not tmpHaendlernummer Is Nothing Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                m_change = New fin_13(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                m_change.AppID = Session("AppID").ToString
                m_change.CreditControlArea = "ZDAD"

                m_change.Haendler = Right("0000000000" & tmpHaendlernummer, 10)
                Dim strTemp As String = ""
                HelpProcedures.getAppParameters(Session("AppID").ToString, strTemp, ConfigurationManager.AppSettings("ConnectionString"))
                Session.Add("m_change", m_change)
                objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                Session("objSuche") = objSuche
                Response.Redirect("Change51_1.aspx?AppID=" & Session("AppID").ToString & strTemp)
            End If
        End If

    End Sub
    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSelektionZurueckSetzen.Click
        SucheHaendler1.SelektionZuruecksetzen()
        lbSelektionZurueckSetzen.Visible = False
    End Sub

End Class
' ************************************************
' $History: Change51.aspx.vb $
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
' User: Jungj        Date: 30.07.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 12.06.08   Time: 16:53
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1979
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 5.06.08    Time: 16:50
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1988
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.03.08    Time: 14:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akf änderungen 1733
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 4.03.08    Time: 18:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF ANPASSUNGEN ITA 1733
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 4.03.08    Time: 10:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733, 1667, 1738 
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.02.08   Time: 15:05
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 25.02.08   Time: 13:52
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733, offene Anforderungen Bank hinzugefügt
' 
' ************************************************