Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Public Class Change02_3
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
    ' Private objSuche As CKG.Components.ComCommon.Finance.Search
    ' Private objAddressList As CKG.Components.ComCommon.Finance.Search
    Private mObjBriefanforderung As Briefanforderung
    Private mstrHDL As String
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As AppBPLG.PageElements.Kopfdaten
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ZeigeZULST As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkZulassungsstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cmbZweigstellen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddl_ZulStelle As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rb_Zweigstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_Manuell As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_ZulStelle As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lbl_PLZ As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Adresse As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txt_Name As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Strasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_PLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Ort As System.Web.UI.WebControls.TextBox
    Protected WithEvents rb_VersandStandard As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_0900 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_1000 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_1200 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lbl_1000 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1200 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_HausNummer As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Nummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lbl_0900 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Name2 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Name2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Land As System.Web.UI.WebControls.Label
    Protected WithEvents ddl_Land As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tbl_Adresse As HtmlTable
    Protected WithEvents rb_SendungsVerfolgt As RadioButton
    Protected WithEvents lbl_SendungsVerfolgt As Label



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change02_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change02.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)

        'htmlTabelle finden
        If tbl_Adresse Is Nothing Then
            tbl_Adresse = CType(FindControl("tbl_Adresse"), HtmlTable)
        End If





        'Ausblendung entfernt laut Rothe 28.01.2008
        'chk1000.Visible = False
        'lbl1000.Visible = False
        'chk1200.Visible = False
        'lbl1200.Visible = False



        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            mstrHDL = ""
            If CType(Session("AppShowNot"), Boolean) = True Then
                tr_Adresse.Visible = False
            End If

            If Session("mObjBriefanforderungSession") Is Nothing Then
                Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString)
            End If

            'If (Session("objSuche") Is Nothing) Then
            '    Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString)
            'Else
            '    '      objSuche = CType(Session("objSuche"), CKG.Components.ComCommon.Finance.Search)
            'End If

            If mObjBriefanforderung Is Nothing Then
                mObjBriefanforderung = CType(Session("mObjBriefanforderungSession"), Briefanforderung)
            End If




            Kopfdaten1.HaendlerNummer = mObjBriefanforderung.Endkundennummer
            Kopfdaten1.HaendlerName = mObjBriefanforderung.EndkundeFullName
            Kopfdaten1.Adresse = mObjBriefanforderung.EndkundenAdresse


            'Kopfdaten1.UserReferenz = m_User.Reference
            'Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            'Dim strTemp As String = objSuche.NAME
            'If objSuche.NAME_2.Length > 0 Then
            '    strTemp &= "<br>" & objSuche.NAME_2
            'End If
            'Kopfdaten1.HaendlerName = strTemp
            'Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

            'Session("objSuche") = objSuche


            If Not IsPostBack Then
                'For future use
                'ZeigeZULST.Visible = False
                cmdSearch.Visible = False



                'Kopfdaten1.Kontingente = mObjBriefanforderung.Kontingente
                Dim cmbItem As New System.Web.UI.WebControls.ListItem()
                If Kopfdaten1.HaendlerNummer <> "" Then
                    cmbItem.Value = Kopfdaten1.HaendlerNummer
                    cmbItem.Text = Kopfdaten1.HaendlerName & ", " & Replace(Kopfdaten1.Adresse, "<br>", ", ")
                    cmbZweigstellen.Items.Add(cmbItem)
                End If



                'füllen der Zweigstellen liste mit HändlerAdresse
                Dim tmpRows() As DataRow = mObjBriefanforderung.Fahrzeuge.Select("MANDT <> '0'")
                Dim AnzahlAllerRows As Int32 = tmpRows.Count

                'wenn die Anzahl der Einträge auf abfrage einer beliebigen händlernummer nicht gleich ist, sind es verschiedene, somit nur noch die Endkundenadresse anzeigen
                If Not AnzahlAllerRows = mObjBriefanforderung.Fahrzeuge.Select("MANDT <> '0' AND EX_KUNNR_ZF='" & tmpRows(0).Item("EX_KUNNR_ZF").ToString & "'").Count Then
                    'keine weiteren Versandadressen
                Else
                    'alle Fahrzeugbriefe gehören zu einem Händler, Diese Adresse als versandadresse hinzufügen
                    If Not mObjBriefanforderung.Versandadressen.Select("EX_KUNNR='" & tmpRows(0).Item("EX_KUNNR_ZF").ToString & "'").Count = 0 Then
                        'wenn 0 dann habe ich keine Partneradresse aus SAP Bekommen oder der Brief hat keinen Händler
                        Dim tmprow As DataRow
                        cmbItem = New System.Web.UI.WebControls.ListItem()
                        tmprow = mObjBriefanforderung.Versandadressen.Select("EX_KUNNR='" & tmpRows(0).Item("EX_KUNNR_ZF").ToString & "'")(0)
                        cmbItem.Value = tmprow.Item("EX_KUNNR").ToString
                        cmbItem.Text = tmprow.Item("NAME1").ToString & " " & tmprow.Item("NAME2").ToString & ", " & tmprow.Item("COUNTRY").ToString & " - " & _
                        tmprow.Item("POST_CODE1").ToString & " " & tmprow.Item("CITY1").ToString & " " & tmprow.Item("STREET").ToString & " " & tmprow.Item("HOUSE_NUM1").ToString
                        cmbZweigstellen.Items.Add(cmbItem)
                    End If

                End If


                '  objAddressList = New CKG.Components.ComCommon.Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

                ' Dim tmpIntValue As Int32 = objAddressList.LeseAdressenSAP(mObjBriefanforderung.Customer)
                'If tmpIntValue > 0 Then
                '    Dim tmpRow As DataRow
                '    For Each tmpRow In objAddressList.SearchResult.Rows
                '        cmbItem = New System.Web.UI.WebControls.ListItem()
                '        cmbItem.Value = tmpRow("ADDRESSNUMBER").ToString
                '        cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                '        cmbZweigstellen.Items.Add(cmbItem)
                '    Next
                'End If

                cmbZweigstellen.SelectedIndex = 0
                'End If
                getZulStellen()

                fillLaenderDLL()

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub fillLaenderDLL()
        Dim sprache As String
        'Länder DLL füllen
        ddl_Land.DataSource = mObjBriefanforderung.Laender
        'ddlLand.DataTextField = "Beschreibung"
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

    Private Sub getZulStellen()
        Dim status As String = ""
        Dim view As DataView
        Dim row As DataRow
        Dim tblTemp As DataTable
        Dim newRow As DataRow
        Dim counter As Integer

        'Zulassungsdienste holen
        mObjBriefanforderung.getZulStelle("", "", status)

        tblTemp = mObjBriefanforderung.ZulStellen.Clone
        tblTemp.Columns.Add("DISPLAY")

        For Each row In mObjBriefanforderung.ZulStellen.Rows
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

        mObjBriefanforderung.ZulStellen = tblTemp.Copy
        mObjBriefanforderung.ZulStellen.AcceptChanges()
        tblTemp = Nothing

        Session.Add("ZulDienste", mObjBriefanforderung.ZulStellen)

        view = mObjBriefanforderung.ZulStellen.DefaultView
        view.Sort = "DISPLAY"
        With ddl_ZulStelle
            .DataSource = view
            .DataTextField = "DISPLAY"
            .DataValueField = "KBANR"
            .DataBind()
        End With

    End Sub


    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim strAdresse As String = ""
        mObjBriefanforderung.kbanr = ""
        If rb_Zweigstellen.Checked Then
            mObjBriefanforderung.VersandAdressText = cmbZweigstellen.SelectedItem.Text
            mObjBriefanforderung.VersandAdressValue = cmbZweigstellen.SelectedItem.Value
            'Session("SelectedDeliveryValue") = cmbZweigstellen.SelectedItem.Value
            'Session("SelectedDeliveryText") = cmbZweigstellen.SelectedItem.Text
            mObjBriefanforderung.VersandKuerzel = "ZWST"
        ElseIf rb_ZulStelle.Checked Then
            mObjBriefanforderung.VersandAdressText = "Zulassungsstelle " & ddl_ZulStelle.SelectedItem.Text
            mObjBriefanforderung.VersandAdressValue = ddl_ZulStelle.SelectedItem.Value
            'Session("SelectedDeliveryText") = "Zulassungsstelle " & ddl_ZulStelle.SelectedItem.Text
            'Session("SelectedDeliveryValue") = cmbZweigstellen.SelectedItem.Value
            mObjBriefanforderung.VersandKuerzel = "ZUST"
        Else 'manuelle eingabe
            If txt_Name.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Name"" eingeben.<br>&nbsp;"
            Else
                mObjBriefanforderung.Name1 = txt_Name.Text.Trim(" "c)
                strAdresse = txt_Name.Text.Trim(" "c) & ", "
            End If
            If txt_PLZ.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""PLZ"" eingeben.<br>&nbsp;"
            Else
                If CInt(mObjBriefanforderung.Laender.Select("Land1='" & ddl_Land.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                    If Not CInt(mObjBriefanforderung.Laender.Select("Land1='" & ddl_Land.SelectedItem.Value & "'")(0)("Lnplz")) = txt_PLZ.Text.Trim(" "c).Length Then
                        lblError.Text = "Postleitzahl hat falsche Länge."
                    Else
                        mObjBriefanforderung.PostCode = txt_PLZ.Text.Trim(" "c) & " "
                        strAdresse = strAdresse & ddl_Land.SelectedItem.Value & "-" & txt_PLZ.Text.Trim(" "c) & " "
                    End If
                End If

            End If
            If txt_Ort.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"
            Else
                mObjBriefanforderung.City = txt_Ort.Text.Trim(" "c)
                strAdresse = strAdresse & txt_Ort.Text.Trim(" "c) & ", "
            End If
            If txt_Strasse.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Strasse"" eingeben.<br>&nbsp;"
            Else
                mObjBriefanforderung.Street = txt_Strasse.Text.Trim(" "c)
                strAdresse = strAdresse & txt_Strasse.Text.Trim(" "c) & " "
            End If
            If txt_Nummer.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Nummer"" eingeben.<br>&nbsp;"
            Else
                mObjBriefanforderung.HouseNum = txt_Nummer.Text.Trim(" "c)
                strAdresse = strAdresse & txt_Nummer.Text.Trim(" "c)
            End If
            mObjBriefanforderung.Name2 = txt_Name2.Text.Trim(" "c)
            mObjBriefanforderung.laenderKuerzel = ddl_Land.SelectedItem.Value


            mObjBriefanforderung.VersandAdressText = strAdresse
            mObjBriefanforderung.VersandAdressValue = Kopfdaten1.HaendlerNummer
            mObjBriefanforderung.VersandKuerzel = "MAEI"
            'Session("SelectedDeliveryValue") = Kopfdaten1.HaendlerNummer
            'Session("SelectedDeliveryText") = strAdresse
        End If

        If rb_VersandStandard.Checked And rb_VersandStandard.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1391"
            mObjBriefanforderung.MaterialText = "Standard"
            'Session("Materialnummer") = "1391"
        ElseIf rb_0900.Checked And rb_0900.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1385"
            mObjBriefanforderung.MaterialText = lbl_0900.Text
            'Session("Materialnummer") = "1385"
        ElseIf rb_1000.Checked And rb_1000.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1389"
            mObjBriefanforderung.MaterialText = lbl_1000.Text
            'Session("Materialnummer") = "1389"
        ElseIf rb_1200.Checked And rb_1200.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1390"
            mObjBriefanforderung.MaterialText = lbl_1200.Text
            'Session("Materialnummer") = "1390"
        ElseIf rb_SendungsVerfolgt.Checked And rb_SendungsVerfolgt.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "5530"
            mObjBriefanforderung.MaterialText = lbl_SendungsVerfolgt.Text
            'Session("Materialnummer") = "XXXX"
        End If

        If Not lblError.Text.Length > 0 Then
            Session("mObjBriefanforderungSession") = mObjBriefanforderung
            Response.Redirect("Change02_4.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub rb_Zweigstellen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Zweigstellen.CheckedChanged
        If rb_Zweigstellen.Checked = True Then
            ddl_ZulStelle.Visible = False
            tbl_Adresse.Visible = False
            cmbZweigstellen.Visible = True
        End If
    End Sub


    Private Sub rb_ZulStelle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_ZulStelle.CheckedChanged
        If rb_ZulStelle.Checked = True Then
            ddl_ZulStelle.Visible = True
            tbl_Adresse.Visible = False
            cmbZweigstellen.Visible = False
        End If
    End Sub

    Private Sub rb_Manuell_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Manuell.CheckedChanged
        If rb_Manuell.Checked = True Then
            ddl_ZulStelle.Visible = False
            tbl_Adresse.Visible = True
            cmbZweigstellen.Visible = False
        End If
    End Sub
End Class

' ************************************************
' $History: Change02_3.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 1.02.10    Time: 16:23
' Updated in $/CKAG/Applications/AppBPLG/Forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 11.08.08   Time: 17:54
' Updated in $/CKAG/Applications/AppBPLG/Forms
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 31.07.08   Time: 13:53
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Doppeltes PageLoad entfernt
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 29.07.08   Time: 9:10
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Nachbesserungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 29.07.08   Time: 8:46
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Nachbesserung
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.07.08   Time: 12:49
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2070
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.07.08   Time: 14:19
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2070 rohversion
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 18.07.08   Time: 12:51
' Created in $/CKAG/Applications/AppBPLG/Forms
' Klassen erstellt
' 
' ************************************************