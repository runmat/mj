Public Class EasyResultField
    Private fieldName As String
    Private fieldID As Integer
    Private fieldIndex As Integer

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

' ************************************************
' $History: EasyResultField.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:09
' Created in $/CKAG/EasyAccess
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:03
' Created in $/CKAG/Components/ComArchive/Work/CKG/EasyAccess
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:55
' Updated in $/CKG/EasyAccess
' 
' ************************************************