
Namespace DigitalesFilialbuch

    ''' <summary>
    ''' Arbeitsklasse die ein Filialbuch darstellt. Sie beinhaltet alle Funktionen zur Kommunikation mit SAP und hält die Daten
    ''' die ein Filialbuch kennzeichnen
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FilialbuchClass
        Inherits ErrorHandlingClass

#Region "Enumeratoren"

        Public Enum EntryTyp
            Alle
            Rückfragen
            Aufgaben
        End Enum

        Public Enum Rolle
            Filiale
            LFB
        End Enum

        Public Enum Antwortart
            Erledigt
            Gelesen
            Antwort
        End Enum

        Public Enum StatusFilter
            Neu
            Alle
        End Enum

#End Region

#Region "Globale Objekte"

        Private SapExc As SAPExecutor.SAPExecutor

        Private dtIn As DataTable
        Private dtOut As DataTable

        Private lstVorgangsarten As List(Of VorgangsartDetails) = New List(Of VorgangsartDetails)()
        Private lstVorgangsartenRolle As List(Of VorgangsartRolleDetails) = New List(Of VorgangsartRolleDetails)()

#End Region

#Region "Properties"

        Property Protokoll As Protokoll
        Property UserLoggedIn As FilialbuchUser
        Property letzterStatus As StatusFilter
        Property Von As Date
        Property Bis As Date

        ReadOnly Property Vorgangsarten As List(Of VorgangsartDetails)
            Get
                Return lstVorgangsarten
            End Get
        End Property

        ReadOnly Property VorgangsartenRolle As List(Of VorgangsartRolleDetails)
            Get
                Return lstVorgangsartenRolle
            End Get
        End Property

        ReadOnly Property tblIn As DataTable
            Get
                Return dtIn
            End Get
        End Property

        ReadOnly Property tblOut As DataTable
            Get
                Return dtOut
            End Get
        End Property

#End Region

        Public Sub New()
            SapExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        End Sub

#Region "Shared Functions"
        ''' <summary>
        ''' Übersetzt Rollenwerte für SAP
        ''' </summary>
        ''' <param name="rl">Rolle</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function TranslateRolle(ByVal rl As Rolle) As String
            Select Case rl
                Case Rolle.Filiale
                    Return "FIL"
                Case Rolle.LFB
                    Return "LFB"
                Case Else
                    Return "FIL"
            End Select
        End Function

        ''' <summary>
        ''' Übersetzt Rollenwerte aus SAP
        ''' </summary>
        ''' <param name="rl">Rolle</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function TranslateRolle(ByVal rl As String) As Rolle
            Select Case rl
                Case "FIL"
                    Return Rolle.Filiale
                Case "LFB"
                    Return Rolle.LFB
                Case Else
                    Return Rolle.Filiale
            End Select
        End Function

        ''' <summary>
        ''' Übersetzt Antwortarten für SAP
        ''' </summary>
        ''' <param name="antwa">Antwortart</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function TranslateAntwortart(ByVal antwa As Antwortart) As Char
            Select Case antwa
                Case Antwortart.Antwort
                    Return "A"c
                Case Antwortart.Gelesen
                    Return "G"c
                Case Antwortart.Erledigt
                    Return "E"c
                Case Else
                    Return "G"c
            End Select
        End Function

        ''' <summary>
        ''' Übersetzt Antwortarten für SAP
        ''' </summary>
        ''' <param name="antwa">Antwortart</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function TranslateAntwortart(ByVal antwa As Char) As Antwortart
            Select Case antwa
                Case "A"c
                    Return Antwortart.Antwort
                Case "G"c
                    Return Antwortart.Gelesen
                Case "E"c
                    Return Antwortart.Erledigt
                Case Else
                    Return Antwortart.Gelesen
            End Select
        End Function

        ''' <summary>
        ''' Übersetzt Eintragsstatus für SAP
        ''' </summary>
        ''' <param name="status">Status</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function TranslateStatus(ByVal status As StatusFilter) As String
            Select Case status
                Case StatusFilter.Alle
                    Return "ALL"
                Case StatusFilter.Neu
                    Return "NEW"
                Case Else
                    Return "ALL"
            End Select
        End Function

        ''' <summary>
        ''' Übersetzt Eintragsstatus für SAP
        ''' </summary>
        ''' <param name="status">Status</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function TranslateStatus(ByVal status As String) As StatusFilter
            Select Case status
                Case "NEW"
                    Return StatusFilter.Neu
                Case "ALL"
                    Return StatusFilter.Alle
                Case Else
                    Return StatusFilter.Alle
            End Select
        End Function
#End Region

        Public Function GetGroups() As DataTable
            Return New DataTable
        End Function

        Public Function LoginUser(ByVal VkBur As String, ByVal LoginValue As String) As FilialbuchUser
            ' FehlerStatus zurücksetzen
            ClearErrorState()

            ' SAPKomunikationstabelle holen
            Dim dtValues As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            'Import-Parameter
            dtValues.Rows.Add(New Object() {"I_VKBUR", False, VkBur})
            dtValues.Rows.Add(New Object() {"I_BD_NR", False, LoginValue})

            'Export-Parameter
            dtValues.Rows.Add(New Object() {"E_UNAME", True})
            dtValues.Rows.Add(New Object() {"E_BD_NAME", True})
            dtValues.Rows.Add(New Object() {"E_ROLLE", True})
            dtValues.Rows.Add(New Object() {"E_BEZEI", True})
            dtValues.Rows.Add(New Object() {"E_ROLLE_PA", True})
            dtValues.Rows.Add(New Object() {"E_UNAME_PA", True})
            dtValues.Rows.Add(New Object() {"E_SUBRC", True})
            dtValues.Rows.Add(New Object() {"E_MESSAGE", True})

            dtValues.Rows.Add(New Object() {"GT_VORGART", True})
            dtValues.Rows.Add(New Object() {"GT_ROLLE_VGART", True})

            SapExc.ExecuteERP("Z_MC_CONNECT", dtValues)

            If SapExc.ErrorOccured Then
                RaiseError(SapExc.E_SUBRC, SapExc.E_MESSAGE)
            Else
                ' User auswerten
                If dtValues.Rows(1)(2) Is Nothing Then
                    RaiseError("9999", "Es konnte kein Benutzer ermittelt werden.")
                ElseIf dtValues.Rows(2)(2) Is Nothing Then
                    RaiseError("9998", "Es konnte keine Rolle ermittelt werden.")
                Else
                    UserLoggedIn = New FilialbuchUser(CStr(dtValues.Rows(2)(2)).Trim(), _
                                                      CStr(dtValues.Rows(3)(2)).Trim(), _
                                                      CStr(dtValues.Rows(4)(2)).Trim(), _
                                                      CStr(dtValues.Rows(5)(2)).Trim(), _
                                                      CStr(dtValues.Rows(6)(2)).Trim(), _
                                                      CStr(dtValues.Rows(7)(2)).Trim(), _
                                                      VkBur)
                End If

                ' Vorgangsarten auslesen
                If Not dtValues.Rows(10)(2) Is Nothing Then
                    Dim dtVorgangsarten As DataTable = CType(dtValues.Rows(10)(2), DataTable)

                    lstVorgangsarten.Clear()

                    For Each Row In dtVorgangsarten.Rows
                        Dim vgart As String = Row("VGART").ToString()
                        Dim bezeichnung As String = Row("BEZEI").ToString()
                        Dim antwortart As Char = Row("ANTW_ART").ToString()
                        Dim filialbuchvorgang As Boolean = False
                        If Not Row("FILIALBUCH_VG") Is Nothing Then
                            If Row("FILIALBUCH_VG").ToString().ToUpper() = "F" Then
                                filialbuchvorgang = True
                            End If
                        End If
                        lstVorgangsarten.Add(New VorgangsartDetails(vgart, bezeichnung, antwortart, filialbuchvorgang))
                    Next
                End If

                ' Vorgangsarten zur Rolle auslesen
                If Not dtValues.Rows(11)(2) Is Nothing Then
                    Dim dtVorgangRollen As DataTable = CType(dtValues.Rows(11)(2), DataTable)

                    lstVorgangsartenRolle.Clear()

                    For Each Row In dtVorgangRollen.Rows
                        Dim rolle As Rolle = TranslateRolle(Row("ROLLE").ToString())
                        Dim vgart As String = Row("VGART").ToString()
                        Dim stufe As Char = Row("STUFE").ToString()
                        Dim close As Boolean = False
                        If Not Row("CLOSE_EMPF") Is Nothing Then
                            If Row("CLOSE_EMPF").ToString().ToUpper() = "X" Then
                                close = True
                            End If
                        End If
                        lstVorgangsartenRolle.Add(New VorgangsartRolleDetails(rolle, vgart, stufe, close))
                    Next
                End If
            End If

            Return UserLoggedIn
        End Function

        Public Sub GetEinträge(ByRef FilBuUser As FilialbuchUser, ByVal Status As StatusFilter, Optional ByVal an_kst As String = Nothing, Optional ByVal datVon As Date = Nothing, Optional ByVal datBis As Date = Nothing)

            ' FehlerStatus zurücksetzen
            ClearErrorState()

            Von = datVon
            Bis = datBis
            letzterStatus = Status

            Dim strVon As String = ""
            Dim strBis As String = ""

            If Status = StatusFilter.Alle Then
                If datVon.CompareTo(Nothing) = 0 Then
                    RaiseError("9999", "Es wurde kein gültiges Von-Datum für die Auswal mitgegeben!")
                    Return
                ElseIf datBis.CompareTo(Nothing) = 0 Then
                    RaiseError("9999", "Es wurde kein gültiges Bis-Datum für die Auswal mitgegeben!")
                    Return
                Else
                    strVon = datVon.ToShortDateString()
                    strBis = datBis.ToShortDateString()
                End If
            End If

            ' SAPKomunikationstabelle holen
            Dim dtValues As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            'Import-Parameter
            dtValues.Rows.Add(New Object() {"I_UNAME", False, FilBuUser.UsernameSAP})
            dtValues.Rows.Add(New Object() {"I_STATUS", False, TranslateStatus(Status)})
            dtValues.Rows.Add(New Object() {"I_VON", False, strVon})
            dtValues.Rows.Add(New Object() {"I_BIS", False, strBis})

            'Export-Parameter          
            dtValues.Rows.Add(New Object() {"E_SUBRC", True})
            dtValues.Rows.Add(New Object() {"E_MESSAGE", True})

            dtValues.Rows.Add(New Object() {"GT_IN", True})
            dtValues.Rows.Add(New Object() {"GT_OUT", True})

            SapExc.ExecuteERP("Z_MC_GET_IN_OUT", dtValues)

            If SapExc.ErrorOccured Then
                RaiseError(SapExc.E_SUBRC, SapExc.E_MESSAGE)
            Else
                Protokoll = New Protokoll(SapExc, lstVorgangsarten)

                ' Eingang auswerten
                If Not dtValues.Rows(6)(2) Is Nothing Then
                    dtIn = CType(dtValues.Rows(6)(2), DataTable)
                    For Each row In dtIn.Rows
                        If Not an_kst Is Nothing Then
                            If row("VON") = an_kst Or row("VON") = UserLoggedIn.UsernameSAP Then 'Filter auf Kostenstelle und User
                                Protokoll.addEntry(Protokoll.Side.Input, New Eingang(CStr(row(0)), CStr(row(1)), CStr(row(2)), CStr(row(3)), CStr(row(4)), _
                                                                                        CStr(row(5)), CStr(row(6)), CStr(row(7)), CStr(row(8)), _
                                                                                        FilialbuchEntry.TranslateEntryStatus(CStr(row(9))), _
                                                                                        FilialbuchEntry.TranslateEmpfängerStatus(CStr(row(10))), _
                                                                                        CStr(row(11)), CStr(row(12)), UserLoggedIn.UsernameSAP))
                            End If
                        Else
                            Protokoll.addEntry(Protokoll.Side.Input, New Eingang(CStr(row(0)), CStr(row(1)), CStr(row(2)), CStr(row(3)), CStr(row(4)), _
                                                                                        CStr(row(5)), CStr(row(6)), CStr(row(7)), CStr(row(8)), _
                                                                                        FilialbuchEntry.TranslateEntryStatus(CStr(row(9))), _
                                                                                        FilialbuchEntry.TranslateEmpfängerStatus(CStr(row(10))), _
                                                                                        CStr(row(11)), CStr(row(12)), UserLoggedIn.UsernameSAP))
                        End If
                    Next
                End If

                ' Ausgang auswerten
                If Not dtValues.Rows(7)(2) Is Nothing Then
                    dtOut = CType(dtValues.Rows(7)(2), DataTable)
                    For Each row In dtOut.Rows
                        If Not an_kst Is Nothing Then
                            If row("ZAN") = an_kst Or row("ZAN") = UserLoggedIn.UsernameSAP Then 'Filter auf Kostenstelle und User
                                Protokoll.addEntry(Protokoll.Side.Output, New Ausgang(CStr(row(0)), CStr(row(1)), CStr(row(2)), CStr(row(3)), CStr(row(4)), _
                                                                                        CStr(row(5)), CStr(row(6)), CStr(row(7)), CStr(row(8)), _
                                                                                        FilialbuchEntry.TranslateEntryStatus(CStr(row(9))), _
                                                                                        FilialbuchEntry.TranslateEmpfängerStatus(CStr(row(10))), _
                                                                                        CStr(row(11)), CStr(row(12)), CStr(row(13))))
                            End If
                        Else
                            Protokoll.addEntry(Protokoll.Side.Output, New Ausgang(CStr(row(0)), CStr(row(1)), CStr(row(2)), CStr(row(3)), CStr(row(4)), _
                                                                                        CStr(row(5)), CStr(row(6)), CStr(row(7)), CStr(row(8)), _
                                                                                        FilialbuchEntry.TranslateEntryStatus(CStr(row(9))), _
                                                                                        FilialbuchEntry.TranslateEmpfängerStatus(CStr(row(10))), _
                                                                                        CStr(row(11)), CStr(row(12)), CStr(row(13))))
                        End If
                    Next
                End If

            End If

        End Sub

        ''' <summary>
        ''' Erstellt einen neuen Filialbucheintrag
        ''' </summary>
        ''' <param name="Betreff"></param>
        ''' <param name="Text"></param>
        ''' <param name="an"></param>
        ''' <param name="BedienernummerAbs">Bedienernummer des Absenders</param>
        ''' <param name="vorgangsart"></param>
        ''' <param name="zuerledigenBis"></param>
        ''' <remarks></remarks>
        Public Sub NeuerEintrag(ByVal Betreff As String, ByVal Text As String, ByVal an As String, ByVal BedienernummerAbs As String, _
                                ByVal vorgangsart As String, ByVal zuerledigenBis As String)
            ' FehlerStatus zurücksetzen
            ClearErrorState()

            Dim lsts As New LongStringToSap()
            Dim ltxnr As String = ""
            If Text.Trim().Length > 0 Then
                ltxnr = lsts.InsertStringERP(Text, "MC")
                If lsts.ErrorCode <> "0" Then
                    RaiseError(lsts.ErrorCode, lsts.ErrorMessage)
                    Exit Sub
                End If
            End If

            ' SAPKomunikationstabelle holen
            Dim dtValues As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            'Import-Parameter
            dtValues.Rows.Add(New Object() {"I_UNAME", False, UserLoggedIn.UsernameSAP})
            dtValues.Rows.Add(New Object() {"I_BD_NR", False, BedienernummerAbs})
            dtValues.Rows.Add(New Object() {"I_AN", False, an})
            dtValues.Rows.Add(New Object() {"I_LTXNR", False, ltxnr})
            dtValues.Rows.Add(New Object() {"I_BETREFF", False, Betreff})
            dtValues.Rows.Add(New Object() {"I_VGART", False, vorgangsart})
            dtValues.Rows.Add(New Object() {"I_ZERLDAT", False, zuerledigenBis})
            dtValues.Rows.Add(New Object() {"I_VKBUR", False, UserLoggedIn.Kostenstelle})

            'Export-Parameter          
            dtValues.Rows.Add(New Object() {"E_SUBRC", True})
            dtValues.Rows.Add(New Object() {"E_MESSAGE", True})

            SapExc.ExecuteERP("Z_MC_NEW_VORGANG", dtValues)

            If SapExc.ErrorOccured Then
                RaiseError(SapExc.E_SUBRC, SapExc.E_MESSAGE)
                If ltxnr <> "" Then
                    lsts.DeleteStringERP(ltxnr)
                End If
            End If

            GetEinträge(UserLoggedIn, letzterStatus, an, Von, Bis)
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

    End Class

#Region "Strukturen"

    ''' <summary>
    ''' Daten eines angemeldeten Filialbuchbenutzers
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure FilialbuchUser
        Property UsernameSAP As String
        Property Bedienername As String
        Property Rolle As FilialbuchClass.Rolle
        Property RollenName As String
        Property RollePa As String
        Property NamePa As String
        Property Kostenstelle As String

        Public Sub New(ByVal usernamesap As String, ByVal bedienername As String, ByVal rolle As String, ByVal rollenname As String,
                       ByVal rollepa As String, ByVal namepa As String, ByVal kostenstelle As String)
            Me.UsernameSAP = usernamesap
            Me.Bedienername = bedienername
            If rolle.ToUpper().Trim = "LFB" Then
                Me.Rolle = FilialbuchClass.Rolle.LFB
            Else
                Me.Rolle = FilialbuchClass.Rolle.Filiale
            End If
            Me.RollenName = rollenname
            Me.RollePa = rollepa
            Me.NamePa = namepa
            Me.Kostenstelle = kostenstelle
        End Sub
    End Structure

    ''' <summary>
    ''' Detailwerte Vorgangsart für Rolle
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure VorgangsartRolleDetails
        Property Rolle As FilialbuchClass.Rolle
        Property Vorgangsart As String
        Property Stufe As Char
        Property VonEmpfängerZuSchliessen As Boolean

        Public Sub New(ByVal rolle As FilialbuchClass.Rolle, ByVal vorgangsart As String, ByVal stufe As Char, ByVal schliessbar As Boolean)
            Me.Rolle = rolle
            Me.Vorgangsart = vorgangsart
            Me.Stufe = stufe
            VonEmpfängerZuSchliessen = schliessbar
        End Sub
    End Structure

    ''' <summary>
    ''' Detailwerte Vorgangsart
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure VorgangsartDetails
        Property Vorgangsart As String
        Property Bezeichnung As String
        Property Antwortart As Char
        Property Filialbuchvorgang As Boolean

        Public Sub New(ByVal vorgangsart As String, ByVal bezeichnung As String, ByVal antwortart As Char, ByVal Filialbuchvorgang As Boolean)
            Me.Vorgangsart = vorgangsart
            Me.Bezeichnung = bezeichnung
            Me.Antwortart = antwortart
            Me.Filialbuchvorgang = Filialbuchvorgang
        End Sub

    End Structure

#End Region

End Namespace

