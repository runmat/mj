Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report46
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
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtAmtlKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBriefnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrdernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lbl_AmtlKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Briefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Leasingvertragsnr As System.Web.UI.WebControls.Label
    Protected WithEvents tr_AmtlKennzeichen As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Fahrgestellnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Briefnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Leasingvertragsnr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lb_Create As System.Web.UI.WebControls.LinkButton
    Protected WithEvents gvSelectOne As GridView
    Protected WithEvents tblSelektion As HtmlTable

    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)


        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If IsPostBack = False Then
            If (Not Request.QueryString("VIN") Is Nothing AndAlso Not Request.QueryString("VIN") Is String.Empty) Then
                txtFahrgestellnummer.Text = Request.QueryString("VIN")
                DoSubmit()
            End If
        End If
    End Sub

    Private Sub DoSubmit(Optional ByVal equnr As String = "")
        Dim m_Report As New fin_14(m_User, m_App, "")

        Dim strBriefnummer As String
        Dim strFahrgestellnummer As String
        Dim strAmtlKennzeichen As String
        Dim strOrdernummer As String

        If txtOrdernummer.Text.Length = 0 Then
            strOrdernummer = ""
        Else
            strOrdernummer = txtOrdernummer.Text
        End If
        If txtBriefnummer.Text.Length = 0 Then
            strBriefnummer = ""
        Else
            strBriefnummer = txtBriefnummer.Text
        End If
        If txtFahrgestellnummer.Text.Length = 0 Then
            strFahrgestellnummer = ""
        Else
            strFahrgestellnummer = txtFahrgestellnummer.Text
        End If
        If txtAmtlKennzeichen.Text.Length = 0 Then
            strAmtlKennzeichen = ""
        Else
            strAmtlKennzeichen = txtAmtlKennzeichen.Text
        End If

        If txtBriefnummer.Text.Length + txtFahrgestellnummer.Text.Length + txtAmtlKennzeichen.Text.Length + txtOrdernummer.Text.Length + equnr.Length = 0 Then
            lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
        Else
            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strOrdernummer, equnr)
            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                'wenn es mehrere DS gibt, ist die tabelle GT_EQUIS, die eine EQUINUMMER zurücklifert wenn aus dieser ein DS ausgewählt wurde. 
                If m_Report.diverseFahrzeuge.Rows.Count < 2 OrElse Not equnr.Length = 0 Then
                    If (m_Report.BRIEFLEBENSLAUF_LPTable Is Nothing) OrElse (m_Report.BRIEFLEBENSLAUF_LPTable.Rows.Count = 0) Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else
                        Session("BRIEFLEBENSLAUF_LPTable") = m_Report.BRIEFLEBENSLAUF_LPTable
                        Session("QMEL_DATENTable") = m_Report.QMEL_DATENTable
                        Session("QMMIDATENTable") = m_Report.QMMIDATENTable
                        Session("objReport") = m_Report

                        If Not m_Report.AnzBemerkungen Is Nothing Then
                            Session("AnzBemerkungen") = m_Report.AnzBemerkungen
                        Else
                            Session("AnzBemerkungen") = ""
                        End If

                        If Request.QueryString("Linked") = "false" Then 'wenn aus hauptmenü aufgerufen
                            Response.Redirect("Report46_2.aspx?AppID=" & Session("AppID").ToString)
                        Else 'wenn verlinkung
                            Response.Redirect("Report46_2.aspx?AppID=" & Session("AppID").ToString & "&Linked=true")
                        End If


                    End If
                Else
                    gvSelectOne.DataSource = m_Report.diverseFahrzeuge
                    gvSelectOne.DataBind()
                    tblSelektion.Visible = False
                    lb_Create.Visible = False
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

    Private Sub lb_Create_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Create.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub gvSelectOne_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSelectOne.RowCommand
        If e.CommandName = "weiter" Then
            DoSubmit(e.CommandArgument.ToString)
        End If
    End Sub
End Class

' ************************************************
' $History: Report46.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 9.04.10    Time: 9:41
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3628
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 4.09.09    Time: 9:55
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3050
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 30.03.09   Time: 14:14
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2760 verlinkung fahrzeughistorie
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 30.03.09   Time: 14:13
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2760 
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 22.12.08   Time: 9:56
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2413 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 31.07.08   Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2154
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 1.02.08    Time: 11:35
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 20.12.07   Time: 11:16
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1505 Fahrzeughistorie in Testversion
' 
' ************************************************
