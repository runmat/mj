Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report200
    Inherits System.Web.UI.Page
    '##### ARVAL Report: Abgemeldete Fahrzeuge
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
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbmeldedatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbmeldedatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents calZul As System.Web.UI.WebControls.Calendar
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
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

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function checkInput() As Boolean
        Dim datevon As Date
        Dim datebis As Date

        Try
            datevon = CType(txtAbmeldedatumVon.Text, Date)
            datebis = CType(txtAbmeldedatumBis.Text, Date)
        Catch ex As Exception
            Return False
        End Try

        If (datevon > datebis) Then
            Return False
        End If
        Return True
    End Function

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        If (checkInput()) Then
            DoSubmit()
        Else
            lblError.Text = "Falsche Eingabeparameter."
        End If
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")),
                         m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName,
                         m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Arval_01(m_User, m_App, strFileName)
            'Dim strKennzeichen As String
            Dim datAbmeldedatumVon As DateTime
            Dim datAbmeldedatumBis As DateTime
            'Dim strFahrgestellnummer As String

            If (txtAbmeldedatumVon.Text = String.Empty And txtAbmeldedatumBis.Text = String.Empty) And (txtKennzeichen.Text = String.Empty And txtFahrgestellnummer.Text = String.Empty) Then
                lblError.Text = "Keine Abfragekriterien eingegeben."
                Exit Sub
            End If

            'If txtKennzeichen.Text.Length <> 0 Then
            '    strKennzeichen = txtKennzeichen.Text
            'End If
            'If txtFahrgestellnummer.Text.Length <> 0 Then
            '    strFahrgestellnummer = txtFahrgestellnummer.Text
            'End If
            lblError.Text = ""

            If Not (txtAbmeldedatumVon.Text = String.Empty) Then
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
            End If
           
            If Not (txtAbmeldedatumBis.Text = String.Empty) Then
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
            End If

            If lblError.Text.Length = 0 Then
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, datAbmeldedatumVon, datAbmeldedatumBis)

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
                        logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Abgemeldete Fahrzeuge")
                        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            'logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler im Report <Abgemeldete Fahrzeuge>. Fehler: " & ex.Message)
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

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtAbmeldedatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
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
' $History: Report200.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:20
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 23.05.07   Time: 10:28
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
