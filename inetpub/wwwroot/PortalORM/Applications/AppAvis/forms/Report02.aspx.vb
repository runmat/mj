Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

'Report Eingang ZBII ohne Vorlage von Grunddaten
'Report Eingang ZBII ohne Eingang von Carport
Partial Public Class Report02
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private UrlParam As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        Try

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            UrlParam = "Grunddaten"
            If (Request.QueryString("Appl")) = "Carport" Then
                UrlParam = "Carport"
                tr_Date.Visible = True
            End If


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
    ' Autor: O.Rudolph
    ' Beschreibung: Aufruf der Methode Avis01.Fill_ZBIIVorlage 
    ' Erstellt am: 27.10.2008
    ' ITA: 2311
    '-----------------------------------------------------------------------
    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Avis01(m_User, m_App, strFileName)

            If UrlParam = "Carport" Then
                If Not (txtAbmeldedatumBis.Text = String.Empty) Then
                    If Not IsDate(txtAbmeldedatumBis.Text) Then
                        If Not IsStandardDate(txtAbmeldedatumBis.Text) Then
                            If Not IsSAPDate(txtAbmeldedatumBis.Text) Then
                                lblError.Text &= "Geben Sie bitte ein gültiges ""Datum bis"" ein!<br>"
                                Exit Sub
                            Else
                                m_Report.EingZBIIbis = txtAbmeldedatumBis.Text
                            End If
                        Else
                            m_Report.EingZBIIbis = txtAbmeldedatumBis.Text
                        End If
                    Else
                        m_Report.EingZBIIbis = txtAbmeldedatumBis.Text
                    End If
                End If
                m_Report.Fill_ZBIICarport(Session("AppID").ToString, Session.SessionID.ToString)
            Else
                m_Report.Fill_ZBIIVorlage(Session("AppID").ToString, Session.SessionID.ToString)
            End If


            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                Dim objExcelExport As New Excel.ExcelExport()
                Try
                    Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch
                End Try
                Session("lnkExcel") = "/PortalORM/Temp/Excel/" & strFileName
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Abgemeldete Fahrzeuge")
                Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
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
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Protected Sub btnCal2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCal2.Click
        calBis.Visible = True
    End Sub

    Protected Sub calBis_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calBis.SelectionChanged
        txtAbmeldedatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub
End Class
' ************************************************
' $History: Report02.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 1.12.08    Time: 16:13
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2359 & kleinere Anpassungen
' 
' ************************************************
