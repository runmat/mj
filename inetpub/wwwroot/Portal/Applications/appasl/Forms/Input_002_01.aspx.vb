Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Input_002_01
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
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents calDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtLeasVVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLeasVBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtField As System.Web.UI.WebControls.TextBox
    Protected WithEvents rbSelect As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents lblKF As System.Web.UI.WebControls.CheckBox
    Protected WithEvents rbMahnung As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents rbStatus As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents txtKundennr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKonzernnr As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichenVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichenBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents rbAuswahl As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try

            If Not (Session("Link")) = Nothing Then
                If Session("Link") = "Report_002" Then
                    lblHead.Text = "Sicherungsscheine Historie"
                Else
                    lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                End If
            Else
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            End If

            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                Session("ShowLink") = "False"
                Session("ResultTable") = Nothing
            End If
            
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        rbSelect.SelectedItem.Value = "H"
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New ASL_01(m_User, m_App, strFileName)
            '---> Eingabedaten

            Dim status As String

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

            If (rbSelect.SelectedItem.Value = "H") Then
                status = rbStatus.SelectedItem.Value.ToString
            Else
                status = rbMahnung.SelectedItem.Value.ToString
            End If

            '---> BAPI-Aufruf, Excel-Export und Darstellung (Report01_2)

            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, status, rbSelect.SelectedItem.Value)
            Session("ResultTable") = m_Report.Result
            Session("ResultTableNative") = m_Report.getNativeData() 'Alle Spalten merken 

            If Not m_Report.Status = 0 Then
                lblError.Text = "Fehler: " & m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Try
                        Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch
                    End Try
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    Response.Redirect("Report_002_01.aspx?AppID=" & Session("AppID").ToString & "&typ=" & rbSelect.SelectedItem.Value)
                End If
            End If
            
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    'Private Sub lnkAuswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If (rbSelect.SelectedItem.Value = "H") Then
    '        rbStatus.Visible = True
    '        rbMahnung.Visible = False
    '        'lblTitel.Text = "Historie"
    '    Else
    '        rbStatus.Visible = False
    '        rbMahnung.Visible = True
    '        'lblTitel.Text = "Mahnstufen"
    '    End If
    'End Sub

    'Private Sub LinkButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    calDatum.Visible = True
    '    txtField.Text = "0"
    'End Sub

    'Private Sub LinkButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    calDatum.Visible = True
    '    txtField.Text = "1"
    'End Sub

    'Private Sub LinkButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    calDatum.Visible = True
    '    txtField.Text = "2"
    'End Sub

    'Private Sub LinkButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
' $History: Input_002_01.aspx.vb $
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
' *****************  Version 9  *****************
' User: Rudolpho     Date: 3.01.08    Time: 9:28
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' ITA: 1593
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:11
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 6.03.07    Time: 18:02
' Updated in $/CKG/Applications/AppASL/AppASLWeb/Forms
' 
' ************************************************
