Imports CKG.Base.Kernel.Common.Common
Imports Telerik.Web.UI

Public Class Change10_Korrektur_2
    Inherits System.Web.UI.Page

    Private _mUser As Base.Kernel.Security.User
    Private _mTransportbeauftragung As Transportbeauftragung

    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header
    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles

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
                FillGrid(True)
                FillGridAuslastung(True)
                FillSpediteure()

                ' Carport-Information für den Header retten
                If _mTransportbeauftragung.tblSAPBestand.Rows.Count > 0 Then
                    _mTransportbeauftragung.strFilterCarport = _mTransportbeauftragung.tblSAPBestand.Rows(0)("CARPORT").ToString()
                End If

                rtbStationscode.Focus()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub Change10_Korrektur_2_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        Session("_mTransportbeauftragung") = _mTransportbeauftragung
    End Sub

    Private Sub cmdBack_Click(sender As Object, e As System.EventArgs) Handles cmdBack.Click
        If CInt(Session("StepKorrektur")) = 2 Then
            Session("StepKorrektur") = 1
            Response.Redirect("Change10.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10_Beauftragung'")(0)("AppID").ToString)
        ElseIf CInt(Session("StepKorrektur")) = 3 Then
            StepKorrekturBack()
        End If
    End Sub

    Private Sub cmdCreate_Click(sender As Object, e As System.EventArgs) Handles cmdCreate.Click
        If DoSubmit() Then
            Session("StepKorrektur") = 3
            SwitchButtons()

            rgResult.Enabled = False

            rgResult.DataSource = _mTransportbeauftragung.tblSAPBestand.Select("ZuBeauftragen=true")
            rgResult.Rebind()
        End If
    End Sub

    Private Sub cmdSend_Click(sender As Object, e As System.EventArgs) Handles cmdSend.Click
        If CInt(Session("StepKorrektur")) = 3 Then
            If DoSubmitFinal() Then
                _mTransportbeauftragung.FillBestand(Session("AppID").ToString, Session.SessionID)

                FillGridAuslastung(True)
               
                StepKorrekturBack()
            End If
        End If
    End Sub

    Private Sub cmdStorno_Click(sender As Object, e As System.EventArgs) Handles cmdStorno.Click
        If CInt(Session("StepKorrektur")) = 3 Then
            If DoStornoFinal() Then
                _mTransportbeauftragung.FillBestand(Session("AppID").ToString, Session.SessionID)

                FillGridAuslastung(True)
               
                StepKorrekturBack()
            End If
        End If
    End Sub

    Protected Sub rtbStationscode_TextChanged(sender As Object, e As EventArgs) Handles rtbStationscode.TextChanged
        rtbStationscode.Text = rtbStationscode.Text.ToUpper()
        _mTransportbeauftragung.FillStationsDaten(rtbStationscode.Text)

        If _mTransportbeauftragung.ErrorOccured Then
            lblError.Text = _mTransportbeauftragung.ErrorMessage
        Else
            If _mTransportbeauftragung.tblStation.Rows.Count > 0 Then
                rtbStation.Text = CStr(_mTransportbeauftragung.tblStation.Rows(0)("NAME1"))
                rtbStation2.Text = ""
                rtbPlz.Text = CStr(_mTransportbeauftragung.tblStation.Rows(0)("POST_CODE1"))
                rtbOrt.Text = CStr(_mTransportbeauftragung.tblStation.Rows(0)("CITY1"))
                rtbStraße.Text = CStr(_mTransportbeauftragung.tblStation.Rows(0)("STREET"))
                rtbHausnummer.Text = CStr(_mTransportbeauftragung.tblStation.Rows(0)("HOUSE_NUM1"))
            Else
                lblError.Text = "Keine Daten zum Stationscode gefunden. Bitte prüfen!"
            End If
        End If

        UpdateBestand()
    End Sub

    Private Sub rdpTermin_SelectedDateChanged(sender As Object, e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles rdpTermin.SelectedDateChanged
        UpdateBestand()
    End Sub

#Region "rgResultEvents"

    Protected Sub rgResult_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgResult.ItemCommand
        UpdateBestand()
    End Sub

    Protected Sub rgResult_UpdateCommand(sender As Object, e As GridCommandEventArgs) Handles rgResult.UpdateCommand
        UpdateBestand()
    End Sub

    Private Sub rgResult_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles rgResult.NeedDataSource
        FillGrid(False)
    End Sub

    Protected Sub rgResult_PreRender(sender As Object, e As System.EventArgs) Handles rgResult.PreRender
        SelectGridRows()
    End Sub

    Public Sub rgResult_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles rgResult.ItemDataBound

        If TypeOf e.Item Is GridPagerItem Then

            ' Pager neu befüllen
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

#End Region

#Region "rgAuslastungEvents"

    Protected Sub rgAuslastung_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles rgAuslastung.NeedDataSource
        FillGridAuslastung(False)
    End Sub

#End Region


#End Region


#Region "Methoden und Funktionen"

#Region "Selektionen"

    ''' <summary>
    ''' Selektiert die im zu beuftragenden Fahrzeuge im RadGrid
    ''' </summary>
    Private Sub SelectGridRows()
        For Each row As DataRow In _mTransportbeauftragung.tblSAPBestand.Rows
            If CBool(row("ZuBeauftragen")) Then
                For Each dataItem As GridDataItem In rgResult.Items
                    If CInt(dataItem("RowID").Text) = CInt(row("RowID")) Then
                        Dim cb As CheckBox = CType(dataItem.FindControl("ClientSelectColumnSelectCheckBox"), CheckBox)
                        cb.Checked = True
                        dataItem.Selected = True
                    End If
                Next
            End If
        Next
    End Sub

 #End Region

#Region "Ansichtsänderungen"

    ''' <summary>
    ''' Setzt die Seite auf den Zustand von StepKorrektur 1 zurück
    ''' </summary>
    Private Sub StepKorrekturBack()
        Session("StepKorrektur") = 2

        SwitchButtons()

        rgResult.Enabled = True
        rgResult.ClientSettings.Selecting.AllowRowSelect = True
        FillGrid(True)
    End Sub

    ''' <summary>
    ''' Tauscht die Sichtbarkeit der Aktion-Buttons
    ''' </summary>
    Private Sub SwitchButtons()
        cmdCreate.Visible = Not cmdCreate.Visible
        cmdSend.Visible = Not cmdSend.Visible
        cmdStorno.Visible = Not cmdStorno.Visible
    End Sub
    
#End Region

#Region "Fill Methoden"

    Private Sub FillGrid(rebind As Boolean)
        rgResult.DataSource = _mTransportbeauftragung.FillBestandStornoAenderung(Session("AppID").ToString, Session.SessionID)

        If rebind Then
            rgResult.Rebind()
        End If

    End Sub

    Private Sub FillGridAuslastung(rebind As Boolean)
        rgAuslastung.DataSource = _mTransportbeauftragung.tblAuslastungWork

        If rebind Then
            rgAuslastung.Rebind()
        End If
    End Sub

    Private Sub FillSpediteure()
        Dim tblSpedi As DataTable = _mTransportbeauftragung.tblSpediteure

        tblSpedi.Columns.Add("NameKombi")

        For Each row As DataRow In tblSpedi.Rows
            row("NameKombi") = row("KUNPDI").ToString() + " - " + row("NAME1").ToString()
        Next

        rcbSpediteur.DataSource = tblSpedi
        rcbSpediteur.DataValueField = "KUNPDI"
        rcbSpediteur.DataTextField = "NameKombi"
        rcbSpediteur.DataBind()

        rcbSpediteur.SelectedValue = _mTransportbeauftragung.strSpediteur
    End Sub

#End Region

#Region "Submits"

    ''' <summary>
    ''' Überträgt die geänderten Daten in die Buisinesslogik-Klasse
    ''' </summary>
    Private Function DoSubmit() As Boolean
        Try
            UpdateBestand()

            Session("_mTransportbeauftragung") = _mTransportbeauftragung
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Sendet geänderte Daten an SAP (Letzer Schritt im Workflow)
    ''' </summary>
    Private Function DoSubmitFinal() As Boolean
        If ValidateInputFields() Then
            Try
                _mTransportbeauftragung.strSpediteur = rcbSpediteur.SelectedValue
                _mTransportbeauftragung.FilltblWebHead(rtbStationscode.Text, rtbStation.Text, rtbStation2.Text, rtbStraße.Text,
                                                       rtbHausnummer.Text, rtbPlz.Text, rtbOrt.Text, "", rdpTermin.SelectedDate,
                                                       rtpUhrzeit.SelectedDate, _mUser.UserName, _mUser.Email)

                _mTransportbeauftragung.FahrzeugeAendern()

                If _mTransportbeauftragung.ErrorOccured Then
                    lblError.Text = _mTransportbeauftragung.GetFormatedErrorMessage
                Else
                    lblSuccess.Text = "Änderung erfolgreich!"
                End If

                Return True
            Catch ex As Exception
                lblError.Text = "Fehler! Daten konnten nicht an SAP übermittelt werden: " + ex.Message
                Return False
            End Try
        End If

        Return False

    End Function

    ''' <summary>
    ''' Sendet Daten an SAP (Letzer Schritt im Workflow)
    ''' </summary>
    Private Function DoStornoFinal() As Boolean

        Try
            _mTransportbeauftragung.strSpediteur = rcbSpediteur.SelectedValue
            _mTransportbeauftragung.FilltblWebHead(rtbStationscode.Text, rtbStation.Text, rtbStation2.Text, rtbStraße.Text, rtbHausnummer.Text,
                                                   rtbPlz.Text, rtbOrt.Text, "", rdpTermin.SelectedDate, rtpUhrzeit.SelectedDate,
                                                   _mUser.UserName, _mUser.Email)

            _mTransportbeauftragung.FahrzeugeStornieren()

            If _mTransportbeauftragung.ErrorOccured Then
                lblError.Text = _mTransportbeauftragung.GetFormatedErrorMessage
            Else
                lblSuccess.Text = "Stornierung erfolgreich!"
            End If

            Return True
        Catch ex As Exception
            lblError.Text = "Fehler! Daten konnten nicht an SAP übermittelt werden: " + ex.Message
            Return False
        End Try

    End Function

#End Region

    ''' <summary>
    ''' Prüft die Pflichteingabefelder
    ''' </summary>
    ''' <returns>True = OK</returns>
    Private Function ValidateInputFields() As Boolean
        If rdpTermin.SelectedDate Is Nothing Then
            lblError.Text = "Wählen Sie ein gültiges Datum!"
        ElseIf rdpTermin.SelectedDate < Today Then
            lblError.Text = "Das Datum darf nicht in der Vergangenheit liegen!"
            Return False
        End If

        If rtpUhrzeit.SelectedDate Is Nothing Then
            lblError.Text = "Wählen Sie eine gültige Zeit!"
            Return False
        End If

        If rcbSpediteur.SelectedIndex = -1 Then
            lblError.Text = "Geben Sie einen Spediteur an!"
            Return False
        End If

        If rtbStation.Text.Trim = String.Empty Then
            lblError.Text = "Geben Sie einen Stationnamen an!"
            Return False
        End If

        If rtbStraße.Text.Trim = String.Empty Then
            lblError.Text = "Geben Sie eine Straße an!"
            Return False
        End If

        If rtbPlz.Text.Trim = String.Empty Then
            lblError.Text = "Geben Sie eine Postleitzahl an!"
            Return False
        End If

        If rtbOrt.Text.Trim = String.Empty Then
            lblError.Text = "Geben Sie einen Ort an!"
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' Aktualisiert den ZuBeauftragen Status der Tabelle tblSAPBestand
    ''' </summary>
    Private Sub UpdateBestand()
        For Each gridRow As GridDataItem In rgResult.Items
            Dim i As Integer = CInt(gridRow("RowID").Text)

            Dim bfound As Boolean = False

            For Each item As GridDataItem In rgResult.SelectedItems
                If CInt(item("RowID").Text) = i Then
                    bfound = True
                    Exit For
                End If
            Next

            If bfound Then
                _mTransportbeauftragung.tblSAPBestand.Rows(i)("ZuBeauftragen") = True
            Else
                _mTransportbeauftragung.tblSAPBestand.Rows(i)("ZuBeauftragen") = False
            End If
        Next

        UpdateAuslastung()
        FillGridAuslastung(True)

        Session("_mTransportbeauftragung") = _mTransportbeauftragung
    End Sub

    ''' <summary>
    ''' Aktualisiert die Auslastungtabelle
    ''' </summary>
    Private Sub UpdateAuslastung()

        Dim auslastDate As String = "Kein Datum"
        If rdpTermin.SelectedDate IsNot Nothing Then
            auslastDate = rdpTermin.SelectedDate.Value.ToShortDateString
        End If

        Dim strStation As String = rtbStationscode.Text

        _mTransportbeauftragung.ResetTblAuslastungWork()

        If auslastDate = "Kein Datum" Then
            If Not _mTransportbeauftragung.tblAuslastungWork.Columns.Contains(auslastDate) Then
                _mTransportbeauftragung.tblAuslastungWork.Columns.Add(auslastDate)
            End If
        Else
            If Not _mTransportbeauftragung.tblAuslastungWork.Columns.Contains(auslastDate & " " & _mTransportbeauftragung.strDispo) Then
                _mTransportbeauftragung.tblAuslastungWork.Columns.Add(auslastDate + " " + _mTransportbeauftragung.strDispo)
                _mTransportbeauftragung.tblAuslastungWork.Columns.Add(auslastDate + " " + _mTransportbeauftragung.strBeauftragt)
            End If

        End If

        _mTransportbeauftragung.tblAuslastungWork.AcceptChanges()

        Transportbeauftragung.ColumnReorder(_mTransportbeauftragung.tblAuslastungWork)

        If auslastDate = "Kein Datum" Then
            For Each row As DataRow In _mTransportbeauftragung.tblAuslastungWork.Rows
                row(auslastDate) = 0
            Next
        Else
            For Each row As DataRow In _mTransportbeauftragung.tblAuslastungWork.Rows
                row(auslastDate + " " + _mTransportbeauftragung.strDispo) = 0
                row(auslastDate + " " + _mTransportbeauftragung.strBeauftragt) = 0
            Next
        End If

        Dim rows As DataRow() = _mTransportbeauftragung.tblAuslastungWork.Select("STATION ='" & strStation & "'")
        If rows.Length = 0 Then
            Dim nrow As DataRow = _mTransportbeauftragung.tblAuslastungWork.NewRow()
            For Each column As DataColumn In _mTransportbeauftragung.tblAuslastungWork.Columns
                Select Case column.ColumnName
                    Case "STATION"
                        nrow("STATION") = strStation
                    Case Else
                        nrow(column) = 0
                End Select
            Next

            _mTransportbeauftragung.tblAuslastungWork.Rows.Add(nrow)
            rows = _mTransportbeauftragung.tblAuslastungWork.Select("STATION ='" & strStation & "'")
        End If

        Dim rowsBeauftragt As DataRow() = _mTransportbeauftragung.tblSAPBestand.Select("ZuBeauftragen='True'")
        For Each rowBeauf As DataRow In rowsBeauftragt
            If rows.Length > 0 Then
                If auslastDate = "Kein Datum" Then
                    rows(0)(auslastDate) = CInt(rows(0)(auslastDate)) + 1
                Else
                    rows(0)(auslastDate + " " + _mTransportbeauftragung.strDispo) = CInt(rows(0)(auslastDate + " " + _mTransportbeauftragung.strDispo)) + 1
                End If

            End If
        Next

        _mTransportbeauftragung.tblAuslastungWork.AcceptChanges()
    End Sub

#End Region

End Class