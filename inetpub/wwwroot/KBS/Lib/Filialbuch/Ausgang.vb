Imports KBSBase

Namespace DigitalesFilialbuch
    Public Class Ausgang
        Inherits FilialbuchEntry

        Private ERLDAT As String

        Public ReadOnly Property ErledigtAm As String
            Get
                Return ERLDAT
            End Get
        End Property

        Sub New(ByVal vorgid As String, ByVal lfdnr As String, ByVal erdat As String, ByVal erzeit As String, ByVal an As String, ByVal vertr As String,
                ByVal betreff As String, ByVal ltxnr As String, ByVal antw_lfdnr As String, ByVal objstatus As IFilialbuchEntry.EntryStatus,
                ByVal statuse As IFilialbuchEntry.EmpfängerStatus, ByVal vgart As String, ByVal zerldat As String, ByVal erldat As String)
            MyBase.New(vorgid, lfdnr, erdat, erzeit, vertr, betreff, ltxnr, antw_lfdnr, objstatus, statuse, vgart, zerldat, an)

            Me.ERLDAT = erldat
        End Sub

        Public Sub EintragAbschliessen(ByVal BedienernummerAbs As String, ByVal stat As IFilialbuchEntry.EntryStatus)
            ClearErrorState()

            Try
                S.AP.Init("Z_MC_SAVE_STATUS_OUT")

                S.AP.SetImportParameter("I_VORGID", VORGID)
                S.AP.SetImportParameter("I_LFDNR", LFDNR)
                S.AP.SetImportParameter("I_BD_NR", BedienernummerAbs)
                S.AP.SetImportParameter("I_STATUS", TranslateEntryStatus(stat))

                S.AP.Execute()

                If S.AP.ResultCode <> 0 Then
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
                End If

            Catch ex As Exception
                RaiseError("9999", ex.Message)
            End Try
        End Sub
    End Class
End Namespace