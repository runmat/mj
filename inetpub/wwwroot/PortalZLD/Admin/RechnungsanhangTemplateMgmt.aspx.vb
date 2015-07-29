Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class RechnungsanhangTemplateMgmt
    Inherits Page

#Region " Membervariables "

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As CKG.PortalZLD.GridNavigation

#End Region

#Region " Data and Function "

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        If String.IsNullOrEmpty(strSort) Then
            If ViewState("ResultSort") IsNot Nothing Then
                strSort = ViewState("ResultSort").ToString()
            Else
                strSort = "ID"
            End If
        End If

        Result.Visible = True

        Dim dvTemplates As DataView

        If Session("dvTemplates") Is Nothing Then
            Dim dtTemplates As New DataTable
            Dim errMsg As String = Rechnungsdatenanhang.LoadTemplates(m_User.App.Connectionstring, dtTemplates)
            If Not String.IsNullOrEmpty(errMsg) Then lblError.Text = errMsg

            dvTemplates = dtTemplates.DefaultView
            Session("dvTemplates") = dvTemplates
        Else
            dvTemplates = CType(Session("dvTemplates"), DataView)
        End If

        dvTemplates.Sort = strSort
        dgSearchResult.DataSource = dvTemplates
        dgSearchResult.DataBind()
    End Sub

    Private Function FillEdit(ByVal intTemplateId As Integer) As Boolean
        SwitchViewMode(False)

        If Session("dvTemplates") Is Nothing Then Return False

        Dim dvTemplates As DataView = CType(Session("dvTemplates"), DataView)

        Dim rows As DataRow() = dvTemplates.Table.Select("ID = " & intTemplateId)

        If (rows.Length < 1) Then Return False

        Dim row As DataRow = rows(0)
        lblID.Text = row("ID")
        txtBezeichnung.Text = row("Bezeichnung")
        txtDatenAbZeile.Text = row("DatenAbZeile")
        txtSpalteKennzeichen.Text = row("SpalteKennzeichen")
        txtSpalteGebuehren.Text = row("SpalteGebuehren")
        txtSpalteZulassungsdatum.Text = row("SpalteZulassungsdatum")

        Return True
    End Function

    Private Sub ClearEdit()
        lblID.Text = "0"
        txtBezeichnung.Text = ""
        txtDatenAbZeile.Text = "1"
        txtSpalteKennzeichen.Text = "A"
        txtSpalteGebuehren.Text = "B"
        txtSpalteZulassungsdatum.Text = "C"
        'Buttons
        lbtnSave.Visible = True
        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = IIf(blnLock, "LightGray", "White")
        txtBezeichnung.Enabled = Not blnLock
        txtBezeichnung.BackColor = Drawing.Color.FromName(strBackColor)
        txtDatenAbZeile.Enabled = Not blnLock
        txtDatenAbZeile.BackColor = Drawing.Color.FromName(strBackColor)
        txtSpalteKennzeichen.Enabled = Not blnLock
        txtSpalteKennzeichen.BackColor = Drawing.Color.FromName(strBackColor)
        txtSpalteGebuehren.Enabled = Not blnLock
        txtSpalteGebuehren.BackColor = Drawing.Color.FromName(strBackColor)
        txtSpalteZulassungsdatum.Enabled = Not blnLock
        txtSpalteZulassungsdatum.BackColor = Drawing.Color.FromName(strBackColor)
    End Sub

    Private Sub EditEditMode(ByVal intTemplateId As Integer)
        If Not FillEdit(intTemplateId) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen&nbsp;&#187; "
    End Sub

    Private Sub SwitchViewMode(Optional ByVal blnShowGrid As Boolean = True)
        Result.Visible = blnShowGrid
        Input.Visible = Not blnShowGrid
        'Buttons
        lbtnNew.Visible = blnShowGrid
        lbtnSave.Visible = Not blnShowGrid
        lbtnCancel.Visible = Not blnShowGrid
    End Sub

    Private Sub Search(Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        ClearEdit()
        If blnClearCache Then
            Session.Remove("dvTemplates")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        SwitchViewMode()
        If blnRefillDataGrid Then FillGrid()
    End Sub

#End Region

#Region " Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        lblHead.Text = "Pflege Rechnungsanhang-Templates"
        AdminAuth(Me, m_User, AdminLevel.Master)
        GridNavigation1.setGridElment(dgSearchResult)

        Try
            m_App = New App(m_User)

            lblError.Text = ""

            If Not IsPostBack Then
                Search(True, True, True, True)
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString()
        End Try
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancel.Click
        If dgSearchResult.Rows.Count = 0 Then
            Search(True, True)
        Else
            Search(, True)
        End If
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnNew.Click
        SwitchViewMode(False)
        ClearEdit()
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSave.Click
        If String.IsNullOrEmpty(txtBezeichnung.Text) Then
            lblError.Text = "Bezeichnung fehlt!"
            Exit Sub
        End If

        If String.IsNullOrEmpty(txtDatenAbZeile.Text) Then
            lblError.Text = "Zeile des Datenbeginns fehlt!"
            Exit Sub
        End If
        If Not IsNumeric(txtDatenAbZeile.Text) Then
            lblError.Text = "Zeile des Datenbeginns muss numerisch sein!"
            Exit Sub
        End If

        If String.IsNullOrEmpty(txtSpalteKennzeichen.Text) Then
            lblError.Text = "Kennzeichen-Spalte fehlt!"
            Exit Sub
        End If

        If String.IsNullOrEmpty(txtSpalteGebuehren.Text) Then
            lblError.Text = "Gebühren-Spalte fehlt!"
            Exit Sub
        End If

        If String.IsNullOrEmpty(txtSpalteZulassungsdatum.Text) Then
            lblError.Text = "Zulassungsdatum-Spalte fehlt!"
            Exit Sub
        End If

        Dim errMsg As String = Rechnungsdatenanhang.SaveTemplate(m_User.App.Connectionstring, CInt(lblID.Text), txtBezeichnung.Text, CInt(txtDatenAbZeile.Text), txtSpalteKennzeichen.Text.ToUpper(), txtSpalteGebuehren.Text.ToUpper(), txtSpalteZulassungsdatum.Text.ToUpper())
        If Not String.IsNullOrEmpty(errMsg) Then
            lblError.Text = errMsg
        Else
            Search(True, True, , True)
            lblError.Text = "Die Änderungen wurden gespeichert."
        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillGrid()
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid()
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
        Dim index As Integer
        Dim row As GridViewRow

        If e.CommandName = "Edit" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            EditEditMode(dgSearchResult.DataKeys(row.RowIndex).Value)
            dgSearchResult.SelectedIndex = row.RowIndex
        ElseIf e.CommandName = "Del" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = dgSearchResult.Rows(index)
            DeleteTemplate(dgSearchResult.DataKeys(row.RowIndex).Value)
            dgSearchResult.SelectedIndex = row.RowIndex
        End If
    End Sub

    Private Sub DeleteTemplate(ByVal intID As Integer)
        Dim errMsg As String = Rechnungsdatenanhang.DeleteTemplate(m_User.App.Connectionstring, intID)
        If Not String.IsNullOrEmpty(errMsg) Then
            lblError.Text = errMsg
        Else
            Search(True, True, True, True)
            lblError.Text = "Das Archiv wurde gelöscht."
        End If
    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillGrid(strSort)
    End Sub

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub

    Private Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("/PortalZLD/Start/Selection.aspx?AppID=" & Session("AppID").ToString())
    End Sub

#End Region

End Class