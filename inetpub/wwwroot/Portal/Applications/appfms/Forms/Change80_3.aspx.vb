Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change80_3
    Inherits System.Web.UI.Page

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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objAddressList As Search

    Private objHaendler As FMS_1

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ddlZulDienst As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents trVersandAdrEnd1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd5 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents chk1200 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk1000 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk0900 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkVersandStandard As System.Web.UI.WebControls.RadioButton
    Protected WithEvents txtName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPlz As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlGrund As System.Web.UI.WebControls.DropDownList
    Protected WithEvents litScript As System.Web.UI.WebControls.Literal
    Protected WithEvents txtGrund As System.Web.UI.WebControls.Label
    Protected WithEvents txtName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLand As System.Web.UI.WebControls.TextBox
    Protected WithEvents trVersandAdrEnd6 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents rb_Zulst As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_SAPAdresse As System.Web.UI.WebControls.RadioButton
    Protected WithEvents ddlSAPAdresse As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rb_VersandAdresse As System.Web.UI.WebControls.RadioButton
    Protected WithEvents trZulst As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblAuf As System.Web.UI.WebControls.Label
    Protected WithEvents txtAuf As System.Web.UI.WebControls.TextBox
    Protected WithEvents trSAPAdresse As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd4 As System.Web.UI.HtmlControls.HtmlTableRow
    Private versandart As String
    Protected WithEvents trVersandart1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandart2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtBemerkung As System.Web.UI.WebControls.TextBox
    Protected WithEvents trVersandAdrEnd9 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tblSuche As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblErrorSuche As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents txtSuchePLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSucheAdresse As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtSucheName As System.Web.UI.WebControls.TextBox
    Private authentifizierung As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        versandart = Request.QueryString.Item("art")
        authentifizierung = Request.QueryString.Item("art2").ToString

        lnkFahrzeugAuswahl.NavigateUrl = "Change80_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung
        lnkFahrzeugsuche.NavigateUrl = "Change80.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung
        'Cursor setzen
        If versandart = "ENDG" Then
            If txtName1.Enabled Then
                litScript.Text = "		<script language=""JavaScript"">"
                litScript.Text &= "			<!-- //" & vbCrLf
                litScript.Text &= "				 window.document.Form1.txtName1.focus();"
                litScript.Text &= "			//-->" & vbCrLf
                litScript.Text &= "		</script>"
            End If
        End If
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            If m_User.Organization.OrganizationAdmin Then
                lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
            End If
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change80.aspx?AppID=" & Session("AppID").ToString & "art=" & versandart & "&art2=" & authentifizierung)
            End If
            objHaendler = CType(Session("objHaendler"), FMS_1)

            '§§§ JVE 25.04.2006
            If Not rb_SAPAdresse.Checked Then
                tblSuche.Visible = False
            Else
                tblSuche.Visible = True
            End If

            '------------------
            If Not IsPostBack Then
                initialLoad()

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

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

        chk0900.Attributes.Add("onClick", "DoWarning()")
        chk1000.Attributes.Add("onClick", "DoWarning()")
        chk1200.Attributes.Add("onClick", "DoWarning()")

        zulDienste = New DataTable()

        '§§§ JVE 25.04.2006: Auskommentiert, da zuerst eine Suche durchgeführt werden muß (Name, PLZ)
        'Hinterlegte Adressen
        'If objHaendler.LeseAdressenSAP(m_User.Customer.KUNNR) > 0 Then
        '    view0 = objHaendler.Adressen.DefaultView
        '    view0.Sort = "DISPLAY_ADDRESS"
        '    With ddlSAPAdresse
        '        .DataSource = view0
        '        .DataTextField = "DISPLAY_ADDRESS"
        '        .DataValueField = "ADDRESSNUMBER"
        '        .DataBind()
        '    End With
        'Else
        '    ddlSAPAdresse.Visible = False
        '    rb_SAPAdresse.Enabled = False
        'End If

        counter = 0
        If (versandart = "TEMP") Then
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

            lblPageTitle.Text = " (Versandadresse, Versandgrund und -art wählen)"
        Else
            lblPageTitle.Text = " (Versandadresse eingeben)"
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
            Case "1391"
                chkVersandStandard.Checked = True
            Case "1385"
                chk0900.Checked = True
            Case "1389"
                chk1000.Checked = True
            Case "1390"
                chk1200.Checked = True
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
            txtLand.Text = objHaendler.ZielLand
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

        'Versandart ausblenden!
        trVersandart1.Visible = False
        trVersandart2.Visible = False

        If (versandart = "TEMP") Then
            trZulst.Visible = True
            rb_VersandAdresse.Text = "Abweichende Versandanschrift"
        Else
            trZulst.Visible = False
            lblPageTitle.Text = " (Versandadresse eingeben)"
        End If
    End Sub

    Private Sub checkGrund()
        If ddlGrund.SelectedItem.Text.Trim(" "c) = "Ummeldung" Or ddlGrund.SelectedItem.Text.Trim(" "c) = "Zulassung" Then
            txtAuf.Visible = True
            lblAuf.Visible = True
        Else
            txtAuf.Text = ""
            txtAuf.Visible = False
            lblAuf.Visible = False
        End If
        'If ddlGrund.SelectedItem.Text.Trim(" "c) = "sonstiges" Then
        '    trVersandAdrEnd8.Visible = True
        'Else
        '    trVersandAdrEnd8.Visible = False
        '    txtBetreff2.Text = ""
        'End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If rb_SAPAdresse.Checked = True Then
            If ddlSAPAdresse.SelectedItem Is Nothing Then
                lblError.Text = "Keine Versandadresse ausgewählt."
                Exit Sub
            End If
        End If

        If txtAuf.Visible Then
            If txtAuf.Text.Trim.Length = 0 Then
                lblError.Text = "Bitte geben Sie an, auf wen die " & ddlGrund.SelectedItem.Text & " erfolgen soll."
            Else
                If rb_VersandAdresse.Checked Then
                    If (txtName1.Text = "" Or txtStr.Text = "" Or txtPlz.Text = "" Or txtNr.Text = "" Or txtOrt.Text = "") Then
                        lblError.Text = "Bitte alle Felder füllen."
                    Else
                        DoSubmit()
                    End If
                Else
                    DoSubmit()
                End If
            End If
        Else
            DoSubmit()
        End If
    End Sub

    Private Sub DoSubmit()

        Dim status As String
        Dim key As String
        Dim row As DataRow()
        Dim zulDienste As DataTable

        status = String.Empty

        zulDienste = CType(Session("ZulDienste"), DataTable)

        If chkVersandStandard.Checked Then
            objHaendler.Materialnummer = "1391"
        ElseIf chk0900.Checked Then
            objHaendler.Materialnummer = "1385"
        ElseIf chk1000.Checked Then
            objHaendler.Materialnummer = "1389"
        ElseIf chk1200.Checked Then
            objHaendler.Materialnummer = "1390"
        End If

        objHaendler = CType(Session("objHaendler"), FMS_1)

        If rb_Zulst.Checked Then
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
        ElseIf rb_SAPAdresse.Checked Then
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
        Else
            objHaendler.ZielFirma = txtName1.Text                               'Adresse für Endgültigen Versand
            objHaendler.ZielFirma2 = txtName2.Text                               'Adresse für Endgültigen Versand
            objHaendler.ZielStrasse = txtStr.Text
            objHaendler.ZielHNr = txtNr.Text
            objHaendler.ZielPLZ = txtPlz.Text
            objHaendler.ZielOrt = txtOrt.Text
            objHaendler.ZielLand = txtLand.Text
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
        Response.Redirect("Change80_4.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung)
    End Sub

    Private Sub rb_Zulst_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_Zulst.CheckedChanged
        checkZulst()
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
        txtLand.Enabled = False
        litScript.Text = ""
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
            trVersandAdrEnd6.Visible = False
            'trVersandAdrEnd7.Visible = False
        End If

    End Sub

    Private Sub rb_SAPAdresse_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_SAPAdresse.CheckedChanged
        checkSAPAdresse()
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
        txtLand.Enabled = False
        litScript.Text = ""
        trVersandAdrEnd1.Visible = False
        trVersandAdrEnd2.Visible = False
        trVersandAdrEnd3.Visible = False

        'trVersandAdrEnd4.Visible immer sichtbar
        'trVersandAdrEnd4.Visible = False

        '§§§JVE 29.03.2006 Versandgrund bei endg. Versand nicht einblenden
        If (versandart = "TEMP") Then
            trVersandAdrEnd4.Visible = True
        Else
            trVersandAdrEnd4.Visible = False
        End If
        trVersandAdrEnd5.Visible = False
        trVersandAdrEnd6.Visible = False
        'trVersandAdrEnd7.Visible = False
        'trVersandAdrEnd8.Visible = False
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
        txtLand.Enabled = True
        'If versandart = "TEMP" Then
        trVersandAdrEnd1.Visible = True
        trVersandAdrEnd2.Visible = True
        trVersandAdrEnd3.Visible = True

        'trVersandAdrEnd4.Visible immer sichtbar
        'trVersandAdrEnd4.Visible = False

        '§§§JVE 29.03.2006 Nur einblenden, wenn Temp. Versand!
        If (versandart = "TEMP") Then
            trVersandAdrEnd4.Visible = True
        Else
            trVersandAdrEnd4.Visible = False
        End If

        trVersandAdrEnd5.Visible = False
        trVersandAdrEnd6.Visible = True
        'trVersandAdrEnd7.Visible = True
        'trVersandAdrEnd8.Visible = False
        'End If
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
            lblErrorSuche.Text = "Bitte den kompletten oder einen Teil des Namens eingeben!"
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
                'rb_SAPAdresse.Enabled = False
            End If
            lblErrorSuche.Text = String.Empty
            lblInfo.Text = "Es wurde(n) " & ddlSAPAdresse.Items.Count & " Adresse(n) gefunden."

        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                    lblErrorSuche.Text = "Keine Daten gefunden!"
                    ddlSAPAdresse.Visible = False
                Case "NO_LIST"
                    ddlSAPAdresse.Visible = False
                    lblErrorSuche.Text = "Es wurden mehr als 200 Treffer gefunden. Bitte Suche weiter einschränken!"
            End Select
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change80_3.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 16.03.10   Time: 13:45
' Updated in $/CKAG/Applications/appfms/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:17
' Updated in $/CKAG/Applications/appfms/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 4.03.10    Time: 14:24
' Updated in $/CKAG/Applications/appfms/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 4.03.10    Time: 10:03
' Updated in $/CKAG/Applications/appfms/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 15:58
' Updated in $/CKAG/Applications/appfms/Forms
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:44
' Created in $/CKAG/Applications/appfms/Forms
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 12:58
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Uha          Date: 7.03.07    Time: 13:11
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Forms
' 
' ******************************************
