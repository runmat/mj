Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class fin_06
    REM § Lese-/Schreibfunktion, Kunde: Übergreifend, 
    REM § GiveCars - BAPI: Z_M_Unangefordert_002,
    REM § Anfordern - BAPI: Z_M_Briefanforderung.

    Inherits BankBaseCredit

#Region " Declarations"
    Private m_tblAbrufgruende As DataTable
    Private m_strSucheTIDNR As String = ""
    Private m_strSucheZZREFERENZ1 As String = ""
    Private m_strSucheFahrgestellNr As String = String.Empty
    Private m_strName1 As String
    Private m_strName2 As String
    Private m_strName3 As String
    Private m_strCity As String
    Private m_strPostcode As String
    Private m_strStreet As String
    Private m_strHouseNum As String
    Private m_tblFahrzeuge As DataTable
    Private m_tblZulStelle As DataTable
    Private m_strZZFAHRG As String
    Private m_strAdresse As String
    Private m_strAuftragsnummer As String
    Private m_strAuftragsstatus As String
    Private m_strAuftragsstatus2 As String
    Private m_strKreditkontrollBereich As String
    Private m_strMaterialNummer As String 'Versandart
    Private m_strAdresseHalter As String    'HEZ
    Private m_strAdresseEmpf As String    'HEZ
    Private m_preis As Decimal 'HEZ
    Private m_datumRange As DataTable       'HEZ:Hier Datumswerte auslesen...
    Private m_kbanr As String 'Zulassungsdienst-Nummer
    Private m_strLIZNR As String = ""
    Private m_boolNewAdress As Boolean = False
    Private m_tblLaender As DataTable
    Private m_strLaenderKuerzel As String
    Private mVersandArtText As String
    Private mE_SUBRC As String
    Private mE_MESSAGE As String

#End Region

