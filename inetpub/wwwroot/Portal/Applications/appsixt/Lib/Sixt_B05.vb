Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class Sixt_B05
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_ABM_CARPORT_OHNE_AUFTRAG,

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
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

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Sixt_B05.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                Dim strAnzahl As String = ""

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_CARPORT_OHNE_AUFTRAG", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_ABM_CARPORT_OHNE_AUFTRAG", "KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'strAnzahl = myProxy.getExportParameter("ANZAHL")

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("AUSGABE")

                strAnzahl = S.AP.GetExportParameter("ANZAHL")

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                m_tblResultPDIs = New DataTable()
                m_tblResultPDIs.Columns.Add("PDI Nummer", System.Type.GetType("System.String"))
                m_tblResultPDIs.Columns.Add("PDI Name", System.Type.GetType("System.String"))
                m_tblResultPDIs.Columns.Add("Fahrzeuge", System.Type.GetType("System.Int32"))
                m_tblResultPDIs.Columns.Add("AppID", System.Type.GetType("System.String"))

                m_tblResult = New DataTable()
                m_tblResult.Columns.Add("PDI Nummer", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("PDI Name", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("Demontagedatum", System.Type.GetType("System.DateTime"))
                m_tblResult.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))

                Dim vwTemp As DataView = tblTemp2.DefaultView
                Dim i As Int32

                vwTemp.Sort = "DADPDI"
                Dim strPDITest As String = "Startwert"
                Dim strPDIName As String = ""
                Dim intFahrzeuge As Int32 = 0
                For i = 0 To vwTemp.Count - 1

                    If Not CStr(vwTemp.Item(i)("DADPDI")) = strPDITest Then
                        If intFahrzeuge > 0 Then
                            Dim rowTemp As DataRow = m_tblResultPDIs.NewRow
                            rowTemp("PDI Nummer") = strPDITest
                            rowTemp("PDI Name") = strPDIName
                            rowTemp("Fahrzeuge") = intFahrzeuge
                            rowTemp("AppID") = strAppID
                            m_tblResultPDIs.Rows.Add(rowTemp)
                        End If
                        strPDITest = CStr(vwTemp.Item(i)("DADPDI"))
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
                For i = 0 To vwTemp.Count - 1
                    Dim rowTemp As DataRow = m_tblResult.NewRow
                    rowTemp("PDI Nummer") = CStr(vwTemp.Item(i)("DADPDI"))
                    rowTemp("PDI Name") = CStr(vwTemp.Item(i)("DADPDI_NAME1"))
                    Dim datTemp As DateTime = CDate(vwTemp.Item(i)("REPLA_DATE"))
                    If datTemp > CDate("01.01.1900") Then
                        rowTemp("Demontagedatum") = datTemp
                    End If
                    If CStr(vwTemp.Item(i)("ZZFAHRG")) = "Fahrzeug nicht vorha" Then
                        rowTemp("Fahrgestellnummer") = "Brief nicht vorhanden"
                    Else
                        rowTemp("Fahrgestellnummer") = CStr(vwTemp.Item(i)("ZZFAHRG"))
                    End If
                    rowTemp("Kennzeichen") = CStr(vwTemp.Item(i)("ZZKENN"))
                    m_tblResult.Rows.Add(rowTemp)
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Sixt_B05.vb $
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
' *****************  Version 5  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
