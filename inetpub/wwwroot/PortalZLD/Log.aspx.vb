Imports GeneralTools.Contracts
Imports GeneralTools.Services
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common

Public Class Log
    Inherits Page

    Public Shared ReadOnly PortalType As Integer = 5  ' 1 = Portal, 2 = Services, 3 = ServicesMvc, 4 = KBS, 5 = PortalZLD, 6 = AutohausPortal

    ''' <summary>
    ''' Page Visit Logging
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request.QueryString.AllKeys.Any(Function(key) key.ToUpper() = "APP-ID") = False Then
            ' Loggen nicht möglich, keine gültige App ID
            Return
        End If

        Dim userObject As User = Common.GetUser(Me)
        If (userObject Is Nothing Or userObject.Customer Is Nothing) Then
            ' Loggen nicht möglich, User nicht eingeloggt oder kein Kunde gesetzt
            Return
        End If

        Dim appId As String = Request.QueryString("APP-ID")

        Dim logService As ILogService = New LogService()
        logService.LogPageVisit(Int32.Parse(appId), userObject.UserID, userObject.Customer.CustomerId, userObject.Customer.KUNNR, PortalType)
    End Sub

End Class
