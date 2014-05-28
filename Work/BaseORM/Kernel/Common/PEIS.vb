Option Strict On
Option Explicit On
Imports System.Diagnostics
Imports System.Configuration


Namespace Common
    Public NotInheritable Class PEIS 'Portal Error Information System
        Private mObjUser As CKG.Base.Kernel.Security.User
        Private mStacFrame As StackFrame


#Region "Informations/Deklarations"

        'systeminformation
        Private Fehlermeldung As String = "<font color=""red"" >nicht verfügbar </font>"
        Private Pfad As String = "<font color=""red"" >nicht verfügbar </font>"
        Private Methodenname As String = "<font color=""red"" >nicht verfügbar </font>"
        Private Fehlerzeile As String = "<font color=""red"" >nicht verfügbar </font>"
        Private angefragteURL As String = "<font color=""red"" >nicht verfügbar </font>"
        Private ClientBrowser As String = "<font color=""red"" >nicht verfügbar </font>"
        Private ClientIP As String = "<font color=""red"" >nicht verfügbar </font>"

        Private Kurzdiagnose As String = "<font color=""red"" >nicht verfügbar </font>"

        'portalinformation
        Private SapSystem As String = "Information nicht verfügbar"
        Private AppID As String = "<font color=""red"" >nicht verfügbar </font>"
        Private AppFreundlicherName As String = "nicht verfügbar"
        Private AppName As String = "<font color=""red"" >nicht verfügbar </font>"
        Private AppTyp As String = "<font color=""red"" >nicht verfügbar </font>"
        Private AppPath As String = "<font color=""red"" >nicht verfügbar </font>"
        Private KundenName As String = "nicht verfügbar"
        Private GruppenName As String = "<font color=""red"" >nicht verfügbar </font>"
        Private Benutzer As String = "<font color=""red"" >nicht verfügbar </font>"
        Private AnzahlKunden As String = "0"

        Private UsedByOtherCustomerHTML As String = "<font color=""red"" >nicht verfügbar </font>"

