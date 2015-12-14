Imports KBS.KBS_BASE

Partial Public Class Change02
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjWareneingangspruefung As Wareneingangspruefung

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)
        lblError.Text = ""

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        Title = lblHead.Text

        If mObjWareneingangspruefung Is Nothing Then
            mObjWareneingangspruefung = mObjKasse.Wareneingangspruefung(Me)
        End If

        If Not IsPostBack Then
            mObjWareneingangspruefung.currentApplikationPage = Me
            mObjWareneingangspruefung.getErwarteteLieferungenFromSAPERP()
            If mObjWareneingangspruefung.ErrorOccured Then
                lblError.Text = mObjWareneingangspruefung.ErrorMessage
                lbWeiter.Visible = False
            Else
                lbWeiter.Visible = True
                lbxBestellungen.DataSource = New DataView(mObjWareneingangspruefung.ErwarteteLieferungen)
                lbxBestellungen.DataBind()
            End If
        End If
    End Sub

    Private Sub responseBack()
        mObjWareneingangspruefung.currentApplikationPage = Nothing
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub RefillFillBestellungen(ByVal sender As Object, ByVal e As EventArgs) Handles txtBestellnummer.TextChanged, txtLieferantName.TextChanged
        '----------------------------------------------------------------------
        'Methode:      RefillFillBestellungen
        'Autor:         Julian Jung
        'Beschreibung:  Listbox der Bestellungen anhand der Suchkriterien neu befüllen
        'Erstellt am:   05.05.2009
        '----------------------------------------------------------------------

        mObjWareneingangspruefung.BestellnummerSelection = txtBestellnummer.Text.Replace(" ", "")
        mObjWareneingangspruefung.LieferantSelection = txtLieferantName.Text.Replace(" ", "")

        Dim tmpDataView As DataView = New DataView(mObjWareneingangspruefung.ErwarteteLieferungen)
        tmpDataView.RowFilter = mObjWareneingangspruefung.SelectionString

        lbxBestellungen.DataSource = tmpDataView
        lbxBestellungen.DataBind()

    End Sub

    Protected Sub lbWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbWeiter.Click
        If Not lbxBestellungen.SelectedIndex = -1 Then

            Dim drow() As DataRow
            drow = mObjWareneingangspruefung.ErwarteteLieferungen.Select("Bestellnummer='" & lbxBestellungen.SelectedValue & "'")
            If drow.Length > 0 Then
                If drow(0)("IstUmlagerung").ToString = "X" Then
                    mObjWareneingangspruefung.IstUmlagerung = "X"
                    mObjWareneingangspruefung.UmlNummer = lbxBestellungen.SelectedValue
                    mObjWareneingangspruefung.getUmlPositionenFromSAPERP(lbxBestellungen.SelectedValue)
                Else
                    mObjWareneingangspruefung.getLieferungsPositionenFromSAPERP(lbxBestellungen.SelectedValue)
                End If
            Else
                lblError.Text = "Fehler beim lesen der Bestellungdetails!"
            End If

            If mObjWareneingangspruefung.ErrorOccured Then
                lblError.Text = mObjWareneingangspruefung.ErrorMessage
            Else
                mObjWareneingangspruefung.LiefantAnzeige = lbxBestellungen.SelectedItem.Text

                mObjWareneingangspruefung.currentApplikationPage = Nothing
                Response.Redirect("Change02_1.aspx")
            End If
        Else
            lblError.Text = "Wählen Sie bitte eine Bestellung aus"
        End If
    End Sub

    Private Sub Change02_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        lblBestellungsAnzahl.Text = lbxBestellungen.Items.Count.ToString
    End Sub

End Class
