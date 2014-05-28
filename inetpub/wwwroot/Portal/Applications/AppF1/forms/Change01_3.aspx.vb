Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG

Public Class Change01_3
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objHaendler As F1_Haendler

#Region "Controldefinitionen"

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents ImageButton1 As ImageButton
    Protected WithEvents lblHead As Label
    Protected WithEvents lblPageTitle As Label
    Protected WithEvents cmdSave As LinkButton
    Protected WithEvents cmdSearch As LinkButton
    Protected WithEvents lnkFahrzeugsuche As HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As HyperLink
    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents tdVersandartStandard As HtmlTableCell
    Protected WithEvents rb_Versandart_Standard As RadioButton
    Protected WithEvents divVersandartStandard As HtmlGenericControl
    Protected WithEvents divVersandartUPS As HtmlGenericControl
    Protected WithEvents rb_UPS_Sendungsverfolgt As RadioButton
    Protected WithEvents lbl_UPS_Sendungsverfolgt As Label
    Protected WithEvents divVersandartTNT As HtmlGenericControl
    Protected WithEvents rb_TNT_Express_0900 As RadioButton
    Protected WithEvents lbl_TNT_Express_0900 As Label
    Protected WithEvents rb_TNT_Express_1000 As RadioButton
    Protected WithEvents lbl_TNT_Express_1000 As Label
    Protected WithEvents rb_TNT_Express_1200 As RadioButton
    Protected WithEvents lbl_TNT_Express_1200 As Label
    Protected WithEvents divVersandartDHL As HtmlGenericControl
    Protected WithEvents rb_DHL_Express_0900 As RadioButton
    Protected WithEvents lbl_DHL_Express_0900 As Label
    Protected WithEvents rb_DHL_Express_1000 As RadioButton
    Protected WithEvents lbl_DHL_Express_1000 As Label
    Protected WithEvents rb_DHL_Express_1200 As RadioButton
    Protected WithEvents lbl_DHL_Express_1200 As Label
    Protected WithEvents imgInfoVersandartStandard As Image
    Protected WithEvents imgInfoVersandartUPS As Image
    Protected WithEvents imgInfoVersandartTNT As Image
    Protected WithEvents imgInfoVersandartDHL As Image
    Protected WithEvents tdVersandartUPS As HtmlTableCell
    Protected WithEvents rb_Versandart_UPS As RadioButton
    Protected WithEvents tdVersandartTNT As HtmlTableCell
    Protected WithEvents rb_Versandart_TNT As RadioButton
    Protected WithEvents tdVersandartDHL As HtmlTableCell
    Protected WithEvents rb_Versandart_DHL As RadioButton
    Protected WithEvents rb_Versandadresse_Hinterlegt As RadioButton
    Protected WithEvents rb_Versandadresse_Zulassungsstellen As RadioButton
    Protected WithEvents rb_Versandadresse_Manuell As RadioButton
    Protected WithEvents divZweigstellen As HtmlGenericControl
    Protected WithEvents ddlZweigstellen As DropDownList
    Protected WithEvents divZulassungsstellen As HtmlGenericControl
    Protected WithEvents ddlZulassungsstellen As DropDownList
    Protected WithEvents divManuelleEingabe As HtmlGenericControl
    Protected WithEvents lbl_Name As Label
    Protected WithEvents txt_Name As TextBox
    Protected WithEvents lbl_Name2 As Label
    Protected WithEvents txt_Name2 As TextBox
    Protected WithEvents lbl_Strasse As Label
    Protected WithEvents txt_Strasse As TextBox
    Protected WithEvents lbl_Nummer As Label
    Protected WithEvents txt_Nummer As TextBox
    Protected WithEvents lbl_PLZ As Label
    Protected WithEvents txt_PLZ As TextBox
    Protected WithEvents lbl_Ort As Label
    Protected WithEvents txt_Ort As TextBox
    Protected WithEvents lbl_Land As Label
    Protected WithEvents ddl_Land As DropDownList
    Protected WithEvents ZeigeTEXT50 As HtmlTableRow
    Protected WithEvents txtTEXT50 As TextBox
    Protected WithEvents RequiredFieldValidator1 As RequiredFieldValidator
    Protected WithEvents lblError As Label

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
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change01_1.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Kopfdaten1.SapInterneHaendlerNummer = objSuche.SapInterneHaendlerReferenzNummer
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Kopfdaten1.Kontingente = objSuche.Kontingente
            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), F1_Haendler)

            Dim rowsTemp() As DataRow = objHaendler.Fahrzeuge.Select("MANDT='4'")
            If rowsTemp.GetLength(0) > 0 Then
                ZeigeTEXT50.Visible = True
                RequiredFieldValidator1.Enabled = True
                If IsPostBack Then
                    Dim r As Integer
                    For r = 0 To rowsTemp.GetLength(0) - 1
                        rowsTemp(r)("TEXT50") = txtTEXT50.Text
                    Next
                End If
            Else
                ZeigeTEXT50.Visible = False
                RequiredFieldValidator1.Enabled = False
            End If

            If Not IsPostBack Then
                Dim cmbItem As New ListItem()
                cmbItem.Value = Kopfdaten1.SapInterneHaendlerNummer
                cmbItem.Text = Kopfdaten1.HaendlerName & ", " & Replace(Kopfdaten1.Adresse, "<br>", ", ")
                ddlZweigstellen.Items.Add(cmbItem)

                Dim tmpIntValue As Int32 = objSuche.LeseAdressenSAP(Session("AppId").ToString, Session.SessionID, Me, objSuche.SapInterneHaendlerReferenzNummer)
                If tmpIntValue > 0 Then
                    Dim tmpRow As DataRow
                    For Each tmpRow In objSuche.SearchResult.Rows
                        cmbItem = New ListItem()
                        cmbItem.Value = tmpRow("ADDRESSNUMBER").ToString
                        cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                        ddlZweigstellen.Items.Add(cmbItem)
                    Next
                End If
                ddlZweigstellen.SelectedIndex = 0

                getZulStellen()
                fillLaenderDLL()
                setVersandadressenberechtigung()

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        DoSubmit()
    End Sub

    Private Sub setVersandadressenberechtigung()

        Dim tmpRows() As DataRow
        tmpRows = objHaendler.Fahrzeuge.Select("MANDT <> 0")

        Dim arrRow As DataRow
        Dim iEingeschraenkt As Integer = 0
        Dim iBerechtigung As Integer = 0
        Dim iAutRight As Integer = m_User.Groups(0).Authorizationright

        For Each arrRow In tmpRows
            Dim sAugru As String
            sAugru = CType(arrRow("AUGRU"), String)
            Dim tmpEingeschraenkt As Integer
            tmpEingeschraenkt = objHaendler.GeteingeschraenkteAbrufgruende(sAugru)
            If iEingeschraenkt = 0 AndAlso Not tmpEingeschraenkt = 0 Then
                iEingeschraenkt = tmpEingeschraenkt
            ElseIf iEingeschraenkt > 0 AndAlso Not tmpEingeschraenkt = 0 Then
                If iEingeschraenkt > tmpEingeschraenkt Then
                    iEingeschraenkt = tmpEingeschraenkt
                End If
            End If
        Next

        If iEingeschraenkt <> 0 Then
            If iAutRight < iEingeschraenkt Then
                iBerechtigung = iAutRight
            Else
                iBerechtigung = iEingeschraenkt
            End If

        Else
            iBerechtigung = iAutRight
        End If

        Select Case iBerechtigung
            Case 0
                rb_Versandadresse_Hinterlegt.Visible = False
                rb_Versandadresse_Zulassungsstellen.Visible = False
                rb_Versandadresse_Manuell.Visible = False
                cmdSave.Enabled = False
                Throw New Exception("Sie sind zu einer Versendung nicht berechtigt. (Gruppenautorisierungslvl)")

            Case 1
                rb_Versandadresse_Hinterlegt.Visible = False
                rb_Versandadresse_Zulassungsstellen.Visible = True
                rb_Versandadresse_Manuell.Visible = False
                rb_Versandadresse_Zulassungsstellen.Checked = True
                divZulassungsstellen.Visible = True

            Case 2
                rb_Versandadresse_Hinterlegt.Visible = True
                rb_Versandadresse_Zulassungsstellen.Visible = True
                rb_Versandadresse_Manuell.Visible = False
                rb_Versandadresse_Hinterlegt.Checked = True
                divZweigstellen.Visible = True

            Case 3
                rb_Versandadresse_Manuell.Visible = True
                rb_Versandadresse_Hinterlegt.Visible = True
                rb_Versandadresse_Zulassungsstellen.Visible = True
                rb_Versandadresse_Hinterlegt.Checked = True
                divZweigstellen.Visible = True
        End Select

    End Sub

    Private Sub DoSubmit()

        Dim strAdresse As String = ""
        If rb_Versandadresse_Hinterlegt.Checked Then
            Session("SelectedDeliveryValue") = ddlZweigstellen.SelectedItem.Value
            Session("SelectedDeliveryText") = ddlZweigstellen.SelectedItem.Text
            objHaendler.kbanr = String.Empty
            objHaendler.NewAdress = False
        ElseIf rb_Versandadresse_Zulassungsstellen.Checked Then
            Session("SelectedDeliveryText") = "Zulassungsstelle " & ddlZulassungsstellen.SelectedItem.Text
            Session("SelectedDeliveryValue") = ddlZulassungsstellen.SelectedItem.Value
            objHaendler.kbanr = ddlZulassungsstellen.SelectedItem.Value
            objHaendler.NewAdress = True
        Else
            If txt_Name.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Name"" eingeben.<br>&nbsp;"
            Else
                objHaendler.Name1 = txt_Name.Text.Trim(" "c)
                strAdresse = txt_Name.Text.Trim(" "c) & ", "
            End If
            If txt_PLZ.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""PLZ"" eingeben.<br>&nbsp;"
            Else
                If CInt(objHaendler.Laender.Select("Land1='" & ddl_Land.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                    If Not CInt(objHaendler.Laender.Select("Land1='" & ddl_Land.SelectedItem.Value & "'")(0)("Lnplz")) = txt_PLZ.Text.Trim(" "c).Length Then
                        lblError.Text = "Postleitzahl hat falsche Länge."
                    Else
                        objHaendler.PostCode = txt_PLZ.Text.Trim(" "c) & " "
                        strAdresse = strAdresse & ddl_Land.SelectedItem.Value & "-" & txt_PLZ.Text.Trim(" "c) & " "
                    End If
                End If

            End If
            If txt_Ort.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"
            Else
                objHaendler.City = txt_Ort.Text.Trim(" "c)
                strAdresse = strAdresse & txt_Ort.Text.Trim(" "c) & ", "
            End If
            If txt_Strasse.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Strasse"" eingeben.<br>&nbsp;"
            Else
                objHaendler.Street = txt_Strasse.Text.Trim(" "c)
                strAdresse = strAdresse & txt_Strasse.Text.Trim(" "c) & " "
            End If
            If txt_Nummer.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Nummer"" eingeben.<br>&nbsp;"
            Else
                objHaendler.HouseNum = txt_Nummer.Text.Trim(" "c)
                strAdresse = strAdresse & txt_Nummer.Text.Trim(" "c)
            End If
            Session("SelectedDeliveryValue") = Kopfdaten1.HaendlerNummer
            Session("SelectedDeliveryText") = strAdresse
            objHaendler.NewAdress = True
            objHaendler.kbanr = String.Empty
            objHaendler.Name2 = txt_Name2.Text.Trim(" "c)
        End If

        Session("Materialnummer") = ""
        Session("VersandArtText") = ""

        If rb_Versandart_Standard.Checked Then
            objHaendler.DienstleisterDetail = ""
            Session("Materialnummer") = "1391"
            Session.Add("VersandArtText", rb_Versandart_Standard.Text)

        ElseIf rb_Versandart_UPS.Checked Then
            objHaendler.DienstleisterDetail = "4"
            If rb_UPS_Sendungsverfolgt.Checked Then
                Session("Materialnummer") = "5530"
                Session.Add("VersandArtText", rb_UPS_Sendungsverfolgt.Text)
            End If

        ElseIf rb_Versandart_TNT.Checked Then
            objHaendler.DienstleisterDetail = "2"
            If rb_TNT_Express_0900.Checked Then
                Session("Materialnummer") = "1385"
                Session.Add("VersandArtText", rb_TNT_Express_0900.Text)
            ElseIf rb_TNT_Express_1000.Checked Then
                Session("Materialnummer") = "1389"
                Session.Add("VersandArtText", rb_TNT_Express_1000.Text)
            ElseIf rb_TNT_Express_1200.Checked Then
                Session("Materialnummer") = "1390"
                Session.Add("VersandArtText", rb_TNT_Express_1200.Text)
            End If

        ElseIf rb_Versandart_DHL.Checked Then
            objHaendler.DienstleisterDetail = "1"
            If rb_DHL_Express_0900.Checked Then
                Session("Materialnummer") = "1385"
                Session.Add("VersandArtText", rb_DHL_Express_0900.Text)
            ElseIf rb_DHL_Express_1000.Checked Then
                Session("Materialnummer") = "1389"
                Session.Add("VersandArtText", rb_DHL_Express_1000.Text)
            ElseIf rb_DHL_Express_1200.Checked Then
                Session("Materialnummer") = "1390"
                Session.Add("VersandArtText", rb_DHL_Express_1200.Text)
            End If

        End If

        If String.IsNullOrEmpty(lblError.Text) Then
            Session("AppHaendler") = objHaendler
            Response.Redirect("Change01_4.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Sub getZulStellen()
        Dim status As String = String.Empty
        Dim view As DataView
        Dim row As DataRow
        Dim tblTemp As DataTable
        Dim newRow As DataRow
        Dim counter As Integer

        'Zulassungsdienste holen
        objHaendler.getZulStelle("", "", status, Session("AppId").ToString, Session.SessionID)

        tblTemp = objHaendler.ZulStellen.Clone
        tblTemp.Columns.Add("DISPLAY")

        For Each row In objHaendler.ZulStellen.Rows
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
                    newRow("PSTL2") = CType(counter, String) 'Lfd. Nr. (für spätere Suche!)
                    newRow("KBANR") = row("KBANR").ToString
                    tblTemp.Rows.Add(newRow)
                    tblTemp.AcceptChanges()
                    counter += 1
                End If
            End If
        Next

        objHaendler.ZulStellen = tblTemp.Copy
        objHaendler.ZulStellen.AcceptChanges()
        tblTemp = Nothing

        Session.Add("ZulDienste", objHaendler.ZulStellen)

        view = objHaendler.ZulStellen.DefaultView
        view.Sort = "DISPLAY"
        With ddlZulassungsstellen
            .DataSource = view
            .DataTextField = "DISPLAY"
            .DataValueField = "KBANR"
            .DataBind()
        End With

    End Sub

    Private Sub fillLaenderDLL()

        objHaendler.getLaender(Session("AppID").ToString, Session.SessionID)

        Dim sprache As String
        ddl_Land.DataSource = objHaendler.Laender
        ddl_Land.DataTextField = "FullDesc"
        ddl_Land.DataValueField = "Land1"
        ddl_Land.DataBind()
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
        ddl_Land.Items.FindByValue(sprache).Selected = True

    End Sub

    Private Sub rb_Versandadresse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Versandadresse_Hinterlegt.CheckedChanged, rb_Versandadresse_Zulassungsstellen.CheckedChanged, rb_Versandadresse_Manuell.CheckedChanged
        divZweigstellen.Visible = rb_Versandadresse_Hinterlegt.Checked
        divZulassungsstellen.Visible = rb_Versandadresse_Zulassungsstellen.Checked
        divManuelleEingabe.Visible = rb_Versandadresse_Manuell.Checked
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub rb_Versandart_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Versandart_Standard.CheckedChanged, rb_Versandart_UPS.CheckedChanged, rb_Versandart_TNT.CheckedChanged, rb_Versandart_DHL.CheckedChanged
        Dim blnStandard As Boolean = rb_Versandart_Standard.Checked
        Dim blnUPS As Boolean = rb_Versandart_UPS.Checked
        Dim blnTNT As Boolean = rb_Versandart_TNT.Checked
        Dim blnDHL As Boolean = rb_Versandart_DHL.Checked

        'Standard
        divVersandartStandard.Visible = blnStandard
        imgInfoVersandartStandard.Visible = blnStandard
        tdVersandartStandard.Attributes.Add("style", "background-color:" & IIf(blnStandard, "#c8c8c8", "transparent").ToString())
        'UPS
        divVersandartUPS.Visible = blnUPS
        imgInfoVersandartUPS.Visible = blnUPS
        tdVersandartUPS.Attributes.Add("style", "background-color:" & IIf(blnUPS, "#c8c8c8", "transparent").ToString())
        'TNT
        divVersandartTNT.Visible = blnTNT
        imgInfoVersandartTNT.Visible = blnTNT
        tdVersandartTNT.Attributes.Add("style", "background-color:" & IIf(blnTNT, "#c8c8c8", "transparent").ToString())
        'DHL
        divVersandartDHL.Visible = blnDHL
        imgInfoVersandartDHL.Visible = blnDHL
        tdVersandartDHL.Attributes.Add("style", "background-color:" & IIf(blnDHL, "#c8c8c8", "transparent").ToString())
    End Sub

End Class
' ************************************************
' $History: Change01_3.aspx.vb $
' 
' *****************  Version 17  *****************
' User: Dittbernerc  Date: 13.01.11   Time: 13:52
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 5.01.11    Time: 16:47
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 15  *****************
' User: Fassbenders  Date: 5.01.11    Time: 9:46
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 5.01.11    Time: 9:06
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 4.01.11    Time: 14:33
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 12.03.10   Time: 16:36
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 12.02.10   Time: 15:16
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 16.11.09   Time: 15:47
' Updated in $/CKAG/Applications/AppF1/forms
' ITA:3298
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 17.04.09   Time: 9:11
' Updated in $/CKAG/Applications/AppF1/forms
' Anpassung GMAC Dokumentenversand
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 9.04.09    Time: 14:45
' Updated in $/CKAG/Applications/AppF1/forms
' Nachbesserungen dokumentenversand
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.03.09   Time: 16:49
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2664 nachbesserungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 30.03.09   Time: 9:10
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2664 nachbesserungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 20.03.09   Time: 9:12
' Updated in $/CKAG/Applications/AppF1/forms
' 2674 nachbesserungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 9.03.09    Time: 14:51
' Updated in $/CKAG/Applications/AppF1/forms
' 2664 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 6.03.09    Time: 15:25
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2664 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.03.09    Time: 17:30
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 4.03.09    Time: 11:12
' Created in $/CKAG/Applications/AppF1/forms
'
' ************************************************