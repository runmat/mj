Imports System
Imports System.IO
Imports System.Linq
Imports System.Collections.Generic

Public Class ReviewFilename

    ''' <summary>
    ''' Format to create a valid filename for Uploads
    ''' 0=Auftrag
    ''' 1=Index/Counter
    ''' 2=FahrerID
    ''' 3=Tour/Fahrt
    ''' 4=Trenner
    ''' </summary>
    Const FilenameFormat As String = "{0}{4}{1:0000}{4}{2:0000}{4}{3:0}"
    Const FileNameDelimiter As Char = "-"c
    Protected Const FileExtensionPdf As String = ".PDF"
    Protected Shared FileExtensionsImage As String() = {".JPG", ".JPEG", ".PNG", ".GIF"}
    Protected Const ThumbPrefix As String = "THUMB_"

    Public Shared Function Create(Optional ByVal auftragsNummer As Nullable(Of Integer) = Nothing, Optional ByVal fahrer As Nullable(Of Integer) = Nothing, Optional ByVal fahrt As Nullable(Of Integer) = Nothing, Optional ByVal index As Nullable(Of Integer) = Nothing) As String
        Dim parameters As New List(Of Object)
        If (auftragsNummer.HasValue) Then
            parameters.Add(auftragsNummer.Value)
        Else
            parameters.Add("*")
        End If
        If (index.HasValue) Then
            parameters.Add(index.Value)
        Else
            parameters.Add("*")
        End If
        If (fahrer.HasValue) Then
            parameters.Add(fahrer.Value)
        Else
            parameters.Add("*")
        End If
        If (fahrt.HasValue) Then
            parameters.Add(fahrt.Value)
        Else
            parameters.Add("*")
        End If
        parameters.Add(FileNameDelimiter)
        Return String.Format(FilenameFormat, parameters.ToArray)
    End Function

    Public Shared Function NextFilename(ByVal folder As String, ByVal extension As String, ByVal auftragsNummer As Integer, ByVal fahrer As Integer, ByVal fahrt As Integer) As String
        Dim pattern = Create(auftragsNummer, fahrer, fahrt)
        Dim endung = "." & extension.TrimStart("."c)
        pattern &= endung
        Dim files() As String = Directory.GetFiles(folder, pattern)
        If (files.Length = 0) Then
            Return Path.Combine(folder, Create(auftragsNummer, fahrer, fahrt, 0) & endung)
        Else
            Dim maxIndex = ReviewFilename.MaxIndex(files)
            Return Path.Combine(folder, Create(auftragsNummer, fahrer, fahrt, maxIndex + 1) & endung)
        End If
    End Function

    Public Shared Function MaxIndex(ByVal filenames As IEnumerable(Of String)) As Integer
        If (filenames.Count() = 0) Then Return 0

        Dim max = filenames.Select(Function(filename) Path.GetFileNameWithoutExtension(filename).Split(FileNameDelimiter)(1)).Max()
        Return Integer.Parse(max)
    End Function

    Public Sub New(ByVal filename As String)
        Dim parts = Path.GetFileNameWithoutExtension(filename).Split(FileNameDelimiter)
        If parts.Length <> 4 Then
            Throw New ApplicationException("Unerwartetes Format - Dateiname: '" + filename + "'")
        End If
        AuftragsNummer = Integer.Parse(parts(0))
        Index = Integer.Parse(parts(1))
        FahrerID = Integer.Parse(parts(2))
        Fahrt = Integer.Parse(parts(3))
        IsImage = FileExtensionsImage.Contains(Path.GetExtension(filename).ToUpper())
    End Sub

    Public Property AuftragsNummer As Integer
    Public Property FahrerID As Integer
    Public Property Fahrt As Integer
    Public Property Index As Integer
    Public Property IsImage As Boolean
End Class
