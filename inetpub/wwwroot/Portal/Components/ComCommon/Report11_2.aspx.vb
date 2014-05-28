Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report11_2
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private ResultTable As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    'Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblBriefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblBriefeingang As System.Web.UI.WebControls.Label
    Protected WithEvents lblHersteller As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugmodell As System.Web.UI.WebControls.Label
    Protected WithEvents lblEingangsdatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeughalter As System.Web.UI.WebControls.Label
    Protected WithEvents lblErstzulassungsdatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblMindestlaufzeitDescription As System.Web.UI.WebControls.Label
    Protected WithEvents lblMindestlaufzeit As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrdernummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblEhemaligesKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblUmgemeldetAm As System.Web.UI.WebControls.Label
    Protected WithEvents lblEhemaligeBriefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents chkBriefaufbietung As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblPDIEingang As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeicheneingang As System.Web.UI.WebControls.Label
    Protected WithEvents lblCheckIn As System.Web.UI.WebControls.Label
    Protected WithEvents chkFahzeugschein As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkVorhandeneElemente As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblStillegung As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersendetAmDescription As System.Web.UI.WebControls.Label
    Protected WithEvents lblAngefordertAm As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersendetAm As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents rbTemporaer As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents rbEndgueltig As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblVersandanschrift As System.Web.UI.WebControls.Label
    Protected WithEvents Tr5 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cbxCOC As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lnkSchluesselinformationen As System.Web.UI.WebControls.HyperLink
    Protected WithEvents HyperLink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblPDI As System.Web.UI.WebControls.Label
    Protected WithEvents lblPDIName As System.Web.UI.WebControls.Label
    Protected WithEvents lblBereitdatum As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        m_App = New Base.Kernel.Security.App(m_User)
        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString)
        Else
            ResultTable = CType(Session("ResultTable"), DataTable)
        End If
        lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString & "?AppID=" & Session("AppID").ToString

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        lblKennzeichen.Text = ResultTable.Rows(0)("ZZKENN").ToString
        lblEhemaligesKennzeichen.Text = ResultTable.Rows(0)("ZZKENN_OLD").ToString
        lblBriefnummer.Text = ResultTable.Rows(0)("ZZBRIEF").ToString
        lblEhemaligeBriefnummer.Text = ResultTable.Rows(0)("ZZBRIEF_OLD").ToString
        If ResultTable.Rows(0)("ZZBRIEF_A").ToString = "X" Then
            chkBriefaufbietung.Checked = True
        Else
            chkBriefaufbietung.Checked = False
        End If
        lblFahrgestellnummer.Text = ResultTable.Rows(0)("ZZFAHRG").ToString
        lnkSchluesselinformationen.NavigateUrl &= lblFahrgestellnummer.Text
        lnkSchluesselinformationen.Visible = (lblFahrgestellnummer.Text <> String.Empty)
        lblOrdernummer.Text = ResultTable.Rows(0)("ZZREF1").ToString

        lblErstzulassungsdatum.Text = MakeStandardDateString(ResultTable.Rows(0)("REPLA_DATE").ToString)


        lblEingangsdatum.Text = MakeStandardDateString(ResultTable.Rows(0)("ZZDAT_EIN").ToString)


        lblStillegung.Text = MakeStandardDateString(ResultTable.Rows(0)("EXPIRY_DATE").ToString)

        If ResultTable.Rows(0)("ZZSTATUS_ZUB").ToString = "X" Then
            lblStatus.Text = "Zulassungsf�hig"
        End If
        If ResultTable.Rows(0)("ZZSTATUS_ZUL").ToString = "X" Then
            lblStatus.Text = "Zugelassen"
        End If
        If ResultTable.Rows(0)("ZZSTATUS_ABG").ToString = "X" Then
            lblStatus.Text = "Abgemeldet"
        End If
        If ResultTable.Rows(0)("ZZSTATUS_BAG").ToString = "X" Then
            lblStatus.Text = "Bei Abmeldung"
        End If
        If ResultTable.Rows(0)("ZZSTATUS_OZU").ToString = "X" Then
            lblStatus.Text = "Ohne Zulassung"
        End If

        '��� JVE 28.09.2006: Bereitdatum eingef�gt.

        lblBereitdatum.Text = MakeStandardDateString(ResultTable.Rows(0)("ZZDAT_BER").ToString)
        '------------------------------------------

        If ResultTable.Rows(0)("ZZCOCKZ").ToString = "X" Then   'COC?
            cbxCOC.Checked = True
        End If

        If ResultTable.Rows(0)("ZZDAT_BER").ToString = "X" Then   'COC?
            cbxCOC.Checked = True
        End If

        'Me.HyperLink1.NavigateUrl = "../../../Shared/Change06_3.aspx?EqNr=" & CType(ResultTable.Rows(0)("EQUNR"), String)
        'Me.HyperLink1.Target = "_blank"
        '------------ TEMP
        Me.HyperLink2.NavigateUrl = "../../Shared/Change06_3NEU.aspx?EqNr=" & CType(ResultTable.Rows(0)("EQUNR"), String)
        Me.HyperLink2.Target = "_blank"
        '------------ TEMP

        lblAngefordertAm.Text = MakeStandardDateString(ResultTable.Rows(0)("ZZTMPDT").ToString)

        If ResultTable.Rows(0)("SCHILDER_PHY").ToString = "X" Then
            chkVorhandeneElemente.Checked = True
        Else
            chkVorhandeneElemente.Checked = False
        End If
        lblVersandanschrift.Text = ResultTable.Rows(0)("NAME1_VS").ToString
        If ResultTable.Rows(0)("NAME2_VS").ToString.Length > 0 Then
            lblVersandanschrift.Text &= "<br>" & ResultTable.Rows(0)("NAME2_VS").ToString
        End If
        lblVersandanschrift.Text &= "<br>"
        If ResultTable.Rows(0)("STRAS_VS").ToString.Length > 0 Then
            lblVersandanschrift.Text &= ResultTable.Rows(0)("STRAS_VS").ToString
        End If
        If ResultTable.Rows(0)("HSNR_VS").ToString.Length > 0 Then
            lblVersandanschrift.Text &= " " & ResultTable.Rows(0)("HSNR_VS").ToString
        End If
        lblVersandanschrift.Text &= "<br>"
        If ResultTable.Rows(0)("PSTLZ_VS").ToString.Length > 0 Then
            lblVersandanschrift.Text &= ResultTable.Rows(0)("PSTLZ_VS").ToString
        End If
        If ResultTable.Rows(0)("ORT01_VS").ToString.Length > 0 Then
            lblVersandanschrift.Text &= " " & ResultTable.Rows(0)("ORT01_VS").ToString
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
        If ResultTable.Rows(0)("SCHILDER_PHY").ToString = "X" Then
            chkVorhandeneElemente.Checked = True
        Else
            chkVorhandeneElemente.Checked = False
        End If
        If ResultTable.Rows(0)("SCHEIN_PHY").ToString = "X" Then
            chkFahzeugschein.Checked = True
        Else
            chkFahzeugschein.Checked = False
        End If
        If ResultTable.Rows(0)("ABCKZ").ToString = "1" Then
            rbTemporaer.Checked = True
            rbEndgueltig.Checked = False
        ElseIf ResultTable.Rows(0)("ABCKZ").ToString = "2" Then
            rbTemporaer.Checked = False
            rbEndgueltig.Checked = True
        Else
            rbTemporaer.Checked = False
            rbEndgueltig.Checked = False
        End If

        lblUmgemeldetAm.Text = MakeStandardDateString(ResultTable.Rows(0)("UDATE").ToString)

        lblFahrzeugmodell.Text = ResultTable.Rows(0)("ZZBEZEI").ToString

        lblBriefeingang.Text = MakeStandardDateString(ResultTable.Rows(0)("ERDAT").ToString)

        lblMindestlaufzeit.Text = ResultTable.Rows(0)("MINDBIS").ToString
        lblHersteller.Text = ResultTable.Rows(0)("HERST_K").ToString

        lblPDIEingang.Text = MakeStandardDateString(ResultTable.Rows(0)("ZZCARPORT_EING").ToString)
        lblCheckIn.Text = MakeStandardDateString(ResultTable.Rows(0)("CHECK_IN").ToString)

        lblKennzeicheneingang.Text = MakeStandardDateString(ResultTable.Rows(0)("ZZKENN_EING").ToString)

        '��� JVE 02.05.2006: PDI-Nummer mit ausgeben!
        lblPDI.Text = ResultTable.Rows(0)("KUNPDI").ToString
        lblPDIName.Text = " / " & ResultTable.Rows(0)("DADPDI_NAME1").ToString
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

    End Sub

    Private Function MakeStandardDateString(ByVal strSAPDate As String) As String
        If IsDate(strSAPDate) Then
            Return CDate(strSAPDate).ToShortDateString
        Else
            Return ""
        End If
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report11_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 17.03.10   Time: 11:32
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' m�gliche try catches entfernt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 5.10.07    Time: 10:46
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' 
