Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class fin_17
    REM § Lese-/Schreibfunktion, Kunde: allg. Banken App, 
    REM § Show - BAPI: Z_M_DATEN_EINZ_Report_001,
    REM § Change - BAPI: Z_M_DATEN_Anf_Change_001.

    Inherits BankBase

#Region " Declarations"
    Private m_strINKennzeichen As String
    Private m_strINVertragsnummer As String
    Private m_strINFahrgestellnummer As String

    'LIZNR	        Lizenznummer des Equipments
    Private m_strOUTKontonummer As String
    'CHASSIS_NUM	Fahrgestellnummer
    Private m_strOUTFahrgestellnummer As String
    'LICENSE_NUM	Kfz-Kennzeichen
    Private m_strOUTKennzeichen As String
    'EQUNR	        Equipmentnummer
    Private m_strEQUNR As String
    'TIDNR	        Technische Identnummer
    Private m_strBriefNr As String
    'ZZLABEL        DAD FFD Label/Marke
    Private m_strLabel As String
    'ZZMODID	    DAD FFD Modell
    Private m_strModell As String
    'EQUI_ERDAT	    Datum, an dem der Satz hinzugefügt wurde
    Private m_datErsteingang As Date
    'VERSAND1	    Feld vom Typ DATS
    Private m_datNochmaligerVersand1 As Date
    'VERSAND2	    Feld vom Typ DATS
    Private m_datNochmaligerVersand2 As Date
    'VERSAND3	    Feld vom Typ DATS
    Private m_datNochmaligerVersand3 As Date
    'ZZAKTSPERRE	Sperrkennzeichen
    Private m_blnGesperrt As Boolean
    'WIEDER1	        Datum, an dem der Satz hinzugefügt wurde
    Private m_datWiedereingang1 As Date
    'WIEDER2	        Datum, an dem der Satz hinzugefügt wurde
    Private m_datWiedereingang2 As Date
    'WIEDER3	        Datum, an dem der Satz hinzugefügt wurde
    Private m_datWiedereingang3 As Date
    'ZZTMPDT	    Feld vom Typ DATS
    Private m_datVersand As Date
    'E_ZZCOCKZ      COC-Bescheinigung vorhanden = X
    Private m_blnCOCBescheinigungVorhanden As Boolean
    'E_COCDT1       COCKZ Änderungsdatum
    Private m_datCOCKZAenderungsdatum As Date

    Private m_strFaxnummer As String
    Private m_strMailadresse As String
    Private m_blnZusatz As Boolean
    Private strZB2Nummer As String
#End Region

