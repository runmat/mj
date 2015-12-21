Imports KBS.KBS_BASE
Imports System.IO

Partial Public Class Change11
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjInvSonstigeLw As InventurSonstigeLagerware

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)

        Title = lblHead.Text
        If mObjKasse Is Nothing Then
            If Session("mKasse") IsNot Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        If mObjInvSonstigeLw Is Nothing Then
            If Session("objInvSonstigeLw") IsNot Nothing Then
                mObjInvSonstigeLw = CType(Session("objInvSonstigeLw"), InventurSonstigeLagerware)
            Else
                mObjInvSonstigeLw = New InventurSonstigeLagerware(mObjKasse.Lagerort)
                mObjInvSonstigeLw.FillArtikelliste()
                Session("objInvSonstigeLw") = mObjInvSonstigeLw
            End If
        End If

        If Not IsPostBack Then
            FillGrid2()

            If File.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\InventurLagerware.pdf") Then
                Session("App_Filepath") = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\InventurLagerware"
                lbNachdruck.Visible = True
            End If
        End If

    End Sub

    Private Sub FillGrid2()
        Dim tmpDataView As New DataView(mObjInvSonstigeLw.Artikelliste)

        tmpDataView.Sort = "Position"
        GridView2.DataSource = tmpDataView
        GridView2.DataBind()
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        lblNoData.Text = ""
        checkgridview2()
        FillGrid()
        mpeCheck.Show()
    End Sub

    Private Sub checkGrid()
        For Each row As GridViewRow In GridView1.Rows
            Dim sMenge As String
            sMenge = row.Cells(1).Text
            If Not IsNumeric(sMenge) OrElse CInt(sMenge) <= 0 Then
                row.Cells(1).ForeColor = Drawing.Color.Red
            End If
        Next
    End Sub

    Private Sub FillGrid()
        Dim tmpDataView As New DataView(mObjInvSonstigeLw.Artikelliste)

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
        Else
            GridView1.Visible = True
            lblNoData.Visible = False

            tmpDataView.Sort = "Position"
            GridView1.DataSource = tmpDataView
            GridView1.DataBind()
            checkGrid()
        End If
    End Sub

    Private Sub CreateExcelFile()
        Dim sFileName As String
        ' Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".csv"
        sFileName = "C:\Inetpub\wwwroot\KBS\temp\Excel\" & Format(Now, "yyyyMMdd_HHmmss_") & mObjKasse.Lagerort & ".xls"
        Dim docFactory As New DocumentGeneration.ExcelDocumentFactory()
        docFactory.CreateDocumentAndWriteToFilesystem(sFileName, mObjInvSonstigeLw.Artikelliste, Page)
        Session("Filename") = sFileName

    End Sub

    Private Sub PrintPDF()
        Try

            Dim headTable = New DataTable("Kopf")
            headTable.Columns.Add("Kostenstelle", GetType(System.String))
            headTable.Columns.Add("Datum", GetType(System.String))

            Dim tmpSAPRow As DataRow
            tmpSAPRow = headTable.NewRow
            tmpSAPRow("Kostenstelle") = mObjKasse.Lagerort
            tmpSAPRow("Datum") = Now.ToShortDateString
            headTable.Rows.Add(tmpSAPRow)

            Dim ReportTable = New DataTable
            ReportTable.TableName = "Inventur"
            ReportTable.Columns.Add("Artikel", GetType(System.String))
            ReportTable.Columns.Add("Menge", GetType(System.String))
            For Each PHRow As DataRow In mObjInvSonstigeLw.Artikelliste.Rows
                'leere Zeilen überspringen
                If PHRow("Artikel").ToString.Length > 0 Then
                    Dim tmpSAPRow2 = ReportTable.NewRow
                    tmpSAPRow2("Artikel") = PHRow("Artikel").ToString
                    tmpSAPRow2("Menge") = PHRow("Menge").ToString
                    ReportTable.Rows.Add(tmpSAPRow2)
                End If
            Next

            Dim imageHt As New Hashtable()

            If Not Directory.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort) Then
                Directory.CreateDirectory(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort)
            End If
            Dim sFilePath As String
            sFilePath = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\InventurLagerware"
            Dim docFactory As New DocumentGeneration.WordDocumentFactory(ReportTable, imageHt)
            docFactory.CreateDocumentTable(sFilePath, Page, "\Vorlagen\InventurLagerware.doc", headTable)
            ReportTable.Rows.Clear()

            PrintNow()

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Erstellen des PDF-Dokuments: " + ex.Message
        End Try
    End Sub

    Private Sub PrintNow()
        Session("App_ContentType") = "Application/pdf"
        Session("App_Filepath") = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Inventur\" & mObjKasse.Lagerort & "\InventurLagerware.pdf"
        If (Not ClientScript.IsStartupScriptRegistered("Enabled")) Then

            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("<script type=""text/javascript"">")
            sb.Append("window.open(""Printpdf.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf)
            sb.Append("</script>")
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Enabled", sb.ToString())

        End If
    End Sub

    Protected Sub lbNachdruck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbNachdruck.Click
        PrintNow()
    End Sub

    Private Sub Sendmail()
        Dim Mail As Net.Mail.MailMessage
        Dim filename As String
        Dim file As Net.Mail.Attachment

        Try
            Dim clsMail As New ComCommon(mObjKasse)

            clsMail.LeseMailTexte("1")

            Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
            Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")

            Dim Adressen() As String
            If Split(clsMail.MailAdress.Trim, ";").Length > 1 Then

                Mail = New Net.Mail.MailMessage()

                Dim Mailsender As New Net.Mail.MailAddress(smtpMailSender)

                Mail.Sender = Mailsender
                Mail.From = Mailsender
                Mail.Body = "Inventur Lagerware für Lagerort " & mObjKasse.Lagerort

                Mail.Subject = clsMail.Betreff
                Adressen = Split(clsMail.MailAdress.Trim, ";")
                For Each tmpStr As String In Adressen
                    Mail.To.Add(tmpStr)
                Next
            Else
                Mail = New Net.Mail.MailMessage(smtpMailSender, clsMail.MailAdress.Trim, clsMail.Betreff, "Inventur Lagerware für Lagerort " & mObjKasse.Lagerort)
            End If

            If Split(clsMail.MailAdressCC.Trim, ";").Length > 1 Then
                Adressen = Split(clsMail.MailAdressCC.Trim, ";")
                For Each tmpStr As String In Adressen
                    Mail.CC.Add(tmpStr)
                Next
            Else
                If clsMail.MailAdressCC.Length > 0 Then
                    Mail.CC.Add(clsMail.MailAdressCC)
                End If
            End If

            Mail.IsBodyHtml = True
            Dim client As New Net.Mail.SmtpClient(smtpMailServer)

            filename = Session("Filename").ToString
            file = New Net.Mail.Attachment(filename)

            Mail.Attachments.Add(file)
            client.Send(Mail)
            Mail.Attachments.Dispose()
            Mail.Dispose()
            file = Nothing

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Versenden der Exceldatei!" + ex.Message
        End Try
    End Sub

    Private Sub lbOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbOk.Click
        mpeCheck.Hide()
        CreateExcelFile()
        Sendmail()
        PrintPDF()
        mObjInvSonstigeLw.FillArtikelliste()
        Session("objInvSonstigeLw") = mObjInvSonstigeLw
        FillGrid2()
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Private Sub checkgridview2()
        For Each drow As GridViewRow In GridView2.Rows
            Dim tblRow As DataRow
            Dim posNr As Int32
            posNr = CInt(drow.Cells(0).Text)
            Dim txtArtikelBox As TextBox
            Dim txtMengeBox As TextBox
            txtArtikelBox = CType(drow.Cells(1).FindControl("txtArtikel"), TextBox)
            txtMengeBox = CType(drow.Cells(1).FindControl("txtMenge"), TextBox)

            If txtArtikelBox.Text.Trim.Length > 0 Then
                tblRow = mObjInvSonstigeLw.Artikelliste.Select("Position=" & posNr)(0)
                tblRow("Artikel") = txtArtikelBox.Text.Trim
                If txtMengeBox.Text.Trim.Length > 0 Then
                    tblRow("Menge") = txtMengeBox.Text.Trim
                Else
                    tblRow("Menge") = "0"
                End If
            End If
        Next
        mObjInvSonstigeLw.Artikelliste.AcceptChanges()
        Session("objInvSonstigeLw") = mObjInvSonstigeLw
    End Sub
    
End Class