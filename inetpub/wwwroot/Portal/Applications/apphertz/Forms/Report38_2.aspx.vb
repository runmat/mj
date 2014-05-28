Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report38_2
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
        Try
            m_App = New Base.Kernel.Security.App(m_User)

            Dim m_Report As New Hertz_02_1(m_User, m_App, "")
            Dim strAppName As String = "Report38_2"
            Dim dr() As DataRow = m_User.Applications.Select("AppName = '" & strAppName & "'")
            Dim strAppID As String = Nothing
            Dim strAppTitle As String = Nothing
            If dr.Length > 0 Then
                strAppID = CStr(dr(0)("AppID"))
                strAppTitle = CStr(dr(0)("AppFriendlyName"))
            End If
            Dim strFahrgestellNummer As String = Nothing 'Testdaten: = "WVGZZZ1TZ4W076640"
            If Not Request.QueryString("chassisnum") Is Nothing Then
                strFahrgestellNummer = CStr(Request.QueryString("chassisnum"))
            End If
            m_Report.FillMe(strAppID, Session.SessionID.ToString, strFahrgestellNummer)
            ResultTable = m_Report.Result

            lblHead.Text = strAppTitle
            ucStyles.TitleText = lblHead.Text

            If Not ResultTable Is Nothing AndAlso Not ResultTable.Rows Is Nothing AndAlso ResultTable.Rows.Count > 0 Then
                'Allgemein
                lblFahrgestellnummer.Text = ResultTable.Rows(0)("CHASSIS_NUM").ToString

                If MakeStandardDateString(ResultTable.Rows(0)("ERDAT").ToString) = "00.00.0000" Then
                    lblEingangSchluessel.Text = ""
                Else
                    lblEingangSchluessel.Text = MakeStandardDateString(ResultTable.Rows(0)("ERDAT").ToString)
                End If
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
                If MakeStandardDateString(ResultTable.Rows(0)("ZZTMPDT").ToString) = "00.00.0000" Then
                    lblAngefordertAm.Text = ""
                Else
                    lblAngefordertAm.Text = MakeStandardDateString(ResultTable.Rows(0)("ZZTMPDT").ToString)
                End If
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
            Else
                lblError.Text = "Es konnten keine Schlüsseldaten gefunden werden."
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub SetCBX(ByVal cbx As CheckBox, ByVal str As String)
        cbx.Checked = (str <> String.Empty)
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
' $History: Report38_2.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 16:17
' Updated in $/CKAG/Applications/apphertz/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:01
' Created in $/CKAG/Applications/apphertz/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 13:01
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 13:30
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Forms
' 
' ************************************************
