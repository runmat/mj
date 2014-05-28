Public Class Logging

    Public Sub InsertPlatinen(ByVal Kostenstelle As String, _
                              ByVal Lieferant As String, _
                              ByVal Verkaeufer As String, _
                              ByVal PosTable As DataTable, _
                              ByVal Message As String, ByVal Nummer As String)

        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim cmd As New SqlClient.SqlCommand

        Try
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.Text
            Dim SqlQuery As String
            SqlQuery = "INSERT INTO [KBS_EFA_Logging] (Kostenstelle, Lieferant, Verkaeufer, Artikelnummer, Menge,Datum,Message,Nummer) " & _
            "VALUES (@Kostenstelle, @Lieferant, @Verkaeufer, @Artikelnummer, @Menge, @Datum, @Message, @Nummer);"
            With cmd
                .Parameters.Add("@Kostenstelle", SqlDbType.NVarChar)
                .Parameters.Add("@Lieferant", SqlDbType.NVarChar)
                .Parameters.Add("@Verkaeufer", SqlDbType.NVarChar)
                .Parameters.Add("@Artikelnummer", SqlDbType.NVarChar)
                .Parameters.Add("@Menge", SqlDbType.NVarChar)
                .Parameters.Add("@Datum", SqlDbType.NVarChar)
                .Parameters.Add("@Message", SqlDbType.NVarChar)
                .Parameters.Add("@Nummer", SqlDbType.NVarChar)
            End With

            cmd.CommandText = SqlQuery

            cmd.Parameters("@Kostenstelle").Value = Kostenstelle
            cmd.Parameters("@Lieferant").Value = Lieferant
            cmd.Parameters("@Verkaeufer").Value = Verkaeufer
            cmd.Parameters("@Message").Value = Message
            cmd.Parameters("@Datum").Value = CStr(Date.Now)
            cmd.Parameters("@Nummer").Value = Nummer
            If PosTable.Rows.Count > 0 Then

                For Each tmpRow As DataRow In PosTable.Rows
                    With cmd
                        .Parameters("@Artikelnummer").Value = tmpRow("ARTLIF").ToString
                        .Parameters("@Menge").Value = tmpRow("Menge").ToString
                    End With

                    cmd.ExecuteNonQuery()
                Next
            Else

                cmd.Parameters("@Artikelnummer").Value = ""
                cmd.Parameters("@Menge").Value = ""

                cmd.ExecuteNonQuery()

            End If

        Catch ex As Exception
            'Throw New Exception(ex.Message)
        Finally
            cn.Close()
        End Try

    End Sub

End Class
