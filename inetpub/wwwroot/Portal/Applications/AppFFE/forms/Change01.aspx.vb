Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change01
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As FFE_Search
    Private objHaendler As FFE_Haendler

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtVertragsNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrderNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Order As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Vertragsnr As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Vertragsnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Ordernummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Fahrgestellnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Kopfdaten1 As Kopfdatenhaendler

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
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Request.QueryString("HDL") = "1" Then ' Kontingente ausblenden?
                Session("AppShowNot") = True
            End If

            If (Session("objSuche") Is Nothing) Then
                objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_User.Reference) Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                    cmdSearch.Visible = False
                End If
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

            If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New FFE_Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objSuche.REFERENZ)
            Else
                objHaendler = CType(Session("objHaendler"), FFE_Haendler)
            End If

            cmdSearch.Enabled = False
            If objHaendler.Status = 0 Then
                cmdSearch.Enabled = True
            Else
                lblError.Text = "Fehler bei der Ermittlung der Kontingentdaten.<br>(" & objHaendler.Message & ")"
            End If

            Session("objHaendler") = objHaendler
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        lblError.Text = ""
        lblError.Visible = False
        Kopfdaten1.Message = ""

        objHaendler.SucheVertragsNr = txtVertragsNr.Text
        objHaendler.SucheOrderNr = txtOrderNr.Text
        objHaendler.SucheFahrgestellNr = Replace(txtFahrgestellNr.Text, "%", "*")
        objHaendler.KUNNR = m_User.KUNNR
        objHaendler.GiveCars()
        If Not objHaendler.Status = 0 Then
            lblError.Text = objHaendler.Message
            lblError.Visible = True
        Else
            If objHaendler.Fahrzeuge.Rows.Count = 0 Then
                Kopfdaten1.Message = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            Else
                Session("objHaendler") = objHaendler
                Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
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
' $History: Change01.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' Ausblenden Händler Kontingente
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/forms
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