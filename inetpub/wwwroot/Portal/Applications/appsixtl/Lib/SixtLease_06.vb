Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class SixtLease_06
    REM § Status-Report, Kunde: ECAN, BAPI: Z_M_ABM_FEHLENDE_UNTERL_001,
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

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ECAN_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1

            Try

                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abm_Fehlende_Unterl_Slan", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_ABM_FEHLENDE_UNTERL_SLAN", m_objApp, m_objUser)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                S.AP.InitExecute("Z_M_ABM_FEHLENDE_UNTERL_SLAN", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

                Dim tr As DataRow

                For Each tr In tblTemp2.Rows
                    If tr("ABCKZ").ToString = "1" Then
                        tr("Brief") = "T"
                    End If
                Next

                tblTemp2.AcceptChanges()

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                'If intID > -1 Then
                '    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                'If intID > -1 Then
                '    m_objlogApp.WriteStandardDataAccessSAP(intID)
                'End If

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: SixtLease_06.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:47
' Updated in $/CKAG/Applications/appsixtl/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:27
' Created in $/CKAG/Applications/appsixtl/Lib
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 20.11.07   Time: 15:55
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 3.07.07    Time: 9:34
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' 
' ************************************************
