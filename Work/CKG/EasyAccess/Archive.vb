Public Class Archive
    Private arcLoc As String
    Private arcNam As String
    Private arcIdx As Int32
    Private arcIdxNam As String
    Private arcTitleNam As String
    Private arcDefaultQu As Object
    Private arcType As String   '§§§ JVE 10.05.2006
    Private aId As String

    Public Sub New(ByVal arcId As Long, ByVal arcLocation As String, ByVal arcName As String, ByVal arcIndex As Int32, ByVal arcIndexName As String, ByVal arcTitleName As String, ByVal arcDefaultQuery As Object, ByVal arcArchiveType As String)
        aId = arcId
        arcLoc = arcLocation
        arcNam = arcName
        arcIdxNam = arcIndexName
        arcTitleNam = arcTitleName
        arcDefaultQu = arcDefaultQuery
        arcType = arcArchiveType
    End Sub

    Public Function getId() As Long
        Return aId
    End Function

    Public Function getLocation() As String
        Return arcLoc
    End Function

    Public Function getName() As String
        Return arcNam
    End Function

    Public Function getIndex() As String
        Return arcIdx
    End Function

    Public Function getIndexName() As String
        Return arcIdxNam
    End Function

    Public Function getTitleName() As String
        Return arcTitleNam
    End Function

    Public Function getDefaultQuery() As Object
        Return arcDefaultQu
    End Function

    Public Function getArcType() As Object
        Return arcType
    End Function
End Class

' ************************************************
' $History: Archive.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:09
' Created in $/CKAG/EasyAccess
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:03
' Created in $/CKAG/Components/ComArchive/Work/CKG/EasyAccess
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:55
' Updated in $/CKG/EasyAccess
' 
' ************************************************