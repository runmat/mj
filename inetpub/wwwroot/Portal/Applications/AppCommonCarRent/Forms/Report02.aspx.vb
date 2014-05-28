Option Explicit On
Option Strict On
Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Report02
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private mObjAbgemeldeteFahrzeuge As AbgemeldeteFahrzeuge

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""

            If Not IsPostBack Then
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If
                GetAppIDFromQueryString(Me)
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

            If mObjAbgemeldeteFahrzeuge Is Nothing Then
                If Session("mObjAbgemeldeteFahrzeugeSession") Is Nothing Then
                    mObjAbgemeldeteFahrzeuge = New AbgemeldeteFahrzeuge(m_User, m_App, "")
                    Session.Add("mObjAbgemeldeteFahrzeugeSession", mObjAbgemeldeteFahrzeuge)
                Else
                    mObjAbgemeldeteFahrzeuge = CType(Session("mObjAbgemeldeteFahrzeugeSession"), AbgemeldeteFahrzeuge)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Protected Sub lb_weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_weiter.Click
        Dim errorText As String = ""

        If Not HelpProcedures.checkDate(txtDatumVon, txtDatumBis, errorText, False, 1) Then
            lblError.Text = errorText
            Exit Sub
        End If
        mObjAbgemeldeteFahrzeuge.DatumBis = txtDatumBis.Text
        mObjAbgemeldeteFahrzeuge.DatumVon = txtDatumVon.Text

        mObjAbgemeldeteFahrzeuge.Fill(Session("AppID").ToString, Session.SessionID, Me)

        If mObjAbgemeldeteFahrzeuge.Result.Rows.Count = 0 Then
            lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
        Else
            Session("ResultTable") = mObjAbgemeldeteFahrzeuge.Result

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & "AbgemeldeteFahrzeuge.xls"

            Try
                Excel.ExcelExport.WriteExcel(mObjAbgemeldeteFahrzeuge.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
            Catch
            End Try

            Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
            Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Sub lb_zurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub
End Class

' ************************************************
' $History: Report02.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' Warnungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.02.09   Time: 13:51
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2596 / 2589
' 
' ************************************************