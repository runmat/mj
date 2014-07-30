Option Explicit On
Option Strict On

Imports SapORM.Contracts
Imports SapORM.Services

Public Class S

    Private _sapDataService As ISapDataService

    Public ReadOnly Property AP() As ISapDataService
        Get
            If (_sapDataService Is Nothing) Then
                _sapDataService = SapDataServiceFactory.Create()
            End If

            Return _sapDataService
        End Get
    End Property

    Public ReadOnly Property SapDataServiceFactory() As ISapDataServiceFactory
        Get
            Return New SapDataServiceFromConfigNoCacheFactory()
        End Get
    End Property

End Class