Imports CKG.Base.Kernel.Common.Common
Imports Telerik.Web.UI

Public Class Change10_Mail
    Inherits System.Web.UI.Page

    Private _mUser As Base.Kernel.Security.User
    Private _mTransportbeauftragung As Transportbeauftragung

    Protected WithEvents ucStyles As Portal.PageElements.Styles
    Protected WithEvents ucHeader As Portal.PageElements.Header

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _mUser = GetUser(Me)
        ucHeader.InitUser(_mUser)
        FormAuth(Me, _mUser)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            lblError.Text = String.Empty
            lblSuccess.Text = String.Empty

            If Session("_mTransportbeauftragung") IsNot Nothing Then
                _mTransportbeauftragung = CType(Session("_mTransportbeauftragung"), Transportbeauftragung)
            End If

            If Not IsPostBack Then
                Session("StepMail") = 1
                FillOverview(True)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub Change10_Mail_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Session("_mTransportbeauftragung") = _mTransportbeauftragung
    End Sub

    Private Sub cmdBack_Click(sender As Object, e As System.EventArgs) Handles cmdBack.Click
        If CInt(Session("StepMail")) = 1 Then
            Response.Redirect("Change10.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10'")(0)("AppID").ToString)
        ElseIf CInt(Session("StepMail")) = 2 Then
            SwitchView()
            Session("StepMail") = 1
        End If
    End Sub

    Protected Sub cmdCreate_Click(sender As Object, e As EventArgs) Handles cmdCreate.Click
        DoSubmitFinal()
    End Sub

    Private Sub rgMail_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles rgMail.ItemCommand
        UpdateBestand()

        If e.CommandName = "Details" Then
            _mTransportbeauftragung.strAuftragsnummer = e.CommandArgument.ToString()
            Session("_mTransportbeauftragung") = _mTransportbeauftragung

            FillDetails(True)

            SwitchView()
            Session("StepMail") = 2
        ElseIf e.CommandName = "Versenden" Then
            _mTransportbeauftragung.strAuftragsnummer = e.CommandArgument.ToString()

            Try
                _mTransportbeauftragung.FahrzeugeMailVersenden()

                If _mTransportbeauftragung.ErrorOccured Then
                    lblError.Text = _mTransportbeauftragung.GetFormatedErrorMessage
                Else
                    lblSuccess.Text = "Mailversand erfolgreich!"
                End If
            Catch ex As Exception
                lblError.Text = "Fehler! Daten konnten nicht an SAP übermittelt werden: " + ex.Message
            End Try

            Session("_mTransportbeauftragung") = _mTransportbeauftragung

            FillOverview(True)
        End If
    End Sub

    Private Sub rgMail_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgMail.NeedDataSource
        FillOverview(False)
    End Sub

    Private Sub rgMail_UpdateCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles rgMail.UpdateCommand
        UpdateBestand()
    End Sub

    Private Sub rgDetails_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgDetails.NeedDataSource
        FillDetails(False)
    End Sub


#End Region


#Region "Methoden und Funktionen"

    Private Sub FillOverview(rebind As Boolean)
        _mTransportbeauftragung.FillBestandWaitingForEmail(Session("AppID").ToString, Session.SessionID)

        rgMail.DataSource = _mTransportbeauftragung.tblSAPWaitingForEmailSumme

        If rebind Then
            rgMail.Rebind()
        End If
    End Sub

    Private Sub FillDetails(rebind As Boolean)
        Dim dv = _mTransportbeauftragung.GetFilterBestandMail()
        Dim tbl = dv.ToTable()
        rgDetails.DataSource = tbl

        If rebind Then
            rgDetails.Rebind()
        End If

    End Sub

    Private Sub SwitchView()
        tdOverview.Visible = Not tdOverview.Visible
        tdDetailsGrid.Visible = Not tdDetailsGrid.Visible

        cmdCreate.Visible = Not cmdCreate.Visible
    End Sub

    ''' <summary>
    ''' Aktualisiert den ZuBeauftragen Status der Tabelle tblSAPBestand
    ''' </summary>
    Private Sub UpdateBestand()

        _mTransportbeauftragung.lstAuftragMail.Clear()

        For Each item As GridDataItem In rgMail.SelectedItems
            _mTransportbeauftragung.lstAuftragMail.Add(item("AUF_NEUW_TRANSP").Text)
        Next

        Session("_mTransportbeauftragung") = _mTransportbeauftragung
    End Sub

#Region "Submits"
    
    ''' <summary>
    ''' Sendet Daten an SAP (Letzer Schritt im Workflow)
    ''' </summary>
    Private Function DoSubmitFinal() As Boolean

        Try
            UpdateBestand()

            _mTransportbeauftragung.FahrzeugeMailVersenden()

            If _mTransportbeauftragung.ErrorOccured Then
                lblError.Text = _mTransportbeauftragung.GetFormatedErrorMessage
            Else
                lblSuccess.Text = "Mailversand erfolgreich!"
            End If

            FillOverview(True)

            Return True
        Catch ex As Exception
            lblError.Text = "Fehler! Daten konnten nicht an SAP übermittelt werden: " + ex.Message
            Return False
        End Try

    End Function

#End Region
    

#End Region

End Class