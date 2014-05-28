Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report70
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
    Protected WithEvents txtStillegungVon As System.Web.UI.WebControls.TextBox
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
        If (txtStillegungVon.Text = String.Empty) Then
            lblError.Text = "Keine Abfragekriterien eingegeben."
        Else
            DoSubmit()
        End If
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Hertz_70(m_User, m_App, strFileName)
            Dim datStillegungVon As DateTime
            Dim strZKundennummer1 As String = ""
            Dim strZKundennummer2 As String = ""

            lblError.Text = ""
            If Not txtStillegungVon.Text.Length = 0 Then
                If Not IsDate(txtStillegungVon.Text) Then
                    If Not IsStandardDate(txtStillegungVon.Text) Then
                        If Not IsSAPDate(txtStillegungVon.Text) Then
                            lblError.Text &= "Geben Sie bitte ein gültiges Datum (von) ein!<br>"
                        Else
                            datStillegungVon = CDate(txtStillegungVon.Text)
                        End If
                    Else
                        datStillegungVon = CDate(txtStillegungVon.Text)
                    End If
                Else
                    datStillegungVon = CDate(txtStillegungVon.Text)
                End If
            Else
                'lblError.Text &= "Geben Sie bitte ein gültiges Datum (von) ein!<br>"
            End If

            If lblError.Text.Length = 0 Then
                m_Report.AppID = Session("AppID").ToString
                m_Report.StillegungVon = datStillegungVon
                'm_Report.StillegungBis = datStillegungBis
                m_Report.SessionID = Session.SessionID
                m_Report.FILL()

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
' $History: Report70.aspx.vb $
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
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 13:01
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 23.05.07   Time: 9:36
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 13:30
' Updated in $/CKG/Applications/AppHERTZ/AppHERTZWeb/Forms
' 
' ************************************************
