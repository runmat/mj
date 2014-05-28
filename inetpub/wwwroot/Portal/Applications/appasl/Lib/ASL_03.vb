Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class ASL_03
    REM § Status-Report, Kunde: FMS, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    'Private m_datAbmeldedatumVon As DateTime
    'Private m_datAbmeldedatumBis As DateTime
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ASL_03.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1

            Try

                S.AP.InitExecute("Z_M_Asl_Sis_Generalsich", "I_KUNNR", Right("0000000000" & m_objUser.Customer.KUNNR, 10))

                'Dim m_strKUNNR As String
                'm_strKUNNR = Right("0000000000" & m_objUser.Customer.KUNNR, 10)


                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Asl_Sis_Generalsich", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", m_strKUNNR)

                'myProxy.callBapi()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
                CreateOutPut(tblTemp2, strAppID)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "")
                    Case "NO_DATA"
                        m_intStatus = -1234
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
' $History: ASL_03.vb $
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
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 16:21
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 18:02
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Lib
' 
' ************************************************
