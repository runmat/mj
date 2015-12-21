Imports KBS.KBS_BASE
Imports System.IO

Partial Public Class Change10
    Inherits Page

    Private mObjKasse As Kasse
    Private ReportTable As DataTable

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

        If Not IsPostBack Then
            If File.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Wechselgeld\" & mObjKasse.Lagerort & "\Wechselgeld.pdf") Then
                Session("App_Filepath") = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Wechselgeld\" & mObjKasse.Lagerort & "\Wechselgeld"
                lbNachdruck.Visible = True

            End If
        End If

        addAttributes()
    End Sub

    Private Sub PrintPDF()

        Try

            Dim imageHt As New Hashtable()
            Dim sFilePath As String = mObjKasse.Lagerort + "\Wechselgeld"

            If Not Directory.Exists(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Wechselgeld\" & mObjKasse.Lagerort) Then
                Directory.CreateDirectory(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Wechselgeld\" & mObjKasse.Lagerort)
            End If

            Dim docFactory As New DocumentGeneration.WordDocumentFactory(ReportTable, imageHt)
            docFactory.CreateDocumentTable(ConfigurationManager.AppSettings("LocalDocumentsPath") & "Wechselgeld\" & sFilePath, Page, "\Vorlagen\Wechselgeld.doc", ReportTable)
            ReportTable.Rows.Clear()

            PrintNow()

        Catch ex As Exception
            lblNoData.Visible = True
            lblNoData.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        If txtDatum.Text.Length > 0 Then
            CreatePrintTable()
            PrintPDF()
        Else
            CreatePrintTable()
            lblError.Text = "Bitte geben Sie ein Datum ein!"
        End If
    End Sub

    Private Sub CreatePrintTable()
        ReportTable = New DataTable
        ReportTable.TableName = "Zaehlung"
        ReportTable.Columns.Add("Datum", GetType(System.String))
        ReportTable.Columns.Add("Schein500", GetType(System.String))
        ReportTable.Columns.Add("Schein200", GetType(System.String))
        ReportTable.Columns.Add("Schein100", GetType(System.String))
        ReportTable.Columns.Add("Schein50", GetType(System.String))
        ReportTable.Columns.Add("Schein20", GetType(System.String))
        ReportTable.Columns.Add("Schein10", GetType(System.String))
        ReportTable.Columns.Add("Schein5", GetType(System.String))
        ReportTable.Columns.Add("Stueck5", GetType(System.String))
        ReportTable.Columns.Add("Stueck2", GetType(System.String))
        ReportTable.Columns.Add("Stueck1", GetType(System.String))
        ReportTable.Columns.Add("Stueck050", GetType(System.String))
        ReportTable.Columns.Add("Stueck020", GetType(System.String))
        ReportTable.Columns.Add("Stueck010", GetType(System.String))
        ReportTable.Columns.Add("Stueck005", GetType(System.String))
        ReportTable.Columns.Add("Stueck002", GetType(System.String))
        ReportTable.Columns.Add("Stueck001", GetType(System.String))

        ReportTable.Columns.Add("Schein500Betrag", GetType(System.String))
        ReportTable.Columns.Add("Schein200Betrag", GetType(System.String))
        ReportTable.Columns.Add("Schein100Betrag", GetType(System.String))
        ReportTable.Columns.Add("Schein50Betrag", GetType(System.String))
        ReportTable.Columns.Add("Schein20Betrag", GetType(System.String))
        ReportTable.Columns.Add("Schein10Betrag", GetType(System.String))
        ReportTable.Columns.Add("Schein5Betrag", GetType(System.String))
        ReportTable.Columns.Add("Stueck5Betrag", GetType(System.String))
        ReportTable.Columns.Add("Stueck2Betrag", GetType(System.String))
        ReportTable.Columns.Add("Stueck1Betrag", GetType(System.String))
        ReportTable.Columns.Add("Stueck050Betrag", GetType(System.String))
        ReportTable.Columns.Add("Stueck020Betrag", GetType(System.String))
        ReportTable.Columns.Add("Stueck010Betrag", GetType(System.String))
        ReportTable.Columns.Add("Stueck005Betrag", GetType(System.String))
        ReportTable.Columns.Add("Stueck002Betrag", GetType(System.String))
        ReportTable.Columns.Add("Stueck001Betrag", GetType(System.String))

        ReportTable.Columns.Add("Gesamt", GetType(System.String))
        ReportTable.Columns.Add("Kostenstelle", GetType(System.String))

        Dim reportRow As DataRow = ReportTable.NewRow

        reportRow("Datum") = txtDatum.Text
        reportRow("Schein500") = txtSchein500.Text
        reportRow("Schein200") = txtSchein200.Text
        reportRow("Schein100") = txtSchein100.Text
        reportRow("Schein50") = txtSchein50.Text
        reportRow("Schein20") = txtSchein20.Text
        reportRow("Schein10") = txtSchein10.Text

        reportRow("Schein5") = txtSchein5.Text
        reportRow("Stueck2") = txtStueck2.Text
        reportRow("Stueck1") = txtStueck1.Text
        reportRow("Stueck050") = txtStueck050.Text
        reportRow("Stueck020") = txtStueck020.Text
        reportRow("Stueck010") = txtStueck010.Text
        reportRow("Stueck005") = txtStueck005.Text
        reportRow("Stueck002") = txtStueck002.Text
        reportRow("Stueck001") = txtStueck001.Text

        reportRow("Schein500Betrag") = lblSchein500.Text
        reportRow("Schein200Betrag") = lblSchein200.Text
        reportRow("Schein100Betrag") = lblSchein100.Text
        reportRow("Schein50Betrag") = lblSchein50.Text
        reportRow("Schein20Betrag") = lblSchein20.Text
        reportRow("Schein10Betrag") = lblSchein10.Text
        reportRow("Schein5Betrag") = lblSchein5.Text
        reportRow("Stueck2Betrag") = lblStueck2.Text
        reportRow("Stueck1Betrag") = lblStueck1.Text
        reportRow("Stueck050Betrag") = lblStueck050.Text
        reportRow("Stueck020Betrag") = lblStueck020.Text
        reportRow("Stueck010Betrag") = lblStueck010.Text
        reportRow("Stueck005Betrag") = lblStueck005.Text
        reportRow("Stueck002Betrag") = lblStueck002.Text
        reportRow("Stueck001Betrag") = lblStueck001.Text

        reportRow("Gesamt") = lblGesamt.Text
        reportRow("Kostenstelle") = mObjKasse.Lagerort
        reportRow("Datum") = txtDatum.Text

        ReportTable.Rows.Add(reportRow)
    End Sub

    Private Sub addAttributes()
        txtSchein500.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit500.ClientID & _
                            "','" & txtSchein500.ClientID & "','" & lblSchein500.ClientID & "','" & lblGesamt.ClientID & "');")
        txtSchein200.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit200.ClientID & _
                    "','" & txtSchein200.ClientID & "','" & lblSchein200.ClientID & "','" & lblGesamt.ClientID & "');")
        txtSchein100.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit100.ClientID & _
                    "','" & txtSchein100.ClientID & "','" & lblSchein100.ClientID & "','" & lblGesamt.ClientID & "');")
        txtSchein50.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit50.ClientID & _
                    "','" & txtSchein50.ClientID & "','" & lblSchein50.ClientID & "','" & lblGesamt.ClientID & "');")
        txtSchein20.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit20.ClientID & _
                    "','" & txtSchein20.ClientID & "','" & lblSchein20.ClientID & "','" & lblGesamt.ClientID & "');")
        txtSchein10.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit10.ClientID & _
                    "','" & txtSchein10.ClientID & "','" & lblSchein10.ClientID & "','" & lblGesamt.ClientID & "');")
        txtSchein5.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit5.ClientID & _
                    "','" & txtSchein5.ClientID & "','" & lblSchein5.ClientID & "','" & lblGesamt.ClientID & "');")
        txtStueck2.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit2.ClientID & _
                    "','" & txtStueck2.ClientID & "','" & lblStueck2.ClientID & "','" & lblGesamt.ClientID & "');")
        txtStueck1.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit1.ClientID & _
                    "','" & txtStueck1.ClientID & "','" & lblStueck1.ClientID & "','" & lblGesamt.ClientID & "');")
        txtStueck050.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit050.ClientID & _
                    "','" & txtStueck050.ClientID & "','" & lblStueck050.ClientID & "','" & lblGesamt.ClientID & "');")
        txtStueck020.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit020.ClientID & _
                    "','" & txtStueck020.ClientID & "','" & lblStueck020.ClientID & "','" & lblGesamt.ClientID & "');")
        txtStueck010.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit010.ClientID & _
                    "','" & txtStueck010.ClientID & "','" & lblStueck010.ClientID & "','" & lblGesamt.ClientID & "');")
        txtStueck005.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit005.ClientID & _
                    "','" & txtStueck005.ClientID & "','" & lblStueck005.ClientID & "','" & lblGesamt.ClientID & "');")
        txtStueck002.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit002.ClientID & _
                    "','" & txtStueck002.ClientID & "','" & lblStueck002.ClientID & "','" & lblGesamt.ClientID & "');")
        txtStueck001.Attributes.Add("onchange", "javascript:Calculate('" & lblEinheit001.ClientID & _
                    "','" & txtStueck001.ClientID & "','" & lblStueck001.ClientID & "','" & lblGesamt.ClientID & "');")
    End Sub

    Protected Sub lbNachdruck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbNachdruck.Click
        PrintNow()
    End Sub

    Private Sub PrintNow()
        Session("App_ContentType") = "Application/pdf"
        Session("App_Filepath") = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Wechselgeld\" & mObjKasse.Lagerort & "\Wechselgeld.pdf"
        If (Not ClientScript.IsStartupScriptRegistered("Enabled")) Then

            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("<script type=""text/javascript"">")
            sb.Append("window.open(""Printpdf.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf)
            sb.Append("</script>")
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Enabled", sb.ToString())

        End If
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

End Class