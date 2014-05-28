Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports System.IO

Public Class Report11
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        m_App = New App(m_User)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString


        If IsPostBack = False Then
            GetFile()
        End If



    End Sub

    Private Sub GetFile()
        Dim fileSourcePath As String

        Try

            fileSourcePath = CType(ConfigurationManager.AppSettings("DownloadPathTargoRetail"), String)

            Dim DirInfo As New DirectoryInfo(fileSourcePath)
            Dim files() As FileInfo = DirInfo.GetFiles("*.csv")

            If files.Length = 0 Then
                lblError.Text = "Es wurde keine Datei bereitgestellt."
            ElseIf files.Length > 1 Then
                lblError.Text = "Verzeichnis enthält mehr als eine Datei."
            Else
                'hplExcel.Text = "Controllingdatei"
                lbtDownload.CommandArgument = fileSourcePath & "\" & files(0).Name
                lbtDownload.Visible = True
            End If


        Catch ex As Exception
            lblError.Text = "Fehler beim Abrufen der Datei."
        End Try


    End Sub


    Protected Sub lbtDownload_Click(sender As Object, e As EventArgs) Handles lbtDownload.Click

        Dim downloadFile As FileInfo = New FileInfo(lbtDownload.CommandArgument)
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", downloadFile.Name))
        HttpContext.Current.Response.AddHeader("Content-Length", downloadFile.Length.ToString())
        HttpContext.Current.Response.ContentType = "application/octet-stream"
        HttpContext.Current.Response.WriteFile(downloadFile.FullName)
        HttpContext.Current.Response.End()



  
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class