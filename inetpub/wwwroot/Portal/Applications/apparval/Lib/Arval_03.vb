Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Public Class Arval_03
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Zugelassene_Fzge_Arval,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    'Private m_strHaendlerNr As String
    'Private m_datAbmeldedatumVon As Date
    'Private m_datAbmeldedatumBis As Date
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    'Public Overloads Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID, m_strHaendlerNr, m_datAbmeldedatumVon, m_datAbmeldedatumBis)
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strHaendlerNr As String,
                              ByVal strLVNr As String, ByVal datAbmeldedatumVon As Date, ByVal datAbmeldedatumBis As Date)
        m_strClassAndMethod = "Arval_03.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1

            Try
                S.AP.Init("Z_M_Zugelassene_Fzge_Arval")

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Zugelassene_Fzge_Arval", m_objApp, m_objUser, Page)

                S.AP.SetImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10)) 'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_ZF", strHaendlerNr) 'myProxy.setImportParameter("I_ZF", strHaendlerNr)
                S.AP.SetImportParameter("I_LVNR", strLVNr) 'myProxy.setImportParameter("I_LVNR", strLVNr)

                If IsDate(datAbmeldedatumVon) Then
                    S.AP.SetImportParameter("I_VDATU_VON", datAbmeldedatumVon) 'myProxy.setImportParameter("I_VDATU_VON", CStr(datAbmeldedatumVon))
                End If

                If IsDate(datAbmeldedatumBis) Then
                    S.AP.SetImportParameter("I_VDATU_BIS", datAbmeldedatumBis) 'myProxy.setImportParameter("I_VDATU_BIS", CStr(datAbmeldedatumBis))
                End If

                S.AP.Execute() 'myProxy.callBapi()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("ITAB") 'myProxy.getExportTable("ITAB")
                CreateOutPut(tblTemp2, m_strAppID)

                WriteLogEntry(True, "WUNSCHLIEFDATAB=" & datAbmeldedatumVon.ToShortDateString & ", WUNSCHLIEFDATBIS=" &
                              datAbmeldedatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
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

                WriteLogEntry(False, "WUNSCHLIEFDATAB=" & datAbmeldedatumVon.ToShortDateString & ", WUNSCHLIEFDATBIS=" & datAbmeldedatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: Arval_03.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' ************************************************
