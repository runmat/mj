Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports Microsoft.Data.SAPClient
Public Class Monatsreport

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblHistory As DataTable
    'Private m_Fill As String = "---------------"
    Private m_Fill As String = "----------"    '§§§ JVE 02.10.2006
#End Region

#Region " Properties"
    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Monatsreport.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1
            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            'con.Open()
            Try
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_EC_AVM_STATUS_EINST_MONAT", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                S.AP.InitExecute("Z_M_EC_AVM_STATUS_EINST_MONAT", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim cmd As New SAPCommand()
                'cmd.Connection = con

                'Dim strCom As String

                'strCom = "EXEC Z_M_EC_AVM_STATUS_EINST_MONAT @I_KUNNR=@pI_KUNNR,@GT_WEB=@pI_GT_WEB,"
                'strCom = strCom & "@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom

                ''importparameter
                'Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                'Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", New DataTable)

                ''outputparameter
                'Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                ''Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR)
                'cmd.Parameters.Add(pI_GT_WEB)

                ''exportparameter hinzufügen
                'cmd.Parameters.Add(pE_GT_WEB)

                ''befüllen der Importparameter
                'pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)


                'cmd.ExecuteNonQuery()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tmpTable As DataTable = S.AP.GetExportTable("GT_WEB")

                If Not tmpTable Is Nothing Then
                    CreateOutPut(tmpTable, m_strAppID)
                End If
                'tmpTable = DirectCast(pE_GT_WEB.Value, DataTable)
                'HelpProcedures.killAllDBNullValuesInDataTable(tmpTable)
                'If Not pE_GT_WEB.Value Is Nothing Then
                '    CreateOutPut(tmpTable, m_strAppID)
                'End If

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
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If
                'con.Close()
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Monatsreport.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 29.09.08   Time: 15:17
' Updated in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 20.08.08   Time: 14:18
' Created in $/CKAG/Applications/appec/Lib
' ITA 2097 hinzugefügt
' 
' ************************************************
