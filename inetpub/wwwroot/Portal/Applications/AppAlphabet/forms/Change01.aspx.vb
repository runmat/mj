Option Explicit On 
Option Strict On

Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change01
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Suche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Neu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtSucheLiznr As System.Web.UI.WebControls.TextBox

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Dim mObjVertragsdaten As Vertragsdaten
    
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
        Try
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte 
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

            FormAuth(Me, m_User)
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Not IsPostBack Then
                mObjVertragsdaten = New Vertragsdaten(m_User, m_App, "")
                mObjVertragsdaten.SessionID = Session.SessionID
                mObjVertragsdaten.AppID = CStr(Session("AppID"))
                Session.Add("objVertragsdatenSession", mObjVertragsdaten)
            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try

    End Sub

    Private Sub lb_Neu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Neu.Click
        If mObjVertragsdaten Is Nothing Then
            mObjVertragsdaten = CType(Session("objVertragsdatenSession"), Vertragsdaten)
        End If
        mObjVertragsdaten.CreateNewVertrag(txtSucheLiznr.Text)
        Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub lb_Suche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Suche.Click
        
        If mObjVertragsdaten Is Nothing Then
            mObjVertragsdaten = CType(Session("objVertragsdatenSession"), Vertragsdaten)
        End If
        
        mObjVertragsdaten.SucheLiznr = txtSucheLiznr.Text.ToUpper

        'füllen der Datentabelle des Auftrags
        mObjVertragsdaten.Fill()

        If Not mObjVertragsdaten.Status = 0 Then
            If mObjVertragsdaten.Status = -1111 Then
                lb_Neu.Visible = True
            End If
            lblError.Text = mObjVertragsdaten.Message 'fehlermeldung ausgeben
            'Teams füllen
            mObjVertragsdaten.FillTeam()

            Exit Sub
        Else
            Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
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
' $History: Change01.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 17.12.08   Time: 10:43
' Updated in $/CKAG/Applications/AppAlphabet/forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.08.08    Time: 13:37
' Updated in $/CKAG/Applications/AppAlphabet/forms
' 2133 weiterentwicklung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.08.08    Time: 13:26
' Updated in $/CKAG/Applications/AppAlphabet/forms
' Weiterentwiclung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 31.07.08   Time: 16:52
' Created in $/CKAG/Applications/AppAlphabet/forms
' ITA 2133 body
' 
' ************************************************

