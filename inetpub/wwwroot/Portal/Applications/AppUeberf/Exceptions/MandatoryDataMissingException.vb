Namespace Exceptions

    '---------------
    'Wird geworfen, wenn Pflichtfelder nicht gefüllt sind
    '---------------
    Public Class MandatoryDataMissingException
        Inherits Exception

        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub

    End Class

End Namespace

' ************************************************
' $History: MandatoryDataMissingException.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Exceptions
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.05.07   Time: 10:41
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Exceptions
' Nacharbeiten + Bereinigungen
' 
' ************************************************
