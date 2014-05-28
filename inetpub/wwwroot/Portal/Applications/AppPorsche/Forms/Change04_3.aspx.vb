Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change04_3
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
    Private objHaendler As Porsche_05

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ZeigeZULST As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmbZuslassungstellen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkZulassungsstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkVersandStandard As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_0900 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_1000 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_1200 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_SendungsVerfolgt As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkZweigstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cmbZweigstellen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents idPreis1 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents idPreis2 As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lbl_0900 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1000 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_1200 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_SendungsVerfolgt As System.Web.UI.WebControls.Label

    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change04_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change04.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)


        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
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

            objHaendler = CType(Session("objHaendler"), Porsche_05)

            'For future use
            'ZeigeZULST.Visible = False
            cmdSearch.Visible = False

            'Dim rowsTemp() As DataRow = objHaendler.Fahrzeuge.Select("MANDT='4'")
            'If rowsTemp.GetLength(0) > 0 Then
            '    'ZeigeTEXT50.Visible = True
            '    'RequiredFieldValidator1.Enabled = True
            '    If IsPostBack Then
            '        Dim r As Integer
            '        For r = 0 To rowsTemp.GetLength(0) - 1
            '            'rowsTemp(r)("TEXT50") = txtTEXT50.Text
            '        Next
            '    End If
            'Else
            '    'ZeigeTEXT50.Visible = False
            '    'RequiredFieldValidator1.Enabled = False
            'End If

            If Not IsPostBack Then
                Kopfdaten1.Kontingente = objHaendler.Kontingente

                Dim cmbItem As New System.Web.UI.WebControls.ListItem()
                cmbItem.Value = Kopfdaten1.HaendlerNummer
                cmbItem.Text = Kopfdaten1.HaendlerName & ", " & Replace(Kopfdaten1.Adresse, "<br>", ", ")
                cmbZweigstellen.Items.Add(cmbItem)

                objAddressList = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

                Dim tmpIntValue As Int32 = objAddressList.LeseAdressenSAP(m_User.Reference)
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
        Session("SelectedDeliveryValue") = cmbZweigstellen.SelectedItem.Value
        Session("SelectedDeliveryText") = cmbZweigstellen.SelectedItem.Text
        If chkVersandStandard.Checked Then
            Session("Materialnummer") = "1391"

        ElseIf rb_0900.Checked Then
            Session("Materialnummer") = "1385"
            objHaendler.VersandArtText = rb_0900.Text & " " & lbl_0900.Text
        ElseIf rb_1000.Checked Then
            Session("Materialnummer") = "1389"
            objHaendler.VersandArtText = rb_1000.Text & " " & lbl_1000.Text
        ElseIf rb_1200.Checked Then
            Session("Materialnummer") = "1390"
            objHaendler.VersandArtText = rb_1200.Text & " " & lbl_1200.Text
        ElseIf rb_SendungsVerfolgt.Checked Then
            Session("Materialnummer") = "5530"
            objHaendler.VersandArtText = rb_SendungsVerfolgt.Text & " " & lbl_SendungsVerfolgt.Text
        End If
        Response.Redirect("Change04_4.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Change04_3.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:47
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:42
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 1.09.09    Time: 13:19
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 24.08.09   Time: 10:54
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 3061
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 13:27
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 5.03.07    Time: 13:54
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' ************************************************
