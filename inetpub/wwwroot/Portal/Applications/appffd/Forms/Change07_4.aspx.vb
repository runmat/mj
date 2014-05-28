Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change07_4
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objAddressList As Search
    Private objHaendler As FDD_Haendler

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents chkVersandStandard As System.Web.UI.WebControls.RadioButton
    Protected WithEvents drpVersand As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkZweigstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cmbZweigstellen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkZulassungsstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cmbZuslassungstellen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtTEXT50 As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ZeigeZULST As System.Web.UI.HtmlControls.HtmlTableRow
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
    Protected WithEvents tr_Adresse As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ZeigeTEXT50 As System.Web.UI.HtmlControls.HtmlTableRow

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
        lnkFahrzeugAuswahl.NavigateUrl = "Change07_3.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change07.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
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

            objHaendler = CType(Session("objHaendler"), FDD_Haendler)

            ZeigeZULST.Visible = True
            cmdSearch.Visible = True

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
                Kopfdaten1.Kontingente = objHaendler.Kontingente

                Dim cmbItem As New System.Web.UI.WebControls.ListItem()
                cmbItem.Value = Kopfdaten1.HaendlerNummer
                cmbItem.Text = Kopfdaten1.HaendlerName & ", " & Replace(Kopfdaten1.Adresse, "<br>", ", ")
                cmbZweigstellen.Items.Add(cmbItem)

                objAddressList = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

                Dim tmpIntValue As Int32 = objAddressList.LeseAdressenSAP(Kopfdaten1.HaendlerNummer)
                If tmpIntValue > 0 Then
                    Dim tmpRow As DataRow
                    For Each tmpRow In objAddressList.SearchResult.Rows
                        cmbItem = New System.Web.UI.WebControls.ListItem()
                        cmbItem.Value = tmpRow("ADDRESSNUMBER").ToString
                        cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                        cmbZweigstellen.Items.Add(cmbItem)
                    Next
                End If
                'UH 13.06.05:
                'Wieso dieses? Voreinstellung sollte immer der Händler selbst sein!
                'If cmbZweigstellen.Items.Count = 2 Then
                '    cmbZweigstellen.SelectedIndex = 1
                'Else
                cmbZweigstellen.SelectedIndex = 0
                'End If
                getZulStellen()
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
                objHaendler.PostCode = txt_PLZ.Text.Trim(" "c) & " "
                strAdresse = strAdresse & "DE-" & txt_PLZ.Text.Trim(" "c) & " "
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
        End If

        'If chkVersandStandard.Checked Then
        '    Session("Materialnummer") = "1391"
        'ElseIf chk0800.Checked Then
        '    Session("Materialnummer") = "17071"
        'ElseIf chk0900.Checked Then
        '    Session("Materialnummer") = "1385"
        'ElseIf chk1000.Checked Then
        '    Session("Materialnummer") = "1389"
        'ElseIf chk1200.Checked Then
        '    Session("Materialnummer") = "1390"
        'End If
        Session("Materialnummer") = drpVersand.SelectedItem.Value
        Response.Redirect("Change07_5.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub getZulStellen()
        Dim status As String = ""
        Dim view As DataView
        Dim row As DataRow
        Dim tblTemp As DataTable
        Dim newRow As DataRow
        Dim counter As Integer

        'Zulassungsdienste holen
        objHaendler.getZulStelle(Session("AppID").ToString, Session.SessionID, Me, "", "", status)

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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

