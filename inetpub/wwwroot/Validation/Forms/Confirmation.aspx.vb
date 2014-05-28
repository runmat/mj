Imports System.Security.Cryptography
Imports System.IO
Imports System.Xml
Imports System.Text

Partial Public Class Confirmation
    Inherits System.Web.UI.Page

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
    Private Sub ValidateKey(ByVal KeyValue As String)


        Dim TempTable As New DataTable

        Dim cn As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConString").ToString)
        cn.Open()

        Dim cmdKey As SqlClient.SqlDataAdapter
        cmdKey = New SqlClient.SqlDataAdapter("SELECT WebUser.UserID,WebUserUpload.ID, mail, Confirmed,MailSend,WebUserUpload.Password,ValidFrom " & _
                                              "FROM WebUserUpload INNER JOIN WebUser ON " & _
                                              "WebUserUpload.UserID = WebUser.UserID INNER JOIN " & _
                                              "WebUserInfo ON WebUserUpload.UserID = WebUserInfo.id_user " & _
                                              "WHERE MailSend = 1 and RightUserLink=@RightUserLink", cn)

        cmdKey.SelectCommand.Parameters.AddWithValue("@RightUserLink", KeyValue)

        cmdKey.Fill(TempTable)

        If TempTable.Rows.Count > 0 Then

            If CBool(TempTable.Rows(0)("Confirmed").ToString) = False Then
                'Mail mit dem Benutzernamen wurde bereits versendet.
                'Weitere Mail mit Passwort versenden und Bestätigung eintragen.

                Dim ValidFrom As String = TempTable.Rows(0)("ValidFrom").ToString

                If ValidFrom.Length > 0 Then CDate(ValidFrom).ToShortDateString.ToString()

                If SendMail(TempTable.Rows(0)("mail").ToString, _
                            TempTable.Rows(0)("Password").ToString, _
                            ValidFrom) = True Then

                    SetConfirmed(TempTable.Rows(0)("ID").ToString, TempTable.Rows(0)("UserID").ToString, 0)
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
            cmdKey = Nothing
            cmdKey = New SqlClient.SqlDataAdapter("SELECT WebUser.UserID,WebUserUpload.ID,WebUser.Username " & _
                                              "FROM WebUserUpload INNER JOIN WebUser ON " & _
                                              "WebUserUpload.UserID = WebUser.UserID INNER JOIN " & _
                                              "WebUserInfo ON WebUserUpload.UserID = WebUserInfo.id_user " & _
                                              "WHERE MailSend = 1 and WrongUserLink=@WrongUserLink", cn)

            cmdKey.SelectCommand.Parameters.AddWithValue("@WrongUserLink", KeyValue)

            cmdKey.Fill(TempTable)

            If TempTable.Rows.Count > 0 Then
                If SendErrorMail(TempTable.Rows(0)("Username").ToString) = True Then

                    SetConfirmed(TempTable.Rows(0)("ID").ToString, TempTable.Rows(0)("UserID").ToString, 0)
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
        cmdKey = Nothing
    End Sub
    '----------------------------------------------------------------------
    ' Methode:      SendMail
    ' Autor:        Sfa
    ' Beschreibung: Schlüssel gefunden. Passwort an User versenden
    ' Erstellt am:  20.04.2009
    ' ITA:          2709
    '----------------------------------------------------------------------
    Private Function SendMail(ByVal Mail As String, ByRef Password As String, ByRef ValidFrom As String) As Boolean

        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
        Dim smtpMailBody As String = ConfigurationManager.AppSettings("SmtpMailBody")
        Dim SmtpMailBodyDateInfo As String = ConfigurationManager.AppSettings("SmtpMailBodyDateInfo")
        Dim Subject As String = "Ihr persönliches Kennwort"

        Dim Decrypt As New Security

        If Not Decrypt.psDecrypt(Password) = String.Empty Then
            Password = Decrypt.psDecrypt(Password)
        End If


        smtpMailBody = Replace(smtpMailBody, "#PW#", Password)

        If Not ValidFrom Is System.DBNull.Value Then
            If ValidFrom.Length > 0 Then

                ValidFrom = Left(ValidFrom, 10)
                SmtpMailBodyDateInfo = Replace(SmtpMailBodyDateInfo, "#ValidFrom#", ValidFrom)

                smtpMailBody = Replace(smtpMailBody, "#ValidFromText#", vbCrLf & SmtpMailBodyDateInfo & vbCrLf)
            End If
        End If

        smtpMailBody = Replace(smtpMailBody, "#ValidFromText#", String.Empty)


        Try
            Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
            client.Send(smtpMailSender, Mail, Subject, smtpMailBody)

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
    Private Sub SetConfirmed(ByVal ID As String, ByVal UserID As String, ByVal AccountIsLockedOut As Integer)
        Dim cn As New SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("ConString").ToString)
        cn.Open()


        Dim cmdUpdate As New SqlClient.SqlCommand("Update WebUserUpload set Confirmed = 1 where ID=@ID", cn)

        Dim SqlParam As New SqlClient.SqlParameter("@ID", SqlDbType.Int)

        cmdUpdate.Parameters.Add(SqlParam)


        cmdUpdate.Parameters("@ID").Value = CInt(ID)
        cmdUpdate.ExecuteNonQuery()

        cmdUpdate.Parameters("@ID").Value = CInt(UserID)
        cmdUpdate.CommandText = "Update WebUser set AccountIsLockedOut =@AccountIsLockedOut where UserID=@ID"
        cmdUpdate.Parameters.AddWithValue("@AccountIsLockedOut", AccountIsLockedOut)

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
    Private Function SendErrorMail(ByVal Username As String) As Boolean

        Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
        Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
        Dim smtpMailBody As String = ConfigurationManager.AppSettings("SmtpMailBodyError")
        Dim Subject As String = "Falscher Benutzer"
        Dim Mail As String = ConfigurationManager.AppSettings("SmtpMailRecipient")

        smtpMailBody = Replace(smtpMailBody, "#Username#", Username)


        Try
            Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
            client.Send(smtpMailSender, Mail, Subject, smtpMailBody)

        Catch ex As Exception
            Return False
        End Try

        Return True


    End Function
#End Region

End Class
' ************************************************
' $History: Confirmation.aspx.vb $