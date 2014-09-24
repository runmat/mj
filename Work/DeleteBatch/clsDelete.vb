Imports System.IO
Imports System.Configuration
Imports System.Xml
Imports System.Net.Mail
Imports System.Data.SqlClient

Public Class clsDelete

    Private configurationAppSettings As New AppSettingsReader()
    'Private strQuery As String

    Shared Sub main()
        Dim clsDel As New clsDelete()
        ' Altes Log löschen
        Console.WriteLine("Altes Log wird gelöscht...")
        clsDel.DelLogtxt()
        Console.WriteLine("Altes Log wurde gelöscht...")

        ' Dateien nach Config.xml löschen
        Console.WriteLine("Ausführen von Config.xml...")
        clsDel.DeleteTempFiles()
        Console.WriteLine("Config.xml ausgeführt...")
#If DEBUG Then
        Console.WriteLine("Weiter mit beliebiger Taste...")
        Console.ReadLine()
#End If

        ' Datenbank bereinigen nach ConfigDelLog
        Console.WriteLine("Ausführen von ConfigDelLog.xml...")
        clsDel.ExecuteConfigDelLog()
        Console.WriteLine("ConfigDelLog.xml ausgeführt...")
#If DEBUG Then
        Console.WriteLine("Weiter mit beliebiger Taste...")
        Console.ReadLine()
