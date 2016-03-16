Option Explicit On
Option Infer On

Imports System.Configuration
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports CKG.Base.Kernel.Security
Imports System.Web.SessionState
Imports Telerik.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Drawing

Namespace Kernel.Common
    Public Class Common

        Private m_User As User
        Private m_strTitleText As String
        Private Shared m_strAppKey As String = ConfigurationManager.AppSettings("ApplicationKey")

        Public Shared Sub GetAppIDFromQueryString(ByVal page As System.Web.UI.Page)
            If page.Request.QueryString("AppID") Is Nothing Then

                page.Response.Redirect("/" & m_strAppKey & "/(S(" & page.Session.SessionID.ToString & "))/Start/Selection.aspx")
            End If
            If page.Request.QueryString("AppID").Length > 0 Then
                page.Session("AppID") = page.Request.QueryString("AppID").ToString
            End If
        End Sub

        Public Shared Sub CheckAllControl(ByVal inControl As System.Web.UI.Control, ByVal inTranslation As DataTable)
            If inControl.HasControls Then
                Dim rows() As DataRow
                Dim control As System.Web.UI.Control
                Dim label As System.Web.UI.WebControls.Label
                Dim linkbutton As System.Web.UI.WebControls.LinkButton
                Dim radiobutton As System.Web.UI.WebControls.RadioButton
                Dim textbox As System.Web.UI.WebControls.TextBox

                For Each control In inControl.Controls
                    'Ist das Control (incl. Unter-Controls) überhaupt sichtbar?
                    'Bzw.: Ist das Control progammatisch unsichtbar gesetzt?
                    '=> Translation überflüssig!

                    If control.Visible Then
                        CheckAllControl(control, inTranslation)
                        rows = inTranslation.Select("ControlID = '" & control.ID & "'")
                        If rows.Length = 1 Then
                            Select Case UCase(CStr(rows(0)("FieldType")))
                                Case "COL"
                                    If TypeOf control Is System.Web.UI.WebControls.LinkButton Then
                                        If TypeOf inControl.Parent Is System.Web.UI.WebControls.DataGridItem OrElse TypeOf inControl.Parent Is System.Web.UI.WebControls.GridViewRow Then
                                            If CBool(rows(0)("Visibility")) Then
                                                linkbutton = CType(control, System.Web.UI.WebControls.LinkButton)
                                                If linkbutton.Visible Then
                                                    If (rows(0)("Content") Is System.DBNull.Value) Or (rows(0)("Content") Is System.String.Empty) Then
                                                        'wenn in der Datenbank nichts als übersetzung eingetragen ist, soll er nichts machen!
                                                    Else
                                                        linkbutton.Text = CStr(rows(0)("Content"))
                                                    End If
                                                End If
                                            Else
                                                If TypeOf inControl.Parent Is System.Web.UI.WebControls.DataGridItem Then
                                                    'wenn DataGrid                    
                                                    Dim tmpDatagrid As System.Web.UI.WebControls.DataGrid
                                                    tmpDatagrid = CType(inControl.Parent.Parent.Parent, System.Web.UI.WebControls.DataGrid)
                                                    Dim tmpColumn As System.Web.UI.WebControls.DataGridColumn
                                                    For Each tmpColumn In tmpDatagrid.Columns
                                                        If tmpColumn.HeaderText = CStr(control.ID) Then
                                                            tmpColumn.Visible = False
                                                        End If
                                                    Next
                                                Else
                                                    'wenn GridView
                                                    Dim tmpGridView As System.Web.UI.WebControls.GridView
                                                    tmpGridView = CType(inControl.Parent.Parent.Parent, System.Web.UI.WebControls.GridView)
                                                    Dim tmpColumn As System.Web.UI.WebControls.DataControlField
                                                    For Each tmpColumn In tmpGridView.Columns
                                                        If tmpColumn.HeaderText = CStr(control.ID) Then
                                                            tmpColumn.Visible = False
                                                        End If
                                                    Next



                                                End If
                                            End If
                                        End If
                                    End If


                                    'Funktioniert irgendwie nicht, da immer noch spalten stehen bleibem im Grid, obwohl Cell.Visible=false JJ2007.11.12
                                    '--------------------------------------------------------                
                                    'If TypeOf control.Parent Is System.Web.UI.WebControls.TableCell Then
                                    '    Dim tmpCell As System.Web.UI.WebControls.TableCell
                                    '    tmpCell = CType(control.Parent, System.Web.UI.WebControls.TableCell)
                                    '    tmpCell.Visible = CBool(rows(0)("Visibility"))
                                    '    If tmpCell.Visible Then
                                    '        If (rows(0)("Content") Is System.DBNull.Value) Or (rows(0)("Content") Is System.String.Empty) Then
                                    '            'wenn in der Datenbank nichts als übersetzung eingetragen ist, soll er nichts machen!
                                    '        Else
                                    '            If TypeOf control Is System.Web.UI.WebControls.LinkButton Then
                                    '                linkbutton = CType(control, System.Web.UI.WebControls.LinkButton)
                                    '                linkbutton.Text = CStr(rows(0)("Content"))
                                    '            ElseIf TypeOf control Is System.Web.UI.WebControls.Label Then
                                    '                label = CType(control, System.Web.UI.WebControls.Label)
                                    '                label.Text = CStr(rows(0)("Content"))
                                    '            End If
                                    '        End If
                                    '    End If
                                    'End If
                                    '--------------------------------------------------------
                                Case "LBL"
                                    If TypeOf control Is System.Web.UI.WebControls.Label Then
                                        label = CType(control, System.Web.UI.WebControls.Label)
                                        label.Visible = CBool(rows(0)("Visibility"))
                                        If label.Visible Then
                                            If (rows(0)("Content") Is System.DBNull.Value) Or (rows(0)("Content") Is System.String.Empty) Then
                                                'wenn in der Datenbank nichts als übersetzung eingetragen ist, soll er nichts machen!
                                            Else
                                                label.Text = CStr(rows(0)("Content"))
                                            End If
                                        End If
                                    End If
                                Case "LB"

                                    If TypeOf control Is System.Web.UI.WebControls.LinkButton Then
                                        linkbutton = CType(control, System.Web.UI.WebControls.LinkButton)
                                        linkbutton.Visible = CBool(rows(0)("Visibility"))
                                        If linkbutton.Visible Then
                                            If (rows(0)("Content") Is System.DBNull.Value) Or (rows(0)("Content") Is System.String.Empty) Then
                                                'wenn in der Datenbank nichts als übersetzung eingetragen ist, soll er nichts machen!
                                            Else
                                                linkbutton.Text = CStr(rows(0)("Content"))
                                            End If
                                        End If
                                    End If

                                Case "RB"
                                    If TypeOf control Is System.Web.UI.WebControls.RadioButton Then
                                        radiobutton = CType(control, System.Web.UI.WebControls.RadioButton)
                                        radiobutton.Visible = CBool(rows(0)("Visibility"))
                                        If radiobutton.Visible Then
                                            If (rows(0)("Content") Is System.DBNull.Value) Or (rows(0)("Content") Is System.String.Empty) Then
                                                'wenn in der Datenbank nichts als übersetzung eingetragen ist, soll er nichts machen!
                                            Else
                                                radiobutton.Text = CStr(rows(0)("Content"))
                                            End If

                                        End If
                                    End If
                                Case "TR"
                                    If TypeOf control Is System.Web.UI.HtmlControls.HtmlTableRow Then
                                        control.Visible = CBool(rows(0)("Visibility"))
                                        If control.Visible Then
                                            TranslateTrLabel(inControl, "LBL_" & UCase(CStr(rows(0)("FieldName"))), CStr(rows(0)("Content")))
                                            'TranslateTrLinkButton(inControl, "LB_" & UCase(CStr(rows(0)("FieldName"))), CStr(rows(0)("Content")))
                                            'TranslateTrRadioButton(inControl, "RB_" & UCase(CStr(rows(0)("FieldName"))), CStr(rows(0)("Content")))
                                        End If
                                    End If
                                Case "TXT"
                                    If TypeOf control Is System.Web.UI.WebControls.TextBox Then
                                        textbox = CType(control, System.Web.UI.WebControls.TextBox)
                                        textbox.Visible = CBool(rows(0)("Visibility"))
                                        If textbox.Visible Then
                                            If (rows(0)("Content") Is System.DBNull.Value) Or (rows(0)("Content") Is System.String.Empty) Then
                                                'wenn in der Datenbank nichts als übersetzung eingetragen ist, soll er nichts machen!
                                            Else
                                                textbox.Text = CStr(rows(0)("Content"))
                                            End If
                                            'tooltip Lesen
                                            If (rows(0)("ToolTip") Is System.DBNull.Value) OrElse (rows(0)("ToolTip") Is System.String.Empty) Then
                                                'wenn in der Datenbank nichts als übersetzung eingetragen ist, soll er nichts machen!
                                            Else
                                                textbox.ToolTip = CStr(rows(0)("ToolTip"))
                                            End If

                                        End If
                                    End If
                                Case Else

                            End Select
                        End If
                    End If
                Next
            End If
        End Sub

        'UHA Origial Algorithmus 
        '---------------------------------------------------------------------------------------------
        'Public Shared Sub CheckAllControl(ByVal inControl As System.Web.UI.Control, ByVal inTranslation As DataTable)
        '    If inControl.HasControls Then
        '        Dim rows() As DataRow
        '        Dim control As System.Web.UI.Control
        '        Dim label As System.Web.UI.WebControls.Label
        '        Dim linkbutton As System.Web.UI.WebControls.LinkButton
        '        Dim radiobutton As System.Web.UI.WebControls.RadioButton

        '        For Each control In inControl.Controls
        '            'Ist das Control (incl. Unter-Controls) überhaupt sichtbar?
        '            'Bzw.: Ist das Control progammatisch unsichtbar gesetzt?
        '            '=> Translation überflüssig!
        '            If control.Visible Then
        '                CheckAllControl(control, inTranslation)
        '                rows = inTranslation.Select("ControlID = '" & control.ID & "'")
        '                If rows.Length = 1 Then
        '                    Select Case UCase(CStr(rows(0)("FieldType")))
        '                        Case "COL"
        '                            If TypeOf control Is System.Web.UI.WebControls.LinkButton Then
        '                                If TypeOf inControl.Parent Is System.Web.UI.WebControls.DataGridItem Then
        '                                    If CBool(rows(0)("Visibility")) Then
        '                                        linkbutton = CType(control, System.Web.UI.WebControls.LinkButton)
        '                                        If linkbutton.Visible Then
        '                                            linkbutton.Text = CStr(rows(0)("Content"))
        '                                        End If
        '                                    Else
        '                                        Dim tmpDatagrid As System.Web.UI.WebControls.DataGrid
        '                                        tmpDatagrid = CType(inControl.Parent.Parent.Parent, System.Web.UI.WebControls.DataGrid)
        '                                        Dim tmpColumn As System.Web.UI.WebControls.DataGridColumn
        '                                        For Each tmpColumn In tmpDatagrid.Columns
        '                                            If tmpColumn.HeaderText = CStr(control.ID) Then
        '                                                tmpColumn.Visible = False
        '                                            End If
        '                                        Next
        '                                    End If
        '                                End If
        '                            End If
        '                        Case "LBL"
        '                            If TypeOf control Is System.Web.UI.WebControls.Label Then
        '                                label = CType(control, System.Web.UI.WebControls.Label)
        '                                label.Visible = True
        '                                If CBool(rows(0)("Visibility")) Then
        '                                    label.Text = CStr(rows(0)("Content"))
        '                                Else
        '                                    label.Text = " "
        '                                End If
        '                            End If
        '                        Case "LB"
        '                            If TypeOf control Is System.Web.UI.WebControls.LinkButton Then
        '                                linkbutton = CType(control, System.Web.UI.WebControls.LinkButton)
        '                                linkbutton.Visible = CBool(rows(0)("Visibility"))
        '                                If linkbutton.Visible Then
        '                                    linkbutton.Text = CStr(rows(0)("Content"))
        '                                Else
        '                                    linkbutton.Text = " "
        '                                End If
        '                            End If
        '                        Case "RB"
        '                            If TypeOf control Is System.Web.UI.WebControls.RadioButton Then
        '                                radiobutton = CType(control, System.Web.UI.WebControls.RadioButton)
        '                                radiobutton.Visible = CBool(rows(0)("Visibility"))
        '                                If radiobutton.Visible Then
        '                                    radiobutton.Text = CStr(rows(0)("Content"))
        '                                Else
        '                                    radiobutton.Text = " "
        '                                End If
        '                            End If
        '                        Case "TR"
        '                            If TypeOf control Is System.Web.UI.HtmlControls.HtmlTableRow Then
        '                                control.Visible = CBool(rows(0)("Visibility"))
        '                                If control.Visible Then
        '                                    TranslateTrLabel(inControl, "LBL_" & UCase(CStr(rows(0)("FieldName"))), CStr(rows(0)("Content")))
        '                                    'TranslateTrLinkButton(inControl, "LB_" & UCase(CStr(rows(0)("FieldName"))), CStr(rows(0)("Content")))
        '                                    'TranslateTrRadioButton(inControl, "RB_" & UCase(CStr(rows(0)("FieldName"))), CStr(rows(0)("Content")))
        '                                End If
        '                            End If
        '                        Case Else

        '                    End Select
        '                End If
        '            End If
        '        Next
        '    End If
        'End Sub
        '---------------------------------------------------------------------------------------------







        'Public Shared Sub TranslateTrLinkButton(ByRef inControl As System.Web.UI.Control, ByVal strFieldName As String, ByVal strContent As String)
        '    Dim innercontrol As System.Web.UI.Control
        '    Dim linkbutton As System.Web.UI.WebControls.LinkButton

        '    If inControl.HasControls Then
        '        For Each innercontrol In inControl.Controls
        '            If TypeOf innercontrol Is System.Web.UI.WebControls.LinkButton Then
        '                If UCase(innercontrol.ID) = strFieldName Then
        '                    linkbutton = CType(innercontrol, System.Web.UI.WebControls.LinkButton)
        '                    linkbutton.Text = strContent
        '                End If
        '            Else
        '                TranslateTrLabel(innercontrol, strFieldName, strContent)
        '            End If
        '        Next
        '    End If
        'End Sub

        'Public Shared Sub TranslateTrRadioButton(ByRef inControl As System.Web.UI.Control, ByVal strFieldName As String, ByVal strContent As String)
        '    Dim innercontrol As System.Web.UI.Control
        '    Dim radiobutton As System.Web.UI.WebControls.RadioButton

        '    If inControl.HasControls Then
        '        For Each innercontrol In inControl.Controls
        '            If TypeOf innercontrol Is System.Web.UI.WebControls.RadioButton Then
        '                If UCase(innercontrol.ID) = strFieldName Then
        '                    radiobutton = CType(innercontrol, System.Web.UI.WebControls.RadioButton)
        '                    radiobutton.Text = strContent
        '                End If
        '            Else
        '                TranslateTrLabel(innercontrol, strFieldName, strContent)
        '            End If
        '        Next
        '    End If
        'End Sub
        Public Shared Function TranslateColLbtn(ByVal inControl As System.Web.UI.Control, ByVal inTranslation As DataTable, ByVal strFieldname As String, ByRef iVisible As Integer) As String
            TranslateColLbtn = ""
            If inControl.HasControls Then
                Dim rows() As DataRow
                Dim control As System.Web.UI.Control
                For Each control In inControl.Controls
                    If control.Visible Then
                        rows = inTranslation.Select("ControlID = '" & strFieldname & "'")
                        If rows.Length = 1 Then
                            If CBool(rows(0)("Visibility")) Then
                                TranslateColLbtn = CStr(rows(0)("Content"))
                                iVisible = 1
                                Exit Function
                            End If

                        End If
                    End If
                Next
            End If

        End Function
        Public Shared Sub TranslateTrLabel(ByRef inControl As System.Web.UI.Control, ByVal strFieldName As String, ByVal strContent As String)
            Dim innercontrol As System.Web.UI.Control
            Dim label As System.Web.UI.WebControls.Label

            If inControl.HasControls Then
                For Each innercontrol In inControl.Controls
                    If TypeOf innercontrol Is System.Web.UI.WebControls.Label Then
                        If UCase(innercontrol.ID) = strFieldName Then
                            label = CType(innercontrol, System.Web.UI.WebControls.Label)
                            label.Text = strContent
                        End If
                    Else
                        TranslateTrLabel(innercontrol, strFieldName, strContent)
                    End If
                Next
            End If
        End Sub

        Public Shared Sub FormAuth(ByVal Form As System.Web.UI.Page, ByVal _User As User, ByVal bCheckHaendlerReferenz As Boolean)
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"

            If Not Form.Session("objUser") Is Nothing Then
                Dim AppName As String = Form.Request.Url.LocalPath
                AppName = Left(AppName, InStrRev(AppName, ".") - 1)
                AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
                AppName = "AppName='" & AppName & "'"
                If Not AppName = "Selection" AndAlso _
                  (Form.Request.UrlReferrer Is Nothing OrElse _
                   _User.Applications.Select(AppName).Length = 0) Then
                    Form.Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
                Else
                    If bCheckHaendlerReferenz Then
                        If _User.Groups(0).IsCustomerGroup Then
                            If _User.Reference.Trim(" "c).Length = 0 Then
                                Form.Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
                            End If
                        End If
                    End If
                    Dim AppURL As String = Replace(Form.Request.Url.LocalPath, "/" & m_strAppKey & "", "..")
                    If Form.Session(AppURL) Is Nothing Then
                        Dim BrowserLanguage As String = Form.Request.ServerVariables("HTTP_ACCEPT_LANGUAGE").Split(","c)(0).Split(";"c)(0)

                        Form.Session(AppURL) = _User.GetTranslations(BrowserLanguage, AppURL)
                    End If
                End If
            Else
                Form.Response.Redirect(strLinkPrefix & "Default.aspx")
            End If
        End Sub

        Public Shared Sub FormAuth(ByVal Form As System.Web.UI.Page, ByVal _User As User)

            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"

            If Not Form.Session("objUser") Is Nothing Then
                Dim AppName As String = Form.Request.Url.LocalPath
                AppName = Left(AppName, InStrRev(AppName, ".") - 1)
                AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
                AppName = "AppName='" & AppName & "'"
                If Not AppName = "Selection" AndAlso _
                  (Form.Request.UrlReferrer Is Nothing OrElse _
                   _User.Applications.Select(AppName).Length = 0) Then
                    Form.Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
                Else
                    Dim AppURL As String = Replace(Form.Request.Url.LocalPath, "/" & m_strAppKey & "", "..")
                    If Form.Session(AppURL) Is Nothing Then
                        Dim BrowserLanguage As String = Form.Request.ServerVariables("HTTP_ACCEPT_LANGUAGE").Split(","c)(0).Split(";"c)(0)

                        Form.Session(AppURL) = _User.GetTranslations(BrowserLanguage, AppURL)
                    End If
                End If
            Else
                Form.Response.Redirect(strLinkPrefix & "Default.aspx")
            End If
        End Sub
        Public Shared Sub FormAuth(ByVal Form As System.Web.UI.Page, ByVal _User As User, ByVal ZLD As String)
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"

            If Not Form.Session("objUser") Is Nothing Then
                Dim AppName As String = Form.Request.Url.LocalPath
                AppName = Left(AppName, InStrRev(AppName, ".") - 1)
                AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
                AppName = "AppName='" & AppName & "'"
                If Not AppName = "Selection" AndAlso _
                  (Form.Request.UrlReferrer Is Nothing OrElse _
                   _User.Applications.Select(AppName).Length = 0) Then
                    Form.Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
                Else
                    Dim AppURL As String = Replace(Form.Request.Url.LocalPath, "/" & m_strAppKey & "", "..")
                    If Form.Session(AppURL) Is Nothing Then
                        Dim BrowserLanguage As String = Form.Request.ServerVariables("HTTP_ACCEPT_LANGUAGE").Split(","c)(0).Split(";"c)(0)

                        Form.Session(AppURL) = _User.GetTranslations(BrowserLanguage, AppURL, ZLD)
                    End If
                End If
            Else
                Form.Response.Redirect(strLinkPrefix & "Default.aspx")
            End If
        End Sub
        Public Shared Sub FormAuthNoReferrer(ByVal Form As System.Web.UI.Page, ByVal _User As User)
            '§§§ JVE 11.04.2006: Sonderfall, nur für SIXT! 
            'Bei dieser Authorisierung wird der Parameter URL-Referrer nicht abgefragt, da dieser
            'bei einem Aufruf über JaveScript leer (null) ist. 
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"

            If Not Form.Session("objUser") Is Nothing Then
                Dim AppName As String = Form.Request.Url.LocalPath
                AppName = Left(AppName, InStrRev(AppName, ".") - 1)
                AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
                AppName = "AppName='" & AppName & "'"

                'UH 02.05.2007: Die Abfrage auf die Selection-Anwendung war falsch => erweitert
                If (Not AppName = "AppName='Selection'") AndAlso (_User.Applications.Select(AppName).Length = 0) Then

                    'SessionId in URL eingesetzt
                    Form.Response.Redirect(strLinkPrefix & "(" & Form.Session.SessionID.ToString & ")/Start/Selection.aspx")
                End If
            Else
                Form.Response.Redirect(strLinkPrefix & "Default.aspx")
            End If
        End Sub

        Public Shared Function getDBConnectionString() As String
            Return ConfigurationManager.AppSettings("Connectionstring")
        End Function

        Public Shared Sub NoDealer(ByVal Form As System.Web.UI.Page, ByVal _User As User)
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"

            If Not _User.Reference.Trim(" "c).Length = 0 Then
                Form.Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
            End If
        End Sub

        Public Shared Sub AdminAuth(ByVal Form As System.Web.UI.Page, ByVal _User As User, ByVal _AdminLevel As AdminLevel)
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"

            If Not Form.Session("objUser") Is Nothing Then
                If (Form.Request.UrlReferrer Is Nothing OrElse _
                    _User.HighestAdminLevel < _AdminLevel) Then
                    Form.Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
                End If
            Else
                Form.Response.Redirect(strLinkPrefix & "Default.aspx")
            End If


            If Not Form.Session("objUser") Is Nothing Then
                Dim AppName As String = Form.Request.Url.LocalPath
                AppName = Left(AppName, InStrRev(AppName, ".") - 1)
                AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
                AppName = "AppName='" & AppName & "'"
                If Not AppName = "Selection" AndAlso _
                  (Form.Request.UrlReferrer Is Nothing) Then
                    Form.Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
                Else
                    Dim AppURL As String = Replace(Form.Request.Url.LocalPath, "/" & m_strAppKey & "", "..")
                    If Form.Session(AppURL) Is Nothing Then
                        Dim BrowserLanguage As String = Form.Request.ServerVariables("HTTP_ACCEPT_LANGUAGE").Split(","c)(0).Split(";"c)(0)
                        Dim tmpDataTable As DataTable = _User.GetTranslations(BrowserLanguage, AppURL)

                        'für den SuperAdmin(also komplette Firma 1 oder andere mit bk=-1) dürfen in der Feldübersetzung keine Felder ausgeblendet werden, hier im admin bereichJJU20081013
                        If _User.HighestAdminLevel = AdminLevel.Master And _User.Customer.AccountingArea = -1 Then
                            For Each tmpRow As DataRow In tmpDataTable.Select("Visibility=0")
                                tmpRow.Item("Visibility") = True
                            Next
                            tmpDataTable.AcceptChanges()
                        End If

                        Form.Session(AppURL) = tmpDataTable
                    End If
                End If
            Else
                Form.Response.Redirect(strLinkPrefix & "Default.aspx")
            End If
        End Sub

        Public Shared Function GetTranslations(ByVal page As Page) As DataTable
            Dim AppURL As String
            Try
                AppURL = HttpContext.Current.Request.Url.LocalPath.Replace("/" & m_strAppKey, "..")
            Catch
                AppURL = ""
            End Try

            If AppURL.Length > 0 Then
                If Not page.Session(AppURL) Is Nothing Then
                    Return CType(page.Session(AppURL), DataTable)
                End If
            End If

            Return Nothing
        End Function

        Public Shared Sub SetEndASPXAccess(ByVal page As Page)
            Dim tblTranslations = GetTranslations(page)
            If Not tblTranslations Is Nothing AndAlso tblTranslations.Rows.Count > 0 Then
                For Each outerControl As System.Web.UI.Control In page.Controls
                    CheckAllControl(outerControl, tblTranslations)
                Next
            End If
        End Sub

        Public Shared Function GetUser(ByVal Form As System.Web.UI.Page) As User
            Dim _user As User
            If Not Form.Session("objUser") Is Nothing Then
                _user = CType(Form.Session("objUser"), User)
                _user.GetCurrentSessionID(_user.UserName)
            Else
                _user = New User(CInt(Form.User.Identity.Name), ConfigurationManager.AppSettings("Connectionstring"))
            End If

            Form.Session("objUser") = _user

            'Benutzerbezogen Setzen des Kundenlogos
            Try
                AddKndLogoToMasterPage(Form, _user.Customer.LogoPath)
            Catch
            End Try

            Return _user
        End Function

        Public Shared Function ListSettingNames(user As User, appID As String, controlID As String) As List(Of String)
            Try
                Dim cnn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim cmd = cnn.CreateCommand
                cmd.CommandText = "select SettingsName from WebUserSetting where UserID=@userID and AppID=@appID and ControlID=@controlID"
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@userID", user.UserID)
                cmd.Parameters.AddWithValue("@appID", appID)
                cmd.Parameters.AddWithValue("@controlID", controlID)

                cnn.Open()
                Dim reader = cmd.ExecuteReader()

                Dim table = New DataTable()
                table.Load(reader)
                cnn.Close()

                Return table.Rows.Cast(Of DataRow).Select(Function(r) CStr(r("SettingsName"))).ToList()
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Shared Function GetSetting(user As User, appID As String, controlID As String, name As String) As String
            Try
                Dim cnn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim cmd = cnn.CreateCommand
                cmd.CommandText = "select Value from WebUserSetting where UserID=@userID and AppID=@appID and ControlID=@controlID and SettingsName=@settingsName"
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@userID", user.UserID)
                cmd.Parameters.AddWithValue("@appID", appID)
                cmd.Parameters.AddWithValue("@controlID", controlID)
                cmd.Parameters.AddWithValue("@settingsName", name)

                cnn.Open()
                Dim reader = cmd.ExecuteReader()

                Dim table = New DataTable()
                table.Load(reader)
                cnn.Close()

                Return table.Rows.Cast(Of DataRow).Select(Function(r) CStr(r("Value"))).FirstOrDefault()
            Catch ex As Exception
                System.Diagnostics.Trace.WriteLine(ex)
                Return Nothing
            End Try
        End Function

        Public Shared Sub SaveSetting(user As User, appID As String, controlID As String, name As String, value As String)
            Try
                Dim cnn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim cmd = cnn.CreateCommand
                cmd.CommandText = "select COUNT(*) from WebUserSetting where UserID=@userID and AppID=@appID and ControlID=@controlID and SettingsName=@settingsName"
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@userID", user.UserID)
                cmd.Parameters.AddWithValue("@appID", appID)
                cmd.Parameters.AddWithValue("@controlID", controlID)
                cmd.Parameters.AddWithValue("@settingsName", name)

                cnn.Open()
                Dim cnt = cmd.ExecuteScalar()

                cmd = cnn.CreateCommand()
                cmd.CommandType = CommandType.Text
                If (cnt = 1) Then
                    cmd.CommandText = "update WebUserSetting set Value=@value where UserID=@userID and AppID=@appID and ControlID=@controlID and SettingsName=@settingsName"
                Else
                    cmd.CommandText = "insert into WebUserSetting (UserID, AppID, ControlID, SettingsName, Value) values (@userID, @appID, @controlID, @settingsName, @value)"
                End If
                cmd.Parameters.AddWithValue("@userID", user.UserID)
                cmd.Parameters.AddWithValue("@appID", appID)
                cmd.Parameters.AddWithValue("@controlID", controlID)
                cmd.Parameters.AddWithValue("@settingsName", name)
                cmd.Parameters.AddWithValue("@value", value)
                cmd.ExecuteNonQuery()
                cnn.Close()
            Catch ex As Exception
                System.Diagnostics.Trace.WriteLine(ex)
            End Try
        End Sub

        Public Shared Sub SaveCustomerSetting(ByVal customerID As Integer, appID As String, name As String, value As String, descript As String)
            Try
                Dim cnn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim cmd = cnn.CreateCommand
                cmd.CommandText = "select COUNT(*) from CustomerSettings where CustomerID=@customerID and AppID=@appID and SettingsName=@settingsName"
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@customerID", customerID)
                cmd.Parameters.AddWithValue("@appID", appID)
                cmd.Parameters.AddWithValue("@settingsName", name)

                cnn.Open()
                Dim cnt = cmd.ExecuteScalar()

                cmd = cnn.CreateCommand()
                cmd.CommandType = CommandType.Text
                If (cnt = 1) Then
                    cmd.CommandText = "update CustomerSettings set Value=@value, Description=@descript where CustomerID=@customerID and AppID=@appID and SettingsName=@settingsName"
                Else
                    cmd.CommandText = "insert into CustomerSettings (CustomerID, AppID, SettingsName, Value, Description) values (@customerID, @appID, @settingsName, @value, @descript)"
                End If
                cmd.Parameters.AddWithValue("@customerID", customerID)
                cmd.Parameters.AddWithValue("@appID", appID)
                cmd.Parameters.AddWithValue("@settingsName", name)
                cmd.Parameters.AddWithValue("@value", value)
                cmd.Parameters.AddWithValue("@descript", descript)
                cmd.ExecuteNonQuery()
                cnn.Close()
            Catch ex As Exception
                System.Diagnostics.Trace.WriteLine(ex)
            End Try
        End Sub

        Public Shared Function GetCustomerSetting(Customer As Customer, appID As String, name As String) As String
            Try
                Dim cnn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim cmd = cnn.CreateCommand
                cmd.CommandText = "select Value from CustomerSettings where CustomerID=@customerID and AppID=@appID and SettingsName=@settingsName"
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@customerID", Customer.CustomerId)
                cmd.Parameters.AddWithValue("@appID", appID)
                cmd.Parameters.AddWithValue("@settingsName", name)
                cnn.Open()

                Dim reader = cmd.ExecuteReader()
                Dim table = New DataTable()

                table.Load(reader)
                cnn.Close()

                Return table.Rows.Cast(Of DataRow).Select(Function(r) CStr(r("Value"))).FirstOrDefault()
            Catch ex As Exception
                System.Diagnostics.Trace.WriteLine(ex)
                Return Nothing
            End Try
        End Function

        Public Shared Sub DeleteCustomerSetting(customerID As Integer, appID As String, name As String)
            Try
                Dim cnn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim cmd = cnn.CreateCommand
                cmd.CommandText = "DELETE FROM CustomerSettings where CustomerID=@customerID and AppID=@appID and SettingsName=@settingsName"
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@customerID", customerID)
                cmd.Parameters.AddWithValue("@appID", appID)
                cmd.Parameters.AddWithValue("@settingsName", name)
                cnn.Open()

                Dim reader = cmd.ExecuteReader()
                cnn.Close()

            Catch ex As Exception
                System.Diagnostics.Trace.WriteLine(ex)
            End Try
        End Sub

        Public Shared Function GetAllCustomerSetting(customerId As Integer, appID As String) As DataTable
            Try
                Dim cnn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim cmd = cnn.CreateCommand
                cmd.CommandText = "select * from CustomerSettings where CustomerID=@customerID and AppID=@appID"
                cmd.CommandType = CommandType.Text
                cmd.Parameters.AddWithValue("@customerID", customerId)
                cmd.Parameters.AddWithValue("@appID", appID)
                cnn.Open()

                Dim reader = cmd.ExecuteReader()
                Dim table = New DataTable()

                table.Load(reader)
                cnn.Close()
                Return table

            Catch ex As Exception
                System.Diagnostics.Trace.WriteLine(ex)
                Return Nothing
            End Try
        End Function

        Public Shared Function BouncePage(ByVal Form As System.Web.UI.Page) As String
            Return "/" & m_strAppKey & "/Start/Bounce.aspx?ReturnURL=" & System.Web.HttpUtility.UrlEncode(Form.Request.RawUrl)
        End Function

        Public Shared Function Proof_SpecialChar(ByVal vText As String) As String
            Dim i, iLen As Integer
            Dim sZeichen As String = ""
            Dim vZeichen As String

            iLen = Len(vText)
            For i = 1 To iLen
                vZeichen = Mid(vText, i, 1)
                If Not IsNumeric(vZeichen) AndAlso Not Asc(vZeichen) = 32 Then
                    If Asc(vZeichen) < 65 Or Asc(vZeichen) > 90 Then
                        If Asc(vZeichen) < 97 Or Asc(vZeichen) > 122 Then
                            sZeichen = vZeichen
                            Exit For
                        End If
                    End If
                End If
            Next
            Return sZeichen
        End Function

        Public Shared Function IsSAPDate(ByRef strInputOutput As String) As Boolean
            Dim strTemp As String
            If Not Len(strInputOutput) = 8 Then
                Return False
            End If
            strTemp = Right(strInputOutput, 2) & "." & Mid(strInputOutput, 5, 2) & "." & Left(strInputOutput, 4)
            If IsDate(strTemp) Then
                strInputOutput = strTemp
                Return True
            Else
                Return False
            End If
        End Function

        Public Function IsDateWithoutPoint(ByRef strInputOutput As String) As Boolean
            Dim strTemp As String
            If Not Len(strInputOutput) = 8 Then
                Return False
            End If
            strTemp = Left(strInputOutput, 2) & "." & Mid(strInputOutput, 3, 2) & "." & Right(strInputOutput, 4)
            If IsDate(strTemp) Then
                strInputOutput = strTemp
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsStandardDate(ByRef strInputOutput As String) As Boolean
            Dim strTemp As String
            If Not Len(strInputOutput) = 8 Then
                Return False
            End If
            strTemp = Left(strInputOutput, 2) & "." & Mid(strInputOutput, 3, 2) & "." & Right(strInputOutput, 4)
            If IsDate(strTemp) Then
                strInputOutput = strTemp
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function GiveAuthorizationDetails(ByVal ConnectionString As String, ByVal ID As Int32, ByVal ObjectName As String) As MemoryStream
            Dim cn As New SqlClient.SqlConnection(ConnectionString)
            Dim cmdOutPut As SqlClient.SqlCommand
            Dim OutPutStream As New MemoryStream()

            Try
                cn = New SqlClient.SqlConnection(ConnectionString)
                cn.Open()

                cmdOutPut = New SqlClient.SqlCommand()
                cmdOutPut.Connection = cn
                cmdOutPut.CommandText = "SELECT ObjectData FROM AuthorizationDetails WHERE AuthorizationID = " & ID.ToString & " AND ObjectName = '" & ObjectName & "'"
                Dim drAppData As SqlClient.SqlDataReader = cmdOutPut.ExecuteReader
                Dim intI As Int32 = 0

                If Not drAppData Is Nothing AndAlso drAppData.Read Then
                    If Not TypeOf drAppData(intI) Is System.DBNull Then
                        ' 1. Daten als bytearray aus der DB lesen:
                        Dim bytData(CInt(drAppData.GetBytes(intI, 0, Nothing, 0, Integer.MaxValue - 1) - 1)) As Byte
                        drAppData.GetBytes(intI, 0, bytData, 0, bytData.Length)
                        ' Dataset über einen Memory Stream aus dem ByteArray erzeugen:
                        Dim stmAppData As MemoryStream
                        stmAppData = New MemoryStream(bytData)
                        OutPutStream = stmAppData
                    Else
                        OutPutStream = Nothing
                    End If
                Else
                    OutPutStream = Nothing
                    cn.Close()
                    Throw New System.Exception("Keine Applikationsdaten für den Logeintrag vorhanden.")
                End If

                Return OutPutStream
            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try
        End Function

        Public Shared Function WriteAuthorization(ByVal ConnectionString As String, ByVal AppID As Int32, ByVal InitializedBy As String, ByVal OrganizationID As Int32, ByVal CustomerReference As String, ByVal ProcessReference As String, ByVal ProcessReference2 As String, ByVal ProcessReference3 As String, ByVal TestUser As Boolean, ByVal ObjectData(,) As Object) As Int32
            Dim cn As New SqlClient.SqlConnection(ConnectionString)
            Dim cmdTable As SqlClient.SqlCommand
            Dim intTemp As Int32

            Try
                cn.Open()

                cmdTable = New SqlClient.SqlCommand("INSERT INTO [Authorization] (AppID, InitializedBy, InitializedWhen, OrganizationID, CustomerReference, ProcessReference,ProcessReference2,ProcessReference3, TestUser) VALUES (@AppID, @InitializedBy, @InitializedWhen, @OrganizationID, @CustomerReference, @ProcessReference,@ProcessReference2,@ProcessReference3, @TestUser);SELECT SCOPE_IDENTITY() AS 'Identity'", cn)
                cmdTable.Parameters.AddWithValue("@AppID", AppID)
                cmdTable.Parameters.AddWithValue("@InitializedBy", InitializedBy)
                cmdTable.Parameters.AddWithValue("@InitializedWhen", Now)
                cmdTable.Parameters.AddWithValue("@OrganizationID", OrganizationID)
                cmdTable.Parameters.AddWithValue("@CustomerReference", CustomerReference)
                cmdTable.Parameters.AddWithValue("@ProcessReference", ProcessReference)
                cmdTable.Parameters.AddWithValue("@ProcessReference2", ProcessReference2)
                cmdTable.Parameters.AddWithValue("@ProcessReference3", ProcessReference3)
                cmdTable.Parameters.AddWithValue("@TestUser", TestUser)
                cmdTable.CommandType = CommandType.Text
                intTemp = CInt(cmdTable.ExecuteScalar)

                Dim para As SqlClient.SqlParameter
                Dim b() As Byte
                Dim i As Int32
                For i = ObjectData.GetLowerBound(0) To ObjectData.GetUpperBound(0) - 1
                    b = CType(ObjectData(i, 0), MemoryStream).ToArray
                    para = New SqlClient.SqlParameter("@ObjectData", SqlDbType.Image, b.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, b)

                    cmdTable = New SqlClient.SqlCommand()
                    cmdTable.Connection = cn
                    cmdTable.CommandText = "INSERT INTO AuthorizationDetails (AuthorizationID, ObjectName, ObjectData) VALUES (@AuthorizationID, @ObjectName, @ObjectData)"
                    cmdTable.Parameters.AddWithValue("@AuthorizationID", intTemp)
                    cmdTable.Parameters.AddWithValue("@ObjectName", CType(ObjectData(i, 1), String))
                    cmdTable.Parameters.Add(para)
                    cmdTable.ExecuteNonQuery()
                Next

                Return intTemp
            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try

        End Function

        Public Shared Sub DeleteAuthorizationEntry(ByVal ConnectionString As String, ByVal AuthorizationID As Int32)
            Dim cn As New SqlClient.SqlConnection(ConnectionString)
            Dim cmdTable As SqlClient.SqlCommand

            Try
                cn.Open()

                cmdTable = New SqlClient.SqlCommand()
                cmdTable.Connection = cn
                cmdTable.CommandText = "DELETE FROM [Authorization] WHERE AuthorizationID = @AuthorizationID"
                cmdTable.Parameters.AddWithValue("@AuthorizationID", AuthorizationID)
                cmdTable.ExecuteNonQuery()
            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try
        End Sub

        Public Function toShortDateString(ByVal dat As String) As String
            Dim result As String
            Try
                result = CType(dat, Date).ToShortDateString
                Return result
            Catch
                Return String.Empty
            End Try
        End Function

        Public Shared Function GetOrCreateObject(Of T)(ByVal key As String, ByVal create As Func(Of T)) As T
            Dim session As HttpSessionState = HttpContext.Current.Session

            If session(key) Is Nothing OrElse Not TypeOf session(key) Is T Then
                Dim result As T = create()
                session(key) = result
                Return result
            End If

            Return CType(session(key), T)
        End Function

        Public Shared Sub TranslateTelerikColumns(ByVal ParamArray grids As RadGrid())
            Dim context As HttpContext = HttpContext.Current
            If context.Request Is Nothing Then Return

            Dim appURL As String = context.Request.Url.LocalPath.Replace("/Services", "..")
            If context.Session(appURL) Is Nothing OrElse Not TypeOf context.Session(appURL) Is DataTable Then Return

            Dim table As DataTable = CType(context.Session(appURL), DataTable)

            Dim nameToTranslation As Dictionary(Of String, String) = table.Rows.Cast(Of DataRow)().Where(Function(r) CBool(r("Visibility"))).ToDictionary(Function(r) CStr(r("ControlID")), Function(r) CStr(r("Content")))

            Dim fieldAccessors As Dictionary(Of Type, PropertyInfo) = New Dictionary(Of Type, PropertyInfo)

            Dim columns As IEnumerable(Of GridEditableColumn) = grids.SelectMany(Function(grid) grid.Columns.OfType(Of GridEditableColumn)())

            For Each col As GridEditableColumn In columns
                Dim colT As Type = col.GetType
                If Not fieldAccessors.ContainsKey(colT) Then
                    fieldAccessors(colT) = colT.GetProperty("DataField")
                End If

                ' TODO: Hide/show non-bound columns?
                If fieldAccessors(colT) Is Nothing Then Continue For

                Dim key As String = CStr(fieldAccessors(colT).GetValue(col, Nothing))

                ' TODO: Hide/show non-bound columns?
                If String.IsNullOrEmpty(key) Then Continue For

                If Not key.StartsWith("col_") Then key = "col_" + key

                Dim translation As String = String.Empty
                If nameToTranslation.TryGetValue(key, translation) Then
                    col.HeaderText = translation
                    col.Visible = True
                Else
                    col.Visible = False
                End If
            Next
        End Sub

        Public Shared Sub ResizeTelerikColumns(ByVal grid As RadGrid, ByVal dt As DataTable, Optional ByVal maxWidth As Integer = 300)
            If dt Is Nothing OrElse grid Is Nothing Then Return

            Dim radGridFontFamily As String = "Verdana"
            Dim radGridFontSize As Integer = 10

            Dim columns As List(Of GridColumn) = grid.Columns.Cast(Of GridColumn).ToList()
            Dim lastVisibleCol As Integer = columns.FindLastIndex(Function(c) c.Display)

            Dim uniq2Width As Dictionary(Of String, Integer) = columns.ToDictionary(Function(c) c.UniqueName, Function(c) GetStringWidth(c.HeaderText, radGridFontFamily, radGridFontSize, False))
            Dim dtColumns As Integer = dt.Columns.Count
            Dim dt2uniq As String() = dt.Columns.Cast(Of DataColumn).Select(Function(c) c.ColumnName).ToArray()

            ' Zeilen durchlaufen
            For Each row As DataRow In dt.Rows
                ' Zellen durchlaufen und Größen ggf. anpassen
                For i As Integer = 0 To dtColumns - 1
                    Try
                        Dim uniq As String = dt2uniq(i)
                        ' Ungemappte (d.h. nicht angezeigte) Spalten überspringen
                        If Not uniq2Width.ContainsKey(uniq) Then Continue For

                        Dim cellText As String = row.ItemArray(i).ToString()

                        Dim width As Integer = Math.Min(maxWidth, GetStringWidth(cellText, radGridFontFamily, radGridFontSize, False))
                        uniq2Width(uniq) = Math.Max(width, uniq2Width(uniq))
                    Catch
                    End Try
                Next
            Next

            Dim sumWidth As Integer = 0
            Dim gridWidth As Integer = CInt(grid.Width.Value)

            ' Spaltenbreiten im Gridview setzen
            For i As Integer = 0 To grid.Columns.Count - 1
                Dim col As GridColumn = grid.Columns(i)

                If (i = lastVisibleCol) Then
                    sumWidth = gridWidth - sumWidth
                    If sumWidth > 0 Then
                        col.HeaderStyle.Width = sumWidth
                    Else
                        col.HeaderStyle.Width = uniq2Width(col.UniqueName)
                    End If
                Else
                    col.HeaderStyle.Width = uniq2Width(col.UniqueName)
                End If

                ' Berechnen der Gesamtbreite der bereits bearbeiteten, sichtbaren Spalten
                If col.Display Then sumWidth += uniq2Width(col.UniqueName)
            Next
        End Sub

        Private Shared Function GetStringWidth(text As String, fontFamily As String, fontSize As Integer, Optional isBold As Boolean = False) As Integer
            Dim bm As Bitmap = New Bitmap(1, 1)
            Dim g As Graphics = Graphics.FromImage(bm)
            Try
                Dim widthOfString As Integer = 0
                Dim ft As Font = IIf(isBold, New Font(fontFamily, fontSize, FontStyle.Bold), New Font(fontFamily, fontSize))

                widthOfString = CInt(Math.Floor(g.MeasureString(text, ft).Width))

                Return widthOfString
            Catch
                Return 0
            Finally
                bm.Dispose()
                g.Dispose()
            End Try
        End Function

        Private Shared Function AddKndLogoToMasterPage(ByRef form As Page, ByVal sLogoPath As String) As Boolean
            Try


                Dim physPath As String = String.Empty

                'Wenn Virtuerller Pfad dann den Physischen setzen
                If sLogoPath.IndexOf("~/") = 0 Then
                    physPath = HttpContext.Current.Server.MapPath(sLogoPath)
                    sLogoPath = VirtualPathUtility.ToAbsolute(sLogoPath)

                End If

                If Not File.Exists(physPath) Then
                    Return False
                End If

                Dim litCrtl As LiteralControl
                Dim imgControlId As String = "imgLogo"
                Dim parentCrtl As Control

                If form.Master Is Nothing Then
                    Return False
                End If


                'Das aktuelle "rechte" Logo ermitteln
                Dim masterImgControl As Control = form.Master.FindControl(imgControlId)
                Dim img As System.Web.UI.WebControls.Image = CType(masterImgControl, System.Web.UI.WebControls.Image)

                Dim imageString As String = "<table cellspacing=""0"" cellpadding=""0"" class=""logoTable""><tr><td align=""left"" class=""ImgKndLogo"">"
                imageString += "<img height=""100%"" src=""" + sLogoPath + """></td><td align=""right"">"
                imageString += "<img src=""" + img.ImageUrl + """></td></tr></table>"

                Dim count = 0

                If Not masterImgControl Is Nothing Then

                    'Übergeordnetes element ermitteln
                    parentCrtl = masterImgControl.Parent

                    Dim nextID As String

                    For Each crtl As Control In parentCrtl.Controls
                        count += 1
                        nextID = parentCrtl.Controls(count).ID
                        'Das Control vor dem Logo Control suchen



                        If nextID Is Nothing Then
                            Continue For
                        End If

                        If count < parentCrtl.Controls.Count And nextID.Equals(imgControlId) Then

                            litCrtl = TryCast(crtl, LiteralControl)

                            If Not litCrtl Is Nothing Then
                                'Den TAG für des Kundenlogo an das ende des Literaltextes anfügen

                                If InStr(litCrtl.Text, "website") > 0 Then
                                    litCrtl.Text += imageString
                                Else
                                    litCrtl.Text = imageString + litCrtl.Text
                                End If

                                'Dem aktuellen Logo eine andere Stylesheetklasse zuordnen
                                img.CssClass = "dadLogoIfIsKndLogo"

                                Exit For

                            Else
                                Return False
                            End If
                        End If

                    Next



                End If

                Return True

            Catch ex As Exception

                Return False

            End Try
        End Function

    End Class
End Namespace
' ************************************************
' $History: Common.vb $
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 22.03.11   Time: 14:50
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 5.10.09    Time: 15:46
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 27.07.09   Time: 9:25
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 5.05.09    Time: 10:10
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 28.04.09   Time: 13:45
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 13.10.08   Time: 13:28
' Updated in $/CKAG/Base/Kernel/Common
' ITA 2315 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 24.06.08   Time: 16:15
' Updated in $/CKAG/Base/Kernel/Common
' ITA 2033 Aufbohren der Feldübersetzung für GridView
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Base/Kernel/Common
' ITA:1865
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 14.05.08   Time: 14:31
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Common
' Migration OR
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Base/Kernel/Common
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 32  *****************
' User: Rudolpho     Date: 11.01.08   Time: 9:22
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA:1604
' 
' *****************  Version 31  *****************
' User: Rudolpho     Date: 9.01.08    Time: 16:42
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA: 1604
' 
' *****************  Version 30  *****************
' User: Uha          Date: 19.12.07   Time: 14:10
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA 1510,1512,1511 Anforderung / Zulassung
' 
' *****************  Version 29  *****************
' User: Jungj        Date: 13.11.07   Time: 10:22
' Updated in $/CKG/Base/Base/Kernel/Common
' 
' *****************  Version 28  *****************
' User: Jungj        Date: 12.11.07   Time: 14:59
' Updated in $/CKG/Base/Base/Kernel/Common
' 
' *****************  Version 27  *****************
' User: Jungj        Date: 9.11.07    Time: 14:58
' Updated in $/CKG/Base/Base/Kernel/Common
' 
' *****************  Version 26  *****************
' User: Uha          Date: 27.09.07   Time: 10:27
' Updated in $/CKG/Base/Base/Kernel/Common
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 25  *****************
' User: Uha          Date: 26.09.07   Time: 13:20
' Updated in $/CKG/Base/Base/Kernel/Common
' Unsichtbare Controls werden nicht mehr übersetzt.
' 
' *****************  Version 24  *****************
' User: Uha          Date: 24.09.07   Time: 17:01
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA 1238: Anlage Floorcheckauftrag - Testversion
' 
' *****************  Version 23  *****************
' User: Uha          Date: 18.09.07   Time: 16:26
' Updated in $/CKG/Base/Base/Kernel/Common
' Feldübersetzungen für Spaltenüberschriften von Datagrids erweitert
' 
' *****************  Version 22  *****************
' User: Uha          Date: 12.09.07   Time: 16:41
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA 1263: Pflege der Feldübersetzungen - Fix 1
' 
' *****************  Version 21  *****************
' User: Uha          Date: 12.09.07   Time: 15:17
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA 1263: Pflege der Feldübersetzungen
' 
' *****************  Version 20  *****************
' User: Uha          Date: 10.09.07   Time: 13:23
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA 1280: Bugfix III; ITA 1263: Testversion Translation
' 
' *****************  Version 19  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 18  *****************
' User: Uha          Date: 8.08.07    Time: 15:10
' Updated in $/CKG/Base/Base/Kernel/Common
' GC.Collect() in Sub SetEndASPXAccess wieder entfernt
' 
' *****************  Version 17  *****************
' User: Uha          Date: 8.08.07    Time: 14:57
' Updated in $/CKG/Base/Base/Kernel/Common
' GC.Collect() in Sub SetEndASPXAccess eingebaut
' 
' *****************  Version 16  *****************
' User: Uha          Date: 7.08.07    Time: 14:23
' Updated in $/CKG/Base/Base/Kernel/Common
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 15  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Kernel/Common
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 14  *****************
' User: Uha          Date: 21.06.07   Time: 15:43
' Updated in $/CKG/Base/Base/Kernel/Common
' Bug in GetUser gefixt
' 
' *****************  Version 13  *****************
' User: Uha          Date: 20.06.07   Time: 16:18
' Updated in $/CKG/Base/Base/Kernel/Common
' Parameter ClearDurationASPX eingebracht
' 
' *****************  Version 12  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Base/Base/Kernel/Common
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 8.06.07    Time: 14:25
' Updated in $/CKG/Base/Base/Kernel/Common
' 
' *****************  Version 10  *****************
' User: Uha          Date: 31.05.07   Time: 11:41
' Updated in $/CKG/Base/Base/Kernel/Common
' ITA 1077 - Login bei bereits aktiver Anmeldung ermöglichen
' 
' *****************  Version 9  *****************
' User: Uha          Date: 23.05.07   Time: 15:02
' Updated in $/CKG/Base/Base/Kernel/Common
' Fehler in Methode GetAppIDFromQueryString beseitigt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.05.07    Time: 15:20
' Updated in $/CKG/Base/Base/Kernel/Common
' Methode FormAuthNoReferrer fehlerbereinigt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 5.03.07    Time: 13:54
' Updated in $/CKG/Base/Base/Kernel/Common
' 
' *****************  Version 6  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Common
' 
' ************************************************
