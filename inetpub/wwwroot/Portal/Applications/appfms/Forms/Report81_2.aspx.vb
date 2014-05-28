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
    Protected WithEvents lblFahrzeughalter As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents cbxCOC As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblErstzulassungsdatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrdernummer As System.Web.UI.WebControls.Label
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
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents lblGrund As System.Web.UI.WebControls.Label
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
            lnkKreditlimit.NavigateUrl = Request.UrlReferrer.ToString

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            Dim dRow As DataRow = ResultTable.Rows(0)

            lblKennzeichen.Text = dRow("ZZKENN").ToString
            lblBriefnummer.Text = dRow("ZZBRIEF").ToString
            lblFahrgestellnummer.Text = dRow("ZZFAHRG").ToString
            lnkSchluesselinformationen.NavigateUrl &= lblFahrgestellnummer.Text
            lblOrdernummer.Text = dRow("ZZREF1").ToString
            If IsDBNull(dRow("REPLA_DATE")) OrElse CType(dRow("REPLA_DATE"), DateTime).Year < 1901 Then
                lblErstzulassungsdatum.Text = ""
            Else
                lblErstzulassungsdatum.Text = CType(dRow("REPLA_DATE"), DateTime).ToShortDateString()
            End If
            If IsDBNull(dRow("ZZDAT_EIN")) OrElse CType(dRow("ZZDAT_EIN"), DateTime).Year < 1901 Then
                lblEingangsdatum.Text = ""
            Else
                lblEingangsdatum.Text = CType(dRow("ZZDAT_EIN"), DateTime).ToShortDateString()
            End If
            If IsDBNull(dRow("EXPIRY_DATE")) OrElse CType(dRow("EXPIRY_DATE"), DateTime).Year < 1901 Then
                lblStillegung.Text = ""
            Else
                lblStillegung.Text = CType(dRow("EXPIRY_DATE"), DateTime).ToShortDateString()
            End If
            If dRow("ZZSTATUS_ZUB").ToString = "X" Then
                lblStatus.Text = "Zulassungsfähig"
            End If
            If dRow("ZZSTATUS_ZUL").ToString = "X" Then
                lblStatus.Text = "Zugelassen"
            End If
            If dRow("ZZSTATUS_ABG").ToString = "X" Then
                lblStatus.Text = "Abgemeldet"
            End If
            If dRow("ZZSTATUS_BAG").ToString = "X" Then
                lblStatus.Text = "In Abmeldung"
            End If
            If dRow("ZZSTATUS_OZU").ToString = "X" Then
                lblStatus.Text = "Ohne Zulassung"
            End If
            If dRow("ZZAKTSPERRE").ToString = "X" Then
                lblStatus.Text = "Gesperrt"
            End If

            If dRow("ZZCOCKZ").ToString = "X" Then   'COC?
                cbxCOC.Checked = True
            End If

            Me.HyperLink1.NavigateUrl = "../../../Shared/Change06_3NEU.aspx?EqNr=" & CType(dRow("EQUNR"), String)

            If IsDBNull(dRow("ZZTMPDT")) OrElse CType(dRow("ZZTMPDT"), DateTime).Year < 1901 Then
                lblAngefordertAm.Text = ""
            Else
                lblAngefordertAm.Text = CType(dRow("ZZTMPDT"), DateTime).ToShortDateString()
            End If
            If dRow("SCHILDER_PHY").ToString = "X" Then
                chkVorhandeneElemente.Checked = True
            Else
                chkVorhandeneElemente.Checked = False
            End If
            lblVersandanschrift.Text = dRow("NAME1_VS").ToString
            If dRow("NAME2_VS").ToString.Length > 0 Then
                lblVersandanschrift.Text &= "<br>" & dRow("NAME2_VS").ToString
            End If
            lblVersandanschrift.Text &= "<br>"
            If dRow("STRAS_VS").ToString.Length > 0 Then
                lblVersandanschrift.Text &= dRow("STRAS_VS").ToString
            End If
            If dRow("HSNR_VS").ToString.Length > 0 Then
                lblVersandanschrift.Text &= " " & dRow("HSNR_VS").ToString
            End If
            lblVersandanschrift.Text &= "<br>"
            If dRow("PSTLZ_VS").ToString.Length > 0 Then
                lblVersandanschrift.Text &= dRow("PSTLZ_VS").ToString
            End If
            If dRow("ORT01_VS").ToString.Length > 0 Then
                lblVersandanschrift.Text &= " " & dRow("ORT01_VS").ToString
            End If
            lblFahrzeughalter.Text = dRow("NAME1_ZH").ToString
            If dRow("NAME2_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= "<br>" & dRow("NAME2_ZH").ToString
            End If
            lblFahrzeughalter.Text &= "<br>"
            If dRow("STRAS_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= dRow("STRAS_ZH").ToString
            End If
            If dRow("HSNR_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= " " & dRow("HSNR_ZH").ToString
            End If
            lblFahrzeughalter.Text &= "<br>"
            If dRow("PSTLZ_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= dRow("PSTLZ_ZH").ToString
            End If
            If dRow("ORT01_ZH").ToString.Length > 0 Then
                lblFahrzeughalter.Text &= " " & dRow("ORT01_ZH").ToString
            End If
            If dRow("SCHILDER_PHY").ToString = "X" Then
                chkVorhandeneElemente.Checked = True
            Else
                chkVorhandeneElemente.Checked = False
            End If
            If dRow("SCHEIN_PHY").ToString = "X" Then
                chkFahzeugschein.Checked = True
            Else
                chkFahzeugschein.Checked = False
            End If
            If dRow("ABCKZ").ToString = "1" Then
                rbTemporaer.Checked = True
                rbEndgueltig.Checked = False
            ElseIf dRow("ABCKZ").ToString = "2" Then
                rbTemporaer.Checked = False
                rbEndgueltig.Checked = True
            Else
                rbTemporaer.Checked = False
                rbEndgueltig.Checked = False
            End If
            lblFahrzeugmodell.Text = dRow("ZZBEZEI").ToString
            If IsDBNull(dRow("ERDAT")) OrElse CType(dRow("ERDAT"), DateTime).Year < 1901 Then
                lblBriefeingang.Text = ""
            Else
                lblBriefeingang.Text = CType(dRow("ERDAT"), DateTime).ToShortDateString()
            End If
            lblHersteller.Text = dRow("HERST_K").ToString
            If IsDBNull(dRow("ZZCARPORT_EING")) OrElse CType(dRow("ZZCARPORT_EING"), DateTime).Year < 1901 Then
                lblPDIEingang.Text = ""
            Else
                lblPDIEingang.Text = CType(dRow("ZZCARPORT_EING"), DateTime).ToShortDateString()
            End If
            If IsDBNull(dRow("CHECK_IN")) OrElse CType(dRow("CHECK_IN"), DateTime).Year < 1901 Then
                lblCheckIn.Text = ""
            Else
                lblCheckIn.Text = CType(dRow("CHECK_IN"), DateTime).ToShortDateString()
            End If
            If IsDBNull(dRow("ZZKENN_EING")) OrElse CType(dRow("ZZKENN_EING"), DateTime).Year < 1901 Then
                lblKennzeicheneingang.Text = ""
            Else
                lblKennzeicheneingang.Text = CType(dRow("ZZKENN_EING"), DateTime).ToShortDateString()
            End If
            '§§§ JVE 25.04.2006: Versandgrund einfügen
            lblGrund.Text = dRow("TEXT50").ToString

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

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
' User: Fassbenders  Date: 7.04.08    Time: 13:44
' Created in $/CKAG/Applications/appfms/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 12:58
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 7.03.07    Time: 13:11
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Forms
' 
' ******************************************
