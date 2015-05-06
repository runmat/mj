Imports KBSBase

Namespace DigitalesFilialbuch

    Public Interface IFilialbuchEntry
        Enum EntryStatus
            Neu
            Gesendet
            Gelöscht
            Geschlossen
            Ausblenden 'Wird in der Funktion Protokoll.CreateTable() genutzt um Datensätze auszublenden
        End Enum

        Enum EmpfängerStatus
            Neu
            Gelesen
            Gelöscht
            Geantwortet
            Erledigt
            AutomatischBeantwortet
            Ausblenden 'Wird in der Funktion Protokoll.CreateTable() genutzt um Datensätze auszublenden
        End Enum

        ReadOnly Property VorgangsID As String
        ReadOnly Property LaufendeNummer As String
        ReadOnly Property DatumErfassung As String
        ReadOnly Property ZeitErfassung As String
        ReadOnly Property Vertreter As String
        ReadOnly Property Betreff As String
        ReadOnly Property Langtextnummer As String
        ReadOnly Property AntwortAufLaufendenummer As String
        ReadOnly Property Status As EntryStatus
        ReadOnly Property StatusEmpfänger As EmpfängerStatus
        ReadOnly Property Vorgangsart As String
        ReadOnly Property ZuErledigenBis As String
        ReadOnly Property Empfänger As String

    End Interface

    Public Class FilialbuchEntry
        Inherits ErrorHandlingClass
        Implements IFilialbuchEntry

        Protected VORGID As String
        Protected LFDNR As String
        Protected ERDAT As String
        Protected ERZEIT As String
        Protected VERTR As String
        Protected strBETREFF As String
        Protected LTXNR As String
        Protected ANTW_LFDNR As String
        Protected objSTATUS As IFilialbuchEntry.EntryStatus
        Protected STATUSE As IFilialbuchEntry.EmpfängerStatus
        Protected VGART As String
        Protected ZERLDAT As String
        Protected AN As String

#Region "Properties"

        Public ReadOnly Property VorgangsID As String Implements IFilialbuchEntry.VorgangsID
            Get
                Return VORGID
            End Get
        End Property

        Public ReadOnly Property LaufendeNummer As String Implements IFilialbuchEntry.LaufendeNummer
            Get
                Return LFDNR
            End Get
        End Property

        Public ReadOnly Property DatumErfassung As String Implements IFilialbuchEntry.DatumErfassung
            Get
                Return ERDAT
            End Get
        End Property

        Public ReadOnly Property ZeitErfassung As String Implements IFilialbuchEntry.ZeitErfassung
            Get
                Return ERZEIT
            End Get
        End Property

        Public ReadOnly Property Vertreter As String Implements IFilialbuchEntry.Vertreter
            Get
                Return VERTR
            End Get
        End Property

        Public ReadOnly Property Betreff As String Implements IFilialbuchEntry.Betreff
            Get
                Return strBETREFF
            End Get
        End Property

        Public ReadOnly Property Langtextnummer As String Implements IFilialbuchEntry.Langtextnummer
            Get
                Return LTXNR
            End Get
        End Property

        Public ReadOnly Property AntwortAufLaufendenummer As String Implements IFilialbuchEntry.AntwortAufLaufendenummer
            Get
                Return ANTW_LFDNR
            End Get
        End Property

        Public ReadOnly Property Status As IFilialbuchEntry.EntryStatus Implements IFilialbuchEntry.Status
            Get
                Return objSTATUS
            End Get
        End Property

        Public ReadOnly Property StatusEmpfänger As IFilialbuchEntry.EmpfängerStatus Implements IFilialbuchEntry.StatusEmpfänger
            Get
                Return STATUSE
            End Get
        End Property

        Public ReadOnly Property Vorgangsart As String Implements IFilialbuchEntry.Vorgangsart
            Get
                Return VGART
            End Get
        End Property

        Public ReadOnly Property ZuErledigenBis As String Implements IFilialbuchEntry.ZuErledigenBis
            Get
                Return ZERLDAT
            End Get
        End Property

        Public ReadOnly Property Empfänger As String Implements IFilialbuchEntry.Empfänger
            Get
                Return AN
            End Get
        End Property

#End Region

