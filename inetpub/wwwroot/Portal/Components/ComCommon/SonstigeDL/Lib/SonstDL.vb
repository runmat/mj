Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class SonstDL
    Inherits BankBase

#Region " Declarations"

    Private m_blnLeaseplanZulassung As Boolean

    Private m_strSucheFahrgestellNr As String
    Private m_strSucheKennzeichen As String
    Private m_strSucheLeasingvertragsNr As String
    Private m_strSucheNummerZB2 As String

    Private m_strBemerkung As String
    Private m_datDurchfuehrungsDatum As Date
    Private m_strEmpfaengerName1 As String
    Private m_strEmpfaengerName2 As String
    Private m_strEmpfaengerOrt As String
    Private m_strEmpfaengerPLZ As String
    Private m_strEmpfaengerStrasse As String
    Private m_strEmpfaengerHausnr As String
    Private m_strHalterName1 As String
    Private m_strHalterName2 As String
    Private m_strHalterOrt As String
    Private m_strHalterPLZ As String
    Private m_strHalterStrasse As String
    Private m_strHalterHausnr As String
    Private m_strKreis As String
    Private m_strReserviertAuf As String
    Private m_strStandortName1 As String
    Private m_strStandortName2 As String
    Private m_strStandortOrt As String
    Private m_strStandortPLZ As String
    Private m_strStandortStrasse As String
    Private m_strStandortHausnr As String
    Private m_strVersicherungstraeger As String
    Private m_strWunschkennzeichen As String
    Private m_strEvbNummer As String

    Private m_strExpress As String
    Private m_blnExpress As Boolean

    Private m_strBeauftragungKlartext As String

    Private m_tableGrund As DataTable
    Private m_auftragsgrund As String
    Private m_equ As String
    Private m_kennz As String
    Private m_tidnr As String
    Private m_liznr As String
    Private strAuftragsstatus As String
    Private strAuftragsnummer As String
    Private dataArray As ArrayList
    Private rowToChange As DataRow
    Private m_IsCustomer As Boolean

#End Region

