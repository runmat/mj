Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG

Partial Public Class PeisAdministration
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private mObjPeisAdministration As PeisAdministrationClass

#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property
#End Region

#Region "Methods"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            AdminAuth(Me, m_User, Security.AdminLevel.Master)

            lblError.Text = ""

            If Not IsPostBack Then
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If

                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

                mObjPeisAdministration = New PeisAdministrationClass()
                Session.Add("mObjPeisAdministrationSession", mObjPeisAdministration)
            End If


            If mObjPeisAdministration Is Nothing Then
                If Session("mObjPeisAdministrationSession") Is Nothing Then
                    Throw New Exception("Benötigtes Session Objekt nicht vorhanden")
                Else
                    mObjPeisAdministration = CType(Session("mObjPeisAdministrationSession"), PeisAdministrationClass)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Public Sub chkFilterEnabled_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim tmpGriditem As DataGridItem = CType(CType(sender, CheckBox).Parent.Parent, DataGridItem)
        If CType(sender, CheckBox).Checked Then
            mObjPeisAdministration.changeEnabled(tmpGriditem.Cells(0).Text, "X")
        Else
            mObjPeisAdministration.changeEnabled(tmpGriditem.Cells(0).Text, "")
        End If

    End Sub

#End Region



End Class
