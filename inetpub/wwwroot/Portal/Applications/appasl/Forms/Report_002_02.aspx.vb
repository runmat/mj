Imports CKG.Base.Business
Imports CKG.Base.Kernel
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
    Private m_objTable As DataTable
    Private resultTable As DataTable

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
        Else
            m_objTable = CType(Session("ResultTableNative"), DataTable)
        End If
        If Not (Session("Link")) = Nothing Then
            If Session("Link") = "Report_002" Then
                lblHead.Text = "Sicherungsscheine Historie"
            Else
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            End If
        Else
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        End If
        ucStyles.TitleText = lblHead.Text
        Try
            m_App = New Base.Kernel.Security.App(m_User)
            If Not IsPostBack Then
                showdata()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub showdata()
        m_App = New Base.Kernel.Security.App(m_User)
        If (Session("ResultTableNative") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
        Else
            resultTable = CType(Session("ResultTableNative"), DataTable)
        End If
        'lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString
        If Not (Session("Link")) = Nothing Then
            If Session("Link") = "Report_002" Then
                lblHead.Text = "Sicherungsscheine Historie"
            Else
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            End If
        Else
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        End If
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

        '§§§ JVE 14.07.2006
        tblBemerkungen.Visible = False
        'lblBemerk.Visible = False
        '------------------------

        If (Not (kf Is Nothing)) AndAlso (kf <> String.Empty) Then
            txtBem1.Enabled = True
            txtBem2.Enabled = True
            txtBem3.Enabled = True
            txtBem4.Enabled = True
            btnSave.Visible = True
            lblBemerkungen.Visible = True

            '§§§ JVE 14.07.2006
            'Nur dann die Bemerkungstabelle anzeigen    
            tblBemerkungen.Visible = True
            'lblBemerk.Visible = True
            '---------------------------------------------
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


        '§§§ JVE 14.07.2006
        lblB1.Text = CStr(selectedRow("Bemerkung1"))
        lblB2.Text = CStr(selectedRow("Bemerkung2"))
        lblB3.Text = CStr(selectedRow("Bemerkung3"))
        lblB4.Text = CStr(selectedRow("Bemerkung4"))
        '-------------------

        'Langtext holen)
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As New ASL_01(m_User, m_App, strFileName)
        Dim langtext As DataTable

        Try
            langtext = m_Report.getLangText(Session("AppID").ToString, Session.SessionID.ToString, equi)   'BAPI-Aufruf...
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
        lblAntrag.Text = ASL_01.toShortDateString(selectedRow(1).ToString)    'Angelegt.
        'lblAntrag2.Text = selectedRow(23).ToString   'Antrags-Nr.

        lblLBeginn.Text = ASL_01.toShortDateString(selectedRow(5).ToString) 'Leasingbeginn
        lblLEnde.Text = ASL_01.toShortDateString(selectedRow(6).ToString)   'Leasingende

        'Leasingnehmer
        lblNameLN.Text = selectedRow(7).ToString 'Leasingnehmer
        lblStrLN.Text = selectedRow("StrasseLN").ToString 'StrasseLN
        lblPLZLN.Text = selectedRow("PLZLeasingnehmer").ToString 'PLZLeasingnehmer
        lblOrtLN.Text = selectedRow("OrtLeasingnehmer").ToString 'OrtLeasingnehmer
        '        lblKonzern.Text = selectedRow(26).ToString 'Konzernname
        lblKonzernID.Text = selectedRow("Kundennummer").ToString 'KonzernID

        'Versicherungsgeber
        lblNameVG.Text = selectedRow(9).ToString    'Versicherung
        lblStrVG.Text = CType(selectedRow("StrasseVG"), String)   'StrasseVG
        lblPLZVG.Text = CType(selectedRow("PLZVersicherungsgeber"), String)   'PLZVersicherungsgeber
        lblOrtVG.Text = CType(selectedRow("OrtVersicherungsgeber"), String)   'OrtVersicherungsgeber

        lblVBeginn.Text = ASL_01.toShortDateString(selectedRow(19).ToString) 'Versicherungsbeginn
        lblVEnde.Text = ASL_01.toShortDateString(selectedRow(20).ToString)  'Versicherungsende
        lblVschein.Text = selectedRow(24).ToString  'VersicherungsSchein-Nr

        'Versanddatum, Rückgabedatum, Zurück LN + VG
        lblVersandLN.Text = ASL_01.toShortDateString(selectedRow(11).ToString) 'VersandLN
        lblVersandVG.Text = ASL_01.toShortDateString(selectedRow(15).ToString) 'RückgabeLN
        lblRueckLN.Text = ASL_01.toShortDateString(selectedRow(14).ToString)   'VersandVG
        lblRueckVG.Text = ASL_01.toShortDateString(selectedRow(18).ToString)  'RückgabeVG

        'JVE 21.03.2006
        lblKonzs_ZK.Text = CType(selectedRow("Konzernschlüssel"), String)
        lblName1_ZK.Text = CType(selectedRow("Name1"), String) '§§§ JVE 27.10.2006
        lblName1.Text = CType(selectedRow("Name1GP"), String)
        lblName2.Text = CType(selectedRow("Name2"), String)
        lblName3.Text = CType(selectedRow("Name3"), String)
        lblStras_ZO.Text = CType(selectedRow("Strasse"), String)
        lblPstlz_ZO.Text = CType(selectedRow("Postleitzahl"), String)
        lblOrt_ZO.Text = CType(selectedRow("Ort"), String)
        lblKonzs_ZO.Text = CType(selectedRow("Konzernschl"), String)
        lblName2LN.Text = CType(selectedRow("Name2LN"), String)
        lblName3LN.Text = CType(selectedRow("Name3LN"), String)
        '--------------

        '§§§ JVE 16.05.2006
        txtBem1.Text = CType(selectedRow("Text1"), String)
        txtBem2.Text = CType(selectedRow("Text2"), String)
        txtBem3.Text = CType(selectedRow("Text3"), String)
        txtBem4.Text = CType(selectedRow("Text4"), String)

        'Versicherungsumfang
        lblVersUmf.Text = selectedRow("Versicherungsumfang").ToString

        Dim VersandLNunv As String = ASL_01.toShortDateString(selectedRow(27).ToString) 'VersandLNunv
        Dim RueckversandLNunv As String = ASL_01.toShortDateString(selectedRow(29).ToString) 'RueckversandLNunv
        Dim VersandLNunv2 As String = ASL_01.toShortDateString(selectedRow(28).ToString) 'VersandLNunv2
        Dim RueckversandLNunv2 As String = ASL_01.toShortDateString(selectedRow(30).ToString) 'RueckversandLNunv2

        If Not VersandLNunv = String.Empty Then
            lblUnvLN.Items.Add(VersandLNunv & " - Erneuter Versand")
        End If
        If Not RueckversandLNunv = String.Empty Then
            lblUnvLN.Items.Add(RueckversandLNunv & " - Rückversand")
        End If
        If Not VersandLNunv2 = String.Empty Then
            lblUnvLN.Items.Add(VersandLNunv2 & " - Erneuter Versand 2")
        End If
        If Not RueckversandLNunv2 = String.Empty Then
            lblUnvLN.Items.Add(RueckversandLNunv2 & " - Rückversand 2")
        End If

        Dim VersandVGunv As String = ASL_01.toShortDateString(selectedRow(31).ToString) 'VersandVGunv
        Dim RueckversandVGunv As String = ASL_01.toShortDateString(selectedRow(33).ToString) 'RueckversandVGunv
        Dim VersandVGunv2 As String = ASL_01.toShortDateString(selectedRow(32).ToString) 'VersandVGunv2
        Dim RueckversandVGunv2 As String = ASL_01.toShortDateString(selectedRow(34).ToString) 'RueckversandVGunv2

        If Not VersandVGunv = String.Empty Then
            lblUnvVG.Items.Add(VersandVGunv & " - Erneuter Versand")
        End If
        If Not RueckversandVGunv = String.Empty Then
            lblUnvVG.Items.Add(RueckversandVGunv & " - Rückversand")
        End If
        If Not VersandVGunv2 = String.Empty Then
            lblUnvVG.Items.Add(VersandVGunv2 & " - Erneuter Versand 2")
        End If
        If Not RueckversandVGunv2 = String.Empty Then
            lblUnvVG.Items.Add(RueckversandVGunv2 & " - Rückversand 2")
        End If
        'Wenn leer, Dropdownlisten verstecken...
        If (lblUnvLN.Items.Count = 0) Then
            lblUnvLN.Visible = False
        End If
        If (lblUnvVG.Items.Count = 0) Then
            lblUnvVG.Visible = False
        End If
        'Fahrzeugdaten
        lblEz.Text = ASL_01.toShortDateString(selectedRow("Erstzulassung").ToString) 'Erstzulassung
        lblFzArt.Text = selectedRow(21).ToString 'Fahrzeugart
        lblFGNr.Text = selectedRow(3).ToString 'Fahrgestellnr
        lblHerst.Text = selectedRow(22).ToString 'Hersteller_Typ
        lblKennz.Text = selectedRow(4).ToString 'Kennzeichen
        'Mahndaten
        lblMahnsLN.Text = CType(selectedRow(13), String) 'MahnstufeLN
        lblMadatLN.Text = ASL_01.toShortDateString(selectedRow(12).ToString) 'MahndatumLN
        lblMahnsVG.Text = CType(selectedRow(17), String) 'MahnstufeVG
        lblMadatVG.Text = ASL_01.toShortDateString(selectedRow(16).ToString) 'MahndatumVG


    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As New ASL_01(m_User, m_App, strFileName)
        Dim equi As String
        equi = Request.Item("Equipment")    'Equipment-Nr. holen

        Try
            m_Report.saveComments(Session("AppID").ToString, Session.SessionID.ToString, equi, txtBem1.Text, txtBem2.Text, txtBem3.Text, txtBem4.Text)
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
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 10:19
' Updated in $/CKAG/Applications/appasl/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 24.04.08   Time: 14:28
' Updated in $/CKAG/Applications/appasl/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:04
' Created in $/CKAG/Applications/appasl/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 10:11
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 6.03.07    Time: 18:02
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' 
' ************************************************
