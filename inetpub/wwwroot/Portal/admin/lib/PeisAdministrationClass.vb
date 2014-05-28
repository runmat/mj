Public Class PeisAdministrationClass

#Region "Deklarations"

    Private mKeyWords As DataTable
    Private mFilter As PeisFilter
    Private Connection As New SqlClient.SqlConnection()




#End Region
#Region "Properties"

    Public ReadOnly Property Filter() As DataTable
        Get
            Return mFilter
        End Get
    End Property

    Public ReadOnly Property KeyWords() As DataTable
        Get
            If mKeyWords Is Nothing Then
                mKeyWords = New DataTable
                mKeyWords.Columns.Add("KeyWord", String.Empty.GetType)
            End If
            Return mKeyWords
        End Get
    End Property

#End Region


#Region "Methodes"


    Public Sub New()
        Connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
        mFilter = New PeisFilter(ConfigurationManager.AppSettings("Connectionstring"))
    End Sub

    Public Sub fillFilter()
        mFilter.refill()
    End Sub


    Public Sub deleteFilter(ByVal FilterID As String)
        Try
            Dim cmd As New SqlClient.SqlCommand
            Connection.Open()
            cmd.Connection = Connection
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String

            SqlQuery = "Delete From PeisFilter " & _
                            "WHERE FilterID=" & FilterID & ";"
            cmd.CommandText = SqlQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            Connection.Close()
        End Try

    End Sub

    Public Sub changeEnabled(ByVal FilterID As String, ByVal newValue As String)
        Try
            Dim cmd As New SqlClient.SqlCommand
            Connection.Open()
            cmd.Connection = Connection
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String

            SqlQuery = "Update PeisFilter " & _
            "SET FilterEnabled='" & newValue & "' " & _
            "WHERE FilterID=" & FilterID & ";"

            cmd.CommandText = SqlQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            Connection.Close()
        End Try

    End Sub

    Public Sub insertFilter(ByVal FehlerName As String, ByVal FehlerBeschreibung As String, ByVal FehlerBeispiel As String)
        Try
            Dim cmd As New SqlClient.SqlCommand
            Connection.Open()
            cmd.Connection = Connection
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String

            Dim KeyWordString As String = ""

            For Each tmprow As DataRow In KeyWords.Rows
                KeyWordString &= tmprow(0) & ","
            Next

            SqlQuery = "INSERT INTO PeisFilter" & _
             " (KeyWords, FehlerName, FehlerBeispiel, FehlerBeschreibung) " & _
             "VALUES ('" & KeyWordString & "','" & FehlerName & "', '" & FehlerBeispiel & "', '" & FehlerBeschreibung & "');"
            cmd.CommandText = SqlQuery
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        Finally
            Connection.Close()
        End Try


    End Sub

#End Region

End Class
' ************************************************
' $History: PeisAdministrationClass.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.07.09   Time: 13:33
' Updated in $/CKAG/admin/lib
' historie hinzugefügt
' 
' ************************************************