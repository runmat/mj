Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report10
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_BriefeundCOC As BriefeundCOC

#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property



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
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If

                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

            If m_BriefeundCOC Is Nothing Then
                If Not Session("AppTypenscheinohneCOC") Is Nothing Then
                    m_BriefeundCOC = CType(Session("AppTypenscheinohneCOC"), BriefeundCOC)
                Else
                    m_BriefeundCOC = New BriefeundCOC(m_User, m_App, "")
                    Session("AppTypenscheinohneCOC") = m_BriefeundCOC
                End If

            End If

            If Not IsPostBack Then

            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_zurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Protected Sub cmdWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdWeiter.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim errorText As String = ""
        If HelpProcedures.checkDate(txtERDatvon, txtERDatbis, errorText, True, 1) Then

            m_BriefeundCOC.ErdatVon = txtERDatvon.Text.Trim(" "c)
            m_BriefeundCOC.ErdatBis = txtERDatbis.Text.Trim(" "c)
            m_BriefeundCOC.FILL_TypenscheinOhneCOC(Session("AppID").ToString, Me.Session.SessionID)
            If m_BriefeundCOC.Status = 0 Then

                Session.Add("AppTypenscheinohneCOC", m_BriefeundCOC)
                Response.Redirect("Report10_1.aspx?AppID=" & Session("AppID").ToString)
            Else
                lblError.Text = m_BriefeundCOC.Message
                Exit Sub
            End If

        Else
            lblError.Text = errorText
        End If
    End Sub
End Class