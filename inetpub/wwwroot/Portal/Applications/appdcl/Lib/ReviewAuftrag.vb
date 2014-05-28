Imports System.Linq
Imports System.Collections.Generic
Imports System.Text

Public Class ReviewAuftrag

    Public Property AuftragsNummer As Integer
    Public Property Fahrt As Integer
    Public Property FahrerID As Integer

    Public Property Files As List(Of ReviewFile)

    Public Shared Function GroupFiles(ByVal files As List(Of ReviewFile)) As List(Of ReviewAuftrag)
        If files Is Nothing OrElse files.Count = 0 Then Return New List(Of ReviewAuftrag)

        Dim grouped = files.GroupBy(Function(f) f.AuftragsNummer & "." & f.Fahrt & "." & f.FahrerID)
        Dim result = New List(Of ReviewAuftrag)
        For Each group In grouped
            Dim auftrag = New ReviewAuftrag
            Dim eintrag = group.First()
            auftrag.AuftragsNummer = eintrag.AuftragsNummer
            auftrag.FahrerID = eintrag.FahrerID
            auftrag.Fahrt = eintrag.Fahrt
            auftrag.Files = group.ToList
            result.Add(auftrag)
        Next
        Return result
    End Function

    Public Sub AddInfo(ByVal row As DataRow)
        Dim i As Integer
        If Integer.TryParse(row("ZZKUNNR").ToString(), i) Then Kundennummer = i Else Kundennummer = Nothing
        FahrtVon = row("FAHRTVON").ToString()
        FahrtNach = row("FAHRTNACH").ToString()
        Kennzeichen = row("ZZKENN").ToString()
        Bezeichnung = row("ZZBEZEI").ToString()
    End Sub

    Public Property Kundennummer As Nullable(Of Integer)
    Public Property FahrtVon As String
    Public Property FahrtNach As String
    Public Property Kennzeichen As String
    Public Property Bezeichnung As String

    Public Overrides Function ToString() As String
        Dim result = New StringBuilder()
        result.Append(AuftragsNummer)
        result.Append("." & Fahrt)
        result.Append("." & FahrerID)
        If Not String.IsNullOrEmpty(FahrtVon) AndAlso Not String.IsNullOrEmpty(FahrtNach) Then result.Append("." & FahrtVon & "->" & FahrtNach)
        If Not String.IsNullOrEmpty(Kennzeichen) Then result.Append("." & Kennzeichen)
        If Not String.IsNullOrEmpty(Bezeichnung) Then result.Append("." & Bezeichnung)
        Return result.ToString
    End Function
End Class
