Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

' Anfroderungsnummer 1067 
' Erstellt am: 01.06.2007 - Sven Faßbender
Public Class Report39
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents txtDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents cldSelect As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnSelectDate As System.Web.UI.WebControls.Button
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Session("ShowLink") = "False"
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            If Request.QueryString("AppID").Length > 0 Then
                Session("AppID") = Request.QueryString("AppID").ToString
            End If

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            'Initiales befüllen
            If Not IsPostBack Then
                cmdCreate_Click(sender, e)
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
            lblError.Text = ""

            If lblError.Text.Length = 0 Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

                Dim m_Report As New FDD_AbgleichRetail(m_User, m_App, strFileName)

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
                        Session("lnkExcel") = "../Temp/Excel/" & strFileName
                        Response.Redirect("../../../Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If

        Catch aex As ArgumentException
            lblError.Text = aex.Message

        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    'Kalender anzeigen für Datumsauswahl
    Private Sub btnSelectDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectDate.Click
        Try
            Try 'Datum aus Textvox als Vorauswahl zu setzen
                Me.cldSelect.SelectedDate = Date.Parse(txtDate.Text)
            Catch ex As Exception
                Me.cldSelect.SelectedDate = Date.Today
            End Try
            Me.cldSelect.Visible = True
        Catch ex As Exception
            lblError.Text = "Beim Öffnen der Datumsauswahl ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    'Auswählen eines Datums und übernehmen in Textbox
    Private Sub cldSelect_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cldSelect.SelectionChanged
        Try
            Me.txtDate.Text = cldSelect.SelectedDate.ToShortDateString()
            Me.cldSelect.Visible = False
        Catch ex As Exception
            lblError.Text = "Beim Selektieren des Datums ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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
' $History: Report39.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.06.09   Time: 11:35
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 19.06.07   Time: 14:34
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************



