Imports KBSBase

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

        Public Sub EintragBeantworten(ByVal betr As String, ByVal Text As String, ByVal BedienernummerAbs As String)
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

            Try
                S.AP.Init("Z_MC_SAVE_ANSWER")

                S.AP.SetImportParameter("I_VORGID", VorgangsID)
                S.AP.SetImportParameter("I_LFDNR", LaufendeNummer)
                S.AP.SetImportParameter("I_VON", Empfänger)
                S.AP.SetImportParameter("I_AN", Verfasser)
                S.AP.SetImportParameter("I_BD_NR", BedienernummerAbs)
                S.AP.SetImportParameter("I_LTXNR", ltextnr)

                S.AP.Execute()

                If S.AP.ResultCode <> 0 Then
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
                    If ltextnr <> "" Then
                        lsts.DeleteStringERP(ltextnr)
                    End If
                End If

            Catch ex As Exception
                RaiseError("9999", ex.Message)
            End Try
        End Sub

        Public Sub Rückfrage(ByVal betr As String, ByVal Text As String, ByVal BedienernummerAbs As String, ByVal kostenstelle As String)
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

            Try
                S.AP.Init("Z_MC_NEW_VORGANG")

                S.AP.SetImportParameter("I_UNAME", AN)
                S.AP.SetImportParameter("I_BD_NR", BedienernummerAbs)
                S.AP.SetImportParameter("I_AN", Verfasser)
                S.AP.SetImportParameter("I_LTXNR", ltextnr)
                S.AP.SetImportParameter("I_BETREFF", betr)
                S.AP.SetImportParameter("I_VGART", "FILL")
                S.AP.SetImportParameter("I_ZERLDAT", "")
                S.AP.SetImportParameter("I_VKBUR", kostenstelle)

                S.AP.Execute()

                If S.AP.ResultCode <> 0 Then
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
                    If ltextnr <> "" Then
                        lsts.DeleteStringERP(ltextnr)
                    End If
                End If

            Catch ex As Exception
                RaiseError("9999", ex.Message)
            End Try
        End Sub

        Public Sub EintragBeantworten(ByVal BedienernummerAbs As String, ByVal stat As IFilialbuchEntry.EmpfängerStatus)
            ClearErrorState()

            Try
                S.AP.Init("Z_MC_SAVE_STATUS_IN")

                S.AP.SetImportParameter("I_VORGID", VORGID)
                S.AP.SetImportParameter("I_LFDNR", LFDNR)
                S.AP.SetImportParameter("I_AN", Empfänger)
                S.AP.SetImportParameter("I_BD_NR", BedienernummerAbs)
                S.AP.SetImportParameter("I_STATUSE", TranslateEmpfängerStatus(stat))

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
