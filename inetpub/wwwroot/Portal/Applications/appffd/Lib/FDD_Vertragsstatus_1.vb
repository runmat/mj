Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class FDD_Vertragsstatus_1
    REM § Lese-/Schreibfunktion, Kunde: FFD, 
    REM § VersandOhneZahlung - BAPI: Z_M_Status_Endg_Vers_Ohne_Zlng,
    REM § VertragSingle - BAPI: Z_M_Status_Versand.

    Inherits Base.Business.BankBaseCredit

#Region " Declarations"
    Private m_strSucheVertragsnummer As String
    Private m_strSucheOrdernummer As String
    Private m_strTage As String

    Private m_tblVertraege As DataTable

    Private m_strHaendlernummer As String
    Private m_strVertragsnummer As String
    Private m_strOrdernummer As String
    Private m_strFahrgestellnummer As String
    Private m_strBriefnummer As String
    Private m_strKennzeichen As String
    Private m_strFinanzierungsart As String
    Private m_strMahnverfahren As String
    Private m_datVersanddatum As Date
    Private m_datAnforderungsdatum As Date
    Private m_datAbrechnungdatum As Date

    Private m_strNAME As String
    Private m_strNAME_2 As String
    Private m_strNAME_3 As String
    Private m_strPOSTL_CODE As String
    Private m_strCITY As String
    Private m_strSTREET As String
    Private m_strHOUSE_NUM As String
    Private m_strBetrag As String

    Private m_strKontingentart As String

    Private m_blnBezahlt As Boolean
    Private m_blnCOCBescheinigungVorhanden As Boolean
    Private m_blnVersendet As Boolean
    Private m_blnAngefordert As Boolean
    Private m_blnAngefordertTemporaer As Boolean
    Private m_blnAngefordertEndgueltig As Boolean
    Private m_blnAngefordertDelayPayment As Boolean
    Private m_blnAngefordertRetail As Boolean

    Private m_strKunde As String
#End Region

