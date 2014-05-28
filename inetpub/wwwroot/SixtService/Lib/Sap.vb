Option Explicit On
Option Strict On

Imports SapORM.Contracts
Imports SapORM.Services

Public Class S

    Private Shared _sapDataService As ISapDataService

    Public Shared ReadOnly Property AP() As ISapDataService
        Get
            If (_sapDataService Is Nothing) Then
                _sapDataService = SapDataServiceFactory.Create()
            End If

            Return _sapDataService
        End Get
    End Property

    Private Shared _sapDataServiceFactory As ISapDataServiceFactory

    Public Shared ReadOnly Property SapDataServiceFactory() As ISapDataServiceFactory
        Get
            If (_sapDataServiceFactory Is Nothing) Then

                _sapDataServiceFactory = New SapDataServiceFromConfigNoCacheFactory() 'New SapDataServiceDefaultFactory()

            End If

            Return _sapDataServiceFactory
        End Get
    End Property

End Class