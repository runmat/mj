Option Strict On
Option Explicit On

Namespace SilverDAT
    Friend NotInheritable Class DatCredentials
        Private ReadOnly _customerNumber As String
        Private ReadOnly _userName As String
        Private ReadOnly _password As String

        Public Sub New(customerNumber As String, userName As String, password As String)
            Me._customerNumber = customerNumber
            Me._userName = userName
            Me._password = password
        End Sub

        'Public Sub New()
        '    Me.New("1321363", "vsQbZ64v", "PW406HfC")
        'End Sub

        Public Sub New(credentials As Base.Kernel.Security.Customer.DatCredentials)
            Me._customerNumber = credentials.CustomerNumber
            Me._userName = credentials.UserName
            Me._password = credentials.Password
        End Sub

        Public ReadOnly Property CustomerNumber As String
            Get
                Return Me._customerNumber
            End Get
        End Property

        Public ReadOnly Property UserName As String
            Get
                Return Me._userName
            End Get
        End Property

        Public ReadOnly Property Password As String
            Get
                Return Me._password
            End Get
        End Property
    End Class
End Namespace