Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report81_2
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private ResultTable As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkSchluesselinformationen As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblBriefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblBriefeingang As System.Web.UI.WebControls.Label
    Protected WithEvents lblHersteller As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugmodell As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeughalter As System.Web.UI.WebControls.Label
    Protected WithEvents lblErstzulassungsdatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrdernummer As System.Web.UI.WebControls.Label
    Protected WithEvents Tr5 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblZB2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblZB1 As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            m_App = New Base.Kernel.Security.App(m_User)
            If (Session("ResultTable") Is Nothing) Then
                Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
            Else
                ResultTable = CType(Session("ResultTable"), DataTable)
            End If
            lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString
            'lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            lblKennzeichen.Text = ResultTable.Rows(0)("ZZKENN").ToString
            lblBriefnummer.Text = ResultTable.Rows(0)("ZZBRIEF").ToString
            lblFahrgestellnummer.Text = ResultTable.Rows(0)("ZZFAHRG").ToString
            lnkSchluesselinformationen.NavigateUrl &= lblFahrgestellnummer.Text
            'lnkSchluesselinformationen.Visible = (lblFahrgestellnummer.Text <> String.Empty)
            lblOrdernummer.Text = ResultTable.Rows(0)("ZZREF1").ToString
            If MakeStandardDateString(ResultTable.Rows(0)("REPLA_DATE").ToString) = "00.00.0000" Then
                lblErstzulassungsdatum.Text = ""
            Else
                lblErstzulassungsdatum.Text = MakeStandardDateString(ResultTable.Rows(0)("REPLA_DATE").ToString)
            End If
            If ResultTable.Rows(0)("ZZSTATUS_ZUB").ToString = "X" Then
                lblStatus.Text = "Zulassungsfähig"
            End If
            If ResultTable.Rows(0)("ZZSTATUS_ZUL").ToString = "X" Then
                lblStatus.Text = "Zugelassen"
            End If
            If ResultTable.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
                lblStatus.Text = "Abgemeldet"
            End If
            If ResultTable.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
                lblStatus.Text = "In Abmeldung"
            End If
            If ResultTable.Rows(0)("ZZSTATUS_OZU").ToString = "X" Then
                lblStatus.Text = "Ohne Zulassung"
            End If
            If ResultTable.Rows(0)("ZZAKTSPERRE").ToString = "X" Then
                lblStatus.Text = "Gesperrt"
            End If

            Me.HyperLink1.NavigateUrl = "../../../Shared/Change06_3.aspx?EqNr=" & CType(ResultTable.Rows(0)("EQUNR"), String)


            lblZB1.Text = ResultTable.Rows(0)("NAME1_ZE").ToString
            If ResultTable.Rows(0)("NAME2_ZE").ToString.Length > 0 Then
                lblZB1.Text &= "<br>" & ResultTable.Rows(0)("NAME2_ZE").ToString
            End If
            lblZB1.Text &= "<br>"
            If ResultTable.Rows(0)("STRAS_ZE").ToString.Length > 0 Then
                lblZB1.Text &= ResultTable.Rows(0)("STRAS_ZE").ToString
            End If
            If ResultTable.Rows(0)("HSNR_ZE").ToString.Length > 0 Then
                lblZB1.Text &= " " & ResultTable.Rows(0)("HSNR_ZE").ToString
            End If
            lblZB1.Text &= "<br>"
            If ResultTable.Rows(0)("PSTLZ_ZE").ToString.Length > 0 Then
                lblZB1.Text &= ResultTable.Rows(0)("PSTLZ_ZE").ToString
            End If
            If ResultTable.Rows(0)("ORT01_ZE").ToString.Length > 0 Then
                lblZB1.Text &= " " & ResultTable.Rows(0)("ORT01_ZE").ToString
            End If
            lblZB2.Text = ResultTable.Rows(0)("NAME1_ZS").ToString
            If ResultTable.Rows(0)("NAME2_ZS").ToString.Length > 0 Then
                lblZB2.Text &= "<br>" & ResultTable.Rows(0)("NAME2_ZS").ToString
            End If
            lblZB2.Text &= "<br>"
            If ResultTable.Rows(0)("STRAS_ZS").ToString.Length > 0 Then
                lblZB2.Text &= ResultTable.Rows(0)("STRAS_ZS").ToString
            End If
            If ResultTable.Rows(0)("HSNR_ZS").ToString.Length > 0 Then
                lblZB2.Text &= " " & ResultTable.Rows(0)("HSNR_ZS").ToString
            End If
            lblZB2.Text &= "<br>"
            If ResultTable.Rows(0)("PSTLZ_ZS").ToString.Length > 0 Then
                lblZB2.Text &= ResultTable.Rows(0)("PSTLZ_ZS").ToString
            End If
            If ResultTable.Rows(0)("ORT01_ZS").ToString.Length > 0 Then
                lblZB2.Text &= " " & ResultTable.Rows(0)("ORT01_ZS").ToString
            End If
            lblFahrzeughalter.Text = ResultTable.Rows(0)("NAME1_ZH").ToString
            If ResultTable.Rows(0)("NAME2_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= "<br>" & ResultTable.Rows(0)("NAME2_ZH").ToString
            End If
            lblFahrzeughalter.Text &= "<br>"
            If ResultTable.Rows(0)("STRAS_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= ResultTable.Rows(0)("STRAS_ZH").ToString
            End If
            If ResultTable.Rows(0)("HSNR_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= " " & ResultTable.Rows(0)("HSNR_ZH").ToString
            End If
            lblFahrzeughalter.Text &= "<br>"
            If ResultTable.Rows(0)("PSTLZ_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= ResultTable.Rows(0)("PSTLZ_ZH").ToString
            End If
            If ResultTable.Rows(0)("ORT01_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= " " & ResultTable.Rows(0)("ORT01_ZH").ToString
            End If
            lblFahrzeugmodell.Text = ResultTable.Rows(0)("ZZBEZEI").ToString
            If MakeStandardDateString(ResultTable.Rows(0)("ERDAT").ToString) = "00.00.0000" Then
                lblBriefeingang.Text = ""
            Else
                lblBriefeingang.Text = MakeStandardDateString(ResultTable.Rows(0)("ERDAT").ToString)
            End If
            lblHersteller.Text = ResultTable.Rows(0)("HERST_K").ToString

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Function MakeStandardDateString(ByVal strSAPDate As String) As String
        Return Right(strSAPDate, 2) & "." & Mid(strSAPDate, 5, 2) & "." & Left(strSAPDate, 4)
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Report81_2.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:48
' Created in $/CKAG/Applications/appvw/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 8.03.07    Time: 15:06
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' ************************************************
