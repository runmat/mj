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

        Public Sub EintragAbschliessen(ByRef SapExc As SAPExecutor.SAPExecutor, ByVal BedienernummerAbs As String, ByVal stat As IFilialbuchEntry.EntryStatus)
            ' FehlerStatus zurücksetzen
            ClearErrorState()

            ' SAPKomunikationstabelle holen
            Dim dtValues As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            'Import-Parameter
            dtValues.Rows.Add(New Object() {"I_VORGID", False, VORGID})
            dtValues.Rows.Add(New Object() {"I_LFDNR", False, LFDNR})
            dtValues.Rows.Add(New Object() {"I_BD_NR", False, BedienernummerAbs})
            dtValues.Rows.Add(New Object() {"I_STATUS", False, TranslateEntryStatus(stat)})

            'Export-Parameter          
            dtValues.Rows.Add(New Object() {"E_SUBRC", True})
            dtValues.Rows.Add(New Object() {"E_MESSAGE", True})

            SapExc.ExecuteERP("Z_MC_SAVE_STATUS_OUT", dtValues)

            If SapExc.ErrorOccured Then
                RaiseError(SapExc.E_SUBRC, SapExc.E_MESSAGE)
            End If
        End Sub
    End Class
End Namespace