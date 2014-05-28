Imports CKG.Base.Kernel.Security

Public Class Mvc
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        SetMvcIFrameUrl()

    End Sub

    Sub SetMvcIFrameUrl()

        If (IsPostBack) Then
            Return
        End If

        If (Request("appID") Is Nothing) Then
            Return
        End If

        Dim logonUser As User = ZLDBaseMvc.MVC.GetSessionUserObject()
        If (logonUser Is Nothing) Then
            Return
        End If

        Dim url As String
        Try
            Dim appTable As DataTable = logonUser.Applications.Copy()
            Dim appUrl As String = appTable.Select("AppID = " & Request("appID"))(0)("AppURL")
            url = ZLDBaseMvc.MVC.MvcPrepareUrl(appUrl, Request("appID"), logonUser.UserName, True, "MvcAppZulassungsdienst")
            ' note: request querystring parameter "inline" overrides customer or user settings (i. e. "MvcRawLayout")
            '       because we render a url for a iframe here, we enforce "MvcEnforceRawLayout" here regardless of any customer or user settings 
            url = url & "&MvcEnforceRawLayout=1"
        Catch
            Return
        End Try

        ifrMvcApp.Attributes("src") = url

    End Sub

End Class
