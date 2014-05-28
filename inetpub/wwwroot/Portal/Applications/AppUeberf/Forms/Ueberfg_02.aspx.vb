Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Ueberfg_02
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As UeberfgStandard_01

    Protected WithEvents lblKundeName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugdaten As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents txtHerstTyp As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVin As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRef As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbgabetermin As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkWagenVolltanken As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkWw As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkEinweisung As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents rdbZugelassen As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents ucHeader As Header
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents rdbBereifung As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents drpFahrzeugwert As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents chkRotKenn As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents rdExpress As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents rdbFahrzeugklasse As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lnkAnschlussfahrt As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtBemerkung As CKG.Portal.PageElements.TextAreaControl
    Protected WithEvents Label99 As System.Web.UI.WebControls.Label
    Protected WithEvents rdbHinZulKCL As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents linkbtTexte As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles



#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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
                clsUeberf = Session("Ueberf")
            End If
            SetData()
        End If

        DisableControls()
    End Sub


    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        Weiter_Click(sender, e)
    End Sub


    Private Sub Weiter_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lnkAnschlussfahrt.Click
        Dim booErr As Boolean
        Dim strErr As String
        Dim strSonder As String

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If


        With clsUeberf

            .Modus = 2

            strErr = "Bitte füllen Sie alle Pflichtfelder aus: <br>"


            If .Herst = "" Then
                booErr = True
                strErr = strErr & "Hersteller / Typ <br>"
            End If

            If .Kenn1 = "" Or .Kenn2 = "" Then
                booErr = True
                strErr = strErr & "Kennzeichen <br>"
            End If

            If .FzgZugelassen = "" Then
                booErr = True
                strErr = strErr & "Fahrzeug zugelassen und fahrbereit? <br>"
            End If



            If .SomWin = "" Then
                booErr = True
                strErr = strErr & "Bereifung  <br>"
            End If

            If .Fahrzeugklasse = "" Then
                booErr = True
                strErr = strErr & "Fahrzeugklasse in Tonnen  <br>"
            End If

            If .Express = "" Then
                booErr = True
                strErr = strErr & "Expressüberführung? <br>"
            End If

            If .Hin_KCL_Zulassen = "" Then
                booErr = True
                strErr = strErr & "Zulassung durch KCL?  <br>"
            End If

            If drpFahrzeugwert.SelectedItem.Value = "0" Then
                booErr = True
                strErr = strErr & "Fahrzeugwert  <br>"
            End If

            .SelFahrzeugwert = drpFahrzeugwert.SelectedItem.Value

        End With

        If txtAbgabetermin.Text <> "" Then
            If IsDate(txtAbgabetermin.Text) = False Then
                If booErr = True Then
                    strErr = strErr & "Bitte geben Sie einen korrekten Überführungstermin im Format TT.MM.JJJJ ein.  <br>"
                Else
                    booErr = True
                    strErr = "Bitte geben Sie einen korrekten Überführungstermin im Format TT.MM.JJJJ ein.  <br>"
                End If
            End If
        End If
        If txtRef.Text.Length > 0 Then
            strSonder = Proof_SpecialChar(txtRef.Text.Trim)
            If strSonder.Trim.Length > 0 Then
                booErr = True
                strErr = "Bitte geben Sie die Referenz-Nr. ohne Sonderzeichen -> " & strSonder & " <- ein.  <br>"
            End If
        End If
        Session("Ueberf") = clsUeberf
        Session("App_UebTexte") = Nothing
        If booErr = False Then

            If sender Is lnkAnschlussfahrt Then
                clsUeberf.Anschluss = True
                Response.Redirect("Ueberfg_03.aspx?AppID=" & Session("AppID").ToString)
            Else
                If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
                    Response.Redirect("Zulg_UebBest.aspx?AppID=" & Session("AppID").ToString)
                Else
                    Response.Redirect("Ueberfg_04.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        Else
            lblError.Text = strErr
        End If
    End Sub





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
            clsUeberf = Session("Ueberf")
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

            lblKundeName1.Text = .KundeName
            lblKundeStrasse.Text = .KundeStrasse
            lblKundePlzOrt.Text = .KundeOrt
            lblKundeAnsprechpartner.Text = .KundeAnsprechpartner
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
            If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
                rdbHinZulKCL.Items(0).Selected = True
                clsUeberf.Hin_KCL_Zulassen = "Ja"
            End If
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
            clsUeberf = Session("Ueberf")
        End If

        clsUeberf.Modus = 1
        Session("App_UebTexte") = Nothing
        Session("Ueberf") = clsUeberf
        Response.Redirect("Ueberfg_01.aspx?AppID=" & Session("AppID").ToString)
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

    Private Sub rdbHinZulKCL_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbHinZulKCL.SelectedIndexChanged
        clsUeberf.Hin_KCL_Zulassen = rdbHinZulKCL.SelectedItem.Value
    End Sub


    Private Sub linkbtTexte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles linkbtTexte.Click
        Session("App_UebTexte") = Nothing
        Session("App_FromPage") = "Ueberfg_02.aspx"
        Response.Redirect("ChangeStdText.aspx?AppID=" & Session("AppID").ToString)
    End Sub


End Class

' ************************************************
' $History: Ueberfg_02.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 25.08.08   Time: 8:28
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 9.01.08    Time: 16:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1604
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 22.10.07   Time: 12:50
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen Standarttexte
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 12.09.07   Time: 15:03
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1218
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 12.09.07   Time: 9:24
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1218
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 6.09.07    Time: 12:45
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1247
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 30.08.07   Time: 9:57
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1247 
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 24.08.07   Time: 11:31
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 24.08.07   Time: 8:43
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 23.08.07   Time: 12:57
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1246 
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 10.07.07   Time: 13:37
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 22.06.07   Time: 16:22
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 19.06.07   Time: 15:11
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 2  *****************
' User: Uha          Date: 5.04.07    Time: 11:14
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Verlinkung der Formulare untereinander korrigiert.
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 16:56
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Überführungs-ASPX-Seiten aus Shared-Bereich übernommen
' 
' ******************************************
