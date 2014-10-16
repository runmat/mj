Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class Sixt_B02
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Briefeohnefzg2 (Fill),
    REM § BAPI: Z_M_Brieflebenslauf (FillHistory),
    REM § Ausgabetabelle per Zuordnung in Web-DB (Fill), Direkte Rückgabe (FillHistory).

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strBriefnummer As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strHaendlerID As String
    Private m_tblHistory As DataTable
    Private m_tblResultModelle As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultModelle() As DataTable
        Get
            Return m_tblResultModelle
        End Get
    End Property

    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal strBriefnummer As String, ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal strFahrgestellnummer As String, ByVal strHaendlerID As String)
        m_strClassAndMethod = "Sixt_B02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEFEOHNEFZG2", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("ZZKUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("ZZBRIEFNR", strBriefnummer)
                'myProxy.setImportParameter("ZZFAHRG", strFahrgestellnummer)
                'myProxy.setImportParameter("ZZHAENDLER", "")
                'If IsDate(datEingangsdatumVon) Then
                '    myProxy.setImportParameter("ZZERDAT_VON", CStr(datEingangsdatumVon))
                'End If
                'If IsDate(datEingangsdatumBis) Then
                '    myProxy.setImportParameter("ZZERDAT_BIS", CStr(datEingangsdatumBis))
                'End If

                'myProxy.callBapi()

                S.AP.Init("Z_M_BRIEFEOHNEFZG2", "ZZKUNNR, ZZBRIEFNR, ZZFAHRG, ZZHAENDLER", Right("0000000000" & m_objUser.KUNNR, 10), strBriefnummer, strFahrgestellnummer, "")

                If IsDate(datEingangsdatumVon) Then
                    S.AP.SetImportParameter("ZZERDAT_VON", CStr(datEingangsdatumVon))
                End If
                If IsDate(datEingangsdatumBis) Then
                    S.AP.SetImportParameter("ZZERDAT_BIS", CStr(datEingangsdatumBis))
                End If

                S.AP.Execute()

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("ITAB")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("ITAB")

                tblTemp2.Columns.Add("Modell", System.Type.GetType("System.String"))
                tblTemp2.Columns.Add("Typdaten", System.Type.GetType("System.String"))
                Dim i As Int32
                For i = 0 To tblTemp2.Rows.Count - 1
                    Dim objZulassungsVorbelegung As Base.Business.ZulassungsVorbelegung
                    objZulassungsVorbelegung = New Base.Business.ZulassungsVorbelegung(CStr(tblTemp2.Rows(i)("ZZFAHRG")), Now, m_objApp.Connectionstring)
                    tblTemp2.Rows(i).AcceptChanges()
                    tblTemp2.Rows(i)("Modell") = objZulassungsVorbelegung.Modell
                    If TypeOf tblTemp2.Rows(i)("EQUNR") Is System.DBNull = False Then
                        tblTemp2.Rows(i)("Typdaten") = "<A href=Change06_3NEU.aspx?EqNr=" & CStr(tblTemp2.Rows(i)("EQUNR")) & " target=_blank>Anzeige</A>"
                    End If
                    tblTemp2.Rows(i).AcceptChanges()
                Next

                CreateOutPut(tblTemp2, strAppID)

                m_tblResultModelle = New DataTable()
                m_tblResultModelle.Columns.Add("Modell", System.Type.GetType("System.String"))
                m_tblResultModelle.Columns.Add("Fahrzeuge", System.Type.GetType("System.Int32"))
                m_tblResultModelle.Columns.Add("AppID", System.Type.GetType("System.String"))

                Dim vwTemp As DataView = tblTemp2.DefaultView

                vwTemp.Sort = "Modell"
                Dim strModellTest As String = "Startwert"
                Dim intFahrzeuge As Int32 = 0
                For i = 0 To vwTemp.Count - 1
                    If Not CStr(vwTemp.Item(i)("Modell")) = strModellTest Then
                        If intFahrzeuge > 0 Then
                            Dim rowTemp As DataRow = m_tblResultModelle.NewRow
                            rowTemp("Modell") = strModellTest
                            rowTemp("Fahrzeuge") = intFahrzeuge
                            rowTemp("AppID") = strAppID
                            m_tblResultModelle.Rows.Add(rowTemp)
                        End If
                        strModellTest = CStr(vwTemp.Item(i)("Modell"))
                        intFahrzeuge = 1
                    Else
                        intFahrzeuge += 1
                    End If
                Next
                If intFahrzeuge > 0 Then
                    Dim rowTemp As DataRow = m_tblResultModelle.NewRow
                    rowTemp("Modell") = strModellTest
                    rowTemp("Fahrzeuge") = intFahrzeuge
                    m_tblResultModelle.Rows.Add(rowTemp)
                End If
                WriteLogEntry(True, "ZZERDAT_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZERDAT_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZHAENDLER=, ZFAHRG=" & strFahrgestellnummer, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -3333
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_NO_KRIT"
                        m_strMessage = "Keine Suchkriterien eingegeben."
                    Case "ERR_INV_ERDAT"
                        m_strMessage = "Geben Sie bitte ein gültiges Eingangsdatum ein!"
                    Case "ERR_INV_BRIEFNR"
                        m_strMessage = "Ungültige Brief-Nr."
                    Case "ERR_INV_KUNNR"
                        m_strMessage = "Ungültige Kundennummer."
                    Case "ERR_INV_FAHRG"
                        m_strMessage = "Ungültige Fahrgestellnummer."
                    Case "ERR_INV_HAENDLER"
                        m_strMessage = "Ungültiger Händler."
                    Case "ERR_NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "ZZERDAT_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZERDAT_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZHAENDLER=, ZFAHRG=" & strFahrgestellnummer & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String)
        m_strClassAndMethod = "Sixt_B02.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEFLEBENSLAUF", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_VKORG", "1510")
                'myProxy.setImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer))
                'myProxy.setImportParameter("I_ZZKENN", UCase(strAmtlKennzeichen))
                'myProxy.setImportParameter("I_ZZBRIEF", UCase(strBriefnummer))
                'myProxy.setImportParameter("I_ZZREF1", UCase(strOrdernummer))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_BRIEFLEBENSLAUF", "I_KUNNR, I_VKORG, I_ZZFAHRG, I_ZZKENN, I_ZZBRIEF, I_ZZREF1", Right("0000000000" & m_objUser.KUNNR, 10),
                                 "1510", UCase(strFahrgestellnummer), UCase(strAmtlKennzeichen), UCase(strBriefnummer), UCase(strOrdernummer))

                'm_tblHistory = myProxy.getExportTable("GT_WEB")
                m_tblHistory = S.AP.GetExportTable("GT_WEB")

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
' $History: Sixt_B02.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
