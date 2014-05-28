Public Class Archive

#Region "Declarations"

    Private arcLoc As String
    Private arcNam As String
    Private arcIdx As Int32
    Private arcIdxNam As String
    Private arcTitleNam As String
    Private arcDefaultQu As Object
    Private arcType As String   '§§§ JVE 10.05.2006
    Private arcId As Long

#End Region

#Region "Properties"

    Public ReadOnly Property Location() As String
        Get
            Return arcLoc
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return arcNam
        End Get
    End Property

    Public ReadOnly Property Index() As String
        Get
            Return arcIdx
        End Get
    End Property

    Public ReadOnly Property IndexName() As String
        Get
            Return arcIdxNam
        End Get
    End Property

    Public ReadOnly Property TitleName() As String
        Get
            Return arcTitleNam
        End Get
    End Property

    Public ReadOnly Property DefaultQuery() As String
        Get
            Return arcDefaultQu
        End Get
    End Property

    Public ReadOnly Property Type() As String
        Get
            Return arcType
        End Get
    End Property

    Public ReadOnly Property Id() As String
        Get
            Return arcId
        End Get
    End Property

#End Region

    Public Sub New()

    End Sub

    Public Sub New(ByVal archId As Long, ByVal arcLocation As String, ByVal arcName As String, ByVal arcIndex As Int32, ByVal arcIndexName As String, ByVal arcTitleName As String, ByVal arcDefaultQuery As Object, ByVal arcArchiveType As String)
        arcId = archId
        arcLoc = arcLocation
        arcNam = arcName
        arcIdxNam = arcIndexName
        arcTitleNam = arcTitleName
        arcDefaultQu = arcDefaultQuery
        arcType = arcArchiveType
    End Sub

End Class
