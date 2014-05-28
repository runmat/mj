Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report25
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdDetails As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.ImageButton
    Protected WithEvents txtEingangsdatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtEingangsdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnBis As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblResults As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents rowResultate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles


#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_objTable As DataTable

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                Session("ShowLink") = "False"
                rowResultate.Visible = False
                cmdDetails.Visible = False
                Session("ResultTable") = Nothing
                Session("ResultTableZBII") = Nothing
            Else
                If (Session("ResultTableZBII") Is Nothing) Then
                    rowResultate.Visible = False
                    Session("ResultTable") = Nothing
                Else
                    m_objTable = CType(Session("ResultTableZBII"), DataTable)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBis.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        Me.txtEingangsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        Me.txtEingangsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim checkInput As Boolean = True

        Session("lnkExcel") = ""

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New ec_22(m_User, m_App, strFileName)
            Dim datEingangsdatumVon As DateTime
            Dim datEingangsdatumBis As DateTime


            lblError.Text = ""

            checkInput = True


            'Datumsfelder prüfen
            If checkInput Then
                If (txtEingangsdatumVon.Text.Length > 0) Then
                    If Not IsDate(txtEingangsdatumVon.Text) Then
                        checkInput = False
                    Else
                        If (txtEingangsdatumBis.Text.Length = 0) Or (Not IsDate(txtEingangsdatumBis.Text)) Then
                            datEingangsdatumVon = txtEingangsdatumVon.Text
                            datEingangsdatumBis = txtEingangsdatumVon.Text
                        ElseIf (txtEingangsdatumVon.Text > txtEingangsdatumBis.Text) Then
                            checkInput = False
                            lblError.Text = "Eingangsdatum (von) muss kleiner oder gleich Eingangsdatum (bis) sein!<br />"
                        Else
                            datEingangsdatumVon = txtEingangsdatumVon.Text
                            datEingangsdatumBis = txtEingangsdatumBis.Text
                        End If
                    End If
                Else
                    checkInput = False
                    lblError.Text = "Geben Sie ein gültigen Wert für ""Eingangsdatum Von"" an!"
                End If
            End If


            If checkInput Then

                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, datEingangsdatumVon, datEingangsdatumBis, Me)

                If Not m_Report.Status = 0 Then
                    lblError.Text = m_Report.Message
                Else
                    If Not m_Report.Result.Rows.Count = 0 Then

                        lblResults.Text = "Es wurden " & m_Report.Result.Rows.Count.ToString & " ZBII-Eingänge gefunden."
                        cmdDetails.Visible = True

                        Session("ResultTable") = m_Report.Result
                        Session("ResultTableZBII") = m_Report.ResultZBII


                        If m_Report.Result.Rows.Count = 1 Then
                            Session("ShowLink") = "True"
                            Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                        Else
                            Try
                                lnkExcel.Visible = True
                            Catch
                                lnkExcel.Visible = False
                            End Try
                        End If

                    End If

                    m_objTable = m_Report.ResultZBII
                    FillGrid(0)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            rowResultate.Visible = False
            lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
        Else
            rowResultate.Visible = True

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

            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.Visible = True
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                If DataGrid1.CurrentPageIndex = DataGrid1.PageCount - 1 Then
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/arrow_right.gif"" width=""12"" height=""11"">"
                End If

                If DataGrid1.CurrentPageIndex = 0 Then
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                Else
                    DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/arrow_left.gif"" width=""12"" height=""11"">"
                End If
                DataGrid1.DataBind()
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub cmdDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetails.Click
        Session("ShowLink") = "True"
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lnkExcel_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles lnkExcel.Click
        If Not m_objTable Is Nothing Then
            Try
                Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_objTable, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        Else
            lblError.Text = "Es wurden keine Daten für den Excel-Download gefunden."
        End If
    End Sub
End Class
