Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel


Public Class ANC_Status
    REM § Status-Report, Kunde: ANC, BAPI: Z_Bapi_National_Status,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_blnAll As Boolean
    Private m_datAbDatum As Date
    Private m_datBisDatum As Date
    Private m_strAction As String
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_blnAll = False
    End Sub

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strsessionid)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ANC_Status.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ANC.SAPProxy_ANC()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_ANC.ZDAD_M_WEB_NATIONAL_STATUSTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_Bapi_National_Status", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_Bapi_National_Status(SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                'Ersetze Bindestrich durch Leerzeichen in Kfz-Kennzeichen
                Dim tmpRow As DataRow
                For Each tmpRow In tblTemp2.Rows
                    If Not TypeOf tmpRow("LICENSE_NUM") Is System.DBNull Then
                        tmpRow("LICENSE_NUM") = Replace(CStr(tmpRow("LICENSE_NUM")), "-", "")
                    End If
                Next

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "", m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, m_strMessage, m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: ANC_Status.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:41
' Created in $/CKAG/Applications/appanc/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 20.08.07   Time: 15:17
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' ITA 1250: In Kennzeichen werden die Bindestriche nicht durch
' Leerzeichen ersetzt, sondern ersatzlos gelöscht.
' 
' *****************  Version 6  *****************
' User: Uha          Date: 20.08.07   Time: 13:30
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' ITA 1250 Testversion
' 
' *****************  Version 5  *****************
' User: Uha          Date: 20.08.07   Time: 10:23
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' ITA 1250 Punkt 5 "Vanguard Statusbericht" - Änderung der Datenausgabe
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 15:47
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 6.03.07    Time: 13:45
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' 
' ************************************************
