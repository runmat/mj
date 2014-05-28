Option Strict On
Option Explicit On

Public Class Report10
    Inherits Page

    Private _user As CKG.Base.Kernel.Security.User
    Private _app As CKG.Base.Kernel.Security.App

    Protected ReadOnly Property Report02AppID As String
        Get
            Return CStr(_user.Applications.Select("AppName = 'Report02'").First().Field(Of Integer)("AppID"))
        End Get
    End Property

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Me._user = CKG.Base.Kernel.Common.Common.GetUser(Me)

        CKG.Base.Kernel.Common.Common.FormAuth(Me, Me._user)

        Me._app = New CKG.Base.Kernel.Security.App(Me._user)

        CKG.Base.Kernel.Common.Common.GetAppIDFromQueryString(Me)

        Me.GridNavigation1.setGridElment(Me.GridView1)

        Me.lblHead.Text = Me._user.Applications.Select("AppID = '" & DirectCast(Session("AppID"), String) & "'").First().Field(Of String)("AppFriendlyName")

        If Not IsPostBack Then
            FillVertragsarten()
        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        CKG.Base.Kernel.Common.Common.SetEndASPXAccess(Me)
        CKG.Base.Business.HelpProcedures.FixedGridViewCols(Me.GridView1)
    End Sub

    Protected Overrides Sub OnUnload(e As EventArgs)
        MyBase.OnUnload(e)

        CKG.Base.Kernel.Common.Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub GridView1_PageIndexChanged(pageindex As Integer)
        Me.Fillgrid(pageindex, "")
    End Sub

    Protected Sub GridView1_ddlPageSizeChanged()
        Me.Fillgrid(0, "")
    End Sub

    Protected Sub GridView1_Sorting(sender As Object, e As GridViewSortEventArgs)
        Me.Fillgrid(Me.GridView1.PageIndex, e.SortExpression)
    End Sub

    Protected Sub GridView1_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If Not e.CommandName = "Sort" Then
            Dim index As Integer = CInt(e.CommandArgument)
            Me.Filldetails(index)
        End If
    End Sub

    Protected Sub lbCreate_Click(sender As Object, e As EventArgs)
        Me.DoSubmit()
    End Sub

    Protected Sub lbtnBack_Click(sender As Object, e As EventArgs)
        Me.GridView1.Visible = True
        Me.GridNavigation1.Visible = True
        Me.lbtnBack.Visible = False
    End Sub

    Protected Sub NewSearch_Click(sender As Object, e As ImageClickEventArgs)
        Me.NewSearch.Visible = False
        Me.NewSearchUp.Visible = True
        Me.lbCreate.Visible = True
        Me.tab1.Visible = True
        Me.Queryfooter.Visible = True

        If Me.GridView1.Visible Then
            Me.Fillgrid(Me.GridView1.PageIndex, "")
        End If
    End Sub

    Protected Sub NewSearchUp_Click(sender As Object, e As ImageClickEventArgs)
        Me.NewSearch.Visible = True
        Me.NewSearchUp.Visible = False
        Me.lbCreate.Visible = False
        Me.tab1.Visible = False
        Me.Queryfooter.Visible = False

        If Me.GridView1.Visible Then
            Me.Fillgrid(Me.GridView1.PageIndex, "")
        End If
    End Sub

    Protected Sub HideListClick(sender As Object, e As EventArgs)
        Me.ibHideList.Visible = False
        Me.ibShowList.Visible = True

        Me.GridNavigation1.Visible = False
        Me.GridView1.Visible = False
    End Sub

    Protected Sub ShowListClick(sender As Object, e As EventArgs)
        Me.ibHideList.Visible = True
        Me.ibShowList.Visible = False

        Me.GridNavigation1.Visible = True
        Me.GridView1.Visible = True
    End Sub

    Private Sub DoSubmit()
        Dim report As New Vertragsbestand(Me._user, Me._app, "")

        report.Kontonummer = txtKontonummer.Text
        report.CIN = txtCIN.Text
        report.PaidNummer = txtPaidNummer.Text
        report.Name = txtName.Text
        report.Vertragsart = ddlVertragsart.SelectedValue

        report.GetVertragsbestand(Session("AppID").ToString(), Session.SessionID, Me)

        If report.ResultTable.Rows.Count = 0 Then
            lblError.Text = "Keine Daten vorhanden."
            Return
        End If

        Session("Report") = report.ResultTable

        Me.Filldetails(-1)
        Me.Fillgrid(0, "")
    End Sub

    Private Sub FillVertragsarten()

        ddlVertragsart.Items.Clear()

        If _user.Organization IsNot Nothing AndAlso Not String.IsNullOrEmpty(_user.Organization.OrganizationName) Then
            Dim vArten As String() = _user.Organization.OrganizationName.Split("+"c)
            Array.Sort(vArten)
            For Each vArt As String In vArten
                ddlVertragsart.Items.Add(vArt.Trim)
            Next
        End If

    End Sub

    Private Sub Fillgrid(intPageIndex As Integer, strSort As String)
        Me.NewSearchUp.Visible = False

        Me.ShowListClick(Nothing, EventArgs.Empty)

        Dim tmpDataView As DataView = DirectCast(Session("Report"), DataTable).DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            Result.Visible = False
        Else
            Result.Visible = True

            If hField.Value = "0" Then
                lblNoData.Visible = False
                lbCreate.Visible = False
                tab1.Visible = False
                Queryfooter.Visible = False
            End If

            hField.Value = "1"

            If Not tab1.Visible Then

                NewSearch.Visible = True
                NewSearchUp.Visible = False
            Else
                NewSearch.Visible = False
                NewSearchUp.Visible = True
            End If

            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Dim intTempPageIndex As Integer = intPageIndex

            If strSort.Trim().Length > 0 Then
                strTempSort = strSort.Trim()

                If (ViewState("Sort") Is Nothing) OrElse (DirectCast(ViewState("Sort"), String) = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = DirectCast(ViewState("Direction"), String)
                    End If
                Else
                    strDirection = "desc"
                End If

                If (strDirection = "asc") Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If ViewState("Sort") IsNot Nothing Then
                    strTempSort = DirectCast(ViewState("Sort"), String)

                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = DirectCast(ViewState("Direction"), String)
                    End If
                End If
            End If

            If strTempSort.Length <> 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            GridView1.PageIndex = intTempPageIndex
            GridView1.DataSource = tmpDataView
            GridView1.DataBind()

            If tmpDataView.Count = 1 Then
                Me.Filldetails(0)
            End If
        End If
    End Sub

    Private Sub Filldetails(intItemIndex As Integer)
        Me.HideListClick(Nothing, EventArgs.Empty)

        If intItemIndex < 0 Then
            Me.FormView1.DataSource = Nothing
        Else
            Dim report As New Vertragsbestand(Me._user, Me._app, "")
            report.ResultTable = DirectCast(Session("Report"), DataTable)
            report.GetWertverlauf(Session("AppID").ToString(), Session.SessionID, Me, intItemIndex)
            report.GetMahnstufen(Session("AppID").ToString(), Session.SessionID, Me, intItemIndex)

            Dim tmpDataView As DataView = report.ResultTable.DefaultView
            Me.FormView1.DataSource = tmpDataView
            Me.FormView1.PageIndex = intItemIndex
        End If

        Me.FormView1.DataBind()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class