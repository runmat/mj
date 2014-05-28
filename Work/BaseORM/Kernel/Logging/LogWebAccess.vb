Imports System.Configuration
Imports System.Data.SqlClient

Namespace Kernel.Logging
    Public Class LogWebAccess
        Inherits System.Web.UI.Page     'Für die SessionID

        Private userName As String
        Private idSession As String
        Private hostAddress As String
        Private hostName As String
        Private requestPort As Int32
        Private requestType As String
        Private browser As String
        Private idLogWebAccess As Int32
        Private blnLogDurationASPX As Boolean
        Private blnClearDurationASPX As Boolean
        Private idLogAccessASPX As Integer

        Public Property LogAccessASPXID() As Integer
            Get
                Return idLogAccessASPX
            End Get
            Set(ByVal Value As Integer)
                idLogAccessASPX = Value
            End Set
        End Property

        Public Property ClearDurationASPX() As Boolean
            Get
                Return blnClearDurationASPX
            End Get
            Set(ByVal Value As Boolean)
                blnClearDurationASPX = Value
            End Set
        End Property

        Public Property LogDurationASPX() As Boolean
            Get
                Return blnLogDurationASPX
            End Get
            Set(ByVal Value As Boolean)
                blnLogDurationASPX = Value
            End Set
        End Property

        Public ReadOnly Property LogWebAccessID() As Int32
            Get
                Return idLogWebAccess
            End Get
        End Property

        Public Function getUser() As String
            Return userName
        End Function

        Public Function getSession() As String
            Return idSession
        End Function


        Public Sub New(ByVal req As System.Web.HttpRequest, ByVal user As Security.User) 'Klassenmethode
            Dim cmd As New SqlCommand()
            userName = user.UserName
            idSession = Session.SessionID()
            hostAddress = req.ServerVariables("REMOTE_ADDR")
            hostName = ""
            requestPort = req.Url.Port
            requestType = req.RequestType
            browser = req.Browser.Browser
            idLogWebAccess = -1

            Dim par_1 As New SqlParameter("@userName", userName)
            Dim par_2 As New SqlParameter("@idSession", idSession)
            Dim par_3 As New SqlParameter("@hostAddress", hostAddress)
            Dim par_4 As New SqlParameter("@hostName", hostName)
            Dim par_5 As New SqlParameter("@requestPort", requestPort)
            Dim par_6 As New SqlParameter("@requestType", requestType)
            Dim par_7 As New SqlParameter("@browser", browser)
            Dim par_8 As New SqlParameter("@startTime", Date.Now)

            cmd.Parameters.Add(par_1)
            cmd.Parameters.Add(par_2)
            cmd.Parameters.Add(par_3)
            cmd.Parameters.Add(par_4)
            cmd.Parameters.Add(par_5)
            cmd.Parameters.Add(par_6)
            cmd.Parameters.Add(par_7)
            cmd.Parameters.Add(par_8)

            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

            cmd.CommandText = "INSERT INTO LogWebAccess (userName,idSession,hostAddress,hostName,requestPort,requestType,browser,startTime)" _
                                    & " VALUES (@userName,@idSession,@hostAddress,@hostName,@requestPort,@requestType,@browser,@startTime)" _
                                    & "; SELECT IDENT_CURRENT('LogWebAccess')"

            cmd.Connection = conn
            Try
                conn.Open()
                idLogWebAccess = CInt(cmd.ExecuteScalar)
                conn.Close()
                conn.Dispose()
                cmd.Dispose()
            Catch
            End Try
        End Sub

        Public Sub setStartTimeASPX(ByVal strPage As String, ByVal intAppID As Integer)
            If blnLogDurationASPX Then
                Dim cmd As New SqlCommand()
                Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

                Try
                    conn.Open()
                    cmd.Connection = conn

                    cmd.CommandText = "INSERT INTO LogAccessASPX (LogWebAccessID,Seite,AppID)" _
                                            & " VALUES (@LogWebAccessID,@Seite,@AppID)" _
                                            & "; SELECT IDENT_CURRENT('LogAccessASPX')"
                    cmd.Parameters.AddWithValue("@LogWebAccessID", idLogWebAccess)
                    cmd.Parameters.AddWithValue("@Seite", strPage)
                    cmd.Parameters.AddWithValue("@AppID", intAppID)
                    idLogAccessASPX = CInt(cmd.ExecuteScalar)
                    conn.Close()
                    conn.Dispose()
                    cmd.Dispose()
                Catch ex As Exception
                    Dim strTest As String = ex.ToString
                End Try
            End If
        End Sub

        Public Sub setEndTimeASPX()
            If blnLogDurationASPX Then
                Dim cmd As New SqlCommand()
                Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

                Try
                    conn.Open()
                    cmd.Connection = conn
                    cmd.Parameters.AddWithValue("@LogWebAccessID", idLogWebAccess)

                    cmd.CommandText = "UPDATE LogAccessASPX SET EndZeit=GetDate()" _
                                            & " WHERE (LogWebAccessID=@LogWebAccessID)" _
                                            & " AND (EndZeit IS NULL)"
                    cmd.ExecuteNonQuery()

                    If blnClearDurationASPX Then
                        cmd.CommandText = "DELETE LogAccessASPX " _
                                                & " WHERE (LogWebAccessID=@LogWebAccessID)" _
                                                & " AND (DATEDIFF(s,StartZeit,EndZeit)<2)"
                        cmd.ExecuteNonQuery()
                    End If
                Catch ex As Exception
                    Dim strTest As String = ex.ToString
                Finally
                    conn.Close()
                    conn.Dispose()
                    cmd.Dispose()
                End Try
            End If
        End Sub

        Public Sub updateEndTime(ByVal mode As Boolean)
            Dim sql As String
            If (mode = True) Then
                'Nicht über den Abmeldebutton abgemeldet
                sql = "UPDATE LogWebAccess SET hostName = 'Timeout', endTime = '" & Date.Now & "' WHERE idSession = '" & Me.idSession & "'"
            Else
                sql = "UPDATE LogWebAccess SET hostName = 'Ok', endTime = '" & Date.Now & "' WHERE idSession = '" & Me.idSession & "'"
            End If
            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand(sql, conn)
            Try
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
                conn.Dispose()
                cmd.Dispose()
            Catch
            End Try
        End Sub
    End Class
End Namespace

' ************************************************
' $History: LogWebAccess.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Logging
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Kernel/Logging
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 20.06.07   Time: 16:30
' Updated in $/CKG/Base/Base/Kernel/Logging
' Es werden jetzt alle ASPX Logeintrage gelöscht, die weniger als 2
' Sekunden gedauert haben (falls Key in Web.config gesetzt)
' 
' *****************  Version 5  *****************
' User: Uha          Date: 20.06.07   Time: 16:18
' Updated in $/CKG/Base/Base/Kernel/Logging
' Parameter ClearDurationASPX eingebracht
' 
' *****************  Version 4  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Base/Base/Kernel/Logging
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Logging
' 
' ************************************************