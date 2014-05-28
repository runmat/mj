Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report03_2
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
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents rbTemporaer As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents rbEndgueltig As System.Web.UI.WebControls.RadioButton
    Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblBriefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblBriefeingang As System.Web.UI.WebControls.Label
    Protected WithEvents lblHersteller As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugmodell As System.Web.UI.WebControls.Label
    Protected WithEvents lblEingangsdatum As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents cbxCOC As System.Web.UI.WebControls.CheckBox
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
    Protected WithEvents lblVersandanschrift As System.Web.UI.WebControls.Label
    Protected WithEvents Tr5 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblLeasingnehmer As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeughalter As System.Web.UI.WebControls.Label
    Protected WithEvents lblLieferant As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        Try
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
            'lnkSchluesselinformationen.Visible = (lblFahrgestellnummer.Text <> String.Empty)
            lblOrdernummer.Text = ResultTable.Rows(0)("ZZREF1").ToString

            If ResultTable.Rows(0)("REPLA_DATE") Is DBNull.Value Then
                lblErstzulassungsdatum.Text = ""
            Else
                lblErstzulassungsdatum.Text = CDate(ResultTable.Rows(0)("REPLA_DATE")).ToShortDateString
            End If
            If ResultTable.Rows(0)("ZZDAT_EIN") Is DBNull.Value Then
                lblEingangsdatum.Text = ""
            Else
                lblEingangsdatum.Text = CDate(ResultTable.Rows(0)("ZZDAT_EIN")).ToShortDateString
            End If

            If ResultTable.Rows(0)("EXPIRY_DATE") Is DBNull.Value Then
                lblStillegung.Text = ""
            Else
                lblStillegung.Text = CDate(ResultTable.Rows(0)("EXPIRY_DATE")).ToShortDateString
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
                lblStatus.Text = "Bei Abmeldung"
            End If
            If ResultTable.Rows(0)("ZZSTATUS_OZU").ToString = "X" Then
                lblStatus.Text = "Ohne Zulassung"
            End If
            If ResultTable.Rows(0)("ZZAKTSPERRE").ToString = "X" Then
                lblStatus.Text = "Gesperrt"
            End If

            If ResultTable.Rows(0)("ZZCOCKZ").ToString = "X" Then   'COC?
                cbxCOC.Checked = True
            End If

            Me.HyperLink1.NavigateUrl = "../../../Shared/Change06_3.aspx?EqNr=" & CType(ResultTable.Rows(0)("EQUNR"), String)


            If ResultTable.Rows(0)("ZZTMPDT") Is DBNull.Value Then
                lblAngefordertAm.Text = ""
            Else
                lblAngefordertAm.Text = CDate(ResultTable.Rows(0)("ZZTMPDT")).ToShortDateString
            End If
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

            '***Leasingnehmer***
            lblLeasingnehmer.Text = ResultTable.Rows(0)("NAME1_ZL").ToString
            If ResultTable.Rows(0)("NAME2_ZL").ToString.Length > 0 Then
                lblLeasingnehmer.Text &= "<br>" & ResultTable.Rows(0)("NAME2_ZL").ToString
            End If
            lblLeasingnehmer.Text &= "<br>"
            If ResultTable.Rows(0)("STRAS_ZL").ToString.Length > 0 Then
                lblLeasingnehmer.Text &= ResultTable.Rows(0)("STRAS_ZL").ToString
            End If
            If ResultTable.Rows(0)("HSNR_ZL").ToString.Length > 0 Then
                lblLeasingnehmer.Text &= " " & ResultTable.Rows(0)("HSNR_ZL").ToString
            End If
            lblLeasingnehmer.Text &= "<br>"
            If ResultTable.Rows(0)("PSTLZ_ZL").ToString.Length > 0 Then
                lblLeasingnehmer.Text &= ResultTable.Rows(0)("PSTLZ_ZL").ToString
            End If
            If ResultTable.Rows(0)("ORT01_ZL").ToString.Length > 0 Then
                lblLeasingnehmer.Text &= " " & ResultTable.Rows(0)("ORT01_ZL").ToString
            End If


            '***Lieferant***
            lblLieferant.Text = ResultTable.Rows(0)("NAME1_ZF").ToString
            If ResultTable.Rows(0)("NAME2_ZF").ToString.Length > 0 Then
                lblLieferant.Text &= "<br>" & ResultTable.Rows(0)("NAME2_ZF").ToString
            End If
            lblLieferant.Text &= "<br>"
            If ResultTable.Rows(0)("STRAS_ZF").ToString.Length > 0 Then
                lblLieferant.Text &= ResultTable.Rows(0)("STRAS_ZF").ToString
            End If
            If ResultTable.Rows(0)("HSNR_ZF").ToString.Length > 0 Then
                lblLieferant.Text &= " " & ResultTable.Rows(0)("HSNR_ZF").ToString
            End If
            lblLieferant.Text &= "<br>"
            If ResultTable.Rows(0)("PSTLZ_ZF").ToString.Length > 0 Then
                lblLieferant.Text &= ResultTable.Rows(0)("PSTLZ_ZF").ToString
            End If
            If ResultTable.Rows(0)("ORT01_ZF").ToString.Length > 0 Then
                lblLieferant.Text &= " " & ResultTable.Rows(0)("ORT01_ZF").ToString
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
            If ResultTable.Rows(0)("UDATE") Is DBNull.Value Then
                lblUmgemeldetAm.Text = ""
            Else
                lblUmgemeldetAm.Text = CDate(ResultTable.Rows(0)("UDATE")).ToShortDateString
            End If
            lblFahrzeugmodell.Text = ResultTable.Rows(0)("ZZBEZEI").ToString

            If ResultTable.Rows(0)("ERDAT") Is DBNull.Value Then
                lblBriefeingang.Text = ""
            Else
                lblBriefeingang.Text = CDate(ResultTable.Rows(0)("ERDAT")).ToShortDateString
            End If
            lblMindestlaufzeit.Text = ResultTable.Rows(0)("MINDBIS").ToString
            lblHersteller.Text = ResultTable.Rows(0)("HERST_K").ToString

            If ResultTable.Rows(0)("ZZCARPORT_EING") Is DBNull.Value Then
                lblPDIEingang.Text = ""
            Else
                lblPDIEingang.Text = CDate(ResultTable.Rows(0)("ZZCARPORT_EING")).ToShortDateString
            End If
            If ResultTable.Rows(0)("CHECK_IN") Is DBNull.Value Then
                lblCheckIn.Text = ""
            Else
                lblCheckIn.Text = CDate(ResultTable.Rows(0)("CHECK_IN")).ToShortDateString
            End If

            If ResultTable.Rows(0)("ZZKENN_EING") Is DBNull.Value Then
                lblKennzeicheneingang.Text = ""
            Else
                lblKennzeicheneingang.Text = CDate(ResultTable.Rows(0)("ZZKENN_EING")).ToShortDateString
            End If


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
' $History: Report03_2.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 30.06.09   Time: 16:23
' Updated in $/CKAG/Applications/AppLeaseTrend/Forms
' ITa 2918 nachbesserungen
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 15:07
' Created in $/CKAG/Applications/AppLeaseTrend/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 11.03.08   Time: 11:17
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' Bugfix OR aus Partner ZF -> ZL jemacht
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 13:19
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Forms
' 
' ************************************************
