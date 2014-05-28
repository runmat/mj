Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report46
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Create As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_AmtlKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents txtAmtlKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Briefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtBriefnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Leasingvertragsnr As System.Web.UI.WebControls.Label
    Protected WithEvents txtOrdernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents tr_AmtlKennzeichen As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Fahrgestellnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Briefnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Leasingvertragsnr As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents gvSelectOne As GridView
    Protected WithEvents tblSelektion As HtmlTable

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
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If (Not Request.QueryString("VIN") Is Nothing AndAlso Not Request.QueryString("VIN") Is String.Empty) Then
                txtFahrgestellnummer.Text = Request.QueryString("VIN")
                DoSubmit()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DoSubmit(Optional ByVal equnr As String = "")
        Dim m_Report As New FFE_Bank_Historie(m_User, m_App, "")

        Try
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
                            Response.Redirect("Report46_2.aspx?AppID=" & Session("AppID").ToString)
                        End If
                    Else
                        gvSelectOne.DataSource = m_Report.diverseFahrzeuge
                        gvSelectOne.DataBind()
                        tblSelektion.Visible = False
                        lb_Create.Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
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
' *****************  Version 5  *****************
' User: Jungj        Date: 16.02.09   Time: 16:50
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2413
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 7.07.08    Time: 10:26
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 2047/2035 Fahrzeughistorie Druckversion 
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
