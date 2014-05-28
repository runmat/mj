Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report18
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
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents rbEin As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbAus As System.Web.UI.WebControls.RadioButton
    Protected WithEvents calBisDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtAbDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents calAbDatum As System.Web.UI.WebControls.Calendar
    Protected WithEvents txtBisDatum As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectAb As System.Web.UI.WebControls.Button
    Protected WithEvents btnOpenSelectBis As System.Web.UI.WebControls.Button
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        Session("ShowOtherString") = ""
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        NoDealer(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
        If Not IsPostBack Then
            calBisDatum.SelectedDate = Today
            txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
        End If
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            lblError.Text = ""
            If Not IsDate(txtAbDatum.Text) Then
                If Not IsStandardDate(txtAbDatum.Text) Then
                    If Not IsSAPDate(txtAbDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                    End If
                End If
            End If
            If Not IsDate(txtBisDatum.Text) Then
                If Not IsStandardDate(txtBisDatum.Text) Then
                    If Not IsSAPDate(txtBisDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                    End If
                End If
            End If

            If lblError.Text.Length > 0 Then
                Exit Sub
            End If

            Dim datAb As Date = CDate(txtAbDatum.Text)
            Dim datBis As Date = CDate(txtBisDatum.Text)
            If datAb > datBis Then
                lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
                Exit Sub
            End If


            Dim strAction As String = "NEU"
            If rbAus.Checked Then
                strAction = "AUS"
            End If

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New CSC_EinAusgaenge(m_User, m_App, datAb, datBis, strAction, strFileName)

            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Session("ShowOtherString") = "Es wurden " & m_Report.Result.Rows.Count.ToString & " Einträge zu ""Eingänge"" gefunden."
                    If rbAus.Checked Then
                        Session("ShowOtherString") = "Es wurden " & m_Report.Result.Rows.Count.ToString & " Einträge zu ""Ausgänge"" gefunden."
                    End If

                    Dim objExcelExport As New Excel.ExcelExport()
                    Try
                        Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch
                    End Try
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try
    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calBisDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        calAbDatum.Visible = False
        txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
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
' $History: Report18.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.03.10   Time: 13:34
' Updated in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 2.05.08    Time: 10:37
' Updated in $/CKAG/Applications/appcsc/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:43
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 23.05.07   Time: 10:08
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' 
' ************************************************
