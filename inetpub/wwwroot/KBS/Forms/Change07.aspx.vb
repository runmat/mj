Imports KBS.KBS_BASE

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

                Dim tmpItem As ListItem
                Dim i As Int32 = 0
                ddlArtikel.Items.Clear()

                Do While i < .Artikel.Rows.Count
                    tmpItem = New ListItem(.Artikel.Rows(i)("MAKTX").ToString, .Artikel.Rows(i)("MATNR").ToString)
                    ddlArtikel.Items.Add(tmpItem)
                    i += 1
                Loop

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
        If txtMenge.Text.Trim(" "c).Length > 0 Then
            If Not txtMenge.Text = "" Then
                'wenn dies gefüllt, dann Artikel korrekt
                With mObjUmlagerung
                    .CheckKostStelleERP(txtKST.Text.Trim)
                    If .ErrorOccured Then
                        lblError.Text = .ErrorMessage
                        SetFocus(txtKST)
                        lblKSTText.Visible = False
                        lblKSTText.Text = ""
                        Exit Sub
                    End If
                    If .KostStelle = txtKST.Text.Trim Then
                        lblError.Text = "Sie können nicht zu Ihrer eigenen Kostenstelle umlagern!"
                        SetFocus(txtKST)
                        Exit Sub
                    ElseIf .KostStelleNeu = "" Then
                        .KostStelleNeu = txtKST.Text.Trim
                    ElseIf .KostStelleNeu <> txtKST.Text.Trim Then
                        lblError.Text = "Bitte schließen Sie erst die Umlagerung für eine Kostenstelle ab!"
                        Exit Sub
                    Else
                        .KostStelleNeu = txtKST.Text.Trim
                    End If

                    lblERDAT.Text = Now.ToShortDateString
                    Dim KennzForm As String = ""
                    Dim rows As DataRow() = mObjUmlagerung.Artikel.Select("MATNR='" & ddlArtikel.SelectedValue & "'")

                    If (tdKennzFormShow.Visible = True) Then
                        KennzForm = ddlKennzform.SelectedItem.Text

                        If .Umlagerung.Select("MATNR='" & ddlArtikel.SelectedValue & "' AND KENNZFORM = '" & KennzForm & "'").Count > 0 Then
                            lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten!"
                            Exit Sub
                        Else
                            If rows.GetLength(0) > 0 Then
                                Dim row As DataRow = rows(0)
                                If Not IsDBNull(row("TEXTPFLICHT")) Then
                                    If CChar(row("TEXTPFLICHT")) = "X"c Then
                                        OpenInfotext(CStr(row("MATNR")), txtMenge.Text, "", "", CStr(row("MAKTX")), "", True, KennzForm)
                                    Else
                                        .insertIntoBestellungen(ddlArtikel.SelectedValue, CInt(txtMenge.Text), ddlArtikel.SelectedItem.Text, "", "", KennzForm)
                                    End If
                                Else
                                    .insertIntoBestellungen(ddlArtikel.SelectedValue, CInt(txtMenge.Text), ddlArtikel.SelectedItem.Text, "", "", KennzForm)
                                End If
                            End If

                            txtMenge.Text = ""
                            FillGrid()
                        End If
                    ElseIf .Umlagerung.Select("MATNR='" & ddlArtikel.SelectedValue & "'").Count = 0 Then

                        If rows.GetLength(0) > 0 Then
                            Dim row As DataRow = rows(0)
                            If Not IsDBNull(row("TEXTPFLICHT")) Then
                                If CChar(row("TEXTPFLICHT")) = "X"c Then
                                    OpenInfotext(CStr(row("MATNR")), txtMenge.Text, "", "", CStr(row("MAKTX")), "", True, KennzForm)
                                Else
                                    .insertIntoBestellungen(ddlArtikel.SelectedValue, CInt(txtMenge.Text), ddlArtikel.SelectedItem.Text, "", "", KennzForm)
                                End If
                            Else
                                .insertIntoBestellungen(ddlArtikel.SelectedValue, CInt(txtMenge.Text), ddlArtikel.SelectedItem.Text, "", "", KennzForm)
                            End If
                        End If
                        txtMenge.Text = ""
                        FillGrid()
                    Else
                        lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten!"
                    End If

                End With

                Session("mUmlagerung") = mObjUmlagerung

            End If
        Else
            lblError.Text = "Bitte geben sie eine Menge ein"
        End If


    End Sub

    Private Sub OpenInfotext(ByVal MatNr As String, _
                             ByVal Menge As String, _
                             ByVal Text As String, _
                             ByVal TextNr As String, _
                             ByVal MatText As String, _
                             ByVal EAN As String, _
                             ByVal Pflicht As Boolean, _
                             ByVal sKennzForm As String)
        txtInfotext.Text = Text
        lblLTextNr.Text = TextNr
        lblMatNr.Text = MatNr
        lblMenge.Text = Menge
        lblKennzForm.Text = sKennzForm
        If Pflicht Then
            lblPflicht.Text = "true"
        Else
            lblPflicht.Text = "false"
        End If
        lblArtikelbezeichnungInfo.Text = MatText
        lblEAN.Text = EAN

        MPEInfotext.Show()
    End Sub

    Private Sub CloseInfotext()
        txtInfotext.Text = ""
        lblLTextNr.Text = ""
        lblMatNr.Text = ""
        lblKennzForm.Text = ""
        lblPflicht.Text = ""
        lblArtikelbezeichnungInfo.Text = ""
        lblEAN.Text = ""

        MPEInfotext.Hide()
        FillGrid()
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjUmlagerung.Umlagerung)

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
        End If
        If mObjKasse.KUNNR = "261030" Then
            GridView2.Columns(3).Visible = False
        End If
    End Sub

    Private Sub doSubmit()

        mObjUmlagerung.BelegNR = ""
        mObjUmlagerung.ChangeERP(lblERDAT.Text)
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
            tmpSAPRow("Datum") = lblERDAT.Text
            If (lblERDAT.Text = String.Empty) Then
                tmpSAPRow("Datum") = Now.ToShortDateString
            End If

            headTable.Rows.Add(tmpSAPRow)


            Dim imageHt As New Hashtable()
            Dim sFilePath As String = mObjUmlagerung.KostStelle & "_" & Replace(Now.ToShortDateString, ".", "") & "_" & Replace(Now.ToShortTimeString, ":", "")
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

                Dim strMenge As String = ""
                Dim strLText As String = ""
                Dim strLTextNr As String = ""
                Dim strMAKTX As String = ""
                Dim strEAN As String = ""
                Dim strKennzForm As String = ""
                Dim bPflicht As Boolean = False

                If Not IsDBNull(TRow("Menge")) Then
                    strMenge = CStr(TRow("Menge"))
                End If
                If Not IsDBNull(TRow("LTEXT")) Then
                    strLText = CStr(TRow("LTEXT"))
                End If
                If Not IsDBNull(TRow("LTEXT_NR")) Then
                    strLTextNr = CStr(TRow("LTEXT_NR"))
                End If
                If Not IsDBNull(PRow("MAKTX")) Then
                    strMAKTX = CStr(PRow("MAKTX"))
                End If
                If Not IsDBNull(TRow("EAN11")) Then
                    strEAN = CStr(TRow("EAN11"))
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
                OpenInfotext(CStr(TRow("MATNR")), strMenge, strLText, strLTextNr, strMAKTX, strEAN, bPflicht, strKennzForm)
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
                Dim dRowParken() As DataRow = dt.Select("LGORT_EMPF = '" + txtKST.Text.Trim + "' AND ERDAT ='" + lblERDAT.Text + "'")

                If dRowParken.Length > 0 Then
                    lblError.Text = "Achtung! Sie haben für heute schon eine Umlagerung an die Kst." + mObjUmlagerung.KostStelleNeu + " geparkt. Bitte diese erst ausparken."
                    Exit Sub
                End If
            End If
            FillGrid2()
            mpeBestellungsCheck.Show()
        End If

    End Sub

    Private Sub lbBestellungOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellungOk.Click
        mpeBestellungsCheck.Hide()
        doSubmit()
    End Sub

    Private Sub txtKST_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtKST.TextChanged
        If txtKST.Text.Length > 0 Then
            With mObjUmlagerung
                .CheckKostStelleERP(txtKST.Text.Trim)
                If .ErrorOccured Then
                    lblError.Text = .ErrorMessage
                    SetFocus(txtKST)
                    lblKSTText.Visible = False
                    lblKSTText.Text = ""
                Else
                    lblKSTText.Visible = True
                    lblKSTText.Text = .KostText
                    If mObjKasse.KUNNR <> "261030" Then
                        SetFocus(ddlArtikel)
                    Else
                        SetFocus(txtEAN)
                    End If

                End If
            End With

        End If
    End Sub

    Private Sub lbCreatePDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreatePDF.Click

    End Sub

    Protected Sub txtEAN_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtEAN.TextChanged
        If Not mObjBestellung.getArtikelInfo(txtEAN.Text.Trim, True, txtMaterialnummer.Text, lblArtikelbezeichnung.Text) Then
            If mObjBestellung.E_MESSAGE.Length > 0 Then
                lblError.Text = mObjBestellung.E_MESSAGE
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
        If txtMengeEAN.Text.Trim(" "c).Length > 0 Then
            If Not txtMaterialnummer.Text = "" Then
                'wenn dies gefüllt, dann Artikel korrekt
                With mObjUmlagerung
                    .CheckKostStelleERP(txtKST.Text.Trim)
                    If .ErrorOccured Then
                        lblError.Text = .ErrorMessage
                        txtKST.Focus()
                        lblKSTText.Visible = False
                        lblKSTText.Text = ""
                        Exit Sub
                    End If
                    If .KostStelle = txtKST.Text.Trim Then
                        lblError.Text = "Sie können nicht zu Ihrer eigenen Kostenstelle umlagern!"
                        txtKST.Focus()
                        Exit Sub
                    ElseIf .KostStelleNeu = "" Then
                        .KostStelleNeu = txtKST.Text.Trim
                    ElseIf .KostStelleNeu <> txtKST.Text.Trim Then
                        lblError.Text = "Bitte schließen Sie erst die Umlagerung für eine Kostenstelle ab!"
                        Exit Sub
                    Else
                        .KostStelleNeu = txtKST.Text.Trim

                    End If
                    lblERDAT.Text = Now.ToShortDateString
                    If .Umlagerung.Select("MATNR='" & txtMaterialnummer.Text & "'").Count = 0 Then
                        Dim KennzForm As String = ""

                        If (tdKennzFormShow.Visible = True) Then
                            KennzForm = ddlKennzform.SelectedItem.Text
                        End If

                        If mObjKasse.KUNNR <> "261030" Then
                            Dim rows As DataRow() = mObjUmlagerung.Artikel.Select("MATNR='" & txtMaterialnummer.Text & "'")
                            If rows.GetLength(0) > 0 Then
                                Dim row As DataRow = rows(0)
                                If Not IsDBNull(row("TEXTPFLICHT")) Then
                                    If CChar(row("TEXTPFLICHT")) = "X"c Then
                                        OpenInfotext(CStr(row("MATNR")), txtMengeEAN.Text, "", "", CStr(row("MAKTX")), txtEAN.Text, True, KennzForm)
                                    Else
                                        .insertIntoBestellungen(txtMaterialnummer.Text, CInt(txtMengeEAN.Text), lblArtikelbezeichnung.Text, txtEAN.Text, "", KennzForm)
                                    End If
                                Else
                                    .insertIntoBestellungen(txtMaterialnummer.Text, CInt(txtMengeEAN.Text), lblArtikelbezeichnung.Text, txtEAN.Text, "", KennzForm)
                                End If
                            End If
                        Else
                            .insertIntoBestellungen(txtMaterialnummer.Text, CInt(txtMengeEAN.Text), lblArtikelbezeichnung.Text, txtEAN.Text, "", KennzForm)
                        End If

                        txtEAN.Text = ""
                        txtMaterialnummer.Text = ""
                        txtMengeEAN.Text = ""
                        lblArtikelbezeichnung.Text = "(wird automatisch ausgefüllt)"
                        FillGrid()
                    Else
                        lblError.Text = "Artikel ist in der aktuellen Bestellung schon enthalten"
                    End If

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
        MPEAusparken.Show()
    End Sub

    Private Sub lbParken_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbParken.Click

        If mObjKasse.Werk = "1010" Then
            Dim dt As DataTable = mObjUmlagerung.GetListeAusparkenERP()
            Dim dRowParken() As DataRow = dt.Select("LGORT_EMPF = '" + txtKST.Text.Trim + "' AND ERDAT ='" + lblERDAT.Text + "'")

            If dRowParken.Length > 0 Then
                lblError.Text = "Achtung! Sie haben für heute schon eine Umlagerung an die Kst." + mObjUmlagerung.KostStelleNeu + " geparkt. Bitte diese erst ausparken."
                Exit Sub
            End If
        End If
        mObjUmlagerung.ParkenERP()
        Session("mUmlagerung") = mObjUmlagerung
        FillGrid()
        txtKST.Text = ""
        lblKSTText.Text = ""

    End Sub

    Private Sub lbInfotextSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbInfotextSave.Click
        lblErrorInfotext.Text = ""
        If lblPflicht.Text = "true" Then
            If txtInfotext.Text.TrimStart(",") = "" Then
                lblErrorInfotext.Text = "Geben Sie einen Text ein!"
                MPEInfotext.Show()
                Exit Sub
            Else
                mObjUmlagerung.insertIntoBestellungen(lblMatNr.Text, CInt(lblMenge.Text), lblArtikelbezeichnungInfo.Text, lblEAN.Text, lblLTextNr.Text, txtInfotext.Text.TrimStart(","), lblKennzForm.Text)
            End If
        Else
            mObjUmlagerung.insertIntoBestellungen(lblMatNr.Text, CInt(lblMenge.Text), lblArtikelbezeichnungInfo.Text, lblEAN.Text, lblLTextNr.Text, txtInfotext.Text.TrimStart(","), lblKennzForm.Text)
        End If
        Session("mUmlagerung") = mObjUmlagerung
        CloseInfotext()
    End Sub

    Private Sub lbInfotextClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbInfotextClose.Click
        CloseInfotext()
    End Sub

    Private Sub lbAusparkenClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAusparkenClose.Click
        MPEAusparken.Hide()
        FillGrid()
    End Sub

    Protected Sub gvAusparken_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvAusparken.RowCommand
        Select Case e.CommandName
            Case "ausparken"
                mObjUmlagerung.AusparkenERP(e.CommandArgument)
                txtKST.Text = mObjUmlagerung.KostStelleNeu
                mObjUmlagerung.CheckKostStelleERP(txtKST.Text)
                lblKSTText.Text = mObjUmlagerung.KostText
                lblERDAT.Text = mObjUmlagerung.ParkDate
            Case "löschen"
                mObjUmlagerung.GeparktLoeschenERP(e.CommandArgument, "X")
                MPEAusparken.Show()
        End Select
        Session("mUmlagerung") = mObjUmlagerung
        FillGrid()
    End Sub

    Protected Sub ibAusparkenTable_Click(ByVal sender As Object, ByVal e As EventArgs)
        txtKST.Text = mObjUmlagerung.KostStelleNeu
        FillGrid()
    End Sub

    Private Sub Change07_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If mObjUmlagerung.KostStelle = "" Then
            mObjUmlagerung.KostStelle = mObjKasse.Lagerort
        End If

        Dim dt As DataTable = mObjUmlagerung.GetListeAusparkenERP()
        If dt.Rows.Count > 0 Then
            lbAusparken.Visible = True
        Else
            lbAusparken.Visible = False
        End If
        gvAusparken.DataSource = dt
        gvAusparken.DataBind()

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

End Class