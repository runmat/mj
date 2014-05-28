Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements

Partial Public Class Report014_1
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Private m_App As Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        If Not Session("App_Filepath") Is Nothing Then
            Dim sPfad As String = Session("App_Filepath").ToString

            If Session("App_ContentDisposition") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Session("App_ContentDisposition").ToString()) Then
                Dim strContentDisposition As String = Session("App_ContentDisposition").ToString()
                If sPfad.Contains("\") Then
                    Response.AddHeader("Content-Disposition", strContentDisposition & "; filename=" & sPfad.Substring(sPfad.LastIndexOf("\"c) + 1, sPfad.Length - (sPfad.LastIndexOf("\"c) + 1)))
                Else
                    Response.AddHeader("Content-Disposition", strContentDisposition & "; filename=" & sPfad)
                End If
            End If

            Response.ContentType = Session("App_ContentType").ToString
            'Get the physical path to the file.
            Dim FilePath As String = sPfad
            'Write the file directly to the HTTP output stream.
            Response.WriteFile(FilePath)
            Response.End()
        End If
    End Sub

End Class