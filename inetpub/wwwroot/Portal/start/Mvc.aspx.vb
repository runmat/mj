Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Namespace Start
    Public Class Mvc
        Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "
        Protected WithEvents ucHeader As PageElements.Header
        'Dieser Aufruf ist für den Web Form-Designer erforderlich.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
            'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
            InitializeComponent()
        End Sub

#End Region

        Private m_User As Security.User
        Private m_App As Security.App
        Protected WithEvents ucStyles As PageElements.Styles
        Protected WithEvents ifrMvcApp As HtmlGenericControl

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            SetMvcIFrameUrl()

        End Sub

        Sub SetMvcIFrameUrl()

            If (IsPostBack) Then
                Return
            End If

            If (Request("appID") Is Nothing) Then
                Return
            End If

            Dim logonUser As Security.User = BtBaseMvc.MVC.GetSessionUserObject()
            If (logonUser Is Nothing) Then
                Return
            End If

            m_User = logonUser
            ucHeader.InitUser(logonUser)

            Dim url As String
            Try
                Dim appTable As DataTable = logonUser.Applications.Copy()
                Dim appUrl As String = appTable.Select("AppID = " & Request("appID"))(0)("AppURL")
                url = BtBaseMvc.MVC.MvcPrepareUrl(appUrl, Request("appID"), logonUser.UserName, True)
                ' note: request querystring parameter "inline" overrides customer or user settings (i. e. "MvcRawLayout")
                '       because we render a url for a iframe here, we enforce "MvcEnforceRawLayout" here regardless of any customer or user settings 
                url = url & "&cp=" & Request("cp")
                url = url & "&MvcEnforceRawLayout=1"
            Catch
                Return
            End Try

            ifrMvcApp.Attributes("src") = url

        End Sub

    End Class
End Namespace

