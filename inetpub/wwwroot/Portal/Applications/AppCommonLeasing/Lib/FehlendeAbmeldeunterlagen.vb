Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
'Imports Microsoft.Data.SAPClient

Public Class FehlendeAbmeldeunterlagen
    Inherits Base.Business.DatenimportBase


    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String)
        m_strClassAndMethod = "FehlendeAbmeldeunterlagen.Fill"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0

            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            'con.Open()
            Try

                S.AP.InitExecute("Z_M_Abm_Fehlende_Unterl_001", "KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim cmd As New SAPCommand()
                'cmd.Connection = con

                'Dim strCom As String

                'strCom = "EXEC Z_M_Abm_Fehlende_Unterl_001 "
                'strCom = strCom & "@KUNNR=@pI_KUNNR,@AUSGABE=@pE_AUSGABE OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom


                ''importparameter

                'Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)


                ''exportParameter
                'Dim pE_AUSGABE As New SAPParameter("@pE_AUSGABE", ParameterDirection.Output)

                ''Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR)

                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_AUSGABE)


                'befüllen der Importparameter
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
                'If Not pE_AUSGABE.Value Is DBNull.Value Then

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                'tblTemp2 = DirectCast(pE_AUSGABE.Value, DataTable)
                'HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)

                For Each tmpRow As DataRow In tblTemp2.Rows
                    Dim i As Int32
                    For i = 0 To tmpRow.ItemArray.Length - 1
                        Dim tmpItem As DataColumn
                        tmpItem = tblTemp2.Columns(i)
                        With tmpItem
                            If .ColumnName = "SCHEIN" OrElse .ColumnName = "ABMAUF" OrElse .ColumnName = "CARPP" OrElse .ColumnName = "CARPC" OrElse .ColumnName = "BRIEF" OrElse .ColumnName = "TREUH" Then
                                If tmpRow(tmpItem.ColumnName).ToString = "X" Then
                                    tmpRow(tmpItem.ColumnName) = "Ja"
                                Else
                                    tmpRow(tmpItem.ColumnName) = "Nein"
                                End If
                            End If
                        End With
                    Next
                Next
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
' $History: FehlendeAbmeldeunterlagen.vb $
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
