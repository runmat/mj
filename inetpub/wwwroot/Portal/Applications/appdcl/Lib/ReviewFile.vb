Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq
Imports System.Collections.Generic

Public Class ReviewFile
    Inherits ReviewFilename

#Region "Properties"
    Public Property SourceFile As String

    Public ReadOnly Property SourceFileExists As Boolean
        Get
            Return File.Exists(SourceFile)
        End Get
    End Property

    Public Property BackupFile As String

    Public ReadOnly Property BackupFileExists As Boolean
        Get
            Return File.Exists(BackupFile)
        End Get
    End Property

    Public Property ServerFile As String

    Public Property Selected As Boolean

    Public ReadOnly Property Filename As String
        Get
            Return Path.GetFileName(SourceFile)
        End Get
    End Property

    Public ReadOnly Property Extension As String
        Get
            Return Path.GetExtension(SourceFile)
        End Get
    End Property
#End Region
    
    Public Shared Function FindUploadedFiles(ByVal folder As String, ByVal backupFolder As String, ByVal serverFolder As String, ByVal images As Boolean,
                                             Optional ByVal auftragsNummer As Nullable(Of Integer) = Nothing, Optional ByVal fahrer As Nullable(Of Integer) = Nothing,
                                             Optional ByVal fahrt As Nullable(Of Integer) = Nothing,
                                             Optional ByVal index As Nullable(Of Integer) = Nothing) As List(Of ReviewFile)
        Dim files As List(Of String)
        Dim pattern = Create(auftragsNummer, fahrer, fahrt)
        If images Then
            pattern &= ".*"
            files = Directory.GetFiles(folder, pattern).Where(Function(f) f.ToUpper.EndsWith(FileExtensionsImage(0)) OrElse f.ToUpper.EndsWith(FileExtensionsImage(1)) OrElse f.ToUpper.EndsWith(FileExtensionsImage(2)) OrElse f.ToUpper.EndsWith(FileExtensionsImage(3))).ToList()
        Else
            pattern &= FileExtensionPdf
            files = Directory.GetFiles(folder, pattern).ToList()
        End If
        Return files _
            .Select(Function(fileName) New ReviewFile(fileName, backupFolder, serverFolder)) _
            .ToList()
    End Function

    Public Sub New(ByVal filename As String, ByVal backupFolder As String, ByVal serverFolder As String)
        MyBase.New(filename)

        SourceFile = filename
        If Not String.IsNullOrEmpty(backupFolder) Then BackupFile = Path.Combine(backupFolder, Path.GetFileName(filename))
        If Not String.IsNullOrEmpty(serverFolder) Then ServerFile = Path.Combine(serverFolder, Path.GetFileName(filename))
    End Sub

    Public Sub Delete()
        If SourceFileExists Then File.Delete(SourceFile)
        If BackupFileExists Then File.Delete(BackupFile)
    End Sub

    Public Sub MoveTo(ByVal folder As String, ByVal extension As String, ByVal backupFolder As String, ByVal auftrag As ReviewAuftrag)
        Dim destFile = NextFilename(folder, extension, auftrag.AuftragsNummer, auftrag.FahrerID, auftrag.Fahrt)
        If SourceFileExists Then
            File.Move(SourceFile, destFile)
        End If

        ' Nur den Ordnernamen im Pfad austauschen, damit Bild und Backup den selben Dateinamen haben
        destFile = destFile.Replace(folder, backupFolder)
        'destFile = NextFilename(backupFolder, IsImage, auftrag.AuftragsNummer, auftrag.FahrerID, auftrag.Fahrt)
        If BackupFileExists Then
            File.Move(BackupFile, destFile)
        End If
    End Sub

    Public Sub Archive(ByVal folder As String)
        Dim destFile = Path.Combine(folder, Filename)
        If SourceFileExists Then
            File.Copy(SourceFile, destFile)
            If File.Exists(destFile) Then
                Dim thumbfile = Path.Combine(Path.GetDirectoryName(destFile), ThumbPrefix & Path.GetFileName(destFile))
                If Not CreateThumbnail(destFile, thumbfile) Then
                    Throw New ApplicationException("Fehler beim Erstellen der Vorschau")
                End If

                If SourceFileExists Then File.Delete(SourceFile)
                If BackupFileExists Then File.Delete(BackupFile)
            End If
        End If
    End Sub

    Private Function CreateThumbnail(ByVal srcfile As String, ByVal thumbfile As String, Optional ByVal width As Integer = 75,
                                     Optional ByVal height As Integer = 75) As Boolean
        If Not File.Exists(srcfile) Then Return False

        Dim sourceStream = File.OpenRead(srcfile)
        Dim sourceImage = Image.FromStream(sourceStream)
        Dim destImage = New Bitmap(width, height)

        Dim destG = Graphics.FromImage(destImage)
        destG.DrawImage(sourceImage, 0, 0, width, height)
        destG.Dispose()

        sourceImage.Dispose()
        sourceStream.Close()

        destImage.Save(thumbfile, ImageFormat.Jpeg)
        destImage.Dispose()
        Return True
    End Function
End Class
