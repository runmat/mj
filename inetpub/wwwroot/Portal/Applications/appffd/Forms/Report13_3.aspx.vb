Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report13_3
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
    Private objVertragsSuche As FDD_Vertragsstatus_1

    Protected WithEvents chkTemporaer As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkEntgueltig As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlernummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblVertragsnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblOrdernummer As System.Web.UI.WebControls.Label
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblBriefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblFinanzierungsart As System.Web.UI.WebControls.Label
    Protected WithEvents lblMahnverfahren As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnforderungsdatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersanddatum As System.Web.UI.WebControls.Label
    Protected WithEvents chkBezahlt As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkJa As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkNein As System.Web.UI.WebControls.RadioButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblVersandadresse As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbrechnungsdatum As System.Web.UI.WebControls.Label
    Protected WithEvents chkVersJa As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkVersNein As System.Web.UI.WebControls.RadioButton
    Protected WithEvents cbxCOCBesch As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkDelayPayment As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblKunde As System.Web.UI.WebControls.Label
    Protected WithEvents Tr2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblBetrag As System.Web.UI.WebControls.Label
    Protected WithEvents chkRetail As System.Web.UI.WebControls.RadioButton
    Protected WithEvents TrBetrag As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lnkKreditlimit.NavigateUrl = "Report13_2.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            'm_App = New DADWebClass.App(m_User)

            If Session("objVertrag") Is Nothing Then
                Response.Redirect("Report13_2.aspx?AppID=" & Session("AppID").ToString)
            Else
                objVertragsSuche = CType(Session("objVertrag"), FDD_Vertragsstatus_1)
                If objVertragsSuche.Anforderungsdatum > CDate("01.01.1900") Then
                    lblAnforderungsdatum.Text = objVertragsSuche.Anforderungsdatum.ToShortDateString
                End If
                If objVertragsSuche.Versanddatum > CDate("01.01.1900") Then
                    lblVersanddatum.Text = objVertragsSuche.Versanddatum.ToShortDateString
                End If
                If objVertragsSuche.Abrechnungdatum > CDate("01.01.1900") Then
                    lblAbrechnungsdatum.Text = objVertragsSuche.Abrechnungdatum.ToShortDateString
                End If
                lblBriefnummer.Text = objVertragsSuche.Briefnummer
                lblFahrgestellnummer.Text = objVertragsSuche.Fahrgestellnummer
                lblFinanzierungsart.Text = objVertragsSuche.Finanzierungsart
                lblHaendlernummer.Text = objVertragsSuche.Haendlernummer
                lblKennzeichen.Text = objVertragsSuche.Kennzeichen
                lblMahnverfahren.Text = objVertragsSuche.Mahnverfahren
                lblOrdernummer.Text = objVertragsSuche.Ordernummer
                lblVertragsnummer.Text = objVertragsSuche.Vertragsnummer
                lblVersandadresse.Text = objVertragsSuche.NAME
                lblKunde.Text = objVertragsSuche.Kunde
                If IsNumeric(objVertragsSuche.Betrag) Then
                    lblBetrag.Text = Format(CDec(objVertragsSuche.Betrag), "##,##0.00 €")
                End If
                If objVertragsSuche.NAME_2.Length > 0 Then
                    lblVersandadresse.Text &= "<br>" & objVertragsSuche.NAME_2
                End If
                If objVertragsSuche.NAME_3.Length > 0 Then
                    lblVersandadresse.Text &= "<br>" & objVertragsSuche.NAME_3
                End If
                If objVertragsSuche.POSTL_CODE.Length > 0 Then
                    lblVersandadresse.Text &= "<br>" & objVertragsSuche.POSTL_CODE
                End If
                If objVertragsSuche.CITY.Length > 0 Then
                    lblVersandadresse.Text &= "&nbsp;" & objVertragsSuche.CITY
                End If
                If objVertragsSuche.STREET.Length > 0 Then
                    lblVersandadresse.Text &= "<br>" & objVertragsSuche.STREET
                End If
                If objVertragsSuche.HOUSE_NUM.Length > 0 Then
                    lblVersandadresse.Text &= "&nbsp;" & objVertragsSuche.HOUSE_NUM
                End If
                If objVertragsSuche.Bezahlt Then
                    chkBezahlt.Checked = True
                Else
                    chkBezahlt.Checked = False
                End If
                cbxCOCBesch.Checked = objVertragsSuche.COCBescheinigungVorhanden
                If objVertragsSuche.Angefordert Then
                    chkJa.Checked = True
                Else
                    chkNein.Checked = True
                End If
                If objVertragsSuche.Versendet Then
                    chkVersJa.Checked = True
                Else
                    chkVersNein.Checked = True
                End If
                If objVertragsSuche.AngefordertTemporaer Then
                    chkTemporaer.Checked = True
                End If
                If objVertragsSuche.AngefordertEndgueltig Then
                    chkEntgueltig.Checked = True
                End If
                If objVertragsSuche.AngefordertRetail Then
                    chkRetail.Checked = True
                End If
                chkDelayPayment.Visible = False
                If objVertragsSuche.AngefordertDelayPayment Then
                    chkDelayPayment.Checked = True
                    chkDelayPayment.Visible = True
                End If

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'DoSubmit()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report13_3.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 27.08.07   Time: 15:48
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA: 1278
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 14.06.07   Time: 14:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 13.06.07   Time: 16:19
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************

