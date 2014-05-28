
Partial Public Class AdminToolDetail
    Inherits Page
    Private mObjKasse As Kasse
    Private mTempObjKasse As Kasse

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Title = lblHead.Text
        Try

            If mObjKasse Is Nothing Then
                If Not Session("mKasse") Is Nothing Then
                    mObjKasse = CType(Session("mKasse"), Kasse)
                Else
                    Throw New Exception("benötigtes Session Objekt nicht vorhanden")
                End If
            End If
            If mTempObjKasse Is Nothing Then
                If Not Session("DetailKasse") Is Nothing Then
                    mTempObjKasse = CType(Session("DetailKasse"), Kasse)
                Else
                    Throw New Exception("benötigtes Detailobjekt nicht vorhanden")
                End If
            End If

            GridNavigation1.setGridElment(GridView1)
            If Not IsPostBack Then
                FillRbList()

            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub FillRbList()

        If Not mTempObjKasse Is Nothing Then

            With mTempObjKasse
                If Not .Platinen(Me) Is Nothing Then
                    rbObjekte.Items.Add("Platinen")
                Else
                    rbObjekte.Items.Add("Platinen nicht gefüllt")

                End If
                If Not .Bestellung(Me) Is Nothing Then
                    rbObjekte.Items.Add("EAN/Handelsware")
                Else
                    rbObjekte.Items.Add("EAN/Handelsware nicht gefüllt")

                End If
                If Not .Wareneingangspruefung(Me) Is Nothing Then
                    rbObjekte.Items.Add("Wareneingangspruefung")
                Else
                    rbObjekte.Items.Add("Wareneingangs nicht gefüllt")

                End If
                If Not .Zentrallager(Me) Is Nothing Then
                    rbObjekte.Items.Add("Zentrallager")
                Else
                    rbObjekte.Items.Add("Zentrallager nicht gefüllt")

                End If
                If Not .Versicherungen(Me) Is Nothing Then
                    rbObjekte.Items.Add("Versicherungen")
                Else
                    rbObjekte.Items.Add("Versicherungen nicht gefüllt")

                End If
                If Not .Umlagerungen(Me) Is Nothing Then
                    rbObjekte.Items.Add("Umlagerung")
                Else
                    rbObjekte.Items.Add("Umlagerung nicht gefüllt")

                End If

            End With

        End If

    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView
        Select Case rbObjekte.SelectedValue
            Case "Platinen"
                tmpDataView = New DataView(mTempObjKasse.Platinen(Me).Bestellungen)
            Case "Versicherungen"
                tmpDataView = New DataView(mTempObjKasse.Versicherungen(Me).Bestellungen)
        End Select

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

    Protected Sub rbObjekte_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbObjekte.SelectedIndexChanged
        FillGrid()
    End Sub

End Class