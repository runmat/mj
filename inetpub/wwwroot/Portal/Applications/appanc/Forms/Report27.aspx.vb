Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report27
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
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbmeldedatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbmeldedatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
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

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        'Session("lnkExcel") = ""
        Try
            'Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            'Dim m_Report As New ANC_B07(m_User, m_App, strFileName)
            Dim m_Report As New ANC_B07(m_User, m_App, "")
            Dim strKennzeichen As String = ""
            Dim datAbmeldedatumVon As DateTime
            Dim datAbmeldedatumBis As DateTime
            Dim strFahrgestellnummer As String = ""

            If txtKennzeichen.Text.Length = 0 Then
                'strKennzeichen = "|"
            Else
                strKennzeichen = txtKennzeichen.Text
            End If
            If txtFahrgestellnummer.Text.Length = 0 Then
                'strFahrgestellnummer = "|"
            Else
                strFahrgestellnummer = txtFahrgestellnummer.Text
            End If
            lblError.Text = ""
            'If Not txtAbmeldedatumVon.Text.Length = 0 Then
            If Not IsDate(txtAbmeldedatumVon.Text) Then
                If Not IsStandardDate(txtAbmeldedatumVon.Text) Then
                    If Not IsSAPDate(txtAbmeldedatumVon.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges ""Abmeldedatum von"" ein!<br>"
                    Else
                        datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
                    End If
                Else
                    datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
                End If
            Else
                datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
            End If
            'End If
            'If Not txtAbmeldedatumBis.Text.Length = 0 Then
            If Not IsDate(txtAbmeldedatumBis.Text) Then
                If Not IsStandardDate(txtAbmeldedatumBis.Text) Then
                    If Not IsSAPDate(txtAbmeldedatumBis.Text) Then
                        lblError.Text &= "Geben Sie bitte ein gültiges ""Abmeldedatum bis"" ein!<br>"
                    Else
                        datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
                    End If
                Else
                    datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
                End If
            Else
                datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
            End If
            'End If
            If lblError.Text.Length = 0 Then
                m_Report.Fill(Me.Page, Session("AppID").ToString, Session.SessionID.ToString, datAbmeldedatumVon, datAbmeldedatumBis, strKennzeichen, strFahrgestellnummer)

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else

                        'Keine "alte" Excel-Erzeugung mehr
                        'Dim objExcelExport As New Excel.ExcelExport()
                        'Try
                        '    objExcelExport.WriteExcel(m_Report.Result, ConfigurationSettings.AppSettings("ExcelPath") & strFileName)
                        'Catch
                        'End Try
                        'Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtAbmeldedatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtAbmeldedatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub
End Class

' ************************************************
' $History: Report27.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:55
' Updated in $/CKAG/Applications/appanc/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 15:28
' Updated in $/CKAG/Applications/appanc/Forms
' Warnungen entfernt.
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:41
' Created in $/CKAG/Applications/appanc/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 20.08.07   Time: 13:30
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Forms
' ITA 1250 Testversion
' 
' *****************  Version 8  *****************
' User: Uha          Date: 21.06.07   Time: 15:20
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 23.05.07   Time: 10:33
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 6  *****************
' User: Uha          Date: 6.03.07    Time: 13:45
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Forms
' 
' ************************************************
