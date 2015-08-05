Public Class Rechnungsdatenanhang

    Public Shared Function LoadTemplates(ByVal strConnectionstring As String, ByRef tbl As DataTable) As String
        Dim strError As String = ""

        Using cn As New SqlClient.SqlConnection(strConnectionstring)
            Try
                cn.Open()

                Using da As New SqlClient.SqlDataAdapter("SELECT * FROM RechnungsanhangTemplates ORDER BY ID", cn)
                    da.Fill(tbl)
                End Using
            Catch ex As Exception
                strError = "Fehler beim Datenbankzugriff: " & ex.Message
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Using

        Return strError
    End Function

    Public Shared Function SaveTemplate(ByVal strConnectionstring As String, ByVal intID As Integer, ByVal strBezeichnung As String, ByVal intDatenAbZeile As Integer,
                                        ByVal strSpalteKennzeichen As String, ByVal strSpalteGebuehren As String, ByVal strSpalteZulassungsdatum As String) As String
        Dim strError As String = ""

        Using cn As New SqlClient.SqlConnection(strConnectionstring)
            Try
                cn.Open()

                Using cmd As SqlClient.SqlCommand = cn.CreateCommand()
                    cmd.CommandType = CommandType.Text

                    If intID > 0 Then
                        cmd.CommandText = "UPDATE RechnungsanhangTemplates SET Bezeichnung = @Bezeichnung, DatenAbZeile = @DatenAbZeile, SpalteKennzeichen = @SpalteKennzeichen, SpalteGebuehren = @SpalteGebuehren, SpalteZulassungsdatum = @SpalteZulassungsdatum WHERE ID = @ID;"
                    Else
                        cmd.CommandText = "INSERT INTO RechnungsanhangTemplates (Bezeichnung, DatenAbZeile, SpalteKennzeichen, SpalteGebuehren, SpalteZulassungsdatum) VALUES (@Bezeichnung, @DatenAbZeile, @SpalteKennzeichen, @SpalteGebuehren, @SpalteZulassungsdatum);"
                    End If

                    cmd.Parameters.AddWithValue("@Bezeichnung", strBezeichnung)
                    cmd.Parameters.AddWithValue("@DatenAbZeile", intDatenAbZeile)
                    cmd.Parameters.AddWithValue("@SpalteKennzeichen", strSpalteKennzeichen)
                    cmd.Parameters.AddWithValue("@SpalteGebuehren", strSpalteGebuehren)
                    cmd.Parameters.AddWithValue("@SpalteZulassungsdatum", strSpalteZulassungsdatum)
                    If intID > 0 Then cmd.Parameters.AddWithValue("@ID", intID)

                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                strError = "Fehler beim Datenbankzugriff: " & ex.Message
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Using

        Return strError
    End Function

    Public Shared Function DeleteTemplate(ByVal strConnectionstring As String, ByVal intID As Integer) As String
        Dim strError As String = ""

        Using cn As New SqlClient.SqlConnection(strConnectionstring)
            Try
                cn.Open()

                Using cmd As SqlClient.SqlCommand = cn.CreateCommand()
                    cmd.CommandType = CommandType.Text

                    cmd.CommandText = "DELETE FROM RechnungsanhangTemplates WHERE ID = @ID;"

                    cmd.Parameters.AddWithValue("@ID", intID)

                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                strError = "Fehler beim Datenbankzugriff: " & ex.Message
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Using

        Return strError
    End Function

End Class
