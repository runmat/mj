Option Explicit On
Option Strict On

Imports SapORM.Contracts
Imports SapORM.Services


Namespace Kernel

    Public Class S

        Public Shared ReadOnly Property AP() As ISapDataService
            Get
                Dim sapDataService As ISapDataService

                If (Web.HttpContext.Current.Session("SapDataService") Is Nothing) Then
                    sapDataService = SapDataServiceFactory.Create()
                    Web.HttpContext.Current.Session("SapDataService") = sapDataService
                Else
                    sapDataService = CType(Web.HttpContext.Current.Session("SapDataService"), ISapDataService)
                End If

                Return sapDataService
            End Get
        End Property

        Private Shared _sapDataServiceFactory As ISapDataServiceFactory
        Public Shared ReadOnly Property SapDataServiceFactory() As ISapDataServiceFactory
            Get
                If (_sapDataServiceFactory Is Nothing) Then

                    _sapDataServiceFactory = New SapDataServiceDefaultFactory()

                End If

                Return _sapDataServiceFactory
            End Get
        End Property

    End Class

End Namespace