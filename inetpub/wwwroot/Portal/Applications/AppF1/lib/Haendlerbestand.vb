
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class Haendlerbestand
    Inherits DatenimportBase

#Region "Declarations"
    Private mResultTable As DataTable
#End Region

#Region "Properties"

    Public ReadOnly Property HaendlerbestandTable() As DataTable
        Get
            Return mResultTable
        End Get
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Security.User, ByVal objApp As Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub FillHaendlerbestand(ByVal UserReference As String)
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BESTAND_LESEN_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER_EX", UserReference)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BESTAND_LESEN_STD", "I_AG,I_HAENDLER_EX", Right("0000000000" & m_objUser.KUNNR, 10), UserReference)

            mResultTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")


            mResultTable.Columns("ZZCOCKZ").MaxLength = 4

            For Each Row As DataRow In mResultTable.Rows

                If IsDBNull(Row("ZZCOCKZ")) = False Then
                    Row("ZZCOCKZ") = IIf(UCase(Row("ZZCOCKZ").ToString) = "X", "ja", "nein").ToString
                End If
                If IsDBNull(Row("DATAB")) = False Then
                    Row("DATAB") = Left(Row("DATAB").ToString, 10)
                End If

                If IsDBNull(Row("ZZHUBRAUM")) = False Then
                    Row("ZZHUBRAUM") = Row("ZZHUBRAUM").ToString.TrimStart("0"c)
                End If
                If IsDBNull(Row("ZZNENNLEISTUNG")) = False Then
                    Row("ZZNENNLEISTUNG") = Row("ZZNENNLEISTUNG").ToString.TrimStart("0"c)
                End If

            Next

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_HAENDLER"
                    m_intStatus = -2502
                    m_strMessage = "Händler nicht vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
        End Try
    End Sub

    Public Sub FillExpressversendungen(ByVal Haendlernummer As String, ByVal DatumVon As String, ByVal DatumBis As String)
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_ZVERSAND_01", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER", Haendlernummer)
            'myProxy.setImportParameter("I_DATUM_VON", DatumVon)
            'myProxy.setImportParameter("I_DATUM_BIS", DatumBis)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_DPM_READ_ZVERSAND_01", "I_KUNNR_AG,I_HAENDLER,I_DATUM_VON,I_DATUM_BIS",
                             Right("0000000000" & m_objUser.KUNNR, 10), Haendlernummer, DatumVon, DatumBis)

            mResultTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_intStatus = -2502
                    m_strMessage = "Keine Versendungen gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
        End Try
    End Sub


#End Region

End Class
' ************************************************
' $History: Haendlerbestand.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 21.08.09   Time: 9:50
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 3033
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 19.03.09   Time: 11:51
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 16.03.09   Time: 15:27
' Updated in $/CKAG/Applications/AppF1/lib
' 