Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change04
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Protected WithEvents ucStyles As Global.CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As Global.CKG.Portal.PageElements.Header

    Private clsBlocken As AvisChange04

    Private Enum Blocken
        Anlegen = 1
        FreigebenBeauftragen = 2
        Bearbeiten = 3
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        If clsBlocken Is Nothing = True Then
            clsBlocken = New AvisChange04(m_User, m_App, "")
        End If

        clsBlocken.Aktion = CInt(Me.rbAktion.SelectedItem.Value)

        Session("clsBlocken") = clsBlocken

        Select Case clsBlocken.Aktion
            Case Blocken.Anlegen
                Response.Redirect("Change04_2.aspx?AppID=" & Session("AppID").ToString)
            Case Blocken.FreigebenBeauftragen
                Response.Redirect("Change04_3.aspx?AppID=" & Session("AppID").ToString)
            Case Blocken.Bearbeiten
                Response.Redirect("Change04_4.aspx?AppID=" & Session("AppID").ToString)
        End Select
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change04.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 25.11.08   Time: 15:20
' Updated in $/CKAG/Applications/AppAvis/forms
'