#End Region

        Public Sub New(ByVal ex As Exception, ByVal context As Web.HttpContext)
            '----------------------------------------------------------------------
            'Methode:       New
            'Autor:         Julian Jung
            'Beschreibung:  Prüft auf vorhandene Daten in Session / Exception und befüllt die 
            '               die Informationsvariablen je nach Möglichkeit                
            'Erstellt am:   05.06.2009
            '----------------------------------------------------------------------

            Try
                If Not ex Is Nothing Then
                    Fehlermeldung = ex.Message

                    If checkForFilter() Then
                        Exit Sub
                    End If

                    Dim tmpTrace As New StackTrace(ex, True)
                    If Not tmpTrace Is Nothing AndAlso Not tmpTrace.FrameCount = 0 Then
                        mStacFrame = tmpTrace.GetFrame(0)
                        fillSystemInformation()
                    End If
                    If Not context Is Nothing Then
                        If Not context.Request Is Nothing Then
                            ClientBrowser = context.Request.UserAgent
                            angefragteURL = context.Request.RawUrl
                            ClientIP = context.Request.UserHostAddress

                        End If

                        If Not context.Session Is Nothing Then
                            If context.Session.IsNewSession Then
                                If CBool(ConfigurationManager.AppSettings("enableSessionTimeOutFilter")) Then
                                    Kurzdiagnose = "SessionTimeOut/SessionKill"
                                Else
                                    Exit Sub
                                End If
                            End If
                            If Not context.Session("objUser") Is Nothing Then
                                mObjUser = CType(context.Session("objUser"), CKG.Base.Kernel.Security.User)
                                If Not context.Session("AppID") Is Nothing Then
                                    AppID = context.Session("AppID").ToString
                                End If
                                fillPortalInformation()
                            End If
                        End If
                    End If
                    sendEmail()
                End If

            Catch ex1 As Exception
                Try
                    If CBool(ConfigurationManager.AppSettings("enablePeisErrorFilter")) Then
                        Kurzdiagnose = "Fehler in PEIS: " & ex1.Message
                        sendEmail()
                    End If
                Catch ex2 As Exception
                    Throw ex2
                End Try
            End Try

        End Sub

        Private Sub fillSystemInformation()
            Pfad = mStacFrame.GetFileName
            Methodenname = mStacFrame.GetMethod.Name
            Fehlerzeile = mStacFrame.GetFileLineNumber.ToString
        End Sub


        Private Function checkForFilter() As Boolean
            Dim cn As SqlClient.SqlConnection
            Dim cmdOutPut As SqlClient.SqlCommand
            Dim adpOutPut As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter()
            Dim tblTemp As DataTable
            cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Try

                cn.Open()

                cmdOutPut = New SqlClient.SqlCommand("Select KeyWords From PeisFilter where FilterEnabled='X'", cn)
                cmdOutPut.CommandType = CommandType.Text

                adpOutPut.SelectCommand = cmdOutPut
                tblTemp = New DataTable()
                adpOutPut.Fill(tblTemp)


                For Each tmpRow As DataRow In tblTemp.Rows
                    Dim keyWords() As String = tmpRow(0).ToString.Split(","c)
                    Dim filtered As Boolean = True

                    For i As Int32 = 0 To keyWords.Length - 2 'letzter splitString ist leer
                        If Not Fehlermeldung.ToUpper.Contains(keyWords(i).ToUpper) Then
                            filtered = False
                            Exit For
                        End If
                    Next
                    If filtered Then
                        Return True
                    End If
                Next

                Return False

            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try

        End Function

        Private Sub fillPortalInformation()
            With mObjUser

                Benutzer = .UserName
                KundenName = .CustomerName
                GruppenName = .Groups.ItemByID(.GroupID).GroupName

                If Not .IsTestUser Then
                    SapSystem = "Prod"
                Else
                    SapSystem = "Test"
                End If

                Dim appRows() As DataRow
                If Not mObjUser.Applications Is Nothing Then
                    If IsNumeric(AppID) Then
                        appRows = mObjUser.Applications.Select("AppID=" & AppID)
                        If appRows.Length = 1 Then
                            AppPath = appRows(0)("AppURL").ToString
                            AppFreundlicherName = appRows(0)("AppFriendlyName").ToString
                            AppName = appRows(0)("AppName").ToString
                            AppTyp = appRows(0)("AppType").ToString

                            fillOtherCustomer()

                        Else
                            'AppKann nicht ausgewertet werden
                        End If
                    End If
                End If
            End With
        End Sub

        Private Sub fillOtherCustomer()
            '----------------------------------------------------------------------
            'Methode:       fillOtherCustomer
            'Autor:         Julian Jung
            'Beschreibung:  Prüft anhand der AppUrl welche Kunden sonst noch betroffen sind.
            'Erstellt am:   05.06.2009
            '----------------------------------------------------------------------
            Dim cn As SqlClient.SqlConnection
            Dim cmdOutPut As SqlClient.SqlCommand
            Dim adpOutPut As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter()
            Dim tblTemp As DataTable
            cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Try

                cn.Open()

                cmdOutPut = New SqlClient.SqlCommand("Select Customername From vwCheckReportToCustomer where AppURL='" & AppPath & "'", cn)
                cmdOutPut.CommandType = CommandType.Text

                adpOutPut.SelectCommand = cmdOutPut
                tblTemp = New DataTable()
                adpOutPut.Fill(tblTemp)

                If tblTemp.Rows.Count > 1 Then
                    AnzahlKunden = tblTemp.Rows.Count.ToString
                    UsedByOtherCustomerHTML = "Anzahl " & tblTemp.Rows.Count - 1 & " <br>"
                    For Each tmpRow As DataRow In tblTemp.Rows
                        If Not tmpRow("Customername").ToString = KundenName Then
                            UsedByOtherCustomerHTML &= tmpRow("Customername").ToString & " <br>"
                        End If
                    Next
                Else
                    UsedByOtherCustomerHTML = "keine"
                End If

            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try
        End Sub

        Private Sub sendEmail()
            '----------------------------------------------------------------------
            'Methode:       sendEmail
            'Autor:         Julian Jung
            'Beschreibung:  erstellt einen HTML String mit den Informationen und versendet Ihn als Email
            '               an die Adresse die in der WebConfig eingetragen ist. 
            'Erstellt am:   05.06.2009
            '----------------------------------------------------------------------
            Try

                Dim mailBody As String

                mailBody = "<html xmlns=""http://www.w3.org/1999/xhtml"">" & _
    "<head>" & _
       " <title></title>" & _
    "</head>" & _
    "<body>" & _
            "<table style=""font-family:Arial; font-size: 11pt;"" width=""100%""  cellpadding=""2"" cellspacing=""2"">" & _
          "<thead>" & _
                 "<tr>" & _
                    "<td width=""250px"" >Fehlerzeitpunkt:" & _
                   " </td><td width=""85%"" style=""font-size: 12pt; color: #ff0000; font-weight: bold"" >" & Now.ToString & "</td>" & _
             "</tr>" & _
               "<tr >" & _
                   "<td>" & _
                      "Kurzdiagnose:" & _
                   "</td>" & _
                   "<td  style=""font-size: 12pt; color: #ff0000; font-weight: bold"" >" & Kurzdiagnose & "</td>" & _
              "</tr>" & _
          "</thead>" & _
           "<tbody>" & _
                     "<tr>" & _
                    "<td colspan=""2"" style=""border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;"" >&nbsp;" & _
                   "</td>" & _
             "</tr>" & _
               "<tr>" & _
                    "<td></td><td  style=""font-size: 12pt; font-weight: bold"">" & _
             "  <u> Systeminformationen </u>" & _
                   "</td>" & _
              "</tr>" & _
               "<tr>" & _
                   " <td colspan=""2"" style=""border-top-color:Black; border-top-style:solid; border-top-width:1px;"">" & _
                  "      &nbsp;" & _
                 "</td>" & _
              "</tr>" & _
                "<tr>" & _
                    "<td >" & _
    "Sap System:" & _
                 "</td>" & _
                  "<td style=""font-weight: bold"">" & _
                     SapSystem & _
                "</td>" & _
             "</tr>" & _
             "<tr>" & _
              "<tr>" & _
                    "<td >" & _
    "Fehlermeldung:" & _
                 "</td>" & _
                  " <td style=""font-weight: bold"">" & _
                     Fehlermeldung & _
                "</td>" & _
             "</tr>" & _
             "<tr>" & _
                  "<td>" & _
    "Pfad:" & _
                   "</td>" & _
                   "<td style=""font-weight: bold"">" & _
                  Pfad & _
                 "</td>" & _
           "</tr>" & _
              "<tr>" & _
                   "<td>" & _
    "Methodenname:" & _
                  " </td>" & _
                   "<td style=""font-weight: bold"">" & _
                    Methodenname & _
                   "</td>" & _
               "</tr>" & _
               "<tr>" & _
                  "<td>" & _
    "Fehlerzeile:" & _
                   "</td>" & _
                   "<td style=""font-weight: bold"">" & _
                       Fehlerzeile & _
                 "</td>" & _
              "</tr>" & _
             "<tr>" & _
                 "<td>" & _
               "angefragte URL:" & _
                   "</td>" & _
                   "<td style=""font-weight: bold"">" & _
                       angefragteURL & _
                  "</td>" & _
             "</tr>" & _
          "<tr>" & _
                 "<td>" & _
              "Client Browser:" & _
                  "</td>" & _
                   "<td style=""font-weight: bold"">" & _
                  ClientBrowser & _
                 "</td>" & _
             "</tr>" & _
                "<tr>" & _
                  "<td>" & _
              "Client IP: " & _
                  "</td>" & _
                    "<td style=""font-weight: bold"">" & _
                     ClientIP & _
                  "</td>" & _
              "</tr>" & _
                   "<tr>" & _
                    "<td colspan=""2"" style=""border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;"" >&nbsp;" & _
                   "</td>" & _
             "</tr>" & _
              "<tr>" & _
                    "<td colspan=""2"">" & _
                      "  &nbsp;" & _
                  "</td>" & _
              "</tr>" & _
               "<tr>" & _
                    "<td colspan=""2"">" & _
                      "&nbsp;" & _
                  "</td>" & _
              "</tr>" & _
                   "<tr>" & _
                    "<td colspan=""2"" style=""border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;"" >&nbsp;" & _
                   "</td>" & _
             "</tr>" & _
             "<tr>" & _
                  "<td></td> <td   style=""font-size: 12pt; font-weight: bold"">" & _
             "  <u> Portalinformationen </u>" & _
                  "</td>" & _
              "</tr>" & _
              "<tr>" & _
                    "<td colspan=""2""  style=""border-top-color:Black; border-top-style:solid; border-top-width:1px;"" >" & _
                       " &nbsp;" & _
                   " </td>" & _
                  "  </tr>" & _
             "<tr>" & _
                   "<td>" & _
                       " App freundlicher Name:" & _
                  "</td>" & _
                   "<td style=""font-weight: bold"">" & _
                        AppFreundlicherName & _
                   "</td>" & _
               "</tr>" & _
           "<tr>" & _
                 "<td>" & _
             "App Name:" & _
                  "</td>" & _
                  "<td style=""font-weight: bold"">" & _
                    AppName & _
                 "</td>" & _
              "</tr>" & _
               "<tr>" & _
                 "<td>" & _
             "App Pfad:" & _
                   "</td>" & _
                 "<td style=""font-weight: bold"">" & _
                     AppPath & _
                 "</td>" & _
             "</tr>" & _
             "<tr>" & _
                 "<td>" & _
            "App Typ:" & _
            "</td>" & _
                   "<td style=""font-weight: bold"">" & _
                     AppTyp & _
                   "</td>" & _
             "</tr>" & _
               "<tr>" & _
               "<td>" & _
    "Kundename:" & _
                  "</td>" & _
                   "<td style=""font-weight: bold"">" & _
                      KundenName & _
               "</td>" & _
            "</tr>" & _
             "<tr>" & _
                  "<td>" & _
    "Gruppenname:" & _
                  "</td>" & _
                 "<td style=""font-weight: bold"">" & _
                  GruppenName & _
                "</td>" & _
             "</tr>" & _
             "<tr>" & _
                  "<td>" & _
    "Benutzer:" & _
              "</td>" & _
                  "<td style=""font-weight: bold"">" & _
                     Benutzer & _
                 "</td>" & _
            "</tr>" & _
                  "<tr>" & _
                  "<td valign=""top"" >" & _
    "weitere betroffene Kunden:" & _
              "</td>" & _
                  "<td style=""font-weight: bold"">" & _
                     UsedByOtherCustomerHTML & _
                 "</td>" & _
            "</tr>" & _
                             "<tr>" & _
                    "<td colspan=""2"" style=""border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;"" >&nbsp;" & _
                   "</td>" & _
             "</tr>" & _
           "</tbody>" & _
     "</table> " & _
              "  </body>" & _
    "</html>"


                Dim Mail As System.Net.Mail.MailMessage
                Dim smtpMailSender As String = ""
                Dim smtpMailServer As String = ""

                Dim betreff As String
                If Not AnzahlKunden = "0" Then
                    betreff = "Kunden: " & AnzahlKunden
                Else
                    betreff = "Kunde: " & KundenName
                End If
                betreff = betreff & " / APP: " & AppFreundlicherName


                smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
                Mail = New System.Net.Mail.MailMessage(smtpMailSender, ConfigurationManager.AppSettings("PEISTargetEmail"), betreff, mailBody)
                Mail.IsBodyHtml = True
                smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                Dim client As New System.Net.Mail.SmtpClient(smtpMailServer)
                client.Send(Mail)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class
End Namespace

'************************************************
' $History: PEIS.vb $
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 15.07.09   Time: 17:31
' Updated in $/CKAG/Base/Kernel/Common
' Anpassung Peis filter
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 15.06.09   Time: 14:36
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 15.06.09   Time: 13:57
' Updated in $/CKAG/Base/Kernel/Common
' Nachbesserungen Layout PEIS
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 11.06.09   Time: 16:46
' Updated in $/CKAG/Base/Kernel/Common
' PEIS Betreff in Emai angepasst
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 5.06.09    Time: 13:00
' Updated in $/CKAG/Base/Kernel/Common
' kommentierung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 12:47
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 9:27
' Updated in $/CKAG/Base/Kernel/Common
' Peis
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.06.09    Time: 16:36
' Updated in $/CKAG/Base/Kernel/Common
' Peis v1
' 
'************************************************