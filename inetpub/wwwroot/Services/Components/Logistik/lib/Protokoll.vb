Imports System.IO
Imports System.Configuration

<Serializable()>
Public Class Protokoll
    Implements IEquatable(Of Protokoll)

    Public Shared Function FromRow(row As DataRow, Optional ByVal fahrt As String = Nothing) As Protokoll
        Dim kategorie = CStr(row("ZZKATEGORIE"))
        Dim protokollart = CStr(row("ZZPROTOKOLLART"))
        Dim tempfilename = CStr(row("Tempfilename"))
        Dim filename = CStr(row("Filename"))
        fahrt = If(fahrt, CStr(row("Fahrt")))

        Return New Protokoll() With {.Fahrt = fahrt, .Kategorie = kategorie, .Protokollart = protokollart, .Tempfilename = tempfilename, .Filename = filename}
    End Function

    <NonSerialized()>
    Private _fahrzeug As Fahrzeug

    Friend Sub SetFahrzeug(fahrzeug As Fahrzeug)
        _fahrzeug = fahrzeug
    End Sub

    Public ReadOnly Property Fahrzeug As Fahrzeug
        Get
            Return _fahrzeug
        End Get
    End Property

    Public Property Fahrt As String
    Public Property Kategorie As String
    Public Property Protokollart As String
    Public Property Tempfilename As String
    Public Property Filename As String

    Public Sub Transfer(vblen As String, kundennummer As String)
        If String.IsNullOrEmpty(Tempfilename) OrElse Not File.Exists(Tempfilename) Then Return
        If _fahrzeug Is Nothing Then Return

        Filename = String.Format("{0}_{1}_{2}_{3}.pdf", vblen, Fahrt, Kategorie, Protokollart.Replace(".", ""))
        Dim destPath = Path.Combine(ConfigurationManager.AppSettings("UploadPathSambaArchive"), kundennummer.PadLeft(10, "0"c) + "\" + vblen + "\Vertraege\")

        If Not Directory.Exists(destPath) Then Directory.CreateDirectory(destPath)

        File.Copy(Tempfilename, Path.Combine(destPath, Filename))

        ' Temporäre Dateien werden von einem Löschjob entfernt - hier also nicht nötig..
    End Sub

    Public Overloads Function Equals(other As Protokoll) As Boolean Implements IEquatable(Of Protokoll).Equals
        If other Is Nothing Then Return False

        Return String.Compare(other.Fahrt, Fahrt) = 0 AndAlso _
            String.Compare(other.Kategorie, Kategorie) = 0 AndAlso _
            String.Compare(other.Protokollart, Protokollart) = 0 AndAlso _
            String.Compare(other.Tempfilename, Tempfilename) = 0 AndAlso _
            String.Compare(other.Filename, Filename) = 0
    End Function
End Class
