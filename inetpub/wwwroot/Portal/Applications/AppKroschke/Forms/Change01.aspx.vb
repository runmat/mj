Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change01
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
    Private objFahrer As CK_01

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectVon As System.Web.UI.WebControls.Button
    Protected WithEvents txtVonDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents ShowVon As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ShowBis As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents calVonDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                'DoSubmit()
            Else
                'objFahrer = CType(Session("objFahrer"), CK_01)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        lblError.Visible = True

        'If txtFahrernummer.Text.Trim(" "c).Length = 0 Then
        '    lblError.Text = "Es muss eine Fahrernummer angegeben werden."
        '    Exit Sub
        'End If
        If Not IsDate(txtVonDatum.Text) Then
            lblError.Text = "Kein gültiges Startdatum eingetragen."
            Exit Sub
        End If
        If Not IsDate(txtBisDatum.Text) Then
            lblError.Text = "Kein gültiges Enddatum eingetragen."
            Exit Sub
        End If
        If CDate(txtVonDatum.Text) > CDate(txtBisDatum.Text) Then
            lblError.Text = "Startdatum muss vor Enddatum liegen."
            Exit Sub
        End If
        If DateAdd(DateInterval.Day, 30, CDate(txtVonDatum.Text)) < CDate(txtBisDatum.Text) Then
            lblError.Text = "Die Zeitspanne darf maximal 30 Tage einschließen."
            Exit Sub
        End If

        Session("lnkExcel") = ""
        lblError.Text = ""
        lblError.Visible = False

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        objFahrer = New CK_01(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
        objFahrer.Customer = m_User.KUNNR
        objFahrer.FahrerNummer = m_User.Reference.Trim(" "c)
        objFahrer.VonDatum = CDate(txtVonDatum.Text)
        objFahrer.BisDatum = CDate(txtBisDatum.Text)
        objFahrer.show(Session("AppID").ToString, Session.SessionID.ToString)
        If Not objFahrer.Status = 0 Then
            lblError.Text = objFahrer.Message
            lblError.Visible = True
        Else
            If objFahrer.FahrerTabelle.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            Else
                Dim tblTemp As DataTable
                tblTemp = objFahrer.FahrerTabelle.Copy
                'tblTemp.Columns.Remove("Status")

                Dim objExcelExport As New Excel.ExcelExport()
                Try
                    Excel.ExcelExport.WriteExcel(tblTemp, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                Catch
                End Try
                Session("objFahrer") = objFahrer
                Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub btnOpenSelectVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectVon.Click
        calVonDatum.Visible = True
        calBisDatum.Visible = False
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calVonDatum.Visible = False
        calBisDatum.Visible = True
    End Sub

    Private Sub calVonDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calVonDatum.SelectionChanged
        calVonDatum.Visible = False
        txtVonDatum.Text = calVonDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
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
' $History: Change01.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 12.01.10   Time: 17:18
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 3332
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 14.10.08   Time: 13:07
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 2301 & Warnungen bereinigt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 13:07
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 9:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' ************************************************
