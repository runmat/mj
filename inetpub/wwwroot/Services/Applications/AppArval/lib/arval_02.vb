Option Explicit On
Option Strict On

Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Arval_02
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Abm_Fehlende_Unterl_001,
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

    'Public Overloads Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID)
    'End Sub

    Public Overloads Sub FILL(ByVal appId As String, ByVal sessionId As String, ByVal page As Page)
        m_strClassAndMethod = "Arval_02.FILL"
        m_strAppID = appId
        m_strSessionID = sessionId
        Dim intID = -1
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Try
                Dim proxy = DynSapProxy.getProxy("Z_M_Abm_Fehlende_Unterl_001", m_objApp, m_objUser, page)
                proxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abm_Fehlende_Unterl_001", appId, sessionId, m_objUser.CurrentLogAccessASPXID)
                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = proxy.getExportTable("AUSGABE")

                CreateOutPut(tblTemp2, appId)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    'Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
    '    m_strClassAndMethod = "Arval_02.FILL"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_Base.SAPProxy_Base() '§§§ JVE 24.02.2006 Statt SAPProxy_ARVAL!
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        Dim intID As Int32 = -1

    '        Try
    '            Dim SAPTable As New SAPProxy_Base.ZDAD_M_WEB_REPORTING_ABM_2Table()

    '            intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abm_Fehlende_Unterl_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
    '            objSAP.Z_M_Abm_Fehlende_Unterl_001(Right("0000000000" & m_objUser.KUNNR, 10), SAPTable)
    '            objSAP.CommitWork()
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, True)
    '            End If

    '            Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

    '            CreateOutPut(tblTemp2, strAppID)
    '            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
    '        Catch ex As Exception

    '            m_intStatus = -9999
    '            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
    '            End If
    '            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
    '        Finally
    '            If intID > -1 Then
    '                m_objlogApp.WriteStandardDataAccessSAP(intID)
    '            End If

    '            objSAP.Connection.Close()
    '            objSAP.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub
#End Region
End Class

' ************************************************
' $History: arval_02.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 21.04.09   Time: 15:21
' Created in $/CKAG2/Applications/AppArval/lib
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
