Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports CKG.Components.ComCommon

Public Class Change43
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
    Private objHaendler As fin_06
    Private m_blnDoSubmit As Boolean

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As ComCommon.PageElements.Kopfdaten
    Protected WithEvents txtVertragsNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrderNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_HaendlerNr As System.Web.UI.WebControls.Label
    Protected WithEvents txtHaendlerNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Vertragsnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Ordernummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents tr_HaendlerNr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Vertragsnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Ordernummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Fahrgestellnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Haendler As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lbl_Haendler As System.Web.UI.WebControls.Label
    Protected WithEvents ddlHaendler As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If m_User.Reference.Trim(" "c).Length > 0 Then
            txtHaendlerNr.Text = m_User.Reference
        End If

        If Me.IsPostBack = False Then
            fillDDL()
        End If

        m_blnDoSubmit = False

        If (Not Request.QueryString("back") Is Nothing) AndAlso CStr(Request.QueryString("back")) = "1" Then
            'Session("AppHaendler") = Nothing
            If Not Session("objSuche") Is Nothing Then
                txtHaendlerNr.Text = CType(Session("objSuche"), CKG.Components.ComCommon.Finance.Search).REFERENZ
            End If
        End If

        If txtHaendlerNr.Text.Trim(" "c).Length = 0 AndAlso ddlHaendler.SelectedIndex = 0 Then
            tr_HaendlerNr.Visible = True
            tr_Haendler.Visible = True
            tr_Fahrgestellnummer.Visible = False
            tr_Vertragsnummer.Visible = False
            tr_Ordernummer.Visible = False
            Kopfdaten1.Visible = False
            Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
            Literal1.Text &= "			<!-- //" & vbCrLf
            Literal1.Text &= "			window.document.Form1.txtHaendlerNr.focus();" & vbCrLf
            Literal1.Text &= "			//-->" & vbCrLf
            Literal1.Text &= "		</script>" & vbCrLf
        Else

            'kontingenttabelle ausblenden wenn Parameter
            If Request.QueryString("HDL") = 1 Then
                'asdfasdf
                'asdfasdf
                Session("AppShowNot") = True
            End If


            Dim strHaendler As String = ""

            If Not txtHaendlerNr.Text = "" OrElse Not txtHaendlerNr.Text Is String.Empty Then
                strHaendler = txtHaendlerNr.Text
            Else
                If Not ddlHaendler.SelectedIndex = 0 Then
                    strHaendler = ddlHaendler.SelectedItem.Value
                End If
            End If


            objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, strHaendler) Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                cmdSearch.Visible = False
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

            If (Session("AppHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New fin_06(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", strHaendler, m_User.KUNNR, True)
            Else
                objHaendler = CType(Session("AppHaendler"), fin_06)
                m_blnDoSubmit = True
            End If

            cmdSearch.Enabled = False
            If objHaendler.Status = 0 Then
                cmdSearch.Enabled = True
                If Not IsPostBack Then
                    Kopfdaten1.Kontingente = objHaendler.Kontingente
                End If
            Else
                lblError.Text = "Fehler bei der Ermittlung der Kontingentdaten.<br>(" & objHaendler.Message & ")"
            End If

            Session("AppHaendler") = objHaendler

            tr_HaendlerNr.Visible = False
            tr_Haendler.Visible = False
            tr_Fahrgestellnummer.Visible = True
            tr_Vertragsnummer.Visible = True
            tr_Ordernummer.Visible = True
            Kopfdaten1.Visible = True
            Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
            Literal1.Text &= "			<!-- //" & vbCrLf
            Literal1.Text &= "			window.document.Form1.txtVertragsNr.focus();" & vbCrLf
            Literal1.Text &= "			//-->" & vbCrLf
            Literal1.Text &= "		</script>" & vbCrLf

        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub


    Private Sub fillDDL()
        objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
        If objSuche.LeseHaendlerSAP(CStr(Me.Session("AppID")), Session.SessionID) > 0 Then
            objSuche.Haendler.Sort = "NAME"
            ddlHaendler.DataSource = objSuche.Haendler
            ddlHaendler.DataTextField = "DISPLAY"
            ddlHaendler.DataValueField = "REFERENZ"
            ddlHaendler.DataBind()
            ddlHaendler.Items.Insert(0, "-keiner-")
            objSuche = Nothing
        Else
            lblError.Text = objSuche.ErrorMessage
        End If
    End Sub

    Private Sub DoSubmit()
        If m_blnDoSubmit Then
            lblError.Text = ""
            lblError.Visible = False
            Kopfdaten1.Message = ""

            objHaendler.SucheTIDNR = txtVertragsNr.Text
            objHaendler.SucheZZREFERENZ1 = txtOrderNr.Text
            objHaendler.SucheFahrgestellNr = Replace(txtFahrgestellNr.Text, "%", "*")
            objHaendler.KUNNR = m_User.KUNNR
            objHaendler.GiveCars(Session("AppID").ToString, Session.SessionID)
            If Not objHaendler.Status = 0 Then
                lblError.Text = objHaendler.Message
                lblError.Visible = True
            Else
                If objHaendler.Fahrzeuge.Rows.Count = 0 Then
                    Kopfdaten1.Message = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    Session("AppHaendler") = objHaendler
                    Response.Redirect("Change43_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
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
' $History: Change43.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.06.09   Time: 14:57
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Unangefordert_002
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
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
' *****************  Version 12  *****************
' User: Jungj        Date: 13.03.08   Time: 14:41
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' RTFS Kopfdaten änderung auf Finance Kopfdaten 
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 13.03.08   Time: 10:54
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' RTFS Anpassungen
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 5.03.08    Time: 12:54
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen 1733
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 4.03.08    Time: 18:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF ANPASSUNGEN ITA 1733
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 4.03.08    Time: 10:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733, 1667, 1738 
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 26.02.08   Time: 8:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 7.01.08    Time: 16:21
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' HEZ Bugfix + Dropdownliste zur Händlerauswahl 
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 4  *****************
' User: Uha          Date: 19.12.07   Time: 16:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
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
