Imports System.Configuration
Imports System.Data.SqlClient
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Namespace Kernel.Admin

    Public Class RightList 
        Inherits DataTable

#Region " Membervariables "
        Private m_cn As SqlClient.SqlConnection
        Private m_intCustomerID As Integer
        Private m_intGroupID As Integer
        Private m_blnTableFilled As Boolean = False
        Private m_intHighestAuthorizationlevel As Int32



#End Region

#Region " Properties"
        Public ReadOnly Property HighestAuthorizationlevel() As Int32
            Get
                Return m_intHighestAuthorizationlevel
            End Get
        End Property
#End Region

#Region " Constructor "

        Public Sub New(ByVal intCustomerId As Integer, ByVal cn As SqlClient.SqlConnection)

            m_cn = cn
            m_intCustomerID = intCustomerId
            m_intGroupID = -2 'Indikator dafuer, dass nicht nach Gruppe gesucht wird.
            m_intHighestAuthorizationlevel = 0 '(noch) keine Anwendung gefunden, die Autorisierung erfordert

        End Sub

#End Region

#Region " Functions "

        Public Sub GetAllPossibleRightsforThisCustomer()

            Dim strSql As String
            Dim daRights As New SqlClient.SqlDataAdapter()

            daRights.SelectCommand = New SqlClient.SqlCommand()
            daRights.SelectCommand.Connection = m_cn

            If m_intGroupID = -2 Then
                
                strSql = " SELECT CustomerID, dbo.CategorySettingsCustomer.CategoryID AS CategoryID, HasSettings, "
                strSql += "Description, EditUserName, EditDate"
                strSql += " FROM dbo.CategorySettingsCustomer LEFT OUTER JOIN "
                strSql += " dbo.CategorySettingsMetadata ON dbo.CategorySettingsCustomer.CategoryID = dbo.CategorySettingsMetadata.CategoryID "
                strSql += "WHERE (CustomerID = @CustomerID) "

            End If

            daRights.SelectCommand.CommandText = strSql
            daRights.SelectCommand.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
            daRights.Fill(Me)

        End Sub

        Public Shared Sub UpdateSingleRightPerCustomer(ByVal customerId As Integer, ByVal categoryId As String, _
                                                       ByVal isChecked As Boolean, ByVal strUsernameBearbeiter As String)

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sSQl As String = ""

            sSQl = "UPDATE CategorySettingsCustomer SET "
            sSQl += "HasSettings = '" & isChecked & "', "
            sSQl += "EditUserName = '" & strUsernameBearbeiter & "', "
            sSQl += "EditDate = '" & Date.Now() & "'"
            sSQl += " WHERE "
            sSQl += "CustomerID = " & customerId
            sSQl += " AND "
            sSQl += "CategoryID = '" & categoryId & "'"

            Try
                cn.Open()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text
                cmd.CommandText = sSQl
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Update der CustomerRights fehlgeschlagen")
            Finally
                cn.Close()
            End Try

        End Sub
        
        Public Shared Function InsertOrDeleteRightForAllUsersOfThisCustomer(ByVal customerId As Integer, ByVal categoryId As String, _
                                                     ByVal isChecked As Boolean, ByVal strUsernameBearbeiter As String)

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim sSql As String

            sSql = "SELECT UserID, Username FROM WebUser WHERE customerID = " & customerId


            Using cn
                Dim command As SqlCommand = New SqlCommand(sSql, cn)
                cn.Open()

                Dim reader As SqlDataReader = command.ExecuteReader()

                If reader.HasRows Then
                    Do While reader.Read()
                        InsertOrDeleteRightForSingleUser(customerId, categoryId, isChecked, strUsernameBearbeiter, reader.GetString(1))
                        '  System.Diagnostics.Debug.WriteLine(reader.GetInt32(0) & vbTab & reader.GetString(1))
                    Loop
                Else
                    Console.WriteLine("No rows found.")
                End If

                reader.Close()
            End Using

            Return Nothing



        End Function




        Private Shared Sub InsertOrDeleteRightForSingleUser(ByVal customerId As Integer, ByVal categoryId As String, _
                                                     ByVal isChecked As Boolean, ByVal strUsernameBearbeiter As String, ByVal strUserId As String)


            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sSQl As String = ""


            If isChecked = True Then

                ' Wie wird der Username des entsprechenden ZielUsers ermittelt? Sie wird aus dem Form ausgelesen, Name des Feldes ist txtUserID
                ' Was ist mit der CustomerID - mit rein?

                sSql += "IF NOT EXISTS("
                sSql += "SELECT CategoryID FROM [CategorySettingsWebUser] "
                sSql += "WHERE "
                sSql += "UserName LIKE '" & strUserId & "' "
                sSql += "AND "
                sSql += "CategoryID LIKE '" & categoryId & "'"
                sSql += ")"
                sSql += "INSERT INTO [CategorySettingsWebUser] "
                sSql += "(UserName, CategoryID, SettingsValue, EditUserName, EditDate)"
                sSql += "VALUES "
                sSQl += "('" & strUserId & "', '" & categoryId & "', 'false', '" & strUsernameBearbeiter & "', CURRENT_TIMESTAMP)  "
                ' Hier noch die CustomerID rein? 2x?

            Else

                sSql += "DELETE FROM [CategorySettingsWebUser] "
                sSql += "WHERE "
                sSql += "UserName LIKE '" & strUserId & "' "
                sSql += "AND "
                sSQl += "CategoryID LIKE '" & categoryId & "'"
                ' hier noch die customerID rein?
                
            End If

            Try
                cn.Open()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text
                cmd.CommandText = sSQl
                
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Throw New Exception("Problem eim Inserten / Deleten der Rechte für den User " & strUserId)
            Finally
                cn.Close()
            End Try
            System.Diagnostics.Debug.WriteLine("Das SQL Statement ist: ")
            System.Diagnostics.Debug.WriteLine(sSql)

        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: ApplicationList.vb $
' 

' ************************************************