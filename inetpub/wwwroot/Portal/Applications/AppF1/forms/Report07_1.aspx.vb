Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel

Partial Public Class Report07_1
    Inherits System.Web.UI.Page

    Private m_User As Security.User
    Private m_App As Security.App


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        Dim FilePath As String = ConfigurationManager.AppSettings("DownloadPathGMAC") & Request.QueryString.Item("File")
        Dim fname As String = Request.QueryString.Item("File")

        'Dim ExcelFactory As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        'ExcelFactory.ReturnExcelTab(FilePath, fname, Me.Page)

        Response.ContentType = "file/xls"
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fname)
        Response.TransmitFile(FilePath)
        Response.End()
    End Sub

End Class
' ************************************************
' $History: Report07_1.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 10.09.09   Time: 17:01
' Updated in $/CKAG/Applications/AppF1/forms
' ITA: 3124
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 28.05.09   Time: 15:47
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 28.05.09   Time: 14:46
' Created in $/CKAG/Applications/AppF1/forms
' ITA: 2681
' 