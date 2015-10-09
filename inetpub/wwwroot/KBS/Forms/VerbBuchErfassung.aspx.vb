Imports KBS.KBS_BASE

Partial Public Class VerbBuchErfassung
    Inherits Page

    Private mObjKasse As Kasse

    Private mObjVerbandbuch As ClsVerbandbuch

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)
        lblError.Text = ""

        'txtOrt.Text = mObjKasse.Lagerort.ToString()

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If
    End Sub

    Private Sub responseBack()
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

   
    Protected Sub lbWeiter_Click(sender As Object, e As EventArgs) Handles lbWeiter.Click

        If txtZeitUnfall.Text.Length <> 4 Or txtUhrHilfe.Text.Length <> 4 Then
            lblError.Text = "Die Zeiten haben ein falsches Format"
        Else
            If _
                Not txtZeitUnfall.Text = "" And Not txtDatHilfe.Text = "" And Not txtDatUnfall.Text = "" And
                Not txtErsteHilfe.Text = "" And Not txtHergang.Text = "" And Not txtNameVerl.Text = "" And
                Not txtOrt.Text = "" And Not txtDatHilfe.Text = "" And Not txtZeitUnfall.Text = "" And
                Not txtErsteHilfe.Text = "" And Not txtNameZeug.Text = "" And Not txtNameErstHelf.Text = "" Then

                mObjVerbandbuch = New ClsVerbandbuch()
                mObjVerbandbuch.VKBUR = mObjKasse.Lagerort
                mObjVerbandbuch.DatumHilfe = txtDatUnfall.Text
                mObjVerbandbuch.DatumUnfall = txtDatHilfe.Text
                mObjVerbandbuch.Hilfeleistung = txtErsteHilfe.Text
                mObjVerbandbuch.NameHelf = txtNameErstHelf.Text
                mObjVerbandbuch.NameVerl = txtNameVerl.Text
                mObjVerbandbuch.NameZeug = txtNameZeug.Text
                mObjVerbandbuch.Verletzung = txtVerletzung.Text
                mObjVerbandbuch.Ort = txtOrt.Text
                mObjVerbandbuch.UnfallHer = txtHergang.Text
                mObjVerbandbuch.ZeitHilf = txtUhrHilfe.Text.Insert(4, "00")
                mObjVerbandbuch.ZeitUnf = txtZeitUnfall.Text.Insert(4, "00")

                mObjVerbandbuch.SaveSAP()

                If mObjVerbandbuch.ErrorOccured Then
                    lblError.Text = "Da ist etwas schiefgelaufen. Wir bitten dies zu entschuldigen."
                Else
                    ClearMask()
                    lblError.Text = "Eintrag wurde gespeichert."
                    lblError.ForeColor = Drawing.Color.Chartreuse
                End If

            Else
                lblError.Text = "Alle Eingaben müssen vollständig sein"

            End If
        End If
    End Sub

    Private Sub ClearMask()

        txtDatUnfall.Text = ""
        txtDatHilfe.Text = ""
        txtErsteHilfe.Text = ""
        txtNameErstHelf.Text = ""
        txtNameVerl.Text = ""
        txtNameZeug.Text = ""
        txtVerletzung.Text = ""
        txtOrt.Text = ""
        txtHergang.Text = ""
        txtUhrHilfe.Text = ""
        txtZeitUnfall.Text = ""

    End Sub

End Class
