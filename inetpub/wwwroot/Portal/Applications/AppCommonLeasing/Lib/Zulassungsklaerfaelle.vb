Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
'Imports Microsoft.Data.SAPClient

Public Class Zulassungsklaerfaelle
    Inherits Base.Business.DatenimportBase


    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String)
        m_strClassAndMethod = "Zulassungsklaerfaelle.Fill"
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

                'strCom = "EXEC Z_M_KLAERFAELLE "
                'strCom = strCom & "@I_KUNNR=@pI_KUNNR,@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom

                S.AP.InitExecute("Z_M_KLAERFAELLE", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'importparameter

                'Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)


                ''exportParameter
                'Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                ''Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR)

                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_GT_WEB)


                ''befüllen der Importparameter
                'pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)


                'If m_objLogApp Is Nothing Then
                '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                'End If

                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_FEHLENDE_COC ", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                'auswerten der exportparameter
                'If Not pE_GT_WEB.Value Is DBNull.Value Then
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
                'tblTemp2 = DirectCast(pE_GT_WEB.Value, DataTable)
                tblTemp2.Columns.Add("Klaerfalltext", String.Empty.GetType)
                'HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
                For Each tmpRow As DataRow In tblTemp2.Rows
                    tmpRow("Klaerfalltext") = tmpRow("TDLINE1").ToString & tmpRow("TDLINE2").ToString & tmpRow("TDLINE3").ToString & tmpRow("TDLINE4").ToString & tmpRow("TDLINE5").ToString
                Next
                tblTemp2.AcceptChanges()
                CreateOutPut(tblTemp2, strAppID)
                'End If

                WriteLogEntry(True, "", m_tblResult)
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
' $History: Zulassungsklaerfaelle.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 23.10.08   Time: 17:01
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2312 nicht durchführbare Zulassungen
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 23.10.08   Time: 12:36
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2312 fehlende Abmeldeunterlagen
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 23.10.08   Time: 10:09
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2312 Fehlende CoC fertig
' 
' ************************************************
