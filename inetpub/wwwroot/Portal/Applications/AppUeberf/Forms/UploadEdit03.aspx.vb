Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class UploadEdit03
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As UeberfgStandard_01

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try


        If IsPostBack = False Then
            GetData()

        Else
            If clsUeberf Is Nothing Then
                clsUeberf = Session("App_Ueberf")
            End If
            SetData()
        End If

        DisableControls()
        lnkAnschlussfahrt.Visible = clsUeberf.NewDataSet
        cmdBack0.Attributes.Add("onclick", "return ShowMessage()")
    End Sub


    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        If Weiter() = False Then
            If clsUeberf.Anschluss = False Then
                Response.Redirect("UploadEdit04.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("UploadEdit03_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub Weiter_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lnkAnschlussfahrt.Click
        Response.Redirect("UploadEdit03_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub
    Private Function Weiter() As Boolean
        Dim booErr As Boolean
        Dim strErr As String
        Dim strSonder As String

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If


        With clsUeberf

            .Modus = 2

            strErr = "Bitte füllen Sie alle Pflichtfelder aus: <br>"


            If .Herst = "" Then
                booErr = True
                strErr = strErr & "Hersteller / Typ <br>"
                txtHerstTyp.BorderStyle = BorderStyle.Solid
                txtHerstTyp.BorderColor = Color.Red
            End If

            If .Kenn1 = "" Or .Kenn2 = "" Then
                booErr = True
                strErr = strErr & "Kennzeichen <br>"
                txtKennzeichen1.BorderStyle = BorderStyle.Solid
                txtKennzeichen1.BorderColor = Color.Red
                txtKennzeichen2.BorderStyle = BorderStyle.Solid
                txtKennzeichen2.BorderColor = Color.Red
            End If

            If .FzgZugelassen = "" Then
                booErr = True
                strErr = strErr & "Fahrzeug zugelassen und fahrbereit? <br>"
                rdbZugelassen.BorderStyle = BorderStyle.Solid
                rdbZugelassen.BorderColor = Color.Red
            End If



            If .SomWin = "" Then
                booErr = True
                strErr = strErr & "Bereifung  <br>"
                rdbBereifung.BorderStyle = BorderStyle.Solid
                rdbBereifung.BorderColor = Color.Red
            End If

            If .Fahrzeugklasse = "" Then
                booErr = True
                strErr = strErr & "Fahrzeugklasse in Tonnen  <br>"
                rdbFahrzeugklasse.BorderStyle = BorderStyle.Solid
                rdbFahrzeugklasse.BorderColor = Color.Red
            End If

            If .Express = "" Then
                booErr = True
                strErr = strErr & "Expressüberführung? <br>"
                rdExpress.BorderStyle = BorderStyle.Solid
                rdExpress.BorderColor = Color.Red
            End If

            If .Hin_KCL_Zulassen = "" Then
                booErr = True
                strErr = strErr & "Zulassung durch KCL?  <br>"
                rdbHinZulKCL.BorderStyle = BorderStyle.Solid
                rdbHinZulKCL.BorderColor = Color.Red
            End If

            If drpFahrzeugwert.SelectedItem.Value = "0" Then
                booErr = True
                strErr = strErr & "Fahrzeugwert  <br>"
                drpFahrzeugwert.BackColor = Color.Red
            End If

            .SelFahrzeugwert = drpFahrzeugwert.SelectedItem.Value

        End With

        If txtAbgabetermin.Text <> "" Then
            If IsDate(txtAbgabetermin.Text) = False Then
                If booErr = True Then
                    strErr = strErr & "Bitte geben Sie einen korrekten Überführungstermin im Format TT.MM.JJJJ ein.  <br>"
                    txtAbgabetermin.BorderStyle = BorderStyle.Solid
                    txtAbgabetermin.BorderColor = Color.Red
                Else
                    booErr = True
                    strErr = "Bitte geben Sie einen korrekten Überführungstermin im Format TT.MM.JJJJ ein.  <br>"
                    txtAbgabetermin.BorderStyle = BorderStyle.Solid
                    txtAbgabetermin.BorderColor = Color.Red
                End If
            End If
        End If
        If txtRef.Text.Length > 0 Then
            strSonder = Proof_SpecialChar(txtRef.Text.Trim)
            If strSonder.Trim.Length > 0 Then
                booErr = True
                strErr = "Bitte geben Sie die Referenz-Nr. ohne Sonderzeichen -> " & strSonder & " <- ein.  <br>"
                txtRef.BorderStyle = BorderStyle.Solid
                txtRef.BorderColor = Color.Red
            End If
        End If
        Session("App_Ueberf") = clsUeberf
        Session("App_UebTexte") = Nothing
        If booErr = False Then
            Return False

        Else
            Return True
            lblError.Text = strErr
        End If
    End Function
    Private Sub SetData()

        'Bei reiner Überführung
        DisableControls()

        With clsUeberf

            .Herst = txtHerstTyp.Text
            .Kenn1 = txtKennzeichen1.Text
            .Kenn2 = txtKennzeichen2.Text
            .Vin = txtVin.Text
            .Ref = txtRef.Text
            .DatumUeberf = txtAbgabetermin.Text
            .Bemerkung = txtBemerkung.Text

        End With


    End Sub

    Private Sub GetData()

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If

        'Bei reiner Überführung
        DisableControls()

        drpFahrzeugwert.AutoPostBack = True


        With drpFahrzeugwert
            .Items.Add(New ListItem("Bitte auswählen", "0"))
            .Items.Add(New ListItem("...Ohne Versicherung", "ZOV"))
            .Items.Add(New ListItem("...bis  50  Tsd. €", "Z00"))
            .Items.Add(New ListItem("...bis 150  Tsd. €", "Z50"))
        End With

        With clsUeberf

            txtHerstTyp.Text = .Herst
            txtKennzeichen1.Text = .Kenn1
            txtKennzeichen2.Text = .Kenn2
            txtVin.Text = .Vin
            txtRef.Text = .Ref
            txtAbgabetermin.Text = .DatumUeberf
            txtBemerkung.Text = .Bemerkung
            chkEinweisung.Checked = .FzgEinweisung
            'chkRetour.Checked = .Anschluss
            chkWagenVolltanken.Checked = .Tanken
            chkWw.Checked = .Waesche
            chkRotKenn.Checked = .RotKenn
            If .FzgZugelassen <> "" Then
                rdbZugelassen.Items.FindByValue(.FzgZugelassen).Selected = True
            End If

            If .Express <> "" Then
                rdExpress.Items.FindByValue(.Express).Selected = True
            End If

            If .SomWin <> "" Then
                rdbBereifung.Items.FindByValue(.SomWin).Selected = True
            End If
            If .Fahrzeugklasse <> "" Then
                rdbFahrzeugklasse.Items.FindByValue(.Fahrzeugklasse).Selected = True
            End If
            If Not .SelFahrzeugwert Is Nothing Then
                drpFahrzeugwert.Items.FindByValue(.SelFahrzeugwert).Selected = True
            End If

            If Not .Hin_KCL_Zulassen Is Nothing Then

                If .Hin_KCL_Zulassen <> "" Then rdbHinZulKCL.Items.FindByValue(.Hin_KCL_Zulassen).Selected = True
            End If
            'If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
            '    rdbHinZulKCL.Items(0).Selected = True
            '    clsUeberf.Hin_KCL_Zulassen = "Ja"
            'End If
        End With
        If Not IsNothing(Session("App_UebTexte")) Then
            txtBemerkung.Text = txtBemerkung.Text & " " & Session("App_UebTexte").ToString.Trim
        End If

    End Sub


    Private Sub DisableControls()
        'Wenn es sich um eine reine Überführung handelt, und dort ein Fahrzeug im
        'Bestand gefunden wurde, sollen bestimmte Felder deaktiviert werden.
        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung And clsUeberf.FahrzeugVorhanden = True Then
            With clsUeberf
                If .Herst <> "" Then
                    txtHerstTyp.Enabled = False
                End If

                If .Kenn1 <> "" Then
                    txtKennzeichen1.Enabled = False
                End If

                If .Kenn2 <> "" Then
                    txtKennzeichen2.Enabled = False
                End If

                If .Ref <> "" Then
                    txtRef.Enabled = False
                End If

                If .Vin <> "" Then
                    txtVin.Enabled = False
                End If
            End With
        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Or _
                clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.OffeneUeberfuehrung Then

            If clsUeberf.Vin <> "" Then
                txtVin.Enabled = False
            End If

            If clsUeberf.Ref <> "" Then
                txtRef.Enabled = False
            End If

        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
            If clsUeberf.Kenn1 <> "" Then
                txtKennzeichen1.Enabled = False
            End If

        End If
    End Sub


    Private Sub rdbZugelassen_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbZugelassen.SelectedIndexChanged
        clsUeberf.FzgZugelassen = rdbZugelassen.SelectedItem.Value
    End Sub

    Private Sub chkWagenVolltanken_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWagenVolltanken.CheckedChanged
        clsUeberf.Tanken = chkWagenVolltanken.Checked
    End Sub

    'Private Sub chkRetour_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRetour.CheckedChanged
    '    clsUeberf.Anschluss = chkRetour.Checked
    'End Sub

    Private Sub chkEinweisung_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEinweisung.CheckedChanged
        clsUeberf.FzgEinweisung = chkEinweisung.Checked
    End Sub

    Private Sub chkWw_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWw.CheckedChanged
        clsUeberf.Waesche = chkWw.Checked
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If

        clsUeberf.Modus = 1
        Session("App_UebTexte") = Nothing
        Session("App_Ueberf") = clsUeberf
        Response.Redirect("UploadEdit02.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub rdbBereifung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbBereifung.SelectedIndexChanged
        clsUeberf.SomWin = rdbBereifung.SelectedItem.Value
    End Sub

    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
    End Sub
    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtAbgabetermin.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub chkRotKenn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRotKenn.CheckedChanged
        clsUeberf.RotKenn = chkRotKenn.Checked
    End Sub

    Private Sub drpFahrzeugwert_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpFahrzeugwert.SelectedIndexChanged
        clsUeberf.SelFahrzeugwert = drpFahrzeugwert.SelectedItem.Value
        clsUeberf.FahrzeugwertTxt = drpFahrzeugwert.SelectedItem.Text

    End Sub

    Private Sub rdbFahrzeugklasse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbFahrzeugklasse.SelectedIndexChanged
        clsUeberf.Fahrzeugklasse = rdbFahrzeugklasse.SelectedItem.Value
        clsUeberf.FahrzeugklasseTxt = rdbFahrzeugklasse.SelectedItem.Text & " to"
    End Sub

    Private Sub rdExpress_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdExpress.SelectedIndexChanged
        clsUeberf.Express = rdExpress.SelectedItem.Value
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub linkbtTexte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles linkbtTexte.Click
        Session("App_UebTexte") = Nothing
        Session("App_FromPage") = "UploadEdit03.aspx"
        Response.Redirect("ChangeStdText.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub rdbHinZulKCL_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbHinZulKCL.SelectedIndexChanged
        clsUeberf.Hin_KCL_Zulassen = rdbHinZulKCL.SelectedItem.Value
    End Sub


    Private Sub btnConfOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfOK.Click
        Dim RetTable As DataTable
        Dim strErr As String = ""

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If
        If Weiter() = False Then



            clsUeberf.AuftragsStatus = "B"
            If clsUeberf.NewDataSet = False Then
                RetTable = clsUeberf.Update()
            Else
                clsUeberf.AufIDSel = "0000000000"
                clsUeberf.MakeAuftragsTable()
                clsUeberf.MakePartnerTable()
                clsUeberf.NewAuftrag()
                RetTable = clsUeberf.Update("N")
            End If

            If clsUeberf.Message.Length > 0 Then
                lblError.Text = clsUeberf.Message
            Else

                Dim Drow As DataRow
                Dim Nrow As DataRow
                Dim DataTblChecked As New DataTable
                DataTblChecked.Columns.Add("Auf_ID", System.Type.GetType("System.String"))
                DataTblChecked.Columns.Add("Ok", System.Type.GetType("System.Boolean"))
                DataTblChecked.Columns.Add("Del", System.Type.GetType("System.Boolean"))
                DataTblChecked.Columns.Add("NoSel", System.Type.GetType("System.Boolean"))
                If clsUeberf.NewDataSet = False Then
                    For Each Drow In clsUeberf.TabUpload.Rows
                        Nrow = DataTblChecked.NewRow
                        Nrow("Auf_ID") = Drow("Auf_ID")
                        Nrow("Ok") = Drow("Ok")
                        Nrow("Del") = Drow("Del")
                        Nrow("NoSel") = Drow("NoSel")
                        DataTblChecked.Rows.Add(Nrow)
                    Next
                    DataTblChecked.AcceptChanges()
                    Session("App_SelData") = DataTblChecked
                End If
                Session("App_Ueberf") = Nothing
                clsUeberf.CleanClass()
                Response.Redirect("UploadUeberf.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub btnConfCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfCancel.Click
        Response.Redirect("UploadUeberf.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub cmdBack0_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack0.Click
        btnConfOK_Click(sender, e)
    End Sub

    Private Sub btnConfOK_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfOK.Load

    End Sub
End Class
' ************************************************
' $History: UploadEdit03.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 13.11.08   Time: 11:33
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 10.09.08   Time: 17:58
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 8.09.08    Time: 11:51
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 25.08.08   Time: 8:28
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' ************************************************
