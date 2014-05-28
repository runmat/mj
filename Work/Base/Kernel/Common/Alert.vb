Imports System.Web.UI.LiteralControl
Imports System.Web.UI.WebControls
Imports System.Configuration

Namespace Kernel.Common
    Public Class Alert
        Inherits System.Web.UI.Page

        Public Shared Sub alert(ByRef lit As Literal, ByVal customerID As Integer)
            Dim table As DataTable
            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim comm As New SqlClient.SqlCommand()
            Dim da As New SqlClient.SqlDataAdapter(comm)
            Dim message As String
            Dim anz As Integer
            '---------------------------------------------------
            'Nur aktivieren, wenn Schalter in Web.config gesetzt
            '---------------------------------------------------
            If ConfigurationManager.AppSettings("Serverzeitcheck") <> "ON" Then
                Exit Sub
            End If
            '-----------------------------------------------------------------------
            'Erst mal testen, ob dem Kunden überhaupt eine Onlinezeit zugewiesen ist
            '-----------------------------------------------------------------------
            comm.CommandText = "SELECT count(id) FROM LoginMessage" & _
                            " WHERE" & _
                            " enableLogin = @customer"

            comm.Parameters.AddWithValue("@customer", customerID)
            comm.Connection = conn
            Try
                conn.Open()
                anz = CType(comm.ExecuteScalar(), Integer)
            Catch ex As Exception
                conn.Dispose()
                Exit Sub
            Finally
                conn.Close()
                comm.Dispose()
            End Try
            If anz = 0 Then 'Nein? Dann raus....
                conn.Dispose()
                Exit Sub
            End If
            '-----------------------------------------------------
            'Rausschmeißen, wenn nicht innerhalb der Onlinezeit...
            '-----------------------------------------------------
            If (reject(lit, customerID) = True) Then
                Throw New Exception("Zugriff außerhalb der Onlinezeit!")    'Text egal...
            End If
            '------------------------------
            'Sonst Warnmeldung ausgeben...
            '------------------------------
            comm = New SqlClient.SqlCommand()
            comm.CommandText = "SELECT getdate() as dat,convert(varchar,activeTimeTo-getdate(),108) as timeleft,* FROM LoginMessage" & _
                    " WHERE" & _
                    " convert(varchar,getdate(),104) BETWEEN activeDateFrom AND activeDateTo" & _
                    " AND" & _
                    " convert(varchar,getdate(),108) BETWEEN (activeTimeTo - '01.01.1900 00:15:00') AND activeTimeTo" & _
                    " AND enableLogin = @customer " & _
                    " AND active <> 0 AND messageColor <> 0" & _
                    " ORDER BY id DESC"

            comm.Parameters.AddWithValue("@customer", customerID)
            comm.Connection = conn

            Try
                da = New SqlClient.SqlDataAdapter(comm)
                conn.Open()
                table = New DataTable()
                da.Fill(table)
            Catch ex As Exception
                Exit Sub
            Finally
                conn.Dispose()
                conn.Close()
                comm.Dispose()
                da.Dispose()
            End Try

            If (table.Rows.Count = 0) Then  'noch genug Onlinezeit...
            Else
                Dim cur_date As String      '15 Min. vor Schluss warnen
                Dim time_left As String

                time_left = CType(table.Rows(0)("timeleft"), String)
                cur_date = CType(table.Rows(0)("dat"), String)

                message = "\n\nSehr geehrte Benutzerin, sehr geehrter Benutzer,\nIhre verbleibende Onlinezeit beträgt\n\n" & time_left & " (Std:Min:Sek)."
                message &= "\n\nBitte beenden Sie zügig Ihre Tätigkeit und\nmelden Sie sich anschließend über den Abmeldelink ab."
                message &= "\n\nHinweis:\nBei Überschreitung der Onlinezeit werden Sie automatisch\nauf die Anmeldeseite zurückgeleitet."
                lit.Text = "		<script language=""JavaScript"">"
                lit.Text &= "			<!-- //" & vbCrLf
                lit.Text &= "				 alert(""Serverzeit: " & cur_date & message & """);"
                lit.Text &= "			//-->" & vbCrLf
                lit.Text &= "		</script>"
            End If
        End Sub

        Private Shared Function reject(ByRef lit As Literal, ByVal customerID As Integer) As Boolean
            Dim table As New DataTable
            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim comm As New SqlClient.SqlCommand()
            Dim da As New SqlClient.SqlDataAdapter(comm)
            Dim datTime As DateTime = CDate("01.01.1900 " & Now.ToShortTimeString)
            Dim message As String
            '----------------------------------------
            'Benutzer darf nicht mehr angemeldet sein
            '----------------------------------------
            comm.CommandText = "SELECT * FROM LoginMessage" & _
                    " WHERE" & _
                    " convert(varchar,getdate(),104) BETWEEN activeDateFrom AND activeDateTo" & _
                    " AND" & _
                    " convert(varchar,getdate(),108) BETWEEN activeTimeFrom AND activeTimeTo" & _
                    " AND enableLogin = @customer " & _
                    " AND active <> 0 AND messageColor <> 0" & _
                    " ORDER BY id DESC"

            comm.Parameters.AddWithValue("@customer", customerID)
            comm.Connection = conn

            Try
                da = New SqlClient.SqlDataAdapter(comm)
                conn.Open()
                da.Fill(table)
            Catch ex As Exception
            Finally
                conn.Dispose()
                conn.Close()
                da.Dispose()
            End Try
            If (table.Rows.Count = 0) Then
                message = "\n\nSehr geehrte Benutzerin, sehr geehrter Benutzer!"
                message &= "\n\nSie haben versucht, sich außerhalb der für Sie gültigen Onlinezeit anzumelden."
                message &= "\nBitte versuchen Sie es zu einem späteren Zeitpunkt erneut."
                lit.Text = "		<script language=""JavaScript"">"
                lit.Text &= "			<!-- //" & vbCrLf
                lit.Text &= "				 alert(""Serverzeit: " & Now() & message & """);"
                lit.Text &= "			//-->" & vbCrLf
                lit.Text &= "		</script>"
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace

' ************************************************
' $History: Alert.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Common
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA:1440
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Common
' 
' ************************************************