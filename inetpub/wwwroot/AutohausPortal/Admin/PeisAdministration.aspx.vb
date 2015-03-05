Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class PeisAdministration
    Inherits Page

    Private m_App As Security.App
    Private m_User As Security.User
    Private mObjPeisAdministration As PeisAdministrationClass

#Region "Methods"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Security.App(m_User) 'erzeugt ein App_objekt 
            AdminAuth(Me, m_User, Security.AdminLevel.Master)
            lblError.Text = ""

            If Not IsPostBack Then

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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Public Sub chkFilterEnabled_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim tmpGriditem As DataGridItem = CType(CType(sender, CheckBox).Parent.Parent, DataGridItem)
        If CType(sender, CheckBox).Checked Then
            mObjPeisAdministration.changeEnabled(tmpGriditem.Cells(0).Text, "X")
        Else
            mObjPeisAdministration.changeEnabled(tmpGriditem.Cells(0).Text, "")
        End If
    End Sub

#End Region



End Class
