Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Partial Public Class Change02s_1
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Private m_App As Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        If Not Session("App_Filepath") Is Nothing Then
            Dim sPfad As String = Session("App_Filepath").ToString
            Response.ContentType = "Application/pdf"
            'Write the file directly to the HTTP output stream.
            Response.WriteFile(sPfad & ".pdf")
            Response.End()
            Session("App_Filepath") = Nothing
        End If
    End Sub

End Class