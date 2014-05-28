Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class FDD_Haendler
    REM § Lese-/Schreibfunktion, Kunde: FFD, 
    REM § GiveCars - BAPI: Z_M_Unangefordert,
    REM § Anfordern - BAPI: Z_M_Briefanforderung.

    Inherits Base.Business.BankBaseCredit

#Region " Declarations"
    Private m_strSucheVertragsNr As String
    Private m_strSucheOrderNr As String
    Private m_strSucheFahrgestellNr As String
    Private m_tblFahrzeuge As DataTable
    Private m_tblZulStelle As DataTable
    Private m_strZZFAHRG As String
    Private m_strAdresse As String
    Private m_strAuftragsnummer As String
    Private m_strAuftragsstatus As String
    Private m_strKreditkontrollBereich As String
    Private m_strMaterialNummer As String 'Versandart
    Private m_strAdresseHalter As String    'HEZ
    Private m_strAdresseEmpf As String    'HEZ
    Private m_preis As Decimal 'HEZ
    Private m_datumRange As DataTable       'HEZ:Hier Datumswerte auslesen...
    Private m_kbanr As String 'Zulassungsdienst-Nummer
    Private m_ZzLaufzeit As String
    Private m_strName1 As String
    Private m_strName2 As String
    Private m_strName3 As String
    Private m_strCity As String
    Private m_strPostcode As String
    Private m_strStreet As String
    Private m_strHouseNum As String
    Private m_bNewAdress As Boolean
#End Region

