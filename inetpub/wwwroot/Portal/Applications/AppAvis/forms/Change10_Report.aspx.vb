Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.DocumentGeneration
Imports Telerik.Web.UI

Public Class Change10_Report
    Inherits System.Web.UI.Page

    Private _mUser As Base.Kernel.Security.User
    Private _mTransportbeauftragung As Transportbeauftragung

    Protected WithEvents ucStyles As Portal.PageElements.Styles
    Protected WithEvents ucHeader As Portal.PageElements.Header

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _mUser = GetUser(Me)
        ucHeader.InitUser(_mUser)
        FormAuth(Me, _mUser)

        GetAppIDFromQueryString(Me)

        lblError.Text = ""

        Try
            lblHead.Text = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
           
            If Session("_mTransportbeauftragung") IsNot Nothing Then
                _mTransportbeauftragung = CType(Session("_mTransportbeauftragung"), Transportbeauftragung)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub cmdBack_Click(sender As Object, e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("Change10.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10'")(0)("AppID").ToString)
    End Sub

    Private Sub cmdCreate_Click(sender As Object, e As System.EventArgs) Handles cmdCreate.Click
        FillGrid(True)
    End Sub

    Private Sub rgResult_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgResult.NeedDataSource
        FillGrid(False)
    End Sub

    Protected Sub rgResult_OnPageSizeChanged(sender As Object, e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles rgResult.PageSizeChanged

    End Sub

    Protected Sub rgResult_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles rgResult.ItemDataBound
        ' Pager neu befüllen
        If TypeOf e.Item Is GridPagerItem Then

            Dim PageSizeCombo As RadComboBox = CType(e.Item.FindControl("PageSizeComboBox"), RadComboBox)

            PageSizeCombo.Items.Clear()
            'PageSizeCombo.Items.Add(New RadComboBoxItem("10"))
            'PageSizeCombo.FindItemByText("10").Attributes.Add("ownerTableViewId", rgResult.MasterTableView.ClientID)
            'PageSizeCombo.Items.Add(New RadComboBoxItem("15"))
            'PageSizeCombo.FindItemByText("15").Attributes.Add("ownerTableViewId", rgResult.MasterTableView.ClientID)
            PageSizeCombo.Items.Add(New RadComboBoxItem("20"))
            PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", rgResult.MasterTableView.ClientID)
            PageSizeCombo.Items.Add(New RadComboBoxItem("50"))
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", rgResult.MasterTableView.ClientID)
            PageSizeCombo.Items.Add(New RadComboBoxItem("100"))
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", rgResult.MasterTableView.ClientID)
            PageSizeCombo.Items.Add(New RadComboBoxItem("200"))
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", rgResult.MasterTableView.ClientID)
            'PageSizeCombo.Items.Add(New RadComboBoxItem("500"))
            'PageSizeCombo.FindItemByText("500").Attributes.Add("ownerTableViewId", rgResult.MasterTableView.ClientID)
            'PageSizeCombo.Items.Add(New RadComboBoxItem("5000"))
            'PageSizeCombo.FindItemByText("5000").Attributes.Add("ownerTableViewId", rgResult.MasterTableView.ClientID)
            If PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()) Is Nothing Then
                PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()).Selected = True
            End If

        ElseIf TypeOf e.Item Is GridDataItem Then

            'Zeilenfarben je nach Verwendungszweck setzen
            Dim item As GridDataItem = CType(e.Item, GridDataItem)
            Select Case item("VERWENDUNGSZWECK").Text
                Case "LIZZI"
                    item.BackColor = Drawing.Color.PowderBlue
                Case "DW"
                    item.BackColor = Drawing.Color.LightGreen
                Case "LF"
                    item.BackColor = Drawing.Color.LightCoral
                Case "IT"
                    item.BackColor = Drawing.Color.NavajoWhite
                Case "EFS"
                    item.BackColor = Drawing.Color.LightGoldenrodYellow
                Case "VAN"
                    item.BackColor = Drawing.Color.LightPink
            End Select
        End If
    End Sub

    Private Sub FillGrid(rebind As Boolean)
        If txtDatZulVon.SelectedDate Is Nothing Or txtDatZulBis.SelectedDate Is Nothing Then
            lblError.Text = "Bitte geben Sie ein Von-/Bis-Datum ein!"
            Exit Sub
        End If

        _mTransportbeauftragung.DatumZulassungVon = txtDatZulVon.SelectedDate
        _mTransportbeauftragung.DatumZulassungBis = txtDatZulBis.SelectedDate

        _mTransportbeauftragung.FillReportBeauftragung(Session("AppID").ToString, Session.SessionID, rtbCarport.Text, rtbDistrikt.Text)

        rgResult.DataSource = _mTransportbeauftragung.tblSAPBestand

        If rebind Then
            rgResult.Rebind()
        End If

        _mTransportbeauftragung.FillBestand(Session("AppID").ToString, Session.SessionID)
        rgAuslastung.DataSource = _mTransportbeauftragung.tblAuslastungWork

        If rebind Then
            rgAuslastung.Rebind()
        End If

        Session("_mTransportbeauftragung") = _mTransportbeauftragung
    End Sub

    Protected Sub ibtnExcelExport_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtnExcelExport.Click
        Dim ExcelFac As New ExcelDocumentFactory()

        ExcelFac.CreateDocumentAndSendAsResponse("Export", _mTransportbeauftragung.tblAuslastungWork, Me)
    End Sub

End Class