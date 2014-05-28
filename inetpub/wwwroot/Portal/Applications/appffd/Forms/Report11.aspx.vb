Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report11
    Inherits System.Web.UI.Page
    '##### MDR Report ITA 1162 "Daten ohne Brief"
    '##### MDR Report ITA 1223 "Gesamtbestand Fahrzeugbriefe"
    '##### MDR Report ITA 1161 "Briefe ohne Fahrzeug"
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
    Private m_intTask As Integer

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
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

            m_intTask = CInt(Request.QueryString("ITA"))

            cmdCreate.Enabled = True

            Select Case m_intTask
                Case 1162, 1223, 1161

                Case Else
                    lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(Fehlender Anwendungsparameter)"
            End Select
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strQueryExtension As String = ""
            Dim intStatus As Integer
            Dim strMessage As String = ""
            Dim intRowsCount As Integer
            Select Case m_intTask
                Case 1162
                    Dim m_Report As New MDR_01(m_User, m_App, "")
                    m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)
                    intStatus = m_Report.Status
                    strMessage = m_Report.Message
                    intRowsCount = CheckRowCount(m_Report.Result)
                    Session("ResultTable") = m_Report.Result
                    If Not intStatus = -1111 AndAlso m_Report.Result Is Nothing OrElse Not intStatus = -1111 AndAlso m_Report.result.Rows.Count = 0 Then
                        intStatus = -1111
                    End If
                Case 1223
                    Dim m_Report As New MDR_04(m_User, m_App, "")
                    m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)
                    intStatus = m_Report.Status
                    strMessage = m_Report.Message
                    intRowsCount = CheckRowCount(m_Report.Result)
                    Session("ResultTable") = m_Report.Result
                    strQueryExtension = "&csv=Ja"
                    If Not intStatus = -1111 AndAlso m_Report.Result Is Nothing OrElse Not intStatus = -1111 AndAlso m_Report.result.Rows.Count = 0 Then
                        intStatus = -1111
                    End If
                Case 1161
                    Dim m_Report As New MDR_05(m_User, m_App, "")
                    m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)
                    intStatus = m_Report.Status
                    strMessage = m_Report.Message
                    intRowsCount = CheckRowCount(m_Report.Result)
                    Session("ResultTable") = m_Report.Result
                    If Not intStatus = -1111 AndAlso m_Report.Result Is Nothing OrElse Not intStatus = -1111 AndAlso m_Report.result.Rows.Count = 0 Then
                        intStatus = -1111
                    End If
            End Select

            If Not intStatus = 0 Then

                If intStatus = -1111 Then
                    lblError.Text = "Keine Daten zur Anzeige gefunden"
                Else
                    lblError.Text = "Fehler: " & strMessage
                End If

            Else
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: " & lblHead.Text)
                Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString & strQueryExtension)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function CheckRowCount(ByVal objTable As Object) As Integer
        Dim intReturn As Integer = 0
        If Not objTable Is Nothing AndAlso TypeOf objTable Is DataTable Then
            intReturn = CType(objTable, DataTable).Rows.Count
        End If
        Return intReturn
    End Function


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report11.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 28.04.08   Time: 15:28
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 16.04.08   Time: 13:09
' Updated in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 8.04.08    Time: 13:45
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ita 1826
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 8.04.08    Time: 13:44
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ita 1826
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 8.04.08    Time: 13:33
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ita 1826
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 8.04.08    Time: 11:37
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITA 1826
' 
' *****************  Version 4  *****************
' User: Uha          Date: 16.08.07   Time: 11:37
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' ITAs 1162, 1223 und 1161 werden jetzt über Report11.aspx abgewickelt.
' Report14 wieder komplett gelöscht.
' 
' *****************  Version 3  *****************
' User: Uha          Date: 15.08.07   Time: 16:34
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logeintrag leicht modifiziert
' 
' *****************  Version 2  *****************
' User: Uha          Date: 8.08.07    Time: 17:23
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Bugfixing "Daten ohne Brief"
' 
' *****************  Version 1  *****************
' User: Uha          Date: 8.08.07    Time: 13:48
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Daten ohne Brief
' 
' ************************************************
