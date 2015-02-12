Imports KBS.KBS_BASE
Imports KBSBase

Partial Public Class TaggleicheMeldungDAD
    Inherits Page

    Private mObjKasse As Kasse
    Dim objMeldungDAD As MeldungDAD

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

        If Not IsPostBack Then
            objMeldungDAD = New MeldungDAD(mObjKasse.Lagerort, mObjKasse.Lagerort)
            Session("objMeldungDAD") = objMeldungDAD

            lbtnGestern.Attributes.Add("onclick", "SetDate( -1,'" & txtZulassungsdatum.ClientID & "'); return false;")
            lbtnHeute.Attributes.Add("onclick", "SetDate( 0,'" & txtZulassungsdatum.ClientID & "'); return false;")
            lbtnMorgen.Attributes.Add("onclick", "SetDate( +1,'" & txtZulassungsdatum.ClientID & "'); return false;")

        ElseIf Session("objMeldungDAD") IsNot Nothing Then
            objMeldungDAD = CType(Session("objMeldungDAD"), MeldungDAD)

        End If
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles NewSearch.Click
        Panel1.Visible = Not Panel1.Visible
        cmdCreate.Visible = Not cmdCreate.Visible
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click, btnEmpty.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        lblError.Text = ""

        If String.IsNullOrEmpty(txtFahrgestellnummer.Text) AndAlso String.IsNullOrEmpty(txtBriefnummer.Text) Then
            lblError.Text = "Bitte geben Sie eine Fahrgestell- oder Briefnummer an!"
            Exit Sub
        End If

        objMeldungDAD.IDSuche = txtBarcode.Text.Trim()
        objMeldungDAD.FahrgestellnummerSuche = txtFahrgestellnummer.Text.Trim().ToUpper()
        objMeldungDAD.BriefnummerSuche = txtBriefnummer.Text.Trim().ToUpper()

        objMeldungDAD.LoadVorgang()

        Session("objMeldungDAD") = objMeldungDAD

        If objMeldungDAD.ErrorOccured Then
            lblError.Text = "Fehler: " & objMeldungDAD.ErrorMessage
        Else
            FillForm()
        End If
    End Sub

    Private Sub FillForm()
        Panel1.Visible = False
        cmdCreate.Visible = False

        lblIDDisplay.Text = objMeldungDAD.ID
        lblBestellnummerDisplay.Text = objMeldungDAD.Bestellnummer
        lblFahrgestellnummerDisplay.Text = objMeldungDAD.Fahrgestellnummer
        lblBriefnummerDisplay.Text = objMeldungDAD.Briefnummer

        If Not String.IsNullOrEmpty(objMeldungDAD.Zulassungsdatum) Then
            Dim tmpDate As String = objMeldungDAD.Zulassungsdatum
            txtZulassungsdatum.Text = tmpDate.Substring(0, 2) & tmpDate.Substring(3, 2) & tmpDate.Substring(8, 2)
        Else
            txtZulassungsdatum.Text = ""
        End If

        If Not String.IsNullOrEmpty(objMeldungDAD.Kennzeichen) Then
            If objMeldungDAD.Kennzeichen.Contains("-") Then
                Dim tmpKennz() As String = objMeldungDAD.Kennzeichen.Split("-"c)
                txtKennz1.Text = ""
                txtKennz2.Text = ""
                If tmpKennz.Length = 1 Then
                    txtKennz1.Text = tmpKennz(0)
                ElseIf tmpKennz.Length = 2 Then
                    txtKennz1.Text = tmpKennz(0)
                    txtKennz2.Text = tmpKennz(1)
                ElseIf tmpKennz.Length = 3 Then 'Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
                    txtKennz1.Text = tmpKennz(0)
                    txtKennz2.Text = tmpKennz(1) & "-" & tmpKennz(2)
                End If
            Else
                txtKennz1.Text = objMeldungDAD.Kennzeichen
                txtKennz2.Text = ""
            End If
        Else
            txtKennz1.Text = ""
            txtKennz2.Text = ""
        End If

        txtGebuehr.Text = ""
        cbxAuslieferung.Checked = False
        txtFrachtbriefnummer.Text = objMeldungDAD.Frachtbriefnummer

        Panel2.Visible = True
        cmdSend.Visible = True
    End Sub

    Protected Sub cmdFrachtbriefnummer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdFrachtbriefnummer.Click
        trFrachtbriefnummer.Visible = True
    End Sub

    Protected Sub cmdSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSend.Click, btnEmpty2.Click
        SaveMeldung()
    End Sub

    Private Sub SaveMeldung()
        Try
            If String.IsNullOrEmpty(txtZulassungsdatum.Text) Then
                lblError.Text = "Bitte geben Sie ein Zulassungsdatum ein."
                Exit Sub
            End If
            objMeldungDAD.Zulassungsdatum = Common.toShortDateStr(txtZulassungsdatum.Text)
            If String.IsNullOrEmpty(objMeldungDAD.Zulassungsdatum) Then
                lblError.Text = "Bitte geben Sie ein gültiges Zulassungsdatum ein."
                Exit Sub
            End If

            If String.IsNullOrEmpty(txtKennz1.Text) OrElse String.IsNullOrEmpty(txtKennz2.Text) Then
                lblError.Text = "Bitte geben Sie ein vollständiges Kennzeichen ein."
                Exit Sub
            End If
            objMeldungDAD.Kennzeichen = txtKennz1.Text.ToUpper() & "-" & txtKennz2.Text.ToUpper()

            If Not String.IsNullOrEmpty(txtGebuehr.Text) AndAlso Not Common.IsDecimal(txtGebuehr.Text.Trim()) Then
                lblError.Text = "Bitte geben Sie einen Zahlenwert für die Gebühr ein."
                Exit Sub
            End If
            objMeldungDAD.Gebuehr = txtGebuehr.Text.Trim()

            objMeldungDAD.Auslieferung = cbxAuslieferung.Checked

            If (trFrachtbriefnummer.Visible) Then
                objMeldungDAD.Frachtbriefnummer = txtFrachtbriefnummer.Text
            End If

            objMeldungDAD.SaveVorgang()

            If objMeldungDAD.ErrorOccured Then
                lblError.Text = "Fehler beim Absenden: " & objMeldungDAD.ErrorMessage
            Else
                lblError.Text = "Die Meldung wurde erfolgreich abgesendet!"
                Panel2.Visible = False
                cmdSend.Visible = False
                Panel1.Visible = True
                objMeldungDAD.ClearFields()
                txtBarcode.Text = ""
                txtFahrgestellnummer.Text = ""
                txtBriefnummer.Text = ""
                trFrachtbriefnummer.Visible = False
                cmdCreate.Visible = True
            End If

            Session("objMeldungDAD") = objMeldungDAD

        Catch ex As Exception
            lblError.Text = "Fehler beim Absenden: " & ex.Message
        End Try
    End Sub

End Class