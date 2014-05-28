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
' User: Fassbenders  Date: 17.04.08   Time: 15:21
' Created in $/CKAG/EasyAccess_ALDBP/EasyAccess_ALDBP
' 
' *****************  Version 3  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/EasyAccess_ALDBP
' 
' ************************************************
