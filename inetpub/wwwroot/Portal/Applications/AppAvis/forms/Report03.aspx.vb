Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

'Report Eingang ZBII mit UseCode00

Partial Public Class Report03
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    '-----------------------------------------------------------------------
    ' Methode: DoSubmit
    ' Autor: J.Jung
    ' Beschreibung: Aufruf der Methode EingangZBIIUseCode00.Fill 
    ' Erstellt am: 31.10.2008
    ' ITA: 2330
    '-----------------------------------------------------------------------
    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim mObjEingangZBIIUseCode00 As New EingangZBIIUseCode00(m_User, m_App, strFileName)

            mObjEingangZBIIUseCode00.Fill(Session("AppID").ToString, Session.SessionID.ToString)


            If Not mObjEingangZBIIUseCode00.Status = 0 Then
                lblError.Text = mObjEingangZBIIUseCode00.Message
            Else
                Session("ResultTable") = mObjEingangZBIIUseCode00.Result
                Dim objExcelExport As New Excel.ExcelExport()
                Try
                    Excel.ExcelExport.WriteExcel(mObjEingangZBIIUseCode00.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch
                End Try
                Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Abgemeldete Fahrzeuge")
                Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
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

    Protected Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub
End Class

' ************************************************
' $History: Report03.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.11.08   Time: 9:35
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2421 testfertig
'
' ************************************************