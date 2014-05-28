Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report30
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

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtZulassungspartner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtZulassungspartnerNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If (Not Request.QueryString("Ges") Is Nothing) AndAlso Request.QueryString("Ges").ToString = "1" Then
                DoSubmit()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim m_Report As New RDZ_01(m_User, m_App, "")

            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, txtKennzeichen.Text, txtPLZ.Text, txtZulassungspartner.Text, txtZulassungspartnerNr.Text, Page)

            Session("ResultTable") = m_Report.Result
            Session("ResultTableRaw") = m_Report.ResultRaw

            If Not m_Report.Status = 0 Then
                lblError.Text = "Fehler: " & m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else

                    'Dim objExcelExport As New DADWebClass.Excel.ExcelExport()
                    'Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    'Try
                    '    objExcelExport.WriteExcel(m_Report.Result, ConfigurationSettings.AppSettings("ExcelPath") & strFileName)
                    'Catch
                    'End Try
                    'Session("lnkExcel") = "/Portal/Temp/Excel/"& strFileName
                    Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Report30.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:59
' Created in $/CKAG/Applications/apprdz/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 13:32
' Updated in $/CKG/Applications/AppRDZ/AppRDZWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 23.05.07   Time: 9:13
' Updated in $/CKG/Applications/AppRDZ/AppRDZWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 11:14
' Updated in $/CKG/Applications/AppRDZ/AppRDZWeb/Forms
' 
' ************************************************
