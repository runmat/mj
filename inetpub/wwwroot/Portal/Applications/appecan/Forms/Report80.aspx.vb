Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report80
    Inherits System.Web.UI.Page
    '##### ECAN Report "Täglicher Eingang Fahrzeugbriefe"
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
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtAbmeldedatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbmeldedatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
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
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New ECAN_02(m_User, m_App, strFileName)
            Dim datAbmeldedatumVon As DateTime
            Dim datAbmeldedatumBis As DateTime

            lblError.Text = ""

            If Not (txtAbmeldedatumVon.Text = String.Empty) Then
                If Not IsDate(txtAbmeldedatumVon.Text) Then
                    If Not IsStandardDate(txtAbmeldedatumVon.Text) Then
                        If Not IsSAPDate(txtAbmeldedatumVon.Text) Then
                            lblError.Text = "Geben Sie bitte ein gültiges ""Datum von"" ein!<br>"
                        Else
                            datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
                        End If
                    Else
                        datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
                    End If
                Else
                    datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
                End If
            Else
                lblError.Text = "Geben Sie bitte ein gültiges ""Datum von"" ein!<br>"
            End If

            If Not (txtAbmeldedatumBis.Text = String.Empty) Then
                If Not IsDate(txtAbmeldedatumBis.Text) Then
                    If Not IsStandardDate(txtAbmeldedatumBis.Text) Then
                        If Not IsSAPDate(txtAbmeldedatumBis.Text) Then
                            lblError.Text &= "Geben Sie bitte ein gültiges ""Datum bis"" ein!<br>"
                        Else
                            datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
                        End If
                    Else
                        datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
                    End If
                Else
                    datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
                End If
            Else
                lblError.Text &= "Geben Sie bitte ein gültiges ""Datum bis"" ein!<br>"
            End If

            If lblError.Text.Length = 0 Then
                If datAbmeldedatumVon > datAbmeldedatumBis Then
                    lblError.Text &= """Datum bis"" muss größer als ""Datum von"" sein!<br>"
                Else
                    If datAbmeldedatumVon > datAbmeldedatumBis Or DateAdd(DateInterval.Month, 1, datAbmeldedatumVon) < datAbmeldedatumBis Then
                        lblError.Text &= "Der maximale Zeitraum (""Datum von"" - ""Datum bis"") beträgt ein Monat!<br>"
                    End If
                End If
            End If

            If lblError.Text.Length = 0 Then
                m_Report.AbmeldedatumVon = datAbmeldedatumVon
                m_Report.AbmeldedatumBis = datAbmeldedatumBis
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

                Session("ResultTable") = m_Report.Result

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
                        logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Täglicher Eingang Fahrzeugbriefe")
                        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString & "&legende=Report80")
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report80.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 16:49
' Updated in $/CKAG/Applications/appecan/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 10:57
' Updated in $/CKAG/Applications/appecan/Forms
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:07
' Created in $/CKAG/Applications/appecan/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 12.07.07   Time: 14:46
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.07.07   Time: 14:45
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' In Report80 "Abmeldedatum" in Anzeige durch "Datum" ersetzt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.07.07   Time: 11:10
' Created in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' Report "Täglicher Eingang Fahrzeugbriefe" hinzugefügt
' 
' ************************************************
