Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change08
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objFahrzeuge As MindesthaltefristIngnorieren

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub


    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        lblError.Text = ""
        lblError.Visible = False

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        objFahrzeuge = New MindesthaltefristIngnorieren(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
        objFahrzeuge.Customer = m_User.KUNNR
        objFahrzeuge.Show(Session("AppID").ToString, Session.SessionID, Me)
        If Not objFahrzeuge.Status = 0 Then
            lblError.Text = objFahrzeuge.Message
            lblError.Visible = True
        Else
            If objFahrzeuge.Fahrzeuge.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            Else
                Dim tblTemp As DataTable
                tblTemp = objFahrzeuge.Fahrzeuge.Copy
                tblTemp.Columns.Remove("ActionDELE")
                tblTemp.Columns.Remove("ActionNOTHING")
                tblTemp.Columns.Remove("Action")
                tblTemp.Columns.Remove("Bemerkung")
                tblTemp.Columns.Remove("Equipmentnummer")
                Session.Add("ExcelResult", tblTemp)

                Session("objFahrzeuge") = objFahrzeuge
                Response.Redirect("Change08_1.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub
End Class

' ************************************************
' $History: Change08.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.03.09   Time: 8:40
' Updated in $/CKAG/Applications/AppAvis/forms
' nachbesserungen
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 10.03.09   Time: 13:34
' Created in $/CKAG/Applications/AppAvis/forms
' ITA 2657 testfertig
' 
' ************************************************
