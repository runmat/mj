Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change53
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtAbDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents tr_Datumab As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Datumbis As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Datumab As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Datumbis As System.Web.UI.WebControls.Label

    Protected WithEvents ucHeader As Header

    Dim mObjBriefversand As Briefversand
    Dim mIDLiznr As String


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
        Session("ShowOtherString") = ""
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        NoDealer(Me, m_User)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)
        mIDLiznr = ""
        If Not Request.QueryString.Item("IDLIZNR") Is Nothing Then
            mIDLiznr = Request.QueryString.Item("IDLIZNR").ToString
        End If
        '&ART=TG
        If Not IsPostBack Then
            txtAbDatum.Text = Now.ToShortDateString
            txtBisDatum.Text = Now.ToShortDateString
        End If

    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        lblError.Text = ""
        Dim errorMessage As String = String.Empty


        If Not HelpProcedures.checkDate(txtAbDatum, txtBisDatum, errorMessage, False) Then
            lblError.Text = errorMessage
            Exit Sub
        End If

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        mObjBriefversand = New Briefversand(m_User, m_App, Session("AppId").ToString, Session.SessionID, strFileName)

        mObjBriefversand.DatumAb = txtAbDatum.Text
        mObjBriefversand.DatumBis = txtBisDatum.Text
        mObjBriefversand.ID_LIZNR = mIDLiznr

        mObjBriefversand.show(Session("AppID").ToString, Session.SessionID.ToString)
        Session("mObjBriefversandSession") = mObjBriefversand

        If Not mObjBriefversand.Status = 0 Then
            If mObjBriefversand.Status = -1111 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                lblError.Text = mObjBriefversand.Message
            End If
        Else
            If mObjBriefversand.BriefversandFehlerTabelle.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                If mIDLiznr = "X" Then
                    Response.Redirect("Change53_1.aspx?AppID=" & Session("AppID").ToString + "&IDLIZNR=" + mIDLiznr)
                Else
                    Response.Redirect("Change53_1.aspx?AppID=" & Session("AppID").ToString)
                End If

            End If
        End If

    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calBisDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        calAbDatum.Visible = False
        txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
        calBisDatum.Visible = False
        txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change53.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 22.12.10   Time: 15:03
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 9.11.10    Time: 10:01
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 3.12.08    Time: 17:25
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2079 anpassungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.09.08   Time: 10:42
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2079 fertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 28.07.08   Time: 15:12
' Created in $/CKAG/Components/ComCommon/Finance
' ITA 2079 fast fertig
'
' ************************************************