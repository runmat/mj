Imports System
Imports KBS.KBS_BASE

Public Class Zulassungsdienstsuche
    Inherits Page

    Protected WithEvents GridNavigation1 As GridNavigation
    Private objZLDSuche As ZLD_Suche

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        lblError.Text = ""
        Title = lblHead.Text

        GridNavigation1.setGridElment(gvZuldienst)
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Protected Sub gvZuldienst_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvZuldienst.Sorting
        Fillgrid(gvZuldienst.PageIndex, e.SortExpression)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        Fillgrid(pageindex, "")
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        Fillgrid(0, "")
    End Sub

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEmpty.Click
        DoSubmit()
    End Sub

    Protected Sub gvZuldienst_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvZuldienst.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim label As Label = CType(e.Row.FindControl("lblDetail"), Label)
            If label IsNot Nothing Then
                Dim ImgBtn As ImageButton = CType(e.Row.FindControl("ibtnDetail"), ImageButton)
                If ImgBtn IsNot Nothing Then
                    ImgBtn.Attributes.Add("onclick", "openinfo('" & "Zulassungsdienstsuche_2.aspx?ID=" & label.Text & "')")
                End If
            End If
        End If
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Dim excelFactory As DocumentGeneration.ExcelDocumentFactory = New DocumentGeneration.ExcelDocumentFactory()
        Dim filename As String = String.Format("{0:yyyyMMdd_HHmmss_}", DateTime.Now)
        excelFactory.CreateDocumentAndSendAsResponse(filename, CType(Session("ResultTable"), DataTable), Page)
    End Sub

    Private Sub Fillgrid(ByVal intPageIndex As Integer, ByVal strSort As String)
        Dim resTable As DataTable = CType(Session("ResultTable"), DataTable)

        Dim tmpDataView As DataView = resTable.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            gvZuldienst.Visible = False
            Result.Visible = False
            GridNavigation1.Visible = False
            Panel1.Visible = True
            cmdCreate.Visible = True
        Else
            Result.Visible = True
            Panel1.Visible = False
            cmdCreate.Visible = False
            gvZuldienst.Visible = True

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String

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
            End If
            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            gvZuldienst.PageIndex = intTempPageIndex
            gvZuldienst.DataSource = tmpDataView
            gvZuldienst.DataBind()
        End If
    End Sub

    Private Sub DoSubmit()
        lblError.Text = ""
        objZLDSuche = New ZLD_Suche()
        objZLDSuche.Kennzeichen = txtKennzeichen.Text.ToUpper()
        objZLDSuche.Zulassungspartner = txtZulassungspartner.Text
        objZLDSuche.PLZ = txtPLZ.Text

        objZLDSuche.Fill()

        Session("ResultTable") = objZLDSuche.tblResult

        If objZLDSuche.ErrorOccured Then
            lblError.Text = "Fehler: " & objZLDSuche.ErrorMessage
        Else
            Fillgrid(0, "")
        End If
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NewSearch.Click
        Panel1.Visible = Not Panel1.Visible
        cmdCreate.Visible = Not cmdCreate.Visible
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

End Class