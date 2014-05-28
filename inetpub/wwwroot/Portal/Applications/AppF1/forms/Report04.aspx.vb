Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report04
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App


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
                Dim m_Report As New F1_Bank_Haendlerfinanzierungen(m_User, m_App, RadioButtonList1.SelectedValue, strFileName)

                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result Is Nothing OrElse m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else
                        Session("ResultTable") = m_Report.Result
                        Session("App_FzgTyp") = RadioButtonList1.SelectedItem.Text




                        Dim objExcelExport As New Excel.ExcelExport()
                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try


                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        Response.Redirect("Report04_1.aspx?AppID=" & Session("AppID").ToString)
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
' $History: Report04.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 30.03.09   Time: 10:52
' Updated in $/CKAG/Applications/AppF1/forms
' 2685 Nachbesserungen
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.03.09   Time: 14:24
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2685 unfertig
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' Ausblenden Hndler Kontingente
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 10.06.08   Time: 17:24
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.05.08   Time: 17:28
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA 1881
' 
' ************************************************
