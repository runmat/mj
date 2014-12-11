Public Class Aktuelles
#Region " Membervariables "
    Private m_strConnectionstring As String
    Private m_intID As Integer
    Private m_CustomerID As Integer
    Private m_blnIsNew As Boolean = False
    Private m_blnDelete As Boolean = False
    Private m_strError As String
    Private m_tblUebersicht As DataTable
#End Region

#Region " Constructor "
    Public Sub New()
        m_blnIsNew = False
        m_strError = ""
    End Sub

#End Region
#Region " Properties "
    Public Property Id() As Integer
        Get
            Return m_intID
        End Get
        Set(ByVal value As Integer)
            m_intID = value
        End Set
    End Property

    Public Property IsNew() As Boolean
        Get
            Return m_blnIsNew
        End Get
        Set(ByVal value As Boolean)
            m_blnIsNew = value
        End Set
    End Property

    Public Property IsDeleted() As Boolean
        Get
            Return m_blnDelete
        End Get
        Set(ByVal value As Boolean)
            m_blnDelete = value
        End Set
    End Property
    Public ReadOnly Property tblUebersicht() As DataTable
        Get
            Return m_tblUebersicht
        End Get
    End Property
    Public Property clsError() As String
        Get
            Return m_strError
        End Get
        Set(ByVal value As String)
            m_strError = value
        End Set
    End Property
    Public Property CustomerID() As Integer
        Get
            Return m_CustomerID
        End Get
        Set(ByVal value As Integer)
            m_CustomerID = value
        End Set
    End Property
#End Region

#Region " Functions "
    Public Sub GetCustomerNews(ByVal intID As Integer, ByVal cn As SqlClient.SqlConnection)

        Dim blnCloseOnEnd As Boolean = False
        CustomerID = intID
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If


        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT " & _
                                "dbo.CustomerNews.ID, " & _
                                "dbo.Customer.CustomerID, " & _
                                "dbo.Customer.Customername, " & _
                                "dbo.CustomerNews.BeitragName, " & _
                                "dbo.CustomerNews.GueltigBis, " & _
                                "dbo.CustomerNews.aktiv " & _
                                "FROM dbo.CustomerNews INNER JOIN " & _
                            " dbo.Customer ON dbo.CustomerNews.CustomerID = dbo.Customer.CustomerID  "

            If intID <> 0 Then
                strSQL &= " Where dbo.Customer.CustomerID=" & CustomerID
            End If

            strSQL &= " ORDER BY dbo.Customer.Customername, dbo.CustomerNews.BeitragName "

            m_tblUebersicht = New DataTable
            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblUebersicht)

        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try
    End Sub

    Public Sub Delete(ByVal strConnectionString As String)
        Try
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            cn.Open()
            Delete(cn)
        Catch ex As Exception
            Throw New Exception("Fehler beim Löschen eines Eintrages!", ex)
        End Try
    End Sub
    Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
        Try
            Dim strSqlDelete As String = "DELETE " & _
                                           "FROM CustomerNews " & _
                                           "WHERE [ID]=@ID"

            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn
            cmd.Parameters.AddWithValue("@Id", m_intID)

            'CustomerNews loeschen
            cmd.CommandText = strSqlDelete
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw New Exception("Fehler beim Löschen eines Eintrages!", ex)
        End Try
    End Sub

#End Region
End Class
