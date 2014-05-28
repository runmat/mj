Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

'#################################################################
' Klasse für das Lesen der Avis-Grundaten#
' Reports : EingangZBIIUseCode00 (Report03)
'#################################################################
Public Class EingangZBIIUseCode00
    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    '----------------------------------------------------------------------
    ' Methode: Fill
    ' Autor: J.Jung
    ' Beschreibung: füllen der Ausgabetabelle für Eingang Fahrzeugbriefe (Report03)
    ' Erstellt am: 31.10.2008
    ' ITA: 2330
    '----------------------------------------------------------------------

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        Dim strKunnr As String = ""
        m_strClassAndMethod = "EingangZBIIUseCode00.Fill"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1

        Try



            'strCom = "EXEC Z_M_READ_GRUNDDAT_004 @I_KUNNR_AG='" & strKunnr & "', " _
            '                                       & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)


            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_READ_GRUNDDAT_002", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            'cmd.ExecuteNonQuery()
            Dim tblTemp2 As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_READ_GRUNDDAT_004.GT_WEB",
                                                         "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            'CreateOutPut(DirectCast(pSAPTableAUFTRAG.Value, DataTable), m_strAppID)
            CreateOutPut(tblTemp2, m_strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'cmd.Dispose()
            'con.Close()
            'con.Dispose()
            m_blnGestartet = False
        End Try
    End Sub
#End Region

End Class

' ************************************************
' $History: EingangZBIIUseCode00.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.11.08   Time: 9:35
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2421 testfertig
'
' ************************************************
