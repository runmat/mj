

Partial Public Class Confirmation
    Inherits Page

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        lbtnPortal.PostBackUrl = ConfigurationManager.AppSettings("ServerURL")

        If Request.QueryString.Item("key") Is Nothing Then
            lblError.Text = "Diese Seite kann nur mit einem entsprechenden Schlüssel aufgerufen werden."
        Else
            ValidateKey(Request.QueryString.Item("key").ToString)
        End If

    End Sub
#End Region

#Region "Methods"
    '----------------------------------------------------------------------
    ' Methode:      ValidateKey
    ' Autor:        Sfa
    ' Beschreibung: Überprüfung des in der URL übergebenen Schlüssels.
    '               Schlüssel gefunden: Mailversand an User, ansonsten Fehler
    '               meldung
    ' Erstellt am:  20.04.2009
    ' ITA:          2709
    '----------------------------------------------------------------------
    Private Sub ValidateKey(ByVal keyValue As String)


        Dim tempTable As New DataTable

        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("ConString").ToString)
        cn.Open()

        Dim cmdKey As SqlClient.SqlDataAdapter
        cmdKey = New SqlClient.SqlDataAdapter("SELECT WebUser.UserID,WebUserUpload.ID, mail, Confirmed,MailSend,WebUserUpload.Password,ValidFrom " & _
                                              "FROM WebUserUpload INNER JOIN WebUser ON " & _
                                              "WebUserUpload.UserID = WebUser.UserID INNER JOIN " & _
                                              "WebUserInfo ON WebUserUpload.UserID = WebUserInfo.id_user " & _
                                              "WHERE MailSend = 1 and RightUserLink=@RightUserLink", cn)

        cmdKey.SelectCommand.Parameters.AddWithValue("@RightUserLink", keyValue)

        cmdKey.Fill(tempTable)

        If tempTable.Rows.Count > 0 Then

            If CBool(tempTable.Rows(0)("Confirmed").ToString) = False Then
                'Mail mit dem Benutzernamen wurde bereits versendet.
                'Weitere Mail mit Passwort versenden und Bestätigung eintragen.

                Dim validFrom As String = tempTable.Rows(0)("ValidFrom").ToString

                If validFrom.Length > 0 Then
                    validFrom = CDate(validFrom).ToShortDateString.ToString()
                End If

                If SendMail(tempTable.Rows(0)("mail").ToString, _
                            tempTable.Rows(0)("Password").ToString, _
                            validFrom) = True Then

                    SetConfirmed(tempTable.Rows(0)("ID").ToString, tempTable.Rows(0)("UserID").ToString, 0)
                    lblAusgabe.Text = "Vielen Dank für Ihre Bestätigung. Sie erhalten in Kürze eine Mail mit Ihrem Passwort."
                    lbtnPortal.Visible = True
                Else
                    lblError.Text = "Fehler: Die Mail konnte nicht versendet werden."

                End If


            Else
                lblError.Text = "Die Bestätigung wurde bereits durchgeführt."
                lbtnPortal.Visible = True
            End If

        Else
            cmdKey.Dispose()

            cmdKey = New SqlClient.SqlDataAdapter("SELECT WebUser.UserID,WebUserUpload.ID,WebUser.Username " & _
                                              "FROM WebUserUpload INNER JOIN WebUser ON " & _
                                              "WebUserUpload.UserID = WebUser.UserID INNER JOIN " & _
                                              "WebUserInfo ON WebUserUpload.UserID = WebUserInfo.id_user " & _
                                              "WHERE MailSend = 1 and WrongUserLink=@WrongUserLink", cn)

            cmdKey.SelectCommand.Parameters.AddWithValue("@WrongUserLink", keyValue)

            cmdKey.Fill(tempTable)

            If tempTable.Rows.Count > 0 Then
                If SendErrorMail(tempTable.Rows(0)("Username").ToString) = True Then

                    SetConfirmed(tempTable.Rows(0)("ID").ToString, tempTable.Rows(0)("UserID").ToString, 0)
                    lblAusgabe.Text = "Vielen Dank für Ihre Information. Das Benutzerkonto wird gesperrt."

                Else
                    lblError.Text = "Fehler: Die Mail konnte nicht versendet werden."

                End If

            Else
                lblError.Text = "Ihr Link enthält einen unbekannten Schlüssel."
            End If

        End If

        cn.Close()
        cn.Dispose()
        cmdKey.Dispose()

    End Sub
    '----------------------------------------------------------------------
    ' Methode:      SendMail
    ' Autor:        Sfa
    ' Beschreibung: Schlüssel gefunden. Passwort an User versenden
    ' Erstellt am:  20.04.2009
    ' ITA:          2709
    '----------------------------------------------------------------------
    Private Function SendMail(ByVal mail As String, ByRef password As String, ByRef validFrom As String) As Boolean

        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
        Dim smtpMailBody As String = ConfigurationManager.AppSettings("SmtpMailBody")
        Dim smtpMailBodyDateInfo As String = ConfigurationManager.AppSettings("SmtpMailBodyDateInfo")
        Dim subject As String = "Ihr persönliches Kennwort"

        Dim decrypt As New Security

        If Not decrypt.PsDecrypt(password) = String.Empty Then
            password = decrypt.PsDecrypt(password)
        End If


        smtpMailBody = Replace(smtpMailBody, "#PW#", password)

        If Not String.IsNullOrEmpty(validFrom) Then
            If validFrom.Length > 0 Then

                validFrom = Left(validFrom, 10)
                smtpMailBodyDateInfo = Replace(smtpMailBodyDateInfo, "#ValidFrom#", validFrom)

                smtpMailBody = Replace(smtpMailBody, "#ValidFromText#", vbCrLf & smtpMailBodyDateInfo & vbCrLf)
            End If
        End If

        smtpMailBody = Replace(smtpMailBody, "#ValidFromText#", String.Empty)


        Try
            Dim client As New Net.Mail.SmtpClient(smtpMailServer)
            client.Send(smtpMailSender, mail, subject, smtpMailBody)

        Catch ex As Exception
            Return False
        End Try

        Return True


    End Function
    '----------------------------------------------------------------------
    ' Methode:      SetConfirmed
    ' Autor:        Sfa
    ' Beschreibung: Bestätigungsflag in der Datenbank setzen
    ' Erstellt am:  20.04.2009
    ' ITA:          2709
    '----------------------------------------------------------------------
    Private Sub SetConfirmed(ByVal webUserUploadiD As String, ByVal userId As String, ByVal accountIsLockedOut As Integer)
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("ConString").ToString)
        cn.Open()


        Dim cmdUpdate As New SqlClient.SqlCommand("Update WebUserUpload set Confirmed = 1 where ID=@ID", cn)

        Dim sqlParam As New SqlClient.SqlParameter("@ID", SqlDbType.Int)

        cmdUpdate.Parameters.Add(sqlParam)


        cmdUpdate.Parameters("@ID").Value = CInt(WebUserUploadiD)
        cmdUpdate.ExecuteNonQuery()

        cmdUpdate.Parameters("@ID").Value = CInt(userId)
        cmdUpdate.CommandText = "Update WebUser set AccountIsLockedOut =@AccountIsLockedOut where UserID=@ID"
        cmdUpdate.Parameters.AddWithValue("@AccountIsLockedOut", accountIsLockedOut)

        cmdUpdate.ExecuteNonQuery()

        cn.Close()
        cn.Dispose()

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      SendErrorMail
    ' Autor:        Sfa
    ' Beschreibung: Schlüssel gefunden(Falscher User). 
    '               Fehlermail an Administrationspostfach
    ' Erstellt am:  20.04.2009
    ' ITA:          2709
    '----------------------------------------------------------------------
    Private Function SendErrorMail(ByVal username As String) As Boolean

        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
        Dim smtpMailBody As String = ConfigurationManager.AppSettings("SmtpMailBodyError")
        Dim subject As String = "Falscher Benutzer"
        Dim mail As String = ConfigurationManager.AppSettings("SmtpMailRecipient")

        smtpMailBody = Replace(smtpMailBody, "#Username#", username)


        Try
            Dim client As New Net.Mail.SmtpClient(smtpMailServer)
            client.Send(smtpMailSender, mail, subject, smtpMailBody)

        Catch ex As Exception
            Return False
        End Try

        Return True


    End Function
#End Region

End Class
