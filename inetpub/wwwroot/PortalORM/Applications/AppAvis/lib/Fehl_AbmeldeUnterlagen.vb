Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts


Public Class Fehl_AbmeldeUnterlagen
    Inherits Base.Business.DatenimportBase
#Region " Declarations"
    Private m_tblSperren As DataTable
    Private m_tblEntsperren As DataTable

#End Region

#Region " Properties"
    Public Property Sperren() As DataTable
        Get
            Return m_tblSperren
        End Get
        Set(ByVal value As DataTable)
            m_tblSperren = value
        End Set
    End Property
    Public Property Entsperren() As DataTable
        Get
            Return m_tblEntsperren
        End Get
        Set(ByVal value As DataTable)
            m_tblEntsperren = value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        'Dim strKunnr As String = Right("0000000000" & m_objUser.KUNNR, 10)
        m_strClassAndMethod = "Fehl_AbmeldeUnterlagen.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID


        Dim intID As Int32 = -1

        Try

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_ABM_FEHL_UNTERL_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            'strCom = "EXEC Z_M_ABM_FEHL_UNTERL_001 @KUNNR='" & strKunnr & "', " _
            '                                       & "@AUSGABE=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)

            'cmd.ExecuteNonQuery()
            S.AP.InitExecute("Z_M_ABM_FEHL_UNTERL_001", "KUNNR", m_objUser.KUNNR.ToSapKunnr())

            Dim tblTemp2 As DataTable
            'tblTemp2 = DirectCast(pSAPTableAUFTRAG.Value, DataTable)
            tblTemp2 = S.AP.GetExportTable("AUSGABE")

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            CreateOutPut(tblTemp2, strAppID)
            'm_tblExcel = m_tblResult.Copy

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
