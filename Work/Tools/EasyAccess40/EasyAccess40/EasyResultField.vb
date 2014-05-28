Public Class EasyResultField

    Private fieldName As String
    Private fieldID As Integer
    Private fieldIndex As Integer

    Public Sub New()

    End Sub

    Public Sub New(ByVal name As String, ByVal id As Integer, ByVal index As Integer)
        fieldName = name
        fieldID = id
        fieldIndex = index
    End Sub

    Public ReadOnly Property Name() As String
        Get
            Return fieldName
        End Get
    End Property

    Public ReadOnly Property Id() As String
        Get
            Return fieldID
        End Get
    End Property

    Public Property Index() As String
        Get
            Return fieldIndex
        End Get
        Set(ByVal value As String)
            fieldIndex = value
        End Set
    End Property

End Class
