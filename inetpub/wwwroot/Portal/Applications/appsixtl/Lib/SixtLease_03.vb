Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class SixtLease_03
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Zugelassene_Fzge_Arval,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strHaendlerNr As String
    Private m_datAbmeldedatumVon As Date
    Private m_datAbmeldedatumBis As Date
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strHaendlerNr As String, ByVal strLVNr As String, ByVal datAbmeldedatumVon As String, ByVal datAbmeldedatumBis As String)
        m_strClassAndMethod = "SixtLease_03.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Zugelassene_Fzge_Arval", m_objApp, m_objUser)

                'If IsDate(datAbmeldedatumVon) Then
                '    myProxy.setImportParameter("I_VDATU_VON", datAbmeldedatumVon)
                'End If

                'If IsDate(datAbmeldedatumBis) Then
                '    myProxy.setImportParameter("I_VDATU_BIS", datAbmeldedatumBis)
                'End If

                'If Not strHaendlerNr = String.Empty Then
                '    myProxy.setImportParameter("I_ZF", Right("0000000000" & strHaendlerNr, 10))
                'End If

                'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_LVNR", strLVNr)

                S.AP.Init("Z_M_Zugelassene_Fzge_Arval", "I_AG, I_LVNR", Right("0000000000" & m_objUser.KUNNR, 10), strLVNr)

                If IsDate(datAbmeldedatumVon) Then
                    S.AP.SetImportParameter("I_VDATU_VON", datAbmeldedatumVon)
                End If

                If IsDate(datAbmeldedatumBis) Then
                    S.AP.SetImportParameter("I_VDATU_BIS", datAbmeldedatumBis)
                End If

                If Not strHaendlerNr = String.Empty Then
                    S.AP.SetImportParameter("I_ZF", Right("0000000000" & strHaendlerNr, 10))
                End If

                'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Zugelassene_Fzge_Arval", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("ITAB")

                Dim tblTemp2 As DataTable = S.AP.GetExportTableWithExecute("ITAB")

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "WUNSCHLIEFDATAB=" & datAbmeldedatumVon & ", WUNSCHLIEFDATBIS=" & datAbmeldedatumBis & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_INV_AG"
                        m_intStatus = -3331
                        m_strMessage = "Ungültige Kundennummer."
                    Case "ERR_INV_ZF"
                        m_intStatus = -3332
                        m_strMessage = "Ungültiger Händler."
                    Case "ERR_NO_DATA"
                        m_intStatus = -3333
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
                WriteLogEntry(False, "WUNSCHLIEFDATAB=" & datAbmeldedatumVon & ", WUNSCHLIEFDATBIS=" & datAbmeldedatumBis & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                'If intID > -1 Then
                '    m_objLogApp.WriteStandardDataAccessSAP(intID)
                'End If

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: SixtLease_03.vb $
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
' *****************  Version 5  *****************
' User: Uha          Date: 3.07.07    Time: 9:34
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' 
' ************************************************
