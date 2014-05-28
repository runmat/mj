Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports CKG.Base.Kernel

Public Class EasyArchive

    Private conn As SqlClient.SqlConnection
    Private comm As SqlClient.SqlCommand
    Private user As Security.User
    Private archives As ArrayList
    Private archive As archive      'Aktuelles Archiv
    Private countArchive As Int32
    Private masks As New ArrayList()
    Private mask As String
    Private DadArchive As String = String.Empty

    Public ReadOnly Property masklist() As ArrayList
        Get
            Return masks
        End Get
    End Property

    Public Function count() As Integer
        Return archives.Count
    End Function

    Public Sub New(ByVal aUser As Security.User, Optional ByVal strDadArchiv As String = "")
        conn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        comm = New SqlClient.SqlCommand()
        comm.Connection = conn
        user = aUser
        countArchive = 0
        DadArchive = strDadArchiv
    End Sub

    Public Function getArchive(ByVal id As Long) As archive
        Dim arc As archive

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
        Try
            Dim adapter As New SqlClient.SqlDataAdapter()
            Dim archiveTable As New DataTable()
            Dim row As DataRow

            archives = New ArrayList()

            conn.Open()
            If DadArchive = String.Empty Then
                comm.CommandText = "SELECT * FROM vwEasyAccess WHERE GroupID = " & user.GroupID & " ORDER BY SortOrder"
            Else
                If DadArchive.ToUpper.Contains("FORD") Then
                    comm.CommandText = "SELECT * FROM Archiv WHERE EasyLagerortName = '" & DadArchive.Substring(0, 4) & "' and EasyArchivName like '" & DadArchive.Substring(5, 3) & "%' and DefaultQuery <> '.1004=34447*'ORDER BY SortOrder"
                Else
                    comm.CommandText = "SELECT * FROM Archiv WHERE EasyLagerortName = '" & DadArchive & "' ORDER BY SortOrder"
                End If
            End If

            adapter.SelectCommand = comm
            adapter.Fill(archiveTable)


            archiveTable.Columns.Add("SortByYear", GetType(System.Int32))

            Dim NumFound As Boolean

            For Each row In archiveTable.Rows

                Dim CountRight As Integer = 1
                NumFound = False

                If Not String.IsNullOrEmpty(row("EasyTitleName")) Then



                    For i As Integer = 0 To row("EasyTitleName").ToString.Length - 1

                        If CountRight > row("EasyTitleName").ToString.Length Then Exit For

                        If IsNumeric(Right(row("EasyTitleName").ToString, CountRight)) Then

                            row("SortByYear") = Right(row("EasyTitleName").ToString, CountRight)

                            CountRight += 1
                            NumFound = True
                        Else
                            Exit For

                        End If

                    Next

                End If

            Next

            If NumFound = True Then

                archiveTable.DefaultView.Sort = "SortByYear desc"
                archiveTable = archiveTable.DefaultView.ToTable

            End If



            For Each row In archiveTable.Rows
                archives.Add(New archive(CType(row("ArchivID"), Long), row("EasyLagerortName").ToString, row("EasyArchivName").ToString, row("EasyQueryIndex").ToString, row("EasyQueryIndexName").ToString, row("EasyTitleName").ToString, row("DefaultQuery").ToString, row("ArchiveType").ToString))
            Next

            If DadArchive = String.Empty Then
                comm.CommandText = "SELECT DISTINCT EasyQueryIndexName FROM vwEasyAccess WHERE (GroupID = " & user.GroupID & ") ORDER BY EasyQueryIndexName"
            Else
                If DadArchive.ToUpper.Contains("FORD") Then
                    comm.CommandText = "SELECT DISTINCT EasyQueryIndexName FROM Archiv WHERE EasyLagerortName = '" & DadArchive.Substring(0, 4) & "' and EasyArchivName like '" & DadArchive.Substring(5, 3) & "%' and DefaultQuery <> '.1004=34447*'"
                Else
                    comm.CommandText = "SELECT DISTINCT EasyQueryIndexName,SortOrder FROM Archiv WHERE EasyLagerortName = '" & DadArchive & "' ORDER BY SortOrder"
                End If
            End If

            adapter.SelectCommand = comm
            archiveTable = New DataTable()
            adapter.Fill(archiveTable)
            For Each row In archiveTable.Rows
                masks.Add(row("EasyQueryIndexName").ToString)
            Next
        Finally
            conn.Dispose()
        End Try
    End Sub

    Public Function hasNext() As Boolean
        Return (countArchive < archives.Count)
    End Function

    Public Sub resetCounter()
        countArchive = 0
    End Sub

    Public Function nextArchive() As archive
        Dim a As archive

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
' *****************  Version 3  *****************
' User: Fassbenders  Date: 6.05.08    Time: 15:55
' Updated in $/CKAG/EasyAccess
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/EasyAccess
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:09
' Created in $/CKAG/EasyAccess
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:03
' Created in $/CKAG/Components/ComArchive/Work/CKG/EasyAccess
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 29.10.07   Time: 17:36
' Updated in $/CKG/EasyAccess
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 9.10.07    Time: 18:12
' Updated in $/CKG/EasyAccess
' 
' *****************  Version 7  *****************
' User: Uha          Date: 27.08.07   Time: 17:19
' Updated in $/CKG/EasyAccess
' ITA 1269: Bugfix (ID-Spalte in vwEasyAccess hat anderen Namen bekommen)
' 
' *****************  Version 6  *****************
' User: Uha          Date: 27.08.07   Time: 13:29
' Updated in $/CKG/EasyAccess
' ITA 1269: Neue EasyAccess-Tabellen => Neue Abrage auf vwEasyAccess
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:55
' Updated in $/CKG/EasyAccess
' 
' ************************************************