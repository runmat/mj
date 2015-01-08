Public Partial Class Fahrzeug
    Inherits System.Web.UI.UserControl
    Private objCarports As Zul_Sperr_Entsperr

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Check()
    End Sub
    Private Sub SalesList_DataBind(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.DataBinding
        Dim dgi As DataGridItem = CType(Me.BindingContainer, DataGridItem)
        Dim ds As DataSet = CType(dgi.DataItem, DataSet)

        DG1.DataSource = ds
        DG1.DataMember = "Fahrzeuge"
        DG1.DataBind()

        objCarports = CType(Session("App_Report"), Zul_Sperr_Entsperr)

        DG1.Columns(9).HeaderText = "Datum <br/> " & objCarports.ArtDerZulassung.ToString()

        Select Case objCarports.Task
            Case "Ausgabe"
                DG1.Columns(15).Visible = True       'Ergebnis
            Case "Zulassen"
                DG1.Columns(9).Visible = True       'Bemerkung Anzeige
                DG1.Columns(10).Visible = False      'Bemerkung Eingabe
            Case "Verschieben"
                DG1.Columns(14).Visible = True      'Ziel-PDI
        End Select
    End Sub
    Public Sub Check()
        Dim ctl As TextBox
        Dim txtBox As TextBox
        Dim ibtn As ImageButton
        Dim item As DataGridItem
        Dim chk As CheckBox
        Dim cell As TableCell
        Dim lbl As Label
        Dim neuSperr As String
        Dim neuEntSperr As String
        For Each item In DG1.Items

            ctl = CType(item.Cells(7).FindControl("txtBemerkungDatum"), TextBox)
            txtBox = CType(item.Cells(6).FindControl("txtBemerkung"), TextBox)
            If ctl.Text <> "" Then
                If IsDate(ctl.Text) Then
                    ctl.Text = CDate(ctl.Text).ToShortDateString
                    ctl.Enabled = False
                    ctl = CType(item.Cells(6).FindControl("txtBemerkung"), TextBox)
                    ctl.Enabled = False
                    ctl = CType(item.Cells(6).FindControl("txtBemerkung"), TextBox)
                    ctl.Enabled = False
                    ctl = CType(item.Cells(8).FindControl("txtDatumErstzulassung"), TextBox)
                    ctl.Enabled = False
                    cell = item.Cells(12)
                    neuSperr = cell.Text
                    cell = item.Cells(13)
                    neuEntSperr = cell.Text
                    chk = CType(item.Cells(10).FindControl("chkAuswahl"), CheckBox)
                    If neuSperr <> "1" AndAlso neuSperr <> "1" Then
                        ibtn = CType(item.Cells(7).FindControl("IbtnSperr"), ImageButton)
                        ibtn.Visible = False
                        ibtn = CType(item.Cells(7).FindControl("IbtnEntsperr"), ImageButton)
                        ibtn.Visible = True
                    Else
                        ibtn = CType(item.Cells(7).FindControl("IbtnSperr"), ImageButton)
                        ibtn.Visible = False
                        ibtn = CType(item.Cells(7).FindControl("IbtnEntsperr"), ImageButton)
                        ibtn.Visible = False
                    End If

                End If
            Else

                cell = item.Cells(12)
                neuSperr = cell.Text
                cell = item.Cells(13)
                neuEntSperr = cell.Text
                chk = CType(item.Cells(11).FindControl("chkAuswahl"), CheckBox)
                lbl = CType(item.Cells(4).FindControl("Label3"), Label)

                If neuSperr <> "1" AndAlso neuEntSperr <> "1" Then
                    ibtn = CType(item.Cells(7).FindControl("IbtnSperr"), ImageButton)
                    ibtn.Visible = True
                    ibtn = CType(item.Cells(7).FindControl("IbtnEntsperr"), ImageButton)
                    ibtn.Visible = False
                Else
                    ibtn = CType(item.Cells(7).FindControl("IbtnSperr"), ImageButton)
                    ibtn.Visible = False
                    If neuEntSperr = "1" Then
                        ctl = CType(item.Cells(6).FindControl("txtBemerkung"), TextBox)
                        If IsNothing(objCarports) Then
                            objCarports = CType(Session("App_Report"), Zul_Sperr_Entsperr)
                        End If
                        lbl = CType(item.Cells(4).FindControl("Label3"), Label)
                        For j = 0 To objCarports.CarPort_Data.Tables("Fahrzeuge").Rows.Count - 1
                            If objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Fahrgestellnummer").ToString = lbl.Text AndAlso _
                                chk.Checked Then
                                objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("SperrVermerk") = ctl.Text
                            End If
                        Next
                        Session("App_Report") = objCarports
                    End If

                End If
            End If
            ctl = CType(item.Cells(9).FindControl("txtDatumErstzulassung"), TextBox)

            chk = CType(item.Cells(11).FindControl("chkAuswahl"), CheckBox)
            chk.Attributes.Add("onclick", "CountChecked(this," & ctl.ClientID & ")")
        Next
    End Sub
    Private Sub DG1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DG1.ItemCommand
        Dim ctl As TextBox
        Dim lblFehler As Label
        Dim lbl As Label
        Dim cell As TableCell
        Dim ibtn As ImageButton
        Dim chk As CheckBox
        Dim SperrDat As String
        Dim SperrBem As String
        If e.CommandName = "Sperren" Then
            ctl = CType(e.Item.Cells(7).FindControl("txtBemerkungDatum"), TextBox)
            If ctl.Text <> "" Then
                If IsDate(ctl.Text) Then
                    If CDate(ctl.Text) >= Now Then
                        cell = e.Item.Cells(12)
                        cell.Text = "1"
                        cell = e.Item.Cells(13)
                        cell.Text = ""
                        ctl.Text = CDate(ctl.Text).ToShortDateString
                        SperrDat = ctl.Text
                        ctl = CType(e.Item.Cells(6).FindControl("txtBemerkung"), TextBox)
                        ctl.Enabled = False
                        SperrBem = ctl.Text
                        ibtn = CType(e.Item.Cells(7).FindControl("IbtnSperr"), ImageButton)
                        ibtn.Visible = False
                        ibtn = CType(e.Item.Cells(7).FindControl("IbtnEntsperr"), ImageButton)
                        ibtn.Visible = False
                        ctl = CType(e.Item.Cells(8).FindControl("txtDatumErstzulassung"), TextBox)
                        ctl.Enabled = False
                        ctl.Text = ""
                        If IsNothing(objCarports) Then
                            objCarports = CType(Session("App_Report"), Zul_Sperr_Entsperr)
                        End If
                        lbl = CType(e.Item.Cells(4).FindControl("Label3"), Label)
                        For j = 0 To objCarports.CarPort_Data.Tables("Fahrzeuge").Rows.Count - 1
                            If objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Fahrgestellnummer").ToString = lbl.Text Then
                                objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("neuGesperrt") = "1"
                                objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("SperrVermerk") = SperrBem
                                objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Datum_zur_Sperre") = SperrDat
                                objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("neuEntsperrt") = ""
                            End If
                        Next
                        chk = CType(e.Item.Cells(10).FindControl("chkAuswahl"), CheckBox)
                        chk.Checked = True
                        lblFehler = CType(e.Item.Cells(5).FindControl("lblFehler"), Label)
                        lblFehler.Text = ""
                        lblFehler.Visible = False
                    Else
                        lblFehler = CType(e.Item.Cells(5).FindControl("lblFehler"), Label)
                        lblFehler.Text = "Ungültiges Sperrdatum!"
                        lblFehler.Font.Size = CType("8", FontUnit)
                        lblFehler.Visible = True
                        ctl.Enabled = True
                        ibtn = CType(e.Item.Cells(7).FindControl("IbtnSperr"), ImageButton)
                        ibtn.Visible = True
                        ibtn = CType(e.Item.Cells(7).FindControl("IbtnEntsperr"), ImageButton)
                        ibtn.Visible = False
                        ctl = CType(e.Item.Cells(6).FindControl("txtBemerkung"), TextBox)
                        ctl.Enabled = True
                        ctl = CType(e.Item.Cells(8).FindControl("txtDatumErstzulassung"), TextBox)
                        ctl.Enabled = True
                    End If
                Else
                    lblFehler = CType(e.Item.Cells(7).FindControl("lblFehler"), Label)
                    lblFehler.Text = "Ungültiges Sperrdatum!"
                    lblFehler.Visible = True
                    ctl.Enabled = True
                    ibtn = CType(e.Item.Cells(7).FindControl("IbtnSperr"), ImageButton)
                    ibtn.Visible = True
                    ibtn = CType(e.Item.Cells(7).FindControl("IbtnEntsperr"), ImageButton)
                    ibtn.Visible = False
                    ctl = CType(e.Item.Cells(6).FindControl("txtBemerkung"), TextBox)
                    ctl.Enabled = True
                    ctl = CType(e.Item.Cells(8).FindControl("txtDatumErstzulassung"), TextBox)
                    ctl.Enabled = True
                End If
            Else
                lblFehler = CType(e.Item.Cells(7).FindControl("lblFehler"), Label)
                lblFehler.Text = "Ungültiges Sperrdatum!"
                lblFehler.Visible = True
            End If
        ElseIf e.CommandName = "Entsperren" Then
            cell = e.Item.Cells(13)
            cell.Text = "1"
            cell = e.Item.Cells(12)
            cell.Text = ""
            cell = e.Item.Cells(8)
            ctl = CType(e.Item.Cells(8).FindControl("txtDatumErstzulassung"), TextBox)
            ctl.Enabled = True
            'ibtn = CType(item.Cells(7).FindControl("IbtnSperr"), ImageButton)
            'ibtn.Visible = True
            ibtn = CType(e.Item.Cells(7).FindControl("IbtnEntsperr"), ImageButton)
            ibtn.Visible = False
            ctl = CType(e.Item.Cells(7).FindControl("txtBemerkungDatum"), TextBox)
            ctl.Enabled = True
            ctl.Text = ""
            ctl = CType(e.Item.Cells(6).FindControl("txtBemerkung"), TextBox)
            ctl.Enabled = True
            lbl = CType(e.Item.Cells(4).FindControl("Label3"), Label)
            If IsNothing(objCarports) Then
                objCarports = CType(Session("App_Report"), Zul_Sperr_Entsperr)
            End If
            For j = 0 To objCarports.CarPort_Data.Tables("Fahrzeuge").Rows.Count - 1
                If objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Fahrgestellnummer").ToString = lbl.Text Then
                    objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("neuGesperrt") = ""
                    objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("Datum_zur_Sperre") = ""
                    objCarports.CarPort_Data.Tables("Fahrzeuge").Rows(j)("neuEntsperrt") = "1"
                End If
            Next
            chk = CType(e.Item.Cells(10).FindControl("chkAuswahl"), CheckBox)
            chk.Checked = True
        End If
    End Sub
End Class