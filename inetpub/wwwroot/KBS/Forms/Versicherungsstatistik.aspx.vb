Imports KBS.KBS_BASE

Public Class Versicherungsstatistik
    Inherits Page

    Dim mobjKasse As Kasse
    Dim mobjVerStat As VersicherungsStatistiken

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)

        lblError.Text = ""
        Title = lblHead.Text

        If Not Session("mKasse") Is Nothing Then
            mobjKasse = Session("mKasse")
        End If

        If Not IsPostBack Then
            Session("mobjVerStat") = Nothing
        End If

        If Not Session("mobjVerStat") Is Nothing Then
            mobjVerStat = Session("mobjVerStat")
        Else
            mobjVerStat = New VersicherungsStatistiken
            Session("mobjVerStat") = mobjVerStat
        End If

        If Not IsPostBack Then
            txtDatumVon.Text = Today.AddDays(-1).ToShortDateString
            txtDatumBis.Text = Today.AddDays(-1).ToShortDateString
        End If

    End Sub

    Protected Sub ibtnFilter_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnFilter.Click
        If ValidateFilter() Then
            Dim tblFiltered As DataTable = Nothing

            Select Case ddlFilter.SelectedValue
                Case "alle"
                    tblFiltered = mobjVerStat.GetVersicherungen(mobjKasse.Lagerort, mobjKasse.Werk, txtDatumVon.Text, txtDatumBis.Text)
                Case "Artikel-Nr."
                    tblFiltered = mobjVerStat.GetVersicherungen(mobjKasse.Lagerort, mobjKasse.Werk, txtDatumVon.Text, txtDatumBis.Text, , ddlArtikel.SelectedValue)
                Case "EVB"
                    If txtEVB.Text <> "" Then
                        txtEVB.Text = txtEVB.Text.ToUpper
                        tblFiltered = mobjVerStat.GetVersicherungen(mobjKasse.Lagerort, mobjKasse.Werk, txtDatumVon.Text, txtDatumBis.Text, , , txtEVB.Text)
                    Else
                        lblError.Text = "Der Filterwert darf nicht leer sein."
                    End If
            End Select

            If mobjVerStat.ErrorOccured() Then
                lblError.Text = mobjVerStat.ErrorMessage

            ElseIf lblError.Text <> "" Then

            Else
                gvVersicherungen.DataSource = tblFiltered
                gvVersicherungen.DataBind()
                dataHeader.Visible = True

                If Not ibtnNoFilter.Visible Then
                    ibtnNoFilter.Visible = True
                End If
            End If
        End If
    End Sub

    Protected Sub ibtnNoFilter_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnNoFilter.Click
        txtEVB.Visible = False
        ibtnFilter.Visible = False
        ibtnNoFilter.Visible = False
        ddlArtikel.Visible = False
        cmdCreate.Visible = True

        ddlFilter.SelectedValue = "alle"
        cmdCreate_Click(cmdCreate, New EventArgs())
    End Sub

    Protected Sub cmdCreate_Click(sender As Object, e As EventArgs) Handles cmdCreate.Click
        If ValidateFilter() Then
            Dim tblFiltered As DataTable = mobjVerStat.GetVersicherungen(mobjKasse.Lagerort, mobjKasse.Werk, txtDatumVon.Text, txtDatumBis.Text)

            If mobjVerStat.ErrorOccured() Then
                lblError.Text = mobjVerStat.ErrorMessage

                gvVersicherungen.DataSource = Nothing
                gvVersicherungen.DataBind()
                dataHeader.Visible = False
            Else
                gvVersicherungen.DataSource = tblFiltered
                gvVersicherungen.DataBind()
                dataHeader.Visible = True
            End If
        End If
    End Sub

    Protected Sub ddlFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFilter.SelectedIndexChanged
        Select Case ddlFilter.SelectedValue
            Case "alle"
                txtEVB.Visible = False
                ibtnFilter.Visible = False
                ddlArtikel.Visible = False
                cmdCreate.Visible = True
            Case "Artikel-Nr."
                txtEVB.Visible = False
                ibtnFilter.Visible = True
                ddlArtikel.Visible = True
                cmdCreate.Visible = False

                FillFilter()
            Case "EVB"
                txtEVB.Visible = True
                ibtnFilter.Visible = True
                ddlArtikel.Visible = False
                cmdCreate.Visible = False
        End Select
    End Sub

    ''' <summary>
    ''' Füllt die Filterliste
    ''' </summary>
    Private Sub FillFilter()

        ddlArtikel.Items.Clear()

        If mobjVerStat.FilterListe Is Nothing Then
            ddlArtikel.Items.Add("Keine Werte vorhanden")
        ElseIf mobjVerStat.FilterListe.Count = 0 Then
            ddlArtikel.Items.Add("Keine Werte vorhanden")
        Else
            ddlArtikel.DataSource = mobjVerStat.FilterListe
            ddlArtikel.DataTextField = "Materialkurztext"
            ddlArtikel.DataValueField = "Materialnummer"
            ddlArtikel.DataBind()
        End If

    End Sub

    ''' <summary>
    ''' Prüft die Filterkriterien der Eingabefelder ab
    ''' </summary>
    ''' <returns><code>True</code> wenn alle Filter erfüllt, sonst <code>False</code></returns>
    Private Function ValidateFilter() As Boolean
        If txtDatumVon.Text = "" Then
            lblError.Text = "Geben Sie ein gültiges Von Datum ein!"
        ElseIf txtDatumBis.Text = "" Then
            lblError.Text = "Geben Sie ein gültiges Bis Datum ein!"
        ElseIf CDate(txtDatumBis.Text) < CDate(txtDatumVon.Text) Then
            lblError.Text = "Datum Von muss kleiner sein als Datum Bis!"
        ElseIf CDate(txtDatumBis.Text) >= Today Or CDate(txtDatumVon.Text) >= Today Then
            lblError.Text = "Hinweis: Daten aktueller als vom " & Today.AddDays(-1).ToShortDateString() & " sind noch nicht abrufbar!"
        ElseIf (CDate(txtDatumBis.Text).Ticks - CDate(txtDatumVon.Text).Ticks) > New TimeSpan(90, 0, 0, 0).Ticks Then
            lblError.Text = "Die maximal auszuwertende Zeitspanne beträgt 90 Tage!"
        Else
            Return True
        End If

        Return False
    End Function

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

End Class