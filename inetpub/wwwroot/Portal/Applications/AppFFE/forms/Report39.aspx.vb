Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report39 ' Abgleich Wholesale Retail
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                Me.txtDate.Text = Date.Today.ToShortDateString()
                If Request.Params("art") = "HISTORIE" Then
                    Me.lblDate.Visible = True
                    Me.txtDate.Visible = True
                    Me.btnSelectDate.Visible = True
                Else
                    'Report gleich auslösen
                    cmdCreate_Click(sender, e)
                End If
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

            If txtDate.Text.Trim() = String.Empty Then
                lblError.Text = "Bitte geben Sie zunächst einen Datum an."
            End If

            If lblError.Text.Length = 0 Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

                Dim baseDate As Date
                Try
                    baseDate = Date.Parse(txtDate.Text)
                Catch ex As Exception
                    Throw New ArgumentException("Bitte geben Sie ein gültiges Datum ein.")
                End Try
                Dim m_Report As New FFE_AbgleichRetail(m_User, m_App, strFileName)

                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Date.Parse(txtDate.Text))

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else

                        Dim objExcelExport As New Excel.ExcelExport()
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
' *****************  Version 2  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
