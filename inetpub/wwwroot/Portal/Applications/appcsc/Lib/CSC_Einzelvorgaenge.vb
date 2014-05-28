Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class CSC_Einzelvorgaenge
    REM § Lese-/Schreibfunktion, Kunde: CSC, 
    REM § Show - BAPI: Zz_Csc_Daten_Einz_Report_001,
    REM § Change - BAPI: Zz_Csc_Daten_Einz_Change_001.

    Inherits BankBase ' FDD_Bank_Base

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
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strKennzeichen As String, ByVal strVertragsnummer As String, ByVal strFahrgestellnummer As String, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_strINKennzeichen = strKennzeichen
        m_strINVertragsnummer = strVertragsnummer
        m_strINFahrgestellnummer = strFahrgestellnummer
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "CSC_Einzelvorgaenge.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)
            Dim strCOCBescheinigungVorhanden As String = ""
            Dim strCOCKZAenderungsdatum As String = ""
            Dim strSperre As String = ""
            Dim strErsteingang As String = ""
            Dim strVersand As String = ""
            Dim strWiedereingang1 As String = ""
            Dim strWiedereingang2 As String = ""
            Dim strWiedereingang3 As String = ""
            Dim strNochmaligerVersand1 As String = ""
            Dim strNochmaligerVersand2 As String = ""
            Dim strNochmaligerVersand3 As String = ""

            Try
                S.AP.Init("Zz_Csc_Daten_Einz_Report_001")

                S.AP.SetImportParameter("I_KUNNR", strKUNNR)
                S.AP.SetImportParameter("I_LICENSE_NUM", m_strINKennzeichen.ToUpper)
                S.AP.SetImportParameter("I_LIZNR", m_strINVertragsnummer.ToUpper)
                S.AP.SetImportParameter("I_CHASSIS_NUM", m_strINFahrgestellnummer.ToUpper)

                S.AP.Execute()

                m_strEQUNR = S.AP.GetExportParameter("E_EQUNR")
                m_strOUTKontonummer = S.AP.GetExportParameter("E_LIZNR")
                m_strOUTFahrgestellnummer = S.AP.GetExportParameter("E_CHASSIS_NUM")
                m_strOUTKennzeichen = S.AP.GetExportParameter("E_LICENSE_NUM")
                m_strBriefNr = S.AP.GetExportParameter("E_TIDNR")
                m_strLabel = S.AP.GetExportParameter("E_ZZLABEL")
                m_strModell = S.AP.GetExportParameter("E_ZZMODID")
                strErsteingang = S.AP.GetExportParameter("E_ERDAT")
                strVersand = S.AP.GetExportParameter("E_ZZTMPDT")
                strSperre = S.AP.GetExportParameter("E_ZZAKTSPERRE")
                strWiedereingang1 = S.AP.GetExportParameter("E_WIEDER1")
                strNochmaligerVersand1 = S.AP.GetExportParameter("E_VERSAND1")
                strWiedereingang2 = S.AP.GetExportParameter("E_WIEDER2")
                strNochmaligerVersand2 = S.AP.GetExportParameter("E_VERSAND2")
                strWiedereingang3 = S.AP.GetExportParameter("E_WIEDER3")
                strNochmaligerVersand3 = S.AP.GetExportParameter("E_VERSAND3")
                strCOCBescheinigungVorhanden = S.AP.GetExportParameter("E_ZZCOCKZ")
                strCOCKZAenderungsdatum = S.AP.GetExportParameter("E_COCDT1")

                If strSperre = "X" Then
                    m_blnGesperrt = True
                Else
                    m_blnGesperrt = False
                End If

                If strCOCBescheinigungVorhanden = "X" Then
                    m_blnCOCBescheinigungVorhanden = True
                Else
                    m_blnCOCBescheinigungVorhanden = False
                End If

                If IsDate(strErsteingang) Then m_datErsteingang = CDate(strErsteingang)
                If IsDate(strVersand) Then m_datVersand = CDate(strVersand)
                If IsDate(strCOCKZAenderungsdatum) Then m_datCOCKZAenderungsdatum = CDate(strCOCKZAenderungsdatum)
                If IsDate(strWiedereingang1) Then m_datWiedereingang1 = CDate(strWiedereingang1)
                If IsDate(strWiedereingang2) Then m_datWiedereingang2 = CDate(strWiedereingang2)
                If IsDate(strWiedereingang3) Then m_datWiedereingang3 = CDate(strWiedereingang3)
                If IsDate(strNochmaligerVersand1) Then m_datNochmaligerVersand1 = CDate(strNochmaligerVersand1)
                If IsDate(strNochmaligerVersand2) Then m_datNochmaligerVersand2 = CDate(strNochmaligerVersand2)
                If IsDate(strNochmaligerVersand3) Then m_datNochmaligerVersand3 = CDate(strNochmaligerVersand3)

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -8721
                        m_strMessage = "Keine Eingabedaten vorhanden"
                    Case "NO_AKTIV"
                        m_intStatus = -8722
                        m_strMessage = "Equipment ist inaktiv"
                    Case "NO_SELEK"
                        m_intStatus = -8723
                        m_strMessage = "Keine Selektionsbedingungen"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal AlteKontonummer As String)
        m_strClassAndMethod = "CSC_Einzelvorgaenge.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)
            Dim strSperre As String = ""
            If m_blnGesperrt Then strSperre = "X"

            Try
                S.AP.Init("Zz_Csc_Daten_Einz_Change_001")

                S.AP.SetImportParameter("I_KUNNR", strKUNNR)
                S.AP.SetImportParameter("I_LIZNR_ALT", AlteKontonummer)
                S.AP.SetImportParameter("I_LIZNR_NEU", m_strOUTKontonummer)
                S.AP.SetImportParameter("I_ZZAKTSPERRE", strSperre)

                S.AP.Execute()

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "VERTRAG_FEHLT"
                        m_intStatus = -4510
                        m_strMessage = "Neue Kontonummer fehlt"
                    Case "VERTRAG_VORHANDEN"
                        m_intStatus = -4511
                        m_strMessage = "Neue Kontonummer vorhanden"
                    Case "VERTRAG_UNGUELTIG"
                        m_intStatus = -4512
                        m_strMessage = "Neue Kontonummer ungültig"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()

    End Sub

#End Region

End Class

' ************************************************
' $History: CSC_Einzelvorgaenge.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 31.08.10   Time: 16:43
' Updated in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 18.03.10   Time: 9:58
' Updated in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 11.03.10   Time: 13:34
' Updated in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.11.08   Time: 11:51
' Updated in $/CKAG/Applications/appcsc/Lib
' finally block hinzugefügt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 2.05.08    Time: 10:37
' Updated in $/CKAG/Applications/appcsc/Lib
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:46
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' 
' ************************************************
