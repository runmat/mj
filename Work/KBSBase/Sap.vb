Option Explicit On
Option Strict On

Imports SapORM.Contracts
Imports SapORM.Services
Imports System.Web

Public Class S

    Public Shared ReadOnly Property AP() As ISapDataService
        Get
            Dim sapDataService As ISapDataService

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
            Return New SapDataServiceFromConfigNoCacheFactory()
        End Get
    End Property

End Class
