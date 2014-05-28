Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Portal.PageElements

Public Class Report13
    Inherits Page

    Private _user As User
    Private _app As App
    Private _vers As Versandsperre

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Try
            _user = Common.GetUser(Me)
            _app = New Base.Kernel.Security.App(_user) 'erzeugt ein App_objekt 
            ucHeader.InitUser(_user)
            Common.FormAuth(Me, _user)
            lblError.Text = ""

            If Not IsPostBack Then
                If Refferer Is Nothing Then
                    If Not Request.UrlReferrer Is Nothing Then
                        Refferer = Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If

                Common.GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = _user.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

            _vers = TryCast(Session("Versandsperre"), Versandsperre)
            If _vers Is Nothing Then
                _vers = New Versandsperre(_user, _app, CStr(Session("AppID")))
                Session("Versandsperre") = _vers
            End If

            If Not IsPostBack Then

            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(e As EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Property Refferer() As String

    Protected Sub SucheClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim tmp As DateTime

        Dim von = If(DateTime.TryParse(txtDatumVon.Text, tmp), tmp, CType(Nothing, DateTime?))
        Dim bis = If(DateTime.TryParse(txtDatumBis.Text, tmp), tmp, CType(Nothing, DateTime?))

        _vers.GetData(txtFahrgestellnummer.Text, txtKennzeichen.Text, txtVertragsnummer.Text, txtObjektnummer.Text, von, bis)

        If (_vers.Status <> 0) Then
            lblError.Visible = True
            lblError.Text = _vers.Message
            Return
        End If

        LoadData()

        OpenSearch(False)
    End Sub

    Private Sub LoadData()
        Dim sort = CStr(ViewState("Sort"))
        If (String.IsNullOrEmpty(sort)) Then sort = String.Empty

        tempBriefGrid.DataSource = New DataView(_vers.Result, "", sort, DataViewRowState.CurrentRows)
        tempBriefGrid.DataBind()

        lblInfo.Text = "Anzahl: " & _vers.Result.Rows.Count

        cmdEnd.Enabled = _vers.Result.Select("Status='X'").Length > 0
    End Sub

    Protected Sub NavigateBackClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not String.IsNullOrEmpty(Refferer) Then
            Response.Redirect(Refferer)
        Else
            Response.Redirect(String.Format("/{0}/Start/Selection.aspx", ConfigurationManager.AppSettings("WebAppPath")))
        End If
    End Sub

    Protected Sub PageSizeChanged(ByVal sender As Object, ByVal e As EventArgs)
        tempBriefGrid.CurrentPageIndex = 0
        tempBriefGrid.PageSize = CInt(pageSize.SelectedValue)
        LoadData()
    End Sub

    Protected Sub SortChanged(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs)
        Dim sort = e.SortExpression

        If (sort.CompareTo(ViewState("Sort")) = 0) Then
            ViewState("Sort") = sort & " desc"
        Else
            ViewState("Sort") = sort
        End If

        tempBriefGrid.CurrentPageIndex = 0
        LoadData()
    End Sub

    Protected Sub PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs)
        tempBriefGrid.CurrentPageIndex = e.NewPageIndex
        LoadData()
    End Sub

    Protected Sub ToggleBrief(ByVal sender As Object, ByVal e As EventArgs)
        Dim box = CType(sender, CheckBox)
        Dim item = CType(box.NamingContainer, DataGridItem)

        Dim lblEQUNR = TryCast(item.FindControl("EQUNR"), Label)
        Dim lblCHASSIS_NUM = TryCast(item.FindControl("CHASSIS_NUM"), Label)

        If lblEQUNR Is Nothing OrElse lblCHASSIS_NUM Is Nothing Then Return

        Dim row = _vers.Result.Select(String.Format("EQUNR='{0}' and CHASSIS_NUM='{1}'", lblEQUNR.Text, lblCHASSIS_NUM.Text)).FirstOrDefault()
        If Not row Is Nothing Then row("Status") = If(box.Checked, "X", "-")

        cmdEnd.Enabled = _vers.Result.Select("Status='X'").Length > 0

        LoadData()
    End Sub

    Protected Sub Entsperren(ByVal sender As Object, ByVal e As EventArgs)
        _vers.Entsperren()
        If _vers.Status <> 0 Then
            lblError.Visible = True
            lblError.Text = _vers.Message
        End If

        cmdEnd.Enabled = _vers.Result.Select("Status='X'").Length > 0

        LoadData()
    End Sub

    Protected Sub OpenSucheClick(ByVal sender As Object, ByVal e As EventArgs)
        OpenSearch(True)
    End Sub

    Private Sub OpenSearch(open As Boolean)
        cmdNewSearch.Visible = Not open
        cmdEnd.Visible = Not open
        tblResult.Visible = Not open

        cmdSearch.Visible = open
        Table1.Visible = open
    End Sub
End Class