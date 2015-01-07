Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change02_2
    Inherits Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lb_weiter As LinkButton
    Protected WithEvents lb_zurueck As LinkButton

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App


    Protected WithEvents rb_Geschaeftsstelle As RadioButton
    Protected WithEvents rb_Partneradresse As RadioButton
    Protected WithEvents rb_Versandanschrift As RadioButton
    Protected WithEvents rb_1391 As RadioButton
    Protected WithEvents rb_1385 As RadioButton
    Protected WithEvents rb_1390 As RadioButton
    Protected WithEvents rb_1389 As RadioButton
    Protected WithEvents rb_5530 As RadioButton
    Protected WithEvents rb_Selbstabholung As RadioButton
    Protected WithEvents rb_47076 As RadioButton

    Protected WithEvents ddlPartneradressen As DropDownList
    Protected WithEvents ddlGeschaeftsstelle As DropDownList
    Protected WithEvents ddlLand As DropDownList
    Protected WithEvents ddlVersandgrund As DropDownList

    Protected WithEvents tr_Partneradresse As HtmlTableRow
    Protected WithEvents tr_Geschaeftsstelle As HtmlTableRow
    Protected WithEvents tr_Versandanschrift As HtmlTableRow
    Protected WithEvents trVersandanschriftValue As HtmlTableRow
    Protected WithEvents tr_Bemerkung As HtmlTableRow
    Protected WithEvents trVersandart As HtmlTableRow
    Protected WithEvents trVersandartValue As HtmlTableRow
    Protected WithEvents trParameterTemp1 As HtmlTableRow
    Protected WithEvents trParameterTemp2 As HtmlTableRow
    Protected WithEvents trParameterEndg1 As HtmlTableRow
    Protected WithEvents trParameterEndg2 As HtmlTableRow

    Protected WithEvents txtName1 As TextBox
    Protected WithEvents txtName2 As TextBox
    Protected WithEvents txtStrasse As TextBox
    Protected WithEvents txtOrt As TextBox
    Protected WithEvents txtHausnummer As TextBox
    Protected WithEvents txtPostleitzahl As TextBox
    Protected WithEvents txtVersandGrundZusatzEingabe As TextBox
    Protected WithEvents txtBemerkung As TextBox


    Protected WithEvents lblVersandGrundZusatzBemerkung As Label

    Protected WithEvents chbEingentumsvorbehaltEintragen As CheckBox
    Protected WithEvents chbBenutzerueberlassung As CheckBox
    Protected WithEvents chbEingentumsvorbehaltLoeschen As CheckBox
    Protected WithEvents chbDevinkulierungsschreiben As CheckBox

    Protected WithEvents lb_Suche As LinkButton
    Protected WithEvents lb_NeuSuche As LinkButton
    Protected WithEvents txtNameSuche As TextBox
    Protected WithEvents txtOrtSuche As TextBox
    Protected WithEvents txtPostleitzahlSuche As TextBox
    Protected WithEvents lblSuche As Label
    Protected WithEvents tblSuche As HtmlTable
    Protected WithEvents tblVersandanschrift As HtmlTable
    Protected WithEvents lblInfo As Label

    Private mObjBriefanforderung As Briefanforderung

#Region "Properties"


    Private Property Refferer() As String
        Get
            Dim tmpStr As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, tmpStr, ConfigurationManager.AppSettings.Item("Connectionstring"))
            If Not Session.Item(Me.Request.Url.LocalPath & tmpStr & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & tmpStr & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Dim tmpStr As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, tmpStr, ConfigurationManager.AppSettings.Item("Connectionstring"))
            Session.Item(Me.Request.Url.LocalPath & tmpStr & "Refferer") = value
        End Set
    End Property