#Region "Methods and Functions"

        Public Sub New(ByVal vorgid As String, ByVal lfdnr As String, ByVal erdat As String, ByVal erzeit As String, ByVal vertr As String,
            ByVal betreff As String, ByVal ltxnr As String, ByVal antw_lfdnr As String, ByVal objstatus As IFilialbuchEntry.EntryStatus,
            ByVal statuse As IFilialbuchEntry.EmpfängerStatus, ByVal vgart As String, ByVal zerldat As String, ByVal an As String)

            Me.VORGID = vorgid
            Me.LFDNR = lfdnr
            Me.ERDAT = erdat
            Me.ERZEIT = erzeit
            Me.VERTR = vertr
            strBETREFF = betreff
            Me.LTXNR = ltxnr
            Me.ANTW_LFDNR = antw_lfdnr
            Me.objSTATUS = objstatus
            Me.STATUSE = statuse
            Me.VGART = vgart
            Me.ZERLDAT = zerldat
            Me.AN = an

        End Sub

        Public Shared Function TranslateEntryStatus(ByVal status As IFilialbuchEntry.EntryStatus) As String
            Select Case status
                Case IFilialbuchEntry.EntryStatus.Neu
                    Return "NEW"
                Case IFilialbuchEntry.EntryStatus.Gelöscht
                    Return "LOE"
                Case IFilialbuchEntry.EntryStatus.Geschlossen
                    Return "CLOSE"
                Case IFilialbuchEntry.EntryStatus.Gesendet
                    Return "SEND"
                Case Else
                    Return ""
            End Select
        End Function

        Public Shared Function TranslateEntryStatus(ByVal status As String) As IFilialbuchEntry.EntryStatus
            Select Case status.ToUpper()
                Case "NEW"
                    Return IFilialbuchEntry.EntryStatus.Neu
                Case "LOE"
                    Return IFilialbuchEntry.EntryStatus.Gelöscht
                Case "CLOSE"
                    Return IFilialbuchEntry.EntryStatus.Geschlossen
                Case "SEND"
                    Return IFilialbuchEntry.EntryStatus.Gesendet
                Case Else
                    Return IFilialbuchEntry.EntryStatus.Ausblenden
            End Select
        End Function

        Public Shared Function TranslateEmpfängerStatus(ByVal status As IFilialbuchEntry.EmpfängerStatus) As String
            Select Case status
                Case IFilialbuchEntry.EmpfängerStatus.Neu
                    Return "NEW"
                Case IFilialbuchEntry.EmpfängerStatus.Gelöscht
                    Return "LOE"
                Case IFilialbuchEntry.EmpfängerStatus.Gelesen
                    Return "READ"
                Case IFilialbuchEntry.EmpfängerStatus.Geantwortet
                    Return "ANTW"
                Case IFilialbuchEntry.EmpfängerStatus.Erledigt
                    Return "ERL"
                Case IFilialbuchEntry.EmpfängerStatus.AutomatischBeantwortet
                    Return "AUT"
                Case Else
                    Return ""
            End Select
        End Function

        Public Shared Function TranslateEmpfängerStatus(ByVal status As String) As IFilialbuchEntry.EmpfängerStatus
            Select Case status.ToUpper
                Case "NEW"
                    Return IFilialbuchEntry.EmpfängerStatus.Neu
                Case "LOE"
                    Return IFilialbuchEntry.EmpfängerStatus.Gelöscht
                Case "READ"
                    Return IFilialbuchEntry.EmpfängerStatus.Gelesen
                Case "ANTW"
                    Return IFilialbuchEntry.EmpfängerStatus.Geantwortet
                Case "ERL"
                    Return IFilialbuchEntry.EmpfängerStatus.Erledigt
                Case "AUT"
                    Return IFilialbuchEntry.EmpfängerStatus.AutomatischBeantwortet
                Case Else
                    Return IFilialbuchEntry.EmpfängerStatus.Ausblenden
            End Select
        End Function

        Public Sub EintragStatusÄndern(ByVal BedienernummerAbs As String)
            ClearErrorState()

            Try
                S.AP.Init("Z_MC_SAVE_STATUS_OUT")

                S.AP.SetImportParameter("I_VORGID", VORGID)
                S.AP.SetImportParameter("I_LFDNR", LFDNR)
                S.AP.SetImportParameter("I_BD_NR", BedienernummerAbs)
                S.AP.SetImportParameter("I_STATUS", TranslateEntryStatus(Status))

                S.AP.Execute()

                If S.AP.ResultCode <> 0 Then
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
                End If

            Catch ex As Exception
                RaiseError("9999", ex.Message)
            End Try
        End Sub

#End Region

    End Class

End Namespace
