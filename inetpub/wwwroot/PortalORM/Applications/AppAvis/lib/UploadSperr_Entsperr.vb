Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class UploadSperr_Entsperr
    Inherits CKG.Base.Business.DatenimportBase
#Region " Declarations"
    Private m_tblSperren As DataTable
    Private m_tblEntsperren As DataTable
    Private m_tblErledigt As DataTable
    Private mSperrTabelle As DataTable
    Private mSAPTabelleZulassung As DataTable
    Private m_intFehlerCount As Integer
    Private m_strTask As String
#End Region
#Region " Properties"
    Public ReadOnly Property Sperren() As DataTable
        Get
            If m_tblSperren Is Nothing Then

                m_tblSperren = S.AP.GetImportTableWithInit("Z_M_FZG_SPERR_001", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

                'm_tblSperren = New DataTable
                'With m_tblSperren
                '    .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
                '    .Columns.Add("DAT_SPERRE", System.Type.GetType("System.String"))
                '    .Columns.Add("SPERRVERMERK", System.Type.GetType("System.String"))
                '    .Columns.Add("WEB_USER", System.Type.GetType("System.String"))
                '    .Columns.Add("RETUR_BEM", System.Type.GetType("System.String"))
                'End With
                'm_tblSperren.AcceptChanges()
            End If
            Return m_tblSperren
        End Get
    End Property

    Public ReadOnly Property Entsperren() As DataTable
        Get
            If m_tblEntsperren Is Nothing Then

                m_tblEntsperren = S.AP.GetImportTableWithInit("Z_M_FZG_ENTSPERR_001", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

                'm_tblEntsperren = New DataTable
                'With m_tblEntsperren
                '    .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
                '    .Columns.Add("WEB_USER", System.Type.GetType("System.String"))
                '    .Columns.Add("RETUR_BEM", System.Type.GetType("System.String"))
                'End With
                'm_tblEntsperren.AcceptChanges()
            End If
            Return m_tblEntsperren
        End Get
    End Property

    Public Property Erledigt() As DataTable
        Get
            Return m_tblErledigt
        End Get
        Set(ByVal value As DataTable)
            m_tblErledigt = value
        End Set
    End Property

    Public Property Task() As String
        Get
            Return m_strTask
        End Get
        Set(ByVal value As String)
            m_strTask = value
        End Set
    End Property
#End Region
#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub save()
        m_strClassAndMethod = "UploadSperr_Entsperr.Change"
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
                'If m_strTask = "Sperren" Then

                '    strCom = "EXEC Z_M_FZG_SPERR_001 @I_KUNNR_AG=@pI_KUNNR_AG,"
                '    strCom = strCom & "@GT_WEB=@pI_GT_WEB,@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'Else

                '    strCom = "EXEC Z_M_FZG_ENTSPERR_001 @I_KUNNR_AG=@pI_KUNNR_AG,"
                '    strCom = strCom & "@GT_WEB=@pI_GT_WEB,@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'End If

                'cmd.CommandText = strCom
                ''exportParameter
                'Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                ''importparameter
                'Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)
                'If m_strTask = "Sperren" Then
                '    Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", m_tblSperren)
                '    cmd.Parameters.Add(pI_GT_WEB)
                'Else
                '    Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", m_tblEntsperren)
                '    cmd.Parameters.Add(pI_GT_WEB)
                'End If

                'cmd.Parameters.Add(pI_KUNNR_AG)

                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_GT_WEB)

                ''befüllen der Importparameter
                'pI_KUNNR_AG.Value = Right("0000000000" & m_objUser.KUNNR, 10)

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_FZG_SPERR_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()
                S.AP.Execute()

                'm_tblErledigt = DirectCast(pE_GT_WEB.Value, DataTable)
                m_tblErledigt = S.AP.GetExportTable("GT_WEB")

                'Für DataBinding erforderliche Spalten ggf. hinzufügen
                If m_strTask = "EntSperren" Then
                    m_tblErledigt.Columns.Add("DAT_SPERRE", System.Type.GetType("System.String"))
                    m_tblErledigt.Columns.Add("SPERRVERMERK", System.Type.GetType("System.String"))
                    'ElseIf m_strTask = "Sperren" Then
                    '    Dim row As DataRow
                    '    For Each row In m_tblErledigt.Rows
                    '        row("DAT_SPERRE") = MakeDateStandard(row("DAT_SPERRE").ToString).ToShortDateString
                    '    Next
                End If
                'HelpProcedures.killAllDBNullValuesInDataTable(m_tblErledigt)



            Catch ex As Exception
                m_intStatus = -9999
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                'con.Close()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub generateSperrTable(ByVal tmpTable As DataTable)

        Dim tmpNewRow As DataRow

        If Not Sperren Is Nothing Then
            Sperren.Clear()
        End If
        Try
            For Each tmpRow As DataRow In tmpTable.Rows
                tmpNewRow = Sperren.NewRow

                If Not tmpTable.Rows(0) Is tmpRow Then
                    'nur nicht die überschriftstzeile bitte
                    If tmpRow(0).ToString.Length > 0 Then
                        tmpNewRow("CHASSIS_NUM") = tmpRow(0).ToString
                        'tmpNewRow("DAT_SPERRE") = MakeDateSAP(tmpRow(1).ToString)
                        tmpNewRow("DAT_SPERRE") = tmpRow(1)
                        tmpNewRow("SPERRVERMERK") = tmpRow(2).ToString
                        tmpNewRow("WEB_USER") = m_objUser.UserName
                        Sperren.Rows.Add(tmpNewRow)
                    Else
                        Exit For
                    End If

                End If
            Next
            Sperren.AcceptChanges()
            'HelpProcedures.killAllDBNullValuesInDataTable(Sperren)
        Catch ex As Exception
            m_intStatus = -111
            m_strMessage = "Die Übergabetabelle konnte nicht generiert werden, überprüfen Sie Ihre Exceldatei."
        End Try
    End Sub

    Public Sub generateEntsperrTable(ByVal tmpTable As DataTable)
        Dim tmpNewRow As DataRow

        If Not Entsperren() Is Nothing Then
            Entsperren.Clear()
        End If
        Try
            For Each tmpRow As DataRow In tmpTable.Rows
                tmpNewRow = Entsperren.NewRow

                If Not tmpTable.Rows(0) Is tmpRow Then
                    'nur nicht die überschriftstzeile bitte
                    tmpNewRow("CHASSIS_NUM") = tmpRow(0).ToString
                    tmpNewRow("WEB_USER") = m_objUser.UserName
                    Entsperren.Rows.Add(tmpNewRow)
                End If
            Next
            Entsperren.AcceptChanges()
            'HelpProcedures.killAllDBNullValuesInDataTable(Entsperren)
        Catch ex As Exception
            m_intStatus = -111
            m_strMessage = "Die Übergabetabelle konnte nicht generiert werden, überprüfen Sie Ihre Exceldatei."
        End Try
    End Sub

#End Region
End Class
