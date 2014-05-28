Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class FFE_Bank_UnbekHaendler
    REM § Status-Report, Kunde: FFE, BAPI: Z_M_Unbekannte_Haendler,
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

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strsessionid)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "FFD_Bank_UnbekHaendler.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_FFE.ZFCEIMPORT_UNBTable()
                Dim strKUNNR As String = ""

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unbekannte_Haendler", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Unbekannte_Haendler_Fce(SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "", m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden"
                    Case Else
                        m_strMessage = ex.Message
                End Select
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
' $History: FFE_Bank_UnbekHaendler.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugef�gt
' 
' ************************************************
