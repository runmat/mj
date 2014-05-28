Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports CKG.Base.Kernel

Public Class EasyArchive

    Private conn As SqlClient.SqlConnection
    Private comm As SqlClient.SqlCommand
    Private user As Base.Kernel.Security.User
    Private archives As ArrayList
    Private archive As archive      'Aktuelles Archiv
    Private countArchive As Int32


    Public Sub New(ByVal aUser As Object)
        conn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        comm = New SqlClient.SqlCommand()
        comm.Connection = conn
        user = aUser
        countArchive = 0
    End Sub

    Public Function getArchive(ByVal id As Long) As archive
        Dim arc As Archive

        archive = Nothing
        For Each arc In archives
            If (arc.getId = id) Then
                archive = arc
            End If
        Next
        Return archive
    End Function

    Public Function getCurrentArchive() As archive
        Return archive
    End Function

    Public Sub getArchives(ByRef status As String)
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim archiveTable As New DataTable()
        Dim row As DataRow

        archives = New ArrayList()

        Try
            conn.Open()
            comm.CommandText = "SELECT * FROM vwEasyAccess WHERE GroupID = " & user.GroupID
            adapter.SelectCommand = comm
            adapter.Fill(archiveTable)
            For Each row In archiveTable.Rows
                archives.Add(New Archive(CType(row("ArchivID"), Long), row("EasyLagerortName").ToString, row("EasyArchivName").ToString, row("EasyQueryIndex").ToString, row("EasyQueryIndexName").ToString, row("EasyTitleName").ToString))
            Next
        Catch e As Exception
            status = "getArc:" & e.Message
        Finally
            conn.Dispose()
        End Try
    End Sub

    Public Function hasNext() As Boolean
        Return (countArchive < archives.Count)
    End Function

    Public Function nextArchive() As archive
        Dim a As Archive

        If hasNext() Then
            a = archives(countArchive)
            countArchive = countArchive + 1
            Return a
        End If
        Return Nothing
    End Function
End Class

' ************************************************
' $History: EasyArchive.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/EasyAccess_ALDBP/EasyAccess_ALDBP
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:21
' Created in $/CKAG/EasyAccess_ALDBP/EasyAccess_ALDBP
' 
' *****************  Version 5  *****************
' User: Uha          Date: 27.08.07   Time: 17:25
' Updated in $/CKG/Applications/AppALD/EasyAccess_ALDBP
' ITA 1269: Bugfix (ID-Spalte in vwEasyAccess hat anderen Namen bekommen)
' 
' *****************  Version 4  *****************
' User: Uha          Date: 27.08.07   Time: 13:27
' Updated in $/CKG/Applications/AppALD/EasyAccess_ALDBP
' ITA 1269: Neue EasyAccess-Tabellen => Neue Abrage auf vwEasyAccess
' 
' *****************  Version 3  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/EasyAccess_ALDBP
' 
' ************************************************
