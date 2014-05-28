Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
'Imports CKG.Base.Business
'Imports Microsoft.Data.SAPClient

Public Class FFE_Bank_3
    REM § Lese-/Schreibfunktion, Kunde: FFD, 
    REM § Show/Report - BAPI: Zz_Unbezahlt_Equipment,
    REM § Change - BAPI: Als Internetfunktionalität abgeschaltet (Zz_Unbezahlt_Equi_Change).

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_tblFahrzeuge As DataTable
    Private m_tblFahrzeugeExcel As DataTable
    Private m_strFahrgestellnummer As String
    Private m_strFahrgestellnummerSuche As String
    Private m_strBriefnummerSuche As String
    Private m_strVertragsnummer As String
    Private m_strOrdernummer As String
    Private m_strVorgaenge As String
    Private gesamterBestand As Boolean
    Private kontingentenotVisible As Boolean
    Private haendlernr As String
#End Region

#Region " Properties"
    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeuge = Value
        End Set
    End Property

    Public Property FahrzeugeExcel() As DataTable
        Get
            Return m_tblFahrzeugeExcel
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeugeExcel = Value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property

    Public Property FahrgestellnummerSuche() As String
        Get
            Return m_strFahrgestellnummerSuche
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummerSuche = Value
        End Set
    End Property
    Public Property BriefnummerSuche() As String
        Get
            Return m_strBriefnummerSuche
        End Get
        Set(ByVal Value As String)
            m_strBriefnummerSuche = Value
        End Set
    End Property
    Public Property Ordernummer() As String
        Get
            Return m_strOrdernummer
        End Get
        Set(ByVal Value As String)
            m_strOrdernummer = Value
        End Set
    End Property

    Public Property Vertragsnummer() As String
        Get
            Return m_strVertragsnummer
        End Get
        Set(ByVal Value As String)
            m_strVertragsnummer = Value
        End Set
    End Property

    Public Property Vorgaenge() As String
        Get
            Return m_strVorgaenge
        End Get
        Set(ByVal Value As String)
            m_strVorgaenge = Value
        End Set
    End Property

    Public Property GesamtBestand() As Boolean
        Get
            Return gesamterBestand
        End Get
        Set(ByVal Value As Boolean)
            gesamterBestand = Value
        End Set
    End Property

    Public Property KontingentNotVisible() As Boolean
        Get
            Return kontingentenotVisible
        End Get
        Set(ByVal Value As Boolean)
            kontingentenotVisible = Value
        End Set
    End Property
    Public Property Haendlernummer() As String
        Get
            Return haendlernr
        End Get
        Set(ByVal Value As String)
            haendlernr = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_strVertragsnummer = ""
        m_strOrdernummer = ""
        m_strFahrgestellnummer = ""
        m_strVorgaenge = "alle"
    End Sub

    Public Sub Report()
        m_strClassAndMethod = "FDD_Bank_3.Report"

        If Not m_blnGestartet Then
            m_blnGestartet = True

            kontingentenotVisible = False

            Dim nummerhaendler As String = ""

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                ClearError()
               
                Dim tempproof As Boolean
                If GesamtBestand = False Then
                    nummerhaendler = Right("00000" & m_strCustomer, 5)
                    tempproof = CheckCustomerData()
                Else
                    tempproof = True
                End If

                If tempproof Then
                    Dim strLeer As String = String.Empty

                    S.AP.Init("Z_M_Unbezahlte_Equipments_Fce")

                    S.AP.SetImportParameter("I_KUNNR", nummerhaendler)
                    S.AP.SetImportParameter("I_KNRZE", strLeer)
                    S.AP.SetImportParameter("I_KONZS", m_strKUNNR)
                    S.AP.SetImportParameter("I_CHASSIS_NUM", m_strFahrgestellnummerSuche)
                    S.AP.SetImportParameter("I_LIZNR", m_strVertragsnummer)
                    S.AP.SetImportParameter("I_ZZREFERENZ1", m_strOrdernummer)
                    S.AP.SetImportParameter("I_VKORG", "1510")
                    S.AP.SetImportParameter("I_ZB2", m_strBriefnummerSuche)

                    S.AP.Execute()
                    
                    Dim tblFahrzeugeEquiSAP As DataTable = S.AP.GetExportTable("GT_WEB") 'DirectCast(pSAPTable.Value, DataTable)

                    m_tblFahrzeuge = New DataTable()
                    m_tblFahrzeuge.Columns.Add("Vertragsnummer", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Ordernummer", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("COC", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Erfassung Fahrzeug", System.Type.GetType("System.DateTime"))
                    m_tblFahrzeuge.Columns.Add("Angefordert am", System.Type.GetType("System.DateTime"))
                    m_tblFahrzeuge.Columns.Add("Angefordert um", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Gesperrt", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Freigegeben am", System.Type.GetType("System.DateTime"))
                    m_tblFahrzeuge.Columns.Add("Freigegeben um", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Versendet am", System.Type.GetType("System.DateTime"))

                    m_tblFahrzeugeExcel = New DataTable()
                    m_tblFahrzeugeExcel.Columns.Add("Finanzierungsnummer", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Ordernummer", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("COC", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Eingang ZBII", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Angefordert am", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Angefordert um", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Gesperrt", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Freigegeben am", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Freigegeben um", System.Type.GetType("System.String"))
                    m_tblFahrzeugeExcel.Columns.Add("Versendet am", System.Type.GetType("System.String"))

                    Dim tblTemp As DataTable = tblFahrzeugeEquiSAP
                    Dim rowTemp As DataRow
                    Dim kunnrchange As String = ""

                    If tblTemp.Rows.Count > 0 Then
                        kunnrchange = tblTemp.Rows(0)("KUNNR").ToString
                    End If

                    For Each rowTemp In tblTemp.Rows
                        Dim blnAddRow As Boolean = False

                        If m_strVorgaenge = "alle" Then
                            blnAddRow = True
                        Else
                            If m_strVorgaenge = "angefordert" And rowTemp("ZZANFDT").ToString.Trim(" "c) <> "00000000" Then
                                blnAddRow = True
                            Else
                                If m_strVorgaenge = "nichtangefordert" And rowTemp("ZZANFDT").ToString.Trim(" "c) = "00000000" Then
                                    blnAddRow = True
                                End If
                            End If
                        End If

                        If blnAddRow Then
                            Dim kunnrproof As String = rowTemp("KUNNR").ToString
                            
                            Dim rowNew As DataRow = m_tblFahrzeuge.NewRow
                            Dim datTemp As DateTime

                            Dim rowNewExcel As DataRow = m_tblFahrzeugeExcel.NewRow
                            rowNew("Vertragsnummer") = rowTemp("LIZNR")
                            rowNewExcel("Finanzierungsnummer") = rowTemp("LIZNR")

                            rowNew("Fahrgestellnummer") = rowTemp("CHASSIS_NUM")
                            rowNewExcel("Fahrgestellnummer") = rowTemp("CHASSIS_NUM")

                            rowNew("Ordernummer") = rowTemp("ZZREFERENZ1")
                            rowNewExcel("Ordernummer") = rowTemp("ZZREFERENZ1")

                            rowNew("COC") = rowTemp("ZZCOCKZ")
                            rowNewExcel("COC") = rowTemp("ZZCOCKZ")

                            datTemp = CDate(rowTemp("ERDAT"))
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Erfassung Fahrzeug") = datTemp
                                rowNewExcel("Eingang ZBII") = Format(datTemp, "dd.MM.yyyy")
                            End If

                            datTemp = CDate(rowTemp("ZZANFDT"))
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Angefordert am") = datTemp
                                rowNewExcel("Angefordert am") = Format(datTemp, "dd.MM.yyyy")
                            End If

                            datTemp = CDate(rowTemp("ZZANFZT"))
                            If datTemp > CDate("00:00:00") Then
                                rowNew("Angefordert um") = Format(datTemp, "HH:mm:ss")
                                rowNewExcel("Angefordert um") = Format(datTemp, "HH:mm:ss")
                            End If

                            Dim strTemp As String = ""
                            Select Case rowTemp("ZZKKBER").ToString
                                Case "0001"
                                    strTemp = "Standard temporär"
                                Case "0002"
                                    strTemp = "Standard endgültig"
                                Case "0003"
                                    strTemp = "Retail"
                                Case "0004"
                                    strTemp = "Delayed Payment"
                                Case "0005"
                                    strTemp = "Händlereigene Zulassung (HEZ)"       'HEZ 31.05.05 eingefügt
                                Case "0006"
                                    strTemp = "KF/KL"       'HEZ 31.05.05 eingefügt
                            End Select

                            If strTemp.Length > 0 Then
                                rowNew("Kontingentart") = strTemp
                                rowNewExcel("Kontingentart") = strTemp
                            End If

                            If rowTemp("CMGST").ToString = "B" Then
                                rowNew("Gesperrt") = "In Bearbeitung Bank"
                                rowNewExcel("Gesperrt") = "In Bearbeitung Bank"
                            Else
                                rowNew("Gesperrt") = ""
                                rowNewExcel("Gesperrt") = ""
                            End If

                            datTemp = CDate(rowTemp("ZZFREIDT"))
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Freigegeben am") = datTemp
                                rowNewExcel("Freigegeben am") = Format(datTemp, "dd.MM.yyyy")
                            End If

                            datTemp = CDate(rowTemp("ZZFREIZT"))
                            If datTemp > CDate("00:00:00") Then
                                rowNew("Freigegeben um") = Format(datTemp, "HH:mm:ss")
                                rowNewExcel("Freigegeben um") = Format(datTemp, "HH:mm:ss")
                            End If

                            datTemp = CDate(rowTemp("ZZTMPDT"))
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Versendet am") = datTemp
                                rowNewExcel("Versendet am") = Format(datTemp, "dd.MM.yyyy")
                            End If

                            m_tblFahrzeuge.Rows.Add(rowNew)
                            m_tblFahrzeugeExcel.Rows.Add(rowNewExcel)

                            If kunnrchange <> kunnrproof Then
                                kontingentenotVisible = True
                                haendlernr = String.Empty
                            Else : haendlernr = kunnrproof
                            End If
                        End If
                    Next

                    WriteLogEntry(True, "CHASSIS_NUM=" & m_strFahrgestellnummerSuche & ", KNRZE=, KONZS=" & m_objUser.KUNNR & ", KUNNR=" &
                                  Right("00000" & m_strCustomer, 5) & ", LIZNR=" & m_strVertragsnummer & ", ZZREFERENZ1=" & m_strOrdernummer, m_tblFahrzeugeExcel)
                End If
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        RaiseError("-1", "Keine Eingabedaten vorhanden.")
                    Case "NO_HAENDLER"
                        RaiseError("-2", "Händler nicht vorhanden.")
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select

                WriteLogEntry(False, "CHASSIS_NUM=" & m_strFahrgestellnummerSuche & ", KNRZE=, KONZS=" & m_objUser.KUNNR &
                              ", KUNNR=" & Right("00000" & m_strCustomer, 5) & ", LIZNR=" & m_strVertragsnummer & ", ZZREFERENZ1=" &
                              m_strOrdernummer & " , " & Replace(m_strMessage, "<br>", " "), m_tblFahrzeugeExcel)
            End Try
        End If

    End Sub
 
    Public Overrides Sub Show()

    End Sub

    Public Overrides Sub Change()

    End Sub

#End Region

End Class
' ************************************************
' $History: FFE_Bank_3.vb $
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 7.08.08    Time: 14:34
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA:2058
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 24.07.08   Time: 12:39
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.06.08    Time: 9:21
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 5.06.08    Time: 13:04
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 21.05.08   Time: 16:34
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Applications/AppFFE/lib
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.04.08    Time: 13:32
' Created in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 9.04.08    Time: 13:06
' Updated in $/CKG/Applications/AppFFE/AppFFEWeb/lib
' ITA: 1790
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 3.04.08    Time: 11:19
' Created in $/CKG/Applications/AppFFE/AppFFEWeb/lib
' ITA 1790
' 
' ************************************************