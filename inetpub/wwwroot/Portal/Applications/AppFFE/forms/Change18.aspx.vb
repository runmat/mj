Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Change18
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        NoDealer(Me, m_User)
        suche1.NurFilialSuche = True
        suche1.AllowEmptySearch = True
        suche1.RedirectUrl = "Change18_2.aspx"
        suche1.User = m_User
        Try
            If Request.QueryString("AppID").Length > 0 Then
                Session("AppID") = Request.QueryString("AppID").ToString
            End If

            If Not IsPostBack Then
                If (Not Session("Authorization") Is Nothing) AndAlso (CBool(Session("Authorization"))) Then
                    If (Not Session("AuthorizationID") Is Nothing) AndAlso CInt(Session("AuthorizationID")) > 0 Then
                        Dim strLastRecordParam As String = ""
                        If CStr(Request.QueryString("LastRecord")) = "True" Then
                            strLastRecordParam = "?LastRecord=True"
                        End If
                        Response.Redirect("Change02Aut.aspx" & strLastRecordParam)
                    End If
                End If
            End If

            Session("SelectedDealer") = Nothing

            suche1.Headline = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)
            ucStyles.TitleText = suche1.Headline

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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
' $History: Change18.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
