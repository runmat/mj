Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report15_2
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
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
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
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

            Session("ABEKennzeichen") = ResultTable.Rows(0)("ZZKENN").ToString

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
            If Not IsDate(ResultTable.Rows(0)("REPLA_DATE").ToString) Then
                lblErstzulassungsdatum.Text = ""
            Else
                lblErstzulassungsdatum.Text = CDate(ResultTable.Rows(0)("REPLA_DATE")).ToShortDateString
            End If

            If Not IsDate(ResultTable.Rows(0)("ZZDAT_EIN")) Then
                lblEingangsdatum.Text = ""
            Else
                lblEingangsdatum.Text = CDate(ResultTable.Rows(0)("ZZDAT_EIN")).ToShortDateString
            End If

            If Not IsDate(ResultTable.Rows(0)("EXPIRY_DATE")) Then
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

            'Me.HyperLink1.NavigateUrl = "../../../Shared/Change06_3.aspx?EqNr=" & CType(ResultTable.Rows(0)("EQUNR"), String)
            'Me.HyperLink1.Target = "_blank"
            '------------ TEMP
            Me.HyperLink2.NavigateUrl = "../../../Shared/Change06_3NEU.aspx?EqNr=" & CType(ResultTable.Rows(0)("EQUNR"), String)
            Me.HyperLink2.Target = "_blank"
            '------------ TEMP
            If Not IsDate(ResultTable.Rows(0)("ZZTMPDT")) Then
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
            If Not IsDate(ResultTable.Rows(0)("UDATE")) Then
                lblUmgemeldetAm.Text = ""
            Else
                lblUmgemeldetAm.Text = CType(ResultTable.Rows(0)("UDATE"), Date).ToShortDateString
            End If
            lblFahrzeugmodell.Text = ResultTable.Rows(0)("ZZBEZEI").ToString
            If Not IsDate(ResultTable.Rows(0)("ERDAT")) Then
                lblBriefeingang.Text = ""
            Else
                lblBriefeingang.Text = CType(ResultTable.Rows(0)("ERDAT"), Date).ToShortDateString
            End If
            lblMindestlaufzeit.Text = ResultTable.Rows(0)("MINDBIS").ToString

            'lblHersteller.Text = ResultTable.Rows(0)("HERST_K").ToString   '§§§ JVE 27.09.2006
            lblHersteller.Text = ResultTable.Rows(0)("HERST_T").ToString
            '------------------------------------------------------------
            If Not IsDate(ResultTable.Rows(0)("ZZCARPORT_EING")) Then
                lblPDIEingang.Text = ""
            Else
                lblPDIEingang.Text = CDate(ResultTable.Rows(0)("ZZCARPORT_EING")).ToShortDateString
            End If
            If Not IsDate(ResultTable.Rows(0)("CHECK_IN")) Then
                lblCheckIn.Text = ""
            Else
                lblCheckIn.Text = CDate(ResultTable.Rows(0)("CHECK_IN")).ToShortDateString
            End If
            '§§§ 27.09.2006: Nur dann füllen, wenn Unfallfahrzeug: STATUS="U"
            If (Not ResultTable.Rows(0)("STATUS") Is Nothing) AndAlso (ResultTable.Rows(0)("STATUS").ToString = "U") Then
                If Not IsDate(ResultTable.Rows(0)("ZZKENN_EING")) Then
                    lblKennzeicheneingang.Text = String.Empty
                Else
                    lblKennzeicheneingang.Text = CDate(ResultTable.Rows(0)("ZZKENN_EING")).ToShortDateString
                End If
            End If

            '§§§ JVE 02.05.2006: PDI-Nummer mit ausgeben!
            lblPDI.Text = ResultTable.Rows(0)("KUNPDI").ToString
            lblPDIName.Text = " / " & ResultTable.Rows(0)("DADPDI_NAME1").ToString

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("Report15.aspx?AppID=" & Session("AppID").ToString)
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
' $History: Report15_2.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 10.03.11   Time: 11:37
' Updated in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 30.03.10   Time: 15:32
' Updated in $/CKAG/Applications/appec/Forms
' ITA: 3552
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 28.07.09   Time: 11:44
' Updated in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 28.07.09   Time: 11:42
' Updated in $/CKAG/Applications/appec/Forms
' anpassungen nach dynproxy umstellungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 26.06.09   Time: 14:53
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918, Z_M_Ec_Avm_Zulauf, Z_V_FAHRZEUG_STATUS_001,
' Z_M_EC_AVM_STATUS_BESTAND, Z_M_ABMBEREIT_LAUFAEN,
' Z_M_ABMBEREIT_LAUFZEIT, Z_M_Brief_Temp_Vers_Mahn_001,
' Z_M_SCHLUE_SET_MAHNSP_001, Z_M_SCHLUESSELDIFFERENZEN,
' Z_M_SCHLUESSELVERLOREN, Z_M_SCHLUE_TEMP_VERS_MAHN_001,
' Z_M_Ec_Avm_Status_Zul,  Z_M_ECA_TAB_BESTAND
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
