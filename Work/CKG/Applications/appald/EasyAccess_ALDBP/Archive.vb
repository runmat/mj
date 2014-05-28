Public Class Archive
    Private arcLoc As String
    Private arcNam As String
    Private arcIdx As Int32
    Private arcIdxNam As String
    Private arcTitleNam As String
    Private aId As String

    Public Sub New(ByVal arcId As Long, ByVal arcLocation As String, ByVal arcName As String, ByVal arcIndex As Int32, ByVal arcIndexName As String, ByVal arcTitleName As String)
        aId = arcId
        arcLoc = arcLocation
        arcNam = arcName
        'arcIdx = arcIndex
        arcIdxNam = arcIndexName
        arcTitleNam = arcTitleName
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
End Class

' ************************************************
' $History: Archive.vb $
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
