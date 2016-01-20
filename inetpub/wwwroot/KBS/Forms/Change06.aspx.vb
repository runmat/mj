Imports KBS.KBS_BASE
Partial Public Class Change06
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjEinzahlungsbelege As Einzahlungsbelege

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        lblError.Text = ""
        Title = lblHead.Text
        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If
        If Not IsPostBack Then
            txtMenge.Text = "30"
        End If
        If mObjEinzahlungsbelege Is Nothing Then
            mObjEinzahlungsbelege = mObjKasse.Einzahlungsbelege(Me)
        End If
    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        mObjEinzahlungsbelege.KostStelle = mObjKasse.Lagerort
        mObjEinzahlungsbelege.Menge = txtMenge.Text
        mObjEinzahlungsbelege.ChangeERP()
        If mObjEinzahlungsbelege.ErrorOccured Then
            lblBestellMeldung.ForeColor = Drawing.Color.Red
            lblBestellMeldung.Text = "Ihre Bestellung ist fehlgeschlagen: <br><br> " & mObjEinzahlungsbelege.ErrorMessage
            MPEBestellResultat.Show()
        Else
            lblBestellMeldung.ForeColor = Drawing.Color.Green
            lblBestellMeldung.Text = "Ihre Bestellung war erfolgreich!"
            MPEBestellResultat.Show()
            mObjEinzahlungsbelege = Nothing
        End If
    End Sub

    Private Sub responseBack()
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Protected Sub lbBestellFinalize_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBestellFinalize.Click
        Response.Redirect("../Selection.aspx")
    End Sub

End Class