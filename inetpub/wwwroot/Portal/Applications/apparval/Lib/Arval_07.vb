Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class Arval_07
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Briefeohnefzg2 (Fill),
    REM § BAPI: Z_M_Brieflebenslauf (FillHistory),
    REM § Ausgabetabelle per Zuordnung in Web-DB (Fill), Direkte Rückgabe (FillHistory).

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    'Private m_strBriefnummer As String
    'Private m_datEingangsdatumVon As DateTime
    'Private m_datEingangsdatumBis As DateTime
    'Private m_strFahrgestellnummer As String
    'Private m_strHaendlerID As String
    Private m_tblHistory As DataTable
    Private m_tblResultModelle As DataTable
    Private m_tblQMEL_DATENTable As DataTable
    Private m_tblQMMIDATENTable As DataTable
    Private mGT_EQUI As DataTable
#End Region

#Region " Properties"
    'Public ReadOnly Property ResultModelle() As DataTable
    '    Get
    '        Return m_tblResultModelle
    '    End Get
    'End Property

    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property

    Public ReadOnly Property QMMIDATENTable() As DataTable
        Get
            Return m_tblQMMIDATENTable
        End Get
    End Property

    Public ReadOnly Property QMEL_DATENTable() As DataTable
        Get
            Return m_tblQMEL_DATENTable
        End Get
    End Property
    
    'Public ReadOnly Property diverseFahrzeuge() As DataTable
    '    Get
    '        Return mGT_EQUI
    '    End Get
    'End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    'Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strBriefnummer As String,
    '                          ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal strFahrgestellnummer As String,
    '                          ByVal strHaendlerID As String)
    '    m_strClassAndMethod = "Arval_07.FILL"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID

    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Try
    '            S.AP.Init("Z_M_Zuldokumente_Evb")
    '            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Zuldokumente_Evb", m_objApp, m_objUser, Page)

    '            S.AP.SetImportParameter("ZZKUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
    '            S.AP.SetImportParameter("ZZBRIEFNR", strBriefnummer)
    '            S.AP.SetImportParameter("ZZFAHRG", strFahrgestellnummer)
    '            S.AP.SetImportParameter("ZZHAENDLER", "")
    '            If IsDate(datEingangsdatumVon) Then
    '                S.AP.SetImportParameter("ZZERDAT_VON", CStr(datEingangsdatumVon))
    '            End If
    '            If IsDate(datEingangsdatumBis) Then
    '                S.AP.SetImportParameter("ZZERDAT_BIS", CStr(datEingangsdatumBis))
    '            End If

    '            S.AP.Execute() 'myProxy.callBapi()

    '            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

    '            tblTemp2.Columns.Add("Modell", System.Type.GetType("System.String"))
    '            tblTemp2.Columns.Add("VM", System.Type.GetType("System.String"))
    '            Dim i As Int32
    '            For i = 0 To tblTemp2.Rows.Count - 1
    '                Dim objZulassungsVorbelegung As ZulassungsVorbelegung
    '                objZulassungsVorbelegung = New ZulassungsVorbelegung(CStr(tblTemp2.Rows(i)("ZZFAHRG")), Now, m_objApp.Connectionstring)
    '                tblTemp2.Rows(i).AcceptChanges()
    '                tblTemp2.Rows(i)("Modell") = objZulassungsVorbelegung.Modell
    '                If objZulassungsVorbelegung.CheckVMBoolean(CStr(tblTemp2.Rows(i)("ZZREF1")), CStr(tblTemp2.Rows(i)("FLEET_VIN"))) Then
    '                    tblTemp2.Rows(i)("VM") = "Ja"
    '                Else
    '                    tblTemp2.Rows(i)("VM") = "Nein"
    '                End If
    '                tblTemp2.Rows(i).AcceptChanges()
    '            Next

    '            CreateOutPut(tblTemp2, strAppID)

    '            m_tblResultModelle = New DataTable()
    '            m_tblResultModelle.Columns.Add("Modell", System.Type.GetType("System.String"))
    '            m_tblResultModelle.Columns.Add("Fahrzeuge", System.Type.GetType("System.Int32"))
    '            m_tblResultModelle.Columns.Add("AppID", System.Type.GetType("System.String"))

    '            Dim vwTemp As DataView = tblTemp2.DefaultView

    '            vwTemp.Sort = "Modell"
    '            Dim strModellTest As String = "Startwert"
    '            Dim intFahrzeuge As Int32 = 0
    '            For i = 0 To vwTemp.Count - 1
    '                If Not CStr(vwTemp.Item(i)("Modell")) = strModellTest Then
    '                    If intFahrzeuge > 0 Then
    '                        Dim rowTemp As DataRow = m_tblResultModelle.NewRow
    '                        rowTemp("Modell") = strModellTest
    '                        rowTemp("Fahrzeuge") = intFahrzeuge
    '                        rowTemp("AppID") = strAppID
    '                        m_tblResultModelle.Rows.Add(rowTemp)
    '                    End If
    '                    strModellTest = CStr(vwTemp.Item(i)("Modell"))
    '                    intFahrzeuge = 1
    '                Else
    '                    intFahrzeuge += 1
    '                End If
    '            Next
    '            If intFahrzeuge > 0 Then
    '                Dim rowTemp As DataRow = m_tblResultModelle.NewRow
    '                rowTemp("Modell") = strModellTest
    '                rowTemp("Fahrzeuge") = intFahrzeuge
    '                m_tblResultModelle.Rows.Add(rowTemp)
    '            End If
    '            WriteLogEntry(True, "ZZERDAT_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZERDAT_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZHAENDLER=, ZFAHRG=" & strFahrgestellnummer, m_tblResult, False)
    '        Catch ex As Exception
    '            m_intStatus = -3333
    '            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
    '                Case "ERR_NO_KRIT"
    '                    m_strMessage = "Keine Suchkriterien eingegeben."
    '                Case "ERR_INV_ERDAT"
    '                    m_strMessage = "Geben Sie bitte ein gültiges Eingangsdatum ein!"
    '                Case "ERR_INV_BRIEFNR"
    '                    m_strMessage = "Ungültige Brief-Nr."
    '                Case "ERR_INV_KUNNR"
    '                    m_strMessage = "Ungültige Kundennummer."
    '                Case "ERR_INV_FAHRG"
    '                    m_strMessage = "Ungültige Fahrgestellnummer."
    '                Case "ERR_INV_HAENDLER"
    '                    m_strMessage = "Ungültiger Händler."
    '                Case "ERR_NO_DATA"
    '                    m_strMessage = "Keine Daten gefunden."
    '                Case Else
    '                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '            End Select
    '            WriteLogEntry(False, "ZZERDAT_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZERDAT_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZHAENDLER=, ZFAHRG=" & strFahrgestellnummer & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
    '        Finally
    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String,
                           ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String)
        m_strClassAndMethod = "Arval_07.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

           
            Try
                S.AP.Init("Z_M_Brieflebenslauf_Arval")

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Brieflebenslauf_Arval", m_objApp, m_objUser, Page)

                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_VKORG", "1510")
                S.AP.SetImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer))
                S.AP.SetImportParameter("I_ZZKENN", UCase(strAmtlKennzeichen))
                S.AP.SetImportParameter("I_ZZBRIEF", UCase(strBriefnummer))
                S.AP.SetImportParameter("I_ZZREF1", UCase(strOrdernummer))

                S.AP.Execute() 'myProxy.callBapi()

                m_tblHistory = S.AP.GetExportTable("GT_WEB")
                m_tblQMEL_DATENTable = S.AP.GetExportTable("GT_QMEL")
                m_tblQMMIDATENTable = S.AP.GetExportTable("GT_QMMA")

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen), m_tblHistory, False)
            Catch ex As Exception
                m_intStatus = -2222
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = ex.Message
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: Arval_07.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 5.01.10    Time: 13:50
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.03.08   Time: 10:15
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' ************************************************
