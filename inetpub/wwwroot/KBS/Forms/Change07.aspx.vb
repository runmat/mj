Imports System.Globalization
Imports KBS.KBS_BASE
Imports System.IO

Partial Public Class Change07
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjUmlagerung As Umlagerung
    Private ReportTable As DataTable
    Private mObjBestellung As Bestellung

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)
        lblError.Text = ""

        Title = lblHead.Text

        If mObjKasse Is Nothing Then
            If Session("mKasse") IsNot Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        If mObjBestellung Is Nothing Then
            If Session("mBestellung") IsNot Nothing Then
                mObjBestellung = CType(Session("mBestellung"), Bestellung)
            Else
                mObjBestellung = mObjKasse.Bestellung(Me)
                Session("mBestellung") = mObjBestellung
            End If
        End If

        If mObjUmlagerung Is Nothing Then
            If Session("mUmlagerung") IsNot Nothing Then
                mObjUmlagerung = CType(Session("mUmlagerung"), Umlagerung)
            Else
                mObjUmlagerung = mObjKasse.Umlagerungen(Me)
                Session("mUmlagerung") = mObjUmlagerung
            End If
        End If

        If mObjUmlagerung.ErrorOccured Then
            lblError.Text = mObjUmlagerung.ErrorMessage
        End If

        If Not IsPostBack Then

            mObjUmlagerung.KostStelle = mObjKasse.Lagerort
            If mObjKasse.KUNNR <> "261030" Then
                fillDropdown()
                trPlaceHolderArtikel.Visible = True
                trArtikel.Visible = True
                lbAusparken.Visible = True
                lbParken.Visible = True
            End If

            txtKST.Focus()

            lbCreatePDF.Attributes.Add("onclick", "window.open('Printpdf.aspx', '_blank', 'left=0,top=0,resizable=YES,scrollbars=YES, menubar=no ');")

        End If

    End Sub

    Public Sub fillDropdown()
        mObjUmlagerung.ShowERP()
        If Not mObjUmlagerung.ErrorOccured Then

            With mObjUmlagerung
                ddlArtikel.DataSource = .Artikel
                ddlArtikel.DataValueField = "MATNR"
                ddlArtikel.DataTextField = "MAKTX"
                ddlArtikel.DataBind()
            End With

            If Not mObjUmlagerung.Umlagerung Is Nothing Then
                FillGrid()
            End If
        Else
            lblError.Text = "Es konnten keine Artikel geladen werden!"
        End If

        Session("mUmlagerung") = mObjUmlagerung
    End Sub

    Protected Sub lbtnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnInsert.Click
        doinsert()
    End Sub

    Private Sub doinsert()
        If Not String.IsNullOrEmpty(txtMenge.Text) Then
            'wenn dies gefüllt, dann Artikel korrekt
            With mObjUmlagerung
                Dim KennzForm As String = ""
                Dim rows As DataRow() = .Artikel.Select("MATNR='" & ddlArtikel.SelectedValue & "'")

                If tdKennzFormShow.Visible Then
                    KennzForm = ddlKennzform.SelectedItem.Text
                    If .Umlagerung.Select("MATNR='" & ddlArtikel.SelectedValue & "' AND KENNZFORM = '" & KennzForm & "'").Count > 0 Then
                        lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten!"
                        Exit Sub
                    End If
                ElseIf .Umlagerung.Select("MATNR='" & ddlArtikel.SelectedValue & "'").Count > 0 Then
                    lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten!"
                    Exit Sub
                End If

                If rows.Length > 0 Then
                    .insertIntoBestellungen(ddlArtikel.SelectedValue, CInt(txtMenge.Text), ddlArtikel.SelectedItem.Text, "", KennzForm)

                    If Not IsDBNull(rows(0)("TEXTPFLICHT")) AndAlso CChar(rows(0)("TEXTPFLICHT")) = "X"c Then
                        OpenInfotext(CStr(rows(0)("MATNR")), "", True, KennzForm)
                    End If
                End If

                txtMenge.Text = ""
                FillGrid()

            End With

            Session("mUmlagerung") = mObjUmlagerung

        Else
            lblError.Text = "Bitte geben sie eine Menge ein"
        End If
    End Sub

    Private Sub OpenInfotext(ByVal MatNr As String, _
                             ByVal Text As String, _
                             ByVal Pflicht As Boolean, _
                             ByVal sKennzForm As String)
        txtInfotext.Text = Text
        lblMatNr.Text = MatNr
        lblKennzForm.Text = sKennzForm
        If Pflicht Then
            lblPflicht.Text = "true"
        Else
            lblPflicht.Text = "false"
        End If

        MPEInfotext.Show()
    End Sub

    Private Sub CloseInfotext()
        txtInfotext.Text = ""
        lblMatNr.Text = ""
        lblKennzForm.Text = ""
        lblPflicht.Text = ""

        MPEInfotext.Hide()
        FillGrid()
    End Sub

    Private Sub FillGrid()

        Dim tmpDataView As New DataView(mObjUmlagerung.Umlagerung)

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
        Else
            GridView1.Visible = True
            lblNoData.Visible = False

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()

            If mObjKasse.KUNNR = "261030" Then
                GridView1.Columns(3).Visible = False
                GridView1.Columns(9).Visible = False
            End If
        End If
    End Sub

    Private Sub FillGrid2()

        Dim tmpDataView As New DataView(mObjUmlagerung.Umlagerung)

        If tmpDataView.Count = 0 Then
            GridView2.Visible = False
            lblNoData.Visible = True
        Else
            GridView2.Visible = True
            lblNoData.Visible = False

            GridView2.DataSource = tmpDataView
            GridView2.DataBind()

            If mObjKasse.KUNNR = "261030" Then
                GridView2.Columns(3).Visible = False
            End If
        End If   
    End Sub

    Private Sub doSubmit()

        mObjUmlagerung.ChangeERP()
        If mObjUmlagerung.ErrorOccured Then
            lblBestellMeldung.ForeColor = Drawing.Color.Red
            lblBestellMeldung.Text = "Ihre Umlagerung ist fehlgeschlagen: <br><br> " & mObjUmlagerung.ErrorMessage
            MPEBestellResultat.Show()
        Else
            lblBestellMeldung.ForeColor = Drawing.Color.Green
            lblBestellMeldung.Text = "Ihre Umlagerung war erfolgreich!"

            Dim selRows As DataRow

            ReportTable = New DataTable
            ReportTable.TableName = "Bestellung"
            ReportTable.Columns.Add("Artikel", GetType(System.String))
            ReportTable.Columns.Add("Menge", GetType(System.String))
            ReportTable.Columns.Add("Langtext", GetType(System.String))
            ReportTable.Columns.Add("KennzForm", GetType(System.String))

            For Each selRows In mObjUmlagerung.Umlagerung.Rows
                Dim tmpSAPRow2 = ReportTable.NewRow
                tmpSAPRow2("Artikel") = selRows("MAKTX").ToString
                tmpSAPRow2("Menge") = selRows("Menge").ToString
                tmpSAPRow2("Langtext") = selRows("LTEXT").ToString
                tmpSAPRow2("KennzForm") = selRows("KennzForm").ToString
                ReportTable.Rows.Add(tmpSAPRow2)
            Next

            PrintPDF()
            Session("App_ContentType") = "Application/pdf"

            If mObjKasse.KUNNR <> "261030" Then
                ddlArtikel.SelectedIndex = 0
            End If

            txtKST.Text = ""
            txtMenge.Text = ""
            txtMengeEAN.Text = ""
            mObjUmlagerung.Umlagerung.Rows.Clear()

            mObjUmlagerung.KostStelleNeu = ""
            mObjUmlagerung.BelegNR = ""
            mObjUmlagerung.KostText = ""
            mObjUmlagerung.FilePath = ""
            mObjUmlagerung.Umlagerung.Rows.Clear()

            mObjUmlagerung.ClearErrorState()

            GridView1.Visible = False
            GridView1.DataSource = Nothing
            lblNoData.Visible = True
            lblKSTText.Text = ""
            lblKSTText.Visible = False
            MPEBestellResultat.Show()
        End If

        Session("mUmlagerung") = mObjUmlagerung
    End Sub

    Private Sub PrintPDF()

        Try
            Dim headTable As New DataTable("Kopf")
            headTable.Columns.Add("Kostenstelle", GetType(System.String))
            headTable.Columns.Add("KostenstelleText", GetType(System.String))
            headTable.Columns.Add("KostenstelleNeu", GetType(System.String))
            headTable.Columns.Add("KostenstelleNeuText", GetType(System.String))
            headTable.Columns.Add("KostenstelleNeuAdresse", GetType(System.String))
            headTable.Columns.Add("Referenz", GetType(System.String))
            headTable.Columns.Add("Datum", GetType(System.String))

            Dim tmpSAPRow As DataRow
            tmpSAPRow = headTable.NewRow
            tmpSAPRow("Kostenstelle") = mObjUmlagerung.KostStelle
            mObjUmlagerung.CheckKostStelleERP(mObjUmlagerung.KostStelle)
            tmpSAPRow("KostenstelleText") = mObjUmlagerung.KostText
            mObjUmlagerung.CheckKostStelleERP(mObjUmlagerung.KostStelleNeu)
            tmpSAPRow("KostenstelleNeu") = mObjUmlagerung.KostStelleNeu
            tmpSAPRow("KostenstelleNeuText") = mObjUmlagerung.KostText

            Dim adr As DataTable = mObjUmlagerung.GetAdresseLagerort(mObjKasse.Werk, mObjUmlagerung.KostStelleNeu)
            If adr.Rows.Count > 0 Then
                tmpSAPRow("KostenstelleNeuAdresse") = adr(0)("NAME1") & vbNewLine & _
                                                        adr(0)("NAME2") & vbNewLine & _
                                                        adr(0)("STREET") & " " & adr(0)("HOUSE_NUM1") & vbNewLine & _
                                                        adr(0)("POST_CODE1") & " " & adr(0)("CITY1")
            Else
                tmpSAPRow("KostenstelleNeuAdresse") = ""
            End If

            tmpSAPRow("Referenz") = mObjUmlagerung.BelegNR
            tmpSAPRow("Datum") = DateTime.Today.ToShortDateString()

            headTable.Rows.Add(tmpSAPRow)

            Dim imageHt As New Hashtable()
            Dim sFilePath As String = mObjUmlagerung.KostStelle & "_" & mObjUmlagerung.KostStelleNeu & "_" & Replace(Now.ToShortDateString, ".", "") & "_" & Replace(Now.ToShortTimeString, ":", "")
            mObjUmlagerung.FilePath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Umlagerung\" & sFilePath & ".pdf"
            Dim docFactory As New DocumentGeneration.WordDocumentFactory(ReportTable, imageHt)
            If mObjKasse.KUNNR = "261030" Then
                docFactory.CreateDocumentTable(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Umlagerung\" & sFilePath, Page, "\Vorlagen\BestellungOhneKZGroesse.doc", headTable)
            Else
                docFactory.CreateDocumentTable(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Umlagerung\" & sFilePath, Page, "\Vorlagen\Bestellung.doc", headTable)
            End If

            Session("App_Filepath") = mObjUmlagerung.FilePath
            ReportTable.Rows.Clear()
        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Private Sub responseBack()
        Session("mBestellung") = Nothing
        Session("mUmlagerung") = Nothing
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView1.RowCommand
        ApplyMengen()

        Select Case e.CommandName

            Case "entfernen"
                mObjUmlagerung.Umlagerung.Select("MATNR='" & e.CommandArgument.ToString & "'")(0).Delete()
                If mObjUmlagerung.Umlagerung.Rows.Count = 0 Then
                    mObjUmlagerung.Umlagerung.Rows.Clear()
                    mObjUmlagerung.KostStelleNeu = ""
                End If

            Case "bearbeiten"
                Dim TRow As DataRow = mObjUmlagerung.Umlagerung.Select("MATNR='" & e.CommandArgument.ToString & "'")(0)
                Dim PRow As DataRow = mObjUmlagerung.Artikel.Select("MATNR='" & e.CommandArgument.ToString & "'")(0)

                Dim strLText As String = ""
                Dim strKennzForm As String = ""
                Dim bPflicht As Boolean = False

                If Not IsDBNull(TRow("LTEXT")) Then
                    strLText = CStr(TRow("LTEXT"))
                End If
                If Not IsDBNull(PRow("TEXTPFLICHT")) Then
                    If CChar(PRow("TEXTPFLICHT")) = "X"c Then
                        bPflicht = True
                    Else
                        bPflicht = False
                    End If
                End If
                If Not IsDBNull(TRow("KENNZFORM")) Then
                    strKennzForm = CStr(TRow("KENNZFORM"))
                End If
                OpenInfotext(CStr(TRow("MATNR")), strLText, bPflicht, strKennzForm)

            Case "minusMenge"
                Dim rows As DataRow() = mObjUmlagerung.Umlagerung.Select("MATNR=" & e.CommandArgument)
                If rows.GetLength(0) > 0 Then
                    rows(0)("Menge") -= 1
                End If

            Case "plusMenge"
                Dim rows As DataRow() = mObjUmlagerung.Umlagerung.Select("MATNR=" & e.CommandArgument)
                If rows.GetLength(0) > 0 Then
                    rows(0)("Menge") += 1
                End If

        End Select

        Session("mUmlagerung") = mObjUmlagerung

        FillGrid()
    End Sub

    Private Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        If mObjUmlagerung.Umlagerung.Rows.Count = 0 Then
            lblError.Text = "Sie haben keine Artikel für eine Umlagerung hinzugefügt"
        Else
            If mObjKasse.Werk = "1010" Then
                Dim dt As DataTable = mObjUmlagerung.GetListeAusparkenERP()
                Dim dRowParken() As DataRow
                If Not String.IsNullOrEmpty(mObjUmlagerung.BelegNrParken) Then
                    dRowParken = dt.Select("LGORT_EMPF = '" & mObjUmlagerung.KostStelleNeu.Trim() & "' AND ERDAT ='" + DateTime.Today.ToShortDateString() + "' AND BELNR <> '" & mObjUmlagerung.BelegNrParken & "'")
                Else
                    dRowParken = dt.Select("LGORT_EMPF = '" & mObjUmlagerung.KostStelleNeu.Trim() & "' AND ERDAT ='" + DateTime.Today.ToShortDateString() + "'")
                End If

                If dRowParken.Length > 0 Then
                    lblError.Text = "Achtung! Sie haben für heute schon eine Umlagerung an die Kst." + mObjUmlagerung.KostStelleNeu + " geparkt. Bitte diese erst ausparken."
                    Exit Sub
                End If
            End If
            If Not CheckFreitexte() Then
                lblError.Text = "Achtung! Bei mindestens einer Position fehlt der erforderliche Freitext. Bitte erfassen Sie diesen vor dem Absenden."
                Exit Sub
            End If
            ApplyMengen()
            FillGrid2()
            mpeBestellungsCheck.Show()
        End If
    End Sub

    Private Sub lbBestellungOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellungOk.Click
        mpeBestellungsCheck.Hide()
        doSubmit()
    End Sub

    Private Sub txtKST_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtKST.TextChanged
        If Not String.IsNullOrEmpty(txtKST.Text) Then
            If ApplyKst(True) Then
                If mObjKasse.KUNNR <> "261030" Then
                    SetFocus(ddlArtikel)
                Else
                    SetFocus(txtEAN)
                End If
            End If
        End If
    End Sub

    Private Sub lbCreatePDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreatePDF.Click

    End Sub

    Protected Sub txtEAN_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEAN.TextChanged
        If Not mObjBestellung.getArtikelInfo(txtEAN.Text.Trim, True, txtMaterialnummer.Text, lblArtikelbezeichnung.Text) Then
            If mObjBestellung.ErrorOccured Then
                lblError.Text = mObjBestellung.ErrorMessage
            Else
                lblError.Text = "Artikel nicht vorhanden"
            End If

            lblArtikelbezeichnung.Text = "(wird automatisch ausgefüllt)"
            txtMaterialnummer.Text = ""
            lbtnInsert.Visible = False
        Else
            lbtnInsert.Visible = True
            SetFocus(txtMengeEAN)
        End If

        Session("mBestellung") = mObjBestellung
    End Sub

    Protected Sub lbtnInsertEAN_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnInsertEAN.Click
        doinsertEan()
    End Sub

    Private Sub doinsertEan()
        If Not String.IsNullOrEmpty(txtMengeEAN.Text) Then
            If Not String.IsNullOrEmpty(txtMaterialnummer.Text) Then
                'wenn dies gefüllt, dann Artikel korrekt
                With mObjUmlagerung

                    If .Umlagerung.Select("MATNR='" & txtMaterialnummer.Text & "'").Count > 0 Then
                        lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten"
                        Exit Sub
                    End If

                    Dim KennzForm As String = ""

                    If (tdKennzFormShow.Visible = True) Then
                        KennzForm = ddlKennzform.SelectedItem.Text
                    End If

                    If mObjKasse.KUNNR <> "261030" Then
                        Dim rows As DataRow() = .Artikel.Select("MATNR='" & txtMaterialnummer.Text & "'")

                        If rows.Length > 0 Then
                            .insertIntoBestellungen(txtMaterialnummer.Text, CInt(txtMengeEAN.Text), lblArtikelbezeichnung.Text, txtEAN.Text, KennzForm)

                            If Not IsDBNull(rows(0)("TEXTPFLICHT")) AndAlso CChar(rows(0)("TEXTPFLICHT")) = "X"c Then
                                OpenInfotext(CStr(rows(0)("MATNR")), "", True, KennzForm)
                            End If
                        End If
                    Else
                        .insertIntoBestellungen(txtMaterialnummer.Text, CInt(txtMengeEAN.Text), lblArtikelbezeichnung.Text, txtEAN.Text, KennzForm)
                    End If

                    txtEAN.Text = ""
                    txtMaterialnummer.Text = ""
                    txtMengeEAN.Text = ""
                    lblArtikelbezeichnung.Text = "(wird automatisch ausgefüllt)"
                    FillGrid()

                End With

                Session("mUmlagerung") = mObjUmlagerung
            Else
                lblError.Text = "Artikel nicht vorhanden"
            End If
        Else
            lblError.Text = "Bitte geben sie eine Menge ein"
        End If


    End Sub

    Private Sub lbAusparken_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAusparken.Click
        lblErrorAusparken.Text = ""
        MPEAusparken.Show()
    End Sub

    Private Sub lbParken_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbParken.Click
        If mObjKasse.Werk = "1010" Then
            Dim dt As DataTable = mObjUmlagerung.GetListeAusparkenERP()
            Dim dRowParken() As DataRow
            If Not String.IsNullOrEmpty(mObjUmlagerung.BelegNrParken) Then
                dRowParken = dt.Select("LGORT_EMPF = '" & mObjUmlagerung.KostStelleNeu.Trim() & "' AND ERDAT ='" + DateTime.Today.ToShortDateString() + "' AND BELNR <> '" & mObjUmlagerung.BelegNrParken & "'")
            Else
                dRowParken = dt.Select("LGORT_EMPF = '" & mObjUmlagerung.KostStelleNeu.Trim() & "' AND ERDAT ='" + DateTime.Today.ToShortDateString() + "'")
            End If

            If dRowParken.Length > 0 Then
                lblError.Text = "Achtung! Sie haben für heute schon eine Umlagerung an die Kst." + mObjUmlagerung.KostStelleNeu + " geparkt. Bitte diese erst ausparken."
                Exit Sub
            End If
        End If
        If Not CheckFreitexte() Then
            lblError.Text = "Achtung! Bei mindestens einer Position fehlt der erforderliche Freitext. Bitte erfassen Sie diesen vor dem Parken."
            Exit Sub
        End If
        ApplyMengen()
        mObjUmlagerung.ParkenERP()
        If mObjUmlagerung.ErrorOccured Then
            lblError.Text = mObjUmlagerung.ErrorMessage
        End If
        Session("mUmlagerung") = mObjUmlagerung
        FillGrid()
        txtKST.Text = ""
        lblKSTText.Text = ""
    End Sub

    Private Sub lbInfotextSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbInfotextSave.Click
        lblErrorInfotext.Text = ""
        If lblPflicht.Text = "true" AndAlso txtInfotext.Text.TrimStart(",") = "" Then
            lblErrorInfotext.Text = "Geben Sie einen Text ein!"
            MPEInfotext.Show()
            Exit Sub
        Else
            mObjUmlagerung.updateBestellungInfotext(lblMatNr.Text, txtInfotext.Text.TrimStart(","), lblKennzForm.Text)
        End If
        Session("mUmlagerung") = mObjUmlagerung
        CloseInfotext()
    End Sub

    Private Sub lbInfotextClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbInfotextClose.Click
        lblErrorInfotext.Text = ""
        If lblPflicht.Text = "true" AndAlso txtInfotext.Text.TrimStart(",") = "" Then
            lblErrorInfotext.Text = "Geben Sie einen Text ein!"
            MPEInfotext.Show()
            Exit Sub
        End If
        CloseInfotext()
    End Sub

    Private Sub lbAusparkenClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAusparkenClose.Click
        MPEAusparken.Hide()
        FillGrid()
    End Sub

    Protected Sub gvAusparken_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvAusparken.RowCommand
        lblErrorAusparken.Text = ""

        Select Case e.CommandName

            Case "ausparken"
                mObjUmlagerung.AusparkenERP(e.CommandArgument)
                If mObjUmlagerung.ErrorOccured Then
                    lblErrorAusparken.Text = mObjUmlagerung.ErrorMessage
                Else
                    txtKST.Text = mObjUmlagerung.KostStelleNeu
                    ApplyKst()
                End If

            Case "löschen"
                mObjUmlagerung.GeparktLoeschenERP(e.CommandArgument, "X")
                If mObjUmlagerung.ErrorOccured Then
                    lblErrorAusparken.Text = mObjUmlagerung.ErrorMessage
                End If
                MPEAusparken.Show()

        End Select
        Session("mUmlagerung") = mObjUmlagerung
        FillGrid()
    End Sub

    Private Sub Change07_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim dt As DataTable = mObjUmlagerung.GetListeAusparkenERP()
        If dt.Rows.Count > 0 Then
            lbAusparken.Visible = True
        Else
            lbAusparken.Visible = False
        End If
        gvAusparken.DataSource = dt
        gvAusparken.DataBind()

        Dim dtNachdruck As DataTable = GetListeNachdruck()
        If dtNachdruck.Rows.Count > 0 Then
            lbNachdruck.Visible = True
        Else
            lbNachdruck.Visible = False
        End If
        dtNachdruck.DefaultView.Sort = "Datum DESC"
        gvNachdruck.DataSource = dtNachdruck.DefaultView
        gvNachdruck.DataBind()

        If mObjKasse.KUNNR <> "261030" Then
            If mObjUmlagerung.Umlagerung.Rows.Count > 0 Then
                lbParken.Visible = True
            Else
                lbParken.Visible = False
            End If
        End If

    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim editLink As ImageButton = CType(e.Row.FindControl("ibEditInfotext"), ImageButton)
            If editLink IsNot Nothing Then
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(editLink)
            End If
        End If
    End Sub

    Protected Sub ddlArtikel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlArtikel.SelectedIndexChanged

        mObjUmlagerung.GetKennzFormERP(ddlArtikel.SelectedValue)

        If mObjUmlagerung.ErrorOccured Then
            tdKennzForm.Visible = False
            tdKennzFormShow.Visible = False

            If mObjUmlagerung.ErrorCode <> "101" Then
                lblError.Text = mObjUmlagerung.ErrorMessage
            End If
        Else
            tdKennzForm.Visible = True
            tdKennzFormShow.Visible = True
            ddlKennzform.DataSource = mObjUmlagerung.ListeGroesse
            ddlKennzform.DataTextField = "KENNZFORM"
            ddlKennzform.DataValueField = "VK_MATNR"
            ddlKennzform.DataBind()
            Dim ItemDefault As ListItem
            ItemDefault = ddlKennzform.Items.FindByText("520x114")
            If (ItemDefault IsNot Nothing) Then
                ItemDefault.Selected = True
            End If
        End If

        Session("mUmlagerung") = mObjUmlagerung
    End Sub

    Private Sub ApplyMengen()
        For Each tmprow As GridViewRow In GridView1.Rows
            Dim labelMatnr As Label = CType(tmprow.FindControl("lblMatnr"), Label)
            Dim textMenge As TextBox = CType(tmprow.FindControl("txtMenge"), TextBox)

            Dim rows As DataRow() = mObjUmlagerung.Umlagerung.Select("MATNR='" & labelMatnr.Text & "'")
            If rows.GetLength(0) > 0 Then
                If IsNumeric(textMenge.Text) Then
                    rows(0)("Menge") = Int32.Parse(textMenge.Text)
                Else
                    rows(0)("Menge") = 0
                End If
            End If
        Next

        Session("mUmlagerung") = mObjUmlagerung
    End Sub

    Private Function CheckFreitexte() As Boolean
        If mObjUmlagerung.Artikel IsNot Nothing Then
            For Each row As DataRow In mObjUmlagerung.Umlagerung.Rows
                Dim artRows As DataRow() = mObjUmlagerung.Artikel.Select("MATNR='" & row("MATNR") & "'")
                If artRows.Length > 0 AndAlso Not IsDBNull(artRows(0)("TEXTPFLICHT")) AndAlso CChar(artRows(0)("TEXTPFLICHT")) = "X"c AndAlso (IsDBNull(row("LTEXT")) OrElse String.IsNullOrEmpty(row("LTEXT").ToString())) Then
                    'Erforderlicher Freitext nicht gefüllt
                    Return False
                End If
            Next
        End If      

        Return True
    End Function

    Private Sub lbNachdruck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbNachdruck.Click
        MPENachdruck.Show()
    End Sub

    Private Sub lbNachdruckClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbNachdruckClose.Click
        MPENachdruck.Hide()
    End Sub

    Private Function GetListeNachdruck() As DataTable
        Dim tbl As New DataTable()
        tbl.Columns.Add("Dateiname", GetType(String))
        tbl.Columns.Add("EmpfangendeKst", GetType(String))
        tbl.Columns.Add("Datum", GetType(DateTime))

        Dim verzeichnis As New DirectoryInfo(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Umlagerung")
        Dim dateien() As FileInfo = verzeichnis.GetFiles(mObjKasse.Lagerort & "_*")
        For Each datei In dateien
            Dim newRow As DataRow = tbl.NewRow()
            newRow("Dateiname") = datei.Name
            Dim nameRaw As String = datei.Name.Substring(0, datei.Name.Length - datei.Extension.Length)
            If nameRaw.Length > 18 Then
                'Neues Format: Kst_EmpfKst_Datum_Zeit
                newRow("EmpfangendeKst") = nameRaw.Substring(5, 4)
                newRow("Datum") = DateTime.ParseExact(nameRaw.Substring(10, 13), "ddMMyyyy_HHmm", CultureInfo.CurrentCulture)
            Else
                'Altes Format: Kst_Datum_Zeit
                newRow("EmpfangendeKst") = ""
                newRow("Datum") = DateTime.ParseExact(nameRaw.Substring(5, 13), "ddMMyyyy_HHmm", CultureInfo.CurrentCulture)
            End If
            tbl.Rows.Add(newRow)
        Next

        Return tbl
    End Function

    Protected Sub gvNachdruck_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvNachdruck.RowCommand
        Select Case e.CommandName
            Case "drucken"
                ShowBeleg(e.CommandArgument)
        End Select
    End Sub

    Private Sub ShowBeleg(ByVal dateiname As String)
        Session("App_ContentType") = "Application/pdf"
        Session("App_Filepath") = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Umlagerung\" & dateiname
        If (Not ClientScript.IsStartupScriptRegistered("Enabled")) Then

            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("<script type=""text/javascript"">")
            sb.Append("window.open(""Printpdf.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf)
            sb.Append("</script>")
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Enabled", sb.ToString())

        End If
    End Sub

    Private Function ApplyKst(Optional ByVal skipIfNotChanged As Boolean = False) As Boolean
        With mObjUmlagerung

            If skipIfNotChanged AndAlso .KostStelleNeu = txtKST.Text.Trim Then
                'Kst nicht geändert
                Return True
            End If

            .CheckKostStelleERP(txtKST.Text.Trim)

            If .ErrorOccured Then
                lblError.Text = .ErrorMessage
                lblKSTText.Visible = False
                lblKSTText.Text = ""
                SetFocus(txtKST)
                Return False
            End If

            lblKSTText.Visible = True
            lblKSTText.Text = .KostText

            If .KostStelle = txtKST.Text.Trim Then
                lblError.Text = "Sie können nicht zu Ihrer eigenen Kostenstelle umlagern!"
                SetFocus(txtKST)
                Return False
            Else
                .KostStelleNeu = txtKST.Text.Trim
            End If

        End With

        Session("mUmlagerung") = mObjUmlagerung

        Return True
    End Function

End Class