#Region " Properties"
    Public ReadOnly Property Abrechnungdatum() As Date
        Get
            Return m_datAbrechnungdatum
        End Get
    End Property

    Public ReadOnly Property Kontingentart() As String
        Get
            Return m_strKontingentart
        End Get
    End Property

    Public ReadOnly Property NAME() As String
        Get
            Return m_strNAME
        End Get
    End Property

    Public ReadOnly Property NAME_2() As String
        Get
            Return m_strNAME_2
        End Get
    End Property

    Public ReadOnly Property NAME_3() As String
        Get
            Return m_strNAME_3
        End Get
    End Property

    Public ReadOnly Property POSTL_CODE() As String
        Get
            Return m_strPOSTL_CODE
        End Get
    End Property

    Public ReadOnly Property CITY() As String
        Get
            Return m_strCITY
        End Get
    End Property

    Public ReadOnly Property STREET() As String
        Get
            Return m_strSTREET
        End Get
    End Property

    Public ReadOnly Property HOUSE_NUM() As String
        Get
            Return m_strHOUSE_NUM
        End Get
    End Property

    Public ReadOnly Property Haendlernummer() As String
        Get
            Return m_strHaendlernummer
        End Get
    End Property

    Public ReadOnly Property Vertragsnummer() As String
        Get
            Return m_strVertragsnummer
        End Get
    End Property

    Public ReadOnly Property Ordernummer() As String
        Get
            Return m_strOrdernummer
        End Get
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property

    Public ReadOnly Property Briefnummer() As String
        Get
            Return m_strBriefnummer
        End Get
    End Property

    Public ReadOnly Property Kennzeichen() As String
        Get
            Return m_strKennzeichen
        End Get
    End Property

    Public ReadOnly Property Finanzierungsart() As String
        Get
            Return m_strFinanzierungsart
        End Get
    End Property

    Public ReadOnly Property Mahnverfahren() As String
        Get
            Return m_strMahnverfahren
        End Get
    End Property

    Public ReadOnly Property Versanddatum() As Date
        Get
            Return m_datVersanddatum
        End Get
    End Property

    Public ReadOnly Property Anforderungsdatum() As Date
        Get
            Return m_datAnforderungsdatum
        End Get
    End Property

    Public ReadOnly Property Bezahlt() As Boolean
        Get
            Return m_blnBezahlt
        End Get
    End Property

    Public ReadOnly Property COCBescheinigungVorhanden() As Boolean
        Get
            Return m_blnCOCBescheinigungVorhanden
        End Get
    End Property

    Public ReadOnly Property Versendet() As Boolean
        Get
            Return m_blnVersendet
        End Get
    End Property

    Public ReadOnly Property Angefordert() As Boolean
        Get
            Return m_blnAngefordert
        End Get
    End Property

    Public ReadOnly Property AngefordertTemporaer() As Boolean
        Get
            Return m_blnAngefordertTemporaer
        End Get
    End Property

    Public ReadOnly Property AngefordertEndgueltig() As Boolean
        Get
            Return m_blnAngefordertEndgueltig
        End Get
    End Property

    Public ReadOnly Property AngefordertDelayPayment() As Boolean
        Get
            Return m_blnAngefordertDelayPayment
        End Get
    End Property

    Public ReadOnly Property AngefordertRetail() As Boolean
        Get
            Return m_blnAngefordertRetail
        End Get
    End Property

    Public ReadOnly Property Betrag() As String
        Get
            Return m_strBetrag
        End Get
    End Property
    Public Property SucheVertragsnummer() As String
        Get
            Return m_strSucheVertragsnummer
        End Get
        Set(ByVal Value As String)
            m_strSucheVertragsnummer = Value
        End Set
    End Property

    Public Property SucheOrdernummer() As String
        Get
            Return m_strSucheOrdernummer
        End Get
        Set(ByVal Value As String)
            m_strSucheOrdernummer = Value
        End Set
    End Property

    Public Property Vertraege() As DataTable
        Get
            Return m_tblVertraege
        End Get
        Set(ByVal Value As DataTable)
            m_tblVertraege = Value
        End Set
    End Property

    Public Property Tage() As String
        Get
            Return m_strTage
        End Get
        Set(ByVal Value As String)
            m_strTage = Right("00" & Value, 2)
        End Set
    End Property

    Public Property Kunde() As String
        Get
            Return m_strKunde
        End Get
        Set(ByVal Value As String)
            m_strKunde = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal strCreditControlArea As String = "ZDAD")
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        Customer = objUser.KUNNR
        CreditControlArea = strCreditControlArea
        m_intStatus = 0
        m_strMessage = ""
        m_strTage = "00"
        m_strSucheOrdernummer = ""
        m_strSucheVertragsnummer = ""
        m_blnAngefordert = False
        m_blnAngefordertEndgueltig = False
        m_blnAngefordertTemporaer = False
        m_blnVersendet = False
    End Sub

    Public Sub VertragSingle(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FDD_Vertragsstatus_1.VertragSingle"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""
                m_strHaendlernummer = ""
                m_strVertragsnummer = ""
                m_strOrdernummer = ""
                m_strBriefnummer = ""
                m_strKennzeichen = ""
                m_strFinanzierungsart = ""
                m_strMahnverfahren = ""
                m_datVersanddatum = Nothing
                m_datAnforderungsdatum = Nothing
                m_datAbrechnungdatum = Nothing
                m_strBetrag = ""
                m_strNAME = ""
                m_strNAME_2 = ""
                m_strNAME_3 = ""
                m_strPOSTL_CODE = ""
                m_strCITY = ""
                m_strSTREET = ""
                m_strHOUSE_NUM = ""

                m_strKontingentart = ""
                m_blnBezahlt = False
                m_blnVersendet = False
                m_blnAngefordert = False
                m_blnAngefordertTemporaer = False
                m_blnAngefordertEndgueltig = False

                If CheckCustomerData() Then
                   
                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Status", m_objApp, m_objUser, page)

                    'myProxy.setImportParameter("I_KUNNR", strLeer)
                    'myProxy.setImportParameter("I_KNRZE", m_strFiliale)
                    'myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_strCustomer, 10))
                    'myProxy.setImportParameter("I_CHASSIS_NUM", m_strFahrgestellnummer)
                    'myProxy.setImportParameter("I_LIZNR", m_strSucheVertragsnummer)
                    'myProxy.setImportParameter("I_ZZREFERENZ1", m_strSucheOrdernummer)
                    'myProxy.setImportParameter("I_VKORG", "1510")

                    'myProxy.callBapi()

                    S.AP.InitExecute("Z_M_Status", "I_KUNNR,I_KNRZE,I_KONZS,I_CHASSIS_NUM,I_LIZNR,I_ZZREFERENZ1,I_VKORG",
                                     "", m_strFiliale, Right("0000000000" & m_strCustomer, 10), m_strFahrgestellnummer, m_strSucheVertragsnummer, m_strSucheOrdernummer, "1510")

                    Dim tblTemp As DataTable
                    tblTemp = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                    Dim rowVertrag As DataRow
                    If tblTemp.Rows.Count > 0 Then
                        rowVertrag = tblTemp.Rows(0)
                        m_strHaendlernummer = Right(rowVertrag("Kunnr").ToString, 5).TrimStart("0"c)
                        m_strVertragsnummer = rowVertrag("Liznr").ToString
                        m_strOrdernummer = rowVertrag("ZZREFERENZ1").ToString
                        m_strFahrgestellnummer = rowVertrag("Chassis_Num").ToString
                        m_strBriefnummer = rowVertrag("Tidnr").ToString
                        m_strKennzeichen = rowVertrag("License_Num").ToString
                        m_strFinanzierungsart = rowVertrag("Zzfinart").ToString
                        m_strMahnverfahren = rowVertrag("Zzmahna").ToString
                        m_strNAME = rowVertrag("Name1").ToString
                        m_strNAME_2 = rowVertrag("Name2").ToString
                        m_strNAME_3 = rowVertrag("Name3").ToString
                        m_strPOSTL_CODE = rowVertrag("Post_Code1").ToString
                        m_strCITY = rowVertrag("City1").ToString
                        m_strSTREET = rowVertrag("Street").ToString
                        m_strHOUSE_NUM = rowVertrag("House_Num1").ToString
                        m_strKunde = rowVertrag("Text50").ToString
                        m_strBetrag = rowVertrag("Answt").ToString
                        Select Case rowVertrag("Zzkkber").ToString
                            Case "0001"
                                m_strKontingentart = "Standard temporär"
                            Case "0002"
                                m_strKontingentart = "Standard endgültig"
                            Case "0003"
                                m_strKontingentart = "Retail"
                            Case "0004"
                                m_strKontingentart = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                        End Select

                        If IsDate(rowVertrag("Zzabrdt").ToString) Then
                            m_datAbrechnungdatum = CDate(rowVertrag("Zzabrdt").ToString)
                        End If
                        If IsDate(rowVertrag("Zztmpdt").ToString) Then
                            m_datVersanddatum = CDate(rowVertrag("Zztmpdt").ToString)
                            If Not rowVertrag("Zztmpdt").ToString = "" Then
                                m_blnVersendet = True
                            Else
                                m_blnVersendet = False
                            End If
                        Else
                            m_blnVersendet = False
                        End If
                        If IsDate(rowVertrag("Zzanfdt").ToString) Then
                            m_datAnforderungsdatum = CDate(rowVertrag("Zzanfdt").ToString)
                        End If
                        Dim sTemp As String = rowVertrag("Abckz").ToString
                        Select Case rowVertrag("Abckz").ToString.Trim(" "c)
                            Case ""
                                m_blnAngefordert = False
                                m_blnAngefordertTemporaer = False
                                m_blnAngefordertEndgueltig = False
                                m_blnAngefordertDelayPayment = False
                                m_blnAngefordertRetail = False
                            Case "1"
                                m_blnAngefordert = True
                                m_blnAngefordertTemporaer = True
                                m_blnAngefordertEndgueltig = False
                                m_blnAngefordertDelayPayment = False
                                m_blnAngefordertRetail = False
                            Case "2"
                                m_blnAngefordert = True
                                m_blnAngefordertTemporaer = False
                                m_blnAngefordertEndgueltig = True
                                m_blnAngefordertDelayPayment = False
                                m_blnAngefordertRetail = False
                                If rowVertrag("Zzkkber").ToString = "0003" Then
                                    m_blnAngefordert = True
                                    m_blnAngefordertTemporaer = False
                                    m_blnAngefordertEndgueltig = False
                                    m_blnAngefordertRetail = True
                                    m_blnAngefordertDelayPayment = False
                                End If
                                If rowVertrag("Zzkkber").ToString = "0004" Then
                                    m_blnAngefordert = True
                                    m_blnAngefordertTemporaer = False
                                    m_blnAngefordertEndgueltig = False
                                    m_blnAngefordertRetail = False
                                    m_blnAngefordertDelayPayment = True
                                End If
                        End Select
                        If UCase(rowVertrag("Zzbezahlt").ToString.Trim(" "c)) = "X" Then
                            m_blnBezahlt = True
                        Else
                            m_blnBezahlt = False
                        End If
                        If UCase(rowVertrag("Zzcockz").ToString.Trim(" "c)) = "X" Then
                            m_blnCOCBescheinigungVorhanden = True
                        Else
                            m_blnCOCBescheinigungVorhanden = False
                        End If
                    Else
                        m_intStatus = -1
                        m_strMessage = "Zu den gewählten Kriterien wurden keine Daten gefunden." '"Keine Ergebnisse."
                    End If
                    WriteLogEntry(True, "KNRZE=" & m_strFiliale & ", KONZS=" & m_strCustomer & ", KUNNR=, CHASSIS_NUM=" & m_strFahrgestellnummer & ", LIZNR= " & m_strSucheVertragsnummer & ", ZZREFERENZ1= " & m_strSucheOrdernummer, m_tblResult)
                End If
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -1
                        m_strMessage = "Zu den gewählten Kriterien wurden keine Daten gefunden." '"Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_intStatus = -2
                        m_strMessage = "Zu den gewählten Kriterien wurden keine Daten gefunden."    '"Keine Web-Tabelle erstellt."
                    Case "NO_HAENDLER"
                        m_intStatus = -3
                        m_strMessage = "Zu den gewählten Kriterien wurden keine Daten gefunden."    '"Händler nicht vorhanden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "KNRZE=" & m_strFiliale & ", KONZS=" & m_strCustomer & ", KUNNR=, CHASSIS_NUM=" & m_strFahrgestellnummer & ", LIZNR= " & m_strSucheVertragsnummer & ", ZZREFERENZ1= " & m_strSucheOrdernummer & " , " & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If
                m_blnGestartet = False
            End Try
        End If
    End Sub
  
#End Region

End Class

' ************************************************
' $History: FDD_Vertragsstatus_1.vb $
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
' *****************  Version 7  *****************
' User: Rudolpho     Date: 27.08.07   Time: 15:48
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA: 1278
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 2.07.07    Time: 13:43
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.06.07   Time: 17:03
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Portal - Startapplication 13.06.2005
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' ************************************************
