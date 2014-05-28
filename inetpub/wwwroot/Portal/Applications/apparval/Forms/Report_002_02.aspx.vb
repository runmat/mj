Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report_002_02
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    'Private m_objTable As DataTable
    'Private resultTable As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblLVNr As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblNameLN As System.Web.UI.WebControls.Label
    Protected WithEvents lblNameVG As System.Web.UI.WebControls.Label
    Protected WithEvents lblStrLN As System.Web.UI.WebControls.Label
    Protected WithEvents lblStrVG As System.Web.UI.WebControls.Label
    Protected WithEvents lblPLZLN As System.Web.UI.WebControls.Label
    Protected WithEvents lblPLZVG As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrtVG As System.Web.UI.WebControls.Label
    Protected WithEvents lblVschein As System.Web.UI.WebControls.Label
    Protected WithEvents lblKonzernID As System.Web.UI.WebControls.Label
    Protected WithEvents lblVBeginn As System.Web.UI.WebControls.Label
    Protected WithEvents lblVEnde As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandLN As System.Web.UI.WebControls.Label
    Protected WithEvents lblRueckLN As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandVG As System.Web.UI.WebControls.Label
    Protected WithEvents lblRueckVG As System.Web.UI.WebControls.Label
    Protected WithEvents lblMahnsLN As System.Web.UI.WebControls.Label
    Protected WithEvents lblMahnsVG As System.Web.UI.WebControls.Label
    Protected WithEvents lblMadatLN As System.Web.UI.WebControls.Label
    Protected WithEvents lblMadatVG As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersumfang As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFGNr As System.Web.UI.WebControls.Label
    Protected WithEvents Tr5 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents Label26 As System.Web.UI.WebControls.Label
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFzArt As System.Web.UI.WebControls.Label
    Protected WithEvents lblLEnde As System.Web.UI.WebControls.Label
    Protected WithEvents lblLBeginn As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblEz As System.Web.UI.WebControls.Label
    Protected WithEvents lblHerst As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents lblUnvLN As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblUnvVG As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAntrag As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersUmf As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblKonzs_ZK As System.Web.UI.WebControls.Label
    Protected WithEvents lblName1_ZK As System.Web.UI.WebControls.Label
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblName3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblStras_ZO As System.Web.UI.WebControls.Label
    Protected WithEvents lblPstlz_ZO As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrt_ZO As System.Web.UI.WebControls.Label
    Protected WithEvents lblKonzs_ZO As System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents Label32 As System.Web.UI.WebControls.Label
    Protected WithEvents Label33 As System.Web.UI.WebControls.Label
    Protected WithEvents Label34 As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrtLN As System.Web.UI.WebControls.Label
    Protected WithEvents Label35 As System.Web.UI.WebControls.Label
    Protected WithEvents Label36 As System.Web.UI.WebControls.Label
    Protected WithEvents ddl1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label37 As System.Web.UI.WebControls.Label
    Protected WithEvents Label38 As System.Web.UI.WebControls.Label
    Protected WithEvents lblName2LN As System.Web.UI.WebControls.Label
    Protected WithEvents lblName3LN As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents Label39 As System.Web.UI.WebControls.Label
    Protected WithEvents Label40 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennz As System.Web.UI.WebControls.Label
    Protected WithEvents Label41 As System.Web.UI.WebControls.Label
    Protected WithEvents Label42 As System.Web.UI.WebControls.Label
    Protected WithEvents Label43 As System.Web.UI.WebControls.Label
    Protected WithEvents tblBem1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblBem2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents txtBem1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBem2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBem4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblBemerkungen As System.Web.UI.WebControls.Label
    Protected WithEvents txtBem3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblB1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblB2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblB3 As System.Web.UI.WebControls.Label
    Protected WithEvents tblBemerkungen As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblB4 As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        If (Session("ResultTableNative") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        End If

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Try
            m_App = New Base.Kernel.Security.App(m_User)
            If Not IsPostBack Then
                Showdata()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Showdata()
        m_App = New Base.Kernel.Security.App(m_User)
        If (Session("ResultTableNative") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
        End If

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        'Bemerkungsfelder nur editierbar, wenn "Klärfall"
        Dim kf As String

        kf = Request.QueryString("kf")

        txtBem1.Enabled = False
        txtBem2.Enabled = False
        txtBem3.Enabled = False
        txtBem4.Enabled = False
        btnSave.Visible = False
        lblBemerkungen.Visible = False
        tblBemerkungen.Visible = False

        If (Not (kf Is Nothing)) AndAlso (kf <> String.Empty) Then
            txtBem1.Enabled = True
            txtBem2.Enabled = True
            txtBem3.Enabled = True
            txtBem4.Enabled = True
            btnSave.Visible = True
            lblBemerkungen.Visible = True
            tblBemerkungen.Visible = True
        End If

        'Daten filtern
        Dim equi As String
        equi = Request.Item("Equipment")    'Equipment-Nr. holen
        Dim data As DataTable
        Dim rowIndex As Integer = 0
        Dim selectedRow As DataRow

        data = CType(Session("ResultTableNative"), DataTable)
        While data.Rows(rowIndex)("Equipment").ToString <> equi
            rowIndex += 1
        End While
        selectedRow = data.Rows(rowIndex)


        lblB1.Text = CStr(selectedRow("Bemerkung1"))
        lblB2.Text = CStr(selectedRow("Bemerkung2"))
        lblB3.Text = CStr(selectedRow("Bemerkung3"))
        lblB4.Text = CStr(selectedRow("Bemerkung4"))


        'Langtext holen
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As New Arval_2(m_User, m_App, strFileName)
        Dim langtext As DataTable

        Try
            langtext = m_Report.GetLangText(Session("AppID").ToString, Session.SessionID.ToString, equi)
        Catch ex As Exception
            lblError.Text = "Fehler beim Laden des Langtextes."
            Exit Sub
        End Try

        If Not (langtext Is Nothing) Then
            If langtext.Rows.Count > 0 Then
                ddl1.Visible = True
                With ddl1
                    .Items.Clear()
                    .DataSource = langtext
                    .DataTextField = "Tdline"
                    .DataBind()
                End With
            End If
        Else
            ddl1.Visible = False
        End If

        '####### Daten darstellen
        lblInfo.Text = selectedRow("Info").ToString 'Info

        'Vertragsnummer, Status,Antrags-Nr.
        lblLVNr.Text = selectedRow(2).ToString    'LvNr.
        lblStatus.Text = selectedRow(10).ToString.Replace("Sicherungsschein", "")   'Status
        lblAntrag.Text = Arval_2.ToShortDateString(selectedRow(1).ToString)    'Angelegt.

        lblLBeginn.Text = Arval_2.ToShortDateString(selectedRow(5).ToString) 'Leasingbeginn
        lblLEnde.Text = Arval_2.ToShortDateString(selectedRow(6).ToString)   'Leasingende

        'Leasingnehmer
        lblNameLN.Text = selectedRow(7).ToString 'Leasingnehmer
        lblStrLN.Text = selectedRow("StrasseLN").ToString 'StrasseLN
        lblPLZLN.Text = selectedRow("PLZLeasingnehmer").ToString 'PLZLeasingnehmer
        lblOrtLN.Text = selectedRow("OrtLeasingnehmer").ToString 'OrtLeasingnehmer

        'Versicherungsgeber
        If Not IsDBNull(selectedRow(9)) Then lblNameVG.Text = CStr(selectedRow(9)) 'Versicherung
        If Not IsDBNull(selectedRow("StrasseVG")) Then lblStrVG.Text = CStr(selectedRow("StrasseVG")) 'StrasseVG
        If Not IsDBNull(selectedRow("PLZVersicherungsgeber")) Then lblPLZVG.Text = CStr(selectedRow("PLZVersicherungsgeber")) 'PLZVersicherungsgeber
        If Not IsDBNull(selectedRow("OrtVersicherungsgeber")) Then lblOrtVG.Text = CStr(selectedRow("OrtVersicherungsgeber")) 'OrtVersicherungsgeber

        lblVBeginn.Text = Arval_2.ToShortDateString(selectedRow(19).ToString) 'Versicherungsbeginn
        lblVEnde.Text = Arval_2.ToShortDateString(selectedRow(20).ToString)  'Versicherungsende
        lblVschein.Text = selectedRow(24).ToString  'VersicherungsSchein-Nr

        'Versanddatum, Rückgabedatum, Zurück LN + VG
        lblVersandLN.Text = Arval_2.ToShortDateString(selectedRow(11).ToString) 'VersandLN
        lblVersandVG.Text = Arval_2.ToShortDateString(selectedRow(15).ToString) 'RückgabeLN
        lblRueckLN.Text = Arval_2.ToShortDateString(selectedRow(14).ToString)   'VersandVG
        lblRueckVG.Text = Arval_2.ToShortDateString(selectedRow(18).ToString)  'RückgabeVG

        If Not IsDBNull(selectedRow("Name1")) Then lblName1_ZK.Text = CStr(selectedRow("Name1"))
        If Not IsDBNull(selectedRow("Name1GP")) Then lblName1.Text = CStr(selectedRow("Name1GP"))
        If Not IsDBNull(selectedRow("Name2")) Then lblName2.Text = CStr(selectedRow("Name2"))
        If Not IsDBNull(selectedRow("Name3")) Then lblName3.Text = CStr(selectedRow("Name3"))
        If Not IsDBNull(selectedRow("Strasse")) Then lblStras_ZO.Text = CStr(selectedRow("Strasse"))
        If Not IsDBNull(selectedRow("Postleitzahl")) Then lblPstlz_ZO.Text = CStr(selectedRow("Postleitzahl"))
        If Not IsDBNull(selectedRow("Ort")) Then lblOrt_ZO.Text = CStr(selectedRow("Ort"))
        If Not IsDBNull(selectedRow("Halternr")) Then lblKonzs_ZO.Text = CStr(selectedRow("Halternr"))
        If Not IsDBNull(selectedRow("Name2LN")) Then lblName2LN.Text = CStr(selectedRow("Name2LN"))
        If Not IsDBNull(selectedRow("Name3LN")) Then lblName3LN.Text = CStr(selectedRow("Name3LN"))

        txtBem1.Text = CStr(selectedRow("Text1"))
        txtBem2.Text = CStr(selectedRow("Text2"))
        txtBem3.Text = CStr(selectedRow("Text3"))
        txtBem4.Text = CStr(selectedRow("Text4"))

        'Versicherungsumfang
        lblVersUmf.Text = selectedRow("Versicherungsumfang").ToString

        Dim versandLNunv As String = Arval_2.ToShortDateString(selectedRow(27).ToString) 'VersandLNunv
        Dim rueckversandLNunv As String = Arval_2.ToShortDateString(selectedRow(29).ToString) 'RueckversandLNunv
        Dim versandLNunv2 As String = Arval_2.ToShortDateString(selectedRow(28).ToString) 'VersandLNunv2
        Dim rueckversandLNunv2 As String = Arval_2.ToShortDateString(selectedRow(30).ToString) 'RueckversandLNunv2

        If Not versandLNunv = String.Empty Then
            lblUnvLN.Items.Add(versandLNunv & " - Erneuter Versand")
        End If

        If Not rueckversandLNunv = String.Empty Then
            lblUnvLN.Items.Add(rueckversandLNunv & " - Rückversand")
        End If

        If Not versandLNunv2 = String.Empty Then
            lblUnvLN.Items.Add(versandLNunv2 & " - Erneuter Versand 2")
        End If

        If Not rueckversandLNunv2 = String.Empty Then
            lblUnvLN.Items.Add(rueckversandLNunv2 & " - Rückversand 2")
        End If

        Dim versandVGunv As String = Arval_2.ToShortDateString(selectedRow(31).ToString)
        Dim rueckversandVGunv As String = Arval_2.ToShortDateString(selectedRow(33).ToString)
        Dim versandVGunv2 As String = Arval_2.ToShortDateString(selectedRow(32).ToString)
        Dim rueckversandVGunv2 As String = Arval_2.ToShortDateString(selectedRow(34).ToString)

        If Not versandVGunv = String.Empty Then
            lblUnvVG.Items.Add(versandVGunv & " - Erneuter Versand")
        End If

        If Not rueckversandVGunv = String.Empty Then
            lblUnvVG.Items.Add(rueckversandVGunv & " - Rückversand")
        End If

        If Not versandVGunv2 = String.Empty Then
            lblUnvVG.Items.Add(versandVGunv2 & " - Erneuter Versand 2")
        End If

        If Not rueckversandVGunv2 = String.Empty Then
            lblUnvVG.Items.Add(rueckversandVGunv2 & " - Rückversand 2")
        End If

        'Wenn leer, Dropdownlisten verstecken...
        If (lblUnvLN.Items.Count = 0) Then
            lblUnvLN.Visible = False
        End If

        If (lblUnvVG.Items.Count = 0) Then
            lblUnvVG.Visible = False
        End If

        'Fahrzeugdaten
        lblEz.Text = Arval_2.ToShortDateString(selectedRow("Erstzulassung").ToString) 'Erstzulassung
        lblFzArt.Text = selectedRow(21).ToString 'Fahrzeugart
        lblFGNr.Text = selectedRow(3).ToString 'Fahrgestellnr
        lblHerst.Text = selectedRow(22).ToString 'Hersteller_Typ
        lblKennz.Text = selectedRow(4).ToString 'Kennzeichen

        'Mahndaten
        lblMahnsLN.Text = CStr(selectedRow(13)) 'MahnstufeLN
        lblMadatLN.Text = Arval_2.ToShortDateString(selectedRow(12).ToString) 'MahndatumLN
        lblMahnsVG.Text = CStr(selectedRow(17)) 'MahnstufeVG
        lblMadatVG.Text = Arval_2.ToShortDateString(selectedRow(16).ToString) 'MahndatumVG
        
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As New Arval_2(m_User, m_App, strFileName)
        Dim equi As String
        equi = Request.Item("Equipment")    'Equipment-Nr. holen

        Try
            m_Report.SaveComments(Session("AppID").ToString, Session.SessionID.ToString, _
                                  equi, txtBem1.Text, txtBem2.Text, txtBem3.Text, txtBem4.Text)
            lblError.Text = "Die Eingaben wurden gespeichert."
        Catch ex As Exception
            lblError.Text = ex.Message
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
' $History: Report_002_02.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Forms
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
