Imports CKG.Base.Business
Imports CKG.Base.Business.HelpProcedures
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports System.Collections.Generic
Imports System.Text

Namespace Start
    Public Class Selection
        Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "
        Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
        Protected WithEvents litAlert As System.Web.UI.WebControls.Literal
        Protected WithEvents lblHead As System.Web.UI.WebControls.Label
        Protected WithEvents tr4 As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents tbAnwendungen As System.Web.UI.HtmlControls.HtmlTable
        Protected WithEvents tr5 As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents lnkAdmin As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents ucHeader As PageElements.Header
        'Dieser Aufruf ist für den Web Form-Designer erforderlich.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
            'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
            InitializeComponent()
        End Sub

#End Region

        Private m_User As Security.User
        Private m_App As Security.App
        Protected WithEvents ucStyles As PageElements.Styles

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
                Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Start/IframeLogin.aspx")
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

                If m_User.AccountIsLockedOut And Not m_User.AccountIsLockedBy = "User" Then
                    Response.Redirect("ChangePassword.aspx?pwdreq=true")
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

                ucHeader.InitUser(m_User)

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
                            Response.Redirect(strStartMethod, False)

                            Session.Add("StartMethodExecuted", "X")
                        Else
                            blnStartMethod = True
                        End If

                        '**********************************************************




                        '**********************************************************
                        'Ursprüngliche Methode zum Ausführen von StartApplikationen JJ2007.12.14
                        'Bei der Gruppe musste der NameSpace der Applikation angegebene in dem Feld "Startmethode eingegeben werden.
                        'z.B. FFD: CKG.Base.Business.sm_mahnungen 
                        '**********************************************************
                        'Dim tmpSplit() As String = strStartMethod.Split("."c)
                        'If (Not Session(tmpSplit(1)) Is Nothing) Then
                        '    blnStartMethod = True
                        'End If

                        'Dim arr(2) As Object
                        'm_User.SessionID = Session.SessionID.ToString
                        'arr(0) = Me
                        'arr(1) = m_User
                        'arr(2) = ""
                        'Try
                        '    Dim t As Type
                        '    t = System.Reflection.Assembly.GetAssembly(GetType(StartMethodBase)).GetType(strStartMethod, False, True)
                        '    If t Is Nothing Then
                        '        t = System.Reflection.Assembly.GetAssembly(GetType(ReportBase)).GetType(strStartMethod, False, True)
                        '    End If
                        '    If t Is Nothing Then
                        '        t = System.Reflection.Assembly.GetAssembly(GetType(BankBase)).GetType(strStartMethod, False, True)
                        '    End If
                        '    If t Is Nothing Then
                        '        Throw New Exception("Die Startmethode konnte nicht gefunden werden.")
                        '    End If
                        '    Activator.CreateInstance(t, arr)
                        'Catch ex As Exception
                        '    lblError.Text = "Fehler beim Aufrufen der Startmethode (" & ex.Message & ")"
                        'End Try

                        'If (Not Session(tmpSplit(1)) Is Nothing) AndAlso (Session("StartMethodeRedirect") Is Nothing) Then
                        '    blnStartMethod = True
                        'End If
                        '**********************************************************
                    Else
                        blnStartMethod = True
                    End If


                    If blnStartMethod And (Not m_User.Groups(0).Message.Length = 0) And (m_User.ReadMessageCount <= m_User.Groups(0).MaxReadMessageCount) Then

                        Dim msg As String = HelpProcedures.TranslateHTML(m_User.Groups(0).Message, TranslationDirection.ReadHTML)

                        ShowMessageDialog("Information", msg, LogonMsgType.INFO)

                    End If





                End If
                '§§§ JVE 28.06.2005/Comment <end>

                lblHead.Text = "Hauptmenü"
                ucStyles.TitleText = lblHead.Text
                Session("BackLink") = Nothing



                If Not IsPostBack Then
                    lblError.Text = ""

                    If m_User.HighestAdminLevel > Security.AdminLevel.None And m_User.FirstLevelAdmin = False Then
                        lnkAdmin.Visible = True
                    End If

                    'UH: 23.01.2006
                    'Applikationstypen
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
                                tdApp1.Attributes.Add("class", "TaskTitle")
                                tdApp1.Align = "left"
                                tdApp1.Width = "100"
                                tdApp1.ColSpan = "2"
                                litApp = New Literal()
                                litApp.Text = "&nbsp;" & CStr(tmpRow("DisplayName"))
                                tdApp1.Controls.Add(litApp)
                                trApp.Cells.Add(tdApp1)
                                tdApp2 = New HtmlTableCell()
                                tdApp2.Attributes.Add("class", "TaskTitle")
                                tdApp2.VAlign = "left"
                                tdApp2.Width = "100%"
                                trApp.Cells.Add(tdApp2)
                                tbAnwendungen.Rows.Add(trApp)

                                trApp = New HtmlTableRow()
                                tdApp1 = New HtmlTableCell()
                                tdApp1.Attributes.Add("class", "TextLarge")
                                tdApp1.Align = "left"
                                tdApp1.Width = "100"
                                tdApp1.ColSpan = "2"
                                litApp = New Literal()
                                litApp.Text = "&nbsp;"
                                tdApp1.Controls.Add(litApp)
                                trApp.Cells.Add(tdApp1)
                                tdApp2 = New HtmlTableCell()
                                tdApp2.Attributes.Add("class", "TextLarge")
                                tdApp2.VAlign = "left"
                                tdApp2.Width = "100%"
                                Dim blnAlternate As Boolean = False
                                litApp = New Literal()
                                litApp.Text = "<table cellspacing=""0"" border=""0"" style=""width:100%;border-collapse:collapse;"">" & vbCrLf
                                'Anwendungen je Bereich anzeigen (Links)
                                Dim appUrl As String
                                Dim appSymbol As String
                                Dim appInMenu As Boolean


                                For intRows = 0 To dvAppLinks.Count - 1
                                    ' --- 18.12.2013, MJE
                                    ' --- MVC Integration:
                                    ' ---
                                    Dim dRow As DataRowView = dvAppLinks(intRows)
                                    If dRow("AppURL").ToString().ToLower().StartsWith("mvc/") Then
                                        appUrl = BtBaseMvc.MVC.MvcPrepareUrl(dRow("AppURL").ToString(), dRow("AppID").ToString(), m_User.UserName)
                                    Else
                                        appUrl = GetUrlString(dvAppLinks(intRows)("AppURL"), dvAppLinks(intRows)("AppID").ToString)
                                    End If

                                    appUrl = appUrl & "&cp=" & GetUserContextParams(dRow("AppID").ToString())

                                    ' Url encoden für die Verwendung als Query Params
                                    appUrl = HttpUtility.UrlEncode(appUrl)
                                    appUrl = Convert.ToBase64String(Encoding.UTF8.GetBytes(appUrl.ToCharArray()))

                                    ' Jetzt besteht die neue url aus: appid, original url unverändert übernehmen
                                    appUrl = String.Concat("../Start/Log.aspx?", "APP-ID=", dRow("AppID"), "&url=", appUrl)

                                    If blnAlternate Then
                                        litApp.Text &= "<tr class=""MainmenuItemAlternate"">" & vbCrLf
                                    Else
                                        litApp.Text &= "<tr>" & vbCrLf
                                    End If
                                    blnAlternate = Not blnAlternate

                                    appInMenu = CType(dvAppLinks(intRows)("AppInMenu"), Boolean)

                                    appSymbol = "../Images/arrowgrey.gif"
                                    litApp.Text &= "<td><img src=""" & appSymbol & """ border=""0"" /></td>" & vbCrLf

                                    litApp.Text &= "<td class=""MainmenuItem"" nowrap=""nowrap"">&nbsp;<a " & " href=""" & appUrl & """ target=""_self"">" & CStr(dvAppLinks(intRows)("AppFriendlyName")) & "</a>&nbsp;</td>" & vbCrLf
                                    '---------------------------------------------
                                    Dim strTemp As String = String.Empty
                                    If Not TypeOf dvAppLinks(intRows)("AppComment") Is DBNull Then
                                        strTemp = CStr(dvAppLinks(intRows)("AppComment"))
                                    End If
                                    litApp.Text &= "<td class=""MainmenuItemComment"" style=""width:100%;"">&nbsp;&nbsp;<span class=""MainmenuItemComment"">" & strTemp & "</span></td>" & vbCrLf

                                    litApp.Text &= "</tr>" & vbCrLf
                                Next

                                litApp.Text &= "</table>" & vbCrLf
                                tdApp2.Controls.Add(litApp)
                                trApp.Cells.Add(tdApp2)
                                tbAnwendungen.Rows.Add(trApp)
                            End If
                        Next
                    End If
                    conn.Close()
                    conn.Dispose()
                    da.Dispose()

                End If
            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "Selection", "Page_Load", ex.ToString)

                lblError.Text = "Fehler bei der Ermittlung der Menüpunkte (" & ex.Message & ")"
                lblError.Visible = True
            End Try
        End Sub

        Function GetUserContextParams(appID As String) As String
            Dim userObject As User = GetUser(Me)
            If (userObject Is Nothing Or userObject.Customer Is Nothing) Then
                Return ""
            End If

            Return String.Format("{0}_{1}_{2}_{3}_{4}", appID, userObject.UserID, userObject.Customer.CustomerId, userObject.Customer.KUNNR, Log.PortalType)
        End Function

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
        '##############################################################################
        '##############################################################################
        '##############################################################################
    End Class
End Namespace

' ************************************************
' $History: Selection.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 13:39
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 3.03.11    Time: 17:02
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 7.09.10    Time: 10:57
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 28.06.10   Time: 9:50
' Updated in $/CKAG/portal/Start
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 21.04.09   Time: 13:51
' Updated in $/CKAG/portal/Start
' ITA 2805 nachbesserungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 21.04.09   Time: 13:12
' Updated in $/CKAG/portal/Start
' ITA 2805 testfertig
' 
' *****************  Version 5  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/portal/Start
' ITA 2152 und 2158
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 17.06.08   Time: 9:52
' Updated in $/CKAG/portal/Start
' Bugfix Statmessage!
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/portal/Start
' Migration OR
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/portal/Start
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:21
' Created in $/CKAG/portal/start
' 
' *****************  Version 26  *****************
' User: Jungj        Date: 14.12.07   Time: 10:26
' Updated in $/CKG/Portal/Start
' ITA 1466/1499 Änderung an Selection.aspx für Startmehtoden der Händler
' z.b. AKF/RTFS/FFD Fällige Vorgänge Händler 
' 
' *****************  Version 25  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Portal/Start
' ITA:1440
' 
' *****************  Version 24  *****************
' User: Uha          Date: 10.09.07   Time: 13:23
' Updated in $/CKG/Portal/Start
' ITA 1280: Bugfix III; ITA 1263: Testversion Translation
' 
' *****************  Version 23  *****************
' User: Uha          Date: 30.08.07   Time: 18:43
' Updated in $/CKG/Portal/Start
' ITA 1280: Bugfix II
' 
' *****************  Version 22  *****************
' User: Uha          Date: 30.08.07   Time: 15:17
' Updated in $/CKG/Portal/Start
' ITA 1280: Bugfix
' 
' *****************  Version 21  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Portal/Start
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 20  *****************
' User: Uha          Date: 13.08.07   Time: 14:37
' Updated in $/CKG/Portal/Start
' In Report01_2 kann sich jetzt die ExcelTabelle von der Tabelle im
' HTML-Datagrid unterscheiden.
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 23.07.07   Time: 11:51
' Updated in $/CKG/Portal/Start
' Session Variablen der Reports löschen
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 11.07.07   Time: 14:47
' Updated in $/CKG/Portal/Start
' Session Variablen der Reports löschen
' 
' *****************  Version 17  *****************
' User: Uha          Date: 21.06.07   Time: 18:45
' Updated in $/CKG/Portal/Start
' Bugfixing VFS 2
' 
' *****************  Version 16  *****************
' User: Uha          Date: 31.05.07   Time: 11:41
' Updated in $/CKG/Portal/Start
' ITA 1077 - Login bei bereits aktiver Anmeldung ermöglichen
' 
' *****************  Version 15  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal/Start
' 
' ************************************************