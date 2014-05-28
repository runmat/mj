Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business




Public Class Change41
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lbSuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbSelektionZurueckSetzen As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Dim m_change As fin_05
    Dim objSuche As CKG.Components.ComCommon.Finance.Search
    Protected WithEvents SucheHaendler1 As SucheHaendler
    Dim strErrorText As String

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
            m_change = New fin_05(m_User, m_App, CStr(Session("AppID")), Session.SessionID, "")
            m_change.SessionID = Session.SessionID
            m_change.AppID = CStr(Session("AppID"))
            Session.Add("m_change", m_change)

        Else
            If m_change Is Nothing Then
                m_change = CType(Session("m_change"), fin_05)
            End If
        End If

    End Sub

    Private Sub lb_Suche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbSuche.Click
        If SucheHaendler1.IsInUse = False Then
            Response.Redirect("Change41_1.aspx?AppID=" & Session("AppID").ToString)
        Else
            Dim tmpHaendlernummer As String = SucheHaendler1.giveHaendlernummer
            lbSelektionZurueckSetzen.Visible = True

            If Not tmpHaendlernummer Is Nothing Then
                m_change.personenennummer = tmpHaendlernummer
                Response.Redirect("Change41_1.aspx?AppID=" & Session("AppID").ToString)
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
' $History: Change41.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.07.08   Time: 9:52
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 6.06.08    Time: 8:24
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 1988
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon/Finance
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
' User: Jungj        Date: 14.01.08   Time: 17:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1619
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 7.01.08    Time: 14:32
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1499 Verbesserungen Change41_X
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.12.07   Time: 14:10
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1466/1499 (Fällige Vorgänge) Kompilierfähig = unfertig eingefügt
' 
' ************************************************
