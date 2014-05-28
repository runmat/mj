Public Class Mails
#Region " Membervariables "
    Private m_blnIsNew As Boolean = False
    Private m_strConnectionstring As String
    Private m_intID As Integer
    Private m_CustomerID As Integer
    Private m_blnDelete As Boolean = False
    Private m_strError As String
    Private m_tblUebersicht As DataTable
#End Region


#Region "Properties"

    Public Property CustomerID() As Integer
        Get
            Return m_CustomerID
        End Get
        Set(ByVal value As Integer)
            m_CustomerID = value
        End Set
    End Property

    Public Property ClassError() As String
        Get
            Return m_strError
        End Get
        Set(ByVal value As String)
            m_strError = value
        End Set
    End Property
#End Region

#Region " Functions "
    Public Function GetMailList(ByVal KundenID As Integer, ByVal cn As SqlClient.SqlConnection) As DataTable

        Dim blnCloseOnEnd As Boolean = False
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If


        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT dbo.EmailAdressen.MailID, dbo.EmailAdressen.EmailAdresse " & _
                        "FROM dbo.EmailAdressen " & _
                        "WHERE dbo.EmailAdressen.KundenID = " & KundenID

            strSQL &= " ORDER BY dbo.EmailAdressen.MailID"

            m_tblUebersicht = New DataTable
            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblUebersicht)
            GetMailList = m_tblUebersicht
        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try
        GetMailList = m_tblUebersicht
    End Function

    Public Function GetMailListOhneEmpfaenger(ByVal KundenID As Integer, ByVal TextID As Integer, ByVal cn As SqlClient.SqlConnection) As DataTable

        Dim blnCloseOnEnd As Boolean = False
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If


        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT " & _
                        "dbo.EmailAdressen.MailID, " & _
                        "dbo.EmailAdressen.EmailAdresse " & _
                        "FROM dbo.EmailAdressen " & _
                        "WHERE dbo.EmailAdressen.KundenID = " & KundenID & _
                        " AND NOT dbo.EmailAdressen.MailID = (SELECT TOP 100 PERCENT dbo.EmailVersand.EmpfängerID " & _
                        " FROM dbo.EmailVersand " & _
                        "WHERE dbo.EmailVersand.TextID = " & TextID & ")"

            m_tblUebersicht = New DataTable
            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblUebersicht)
            GetMailListOhneEmpfaenger = m_tblUebersicht
        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try
        GetMailListOhneEmpfaenger = m_tblUebersicht
    End Function

    Public Function GetTextList(ByVal KundenID As Integer, ByVal cn As SqlClient.SqlConnection) As DataTable

        Dim blnCloseOnEnd As Boolean = False
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If


        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT " & _
                                "TextID, " & _
                                "Betreff " & _
                                "FROM EmailTexte"

            If KundenID <> 0 Then
                strSQL &= " Where dbo.EmailTexte.KundenID=" & KundenID
            End If

            strSQL &= " ORDER BY dbo.EmailTexte.TextID"

            m_tblUebersicht = New DataTable
            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblUebersicht)
            GetTextList = m_tblUebersicht
        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try
        GetTextList = m_tblUebersicht
    End Function

    Public Function GetVorgangsnummernList(ByVal cn As SqlClient.SqlConnection, ByVal KundenID As Integer) As DataTable

        Dim blnCloseOnEnd As Boolean = False
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If


        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT  " & _
                                "Vorgangsnummer " & _
                                "FROM EmailTexte Where KundenID=" & KundenID

            m_tblUebersicht = New DataTable
            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblUebersicht)
            GetVorgangsnummernList = m_tblUebersicht
        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try
        GetVorgangsnummernList = m_tblUebersicht
    End Function

    Public Function TextAllByTextID(ByVal TextID As Integer, ByVal cn As SqlClient.SqlConnection) As DataTable
        Dim ResultTable As New DataTable
        Dim blnCloseOnEnd As Boolean = False

        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If

        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT *" & _
                                "FROM EmailTexte " & _
                                "WHERE TextID = " & TextID

            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(ResultTable)

        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try
        TextAllByTextID = ResultTable
    End Function

    Public Function TextByTextID(ByVal TextID As Integer, ByVal cn As SqlClient.SqlConnection) As String
        Dim Text As String
        Dim m_tblText As New DataTable
        Dim blnCloseOnEnd As Boolean = False
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If


        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT " & _
                                "Text " & _
                                "FROM EmailTexte " & _
                                "WHERE TextID = " & TextID

            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblText)

        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try
        Text = m_tblText.Rows(0)("Text").ToString
        TextByTextID = Text
    End Function

    Public Function VorgangsnummerByTextID(ByVal TextID As Integer, ByVal cn As SqlClient.SqlConnection) As String
        Dim Text As String
        Dim m_tblText As New DataTable
        Dim blnCloseOnEnd As Boolean = False
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If

        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT " & _
                                "Vorgangsnummer " & _
                                "FROM EmailTexte " & _
                                "WHERE TextID = " & TextID

            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblText)

        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try
        Text = m_tblText.Rows(0)("Vorgangsnummer").ToString
        VorgangsnummerByTextID = Text
    End Function

    Public Function AktivByTextID(ByVal TextID As Integer, ByVal cn As SqlClient.SqlConnection) As Boolean
        Dim Aktiv As Boolean
        Dim m_tblText As New DataTable
        Dim blnCloseOnEnd As Boolean = False
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If

        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT " & _
                                "Aktiv " & _
                                "FROM EmailTexte " & _
                                "WHERE TextID = " & TextID

            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblText)

        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try

        Try
            Aktiv = CBool(m_tblText.Rows(0)("Aktiv"))
        Catch ex As Exception
            Aktiv = False
        End Try

        AktivByTextID = Aktiv
    End Function

    Public Function GetEmpfaengerList(ByVal TextID As Integer, ByVal KundenID As Integer, ByVal cn As SqlClient.SqlConnection) As DataTable
        Dim m_tblEmpfaenger As New DataTable
        Dim blnCloseOnEnd As Boolean = False
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If

        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT " & _
                        "dbo.EmailVersand.EmailVersandID, " & _
                        "dbo.EmailVersand.[EmpfängerID], " & _
                        "dbo.EmailVersand.TextID, " & _
                        "dbo.EmailVersand.CC, " & _
                        "dbo.EmailAdressen.KundenID, " & _
                        "dbo.EmailAdressen.EmailAdresse " & _
                        "FROM dbo.EmailVersand INNER JOIN dbo.EmailAdressen ON dbo.EmailVersand.[EmpfängerID] = dbo.EmailAdressen.MailID " & _
                        "WHERE dbo.EmailVersand.TextID = " & TextID & " AND dbo.EmailAdressen.KundenID = " & KundenID & _
                        " AND dbo.EmailVersand.CC = 0"

            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblEmpfaenger)

        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try

        GetEmpfaengerList = m_tblEmpfaenger
    End Function

    Public Function GetEmpfaengerListCC(ByVal TextID As Integer, ByVal KundenID As Integer, ByVal cn As SqlClient.SqlConnection) As DataTable
        Dim m_tblEmpfaenger As New DataTable
        Dim blnCloseOnEnd As Boolean = False
        If cn.State = ConnectionState.Closed Then
            cn.Open()
            blnCloseOnEnd = True
        End If

        Try

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT " & _
                        "dbo.EmailVersand.EmailVersandID, " & _
                        "dbo.EmailVersand.[EmpfängerID], " & _
                        "dbo.EmailVersand.TextID, " & _
                        "dbo.EmailVersand.CC, " & _
                        "dbo.EmailAdressen.KundenID, " & _
                        "dbo.EmailAdressen.EmailAdresse " & _
                        "FROM dbo.EmailVersand INNER JOIN dbo.EmailAdressen ON dbo.EmailVersand.[EmpfängerID] = dbo.EmailAdressen.MailID " & _
                        "WHERE dbo.EmailVersand.TextID = " & TextID & " AND dbo.EmailAdressen.KundenID = " & KundenID & _
                        " AND dbo.EmailVersand.CC = 1"

            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblEmpfaenger)

        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try

        GetEmpfaengerListCC = m_tblEmpfaenger
    End Function
