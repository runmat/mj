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
    Private m_strTask As String
#End Region

#Region " Properties"
    Public ReadOnly Property Sperren() As DataTable
        Get
            Return m_tblSperren
        End Get
    End Property

    Public ReadOnly Property Entsperren() As DataTable
        Get
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

            m_intStatus = 0

            Try
                'Init bereits durch generateSperrTable bzw. generateEntsperrTable erfolgt
                S.AP.Execute()

                m_tblErledigt = S.AP.GetExportTable("GT_WEB")

                'Für DataBinding erforderliche Spalten ggf. hinzufügen
                If m_strTask = "EntSperren" Then
                    m_tblErledigt.Columns.Add("DAT_SPERRE", GetType(DateTime))
                    m_tblErledigt.Columns.Add("SPERRVERMERK", GetType(String))
                End If

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

    Public Sub generateSperrTable(ByVal tmpTable As DataTable)

        Dim tmpNewRow As DataRow

        If m_tblSperren Is Nothing Then
            m_tblSperren = S.AP.GetImportTableWithInit("Z_M_FZG_SPERR_001", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())
        Else
            m_tblSperren.Clear()
        End If

        Try
            For Each tmpRow As DataRow In tmpTable.Rows
                tmpNewRow = m_tblSperren.NewRow

                If Not tmpTable.Rows(0) Is tmpRow Then
                    'nur nicht die überschriftstzeile bitte
                    If tmpRow(0).ToString.Length > 0 Then
                        tmpNewRow("CHASSIS_NUM") = tmpRow(0).ToString
                        tmpNewRow("DAT_SPERRE") = tmpRow(1)
                        tmpNewRow("SPERRVERMERK") = tmpRow(2).ToString
                        tmpNewRow("WEB_USER") = m_objUser.UserName
                        m_tblSperren.Rows.Add(tmpNewRow)
                    Else
                        Exit For
                    End If

                End If
            Next
            m_tblSperren.AcceptChanges()
        Catch ex As Exception
            m_intStatus = -111
            m_strMessage = "Die Übergabetabelle konnte nicht generiert werden, überprüfen Sie Ihre Exceldatei. (Fehler: " & ex.Message & ")"
        End Try
    End Sub

    Public Sub generateEntsperrTable(ByVal tmpTable As DataTable)
        Dim tmpNewRow As DataRow

        If m_tblEntsperren Is Nothing Then
            m_tblEntsperren = S.AP.GetImportTableWithInit("Z_M_FZG_ENTSPERR_001", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())
        Else
            m_tblEntsperren.Clear()
        End If

        Try
            For Each tmpRow As DataRow In tmpTable.Rows
                tmpNewRow = m_tblEntsperren.NewRow

                If Not tmpTable.Rows(0) Is tmpRow Then
                    'nur nicht die überschriftstzeile bitte
                    tmpNewRow("CHASSIS_NUM") = tmpRow(0).ToString
                    tmpNewRow("WEB_USER") = m_objUser.UserName
                    m_tblEntsperren.Rows.Add(tmpNewRow)
                End If
            Next
            m_tblEntsperren.AcceptChanges()
        Catch ex As Exception
            m_intStatus = -111
            m_strMessage = "Die Übergabetabelle konnte nicht generiert werden, überprüfen Sie Ihre Exceldatei. (Fehler: " & ex.Message & ")"
        End Try
    End Sub

#End Region
End Class
