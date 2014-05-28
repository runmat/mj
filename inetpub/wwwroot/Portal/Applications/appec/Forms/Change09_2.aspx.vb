Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change09_2
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objMeldungen As Unfallmeldung

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lnkKreditlimit.NavigateUrl = "Change09.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("App_UnfallMeldungen") Is Nothing Then
                Response.Redirect("Change09.aspx?AppID=" & Session("AppID").ToString)
            End If

            objMeldungen = CType(Session("App_UnfallMeldungen"), Unfallmeldung)

            If Not IsPostBack Then
                If Not Session("lnkExcel").ToString.Length = 0 Then
                    lblDownloadTip.Visible = True
                    lnkExcel.Visible = True
                    lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                End If
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.SelectedIndex = 2
                FillGrid(0)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objMeldungen.UnfallMeldungen.DefaultView

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lblError.Visible = True
        Else
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

        End If
    End Sub

    Private Function CheckGrid() As Int32
        Dim item As DataGridItem
        Dim cellCheckbox As TableCell
        Dim cellTextBox As TableCell
        Dim chbox As CheckBox
        Dim txtbox As TextBox
        Dim blnScriptFound As Boolean = False
        Dim intZaehl As Int32
        Dim intReturn As Int32 = 0

        Try
            For Each item In DataGrid1.Items
                intZaehl = 0
                Dim strUnfallnummer As String = "Unfallnummer='" & item.Cells(0).Text & "'"

                cellCheckbox = item.Cells(1)
                chbox = CType(cellCheckbox.FindControl("cbxStorno"), CheckBox)
                objMeldungen.UnfallMeldungen.AcceptChanges()
                Dim tmpRows As DataRow()
                tmpRows = objMeldungen.UnfallMeldungen.Select(strUnfallnummer)
                If tmpRows.Length > 0 Then
                    tmpRows(0).BeginEdit()
                    If chbox.Checked Then
                        tmpRows(0).Item("Selected") = "99"
                        cellTextBox = item.Cells(2)
                        txtbox = CType(cellCheckbox.FindControl("txtStorno"), TextBox)
                        tmpRows(0).Item("Stornotext") = txtbox.Text
                        'Stornotext
                    End If
                    tmpRows(0).EndEdit()
                Else
                    Throw New Exception("Unfallnummer nicht gefunden!")
                End If
                objMeldungen.UnfallMeldungen.AcceptChanges()
                intZaehl += 1
            Next
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
        Session("App_UnfallMeldungen") = objMeldungen
        Return intReturn
    End Function

    Protected Sub cmdConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdConfirm.Click
        CheckGrid()
        Dim tmpRows() As DataRow

        tmpRows = objMeldungen.UnfallMeldungen.Select("Selected = '99'")
        If tmpRows.Length > 0 Then
            For Each saprow As DataRow In tmpRows
                objMeldungen.Unfallnummer = saprow("Unfallnummer").ToString
                objMeldungen.StornoBem = saprow("Stornotext").ToString
                objMeldungen.SetStornoMeldung(Session("AppID").ToString, Session.SessionID.ToString, Me)
                If objMeldungen.Status <> 0 Then
                    lblError.Text = "Es konnten nicht alle ausgewählten Unfallmeldungen storniert werden"
                    saprow("Status") = "F"
                Else
                    saprow("Status") = "S"
                    lblNoData.Text = "Die ausgewählten Unfallmeldungen wurden erfolgreich storniert!"
                End If
            Next
            FillGrid(0)
        Else
            lblError.Text = "Keine Meldungen gewählt!"
        End If
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim chbox As CheckBox
        Dim txtbox As TextBox
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            chbox = CType(e.Item.Cells(1).FindControl("cbxStorno"), CheckBox)
            txtbox = CType(e.Item.Cells(2).FindControl("txtStorno"), TextBox)

            chbox.Attributes.Add("onclick", "javascript:enableTextbox('" & chbox.ClientID & "', '" & txtbox.ClientID & "')")
            txtbox.Attributes.Add("disabled", "disabled")
        End If

    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("Change09.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class