Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class FFD_Bank_FehlCOC
    REM § Status-Report, Kunde: FFD, BAPI: Z_M_Fehlende_Coc_Nach_Wiedeing,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFiliale As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFiliale = strFiliale
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FFD_Bank_FehlCOC.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1
            Dim strKNRZE As String = m_strFiliale
            Dim strKONZS As String = "0000324562"
            Dim strKUNNR As String = ""

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Fehlende_Coc_Nach_Wiedeing", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", strKUNNR)
                'myProxy.setImportParameter("I_KONZS", strKONZS)
                'myProxy.setImportParameter("I_KNRZE", strKNRZE)
                'myProxy.setImportParameter("I_VKORG", "1510")


                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Fehlende_Coc_Nach_Wiedeing", "I_KUNNR,I_KONZS,I_KNRZE,I_VKORG",
                                                                    strKUNNR, strKONZS, strKNRZE, "1510")

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KNRZE=" & strKNRZE & ", KONZS=" & strKONZS & ", KUNNR=" & strKUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Daten zur Anzeige gefunden"
                    Case "NO_HAENDLER"
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KNRZE=" & strKNRZE & ", KONZS=" & strKONZS & ", KUNNR=" & strKUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: FFD_Bank_FehlCOC.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' ************************************************
