Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts


Public Class UploadBereitstellung
    Inherits CKG.Base.Business.DatenimportBase
#Region " Declarations"
    Private m_tblUploadTable As DataTable
    Private m_intFehlerCount As Integer
    Private m_strTask As String
    Private m_tblErledigt As DataTable
    Private mE_SUBRC As Integer
    Private mE_MESSAGE As String
#End Region
#Region " Properties"
    Public Property UploadTable() As DataTable
        Get

            Return m_tblUploadTable
        End Get
        Set(ByVal value As DataTable)
            m_tblUploadTable = value
        End Set
    End Property
    Public Property Erledigt() As DataTable
        Get
            Return m_tblErledigt
        End Get
        Set(ByVal value As DataTable)
            m_tblErledigt = value
        End Set
    End Property

    Public Property E_SUBRC() As Integer
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As Integer)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
        End Set
    End Property
#End Region
#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub SetBereitstellung()
        m_strClassAndMethod = "UploadZulassung.SetBereitstellung"
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


                'strCom = "EXEC Z_DPM_SET_FZGPOOL_BEREIT_01 @I_KUNNR_AG=@pI_KUNNR_AG,"
                'strCom = strCom & "@GT_IN=@pI_GT_IN,@GT_OUT=@pE_GT_OUT OUTPUT,@E_SUBRC=@pE_SUBRC OUTPUT,@E_MESSAGE=@pE_MESSAGE OUTPUT OPTION 'disabledatavalidation'"


                'cmd.CommandText = strCom
                ''exportParameter
                'Dim pE_GT_OUT As New SAPParameter("@pE_GT_OUT", ParameterDirection.Output)
                'Dim pE_SUBRC As New SAPParameter("@pE_SUBRC", ParameterDirection.Output)
                'Dim pE_MESSAGE As New SAPParameter("@pE_MESSAGE", ParameterDirection.Output)

                ''importparameter
                'Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)

                'Dim pI_GT_IN As New SAPParameter("@pI_GT_IN", m_tblUploadTable)
                'cmd.Parameters.Add(pI_GT_IN)

                'cmd.Parameters.Add(pI_KUNNR_AG)

                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_GT_OUT)
                'cmd.Parameters.Add(pE_SUBRC)
                'cmd.Parameters.Add(pE_MESSAGE)

                'befüllen der Importparameter
                'pI_KUNNR_AG.Value = Right("0000000000" & m_objUser.KUNNR, 10)


                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_DPM_SET_FZGPOOL_BEREIT_01", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()
                S.AP.Execute()

                m_tblErledigt = S.AP.GetExportTable("GT_OUT")

                'm_tblErledigt = DirectCast(pE_GT_OUT.Value, DataTable)

                E_SUBRC = S.AP.ResultCode
                E_MESSAGE = S.AP.ResultMessage
                'If Not IsDBNull(pE_SUBRC.Value) Then
                '    E_SUBRC = DirectCast(pE_SUBRC.Value, Integer)
                'End If
                'If Not IsDBNull(pE_MESSAGE.Value) Then
                '    E_MESSAGE = DirectCast(pE_MESSAGE.Value, String)
                'End If
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

    Public Sub generateUploadTable(ByVal tmpTable As DataTable)
        Dim tmpNewRow As DataRow

        If m_tblUploadTable Is Nothing Then

            m_tblUploadTable = S.AP.GetImportTableWithInit("Z_DPM_SET_FZGPOOL_BEREIT_01.GT_IN", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            'm_tblUploadTable = New DataTable
            'With m_tblUploadTable
            '    .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
            'End With
            'm_tblUploadTable.AcceptChanges()
        End If
        Try
            For Each tmpRow As DataRow In tmpTable.Rows
                If tmpRow(0).ToString.Length > 0 Then
                    tmpNewRow = m_tblUploadTable.NewRow
                    tmpNewRow("CHASSIS_NUM") = tmpRow(0).ToString
                    m_tblUploadTable.Rows.Add(tmpNewRow)
                End If
            Next
            m_tblUploadTable.AcceptChanges()
            'HelpProcedures.killAllDBNullValuesInDataTable(m_tblUploadTable)
        Catch ex As Exception
            m_intStatus = -111
            m_strMessage = "Die Übergabetabelle konnte nicht generiert werden, überprüfen Sie Ihre Exceldatei."
        End Try
    End Sub

#End Region
End Class
