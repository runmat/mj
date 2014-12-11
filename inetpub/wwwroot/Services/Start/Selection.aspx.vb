Imports CKG.Base.Business
Imports CKG.Base.Business.HelpProcedures
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Selection
    Inherits System.Web.UI.Page
    Private m_User As Security.User
    Private m_App As Security.App

    Public Function GetTargetString(ByVal strAppName As String, ByVal strAppUrl As String) As String
        If Left(strAppName, 3) = "ext" AndAlso Left(strAppUrl, 4) = "http" Then
            Return "_blank"
        Else
            Return "_self"
        End If
    End Function

    Public Function getAppParameters(ByVal strAppID As String, ByRef paramlist As String) As Boolean
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim result As New DataTable()

        command.CommandType = CommandType.Text
        command.CommandText = "SELECT * FROM ApplicationParamlist WHERE id_app = " & strAppID
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
        command.Connection = conn
        Try

            conn.Open()
            adapter.SelectCommand = command
            adapter.Fill(result)
            paramlist = String.Empty
            If Not (result.Rows.Count = 0) Then
                paramlist = result.Rows(0)("paramlist").ToString
            End If
            Return True
        Catch ex As Exception
            paramlist = String.Empty
            Return False
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Function

    Public Function GetUrlString(ByVal strAppUrl As String, ByVal strAppID As String) As String
        Dim paramlist As String = ""
        Dim getParamList As Boolean

        getParamList = getAppParameters(strAppID, paramlist)

        If Left(strAppUrl, 4) = "http" Then
            Return strAppUrl
        Else
            Return strAppUrl & "?AppID=" & strAppID & paramlist
        End If
    End Function

    Public Sub WriteErrorText(ByVal strWebUser As String, ByVal strObject As String, ByVal strTask As String, ByVal strExceptionToString As String)
        Try
            If InStr(strExceptionToString, "System.Threading.ThreadAbortException") = 0 Then
                Dim FileNumber As Short = CShort(FreeFile())
                Dim FileName As String = ConfigurationManager.AppSettings("ExcelPath") & Format(Now, "yyyyMMdd") & ".txt"

                'File öffnen
                Try
                    Dim FileAttribut As FileAttribute = GetAttr(FileName)
                    FileOpen(FileNumber, FileName, OpenMode.Append, OpenAccess.Default, OpenShare.Shared)
                Catch ex0 As Exception
                    Try
                        If TypeOf ex0 Is System.IO.FileNotFoundException Then
                            FileOpen(FileNumber, FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)
                        Else
                            Throw ex0
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Try

                'Zeile schreiben
                PrintLine(FileNumber, Format(Now, "hh:mm:ss,fff") & vbTab & strWebUser & vbTab & strObject & vbTab & strTask & vbTab & strExceptionToString)

                'File schließen
                FileClose(FileNumber)
            End If
        Catch exOut As Exception
            'Fehler beim Fehler schreiben
            '=> keine Idee!
        End Try
    End Sub

    Private Sub Selection_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        Session("ShowOtherString") = ""
        Session("Authorization") = Nothing
        Session("AuthorizationID") = Nothing
        Session("SelectedDealer") = Nothing
        Session("WaitObject") = Nothing
        Session("InaktiveVertraege") = Nothing
        Session("SammelAutorisierungen") = Nothing
        Session("dsLogData") = Nothing
        Session("MultiResult") = Nothing
        Session("objVFS02") = Nothing
        Session("objHandler") = Nothing
        Session("ResultTable") = Nothing
        Session("ExcelTable") = Nothing

        Dim cn As SqlClient.SqlConnection

        Dim iLoop As Integer
        Dim item As Object
        Dim sName As String
        For iLoop = Session.Contents.Count - 1 To 0 Step -1
            item = Session.Contents.Keys.Item(iLoop)
            If Not IsNothing(item) Then
                sName = item.ToString
                If Left(sName, 3) = "App" Then
                    Session.Contents.Item(iLoop) = Nothing
                End If
                If Left(sName, 3) = "../" Then
                    Session.Contents.Item(iLoop) = Nothing
                End If
            End If
        Next
        If User.Identity.Name = "IFrameLogon" Then
            'System.Web.Security.FormsAuthentication.(m_User.UserID.ToString, False)
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/IframeLogin.aspx")
        End If
        m_User = GetUser(Me)

        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)

        Try
            If m_User.FailedLogins > 0 Then
                If m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID > -1 Then
                    Try
                        Response.Redirect("ChangePassword.aspx?pwdreq=true")
                    Catch DoNothing As Exception
                    End Try
                Else
                    Try
                        Response.Redirect(ConfigurationManager.AppSettings("Exit"))
                    Catch DoNothing As Exception
                    End Try
                End If
            End If

            If m_User.LoggedOn = False Then
                Try
                    Response.Redirect(ConfigurationManager.AppSettings("Exit"))
                Catch DoNothing As Exception
                End Try
            End If

            m_App = New Security.App(m_User)

            If m_User.PasswordExpired And Not m_User.InitialPassword = True Then
                Try
                    Response.Redirect("ChangePassword.aspx?pwdexp=true")
                Catch DoNothing As Exception
                End Try
            ElseIf m_User.InitialPassword = True Then
                Try
                    Response.Redirect("FirstLogin.aspx")
                Catch DoNothing As Exception
                End Try
            End If

            If m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID = -1 Then
                Try
                    Response.Redirect("ChangePassword.aspx?qstreq=true")
                Catch DoNothing As Exception
                End Try
            End If


            '§§§ JVE 28.06.2005/Comment <begin>
            'DAs ganze wird nur benötigt, wenn eine Anwendung eine Startmethode hat (z.B. Mahnungen bei FFD) !?

            'Erwartet "Namespace.Object" als String:
            Dim strStartMethod As String
            Dim blnStartMethod As Boolean = False
            If m_User.Groups.HasGroups Then
                strStartMethod = m_User.Groups(0).StartMethod

                If Not strStartMethod Is Nothing AndAlso strStartMethod.Length > 0 Then

                    '**********************************************************
                    'Neue Methode zum Ausführen von StartApplikationen JJ2007.12.14
                    'Bei der Gruppe musste die Relative URL zur der Applikation in dem Feld "Startmethode eingegeben werden mit Parameter der APPlikationID und weiteren Parametern die der Report benötigt.
                    'z.B.FFD: ../Applications/AppFFD/Forms/Report33.aspx?Appid=135
                    'im gegensatz zur ursprünglichen Methode mussten kleine Änderungen am Report33 vorgenommen werden
                    'z.B. AKF: ../Components/ComCommon/Finance/Report44.aspx&AppId=804&ZZKKBER=0001
                    '**********************************************************
                    'Erstes login des Benutzers mit StartMethode, bitte ausführen, und in Session vermerken das Startmethode ausgeführt wurde
                    If Session("StartMethodExecuted") Is Nothing Then

                        Try
                            Response.Redirect(strStartMethod, False)
                        Catch DoNothing As Exception
                            Response.Write(DoNothing.Message)
                        End Try
                        blnStartMethod = True
                        Session.Add("StartMethodExecuted", "X")
                    Else
                        blnStartMethod = True
 

                    End If

                Else
                    blnStartMethod = True
                End If


                If blnStartMethod And (Not m_User.Groups(0).Message.Length = 0) And (m_User.ReadMessageCount <= m_User.Groups(0).MaxReadMessageCount) Then


                    Dim msg As String = HelpProcedures.TranslateHTML(m_User.Groups(0).Message, TranslationDirection.ReadHTML)
                
                    ShowMessageDialog("Information", msg, LogonMsgType.INFO)


                End If
            End If
            '§§§ JVE 28.06.2005/Comment <end>

            Session("BackLink") = Nothing
            'lblHead.Text = "Startseite"


            If Not IsPostBack Then
                lblError.Text = ""

                ' Master-Admins direkt zur Benutzerverwaltung umleiten
                If m_User.HighestAdminLevel = Security.AdminLevel.Master Then
                    Response.Redirect("/Services/(S(" & Session.SessionID & "))/Admin/UserManagement.aspx", True)
                End If

                If m_User.HighestAdminLevel > Security.AdminLevel.None And m_User.FirstLevelAdmin = False Then
                    lnkAdmin.Visible = True
                End If

                If m_User.Title.Trim.Length + m_User.FirstName.Trim.Length + m_User.LastName.Trim.Length > 0 Then
                    lblGreet.Text = "Hallo " & m_User.Title & " " & m_User.FirstName & " " & m_User.LastName & ", herzlich willkommen!  (Letztes Login: " & m_User.LastLogin & " Uhr)"
                End If

                'UH: 23.01.2006
                'Applikationstypen
                'Prüfe, ob SessionId schon in DB existent
                Dim table As DataTable
                Dim command As New SqlClient.SqlCommand()
                Dim blnReturn As Boolean = True

                command.CommandText = "SELECT AppType,DisplayName FROM ApplicationType ORDER BY Rank"

                Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim da As New SqlClient.SqlDataAdapter(command)
                command.Connection = conn
                conn.Open()
                table = New DataTable()
                da.Fill(table)
                If table.Rows.Count > 0 Then

                    'ggf. später Hierarchisches Grid!!
                    Dim tmpRow As DataRow
                    For Each tmpRow In table.Rows
                        Dim dvAppLinks As DataView = m_User.Applications.DefaultView
                        dvAppLinks.RowFilter = "AppInMenu=1"

                        If dvAppLinks.Count > 16 Then
                            'trPlaceHolder.Visible = "false"
                        End If

                        dvAppLinks.RowFilter = "AppType='" & CStr(tmpRow("AppType")) & "' AND AppInMenu=1"
                        If dvAppLinks.Count > 0 Then

                            dvAppLinks.Sort = "AppRank"
                            Dim trApp As HtmlTableRow
                            Dim tdApp1 As HtmlTableCell
                            Dim tdApp2 As HtmlTableCell
                            Dim litApp As Literal
                            Dim intRows As Int32

                            'Bereichsnamen (Reports, Helpdesk, ....)
                            trApp = New HtmlTableRow()
                            tdApp1 = New HtmlTableCell()
                            tdApp1.Attributes.Add("class", "Menutitle")
                            tdApp1.Align = "left"
                            tdApp1.Width = "100%"
                            tdApp1.ColSpan = "0"
                            litApp = New Literal()
                            litApp.Text = "&nbsp;" & CStr(tmpRow("DisplayName"))
                            tdApp1.Controls.Add(litApp)
                            trApp.Cells.Add(tdApp1)
                            tdApp2 = New HtmlTableCell()
                            tdApp2.Attributes.Add("class", "")
                            tdApp2.VAlign = "left"
                            tdApp2.Width = "100%"
                            trApp.Cells.Add(tdApp2)
                            trApp = New HtmlTableRow()
                            tdApp2 = New HtmlTableCell()
                            tdApp2.VAlign = "left"
                            tdApp2.Width = "100%"

                            'Anwendungen je Bereich anzeigen (Links)
                            Dim appUrl As String
                            'Dim appSymbol As String
                            Dim appInMenu As Boolean
                            Dim blnAlternate As Boolean = False

                            litApp = New Literal()
                            litApp.Text = "<table cellspacing=""0"" border=""0"" style=""width:100%;border-collapse:collapse;"">" & vbCrLf



                            For intRows = 0 To dvAppLinks.Count - 1
                                appUrl = GetUrlString(dvAppLinks(intRows)("AppURL"), dvAppLinks(intRows)("AppID").ToString)
                                appUrl = Replace(appUrl, "AppVFS", "AppGenerali")

                                appInMenu = CType(dvAppLinks(intRows)("AppInMenu"), Boolean)


                                litApp.Text &= "<td width=""100%"" class=""MainmenuItemAlternate"" nowrap=""nowrap"">&nbsp;<a width=""100%"" class=""MainmenuLink"" href=""" & appUrl & """ target=""_self"">" & CStr(dvAppLinks(intRows)("AppFriendlyName")) & "</a></td>" & vbCrLf

                                litApp.Text &= "</tr>" & vbCrLf
                            Next

                            litApp.Text &= "</table>" & vbCrLf
                            tdApp2.Controls.Add(litApp)
                            trApp.Cells.Add(tdApp2)
                        End If
                    Next

                End If
                conn.Close()
                conn.Dispose()
                da.Dispose()

                If Not m_User Is Nothing AndAlso Not m_User.Customer Is Nothing AndAlso Not m_User.Groups.Count = 0 Then

                    Dim result As New DataTable()


                    If cn.State = ConnectionState.Closed Then
                        cn.Open()
                    End If

                    Dim daApp As New SqlClient.SqlDataAdapter("SELECT *, Name2 + ',' + Name1 + ' (' + [position] + ')' AS EmployeeName" & _
                                                              " FROM Contact INNER JOIN ContactGroups ON Contact.id = ContactGroups.ContactID " & _
                                                              " WHERE     (ContactGroups.CustomerID = @CustomerID) AND (ContactGroups.GroupID = @GroupID)", cn)


                    daApp.SelectCommand.Parameters.AddWithValue("@CustomerID", m_User.Customer.CustomerId)
                    daApp.SelectCommand.Parameters.AddWithValue("@GroupID", m_User.GroupID)
                    daApp.Fill(result)

                    If result.DefaultView.Count > 0 Then
                        'result.DefaultView.Sort = "employeeHierarchy"

                        If m_User.Customer.CustomerContact.Kundenpostfach.Trim.Length > 0 Then
                            For Each row In result.Rows
                                row("mail") = m_User.Customer.CustomerContact.Kundenpostfach.Trim
                            Next
                        End If

                        If m_User.Customer.CustomerContact.Kundenhotline.Trim.Length > 0 Then
                            For Each row In result.Rows
                                row("Telefon") = m_User.Customer.CustomerContact.Kundenhotline.Trim
                            Next
                        End If

                        If m_User.Customer.CustomerContact.Kundenfax.Trim.Length > 0 Then
                            For Each row In result.Rows
                                row("fax") = m_User.Customer.CustomerContact.Kundenfax.Trim
                            Next
                        End If

                        Repeater1.DataSource = result.DefaultView
                        Repeater1.DataBind()
                        Repeater1.Visible = True

                    Else
                        Repeater1.Visible = False
                        TableRepaeter.Height = "200px"
                    End If
                    getCustomerNews()
                Else
                    Repeater1.Visible = False
                    imgNews1.Visible = False
                    imgNews2.Visible = False
                    lblNews1.Visible = False
                    lblNews2.Visible = False
                    TableRepaeter.Height = "200px"
                    TableNews.Height = "200px"
                End If
            End If
            Me.Page.Title = "Startseite"

        Catch ex As Exception
            If m_App Is Nothing Then
                m_App = New Security.App(m_User)
            End If
            m_App.WriteErrorText(1, m_User.UserName, "Selection", "Page_Load", ex.ToString)

            lblError.Text = "Fehler bei der Ermittlung der Menüpunkte (" & ex.Message & ")"
            lblError.Visible = True
        Finally
            cn.Close()
        End Try
    End Sub

    Public Sub getCustomerNews()
        Dim conn As New SqlClient.SqlConnection()
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim result As New DataTable()

        command.CommandType = CommandType.Text
        command.CommandText = "SELECT * FROM CustomerNews WHERE CustomerID = " & m_User.Customer.CustomerId
        command.CommandText &= " AND (aktiv=1)  And (GueltigBis >= CONVERT(DATETIME, '" & Today & "', 104))"
        conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
        command.Connection = conn
        Try

            conn.Open()
            adapter.SelectCommand = command
            adapter.Fill(result)

            If result.Rows.Count > 0 Then
                If Not result.Rows.Count > 2 Then
                    Dim NewsCount As String = 1
                    For i As Integer = 0 To result.Rows.Count - 1
                        If result.Rows(i)("Imagepath").ToString.Length > 0 Then
                            If NewsCount > 1 Then
                                imgNews2.Visible = True
                                imgNews2.ImageUrl = result.Rows(i)("Imagepath").ToString.Trim
                            Else
                                imgNews1.Visible = True
                                imgNews1.ImageUrl = result.Rows(i)("Imagepath").ToString.Trim
                            End If
                        End If
                        If result.Rows(i)("Text").ToString.Length > 0 Then
                            If NewsCount > 1 Then
                                lblNews2.Visible = True
                                lblNews2.Text = result.Rows(i)("Text").ToString
                                lblNews2.Text = Replace(lblNews2.Text, "}", ">")
                                lblNews2.Text = Replace(lblNews2.Text, "{", "<")
                            Else
                                lblNews1.Visible = True
                                lblNews1.Text = result.Rows(i)("Text").ToString
                                lblNews1.Text = Replace(lblNews1.Text, "}", ">")
                                lblNews1.Text = Replace(lblNews1.Text, "{", "<")
                            End If

                        End If
                        NewsCount += 1
                    Next

                Else

                End If

            Else
                imgNews1.Visible = False
                imgNews2.Visible = False
                lblNews1.Visible = False
                lblNews2.Visible = False
            End If

        Catch ex As Exception

        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub


    Private Sub ShowMessageDialog(ByVal title As String, ByVal text As String, ByVal type As LogonMsgType)

        Dim cScript As String = "window.showModalDialog(""LogonMessage.aspx"",null,""dialogWidth:605px; dialogHeight:405px; center:yes; scroll:no;"");"
        Dim msgData As LogonMsgData = New LogonMsgData()



        msgData.MsgType = type
        msgData.MsgTitle = title
        msgData.MsgText = text
        Session("LOGONMSGDATA") = msgData

        Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
        Literal1.Text &= "			<!-- //" & vbCrLf
        Literal1.Text &= cScript & vbCrLf
        Literal1.Text &= "			//-->" & vbCrLf
        Literal1.Text &= "		</script>" & vbCrLf
        m_User.Groups(0).Message = ""



    End Sub

End Class