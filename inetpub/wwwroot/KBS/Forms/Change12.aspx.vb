Imports KBS.KBS_BASE
Imports System.IO

Partial Public Class Change12
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjRetoure As Retoure
    Private RetoureTable As DataTable
    Private HeadTable As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)
        lblError.Text = ""
        Title = lblHead.Text
        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        If mObjRetoure Is Nothing Then
            If Session("mObjRetoure") Is Nothing Then
                mObjRetoure = mObjKasse.Retoure(Me)
            Else
                mObjRetoure = CType(Session("mObjRetoure"), Retoure)
            End If
        End If

        If Not IsPostBack Then
            If mObjKasse.Master Then
                txtKST.Enabled = True
                ddlArtikel.Enabled = False
                ddlLieferant.Enabled = False
                rdbRückPost.Enabled = False
                rdbADM.Enabled = False
                txtStückzahl.Enabled = False
                ddlRetouregrund.Enabled = False
                lnbAdd.Enabled = False
            Else
                txtKST.Enabled = False
                txtKST.Text = mObjKasse.Lagerort
                fillDropdown()
                fillDropdownGruende()
                ddlLieferant.SelectedIndex = 0
            End If

            'Wurde schon an SAP übermittelt?
            Session.Add("SendToSap", False)
            Session("Commited") = Nothing
            lbCreatePDF.Attributes.Add("onclick", "window.open('Printpdf.aspx', '_blank', 'left=0,top=0,resizable=YES,scrollbars=YES, menubar=no ');")
        Else
            Dim eventArg As String = Request("__EVENTARGUMENT")
            If eventArg = "MyCustomArgument" Then
                txtBedienerkarte_TextChanged()
            End If

        End If
        txtBedienerkarte.Attributes.Add("onkeyup", "javascript:ControlField(this);")

    End Sub

    Public Sub fillDropdownGruende()
        mObjRetoure.getRetoureGruendeERP()

        ddlRetouregrund.Items.Clear()
        Dim tmpItem As ListItem

        For Each row As DataRow In mObjRetoure.Retouregruende.Rows
            tmpItem = New ListItem(row("DDTEXT").ToString(), row("DOMVALUE_L").ToString())
            ddlRetouregrund.Items.Add(tmpItem)
        Next
    End Sub

    Public Sub fillDropdown()
        If mObjKasse.Master Then
            mObjRetoure.Kostenstelle = mObjRetoure.SendToKost
        Else
            mObjRetoure.Kostenstelle = mObjKasse.Lagerort
        End If
        mObjRetoure.getLieferantenERP(mObjKasse.Firma)

        If mObjRetoure.ErrorOccured Then
            lblError.Text = mObjRetoure.ErrorMessage
        Else
            With mObjRetoure

                Dim tmpItem As ListItem
                Dim i As Int32 = 0
                ddlLieferant.Items.Clear()
                .SelLief = 0
                Do While i < mObjRetoure.Lieferanten.Rows.Count
                    tmpItem = New ListItem(.Lieferanten.Rows(i)("NAME1").ToString, .Lieferanten.Rows(i)("LIFNR").ToString)
                    ddlLieferant.Items.Add(tmpItem)
                    If .Lieferanten.Rows(i)("HAUPT").ToString = "X" Then
                        ddlLieferant.Items(ddlLieferant.Items.Count - 1).Selected = True
                        .SelLief = ddlLieferant.SelectedIndex
                    End If
                    i += 1
                Loop
                CreateDataDisplay()

                FillDropdownArtikel(ddlLieferant.SelectedValue)

            End With
            Session("mObjRetoure") = mObjRetoure
        End If
    End Sub

    Sub FillDropdownArtikel(ByVal Lieferantennummer As String)
        Dim i As Int32 = 0
        Dim tmpItem As ListItem
        mObjRetoure.Kostenstelle = mObjKasse.Lagerort

        mObjRetoure.getArtikelReiterERP(Lieferantennummer)

        ddlArtikel.Items.Clear()
        mObjRetoure.SelArtikel = Nothing
        Dim dv As New DataView(mObjRetoure.Artikel)
        dv.Sort = "REITER asc, POS asc"
        Do While i < mObjRetoure.Artikel.Rows.Count
            tmpItem = New ListItem(dv.Table.Rows(i)("ARTBEZ").ToString, dv.Table.Rows(i)("ARTLIF").ToString)
            ddlArtikel.Items.Add(tmpItem)
            i += 1
        Loop

        ddlArtikel.SelectedIndex = mObjRetoure.SelArtikel
    End Sub

    Private Sub CreateDataDisplay()
        If mObjRetoure.ErrorOccured Then
            GridView3.Visible = False
            lblError.Text = "Es konnten keine Artikel zum ausgewählten Lieferanten geladen werden!"
        Else
            FillGrid("Artikelbezeichnung")
        End If
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")

        Dim tmpDataView As DataView = New DataView(mObjRetoure.Retouren)

        If tmpDataView.Count = 0 Then
            GridView3.Visible = False
            lblNoData.Visible = True
        Else
            GridView3.Visible = True
            lblNoData.Visible = False

            If Not strSort.Length = 0 Then
                tmpDataView.Sort = strSort & " asc"
            End If

            GridView3.DataSource = tmpDataView

            GridView3.DataBind()
        End If
    End Sub

    Protected Sub ddlLieferant_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlLieferant.SelectedIndexChanged
        If Hidden1.Value = "1" Then
            MPE_ChangeLieferant.Show()
            Hidden1.Value = ""
        Else
            If mObjRetoure.ErrorOccured Then
                lblError.Text = mObjRetoure.ErrorMessage
            Else
                If mObjRetoure.Retouren.Rows.Count > 0 Then
                    lblError.Text = "Sie können den Lieferanten erst wechseln, wenn die Retoureartikelliste keine Positionen mehr enthält.<br>" & _
                                    "Schließen Sie die aktuelle Retoure ab oder löschen Sie alle Positionen!"
                    ddlLieferant.SelectedIndex = mObjRetoure.SelLief
                Else
                    FillDropdownArtikel(ddlLieferant.SelectedValue)
                End If
            End If

            mObjRetoure.SelLief = ddlLieferant.SelectedIndex
            CreateDataDisplay()
            lblMessage.Text = ""
            Hidden1.Value = ""
        End If
    End Sub

    Protected Sub txtMenge_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        If IsNumeric(CType(sender, TextBox).Text) Then
            Dim ID As String = CType(CType(sender, TextBox).Parent.Parent, GridViewRow).Cells(0).Text
            For Each Row In mObjRetoure.Retouren.Rows
                If Row("Artikelbezeichnung") = ID Then
                    Row("Menge") = CType(sender, TextBox).Text
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancel.Click
        ddlLieferant.SelectedIndex = mObjRetoure.SelLief
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        Dim blError As Boolean = False

        FillGridBestell()
        If mObjRetoure.Retouren.Rows.Count > 0 Then
            If trADM.Visible And rdbADM.Checked Then
                Dim strLFSnr As String = txtLiefsNr.Text.Trim
                If strLFSnr = "" Then
                    blError = True
                    lblError.Text = "Geben Sie die Lieferscheinnummer ein. <br> " & _
                                    "Diese erhalten Sie vom Außendienst-Mitarbeiter, der die Retoure entgegengenommen hat."
                Else
                    mObjRetoure.Lieferscheinnummer = strLFSnr
                    mObjRetoure.IsADM = True
                End If
            End If

            If blError = False Then
                If CheckBedienerKarte() Then
                    doSubmit()
                Else
                    mpeRetoureCheck.Show()
                    WorkaroundSetFocus()
                End If
            End If
        Else
            lblError.Text = "Es sind keine Artikel in der Liste."
        End If

    End Sub

    Private Sub FillGridBestell()

        Dim tmpDataView As New DataView(mObjRetoure.Retouren)
        tmpDataView.RowFilter = "Menge > 0"
        If tmpDataView.Count = 0 Then
            GridView2.Visible = False
            lblNoData.Visible = True
        Else
            GridView2.Visible = True
            lblNoData.Visible = False

            GridView2.DataSource = tmpDataView
            GridView2.DataBind()

            For Each row As GridViewRow In GridView2.Rows
                If CInt(mObjRetoure.Retouren.Select("ArtIdx='" & CType(row.FindControl("lblArtIdx"), Label).Text & "'")(0)("Menge")) > 2000 Then
                    row.BackColor = Drawing.ColorTranslator.FromHtml("#D87D7D")
                    For Each cell As TableCell In row.Cells
                        cell.ForeColor = Drawing.ColorTranslator.FromHtml("#ffffff")
                    Next

                    CType(row.FindControl("lblStatus"), Label).Text = "Retouremenge über 2000!"
                    GridView2.Columns(GridView2.Columns.Count - 1).Visible = True
                End If
            Next
        End If

    End Sub

    Private Sub WorkaroundSetFocus()
        Dim sb As StringBuilder = New StringBuilder()
        sb.Append("<script type=""text/javascript"">")
        sb.Append("Sys.Application.add_load(modalSetup);")
        sb.Append("function modalSetup() {")
        sb.Append(String.Format("var modalPopup = $find('{0}');", mpeRetoureCheck.BehaviorID))
        sb.Append("modalPopup.add_shown(SetFocusOnControl); }")
        sb.Append("function SetFocusOnControl() {")
        sb.Append(String.Format("var textBox1 = $get('{0}');", txtBedienerkarte.ClientID))
        sb.Append("if (textBox1 != null){textBox1.focus();}}")
        sb.Append("</script>")
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Startup", sb.ToString())
    End Sub

    Private Sub lbRetoureOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbRetoureOk.Click
        If CheckBedienerKarte() Then
            doSubmit()
        Else
            mpeRetoureCheck.Show()
        End If
    End Sub

    Private Function CheckBedienerKarte() As Boolean

        If txtBedienerkarte.Text = String.Empty Then
            lblBedienError.Text = "Bitte lesen Sie die Bedienerkarte ein!"
            Return False
        ElseIf txtBedienerkarte.Text.Length <> 15 Then
            lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Barcode hat die falsche Länge!"
            Return False
        Else
            Try
                Dim strCode As String
                Dim strVerkaeufer As String
                strCode = Left(txtBedienerkarte.Text, 14)
                strCode = Right(strCode, 13)
                strVerkaeufer = strCode.Substring(3, 1)
                strVerkaeufer &= strCode.Substring(6, 1)
                strVerkaeufer &= strCode.Substring(8, 1)
                strVerkaeufer &= strCode.Substring(11, 1)
                mObjRetoure.Verkaeufer = strVerkaeufer
                Return True
            Catch ex As Exception
                lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Versuchen Sie es nochmal!"
                Return False

            End Try
        End If
    End Function

    Private Sub doSubmit()
        '----------------------------------------------------------------------
        'Methode:      doSubmit
        'Autor:         ORu
        'Beschreibung:  je nach SAP status wird hier eine Meldung ausgegeben, im Fehlerfalle bleibt das OBJ bestehen, sonst kill
        'Erstellt am:   11.02.2010
        '----------------------------------------------------------------------
        If CType(Session("SendToSap"), Boolean) = False Then
            mObjRetoure.KostStelle = mObjKasse.Lagerort
            mObjRetoure.SendToKost = txtKST.Text
            mObjRetoure.sendRetoureToSAPERP(ddlLieferant.SelectedValue, mObjKasse.Master)
            Session("Commited") = mObjRetoure.Commited
            Session("SendToSap") = True
        End If

        If Session("Commited") IsNot Nothing Then
            If CType(Session("Commited"), Boolean) = True Then
                lblRetoureMeldung.ForeColor = Drawing.Color.Green
                lblRetoureMeldung.Text = "Ihre Retoure war erfolgreich!"
                If Not mObjRetoure.E_BSTNR = "" Then
                    lblRetoureMeldung.Text += "<br><br>Retourebestellung(" + mObjRetoure.E_BSTNR + ") erfolgreich angelegt!"
                End If
                If Not mObjRetoure.E_BELNR = "" Then
                    lblRetoureMeldung.Text += "<br><br>Materialbeleg(" + mObjRetoure.E_BELNR + ") erfolgreich angelegt!"
                End If
                Abschliessen()
            Else
                lblRetoureMeldung.ForeColor = Drawing.Color.Red
                lblRetoureMeldung.Text = "Ihre Retoure ist fehlgeschlagen: <br><br> " & mObjRetoure.ErrorMessage

            End If
        Else
            lblRetoureMeldung.ForeColor = Drawing.Color.Red
            lblRetoureMeldung.Text = "Ihre Retoure ist fehlgeschlagen."
        End If

        lbRetoureFinalize.Visible = True
        lblRetoureMeldung.Visible = True
        lblStatus.Visible = True
        lbRetoureOk.Visible = False
        trGridview.Visible = False
        txtBedienerkarte.Visible = False

        lblBedienError.Visible = False
        trInfo.Visible = False
        lbRetoureKorrektur.Visible = False
        mpeRetoureCheck.Show()
    End Sub

    Private Sub responseBack()
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub lnbAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnbAdd.Click
        Dim tmpDataTable As DataTable = mObjRetoure.Retouren
        Dim tmpRow As DataRow
        Dim blExist As Boolean = False
        If Not mObjRetoure.Retouren Is Nothing Then
            For Each Row In tmpDataTable.Rows
                If Row("Artikelbezeichnung") = ddlArtikel.SelectedItem.Text And Row("Retouregrund") = ddlRetouregrund.SelectedItem.Text Then
                    lblError.Text = "Der gewählte Artikel befindet sich bereits in der Retoureartikelliste. <br>" & _
                                    "Ändern Sie die Stückzahl direkt in der Liste!"
                    blExist = True
                    Exit For
                End If
            Next

            If blExist = False Then
                ' Neue Spalte mit Index Wert festlegen
                mObjRetoure.Retoureindex = mObjRetoure.Retoureindex + 1
                Dim tmpobj() As Object = {mObjRetoure.Retoureindex}
                tmpRow = tmpDataTable.Rows.Add(tmpobj)

                tmpRow("ARTLIF") = ddlArtikel.SelectedValue
                tmpRow("Artikelbezeichnung") = ddlArtikel.SelectedItem.Text
                tmpRow("Retouregrund") = ddlRetouregrund.SelectedItem.Text
                tmpRow("GRUND") = ddlRetouregrund.SelectedValue
                If txtStückzahl.Text.Trim() = "" Or CInt(txtStückzahl.Text.Trim()) < 1 Then
                    tmpRow.Delete()
                    lblError.Text = "Geben Sie ein Menge größer 0 ein!"
                Else
                    tmpRow("Menge") = CInt(txtStückzahl.Text.Trim)
                End If
            End If
            txtStückzahl.Text = 0
        End If
        CreateDataDisplay()
    End Sub

    Private Sub ddlArtikel_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlArtikel.SelectedIndexChanged
        mObjRetoure.SelArtikel = ddlArtikel.SelectedIndex
    End Sub

    Private Sub ProofCheckADM()
        If rdbADM.Checked = True Then
            spRückADM.Visible = True
            txtLiefsNr.Visible = True
        Else
            spRückADM.Visible = False
            txtLiefsNr.Visible = False
            txtLiefsNr.Text = String.Empty
        End If
    End Sub

    Private Sub GridView3_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView3.RowCommand
        Select Case e.CommandName
            Case "minusMenge"
                Dim tmpRow As DataRow = mObjRetoure.Retouren.Rows.Find(CType(e.CommandArgument, Integer))
                tmpRow("Menge") = CType(tmpRow("Menge"), Integer) - 1
                FillGrid()
            Case "plusMenge"
                Dim tmpRow As DataRow = mObjRetoure.Retouren.Rows.Find(CType(e.CommandArgument, Integer))
                tmpRow("Menge") = CType(tmpRow("Menge"), Integer) + 1
                FillGrid()
            Case "entfernen"
                Dim tmpRow As DataRow = mObjRetoure.Retouren.Rows.Find(CType(e.CommandArgument, Integer))
                tmpRow.Delete()
                FillGrid()
        End Select
    End Sub

    Private Sub lbRetoureFinalize_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbRetoureFinalize.Click
        mpeRetoureCheck.Hide()
        If CBool(Session("ShowPrint")) Then
            MPERetourePrint.Show()
        End If
        Session("ShowPrint") = True
        Session("SendToSap") = False
        mObjRetoure.Clear()
        FillGrid()
        RetoureCheckReset()
    End Sub

    Private Sub lbRetoureKorrektur_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbRetoureKorrektur.Click
        mpeRetoureCheck.Hide()
    End Sub

    Private Sub txtBedienerkarte_TextChanged()
        If CheckBedienerKarte() Then
            doSubmit()
        Else
            mpeRetoureCheck.Show()
        End If
    End Sub

    Private Sub Abschliessen()

        Try
            Dim files() As String
            If Directory.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Retoure\" & mObjKasse.Lagerort) Then
                files = Directory.GetFiles(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Retoure\" & mObjKasse.Lagerort & "\", "*.pdf")
                For Each sFile As String In files
                    File.Delete(sFile)
                Next
            End If

            HeadTable = New DataTable("Kopf")
            HeadTable.Columns.Add("Kostenstelle", GetType(System.String))
            HeadTable.Columns.Add("Filiale", GetType(System.String))
            HeadTable.Columns.Add("Lieferant", GetType(System.String))
            HeadTable.Columns.Add("BSTNR", GetType(System.String))
            HeadTable.Columns.Add("BELNR", GetType(System.String))
            HeadTable.Columns.Add("Datum", GetType(System.String))
            HeadTable.Columns.Add("LSNrADM", GetType(System.String))


            RetoureTable = New DataTable
            RetoureTable.TableName = "Retoure"
            RetoureTable.Columns.Add("Artikel", GetType(System.String))
            RetoureTable.Columns.Add("Menge", GetType(System.String))
            RetoureTable.Columns.Add("Retouregrund", GetType(System.String))

            Dim tmpSAPRow As DataRow
            tmpSAPRow = HeadTable.NewRow
            If mObjKasse.Master = True Then
                tmpSAPRow("Kostenstelle") = mObjRetoure.SendToKost
                tmpSAPRow("Filiale") = mObjRetoure.KostText
            Else
                tmpSAPRow("Kostenstelle") = mObjKasse.Lagerort
                tmpSAPRow("Filiale") = mObjRetoure.KostStelleNameERP(mObjKasse.Lagerort)
            End If

            tmpSAPRow("Lieferant") = ddlLieferant.SelectedItem.Text
            If Not mObjRetoure.E_BSTNR Is Nothing Then
                tmpSAPRow("BSTNR") = mObjRetoure.E_BSTNR
            Else
                tmpSAPRow("BSTNR") = ""
            End If
            If Not mObjRetoure.E_BELNR Is Nothing Then
                tmpSAPRow("BELNR") = mObjRetoure.E_BELNR
            Else
                tmpSAPRow("BELNR") = ""
            End If
            tmpSAPRow("Datum") = Now.ToShortDateString
            If Not mObjRetoure.Lieferscheinnummer Is Nothing Then
                tmpSAPRow("LSNrADM") = mObjRetoure.Lieferscheinnummer
            Else
                tmpSAPRow("LSNrADM") = ""
            End If

            HeadTable.Rows.Add(tmpSAPRow)

            For Each selRows As DataRow In mObjRetoure.Retouren.Rows
                Dim tmpSAPRow2 = RetoureTable.NewRow
                tmpSAPRow2("Artikel") = selRows("Artikelbezeichnung").ToString
                tmpSAPRow2("Menge") = selRows("Menge").ToString
                tmpSAPRow2("Retouregrund") = selRows("Retouregrund").ToString
                RetoureTable.Rows.Add(tmpSAPRow2)
            Next

            PrintPDF()
            txtLiefsNr.Text = ""
            Session("App_ContentType") = "Application/pdf"
            Session("ShowPrint") = True

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Erstellen des PDF-Dokuments: " + ex.Message
            Session("ShowPrint") = False
        Finally
            FillGrid()
        End Try
    End Sub

    Private Sub PrintPDF()

        Try

            Dim imageHt As New Hashtable()
            Dim sFilePath As String = mObjKasse.Lagerort & "_Retoure"
            If mObjKasse.Master = True Then
                sFilePath = mObjRetoure.SendToKost & "_Retoure"
            End If
            If Not Directory.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Retoure\" & mObjKasse.Lagerort) Then
                Directory.CreateDirectory(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Retoure\" & mObjKasse.Lagerort)
            End If

            mObjRetoure.Filepath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Retoure\" & mObjKasse.Lagerort & "\" & sFilePath & ".pdf"
            Dim docFactory As New DocumentGeneration.WordDocumentFactory(RetoureTable, imageHt)
            If mObjRetoure.Lieferscheinnummer Is Nothing Then
                docFactory.CreateDocumentTable(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Retoure\" & mObjKasse.Lagerort & "\" & sFilePath, Page, "\Vorlagen\Retoureschein.doc", HeadTable)
            Else
                docFactory.CreateDocumentTable(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Retoure\" & mObjKasse.Lagerort & "\" & sFilePath, Page, "\Vorlagen\RetourescheinADM.doc", HeadTable)
            End If
            Session("App_Filepath") = mObjRetoure.Filepath

            RetoureTable.Rows.Clear()

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Private Sub lbCreatePDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreatePDF.Click

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub rdbADM_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbADM.CheckedChanged
        ProofCheckADM()
    End Sub

    Private Sub rdbRückPost_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbRückPost.CheckedChanged
        ProofCheckADM()
    End Sub

    Private Sub RetoureCheckReset()
        lblBedienError.Visible = True
        lblBedienError.Text = "Einscannen der Bedienerkarte!"
        lblStatus.Visible = False
        txtBedienerkarte.Text = ""
        txtBedienerkarte.Visible = True
        trGridview.Visible = True
        lblRetoureMeldung.Visible = False
        lblRetoureMeldung.Text = ""
        trInfo.Visible = True
        lbRetoureFinalize.Visible = False
        lbRetoureKorrektur.Visible = True
        lbRetoureOk.Visible = True
    End Sub

    Protected Sub txtKST_TextChanged(sender As Object, e As EventArgs) Handles txtKST.TextChanged
        If mObjKasse.Master Then
            If txtKST.Text.Length > 0 Then
                With mObjRetoure
                    .CheckKostStelleERP(txtKST.Text.Trim)
                    If .ErrorOccured Then
                        lblError.Text = .ErrorMessage
                        SetFocus(txtKST)
                        lblKSTText.Visible = False
                        lblKSTText.Text = ""
                        ddlArtikel.Enabled = False
                        ddlLieferant.Enabled = False
                        txtStückzahl.Enabled = False
                        ddlRetouregrund.Enabled = False
                        rdbRückPost.Enabled = False
                        rdbADM.Enabled = False
                        lnbAdd.Enabled = False
                    Else
                        lblKSTText.Visible = True
                        lblKSTText.Text = .KostText
                        fillDropdown()
                        fillDropdownGruende()
                        ddlLieferant.SelectedIndex = 0
                        ddlArtikel.Enabled = True
                        ddlLieferant.Enabled = True
                        txtStückzahl.Enabled = True
                        ddlRetouregrund.Enabled = True
                        lnbAdd.Enabled = True
                        rdbRückPost.Enabled = True
                        rdbADM.Enabled = True
                        SetFocus(ddlArtikel)
                    End If
                End With
            End If
        Else
            SetFocus(txtKST)
            lblKSTText.Visible = False
            lblKSTText.Text = ""
            ddlArtikel.Enabled = True
            ddlLieferant.Enabled = True
            txtStückzahl.Enabled = True
            ddlRetouregrund.Enabled = True
            lnbAdd.Enabled = True
            rdbRückPost.Enabled = True
            rdbADM.Enabled = True
        End If
    End Sub

End Class