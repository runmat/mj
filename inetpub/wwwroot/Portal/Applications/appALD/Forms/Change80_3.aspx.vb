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

    Private objHaendler As ALD_1

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
    Protected WithEvents trZeit As System.Web.UI.HtmlControls.HtmlTableRow
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
    Protected WithEvents trVersandAdrEnd7 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtBetreff As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblAuf As System.Web.UI.WebControls.Label
    Protected WithEvents txtAuf As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBetreffHA As System.Web.UI.WebControls.TextBox
    Protected WithEvents trSAPAdresse As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSAPAdresseBetreff As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd4 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd8 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtBetreff2 As System.Web.UI.WebControls.TextBox
    Private versandart As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        versandart = Request.QueryString.Item("art")

        lnkFahrzeugAuswahl.NavigateUrl = "Change80_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkFahrzeugsuche.NavigateUrl = "Change80.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
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

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            If m_User.Organization.OrganizationAdmin Then
                lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
            End If
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change80.aspx?AppID=" & Session("AppID").ToString & "art=" & versandart)
            End If
            objHaendler = CType(Session("objHaendler"), ALD_1)

            If Not IsPostBack Then
                initialLoad()
            Else

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
        Dim rows As DataRow() = Nothing

        chk0900.Attributes.Add("onClick", "DoWarning()")
        chk1000.Attributes.Add("onClick", "DoWarning()")
        chk1200.Attributes.Add("onClick", "DoWarning()")

        zulDienste = New DataTable()

        'Hinterlegte Adressen
        If objHaendler.LeseAdressenSAP(m_User.Customer.KUNNR) > 0 Then
            view0 = objHaendler.Adressen.DefaultView
            view0.Sort = "DISPLAY_ADDRESS"
            With ddlSAPAdresse
                .DataSource = view0
                .DataTextField = "DISPLAY_ADDRESS"
                .DataValueField = "ADDRESSNUMBER"
                .DataBind()
            End With
        Else
            ddlSAPAdresse.Visible = False
            rb_SAPAdresse.Enabled = False
        End If

        counter = 0
        If (versandart = "TEMP") Then
            'Zulassungsdientste holen
            objHaendler.getZulassungsdienste(zulDienste, status)

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
        End If

        'Gründe Füllen...
        For Each row In objHaendler.GrundTabelle.Rows
            item = New ListItem()
            item.Value = row("ZZVGRUND").ToString
            item.Text = row("TEXT50").ToString
            ddlGrund.Items.Add(item)
        Next
        checkGrund()

        'Weise frühere Eingaben zu nach Rücksprung
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

        If (versandart = "TEMP") Then
            key = objHaendler.VersandAdresse_ZE
            rows = zulDienste.Select("LIFNR='" & key & "'")
        End If
        If (Not rows Is Nothing) AndAlso (rows.GetUpperBound(0) > -1) Then
            ddlZulDienst.ClearSelection()
            ddlZulDienst.Items.FindByValue(rows(0)("Pstl2").ToString).Selected = True
            txtBetreff2.Text = objHaendler.Betreff

            ddlGrund.ClearSelection()
            ddlGrund.Items.FindByValue(objHaendler.VersandGrund).Selected = True
            checkGrund()

            txtAuf.Text = objHaendler.Auf

            rb_Zulst.Checked = True
            checkZulst()
        Else
            Dim rows2 As DataRow()
            key = objHaendler.VersandAdresse_ZS
            rows2 = objHaendler.Adressen.Select("ADDRESSNUMBER='" & key & "'")
            If (Not rows2 Is Nothing) AndAlso (rows2.GetUpperBound(0) > -1) Then
                ddlSAPAdresse.ClearSelection()
                ddlSAPAdresse.Items.FindByValue(rows2(0)("ADDRESSNUMBER").ToString).Selected = True

                txtBetreffHA.Text = objHaendler.Betreff

                rb_SAPAdresse.Checked = True
                checkSAPAdresse()
            Else
                If objHaendler.ZielFirma <> String.Empty And objHaendler.ZielFirma2 <> String.Empty And objHaendler.ZielStrasse <> String.Empty And objHaendler.ZielPLZ <> String.Empty And objHaendler.ZielOrt <> String.Empty Then
                    txtName1.Text = objHaendler.ZielFirma
                    txtName2.Text = objHaendler.ZielFirma2
                    txtStr.Text = objHaendler.ZielStrasse
                    txtNr.Text = objHaendler.ZielHNr
                    txtPlz.Text = objHaendler.ZielPLZ
                    txtOrt.Text = objHaendler.ZielOrt
                    txtLand.Text = objHaendler.ZielLand
                    txtBetreff.Text = objHaendler.Betreff

                    rb_VersandAdresse.Checked = True
                    checkVersand()
                Else
                    If (versandart = "TEMP") Then
                        rb_Zulst.Checked = True
                        checkZulst()
                    Else
                        rb_VersandAdresse.Checked = True
                        checkVersand()
                    End If
                End If
            End If

        End If

        If (versandart = "TEMP") Then
            trZeit.Visible = True
            trZulst.Visible = True
            rb_VersandAdresse.Text = "Abweichende Versandanschrift"
        Else
            trZeit.Visible = True
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
        If ddlGrund.SelectedItem.Text.Trim(" "c) = "sonstiges" Then
            trVersandAdrEnd8.Visible = True
        Else
            trVersandAdrEnd8.Visible = False
            txtBetreff2.Text = ""
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If rb_VersandAdresse.Checked Then
            If (txtName1.Text = "" Or txtStr.Text = "" Or txtPlz.Text = "" Or txtNr.Text = "" Or txtOrt.Text = "") Then
                lblError.Text = "Bitte alle Felder füllen."
            Else
                DoSubmit()
            End If
        Else
            If txtAuf.Visible Then
                If txtAuf.Text.Trim.Length = 0 Then
                    lblError.Text = "Bitte geben Sie an, auf wen die " & ddlGrund.SelectedItem.Text & " erfolgen soll."
                Else
                    DoSubmit()
                End If
            Else
                DoSubmit()
            End If
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

        objHaendler = CType(Session("objHaendler"), ALD_1)

        If rb_Zulst.Checked Then
            key = ddlZulDienst.SelectedItem.Value
            row = zulDienste.Select("Pstl2='" & key & "'")

            objHaendler.VersandAdresse_ZE = row(0)("LIFNR").ToString      'Versandadresse Nr. (60...)
            objHaendler.VersandAdresseText = ddlZulDienst.SelectedItem.Text   'Versanddresse (Text...)

            objHaendler.VersandAdresseText = "Zulassungsstelle" & "<br>" & row(0)("STRAS").ToString & "<br>" & row(0)("PSTLZ").ToString & "&nbsp; " & row(0)("ORT01").ToString

            objHaendler.Betreff = txtBetreff2.Text

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
            objHaendler.Betreff = txtBetreffHA.Text

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
            objHaendler.Betreff = txtBetreff.Text

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
        Response.Redirect("Change80_4.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
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
        trSAPAdresseBetreff.Visible = False
        If versandart = "TEMP" Then
            trVersandAdrEnd1.Visible = False
            trVersandAdrEnd2.Visible = False
            trVersandAdrEnd3.Visible = False
            trVersandAdrEnd4.Visible = True
            trVersandAdrEnd5.Visible = True
            trVersandAdrEnd6.Visible = False
            trVersandAdrEnd7.Visible = False
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
        trSAPAdresseBetreff.Visible = True
        trVersandAdrEnd1.Visible = False
        trVersandAdrEnd2.Visible = False
        trVersandAdrEnd3.Visible = False
        trVersandAdrEnd4.Visible = False
        trVersandAdrEnd5.Visible = False
        trVersandAdrEnd6.Visible = False
        trVersandAdrEnd7.Visible = False
        trVersandAdrEnd8.Visible = False
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
        trVersandAdrEnd4.Visible = False
        trVersandAdrEnd5.Visible = False
        trVersandAdrEnd6.Visible = True
        trVersandAdrEnd7.Visible = True
        trVersandAdrEnd8.Visible = False
        trSAPAdresseBetreff.Visible = False
        'End If
    End Sub

    Private Sub rb_VersandAdresse_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_VersandAdresse.CheckedChanged
        checkVersand()
    End Sub

    Private Sub ddlGrund_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGrund.SelectedIndexChanged
        checkGrund()
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
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Forms
' 
' *****************  Version 15  *****************
' User: Uha          Date: 20.06.07   Time: 15:25
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 14  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' 
' ************************************************
