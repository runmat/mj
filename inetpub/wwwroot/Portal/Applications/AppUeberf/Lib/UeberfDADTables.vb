Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class UeberfDADTables
    Inherits Base.Business.DatenimportBase

#Region "Declarations"

    '***Auftragsart****
    Private Enum Auftragsarten
        Zulassung = 1
        Auslieferung = 2
        ZulassungAuslieferung = 3
        AuslieferungRueckfuehrung = 4
        Rueckfuehrung = 5
        Alles = 6
    End Enum

    Private mLaender As DataTable
#End Region

#Region "Properties"

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    '----------------------------------------------------------------------
    ' Methode:      GetLnKunde
    ' Autor:        SFa
    ' Beschreibung: Ruft das BAPI Z_M_GET_LN_KUNDE auf und liefert eine
    '               Tabelle mit Leasingnehmeradressdaten zurück
    ' Erstellt am:  26.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Function GetLnKunde(ByVal LnNummer As String) As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        
        Try
            'strCom = "EXEC Z_M_GET_LN_KUNDE @I_AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@I_EXKUNNR_ZL='" & LnNummer & "'," _
            '            & "@ET_ANSPP=@ExportAnsprechpartner OUTPUT," _
            '            & "@ET_VSBD=@ExportVersandbedingungen OUTPUT," _
            '            & "@ET_TEAM=@ExportTeam OUTPUT," _
            '            & "@ES_LN_KUNDE=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            S.AP.InitExecute("Z_M_GET_LN_KUNDE", "I_AG,I_EXKUNNR_ZL", Right("0000000000" & m_objUser.KUNNR, 10), LnNummer)

            'Exportparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTable)

            'Dim ExportAnsprechpartner As New SAPParameter("@ExportAnsprechpartner", ParameterDirection.Output)
            'cmd.Parameters.Add(ExportAnsprechpartner)

            'Dim ExportVersandbedingungen As New SAPParameter("@ExportVersandbedingungen", ParameterDirection.Output)
            'cmd.Parameters.Add(ExportVersandbedingungen)

            'Dim ExportTeam As New SAPParameter("@ExportTeam", ParameterDirection.Output)
            'cmd.Parameters.Add(ExportTeam)

            'cmd.ExecuteNonQuery()

            Dim sapTable As DataTable = S.AP.GetExportTable("ES_LN_KUNDE") 'DirectCast(pSAPTable.Value, DataTable)

            m_tblResult = sapTable
            
            WriteLogEntry(True, "I_AG=" & m_objUser.KUNNR & ",I_EXKUNNR_ZL=" & LnNummer, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_AG"
                    m_strMessage = "Kein Auftraggeber angegeben."
                Case "NO_LN"
                    m_strMessage = "Kein Leasingnehmer angegeben."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Leasingnehmerdaten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, Nothing)
            
        End Try

        Return m_tblResult

    End Function

    '----------------------------------------------------------------------
    ' Methode:      GetLnVertragsdaten
    ' Autor:        SFa
    ' Beschreibung: Ruft das BAPI Z_M_LESEN_LHS auf und liefert eine
    '               Tabelle mit Vertragsdaten zurück
    ' Erstellt am:  27.08.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Function GetLnVertragsdaten(ByVal Referenz As String) As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)


        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        'Dim intID As Int32 = -1

        Try
            'strCom = "EXEC Z_M_LESEN_LHS @I_KUNNR='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@I_LIZNR='" & Referenz & "'," _
            '            & "@GT_WEB=@ExportVertragsdaten OUTPUT" _
            '            & " OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            'Exportparameter
            'Dim ExportVertragsdaten As New SAPParameter("@ExportVertragsdaten", ParameterDirection.Output)
            'cmd.Parameters.Add(ExportVertragsdaten)


            'cmd.ExecuteNonQuery()

            S.AP.InitExecute("Z_M_LESEN_LHS", "I_KUNNR,I_LIZNR", Right("0000000000" & m_objUser.KUNNR, 10), Referenz)


            Dim sapTable As DataTable = S.AP.GetExportTable("GT_WEB") 'DirectCast(ExportVertragsdaten.Value, DataTable)
            m_tblResult = sapTable
            
            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR & ",I_LIZNR=" & Referenz, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Leasingnehmerdaten.<br>(" & ex.Message & ")"
            End Select
            
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
            
        End Try

        Return m_tblResult
    End Function

    '----------------------------------------------------------------------
    ' Methode:      GetAdresse
    ' Autor:        SFa
    ' Beschreibung: Ruft das BAPI Z_M_IMP_AUFTRDAT_007 auf und liefert eine
    '               Tabelle mit Adressdaten zurück
    ' Erstellt am:  04.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Function GetAdresse(ByVal Kennung As String, ByVal Name As String, ByVal PLZ As String, ByVal Ort As String) As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        'Dim intID As Int32 = -1

        Dim ExportTable As New DataTable


        Try
            'strCom = "EXEC Z_M_IMP_AUFTRDAT_007 @I_KUNNR='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@I_KENNUNG='" & Kennung & "'," _
            '            & "@I_NAME1='" & Name & "'," _
            '            & "@I_PSTLZ='" & PLZ & "'," _
            '            & "@GT_WEB=@Exportdaten OUTPUT" _
            '            & " OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            ''Exportparameter
            'Dim Exportdaten As New SAPParameter("@Exportdaten", ParameterDirection.Output)
            'cmd.Parameters.Add(Exportdaten)

            'cmd.ExecuteNonQuery()

            S.AP.Init("Z_M_IMP_AUFTRDAT_007")

            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_KENNUNG", Kennung)
            S.AP.SetImportParameter("I_NAME1", Name)
            S.AP.SetImportParameter("I_PSTLZ", PLZ)

            S.AP.Execute()

            Dim sapTable As DataTable = S.AP.GetExportTable("GT_WEB") 'DirectCast(Exportdaten.Value, DataTable)
            m_tblResult = sapTable

            'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
            '                                            "Z_M_IMP_AUFTRDAT_007", m_strAppID, m_strSessionID, _
            '                                            m_objUser.CurrentLogAccessASPXID)

            'If intID > -1 Then
            '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
            'End If


            With ExportTable
                .Columns.Add("POS_TEXT", Type.GetType("System.String"))
                .Columns.Add("POS_KURZTEXT", Type.GetType("System.String"))
                .Columns.Add("Adresse", Type.GetType("System.String"))
                .Columns.Add("Name1", Type.GetType("System.String"))
                .Columns.Add("Name2", Type.GetType("System.String"))
                .Columns.Add("STRAS", Type.GetType("System.String"))
                .Columns.Add("PSTLZ", Type.GetType("System.String"))
                .Columns.Add("ORT01", Type.GetType("System.String"))
                .Columns.Add("EMAIL", Type.GetType("System.String"))
                .Columns.Add("LAND1", Type.GetType("System.String"))
                .Columns.Add("TELNR", Type.GetType("System.String"))
            End With

            Dim Row As DataRow
            Dim NewRow As DataRow
            Dim i As Long = 0
            Dim Adresse As String

            For Each Row In m_tblResult.Rows

                NewRow = ExportTable.NewRow

                NewRow("POS_TEXT") = Row("POS_TEXT").ToString
                NewRow("POS_KURZTEXT") = Row("POS_KURZTEXT").ToString
                NewRow("Name1") = Row("Name1").ToString
                NewRow("Name2") = Row("Name2").ToString
                NewRow("STRAS") = Row("STRAS").ToString
                NewRow("PSTLZ") = Row("PSTLZ").ToString
                NewRow("ORT01") = Row("ORT01").ToString
                NewRow("EMAIL") = Row("EMAIL").ToString
                NewRow("LAND1") = Row("LAND1").ToString
                NewRow("TELNR") = Row("TELNR").ToString

                'Adresse für Ausgabe in Dropdown verketten
                Adresse = Row("Name1").ToString & "|"
                Adresse = Adresse & Row("STRAS").ToString & "|"
                Adresse = Adresse & Row("PSTLZ").ToString & "|"
                Adresse = Adresse & Row("ORT01").ToString

                NewRow("Adresse") = Adresse

                ExportTable.Rows.Add(NewRow)
                i = i + 1
            Next

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR & ",I_KENNUNG=" & Kennung, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."

                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Leasingnehmerdaten.<br>(" & ex.Message & ")"

            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Throw New Exception(m_strMessage)

        End Try

        Return ExportTable
    End Function

    '----------------------------------------------------------------------
    ' Methode:      Save
    ' Autor:        SFa
    ' Beschreibung: Speichert die Beauftragung im BAPI Z_M_ORDER_LN
    ' Erstellt am:  05.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Sub Save(ByVal SaveData As UeberfDAD)

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String

        Dim ImportRow As DataRow
        Dim Bemerkung As String = String.Empty
        Dim TerminhinweisAus As String = String.Empty
        Dim BemerkungAus1 As String = String.Empty
        Dim BemerkungAus2 As String = String.Empty
        Dim Bemerkung5 As String = String.Empty
        Dim Bemerkung6 As String = String.Empty
        Dim BemerkungRueckConcat As String = String.Empty
        Dim BemerkungRueck1 As String = String.Empty
        Dim BemerkungRueck2 As String = String.Empty
        Dim BemerkungWinter1 As String = String.Empty
        Dim BemerkungWinter2 As String = String.Empty
        Dim VDatum As Date
        Dim EquiNr As String = String.Empty
        Dim intID As Int32 = -1

        Try

            VDatum = Today
            
            With SaveData

                If Len(.EquiNr) > 0 Then EquiNr = .EquiNr

                Select Case SaveData.Auftragsart
                    'Auslieferung
                    Case Auftragsarten.Auslieferung.ToString, Auftragsarten.AuslieferungRueckfuehrung.ToString, _
                            Auftragsarten.ZulassungAuslieferung.ToString, Auftragsarten.Alles.ToString

                        Bemerkung = "Fahrzeugnutzer: " & .FnName & " Telefon: " & .FnTelefon & " E-Mail: " & .FnMail & " Tankkarten: " & .Tankkarten

                        If .Auftragsart = Auftragsarten.AuslieferungRueckfuehrung.ToString OrElse _
                                                  .Auftragsart = Auftragsarten.Alles.ToString Then

                            Bemerkung = Bemerkung & BemerkungRueckConcat

                        End If

                        TerminhinweisAus = "Datum Auslieferung: " & .AuslieferungDatum & _
                                        "(" & .TerminhinweisAuslieferung & ")"


                        If Auftragsarten.AuslieferungRueckfuehrung.ToString = SaveData.Auftragsart Or Auftragsarten.Alles.ToString = SaveData.Auftragsart Then
                            TerminhinweisAus = TerminhinweisAus & " Wunschtermin: " & SaveData.Wunschtermin
                        End If

                        'Bemerkung Auslieferung
                        If Len(.BemerkungAus) > 132 Then
                            BemerkungAus1 = Left(.BemerkungAus, 132)
                            BemerkungAus2 = Mid(.BemerkungAus, 133)
                        Else
                            BemerkungAus1 = .BemerkungAus
                        End If

                        'Winterreifen
                        If Len(.WinterText) > 132 Then
                            BemerkungWinter1 = Left(.WinterText, 132)
                            BemerkungWinter2 = Mid(.WinterText, 133)
                        Else
                            BemerkungWinter1 = .WinterText
                        End If

                        'Bemerkung Rückführung
                        Bemerkung6 = .RBemerkung
                        Bemerkung5 = .Bemerkung5

                    Case Auftragsarten.Rueckfuehrung.ToString

                        Bemerkung = BemerkungRueckConcat

                        'Bemerkung Rückführung(Einzelne Rückführung wird wie Auslieferung übergeben!)
                        If Len(.RBemerkung) > 132 Then
                            BemerkungAus1 = Left(.RBemerkung, 132)
                            BemerkungAus2 = Mid(.RBemerkung, 133)
                        Else
                            BemerkungAus1 = .RBemerkung
                        End If

                        TerminhinweisAus = "Wunschtermin(" & SaveData.RWunschterminart & "): " & SaveData.RWunschtermin

                End Select


                'strCom = "EXEC Z_M_ORDER_LN @AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
                '& "@VBELN_UEBERF=@ExVBELN_UEBERF OUTPUT," _
                '& "@VBELN_ZUL=@ExVBELN_ZUL OUTPUT," _
                '& "@VDATU='" & VDatum & "'," _
                '& "@AUGRU='ZFB'," _
                '& "@WEB_USER='" & ResizeForImport(m_objUser.UserName, 50) & "'," _
                '& "@EQUNR='" & EquiNr & "'," _
                '& "@BEMERKUNG='" & ResizeForImport(Bemerkung, 132) & "'," _
                '& "@BEMERKUNG01='" & ResizeForImport(BemerkungAus1, 132) & "'," _
                '& "@BEMERKUNG02='" & ResizeForImport(BemerkungAus2, 132) & "'," _
                '& "@BEMERKUNG03='" & ResizeForImport(BemerkungRueck1, 132) & "'," _
                '& "@BEMERKUNG04='" & ResizeForImport(BemerkungRueck2, 132) & "'," _
                '& "@BEMERKUNG05='" & ResizeForImport(Bemerkung5, 132) & "'," _
                '& "@BEMERKUNG06='" & ResizeForImport(Bemerkung6, 132) & "'," _
                '& "@WINTER01='" & ResizeForImport(BemerkungWinter1, 132) & "'," _
                '& "@WINTER02='" & ResizeForImport(BemerkungWinter2, 132) & "'," _
                '& "@TERMINHINWEIS='" & ResizeForImport(TerminhinweisAus, 132) & "'," _
                '& "@HIN=@ImportHin," _
                '& "@RUECK=@ImportRueck," _
                '& "@ZUL=@ImportZul," _
                '& "@IHREZ_E='" & ResizeForImport(.Buchungscode, 12) & "'," _
                '& "@PARTNER_UEBERF=@ImportPartnerUeberf," _
                '& "@PARTNER_ZUL=@ImportPartnerZul," _
                '& "@RETURN_UEBERF=@ExportUeberf OUTPUT," _
                '& "@RETURN_ZUL=@ExportZul OUTPUT OPTION 'disabledatavalidation'"
            End With

            S.AP.Init("Z_M_ORDER_LN")

            S.AP.SetImportParameter("AG", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("VDATU", VDatum)
            S.AP.SetImportParameter("AUGRU", "ZFB")
            S.AP.SetImportParameter("WEB_USER", ResizeForImport(m_objUser.UserName, 50))
            S.AP.SetImportParameter("EQUNR", EquiNr)
            S.AP.SetImportParameter("BEMERKUNG", ResizeForImport(Bemerkung, 132))
            S.AP.SetImportParameter("BEMERKUNG01", ResizeForImport(BemerkungAus1, 132))
            S.AP.SetImportParameter("BEMERKUNG02", ResizeForImport(BemerkungAus2, 132))
            S.AP.SetImportParameter("BEMERKUNG03", ResizeForImport(BemerkungRueck1, 132))
            S.AP.SetImportParameter("BEMERKUNG04", ResizeForImport(BemerkungRueck2, 132))
            S.AP.SetImportParameter("BEMERKUNG05", ResizeForImport(Bemerkung5, 132))
            S.AP.SetImportParameter("BEMERKUNG06", ResizeForImport(Bemerkung6, 132))
            S.AP.SetImportParameter("WINTER01", ResizeForImport(BemerkungWinter1, 132))
            S.AP.SetImportParameter("WINTER02", ResizeForImport(BemerkungWinter2, 132))
            S.AP.SetImportParameter("TERMINHINWEIS", ResizeForImport(TerminhinweisAus, 132))
            S.AP.SetImportParameter("IHREZ_E", ResizeForImport(SaveData.Buchungscode, 12))

            'cmd.Connection = con
            'cmd.CommandText = strCom

            'Dim ExVBELN_UEBERF As New SAPParameter("@ExVBELN_UEBERF", ParameterDirection.Output)
            'cmd.Parameters.Add(ExVBELN_UEBERF)

            'Dim ExVBELN_ZUL As New SAPParameter("@ExVBELN_ZUL", ParameterDirection.Output)
            'cmd.Parameters.Add(ExVBELN_ZUL)

            'Importparameter
            Dim ImportHin As DataTable = S.AP.GetImportTable("HIN")
            Dim ImportRueck As DataTable = S.AP.GetImportTable("RUECK")
            Dim ImportZul As DataTable = S.AP.GetImportTable("ZUL")
            Dim PartnerUeberf As DataTable = S.AP.GetImportTable("PARTNER_UEBERF")
            
            'Auslieferung
            Select Case SaveData.Auftragsart
                Case Auftragsarten.Auslieferung.ToString, Auftragsarten.AuslieferungRueckfuehrung.ToString,
                    Auftragsarten.ZulassungAuslieferung.ToString, Auftragsarten.Alles.ToString

                    ImportRow = ImportHin.NewRow

                    With SaveData

                        ImportRow("ZZBRIEF") = ResizeForImport(.Briefnummer, 10)
                        ImportRow("ZZKENN") = ResizeForImport(.Kennzeichen, 20)
                        ImportRow("ZZREFNR") = ResizeForImport(.LeasingnehmerReferenz, 20)
                        ImportRow("ZZFAHRG") = ResizeForImport(.LnFahrgestellnummer, 20)
                        ImportRow("ZZFAHRZGTYP") = ResizeForImport(.Fahrzeugtyp, 25)
                        ImportRow("ZFZGKAT") = "N"
                        ImportRow("FZGART") = .FahrzeugklasseValue
                        ImportRow("WASCHEN") = .Waesche.ToString
                        ImportRow("TANKE") = .Tanken.ToString
                        ImportRow("EINW") = .FzgEinweisung.ToString
                        ImportRow("SOWI") = .SomWin

                        ImportRow("EXKUNNR_ZL") = ResizeForImport(.Leasingnehmernummer, 12)

                    End With

                    ImportHin.Rows.Add(ImportRow)

                Case Auftragsarten.Rueckfuehrung.ToString
                    ImportRow = ImportHin.NewRow

                    With SaveData

                        ImportRow("ZZKENN") = ResizeForImport(.RKennzeichen, 20)
                        ImportRow("ZZREFNR") = ResizeForImport(.RLeasingnehmerReferenz, 20)
                        ImportRow("ZZFAHRG") = ResizeForImport(.RFahrgestellnummer, 20)
                        ImportRow("ZZFAHRZGTYP") = ResizeForImport(.RFahrzeugtyp, 25)
                        ImportRow("ZFZGKAT") = Left(.RFahrzeugStatus, 1)
                        ImportRow("FZGART") = .RFahrzeugklasseValue
                        ImportRow("SOWI") = .RSomWin

                        ImportRow("EXKUNNR_ZL") = ResizeForImport(.RLeasingnehmernummer, 12)

                    End With

                    ImportHin.Rows.Add(ImportRow)
            End Select
            
            Select Case SaveData.Auftragsart
                Case Auftragsarten.AuslieferungRueckfuehrung.ToString, Auftragsarten.Alles.ToString

                    ImportRow = ImportRueck.NewRow

                    With SaveData

                        ImportRow("ZZKENN") = ResizeForImport(.RKennzeichen, 20)
                        ImportRow("ZZREFNR") = ResizeForImport(.RLeasingnehmerReferenz, 20)
                        ImportRow("ZZFAHRG") = ResizeForImport(.RFahrgestellnummer, 20)
                        ImportRow("ZZFAHRZGTYP") = ResizeForImport(.RFahrzeugtyp, 20)
                        ImportRow("ZFZGKAT") = Left(.RFahrzeugStatus, 1)
                        ImportRow("FZGART") = .RFahrzeugklasseValue
                        ImportRow("SOWI") = .RSomWin

                        ImportRow("EXKUNNR_ZL") = ResizeForImport(.RLeasingnehmernummer, 12)

                    End With

                    ImportRueck.Rows.Add(ImportRow)

            End Select

            'ImportZul füllen
            Select Case SaveData.Auftragsart
                Case Auftragsarten.Zulassung.ToString, Auftragsarten.ZulassungAuslieferung.ToString, _
                    Auftragsarten.Alles.ToString


                    ImportRow = ImportZul.NewRow
                    Dim WunschkennzeichenData As String = String.Empty

                    With SaveData

                        If .Wunschkennzeichen1 <> String.Empty Then
                            WunschkennzeichenData = .Wunschkennzeichen1
                            If .Wunschkennzeichen2 <> String.Empty Then
                                WunschkennzeichenData = WunschkennzeichenData & "," & .Wunschkennzeichen2
                                If .Wunschkennzeichen3 <> String.Empty Then
                                    WunschkennzeichenData = WunschkennzeichenData & "," & .Wunschkennzeichen3
                                End If
                            End If

                        End If

                        'Reservierungsname und -nummer in das Wunschkennzeichen eintragen
                        If .ResNummer <> String.Empty Then
                            If WunschkennzeichenData <> String.Empty Then
                                WunschkennzeichenData = WunschkennzeichenData & ", " & .ResNummer
                            Else
                                WunschkennzeichenData = .ResNummer
                            End If
                        End If

                        If .ResName <> String.Empty Then
                            If WunschkennzeichenData <> String.Empty Then
                                WunschkennzeichenData = WunschkennzeichenData & ", " & .ResName
                            Else
                                WunschkennzeichenData = .ResName
                            End If
                        End If

                        ImportRow("ZZFAHRG") = ResizeForImport(.LnFahrgestellnummer, 20)
                        ImportRow("ZZBRIEF") = ResizeForImport(.Briefnummer, 10)
                        ImportRow("ZZREFNR") = ResizeForImport(.LeasingnehmerReferenz, 20)
                        ImportRow("ZULDAT") = .Zulassungsdatum 'ChangeDate(.Zulassungsdatum)
                        ImportRow("EVBNR") = ResizeForImport(.EVBNummer, 7)
                        If String.IsNullOrEmpty(.EVBVon) = False Then
                            ImportRow("EVBVONDAT") = .EVBVon
                        End If

                        If String.IsNullOrEmpty(.EVBBis) = False Then
                            ImportRow("EVBBISDAT") = .EVBBis
                        End If

                        ImportRow("WUNSCHKENNZ") = ResizeForImport(WunschkennzeichenData, 132)
                        ImportRow("VERSICHERUNG") = ResizeForImport(.VersGesellschaft, 132)
                        ImportRow("TERMINHINWEIS") = .ZulTerminart
                        ImportRow("FEINSTAUBPL") = .Feinstaub
                        ImportRow("STEUERN") = .KfzSteuer
                        ImportRow("EXKUNNR_ZL") = ResizeForImport(.Leasingnehmernummer, 12)

                        ImportZul.Rows.Add(ImportRow)

                    End With


            End Select

            'PartnerUeberf und PartnerZul haben diesselbe Struktur
            Dim PartnerZul As DataTable = S.AP.GetImportTable("PARTNER_ZUL")

            'PartnerZul = PartnerUeberf.Clone

            'Partner Zulassung
            Select Case SaveData.Auftragsart
                Case Auftragsarten.Zulassung.ToString, Auftragsarten.ZulassungAuslieferung.ToString, _
                    Auftragsarten.Alles.ToString


                    Dim PartnerZulRow As DataRow = PartnerZul.NewRow

                    'Rechnungsemfänger
                    'PartnerZulRow = PartnerZul.NewRow

                    PartnerZulRow("PARTN_ROLE") = "RE"
                    PartnerZulRow("PARTN_NUMB") = Right("0000000000" & m_objUser.Reference, 10)

                    PartnerZul.Rows.Add(PartnerZulRow)

                    'Regulierer
                    PartnerZulRow = PartnerZul.NewRow

                    PartnerZulRow("PARTN_ROLE") = "RG"
                    PartnerZulRow("PARTN_NUMB") = Right("0000000000" & m_objUser.Reference, 10)

                    PartnerZul.Rows.Add(PartnerZulRow)

                    PartnerZulRow = PartnerZul.NewRow

                    With SaveData
                        'Halter
                        PartnerZulRow("PARTN_ROLE") = "ZH"
                        PartnerZulRow("NAME") = ResizeForImport(.Halter, 35)
                        PartnerZulRow("STREET") = ResizeForImport(.HalterStrasse, 35)
                        PartnerZulRow("POSTL_CODE") = .HalterPLZ
                        PartnerZulRow("CITY") = ResizeForImport(.HalterOrt, 35)
                        PartnerZulRow("COUNTRY") = ResizeForImport(.HalterLand, 3)

                        PartnerZul.Rows.Add(PartnerZulRow)

                        PartnerZulRow = PartnerZul.NewRow
                        ' Versicherungsnehmer
                        PartnerZulRow("PARTN_ROLE") = "ZC"
                        PartnerZulRow("NAME") = ResizeForImport(.VersNehmer, 35)
                        'PartnerZulRow("NAME_2") = .VersAnsprechpartner
                        PartnerZulRow("STREET") = ResizeForImport(.VersNehmerStrasse, 35)
                        PartnerZulRow("POSTL_CODE") = .VersNehmerPLZ
                        PartnerZulRow("CITY") = ResizeForImport(.VersNehmerOrt, 35)
                        PartnerZulRow("COUNTRY") = ResizeForImport(.VersNehmerLand, 3)
                        'PartnerZulRow("TELEPHONE") = .VersTelefon
                        'PartnerZulRow("SMTP_ADDR") = .VersMail
                        PartnerZulRow("FLGDEFAULT") = "X"

                        PartnerZul.Rows.Add(PartnerZulRow)

                        PartnerZulRow = PartnerZul.NewRow
                        'Versand Schein und Schilder
                        PartnerZulRow("PARTN_ROLE") = "ZE"
                        PartnerZulRow("NAME") = ResizeForImport(.VssName, 35)
                        PartnerZulRow("NAME_2") = ResizeForImport(.VssName2, 35)
                        PartnerZulRow("STREET") = ResizeForImport(.VssStrasse, 35)
                        PartnerZulRow("POSTL_CODE") = .VssPLZ
                        PartnerZulRow("CITY") = ResizeForImport(.VssOrt, 35)
                        PartnerZulRow("COUNTRY") = "DE"

                        PartnerZul.Rows.Add(PartnerZulRow)

                        PartnerZulRow = PartnerZul.NewRow
                        'Leasingnehmer
                        PartnerZulRow("PARTN_ROLE") = "ZL"
                        PartnerZulRow("PARTN_NUMB") = "0000390051"
                        PartnerZulRow("NAME") = ResizeForImport(.Leasingnehmer, 35)
                        PartnerZulRow("NAME_4") = ResizeForImport(.Leasingnehmernummer, 35)
                        PartnerZulRow("STREET") = ResizeForImport(.LeasingnehmerStrasse, 35)
                        PartnerZulRow("POSTL_CODE") = .LeasingnehmerPLZ
                        PartnerZulRow("CITY") = ResizeForImport(.LeasingnehmerOrt, 35)
                        PartnerZulRow("COUNTRY") = "DE"

                        PartnerZul.Rows.Add(PartnerZulRow)


                    End With

            End Select


            With SaveData

                'Auftragsarten mit Auslieferung
                If .Auftragsart <> Auftragsarten.Zulassung.ToString Then

                    Dim PartnerUeberfRow As DataRow = PartnerUeberf.NewRow

                    'Rechnungsemfänger
                    'PartnerUeberfRow = PartnerUeberf.NewRow

                    PartnerUeberfRow("PARTN_ROLE") = "RE"
                    PartnerUeberfRow("PARTN_NUMB") = Right("0000000000" & m_objUser.Reference, 10)

                    PartnerUeberf.Rows.Add(PartnerUeberfRow)

                    'Regulierer
                    PartnerUeberfRow = PartnerUeberf.NewRow

                    PartnerUeberfRow("PARTN_ROLE") = "RG"
                    PartnerUeberfRow("PARTN_NUMB") = Right("0000000000" & m_objUser.Reference, 10)

                    PartnerUeberf.Rows.Add(PartnerUeberfRow)

                    'PartnerUeberfRow = PartnerUeberf.NewRow

                    If .Auftragsart = Auftragsarten.Rueckfuehrung.ToString Then
                        'Abholadresse
                        PartnerUeberfRow = PartnerUeberf.NewRow

                        PartnerUeberfRow("PARTN_ROLE") = "ZB"
                        PartnerUeberfRow("PARTN_NUMB") = "0000390051"
                        PartnerUeberfRow("NAME") = ResizeForImport(.RAbName, 35)
                        PartnerUeberfRow("NAME_2") = ResizeForImport(.RAbAnsprechpartner, 35)
                        PartnerUeberfRow("STREET") = ResizeForImport(.RAbStrasse, 35)
                        PartnerUeberfRow("POSTL_CODE") = .RAbPLZ
                        PartnerUeberfRow("CITY") = ResizeForImport(.RAbOrt, 35)
                        PartnerUeberfRow("COUNTRY") = "DE"
                        PartnerUeberfRow("TELEPHONE") = ResizeForImport(.RAbTelefon, 16)
                        PartnerUeberfRow("TELEPHONE2") = ResizeForImport(.RAbHandy, 16)
                        PartnerUeberfRow("SMTP_ADDR") = ResizeForImport(.RAbMail, 241)
                        PartnerUeberfRow("FLGDEFAULT") = "X"

                        PartnerUeberf.Rows.Add(PartnerUeberfRow)

                        PartnerUeberfRow = PartnerUeberf.NewRow
                        'Anlieferadresse
                        PartnerUeberfRow("PARTN_ROLE") = "WE"
                        PartnerUeberfRow("PARTN_NUMB") = "0000390051"
                        PartnerUeberfRow("NAME") = ResizeForImport(.RAnName, 35)
                        PartnerUeberfRow("NAME_2") = ResizeForImport(.RAnAnsprechpartner, 35)
                        PartnerUeberfRow("STREET") = ResizeForImport(.RAnStrasse, 35)
                        PartnerUeberfRow("POSTL_CODE") = .RAnPLZ
                        PartnerUeberfRow("CITY") = ResizeForImport(.RAnOrt, 35)
                        PartnerUeberfRow("COUNTRY") = "DE"
                        PartnerUeberfRow("TELEPHONE") = ResizeForImport(.RAnTelefon, 16)
                        PartnerUeberfRow("TELEPHONE2") = ResizeForImport(.RAnHandy, 16)
                        PartnerUeberfRow("SMTP_ADDR") = ResizeForImport(.RAnMail, 241)
                        PartnerUeberfRow("FLGDEFAULT") = "X"

                        PartnerUeberf.Rows.Add(PartnerUeberfRow)

                    End If

                    If .Auftragsart <> Auftragsarten.Rueckfuehrung.ToString Then
                        PartnerUeberfRow = PartnerUeberf.NewRow
                        'Händler Auslieferung
                        PartnerUeberfRow("PARTN_ROLE") = "ZB"
                        PartnerUeberfRow("PARTN_NUMB") = "0000390051"
                        PartnerUeberfRow("NAME") = ResizeForImport(.HaendlerName1, 35)
                        PartnerUeberfRow("NAME_2") = ResizeForImport(.HaendlerAnsprech, 35)
                        PartnerUeberfRow("STREET") = ResizeForImport(.HaendlerStrasse, 35)
                        PartnerUeberfRow("POSTL_CODE") = .HaendlerPLZ
                        PartnerUeberfRow("CITY") = ResizeForImport(.HaendlerOrt, 35)
                        PartnerUeberfRow("COUNTRY") = "DE"
                        PartnerUeberfRow("TELEPHONE") = ResizeForImport(.HaendlerTelefon, 16)
                        PartnerUeberfRow("TELEPHONE2") = ResizeForImport(.HaendlerTelefon2, 16)
                        PartnerUeberfRow("SMTP_ADDR") = ResizeForImport(.HaendlerMail, 241)
                        PartnerUeberfRow("FLGDEFAULT") = "X"


                        PartnerUeberf.Rows.Add(PartnerUeberfRow)

                        PartnerUeberfRow = PartnerUeberf.NewRow
                        'Fahrzeugempfänger
                        PartnerUeberfRow("PARTN_ROLE") = "WE"
                        PartnerUeberfRow("PARTN_NUMB") = "0000390051"
                        PartnerUeberfRow("NAME") = ResizeForImport(.EmpfName, 35)
                        PartnerUeberfRow("NAME_2") = ResizeForImport(.EmpfAnsprechpartner, 35)
                        PartnerUeberfRow("STREET") = ResizeForImport(.EmpfStrasse, 35)
                        PartnerUeberfRow("POSTL_CODE") = .EmpfPLZ
                        PartnerUeberfRow("CITY") = ResizeForImport(.EmpfOrt, 35)
                        PartnerUeberfRow("COUNTRY") = "DE"
                        PartnerUeberfRow("TELEPHONE") = ResizeForImport(.EmpfTelefon, 16)
                        PartnerUeberfRow("TELEPHONE2") = ResizeForImport(.EmpfTelefon2, 16)
                        PartnerUeberfRow("SMTP_ADDR") = ResizeForImport(.EmpfMail, 241)
                        PartnerUeberfRow("FLGDEFAULT") = "X"

                        PartnerUeberf.Rows.Add(PartnerUeberfRow)

                        PartnerUeberfRow = PartnerUeberf.NewRow
                        'Leasingnehmer
                        PartnerUeberfRow("PARTN_ROLE") = "ZL"
                        PartnerUeberfRow("PARTN_NUMB") = "0000390051"
                        PartnerUeberfRow("NAME") = ResizeForImport(.Leasingnehmer, 35)
                        PartnerUeberfRow("STREET") = ResizeForImport(.LeasingnehmerStrasse, 35)
                        PartnerUeberfRow("POSTL_CODE") = .LeasingnehmerPLZ
                        PartnerUeberfRow("CITY") = ResizeForImport(.LeasingnehmerOrt, 35)
                        PartnerUeberfRow("COUNTRY") = "DE"

                        PartnerUeberf.Rows.Add(PartnerUeberfRow)

                        If .Auftragsart = Auftragsarten.AuslieferungRueckfuehrung.ToString OrElse
                            .Auftragsart = Auftragsarten.Alles.ToString Then
                            '... + Rückführung
                            PartnerUeberfRow = PartnerUeberf.NewRow

                            PartnerUeberfRow("PARTN_ROLE") = "ZR"
                            PartnerUeberfRow("PARTN_NUMB") = "0000390051"
                            PartnerUeberfRow("NAME") = ResizeForImport(.RAnName, 35)
                            PartnerUeberfRow("NAME_2") = ResizeForImport(.RAnAnsprechpartner, 35)
                            PartnerUeberfRow("STREET") = ResizeForImport(.RAnStrasse, 35)
                            PartnerUeberfRow("POSTL_CODE") = .RAnPLZ
                            PartnerUeberfRow("CITY") = ResizeForImport(.RAnOrt, 35)
                            PartnerUeberfRow("COUNTRY") = "DE"
                            PartnerUeberfRow("TELEPHONE") = ResizeForImport(.RAnTelefon, 16)
                            PartnerUeberfRow("TELEPHONE2") = ResizeForImport(.RAnHandy, 16)
                            PartnerUeberfRow("SMTP_ADDR") = ResizeForImport(.RAnMail, 241)
                            PartnerUeberfRow("FLGDEFAULT") = "X"

                            PartnerUeberf.Rows.Add(PartnerUeberfRow)

                        End If

                    End If

                End If

            End With


            ''Inputparameter
            'Dim Parameter1 As New SAPParameter("@ImportHin", ImportHin)
            ''Inputparameter hinzufügen
            'cmd.Parameters.Add(Parameter1)

            'Dim Parameter2 As New SAPParameter("@ImportRueck", ImportRueck)
            ''Inputparameter hinzufügen
            'cmd.Parameters.Add(Parameter2)

            'Dim Parameter3 As New SAPParameter("@ImportZul", ImportZul)
            ''Inputparameter hinzufügen
            'cmd.Parameters.Add(Parameter3)

            'Dim Parameter4 As New SAPParameter("@ImportPartnerUeberf", PartnerUeberf)
            ''Inputparameter hinzufügen
            'cmd.Parameters.Add(Parameter4)

            'Dim Parameter5 As New SAPParameter("@ImportPartnerZul", PartnerZul)
            ''Inputparameter hinzufügen
            'cmd.Parameters.Add(Parameter5)


            ''Exportparameter
            'Dim ExportUeberf As New SAPParameter("@ExportUeberf", ParameterDirection.Output)
            'cmd.Parameters.Add(ExportUeberf)

            'Dim ExportZul As New SAPParameter("@ExportZul", ParameterDirection.Output)
            'cmd.Parameters.Add(ExportZul)

            ''Statement abschicken
            'cmd.ExecuteNonQuery()

            S.AP.Execute()

            'Exportparameter schreiben
            'SaveData.AuftragsnummerUeberf = String.Empty
            'SaveData.AuftragsnummerZul = String.Empty

            SaveData.AuftragsnummerUeberf = S.AP.GetExportParameter("VBELN_UEBERF") 'ExVBELN_UEBERF.ToString.TrimStart("0"c)
            SaveData.AuftragsnummerZul = S.AP.GetExportParameter("VBELN_ZUL") 'ExVBELN_ZUL.ToString.TrimStart("0"c)


            'Prüfen, ob die Aufträge korrekt angelegt wurden
            Select Case SaveData.Auftragsart
                Case Auftragsarten.Zulassung.ToString
                    If SaveData.AuftragsnummerZul = String.Empty Then
                        Throw New Exception("Der Auftrag konnte nicht angelegt werden.")
                    End If
                Case Auftragsarten.Auslieferung.ToString, Auftragsarten.AuslieferungRueckfuehrung.ToString
                    If SaveData.AuftragsnummerUeberf = String.Empty Then
                        Throw New Exception("Der Auftrag konnte nicht angelegt werden.")
                    End If
                Case Auftragsarten.ZulassungAuslieferung.ToString, Auftragsarten.Alles.ToString
                    If SaveData.AuftragsnummerUeberf = String.Empty AndAlso SaveData.AuftragsnummerUeberf = String.Empty Then
                        Throw New Exception("Der Auftrag konnte nicht angelegt werden.")
                    ElseIf SaveData.AuftragsnummerUeberf = String.Empty Then
                        Throw New Exception("Der Überführungsauftrag konnte nicht angelegt werden.")
                    ElseIf SaveData.AuftragsnummerZul = String.Empty Then
                        Throw New Exception("Der Zulassungsauftrag konnte nicht angelegt werden.")
                    End If

                Case Auftragsarten.Zulassung.ToString, Auftragsarten.ZulassungAuslieferung.ToString, _
                    Auftragsarten.Alles.ToString

            End Select

            WriteLogEntry(True, "I_AG=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_VKORG"
                    m_strMessage = "VKORG konnte nicht ermittelt werden."
                Case "STATUS_ERROR"
                    m_strMessage = "Fehler beim Statusändern des ÜberfOrders."
                Case "NO_MAT"
                    m_strMessage = "Kein Material für AG gefunden."
                Case "ZH_NOT_FOUND"
                    m_strMessage = "Halter nicht gefunden."
                Case "ZH_UNVOLLSTAENDIG"
                    m_strMessage = "Halteradresse ist unvollständig."
                Case "ZV_NOT_FOUND"
                    m_strMessage = "Versicherer nicht gefunden."
                Case "ZE_UNVOLLSTAENDIG"
                    m_strMessage = "Empfängeradresse Schein ist unvollständig."
                Case "ZC_NOT_FOUND"
                    m_strMessage = "abw.Versicherungsnehmer nicht gefunden."
                Case "ZC_UNVOLLSTAENDIG"
                    m_strMessage = "abw.Versicherungsnehmer ist unvollständig."
                Case "RE_NOT_FOUND"
                    m_strMessage = "Rechnungsempfänger nicht gefunden."
                Case "RG_NOT_FOUND"
                    m_strMessage = "Regulierer nicht gefunden."
                Case "NO_ZH"
                    m_strMessage = "Halter nicht angegeben."
                Case "NO_ZE"
                    m_strMessage = "Empfänger Scheinr nicht angegeben."
                Case "NO_RE"
                    m_strMessage = "Rechnungsempfänger nicht angegeben."
                Case "NO_RG"
                    m_strMessage = "Regulierer nicht angegeben."
                Case "NO_ZS_ZUM_RE"
                    m_strMessage = "Zum Rechnungsempfänger konnte kein Empfänger des Briefes gefunden werden."
                Case "INV_PLZ"
                    m_strMessage = "Postleitzahl ZH falsch."
                Case "NO_ZULST"
                    m_strMessage = "Keine Zulassungsstelle gefunden."
                Case "SMTP_ERROR"
                    m_strMessage = "E-Mail Adresse fehlerhaft."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Speichern.<br>(" & ex.Message & ")"
            End Select

            If m_intStatus <> -9999 Then
                m_strMessage = m_strMessage & " Bitte korrigieren Sie Ihre Eingaben."
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Throw New Exception(m_strMessage)

        End Try

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      GetLaender
    ' Autor:        SFa
    ' Beschreibung: Liefert Länderkürzel aus Z_M_Land_Plz_001
    ' Erstellt am:  09.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Function GetLaender() As DataTable

        'Dim intID As Int32 = -1

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        Try

            'Dim cmd As New SAPCommand()
            'cmd.Connection = con

            'Dim strCom As String

            'strCom = "EXEC Z_M_Land_Plz_001 @GT_WEB=@pIGT_WEB,@GT_PREG=@pIGT_PREG,"
            'strCom = strCom & "@GT_WEB=@pEGT_WEB OUTPUT,@GT_PREG=@pEGT_PREG OUTPUT OPTION 'disabledatavalidation'"

            'cmd.CommandText = strCom



            ''exportParameter
            'Dim pEGT_WEB As New SAPParameter("@pEGT_WEB", ParameterDirection.Output)
            'Dim pEGT_PREG As New SAPParameter("@pEGT_PREG", ParameterDirection.Output)

            'Dim pIGT_WEB As New SAPParameter("@pIGT_WEB", New DataTable)
            'Dim pIGT_PREG As New SAPParameter("@pIGT_PREG", New DataTable)


            ''exportparameter hinzugfügen
            'cmd.Parameters.Add(pEGT_WEB)
            'cmd.Parameters.Add(pEGT_PREG)

            '' importParameter(hinzufügen)
            'cmd.Parameters.Add(pIGT_WEB)
            'cmd.Parameters.Add(pIGT_PREG)

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            'cmd.ExecuteNonQuery()

            mLaender = S.AP.GetExportTableWithInitExecute("Z_M_Land_Plz_001.GT_WEB") 'DirectCast(pEGT_WEB.Value, DataTable)

            mLaender.Columns.Add("Beschreibung", Type.GetType("System.String"))
            mLaender.Columns.Add("FullDesc", Type.GetType("System.String"))

            Dim rowTemp As DataRow
            For Each rowTemp In mLaender.Rows
                If CInt(rowTemp("LNPLZ")) > 0 Then
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx")) & " (" & CStr(CInt(rowTemp("LNPLZ"))) & ")"
                Else
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx"))
                End If
                rowTemp("FullDesc") = CStr(rowTemp("Land1")) & " " & CStr(rowTemp("Beschreibung"))
            Next

        Catch ex As Exception

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select

        End Try

        Return mLaender

    End Function

    '----------------------------------------------------------------------
    ' Methode:      GeoAdressen
    ' Autor:        SFa
    ' Beschreibung: Prüft die übergebene Adresse auf Richtigkeit und liefert
    '               ggf. Alternativadresse zurück
    ' Erstellt am:  17.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Function GeoAdressen(ByVal Strasse As String, ByVal PLZ As String, ByVal Ort As String) As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        'Dim intID As Int32 = -1


        Try
            'strCom = "EXEC Z_M_CHECK_ADRESS @I_LAND='DE'," _
            '            & "@I_STRASSE='" & Strasse & "'," _
            '            & "@I_HAUSNR=''," _
            '            & "@I_POSTLTZ='" & PLZ & "'," _
            '            & "@I_ORT='" & Ort & "'," _
            '            & "@GT_GEO=@Exportdaten OUTPUT" _
            '            & " OPTION 'disabledatavalidation'"

            S.AP.Init("Z_M_CHECK_ADRESS")

            S.AP.SetImportParameter("I_LAND", "DE")
            S.AP.SetImportParameter("I_STRASSE", Strasse)
            S.AP.SetImportParameter("I_HAUSNR", "")
            S.AP.SetImportParameter("I_POSTLTZ", PLZ)
            S.AP.SetImportParameter("I_ORT", Ort)

            'cmd.Connection = con
            'cmd.CommandText = strCom

            'Exportparameter
            'Dim Exportdaten As New SAPParameter("@Exportdaten", ParameterDirection.Output)
            'cmd.Parameters.Add(Exportdaten)

            'cmd.ExecuteNonQuery()
            S.AP.Execute()

            Dim sapTable As DataTable = S.AP.GetExportTable("GT_GEO") 'DirectCast(Exportdaten.Value, DataTable)

            m_tblResult = sapTable

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "GEO_ERROR"
                    m_strMessage = "Fehler beim Aufruf der Geokodierung."
                Case "NO_DATA"
                    m_strMessage = "Keine passenden Adressen gefunden."
                Case "ADRESS_ERROR"
                    m_strMessage = "Adressdaten ungenügend."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Abholadresse.<br>(" & ex.Message & ")"

            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Throw New Exception(m_strMessage)

        End Try

        Return m_tblResult
    End Function

    '----------------------------------------------------------------------
    ' Methode:      GeoAutoland
    ' Autor:        SFa
    ' Beschreibung: Liefert das nächstgelegene Autoland zurück
    ' Erstellt am:  22.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Function GeoAutoland(ByVal GeoX As String, ByVal GeoY As String, ByVal GeoAdresse As String) As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        'Dim intID As Int32 = -1


        Try
            'strCom = "EXEC Z_M_FIND_SPLACE @I_AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@I_VKORG='1510'," _
            '            & "@IS_GEO=@GeoIn," _
            '            & "@E_SPLATZ=@ExSPLATZ OUTPUT," _
            '            & "@ES_ADRESSE=@Exportdaten OUTPUT" _
            '            & " OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            S.AP.Init("Z_M_FIND_SPLACE", "I_AG,I_VKORG", Right("0000000000" & m_objUser.KUNNR, 10), "1510")

            Dim rowNew As DataRow

            'Tabelle für Übergabe an SAP
            Dim GeoIn As DataTable = S.AP.GetImportTable("IS_GEO")

            'With GeoIn
            '    .Columns.Add("GEOX", Type.GetType("System.String"))
            '    .Columns.Add("GEOY", Type.GetType("System.String"))
            '    .Columns.Add("ADRESSE", Type.GetType("System.String"))
            '    .Columns.Add("MARK", Type.GetType("System.String"))
            'End With

            rowNew = GeoIn.NewRow

            rowNew("GEOX") = GeoX
            rowNew("GEOY") = GeoY
            rowNew("ADRESSE") = GeoAdresse


            GeoIn.Rows.Add(rowNew)

            ''Importparameter
            'Dim param As New SAPParameter("@GeoIn", GeoIn)
            ''Inputparameter hinzufügen
            'cmd.Parameters.Add(param)

            ''Exportparameter
            'Dim SPlatz As New SAPParameter("@ExSPLATZ", ParameterDirection.Output)
            'cmd.Parameters.Add(SPlatz)

            'Dim Exportdaten As New SAPParameter("@Exportdaten", ParameterDirection.Output)
            'cmd.Parameters.Add(Exportdaten)

            'cmd.ExecuteNonQuery()

            S.AP.Execute()

            Dim SAPTable As DataTable = S.AP.GetExportTable("ES_ADRESSE") 'DirectCast(Exportdaten.Value, DataTable)
            m_tblResult = SAPTable

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_PLACE_FOUND"
                    m_strMessage = "Kein Fahrzeugsammelplatz gefunden"
                Case "KUNNR_NOT_FOUND"
                    m_strMessage = "Ermittelter Kunde (Fahrzeugsammelplatz) nicht im SAP angelegt"
                Case "GEO_ERROR"
                    m_strMessage = "Fehler beim Aufruf der Geokodierung"
                Case "NO_DATA"
                    m_strMessage = "Keine passenden Adressen gefunden"
                Case "ADRESS_ERROR"
                    m_strMessage = "Adressdaten ungenügend"
                Case "SPLACE_ERROR"
                    m_strMessage = "Fahrzeugsammelplatzadresse nicht im GEO gefunden"

                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen des Autolandes.<br>(" & ex.Message & ")"

            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Throw New Exception(m_strMessage)

        End Try

        Return m_tblResult

    End Function


    '----------------------------------------------------------------------
    ' Methode:      GetEquiData
    ' Autor:        SFa
    ' Beschreibung: Liefert Daten aus dem Equipment
    ' Erstellt am:  26.09.2008
    ' ITA:          2258
    '----------------------------------------------------------------------
    Public Function GetEquiData(ByVal Vertragsnummer As String) As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        'Dim intID As Int32 = -1


        Try
            'strCom = "EXEC Z_M_READ_EQUI_LN @I_KUNNR='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@I_LIZNR='" & Vertragsnummer & "'," _
            '            & "@I_EQUNR=''," _
            '            & "@GT_DATEN=@Exportdaten OUTPUT" _
            '            & " OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            'Dim Exportdaten As New SAPParameter("@Exportdaten", ParameterDirection.Output)
            'cmd.Parameters.Add(Exportdaten)

            S.AP.InitExecute("Z_M_READ_EQUI_LN", "I_KUNNR,I_LIZNR", Right("0000000000" & m_objUser.KUNNR, 10), Vertragsnummer)

            'cmd.ExecuteNonQuery()

            Dim sapTable As DataTable = S.AP.GetExportTable("GT_DATEN") 'DirectCast(Exportdaten.Value, DataTable)
            m_tblResult = sapTable


            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten zur Vertragsnummer gefunden."
                Case "NO_SELECT_PAR"
                    m_strMessage = "Importparameter Vertrag und Vertrag leer."

                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen des Equipments.<br>(" & ex.Message & ")"

            End Select


            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        End Try

        Return m_tblResult

    End Function


    '----------------------------------------------------------------------
    ' Methode:      ChangeDate
    ' Autor:        SFa
    ' Beschreibung: Wandelt den übergebenen Wert in ein SAP-Datumswert um
    ' Erstellt am:  23.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    'Private Function ChangeDate(ByVal Datum As String) As String

    '    Dim Temp As String = String.Empty
    '    If Datum = Nothing Then
    '        Temp = "00000000"
    '    Else
    '        If Datum.Length = 10 Then
    '            Temp = Datum.Replace(".", "")
    '            Temp = Right(Temp, 4) & Mid(Temp, 3, 2) & Left(Temp, 2)
    '        Else
    '            Temp = "00000000"
    '        End If
    '    End If

    '    Return Temp

    'End Function

    Private Function ResizeForImport(ByVal ImportString As String, ByVal MaxLength As Integer) As String

        If String.IsNullOrEmpty(ImportString) Then
            ImportString = ""
        End If

        Dim ExportString As String = ImportString

        If ExportString.Length > MaxLength Then
            ExportString = Left(ExportString, MaxLength)
        End If

        Return ExportString
    End Function
    

#End Region

End Class
' ************************************************
' $History: UeberfDADTables.vb $
' 
' *****************  Version 24  *****************
' User: Fassbenders  Date: 7.12.10    Time: 11:32
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 23  *****************
' User: Jungj        Date: 11.08.10   Time: 8:59
' Updated in $/CKAG/Applications/AppUeberf/Lib
' ITA 4018 Z_M_ORDER_LN Struktur anpassung
' 
' *****************  Version 22  *****************
' User: Jungj        Date: 16.12.08   Time: 8:43
' Updated in $/CKAG/Applications/AppUeberf/Lib
' ita 2472 testfertig
' 
' *****************  Version 21  *****************
' User: Fassbenders  Date: 8.12.08    Time: 14:41
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 20  *****************
' User: Jungj        Date: 4.12.08    Time: 9:05
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 2377 wiederherstellung
' 
' *****************  Version 19  *****************
' User: Jungj        Date: 3.12.08    Time: 9:20
' Updated in $/CKAG/Applications/AppUeberf/Lib
' zurckentwicklung 2377
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 2.12.08    Time: 10:05
' Updated in $/CKAG/Applications/AppUeberf/Lib
' ita 2377
' testfertig
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 3.11.08    Time: 11:15
' Updated in $/CKAG/Applications/AppUeberf/Lib
' ITA 2343 fertigstellung 
'
' ************************************************