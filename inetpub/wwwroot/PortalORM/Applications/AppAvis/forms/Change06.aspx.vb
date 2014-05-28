Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change06
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_report As AvisChange06
    Private m_objTable As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            Literal1.Text = ""
            If Not IsPostBack Then
                DoSubmit()
            Else
                If Not Session("App_Report") Is Nothing Then
                    m_report = CType(Session("App_Report"), AvisChange06)
                End If
                If Not (Session("App_ResultTable") Is Nothing) Then
                    m_objTable = CType(Session("App_ResultTable"), DataTable)
                End If
            End If

        Catch ex As Exception


        End Try

    End Sub

    Protected Sub cmdSave0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Down.Click
        txtID.Text = ""
        txtName.Text = ""
        txtOrt.Text = ""
        txtMail1.Text = ""
        txtMail2.Text = ""
        txtMail3.Text = ""
        txtID.Enabled = True
        txtName.Enabled = True
        txtOrt.Enabled = True
        cmdDel.Visible = False
        insertScript()
    End Sub
    Private Sub insertScript()

        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							slidedown('Suche2');" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf

    End Sub
    Private Sub insertScriptHoldDisplay()

        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							document.getElementById('Suche2').style.display='block';" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf

    End Sub

    Protected Sub UP_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles UP.Click
        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "							slideup('Suche2');" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
    End Sub
    Private Sub DoSubmit()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        Try
            m_report = New AvisChange06(m_User, m_App, strFileName)
            m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString)
            If Not m_report.Status = 0 Then
                lblError.Text = m_report.Message
            Else
                Session("lnkExcel") = "/PortalORM/Temp/Excel/" & strFileName
                Session("App_Report") = m_report
                Session("App_ResultTable") = m_report.Result
                Session("ExcelTable") = m_report.Result
                m_objTable = m_report.Result
                FillGrid(0)
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Try


            If m_objTable.Rows.Count = 0 Then
                GridView1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                GridView1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = m_objTable.DefaultView

                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
                    strTempSort = strSort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If strDirection = "asc" Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    ViewState("Sort") = strTempSort
                    ViewState("Direction") = strDirection
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        strTempSort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "asc"
                            ViewState("Direction") = strDirection
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()

                If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                    lblNoData.Text = CStr(Session("ShowOtherString"))
                Else
                    lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
                End If

                lblNoData.Visible = True
                ExcelCell.Visible = True
                If GridView1.PageCount > 1 Then
                    GridView1.PagerStyle.CssClass = "PagerStyle"
                    GridView1.DataBind()
                    'GridView1.PagerStyle.Visible = True
                Else
                    'GridView1.PagerStyle.Visible = False
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Daten konnten nicht geladen werden!"
        End Try
    End Sub
    Private Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            If Not Session("App_Report") Is Nothing Then
                lblError.Text = ""
                m_report = CType(Session("App_Report"), AvisChange06)

                If txtID.Text.Length > 0 Then
                    m_report.AvisID = txtID.Text
                Else
                    lblError.Text = "ID muß gefüllt sein!"
                    insertScriptHoldDisplay()
                    Exit Sub
                End If

                If txtName.Text.Length > 0 Then
                    m_report.Name = txtName.Text
                Else
                    lblError.Text = "Name muß gefüllt sein!"
                    insertScriptHoldDisplay()
                    Exit Sub
                End If
                If txtOrt.Text.Length > 0 Then
                    m_report.Ort = txtOrt.Text
                Else
                    lblError.Text = "Ort muß gefüllt sein!"
                    insertScriptHoldDisplay()
                    Exit Sub
                End If
                If txtMail1.Text.Length > 0 Then
                    m_report.Mail = txtMail1.Text
                Else
                    lblError.Text = "1. E-Mailadresse muß gefüllt sein!"
                    insertScriptHoldDisplay()
                    Exit Sub
                End If
                If txtMail2.Text.Length > 0 Then
                    m_report.Mail = m_report.Mail & ";" & txtMail2.Text

                End If
                If txtMail3.Text.Length > 0 Then
                    m_report.Mail = m_report.Mail & ";" & txtMail3.Text
                End If
                m_report.Loesch = ""
                If lblError.Text.Length = 0 Then

                    m_report.SaveFirmenDat()
                    m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString)
                    If Not m_report.Status = 0 Then
                        lblError.Text = m_report.Message
                    Else
                        Session("App_Report") = m_report
                        Session("App_ResultTable") = m_report.Result
                        Session("ExcelTable") = m_report.Result
                        m_objTable = m_report.Result
                        FillGrid(0)
                    End If
                End If

            End If
        Catch ex As Exception
            lblError.Text = "Daten konnten nicht gespeichert werden!"
        End Try
    End Sub

    Private Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing
        Dim row As GridViewRow = GridView1.Rows(e.NewEditIndex)
        txtID.Text = row.Cells.Item(0).Text
        txtName.Text = HttpUtility.HtmlDecode(row.Cells.Item(1).Text)
        txtOrt.Text = HttpUtility.HtmlDecode(row.Cells.Item(2).Text)
        txtMail1.Text = row.Cells.Item(3).Text
        txtMail2.Text = Replace(row.Cells.Item(4).Text, "&nbsp;", "")
        txtMail3.Text = Replace(row.Cells.Item(5).Text, "&nbsp;", "")
        txtID.Enabled = False
        txtName.Enabled = False
        txtOrt.Enabled = False
        insertScript()
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        If Not (Session("App_ExcelTable") Is Nothing) Then
            Dim m_objExcel As DataTable = CType(Session("ExcelTable"), DataTable)

            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_objExcel, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

    Private Sub cmdDel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDel.Click
        Try
            If Not Session("App_Report") Is Nothing Then
                lblError.Text = ""
                m_report = CType(Session("App_Report"), AvisChange06)

                m_report.AvisID = txtID.Text
                m_report.Loesch = "X"

                If lblError.Text.Length = 0 Then

                    m_report.SaveFirmenDat()
                    m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString)
                    If Not m_report.Status = 0 Then
                        lblError.Text = m_report.Message
                    Else
                        Session("App_Report") = m_report
                        Session("App_ResultTable") = m_report.Result
                        Session("App_ExcelTable") = m_report.Result
                        m_objTable = m_report.Result
                        FillGrid(0)
                    End If
                End If

            End If
        Catch ex As Exception
            lblError.Text = "Daten konnten nicht gespeichert werden!"
        End Try
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Session("App_Report") = Nothing
        Session("App_ResultTable") = Nothing
        Session("App_ExcelTable") = Nothing
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub
End Class