Imports CKG
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change03
    Inherits System.Web.UI.Page


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Private mObjFahrerAuftraege As FahrerAuftraege


#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblError.Text = ""

        If Not IsPostBack Then
            If Refferer Is Nothing Then
                If Not Request.UrlReferrer Is Nothing Then
                    Refferer = Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If
            End If

            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
        End If


        If mObjFahrerAuftraege Is Nothing Then
            If Not Session("mObjFahrerAuftraegeSession") Is Nothing Then
                mObjFahrerAuftraege = CType(Session("mObjFahrerAuftraegeSession"), FahrerAuftraege)
            Else
                mObjFahrerAuftraege = New FahrerAuftraege(m_User, m_App, "")
                Session.Add("mObjFahrerAuftraegeSession", mObjFahrerAuftraege)
            End If
        End If
    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        With mObjFahrerAuftraege
            'füllen der Importparameter und ab
            .Auswahl = rblAuswahl.SelectedValue
            .Fill(Session("AppID").ToString, Me.Session.SessionID)
            If Not .Status = 0 Then
                lblError.Text = mObjFahrerAuftraege.Message
            Else
                If .Auftraege.Rows.Count > 0 Then
                    Response.Redirect("Change03_1.aspx?AppID=" & Session("AppID").ToString)
                Else
                    lblError.Text = "keine Aufträge vorhanden"
                End If

            End If
        End With
    End Sub

End Class

' ************************************************
' $History: Change03.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 2.07.09    Time: 15:24
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA 2641 Testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 1.07.09    Time: 16:45
' Created in $/CKAG/Applications/AppKroschke/Forms
' ITA 2641 unfertig
' 
' ************************************************
