Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report64
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
    Protected WithEvents txtZulassungsdatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtZulassungsdatumBis As System.Web.UI.WebControls.TextBox
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
        If (txtZulassungsdatumVon.Text = String.Empty) And (txtZulassungsdatumBis.Text = String.Empty) Then
            lblError.Text = "Keine Abfragekriterien eingegeben."
        Else
            DoSubmit()
        End If
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            'Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            'Dim m_Report As New ANC_B04(m_User, m_App, strFileName)
            Dim m_Report As New ANC_B04(m_User, m_App, "")
            Dim datZulassungsdatumVon As DateTime
            Dim datZulassungsdatumBis As DateTime
            Dim strZKundennummer1 As String = ""
            Dim strZKundennummer2 As String = ""

            lblError.Text = ""
            If Not txtZulassungsdatumVon.Text.Length = 0 Then
                If Not IsDate(txtZulassungsdatumVon.Text) Then
                    If Not IsStandardDate(txtZulassungsdatumVon.Text) Then
                        If Not IsSAPDate(txtZulassungsdatumVon.Text) Then
                            lblError.Text &= "Geben Sie bitte ein gültiges Datum (von) ein!<br>"
                        Else
                            datZulassungsdatumVon = CDate(txtZulassungsdatumVon.Text)
                        End If
                    Else
                        datZulassungsdatumVon = CDate(txtZulassungsdatumVon.Text)
                    End If
                Else
                    datZulassungsdatumVon = CDate(txtZulassungsdatumVon.Text)
                End If
            Else
                'lblError.Text &= "Geben Sie bitte ein gültiges Datum (von) ein!<br>"
            End If
            If Not txtZulassungsdatumBis.Text.Length = 0 Then
                If Not IsDate(txtZulassungsdatumBis.Text) Then
                    If Not IsStandardDate(txtZulassungsdatumBis.Text) Then
                        If Not IsSAPDate(txtZulassungsdatumBis.Text) Then
                            lblError.Text &= "Geben Sie bitte ein gültiges Datum (bis) ein!<br>"
                        Else
                            datZulassungsdatumBis = CDate(txtZulassungsdatumBis.Text)
                        End If
                    Else
                        datZulassungsdatumBis = CDate(txtZulassungsdatumBis.Text)
                    End If
                Else
                    datZulassungsdatumBis = CDate(txtZulassungsdatumBis.Text)
                End If
            Else
                'lblError.Text &= "Geben Sie bitte ein gültiges Datum (bis) ein!<br>"
            End If

            If lblError.Text.Length = 0 Then
                m_Report.AppID = Session("AppID").ToString
                m_Report.ZulassungsdatumVon = datZulassungsdatumVon
                m_Report.ZulassungsdatumBis = datZulassungsdatumBis
                m_Report.SessionID = Session.SessionID
                m_Report.Fill()

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
        txtZulassungsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtZulassungsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub
End Class

' ************************************************
' $History: Report64.aspx.vb $
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