#End Region



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

        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""
            lblInfo.Text = ""
            If Not IsPostBack Then
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If
                lblInfo.Text = "Bitte wählen Sie zuerst die Art der Empfängeranschrift aus!"
                GetAppIDFromQueryString(Me)
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

            If mObjBriefanforderung Is Nothing Then
                If Session("mObjBriefanforderungSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjBriefanforderung = CType(Session("mObjBriefanforderungSession"), Briefanforderung)
                End If
            End If
            'seitenbezifische PageLoads
            If Not IsPostBack Then
                setTempOrEndgElements()
                setVersandadressenberechtigung()
                fillLaenderDDL()
                fillAbrufgruendeDDL()
                fillPartneradressenDDL()
                fillGeschaeftsstellenDDL()
                fillFormular()



            End If

            checkVisibleVersandAdressArtSection()

        Catch ex As Exception
            lblError.Text = "beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub


    Private Sub setVersandadressenberechtigung()

        'ausblenden verschiedener versandadres möglichkeiten nach gruppenLVL
        'ita 2561 Rothe
        'JJU20090128
        Select Case m_User.Groups(0).Authorizationright

            Case 0
                rb_Geschaeftsstelle.Visible = False
                rb_Partneradresse.Visible = False
                rb_Versandanschrift.Visible = False
                Throw New Exception("Sie sind zu einer Versendung nicht berechtigt. (Gruppenautorisierungslvl)")
            Case 1
                rb_Geschaeftsstelle.Visible = True
                'rb_Geschaeftsstelle.Checked = True
                rb_Partneradresse.Visible = False
                rb_Versandanschrift.Visible = False
                rb_Partneradresse.Checked = False
                rb_Versandanschrift.Checked = False
            Case 2
                rb_Geschaeftsstelle.Visible = True
                rb_Partneradresse.Visible = True
                'rb_Partneradresse.Checked = True
                rb_Versandanschrift.Visible = False
                rb_Versandanschrift.Checked = False
            Case 3
                rb_Geschaeftsstelle.Visible = True
                rb_Partneradresse.Visible = True
                'rb_Partneradresse.Checked = True
                rb_Versandanschrift.Visible = True
        End Select


    End Sub

    Private Sub fillGeschaeftsstellenDDL()
        Dim tmpItem As ListItem
        For Each tmpRow As DataRow In mObjBriefanforderung.VersandAdressen.Rows
            tmpItem = New ListItem(tmpRow("SORTL").ToString & " " & tmpRow("Name1").ToString & " " & tmpRow("Name2").ToString & ", " & tmpRow("POST_CODE1").ToString & " " & tmpRow("CITY1").ToString, tmpRow("KUNNR").ToString)
            ddlGeschaeftsstelle.Items.Add(tmpItem)
        Next
        tmpItem = New ListItem("- keine Auswahl -", "")
        ddlGeschaeftsstelle.Items.Insert(0, tmpItem)
        If mObjBriefanforderung.VersandAdressen.Rows.Count <= 30 Then
            tblSuche.Visible = False
        Else
            lblSuche.Visible = True
            tblSuche.Visible = True
            lblSuche.Text = ""
        End If
    End Sub


    Private Sub fillPartneradressenDDL()
        Dim tmpItem As ListItem
        For Each tmpRow As DataRow In mObjBriefanforderung.PartnerAdressen.Rows
            tmpItem = New ListItem(tmpRow("SORTL").ToString & " " & tmpRow("Name1").ToString & " " & tmpRow("Name2").ToString & ", " & tmpRow("POST_CODE1").ToString & " " & tmpRow("CITY1").ToString, tmpRow("EX_KUNNR").ToString)
            ddlPartneradressen.Items.Add(tmpItem)
        Next
        tmpItem = New ListItem("- keine Auswahl -", "")
        ddlPartneradressen.Items.Insert(0, tmpItem)
    End Sub


    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_weiter.Click
        doSubmit()
    End Sub

    Private Sub fillAbrufgruendeDDL()
        Dim dataView As New DataView(mObjBriefanforderung.Abrufgruende)
        dataView.RowFilter = "AbrufTyp='" & Me.Request.QueryString.Item("Art") & "'"
        ddlVersandgrund.DataSource = dataView
        ddlVersandgrund.DataTextField = "WebBezeichnung"
        ddlVersandgrund.DataValueField = "SapWert"
        ddlVersandgrund.DataBind()
    End Sub

    Private Sub fillFormular()
        If Not mObjBriefanforderung.VersandEmpfängerArt = "" Then
            'also muss er schonmal das formular ausgefüllt haben. ->wiederherstellen
            With mObjBriefanforderung

                Select Case .VersandEmpfängerArt
                    Case "Partner"
                        rb_Partneradresse.Checked = True
                    Case "Geschaeft"
                        rb_Geschaeftsstelle.Checked = True
                    Case "Anschrift"
                        rb_Versandanschrift.Checked = True
                End Select

                txtStrasse.Text = .Street
                txtName1.Text = .Name1
                txtName1.Text = .Name2
                txtHausnummer.Text = .HouseNum
                txtOrt.Text = .City
                txtPostleitzahl.Text = .PostCode
                ddlLand.SelectedValue = .LaenderKuerzel

                Select Case .MaterialNummer
                    Case "1385"
                        rb_1385.Checked = True
                    Case "1390"
                        rb_1390.Checked = True
                    Case "1391"
                        rb_1391.Checked = True
                    Case "1389"
                        rb_1389.Checked = True
                    Case "47076"
                        rb_47076.Checked = True
                End Select

                ddlGeschaeftsstelle.SelectedValue = .Geschaefsstelle
                ddlPartneradressen.SelectedValue = .Partner
                ddlVersandgrund.SelectedValue = .Versandgrund
                txtVersandGrundZusatzEingabe.Text = .VersandgrundZusatztext
                doAbrufgrundAktion()
                checkVisibleVersandAdressArtSection()
            End With
        End If
    End Sub


    Private Sub checkVisibleVersandAdressArtSection()
        If rb_Geschaeftsstelle.Checked And tr_Geschaeftsstelle.Visible = True Then
            ddlGeschaeftsstelle.Visible = True
            ddlPartneradressen.Visible = False
            trVersandanschriftValue.Visible = False

        ElseIf rb_Partneradresse.Checked And tr_Partneradresse.Visible = True Then
            ddlGeschaeftsstelle.Visible = False
            ddlPartneradressen.Visible = True
            trVersandanschriftValue.Visible = False

        ElseIf rb_Versandanschrift.Checked And tr_Versandanschrift.Visible = True Then
            ddlGeschaeftsstelle.Visible = False
            ddlPartneradressen.Visible = False
            trVersandanschriftValue.Visible = True

        End If
    End Sub

    Private Sub setTempOrEndgElements()
        Select Case Me.Request.QueryString.Item("Art").ToUpper
            Case "ENDG"
                trParameterEndg1.Visible = True
                trParameterEndg2.Visible = True
            Case "TEMP"
                trParameterTemp1.Visible = True
                trParameterTemp2.Visible = True

            Case Else
                Throw New Exception("Fehlender Querystringparameter 'Art' Vorgang wird abgebrochen.")
        End Select
    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub


    Private Sub setVersandart()
        If rb_1385.Checked And rb_1385.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1385"
            mObjBriefanforderung.MaterialText = rb_1385.Text
        ElseIf rb_1390.Checked And rb_1390.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1390"
            mObjBriefanforderung.MaterialText = rb_1390.Text
        ElseIf rb_1391.Checked And rb_1391.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1391"
            mObjBriefanforderung.MaterialText = rb_1391.Text
        ElseIf rb_1389.Checked And rb_1389.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "47075" 'Material wurde von 1389 auf 47075 geändert.
            mObjBriefanforderung.MaterialText = rb_1389.Text
        ElseIf rb_5530.Checked And rb_5530.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "5530"
            mObjBriefanforderung.MaterialText = rb_5530.Text
        ElseIf rb_Selbstabholung.Checked And rb_Selbstabholung.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "13950"
            mObjBriefanforderung.MaterialText = rb_Selbstabholung.Text
        ElseIf rb_47076.Checked And rb_47076.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "47076"
            mObjBriefanforderung.MaterialText = rb_47076.Text


        End If
    End Sub
    Private Function checkAndSetManuelleAdresseingabe() As Boolean
        If txtName1.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Name"" eingeben.<br>&nbsp;"
            Return False
        Else
            mObjBriefanforderung.Name1 = txtName1.Text.Trim(" "c)
        End If
        If txtPostleitzahl.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""PLZ"" eingeben.<br>&nbsp;"
            Return False
        Else
            If CInt(mObjBriefanforderung.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(mObjBriefanforderung.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtPostleitzahl.Text.Trim(" "c).Length Then
                    lblError.Text = "Postleitzahl hat falsche Länge."
                    Return False
                Else
                    mObjBriefanforderung.PostCode = txtPostleitzahl.Text.Trim(" "c) & " "
                End If
            End If

        End If
        If txtOrt.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"
            Return False
        Else
            mObjBriefanforderung.City = txtOrt.Text.Trim(" "c)
        End If
        If txtStrasse.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Strasse"" eingeben.<br>&nbsp;"
            Return False
        Else
            mObjBriefanforderung.Street = txtStrasse.Text.Trim(" "c)
        End If
        If txtHausnummer.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Hausnummer"" eingeben.<br>&nbsp;"
            Return False
        Else
            mObjBriefanforderung.HouseNum = txtHausnummer.Text.Trim(" "c)
        End If

        mObjBriefanforderung.Name2 = txtName2.Text.Trim(" "c)
        mObjBriefanforderung.LaenderKuerzel = ddlLand.SelectedItem.Value
        Return True

    End Function

    Private Sub fillLaenderDDL()
        Dim sprache As String
        'Länder DLL füllen
        ddlLand.DataSource = mObjBriefanforderung.Laender
        ddlLand.DataTextField = "FullDesc"
        ddlLand.DataValueField = "Land1"
        ddlLand.DataBind()
        'vorbelegung der Länderddl auf Grund der im Browser eingestellten erstsprache JJ2007.12.06
        Dim tmpstr() As String
        If Request("HTTP_ACCEPT_Language").IndexOf(",") = -1 Then
            'es ist nur eine sprache ausgewählt
            sprache = Request("HTTP_ACCEPT_Language")
        Else
            'es gibt eine erst und eine zweitsprache
            sprache = Request("HTTP_ACCEPT_Language").Substring(0, Request("HTTP_ACCEPT_Language").IndexOf(","))
        End If

        tmpstr = sprache.Split(CChar("-"))
        'Länderkennzeichen setzen sich aus Region und Sprache zusammen. de-ch, de-at usw. leider werden bei Regionen in denen die Sprache das selbe Kürzel hat nur einfache Kürzel geschrieben, z.b. bei "de"
        If tmpstr.Length > 1 Then
            sprache = tmpstr(1).ToUpper
        Else
            sprache = tmpstr(0).ToUpper
        End If
        ddlLand.Items.FindByValue(sprache).Selected = True

    End Sub


    Private Sub lb_zurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub doSubmit()
        If checkAndWriteEmpfaengerAndOptions() Then
            setVersandart()
            setZusatzAnschreibenAndEigentumsvorbehalt()

            Dim Parameterlist As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, Parameterlist, ConfigurationManager.AppSettings.Get("Connectionstring"))
            Response.Redirect("Change02_3.aspx?AppID=" & Session("AppID").ToString & Parameterlist)
        End If
    End Sub

    Private Function checkAndWriteEmpfaengerAndOptions() As Boolean
        'prüfung und schreiben des Empfängers
        rb_Partneradresse.BackColor = Drawing.ColorTranslator.FromHtml("#ffffff")
        rb_Geschaeftsstelle.BackColor = Drawing.ColorTranslator.FromHtml("#ffffff")
        rb_Versandanschrift.BackColor = Drawing.ColorTranslator.FromHtml("#ffffff")
        With mObjBriefanforderung
            If Not rb_Partneradresse.Checked AndAlso Not rb_Geschaeftsstelle.Checked AndAlso Not rb_Versandanschrift.Checked Then
                lblError.Text = "Bitte wählen Sie zuerst die Art der Empfängeranschrift aus!"
                rb_Partneradresse.BackColor = Drawing.ColorTranslator.FromHtml("#DD0000")
                rb_Geschaeftsstelle.BackColor = Drawing.ColorTranslator.FromHtml("#DD0000")
                rb_Versandanschrift.BackColor = Drawing.ColorTranslator.FromHtml("#DD0000")
                Return False
            End If
            If rb_Partneradresse.Checked AndAlso tr_Partneradresse.Visible = True Then
                .VersandEmpfängerArt = "Partner"
                If ddlPartneradressen.SelectedValue = "" Then
                    lblError.Text = "Wählen Sie einen Empfänger"
                    Return False
                End If
                .Partner = ddlPartneradressen.SelectedValue
            ElseIf rb_Geschaeftsstelle.Checked AndAlso tr_Geschaeftsstelle.Visible = True Then
                .VersandEmpfängerArt = "Geschaeft"
                If ddlGeschaeftsstelle.SelectedValue = "" Then
                    lblError.Text = "Wählen Sie einen Empfänger"
                    Return False
                End If
                .Geschaefsstelle = ddlGeschaeftsstelle.SelectedValue
            ElseIf rb_Versandanschrift.Checked AndAlso tr_Versandanschrift.Visible = True Then
                .VersandEmpfängerArt = "Anschrift"
                If Not checkAndSetManuelleAdresseingabe() Then
                    Return False
                End If
            End If

            If ddlVersandgrund.SelectedValue = "000" Then
                lblError.Text = "Wählen Sie einen Versandgrund"
                Return False
            End If
            .Versandgrund = ddlVersandgrund.SelectedValue

            If txtVersandGrundZusatzEingabe.Visible = True And txtVersandGrundZusatzEingabe.Text.Trim(" "c) = "" Then
                lblError.Text = "Wählen füllen Sie bitte alle benötigten Felder"
                Return False
            End If
            .VersandgrundZusatztext = txtVersandGrundZusatzEingabe.Text


            mObjBriefanforderung.Bemerkung = txtBemerkung.Text


            Return True

        End With
    End Function

    Private Sub setZusatzAnschreibenAndEigentumsvorbehalt()
        If chbBenutzerueberlassung.Visible = True Then
            If chbBenutzerueberlassung.Checked Then
                mObjBriefanforderung.Zusatzanschreiben(0) = "BENU"
            Else
                mObjBriefanforderung.Zusatzanschreiben(0) = ""
            End If
        End If

        If chbDevinkulierungsschreiben.Visible = True Then
            If chbDevinkulierungsschreiben.Checked Then
                mObjBriefanforderung.Zusatzanschreiben(1) = "DEVI"
            Else
                mObjBriefanforderung.Zusatzanschreiben(1) = ""
            End If
        End If

        If rb_Selbstabholung.Visible AndAlso rb_Selbstabholung.Checked Then
            mObjBriefanforderung.Zusatzanschreiben(2) = "SELB"
        Else
            mObjBriefanforderung.Zusatzanschreiben(2) = ""

        End If

        If chbEingentumsvorbehaltEintragen.Visible = True Then
            If chbEingentumsvorbehaltEintragen.Checked Then

                mObjBriefanforderung.Eigentumsvorbehalt = "EVE"
            Else
                mObjBriefanforderung.Eigentumsvorbehalt = ""
            End If
        End If

        If chbEingentumsvorbehaltLoeschen.Visible = True Then
            If chbEingentumsvorbehaltLoeschen.Checked Then
                mObjBriefanforderung.Eigentumsvorbehalt = "EVL"
            Else
                mObjBriefanforderung.Eigentumsvorbehalt = ""
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub ddlVersandgrund_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlVersandgrund.SelectedIndexChanged
        doAbrufgrundAktion()
    End Sub

    Private Sub doAbrufgrundAktion()
        txtVersandGrundZusatzEingabe.Visible = CBool(mObjBriefanforderung.Abrufgruende.Select("SapWert='" & ddlVersandgrund.SelectedValue & "'")(0)("MitZusatzText"))
        lblVersandGrundZusatzBemerkung.Text = mObjBriefanforderung.Abrufgruende.Select("SapWert='" & ddlVersandgrund.SelectedValue & "'")(0)("Zusatzbemerkung").ToString
    End Sub

  
    Protected Sub lb_Suche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Suche.Click
        Dim tmpItem As ListItem

        If txtOrtSuche.Text.Length + txtPostleitzahlSuche.Text.Length + txtNameSuche.Text.Length > 0 Then

            Dim sQuery As String = ""

            If txtNameSuche.Text.Length > 0 Then
                sQuery += "Name1 LIKE '" & txtNameSuche.Text.Trim & "' AND "
            End If

            If txtOrtSuche.Text.Length > 0 Then
                sQuery += "CITY1 LIKE '" & txtOrtSuche.Text.Trim & "' AND "
            End If

            If txtPostleitzahlSuche.Text.Length > 0 Then
                sQuery += "POST_CODE1 LIKE '" & txtPostleitzahlSuche.Text.Trim & "' AND "
            End If

            sQuery = Left(sQuery, sQuery.Length - 4)


            Dim dv As New DataView(mObjBriefanforderung.VersandAdressen)
            dv.RowFilter = sQuery
            dv.Sort = "Name1 asc"
            Dim i As Int32 = 0
            ddlGeschaeftsstelle.Items.Clear()
            Do While i < dv.Count
                tmpItem = New ListItem(dv.Item(i)("Name1").ToString & " " & dv.Item(i)("Name2").ToString & ", " & dv.Item(i)("POST_CODE1").ToString & " " & dv.Item(i)("CITY1").ToString, dv.Item(i)("KUNNR").ToString)
                ddlGeschaeftsstelle.Items.Add(tmpItem)
                i += 1
            Loop
            tmpItem = New ListItem("- keine Auswahl -", "")
            ddlGeschaeftsstelle.Items.Insert(0, tmpItem)
            If dv.Count > 0 Then
                If dv.Count = 1 Then ddlGeschaeftsstelle.SelectedIndex = 1

                If ddlGeschaeftsstelle.Items.Count > 20 Then

                    ddlGeschaeftsstelle.Visible = False
                    lblSuche.Visible = True
                    tblSuche.Visible = True
                    lblSuche.Text = "Bitte über die Suchkriterien genauer eingrenzen!"

                Else
                    ddlGeschaeftsstelle.Visible = True
                    lb_NeuSuche.Visible = True
                    lblSuche.Visible = False
                    tblSuche.Visible = False
                End If
            Else
                lblSuche.ForeColor = Drawing.Color.Red
                lblSuche.Text = "Keine Geschäftstelle gefunden!"
                lblSuche.Visible = True
            End If


        Else
            txtNameSuche.BorderColor = Drawing.Color.Red
            txtOrtSuche.BorderColor = Drawing.Color.Red
            txtPostleitzahlSuche.BorderColor = Drawing.Color.Red
            lblSuche.ForeColor = Drawing.Color.Red
            lblSuche.Text = "Kein Suchkriterium gefüllt!"
        End If
    End Sub
    Protected Sub lb_NeuSuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_NeuSuche.Click
        ddlGeschaeftsstelle.Visible = False
        lblSuche.Visible = True
        tblSuche.Visible = True
        lb_NeuSuche.Visible = False
        lblSuche.ForeColor = Drawing.Color.Black
        lblSuche.Text = ""
    End Sub


    Protected Sub rb_Geschaeftsstelle_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Geschaeftsstelle.CheckedChanged
        tblVersandanschrift.Visible=False
        ddlPartneradressen.Visible=False

        If ddlGeschaeftsstelle.Items.Count <= 30 Then
            tblSuche.Visible = False
            lb_NeuSuche.Visible = False
            lblSuche.Visible = False
            ddlGeschaeftsstelle.Visible = True
        Else
            lblSuche.Visible = True
            tblSuche.Visible = True
            ddlGeschaeftsstelle.Visible = False
            lblSuche.Text = ""
        End If
    End Sub

    Protected Sub rb_Partneradresse_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Partneradresse.CheckedChanged
        tblVersandanschrift.Visible=False
        ddlPartneradressen.Visible=True
        ddlGeschaeftsstelle.Visible = False
       lb_NeuSuche.Visible = False
        lblSuche.Visible = False
        tblSuche.Visible = False
    End Sub

    Protected Sub rb_Versandanschrift_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Versandanschrift.CheckedChanged
        tblVersandanschrift.Visible=true
        ddlPartneradressen.Visible=False
        ddlGeschaeftsstelle.Visible = False
       lb_NeuSuche.Visible = False
        lblSuche.Visible = False
        tblSuche.Visible = False
    End Sub
End Class
' ************************************************
' $History: Change02_2.aspx.vb $
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 14.01.11   Time: 10:52
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA: 4491(CPC)
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 16.09.10   Time: 17:48
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 22.12.09   Time: 14:42
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA: 3408
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 8.06.09    Time: 17:07
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2909
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 28.01.09   Time: 13:31
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2561
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 28.11.08   Time: 9:14
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' fehlerhafte rückwärtsnavigation fixed
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 11.11.08   Time: 8:52
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2372 + 2367 (Nachbesserung)
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 10.11.08   Time: 15:34
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2367 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 30.10.08   Time: 11:51
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' weiterentwicklung
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 29.10.08   Time: 16:27
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 unfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 27.10.08   Time: 17:20
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2286 Änderungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.10.08   Time: 11:15
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 Weiterentwicklung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 21.10.08   Time: 17:11
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 weiterentwicklung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 20.10.08   Time: 15:37
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' 2284
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 17.10.08   Time: 15:27
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 weiterentwicklung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 17.10.08   Time: 13:48
' Updated in $/CKAG/Applications/AppCommonLeasing/Forms
' ITA 2284 torso
'
' ************************************************