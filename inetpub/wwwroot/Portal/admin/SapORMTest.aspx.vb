'Imports CKG.Base.Kernel
'Imports CKG.Base.Kernel.Common.Common

'Public Class SapORMTest
'    Inherits System.Web.UI.Page

'    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles
'    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header

'    Dim strOKImgURL As String = "../Images/AllesOk2.jpg"
'    Dim strFehlerImgURL As String = "../Images/Problem.jpg"
'    Dim objAPP As CKG.Base.Kernel.Security.App
'    Dim objUser As CKG.Base.Kernel.Security.User

'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        objUser = GetUser(Me)
'        ucHeader.InitUser(objUser)
'        FormAuth(Me, objUser)

'        objAPP = New Base.Kernel.Security.App(objUser)
'    End Sub

'    Protected Sub btnCallSAP_Click(sender As Object, e As EventArgs) Handles btnCallSAP.Click

'        lblERPvorhanden.Text = ""
'        lblSAPORMvorhanden.Text = ""

'        Try
'            Base.Common.DynSapProxyErp.getProxy(txtTestBapi.Text, objAPP, objUser, Me)
'            imgERPvorhanden.ImageUrl = strOKImgURL
'        Catch ex As Exception
'            imgERPvorhanden.ImageUrl = strFehlerImgURL
'            lblERPvorhanden.Text = ex.Message & ": " & ex.InnerException.ToString
'        End Try

'        Try
'            S.AP.Init(txtTestBapi.Text.Trim)
'            imgSAPORMvorhanden.ImageUrl = strOKImgURL
'        Catch ex As Exception
'            imgSAPORMvorhanden.ImageUrl = strFehlerImgURL
'            lblSAPORMvorhanden.Text = ex.Message & ": " & ex.InnerException.ToString
'        End Try

'    End Sub
'End Class