Imports KBS.KBS_BASE
Partial Public Class Report01
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjoffBestellungen As offBestellungen

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

        If mObjoffBestellungen Is Nothing Then
            mObjoffBestellungen = mObjKasse.offBestellung(Me)
        End If

        If Not IsPostBack Then
            mObjoffBestellungen.currentApplikationPage = Me
            mObjoffBestellungen.KostStelle = mObjKasse.Lagerort
            mObjoffBestellungen.getErwarteteLieferungenFromSAPERP()

            If mObjoffBestellungen.ErrorOccured Then
                Select Case mObjoffBestellungen.ErrorCode
                    Case "-1"
                        lblError.Text = mObjoffBestellungen.ErrorMessage
                    Case Else
                        lblError.Text = "Es ist ein Fehler aufgetreten <br> " & mObjoffBestellungen.ErrorMessage
                End Select
                lbWeiter.Visible = False

            Else
                lbWeiter.Visible = True
                lbxBestellungen.DataSource = New DataView(mObjoffBestellungen.ErwarteteLieferungen)
                lbxBestellungen.DataBind()
            End If
        End If
    End Sub

    Private Sub responseBack()
        mObjoffBestellungen.currentApplikationPage = Nothing
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

        mObjoffBestellungen.BestellnummerSelection = txtBestellnummer.Text.Replace(" ", "")
        mObjoffBestellungen.LieferantSelection = txtLieferantName.Text.Replace(" ", "")

        Dim tmpDataView As DataView = New DataView(mObjoffBestellungen.ErwarteteLieferungen)
        tmpDataView.RowFilter = mObjoffBestellungen.SelectionString

        lbxBestellungen.DataSource = tmpDataView
        lbxBestellungen.DataBind()

    End Sub

    Protected Sub lbWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbWeiter.Click
        If Not lbxBestellungen.SelectedIndex = -1 Then

            Dim drow() As DataRow
            drow = mObjoffBestellungen.ErwarteteLieferungen.Select("Bestellnummer='" & lbxBestellungen.SelectedValue & "'")
            If drow.Length > 0 Then
                If drow(0)("IstUmlagerung").ToString = "X" Then
                    mObjoffBestellungen.IstUmlagerung = "X"
                    mObjoffBestellungen.UmlNummer = lbxBestellungen.SelectedValue
                    mObjoffBestellungen.getUmlPositionenFromSAPERP(lbxBestellungen.SelectedValue)
                ElseIf drow(0)("IstUmlagerung").ToString = "BANF" Then
                    mObjoffBestellungen.IstUmlagerung = "BANF"
                    mObjoffBestellungen.BANF = lbxBestellungen.SelectedValue
                    mObjoffBestellungen.getBANFPositionenFromSAPERP()
                Else
                    mObjoffBestellungen.getLieferungsPositionenFromSAPERP(lbxBestellungen.SelectedValue)
                End If
            Else
                lblError.Text = "Fehler beim lesen der Bestellungdetails!"
            End If

            If mObjoffBestellungen.ErrorOccured Then
                Select Case mObjoffBestellungen.ErrorCode
                    Case "-1"
                        lblError.Text = mObjoffBestellungen.ErrorMessage
                    Case Else
                        lblError.Text = "Es ist ein Fehler aufgetreten: <br>" & mObjoffBestellungen.ErrorMessage
                End Select
            Else
                mObjoffBestellungen.LiefantAnzeige = lbxBestellungen.SelectedItem.Text

                mObjoffBestellungen.currentApplikationPage = Nothing
                Response.Redirect("Report01_2.aspx")
            End If

        Else
            lblError.Text = "Wählen Sie bitte eine Bestellung aus!"
        End If
    End Sub

    Private Sub Report01_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        lblBestellungsAnzahl.Text = lbxBestellungen.Items.Count.ToString
    End Sub

End Class