Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.Globalization
Imports System.Web.UI

Public Class Report29
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


            Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "window.open('https://sgwt.kroschke.de/cockpitv6pro','_blank')", True)


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub


End Class