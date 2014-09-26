Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports SapORM.Contracts

Public Class UploadTransportbeauftragung
    Inherits DatenimportBase

#Region " Declarations"

    Private m_tblUploadTable As DataTable
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

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Sub generateUploadTable(ByVal tmpTable As DataTable)

        If m_tblUploadTable Is Nothing Then
            m_tblUploadTable = S.AP.GetImportTableWithInit("Z_DPM_CHECK_UPL_TRANSBEAUF_001.GT_IN", "I_KUNNR_AG, I_WEBUSER", m_objUser.KUNNR.ToSapKunnr(), m_objUser.UserName)
        End If

        Try
            'Ohne Überschriftszeile => Start bei Index 1
            For i As Integer = 1 To tmpTable.Rows.Count - 1
                Dim tmpRow As DataRow = tmpTable.Rows(i)
                If Not String.IsNullOrEmpty(tmpRow(0).ToString()) Then
                    Dim tmpNewRow As DataRow = m_tblUploadTable.NewRow
                    tmpNewRow("CHASSIS_NUM") = tmpRow(0).ToString()
                    tmpNewRow("EX_KUNNR") = tmpRow(1).ToString()
                    tmpNewRow("BEAUFDAT") = tmpRow(2).ToString()
                    tmpNewRow("SPEDITION") = tmpRow(3).ToString()
                    m_tblUploadTable.Rows.Add(tmpNewRow)
                End If
            Next
            m_tblUploadTable.AcceptChanges()

        Catch ex As Exception
            m_intStatus = -111
            m_strMessage = "Die Übergabetabelle konnte nicht generiert werden, überprüfen Sie Ihre Exceldatei. (Fehler: " & ex.Message & ")"
        End Try
    End Sub

    Public Overloads Sub SaveTransportbeauftragung()
        m_strClassAndMethod = "UploadZulassung.SaveTransportbeauftragung"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0

            Try
                S.AP.Execute()

                m_tblErledigt = S.AP.GetExportTable("GT_OUT")

                E_SUBRC = S.AP.ResultCode
                E_MESSAGE = S.AP.ResultMessage

            Catch ex As Exception
                m_intStatus = -9999
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class
