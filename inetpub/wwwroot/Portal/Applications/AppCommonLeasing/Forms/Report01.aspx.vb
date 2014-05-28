Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report01
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lbl_Info As Label

    Protected WithEvents lb_weiter As LinkButton

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Private mObjGesamtbestand As Gesamtbestand





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
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""

            If Not IsPostBack Then
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

        Catch ex As Exception
            lblError.Text = "beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_weiter.Click
        doSubmit()
    End Sub

    Private Sub doSubmit()
        mObjGesamtbestand = New Gesamtbestand(m_User, m_App, Session("AppId").ToString, Me.Session.SessionID, "")
        mObjGesamtbestand.Fill()
        If mObjGesamtbestand.Status = 0 Then
            Session.Add("mObjGesamtbestandSession", mObjGesamtbestand)
            Response.Redirect("Report01_1.aspx?AppID=" & Session("AppID").ToString)
        Else
            lblError.Text = mObjGesamtbestand.Message
            Exit Sub
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
' $History: Report01.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.10.08   Time: 16:47
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2289 warte auf bapi
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.10.08   Time: 15:25
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ita 2289 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 30.10.08   Time: 14:26
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2289 
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 14.10.08   Time: 15:14
' Created in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2289 unfertig
'
' ************************************************