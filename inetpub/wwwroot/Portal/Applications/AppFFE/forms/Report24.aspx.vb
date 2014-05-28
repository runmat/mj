Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report24
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
    Private objSuche As FFE_Search
    Private objReport24_objFDDBank As FFE_BankBase
    Private objReport24_objFDDBank3 As FFE_Bank_3

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkAlle As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkAngefordert As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkNichtAngefordert As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Vertragsnr As System.Web.UI.WebControls.Label
    Protected WithEvents txtVertragsNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Order As System.Web.UI.WebControls.Label
    Protected WithEvents txtOrderNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_Vertragsnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Ordernummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Fahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents tr_Fahrgestellnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Briefnummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtBriefnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents tr_Briefnummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Kopfdaten1 As Kopfdatenhaendler
    'Private AppID As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("objSuche") Is Nothing) Then
                objSuche = New FFE_Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                'objSuche = New DealerSearch.Search(m_App, m_User, Session.SessionID.ToString, AppID)
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

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Request.QueryString("HDL") = "1" Then
                Session("AppShowNot") = True
            End If

            If Not IsPostBack Then
                objReport24_objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objReport24_objFDDBank.Customer = "60" & objSuche.REFERENZ
                objReport24_objFDDBank.CreditControlArea = "ZDAD"
                objReport24_objFDDBank.Show()
                If objReport24_objFDDBank.Status = 0 Then
                    Session("objReport24_objFDDBank") = objReport24_objFDDBank
                    cmdSearch.Enabled = True
                Else
                    lblError.Text = objReport24_objFDDBank.Message
                    cmdSearch.Enabled = False
                End If
            Else
                If Not Session("objReport24_objFDDBank") Is Nothing Then
                    objReport24_objFDDBank = CType(Session("objReport24_objFDDBank"), FFE_BankBase)
                    If objReport24_objFDDBank.Status = 0 Then
                        'Kopfdaten1.Kontingente = objReport24_objFDDBank.Kontingente
                        'Session("objReport24_objFDDBank") = objReport24_objFDDBank
                        cmdSearch.Enabled = True
                    Else
                        cmdSearch.Enabled = False
                        lblError.Text = objReport24_objFDDBank.Message
                    End If
                Else
                    Response.Redirect("../../../Start/Selection.aspx")
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        lblError.Text = ""
        lblError.Visible = False
        lblNoData.Text = ""

        If objReport24_objFDDBank.Status = 0 Then
            'Kopfdaten1.Kontingente = objReport24_objFDDBank.Kontingente

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objReport24_objFDDBank3 = New FFE_Bank_3(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            objReport24_objFDDBank3.Customer = "60" & objSuche.REFERENZ
            objReport24_objFDDBank3.CreditControlArea = "ZDAD"
            objReport24_objFDDBank3.FahrgestellnummerSuche = txtFahrgestellnummer.Text
            objReport24_objFDDBank3.BriefnummerSuche = txtBriefnummer.Text
            objReport24_objFDDBank3.Ordernummer = txtOrderNr.Text
            objReport24_objFDDBank3.Vertragsnummer = txtVertragsNr.Text
            If chkAlle.Checked Then
                objReport24_objFDDBank3.Vorgaenge = "alle"
            End If
            If chkAngefordert.Checked Then
                objReport24_objFDDBank3.Vorgaenge = "angefordert"
            End If
            If chkNichtAngefordert.Checked Then
                objReport24_objFDDBank3.Vorgaenge = "nichtangefordert"
            End If
            objReport24_objFDDBank3.Report()
            If Not objReport24_objFDDBank3.Fahrzeuge Is Nothing AndAlso objReport24_objFDDBank3.Fahrzeuge.Rows.Count > 0 Then
                Session("objReport24_objFDDBank3") = objReport24_objFDDBank3

                'Dim objExcelExport As New Excel.ExcelExport()
                'Try
                '    Excel.ExcelExport.WriteExcel(objReport24_objFDDBank3.FahrzeugeExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                '    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                'Catch
                'End Try
                Response.Redirect("Report24_2.aspx?AppID=" & Session("AppID").ToString)


            Else
                lblError.Text = "Zu den gewählten Kriterien wurden keine Fahrzeuge gefunden."
                lblError.Visible = True
            End If
        Else
            lblError.Text = objReport24_objFDDBank.Message
            lblError.Visible = True
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
' $History: Report24.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' Ausblenden Händler Kontingente
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 21.05.08   Time: 16:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Applications/AppFFE/forms
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.04.08    Time: 13:32
' Created in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 9.04.08    Time: 13:06
' Updated in $/CKG/Applications/AppFFE/AppFFEWeb/forms
' ITA: 1790
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
