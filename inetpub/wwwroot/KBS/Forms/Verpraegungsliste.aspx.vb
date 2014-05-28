Imports System.Globalization
Imports KBS.KBS_BASE
Imports System.Net

Public Class Verpraegungsliste
    Inherits Page

    Dim mobjKasse As Kasse

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)

        lblError.Text = ""
        Title = lblHead.Text

        If Not Session("mKasse") Is Nothing Then
            mobjKasse = Session("mKasse")
        End If

    End Sub

    Protected Sub cmdCreate_Click(sender As Object, e As EventArgs) Handles cmdCreate.Click

        If ValidateInput() Then

            Try

                Dim vonDatum As DateTime = DateTime.ParseExact(txtDatumVon.Text, "ddMMyy", CultureInfo.CurrentCulture, DateTimeStyles.None)
                Dim bisDatum As DateTime = DateTime.ParseExact(txtDatumBis.Text, "ddMMyy", CultureInfo.CurrentCulture, DateTimeStyles.None)

                Dim serverAddress As String = "http://192.168.10.9"
 
                Dim templateString = "/ReportServer?/public/Liste_der_Verpraegungen&rs:Format=PDF&FromCostCenter={0}&ToCostCenter={1}&VoucherNumber={2}&FromDate={3}&TillDate={4}"

                Dim fullLink As String = serverAddress & String.Format(templateString,
                    mobjKasse.Lagerort, mobjKasse.Lagerort, txtBelegnummer.Text, vonDatum.ToString("dd/MM/yyyy").Replace("."c, "/"c), bisDatum.ToString("dd/MM/yyyy").Replace("."c, "/"c))

                Dim downloadPath As New Uri(fullLink)
                Dim fileName As String = "Verpraegungsliste_" & mobjKasse.Lagerort & "_" & Now.ToString("yyyyMMddHHmmss") & ".pdf"
                Dim savePath As String = ConfigurationManager.AppSettings("LocalDocumentsPath") & "Verpraegungsliste\" & fileName

                'PDF vom Reporting Server herunterladen
                Dim client As New WebClient()
                client.Credentials = New NetworkCredential("SRV_EFA", "Kasse4Kasse", "KROSCHKE")
                client.DownloadFile(downloadPath, savePath)

                'PDF an Client senden
                Response.Clear()
                Response.ClearContent()
                Response.ClearHeaders()
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
                Response.WriteFile(savePath)
                Response.End()

            Catch ex As Exception
                lblError.Text = "Fehler beim Abruf der Verprägungsliste: " & ex.Message
            End Try

        End If

    End Sub

    Private Function ValidateInput() As Boolean
        Dim vonDat As DateTime
        Dim bisDat As DateTime

        If String.IsNullOrEmpty(txtBelegnummer.Text) OrElse txtBelegnummer.Text = "0" Then
            lblError.Text = "Geben Sie eine Belegnummer > 0 ein!"
            Return False
        End If

        If String.IsNullOrEmpty(txtDatumVon.Text) OrElse Not DateTime.TryParseExact(txtDatumVon.Text, "ddMMyy", CultureInfo.CurrentCulture, DateTimeStyles.None, vonDat) Then
            lblError.Text = "Geben Sie ein gültiges Von Datum ein!"
            Return False
        End If
        If String.IsNullOrEmpty(txtDatumBis.Text) OrElse Not DateTime.TryParseExact(txtDatumBis.Text, "ddMMyy", CultureInfo.CurrentCulture, DateTimeStyles.None, bisDat) Then
            lblError.Text = "Geben Sie ein gültiges Bis Datum ein!"
            Return False
        End If
        If bisDat < vonDat Then
            lblError.Text = "Datum Von muss kleiner sein als Datum Bis!"
            Return False
        End If

        Return True

    End Function

End Class