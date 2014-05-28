Imports CKG.Base.Business
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change01_3
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

    Private objHaendler As LT_01c

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents trZeit As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd3 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd5 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents chkZulassungsstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkZweigstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk1200 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk1000 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk0900 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkVersandStandard As System.Web.UI.WebControls.RadioButton
    Protected WithEvents txtName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPlz As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlGrund As System.Web.UI.WebControls.DropDownList
    Protected WithEvents litScript As System.Web.UI.WebControls.Literal
   Protected WithEvents txtName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLand As System.Web.UI.WebControls.TextBox
    Protected WithEvents trVersandAdrEnd6 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trVersandAdrEnd4 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents txtGrundBemerkung As System.Web.UI.WebControls.TextBox
    Protected WithEvents tdInput1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tdInput As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblVersandAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents ddlZulDienst As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblLeasingnehmer As System.Web.UI.WebControls.Label
    Protected WithEvents ddlLeasingnehmer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents ddlLieferant As System.Web.UI.WebControls.DropDownList
    Protected WithEvents trVersandAdrTemp As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlFormular As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblVersandgrund As System.Web.UI.WebControls.Label
    Protected WithEvents lblFormularart As System.Web.UI.WebControls.Label
   Private versandart As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        versandart = Request.QueryString.Item("art")

        lnkFahrzeugAuswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        'Cursor setzen
        If versandart = "ENDG" Then
            litScript.Text = "		<script language=""JavaScript"">"
            litScript.Text &= "			<!-- //" & vbCrLf
            litScript.Text &= "				 window.document.Form1.txtName1.focus();"
            litScript.Text &= "			//-->" & vbCrLf
            litScript.Text &= "		</script>"
            ddlGrund.Visible = False
            lblVersandgrund.Visible = False
        End If
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString & "art=" & versandart)
            End If
            objHaendler = CType(Session("objHaendler"), LT_01c)

            If Not IsPostBack Then
                initialLoad()
                
                ddlLeasingnehmer.AutoPostBack = True
                ddlLieferant.AutoPostBack = True
                ddlZulDienst.AutoPostBack = True

                If (versandart = "ENDG") Then
                    tdInput.Visible = False
                    tdInput1.Visible = False
                End If
            Else

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub initialLoad()
        Dim tblZulst As DataTable
        Dim tblLeasingnehmer As DataTable
        Dim tblLieferant As DataTable
        Dim view As DataView
        Dim row As DataRow
        Dim tblTemp As DataTable
        Dim newRow As DataRow
        Dim counter As Integer

        'Tabellen für Zulassungsstellen, Leasingnehmer und Lieferanten füllen 
        objHaendler.Fill_Zulst_Leasingnehmer_Lieferant(Me)


        counter = 0
        If (versandart = "TEMP") Then


            '*******Zulassungsstellen in die Dropdown eintragen********
            tblTemp = objHaendler.ZulstTabelle.Copy   'Kopie erstellen
            tblTemp.Clear()
            tblTemp.Columns("ORT01").MaxLength = 100
            tblTemp.AcceptChanges()
            tblZulst = objHaendler.ZulstTabelle

            'Leereintrag hinzufügen
            newRow = tblTemp.NewRow
            newRow("ORT01") = "-- Keine Auswahl --"
            newRow("PSTL2") = "0"
            tblTemp.Rows.Add(newRow)
            tblTemp.AcceptChanges()


            For Each row In tblZulst.Rows
                If row("ORT01").ToString <> String.Empty Then
                    newRow = tblTemp.NewRow
                    newRow("LIFNR") = row("LIFNR").ToString
                    newRow("ORT01") = row("ORT01").ToString
                    If row("STRAS").ToString <> String.Empty Then
                        newRow("ORT01") = newRow("ORT01") & ", " & row("STRAS")     '§§§ JVE 24.02.2006 Auch die Strasse mit anzeigen!
                    End If
                    newRow("STRAS") = row("STRAS").ToString
                    newRow("PSTLZ") = row("PSTLZ").ToString
                    newRow("PSTL2") = CType(counter, String)                        'Lfd. Nr. (für spätere Suche!)
                    tblTemp.Rows.Add(newRow)
                    tblTemp.AcceptChanges()
                    counter += 1
                End If
            Next

            tblZulst = tblTemp.Copy
            tblZulst.AcceptChanges()
            tblTemp = Nothing

            Session.Add("tblZulst", tblZulst)



            view = tblZulst.DefaultView
            view.Sort = "ORT01"
            With ddlZulDienst
                .DataSource = view
                .DataTextField = "ORT01"
                .DataValueField = "PSTL2"
                .DataBind()
            End With


            lblPageTitle.Text = " (Versandadresse, Versandgrund und -art wählen)"


            'Gründe Füllen...
            view = objHaendler.GrundTabelle.DefaultView
            view.Sort = "TEXT50"
            With ddlGrund
                .DataSource = view
                .DataTextField = "TEXT50"
                .DataValueField = "ZZVGRUND"
                .DataBind()
            End With


        Else

            ddlFormular.AutoPostBack = True

            'Listenart füllen
            With ddlFormular
                .Items.Add(New ListItem("Leasing, freundliches Anschreiben", "T3"))
                .Items.Add(New ListItem("Leasing, kurzes Anschreiben", "T4"))
                .Items.Add(New ListItem("Miete/ Kauf, freundliches Anschreiben", "T6"))
                .Items.Add(New ListItem("Miete/Kauf, kurzes Anschreiben", "T7"))
            End With

            trZeit.Visible = True
            lblVersandAdresse.Visible = False
            ddlZulDienst.Visible = False
            lblVersandgrund.Visible = False
            ddlGrund.Visible = False
            ddlFormular.Visible = True
            lblFormularart.Visible = True

            lblPageTitle.Text = " (Versandadresse eingeben)"
        End If



        'Leasingnehmer füllen...
        tblTemp = objHaendler.LeasingnehmerTabelle.Copy   'Kopie erstellen
        tblTemp.Clear()
        tblLeasingnehmer = objHaendler.LeasingnehmerTabelle

        'Leereintrag hinzufügen
        newRow = tblTemp.NewRow
        newRow("NAME1") = "-- Keine Auswahl --"
        newRow("KUNNR") = "0"
        tblTemp.Rows.Add(newRow)
        tblTemp.AcceptChanges()


        For Each row In tblLeasingnehmer.Rows
            newRow = tblTemp.NewRow
            newRow("NAME1") = row("NAME1").ToString
            newRow("KUNNR") = row("KUNNR").ToString
            tblTemp.Rows.Add(newRow)
            tblTemp.AcceptChanges()
        Next

        tblLeasingnehmer = tblTemp.Copy
        tblLeasingnehmer.AcceptChanges()
        tblTemp = Nothing

        Session.Add("tblLeasingnehmer", tblLeasingnehmer)


        view = tblLeasingnehmer.DefaultView
        view.Sort = "NAME1"
        With ddlLeasingnehmer
            .DataSource = view
            .DataTextField = "NAME1"
            .DataValueField = "KUNNR"
            .DataBind()
        End With

        'Lieferanten füllen...

        tblTemp = objHaendler.LieferantTabelle.Copy   'Kopie erstellen
        tblTemp.Clear()
        tblLieferant = objHaendler.LieferantTabelle

        'Leereintrag hinzufügen
        newRow = tblTemp.NewRow
        newRow("NAME1") = "-- Keine Auswahl --"
        newRow("KUNNR") = "0"
        tblTemp.Rows.Add(newRow)
        tblTemp.AcceptChanges()


        For Each row In tblLieferant.Rows
            newRow = tblTemp.NewRow
            newRow("NAME1") = row("NAME1").ToString
            newRow("KUNNR") = row("KUNNR").ToString
            tblTemp.Rows.Add(newRow)
            tblTemp.AcceptChanges()
        Next

        tblLieferant = tblTemp.Copy
        tblLieferant.AcceptChanges()
        tblTemp = Nothing

        Session.Add("tblLieferant", tblLieferant)

        view = tblLieferant.DefaultView
        view.Sort = "NAME1"
        With ddlLieferant
            .DataSource = view
            .DataTextField = "NAME1"
            .DataValueField = "KUNNR"
            .DataBind()
        End With


    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If (versandart = "TEMP") Then
            DoSubmit()
        Else
            If (txtName1.Text = "" Or txtStr.Text = "" Or txtPlz.Text = "" Or txtOrt.Text = "") Then
                lblError.Text = "Bitte alle Felder füllen."
            Else
                DoSubmit()
            End If
        End If
    End Sub

    Private Sub DoSubmit()
        Dim status As String
        Dim tblZulst As DataTable

        status = String.Empty

        tblZulst = CType(Session("tblZulst"), DataTable)

        'UH 14.06.2005
        'Jetzt auch für endgültig:
        If chkVersandStandard.Checked Then                                'Versandart
            objHaendler.Materialnummer = "5530"
        ElseIf chk0900.Checked Then
            objHaendler.Materialnummer = "1385"
        ElseIf chk1000.Checked Then
            objHaendler.Materialnummer = "1389"
        ElseIf chk1200.Checked Then
            objHaendler.Materialnummer = "1390"
        End If

        If (versandart = "TEMP") Then

            objHaendler = CType(Session("objHaendler"), LT_01c)

            objHaendler.Bemerkung = txtGrundBemerkung.Text
            objHaendler.VersandGrund = ddlGrund.SelectedItem.Value.ToString
            objHaendler.VersandGrundText = ddlGrund.SelectedItem.Text

            objHaendler.Listenart = "T5"
            objHaendler.ListenartText = "Standard temp. Versand"


        Else
            objHaendler.Listenart = ddlFormular.SelectedItem.Value
            objHaendler.ListenartText = ddlFormular.SelectedItem.Text

        End If

        objHaendler.ZielFirma = txtName1.Text                               'Adresse für Endgültigen Versand
        objHaendler.ZielFirma2 = txtName2.Text                               'Adresse für Endgültigen Versand
        objHaendler.ZielStrasse = txtStr.Text
        objHaendler.ZielPLZ = txtPlz.Text
        objHaendler.ZielOrt = txtOrt.Text
        objHaendler.ZielLand = txtLand.Text
        objHaendler.VersandAdresseText = objHaendler.ZielFirma & "<br>" & objHaendler.ZielFirma2 & "<br>" & objHaendler.ZielStrasse & "&nbsp; " & objHaendler.ZielHNr & "<br>" & objHaendler.ZielPLZ & "&nbsp; " & objHaendler.ZielOrt


        Session("objHaendler") = objHaendler
        Response.Redirect("Change01_4.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
    End Sub
    
    Private Sub ddlZulDienst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlZulDienst.SelectedIndexChanged

        Dim tblZulst As DataTable


        If ddlZulDienst.SelectedItem.Value <> "0" Then

            ddlLeasingnehmer.SelectedItem.Selected = False
            ddlLieferant.SelectedItem.Selected = False

            ddlLeasingnehmer.Items.FindByValue("0").Selected = True
            ddlLieferant.Items.FindByValue("0").Selected = True

            Dim dv As DataView

            tblZulst = CType(Session("tblZulst"), DataTable)


            dv = tblZulst.DefaultView

            dv.RowFilter = "PSTL2 = '" & ddlZulDienst.SelectedItem.Value() & "'"

            txtName1.Text = "Zulassungsstelle"
            txtName2.Text = String.Empty
            txtStr.Text = dv.Item(0)("STRAS")
            txtPlz.Text = dv.Item(0)("PSTLZ")
            txtOrt.Text = Left(dv.Item(0)("ORT01"), InStr(dv.Item(0)("ORT01"), ",") - 1)
            txtLand.Text = "DE"

        End If
    End Sub

    Private Sub ddlLeasingnehmer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlLeasingnehmer.SelectedIndexChanged
        If ddlLeasingnehmer.SelectedItem.Value <> "0" Then
            If (versandart = "TEMP") Then
                ddlZulDienst.SelectedItem.Selected = False
                ddlZulDienst.Items.FindByValue("0").Selected = True
            End If

            ddlLieferant.SelectedItem.Selected = False
            ddlLieferant.Items.FindByValue("0").Selected = True

            Dim dv As DataView

            dv = objHaendler.LeasingnehmerTabelle.DefaultView

            dv.RowFilter = "KUNNR = '" & ddlLeasingnehmer.SelectedItem.Value() & "'"

            txtName1.Text = dv.Item(0)("NAME1")
            txtName2.Text = dv.Item(0)("NAME2")
            txtStr.Text = dv.Item(0)("STREET")
            txtPlz.Text = dv.Item(0)("POST_CODE1")
            txtOrt.Text = dv.Item(0)("CITY1")
            txtLand.Text = dv.Item(0)("COUNTRY")
        End If

    End Sub
    
    Private Sub ddlLieferant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlLieferant.SelectedIndexChanged
        If ddlLieferant.SelectedItem.Value <> "0" Then

            If (versandart = "TEMP") Then
                ddlZulDienst.SelectedItem.Selected = False
                ddlZulDienst.Items.FindByValue("0").Selected = True
            End If

            ddlLeasingnehmer.SelectedItem.Selected = False
            ddlLeasingnehmer.Items.FindByValue("0").Selected = True


            Dim dv As DataView

            dv = objHaendler.LieferantTabelle.DefaultView

            dv.RowFilter = "KUNNR = '" & ddlLieferant.SelectedItem.Value() & "'"

            txtName1.Text = dv.Item(0)("NAME1")
            txtName2.Text = dv.Item(0)("NAME2")
            txtStr.Text = dv.Item(0)("STREET")
            txtPlz.Text = dv.Item(0)("POST_CODE1")
            txtOrt.Text = dv.Item(0)("CITY1")
            txtLand.Text = dv.Item(0)("COUNTRY")

        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change01_3.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 14.01.10   Time: 15:55
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.06.09   Time: 15:36
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.06.09   Time: 14:58
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' ITA 2918  Z_M_FEHLENDE_VERTRAGSNUMMERN, Z_M_BRIEFLEBENSLAUF_LT,
' Z_M_BRIEFERSTEINGANG_LEASETR, Z_M_BRIEFANFORDERUNG_ALLG,
' Z_M_UNANGEFORDERT_L, Z_M_KUNDENDATEN_LT, Z_M_VERSENDETE_ZB2_ENDG_LT,
' Z_M_VERSENDETE_ZB2_TEMP_LT, Z_M_BRIEF_TEMP_VERS_MAHN_002,
' Z_M_DATEN_OHNE_ZB2_LT
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:12
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 15:07
' Created in $/CKAG/Applications/AppLeaseTrend/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 13:19
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' 
' ************************************************
