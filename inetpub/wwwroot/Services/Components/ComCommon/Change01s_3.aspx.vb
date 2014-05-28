Imports CKG.Base.Business
Imports CKG.Base.Kernel

Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change01s_3
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    'Private objSuche As Search
    'Private objAddressList As Search

    Private objHaendler As Versand1
    Private versandart As String
    Private authentifizierung As String
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        FormAuth(Me, m_User)

        versandart = Request.QueryString.Item("art")
        authentifizierung = Request.QueryString.Item("art2").ToString

        lnkFahrzeugAuswahl.NavigateUrl = "Change01s_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung
        lnkFahrzeugsuche.NavigateUrl = "Change01s.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung


        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If

        m_App = New Base.Kernel.Security.App(m_User)

        If Session("objHaendler") Is Nothing Then
            Response.Redirect("Change01s.aspx?AppID=" & Session("AppID").ToString & "art=" & versandart & "&art2=" & authentifizierung)
        End If
        objHaendler = CType(Session("objHaendler"), Versand1)


        If Not rb_SAPAdresse.Checked Then
            tblSuche.Visible = False
        Else
            tblSuche.Visible = True
        End If


        If Not IsPostBack Then
            initialLoad()
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click


        If rb_Zulst.Checked = False AndAlso ddlLand.Visible = True Then
            If CInt(objHaendler.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(objHaendler.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtPlz.Text.Trim(" "c).Length Then
                    lblError.Text = "Postleitzahl hat falsche Länge."
                    Exit Sub
                End If
            End If
        End If

        If rb_SAPAdresse.Checked = True Then
            If ddlSAPAdresse.SelectedItem Is Nothing Then
                lblError.Text = "Keine Versandadresse ausgewählt."
                Exit Sub
            End If
        End If

        If rb_VersandAdresse.Checked AndAlso rb_VersandAdresse.Visible Then '# 1015 - 04.05.07 TB Auch auf Sichtbarkeit prüfen, da komischerweise sowohl rb_Versandadresse als auch rb_SAPAdresse gesetzt sein können, wenn rb_Versandadresse unsichtbar ist
            If (txtName1.Text = "" Or txtStr.Text = "" Or txtPlz.Text = "" Or txtNr.Text = "" Or txtOrt.Text = "") Then
                lblError.Text = "Bitte alle Felder füllen."
                Exit Sub
            End If
        End If

        If txtAuf.Visible Then
            If txtAuf.Text.Trim.Length = 0 Then
                lblError.Text = "Bitte geben Sie an, auf wen die " & ddlGrund.SelectedItem.Text & " erfolgen soll."
            Else
                DoSubmit()
            End If
        Else
            DoSubmit()
        End If
    End Sub
    Private Sub rb_Zulst_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_Zulst.CheckedChanged
        checkZulst()
    End Sub

    Private Sub rb_SAPAdresse_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_SAPAdresse.CheckedChanged
        checkSAPAdresse()
    End Sub

    Private Sub rb_VersandAdresse_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_VersandAdresse.CheckedChanged
        checkVersand()
    End Sub

    Private Sub ddlGrund_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrund.SelectedIndexChanged
        checkGrund()
    End Sub

    Private Sub btnSucheAdresse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSucheAdresse.Click
        'Hinterlegte Adressen
        Dim view0 As DataView
        Dim strName As String
        Dim strPLZ As String

        strName = txtSucheName.Text
        strPLZ = txtSuchePLZ.Text

        If (strName.Trim = String.Empty) Then
            lblError.Text = "Bitte den kompletten oder einen Teil des Namens eingeben!"
            Exit Sub
        End If

        Try
            If objHaendler.LeseAdressenSAP(Me.Page, m_User.Customer.KUNNR, strName, strPLZ) > 0 Then
                view0 = objHaendler.Adressen.DefaultView
                view0.Sort = "DISPLAY_ADDRESS"
                With ddlSAPAdresse
                    .DataSource = view0
                    .DataTextField = "DISPLAY_ADDRESS"
                    .DataValueField = "ADDRESSNUMBER"
                    .DataBind()
                End With
                ddlSAPAdresse.Visible = True
            Else
                ddlSAPAdresse.Visible = False
                rb_SAPAdresse.Enabled = False
            End If
            lblError.Text = String.Empty
            'lblInfo.Text = "Es wurde(n) " & ddlSAPAdresse.Items.Count & " Adresse(n) gefunden."

        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                    lblError.Text = "Keine Daten gefunden!"
                    ddlSAPAdresse.Visible = False
                Case "NO_LIST"
                    ddlSAPAdresse.Visible = False
                    lblError.Text = "Es wurden mehr als 200 Treffer gefunden. Bitte Suche weiter einschränken!"
            End Select
        End Try


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If m_User.Groups(0).Authorizationright < 3 Then
            trVersandAdrEnd0.Visible = False
            trVersandAdrEnd1.Visible = False
            trVersandAdrEnd2.Visible = False
            trVersandAdrEnd3.Visible = False
            trVersandAdrEnd5.Visible = False
            tr_Land.Visible = False
        End If
        If m_User.Groups(0).Authorizationright < 2 Then
            trSAPAdresse.Visible = False
        End If
        If m_User.Groups(0).Authorizationright < 1 Then
            tr_ZulstSichtbarkeit.Visible = False
            cmdSave.Enabled = False
        End If

        SetEndASPXAccess(Me)

        If objHaendler.Materialnummer Is Nothing OrElse objHaendler.Materialnummer Is String.Empty Then
            'sollte kein Rücksprung sein, sollen der RB checked sein der als erstes nach unten stehender reihenfolge sichtbar ist 
            'muss nach feldübersetzung passieren da dort visible werte der Contorls gesetzt werdenJJ2008.02.19
            'was unverständlich bleibt ist das nach einem postback diese sache nicht mehr funktioniert, weil
            'wenn ein fehler auftritt(z.B. Eingabe vergessen) und ein RB per hand ausgewählt worden ist, 
            'ist eine checken programatisch hier nicht mehr möglich, ist aber auch gut so da es so richtig ist. JJU2008.02.27
            Dim alRBs As New ArrayList()
            alRBs.Add(rb_VersandStandard)
            alRBs.Add(rb_Sendungsverfolgt)
            alRBs.Add(rb_0900)
            alRBs.Add(rb_1000)
            alRBs.Add(rb_1200)
            selectVisibleRadiobutton(alRBs)
        End If

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methods"

    Private Sub initialLoad()
        Dim zulDienste As New DataTable()
        Dim status As String = ""
        Dim item As ListItem
        Dim view0 As DataView
        Dim view As DataView
        Dim row As DataRow
        Dim tblTemp As DataTable
        Dim newRow As DataRow
        Dim counter As Integer

        Dim key As String
        Dim rows As DataRow()
        Dim sprache As String

        'Länder DLL füllen
        ddlLand.DataSource = objHaendler.Laender
        'ddlLand.DataTextField = "Beschreibung"
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


        zulDienste = New DataTable()

        counter = 0
        If (versandart = "TEMP") Or (versandart = "ENDGZUL") Then
            'Zulassungsdientste holen
            objHaendler.getZulassungsdienste(Me.Page, zulDienste, status)

            'tblTemp = zulDienste.Copy   'Kopie erstellen
            'tblTemp.Clear()
            tblTemp = zulDienste.Clone
            tblTemp.Columns.Add("DISPLAY")

            For Each row In zulDienste.Rows
                If row("ORT01").ToString <> String.Empty And row("LIFNR").ToString <> String.Empty Then
                    Dim intTemp As Integer = InStr(row("ORT01").ToString, "ZLS")
                    Dim intTemp2 As Integer = InStr(row("ORT01").ToString, "geschl")
                    If Not (intTemp > 0 AndAlso intTemp2 > intTemp) Then
                        newRow = tblTemp.NewRow
                        newRow("DISPLAY") = row("PSTLZ").ToString & " - " & row("ORT01").ToString & " - " & row("STRAS").ToString
                        newRow("LIFNR") = row("LIFNR").ToString
                        newRow("ORT01") = row("ORT01").ToString
                        newRow("STRAS") = row("STRAS").ToString
                        newRow("PSTLZ") = row("PSTLZ").ToString
                        newRow("PSTL2") = CType(counter, String)                  'Lfd. Nr. (für spätere Suche!)
                        tblTemp.Rows.Add(newRow)
                        tblTemp.AcceptChanges()
                        counter += 1
                    End If
                End If
            Next

            zulDienste = tblTemp.Copy
            zulDienste.AcceptChanges()
            tblTemp = Nothing

            Session.Add("ZulDienste", zulDienste)

            view = zulDienste.DefaultView
            view.Sort = "DISPLAY"
            With ddlZulDienst
                .DataSource = view
                .DataTextField = "DISPLAY"
                .DataValueField = "PSTL2"
                .DataBind()
            End With

            'lblHead.Text = " (Versandadresse, Versandgrund und -art wählen)"
        Else
            'lblHead.Text = " (Versandadresse eingeben)"
            '§§§JVE 29.03.2006 Versandgrund bei endg. Versand entfernen
            trVersandAdrEnd4.Visible = False
            objHaendler.VersandGrund = String.Empty
        End If

        'Gründe Füllen...
        For Each row In objHaendler.GrundTabelle.Rows
            item = New ListItem()
            item.Value = row("ZZVGRUND").ToString
            item.Text = row("TEXT50").ToString
            ddlGrund.Items.Add(item)
        Next
        '§§§JVE 29.03.2006 Versandgrund bei endg. Versand nicht prüfen
        If (versandart = "TEMP") Then
            checkGrund()
        End If
        'checkGrund()

        '------------------------------------------------------------------------------------
        'Weise frühere Eingaben zu nach Rücksprung

        'Versandart

        Select Case objHaendler.Materialnummer
            Case "2035"
                rb_VersandStandard.Checked = True
            Case "1391"
                rb_Sendungsverfolgt.Checked = True
            Case "1385"
                rb_0900.Checked = True
            Case "1389"
                rb_1000.Checked = True
            Case "1390"
                rb_1200.Checked = True
        End Select

        'Versandgrund

        If (versandart = "TEMP") Then
            If Not (objHaendler.VersandGrund = String.Empty) Then
                ddlGrund.ClearSelection()
                ddlGrund.Items.FindByValue(objHaendler.VersandGrund).Selected = True
                checkGrund()
                txtAuf.Text = objHaendler.Auf
            End If
        End If

        'Versandadresse: Zulassungsdienst (nur bei temp. Versand)

        If (versandart = "TEMP") Then
            If Not (objHaendler.VersandAdresse_ZE = String.Empty) Then
                key = objHaendler.VersandAdresse_ZE
                rows = zulDienste.Select("LIFNR='" & key & "'")

                If (Not rows Is Nothing) AndAlso (rows.GetUpperBound(0) > -1) Then
                    ddlZulDienst.ClearSelection()
                    ddlZulDienst.Items.FindByValue(rows(0)("Pstl2").ToString).Selected = True
                    txtBemerkung.Text = objHaendler.Betreff

                    ddlGrund.ClearSelection()
                    ddlGrund.Items.FindByValue(objHaendler.VersandGrund).Selected = True
                    checkGrund()

                    txtAuf.Text = objHaendler.Auf
                    rb_Zulst.Checked = True
                    checkZulst()
                End If
            End If
        End If

        'Versandadresse: Hinterlegte Adresse

        If Not (objHaendler.VersandAdresse_ZS = String.Empty) Then
            Dim rows2 As DataRow()
            key = objHaendler.VersandAdresse_ZS
            rows2 = objHaendler.Adressen.Select("ADDRESSNUMBER='" & key & "'")
            If (Not rows2 Is Nothing) AndAlso (rows2.GetUpperBound(0) > -1) Then
                'Werte zuweisen...
                view0 = objHaendler.Adressen.DefaultView
                view0.Sort = "DISPLAY_ADDRESS"
                With ddlSAPAdresse
                    .DataSource = view0
                    .DataTextField = "DISPLAY_ADDRESS"
                    .DataValueField = "ADDRESSNUMBER"
                    .DataBind()
                End With
                'Auswahl setzen..
                ddlSAPAdresse.ClearSelection()
                ddlSAPAdresse.Items.FindByValue(rows2(0)("ADDRESSNUMBER").ToString).Selected = True

                txtBemerkung.Text = objHaendler.Betreff

                rb_SAPAdresse.Checked = True
                ddlSAPAdresse.Visible = True
                tblSuche.Visible = True

                checkSAPAdresse()
            End If
        End If

        'Versandadresse: Versandanschrift

        If (objHaendler.ZielFirma <> String.Empty) And (objHaendler.ZielFirma2 <> String.Empty) And (objHaendler.ZielStrasse <> String.Empty) And (objHaendler.ZielPLZ <> String.Empty) And (objHaendler.ZielOrt <> String.Empty) Then
            txtName1.Text = objHaendler.ZielFirma
            txtName2.Text = objHaendler.ZielFirma2
            txtStr.Text = objHaendler.ZielStrasse
            txtNr.Text = objHaendler.ZielHNr
            txtPlz.Text = objHaendler.ZielPLZ
            txtOrt.Text = objHaendler.ZielOrt
            ddlLand.Items.FindByValue(objHaendler.ZielLand).Selected = True
            txtBemerkung.Text = objHaendler.Betreff

            rb_VersandAdresse.Checked = True
            checkVersand()
        End If

        'Zunächst Standardauswahl....

        If (versandart = "TEMP") Then
            If (rb_Zulst.Checked = False And rb_SAPAdresse.Checked = False And rb_VersandAdresse.Checked = False) Then
                rb_Zulst.Checked = True
                checkZulst()
            End If
        Else
            If rb_SAPAdresse.Checked = False And rb_VersandAdresse.Checked = False Then
                rb_VersandAdresse.Checked = True
                checkVersand()
            End If
        End If

        If (versandart = "TEMP") Or (versandart = "ENDGZUL") Then
            tr_ZulstSichtbarkeit.Visible = True
            If (versandart = "ENDGZUL") Then
                lblHead.Text = " (Versandadresse eingeben)"
            Else
                rb_VersandAdresse.Text = "Abweichende Versandanschrift"
            End If
        Else
            tr_ZulstSichtbarkeit.Visible = False
            lblHead.Text = " (Versandadresse eingeben)"
        End If
    End Sub

    Private Sub selectVisibleRadiobutton(ByVal al As ArrayList)
        Dim tmpRB As RadioButton
        For Each tmpRB In al
            If tmpRB.Visible = True Then
                tmpRB.Checked = True
                Exit Sub
            End If
        Next
    End Sub

    Private Sub checkGrund()
        If ddlGrund.SelectedItem.Text.Trim(" "c) = "Ummeldung" Or ddlGrund.SelectedItem.Text.Trim(" "c) = "Zulassung" Then
            txtAuf.Visible = True
            lbl_Auf.Visible = True
        Else
            txtAuf.Text = ""
            txtAuf.Visible = False
            lbl_Auf.Visible = False
        End If

    End Sub

    Private Sub DoSubmit()
        Dim status As String
        Dim key As String
        Dim row As DataRow()
        Dim zulDienste As DataTable

        status = String.Empty

        zulDienste = CType(Session("ZulDienste"), DataTable)

        If tr_Versandart1.Visible = False Then
            objHaendler.Materialnummer = "2035"
            objHaendler.MaterialBezeichnung = rb_VersandStandard.Text
        End If


        If rb_VersandStandard.Checked And rb_VersandStandard.Visible = True Then  'JJ2007.02.14 mit Sichtbarkeitsprüfung, da nicht sichtbare Contorls automatisch checked sin
            objHaendler.Materialnummer = "1391"
            objHaendler.MaterialBezeichnung = rb_VersandStandard.Text
        ElseIf rb_Sendungsverfolgt.Checked And rb_Sendungsverfolgt.Visible = True Then
            objHaendler.Materialnummer = "5530"
            objHaendler.MaterialBezeichnung = rb_Sendungsverfolgt.Text
        ElseIf rb_0900.Checked And rb_0900.Visible = True Then
            objHaendler.Materialnummer = "1385"
            objHaendler.MaterialBezeichnung = rb_0900.Text
        ElseIf rb_1000.Checked And rb_1000.Visible = True Then
            objHaendler.Materialnummer = "1389"
            objHaendler.MaterialBezeichnung = rb_1000.Text
        ElseIf rb_1200.Checked And rb_1200.Visible = True Then
            objHaendler.Materialnummer = "1390"
            objHaendler.MaterialBezeichnung = rb_1200.Text
        End If




        objHaendler = CType(Session("objHaendler"), Versand1)

        If rb_Zulst.Checked AndAlso rb_Zulst.Visible Then 'JJ2007.11.20 mit Sichtbarkeitsprüfung, da nicht sichtbare Contorls automatisch checked sin
            key = ddlZulDienst.SelectedItem.Value
            row = zulDienste.Select("Pstl2='" & key & "'")

            objHaendler.VersandAdresse_ZE = row(0)("LIFNR").ToString      'Versandadresse Nr. (60...)
            objHaendler.VersandAdresseText = ddlZulDienst.SelectedItem.Text   'Versanddresse (Text...)

            objHaendler.VersandAdresseText = "Zulassungsstelle" & "<br>" & row(0)("STRAS").ToString & "<br>" & row(0)("PSTLZ").ToString & "&nbsp; " & row(0)("ORT01").ToString

            objHaendler.Betreff = txtBemerkung.Text

            'SAP-Adresse nullen
            objHaendler.VersandAdresse_ZS = String.Empty

            'Manuelle Adresse nullen
            objHaendler.ZielFirma = String.Empty
            objHaendler.ZielFirma2 = String.Empty
            objHaendler.ZielStrasse = String.Empty
            objHaendler.ZielHNr = String.Empty
            objHaendler.ZielPLZ = String.Empty
            objHaendler.ZielOrt = String.Empty
            objHaendler.ZielLand = String.Empty
        ElseIf rb_SAPAdresse.Checked AndAlso rb_SAPAdresse.Visible Then 'JJ2007.11.20 mit Sichtbarkeitsprüfung, da nicht sichtbare Contorls automatisch checked sin
            objHaendler.VersandAdresse_ZS = ddlSAPAdresse.SelectedItem.Value
            objHaendler.VersandAdresseText = ddlSAPAdresse.SelectedItem.Text
            objHaendler.Betreff = txtBemerkung.Text

            'Zulassungsstelle nullen
            objHaendler.VersandAdresse_ZE = String.Empty

            'Manuelle Adresse nullen
            objHaendler.ZielFirma = String.Empty
            objHaendler.ZielFirma2 = String.Empty
            objHaendler.ZielStrasse = String.Empty
            objHaendler.ZielHNr = String.Empty
            objHaendler.ZielPLZ = String.Empty
            objHaendler.ZielOrt = String.Empty
            objHaendler.ZielLand = String.Empty
        ElseIf rb_VersandAdresse.Checked AndAlso rb_VersandAdresse.Visible Then 'JJ2007.11.20 mit Sichtbarkeitsprüfung, da nicht sichtbare Contorls automatisch checked sind
            objHaendler.ZielFirma = txtName1.Text                               'Adresse für Endgültigen Versand
            objHaendler.ZielFirma2 = txtName2.Text                               'Adresse für Endgültigen Versand
            objHaendler.ZielStrasse = txtStr.Text
            objHaendler.ZielHNr = txtNr.Text
            objHaendler.ZielPLZ = txtPlz.Text
            objHaendler.ZielOrt = txtOrt.Text
            objHaendler.ZielLand = ddlLand.SelectedItem.Value.ToString
            objHaendler.VersandAdresseText = objHaendler.ZielFirma & "<br>" & objHaendler.ZielFirma2 & "<br>" & objHaendler.ZielStrasse & "&nbsp; " & objHaendler.ZielHNr & "<br>" & objHaendler.ZielPLZ & "&nbsp; " & objHaendler.ZielOrt
            objHaendler.Betreff = txtBemerkung.Text

            'SAP-Adresse nullen
            objHaendler.VersandAdresse_ZS = String.Empty

            'Zulassungsstelle nullen
            objHaendler.VersandAdresse_ZE = String.Empty
        End If

        If trVersandAdrEnd4.Visible Then
            objHaendler.VersandGrund = ddlGrund.SelectedItem.Value.ToString
            objHaendler.VersandGrundText = ddlGrund.SelectedItem.Text
            objHaendler.Auf = txtAuf.Text
        End If
        Session("objHaendler") = objHaendler
        Response.Redirect("Change01s_4.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung)
    End Sub
    Private Sub checkZulst()
        ddlZulDienst.Enabled = True
        ddlSAPAdresse.Enabled = False
        txtName1.Enabled = False
        txtName2.Enabled = False
        txtStr.Enabled = False
        txtNr.Enabled = False
        txtPlz.Enabled = False
        txtOrt.Enabled = False
        ddlLand.Enabled = False
        'litScript.Text = ""
        If versandart = "TEMP" Then
            trVersandAdrEnd1.Visible = False
            trVersandAdrEnd2.Visible = False
            trVersandAdrEnd3.Visible = False
            '§§§JVE 29.03.2006 Versandgrund bei endg. Versand nicht einblenden
            If (versandart = "TEMP") Then
                trVersandAdrEnd4.Visible = True
            Else
                trVersandAdrEnd4.Visible = False
            End If

            trVersandAdrEnd5.Visible = True
            tr_Land.Visible = False
            'trVersandAdrEnd7.Visible = False
        End If

    End Sub

    Private Sub checkSAPAdresse()
        ddlZulDienst.Enabled = False
        ddlSAPAdresse.Enabled = True
        txtName1.Enabled = False
        txtName2.Enabled = False
        txtStr.Enabled = False
        txtNr.Enabled = False
        txtPlz.Enabled = False
        txtOrt.Enabled = False
        ddlLand.Enabled = False
        trVersandAdrEnd1.Visible = False
        trVersandAdrEnd2.Visible = False
        trVersandAdrEnd3.Visible = False

        If (versandart = "TEMP") Then
            trVersandAdrEnd4.Visible = True
        Else
            trVersandAdrEnd4.Visible = False
        End If
        trVersandAdrEnd5.Visible = False
        tr_Land.Visible = False
    End Sub

    Private Sub checkVersand()
        ddlZulDienst.Enabled = False
        ddlSAPAdresse.Enabled = False
        txtName1.Enabled = True
        txtName2.Enabled = True
        txtStr.Enabled = True
        txtNr.Enabled = True
        txtPlz.Enabled = True
        txtOrt.Enabled = True
        ddlLand.Enabled = True
        trVersandAdrEnd1.Visible = True
        trVersandAdrEnd2.Visible = True
        trVersandAdrEnd3.Visible = True


        If (versandart = "TEMP") Then
            trVersandAdrEnd4.Visible = True
        Else
            trVersandAdrEnd4.Visible = False
        End If

        trVersandAdrEnd5.Visible = False
        tr_Land.Visible = True
    End Sub

#End Region

End Class
' ************************************************
' $History: Change01s_3.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 11.03.10   Time: 15:53
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 2918
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 4.03.10    Time: 16:36
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 29.09.09   Time: 17:13
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 24.09.09   Time: 17:29
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 3112
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 24.09.09   Time: 10:44
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 3112
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 24.09.09   Time: 9:47
' Created in $/CKAG2/Services/Components/ComCommon
' 
