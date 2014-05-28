Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class DCB_000
    REM § Status-Report, Kunde: Daimler Chrysler Bank, BAPI: Z_M_BRIEFBESTAND_001,
    REM § Ausgabetabelle per Zuordnung in Web-DB.
    'Code: DCB_000 

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "DCB_000.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1

            Try

                S.AP.InitExecute("Z_M_BRIEFBESTAND_001", "I_KUNNR", Right("0000000000" & m_objUser.Customer.KUNNR, 10))

                'Dim m_strKUNNR As String
                'm_strKUNNR = Right("0000000000" & m_objUser.Customer.KUNNR, 10)


                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEFBESTAND_001", m_objApp, m_objUser, Page)

                'myProxy.setImportParameter("I_KUNNR", m_strKUNNR)

                'myProxy.callBapi()


                Dim tblTemp As DataTable = S.AP.getExportTable("GT_DATEN") 'myProxy.getExportTable("GT_DATEN")

                CreateOutPut(tblTemp, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: DCB_000.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 13:56
' Updated in $/CKAG/Applications/appdcb/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 8:39
' Updated in $/CKAG/Applications/appdcb/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:52
' Created in $/CKAG/Applications/appdcb/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:50
' Updated in $/CKG/Applications/AppDCB/AppDCBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 9:53
' Updated in $/CKG/Applications/AppDCB/AppDCBWeb/Lib
' 
' ************************************************
