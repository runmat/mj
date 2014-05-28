Imports CuteWebUI

Public Class UploadedFile
    Public Property Name As String
    Public Property Id As Guid
    Public Property Size As Integer
    Public Property Status As String

    Public Shared Function FromAttachmentItem(ByVal item As AttachmentItem) As UploadedFile
        Dim result = New UploadedFile()
        With result
            .Name = item.FileName
            .Id = item.FileGuid
            .Size = item.FileSize
        End With

        Return result
    End Function
End Class
