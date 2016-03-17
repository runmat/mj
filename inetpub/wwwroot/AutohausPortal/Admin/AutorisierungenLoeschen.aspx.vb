Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class AutorisierungenLoeschen
    Inherits System.Web.UI.Page

#Region " Membervariables "

    Private m_User As User
    Private m_App As App

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        lblHead.Text = "Autorisierungen löschen"
        FormAuth(Me, m_User)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                FillForm()
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
            lblError.Visible = True
            m_App.WriteErrorText(1, m_User.UserName, "AutorisierungenLoeschen", "PageLoad", lblError.Text)
        End Try
    End Sub

#Region " Data and Function "

    Private Sub FillForm()
        trEditUser.Visible = False
        trSearchResult.Visible = False
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Search(True, True, True, True)
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "InitializedWhen"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        trSearchResult.Visible = True
        Dim dvZV As DataView

        'If Not m_context.Cache("myAutorisierungenLoeschenListView") Is Nothing Then
        '    dvZV = CType(m_context.Cache("myAutorisierungenLoeschenListView"), DataView)
        If Not Session("myAutorisierungenLoeschenListView") Is Nothing Then
            dvZV = CType(Session("myAutorisierungenLoeschenListView"), DataView)
        Else
            Dim dtAutorisierungenLoeschenList As New DataTable()
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Try
                cn.Open()

                Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                          "FROM vwAuthorizationToDelete", cn)
                daApp.Fill(dtAutorisierungenLoeschenList)

                dvZV = dtAutorisierungenLoeschenList.DefaultView
                'm_context.Cache.Insert("myAutorisierungenLoeschenListView", dvZV, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Session("myAutorisierungenLoeschenListView") = dvZV
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End If
        dvZV.Sort = strSort
        If dvZV.Count > dgSearchResult.PageSize Then
            dgSearchResult.PagerSettings.Visible = True
        Else
            dgSearchResult.PagerSettings.Visible = False
        End If

        With dgSearchResult
            .DataSource = dvZV
            .DataBind()
        End With

        Dim str As String
        If dvZV.Count = 1 Then
            str = "Es wurde 1 Eintrag gefunden."
        Else
            str = String.Format("Es wurden {0} Einträge gefunden.", dvZV.Count)
        End If
        lblMessage.Text = str
    End Sub

    Private Function FillEdit(ByVal intZVId As Integer) As Boolean
        SearchMode(False)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim dtAutorisierungenLoeschenList As New DataTable()
            Dim daApp As New SqlClient.SqlDataAdapter("SELECT * " & _
                                                      "FROM vwAuthorizationToDelete WHERE AuthorizationID = " & intZVId.ToString, cn)
            daApp.Fill(dtAutorisierungenLoeschenList)

            txtAuthorizationID.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("AuthorizationID"))
            txtAppFriendlyName.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("AppFriendlyName"))
            txtInitializedBy.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("InitializedBy"))
            txtInitializedWhen.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("InitializedWhen"))
            txtOrganizationName.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("OrganizationName"))
            txtCustomerReference.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("CustomerReference"))
            txtProcessReference.Text = CStr(dtAutorisierungenLoeschenList.Rows(0)("ProcessReference"))
            Return True
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        txtAppFriendlyName.Enabled = Not blnLock
        txtAppFriendlyName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtAppFriendlyName.Enabled = Not blnLock
        txtAppFriendlyName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtInitializedBy.Enabled = Not blnLock
        txtInitializedBy.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtInitializedWhen.Enabled = Not blnLock
        txtInitializedWhen.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtOrganizationName.Enabled = Not blnLock
        txtOrganizationName.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtCustomerReference.Enabled = Not blnLock
        txtCustomerReference.BackColor = System.Drawing.Color.FromName(strBackColor)
        txtProcessReference.Enabled = Not blnLock
        txtProcessReference.BackColor = System.Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub EditEditMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            LockEdit(True)
        End If
        lbtnCancel.Text = "Verwerfen&nbsp;&#187;"
    End Sub

    Private Sub EditDeleteMode(ByVal intGroupId As Integer)
        If Not FillEdit(intGroupId) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Autorisierung wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen&nbsp;&#187;"
        lbtnDelete.Visible = True
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        trEditUser.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        If blnClearCache Then
            'm_context.Cache.Remove("myAutorisierungenLoeschenListView")
            Session.Remove("myAutorisierungenLoeschenListView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        SearchMode()
        If blnRefillDataGrid Then FillDataGrid()
    End Sub

#End Region

#Region " Events "

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        Search(, True)
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()

            Dim strDeleteSQL As String = "DELETE " & _
                                          "FROM [Authorization] " & _
                                          "WHERE AuthorizationID=@AuthorizationID"

            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn
            cmd.Parameters.AddWithValue("@AuthorizationID", CInt(txtAuthorizationID.Text))

            cmd.CommandText = strDeleteSQL
            cmd.ExecuteNonQuery()

            Search(True, True, True, True)
            lblMessage.Text = "Die Autorisierung wurde gelöscht."
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AutorisierungenLoeschen", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
            If Not ex.InnerException Is Nothing Then
                lblError.Text &= ": " & ex.InnerException.Message
            End If
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub dgSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgSearchResult.PageIndexChanging
        dgSearchResult.PageIndex = e.NewPageIndex
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
        Dim index As Integer
        Dim row As GridViewRow
        Dim CtrlLabel As Label


        If e.CommandName = "Edit" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblAuthID")
            EditEditMode(CInt(CtrlLabel.Text))
            dgSearchResult.SelectedIndex = row.RowIndex
        ElseIf e.CommandName = "Del" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            CtrlLabel = row.Cells(0).FindControl("lblAuthID")
            EditDeleteMode(CInt(CtrlLabel.Text))
            dgSearchResult.SelectedIndex = row.RowIndex
        End If
    End Sub

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

#End Region

End Class