Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report50
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
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Hertz_01(m_User, m_App, strFileName)
            Dim strKennzeichen As String = Nothing
            Dim datAbmeldedatumVon As DateTime
            Dim datAbmeldedatumBis As DateTime
            Dim strFahrgestellnummer As String = Nothing

            If (txtAbmeldedatumVon.Text = String.Empty And txtAbmeldedatumBis.Text = String.Empty) And (txtKennzeichen.Text = String.Empty And txtFahrgestellnummer.Text = String.Empty) Then
                lblError.Text = "Keine Abfragekriterien eingegeben."
                Exit Sub
            End If

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
            'End If
            'If Not txtAbmeldedatumBis.Text.Length = 0 Then
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

                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
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
End Class

' ************************************************
' $History: Report50.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:19
' Updated in $/CKAG/Applications/apphertz/Forms
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
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 13:01
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 23.05.07   Time: 9:36
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 22.05.07   Time: 13:07
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 6  *****************
' User: Uha          Date: 7.03.07    Time: 13:30
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Forms
' 
' ************************************************