#Region " Properties"
    Public Property Kontonummer() As String
        Get
            Return m_strOUTKontonummer
        End Get
        Set(ByVal Value As String)
            m_strOUTKontonummer = Value
        End Set
    End Property

    Public Property ZB2Nummer() As String
        Get
            Return strZB2Nummer
        End Get
        Set(ByVal Value As String)
            strZB2Nummer = Value
        End Set
    End Property

    Public ReadOnly Property Fahrgestellnummer() As String
        Get
            Return m_strOUTFahrgestellnummer
        End Get
    End Property

    Public ReadOnly Property Kennzeichen() As String
        Get
            Return m_strOUTKennzeichen
        End Get
    End Property

    Public ReadOnly Property EQUNR() As String
        Get
            Return m_strEQUNR
        End Get
    End Property

    Public ReadOnly Property BriefNr() As String
        Get
            Return m_strBriefNr
        End Get
    End Property

    Public ReadOnly Property Label() As String
        Get
            Return m_strLabel
        End Get
    End Property

    Public ReadOnly Property Modell() As String
        Get
            Return m_strModell
        End Get
    End Property

    Public ReadOnly Property Ersteingang() As Date
        Get
            Return m_datErsteingang
        End Get
    End Property

    Public ReadOnly Property NochmaligerVersand1() As Date
        Get
            Return m_datNochmaligerVersand1
        End Get
    End Property

    Public ReadOnly Property NochmaligerVersand2() As Date
        Get
            Return m_datNochmaligerVersand2
        End Get
    End Property

    Public ReadOnly Property NochmaligerVersand3() As Date
        Get
            Return m_datNochmaligerVersand3
        End Get
    End Property

    Public Property Gesperrt() As Boolean
        Get
            Return m_blnGesperrt
        End Get
        Set(ByVal Value As Boolean)
            m_blnGesperrt = Value
        End Set
    End Property

    Public ReadOnly Property Wiedereingang1() As Date
        Get
            Return m_datWiedereingang1
        End Get
    End Property

    Public ReadOnly Property Wiedereingang2() As Date
        Get
            Return m_datWiedereingang2
        End Get
    End Property

    Public ReadOnly Property Wiedereingang3() As Date
        Get
            Return m_datWiedereingang3
        End Get
    End Property

    Public ReadOnly Property Versand() As Date
        Get
            Return m_datVersand
        End Get
    End Property

    Public ReadOnly Property COCBescheinigungVorhanden() As Boolean
        Get
            Return m_blnCOCBescheinigungVorhanden
        End Get
    End Property

    Public ReadOnly Property COCKZAenderungsdatum() As Date
        Get
            Return m_datCOCKZAenderungsdatum
        End Get
    End Property

    Public WriteOnly Property Faxnummer() As String
        Set(ByVal Value As String)
            m_strFaxnummer = Value
        End Set
    End Property

    Public WriteOnly Property Mailadresse() As String
        Set(ByVal Value As String)
            m_strMailadresse = Value
        End Set
    End Property

    Public WriteOnly Property Zusatz() As Boolean
        Set(ByVal Value As Boolean)
            m_blnZusatz = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strKennzeichen As String,
                    ByVal strVertragsnummer As String, ByVal strFahrgestellnummer As String, ByVal strAppID As String,
                    ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        m_strINKennzeichen = strKennzeichen
        m_strINVertragsnummer = strVertragsnummer
        m_strINFahrgestellnummer = strFahrgestellnummer
    End Sub
    
    Public Overrides Sub Show()
        m_strClassAndMethod = "fin_17.Show"

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Try
            ClearError()

            S.AP.InitExecute("Z_M_Daten_Einz_Report_001", "I_AG, I_CHASSIS_NUM, I_LIZNR, I_TIDNR, I_LICENSE_NUM",
                             m_strCustomer.PadLeft(10, "0"c),
                             m_strINFahrgestellnummer.ToUpper(),
                             m_strINVertragsnummer.ToUpper(),
                             strZB2Nummer.ToUpper(),
                             m_strINKennzeichen.ToUpper())

            With S.AP
                m_strOUTFahrgestellnummer = .GetExportParameter("E_CHASSIS_NUM")
                m_datCOCKZAenderungsdatum = CDate(.GetExportParameter("E_COCDT1"))
                m_strEQUNR = .GetExportParameter("E_EQUNR")
                m_datErsteingang = CDate(.GetExportParameter("E_ERDAT"))
                m_strOUTKontonummer = .GetExportParameter("E_LIZNR")
                m_strOUTKennzeichen = .GetExportParameter("E_LICENSE_NUM")
                m_strBriefNr = .GetExportParameter("E_TIDNR")
                m_datNochmaligerVersand1 = CDate(.GetExportParameter("E_VERSAND1"))
                m_datNochmaligerVersand2 = CDate(.GetExportParameter("E_VERSAND2"))
                m_datNochmaligerVersand3 = CDate(.GetExportParameter("E_VERSAND3"))
                m_datWiedereingang1 = CDate(.GetExportParameter("E_WIEDER1"))
                m_datWiedereingang2 = CDate(.GetExportParameter("E_WIEDER2"))
                m_datWiedereingang3 = CDate(.GetExportParameter("E_WIEDER3"))

                If .GetExportParameter("E_ZZAKTSPERRE") = "X" Then
                    m_blnGesperrt = True
                Else
                    m_blnGesperrt = False
                End If

                If .GetExportParameter("E_ZZCOCKZ") = "X" Then
                    m_blnCOCBescheinigungVorhanden = True
                Else
                    m_blnCOCBescheinigungVorhanden = False
                End If

                m_strLabel = .GetExportParameter("E_ZZLABEL")
                m_strModell = .GetExportParameter("E_ZZMODID")
                m_datVersand = CDate(.GetExportParameter("E_ZZTMPDT"))

            End With

            WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", LICENSE_NUM=" & m_strINKennzeichen.ToUpper & ", LIZNR=" & m_strINVertragsnummer.ToUpper &
                          ", CHASSIS_NUM=" & m_strINFahrgestellnummer.ToUpper, m_tblResult)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    RaiseError("-8711", "Keine Eingabedaten vorhanden")
                Case "NO_AKTIV"
                    RaiseError("-8712", "Equipment ist inaktiv")
                Case "NO_SELEK"
                    RaiseError("-8713", "Keine Selektionsbedingungen")
                Case Else
                    RaiseError("-9999", ex.Message)
            End Select
            WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", LICENSE_NUM=" & m_strINKennzeichen.ToUpper & ", LIZNR=" & m_strINVertragsnummer.ToUpper &
                          ", CHASSIS_NUM=" & m_strINFahrgestellnummer.ToUpper & " , " & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub
  
    Public Overrides Sub Change()

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        m_intIDSAP = -1

        Try
            ClearError()

            Dim strZusatz As String = ""
            If m_blnZusatz Then strZusatz = "X"

            S.AP.InitExecute("Z_M_Daten_Anf_Change_001", "I_AG, FAXNR, I_CHASSIS_NUM, MAIL, ZUSATZ",
                             m_strCustomer.PadLeft(10, "0"c), m_strFaxnummer, m_strOUTFahrgestellnummer, m_strMailadresse, strZusatz)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_AUFTRAG"
                    RaiseError("-8701", "Kein Auftrag angelegt")
                Case "NO_ZDADVERSAND"
                    RaiseError("-8702", "Keine Einträge für die Versanddatei erstellt")
                Case "NO_UPDATE_EQUI"
                    RaiseError("-8703", "Fehler bei der Datenspeicherung (EQUI-UPDATE)")
                Case "NO_DATA"
                    RaiseError("-8704", "Keine Equipmentdaten gefunden")
                Case Else
                    RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End Select
        End Try
    End Sub
 
#End Region

End Class

' ************************************************
' $History: fin_17.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 24.06.09   Time: 16:00
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 finalisierung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 22.06.09   Time: 11:33
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITa 2918 nachbesserungen Z_M_SAVEABWVERSGRUND, Z_M_ABWEICH_ABRUFGRUND
' 
' *****************  Version 3  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 15:49
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA - 2918 .Net Connector Umstellung
' 
' Bapis:
' Z_M_Brief_Ohne_Daten
' Z_M_Daten_Einz_Report_001
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
' *****************  Version 9  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 12.02.08   Time: 13:23
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' akf änderungen
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 10.01.08   Time: 16:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1477 Torso
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 10.01.08   Time: 15:52
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 10.01.08   Time: 14:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' .......
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 10.01.08   Time: 14:45
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 10.01.08   Time: 12:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1482 Torso
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 10.01.08   Time: 10:40
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1482/Change49 hinzugefügt
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 10.01.08   Time: 10:37
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' ************************************************
