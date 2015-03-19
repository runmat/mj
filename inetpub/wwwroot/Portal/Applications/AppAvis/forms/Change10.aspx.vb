Imports CKG.Base.Kernel.Common.Common


Public Class Change10
    Inherits Page
    Private _mUser As Base.Kernel.Security.User
    Private _mApp As Base.Kernel.Security.App

    Protected WithEvents UcStyles As Portal.PageElements.Styles
    Protected WithEvents UcHeader As Portal.PageElements.Header

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _mUser = GetUser(Me)
        ucHeader.InitUser(_mUser)
        FormAuth(Me, _mUser)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            UcStyles.TitleText = lblHead.Text
            _mApp = New Base.Kernel.Security.App(_mUser)

            ' Links mit AppID versehen
            btnTransportbeauftragung.PostBackUrl = "Change10_Beauftragung.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10_Beauftragung'")(0)("AppID").ToString
            btnTransportbeauftragungUpload.PostBackUrl = "Change10_BeauftragungUpload.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10_BeauftragungUpload'")(0)("AppID").ToString
            btnKorrekturStorno.PostBackUrl = "Change10_Korrektur.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10_Korrektur'")(0)("AppID").ToString
            btnReport.PostBackUrl = "Change10_Report.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10_Report'")(0)("AppID").ToString
            btnMailBeauftragung.PostBackUrl = "Change10_Mail.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10_Mail'")(0)("AppID").ToString
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try

        If Not IsPostBack Then
            Session("_mTransportbeauftragung") = New Transportbeauftragung(_mApp, _mUser)
            Session("Step") = 1
        End If
    End Sub

    ''' <summary>
    ''' Weiterleitung auf die Korrektur
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnKorrekturStorno_Click(sender As Object, e As EventArgs) Handles btnKorrekturStorno.Click
        Session("StepKorrektur") = 1
    End Sub

    ''' <summary>
    ''' Weiterleitung auf das Reporting
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click

    End Sub
End Class