#End If


    End Sub

    Public Sub New()

    End Sub

    Private Sub DeleteTempFiles()
        Dim path As String
        Dim range As Integer
        Dim pattern As String
        Dim Unterverzeichnis As String
        Dim Ordner As String
        Dim dirInfo As DirectoryInfo
        Dim dirInfo2 As DirectoryInfo
        Dim Doc As New XmlDocument()
        'Dim protocol As FileInfo
        'Dim writer As StreamWriter


        Try

            'Konfigurationsdatei aus dem bin-Verzeichnis laden
            Doc.Load("config.xml")

            Dim Node As XmlNode
            Dim PatternNode As XmlNode
            Dim Nodeunter As XmlNode

            Node = Doc.DocumentElement
            PatternNode = Doc.DocumentElement
            Nodeunter = Doc.DocumentElement

            For Each Node In Doc.DocumentElement.ChildNodes


                If Node.Name = "Paths" Then
                    'Pfad auslesen

                    path = Node.Attributes.GetNamedItem("Path").Value

                    'Zeitraum auslesen
                    range = Node.Attributes.GetNamedItem("DaysBack").Value

                    'Hauptverzeichnis
                    dirInfo = New DirectoryInfo(path)
                    If dirInfo.Name = "0000261510" Then
                        Debug.Write("0000261510")
                    End If
                    'Array mit den Unterverzeichnissen

                    'Pattern auslesen
                    'Sollen Dateien aus den Unterverzeichnissen gelöscht werden?
                    Unterverzeichnis = Node.Attributes.GetNamedItem("MitUnterverzeichnis").Value

                    'Leere Ordner löschen?
                    Ordner = Node.Attributes.GetNamedItem("MitOrdner").Value
                    'if Node.ChildNodes.Count > 1

                    For Each PatternNode In Node.ChildNodes
                        Dim dirInfoArr As DirectoryInfo() = dirInfo.GetDirectories()
                        Dim dri As DirectoryInfo
                        If Unterverzeichnis = "Ja" Then
                            pattern = PatternNode.Attributes.GetNamedItem("Pattern").Value
                            For Each dri In dirInfoArr
                                If dri.Name = "0000261510" Then
                                    Debug.Write("0000261510")
                                End If

                                dirInfo2 = New DirectoryInfo(dri.FullName)
                                Dim dirInfoArr2 As DirectoryInfo() = dirInfo2.GetDirectories()
                                Dim dri2 As DirectoryInfo
                                If Unterverzeichnis = "Ja" Then
                                    For Each dri2 In dirInfoArr2
                                        'Daten in den Unterverzeichnissen löschen
                                        DelRenameFiles(dri2.FullName, pattern, range, Ordner)
                                    Next
                                End If
                                'Daten in den Unterverzeichnissen löschen
                                DelRenameFiles(dri.FullName, pattern, range, Ordner)
                            Next
                        End If
                        pattern = PatternNode.Attributes.GetNamedItem("Pattern").Value
                        'Daten im Hauptverzeichnis löschen
                        DelRenameFiles(path, pattern, range, Ordner)

                    Next
                End If
                'protocol = New FileInfo("log.txt")
                'If Not (protocol.Exists) Then
                '    writer = protocol.CreateText()
                'Else
                '    writer = protocol.AppendText
                'End If
                'writer.WriteLine(Date.Now & " - Ordner " & dirInfo.FullName & " wurde bereinigt!")
                'writer.Flush()
                'writer.Close()
                LogPortalUserActions("Success", Date.Now & " - Ordner " & dirInfo.FullName & " wurde bereinigt!")
            Next
        Catch ex As Exception
            LogPortalUserActions("Fehler", ex.Message)
            sendMail(ex)
            'Finally
            '    If Not (writer Is Nothing) Then
            '        writer.Close()
            '    End If
        End Try
    End Sub

    Private Sub DelRenameFiles(ByVal path As String, ByVal pattern As String, ByVal range As Integer, ByVal Ordner As String)
        Dim dirInfo As DirectoryInfo
        Dim dir As DirectoryInfo()
        Dim filInfo As FileInfo()
        Dim dateDelete As Date

        Try

            'Löschdatum ermitteln
            dateDelete = Date.Now.Subtract(New TimeSpan(range, 0, 0, 0))
            'LogPortalUserActions("DEBUG", "Löschdatum: " + dateDelete.ToString())

            'Erst einmal schauen, ob es Dateien zum löschen gibt...
            dirInfo = New DirectoryInfo(path)
            If dirInfo.Exists Then
                filInfo = dirInfo.GetFiles(pattern)
               
                For Each filSingle As FileInfo In filInfo
                    LogPortalUserActions("DEBUG", filSingle.FullName + " found")

                    If (filSingle.LastWriteTime < dateDelete) Then
                        Try
                            filSingle.Delete()
                        Catch ex As Exception
                            LogPortalUserActions("Fehler", filSingle.FullName & ": " & ex.Message)
                        End Try
                    End If
                Next

                If Ordner = "Ja" Then
                    dir = dirInfo.GetDirectories
                    filInfo = dirInfo.GetFiles()
                    If dir.Length = 0 AndAlso filInfo.Length = 0 Then
                        If dirInfo.Exists Then
                            dirInfo.Delete()
                            LogPortalUserActions("Hinweis:", "Verzeichnis " & dirInfo.FullName & " wurde gelöscht.")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            sendMail(ex)
            LogPortalUserActions("Fehler", ex.Message & ": " & ex.InnerException.Message)
        Finally

        End Try

    End Sub

    'Private Sub sendMail(ByVal errorMsg As Exception)
    '    Dim sendmail As Int16
    '    Dim mail As System.Web.Mail.MailMessage
    '    Dim mailAdresse As String
    '    Dim mailSender As String
    '    Dim mailBody As String
    '    Dim mailSubject As String
    '    Dim mailServer As String

    '    Try
    '        mail = New System.Web.Mail.MailMessage()
    '        mailSender = New MailAddress(configurationAppSettings.GetValue("SmtpMailSender", GetType(System.String)))
    '        mailAdresse = configurationAppSettings.GetValue("SmtpMailRecipient", GetType(System.String))
    '        mailServer = configurationAppSettings.GetValue("SmtpMailServer", GetType(System.String))

    '        mailBody = "Es ist ein Anwendungsfehler aufgetreten!" & vbCrLf & vbCrLf
    '        mailBody &= "System  :" & Environment.MachineName() & vbCrLf
    '        mailBody &= "Quelle  :<" & "DeleteBatch.exe>" & vbCrLf

    '        mailBody &= "Kurzinfo:Kontext = Kroschke Car Logistic, Überführungen." & vbCrLf
    '        mailBody &= "        :Die Anwendung löscht alle Dateien (Bilder/Protokolle) älter als X Tage(Einstellung config.xml) aus dem Verzeichnis" & vbCrLf
    '        mailBody &= "        :'" & configurationAppSettings.GetValue("Path", GetType(System.String)) & "'" & vbCrLf
    '        mailBody &= "        :Aktivierung: Täglich 00:10 über Windows-Taskplaner." & vbCrLf & vbCrLf

    '        mailBody &= "Uhrzeit :" & Now() & vbCrLf
    '        mailBody &= "Fehler  :" & errorMsg.Message & vbCrLf
    '        mailBody &= "Details :" & CurDir() & "\log.txt" & vbCrLf & vbCrLf
    '        mailBody &= "Info an : Oliver Rudolph, Abteilung IT-Webentwicklung"

    '        With mail
    '            .From = mailSender
    '            .To = mailAdresse
    '            .Subject = "Maschinell erzeugte Mail."
    '            .Body = mailBody
    '        End With

    '        System.Web.Mail.SmtpMail.SmtpServer = mailServer
    '        System.Web.Mail.SmtpMail.Send(mail)

    '    Catch ex As Exception
    '    End Try
    'End Sub

    ''' <summary>
    ''' Sendet eine E-Mail mit einer kurzen Fehlerbeschreibung an die in der <c>App.Config</c> hinterlegte Empfänger-Adresse(n)
    ''' </summary>
    ''' <param name="errorMsg">Die ausgelöste Exception die aufbereitet werden soll</param>
    ''' <remarks></remarks>

    Private Sub sendMail(ByVal errorMsg As Exception)
        Dim mail As MailMessage
        Dim mailAdresse As String
        Dim mailSender As String
        Dim mailBody As String
        Dim mailServer As String

        Try
            ' Adresswerte aus Config lesen
            mailServer = configurationAppSettings.GetValue("SmtpMailServer", GetType(System.String))
            mailSender = configurationAppSettings.GetValue("SmtpMailSender", GetType(System.String))
            mailAdresse = configurationAppSettings.GetValue("SmtpMailRecipient", GetType(System.String))

            ' Mailserver Objekt generieren
            Dim SMTP As New SmtpClient(mailServer)

            ' Mail Inhalt erstellen
            mailBody = "Es ist ein Anwendungsfehler aufgetreten!" & vbCrLf & vbCrLf
            mailBody &= "System  :" & Environment.MachineName() & vbCrLf
            mailBody &= "Quelle  :<" & "DeleteBatch.exe>" & vbCrLf

            mailBody &= "Kurzinfo:Kontext = Kroschke Car Logistic, Überführungen." & vbCrLf
            mailBody &= "        :Die Anwendung löscht alle Dateien (Bilder/Protokolle) älter als X Tage(Einstellung config.xml) aus dem Verzeichnis" & vbCrLf
            mailBody &= "        :'" & configurationAppSettings.GetValue("Path", GetType(System.String)) & "'" & vbCrLf
            mailBody &= "        :Aktivierung: Täglich 00:10 über Windows-Taskplaner." & vbCrLf & vbCrLf

            mailBody &= "Uhrzeit :" & Now() & vbCrLf
            mailBody &= "Fehler  :" & errorMsg.Message & vbCrLf
            mailBody &= "Details :" & CurDir() & "\log.txt" & vbCrLf & vbCrLf
            mailBody &= "Info an : Oliver Rudolph, Abteilung IT-Webentwicklung"

            ' Mail generieren
            mail = New System.Net.Mail.MailMessage(mailSender, mailAdresse, "Maschinell erzeugte Mail.", mailBody)
            ' Mail versenden
            SMTP.Send(mail)
        Catch ex As Exception
        End Try
    End Sub

#Region "Portal_User_Lock_Delete_Process"

    Private Sub Regelprozess(ByVal ConnectionString As String, ByVal QueryString As String)
        Dim blNoError As Boolean = True

        Dim Doc As XmlDocument = New XmlDocument()
        Dim con As SqlConnection
        Dim com As SqlCommand

        If ConnectionString <> "" Then
            con = New SqlConnection(ConnectionString)
            com = New SqlCommand(QueryString, con)
            com.CommandType = CommandType.Text

            Try
                con.Open()
                com.ExecuteNonQuery()
                LogPortalUserActions("Success", "Regelprozess erfolgreich ausgeführt.")
            Catch ex As SqlException
                LogPortalUserActions("Fehler", ex.InnerException.InnerException.Message)
                sendMail(ex)
            Finally
                con.Close()
            End Try
        Else
            LogPortalUserActions("Fehler", "Der SQL-Connectionstring konnte nicht ausgelesen werden!")
        End If

    End Sub

    Private Sub LogPortalUserActions(ByVal Category As String, ByVal LogMessage As String)
        Dim Connection As New SqlClient.SqlConnection()
        Dim Command As New SqlClient.SqlCommand()
        Dim protocol As FileInfo
        Dim writer As StreamWriter

        Try
            protocol = New FileInfo("log.txt")
            If Not (protocol.Exists) Then
                writer = protocol.CreateText()
            Else
                writer = protocol.AppendText()
            End If

            writer.WriteLine(Category & ": " & LogMessage)

        Catch ex As Exception
            EventLog.WriteEntry("DeleteBatch", ex.Message & ": " & ex.InnerException.ToString())
            sendMail(ex)
        Finally
            If Not (writer Is Nothing) Then
                writer.Flush()
                writer.Close()
            End If
        End Try
    End Sub

#End Region

#Region "Delete_Logeinträge"

    Private Sub ExecuteConfigDelLog()

        Dim sQuery As String
        Dim sConn As String
        Dim Doc As New XmlDocument()

        Try
            Dim runOnlyOn As String = configurationAppSettings.GetValue("RunUserLockOnlyOn", GetType(System.String))
            Dim blnRunToday As Boolean = True
            If Not String.IsNullOrEmpty(runOnlyOn) Then
                Select Case runOnlyOn
                    Case "1"
                        blnRunToday = (Today.DayOfWeek = DayOfWeek.Monday)
                    Case "2"
                        blnRunToday = (Today.DayOfWeek = DayOfWeek.Tuesday)
                    Case "3"
                        blnRunToday = (Today.DayOfWeek = DayOfWeek.Wednesday)
                    Case "4"
                        blnRunToday = (Today.DayOfWeek = DayOfWeek.Thursday)
                    Case "5"
                        blnRunToday = (Today.DayOfWeek = DayOfWeek.Friday)
                    Case "6"
                        blnRunToday = (Today.DayOfWeek = DayOfWeek.Saturday)
                    Case "7"
                        blnRunToday = (Today.DayOfWeek = DayOfWeek.Sunday)
                End Select
            End If

            Doc.Load("ConfigDelLog.xml")

            For Each Node As XmlNode In Doc.DocumentElement.ChildNodes
                If Node.Name = "Connections" Then
                    sConn = Node.Attributes.GetNamedItem("Conn").Value
                    For Each connNode As XmlNode In Node.ChildNodes
                        If connNode.Name = "Queries" Then
                            sQuery = connNode.Attributes.GetNamedItem("qString").Value
                            DelLog(sConn, sQuery)
                        ElseIf connNode.Name = "UserLock" Then
                            ' Automatische Benutzersperrung und Löschung
                            If blnRunToday Then
                                sQuery = connNode.Attributes.GetNamedItem("qString").Value
                                Regelprozess(sConn, sQuery)
                            Else
                                LogPortalUserActions("Hinweis", "Regelprozess geplant übersprungen.")
                            End If
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            If Not (ex.InnerException Is Nothing) Then
                LogPortalUserActions("Fehler", ex.InnerException.InnerException.Message & "\r\n")
            End If
        End Try
    End Sub

    Private Sub DelLog(ByVal sConn As String, ByVal sQuery As String)
        Dim Connection As New SqlClient.SqlConnection()
        Dim Command As New SqlClient.SqlCommand()
        Dim sSQL As String
        Dim sTableNames() As String
        Dim iResult As Integer
        Dim sTemp As String = "[QUERY]"
        Dim sTemp2 As String = "[TABLE]"

        Connection.ConnectionString = sConn
        sSQL = sQuery

        Try
            Connection.Open()
            With Command

                .Connection = Connection
                .CommandType = CommandType.Text
                .CommandTimeout = 300

                sSQL = sQuery
                .CommandText = sSQL
                iResult = .ExecuteNonQuery()


                sTemp = sQuery.Substring(12)
                sTableNames = sTemp.Split(" ")
                sTemp2 = sTableNames(0)

            End With

            LogPortalUserActions("Delete", Date.Now & " - Einträge aus Datenbank: " & Connection.Database & ", Tabelle: " & sTemp2 & " , Server:" & Connection.DataSource & " gelöscht.")
        Catch ex As Exception
            If Not (ex.InnerException Is Nothing) Then
                LogPortalUserActions("Fehler", ex.InnerException.InnerException.Message)
            Else
                LogPortalUserActions("Fehler", ex.Message)
            End If
        Finally
            Connection.Close()
        End Try
    End Sub

    Private Sub DelLogtxt()
        Dim slines() As String
        Dim protocol As FileInfo
        Dim writer As StreamWriter
        Dim reader As StreamReader

        Dim sTemp As String
        Dim sTemp2 As String
        Dim sTemp3 As String
        Dim sTextNew As String = ""

        Try
            protocol = New FileInfo("log.txt")
            If (protocol.Exists) Then
                reader = protocol.OpenText()
                sTemp = reader.ReadToEnd
                reader.Close()
                slines = sTemp.Split(vbCrLf)
                For Each sTemp2 In slines
                    sTemp3 = Left(sTemp2, 11)
                    If IsDate(sTemp3) Then
                        If CDate(sTemp3) > Today.AddDays(-21) Then
                            sTextNew = sTextNew & sTemp2
                        End If
                    End If
                Next
            End If
            writer = protocol.CreateText
            writer.Write(sTextNew)

        Catch ex As Exception
            If writer IsNot Nothing Then
                If ex.InnerException IsNot Nothing Then
                    writer.WriteLine("Fehler: " & ex.InnerException.InnerException.Message)
                Else
                    writer.WriteLine("Fehler: " & ex.Message)
                End If

                writer.Flush()
            End If
        Finally
            If writer IsNot Nothing Then
                writer.WriteLine(vbCrLf & Date.Now & " Einträge in der log.txt gelöscht!")
                writer.Flush()
                writer.Close()
            End If
        End Try
    End Sub
#End Region
    
End Class

