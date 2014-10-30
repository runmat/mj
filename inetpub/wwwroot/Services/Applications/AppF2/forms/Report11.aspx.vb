Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports System.IO

Public Class Report11
    Inherits Page

#Region "Declarations"

    Private m_User As User

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If IsPostBack = False Then
            GetFiles()
        End If
    End Sub

    Private Sub GetFiles()
        Try
            Dim fileSourcePath As String = ConfigurationManager.AppSettings("DownloadPathTargoRetail")

            Dim DirInfo As New DirectoryInfo(fileSourcePath)
            Dim files() As FileInfo = DirInfo.GetFiles("*.csv")

            If files.Length > 0 Then
                For Each fi As FileInfo In files

                    If fi.Name.StartsWith("CONTROLLINGDATEI_ASF", StringComparison.InvariantCultureIgnoreCase) Then
                        lbtDownloadASF.CommandArgument = fileSourcePath & "\" & fi.Name

                    ElseIf fi.Name.StartsWith("CONTROLLINGDATEI_EKF", StringComparison.InvariantCultureIgnoreCase) Then
                        lbtDownloadEKF.CommandArgument = fileSourcePath & "\" & fi.Name

                    ElseIf fi.Name.StartsWith("CONTROLLINGDATEI_OAK", StringComparison.InvariantCultureIgnoreCase) Then
                        lbtDownloadOAK.CommandArgument = fileSourcePath & "\" & fi.Name

                    End If

                Next
            End If

            'ASF
            Dim blnASF As Boolean = Not String.IsNullOrEmpty(lbtDownloadASF.CommandArgument)
            lbtDownloadASF.Visible = blnASF
            lblErrorASF.Text = IIf(blnASF, "", "Es wurde keine ASF-Datei bereitgestellt.")
            'EKF
            Dim blnEKF As Boolean = Not String.IsNullOrEmpty(lbtDownloadEKF.CommandArgument)
            lbtDownloadEKF.Visible = blnEKF
            lblErrorEKF.Text = IIf(blnEKF, "", "Es wurde keine EKF-Datei bereitgestellt.")
            'OAK
            Dim blnOAK As Boolean = Not String.IsNullOrEmpty(lbtDownloadOAK.CommandArgument)
            lbtDownloadOAK.Visible = blnOAK
            lblErrorOAK.Text = IIf(blnOAK, "", "Es wurde keine OAK-Datei bereitgestellt.")

        Catch ex As Exception
            lblError.Text = "Fehler beim Abrufen der Datei(en)."
        End Try

    End Sub

    Protected Sub lbtDownload_Click(sender As Object, e As EventArgs) Handles lbtDownloadASF.Click, lbtDownloadEKF.Click, lbtDownloadOAK.Click

        Dim downloadFile As New FileInfo(CType(sender, LinkButton).CommandArgument)

        With HttpContext.Current.Response
            .Clear()
            .AddHeader("Content-Disposition", String.Format("attachment; filename={0}", downloadFile.Name))
            .AddHeader("Content-Length", downloadFile.Length.ToString())
            .ContentType = "application/octet-stream"
            .WriteFile(downloadFile.FullName)
            .End()
        End With

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class