Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class FFD_Bank_TempVers
    REM § Status-Report, Kunde: FFD, BAPI: Z_M_Status,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FFD_Bank_TempVers.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                Dim strKUNNR As String = Nothing

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Status", m_objApp, m_objUser, Page)

                'myProxy.setImportParameter("I_KUNNR", strKUNNR)
                'myProxy.setImportParameter("I_KNRZE", m_strFiliale)
                'myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_CHASSIS_NUM", "")
                'myProxy.setImportParameter("I_LIZNR", "")
                'myProxy.setImportParameter("I_ZZREFERENZ1", "")
                'myProxy.setImportParameter("I_VKORG", "1510")
                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Status", "I_KUNNR,I_KNRZE,I_KONZS,I_CHASSIS_NUM,I_LIZNR,I_ZZREFERENZ1,I_VKORG",
                                 strKUNNR, m_strFiliale, Right("0000000000" & m_objUser.KUNNR, 10), "", "", "", "1510")

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KNRZE=" & m_strFiliale & ", KONZS=" & m_objUser.KUNNR & ", KUNNR=, CHASSIS_NUM=, LIZNR=, ZZREFERENZ1= ", m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -9999
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_HAENDLER"
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KNRZE=" & m_strFiliale & ", KONZS=" & m_objUser.KUNNR & ", KUNNR=, CHASSIS_NUM=, LIZNR=, ZZREFERENZ1= , " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
   
#End Region
End Class


' ************************************************
' $History: FFD_Bank_TempVers.vb $
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