#End Region

#Region "Methods"
    Public Sub NewMail(ByVal Mailadresse As String, ByVal KundenID As Integer, ByVal cn As SqlClient.SqlConnection)

        If Mailadresse.Contains("@") And Mailadresse.Contains(".") Then
            Dim blnCloseOnEnd As Boolean = False
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If

            Try
                Dim strSQL As String

                strSQL = "INSERT INTO dbo.EmailAdressen(EMailAdresse, KundenID) " & _
                            "VALUES ('" & Mailadresse & "', " & KundenID & ")"

                Dim com As New SqlClient.SqlCommand(strSQL, cn)
                com.ExecuteNonQuery()

            Catch ex As Exception
                m_strError = "Fehler beim anlegen der Daten"
            Finally
                cn.Close()
            End Try
        Else
            m_strError = "Ungültige E-Mail-Adresse, es wurde kein Datensatz angelegt."
        End If
    End Sub

    Public Sub NewText(ByVal Text As String, ByVal Betreff As String, ByVal KundenID As Integer, ByVal Vorgangsnummer As String, ByVal Aktiv As Boolean, ByVal cn As SqlClient.SqlConnection)
        Dim m_tblVorgangsnummer As New DataTable
        If Betreff <> "" Then
            If Text <> "" Then
                If Vorgangsnummer <> "" Then

                    Dim blnCloseOnEnd As Boolean = False
                    If cn.State = ConnectionState.Closed Then
                        cn.Open()
                        blnCloseOnEnd = True
                    End If

                    Dim strSelect As String

                    strSelect = "SELECT Vorgangsnummer " & _
                                    "FROM dbo.EmailTexte " & _
                                    "WHERE Vorgangsnummer LIKE '" & Vorgangsnummer & "' AND KundenID=" & KundenID

                    Dim da As New SqlClient.SqlDataAdapter(strSelect, cn)
                    da.Fill(m_tblVorgangsnummer)

                    If m_tblVorgangsnummer.Rows.Count < 1 Then
                        Try
                            Dim strSQL As String

                            strSQL = "INSERT INTO dbo.EmailTexte(Text, Betreff, KundenID, Vorgangsnummer, Aktiv) " & _
                                        "VALUES ('" & Text & "', '" & Betreff & "', " & KundenID & ", '" & Vorgangsnummer & "', " & CInt(Aktiv) & ")"

                            Dim com As New SqlClient.SqlCommand(strSQL, cn)
                            com.ExecuteNonQuery()

                        Catch ex As Exception
                            m_strError = "Fehler beim anlegen der Daten"
                        Finally
                            cn.Close()
                        End Try
                    Else
                        m_strError = "Vorgangsnummer existiert bereits"
                    End If
                Else
                    m_strError = "Text ohne Vorgangsnummer"
                End If
            Else
                m_strError = "Mail ohne Text"
            End If
        Else
            m_strError = "Mail ohne Betreff"
        End If
    End Sub

    Public Sub UpdateText(ByVal TextID As Integer, ByVal Text As String, ByVal Betreff As String, ByVal KundenID As Integer, ByVal Vorgangsnummer As String, ByVal Aktiv As Boolean, ByVal cn As SqlClient.SqlConnection)
        Dim m_tblVorgangsnummer As New DataTable
        If Betreff <> "" Then
            If Text <> "" Then
                If Vorgangsnummer <> "" Then

                    Dim blnCloseOnEnd As Boolean = False
                    If cn.State = ConnectionState.Closed Then
                        cn.Open()
                        blnCloseOnEnd = True
                    End If

                    Try
                        Dim strSQL As String

                        strSQL = "UPDATE dbo.EmailTexte " & _
                                    "SET KundenID = " & KundenID & ", " & _
                                    "Text = '" & Text & "', " & _
                                    "Betreff = '" & Betreff & "', " & _
                                    "Vorgangsnummer = '" & Vorgangsnummer & "', " & _
                                    "Aktiv = 0" & _
                                    " WHERE TextID = " & TextID

                        Dim com As New SqlClient.SqlCommand(strSQL, cn)
                        com.ExecuteNonQuery()

                    Catch ex As Exception
                        m_strError = "Fehler beim anlegen der Daten"
                    Finally
                        cn.Close()
                    End Try
                Else
                    m_strError = "Text ohne Vorgangsnummer"
                End If
            Else
                m_strError = "Mail ohne Text"
            End If
        Else
            m_strError = "Mail ohne Betreff"
        End If
    End Sub
    Public Sub AktivChange(ByVal TextID As Integer, ByVal Aktiv As Boolean, ByVal cn As SqlClient.SqlConnection)
        Dim strSQL As String
        Try
            cn.Open()
            strSQL = "UPDATE dbo.EmailTexte " & _
                        "SET Aktiv = " & CInt(Aktiv) & _
                        " WHERE TextID = " & TextID

            Dim com As New SqlClient.SqlCommand(strSQL, cn)
            com.ExecuteNonQuery()

        Catch ex As Exception
            m_strError = "Fehler beim anlegen der Daten"
        Finally
            cn.Close()
        End Try
    End Sub

    Sub Empfaenger_Aktualisieren(ByVal Empfaenger As Integer, ByVal TextID As Integer, ByVal CC As Boolean, ByVal cn As SqlClient.SqlConnection)
        Dim blnCloseOnEnd As Boolean = False
        Dim m_tblEmpfaenger As New DataTable

        Try
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If

            Dim strSQL As String

            strSQL = "SELECT TOP 100 PERCENT " & _
                        "dbo.EmailVersand.[EmpfängerID], " & _
                        "dbo.EmailVersand.TextID " & _
                        "FROM dbo.EmailVersand " & _
                        "WHERE dbo.EmailVersand.TextID = " & TextID & " AND dbo.EmailVersand.[EmpfängerID] = " & Empfaenger

            Dim da As New SqlClient.SqlDataAdapter(strSQL, cn)
            da.Fill(m_tblEmpfaenger)

        Catch ex As Exception
            m_strError = "Fehler beim Laden der Daten"
        Finally
            cn.Close()
        End Try

        If m_tblEmpfaenger.Rows.Count = 0 Then
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If

            Try
                Dim strSQL As String

                strSQL = "INSERT INTO dbo.EmailVersand(EmpfängerID, TextID, CC) " & _
                            "VALUES (" & Empfaenger & ", " & TextID & ", " & CInt(CC) & ")"

                Dim com As New SqlClient.SqlCommand(strSQL, cn)
                com.ExecuteNonQuery()

            Catch ex As Exception
                m_strError = "Fehler beim anlegen der Daten"
            Finally
                cn.Close()
            End Try
        Else
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If

            Try
                Dim strSQL As String

                strSQL = "UPDATE dbo.EmailVersand " & _
                            "SET [EmpfängerID] = " & Empfaenger & " ," & _
                            "TextID = " & TextID & " ," & _
                            "CC = " & CInt(CC) & _
                            " WHERE [EmpfängerID] = " & Empfaenger & " AND TextID = " & TextID

                Dim com As New SqlClient.SqlCommand(strSQL, cn)
                com.ExecuteNonQuery()

            Catch ex As Exception
                m_strError = "Fehler beim anlegen der Daten"
            Finally
                cn.Close()
            End Try
        End If

    End Sub

    Public Sub Empfaenger_Loeschen(ByVal VersandID As Integer, ByVal cn As SqlClient.SqlConnection)
        Try
            ' Statement anpassen -------------
            Dim strSqlDelete As String = "DELETE " & _
                                           "FROM EmailVersand " & _
                                           "WHERE [EmailVersandID] = " & VersandID

            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn
            cn.Open()

            cmd.CommandText = strSqlDelete
            cmd.ExecuteNonQuery()

            cn.Close()
        Catch ex As Exception
            Throw New Exception("Fehler beim Löschen eines Eintrages!", ex)
        End Try
    End Sub

    Public Sub Text_Loeschen(ByVal TextID As Integer, ByVal cn As SqlClient.SqlConnection)
        Try
            ' Statement anpassen -------------
            Dim strSqlDelete As String = "DELETE " & _
                                           "FROM EmailTexte " & _
                                           "WHERE EmailTexte.TextID = " & TextID

            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn
            cn.Open()

            cmd.CommandText = strSqlDelete
            cmd.ExecuteNonQuery()

            strSqlDelete = "DELETE " & _
                            "FROM EmailVersand " & _
                            "WHERE EmailVersand.TextID = " & TextID

            cmd.CommandText = strSqlDelete
            cmd.ExecuteNonQuery()

            cn.Close()
        Catch ex As Exception
            Throw New Exception("Fehler beim Löschen eines Eintrages!", ex)
        End Try
    End Sub
#End Region

End Class