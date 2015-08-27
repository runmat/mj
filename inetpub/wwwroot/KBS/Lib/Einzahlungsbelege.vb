Imports KBSBase

Public Class Einzahlungsbelege
    Inherits ErrorHandlingClass

    Private mstrKostStelle As String
    Private mstrMenge As String

    Public Property KostStelle() As String
        Get
            KostStelle = mstrKostStelle
        End Get
        Set(ByVal value As String)
            mstrKostStelle = value
        End Set
    End Property

    Public Property Menge() As String
        Get
            Menge = mstrMenge
        End Get
        Set(ByVal value As String)
            mstrMenge = value
        End Set
    End Property

    Public Sub ChangeERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_PO_EINZAHLUNGSBELEGE", "I_KOSTL, I_MENGE", mstrKostStelle.PadLeft(10, "0"c), mstrMenge)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub
End Class
