Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class SixtLease_01
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datAbmeldedatumVon As DateTime
    Private m_datAbmeldedatumBis As DateTime
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime)

        m_strClassAndMethod = "SixtLease_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True
            
            Try
                Dim strAnzahl As String = ""
                Dim strDatTempVon As String
                If IsDate(datAbmeldedatumVon) Then
                    strDatTempVon = datAbmeldedatumVon.ToShortDateString
                Else
                    strDatTempVon = ""
                End If
                Dim strDatTempBis As String
                If IsDate(datAbmeldedatumBis) Then
                    strDatTempBis = datAbmeldedatumBis.ToShortDateString
                Else
                    strDatTempBis = ""
                End If

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_ABGEMELDETE_KFZ", m_objApp, m_objUser, Page)

                'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                S.AP.Init("Z_M_ABM_ABGEMELDETE_KFZ", "KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'If strDatTempVon.Length > 0 Then
                '    myProxy.setImportParameter("ABMDATAB", strDatTempVon)
                'End If
                'If strDatTempBis.Length > 0 Then
                '    myProxy.setImportParameter("ABMDATBI", strDatTempBis)
                'End If

                If strDatTempVon.Length > 0 Then
                    S.AP.SetImportParameter("ABMDATAB", strDatTempVon)
                End If
                If strDatTempBis.Length > 0 Then
                    S.AP.SetImportParameter("ABMDATBI", strDatTempBis)
                End If

                'myProxy.callBapi()
                S.AP.Execute()

                'strAnzahl = myProxy.getExportParameter("ANZAHL")

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("AUSGABE")

                strAnzahl = S.AP.GetExportParameter("ANZAHL")

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ABMDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMDATBI=" & datAbmeldedatumBis.ToShortDateString, m_tblResult, False)


            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ABMDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMDATBI=" & datAbmeldedatumBis.ToShortDateString, m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub
#End Region
End Class

' ************************************************
' $History: SixtLease_01.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 8.03.10    Time: 16:52
' Updated in $/CKAG/Applications/appsixtl/Lib
' ITA: 2918
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
