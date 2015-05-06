Imports KBS.KBS_BASE
Partial Public Class Change02_1
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjWareneingangspruefung As Wareneingangspruefung

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)
        lblError.Text = ""
        lblNoData.Text = ""
        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        If mObjWareneingangspruefung Is Nothing Then
            mObjWareneingangspruefung = mObjKasse.Wareneingangspruefung(Me)
        End If

        If Not IsPostBack Then

            mObjWareneingangspruefung.currentApplikationPage = Me

            lblBestellnummerLieferant.Text = mObjWareneingangspruefung.LiefantAnzeige
            FillGrid(0)

        End If

        If mObjWareneingangspruefung.IstUmlagerung = "X" Then
            TrLiefernr.Visible = False
            GridView1.Columns(8).Visible = False
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        '----------------------------------------------------------------------
        'Methode:      FillGrid
        'Autor:        Julian Jung
        'Beschreibung: Grid mit den Positionen zu einer Bestellung
        'Erstellt am:  05.05.2009
        '----------------------------------------------------------------------

        Dim tmpDataView As New DataView(mObjWareneingangspruefung.Bestellpositionen)

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
        Else
            GridView1.Visible = True
            lblNoData.Visible = False

            Dim intTempPageIndex As Int32 = intPageIndex
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

            GridView1.PageIndex = intTempPageIndex
            GridView1.DataSource = tmpDataView

            GridView1.DataBind()

            Dim row As GridViewRow = GridView1.HeaderRow

            Dim ImgButton As Image
            ImgButton = CType(row.FindControl("imgbAllVollstaendig"), Image)
            ImgButton.Attributes.Add("onclick", "javascript:SelectRbandChk('" & GridView1.Rows.Count & "', true)")
            ImgButton = CType(row.FindControl("imgbAlleUnvollstaendig"), Image)
            ImgButton.Attributes.Add("onclick", "javascript:SelectRbandChk('" & GridView1.Rows.Count & "', false)")

            If mObjKasse.KUNNR = "261030" Then
                GridView1.Columns(5).Visible = False
            End If
        End If
    End Sub

    Private Sub responseBack()
        mObjKasse.Wareneingangspruefung(Me) = Nothing
        Response.Redirect("Change02.aspx")

    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Public Sub chk_Vollstandig_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim tmpBool As Boolean
        Dim tmpGridRow As GridViewRow = CType(CType(sender, CheckBox).Parent.Parent, GridViewRow)
        If CType(sender, CheckBox).Checked Then
            mObjWareneingangspruefung.Bestellpositionen.Select("Bestellposition='" & CType(tmpGridRow.FindControl("lblEAN"), Label).Text & "'")(0)("PositionVollstaendig") = "X"
            mObjWareneingangspruefung.Bestellpositionen.Select("Bestellposition='" & CType(tmpGridRow.FindControl("lblEAN"), Label).Text & "'")(0)("PositionAbgeschlossen") = "J"
            tmpBool = False
        Else
            mObjWareneingangspruefung.Bestellpositionen.Select("Bestellposition='" & CType(tmpGridRow.FindControl("lblEAN"), Label).Text & "'")(0)("PositionVollstaendig") = ""
            tmpBool = True
        End If

        CType(tmpGridRow.FindControl("txtPositionLieferMenge"), TextBox).Enabled = tmpBool
        CType(tmpGridRow.FindControl("rbPositionAbgeschlossenJA"), RadioButton).Enabled = tmpBool
        CType(tmpGridRow.FindControl("rbPositionAbgeschlossenNEIN"), RadioButton).Enabled = tmpBool
        CType(tmpGridRow.FindControl("rbPositionAbgeschlossenJA"), RadioButton).Checked = False
        CType(tmpGridRow.FindControl("rbPositionAbgeschlossenNEIN"), RadioButton).Checked = False

    End Sub

    Public Sub rbPositionAbgeschlossenJA_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Public Sub rbPositionAbgeschlossenNEIN_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Public Sub txtPositionLieferMenge_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Protected Sub imgbAllVollstaendig_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        'alle rows die nicht verändert wurden in Menge und abgeschlossen auf vollständig setzen
        For Each tmprow In mObjWareneingangspruefung.Bestellpositionen.Select("PositionVollstaendig=''")
            If tmprow("PositionLieferMenge") Is DBNull.Value AndAlso tmprow("PositionAbgeschlossen").ToString = "" Then
                tmprow("PositionVollstaendig") = "X"
                tmprow("PositionAbgeschlossen") = "J"
            End If
        Next
        For Each tmprow As GridViewRow In GridView1.Rows
            Dim tmpBool As Boolean = False
            CType(tmprow.FindControl("txtPositionLieferMenge"), TextBox).Enabled = tmpBool
            CType(tmprow.FindControl("rbPositionAbgeschlossenJA"), RadioButton).Enabled = tmpBool
            CType(tmprow.FindControl("rbPositionAbgeschlossenNEIN"), RadioButton).Enabled = tmpBool
            If tmpBool = False Then
                CType(tmprow.FindControl("rbPositionAbgeschlossenJA"), RadioButton).Checked = False
                CType(tmprow.FindControl("rbPositionAbgeschlossenNEIN"), RadioButton).Checked = False
            End If
            CType(tmprow.FindControl("chkVollstaendig"), CheckBox).Checked = Not tmpBool
        Next
    End Sub

    Protected Sub imgbAlleUnvollstaendig_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        For Each tmprow In mObjWareneingangspruefung.Bestellpositionen.Select("PositionVollstaendig='X'")
            If tmprow("PositionLieferMenge") Is DBNull.Value Then
                tmprow("PositionVollstaendig") = ""
                tmprow("PositionAbgeschlossen") = ""
            End If
        Next
        For Each tmprow As GridViewRow In GridView1.Rows
            Dim tmpBool As Boolean = True
            CType(tmprow.FindControl("txtPositionLieferMenge2"), TextBox).Enabled = tmpBool
            CType(tmprow.FindControl("rbPositionAbgeschlossenJA"), RadioButton).Enabled = tmpBool
            CType(tmprow.FindControl("rbPositionAbgeschlossenNEIN"), RadioButton).Enabled = tmpBool
            If tmpBool = False Then
                CType(tmprow.FindControl("rbPositionAbgeschlossenJA"), RadioButton).Checked = False
                CType(tmprow.FindControl("rbPositionAbgeschlossenNEIN"), RadioButton).Checked = False
            End If
            CType(tmprow.FindControl("chkVollstaendig"), CheckBox).Checked = tmpBool
        Next
    End Sub

    Private Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GridView1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim tmpChkBox As CheckBox = CType(e.Row.FindControl("chkVollstaendig"), CheckBox)
            Dim tmpTxtBox As TextBox = CType(e.Row.FindControl("txtPositionLieferMenge"), TextBox)
            Dim tmphidden As HtmlInputHidden = CType(e.Row.FindControl("txtPositionLieferMenge2"), HtmlInputHidden)
            Dim tmprbJa As RadioButton = CType(e.Row.FindControl("rbPositionAbgeschlossenJA"), RadioButton)
            Dim tmprbNein As RadioButton = CType(e.Row.FindControl("rbPositionAbgeschlossenNEIN"), RadioButton)

            tmpChkBox.Attributes.Add("onclick", "javascript:checkedRow('" + tmpChkBox.ClientID + "' , '" _
                                                + tmpTxtBox.ClientID + "','" + tmprbJa.ClientID + "','" + tmprbNein.ClientID + "')")
            tmpTxtBox.Attributes.Add("onkeyup", "javascript:MengeChanged('" + tmphidden.ClientID + "' , '" + tmpTxtBox.ClientID + "')")
        End If

    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles GridView1.Sorting
        Checkgrid()
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        '----------------------------------------------------------------------
        'Methode:      lbAbsenden_Click
        'Autor:         Julian Jung
        'Beschreibung: prüft auf vollständigkeit der Eingaben und kennzeichnet diese, sonst Kontroll Anzeige aufrufen
        'Erstellt am:   05.05.2009
        '----------------------------------------------------------------------

        Dim tmpValid As Boolean = True
        Checkgrid()
        For Each tmprow As GridViewRow In GridView1.Rows
            Dim tmpPosition As DataRow = mObjWareneingangspruefung.Bestellpositionen.Select("Bestellposition='" & CType(tmprow.FindControl("lblEAN"), Label).Text & "'")(0)
            If tmpPosition("PositionAbgeschlossen").ToString = "N" AndAlso tmpPosition("PositionLieferMenge") Is DBNull.Value OrElse tmpPosition("PositionAbgeschlossen").ToString = "" AndAlso tmpPosition("PositionVollstaendig").ToString = "0" Then
                'wenn eine Position nicht vollständig ist und menge oder Abgeschlossen nicht gefüllt, dann fehler
                tmpValid = False
            End If
            If tmpPosition("PositionVollstaendig").ToString = "0" AndAlso tmpPosition("PositionAbgeschlossen").ToString = "J" AndAlso tmpPosition("PositionLieferMenge") Is DBNull.Value Then
                tmpValid = False
            End If
        Next

        If mObjWareneingangspruefung.IstUmlagerung = "" Then
            If txtLieferscheinnummer.Text.Replace(" ", "").Length = 0 Then
                tmpValid = False
                txtLieferscheinnummer.BorderColor = Drawing.Color.Red
            Else
                txtLieferscheinnummer.BorderColor = Drawing.Color.Empty
            End If
        End If

        If txtBelegdatum.Text.Replace(" ", "").Length = 0 Then
            tmpValid = False
            txtBelegdatum.BorderColor = Drawing.Color.Red
        ElseIf CDate(txtBelegdatum.Text) <= CDate(Now.ToShortDateString) AndAlso CDate(txtBelegdatum.Text) >= DateAdd(DateInterval.Day, -5, Now) Then
            txtBelegdatum.BorderColor = Drawing.Color.Empty
        Else
            txtBelegdatum.BorderColor = Drawing.Color.Red
            tmpValid = False
        End If

        FillGrid(0)
        If Not tmpValid Then

            For Each tmprow As GridViewRow In GridView1.Rows
                Dim tmpPosition As DataRow = mObjWareneingangspruefung.Bestellpositionen.Select("Bestellposition='" & CType(tmprow.FindControl("lblEAN"), Label).Text & "'")(0)
                If tmpPosition("PositionAbgeschlossen").ToString = "N" AndAlso tmpPosition("PositionLieferMenge") Is DBNull.Value OrElse tmpPosition("PositionAbgeschlossen").ToString = "" AndAlso tmpPosition("PositionVollstaendig").ToString = "0" Then
                    'wenn eine Position nicht vollständig ist und menge oder Abgeschlossen nicht gefüllt, dann fehler
                    tmprow.BackColor = Drawing.Color.Red
                ElseIf tmpPosition("PositionVollstaendig").ToString = "0" AndAlso tmpPosition("PositionAbgeschlossen").ToString = "J" AndAlso tmpPosition("PositionLieferMenge") Is DBNull.Value Then
                    tmprow.BackColor = Drawing.Color.Red
                Else
                    tmprow.BackColor = Drawing.Color.Empty
                End If
                If mObjWareneingangspruefung.IstUmlagerung = "X" Then
                    If tmpPosition("PositionVollstaendig").ToString = "0" AndAlso tmpPosition("PositionLieferMenge") Is DBNull.Value Then
                        tmprow.BackColor = Drawing.Color.Red
                    Else
                        tmprow.BackColor = Drawing.Color.Empty
                    End If
                End If

            Next
            lblError.Text = "Bitte prüfen Sie rot markierte Positionen"
        Else
            'Kontrollscreen anzeigen
            For Each tmprow As GridViewRow In GridView1.Rows
                tmprow.BackColor = Drawing.Color.Empty
            Next
            FillGrid2(0)
            mpeWareneingangsCheck.Show()
        End If
    End Sub

    Private Sub Checkgrid()
        
        For Each tmprow As GridViewRow In GridView1.Rows
            Dim tmpPosition As DataRow = mObjWareneingangspruefung.Bestellpositionen.Select("Bestellposition='" & CType(tmprow.FindControl("lblEAN"), Label).Text & "'")(0)
            Dim tmpMenge As String

            tmpMenge = CType(tmprow.FindControl("txtPositionLieferMenge2"), HtmlInputHidden).Value

            If tmpMenge.Trim = "" Then
                tmpPosition("PositionLieferMenge") = DBNull.Value
            Else
                tmpPosition("PositionLieferMenge") = tmpMenge
            End If

            Dim sVollAll As String
            Dim sVollRowJa As String

            sVollAll = IIf(CType(tmprow.FindControl("chkVollstaendig"), CheckBox).Checked, "X", "0").ToString
            sVollRowJa = IIf(CType(tmprow.FindControl("rbPositionAbgeschlossenJA"), CheckBox).Checked, "J", "").ToString
            If sVollRowJa = "" Then
                sVollRowJa = IIf(CType(tmprow.FindControl("rbPositionAbgeschlossenNEIN"), CheckBox).Checked, "N", "").ToString
            End If

            tmpPosition("PositionVollstaendig") = sVollAll
            If sVollAll = "X" Then
                sVollRowJa = "J"
            End If
            tmpPosition("PositionAbgeschlossen") = sVollRowJa

            mObjWareneingangspruefung.Bestellpositionen.AcceptChanges()
        Next

    End Sub

    Private Sub FillGrid2(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        '----------------------------------------------------------------------
        'Methode:      FillGrid2
        'Autor:         Julian Jung
        'Beschreibung: dieses Grid ist die für die Kontrollanzeige
        'Erstellt am:   05.05.2009
        '----------------------------------------------------------------------

        Dim tmpDataView As New DataView(mObjWareneingangspruefung.Bestellpositionen)

        tmpDataView.Sort = "PositionVollstaendig"

        If tmpDataView.Count = 0 Then
            GridView2.Visible = False
            lblNoData.Visible = True
        Else
            GridView2.Visible = True
            lblNoData.Visible = False

            Dim intTempPageIndex As Int32 = intPageIndex

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

            GridView2.PageIndex = intTempPageIndex
            GridView2.DataSource = tmpDataView

            GridView2.DataBind()

            For Each GridRow As GridViewRow In GridView2.Rows
                Dim tmpPosition As DataRow = mObjWareneingangspruefung.Bestellpositionen.Select("Bestellposition='" & CType(GridRow.FindControl("lblEAN"), Label).Text & "'")(0)

                If tmpPosition("PositionLieferMenge") Is DBNull.Value Then
                    CType(GridRow.FindControl("lblLieferMenge"), Label).Text = tmpPosition("BestellteMenge").ToString
                End If
            Next

        End If
        If mObjKasse.KUNNR = "261030" Then
            GridView2.Columns(4).Visible = False
        End If
    End Sub

    Private Sub GridView2_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles GridView2.Sorting
        FillGrid2(0, e.SortExpression)
        mpeWareneingangsCheck.Show()
    End Sub

    Private Sub doSubmit()
        '----------------------------------------------------------------------
        'Methode:      doSubmit
        'Autor:         Julian Jung
        'Beschreibung: Bucht den Wareneingang im SAP, je nach SAP Status Meldung, bei Fehler bleibt OBJ bestehen.
        'Erstellt am:   05.05.2009
        '----------------------------------------------------------------------
        If mObjWareneingangspruefung.IstUmlagerung = "" Then
            mObjWareneingangspruefung.sendOrderCheckToSAPERP(txtLieferscheinnummer.Text, CDate(txtBelegdatum.Text))
        Else
            mObjWareneingangspruefung.sendUmlToSAPERP(CDate(txtBelegdatum.Text))
        End If

        MPEWareneingangsbuchungResultat.Show()

        If mObjWareneingangspruefung.ErrorOccured Then
            lblWareneingangsbuchungMeldung.ForeColor = Drawing.Color.Red
            lblWareneingangsbuchungMeldung.Text = mObjWareneingangspruefung.ErrorMessage
        Else
            lblWareneingangsbuchungMeldung.ForeColor = Drawing.Color.Green
            lblWareneingangsbuchungMeldung.Text = "Ihre Wareneingangsbuchung war erfolgreich. "
            mObjKasse.Wareneingangspruefung(Me) = Nothing
        End If

    End Sub

    Protected Sub lbWareneingangspruefungFinalize_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbWareneingangsbuchungFinalize.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lbWareneingangOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbWareneingangOk.Click

        mpeWareneingangsCheck.Hide()
        doSubmit()

    End Sub

End Class
