Namespace Common
    ''' <summary>
    ''' Schnittstelle für eine SAP-Fehlermeldung über das SUBRC-System
    ''' </summary>
    Public Interface ISapError
        ReadOnly Property ErrorMessage As String
        ReadOnly Property ErrorCode As String
        ReadOnly Property ErrorOccured As Boolean

        '''<summary>
        ''' Setzt den Fehlerzustand der Klasse zurück
        '''</summary>
        Sub ClearError()

        '''<summary>
        ''' Löst ein Fehlerereignis mit Fehlercode und Fehlermeldung aus
        '''</summary>
        Sub RaiseError(ByVal errorcode As String, ByVal message As String)

    End Interface
End Namespace
