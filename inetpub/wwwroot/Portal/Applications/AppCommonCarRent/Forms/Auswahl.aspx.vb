Option Explicit On
Option Strict On
Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Auswahl
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            lblError.Text = "Beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Protected Sub lb_Weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Weiter.Click
        Select Case rbl_Auswahl.SelectedIndex

            Case 0
                Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString & "&Art=BZKennz")
            Case 1
                Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString & "&Art=Endg")
            Case 2
                Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString & "&Art=Sperr")
            Case 3
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)

        End Select
    End Sub
End Class