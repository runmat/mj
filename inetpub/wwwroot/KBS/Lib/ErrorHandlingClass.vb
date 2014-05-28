Imports System.Data

Public MustInherit Class ErrorHandlingClass
    Private E_MESSAGE As String = ""
    Private E_SUBRC As String = ""
    Private GT_MESSAGE As DataTable
    Private bError As Boolean = False

    ReadOnly Property ErrorMessage As String
        Get
            Return E_MESSAGE
        End Get
    End Property

    ReadOnly Property ErrorCode As String
        Get
            Return E_SUBRC
        End Get
    End Property

    ReadOnly Property ErrorTable As DataTable
        Get
            Return GT_MESSAGE
        End Get
    End Property

    ReadOnly Property ErrorOccured As Boolean
        Get
            Return bError
        End Get
    End Property

    ''' <summary>
    ''' Löst einen Fehler aus und füllt die entsprechenden Statuswerte
    ''' </summary>
    ''' <param name="subrc">Subrc - Fehlernummer</param>
    ''' <param name="message">Fehlertext</param>
    ''' <param name="errtable">Fehlertabelle</param>
    ''' <remarks></remarks>
    Public Sub RaiseError(ByVal subrc As String, ByVal message As String, Optional ByVal errtable As DataTable = Nothing)
        bError = True
        E_SUBRC = subrc
        E_MESSAGE = message
        GT_MESSAGE = errtable
    End Sub

    ''' <summary>
    ''' Setzt den Fehlerstatus komplett zurück
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearErrorState()
        E_MESSAGE = ""
        E_SUBRC = "0"
        GT_MESSAGE = Nothing
        bError = False
    End Sub
End Class
