Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class Sixt_B03
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Fzgeohnebrief,
    REM § Ausgabetabelle per individueller Erzeugung (Texte aus zweiter Struktur).

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strPDI As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strModell As String
    Private m_tblResultPDIs As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultPDIs() As DataTable
        Get
            Return m_tblResultPDIs
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, _
                                ByVal strPDI As String, ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, _
                                ByVal strFahrgestellnummer As String, ByVal strModell As String)
        m_strClassAndMethod = "Sixt_B03.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim strDatTempVon As String
                If IsDate(datEingangsdatumVon) Then
                    strDatTempVon = CDate(datEingangsdatumVon).ToString
                Else
                    strDatTempVon = ""
                End If
                Dim strDatTempBis As String
                If IsDate(datEingangsdatumBis) Then
                    strDatTempBis = CDate(datEingangsdatumBis).ToString
                Else
                    strDatTempBis = ""
                End If

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_FZGEOHNEBRIEF", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("ZZKUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("ZZKUNPDI", strPDI)
                'myProxy.setImportParameter("ZZFAHRG", strFahrgestellnummer)
                'If IsDate(datEingangsdatumVon) Then
                '    myProxy.setImportParameter("ZZDAT_EIN_VON", strDatTempVon)
                'End If
                'If IsDate(datEingangsdatumBis) Then
                '    myProxy.setImportParameter("ZZDAT_EIN_BIS", strDatTempBis)
                'End If
                'myProxy.setImportParameter("ZZBEZEI", strModell)

                'myProxy.callBapi()

                S.AP.Init("Z_M_FZGEOHNEBRIEF", "ZZKUNNR, ZZKUNPDI, ZZFAHRG, ZZBEZEI", Right("0000000000" & m_objUser.KUNNR, 10), strPDI, strFahrgestellnummer, strModell)

                If IsDate(datEingangsdatumVon) Then
                    S.AP.SetImportParameter("ZZDAT_EIN_VON", strDatTempVon)
                End If
                If IsDate(datEingangsdatumBis) Then
                    S.AP.SetImportParameter("ZZDAT_EIN_BIS", strDatTempBis)
                End If

                S.AP.Execute()

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("ITAB")
                'Dim tblTemp3 As DataTable = myProxy.getExportTable("ITAB_BEMERKUNG")

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("ITAB")
                Dim tblTemp3 As DataTable = S.AP.GetExportTable("ITAB_BEMERKUNG")

                m_tblResultPDIs = New DataTable()
                m_tblResultPDIs.Columns.Add("PDI Nummer", System.Type.GetType("System.String"))
                m_tblResultPDIs.Columns.Add("PDI Name", System.Type.GetType("System.String"))
                m_tblResultPDIs.Columns.Add("Fahrzeuge", System.Type.GetType("System.Int32"))
                m_tblResultPDIs.Columns.Add("AppID", System.Type.GetType("System.String"))

                m_tblResult = New DataTable()
                m_tblResult.Columns.Add("PDI Nummer", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("PDI Name", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("Eingangsdatum", System.Type.GetType("System.DateTime"))
                '§§§ JVE 29.09.2006: Bereit-Datum einfügen
                m_tblResult.Columns.Add("Bereitdatum", System.Type.GetType("System.DateTime"))
                '-----------------------------------------
                m_tblResult.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("Modell", System.Type.GetType("System.String"))
                'm_tblResult.Columns.Add("Bemerkung", System.Type.GetType("System.String"))

                Dim vwTemp As DataView = tblTemp2.DefaultView
                Dim i As Int32

                vwTemp.Sort = "KUNPDI"
                Dim strPDITest As String = "Startwert"
                Dim strPDIName As String = ""
                Dim intFahrzeuge As Int32 = 0
                For i = 0 To vwTemp.Count - 1
                    If Not CStr(vwTemp.Item(i)("KUNPDI")) = strPDITest Then
                        If intFahrzeuge > 0 Then
                            Dim rowTemp As DataRow = m_tblResultPDIs.NewRow
                            rowTemp("PDI Nummer") = strPDITest
                            rowTemp("PDI Name") = strPDIName
                            rowTemp("Fahrzeuge") = intFahrzeuge
                            rowTemp("AppID") = strAppID
                            m_tblResultPDIs.Rows.Add(rowTemp)
                        End If
                        strPDITest = CStr(vwTemp.Item(i)("KUNPDI"))
                        strPDIName = CStr(vwTemp.Item(i)("DADPDI_NAME1"))
                        intFahrzeuge = 1
                    Else
                        intFahrzeuge += 1
                    End If
                Next
                If intFahrzeuge > 0 Then
                    Dim rowTemp As DataRow = m_tblResultPDIs.NewRow
                    rowTemp("PDI Nummer") = strPDITest
                    rowTemp("PDI Name") = strPDIName
                    rowTemp("Fahrzeuge") = intFahrzeuge
                    m_tblResultPDIs.Rows.Add(rowTemp)
                End If

                vwTemp.Sort = "ZZDAT_EIN"

                Dim datTemp As DateTime

                For i = 0 To vwTemp.Count - 1
                    Dim rowTemp As DataRow = m_tblResult.NewRow
                    rowTemp("PDI Nummer") = CStr(vwTemp.Item(i)("KUNPDI"))
                    rowTemp("PDI Name") = CStr(vwTemp.Item(i)("DADPDI_NAME1"))
                    datTemp = CDate(vwTemp.Item(i)("ZZDAT_EIN"))
                    If datTemp > CDate("01.01.1900") Then
                        rowTemp("Eingangsdatum") = datTemp
                    End If
                    '§§§ JVE 29.09.2006: Bereit-Datum einfügen
                    datTemp = CDate(vwTemp.Item(i)("ZZDAT_BER"))
                    If datTemp > CDate("01.01.1900") Then
                        rowTemp("Bereitdatum") = datTemp
                    End If
                    '-----------------------------------------
                    rowTemp("Fahrgestellnummer") = CStr(vwTemp.Item(i)("ZZFAHRG"))
                    rowTemp("Modell") = CStr(vwTemp.Item(i)("ZZBEZEI"))
                    Dim strTemp As String = ""
                    Dim vwTemp2 As DataView = tblTemp3.DefaultView
                    vwTemp2.RowFilter = "QMNUM = '" & CStr(vwTemp.Item(i)("QMNUM")) & "'"
                    Dim j As Int32
                    For j = 0 To vwTemp2.Count - 1
                        strTemp &= "<br>" & Replace(CStr(vwTemp2.Item(j)("TDLINE")), "==", "")
                    Next
                    If strTemp.Length > 4 Then
                        rowTemp("Bemerkung") = Right(strTemp, strTemp.Length - 4)
                    End If
                    m_tblResult.Rows.Add(rowTemp)
                Next

                WriteLogEntry(True, "ZZDAT_EIN_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZDAT_EIN_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZKUNPDI=" & strPDI & ", ZZFAHRG=" & strFahrgestellnummer & ", ZZBEZEI=" & strModell, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -4444
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_INV_KUNNR"
                        m_strMessage = "Ungültige Kundennummer."
                    Case "ERR_NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case "ERR_NO_KRIT"
                        m_strMessage = "Keine Suchkriterien eingegeben."
                    Case "ERR_INV_PDI"
                        m_strMessage = "Ungültiger PDI."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "ZZDAT_EIN_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZDAT_EIN_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZKUNPDI=" & strPDI & ", ZZFAHRG=" & strFahrgestellnummer & ", ZZBEZEI=" & strModell & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Sixt_B03.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
