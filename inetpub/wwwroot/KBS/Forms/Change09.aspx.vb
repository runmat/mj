Imports KBS.KBS_BASE
Imports System.IO
Imports DocumentTools.Services

Partial Public Class Change09
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjInventur As Inventur
    Private ReportTable As DataTable
    Private KonsiReportTable As DataTable
    Private headTable As DataTable
    Private KonsiHeadTable As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        Title = lblHead.Text

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        If Session("mObjInventur") IsNot Nothing Then
            mObjInventur = CType(Session("mObjInventur"), Inventur)
        Else
            mObjInventur = New Inventur(mObjKasse)
            mObjInventur.FillProdukthierarchieERP()
            Session("mObjInventur") = mObjInventur
        End If

        If mObjInventur.ErrorOccured Then
            lblNoData.Text = mObjInventur.ErrorMessage
            lblNoData.Visible = True
            lbAbsenden.Visible = False
            If mObjInventur.ErrorCode = "102" Then
                If File.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\Inventur.pdf") Then
                    mObjInventur.Filepath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\Inventur.pdf"
                    lbNachdruck.Visible = True
                End If
            End If
        End If

        If Not IsPostBack Then
            If mObjInventur.ErrorCode = "102" Then
                Exit Sub
            Else
                fillRepeater()
            End If
        End If

        If Not mObjInventur.Produktierarchie Is Nothing Then
            Dim iZaehl As Integer = 0
            For Each Row As DataRow In mObjInventur.Produktierarchie.Rows
                If Not Row("ZAEHLVH") Is String.Empty Then
                    iZaehl += 1
                End If
            Next
            If iZaehl = mObjInventur.Produktierarchie.Rows.Count AndAlso mObjInventur.Produktierarchie.Rows.Count > 0 Then
                lbAbsenden.Visible = True
            Else
                lbAbsenden.Visible = False
            End If
        End If

    End Sub

    Private Sub fillRepeater()
        Repeater1.DataSource = mObjInventur.Produktierarchie
        Repeater1.DataBind()
    End Sub

    Protected Sub lbtNext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim lbTemp As LinkButton = CType(sender, LinkButton)
            mObjInventur.ProdHNr = lbTemp.CommandArgument
            Dim dRow As DataRow = mObjInventur.Produktierarchie.Select("PRODH = '" & mObjInventur.ProdHNr & "'")(0)
            Dim tmpERFTYP As String = dRow("ERFTYP").ToString
            Dim tmpProdBezeich As String = dRow("VTEXT").ToString
            mObjInventur.ErfTyp = tmpERFTYP
            mObjInventur.ProdHBezeichnung = tmpProdBezeich
            mObjInventur.FillInventurMaterialienERP()

            If mObjInventur.ErrorOccured Then
                If mObjInventur.ErrorCode = "104" Then
                    dRow("ZAEHLVH") = "X"
                    fillRepeater()
                End If
                lblNoData.Text = mObjInventur.ErrorMessage
                lblNoData.Visible = True
                Session("mObjInventur") = mObjInventur
                Exit Sub
            End If
            Session("mObjInventur") = mObjInventur

            If tmpERFTYP = "1" Then
                Response.Redirect("Change09_1.aspx")
            ElseIf mObjKasse.Werk = "1030" Then
                Response.Redirect("Change09_KBS.aspx")
            Else
                Response.Redirect("Change09_2.aspx")
            End If

        Catch ex As Exception
            lblNoData.Text = "Fehler beim Lesen der Materialien: " + ex.Message
        End Try
    End Sub

    Private Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        mpeCheck1.Show()
    End Sub

    Private Sub InventurDrucken(ByVal blnTestdruck As Boolean)
        Try
            Dim bKonsiPrint As Boolean = False
            Dim files() As String
            Dim filesUtsch() As String
            If Directory.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort) Then
                files = Directory.GetFiles(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\", "*.pdf")
                For Each sFile As String In files
                    File.Delete(sFile)
                Next
            End If

            Dim ProdHCount As Integer = 0
            For Each PHRow As DataRow In mObjInventur.Produktierarchie.Rows
                ProdHCount += 1
                headTable = New DataTable("Kopf")
                headTable.Columns.Add("Kostenstelle", GetType(System.String))
                headTable.Columns.Add("Inventurart", GetType(System.String))
                headTable.Columns.Add("ProdHBezeichnung", GetType(System.String))
                headTable.Columns.Add("Testdruck", GetType(System.String))
                headTable.Columns.Add("Datum", GetType(System.String))
                headTable.Columns.Add("Kunnr", GetType(System.String))

                KonsiHeadTable = headTable.Clone

                Dim selRows As DataRow

                ReportTable = New DataTable
                ReportTable.TableName = "Inventur"
                ReportTable.Columns.Add("ArtNR", GetType(System.String))
                ReportTable.Columns.Add("Artikel", GetType(System.String))
                ReportTable.Columns.Add("Menge", GetType(System.String))
                '### Konsi-Druck
                KonsiReportTable = ReportTable.Clone
                KonsiReportTable.Columns.Add("ArtNRUtsch", GetType(System.String))

                Dim tmpSAPRow As DataRow
                tmpSAPRow = headTable.NewRow
                tmpSAPRow("Kostenstelle") = mObjKasse.Lagerort
                If blnTestdruck Then
                    tmpSAPRow("Testdruck") = "Testdruck"
                Else
                    tmpSAPRow("Testdruck") = ""
                End If
                If mObjInventur.InvTyp = "1" Then
                    tmpSAPRow("Inventurart") = "Jahresinventur"
                Else
                    tmpSAPRow("Inventurart") = "Zwischeninventur"
                End If
                tmpSAPRow("ProdHBezeichnung") = PHRow("VTEXT").ToString
                tmpSAPRow("Datum") = Now.ToShortDateString
                headTable.Rows.Add(tmpSAPRow)

                '### Konsi-Druck
                Dim tmpKonsiSAPRow As DataRow
                tmpKonsiSAPRow = KonsiHeadTable.NewRow
                tmpKonsiSAPRow("Kostenstelle") = mObjKasse.Lagerort
                If blnTestdruck Then
                    tmpKonsiSAPRow("Testdruck") = "Testdruck"
                Else
                    tmpKonsiSAPRow("Testdruck") = ""
                End If
                If mObjInventur.InvTyp = "1" Then
                    tmpKonsiSAPRow("Inventurart") = "Jahresinventur"
                Else
                    tmpKonsiSAPRow("Inventurart") = "Zwischeninventur"
                End If
                tmpKonsiSAPRow("ProdHBezeichnung") = PHRow("VTEXT").ToString
                tmpKonsiSAPRow("Datum") = Now.ToShortDateString

                mObjInventur.ProdHNr = PHRow("PRODH").ToString
                mObjInventur.ErfTyp = PHRow("ERFTYP").ToString
                mObjInventur.FillInventurMaterialienERP()

                If mObjInventur.InvMaterialien.Rows.Count > 0 Then
                    Dim bKonsi As Boolean = False
                    For Each selRows In mObjInventur.InvMaterialien.Rows

                        If selRows("SOBKZ").ToString = "K" Then '### Konsi-Druck
                            Dim tmpSAPRowKonsi = KonsiReportTable.NewRow
                            tmpSAPRowKonsi("ArtNR") = selRows("MATNR").ToString
                            tmpSAPRowKonsi("ArtNRUtsch") = selRows("ARTLIF").ToString.TrimStart("0")
                            tmpSAPRowKonsi("Artikel") = selRows("MAKTX").ToString
                            tmpSAPRowKonsi("Menge") = CInt(selRows("ERFMG").ToString)
                            KonsiReportTable.Rows.Add(tmpSAPRowKonsi)
                            bKonsi = True
                        Else
                            Dim tmpSAPRow2 = ReportTable.NewRow
                            tmpSAPRow2("ArtNR") = selRows("MATNR").ToString
                            tmpSAPRow2("Artikel") = selRows("MAKTX").ToString
                            tmpSAPRow2("Menge") = CInt(selRows("ERFMG").ToString)
                            ReportTable.Rows.Add(tmpSAPRow2)
                        End If
                    Next
                    Dim stemp As String = ""
                    If ProdHCount.ToString.Length = 1 Then
                        stemp = "0" & ProdHCount.ToString
                    ElseIf ProdHCount.ToString.Length > 1 Then
                        stemp = ProdHCount.ToString
                    End If
                    ' Konsiartikel in der Produkthierarchie
                    If bKonsi = True Then
                        tmpKonsiSAPRow("Kunnr") = mObjInventur.KunnrUtschFiliale
                        KonsiHeadTable.Rows.Add(tmpKonsiSAPRow)
                        PrintPDFKonsi(tmpSAPRow("ProdHBezeichnung").ToString, stemp)
                        bKonsiPrint = bKonsi
                    End If
                    ' Vielleicht nur Konsiartikel in der Produkthierarchie
                    If ReportTable.Rows.Count > 0 Then
                        PrintPDF(tmpSAPRow("ProdHBezeichnung").ToString, stemp)
                    Else
                        mObjInventur.Filepath = Nothing
                        Session("App_Filepath") = Nothing
                    End If
                End If
            Next

            files = Directory.GetFiles(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\", "*Kroschke.pdf")
            filesUtsch = Directory.GetFiles(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\", "*Utsch.pdf")

            Dim sPath As String
            Dim filesByte As New List(Of Byte())()

            'Seiten der Utsch-Inventur sollen am Anfang des Dokuments stehen
            If bKonsiPrint = True Then
                For Each sFile As String In filesUtsch
                    filesByte.Add(File.ReadAllBytes(sFile))
                Next
            End If
            For Each sFile As String In files
                filesByte.Add(File.ReadAllBytes(sFile))
            Next

            If blnTestdruck Then
                sPath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\TestInventur.pdf"
            Else
                sPath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\Inventur.pdf"
            End If
            If filesByte.Count > 0 Then
                File.WriteAllBytes(sPath, PdfDocumentFactory.MergePdfDocuments(filesByte))
                mObjInventur.Filepath = sPath
            End If
            Session("App_ContentType") = "Application/pdf"
            Session("mObjInventur") = mObjInventur

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Erstellen des PDF-Dokuments: " + ex.Message
        End Try
    End Sub

    Private Sub PrintPDF(ByVal sProdH As String, ByVal sProdHCount As String)

        Try

            Dim imageHt As New Hashtable()
            Dim sFilePath As String = mObjKasse.Lagerort & "_" & sProdHCount & sProdH & "Kroschke"

            If Not Directory.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort) Then
                Directory.CreateDirectory(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort)
            End If

            mObjInventur.Filepath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\Inventur"
            Dim docFactory As New DocumentGeneration.WordDocumentFactory(ReportTable, imageHt)
            docFactory.CreateDocumentTable(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\" & sFilePath, Page, "\Vorlagen\Inventur.doc", headTable)
            ReportTable.Rows.Clear()

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Private Sub PrintPDFKonsi(ByVal sProdH As String, ByVal sProdHCount As String)

        Try

            Dim imageHt As New Hashtable()
            Dim sFilePath As String = mObjKasse.Lagerort & "_" & sProdHCount & sProdH & "Utsch"

            If Not Directory.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort) Then
                Directory.CreateDirectory(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort)
            End If

            mObjInventur.Filepath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\InventurUtsch"
            Dim docFactory As New DocumentGeneration.WordDocumentFactory(KonsiReportTable, imageHt)
            docFactory.CreateDocumentTable(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\" & sFilePath, Page, "\Vorlagen\InventurKonsi.doc", KonsiHeadTable)
            KonsiReportTable.Rows.Clear()

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Private Sub PDFAnzeigen(ByVal blnTestdruck As Boolean)
        Session("App_ContentType") = "Application/pdf"
        Session("App_Filepath") = mObjInventur.Filepath
        If (Not ClientScript.IsStartupScriptRegistered("Enabled")) Then

            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("<script type=""text/javascript"">")
            If Not mObjInventur.Filepath Is Nothing Then
                sb.Append("window.open(""Printpdf.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf)
            End If
            sb.Append("</script>")
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Enabled", sb.ToString())

        End If
        Session("mObjInventur") = mObjInventur
        If blnTestdruck Then
            mpeCheck1.Show()
        Else
            lbAbsenden.Enabled = False
        End If
    End Sub

    Private Sub InventurAbschliessen()
        Try
            mObjInventur.SetInventurEndERP()
            If mObjInventur.ErrorOccured Then
                lblNoData.Visible = True
                lblNoData.Text = "Fehler beim Abschließen der Inventur: " + mObjInventur.ErrorMessage
                Session("mObjInventur") = mObjInventur
                Exit Sub
            Else
                For Each RepItem As RepeaterItem In Repeater1.Items
                    For Each ctrl As Control In RepItem.Controls
                        If TypeOf ctrl Is LinkButton Then
                            Dim lButton As LinkButton = CType(ctrl, LinkButton)
                            lButton.Enabled = False
                        End If
                    Next
                Next

                lbAbsenden.Enabled = False
                lblNoData.Visible = True
                lblNoData.Text = "Inventur erfolgreich abgeschlossen!"
                PDFAnzeigen(False)
                Session("mObjInventur") = Nothing
            End If
        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Abschließen der Inventur: " + ex.Message
        End Try
    End Sub

    Private Sub lbTestdruck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbTestdruck.Click
        InventurDrucken(True)
        PDFAnzeigen(True)
    End Sub

    Private Sub lbOk1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbOk1.Click
        mpeCheck2.Show()
    End Sub

    Private Sub lbOk2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbOk2.Click
        InventurDrucken(False)
        InventurAbschliessen()
    End Sub

    Protected Sub lbNachdruck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbNachdruck.Click
        PDFAnzeigen(False)
    End Sub

End Class