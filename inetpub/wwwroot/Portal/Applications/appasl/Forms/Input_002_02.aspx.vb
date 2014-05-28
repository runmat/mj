Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Input_002_02
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
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents calDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtLeasEVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLeasEBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLeasVVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLeasVBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtField As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblKF As System.Web.UI.WebControls.CheckBox
    Protected WithEvents rbMahnung As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rbSelect As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents txtKundennr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKonzernnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichenVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichenBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellBis As System.Web.UI.WebControls.TextBox
    Private flag As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        If Request.QueryString("FLAG") <> String.Empty Then
            flag = Request.QueryString("FLAG").ToString
        End If

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            Table1.Visible = True
            lblPageTitle.Visible = True
            cmdCreate.Visible = True

            If (flag = "1") Then
                DoSubmit(1)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub DoSubmit(ByVal direct As Integer)
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New ASL_01(m_User, m_App, strFileName)
            '---> Eingabedaten
            Dim status As String = ""
            Dim art As String

            With m_Report
                .PKennzeichenVon = txtKennzeichenVon.Text
                .PKennzeichenBis = txtKennzeichenBis.Text
                .PFahrgestellVon = txtFahrgestellVon.Text
                .PFahrgestellBis = txtFahrgestellBis.Text
                .PLeasingNrVon = txtLeasVVon.Text
                .PLeasingNrBis = txtLeasVBis.Text

                .PKonzernNr = txtKonzernnr.Text
                .PKundenNr = txtKundennr.Text
                .PKlaerfall = lblKF.Checked
            End With

            'art = rbSelect.SelectedItem.Value
            art = "M"
            If (art = "H") Then
                'status = rbStatus.SelectedItem.Value.ToString
            Else
                status = rbMahnung.SelectedItem.Value.ToString
            End If

            If (direct = 0) Then
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, status, art, True)
            Else
                art = "HM"
                m_Report.PKlaerfall = True
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, "ALL", art, True)
            End If
            Session("ResultTable") = m_Report.Result
            Session("ResultTableNative") = m_Report.getNativeData() 'Alle Spalten merken 

            If Not m_Report.Status = 0 Then
                Table1.Visible = False
                lblPageTitle.Visible = False
                cmdCreate.Visible = False
                lblError.Text = m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Table1.Visible = False
                    cmdCreate.Visible = False
                    lblPageTitle.Visible = False
                    lblHead.Visible = False
                Else
                   Try
                        Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch
                    End Try
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    Response.Redirect("Report_002_01.aspx?AppID=" & Session("AppID").ToString & "&typ=" & art)
                End If
            End If
            
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit(0)
    End Sub

    'Private Sub LinkButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    calDatum.Visible = True
    '    txtField.Text = "0"
    'End Sub

    'Private Sub LinkButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    calDatum.Visible = True
    '    txtField.Text = "1"
    'End Sub

    'Private Sub LinkButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    calDatum.Visible = True
    '    txtField.Text = "2"
    'End Sub

    'Private Sub LinkButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    calDatum.Visible = True
    '    txtField.Text = "3"
    'End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Input_002_02.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 5.08.10    Time: 14:52
' Updated in $/CKAG/Applications/appasl/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 10:19
' Updated in $/CKAG/Applications/appasl/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 24.04.08   Time: 14:28
' Updated in $/CKAG/Applications/appasl/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:04
' Created in $/CKAG/Applications/appasl/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 10:11
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 6.03.07    Time: 18:02
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' 
' ************************************************
