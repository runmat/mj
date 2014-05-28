Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change01_3
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As FFE_Search
    Private objAddressList As FFE_Search
    Private objHaendler As FFE_Haendler

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents Kopfdaten1 As Kopfdatenhaendler
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents chkZweigstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cmbZweigstellen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkZulassungsstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cmbZuslassungstellen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents rb_Manuell As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Name As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Strasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Nummer As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Nummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_PLZ As System.Web.UI.WebControls.Label
    Protected WithEvents txt_PLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_Ort As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTEXT50 As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ZeigeZULST As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Adresse As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents rb_VersandStandard As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_0900 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_1000 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_1200 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lbl_0900 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1000 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1200 As System.Web.UI.WebControls.Label
    Protected WithEvents tbl_Adresse As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents ddl_Land As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbl_Land As System.Web.UI.WebControls.Label
    Protected WithEvents ZeigeTEXT50 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txt_Name2 As System.Web.UI.WebControls.TextBox

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
        lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), FFE_Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), FFE_Haendler)

            ZeigeZULST.Visible = True
            'cmdSearch.Visible = True

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
                Dim cmbItem As New System.Web.UI.WebControls.ListItem()
                cmbItem.Value = Kopfdaten1.HaendlerNummer
                cmbItem.Text = Kopfdaten1.HaendlerName & ", " & Replace(Kopfdaten1.Adresse, "<br>", ", ")
                cmbZweigstellen.Items.Add(cmbItem)

                objAddressList = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

                Dim tmpIntValue As Int32 = objAddressList.LeseAdressenSAP(Session("AppID").ToString, Session.SessionID.ToString, Me.Page, m_User.Reference)
                If tmpIntValue > 0 Then
                    Dim tmpRow As DataRow
                    For Each tmpRow In objAddressList.SearchResult.Rows
                        cmbItem = New System.Web.UI.WebControls.ListItem()
                        cmbItem.Value = tmpRow("ADDRESSNUMBER").ToString
                        cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                        cmbZweigstellen.Items.Add(cmbItem)
                    Next
                End If
                cmbZweigstellen.SelectedIndex = 0

                getZulStellen()

                fillLaenderDLL()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()

        Dim strAdresse As String = ""
        If chkZweigstellen.Checked Then
            Session("SelectedDeliveryValue") = cmbZweigstellen.SelectedItem.Value
            Session("SelectedDeliveryText") = cmbZweigstellen.SelectedItem.Text
            objHaendler.kbanr = String.Empty
            objHaendler.NewAdress = False
        ElseIf chkZulassungsstellen.Checked Then
            Session("SelectedDeliveryText") = "Zulassungsstelle " & cmbZuslassungstellen.SelectedItem.Text
            Session("SelectedDeliveryValue") = cmbZuslassungstellen.SelectedItem.Value
            objHaendler.kbanr = cmbZuslassungstellen.SelectedItem.Value
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

        If rb_VersandStandard.Checked Then
            Session("Materialnummer") = "1391"
        ElseIf rb_0900.Checked Then
            Session("Materialnummer") = "1385"
        ElseIf rb_1000.Checked Then
            Session("Materialnummer") = "1389"
        ElseIf rb_1200.Checked Then
            Session("Materialnummer") = "1390"
        End If
        If Not lblError.Text.Length > 0 Then
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
        objHaendler.getZulStelle(Me.Page, "", "", status)

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
        With cmbZuslassungstellen
            .DataSource = view
            .DataTextField = "DISPLAY"
            .DataValueField = "KBANR"
            .DataBind()
        End With

    End Sub

    Private Sub fillLaenderDLL()
        Dim sprache As String
        'Länder DLL füllen
        ddl_Land.DataSource = objHaendler.Laender
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

    Private Sub chkZulassungsstellen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkZulassungsstellen.CheckedChanged
        If chkZulassungsstellen.Checked = True Then
            cmbZuslassungstellen.Visible = True
            tbl_Adresse.Visible = False
            cmbZweigstellen.Visible = False
        End If
    End Sub

    Private Sub rb_Manuell_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Manuell.CheckedChanged
        If rb_Manuell.Checked = True Then
            cmbZuslassungstellen.Visible = False
            tbl_Adresse.Visible = True
            cmbZweigstellen.Visible = False
        End If
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub chkZweigstellen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkZweigstellen.CheckedChanged
        If chkZweigstellen.Checked = True Then
            cmbZuslassungstellen.Visible = False
            tbl_Adresse.Visible = False
            cmbZweigstellen.Visible = True
        End If
    End Sub
End Class
' ************************************************
' $History: Change01_3.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 9.03.10    Time: 9:22
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA: 2918
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 3.03.10    Time: 15:08
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' Ausblenden Händler Kontingente
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 5.06.08    Time: 13:04
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 5.05.08    Time: 17:09
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Applications/AppFFE/forms
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.04.08    Time: 13:32
' Created in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 7.04.08    Time: 14:04
' Updated in $/CKG/Applications/AppFFE/AppFFEWeb/forms
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 3.04.08    Time: 11:19
' Created in $/CKG/Applications/AppFFE/AppFFEWeb/forms
' ITA 1790
' 
' ************************************************