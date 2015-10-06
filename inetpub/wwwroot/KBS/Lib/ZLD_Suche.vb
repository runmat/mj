Imports System
Imports KBSBase
Imports SapORM.Models

Public Class ZLD_Suche
    Inherits ErrorHandlingClass

#Region "Properties"

    Public Property Kennzeichen As String
    Public Property PLZ As String
    Public Property Zulassungspartner As String
    Public Property ZulassungspartnerNr As String

    Private m_tblResult As DataTable
    Public ReadOnly Property tblResult As DataTable
        Get
            Return m_tblResult
        End Get
    End Property

#End Region

#Region "Methods"

    Public Sub Fill()
        ClearErrorState()

        Try
            Z_M_BAPIRDZ.Init(S.AP)

            S.AP.SetImportParameter("IZKFZKZ", Kennzeichen)
            S.AP.SetImportParameter("IPOST_CODE1", PLZ)
            S.AP.SetImportParameter("INAME1", Zulassungspartner)
            S.AP.SetImportParameter("IREMARK", ZulassungspartnerNr)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                m_tblResult = S.AP.GetExportTable("ITAB")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

#End Region

End Class