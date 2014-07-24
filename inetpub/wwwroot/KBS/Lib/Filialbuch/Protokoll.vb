Namespace DigitalesFilialbuch

    Public Class Protokoll

        Public Enum Side
            Input = 0
            Output = 1
        End Enum

        Private protokoll(,) As FilialbuchEntry
        Private length As Integer = 0
        Private dtExport As DataTable
        Private SapExc As SAPExecutor.SAPExecutor
        Private lstVorgangsarten As List(Of VorgangsartDetails)
        Private mFehler As Boolean
        Private mStatus As String
        Private mMessage As String

        Public ReadOnly Property ProtokollTabelle As DataTable
            Get
                Return dtExport
            End Get
        End Property

        Public ReadOnly Property Fehler As Boolean
            Get
                Return mFehler
            End Get
        End Property

        Public ReadOnly Property Status As String
            Get
                Return mStatus
            End Get
        End Property

        Public ReadOnly Property Message As String
            Get
                Return mMessage
            End Get
        End Property

        Sub New(ByRef SapExc As SAPExecutor.SAPExecutor, ByRef lstVorgänge As List(Of VorgangsartDetails))
            Me.SapExc = SapExc
            protokoll = New FilialbuchEntry(-1, 1) {}
            lstVorgangsarten = lstVorgänge
        End Sub

        Sub addEntry(ByVal Side As Side, ByVal eintrag As FilialbuchEntry)
            If eintrag.AntwortAufLaufendenummer = String.Empty Then
                Dim index As Integer = FindChildEntry(Side, eintrag.VorgangsID, eintrag.LaufendeNummer)
                If index <> -1 Then
                    protokoll(index, Side) = eintrag
                Else
                    length += 1
                    ExtendArray()
                    protokoll(length - 1, Side) = eintrag
                End If

            Else
                Dim index As Integer = FindParentEntry(Side, eintrag.VorgangsID, eintrag.AntwortAufLaufendenummer)
                If index <> -1 Then
                    protokoll(index, Side) = eintrag
                Else
                    length += 1
                    ExtendArray()
                    protokoll(length - 1, Side) = eintrag
                End If
            End If

        End Sub

        Public Function GetAntwortToVorgangsart(ByVal vgart As String) As String
            Dim Antwort As VorgangsartDetails = lstVorgangsarten.Find(
                Function(vg As VorgangsartDetails) As Boolean
                    If vg.Vorgangsart = vgart Then
                        Return True
                    End If
                    Return False
                End Function)
            Return Antwort.Antwortart
        End Function

        Public Sub EintragAbschliessen(ByVal rowindex As Integer, ByVal BedienernummerAbs As String, ByVal stat As IFilialbuchEntry.EntryStatus)
            Dim Aus As Ausgang = protokoll(rowindex, Side.Output)
            Aus.EintragAbschliessen(SapExc, BedienernummerAbs, stat)
            mFehler = Aus.ErrorOccured
            mStatus = Aus.ErrorCode
            mMessage = Aus.ErrorMessage
        End Sub

        Public Sub EintragBeantworten(ByVal rowindex As Integer, ByVal BedienernummerAbs As String, ByVal stat As IFilialbuchEntry.EmpfängerStatus)
            Dim Ein As Eingang = protokoll(rowindex, Side.Input)
            Ein.EintragBeantworten(SapExc, BedienernummerAbs, stat)
            mFehler = Ein.ErrorOccured
            mStatus = Ein.ErrorCode
            mMessage = Ein.ErrorMessage
        End Sub

        Public Sub EintragBeantworten(ByVal rowindex As Integer, ByVal Betreff As String, ByVal Text As String, ByVal BedienernummerAbs As String)
            Dim Ein As Eingang = protokoll(rowindex, Side.Input)
            Ein.EintragBeantworten(SapExc, Betreff, Text, BedienernummerAbs)
            mFehler = Ein.ErrorOccured
            mStatus = Ein.ErrorCode
            mMessage = Ein.ErrorMessage
        End Sub

        Public Sub Rückfrage(ByVal rowindex As Integer, ByVal Betreff As String, ByVal Text As String, ByVal BedienernummerAbs As String, ByVal kostenstelle As String)
            Dim Ein As Eingang = protokoll(rowindex, Side.Input)
            Ein.Rückfrage(SapExc, Betreff, Text, BedienernummerAbs, kostenstelle)
            mFehler = Ein.ErrorOccured
            mStatus = Ein.ErrorCode
            mMessage = Ein.ErrorMessage
        End Sub

        Private Sub ExtendArray()
            Dim oldArray As FilialbuchEntry(,) = protokoll.Clone()
            protokoll = New FilialbuchEntry(protokoll.GetLength(0), 1) {}

            For i = 0 To oldArray.GetLength(0) - 1
                protokoll(i, 0) = oldArray(i, 0)
                protokoll(i, 1) = oldArray(i, 1)
            Next
        End Sub

        Private Function FindParentEntry(ByVal side As Side, ByVal vorgangsid As String, ByVal antwortnummer As String) As Integer
            Dim OtherSide As Side

            If side = DigitalesFilialbuch.Protokoll.Side.Input Then
                OtherSide = DigitalesFilialbuch.Protokoll.Side.Output
            Else
                OtherSide = DigitalesFilialbuch.Protokoll.Side.Input
            End If

            For i = 0 To protokoll.GetUpperBound(0)
                If Not protokoll(i, OtherSide) Is Nothing Then
                    If protokoll(i, OtherSide).VorgangsID = vorgangsid And protokoll(i, OtherSide).LaufendeNummer = antwortnummer Then
                        Return i
                    End If
                End If
            Next

            Return -1
        End Function

        Private Function FindChildEntry(ByVal side As Side, ByVal vorgangsid As String, ByVal Laufendenummer As String) As Integer
            Dim OtherSide As Side

            If side = DigitalesFilialbuch.Protokoll.Side.Input Then
                OtherSide = DigitalesFilialbuch.Protokoll.Side.Output
            Else
                OtherSide = DigitalesFilialbuch.Protokoll.Side.Input
            End If

            For i = 0 To protokoll.GetUpperBound(0)
                If Not protokoll(i, OtherSide) Is Nothing Then
                    If protokoll(i, OtherSide).VorgangsID = vorgangsid And protokoll(i, OtherSide).AntwortAufLaufendenummer = Laufendenummer Then
                        Return i
                    End If
                End If
            Next

            Return -1
        End Function

        ''' <summary>
        ''' Erzeugt eine aufbereitete DataTable aus dem vorhandenen Protokoll
        ''' </summary>
        ''' <param name="FilterInput">Filterwert für den Status im Protokoll Input, NULL-Wert entspricht alles anzeigen</param>
        ''' <param name="FilterOutput">Filterwert für den Status im Protokoll Output, NULL-Wert entspricht alles anzeigen</param>
        ''' <param name="FilterInputE">Filterwert für den StatusEmpfänger im Protokoll Input, NULL-Wert entspricht alles anzeigen</param>
        ''' <param name="FilterOutputE">Filterwert für den StatusEmpfänger im Protokoll Output, NULL-Wert entspricht alles anzeigen</param>
        ''' <returns>Protokolltabelle</returns>
        ''' <remarks></remarks>
        Public Function CreateTable(Optional ByVal FilterInput As IFilialbuchEntry.EntryStatus? = Nothing,
                                    Optional ByVal FilterOutput As IFilialbuchEntry.EntryStatus? = Nothing,
                                    Optional ByVal FilterInputE As IFilialbuchEntry.EmpfängerStatus? = Nothing,
                                    Optional ByVal FilterOutputE As IFilialbuchEntry.EmpfängerStatus? = Nothing) As DataTable
            dtExport = New DataTable()

            dtExport.Columns.Add("Rowindex")

            dtExport.Columns.Add("I_VORGID")
            dtExport.Columns.Add("I_LFDNR")
            dtExport.Columns.Add("I_ERDAT")
            dtExport.Columns.Add("I_ERZEIT")
            dtExport.Columns.Add("I_DATETIME", Type.GetType("System.DateTime"))
            dtExport.Columns.Add("I_VON")
            dtExport.Columns.Add("I_VERTR")
            dtExport.Columns.Add("I_BETREFF")
            dtExport.Columns.Add("I_LTXNR")
            dtExport.Columns.Add("I_ANTW_LFDNR")
            dtExport.Columns.Add("I_STATUS")
            dtExport.Columns.Add("I_STATUSE")
            dtExport.Columns.Add("I_VGART")
            dtExport.Columns.Add("I_ZERLDAT")

            dtExport.Columns.Add("I_HASLANGTEXT", Type.GetType("System.Boolean"))
            dtExport.Columns.Add("I_ERL", Type.GetType("System.Boolean"))
            dtExport.Columns.Add("I_READ", Type.GetType("System.Boolean"))
            dtExport.Columns.Add("I_ANTW", Type.GetType("System.Boolean"))
            dtExport.Columns.Add("I_TRENN", Type.GetType("System.Boolean"))

            dtExport.Columns.Add("O_VORGID")
            dtExport.Columns.Add("O_LFDNR")
            dtExport.Columns.Add("O_ERDAT")
            dtExport.Columns.Add("O_ERZEIT")
            dtExport.Columns.Add("O_DATETIME", Type.GetType("System.DateTime"))
            dtExport.Columns.Add("O_AN")
            dtExport.Columns.Add("O_VERTR")
            dtExport.Columns.Add("O_BETREFF")
            dtExport.Columns.Add("O_LTXNR")
            dtExport.Columns.Add("O_ANTW_LFDNR")
            dtExport.Columns.Add("O_STATUS")
            dtExport.Columns.Add("O_STATUSE")
            dtExport.Columns.Add("O_VGART")
            dtExport.Columns.Add("O_ZERLDAT")

            dtExport.Columns.Add("O_HASLANGTEXT", Type.GetType("System.Boolean"))
            dtExport.Columns.Add("O_LOE", Type.GetType("System.Boolean"))
            dtExport.Columns.Add("O_CLO", Type.GetType("System.Boolean"))
            dtExport.Columns.Add("O_TRENN", Type.GetType("System.Boolean"))

            dtExport.Columns.Add("DEBUG", Type.GetType("System.Boolean"))

            dtExport.AcceptChanges()

            For i = 0 To length - 1
                Dim bExit = False

                ' Gelöschte Datensätze überspringen
                If Not protokoll(i, Side.Input) Is Nothing AndAlso protokoll(i, Side.Input).Status = IFilialbuchEntry.EntryStatus.Gelöscht Then
                    bExit = True
                End If
                If Not protokoll(i, Side.Output) Is Nothing AndAlso protokoll(i, Side.Output).Status = IFilialbuchEntry.EntryStatus.Gelöscht Then
                    bExit = True
                End If

                If Not bExit Then
                    ' Zu filternde Datensätze überspringen
                    Dim bIsInput = True
                    Dim bIsOutput = True
                    Dim bIsInputE = True
                    Dim bIsOutputE = True

                    If Not FilterInput Is Nothing Then
                        If protokoll(i, Side.Input) Is Nothing OrElse protokoll(i, Side.Input).Status <> FilterInput Then
                            bIsInput = False
                        End If
                    End If

                    If Not FilterOutput Is Nothing Then
                        If protokoll(i, Side.Output) Is Nothing OrElse protokoll(i, Side.Output).Status <> FilterOutput Then
                            bIsOutput = False
                        End If
                    End If

                    If Not FilterInputE Is Nothing Then
                        If protokoll(i, Side.Input) Is Nothing OrElse protokoll(i, Side.Input).StatusEmpfänger <> FilterInputE Then
                            bIsInputE = False
                        End If
                    End If

                    If Not FilterOutputE Is Nothing Then
                        If protokoll(i, Side.Output) Is Nothing OrElse protokoll(i, Side.Output).StatusEmpfänger <> FilterOutputE Then
                            bIsOutputE = False
                        End If
                    End If

                    ' Wenn weder Input noch Output den gesuchten Wert enthalten, dann Exit
                    If bIsInput Or bIsOutput Or bIsInputE Or bIsOutputE Then
                        bExit = False
                    Else
                        bExit = True
                    End If

                End If

                If Not bExit Then
                    Dim Row = dtExport.NewRow()
                    Row("Rowindex") = i

                    Row("I_HASLANGTEXT") = False
                    Row("I_ERL") = False
                    Row("I_READ") = False
                    Row("I_ANTW") = False
                    Row("I_TRENN") = False

                    Row("O_HASLANGTEXT") = False
                    Row("O_LOE") = False
                    Row("O_CLO") = False
                    Row("O_TRENN") = False

                    If Not protokoll(i, Side.Input) Is Nothing Then
                        Dim Ein As Eingang = protokoll(i, Side.Input)

                        Row("I_VORGID") = Ein.VorgangsID
                        Row("I_LFDNR") = Ein.LaufendeNummer
                        Row("I_ERDAT") = Ein.DatumErfassung
                        Row("I_ERZEIT") = Ein.ZeitErfassung.ToString().Insert(4, ":").Insert(2, ":")
                        Row("I_DATETIME") = DateTime.Parse(Ein.DatumErfassung & " " & Ein.ZeitErfassung.ToString().Insert(4, ":").Insert(2, ":"))
                        Row("I_VON") = Ein.Verfasser
                        Row("I_VERTR") = Ein.Vertreter
                        Row("I_BETREFF") = Ein.Betreff
                        Row("I_LTXNR") = Ein.Langtextnummer
                        Row("I_ANTW_LFDNR") = Ein.AntwortAufLaufendenummer
                        Row("I_STATUS") = Ein.Status
                        Row("I_STATUSE") = Ein.StatusEmpfänger
                        Row("I_VGART") = Ein.Vorgangsart
                        Row("I_ZERLDAT") = Ein.ZuErledigenBis

                        Row("I_HASLANGTEXT") = CBool(Ein.Langtextnummer.Length > 0)
                        ' Steuert die Anzeige von Erledigt-, Gelesen- und Antwortbuttons
                        If protokoll(i, Side.Input).Status <> IFilialbuchEntry.EntryStatus.Geschlossen And
                            protokoll(i, Side.Input).Status <> IFilialbuchEntry.EntryStatus.Gelöscht And
                            protokoll(i, Side.Input).StatusEmpfänger = IFilialbuchEntry.EmpfängerStatus.Neu Then
                            Select Case GetAntwortToVorgangsart(Ein.Vorgangsart)
                                Case "E"
                                    Row("I_ERL") = True
                                Case "G"
                                    Row("I_READ") = True
                                Case "A"
                                    Row("I_ANTW") = True
                            End Select
                        End If

                        Row("I_TRENN") = True
                    End If

                    If Not protokoll(i, Side.Output) Is Nothing Then
                        Dim Aus As Ausgang = protokoll(i, Side.Output)

                        Row("O_VORGID") = Aus.VorgangsID
                        Row("O_LFDNR") = Aus.LaufendeNummer
                        Row("O_ERDAT") = Aus.DatumErfassung
                        Row("O_ERZEIT") = Aus.ZeitErfassung.ToString().Insert(4, ":").Insert(2, ":")
                        Row("O_DATETIME") = DateTime.Parse(Aus.DatumErfassung & " " & Aus.ZeitErfassung.ToString().Insert(4, ":").Insert(2, ":"))
                        Row("O_AN") = Aus.Empfänger
                        Row("O_VERTR") = Aus.Vertreter
                        Row("O_BETREFF") = Aus.Betreff
                        Row("O_LTXNR") = Aus.Langtextnummer
                        Row("O_ANTW_LFDNR") = Aus.AntwortAufLaufendenummer
                        Row("O_STATUS") = Aus.Status
                        Row("O_STATUSE") = Aus.StatusEmpfänger
                        Row("O_VGART") = Aus.Vorgangsart
                        Row("O_ZERLDAT") = Aus.ZuErledigenBis

                        Row("O_HASLANGTEXT") = CBool(Aus.Langtextnummer.Length > 0)
                        ' Steuert die Anzeige von Lösch- und Schließbuttons
                        If protokoll(i, Side.Output).Status <> IFilialbuchEntry.EntryStatus.Geschlossen And
                            protokoll(i, Side.Output).Status <> IFilialbuchEntry.EntryStatus.Gelöscht Then
                            Row("O_LOE") = True
                            Row("O_CLO") = True
                        End If

                        Row("O_TRENN") = True
                    End If

                    Row("DEBUG") = False
#If DEBUG Then
                    Row("DEBUG") = True
#End If



                    dtExport.Rows.Add(Row)

                End If
            Next

            dtExport.AcceptChanges()

            Return dtExport
        End Function

    End Class

End Namespace
