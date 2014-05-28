Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Components.ComCommon

Public Class Change43_3
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
    Private objSuche As CKG.Components.ComCommon.Finance.Search
    Private objAddressList As CKG.Components.ComCommon.Finance.Search

    Private objHaendler As fin_06
    Private strHaendlernr As String

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As ComCommon.PageElements.Kopfdaten
    Protected WithEvents chkZweigstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkZulassungsstellen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents ZeigeZULST As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmbZweigstellen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmbZuslassungstellen As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtTEXT50 As System.Web.UI.WebControls.TextBox
    Protected WithEvents RequiredFieldValidator1 As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ddlHalter As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlEmpf As System.Web.UI.WebControls.DropDownList
    Protected WithEvents chkVersandStandard As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk0900 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk1000 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chk1200 As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change43_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change43.aspx?AppID=" & Session("AppID").ToString

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If Session("AppHaendler") Is Nothing Then
            Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
        End If

        If (Session("objSuche") Is Nothing) Then
            Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objSuche"), CKG.Components.ComCommon.Finance.Search)
        End If
        If m_User.Reference.Trim(" "c).Length > 0 Then
            strHaendlernr = m_User.Reference
        Else
            strHaendlernr = objSuche.REFERENZ
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

        objHaendler = CType(Session("AppHaendler"), fin_06)

        'For future use
        ZeigeZULST.Visible = False
        cmdSearch.Visible = False

        If Not IsPostBack Then
            Kopfdaten1.Kontingente = objHaendler.Kontingente
            '---------------HALTERADRESSEN FÜLLEN
            Dim cmbItem As New System.Web.UI.WebControls.ListItem()
            cmbItem.Value = Kopfdaten1.HaendlerNummer
            cmbItem.Text = Kopfdaten1.HaendlerName & ", " & Replace(Kopfdaten1.Adresse, "<br>", ", ")
            cmbZweigstellen.Items.Add(cmbItem)

            objAddressList = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

            Dim tmpIntValue As Int32 = objAddressList.LeseAdressenSAP(Session("AppID").ToString, Session.SessionID, strHaendlernr, "B") 'B = HEZ Halter
            If tmpIntValue > 0 Then
                Dim tmpRow As DataRow
                For Each tmpRow In objAddressList.SearchResult.Rows
                    cmbItem = New System.Web.UI.WebControls.ListItem()
                    cmbItem.Value = tmpRow("POSTL_CODE").ToString & "," & tmpRow("CITY").ToString & ";" & tmpRow("ADDRESSNUMBER").ToString
                    cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                    ddlHalter.Items.Add(cmbItem)  'Hinzufügen der Elemente für Halter
                Next
            End If
            '---------------EMPFÄNGER SCHEIN & SCHILDER - ADRESSEN FÜLLEN
            tmpIntValue = objAddressList.LeseAdressenSAP(Session("AppID").ToString, Session.SessionID, strHaendlernr, "C") 'C = HEZ Schein & Schilder
            If tmpIntValue > 0 Then
                Dim tmpRow As DataRow

                For Each tmpRow In objAddressList.SearchResult.Rows
                    cmbItem = New System.Web.UI.WebControls.ListItem()
                    cmbItem.Value = tmpRow("ADDRESSNUMBER").ToString
                    cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                    ddlEmpf.Items.Add(cmbItem)  'Hinzufügen der Elemente für Schein & Schilder
                Next

                'cmbItem = New System.Web.UI.WebControls.ListItem()
                'cmbItem.Value = ""
                'cmbItem.Text = "DAD (inaktiv)"
                'ddlEmpf.Items.Add(cmbItem)
            End If
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim plz As String
        Dim ort As String
        Dim temp As String
        Dim status As String
        Dim nr As String

        temp = ddlHalter.SelectedItem.Value.ToString
        status = String.Empty

        plz = Left(temp, temp.IndexOf(","))
        ort = Right(temp, temp.Length - temp.IndexOf(",") - 1)
        ort = Left(ort, ort.IndexOf(";"))
        objHaendler = CType(Session("AppHaendler"), fin_06)
        objHaendler.getZulStelle(plz, ort, status)

        If (status <> String.Empty) Then
            lblError.Text = "Fehler: Für die angegebene Halter - PLZ konnte keine Zulassungsstelle ermittelt werden."
            Exit Sub
        End If

        Session("AppHaendler") = objHaendler    'Änderung merken

        Session("SelectedDeliveryValue") = cmbZweigstellen.SelectedItem.Value       'Haendlernummer (60...)
        Session("SelectedDeliveryText") = cmbZweigstellen.SelectedItem.Text         'Haendleradresse (Text...)

        'Für HEZ
        nr = ddlHalter.SelectedItem.Value
        nr = Right(nr, nr.Length - nr.IndexOf(";") - 1)

        Session("Materialnummer") = String.Empty
        Session("HalterAdresseNummer") = nr
        Session("HalterAdresseText") = ddlHalter.SelectedItem.Text
        Session("EmpfAdresseNummer") = ddlEmpf.SelectedItem.Value
        Session("EmpfAdresseText") = ddlEmpf.SelectedItem.Text

        Response.Redirect("Change43_4.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub ddlEmpf_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlEmpf.SelectedIndexChanged

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Change43_3.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 5.03.10    Time: 12:57
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 23.06.09   Time: 17:50
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Customer_Get_Children
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 13.03.08   Time: 14:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' RTFS Kopfdaten änderung auf Finance Kopfdaten 
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 21.01.08   Time: 9:45
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1618
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 3  *****************
' User: Uha          Date: 19.12.07   Time: 14:10
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1510,1512,1511 Anforderung / Zulassung
' 
' *****************  Version 2  *****************
' User: Uha          Date: 17.12.07   Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Versand ZB II / Briefe - komplierfähige Zwischenversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.12.07   Time: 17:18
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Elemente für Temp./Endg. bzw. HEZ Anforderung hinzugefügt (Change42ff,
' fin_06, Change43ff und fin_08)
' 
' ************************************************
