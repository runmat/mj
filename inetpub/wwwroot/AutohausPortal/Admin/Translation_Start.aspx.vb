Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Admin

Partial Public Class Translation_Start
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.Admin.GridNavigation

#Region " Declarations"
    Private dvApplication As DataView
    Dim TranslationArt As String
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            m_User = GetUser(Me)
            m_App = New App(m_User) 'erzeugt ein App_objekt 
            FormAuth(Me, m_User)
            GridNavigation1.setGridElment(dgSearchResult)

            If Not IsPostBack Then
                If Not Me.Request.UrlReferrer Is Nothing Then
                    If Me.Request.UrlReferrer.ToString.Contains("FieldTranslation") OrElse Me.Request.UrlReferrer.ToString.Contains("ColumnTranslation") Then
                        FillDataGrid()
                    End If
                End If
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            End If
            If rbField.Checked Then
                TranslationArt = "Field"
            ElseIf rbColumn.Checked Then
                TranslationArt = "Column"
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        search()
        FillDataGrid()
    End Sub


    Private Sub FillDataGrid()
        Dim strSort As String = "AppID"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub
    Private Sub FillDataGrid(ByVal strSort As String)

        'If Not mcontext.Cache("myAppListView") Is Nothing Then
        '    dvApplication = CType(mcontext.Cache("myAppListView"), DataView)
        If Not Session("myAppListView") Is Nothing Then
            dvApplication = CType(Session("myAppListView"), DataView)
        Else
            search()
        End If
        dvApplication.Sort = strSort
        If dvApplication.Count > 0 Then
            dgSearchResult.DataSource = dvApplication
            dgSearchResult.DataBind()
            Result.Visible = True
        Else
            lblError.Text = "Keine Daten zur Anzeige gefunden!"
        End If


    End Sub

    Private Sub search()
        If Not m_User.HighestAdminLevel = AdminLevel.None Then

            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try

                Dim dtApplication As ApplicationList

                cn.Open()

                If m_User.FirstLevelAdmin Then
                    dtApplication = New ApplicationList(m_User.GroupID, m_User.Customer.CustomerId, cn)
                    dtApplication.GetAssignedCustomerAdmin()
                End If
                If m_User.IsCustomerAdmin Or m_User.HighestAdminLevel = AdminLevel.Organization Then
                    dtApplication = New ApplicationList(m_User.Customer.CustomerId, cn)
                    dtApplication.GetAssignedCustomerAdmin()
                End If
                If m_User.HighestAdminLevel = AdminLevel.Master AndAlso m_User.Customer.CustomerId = 1 Then
                    dtApplication = New ApplicationList(txtFilterAppName.Text, _
                                                        cn, _
                                                        txtFilterAppFriendlyName.Text)
                End If

                dvApplication = dtApplication.DefaultView
                Session("myAppListView") = dvApplication


            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        Else
            Response.Redirect("../Start/Selection.aspx")
        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
        If TranslationArt = "Field" Then
            Response.Redirect("FieldTranslation.aspx?AppURL=" & e.CommandArgument)
        Else
            Response.Redirect("ColumnTranslation.aspx?AppURL=" & e.CommandArgument)
        End If
    End Sub

    Private Sub dgSearchResult_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgSearchResult.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim addButton As LinkButton = CType(e.Row.Cells(1).Controls(0).FindControl("btnSelect"), LinkButton)
            addButton.CommandArgument = e.Row.RowIndex.ToString()
        End If
    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid(False)
    End Sub

    Protected Sub cmdback_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdback.Click
        Response.Redirect("../Start/Selection.aspx")
    End Sub
End Class