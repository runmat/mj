Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Namespace Zulassung
    Partial Public Class Change01_4
        Inherits System.Web.UI.Page

        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private objZulassung As Zulassung1

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            FormAuth(Me, m_User, True)
            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Security.App(m_User)
            GridNavigation1.setGridElment(GridView1)

            If IsNothing(Session("objZulassung")) Then Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)

            lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
            lnkFahrzeugauswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString
            lnkAdressen.NavigateUrl = "Change01_3.aspx?AppID=" & Session("AppID").ToString

            objZulassung = CType(Session("objZulassung"), Zulassung1)
            Try
                If Not IsPostBack Then
                    FillGrid(0)
                End If
            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            End Try
        End Sub
        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

            If objZulassung.Fahrzeuge.Rows.Count = 0 Then
                Result.Visible = True
            Else
                Result.Visible = True

                Dim tmpDataView As New DataView()
                tmpDataView = objZulassung.Fahrzeuge.DefaultView
                tmpDataView.RowFilter = "AUSWAHL = '99'"
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

                GridView1.PageIndex = intTempPageIndex

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()
            End If
        End Sub

        Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
            If Not e.CommandName = "Sort" Then
                'Dim sEqnr As String = e.CommandArgument.ToString
                Dim index As Integer = CType(e.CommandArgument, Integer)
                Dim row As GridViewRow = GridView1.Rows(index)
                Dim txtGrid As TextBox
                Dim lblEqunr As Label
                Dim tblRows() As DataRow
                lblEqunr = CType(row.FindControl("lblEqunr"), Label)
                tblRows = objZulassung.Fahrzeuge.Select("EQUNR='" & lblEqunr.Text & "'")
                If e.CommandName = "Save" Then
                    If tblRows.Length = 1 Then
                        txtGrid = CType(row.FindControl("txtWunschkennz1"), TextBox)

                        tblRows(0)("Wunschkennz1") = txtGrid.Text
                        txtGrid.Enabled = False
                        txtGrid = CType(row.FindControl("txtWunschkennz2"), TextBox)
                        tblRows(0)("Wunschkennz2") = txtGrid.Text
                        txtGrid.Enabled = False
                        txtGrid = CType(row.FindControl("txtWunschkennz3"), TextBox)
                        tblRows(0)("Wunschkennz3") = txtGrid.Text
                        txtGrid.Enabled = False
                        txtGrid = CType(row.FindControl("txtResNr"), TextBox)
                        tblRows(0)("ResNr") = txtGrid.Text
                        txtGrid.Enabled = False
                        txtGrid = CType(row.FindControl("txtResName"), TextBox)
                        tblRows(0)("ResName") = txtGrid.Text
                        txtGrid.Enabled = False
                    End If
                    objZulassung.Fahrzeuge.AcceptChanges()
                    Session("objZulassung") = objZulassung
                ElseIf e.CommandName = "Edit" Then
                    If tblRows.Length = 1 Then
                        txtGrid = CType(row.FindControl("txtWunschkennz1"), TextBox)
                        txtGrid.Enabled = True
                        txtGrid = CType(row.FindControl("txtWunschkennz2"), TextBox)
                        txtGrid.Enabled = True
                        txtGrid = CType(row.FindControl("txtWunschkennz3"), TextBox)
                        txtGrid.Enabled = True
                        txtGrid = CType(row.FindControl("txtResNr"), TextBox)
                        txtGrid.Enabled = True
                        txtGrid = CType(row.FindControl("txtResName"), TextBox)
                        txtGrid.Enabled = True
                    End If

                End If


            End If
        End Sub
        Private Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing

        End Sub

        Protected Sub cmdContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdContinue.Click
            Session("objZulassung") = objZulassung
            Response.Redirect("Change01_5.aspx?AppID=" & Session("AppID").ToString)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub
    End Class
End Namespace
