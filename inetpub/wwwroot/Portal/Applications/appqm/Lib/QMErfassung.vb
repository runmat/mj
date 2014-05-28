Public Class QMErfassung
    Public Erfasser As String = String.Empty
    Public VKORG As String = String.Empty
    Public VKBUR As String = String.Empty
    Public ErfasstAm As DateTime = Now
    Public ErfasstUm As String = Now.TimeOfDay.ToString
    Public Referenz As String = String.Empty
    Public AnzPos As String = "1"
    Public Meldedatum As String = String.Empty
    Public Kunnr As String = String.Empty
    Public Kundenname As String = String.Empty
    Public Ansprechpartner As String = String.Empty
    Public Kontaktdaten As String = String.Empty
    Public Kundenreklamation As Boolean = True
    Public Prozess As String = String.Empty
    Public ProzessKey As String = String.Empty
    Public Fehler As String = String.Empty
    Public FehlerKey As String = String.Empty
    Public Fehlerbeschreibung As String = String.Empty
    Public FehlerverursacherFirma As String = String.Empty
    Public FehlerverursacherName As String = String.Empty
    Public KlaerungsverantwortlicherName As String = String.Empty
    Public Status As String = String.Empty
    Public StatusKey As String = String.Empty
End Class
