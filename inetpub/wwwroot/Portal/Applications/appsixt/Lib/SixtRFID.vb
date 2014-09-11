Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class SixtRFID
    Inherits DatenimportBase

#Region "Declarations"
    Private mResultTable As DataTable
#End Region


#Region "Properties"

    Public ReadOnly Property BestandTable() As DataTable
        Get
            Return mResultTable
        End Get
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Security.User, ByVal objApp As Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub FillRFIDbestand(ByVal page As Web.UI.Page, ByVal DatumVon As String, ByVal DatumBis As String, ByVal Kennzeichen As String)
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_RFID_LESEN", m_objApp, m_objUser, page)

            If DatumVon.Length > 0 Then
                DatumVon = CStr(CDate(DatumVon).ToShortDateString)
            End If

            If DatumBis.Length > 0 Then
                DatumBis = CStr(CDate(DatumBis).ToShortDateString)
            End If

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_ERDAT_VON", DatumVon)
            'myProxy.setImportParameter("I_ERDAT_BIS", DatumBis)
            'myProxy.setImportParameter("I_LICENSE_NUM", Kennzeichen)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_DPM_RFID_LESEN", "I_KUNNR_AG, I_ERDAT_VON, I_ERDAT_BIS, I_LICENSE_NUM", Right("0000000000" & m_objUser.KUNNR, 10), DatumVon, DatumBis, Kennzeichen)

            'mResultTable = myProxy.getExportTable("GT_WEB")
            mResultTable = S.AP.GetExportTable("GT_WEB")

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -2502
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try
    End Sub
#End Region

End Class
' ************************************************
' $History: SixtRFID.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 19.05.09   Time: 16:39
' Updated in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 19.05.09   Time: 10:20
' Updated in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 15.05.09   Time: 16:17
' Created in $/CKAG/Applications/appsixt/Lib
' ITA: 2814
' 