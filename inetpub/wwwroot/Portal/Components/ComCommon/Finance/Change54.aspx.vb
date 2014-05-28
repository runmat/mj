Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Components.ComCommon

Public Class Change54
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents SucheHaendler1 As SucheHaendler
    Protected WithEvents lbSelektionZurueckSetzen As System.Web.UI.WebControls.LinkButton

    Protected WithEvents lb_weiter As LinkButton

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Private mObjSuche As CKG.Components.ComCommon.Finance.Search
    Private m_context As HttpContext = HttpContext.Current
    Private objBank As CKG.Components.ComCommon.BankBaseCredit
    Private mObjInanspruchnahme As Inanspruchnahme





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

        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblError.Text = ""



        If Not IsPostBack Then
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Not Me.Request.QueryString.Item("Haendlernummer") Is Nothing Then
                doSubmit(Me.Request.QueryString.Item("Haendlernummer"))
            End If

        End If

    End Sub

    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_weiter.Click
        doSubmit()
    End Sub

    Private Sub doSubmit(Optional ByVal linkedHaendlernummer As String = Nothing)

        Dim tmpHaendlernummer As String
        If linkedHaendlernummer Is Nothing Then
            tmpHaendlernummer = SucheHaendler1.giveHaendlernummer
        Else
            tmpHaendlernummer = linkedHaendlernummer
        End If


        If Not tmpHaendlernummer Is Nothing Then
            tmpHaendlernummer = Right("0000000000" & tmpHaendlernummer, 10)
        End If


        If SucheHaendler1.isInUse = False Then
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            mObjInanspruchnahme = New Inanspruchnahme(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            mObjInanspruchnahme.AppID = Session("AppID").ToString
            mObjInanspruchnahme.CreditControlArea = "ZDAD"
            Dim strTemp As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, strTemp, System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
            Session.Add("mObjInanspruchnahmeSession", mObjInanspruchnahme)
            mObjSuche = New CKG.Components.ComCommon.Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Session.Add("mObjSucheSession", mObjSuche)

            mObjInanspruchnahme.Haendler = tmpHaendlernummer
            mObjInanspruchnahme.Show()
            If mObjInanspruchnahme.Status = 0 Then
                Response.Redirect("Change54_1.aspx?AppID=" & Session("AppID").ToString & strTemp)
            Else
                lblError.Text = mObjInanspruchnahme.Message
                Exit Sub
            End If

        Else


            lbSelektionZurueckSetzen.Visible = True

            If Not tmpHaendlernummer Is Nothing Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                mObjInanspruchnahme = New Inanspruchnahme(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
                mObjInanspruchnahme.AppID = Session("AppID").ToString
                mObjInanspruchnahme.CreditControlArea = "ZDAD"

                mObjInanspruchnahme.Haendler = Right("0000000000" & tmpHaendlernummer, 10)
                Dim strTemp As String = ""
                HelpProcedures.getAppParameters(Session("AppID").ToString, strTemp, System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
                Session.Add("mObjInanspruchnahmeSession", mObjInanspruchnahme)
                mObjSuche = New CKG.Components.ComCommon.Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                Session.Add("mObjSucheSession", mObjSuche)

                mObjInanspruchnahme.Haendler = tmpHaendlernummer
                mObjInanspruchnahme.Show()
                If mObjInanspruchnahme.Status = 0 Then
                    Response.Redirect("Change54_1.aspx?AppID=" & Session("AppID").ToString & strTemp)
                Else
                    lblError.Text = mObjInanspruchnahme.Message
                    Exit Sub
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
' $History: Change54.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 25.11.08   Time: 10:51
' Updated in $/CKAG/Components/ComCommon/Finance
' Bugfix Inanspruchnahme
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 18.11.08   Time: 13:49
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2263 testfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 10.10.08   Time: 16:51
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2246 fertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 10.10.08   Time: 11:48
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2246 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 8.10.08    Time: 10:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2246 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 7.10.08    Time: 16:44
' Created in $/CKAG/Components/ComCommon/Finance
' ITA 2246 Torso
' 
' ************************************************