Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report38
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
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblFahrgestellnummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblAngefordertAm As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersandanschrift As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersendetAmDescription As System.Web.UI.WebControls.Label
    Protected WithEvents lblVersendetAm As System.Web.UI.WebControls.Label
    Protected WithEvents lblEingangSchluessel As System.Web.UI.WebControls.Label
    Protected WithEvents cbxErsatzschluessel As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxCarpass As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxRadiocodekarte As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxCDNavi As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxChipkarte As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxCoCPapier As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxNaviCodekarte As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxCodekarteWFS As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxErsatzfernbedStandh As System.Web.UI.WebControls.CheckBox
    Protected WithEvents cbxPruefbuch As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents rbEndgueltig As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents rbTemporaer As System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        m_App = New Base.Kernel.Security.App(m_User)

        Dim m_Report As New Report_38(m_User, m_App, "")
        Dim strAppName As String = "Report38"
        Dim dr() As DataRow = m_User.Applications.Select("AppName = '" & strAppName & "'")
        Dim strAppID As String = ""
        Dim strAppTitle As String = ""
        If dr.Length > 0 Then
            strAppID = CStr(dr(0)("AppID"))
            strAppTitle = CStr(dr(0)("AppFriendlyName"))
        End If
        Dim strFahrgestellNummer As String = ""
        If Not Request.QueryString("chassisnum") Is Nothing Then
            strFahrgestellNummer = CStr(Request.QueryString("chassisnum"))
        End If
        m_Report.Fill(strAppID, Session.SessionID.ToString, Me.Page, strFahrgestellNummer)
        ResultTable = m_Report.Result

        lblHead.Text = strAppTitle
        ucStyles.TitleText = lblHead.Text

        If Not ResultTable Is Nothing AndAlso Not ResultTable.Rows Is Nothing AndAlso ResultTable.Rows.Count > 0 Then
            'Allgemein
            lblFahrgestellnummer.Text = ResultTable.Rows(0)("CHASSIS_NUM").ToString


            lblEingangSchluessel.Text = MakeStandardDateString(ResultTable.Rows(0)("ERDAT").ToString)

            'Inhalt Tuete
            SetCBX(cbxErsatzschluessel, ResultTable.Rows(0)("ERSATZSCHLUESSEL").ToString)
            SetCBX(cbxCarpass, ResultTable.Rows(0)("CARPASS").ToString)
            SetCBX(cbxRadiocodekarte, ResultTable.Rows(0)("RADIOCODEKARTE").ToString)
            SetCBX(cbxCDNavi, ResultTable.Rows(0)("NAVICD").ToString)
            SetCBX(cbxChipkarte, ResultTable.Rows(0)("CHIPKARTE").ToString)
            SetCBX(cbxCoCPapier, ResultTable.Rows(0)("COCBESCHEINIGUNG").ToString)
            SetCBX(cbxNaviCodekarte, ResultTable.Rows(0)("NAVICODEKARTE").ToString)
            SetCBX(cbxCodekarteWFS, ResultTable.Rows(0)("WEGFAHRSPCODEKARTE").ToString)
            SetCBX(cbxErsatzfernbedStandh, ResultTable.Rows(0)("ERSATZFERNBSH").ToString)
            SetCBX(cbxPruefbuch, ResultTable.Rows(0)("PRUEFBUCH_LKW").ToString)
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
            'Letzter Versanddaten
            'Datum

            lblAngefordertAm.Text = MakeStandardDateString(ResultTable.Rows(0)("ZZTMPDT").ToString)

            'Versandanschrift
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
            'Ende
        End If
       
    End Sub

    Private Sub SetCBX(ByVal cbx As CheckBox, ByVal str As String)
        cbx.Checked = (str <> String.Empty)
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
' $History: Report38.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 17.03.10   Time: 11:32
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 9.03.10    Time: 14:02
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon
' Migration
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
