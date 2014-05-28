Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change45
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lbSuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_Haendlernummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtPersonennummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lbl_FIN As System.Web.UI.WebControls.Label
    Protected WithEvents txtFIN As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Hersteller As System.Web.UI.WebControls.Label
    Protected WithEvents ddl_Haendler As System.Web.UI.WebControls.DropDownList
    Private objSuche As CKG.Components.ComCommon.Finance.Search
    Protected WithEvents lbl_Haendler As System.Web.UI.WebControls.Label
    Protected WithEvents SucheHaendler1 As SucheHaendler
    Protected WithEvents lbSelektionZurueckSetzen As System.Web.UI.WebControls.LinkButton

    Dim m_change As fin_11

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
        lblError.Text = ""

        If Not IsPostBack Then
            m_change = New fin_11(m_User, m_App, CStr(Session("AppID")), Me.Session.SessionID, "")
            m_change.SessionID = Me.Session.SessionID
            m_change.AppID = CStr(Session("AppID"))
            Session.Add("m_change", m_change)

            'kontingenttabelle ausblenden wenn Parameter
            If Request.QueryString("HDL") = 1 Then
                Session("AppShowNot") = True
            End If

        Else
            If m_change Is Nothing Then
                m_change = CType(Session("m_change"), fin_11)
            End If
        End If


    End Sub

    Private Sub lbSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbSuche.Click
        m_change.Haendlernummer = ""
        m_change.Fahrgestellnr = txtFIN.Text


        If SucheHaendler1.isInUse = False Then
            m_change.show(Session("AppID").ToString, Session.SessionID)
            If m_change.Status = 0 AndAlso Not m_change.Result Is Nothing Then
                Response.Redirect("Change45_1.aspx?AppID=" & Session("AppID").ToString)
            ElseIf m_change.Status < 0 Then
                lblError.Text = m_change.Message
            ElseIf m_change.Result.Rows.Count <= 0 Then
                lblError.Text = "keine Daten vorhanden"
            End If
        ElseIf SucheHaendler1.NotInUseButinBorder Then
            Dim tmpHaendlernummer As String = ""
            tmpHaendlernummer = SucheHaendler1.giveHaendlernummer
            lbSelektionZurueckSetzen.Visible = True

            If Not tmpHaendlernummer Is Nothing Then
                m_change.Haendlernummer = tmpHaendlernummer
                m_change.show(Session("AppID").ToString, Session.SessionID)
                If m_change.Status = 0 AndAlso Not m_change.Result Is Nothing Then
                    Response.Redirect("Change45_1.aspx?AppID=" & Session("AppID").ToString)
                ElseIf m_change.Status < 0 Then
                    lblError.Text = m_change.Message
                ElseIf m_change.Result.Rows.Count <= 0 Then
                    lblError.Text = "keine Daten vorhanden"
                End If
            Else
                m_change.show(Session("AppID").ToString, Session.SessionID)
                If m_change.Status = 0 AndAlso Not m_change.Result Is Nothing Then
                    Response.Redirect("Change45_1.aspx?AppID=" & Session("AppID").ToString)
                ElseIf m_change.Status < 0 Then
                    lblError.Text = m_change.Message
                ElseIf m_change.Result.Rows.Count <= 0 Then
                    lblError.Text = "keine Daten vorhanden"
                End If
            End If


        Else
            Dim tmpHaendlernummer As String = ""
            tmpHaendlernummer = SucheHaendler1.giveHaendlernummer
            lbSelektionZurueckSetzen.Visible = True

            If Not tmpHaendlernummer Is Nothing Then
                m_change.Haendlernummer = tmpHaendlernummer
                m_change.show(Session("AppID").ToString, Session.SessionID)
                If m_change.Status = 0 AndAlso Not m_change.Result Is Nothing Then
                    Response.Redirect("Change45_1.aspx?AppID=" & Session("AppID").ToString)
                ElseIf m_change.Status < 0 Then
                    lblError.Text = m_change.Message
                ElseIf m_change.Result.Rows.Count <= 0 Then
                    lblError.Text = "keine Daten vorhanden"
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

    Private Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSelektionZurueckSetzen.Click
        SucheHaendler1.SelektionZuruecksetzen()
        lbSelektionZurueckSetzen.Visible = False
    End Sub
End Class
' ************************************************
' $History: Change45.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.07.09   Time: 17:09
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.06.09   Time: 16:58
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918  Z_M_Gesperrte_Auftraege_001
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.03.09   Time: 17:17
' Updated in $/CKAG/Components/ComCommon/Finance
' Rtfs Händlerkontingente + css visted anpassung
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.07.08   Time: 10:21
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2119
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
' *****************  Version 9  *****************
' User: Jungj        Date: 14.01.08   Time: 17:21
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1619
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 8.01.08    Time: 12:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Authorisierung Freigabe gesperrter Aufträge
' 
' ************************************************
