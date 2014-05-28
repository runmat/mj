Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Report_400
    Inherits DatenimportBase

#Region "Declarations"
    Private mResultTable As DataTable
#End Region

#Region "Properties"

    Public ReadOnly Property ResultDataTable() As DataTable
        Get
            Return mResultTable
        End Get
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Security.User, ByVal objApp As Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub FillUnzugelasseneFahrzeuge()
        m_intStatus = 0

        Try
            S.AP.InitExecute("Z_M_Unzugelassene_Fzge_Arval", "I_AG", m_objUser.KUNNR.PadLeft(10, "0"))

            mResultTable = S.AP.GetExportTable("T_DATA")

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "ERR_INV_AG"
                    m_intStatus = -2502
                    m_strMessage = "Ungültiger Auftraggeber."
                Case "ERR_NO_DATA"
                    m_intStatus = -2503
                    m_strMessage = "Es wurden keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try
    End Sub
#End Region

End Class
' ************************************************
' $History: Report_400.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 6.05.09    Time: 13:33
' Created in $/CKAG/Components/ComCommon/Kernel
' ITA: 2848
' 