#Region " Properties"
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
    Public Property kbanr() As String
        Get
            Return m_kbanr
        End Get
        Set(ByVal Value As String)
            m_kbanr = Value
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
            If Value.Length > 0 Then
                m_strAdresse = Right("00000000000" & Value, 10)
            Else
                m_strAdresse = ""
            End If
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

    Public Property SucheVertragsNr() As String
        Get
            Return m_strSucheVertragsNr
        End Get
        Set(ByVal Value As String)
            m_strSucheVertragsNr = Value
        End Set
    End Property

    Public Property SucheOrderNr() As String
        Get
            Return m_strSucheOrderNr
        End Get
        Set(ByVal Value As String)
            m_strSucheOrderNr = Value
        End Set
    End Property

    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellNr
        End Get
        Set(ByVal Value As String)
            m_strSucheFahrgestellNr = Value
        End Set
    End Property

    Public Property Laufzeit() As String
        Get
            Return m_ZzLaufzeit
        End Get
        Set(ByVal Value As String)
            m_ZzLaufzeit = Value
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

    Public Property NewAdress() As Boolean
        Get
            Return m_bNewAdress
        End Get
        Set(ByVal Value As Boolean)
            m_bNewAdress = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByVal page As System.Web.UI.Page, ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, ByVal strCustomer As String, Optional ByVal strCreditControlArea As String = "ZDAD", Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        Customer = strCustomer
        CreditControlArea = strCreditControlArea
        m_intStatus = 0
        m_strMessage = ""
        m_strAuftragsnummer = ""
        m_strAuftragsstatus = ""
        m_strSucheOrderNr = ""
        m_strSucheVertragsNr = ""
        m_strSucheFahrgestellNr = ""
        m_hez = hez
        m_ZzLaufzeit = ""
        Show()
    End Sub

    Public Function isHEZ() As Boolean
        Return m_hez
    End Function

    Public Function checkInputHEZ(ByVal zulDatum As String, ByRef message As String) As Boolean
        Dim datum As Date
        Dim row As DataRow()

        If Not IsDate(zulDatum) Then
            message = "Falsches Datumsformat."
            Return False
        End If

        datum = CDate(zulDatum)

        Dim SArr() As String
        SArr = Split(zulDatum, ".")
        Dim strSelect As String = "Low = #" & CInt(SArr(1)) & "/" & CInt(SArr(0)) & "/" & CInt(SArr(2)) & "#"
        row = m_datumRange.Select(strSelect)

        If row.Length = 0 Then
            message = "Ungültiges Zulassungsdatum (" & zulDatum & ")"
            Return False
        End If

        Return True
    End Function

    Public Sub GiveCars(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FDD_Haendler.GiveCars"

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim tblTemp As DataTable = Nothing
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
                    .Add("ZZBOOLAUFZEIT", System.Type.GetType("System.Boolean"))
                    .Add("TEXT300", System.Type.GetType("System.String"))
                End With


                If CheckCustomerData() Then
                    If (m_hez = False) Then

                        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unangefordert", m_objApp, m_objUser, page)

                        'myProxy.setImportParameter("I_KUNNR", Right(m_strCustomer, 5))
                        'myProxy.setImportParameter("I_KNRZE", m_strFiliale)
                        'myProxy.setImportParameter("I_KONZS", m_strKUNNR)
                        'myProxy.setImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr)
                        'myProxy.setImportParameter("I_LIZNR", m_strSucheVertragsNr)
                        'myProxy.setImportParameter("I_ZZREFERENZ1", m_strSucheOrderNr)
                        'myProxy.setImportParameter("I_VKORG", "1510")
                        'myProxy.setImportParameter("I_ZB2", "")

                        'myProxy.callBapi()

                        S.AP.InitExecute("Z_M_Unangefordert", "I_KUNNR,I_KNRZE,I_KONZS,I_CHASSIS_NUM,I_LIZNR,I_ZZREFERENZ1,I_VKORG,I_ZB2",
                                         Right(m_strCustomer, 5), m_strFiliale, m_strKUNNR, m_strSucheFahrgestellNr, m_strSucheVertragsNr, m_strSucheOrderNr, "1510", "")

                        tblTemp = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
                    Else
                        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unangefordert_Hez", m_objApp, m_objUser, page)

                        'myProxy.setImportParameter("I_KUNNR", Right(m_strCustomer, 5))
                        'myProxy.setImportParameter("I_KNRZE", m_strFiliale)
                        'myProxy.setImportParameter("I_KONZS", m_strKUNNR)
                        'myProxy.setImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr)
                        'myProxy.setImportParameter("I_LIZNR", m_strSucheVertragsNr)
                        'myProxy.setImportParameter("I_ZZREFERENZ1", m_strSucheOrderNr)
                        'myProxy.setImportParameter("I_VKORG", "1510")

                        'myProxy.callBapi()

                        S.AP.InitExecute("Z_M_Unangefordert_Hez", "I_KUNNR,I_KNRZE,I_KONZS,I_CHASSIS_NUM,I_LIZNR,I_ZZREFERENZ1,I_VKORG",
                                        Right(m_strCustomer, 5), m_strFiliale, m_strKUNNR, m_strSucheFahrgestellNr, m_strSucheVertragsNr, m_strSucheOrderNr, "1510")

                        m_datumRange = S.AP.GetExportTable("GT_ZUL") 'myProxy.getExportTable("GT_ZUL")
                        tblTemp = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
                    End If


                    Dim tmpRow As DataRow
                    For Each tmpRow In tblTemp.Rows
                        Dim rowFahrzeug As DataRow
                        rowFahrzeug = m_tblFahrzeuge.NewRow
                        rowFahrzeug("ZZFAHRG") = tmpRow("Chassis_Num").ToString
                        rowFahrzeug("MANDT") = "0"
                        rowFahrzeug("EQUNR") = tmpRow("Equnr").ToString
                        rowFahrzeug("LIZNR") = tmpRow("Liznr").ToString
                        rowFahrzeug("TIDNR") = tmpRow("Tidnr").ToString
                        rowFahrzeug("LICENSE_NUM") = tmpRow("License_Num").ToString
                        rowFahrzeug("ZZREFERENZ1") = tmpRow("Zzreferenz1").ToString
                        rowFahrzeug("ZZLABEL") = tmpRow("Zzlabel").ToString
                        If UCase(tmpRow("Zzcockz").ToString) = "X" Then
                            rowFahrzeug("ZZCOCKZ") = True
                        Else
                            rowFahrzeug("ZZCOCKZ") = False
                        End If
                        If UCase(tmpRow("Zzbezahlt").ToString) = "X" Then
                            rowFahrzeug("ZZBEZAHLT") = True
                        Else
                            rowFahrzeug("ZZBEZAHLT") = False
                        End If
                        rowFahrzeug("VBELN") = ""
                        rowFahrzeug("COMMENT") = ""
                        rowFahrzeug("ZZFINART") = tmpRow("Zzfinart").ToString
                        rowFahrzeug("KOPFTEXT") = String.Empty
                        rowFahrzeug("POSITIONSTEXT") = String.Empty

                        'If Not tmpRow("Zzlaufzeit").ToString = "0" Or _
                        '    Not tmpRow("Zzabmdat").ToString = "" Then
                        '    rowFahrzeug("ZZBOOLAUFZEIT") = True
                        'Else
                        '    rowFahrzeug("ZZBOOLAUFZEIT") = False
                        'End If

                        If tmpRow("Zzlaufzeit").ToString.TrimStart("0"c) = "" AndAlso tmpRow("Zzabmdat").ToString = "" Then
                            rowFahrzeug("ZZBOOLAUFZEIT") = False
                        Else
                            rowFahrzeug("ZZBOOLAUFZEIT") = True
                        End If

                        m_tblFahrzeuge.Rows.Add(rowFahrzeug)
                    Next

                    Dim col As DataColumn
                    For Each col In m_tblFahrzeuge.Columns
                        col.ReadOnly = False
                    Next

                End If
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -2501
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -2502
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message

                        Dim strLog As String = ""

                        If m_strSucheFahrgestellNr <> String.Empty Then
                            strLog += "CHASSIS_NUM=" & m_strSucheFahrgestellNr & ","
                        End If
                        If m_strKUNNR <> String.Empty Then
                            strLog += "KONZS=" & m_strKUNNR.TrimStart("0"c) & ","
                        End If
                        If m_strFiliale <> String.Empty Then
                            strLog += "KNRZE=" & m_strFiliale & ","
                        End If
                        If m_strCustomer <> String.Empty Then
                            strLog += "KUNNR=" & Right(m_strCustomer, 5) & ","
                        End If
                        If m_strSucheVertragsNr <> String.Empty Then
                            strLog += "LIZNR=" & m_strSucheVertragsNr & ","
                        End If
                        If m_strSucheOrderNr <> String.Empty Then
                            strLog += "ZZREFERENZ1=" & m_strSucheOrderNr
                        End If

                        WriteLogEntry(False, strLog & Replace(m_strMessage, "<br>", " "))
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub getZulStelle(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal plz As String, ByVal ort As String, ByRef status As String)

        Dim table As New DataTable()
        Dim tblOrte As New DataTable
        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_PLZ", plz)
            'myProxy.setImportParameter("I_ORT", ort)


            'myProxy.callBapi()

            S.AP.InitExecute("Z_Get_Zulst_By_Plz", "I_PLZ,I_ORT", plz, ort)

            table = S.AP.GetExportTable("T_ZULST") 'myProxy.getExportTable("T_ZULST")
            tblOrte = S.AP.GetExportTable("T_ORTE") 'myProxy.getExportTable("T_ORTE")

            If (table.Rows.Count > 1) Then
                'Mehr als ein Eintrag gefunden! Darf nicht sein!
                status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            m_kbanr = table.Rows(0)("KBANR").ToString
            m_tblZulStelle = table.Copy
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select

            status = m_strMessage
            m_kbanr = ""
        End Try
    End Sub

    Public Sub Anfordern(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, Optional ByVal hez As Boolean = False)
        If Not m_blnGestartet Then
            m_blnGestartet = True


            Try
                m_intStatus = 0
                m_strMessage = ""
                m_strAuftragsnummer = ""
                m_strAuftragsstatus = ""

                Dim strAuftragsnummer As String = ""
                Dim strAuftragsstatus As String = ""

                If CheckCustomerData() Then
                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Briefanforderung", m_objApp, m_objUser, page)


                    Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("ZZFAHRG = '" & m_strZZFAHRG & "'")

                    'myProxy.setImportParameter("I_KUNNR", m_strCustomer)
                    'myProxy.setImportParameter("I_KONZS", m_strKUNNR)
                    'myProxy.setImportParameter("I_KUNNR_ZS", m_strAdresse)
                    'myProxy.setImportParameter("I_EQUNR", rowFahrzeug(0)("EQUNR").ToString)
                    'myProxy.setImportParameter("I_ERNAM", Left(m_strInternetUser, 12))
                    'myProxy.setImportParameter("I_CHASSIS_NUM", rowFahrzeug(0)("ZZFAHRG").ToString)
                    'myProxy.setImportParameter("I_LICENSE_NUM", rowFahrzeug(0)("LICENSE_NUM").ToString)
                    'myProxy.setImportParameter("I_TIDNR", rowFahrzeug(0)("TIDNR").ToString)
                    'myProxy.setImportParameter("I_LIZNR", rowFahrzeug(0)("LIZNR").ToString)
                    'myProxy.setImportParameter("I_ZZLABEL", rowFahrzeug(0)("ZZLABEL").ToString)

                    'If (hez = False) Then
                    '    'myProxy.setImportParameter("I_ZZKKBER", m_strKreditkontrollBereich)
                    'Else
                    '    'myProxy.setImportParameter("I_ZZKKBER", "0005")
                    'End If

                    'myProxy.setImportParameter("I_MATNR", m_strMaterialNummer)
                    'myProxy.setImportParameter("I_TEXT50", rowFahrzeug(0)("TEXT50").ToString)
                    'myProxy.setImportParameter("I_HEZKZ", "")
                    'myProxy.setImportParameter("I_PREIS", "")
                    'myProxy.setImportParameter("I_KUNNR_ZH", "")
                    'myProxy.setImportParameter("I_KUNNR_ZE", "")
                    'myProxy.setImportParameter("I_ZULST", m_kbanr)
                    'myProxy.setImportParameter("I_KVGR3", "")
                    'myProxy.setImportParameter("I_TXT_KOPF", rowFahrzeug(0)("KOPFTEXT").ToString)
                    'myProxy.setImportParameter("I_TXT_POS", rowFahrzeug(0)("POSITIONSTEXT").ToString)
                    'myProxy.setImportParameter("I_KDGRP", "")
                    'myProxy.setImportParameter("I_ZZDIEN1", "")
                    'myProxy.setImportParameter("I_AUGRU", "")
                    'myProxy.setImportParameter("I_ZZLAUFZEIT", rowFahrzeug(0)("ZZLAUFZEIT").ToString)
                    'myProxy.setImportParameter("I_ANFNR", rowFahrzeug(0)("TEXT300").ToString)

                    S.AP.Init("Z_M_Briefanforderung",
                        "I_KUNNR,I_KONZS,I_KUNNR_ZS,I_EQUNR,I_ERNAM,I_CHASSIS_NUM,I_LICENSE_NUM,I_TIDNR,I_LIZNR,I_ZZLABEL," & _
                            "I_MATNR,I_TEXT50,I_PREIS,I_ZULST,I_ZZDIEN1,I_TXT_KOPF,I_TXT_POS,I_KDGRP,I_AUGRU,I_ZZLAUFZEIT,I_ANFNR",
                        m_strCustomer,
                        m_strKUNNR,
                        m_strAdresse,
                        rowFahrzeug(0)("EQUNR").ToString,
                        Left(m_strInternetUser, 12),
                        rowFahrzeug(0)("ZZFAHRG").ToString,
                        rowFahrzeug(0)("LICENSE_NUM").ToString,
                        rowFahrzeug(0)("TIDNR").ToString,
                        rowFahrzeug(0)("LIZNR").ToString,
                        rowFahrzeug(0)("ZZLABEL").ToString,
                            m_strMaterialNummer,
                            rowFahrzeug(0)("TEXT50").ToString,
                            0,
                            m_kbanr,
                            "",
                            rowFahrzeug(0)("KOPFTEXT").ToString,
                            rowFahrzeug(0)("POSITIONSTEXT").ToString,
                            "",
                            "",
                            rowFahrzeug(0)("ZZLAUFZEIT").ToString,
                            rowFahrzeug(0)("TEXT300").ToString
                    )

                    If hez Then
                        If IsDate(zuldatum) Then
                            S.AP.SetImportParameter("I_DATUM", zuldatum)
                        End If
                        S.AP.SetImportParameter("I_KUNNR_ZH", m_strAdresseHalter)
                        S.AP.SetImportParameter("I_KUNNR_ZE", m_strAdresseEmpf)
                        ' m_strKreditkontrollBereich enthält hier die Zul.art (->KVGR3), da ZZKKBER bei HEZ immer 0005
                        S.AP.SetImportParameter("I_ZZKKBER", "0005")
                        S.AP.SetImportParameter("I_KVGR3", Right("0" & m_strKreditkontrollBereich, 1)) '1-stelligen Wert übergeben
                        S.AP.SetImportParameter("I_HEZKZ", "X")
                    Else
                        S.AP.SetImportParameter("I_KUNNR_ZH", "")
                        S.AP.SetImportParameter("I_KUNNR_ZE", "")
                        S.AP.SetImportParameter("I_ZZKKBER", m_strKreditkontrollBereich)
                        S.AP.SetImportParameter("I_KVGR3", "")
                        S.AP.SetImportParameter("I_HEZKZ", "")
                    End If

                    'myProxy.callBapi()
                    S.AP.Execute()

                    strAuftragsnummer = S.AP.GetExportParameter("E_VBELN") 'myProxy.getExportParameter("E_VBELN")
                    strAuftragsstatus = S.AP.GetExportParameter("E_CMGST") 'myProxy.getExportParameter("E_CMGST")
                    m_strAuftragsnummer = strAuftragsnummer.TrimStart("0"c)

                    Select Case UCase(strAuftragsstatus)
                        Case ""
                            m_strAuftragsstatus = "Kreditprüfung nicht durchgeführt"
                        Case "A"
                            m_strAuftragsstatus = "Vorgang OK"
                        Case "B"
                            m_strAuftragsstatus = "Vorgang gesperrt angelegt"
                        Case "C"
                            m_strAuftragsstatus = "Vorgang gesperrt angelegt"
                        Case "D"
                            m_strAuftragsstatus = "Freigegeben"
                        Case Else
                            m_strAuftragsstatus = "Unbekannt"
                    End Select

                    If m_strAuftragsnummer.Length = 0 Then
                        m_intStatus = -2100
                        m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    End If
                End If
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        m_strMessage = "System ausgelastet. Bitte klicken Sie erneut auf ""Absenden""."
                        m_intStatus = -1111
                    Case "NO_UPDATE_EQUI"
                        m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                        m_intStatus = -1112
                    Case "NO_AUFTRAG"
                        m_strMessage = "Kein Auftrag angelegt"
                        m_intStatus = -1113
                    Case "NO_ZDADVERSAND"
                        m_strMessage = "Keine Einträge für die Versanddatei erstellt"
                        m_intStatus = -1114
                    Case "NO_MODIFY_ILOA"
                        m_strMessage = "ILOA-MODIFY-Fehler"
                        m_intStatus = -1115
                    Case "NO_BRIEFANFORDERUNG"
                        m_strMessage = "Brief bereits angefordert"
                        m_intStatus = -1116
                    Case "NO_EQUZ"
                        m_strMessage = "Kein Brief vorhanden (EQUZ)"
                        m_intStatus = -1117
                    Case "NO_ILOA"
                        m_strMessage = "Kein Brief vorhanden (ILOA)"
                        m_intStatus = -1118
                    Case Else
                        m_strMessage = ex.Message
                        m_intStatus = -9999

                        ' Fehler Logging
                        WriteLogEntry(False, "Beim Anfordern eines Fahrzeugs (CHASSIS_NUM=" & m_strZZFAHRG & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & _
                                      ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ") ist folgender Fehler aufgetreten: " & Replace(m_strMessage, "<br>", " "))

                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Anfordern_Bank(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, Optional ByVal hez As Boolean = False)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""
                m_strAuftragsnummer = ""
                m_strAuftragsstatus = ""

                Dim strAuftragsnummer As String = ""
                Dim strAuftragsstatus As String = ""
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Briefanforderung_003", m_objApp, m_objUser, page)

                S.AP.Init("Z_M_Briefanforderung_003")

                Dim SAPTable As DataTable = S.AP.GetImportTable("IMP") 'myProxy.getImportTable("IMP")
                If CheckCustomerData() Then
                    Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("ZZFAHRG = '" & m_strZZFAHRG & "'")

                    If m_bNewAdress = True Then
                        m_strAdresse = String.Empty
                    End If

                    Dim Saprow As DataRow
                    With SAPTable
                        Saprow = .NewRow
                        Saprow("Anfnr") = rowFahrzeug(0)("TEXT300").ToString
                        Saprow("Augru") = "VSO"
                        Saprow("City1") = m_strCity
                        If IsDate(zuldatum) Then
                            Saprow("Datum") = zuldatum
                        End If

                        Saprow("Equnr") = rowFahrzeug(0)("EQUNR").ToString
                        Saprow("Ernam") = m_strInternetUser
                        If hez Then
                            Saprow("Hezkz") = "X"
                        Else
                            Saprow("Hezkz") = ""
                        End If
                        Saprow("Chassis_Num") = m_strZZFAHRG
                        Saprow("House_Num1") = m_strHouseNum
                        Saprow("Kdgrp") = ""
                        Saprow("Kunnr") = Right("0000000000" & m_strCustomer, 10)
                        Saprow("Konzs") = Right("0000000000" & m_strKUNNR, 10)
                        Saprow("Kunnr_Ze") = ""
                        If hez Then
                            Saprow("Kunnr_Zh") = m_strAdresseHalter
                        Else
                            Saprow("Kunnr_Zh") = ""
                        End If
                        Saprow("Kunnr_Zs") = m_strAdresse
                        Saprow("Kvgr3") = ""
                        Saprow("License_Num") = rowFahrzeug(0)("LICENSE_NUM").ToString
                        Saprow("Liznr") = rowFahrzeug(0)("LIZNR").ToString
                        Saprow("Matnr") = m_strMaterialNummer
                        Saprow("Name1") = m_strName1
                        Saprow("Name2") = m_strName2
                        Saprow("Name3") = m_strName3
                        Saprow("Post_Code1") = m_strPostcode
                        Saprow("Preis") = 0
                        Saprow("Street") = m_strStreet
                        Saprow("Text50") = rowFahrzeug(0)("TEXT50").ToString
                        Saprow("Tidnr") = rowFahrzeug(0)("TIDNR").ToString
                        Saprow("Txt_Kopf") = rowFahrzeug(0)("KOPFTEXT").ToString
                        Saprow("Txt_Pos") = rowFahrzeug(0)("POSITIONSTEXT").ToString
                        Saprow("Zulst") = m_kbanr
                        Saprow("Zzdien1") = ""
                        Saprow("Datum") = Today.ToShortDateString
                        Saprow("Zzkkber") = m_strKreditkontrollBereich
                        Saprow("Zzlabel") = rowFahrzeug(0)("ZZLABEL").ToString
                        Saprow("Zzlaufzeit") = Right("000" & rowFahrzeug(0)("ZZLAUFZEIT").ToString, 3)

                        .Rows.Add(Saprow)
                    End With


                Else
                End If
                'myProxy.callBapi()
                S.AP.Execute()

                strAuftragsnummer = S.AP.GetExportParameter("E_VBELN") 'myProxy.getExportParameter("E_VBELN")
                strAuftragsstatus = S.AP.GetExportParameter("E_CMGST") 'myProxy.getExportParameter("E_CMGST")
                m_strAuftragsnummer = strAuftragsnummer.TrimStart("0"c)

                Select Case UCase(strAuftragsstatus)
                    Case ""
                        m_strAuftragsstatus = "Kreditprüfung nicht durchgeführt"
                    Case "A"
                        m_strAuftragsstatus = "Vorgang OK"
                    Case "B"
                        m_strAuftragsstatus = "Freigabe erforderlich"
                    Case "C"
                        m_strAuftragsstatus = "Freigabe erforderlich"
                    Case "D"
                        m_strAuftragsstatus = "Freigegeben"
                    Case Else
                        m_strAuftragsstatus = "Unbekannt"
                End Select

                If m_strAuftragsnummer.Length = 0 Then
                    m_intStatus = -2100
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                End If
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        m_strMessage = "System ausgelastet. Bitte clicken Sie erneut auf ""Absenden""."
                        m_intStatus = -1111
                    Case "NO_UPDATE_EQUI"
                        m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                        m_intStatus = -1112
                    Case "NO_AUFTRAG"
                        m_strMessage = "Kein Auftrag angelegt"
                        m_intStatus = -1113
                    Case "NO_ZDADVERSAND"
                        m_strMessage = "Keine Einträge für die Versanddatei erstellt"
                        m_intStatus = -1114
                    Case "NO_MODIFY_ILOA"
                        m_strMessage = "ILOA-MODIFY-Fehler"
                        m_intStatus = -1115
                    Case "NO_BRIEFANFORDERUNG"
                        m_strMessage = "Brief bereits angefordert"
                        m_intStatus = -1116
                    Case "NO_EQUZ"
                        m_strMessage = "Kein Brief vorhanden (EQUZ)"
                        m_intStatus = -1117
                    Case "NO_ILOA"
                        m_strMessage = "Kein Brief vorhanden (ILOA)"
                        m_intStatus = -1118
                    Case "EQUI_SPERRE"
                        m_strMessage = "Vorgang in Bearbeitung"
                        m_intStatus = -1119
                    Case Else
                        m_strMessage = ex.Message
                        m_intStatus = -9999

                        ' Fehler Logging
                        WriteLogEntry(False, "Beim Anfordern eines Fahrzeugs (CHASSIS_NUM=" & m_strZZFAHRG & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & _
                                      ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ") ist folgender Fehler aufgetreten: " & Replace(m_strMessage, "<br>", " "))
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
   
#End Region

End Class

' ************************************************
' $History: FDD_Haendler.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Lib
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 16.03.10   Time: 10:16
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 17.03.08   Time: 9:49
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA: 1737
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 11.03.08   Time: 14:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Bugfix
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 27.02.08   Time: 10:33
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA: 1737
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 25.02.08   Time: 15:18
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA: 1773
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 18.02.08   Time: 13:22
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Ita:1690
' 
' *****************  Version 12  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 8.06.07    Time: 11:26
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 10  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' ************************************************
