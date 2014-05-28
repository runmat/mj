Option Strict On
Option Explicit On

Public Class Change09
    Inherits Page

    Private _user As CKG.Base.Kernel.Security.User
    Private _app As CKG.Base.Kernel.Security.App

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Me._user = CKG.Base.Kernel.Common.Common.GetUser(Me)

        CKG.Base.Kernel.Common.Common.FormAuth(Me, Me._user)

        Me._app = New CKG.Base.Kernel.Security.App(Me._user)

        CKG.Base.Kernel.Common.Common.GetAppIDFromQueryString(Me)

        Me.GridNavigation1.setGridElment(Me.GridView1)

        Me.lblHead.Text = Me._user.Applications.Select("AppID = '" & DirectCast(Session("AppID"), String) & "'").First().Field(Of String)("AppFriendlyName")
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
        Dim index As Integer = CInt(e.CommandArgument)
        Me.Reactivate(index)
    End Sub

    Protected Sub lbCreate_Click(sender As Object, e As EventArgs)
        Me.DoSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
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

        report.GetVertragsbestand(Session("AppID").ToString(), Session.SessionID, Me, True)

        If report.ResultTable.Rows.Count = 0 Then
            lblError.Text = "Keine Daten vorhanden."
            Return
        End If

        Session("Report") = report.ResultTable

        Me.Fillgrid(0, "")
    End Sub

    Private Sub Fillgrid(intPageIndex As Integer, strSort As String)
        Me.NewSearchUp.Visible = False

        Me.ibHideList.Visible = True
        Me.ibShowList.Visible = False

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
        End If
    End Sub

    Private Sub Reactivate(intItemIndex As Integer)
        Dim report As New Vertragsbestand(Me._user, Me._app, "")
        report.ResultTable = DirectCast(Session("Report"), DataTable)
        report.SetReaktivierung(Session("AppID").ToString(), Session.SessionID, Me, intItemIndex)
        ScriptManager.RegisterStartupScript(Me, GetType(Change09), "show", "$find('mpeReaktiviert').show();", True)
    End Sub
End Class