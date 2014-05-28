Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class AvisChange05
    Inherits CKG.Base.Business.DatenimportBase
#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub


    Public Function SearchABMDat(ByVal FIN As String, _
                                             ByVal Kennzeichen As String, _
                                             ByVal MvaNr As String) As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        Dim intID As Int32 = -1

        Try
            'strCom = "EXEC Z_M_READ_ABMDAT_FZG_001 @I_KUNNR_AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '         & "@I_CHASSIS_NUM='" & FIN & "'," _
            '        & "@I_MVA_NUMMER='" & MvaNr & "'," _
            '        & "@I_LICENSE_NUM='" & Kennzeichen & "'," _
            '        & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            ''Exportparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTable)


            'cmd.ExecuteNonQuery()

            'Dim SAPTable As DataTable = DirectCast(pSAPTable.Value, DataTable)

            Dim SAPTable As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_READ_ABMDAT_FZG_001.GT_WEB",
                                                                           "I_KUNNR_AG, I_CHASSIS_NUM, I_MVA_NUMMER, I_LICENSE_NUM",
                                                                           m_objUser.KUNNR.ToSapKunnr(), FIN, MvaNr, Kennzeichen)

            Dim Row As DataRow

            If SAPTable.Rows.Count > 0 Then

                For Each Row In SAPTable.Rows

                    Row("REPLA_DATE") = Row("REPLA_DATE").ToString.NotEmptyOrDbNull
                    Row("ZZABMDAT") = Row("ZZABMDAT").ToString.NotEmptyOrDbNull

                Next

            End If


            m_tblResult = SAPTable

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
                                                        "Z_M_READ_ABMDAT_FZG_001", m_strAppID, m_strSessionID, _
                                                        m_objUser.CurrentLogAccessASPXID)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Daten.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            'Throw New Exception(m_strMessage)

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'con.Close()
            'con.Dispose()

            'cmd.Dispose()

        End Try

        Return m_tblResult

    End Function


    Public Function GetImportTableForSave() As DataTable

        Return S.AP.GetImportTableWithInit("Z_M_SAVE_ABMDAT_FZG_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

    End Function

    Public Function SaveABMDat() As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        Dim intID As Int32 = -1

        Try
            'strCom = "EXEC Z_M_SAVE_ABMDAT_FZG_001 @I_KUNNR_AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@GT_WEB=@pSAPTable,@GT_WEB=@SAPTableExport OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            ''Importparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", SAPTable)
            'cmd.Parameters.Add(pSAPTable)

            ''Exportparameter
            'Dim SAPTableExport As New SAPParameter("@SAPTableExport", ParameterDirection.Output)
            'cmd.Parameters.Add(SAPTableExport)

            'cmd.ExecuteNonQuery()

            'Dim SAPTableEx As DataTable = DirectCast(SAPTableExport.Value, DataTable)
            '
            '** already processed by "GetImportTableForSave()"
            '** SAPTable = S.AP.GetImportTableWithInit("Z_M_SAVE_ABMDAT_FZG_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            S.AP.Execute()
            Dim SAPTableEx As DataTable = S.AP.GetExportTable("GT_WEB")

            m_tblResult = SAPTableEx

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
                                                        "Z_M_SAVE_ABMDAT_FZG_001", m_strAppID, m_strSessionID, _
                                                        m_objUser.CurrentLogAccessASPXID)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Speichern.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            'Throw New Exception(m_strMessage)

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'con.Close()
            'con.Dispose()

            'cmd.Dispose()

        End Try

        Return m_tblResult

    End Function




    'Private Function MakeStandardDate(ByVal strInput As String) As String
    '    Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)

    '    If IsDate(strTemp) Then
    '        Return strTemp
    '    Else
    '        Return String.Empty
    '    End If
    'End Function

#End Region
End Class
