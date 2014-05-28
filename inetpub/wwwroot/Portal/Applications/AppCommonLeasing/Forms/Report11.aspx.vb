Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.DocumentGeneration

Public Class Report11
    Inherits System.Web.UI.Page

    Private m_App As App
    Private m_User As User
    Private referrer As String

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles


    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)

        Try
            m_User = Common.GetUser(Me)
            m_App = New App(m_User)
            ucHeader.InitUser(m_User)
            Common.FormAuth(Me, m_User)
            lblError.Text = ""

            Common.GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            Dim friendlyName = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            lblHead.Text = friendlyName
            ucStyles.TitleText = friendlyName

            If Not IsPostBack Then
                Session("NichtAktiveZulassungen") = New NichtAktiveZulassungen(m_User, m_App)

                If referrer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        referrer = Me.Request.UrlReferrer.ToString
                    Else
                        referrer = ""
                    End If
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
        MyBase.OnPreRender(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(ByVal e As EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub NavigateBack(sender As Object, e As EventArgs)
        Session.Remove("NichtAktiveZulassungen")

        If String.IsNullOrEmpty(referrer) Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(referrer)
        End If
    End Sub

    Protected Sub CreateReport(sender As Object, e As EventArgs)
        Dim naz = DirectCast(Session("NichtAktiveZulassungen"), NichtAktiveZulassungen)

        Try
            naz.Fill()

            If naz.Status <> 0 Then Throw New Exception(naz.Message)

            FillDataGrid()
        Catch ex As Exception
            lblInfo.Text = "Anzahl: 0"
            lblError.Visible = True
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub PageIndexChanged(sender As Object, e As DataGridPageChangedEventArgs)
        DataGrid1.CurrentPageIndex = e.NewPageIndex

        FillDataGrid()
    End Sub

    Protected Sub PageSizeChanged(sender As Object, e As EventArgs)
        Dim size = CInt(pageSize.SelectedValue)

        DataGrid1.CurrentPageIndex = 0
        DataGrid1.PageSize = size

        FillDataGrid()
    End Sub

    Protected Sub SortChanged(sender As Object, e As DataGridSortCommandEventArgs)
        Dim sort = e.SortExpression

        If (sort.CompareTo(ViewState("Sort")) = 0) Then
            ViewState("Sort") = sort & " desc"
        Else
            ViewState("Sort") = sort
        End If

        DataGrid1.CurrentPageIndex = 0

        FillDataGrid()
    End Sub

    Private Sub FillDataGrid()
        Try
            Dim naz = DirectCast(Session("NichtAktiveZulassungen"), NichtAktiveZulassungen)

            Dim sort = CStr(ViewState("Sort"))
            If (String.IsNullOrEmpty(sort)) Then sort = String.Empty

            DataGrid1.DataSource = New DataView(naz.Result, "", sort, DataViewRowState.CurrentRows)
            DataGrid1.DataBind()
            TableKleinerAbstandVorGrid.Visible = True
            lblInfo.Text = "Anzahl: " & naz.Result.Rows.Count
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub Export(sender As Object, e As EventArgs)
        Try
            Dim data = DirectCast(DataGrid1.DataSource, DataView)
            If data Is Nothing Then
                FillDataGrid()
                data = DirectCast(DataGrid1.DataSource, DataView)
            End If
            Dim table = data.ToTable()
            Dim gridColumns = DataGrid1.Columns.Cast(Of DataGridColumn).ToDictionary(Function(c) c.SortExpression, Function(c) c.HeaderText)

            Dim appURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            Dim translations = CType(Me.Session(appURL), DataTable)

            For i = table.Columns.Count - 1 To 0 Step -1
                Dim col = table.Columns(i)
                If Not gridColumns.ContainsKey(col.ColumnName) Then
                    table.Columns.Remove(col)
                Else
                    Dim visibility = 0
                    Dim text = Common.TranslateColLbtn(DataGrid1, translations, gridColumns(col.ColumnName), visibility)
                    If visibility = 0 Then
                        table.Columns.Remove(col)
                    Else
                        col.ColumnName = text
                    End If
                End If
            Next
            table.AcceptChanges()

            Dim excelFactory = New ExcelDocumentFactory()
            Dim friendlyName = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            excelFactory.CreateDocumentAndSendAsResponse(friendlyName & "_" & DateTime.Now.ToString("yyyyMMdd-HHmmss"), table, Me)
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
        End Try
    End Sub
End Class