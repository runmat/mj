Imports System
Imports KBS.KBS_BASE
Imports Telerik.Web.UI

Public Class AuftraegeOnline
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjOnline As Online

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        If Not Session("mKasse") Is Nothing Then
            mObjKasse = Session("mKasse")
        End If

        lblError.Text = ""
        Literal1.Text = ""
        Title = lblHead.Text

        If Session("ObjOnline") IsNot Nothing Then
            mObjOnline = CType(Session("ObjOnline"), Online)
        Else
            mObjOnline = New Online(mObjKasse.Werk, mObjKasse.Lagerort)
            Session("ObjOnline") = mObjOnline
        End If

        If Not IsPostBack Then
            Session("LastPage") = Me

            LadeAuftraege()
        End If
    End Sub

    Private Sub LadeAuftraege()
        mObjOnline.LoadAuftraege()
        Session("ObjOnline") = mObjOnline

        If Not mObjOnline.ErrorOccured Then
            FillGrid()
        Else
            lblError.Text = "Fehler beim Laden der Auftragsliste: " & mObjOnline.ErrorMessage
        End If
    End Sub

    Private Sub FillGrid()
        If mObjOnline.Auftraege IsNot Nothing AndAlso mObjOnline.Auftraege.Rows.Count > 0 Then
            rgGrid1.Visible = True
            rgGrid1.Rebind()
            'Setzen der DataSource geschieht durch das NeedDataSource-Event
        Else
            rgGrid1.Visible = False
            lblError.Text = "Keine Daten gefunden"
        End If
    End Sub

    Protected Sub rgGrid1_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles rgGrid1.NeedDataSource
        If mObjOnline.Auftraege IsNot Nothing Then
            rgGrid1.DataSource = mObjOnline.Auftraege.DefaultView
        Else
            rgGrid1.DataSource = Nothing
        End If
    End Sub

    Protected Sub lbAuswahlAlle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAuswahlAlle.Click
        mObjOnline.SetAuswahlAlle()
        Session("ObjOnline") = mObjOnline
        FillGrid()
    End Sub

    Protected Sub rgGrid1_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles rgGrid1.ItemCommand
        If TypeOf e.Item Is GridDataItem Then
            Dim gridRow As GridDataItem = CType(e.Item, GridDataItem)

            Select Case e.CommandName

                Case "showDocument"
                    ShowDokument(gridRow("PRAEG_ID").Text)

            End Select
        End If
    End Sub

    Protected Sub lbAlleDokumente_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAlleDokumente.Click
        ShowDokument()
    End Sub

    Private Sub ShowDokument(Optional ByVal praegId As String = Nothing)
        Dim pdfBytes As Byte() = mObjOnline.GetMergedPdf(praegId)

        If mObjOnline.ErrorOccured OrElse pdfBytes Is Nothing Then
            lblError.Text = "Beim Abrufen des Dokuments ist ein Fehler aufgetreten: " & mObjOnline.ErrorMessage
            Exit Sub
        End If

        ShowPdf(pdfBytes)
    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Session("ObjOnline") = Nothing
        Session("OnlinePdfBytes") = Nothing
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        For Each row As GridDataItem In rgGrid1.Items
            If row("POSNR").Text = "10" Then
                Dim cbx As CheckBox = CType(row.FindControl("chkAuswahl"), CheckBox)
                mObjOnline.SetAuswahlAuftrag(row("PRAEG_ID").Text, cbx.Checked)
            End If
        Next

        Dim pdfBytes As Byte() = mObjOnline.SendAuftraege()

        Session("mObjOnline") = mObjOnline
        FillGrid()

        If mObjOnline.ErrorOccured Then
            lblError.Text = "Es konnte nicht alle Aufträge erfolgreich gespeichert werden: " & mObjOnline.ErrorMessage
        Else
            lblError.Text = "Aufträge erfolgreich gespeichert"
        End If

        If pdfBytes IsNot Nothing Then
            ShowPdf(pdfBytes)
        End If
    End Sub

    Private Sub ShowPdf(ByVal pdfBytes As Byte())
        Session("OnlinePdfBytes") = pdfBytes

        Literal1.Text = "						<script language=""Javascript"">" & Environment.NewLine
        Literal1.Text &= "						  <!-- //" & Environment.NewLine
        Literal1.Text &= "                          window.open(""DownloadFile2.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & Environment.NewLine
        Literal1.Text &= "						  //-->" & Environment.NewLine
        Literal1.Text &= "						</script>" & Environment.NewLine
    End Sub

End Class