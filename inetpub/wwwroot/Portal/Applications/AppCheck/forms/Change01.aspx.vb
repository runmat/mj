Imports CKG.Base.Kernel.Common.Common
Imports System.Net
Imports System.IO
Imports CKG


Partial Public Class Change01
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    'Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        lblMessage.Text = "Bitte wählen Sie eine Datei aus!"
        Try
            'm_App = New Base.Kernel.Security.App(m_User)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        Dim strMessage As String = ""
        'Dim status As String = ""
        Dim fileInfo As FileInfo
        'Dim FileVersion As String = ""
        Dim path As String
        lblMessage.Text = ""
        Try
            If Not (upFile.PostedFile Is Nothing) Then
                'Dim objFile As Object
                fileInfo = New FileInfo(upFile.PostedFile.FileName)
                path = ConfigurationManager.AppSettings("ExcelPath")


                Select Case fileInfo.Extension.ToUpper.ToString
                    Case ".XLS"
                        upFile.PostedFile.SaveAs(path + fileInfo.Name)

                        Dim Exc As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory
                        Dim Message As String = ""

                        Message = Exc.CheckExcelVersion(path + fileInfo.Name)
                        If Message = "Error" Then
                            lblError.Text = "Die Exceldatei benutzt ein veraltetes Format. Bitte benutzen Sie Excelversion 8 (Excel 97) oder höher!"
                            Exit Sub
                        End If
                    Case ".XLSX"
                        upFile.PostedFile.SaveAs(path + fileInfo.Name)
                    Case Else
                        lblError.Text = "Es können nur Exceldateien hochgeldaden werden."
                        Exit Sub

                End Select

                Dim upPath As String
                If CType(Request.QueryString("check"), String) = "1" Then ' Kunde
                    upPath = ConfigurationManager.AppSettings("UpDownKundeXL")
                    upPath += m_User.Organization.OrganizationReference & "\import\" & m_User.Reference & "\"
                Else 'ZulStelle
                    upPath = ConfigurationManager.AppSettings("UploadPathZulXL")
                    Upload()

                End If

                Dim sStatus As String = Right(upFile.PostedFile.FileName, upFile.PostedFile.FileName.Length - upFile.PostedFile.FileName.LastIndexOf("\") - 1)

                upFile.PostedFile.SaveAs(upPath & sStatus)

                strMessage = strMessage & "Es sind neue Daten für XL-Check vorhanden!" & vbCrLf & vbCrLf
                If CType(Request.QueryString("check"), String) = "1" Then
                    strMessage = strMessage & "Bitte überprüfen Sie den Eingangsordner des Kunden " & m_User.Organization.OrganizationReference & "!" & vbCrLf & vbCrLf & vbCrLf
                Else
                    strMessage = strMessage & "Bitte überprüfen Sie den Eingangsordner der Zulassungsstelle!" & vbCrLf & vbCrLf & vbCrLf
                End If

                strMessage = strMessage & vbCrLf & "Achtung: Diese Nachricht wurde automatisch generiert! Bitte antworten Sie nicht darauf!"


                Dim Mail As Mail.MailMessage
                Dim smtpMailSender As String = ""
                Dim smtpMailServer As String = ""
                Dim EmailAdresse As String = ""

                If CType(Request.QueryString("check"), String) = "1" Then
                    EmailAdresse = ConfigurationManager.AppSettings("MailDad")
                Else
                    'EmailAdresse = ConfigurationManager.AppSettings("MailZulStelle")' 
                    EmailAdresse = ConfigurationManager.AppSettings("MailDad")
                End If

                smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
                Mail = New Mail.MailMessage(smtpMailSender, EmailAdresse, "Neue Daten XL-Check 2.0", strMessage)
                'Mail.Priority = Net.Mail.MailPriority.High

                smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                Dim client As New Mail.SmtpClient(smtpMailServer)
                client.Send(Mail)

                lblMessage.Text = "Datei erfolgreich hochgeladen!"
            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim Hochladen der Datei." & ex.Message
        End Try
    End Sub

    Private Sub Upload()
        Dim fname As String
        Dim fnameNew As String
        Dim path As String
        Dim Extension As String
        'Dim Message As String = ""
        Dim File As HttpPostedFile
        lblError.Text = String.Empty

        If Not (upFile.PostedFile.FileName = String.Empty) Then
            Try
                fname = upFile.PostedFile.FileName
                'Dateigröße prüfen
                If (upFile.PostedFile.ContentLength > CType(ConfigurationManager.AppSettings("MaxUploadSizeUeberf"), Integer)) Then
                    lblError.Text = "Datei '" & Right(fname, fname.Length - fname.LastIndexOf("\") - 1).ToUpper & "' ist zu gross (>200 KB)."
                    Exit Sub
                End If
                ''------------------
                Extension = Right(fname, fname.Length - fname.LastIndexOf("."))

                'path = ConfigurationManager.AppSettings("UploadPathLocal") '"\\192.168.10.79\Datawizard\Upload\Ueberfuehrungen\"

                path = ConfigurationManager.AppSettings("UploadPathZulXL")

                File = upFile.PostedFile
                fnameNew = Now.ToShortDateString & "_" & Replace(Now.ToShortTimeString, ":", "-") & "_" & m_User.KUNNR & "-" & m_User.UserName
                File.SaveAs(path & fnameNew & Extension)

                Dim sUrl As String = ConfigurationManager.AppSettings("DataWizard")
                sUrl = sUrl & "/fpruef_Upload_from_Zulassungsstelle?erfasser=" & _
                             m_User.UserName
                Dim request As WebRequest = WebRequest.Create(sUrl)


                Dim stream As New FileStream(path & fnameNew & Extension, FileMode.Open)
                Dim reader As New StreamReader(File.InputStream)
                Dim lBytes As Long = stream.Length
                Dim byte1(lBytes - 1) As Byte
                'Lesen der Datei in ein Bytearray
                stream.Read(byte1, 0, lBytes)
                stream.Close()

                request.Method = WebRequestMethods.Http.Post
                request.ContentType = File.ContentType
                request.ContentLength = byte1.Length

                'Schreiben des Bytearray in den erwateten Stream des Zielservers
                Dim newStream As Stream = request.GetRequestStream()

                newStream.Write(byte1, 0, byte1.Length)

                newStream.Close()

                Dim response As WebResponse = request.GetResponse()
                Dim responseStream As Stream = response.GetResponseStream()

                Dim reader2 As New StreamReader(responseStream)
                '' Antwort lesen
                Dim responseFromServer As String = reader2.ReadToEnd()
                '' Streams schließen.
                reader.Close()
                response.Close()
                If responseFromServer = "Error" Then
                    lblError.Text = "Fehler beim Verarbeiten Ihrer Datei!"
                End If
                ' IO.File.Delete(path & fname & Extension)
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
        Else
            lblError.Text = "Keine Datei ausgewählt."
        End If
    End Sub
End Class