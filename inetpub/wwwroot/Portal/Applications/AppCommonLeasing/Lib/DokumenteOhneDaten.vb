Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
'Imports Microsoft.Data.SAPClient

Public Class DokumenteOhneDaten
    Inherits Base.Business.DatenimportBase


    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String)
        m_strClassAndMethod = "DokumenteOhneDaten.Fill"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0

            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            'con.Open()
            Try

                'Dim cmd As New SAPCommand()
                'cmd.Connection = con

                'Dim strCom As String

                'strCom = "EXEC Z_M_DOKUMENTE_OHNE_DATEN "
                'strCom = strCom & "@I_KUNNR_AG=@pI_KUNNR_AG,@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom

                S.AP.InitExecute("Z_M_DOKUMENTE_OHNE_DATEN", "I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                CreateOutPut(S.AP.GetExportTable("GT_WEB"), strAppID)

                ''importparameter

                'Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)


                ''exportParameter
                'Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                ''Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR_AG)

                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_GT_WEB)


                'befüllen der Importparameter
                'pI_KUNNR_AG.Value = Right("0000000000" & m_objUser.KUNNR, 10)


                'If m_objLogApp Is Nothing Then
                '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                'End If

                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_FEHLENDE_COC ", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                ''auswerten der exportparameter
                'CreateOutPut(DirectCast(pE_GT_WEB.Value, DataTable), strAppID)
                'HelpProcedures.killAllDBNullValuesInDataTable(Result)

                WriteLogEntry(True, "", Result)
            Catch ex As Exception
                ResultTable = Nothing
                m_intStatus = -9999

                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
            Finally
                'If intID > -1 Then
                '    m_objLogApp.WriteStandardDataAccessSAP(intID)
                'End If

                'con.Close()

                m_blnGestartet = False
            End Try
        End If

    End Sub

End Class

' ************************************************
' $History: DokumenteOhneDaten.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.11.08   Time: 8:52
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2372 + 2367 (Nachbesserung)
' 
' ************************************************
