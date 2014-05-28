Namespace DigitalesFilialbuch
    Public Class Eingang
        Inherits FilialbuchEntry

        Private VON As String

        Public ReadOnly Property Verfasser As String
            Get
                Return VON
            End Get
        End Property

        Sub New(ByVal vorgid As String, ByVal lfdnr As String, ByVal erdat As String, ByVal erzeit As String, ByVal von As String, ByVal vertr As String,
            ByVal betreff As String, ByVal ltxnr As String, ByVal antw_lfdnr As String, ByVal objstatus As IFilialbuchEntry.EntryStatus,
            ByVal statuse As IFilialbuchEntry.EmpfängerStatus, ByVal vgart As String, ByVal zerldat As String, ByVal an As String)
            MyBase.New(vorgid, lfdnr, erdat, erzeit, vertr, betreff, ltxnr, antw_lfdnr, objstatus, statuse, vgart, zerldat, an)

            Me.VON = von
        End Sub

        Public Sub EintragBeantworten(ByRef SapExc As SAPExecutor.SAPExecutor, ByVal betr As String, ByVal Text As String, ByVal BedienernummerAbs As String)
            ' FehlerStatus zurücksetzen
            ClearErrorState()

            Dim lsts As New LongStringToSap()
            Dim ltextnr As String = ""
            If Text.Trim().Length <> 0 Then
                ltextnr = lsts.InsertStringERP(Text, "MC")
                If lsts.ErrorCode <> "0" Then
                    RaiseError(lsts.ErrorCode, lsts.ErrorMessage)
                    Exit Sub
                End If
            End If

            ' SAPKomunikationstabelle holen
            Dim dtValues As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            'Import-Parameter
            dtValues.Rows.Add(New Object() {"I_VORGID", False, VorgangsID})
            dtValues.Rows.Add(New Object() {"I_LFDNR", False, LaufendeNummer})
            dtValues.Rows.Add(New Object() {"I_VON", False, Empfänger})
            dtValues.Rows.Add(New Object() {"I_AN", False, Verfasser})
            dtValues.Rows.Add(New Object() {"I_BD_NR", False, BedienernummerAbs})
            dtValues.Rows.Add(New Object() {"I_LTXNR", False, ltextnr})

            'Export-Parameter          
            dtValues.Rows.Add(New Object() {"E_SUBRC", True})
            dtValues.Rows.Add(New Object() {"E_MESSAGE", True})

            SapExc.ExecuteERP("Z_MC_SAVE_ANSWER", dtValues)

            If SapExc.ErrorOccured Then
                RaiseError(SapExc.E_SUBRC, SapExc.E_MESSAGE)
                If ltextnr <> "" Then
                    lsts.DeleteStringERP(ltextnr)
                End If
            End If

            'GetEinträge(UserLoggedIn, letzterStatus, VON, Bis)
        End Sub

        Public Sub Rückfrage(ByRef SapExc As SAPExecutor.SAPExecutor, ByVal betr As String, ByVal Text As String, ByVal BedienernummerAbs As String, ByVal kostenstelle As String)
            ' FehlerStatus zurücksetzen
            ClearErrorState()

            Dim lsts As New LongStringToSap()
            Dim ltextnr As String = ""
            If Text.Trim().Length > 0 Then
                ltextnr = lsts.InsertStringERP(Text, "MC")
                If lsts.ErrorCode <> "0" Then
                    RaiseError(lsts.ErrorCode, lsts.ErrorMessage)
                    Exit Sub
                End If
            End If

            ' SAPKomunikationstabelle holen
            Dim dtValues As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            'Import-Parameter
            dtValues.Rows.Add(New Object() {"I_UNAME", False, AN})
            dtValues.Rows.Add(New Object() {"I_BD_NR", False, BedienernummerAbs})
            dtValues.Rows.Add(New Object() {"I_AN", False, Verfasser})
            dtValues.Rows.Add(New Object() {"I_LTXNR", False, ltextnr})
            dtValues.Rows.Add(New Object() {"I_BETREFF", False, betr})
            dtValues.Rows.Add(New Object() {"I_VGART", False, "FILL"})
            dtValues.Rows.Add(New Object() {"I_ZERLDAT", False, ""})
            dtValues.Rows.Add(New Object() {"I_VKBUR", False, kostenstelle})

            'Export-Parameter          
            dtValues.Rows.Add(New Object() {"E_SUBRC", True})
            dtValues.Rows.Add(New Object() {"E_MESSAGE", True})

            SapExc.ExecuteERP("Z_MC_NEW_VORGANG", dtValues)

            If SapExc.ErrorOccured Then
                RaiseError(SapExc.E_SUBRC, SapExc.E_MESSAGE)
                If ltextnr <> "" Then
                    lsts.DeleteStringERP(ltextnr)
                End If
            End If
        End Sub

        Public Sub EintragBeantworten(ByRef SapExc As SAPExecutor.SAPExecutor, ByVal BedienernummerAbs As String, ByVal stat As IFilialbuchEntry.EmpfängerStatus)
            ' FehlerStatus zurücksetzen
            ClearErrorState()

            ' SAPKomunikationstabelle holen
            Dim dtValues As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            'Import-Parameter
            dtValues.Rows.Add(New Object() {"I_VORGID", False, VORGID})
            dtValues.Rows.Add(New Object() {"I_LFDNR", False, LFDNR})
            dtValues.Rows.Add(New Object() {"I_AN", False, Empfänger})
            dtValues.Rows.Add(New Object() {"I_BD_NR", False, BedienernummerAbs})
            dtValues.Rows.Add(New Object() {"I_STATUSE", False, TranslateEmpfängerStatus(stat)})

            'Export-Parameter          
            dtValues.Rows.Add(New Object() {"E_SUBRC", True})
            dtValues.Rows.Add(New Object() {"E_MESSAGE", True})

            SapExc.ExecuteERP("Z_MC_SAVE_STATUS_IN", dtValues)

            If SapExc.ErrorOccured Then
                RaiseError(SapExc.E_SUBRC, SapExc.E_MESSAGE)
            End If
        End Sub
    End Class
End Namespace
