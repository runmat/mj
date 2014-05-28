Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change01
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents txtFaelligkeit As TextBox

    Protected WithEvents lb_weiter As LinkButton

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents rblKurzwahl As RadioButtonList

    Private mObjUeberfaelligeRuecksendungen As UeberfaelligeRuecksendungen





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

            'seitenspezifisch:
            If Not IsPostBack Then
                If Not Me.Request.QueryString.Get("AnzahlTage") Is Nothing Then
                    txtFaelligkeit.Text = Me.Request.QueryString.Get("AnzahlTage")
                    doSubmit()
                End If
            End If

        Catch ex As Exception
            lblError.Text = "beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_weiter.Click
        doSubmit()
    End Sub

    Private Sub doSubmit()
        If IsNumeric(txtFaelligkeit.Text) Then
            mObjUeberfaelligeRuecksendungen = New UeberfaelligeRuecksendungen(m_User, m_App, Session("AppId").ToString, Me.Session.SessionID, "")
            mObjUeberfaelligeRuecksendungen.Faelligkeit = txtFaelligkeit.Text
            mObjUeberfaelligeRuecksendungen.Show()
            If mObjUeberfaelligeRuecksendungen.Status = 0 Then
                Session.Add("mObjUeberfaelligeRuecksendungenSession", mObjUeberfaelligeRuecksendungen)
                Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
            Else
                lblError.Text = mObjUeberfaelligeRuecksendungen.Message
                Exit Sub
            End If
        Else
            lblError.Text = "bitte tragen Sie einen nummerischen Wert ein"
        End If
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub rblKurzwahl_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblKurzwahl.SelectedIndexChanged
        txtFaelligkeit.Text = rblKurzwahl.SelectedValue
    End Sub
End Class
' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.11.08    Time: 13:34
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2365,2367,2362
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 27.10.08   Time: 17:20
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2286 Änderungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 14.10.08   Time: 15:14
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2286 Änderung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 14.10.08   Time: 13:58
' Created in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2286 unfertig
' 
' ************************************************