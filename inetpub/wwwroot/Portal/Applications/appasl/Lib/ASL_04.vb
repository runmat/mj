Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class ASL_04
    REM § Status-Report, Kunde: ALD, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_lvnr As String
    Private m_kennzeichen As String
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub
    
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strLvnr As String, ByVal strKennz As String)
        m_strClassAndMethod = "ASL_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1

            Try

                'Dim m_strKUNNR As String
                'm_strKUNNR = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                S.AP.InitExecute("Z_M_ASL_SIS_UEBERNAHME_PS", "I_KUNNR,I_LIZNR,I_LICENSE_NUM", Right("0000000000" & m_objUser.Customer.KUNNR, 10), strLvnr, strKennz)

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ASL_SIS_UEBERNAHME_PS", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", m_strKUNNR)
                'myProxy.setImportParameter("I_LIZNR", strLvnr)
                'myProxy.setImportParameter("I_LICENSE_NUM", strKennz)
                'myProxy.callBapi()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
                Dim row As DataRow

                For Each row In tblTemp2.Rows
                    If CType(row("SIS"), String).Trim <> String.Empty Then
                        row("SIS") = "Sicherungsschein vorhanden."
                    End If
                Next

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "LVNR=" & m_lvnr & ", KENNZ=" & m_kennzeichen & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -1234
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Unbekannter Fehler."
                End Select
                WriteLogEntry(False, "LVNR=" & m_lvnr & ", KENNZ=" & m_kennzeichen & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: ASL_04.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 10:19
' Updated in $/CKAG/Applications/appasl/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 24.04.08   Time: 14:28
' Updated in $/CKAG/Applications/appasl/Lib
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:04
' Created in $/CKAG/Applications/appasl/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 16:21
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 6.03.07    Time: 18:02
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Lib
' 
' ************************************************
