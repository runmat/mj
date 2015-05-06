Imports KBS.KBS_BASE

Partial Public Class Change08
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjCommon As ComCommon

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
        If Not Session("mObjCommon") Is Nothing Then
            mObjCommon = CType(Session("mObjCommon"), ComCommon)
        Else
            mObjCommon = mObjKasse.Neukunde(Me)
            mObjCommon.FillERP()
            Session("mObjCommon") = mObjCommon
        End If
        If Not IsPostBack Then
            fillDropdowns()
            txtMitarbeiter.Focus()
        End If

        Select Case mObjKasse.Werk
            Case 1010
                Dim c As Control = ParseControl("<script type='text/javascript'>" & _
                                    "var EinzugPath ='../Docs/Einzugsermächtigung_SEPA.pdf';" & "</script>")
                Controls.Add(c)
        End Select

        rbEinzugJa.Attributes.Add("onclick", "ShowEinzug()")
    End Sub

    Public Sub fillDropdowns()

        With mObjCommon

            Dim tmpItem As ListItem
            Dim i As Int32 = 0
            ddlBranche.Items.Clear()

            Do While i < .Branchen.Rows.Count
                tmpItem = New ListItem(.Branchen.Rows(i)("BRTXT").ToString, .Branchen.Rows(i)("BRSCH").ToString)
                ddlBranche.Items.Add(tmpItem)
                i += 1
            Loop

            ddLand.Items.Clear()
            i = 0
            Dim tmpDataview As DataView = .Laender.DefaultView
            tmpDataview.Sort = "LANDX"

            Do While i < tmpDataview.Count
                tmpItem = New ListItem(tmpDataview.Item(i)("LANDX").ToString, tmpDataview.Item(i)("LAND1").ToString)
                If tmpItem.Value.ToString = "DE" Then
                    tmpItem.Selected = True
                End If
                ddLand.Items.Add(tmpItem)

                i += 1

            Loop

            ddlFunktion.Items.Clear()
            i = 0
            tmpItem = New ListItem("- Bitte wählen -", "00")
            ddlFunktion.Items.Add(tmpItem)
            Do While i < .Funktionen.Rows.Count
                tmpItem = New ListItem(.Funktionen.Rows(i)("VTEXT").ToString, .Funktionen.Rows(i)("PAFKT").ToString)
                ddlFunktion.Items.Add(tmpItem)
                i += 1
            Loop

        End With

    End Sub

    Protected Sub lbAbsenden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAbsenden.Click
        ClearErrors()
        If ValidateInput() Then
            lblError.Visible = True
            lblError.Text = "Achtung! Es fehlen Angaben. Bitte die markierten Positionen bearbeiten."

        Else
            If ddLand.SelectedValue <> "DE" AndAlso rbLieferscheinKunde.Checked = True Then
                ddLand.BorderStyle = BorderStyle.Solid
                ddLand.BorderWidth = 1
                ddLand.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
                rbLieferscheinKunde.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
                rbLieferscheinKunde.BorderStyle = BorderStyle.Solid
                rbLieferscheinKunde.BorderWidth = 1
                lblError.Visible = True
                lblError.Text = "Achtung! Bitte die markierten Positionen bearbeiten."
                rbLieferscheinKunde.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
                lblNeukundeResultatMeldung.ForeColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
                lblNeukundeResultatMeldung.Text = "Land abweichend von Deutschland! Kein Lieferschein möglich."
                MPENeukundeResultat.Show()
                lblError.Visible = True
                lblError.Text = "Land abweichend von Deutschland! Kein Lieferschein möglich."
            ElseIf CheckBedienerKarte() Then
                GetMaskData()
                DoSubmit()
            End If
        End If
        lb_zurueck.Focus()

    End Sub

    Private Function CheckBedienerKarte() As Boolean
        txtMitarbeiter.Attributes.Add("value", txtMitarbeiter.Text)
        If txtMitarbeiter.Text = String.Empty Then
            lblError.Visible = True
            lblError.Text = "Bitte lesen Sie die Bedienerkarte ein!"
            Return False
        ElseIf txtMitarbeiter.Text.Length <> 15 Then
            lblError.Visible = True
            lblError.Text = "Fehler beim einlesen der Bedienerkarte. Barcode hat die falsche Länge!"
            Return False
        Else
            Try
                Dim strCode As String
                Dim strBediener As String
                strCode = Left(txtMitarbeiter.Text, 14)
                strCode = Right(strCode, 13)
                strBediener = strCode.Substring(3, 1)
                strBediener &= strCode.Substring(6, 1)
                strBediener &= strCode.Substring(8, 1)
                strBediener &= strCode.Substring(11, 1)
                mObjCommon.MitarbeiterNr = strBediener
                Return True
            Catch ex As Exception
                lblError.Visible = True
                lblError.Text = "Fehler beim einlesen der Bedienerkarte. Versuchen Sie es nochmal!"
                Return False

            End Try

        End If

    End Function

    Private Sub GetMaskData()
        With mObjCommon

            .BrancheFreitext = txtBrancheFrei.Text.Trim
            .Name1 = txtName1.Text.Trim
            .Name2 = txtName2.Text.Trim
            .Strasse = txtStrasse.Text.Trim
            .HausNr = txtHausnr.Text.Trim
            .Ort = txtOrt.Text.Trim
            .PLZ = txtPlz.Text.Trim
            .UIDNummer = txtUIDNummer.Text.Trim
            .Telefon = txtTelefon.Text.Trim
            .Mobil = txtMobil.Text.Trim
            .Mail = txtMail.Text.Trim
            .Fax = txtFax.Text.Trim
            .Land = ddLand.SelectedValue
            If rbBarkunde.Checked Then
                .Abruftyp = "3"
            Else
                .Abruftyp = "2"
            End If
            If rbEinzugJa.Checked Then
                .EinzugEr = "X"
            Else
                .EinzugEr = ""
            End If
            If rbFirma.Checked Then
                .Anrede = "0003"
            ElseIf rbHerr.Checked Then
                .Anrede = "0002"
            ElseIf rbFrau.Checked Then
                .Anrede = "0001"
            End If
            .Branche = ddlBranche.SelectedValue
            If ddlFunktion.SelectedValue <> "00" Then
                .Funktion = ddlFunktion.SelectedValue
            End If
            .ASPName = txtNameAnPartner.Text
            .ASPVorname = txtVornameAnPartner.Text
        End With
    End Sub

    Private Sub SetMaskData()
        With mObjCommon

            txtBrancheFrei.Text = .BrancheFreitext
            txtName1.Text = .Name1
            txtName2.Text = .Name2
            txtStrasse.Text = .Strasse
            txtHausnr.Text = .HausNr
            txtOrt.Text = .Ort
            txtPlz.Text = .PLZ
            txtTelefon.Text = .Telefon
            txtMobil.Text = .Mobil
            txtMail.Text = .Mail
            txtFax.Text = .Fax
            ddLand.SelectedValue = .Land

            If .Abruftyp = "3" Then
                rbBarkunde.Checked = True
            End If
            If .EinzugEr = "X" Then
                rbEinzugJa.Checked = True
            End If

            Select Case .Anrede
                Case "0003"
                    rbFirma.Checked = True
                Case "0002"
                    rbHerr.Checked = True
                Case "0002"
                    rbFrau.Checked = True
            End Select
            ddlBranche.SelectedValue = .Branche

            If .Funktion.Length > 0 Then
                ddlFunktion.SelectedValue = .Funktion
            Else
                ddlFunktion.SelectedValue = "00"
            End If

            txtNameAnPartner.Text = .ASPName
            txtVornameAnPartner.Text = .ASPVorname

        End With
    End Sub

    Private Sub DoSubmit()
        mObjCommon.ChangeERP()
        If mObjCommon.ErrorOccured Then
            lblError.Visible = True
            lblError.Text = mObjCommon.ErrorMessage
            lblNeukundeResultatMeldung.ForeColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            lblNeukundeResultatMeldung.Text = mObjCommon.ErrorMessage
        Else
            lblNeukundeResultatMeldung.ForeColor = Drawing.Color.Green
            lblNeukundeResultatMeldung.Text = "Neue Kundenstammdaten erfolgreich angelegt!"
            MPENeukundeResultat.Show()
            EnableDisableControls(False)
            txtMitarbeiter.Focus()
        End If
    End Sub

    Private Sub EnableDisableControls(ByVal enabled As Boolean)
        txtMitarbeiter.Enabled = enabled
        txtBrancheFrei.Enabled = enabled
        txtName1.Enabled = enabled
        txtName2.Enabled = enabled
        txtStrasse.Enabled = enabled
        txtHausnr.Enabled = enabled
        txtOrt.Enabled = enabled
        txtPlz.Enabled = enabled
        txtUIDNummer.Enabled = enabled
        ddLand.Enabled = enabled
        rbBarkunde.Enabled = enabled
        rbLieferscheinKunde.Enabled = enabled
        rbEinzugJa.Enabled = enabled
        rbEinzugNein.Enabled = enabled
        rbHerr.Enabled = enabled
        rbFirma.Enabled = enabled
        rbFrau.Enabled = enabled
        ddlBranche.Enabled = enabled
        ddlFunktion.Enabled = enabled
        txtNameAnPartner.Enabled = enabled
        txtVornameAnPartner.Enabled = enabled
        txtTelefon.Enabled = enabled
        txtFax.Enabled = enabled
        txtMail.Enabled = enabled
        txtMobil.Enabled = enabled
        lbAbsenden.Enabled = enabled
        lbParken.Enabled = enabled
        lbAusparken.Enabled = enabled
        lb_Neu.Visible = Not enabled
        lblNoData.ForeColor = Drawing.Color.Green
        lblNoData.Text = "Klicken Sie auf ""Neuer Kunde"" um einen weiteren Kunden zu erfassen!"
    End Sub

    Private Sub ClearErrors()
        txtMitarbeiter.BorderColor = Drawing.ColorTranslator.FromHtml("#dfdfdf")
        txtBrancheFrei.BorderColor = Drawing.ColorTranslator.FromHtml("#dfdfdf")
        txtName1.BorderColor = Drawing.ColorTranslator.FromHtml("#dfdfdf")
        txtStrasse.BorderColor = Drawing.ColorTranslator.FromHtml("#dfdfdf")
        txtHausnr.BorderColor = Drawing.ColorTranslator.FromHtml("#dfdfdf")
        txtOrt.BorderColor = Drawing.ColorTranslator.FromHtml("#dfdfdf")
        txtPlz.BorderColor = Drawing.ColorTranslator.FromHtml("#dfdfdf")
        txtUIDNummer.BorderColor = Drawing.ColorTranslator.FromHtml("#dfdfdf")
        ddLand.BorderStyle = BorderStyle.None
        rbBarkunde.BorderStyle = BorderStyle.None
        rbLieferscheinKunde.BorderStyle = BorderStyle.None
        rbEinzugJa.BorderStyle = BorderStyle.None
        rbEinzugNein.BorderStyle = BorderStyle.None
        rbHerr.BorderStyle = BorderStyle.None
        rbFirma.BorderStyle = BorderStyle.None
        rbFrau.BorderStyle = BorderStyle.None
        ddlBranche.BorderStyle = BorderStyle.None
        lblError.Visible = False
        lblError.Text = ""
    End Sub

    Private Function ValidateInput() As Boolean

        Dim bReturn As Boolean = False

        If txtMitarbeiter.Text.Trim.Length <> 15 Then
            txtMitarbeiter.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        End If
        If rbBarkunde.Checked = False AndAlso rbLieferscheinKunde.Checked = False Then
            rbBarkunde.BorderStyle = BorderStyle.Solid
            rbBarkunde.BorderWidth = 1
            rbBarkunde.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            rbLieferscheinKunde.BorderStyle = BorderStyle.Solid
            rbLieferscheinKunde.BorderWidth = 1
            rbLieferscheinKunde.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        End If
        If rbLieferscheinKunde.Checked = True Then
            If rbEinzugJa.Checked = False AndAlso rbEinzugNein.Checked = False Then
                rbEinzugJa.BorderStyle = BorderStyle.Solid
                rbEinzugJa.BorderWidth = 1
                rbEinzugJa.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
                rbEinzugNein.BorderStyle = BorderStyle.Solid
                rbEinzugNein.BorderWidth = 1
                rbEinzugNein.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
                bReturn = True
            End If
        End If
        If rbFirma.Checked = False AndAlso rbHerr.Checked = False AndAlso rbFrau.Checked = False Then
            rbFirma.BorderStyle = BorderStyle.Solid
            rbFirma.BorderWidth = 1
            rbFirma.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            rbHerr.BorderStyle = BorderStyle.Solid
            rbHerr.BorderWidth = 1
            rbHerr.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            rbFrau.BorderStyle = BorderStyle.Solid
            rbFrau.BorderWidth = 1
            rbFrau.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        End If
        If ddlBranche.SelectedIndex = -1 Then
            ddlBranche.BorderStyle = BorderStyle.Solid
            ddlBranche.BorderWidth = 1
            ddlBranche.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        ElseIf (ddlBranche.SelectedValue = "0004" Or ddlBranche.SelectedValue = "0014") AndAlso txtBrancheFrei.Text.Trim.Length = 0 Then
            txtBrancheFrei.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        End If
        If txtName1.Text.Trim.Length = 0 Then
            txtName1.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        End If

        If txtStrasse.Text.Trim.Length = 0 Then
            txtStrasse.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        End If

        If txtHausnr.Text.Trim.Length = 0 Then
            txtHausnr.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        End If

        If txtOrt.Text.Trim.Length = 0 Then
            txtOrt.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        End If

        If txtPlz.Text.Trim.Length = 0 Then
            txtPlz.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        ElseIf ddLand.SelectedValue = "DE" AndAlso txtPlz.Text.Trim.Length <> 5 Then
            txtPlz.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        End If

        If ddLand.SelectedIndex = -1 Then
            ddLand.BorderStyle = BorderStyle.Solid
            ddLand.BorderWidth = 1
            ddLand.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
            bReturn = True
        ElseIf ddLand.SelectedValue <> "DE" Then
            If txtUIDNummer.Text.Trim.Length = 0 Then
                txtUIDNummer.BorderColor = Drawing.ColorTranslator.FromHtml("#BC2B2B")
                bReturn = True
            End If
        End If
        Return bReturn
    End Function

    Protected Sub ddlBranche_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlBranche.SelectedIndexChanged
        If ddlBranche.SelectedValue = "0004" Or ddlBranche.SelectedValue = "0014" Then
            txtBrancheFrei.Visible = True
            lblBrancheFrei.Visible = True
        Else
            txtBrancheFrei.Visible = False
            lblBrancheFrei.Visible = False
        End If
        txtMitarbeiter.Attributes.Add("value", txtMitarbeiter.Text)
    End Sub

    Protected Sub lb_Neu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Neu.Click
        EnableDisableControls(True)
        With mObjCommon
            .Abruftyp = ""
            .EinzugEr = ""
            .Anrede = ""
            .Branche = ""
            .BrancheFreitext = ""
            .Name1 = ""
            .Name2 = ""
            .Strasse = ""
            .Ort = ""
            .HausNr = ""
            .PLZ = ""
            .Land = ""
            .UIDNummer = ""
            .ASPVorname = ""
            .ASPName = ""
            .Funktion = ""
            .Telefon = ""
            .Mobil = ""
            .HausNr = ""
            .Fax = ""
            .Land = ""
        End With
        ddLand.SelectedValue = "DE"
        lblStar.Visible = False
        ddlBranche.SelectedIndex = 0
        ddlFunktion.SelectedIndex = 0
        txtBrancheFrei.Visible = False
        txtBrancheFrei.Text = ""
        lblBrancheFrei.Visible = False

        txtName1.Text = ""
        txtName2.Text = ""
        txtStrasse.Text = ""
        txtHausnr.Text = ""
        txtOrt.Text = ""
        txtPlz.Text = ""
        txtUIDNummer.Text = ""
        rbBarkunde.Checked = False
        rbLieferscheinKunde.Checked = False
        rbEinzugJa.Checked = False
        rbEinzugNein.Checked = False
        rbHerr.Checked = False
        rbFirma.Checked = False
        rbFrau.Checked = False
        txtNameAnPartner.Text = ""
        txtVornameAnPartner.Text = ""
        txtTelefon.Text = ""
        txtFax.Text = ""
        txtMail.Text = ""
        txtMobil.Text = ""
        lbAbsenden.Visible = True
        lblNoData.Text = "Bitte füllen Sie alle Pflichtfelder(*) aus!"
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub responseBack()
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub rbLieferscheinKunde_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbLieferscheinKunde.CheckedChanged
        If rbLieferscheinKunde.Checked = True Then
            rbEinzugJa.Checked = False
            rbEinzugNein.Checked = True
        End If
        txtMitarbeiter.Attributes.Add("value", txtMitarbeiter.Text)
    End Sub

    Protected Sub rbBarkunde_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbBarkunde.CheckedChanged
        If rbBarkunde.Checked = True Then
            rbEinzugJa.Checked = False
            rbEinzugNein.Checked = False
        End If
        txtMitarbeiter.Attributes.Add("value", txtMitarbeiter.Text)
    End Sub

    Protected Sub ddLand_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddLand.SelectedIndexChanged
        If ddLand.SelectedValue <> "DE" Then
            lblStar.Visible = True
        Else
            lblStar.Visible = False
        End If
    End Sub

    Protected Sub rbEinzugJa_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbEinzugJa.CheckedChanged
        If rbEinzugJa.Checked Then
           
        End If
    End Sub

    Protected Sub lbParken_Click(sender As Object, e As EventArgs) Handles lbParken.Click
        GetMaskData()
        mObjCommon.ParkenERP()
        If mObjCommon.ErrorOccured Then
            lblError.Text = "Fehler beim Parken der Kundendaten: " + mObjCommon.ErrorMessage
        End If
    End Sub

    Protected Sub lbAusparken_Click(sender As Object, e As EventArgs) Handles lbAusparken.Click
        MPEAusparken.Show()
    End Sub

    Private Sub Change08_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim dt As DataTable = mObjCommon.GetListeAusparkenERP()
        If dt.Rows.Count > 0 Then
            lbAusparken.Visible = True
            gvAusparken.DataSource = dt
            gvAusparken.DataBind()
        Else
            lbAusparken.Visible = False
        End If
    End Sub

    Protected Sub gvAusparken_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAusparken.RowCommand
        Select Case e.CommandName
            Case "ausparken"
                mObjCommon.AusparkenERP(e.CommandArgument)
                If mObjCommon.ErrorOccured Then
                    lblError.Text = "Fehler beim Ausparken der Kundendaten: " + mObjCommon.ErrorMessage
                End If
                SetMaskData()
            Case "löschen"
                mObjCommon.AusparkenERP(e.CommandArgument)
                If mObjCommon.ErrorOccured Then
                    lblError.Text = "Fehler beim Löschen der geparkten Kundendaten: " + mObjCommon.ErrorMessage
                End If
        End Select
    End Sub

End Class