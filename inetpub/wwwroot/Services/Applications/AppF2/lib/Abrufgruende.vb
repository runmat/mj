Imports CKG.Base.Kernel.Security

Public Class Abrufgruende
    Private _user As User
    Public Sub New(ByVal user As User)
        _user = user
    End Sub

    Private _result As DataTable
    Public ReadOnly Property Result As DataTable
        Get
            If _result Is Nothing Then
                Using cnn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                    cnn.Open()
                    Using cmd = cnn.CreateCommand()
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = "SELECT GrundID, WebBezeichnung, SapWert, MitZusatzText, " & _
                            "Zusatzbemerkung, VersandadressArt, Eingeschraenkt FROM CustomerAbrufgruende " & _
                            "WHERE CustomerID=@cID AND GroupID=@gID AND AbrufTyp='endg';"
                        cmd.Parameters.AddWithValue("@cID", _user.Customer.CustomerId)
                        cmd.Parameters.AddWithValue("@gID", _user.GroupID)

                        Using reader = cmd.ExecuteReader
                            _result = New DataTable
                            _result.Load(reader)
                        End Using
                    End Using
                    cnn.Close()
                End Using
            End If

            If _result.Rows.Count = 0 Then Throw New ApplicationException("Keine Abrufgründe für den Kunden definiert!")

            Return _result
        End Get
    End Property
End Class
