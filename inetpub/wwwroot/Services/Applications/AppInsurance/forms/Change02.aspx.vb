Option Explicit On
Option Strict On

Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Public Class Change02
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label

    Protected WithEvents lb_weiter As LinkButton
    Protected WithEvents lbBack As LinkButton

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents txtLeasingvertragsnummer As TextBox
    Protected WithEvents txtKennzeichen As TextBox
    Protected WithEvents txtSuchname As TextBox
    Protected WithEvents txtFahrgestellnummer As TextBox


    Private mObjBriefanforderung As Briefanforderung





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
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            FormAuth(Me, m_User)
            lblError.Text = ""

            ' Me.ClientScript.RegisterForEventValidation(
            If Not IsPostBack Then
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            End If

        Catch ex As Exception
            lblError.Text = "beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub
    Private Sub doSubmit()

        If (txtFahrgestellnummer.Text.Replace("*", "").Trim(" "c) = "" AndAlso txtKennzeichen.Text.Replace("*", "").Trim(" "c) = "" AndAlso txtLeasingvertragsnummer.Text.Replace("*", "").Trim(" "c) = "" AndAlso txtSuchname.Text.Replace("*", "").Trim(" "c) = "") Then
            lblError.Text = "Geben Sie bitte ein Suchkriterium ein"
        Else
            mObjBriefanforderung = New Briefanforderung(m_User, m_App, Session("AppId").ToString, Me.Session.SessionID, "")
            mObjBriefanforderung.SucheFahrgestellnummer = txtFahrgestellnummer.Text.Trim(" "c)
            mObjBriefanforderung.SucheKennzeichen = txtKennzeichen.Text.Trim(" "c)
            mObjBriefanforderung.SucheLeasingvertragsnummer = txtLeasingvertragsnummer.Text.Trim(" "c)
            mObjBriefanforderung.SucheSuchname = txtSuchname.Text.Trim(" "c)

            mObjBriefanforderung.Show()
            If mObjBriefanforderung.Status = 0 Then
                Session.Add("mObjBriefanforderungSession", mObjBriefanforderung)
                Dim Parameterlist As String = ""
                HelpProcedures.getAppParameters(Session("AppID").ToString, Parameterlist, ConfigurationManager.AppSettings.Get("Connectionstring"))
                Response.Redirect("Change02_1.aspx?AppID=" & Session("AppID").ToString & Parameterlist)
            Else
                lblError.Text = mObjBriefanforderung.Message
                Exit Sub
            End If
        End If
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lb_Weiter_Click1(ByVal sender As Object, ByVal e As EventArgs) Handles lb_weiter.Click
        doSubmit()
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub


End Class
' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 2.03.09    Time: 14:10
' Updated in $/CKAG2/Applications/AppGenerali/forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 24.02.09   Time: 12:48
' Updated in $/CKAG2/Applications/AppGenerali/forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.02.09   Time: 10:21
' Updated in $/CKAG2/Applications/AppGenerali/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 23.02.09   Time: 14:50
' Created in $/CKAG2/Applications/AppGenerali/forms
' briefanforderung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.11.08    Time: 13:34
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2365,2367,2362
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.10.08   Time: 11:15
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 Weiterentwicklung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.10.08   Time: 17:11
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 weiterentwicklung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 17.10.08   Time: 10:44
' Created in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 torso
' 
' ************************************************