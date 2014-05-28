Imports KBS.KBS_BASE
Imports System.IO
Imports SmartSoft.PdfLibrary

Partial Public Class Change09_3
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
        If Not Session("mObjInventur") Is Nothing Then
            mObjInventur = CType(Session("mObjInventur"), Inventur)
        Else
            Throw New Exception("benötigtes Session Objekt nicht vorhanden")
        End If

        If Not IsPostBack Then

        End If
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        If mObjInventur.EndInventur = True Then
            mObjInventur = New Inventur(mObjKasse)
            mObjInventur.FillProdukthierarchieERP()
            If mObjInventur.E_SUBRC <> 0 Then
                lblNoData.Text = mObjInventur.E_MESSAGE
                lblNoData.Visible = True
            End If
            Session("mObjInventur") = mObjInventur
        End If
        Response.Redirect("Change09.aspx")
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        If mObjInventur.EndInventur = False Then

            If (Not ClientScript.IsStartupScriptRegistered("Enabled")) Then
                Session("App_FilepathUtsch") = mObjInventur.FilepathUtsch
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append("<script type=""text/javascript"">var newWind;")
                If Not mObjInventur.FilepathUtsch Is Nothing Then
                    sb.Append("newWind=window.open(""PrintPDF2.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf)
                End If
                If Not mObjInventur.Filepath Is Nothing Then
                    Session("App_Filepath") = mObjInventur.Filepath
                    sb.Append("newWind=window.open(""Printpdf.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf)
                End If
                sb.Append("</script>")
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Enabled", sb.ToString())

            End If
            mObjInventur.EndInventur = True
            Session("mObjInventur") = mObjInventur
        Else
            Abschliessen()
        End If

    End Sub

    Private Sub Abschliessen()
        Try
            Dim files() As String
            Dim bKonsiPrint As Boolean = False
            Dim filesUtsch() As String
            Dim filesByte As New List(Of Byte())()
            Dim filesByteUtsch As New List(Of Byte())()
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
                If mObjInventur.EndInventur = False Then
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
                If mObjInventur.EndInventur = False Then
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
                    If bKonsi = True Then
                        tmpKonsiSAPRow("Kunnr") = mObjInventur.KunnrUtschFiliale
                        KonsiHeadTable.Rows.Add(tmpKonsiSAPRow)
                        PrintPDFKonsi(tmpSAPRow("ProdHBezeichnung").ToString, stemp)
                        bKonsiPrint = bKonsi
                    Else
                        mObjInventur.FilepathUtsch = Nothing
                        Session("App_FilepathUtsch") = Nothing
                    End If
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
            Dim sPathUtsch As String
            filesByte = New List(Of Byte())()

            For Each sFile As String In files
                filesByte.Add(File.ReadAllBytes(sFile))
            Next
            If bKonsiPrint = True Then
                For Each sFile As String In filesUtsch
                    filesByteUtsch.Add(File.ReadAllBytes(sFile))
                Next
                If mObjInventur.EndInventur = False Then
                    sPathUtsch = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\TestInventurUtsch.pdf"
                Else
                    sPathUtsch = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\InventurUtsch.pdf"
                End If
                File.WriteAllBytes(sPathUtsch, PdfMerger.MergeFiles(filesByteUtsch))
                mObjInventur.FilepathUtsch = sPathUtsch
            End If

            If mObjInventur.EndInventur = False Then
                sPath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\TestInventur.pdf"
            Else
                sPath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\Inventur.pdf"
            End If
            If files.Length > 0 Then
                File.WriteAllBytes(sPath, PdfMerger.MergeFiles(filesByte))
                mObjInventur.Filepath = sPath
            End If
            Session("App_ContentType") = "Application/pdf"

            If mObjInventur.EndInventur = False Then
                Session("mObjInventur") = mObjInventur
                Response.Redirect("Change09_3.aspx")
            Else
                mpeCheck.Show()
            End If

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

            mObjInventur.Filepath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\Inventur.pdf"
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

            mObjInventur.Filepath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\InventurUtsch.pdf"
            Dim docFactory As New DocumentGeneration.WordDocumentFactory(KonsiReportTable, imageHt)
            docFactory.CreateDocumentTable(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\" & sFilePath, Page, "\Vorlagen\InventurKonsi.doc", KonsiHeadTable)
            KonsiReportTable.Rows.Clear()

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Private Sub lbOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbOk.Click
        Try
            mObjInventur.SetInventurEndERP()
            If mObjInventur.E_SUBRC > 0 Then
                lblNoData.Visible = True
                lblNoData.Text = "Fehler beim Abschließen der Inventur: " + mObjInventur.E_MESSAGE
                Session("mObjInventur") = mObjInventur
                Exit Sub
            Else
                lbAbsenden.Enabled = False
                lblNoData.Visible = True
                lblNoData.Text = "Inventur erfolgreich abgeschlossen!"
                Session("App_ContentType") = "Application/pdf"
                Session("App_Filepath") = mObjInventur.Filepath
                If (Not ClientScript.IsStartupScriptRegistered("Enabled")) Then

                    Session("App_FilepathUtsch") = mObjInventur.FilepathUtsch
                    Dim sb As StringBuilder = New StringBuilder()
                    sb.Append("<script type=""text/javascript"">var newWind;")
                    If Not mObjInventur.FilepathUtsch Is Nothing Then
                        sb.Append("newWind=window.open(""PrintPDF2.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf)
                    End If
                    If Not mObjInventur.Filepath Is Nothing Then
                        sb.Append("window.open(""Printpdf.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf)
                    End If
                    sb.Append("</script>")
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Enabled", sb.ToString())

                End If
                mObjInventur.EndInventur = True
                Session("mObjInventur") = mObjInventur
                lbAbsenden.Enabled = False
            End If
        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Abschließen der Inventur: " + ex.Message
        End Try
    End Sub

End Class