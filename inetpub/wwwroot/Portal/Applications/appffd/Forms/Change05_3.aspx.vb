Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change05_3
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

    Private objHaendler As FDD_Haendler

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As Kopfdaten
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
        lnkFahrzeugAuswahl.NavigateUrl = "Change05_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change05.aspx?AppID=" & Session("AppID").ToString

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

            objHaendler = CType(Session("objHaendler"), FDD_Haendler)

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

                objAddressList = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

                Dim tmpIntValue As Int32 = objAddressList.LeseAdressenSAP(m_User.Reference, "B") 'B = HEZ Halter
                If tmpIntValue > 0 Then
                    Dim tmpRow As DataRow
                    For Each tmpRow In objAddressList.SearchResult.Rows
                        cmbItem = New System.Web.UI.WebControls.ListItem()
                        cmbItem.Value = tmpRow("POSTL_CODE").ToString & "," & tmpRow("CITY").tostring & ";" & tmpRow("ADDRESSNUMBER").ToString
                        cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                        ddlHalter.Items.Add(cmbItem)  'Hinzufügen der Elemente für Halter
                    Next
                End If
                '---------------EMPFÄNGER SCHEIN & SCHILDER - ADRESSEN FÜLLEN
                tmpIntValue = objAddressList.LeseAdressenSAP(m_User.Reference, "C") 'C = HEZ Schein & Schilder
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
        objHaendler = CType(Session("objHaendler"), FDD_Haendler)
        objHaendler.getZulStelle(Session("AppID").ToString, Session.SessionID, Me, plz, ort, status)

        If (status <> String.Empty) Then
            lblError.Text = "Fehler: Für die angegebene Halter - PLZ konnte keine Zulassungsstelle ermittelt werden."
            Exit Sub
        End If

        Session("objHaendler") = objHaendler    'Änderung merken

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

        Response.Redirect("Change05_4.aspx?AppID=" & Session("AppID").ToString)
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
' $History: Change05_3.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************
