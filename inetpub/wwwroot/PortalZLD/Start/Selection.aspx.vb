Imports CKG.Base.Business
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

    Private Sub Repeater1_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs)

    End Sub

    '##############################################################################
    '##############################################################################
    '##############################################################################
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

        'Response.Expires = 0
        'Response.Cache.SetNoStore()
        'Response.AppendHeader("Pragma", "no-cache")

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
            Response.Redirect("/PortalZLD/(S(" & Session.SessionID & "))/Start/IframeLogin.aspx")
        End If
        m_User = GetUser(Me)

        '------------------------------------------------------------------------
        Try
            Common.Alert.alert(litAlert, m_User.Customer.CustomerId)
        Catch ex As Exception
            Response.Redirect(ConfigurationManager.AppSettings("Exit"))
        End Try
        '------------------------------------------------------------------------

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

            If Not (m_User.SessionID = Session.SessionID.ToString) Then
                If Not m_User.Customer.AllowMultipleLogin Then
                    Response.Redirect(ConfigurationManager.AppSettings("Exit") & "?DoubleLogin=True")
                End If
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

            '##############################################################################
            '##############################################################################
            '##############################################################################
            'WriteErrorText("TestUH", "Selection", "Page_Load", "vor m_User.Email.Length > 0 And m_User.Customer.ForcePasswordQuestion And m_User.QuestionID = -1")
            '##############################################################################
            '##############################################################################
            '##############################################################################
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
                    Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
                    Literal1.Text &= "			<!-- //" & vbCrLf
                    Literal1.Text &= "				 alert('" & m_User.Groups(0).Message & "');" & vbCrLf
                    Literal1.Text &= "			//-->" & vbCrLf
                    Literal1.Text &= "		</script>" & vbCrLf
                    m_User.Groups(0).Message = ""
                End If
            End If
            '§§§ JVE 28.06.2005/Comment <end>

            Session("BackLink") = Nothing
            'lblHead.Text = "Startseite"


            If Not IsPostBack Then
                lblError.Text = ""
                lblMessage.Text = ""

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

                    Dim cn As SqlClient.SqlConnection
                    cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                    Dim _EmployeeAssigned As New Admin.EmployeeList(m_User.GroupID, m_User.Customer.AccountingArea, cn)
                    _EmployeeAssigned.GetAssigned()
                    If _EmployeeAssigned.DefaultView.Count > 0 Then
                        _EmployeeAssigned.DefaultView.Sort = "employeeHierarchy"
                        Repeater1.DataSource = _EmployeeAssigned.DefaultView
                        Repeater1.DataBind()
                        Repeater1.Visible = True

                        For Each RepItem As RepeaterItem In Repeater1.Controls
                            Dim Ctrl As Control
                            Dim Ctrl2 As Control
                            Ctrl = RepItem.FindControl("divMail")
                            Ctrl2 = RepItem.FindControl("divMailPartner")
                            If m_User.Customer.CustomerContact.Kundenpostfach.Trim.Length > 0 Then
                                Ctrl.Visible = False
                                Ctrl2.Visible = True
                            Else
                                Ctrl.Visible = True
                                Ctrl2.Visible = False
                            End If

                        Next
                    Else
                        Repeater1.Visible = False
                        TableRepaeter.Height = "200px"
                    End If
                    getCustomerNews()
                    getKVPNews()
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
            Page.Title = "Startseite"
        Catch ex As Exception
            If m_App Is Nothing Then
                m_App = New Security.App(m_User)
            End If
            m_App.WriteErrorText(1, m_User.UserName, "Selection", "Page_Load", ex.ToString)

            lblError.Text = "Fehler bei der Ermittlung der Menüpunkte (" & ex.Message & ")"
            lblError.Visible = True
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

    Public Sub getKVPNews()
        'Prüfen, ob zu bewertende KVPs vorliegen
        If Session("ZuBewertendeKVPs") IsNot Nothing Then
            Dim strKVP As String = Session("ZuBewertendeKVPs").ToString()

            If Not String.IsNullOrEmpty(strKVP) Then
                If IsNumeric(strKVP) Then
                    Dim anzKVP As Integer = Integer.Parse(strKVP)
                    If anzKVP > 0 Then
                        lblMessage.Text = "ACHTUNG: Ihre Meinung ist gefragt! Im KVP-Modul warten noch Verbesserungsvorschläge auf Ihre Bewertung!"
                    End If
                Else
                    lblMessage.Text = strKVP
                End If
            End If

            Session("ZuBewertendeKVPs") = Nothing
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        lnkAnsprech.Text = CType(sender, LinkButton).Text
        lnkAnsprech.NavigateUrl = "mailto:" & lnkAnsprech.Text
        lnkFirmenPost.Text = m_User.Customer.CustomerContact.Kundenpostfach
        lnkFirmenPost.NavigateUrl = "mailto:" & m_User.Customer.CustomerContact.Kundenpostfach
        ModalPopupExtender2.Show()

    End Sub
End Class