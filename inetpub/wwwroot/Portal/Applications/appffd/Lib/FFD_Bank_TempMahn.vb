Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class FFD_Bank_TempMahn
    REM § Mahn-Report, Kunde: FFD, BAPI: Z_M_Temporaer_Zu_Mahnen,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase  ' FFD_Bank_Datenimport

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FFD_Bank_TempMahn.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = m_objUser.Reference
            Dim strKNRZE As String = m_strFiliale
            Dim strKONZS As String = "0000324562"

            Try

                Dim strVKORG As String = "1510"


                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Temporaer_Zu_Mahnen", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", strKUNNR)
                'myProxy.setImportParameter("I_KNRZE", strKNRZE)
                'myProxy.setImportParameter("I_KONZS", strKONZS)
                'myProxy.setImportParameter("I_VKORG", strVKORG)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Temporaer_Zu_Mahnen", "I_KUNNR,I_KNRZE,I_KONZS,I_VKORG", strKUNNR, strKNRZE, strKONZS, strVKORG)

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                If tblTemp2.Rows.Count = 0 Then m_intStatus = -1


                Dim rowTemp As DataRow
                tblTemp2.Columns("ZZKKBER").MaxLength = 50

                For Each rowTemp In tblTemp2.Rows
                    Select Case CStr(rowTemp("ZZKKBER"))
                        Case "0001"
                            rowTemp("ZZKKBER") = "Standard temporär"
                        Case "0002"
                            rowTemp("ZZKKBER") = "Standard endgültig"
                    End Select
                Next

                tblTemp2.AcceptChanges()

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KNRZE=" & strKNRZE & ", KONZS=" & strKONZS & ", KUNNR=" & strKUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                        m_intStatus = -1
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
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
' $History: FFD_Bank_TempMahn.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
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
' *****************  Version 6  *****************
' User: Jungj        Date: 14.12.07   Time: 10:35
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Veränderung an Report33/FFD_BANK_TEMPMAHN.vb für aktuallisierten
' Algorithmus der Startmethoden in der Selection ASPX
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' ************************************************
