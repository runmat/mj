Option Explicit On
Option Strict On

Imports SapORM.Contracts
Imports SapORM.Services
Imports System.Web

Namespace Kernel.Common

    Public Class S

        Public Shared ReadOnly Property AP() As ISapDataService
            Get
                Dim sapDataService As ISapDataService

                TrySetProdSapForWebLogonUser()

                If (HttpContext.Current.Session("SapDataService") Is Nothing) Then
                    sapDataService = SapDataServiceFactory.Create()
                    HttpContext.Current.Session("SapDataService") = sapDataService
                Else
                    sapDataService = CType(HttpContext.Current.Session("SapDataService"), ISapDataService)
                End If

                Return sapDataService
            End Get
        End Property

        Public Shared ReadOnly Property SapDataServiceFactory() As ISapDataServiceFactory
            Get
                Return New SapDataServiceDefaultFactory()
            End Get
        End Property

        Public Shared Sub TrySetProdSapForWebLogonUser()

            HttpContext.Current.Session("WebLogonUserOnProdDataSystem") = Nothing

            If Not HttpContext.Current.Session("objUser") Is Nothing Then
                Dim userObject As Security.User = CType(HttpContext.Current.Session("objUser"), Security.User)

                HttpContext.Current.Session("WebLogonUserOnProdDataSystem") = Not userObject.IsTestUser
            End If

        End Sub

    End Class

End Namespace
