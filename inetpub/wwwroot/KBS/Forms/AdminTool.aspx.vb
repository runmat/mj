Imports KBS.KBS_BASE

Partial Public Class AdminTool
    Inherits Page
    Private mObjKasse As Kasse

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Title = lblHead.Text
        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If
        GridNavigation1.setGridElment(GridView1)
        If Not IsPostBack Then
            FillGrid()
        End If

    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(IPtoKassen)

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
        Else
            GridView1.Visible = True
            lblNoData.Visible = False

            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
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

            GridView1.DataSource = tmpDataView

            GridView1.DataBind()
        End If
    End Sub

    Protected Sub lbShow_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim ibtnShow As ImageButton

        ibtnShow = CType(sender, ImageButton)
        If ibtnShow.CommandName = "Show" Then
            Dim tempIP As String
            tempIP = ibtnShow.CommandArgument

            Dim tmpKasse As Kasse
            tmpKasse = GetConnectedKassen(tempIP)

            If Not tmpKasse.Platinen(Me) Is Nothing Or Not tmpKasse.Bestellung(Me) Is Nothing Or _
                Not tmpKasse.Umlagerungen(Me) Is Nothing Or Not tmpKasse.Versicherungen(Me) Is Nothing Or _
                Not tmpKasse.Wareneingangspruefung(Me) Is Nothing Or Not tmpKasse.Zentrallager(Me) Is Nothing Or _
                Not tmpKasse.offBestellung(Me) Is Nothing Or Not tmpKasse.Einzahlungsbelege(Me) Is Nothing Then
                Session("DetailKasse") = tmpKasse
                Response.Redirect("AdminToolDetail.aspx")
            End If
        End If
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView1.RowCommand

    End Sub

End Class