#Region " Properties"
    Public ReadOnly Property Abrufgruende() As DataTable
        Get
            If m_tblAbrufgruende Is Nothing Then
                Dim cn As SqlClient.SqlConnection
                Dim cmdAg As SqlClient.SqlCommand
                Dim dsAg As DataSet
                Dim adAg As SqlClient.SqlDataAdapter
                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try

                    cn.Open()

                    dsAg = New DataSet()

                    adAg = New SqlClient.SqlDataAdapter()

                    cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                                    "[WebBezeichnung]," & _
                                                    "[SapWert]," & _
                                                    "[MitZusatzText]," & _
                                                    "[Zusatzbemerkung], " & _
                                                    "[AbrufTyp] " & _
                                                    "FROM CustomerAbrufgruende " & _
                                                    "WHERE " & _
                                                    "CustomerID = " & m_objUser.Customer.CustomerId.ToString & _
                                                    " AND GroupID = " & m_objUser.GroupID.ToString _
                                                    , cn)
                    cmdAg.CommandType = CommandType.Text
                    'AbrufTyp: 'temp' oder 'endg'

                    adAg.SelectCommand = cmdAg
                    adAg.Fill(dsAg, "Abrufgruende")

                    If dsAg.Tables("Abrufgruende") Is Nothing OrElse dsAg.Tables("Abrufgruende").Rows.Count = 0 Then
                        Throw New Exception("Keine Abrufgründe für den Kunden hinterlegt.")
                    End If

                    m_tblAbrufgruende = dsAg.Tables("Abrufgruende")
                Catch ex As Exception
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End If

            Return m_tblAbrufgruende
        End Get
    End Property

    Public Property kbanr() As String
        Get
            Return m_kbanr
        End Get
        Set(ByVal Value As String)
            m_kbanr = Value
        End Set
    End Property

    Public Property VersandArtText() As String
        Get
            Return mVersandArtText
        End Get
        Set(ByVal Value As String)
            mVersandArtText = Value
        End Set
    End Property

    Public Property laenderKuerzel() As String
        Get
            Return m_strLaenderKuerzel
        End Get
        Set(ByVal Value As String)
            m_strLaenderKuerzel = Value
        End Set

    End Property

    Public Property neueAdresse() As Boolean
        Get
            Return m_boolNewAdress
        End Get
        Set(ByVal Value As Boolean)
            m_boolNewAdress = Value
        End Set

    End Property
    Public Property Name1() As String
        Get
            Return m_strName1
        End Get
        Set(ByVal Value As String)
            m_strName1 = Value
        End Set

    End Property

    Public Property Name2() As String
        Get
            Return m_strName2
        End Get
        Set(ByVal Value As String)
            m_strName2 = Value
        End Set

    End Property

    Public Property Name3() As String
        Get
            Return m_strName3
        End Get
        Set(ByVal Value As String)
            m_strName3 = Value
        End Set
    End Property
    Public Property City() As String
        Get
            Return m_strCity
        End Get
        Set(ByVal Value As String)
            m_strCity = Value
        End Set
    End Property

    Public Property PostCode() As String
        Get
            Return m_strPostcode
        End Get
        Set(ByVal Value As String)
            m_strPostcode = Value
        End Set
    End Property

    Public Property Street() As String
        Get
            Return m_strStreet
        End Get
        Set(ByVal Value As String)
            m_strStreet = Value
        End Set
    End Property
    Public Property HouseNum() As String
        Get
            Return m_strHouseNum
        End Get
        Set(ByVal Value As String)
            m_strHouseNum = Value
        End Set
    End Property
    Public Property preis() As Decimal
        Get
            Return m_preis
        End Get
        Set(ByVal Value As Decimal)
            m_preis = Value
        End Set

    End Property
    Public Property zuldatum() As Date
        Get
            Return m_zuldatum
        End Get
        Set(ByVal Value As Date)
            m_zuldatum = Value
        End Set
    End Property

    Public Property MaterialNummer() As String
        Get
            Return m_strMaterialNummer
        End Get
        Set(ByVal Value As String)
            m_strMaterialNummer = Value
        End Set
    End Property

    Public Property sucheLIZNR() As String
        Get
            Return m_strLIZNR
        End Get
        Set(ByVal Value As String)
            If Not m_strLIZNR Is Nothing Then
                m_strLIZNR = Value.Replace(" ", "").ToUpper
            Else
                m_strLIZNR = ""
            End If

        End Set
    End Property

    Public Property KreditkontrollBereich() As String
        Get
            Return m_strKreditkontrollBereich
        End Get
        Set(ByVal Value As String)
            m_strKreditkontrollBereich = Right("0000" & Value.Trim(" "c), 4)
        End Set
    End Property

    Public ReadOnly Property Auftragsnummer() As String
        Get
            Return m_strAuftragsnummer
        End Get
    End Property

    Public ReadOnly Property Auftragsstatus() As String
        Get
            Return m_strAuftragsstatus
        End Get
    End Property
    Public ReadOnly Property Auftragsstatus2() As String
        Get
            Return m_strAuftragsstatus2
        End Get
    End Property
    Public Property ZZFAHRG() As String
        Get
            Return m_strZZFAHRG
        End Get
        Set(ByVal Value As String)
            m_strZZFAHRG = Value
        End Set
    End Property

    Public Property Adresse() As String
        Get
            Return m_strAdresse
        End Get
        Set(ByVal Value As String)
            m_strAdresse = Right("00000000000" & Value, 10)
        End Set
    End Property

    Public Property AdresseHalter() As String
        Get
            Return m_strAdresseHalter
        End Get
        Set(ByVal Value As String)
            m_strAdresseHalter = Right("00000000000" & Value, 10)
        End Set
    End Property

    Public Property AdresseEmpf() As String
        Get
            Return m_strAdresseEmpf
        End Get
        Set(ByVal Value As String)
            m_strAdresseEmpf = Right("00000000000" & Value, 10)
        End Set
    End Property

    Public Property SucheTIDNR() As String
        Get
            Return m_strSucheTIDNR
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                m_strSucheTIDNR = Value.Replace(" ", "").ToUpper
            Else
                m_strSucheTIDNR = ""
            End If

        End Set
    End Property

    Public Property SucheZZREFERENZ1() As String
        Get
            Return m_strSucheZZREFERENZ1
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                m_strSucheZZREFERENZ1 = Value.Replace(" ", "").ToUpper
            Else
                m_strSucheZZREFERENZ1 = ""
            End If
        End Set
    End Property



    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellNr
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                m_strSucheFahrgestellNr = Value.Replace(" ", "").ToUpper
            Else
                m_strSucheFahrgestellNr = ""
            End If
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeuge = Value
        End Set
    End Property
    Public Property ZulStellen() As DataTable
        Get
            Return m_tblZulStelle
        End Get
        Set(ByVal Value As DataTable)
            m_tblZulStelle = Value
        End Set
    End Property

    Public ReadOnly Property Laender() As DataTable
        Get
            If m_tblLaender Is Nothing Then
                getLaender(AppID, SessionID)
            End If
            Return m_tblLaender
        End Get
    End Property


