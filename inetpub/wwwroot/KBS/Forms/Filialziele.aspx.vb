Imports System
Imports KBS.KBS_BASE
Imports BusinessDateFunctions

''' <summary>
''' Report Filialziele Code behind
''' </summary>
Public Class Filialziele
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjFilialvergleich As Filialvergleich

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)

        lblError.Text = ""
        Title = lblHead.Text

        If Not Session("mKasse") Is Nothing Then
            mObjKasse = Session("mKasse")
        End If
        If Not Session("mObjFilialvergleich") Is Nothing Then
            mObjFilialvergleich = Session("mObjFilialvergleich")
        End If

        If Not IsPostBack Then
            txtDatum.Text = DateTime.Now.ToString("dd.MM.yyyy")

            Dim kalenderwoche As String = GetKalenderwocheAndYear(DateTime.Today)

            mObjFilialvergleich = New Filialvergleich()
            mObjFilialvergleich.getDataFromSAP(mObjKasse.Lagerort, "PK", kalenderwoche)
            If mObjFilialvergleich.ErrorText <> String.Empty Then
                lblError.Text = "Fehler beim Abruf der Daten aus SAP." & Environment.NewLine & "Fehlerdetails: " & mObjFilialvergleich.ErrorText
            End If

            Session("mObjFilialvergleich") = mObjFilialvergleich

            Title = lblHead.Text
            lblKostenstelle.Text = mObjKasse.Lagerort
            lblFilialname.Text = ""
            lblKW.Text = kalenderwoche.Substring(4, 2) & " " & kalenderwoche.Substring(0, 4)
            lblLFB.Text = mObjFilialvergleich.LFB

            FillForm()
        End If

        Session("LastPage") = Me
    End Sub

    Private Sub FillForm()
        txtRahmenquote.Text = mObjFilialvergleich.Rahmenquote.ToString("F2")
        FillGrids()
    End Sub

    Private Sub FillGrids()

        If gvFilialauswertung.Visible Then
            If mObjFilialvergleich.FilialauswertungTabelle.Rows.Count > 0 Then
                FillFilialauswertung()
            Else
                FillFilialauswertung(True)
                If lblError.Text <> "" Then
                    lblError.Text += Environment.NewLine
                    lblError.Text += "Für die Filialauswertung konnten keine Werte abgerufen werden."
                Else
                    lblError.Text = "Für die Filialauswertung konnten keine Werte abgerufen werden."
                End If
            End If
        Else
            If mObjFilialvergleich.FilialvergleichTabelle.Rows.Count > 0 Then
                FillFilialvergleich()
            Else
                FillFilialvergleich(True)
                If lblError.Text <> "" Then
                    lblError.Text += Environment.NewLine
                    lblError.Text += "Für den Filialvergleich konnten keine Werte abgerufen werden."
                Else
                    lblError.Text = "Für den Filialvergleich konnten keine Werte abgerufen werden."
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' Bindet Filialauswertungsdaten an das passende Gridview
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillFilialauswertung(Optional ByVal blnClearGrid As Boolean = False)
        If blnClearGrid Then
            gvFilialauswertung.DataSource = Nothing
        Else
            lblDatenstand.Text = mObjFilialvergleich.DatumFilialAuswertung
            gvFilialauswertung.DataSource = mObjFilialvergleich.FilialauswertungTabelle
        End If
        gvFilialauswertung.DataBind()
    End Sub

    ''' <summary>
    ''' Bindet Filialvergleichsdaten an das passende Gridview und führt daran optische Anpassungen durch
    ''' </summary>
    Private Sub FillFilialvergleich(Optional ByVal blnClearGrid As Boolean = False)

        If blnClearGrid Then
            gvFilialvergleich.DataSource = Nothing
            gvFilialvergleich.DataBind()
        Else
            lblDatenstand.Text = mObjFilialvergleich.DatumFilialVergleich

            gvFilialvergleich.DataSource = mObjFilialvergleich.FilialvergleichTabelle
            gvFilialvergleich.DataKeyNames = New String() {"RANG"}
            gvFilialvergleich.DataBind()

            ' # Zeilenfarbe bestimmen
            Dim myRang As String = gvFilialvergleich.DataKeys(0)("RANG").ToString()

            Dim Css As String = "ItemStyle"
            For Each row2 As GridViewRow In gvFilialvergleich.Rows

                If gvFilialvergleich.DataKeys(row2.RowIndex)("RANG").ToString() = myRang Then
                    row2.CssClass = Css
                    myRang = gvFilialvergleich.DataKeys(row2.RowIndex)("RANG").ToString()
                Else
                    If Css = "ItemStyle" Then
                        Css = "GridTableAlternate"
                    Else
                        Css = "ItemStyle"
                    End If

                    row2.CssClass = Css
                    myRang = gvFilialvergleich.DataKeys(row2.RowIndex)("RANG").ToString()
                End If
            Next
            ' #
        End If

    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lbFilialauswertung_Click(sender As Object, e As EventArgs) Handles lbFilialauswertung.Click
        gvFilialauswertung.Visible = True
        gvFilialvergleich.Visible = False

        If mObjFilialvergleich.FilialauswertungTabelle.Rows.Count > 0 Then
            FillFilialauswertung()
        Else
            lblError.Text = "Für die Filialauswertung konnten keine Werte abgerufen werden."
        End If

        lbFilialauswertung.CssClass = "TabButtonBig"
        lbFilialvergleich.CssClass = "TabButtonBig Active"
    End Sub

    Protected Sub lbFilialvergleich_Click(sender As Object, e As EventArgs) Handles lbFilialvergleich.Click
        gvFilialauswertung.Visible = False
        gvFilialvergleich.Visible = True

        If mObjFilialvergleich.FilialvergleichTabelle.Rows.Count > 0 Then
            FillFilialvergleich()
        Else
            lblError.Text = "Für den Filialvergleich konnten keine Werte abgerufen werden."
        End If

        lbFilialauswertung.CssClass = "TabButtonBig Active"
        lbFilialvergleich.CssClass = "TabButtonBig"
    End Sub

    Private Sub Filialziele_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        Session("mObjFilialvergleich") = mObjFilialvergleich
    End Sub

    Protected Sub txtDatum_TextChanged(sender As Object, e As EventArgs) Handles txtDatum.TextChanged
        Dim datum As DateTime
        If Not DateTime.TryParse(txtDatum.Text, datum) Then
            datum = DateTime.Today
        End If

        If datum.Date.Year < 2000 Then
            lblError.Text = "Jahr muss >= 2000 sein"
            Exit Sub
        End If

        If datum.Date > DateTime.Today.Date Then
            lblError.Text = "Das Auswertungsdatum darf nicht in der Zukunft liegen"
            Exit Sub
        End If

        Dim kalenderwoche As String = GetKalenderwocheAndYear(datum)

        mObjFilialvergleich.getDataFromSAP(mObjKasse.Lagerort, "PK", kalenderwoche)
        If mObjFilialvergleich.ErrorText <> String.Empty Then
            lblError.Text = "Fehler beim Abruf der Daten aus SAP." & Environment.NewLine & "Fehlerdetails: " & mObjFilialvergleich.ErrorText
        End If

        Session("mObjFilialvergleich") = mObjFilialvergleich

        lblKW.Text = kalenderwoche.Substring(4, 2) & " " & kalenderwoche.Substring(0, 4)

        FillForm()
    End Sub

    Private Function GetKalenderwocheAndYear(ByVal datum As DateTime) As String

        Dim erg As String

        Dim woche As Integer = KW_Berechnung.KwOfDate(datum)

        'Spezialfall berücksichtigen: Woche 1 beginnt im Dez.
        If woche = 1 AndAlso datum.Month = 12 Then
            erg = (datum.Year + 1).ToString() & woche.ToString("00")
        Else
            erg = datum.Year.ToString() & woche.ToString("00")
        End If

        Return erg

    End Function

End Class