#Region " Properties"
    Public Property BeauftragungKlartext() As String
        Get
            Return m_strBeauftragungKlartext
        End Get
        Set(ByVal Value As String)
            m_strBeauftragungKlartext = Value
        End Set
    End Property


    Public Property evbNummer() As String
        Get
            Return m_strEvbNummer
        End Get
        Set(ByVal Value As String)
            m_strEvbNummer = Value
        End Set
    End Property

    Public Property Express() As Boolean
        Get
            Return m_blnExpress
        End Get
        Set(ByVal Value As Boolean)
            m_blnExpress = Value
            If m_blnExpress Then
                m_strExpress = "X"
            Else
                m_strExpress = " "
            End If
        End Set
    End Property
    Public Property Bemerkung() As String
        Get
            Return m_strBemerkung
        End Get
        Set(ByVal Value As String)
            m_strBemerkung = Value
        End Set
    End Property

    Public Property DurchfuehrungsDatum() As Date
        Get
            Return m_datDurchfuehrungsDatum
        End Get
        Set(ByVal Value As Date)
            m_datDurchfuehrungsDatum = Value
        End Set
    End Property

    Public Property EmpfaengerName1() As String
        Get
            Return m_strEmpfaengerName1
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerName1 = Value
        End Set
    End Property

    Public Property EmpfaengerName2() As String
        Get
            Return m_strEmpfaengerName2
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerName2 = Value
        End Set
    End Property

    Public Property EmpfaengerOrt() As String
        Get
            Return m_strEmpfaengerOrt
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerOrt = Value
        End Set
    End Property

    Public Property EmpfaengerPLZ() As String
        Get
            Return m_strEmpfaengerPLZ
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerPLZ = Value
        End Set
    End Property

    Public Property EmpfaengerStrasse() As String
        Get
            Return m_strEmpfaengerStrasse
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerStrasse = Value
        End Set
    End Property

    Public Property EmpfaengerHausnr() As String
        Get
            Return m_strEmpfaengerHausnr
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerHausnr = Value
        End Set
    End Property


    Public Property HalterName1() As String
        Get
            Return m_strHalterName1
        End Get
        Set(ByVal Value As String)
            m_strHalterName1 = Value
        End Set
    End Property

    Public Property HalterName2() As String
        Get
            Return m_strHalterName2
        End Get
        Set(ByVal Value As String)
            m_strHalterName2 = Value
        End Set
    End Property

    Public Property HalterOrt() As String
        Get
            Return m_strHalterOrt
        End Get
        Set(ByVal Value As String)
            m_strHalterOrt = Value
        End Set
    End Property

    Public Property HalterPLZ() As String
        Get
            Return m_strHalterPLZ
        End Get
        Set(ByVal Value As String)
            m_strHalterPLZ = Value
        End Set
    End Property

    Public Property HalterStrasse() As String
        Get
            Return m_strHalterStrasse
        End Get
        Set(ByVal Value As String)
            m_strHalterStrasse = Value
        End Set
    End Property

    Public Property HalterHausnr() As String
        Get
            Return m_strHalterHausnr
        End Get
        Set(ByVal Value As String)
            m_strHalterHausnr = Value
        End Set
    End Property

    Public Property Kreis() As String
        Get
            Return m_strKreis
        End Get
        Set(ByVal Value As String)
            m_strKreis = Value
        End Set
    End Property

    Public Property ReserviertAuf() As String
        Get
            Return m_strReserviertAuf
        End Get
        Set(ByVal Value As String)
            Value = m_strReserviertAuf
        End Set
    End Property

    Public Property StandortName1() As String
        Get
            Return m_strStandortName1
        End Get
        Set(ByVal Value As String)
            m_strStandortName1 = Value
        End Set
    End Property

    Public Property StandortName2() As String
        Get
            Return m_strStandortName2
        End Get
        Set(ByVal Value As String)
            m_strStandortName2 = Value
        End Set
    End Property

    Public Property StandortOrt() As String
        Get
            Return m_strStandortOrt
        End Get
        Set(ByVal Value As String)
            m_strStandortOrt = Value
        End Set
    End Property

    Public Property StandortPLZ() As String
        Get
            Return m_strStandortPLZ
        End Get
        Set(ByVal Value As String)
            m_strStandortPLZ = Value
        End Set
    End Property

    Public Property StandortStrasse() As String
        Get
            Return m_strStandortStrasse
        End Get
        Set(ByVal Value As String)
            m_strStandortStrasse = Value
        End Set
    End Property

    Public Property StandortHausnr() As String
        Get
            Return m_strStandortHausnr
        End Get
        Set(ByVal Value As String)
            m_strStandortHausnr = Value
        End Set
    End Property

    Public Property Versicherungstraeger() As String
        Get
            Return m_strVersicherungstraeger
        End Get
        Set(ByVal Value As String)
            m_strVersicherungstraeger = Value
        End Set
    End Property

    Public Property Wunschkennzeichen() As String
        Get
            Return m_strWunschkennzeichen
        End Get
        Set(ByVal Value As String)
            m_strWunschkennzeichen = Value
        End Set
    End Property

    Public Property IsBooCustomerGroup() As Boolean
        Get
            Return m_IsCustomer
        End Get
        Set(ByVal Value As Boolean)
            m_IsCustomer = Value
        End Set
    End Property

    Public Property rowChange() As DataRow
        Get
            Return rowToChange
        End Get
        Set(ByVal Value As DataRow)
            rowToChange = Value
        End Set
    End Property

    Public Property objData() As ArrayList
        Get
            Return dataArray
        End Get
        Set(ByVal Value As ArrayList)
            dataArray = Value
        End Set
    End Property

    Public Property Auftragsstatus() As String
        Get
            Return strAuftragsstatus
        End Get
        Set(ByVal Value As String)
            strAuftragsstatus = Value
        End Set
    End Property

    Public Property Auftragsnummer() As String
        Get
            Return strAuftragsnummer
        End Get
        Set(ByVal Value As String)
            strAuftragsnummer = Value
        End Set
    End Property

    Public Property LizenzNr() As String
        Get
            Return m_liznr
        End Get
        Set(ByVal Value As String)
            m_liznr = Value
        End Set
    End Property

    Public Property TIDNr() As String
        Get
            Return m_tidnr
        End Get
        Set(ByVal Value As String)
            m_tidnr = Value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return m_kennz
        End Get
        Set(ByVal Value As String)
            m_kennz = Value
        End Set
    End Property

    Public Property Equimpent() As String
        Get
            Return m_equ
        End Get
        Set(ByVal Value As String)
            m_equ = Value
        End Set
    End Property

    Public Property Auftragsgrund() As String
        Get
            Return m_auftragsgrund
        End Get
        Set(ByVal Value As String)
            m_auftragsgrund = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblResult
        End Get
        Set(ByVal Value As DataTable)
            m_tblResult = Value
        End Set
    End Property

    Public ReadOnly Property GrundTabelle() As DataTable
        Get
            Return m_tableGrund
        End Get
    End Property

    Public Property SucheKennzeichen() As String
        Get
            Return m_strSucheKennzeichen
        End Get
        Set(ByVal Value As String)
            m_strSucheKennzeichen = Left(Value, 17)
        End Set
    End Property

    Public Property SucheLeasingvertragsNr() As String
        Get
            Return m_strSucheLeasingvertragsNr
        End Get
        Set(ByVal Value As String)
            m_strSucheLeasingvertragsNr = Value
        End Set
    End Property

    Public Property SucheNummerZB2() As String
        Get
            Return m_strSucheNummerZB2
        End Get
        Set(ByVal Value As String)
            m_strSucheNummerZB2 = Value
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

    Public Property LeaseplanZulassung() As Boolean
        Get
            Return m_blnLeaseplanZulassung
        End Get
        Set(ByVal Value As Boolean)
            m_blnLeaseplanZulassung = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String,
                    ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        m_blnLeaseplanZulassung = False

        m_strSucheFahrgestellNr = ""
        m_strSucheKennzeichen = ""
        m_strSucheLeasingvertragsNr = ""
        m_strSucheNummerZB2 = ""
        m_strBemerkung = ""
        m_strEmpfaengerName1 = ""
        m_strEmpfaengerName2 = ""
        m_strEmpfaengerOrt = ""
        m_strEmpfaengerPLZ = ""
        m_strEmpfaengerStrasse = ""
        m_strEmpfaengerHausnr = ""
        m_strHalterName1 = ""
        m_strHalterName2 = ""
        m_strHalterOrt = ""
        m_strHalterPLZ = ""
        m_strHalterStrasse = ""
        m_strHalterHausnr = ""
        m_strKreis = ""
        m_strReserviertAuf = ""
        m_strStandortName1 = ""
        m_strStandortName2 = ""
        m_strStandortOrt = ""
        m_strStandortPLZ = ""
        m_strStandortStrasse = ""
        m_strStandortHausnr = ""
        m_strVersicherungstraeger = ""
        m_strWunschkennzeichen = ""
        m_strBeauftragungKlartext = ""
        m_strExpress = " "
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Function SuggestionDay() As DateTime
        Dim datTemp As DateTime = Now
        Dim intAddDays As Int32 = 0
        Do While datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday Or intAddDays < 3
            datTemp = datTemp.AddDays(1)
            If Not (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday) Then
                intAddDays += 1
            End If
        Loop
        Return datTemp
    End Function

    Public Sub ClearResultTable()
        m_tblResult = Nothing
    End Sub

    Public Sub GiveCars()
        Dim row As DataRow

        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                S.AP.InitExecute("Z_M_Unangefordert_Lp", "I_KUNNR, I_LICENSE_NUM, I_LIZNR, I_CHASSIS_NUM, I_TIDNR",
                                 Right("0000000000" & m_objUser.KUNNR, 10), m_strSucheKennzeichen, m_strSucheLeasingvertragsNr, m_strSucheFahrgestellNr, m_strSucheNummerZB2)

                m_tableGrund = S.AP.GetExportTable("GT_GRU")
                m_tblResult = S.AP.GetExportTable("GT_WEB")
                m_tblResult.Columns.Add("STATUS", System.Type.GetType("System.String"))
                m_intStatus = 0

                For Each row In m_tblResult.Rows
                    row("STATUS") = String.Empty
                Next
                

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                End If

                WriteLogEntry(True, "FahrgestellNr=" & m_strSucheFahrgestellNr & ", LVNr.=" & m_strSucheLeasingvertragsNr & ", KfzKz.=" & m_strSucheKennzeichen & ", KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -3331
                        m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -3332
                        m_strMessage = "Keine oder falsche Haendlernummer."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                WriteLogEntry(False, "FahrgestellNr=" & m_strSucheFahrgestellNr & ", LVNr.=" & m_strSucheLeasingvertragsNr &
                                 ", KfzKz.=" & m_strSucheKennzeichen & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Anfordern()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                ClearError()


                S.AP.Init("Z_M_Dezdienstl_001")
                S.AP.SetImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_AUGRUND", Right("000000000000000000" & Split(m_auftragsgrund, "-")(0), 18))
                S.AP.SetImportParameter("I_EXPRESS", m_strExpress)
                S.AP.SetImportParameter("I_EQUNR", RemoveSingleSpace(m_equ))
                S.AP.SetImportParameter("I_CHASSIS_NUM", RemoveSingleSpace(m_strSucheFahrgestellNr))
                S.AP.SetImportParameter("I_WUKENNZ", m_strKreis & "-" & m_strWunschkennzeichen)
                S.AP.SetImportParameter("I_RES_AUF", m_strReserviertAuf)
                S.AP.SetImportParameter("I_VERSTR", m_strVersicherungstraeger)
                S.AP.SetImportParameter("I_LIEFDAT", m_datDurchfuehrungsDatum) 'Format(m_datDurchfuehrungsDatum, "yyyyMMdd")
                S.AP.SetImportParameter("I_BEMERKUNG", m_strBemerkung)
                S.AP.SetImportParameter("I_USER", m_objUser.UserName)
                S.AP.SetImportParameter("I_ZZVSNR", evbNummer)

                Dim Partner As DataTable = S.AP.GetImportTable("T_PARTNERS")
                Dim ImportRow As DataRow

                'Partnerrollen füllen
                'Halter
                If m_strHalterName1.Length + m_strHalterName2.Length + m_strHalterStrasse.Length + m_strHalterPLZ.Length + m_strHalterOrt.Length > 0 Then

                    ImportRow = Partner.NewRow

                    ImportRow("Parvw") = "ZH"
                    ImportRow("Name1") = m_strHalterName1
                    ImportRow("Name2") = m_strHalterName2
                    ImportRow("Street") = m_strHalterStrasse
                    ImportRow("House_Num1") = m_strHalterHausnr
                    ImportRow("Post_Code1") = m_strHalterPLZ
                    ImportRow("City1") = m_strHalterOrt
                    ImportRow("State") = "DE"

                    Partner.Rows.Add(ImportRow)

                End If

                'Standort
                If m_strStandortName1.Length + m_strStandortName2.Length + m_strStandortStrasse.Length + m_strStandortPLZ.Length + m_strStandortOrt.Length > 0 Then
                    
                    ImportRow = Partner.NewRow

                    ImportRow("Parvw") = "ZA"
                    ImportRow("Name1") = m_strStandortName1
                    ImportRow("Name2") = m_strStandortName2
                    ImportRow("Street") = m_strStandortStrasse
                    ImportRow("House_Num1") = m_strStandortHausnr
                    ImportRow("Post_Code1") = m_strStandortPLZ
                    ImportRow("City1") = m_strStandortOrt
                    ImportRow("State") = "DE"

                    Partner.Rows.Add(ImportRow)

                End If

                'Empfaenger
                If m_strEmpfaengerName1.Length + m_strEmpfaengerName2.Length + m_strEmpfaengerStrasse.Length + m_strEmpfaengerPLZ.Length + m_strEmpfaengerOrt.Length > 0 Then

                    ImportRow = Partner.NewRow

                    ImportRow("Parvw") = "ZE"
                    ImportRow("Name1") = m_strEmpfaengerName1
                    ImportRow("Name2") = m_strEmpfaengerName2
                    ImportRow("Street") = m_strEmpfaengerStrasse
                    ImportRow("House_Num1") = m_strEmpfaengerHausnr
                    ImportRow("Post_Code1") = m_strEmpfaengerPLZ
                    ImportRow("City1") = m_strEmpfaengerOrt
                    ImportRow("State") = "DE"

                    Partner.Rows.Add(ImportRow)

                End If

                If m_objUser.Reference.Length > 0 Then

                    ImportRow = Partner.NewRow

                    ImportRow("Parvw") = "RE"
                    ImportRow("KUNNR") = Right("0000000000" & m_objUser.Reference.ToString, 10)
                    ImportRow("Name1") = ""
                    ImportRow("Name2") = ""
                    ImportRow("Street") = ""
                    ImportRow("House_Num1") = ""
                    ImportRow("Post_Code1") = ""
                    ImportRow("City1") = ""
                    ImportRow("State") = ""

                    Partner.Rows.Add(ImportRow)

                End If

                If m_objUser.Reference.Length > 0 Then

                    ImportRow = Partner.NewRow

                    ImportRow("Parvw") = "RG"
                    ImportRow("KUNNR") = Right("0000000000" & m_objUser.Reference.ToString, 10)
                    ImportRow("Name1") = ""
                    ImportRow("Name2") = ""
                    ImportRow("Street") = ""
                    ImportRow("House_Num1") = ""
                    ImportRow("Post_Code1") = ""
                    ImportRow("City1") = ""
                    ImportRow("State") = ""

                    Partner.Rows.Add(ImportRow)

                End If

                S.AP.Execute()

                strAuftragsnummer = S.AP.GetExportParameter("O_VBELN").TrimStart("0"c)
                strAuftragsstatus = "Vorgang OK"

                If (strAuftragsnummer.Length = 0) Then
                    m_intStatus = -2100
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    strAuftragsstatus = "Keine Auftragsnummer erzeugt."
                End If

            Catch ex As Exception
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                Select Case Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        strAuftragsstatus = "Equipment zum Kunden nicht vorhanden"
                        m_intStatus = -4411
                    Case "ERR_HALTER"
                        strAuftragsstatus = "Fehler beim Anlegen Halter"
                        m_intStatus = -4412
                    Case "ERR_STANDORT"
                        strAuftragsstatus = "Fehler bei Standortsuche"
                        m_intStatus = -4413
                    Case "ERR_NO_EQUI"
                        strAuftragsstatus = "Kein zulassungsfähiges Equipment zu Fahrgestellnummer gefunden!"
                        m_intStatus = -4414
                    Case "ERR_INV_KUNNR"
                        strAuftragsstatus = "Unbekannte Kunnr!"
                        m_intStatus = -4415
                    Case "ERR_NO_ZULDAT"
                        strAuftragsstatus = "Kein Zulassungsdatum angegeben!"
                        m_intStatus = -4416
                    Case "ERR_INV_ZULDAT"
                        strAuftragsstatus = "Üngültiges Zulassungsdatum!"
                        m_intStatus = -4417
                    Case "ERR_NO_TRANSNAME"
                        strAuftragsstatus = "Kein Transaktionsname angegeben!"
                        m_intStatus = -4418
                    Case "ERR_INV_FAHRG"
                        strAuftragsstatus = "Ungültige Fahrgestellnummer!"
                        m_intStatus = -4419
                    Case "ERR_INV_BRIEFNR"
                        strAuftragsstatus = "Ungültige Briefnummer!"
                        m_intStatus = -4420
                    Case "ERR_NO_LIF"
                        strAuftragsstatus = "Kein Zulassungsdienst zu Zulassungsstelle gefunden!"
                        m_intStatus = -4421
                    Case "ERR_INV_PARVW"
                        strAuftragsstatus = "Ungültige Partnerrolle angegeben!"
                        m_intStatus = -4422
                    Case "ERR_INV_ZH"
                        strAuftragsstatus = "Ungültige Kundennummer für Halter!"
                        m_intStatus = -4423
                    Case "ERR_INV_ZH_ABWADR"
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für Halter!"
                        m_intStatus = -4424
                    Case "ERR_INV_ZS"
                        strAuftragsstatus = "Ungültige Kundennummer für Empfänger Brief!"
                        m_intStatus = -4425
                    Case "ERR_INV_ZS_ABWADR"
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für Empfänger Brief!"
                        m_intStatus = -4426
                    Case "ERR_INV_ZE"
                        strAuftragsstatus = "Ungültige Kundennummer für Empänger Schein & Schilder!"
                        m_intStatus = -4427
                    Case "ERR_INV_ZE_ABWADR"
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für Empfänger Schein & Schilder!"
                        m_intStatus = -4428
                    Case "ERR_INV_ZA"
                        strAuftragsstatus = "Ungültige Kundennummer für Standort!"
                        m_intStatus = -4429
                    Case "ERR_INV_ZA_ABWADR"
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für Standort!"
                        m_intStatus = -4430
                    Case "ERR_SAVE"
                        strAuftragsstatus = "Fehler beim Speichern!"
                        m_intStatus = -4431
                    Case "ERR_INV_VERSART_STR1"
                        strAuftragsstatus = "Fehlerhafte Versandart Strecke 1!"
                        m_intStatus = -4432
                    Case "ERR_INV_VERSART_STR2"
                        strAuftragsstatus = "Fehlerhafte Versandart Strecke 2!"
                        m_intStatus = -4433
                    Case "ERR_INV_ZV"
                        strAuftragsstatus = "Ungültige Kundennummer für Versicherer!"
                        m_intStatus = -4434
                    Case "ERR_INV_ZC"
                        strAuftragsstatus = "Ungültige Kundennummer für abw. Versicherungsnehmer!"
                        m_intStatus = -4435
                    Case "ERR_INV_ZC_ABWADR"
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für abw. Versicherungsnehmer!"
                        m_intStatus = -4436
                    Case "ERR_NO_ZH"
                        strAuftragsstatus = "Kein Halter angegeben!"
                        m_intStatus = -4437
                    Case "ERR_NO_ZS"
                        strAuftragsstatus = "Kein Empfänger Brief angegeben!"
                        m_intStatus = -4438
                    Case "ERR_NO_ZE"
                        strAuftragsstatus = "Kein Empfänger Schein & Schilder angegeben!"
                        m_intStatus = -4439
                    Case "ERR_SONST"
                        strAuftragsstatus = "Unbekannter Fehler"
                        m_intStatus = -4440
                    Case "ERR_CS_MEL"
                        strAuftragsstatus = "Fehler bei anlegen CS-Meldung"
                        m_intStatus = -4441
                    Case Else
                        strAuftragsstatus = ex.Message
                        m_intStatus = -9999
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Function RemoveSingleSpace(ByVal strIn As String) As String
        Dim strReturn As String = ""
        Try
            Dim strOut As String = strIn.Trim(" "c)
            If strOut = "&nbsp;" Then
                strOut = ""
            End If
            strReturn = strOut
        Catch
        End Try
        Return strReturn
    End Function

    Public Function GiveResultStructure() As DataTable
        Dim tblTemp As New DataTable

        With tblTemp
            .Columns.Add("MANDT", System.Type.GetType("System.String"))
            .Columns.Add("KONZS", System.Type.GetType("System.String"))
            .Columns.Add("KNRZE", System.Type.GetType("System.String"))
            .Columns.Add("KUNNR", System.Type.GetType("System.String"))
            .Columns.Add("ZZVBELN", System.Type.GetType("System.String"))
            .Columns.Add("ERDAT", System.Type.GetType("System.String"))
            .Columns.Add("EQUNR", System.Type.GetType("System.String"))
            .Columns.Add("EQTYP", System.Type.GetType("System.String"))
            .Columns.Add("ZZCOCKZ", System.Type.GetType("System.String"))
            .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
            .Columns.Add("LICENSE_NUM", System.Type.GetType("System.String"))
            .Columns.Add("LIZNR", System.Type.GetType("System.String"))
            .Columns.Add("TIDNR", System.Type.GetType("System.String"))
            .Columns.Add("ZZKKBER", System.Type.GetType("System.String"))
            .Columns.Add("ZZBEZAHLT", System.Type.GetType("System.String"))
            .Columns.Add("CMGST", System.Type.GetType("System.String"))
            .Columns.Add("ZZTMPDT", System.Type.GetType("System.String"))
            .Columns.Add("ZZANFDT", System.Type.GetType("System.String"))
            .Columns.Add("ZZANFZT", System.Type.GetType("System.String"))
            .Columns.Add("ZZFAEDT", System.Type.GetType("System.String"))
            .Columns.Add("ZZABRDT", System.Type.GetType("System.String"))
            .Columns.Add("ZZFREIDT", System.Type.GetType("System.String"))
            .Columns.Add("ZZFREIZT", System.Type.GetType("System.String"))
            .Columns.Add("ZZMAHNA", System.Type.GetType("System.String"))
            .Columns.Add("ABCKZ", System.Type.GetType("System.String"))
            .Columns.Add("MSGRP", System.Type.GetType("System.String"))
            .Columns.Add("STORT", System.Type.GetType("System.String"))
            .Columns.Add("ZZFINART", System.Type.GetType("System.String"))
            .Columns.Add("ZZLABEL", System.Type.GetType("System.String"))
            .Columns.Add("ZZREFERENZ1", System.Type.GetType("System.String"))
            .Columns.Add("ZZREFERENZ2", System.Type.GetType("System.String"))
            .Columns.Add("ADRNR", System.Type.GetType("System.String"))
            .Columns.Add("NAME1", System.Type.GetType("System.String"))
            .Columns.Add("NAME2", System.Type.GetType("System.String"))
            .Columns.Add("NAME3", System.Type.GetType("System.String"))
            .Columns.Add("CITY1", System.Type.GetType("System.String"))
            .Columns.Add("POST_CODE1", System.Type.GetType("System.String"))
            .Columns.Add("STREET", System.Type.GetType("System.String"))
            .Columns.Add("HOUSE_NUM1", System.Type.GetType("System.String"))
            .Columns.Add("HINWEIS", System.Type.GetType("System.String"))
            .Columns.Add("VERDT1", System.Type.GetType("System.String"))
            .Columns.Add("VERDT2", System.Type.GetType("System.String"))
            .Columns.Add("VERDT3", System.Type.GetType("System.String"))
            .Columns.Add("WIEDT1", System.Type.GetType("System.String"))
            .Columns.Add("WIEDT2", System.Type.GetType("System.String"))
            .Columns.Add("WIEDT3", System.Type.GetType("System.String"))
            .Columns.Add("COCDT1", System.Type.GetType("System.String"))
            .Columns.Add("CRBLB", System.Type.GetType("System.String"))
            .Columns.Add("ZZSPERR_DAD", System.Type.GetType("System.String"))
            .Columns.Add("ZZINSOLVENZ", System.Type.GetType("System.String"))
            .Columns.Add("TEXT50", System.Type.GetType("System.String"))
            .Columns.Add("TEXT200", System.Type.GetType("System.String"))
            .Columns.Add("REPLA_DATE", System.Type.GetType("System.String"))
            .Columns.Add("EXPIRY_DATE", System.Type.GetType("System.String"))
            .Columns.Add("ZZZLDAT", System.Type.GetType("System.String"))
            .Columns.Add("ZZSTATUS_ZUG", System.Type.GetType("System.String"))
            .Columns.Add("ZZSTATUS_IABG", System.Type.GetType("System.String"))
            .Columns.Add("ZZSTATUS_ABG", System.Type.GetType("System.String"))
            .Columns.Add("ZZVGRUND", System.Type.GetType("System.String"))
            .Columns.Add("STATUS", System.Type.GetType("System.String"))
        End With

        Return tblTemp
    End Function
#End Region

End Class