#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String,
                   ByVal strFileName As String, ByVal strCustomer As String, ByVal strKunnr As String, Optional ByVal strCreditControlArea As String = "ZDAD",
                   Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        Customer = strCustomer
        KUNNR = strKunnr
        CreditControlArea = strCreditControlArea
        m_intStatus = 0
        m_strMessage = ""
        m_strAuftragsnummer = ""
        m_strAuftragsstatus = ""
        m_strSucheZZREFERENZ1 = ""
        m_strSucheTIDNR = ""
        m_strSucheFahrgestellNr = ""
        m_hez = hez
        Show(AppID, SessionID)
    End Sub

    Public Sub ErsetzeAbrufgruende()
        Dim tempRow As DataRow
        Dim rows() As DataRow
        For Each tempRow In m_tblFahrzeuge.Rows
            rows = Abrufgruende.Select("SapWert='" & CStr(tempRow("AUGRU")) & "'")
            If rows.Length > 0 Then
                tempRow("AUGRU_Klartext") = CStr(rows(0)("WebBezeichnung"))
            End If
        Next
    End Sub

    Public Function isHEZ() As Boolean
        Return m_hez
    End Function

    Public Function checkInputHEZ(ByVal zulDatum As String, ByRef message As String) As Boolean 'HEZ
        Dim datum As Date
        Dim row As DataRow()

        'Datum?
        If Not IsDate(zulDatum) Then
            message = "Falsches Datumsformat."
            Return False
        End If
        'Hier SAP-Datumsfelder durchhühnern...
        datum = CType(zulDatum, Date)
        zulDatum = HelpProcedures.MakeDateSAP(datum).ToString

        row = m_datumRange.Select("Low = '" & zulDatum & "'")

        If row.Length = 0 Then  'Kein Datum gefunden. Also ugültig.
            message = "Ungültiges Zulassungsdatum (" & HelpProcedures.MakeDateStandard(zulDatum) & ")"
            Return False
        End If

        Return True
    End Function

    Public Overloads Sub GiveCars(ByVal strAppID As String, ByVal strSessionID As String)

        m_tblFahrzeuge = New DataTable()

        With m_tblFahrzeuge.Columns
            .Add("ZZFAHRG", System.Type.GetType("System.String"))
            .Add("MANDT", System.Type.GetType("System.String"))
            .Add("LIZNR", System.Type.GetType("System.String"))
            .Add("EQUNR", System.Type.GetType("System.String"))
            .Add("TIDNR", System.Type.GetType("System.String"))
            .Add("LICENSE_NUM", System.Type.GetType("System.String"))
            .Add("ZZREFERENZ1", System.Type.GetType("System.String"))
            .Add("ZZBEZAHLT", System.Type.GetType("System.Boolean"))
            .Add("ZZCOCKZ", System.Type.GetType("System.Boolean"))
            .Add("ZZLABEL", System.Type.GetType("System.String"))
            .Add("VBELN", System.Type.GetType("System.String"))
            .Add("COMMENT", System.Type.GetType("System.String"))
            .Add("TEXT50", System.Type.GetType("System.String"))
            .Add("TEXT200", System.Type.GetType("System.String"))
            .Add("KOPFTEXT", System.Type.GetType("System.String"))
            .Add("POSITIONSTEXT", System.Type.GetType("System.String"))
            .Add("ZZFINART", System.Type.GetType("System.String"))
            .Add("ZZLAUFZEIT", System.Type.GetType("System.String"))
            .Add("ANFNR", System.Type.GetType("System.String"))
            .Add("AUGRU", System.Type.GetType("System.String"))
            .Add("AUGRU_Klartext", System.Type.GetType("System.String"))
        End With

        Try
            ClearError()

            'weil bei bankbase führende nullen angehängt werden, verträgt das bapi aber nicht wenn ohne Händlernummer gesucht wird.
            Dim tmpCustomer As String
            If m_strCustomer = "0000000000" Then
                tmpCustomer = ""
            Else
                tmpCustomer = Right("0000000000" & m_strCustomer, 10)
            End If

            S.AP.Init("Z_M_Unangefordert_002")
            S.AP.SetImportParameter("I_AG", KUNNR)
            S.AP.SetImportParameter("I_HAENDLER", tmpCustomer)
            S.AP.SetImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr.Replace("*", ""))
            S.AP.SetImportParameter("I_LIZNR", m_strLIZNR.Replace("*", ""))
            S.AP.SetImportParameter("I_ZZREFERENZ1", m_strSucheZZREFERENZ1.Replace("*", ""))
            S.AP.SetImportParameter("I_TIDNR", SucheTIDNR.Replace("*", ""))
            S.AP.Execute()

            mE_SUBRC = S.AP.GetExportParameter("E_SUBRC")
            mE_MESSAGE = S.AP.GetExportParameter("E_MESSAGE")

            Dim tblFahrzeugeSap As DataTable = S.AP.GetExportTable("GT_WEB")
            Dim rowFahrzeugSAP As DataRow

            For Each rowFahrzeugSAP In tblFahrzeugeSap.Rows
                Dim rowFahrzeug As DataRow

                rowFahrzeug = m_tblFahrzeuge.NewRow
                rowFahrzeug("ZZFAHRG") = rowFahrzeugSAP("ZZFAHRG")
                rowFahrzeug("MANDT") = "0"
                rowFahrzeug("LIZNR") = rowFahrzeugSAP("Liznr")
                rowFahrzeug("EQUNR") = rowFahrzeugSAP("Equnr")
                rowFahrzeug("TIDNR") = rowFahrzeugSAP("Tidnr")
                rowFahrzeug("Zzreferenz1") = rowFahrzeugSAP("Zzreferenz1")
                rowFahrzeug("LICENSE_NUM") = rowFahrzeugSAP("License_Num")
                If UCase(rowFahrzeugSAP("Zzcockz").ToString) = "X" Then
                    rowFahrzeug("ZZCOCKZ") = True
                Else
                    rowFahrzeug("ZZCOCKZ") = False
                End If
                rowFahrzeug("VBELN") = ""
                rowFahrzeug("COMMENT") = ""
                rowFahrzeug("ZZFINART") = rowFahrzeugSAP("Zzfinart")
                rowFahrzeug("KOPFTEXT") = String.Empty
                rowFahrzeug("POSITIONSTEXT") = String.Empty

                rowFahrzeug("text50") = String.Empty
                rowFahrzeug("AUGRU") = String.Empty
                m_tblFahrzeuge.Rows.Add(rowFahrzeug)
                m_tblFahrzeuge.AcceptChanges()
            Next

            Dim col As DataColumn
            For Each col In m_tblFahrzeuge.Columns
                col.ReadOnly = False
            Next

            'Händlernummer Speichern falls bankanforderung und selektion über andere Parameter
            'wenn mehrere Händlernummern falsche selektion im SAP
            If m_tblFahrzeuge.Rows.Count > 0 Then
                Customer = tblFahrzeugeSap.Rows(0)("Kunnr").ToString
            End If

            If mE_SUBRC <> "0" Then
                RaiseError(mE_SUBRC, mE_MESSAGE)
            End If

            WriteLogEntry(True, "CHASSIS_NUM=" & m_strSucheFahrgestellNr & ", KONZS=" & KUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strSucheTIDNR & ", ZZREFERENZ1=" & m_strSucheZZREFERENZ1, m_tblFahrzeuge)
        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NO_DATA") Then
                RaiseError("-2501", "Es wurden keine Daten gefunden.")
            ElseIf errormessage.Contains("NO_HAENDLER") Then
                RaiseError("-2502", "Händler nicht vorhanden.")
            ElseIf errormessage.Contains("TEMP_VERSAND") Then
                RaiseError("-2503", "Brief bereits Temporär versendet!")
            ElseIf errormessage.Contains("HIST_CHECK") Then
                RaiseError("-2504", "Bitte in Historie prüfen.")
            Else
                RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End If
            
            WriteLogEntry(False, "CHASSIS_NUM=" & m_strSucheFahrgestellNr & ", KONZS=" & KUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strSucheTIDNR & ", ZZREFERENZ1=" & m_strSucheZZREFERENZ1 & " , " & Replace(m_strMessage, "<br>", " "), m_tblFahrzeuge)
        End Try
    End Sub

    Public Sub getZulStelle(ByVal plz As String, ByVal ort As String, ByRef statusVal As String)

        Try
            ClearError()

            S.AP.InitExecute("Z_Get_Zulst_By_Plz", "I_PLZ, I_ORT", plz, ort)

            Dim table As DataTable = S.AP.GetExportTable("T_ZULST")

            If (table.Rows.Count > 1) Then
                'Mehr als ein Eintrag gefunden! Darf nicht sein!
                statusVal = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            m_kbanr = table.Rows(0)("KBANR").ToString
            m_tblZulStelle = table.Copy

        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("ERR_INV_PLZ") Then
                RaiseError("-1118", "Ungültige Postleitzahl.")
            Else
                RaiseError("-9999", "Unbekannter Fehler.")
            End If

            statusVal = m_strMessage
            m_kbanr = ""
        End Try

    End Sub

    Private Sub getLaender(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: getLaender
        ' Autor: JJU
        ' Beschreibung: gibt alle Länder zurück
        ' Erstellt am: 06.03.2009
        ' ITA: 2918
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        
        Try
            ClearError()

            S.AP.InitExecute("Z_M_Land_Plz_001")

            m_tblLaender = S.AP.GetExportTable("GT_WEB")

            m_tblLaender.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
            m_tblLaender.Columns.Add("FullDesc", System.Type.GetType("System.String"))
            Dim rowTemp As DataRow
            For Each rowTemp In m_tblLaender.Rows
                If CInt(rowTemp("LNPLZ")) > 0 Then
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx")) & " (" & CStr(CInt(rowTemp("LNPLZ"))) & ")"
                Else
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx"))
                End If
                rowTemp("FullDesc") = CStr(rowTemp("Land1")) & " " & CStr(rowTemp("Beschreibung"))
            Next

        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("ERR_INV_PLZ") Then
                RaiseError("-1118", "Ungültige Postleitzahl.")
            Else
                RaiseError("-9999", "Unbekannter Fehler.")
            End If
        End Try

    End Sub

    Public Sub Anfordern(ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal hez As Boolean = False)
        Try
            ClearError()

            m_strAuftragsnummer = ""
            m_strAuftragsstatus = ""

            S.AP.Init("Z_M_Briefanforderung_002")

            Dim SAPTable As DataTable = S.AP.GetImportTable("IMP")

            If CheckCustomerData() Then
                Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("ZZFAHRG = '" & m_strZZFAHRG & "'")

                If neueAdresse Then
                    'wenn eine Eigene Adresse Angelegt wurde oder eine Zulassungsstelle ausgewählt wurde, muss Kunr_ZS blank sein, sonst legt SAP das bei dieser Händlernummer an JJU2008.02.29
                    m_strAdresse = String.Empty
                End If

                Dim tmpSapRow As DataRow = SAPTable.NewRow

                With tmpSapRow
                    .Item("Anfnr") = rowFahrzeug(0)("ANFNR").ToString
                    .Item("Augru") = rowFahrzeug(0)("AUGRU").ToString
                    .Item("City1") = m_strCity
                    .Item("Datum") = zuldatum
                    .Item("Equnr") = rowFahrzeug(0)("EQUNR").ToString
                    .Item("Ernam") = Left(m_strInternetUser, 12)
                    'autorisierungslevel der Gruppe schreiben, +1 weil in sap das feld nur von 1-4 geht
                    .Item("Kvgr3") = CStr(m_objUser.Groups.ItemByID(m_objUser.GroupID).Authorizationright + 1)
                    If hez Then
                        .Item("Hezkz") = "X"
                    Else
                        .Item("Hezkz") = ""
                    End If
                    .Item("Chassis_Num") = m_strZZFAHRG
                    .Item("House_Num1") = m_strHouseNum
                    .Item("Kdgrp") = ""
                    .Item("Kunnr") = Right("0000000000" & m_strCustomer, 10)
                    .Item("Konzs") = KUNNR
                    .Item("Kunnr_Ze") = ""
                    If hez Then
                        .Item("Kunnr_Zh") = m_strAdresseHalter
                    Else
                        .Item("Kunnr_Zh") = ""
                    End If
                    .Item("Kunnr_Zs") = m_strAdresse
                    .Item("License_Num") = rowFahrzeug(0)("LICENSE_NUM").ToString
                    .Item("Liznr") = rowFahrzeug(0)("LIZNR").ToString
                    .Item("Matnr") = m_strMaterialNummer
                    .Item("Name1") = m_strName1
                    .Item("Name2") = m_strName2
                    .Item("Name3") = m_strName3
                    .Item("Post_Code1") = m_strPostcode
                    .Item("Preis") = 0
                    .Item("Street") = m_strStreet
                    .Item("Text50") = rowFahrzeug(0)("TEXT50").ToString
                    .Item("Tidnr") = rowFahrzeug(0)("TIDNR").ToString
                    .Item("Txt_Kopf") = rowFahrzeug(0)("KOPFTEXT").ToString
                    .Item("Txt_Pos") = rowFahrzeug(0)("POSITIONSTEXT").ToString
                    .Item("Zulst") = m_kbanr
                    .Item("Zzdien1") = ""
                    .Item("Zzkkber") = m_strKreditkontrollBereich
                    .Item("Zzlabel") = rowFahrzeug(0)("ZZLABEL").ToString
                    .Item("Zzlaufzeit") = rowFahrzeug(0)("ZZLAUFZEIT").ToString
                    .Item("Country") = laenderKuerzel
                End With

                SAPTable.Rows.Add(tmpSapRow)
                SAPTable.AcceptChanges()

                S.AP.Execute()
            End If

            m_strAuftragsnummer = S.AP.GetExportParameter("E_Vbeln")

            Select Case UCase(S.AP.GetExportParameter("E_Cmgst"))
                Case ""
                    m_strAuftragsstatus = "Kreditprüfung nicht durchgeführt"
                    m_strAuftragsstatus2 = m_strAuftragsstatus
                Case "A"
                    m_strAuftragsstatus = "Vorgang OK"
                    m_strAuftragsstatus2 = m_strAuftragsstatus
                Case "B"
                    m_strAuftragsstatus = "Freigabe erforderlich"
                    m_strAuftragsstatus2 = "in Bearbeitung Bank"
                Case "C"
                    m_strAuftragsstatus = "Freigabe erforderlich"
                    m_strAuftragsstatus2 = "in Bearbeitung Bank"
                Case "D"
                    m_strAuftragsstatus = "Freigegeben"
                    m_strAuftragsstatus2 = "in Bearbeitung Bank "
                Case Else
                    m_strAuftragsstatus = "Unbekannt"
                    m_strAuftragsstatus2 = "Unbekannt"
            End Select

            If m_strAuftragsnummer.Length = 0 Then
                RaiseError("-2100", "Ihre Anforderung konnte im System nicht erstellt werden.")
            End If
        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("ZCREDITCONTROL_ENTRY_LOCKED") Then
                RaiseError("-1111", "System ausgelastet. Bitte clicken Sie erneut auf ""Absenden"".")
            ElseIf errormessage.Contains("NO_UPDATE_EQUI") Then
                RaiseError("-1112", "Fehler bei der Datenspeicherung (EQUI-UPDATE)")
            ElseIf errormessage.Contains("NO_AUFTRAG") Then
                RaiseError("-1113", "Kein Auftrag angelegt")
            ElseIf errormessage.Contains("NO_ZDADVERSAND") Then
                RaiseError("-1114", "Keine Einträge für die Versanddatei erstellt")
            ElseIf errormessage.Contains("NO_MODIFY_ILOA") Then
                RaiseError("-1115", "ILOA-MODIFY-Fehler")
            ElseIf errormessage.Contains("NO_BRIEFANFORDERUNG") Then
                RaiseError("-1116", "Brief bereits angefordert")
            ElseIf errormessage.Contains("NO_EQUZ") Then
                RaiseError("-1117", "Kein Brief vorhanden (EQUZ)")
            ElseIf errormessage.Contains("NO_ILOA") Then
                RaiseError("-1118", "Kein Brief vorhanden (ILOA)")
            ElseIf errormessage.Contains("EQUI_SPERRE") Then
                RaiseError("-1119", "Vorgang in Bearbeitung")
            Else
                RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End If
        End Try
    End Sub

    Public Function getHaendlernummerByFin(ByVal Fahrgestellnummer As String) As String
        '----------------------------------------------------------------------
        ' Methode: getHaendlerNummerByFIN
        ' Autor: JJU
        ' Beschreibung: Liefert über das BAPI Z_M_GET_HAENDLER Die Händlernummer von einer Fahrgestellnummer
        ' Erstellt am: 20080828
        ' ITA: 2124
        '----------------------------------------------------------------------
        m_strClassAndMethod = "fin_06.getHaendlernummerByFin"
        Dim sHaendlerNo As String = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim tblTmp As New DataTable

            ClearError()

            Try
                S.AP.InitExecute("Z_M_GET_HAENDLER", "AG, CHASSIS_NUM, VKORG",
                                 Right("0000000000" & m_objUser.KUNNR, 10), Fahrgestellnummer.ToUpper(), "1510")

                Dim tblDaten As DataTable = S.AP.GetExportTable("GT_DATEN")

                If tblDaten IsNot Nothing Then

                    tblTmp = tblDaten
                    HelpProcedures.killAllDBNullValuesInDataTable(tblTmp)

                    If Not tblTmp.Rows.Count = 1 Then
                        If tblTmp.Rows.Count = 0 Then
                            Throw New Exception("zur Fahrgstellnummer " & Fahrgestellnummer & " wurde kein Händler gefunden")
                        Else
                            Throw New Exception("zur Fahrgstellnummer " & Fahrgestellnummer & " wurden mehrere Händler gefunden")
                        End If
                    Else
                        sHaendlerNo = tblTmp.Rows(0)("EIKTO").ToString
                    End If

                End If
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, tblTmp)
            Catch ex As Exception
                Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

                If errormessage.Contains("NO_DATA") Then
                    RaiseError("-2222", "Keine Händlernummer zu dem Fahrzeug gefunden")
                Else
                    RaiseError("-9999", "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")")
                End If

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), tblTmp)
            Finally
                m_blnGestartet = False
            End Try
        End If

        Return sHaendlerNo
    End Function

    Public Function getNextWorkDay(ByVal fromDate As Date, ByVal strAppID As String, ByVal strSessionID As String) As Date
        Try
            S.AP.InitExecute("Z_S_Add_Date", "IN_DATUM, FACTORY_CALENDAR_ID, CORRECT_OPTION, ANZAHL",
                             fromDate.ToShortDateString, "01", "+", "1")

            Return CDate(S.AP.GetExportParameter("OUT_DATUM"))
        Catch ex As Exception
            RaiseError("-9999", "Unbekannter Fehler. " & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
        End Try

        Return Nothing
    End Function

#End Region

End Class

' ************************************************
' $History: fin_06.vb $
' 
' *****************  Version 17  *****************
' User: Fassbenders  Date: 5.03.10    Time: 12:57
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA: 2918
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 7.09.09    Time: 16:15
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 24.06.09   Time: 16:00
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 finalisierung
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 24.06.09   Time: 15:53
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_S_Add_Date
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 23.06.09   Time: 16:05
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITa 2918 Z_M_Briefanforderung_002
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 23.06.09   Time: 14:57
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_M_Unangefordert_002
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 22.06.09   Time: 17:12
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_M_LAND_PLZ_001
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 22.06.09   Time: 13:26
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' nachbesserung ita 2918
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 1.09.08    Time: 9:52
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2211 fertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 28.08.08   Time: 15:14
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2124 fertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 25.07.08   Time: 8:18
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2125 done
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.05.08   Time: 10:06
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' RTFS Händlerportal Bug-Fixing
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 27  *****************
' User: Jungj        Date: 11.03.08   Time: 14:48
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1765
' 
' *****************  Version 26  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 25  *****************
' User: Jungj        Date: 4.03.08    Time: 19:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' AKF ANpassungen
' 
' *****************  Version 24  *****************
' User: Jungj        Date: 4.03.08    Time: 18:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' AKF ANPASSUNGEN ITA 1733
' 
' *****************  Version 23  *****************
' User: Jungj        Date: 4.03.08    Time: 10:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1733, 1667, 1738 
' 
' *****************  Version 22  *****************
' User: Jungj        Date: 1.03.08    Time: 13:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Änderung der Händlernummer auf 10 stellen mit führenden 0 
' 
' *****************  Version 21  *****************
' User: Jungj        Date: 29.02.08   Time: 16:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1733
' 
' *****************  Version 20  *****************
' User: Jungj        Date: 28.02.08   Time: 11:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ita 1733
' 
' *****************  Version 19  *****************
' User: Jungj        Date: 27.02.08   Time: 15:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1733
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 26.02.08   Time: 16:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1733
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 26.02.08   Time: 8:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ita 1733
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 12.02.08   Time: 9:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 2.02.08    Time: 15:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 2.02.08    Time: 10:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 2.02.08    Time: 10:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 1.02.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 29.01.08   Time: 15:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Zulassungsdatumcheck über bapi z_s_add_Date realisiert
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 29.01.08   Time: 15:09
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Z_S_ADD_DATE hinzugefügt
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 17.01.08   Time: 12:38
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA: 1617
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.01.08    Time: 16:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.01.08    Time: 9:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 7.01.08    Time: 18:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Bugfix RTFS
' 
' *****************  Version 4  *****************
' User: Uha          Date: 19.12.07   Time: 14:10
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1510,1512,1511 Anforderung / Zulassung
' 
' *****************  Version 3  *****************
' User: Uha          Date: 18.12.07   Time: 17:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Anforderung (temp./endg.) fast fertig
' 
' *****************  Version 2  *****************
' User: Uha          Date: 17.12.07   Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Versand ZB II / Briefe - komplierfähige Zwischenversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.12.07   Time: 17:18
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Elemente für Temp./Endg. bzw. HEZ Anforderung hinzugefügt (Change42ff,
' fin_06, Change43ff und fin_08)
' 
' ************************************************