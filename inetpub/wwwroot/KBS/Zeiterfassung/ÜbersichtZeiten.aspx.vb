Imports System
Imports KBS.KBS_BASE
Imports TimeRegistration

Public Class ÜbersichtZeiten
    Inherits Page

    Private mObjKasse As Kasse
    Private TimeReg As TimeRegistrator

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)

        If Not Session("mKasse") Is Nothing Then
            mObjKasse = Session("mKasse")
        End If

        If Not Session("TimeReg") Is Nothing Then
            TimeReg = Session("TimeReg")
        Else
            If (Not Session("TimeRegUser") Is Nothing) Then
                Dim tUser As TimeRegUser = Session("TimeRegUser")
                Session("TimeReg") = New TimeRegistrator(tUser, mObjKasse.Lagerort)
                TimeReg = Session("TimeReg")
            Else
                Response.Redirect("Zeiterfassung.aspx")
            End If
        End If

        If Not IsPostBack Then
            lblUsername.Text = TimeReg.User.Username
            FillList()
        End If

        lblError.Text = ""
        Title = lblHead.Text

        Session("LastPage") = Me
    End Sub

    Private Sub FillList()
        Dim tov As TimeOverview = TimeReg.getZeitübersicht(Me)
        Dim dt As DataTable = tov.getWebTabelleForBediener(TimeReg.User.Kartennummer)

        'Druckvorbereitung
        Try
            Session("PDFPrintObj") = New PDFPrintObj(TimeReg.SAPConnectionString, TimeReg.User.Kartennummer, tov.Von, tov.Bis)
        Catch ex As Exception
            Session("PDFPrintObj") = Nothing
        End Try

        GridWoche.DataSource = dt
        GridWoche.DataBind()
    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("Zeiterfassung.aspx")
    End Sub

End Class