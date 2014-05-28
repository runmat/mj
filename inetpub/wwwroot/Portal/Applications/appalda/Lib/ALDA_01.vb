Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class ALDA_01
    REM § Status-Report, Kunde: ALD, BAPI: Z_M_Abm_Abgemeldete_Kfz,
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
    '    Fill(m_strAppID, m_strSessionID, m_datAbmeldedatumVon, m_datAbmeldedatumBis)
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime)
        m_strClassAndMethod = "ALDA_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_Base.SAPProxy_Base() '  SAPProxy_ALD.SAPProxy_ALD()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_Base.ZDAD_M_WEB_REPORTING_SIXTTable()
                Dim strDatTempVon As String
                If IsDate(datAbmeldedatumVon) Then
                    strDatTempVon = MakeDateSAP(datAbmeldedatumVon)
                    If strDatTempVon = "10101" Then
                        strDatTempVon = "|"
                    End If
                Else
                    strDatTempVon = "|"
                End If
                Dim strDatTempBis As String
                If IsDate(datAbmeldedatumBis) Then
                    strDatTempBis = MakeDateSAP(datAbmeldedatumBis)
                    If strDatTempBis = "10101" Then
                        strDatTempBis = "|"
                    End If
                Else
                    strDatTempBis = "|"
                End If

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abm_Abgemeldete_Kfz", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Abm_Abgemeldete_Kfz("00000000", "00000000", Right("0000000000" & m_objUser.KUNNR, 10), strDatTempVon, strDatTempBis, "", "", "", "", SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "ABMELDEDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMELDEDATBI=" & datAbmeldedatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "ABMELDEDATAB=" & datAbmeldedatumVon.ToShortDateString & ", ABMELDEDATBI=" & datAbmeldedatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
' $History: ALDA_01.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 15:01
' Updated in $/CKAG/Applications/appalda/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:29
' Created in $/CKAG/Applications/appalda/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 15:43
' Updated in $/CKG/Applications/AppALDA/AppALDAWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 6.03.07    Time: 13:21
' Updated in $/CKG/Applications/AppALDA/AppALDAWeb/Lib
' 
' ************************************************
