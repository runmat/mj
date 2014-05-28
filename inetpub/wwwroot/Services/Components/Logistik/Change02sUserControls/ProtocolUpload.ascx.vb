Imports System.IO
Imports Telerik.Web.UI
Imports System.Configuration

Public Class ProtocolUpload
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterClientScriptBlock(Page, GetType(ProtocolUpload), "async_upload", "function onUploadFailed(sender, args) { " & _
                                                "alert(args.get_message()); " & _
                                                "} " & _
                                                "function onFileUploaded(sender, args) { " & _
                                                "document.getElementById('" & submitFiles.ClientID & "').click(); }", True)

        'If IsPostBack AndAlso uploadsGrid.Rows.Count > 0 Then
        '    For Each r As GridViewRow In uploadsGrid.Rows
        '        Dim rau = DirectCast(r.FindControl("upload"), RadAsyncUpload)
        '        If Not rau Is Nothing Then
        '            For Each uf As UploadedFile In rau.UploadedFiles
        '                System.Diagnostics.Trace.WriteLine(uf.FileName)
        '            Next
        '        End If
        '    Next
        'End If
    End Sub

    Public Property Protokolle As IList(Of Protokoll)
        Get
            Dim ret As IList(Of Protokoll) = DirectCast(Me.ViewState("Protokolle"), IList(Of Protokoll))

            If ret Is Nothing Then
                ret = New List(Of Protokoll)()
                Me.ViewState("Protokolle") = ret
            End If

            Return ret
        End Get
        Set(value As IList(Of Protokoll))
            Me.ViewState("Protokolle") = value
        End Set
    End Property

    Public Sub FillGrid()
        uploadsGrid.DataSource = Protokolle
        uploadsGrid.DataBind()
        Visible = uploadsGrid.Rows.Count > 0
    End Sub

    Public ReadOnly Property NeedsInitialization() As Boolean
        Get
            Return uploadsGrid.Rows.Count = 0 AndAlso uploadsGrid.DataSource Is Nothing
        End Get
    End Property

    Protected Sub UploadsRowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName = "DeleteProtokoll" Then
            Dim protokoll = Protokolle.ElementAtOrDefault(Integer.Parse(e.CommandArgument))
            If protokoll Is Nothing Then Return

            If Not String.IsNullOrEmpty(protokoll.Tempfilename) Then
                If File.Exists(protokoll.Tempfilename) Then
                    File.Delete(protokoll.Tempfilename)
                End If
                protokoll.Tempfilename = String.Empty
                FillGrid()
            End If
        End If
    End Sub

    Protected Sub UploadedComplete(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
        Dim uploader = DirectCast(sender, RadAsyncUpload)
        Dim row = DirectCast(uploader.BindingContainer, GridViewRow)
        Dim protokoll = Protokolle.ElementAtOrDefault(row.DataItemIndex)

        Dim filename = String.Format("{0}_{1:yyyyMMddhhmmss}.pdf", protokoll.Protokollart.Replace(".", ""), DateTime.Now)
        Dim tempFolder = ConfigurationManager.AppSettings("LogistikProtocolUploadTemp")
        If String.IsNullOrEmpty(tempFolder) Then
            tempFolder = "C:\inetpub\wwwroot\Services\Components\Logistik\Dokumente\TempDoc"
        End If
        tempFolder = Path.GetFullPath(tempFolder)

        If Not Directory.Exists(tempFolder) Then
            Directory.CreateDirectory(tempFolder)
        End If
        Dim tempfilename = Path.Combine(tempFolder, filename)

        e.File.SaveAs(tempfilename)

        protokoll.Tempfilename = tempfilename
        FillGrid()
    End Sub

    Friend Function ValidateUploads() As Boolean
        Dim blnFileNotFound As Boolean = False

        Try
            'prüfen, ob hochgeladene Dateien auch wirklich auf dem Server vorhanden sind
            For Each p As Protokoll In Me.Protokolle
                If p IsNot Nothing AndAlso Not String.IsNullOrEmpty(p.Tempfilename) AndAlso Not File.Exists(p.Tempfilename) Then
                    blnFileNotFound = True
                    p.Tempfilename = ""
                End If
            Next

            If blnFileNotFound Then
                FillGrid()
                Return False
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class