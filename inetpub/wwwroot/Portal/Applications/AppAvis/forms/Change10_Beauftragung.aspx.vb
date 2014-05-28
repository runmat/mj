Imports CKG.Base.Kernel.Common.Common
Imports Telerik.Web.UI

Partial Class Change10_Beauftragung
    Inherits System.Web.UI.Page

    Private _mUser As Base.Kernel.Security.User
    'Private _mApp As Base.Kernel.Security.App
    Private _mTransportbeauftragung As Transportbeauftragung

    Protected WithEvents ucStyles As Portal.PageElements.Styles
    Protected WithEvents ucHeader As Portal.PageElements.Header

    Private Const noEntryText As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _mUser = GetUser(Me)
        ucHeader.InitUser(_mUser)
        FormAuth(Me, _mUser)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = _mUser.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            
            If Session("_mTransportbeauftragung") IsNot Nothing Then
                _mTransportbeauftragung = CType(Session("_mTransportbeauftragung"), Transportbeauftragung)
            End If

            If Not IsPostBack Then
                If CInt(Session("Step")) = 2 Then
                    txtDatZulVon.SelectedDate = _mTransportbeauftragung.DatumZulassungVon
                    txtDatZulBis.SelectedDate = _mTransportbeauftragung.DatumZulassungBis
                    txtDatFreiVon.SelectedDate = _mTransportbeauftragung.DatumFreisetzungVon
                    txtDatFreiBis.SelectedDate = _mTransportbeauftragung.DatumFreisetzungBis

                    ShowStep2()
                Else
                    Session("bClearVorbelegung") = True

                    txtDatFreiVon.SelectedDate = Today
                    txtDatFreiBis.SelectedDate = Today

                    Session("Step") = 1
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub cmdBack_Click(sender As Object, e As System.EventArgs) Handles cmdBack.Click
        If CInt(Session("Step")) = 1 Then
            Response.Redirect("Change10.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10'")(0)("AppID").ToString)
        ElseIf CInt(Session("Step")) = 2 Then
            ToggleDatePickerOn(True)
            Session("Step") = 1
        End If
    End Sub

    Private Sub cmdCreate_Click(sender As Object, e As System.EventArgs) Handles cmdCreate.Click
        If CInt(Session("Step")) = 1 Then
            DoSubmit()

            ShowStep2()
        ElseIf CInt(Session("Step")) = 2 Then
            UpdateFilter()

            Session("_mTransportbeauftragung") = _mTransportbeauftragung
            Session("Step") = 3

            Response.Redirect("Change10_Beauftragung_2.aspx?AppID=" & _mUser.Applications.Select("AppName='Change10_Beauftragung_2'")(0)("AppID").ToString)
        End If

    End Sub

    Private Sub rgStatistik_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgStatistik.NeedDataSource
        FillGridStatistik(False)
    End Sub

    Protected Sub txtDatZulVon_SelectedDateChanged(sender As Object, e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles txtDatZulVon.SelectedDateChanged
        ClearVorbelegungOnce()
    End Sub

    Protected Sub txtDatZulBis_SelectedDateChanged(sender As Object, e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles txtDatZulBis.SelectedDateChanged
        ClearVorbelegungOnce()
    End Sub

    ''' <summary>
    ''' Liest den aktuellen Wert der einer RadComboBox aus
    ''' </summary>
    ''' <param name="ComboBox">Das auszuwertende Objekt</param>
    ''' <returns>Wert als String</returns>
    Private Function GetFilterValue(ByRef ComboBox As RadComboBox) As String
        If ComboBox.SelectedValue = noEntryText Then
            Return ""
        Else
            Return ComboBox.SelectedValue
        End If
    End Function

    ''' <summary>
    ''' Holt die Bestandsdaten zu den eingegebenen Daten
    ''' </summary>
    Private Sub DoSubmit()
        _mTransportbeauftragung.DatumZulassungVon = txtDatZulVon.SelectedDate
        _mTransportbeauftragung.DatumZulassungBis = txtDatZulBis.SelectedDate
        _mTransportbeauftragung.DatumFreisetzungVon = txtDatFreiVon.SelectedDate
        _mTransportbeauftragung.DatumFreisetzungBis = txtDatFreiBis.SelectedDate

        _mTransportbeauftragung.FillBestand(Session("AppID").ToString, Session.SessionID)

        Session("_mTransportbeauftragung") = _mTransportbeauftragung
    End Sub

    ''' <summary>
    ''' Füllt das StatistikGrid
    ''' </summary>
    ''' <param name="rebind">Daten neu binden?</param>
    Private Sub FillGridStatistik(rebind As Boolean)
        _mTransportbeauftragung.FillStatistik()

        rgStatistik.DataSource = _mTransportbeauftragung.tblStatistik

        If rebind Then
            rgStatistik.Rebind()
        End If
    End Sub

    ''' <summary>
    ''' Füllt die DropDownlisten der Filter
    ''' </summary>
    Private Sub FillDropDowns()

        If _mTransportbeauftragung.lstCarports.Count = 0 Then
            rcbCarport.Enabled = False
        Else
            rcbCarport.DataSource = _mTransportbeauftragung.lstCarports
            rcbCarport.DataBind()
            rcbCarport.Enabled = True
        End If
        'rcbCarport.Items.Insert(0, New RadComboBoxItem(noEntryText))

        If _mTransportbeauftragung.lstHersteller.Count = 0 Then
            rcbHersteller.Enabled = False
        Else
            rcbHersteller.DataSource = _mTransportbeauftragung.lstHersteller
            rcbHersteller.DataBind()
            rcbHersteller.Enabled = True
        End If
        rcbHersteller.Items.Insert(0, New RadComboBoxItem(noEntryText))

        If _mTransportbeauftragung.lstKraftstoffe.Count = 0 Then
            rcbKraftstoffart.Enabled = False
        Else
            rcbKraftstoffart.DataSource = _mTransportbeauftragung.lstKraftstoffe
            rcbKraftstoffart.DataBind()
            rcbKraftstoffart.Enabled = True
        End If
        rcbKraftstoffart.Items.Insert(0, New RadComboBoxItem(noEntryText))

        If _mTransportbeauftragung.lstVermietgruppe.Count = 0 Then
            rcbVermietgruppe.Enabled = False
        Else
            rcbVermietgruppe.DataSource = _mTransportbeauftragung.lstVermietgruppe
            rcbVermietgruppe.DataBind()
            rcbVermietgruppe.Enabled = True
        End If
        rcbVermietgruppe.Items.Insert(0, New RadComboBoxItem(noEntryText))

        SelectFilterInDropDown()
    End Sub

    ''' <summary>
    '''  Schaltet die Enabled Eigenschaft aller DatePicker an oder aus
    ''' </summary>
    ''' <param name="enable">enable = true</param>
    Private Sub ToggleDatePickerOn(enable As Boolean)
        txtDatZulVon.Enabled = enable
        txtDatZulBis.Enabled = enable
        txtDatFreiVon.Enabled = enable
        txtDatFreiBis.Enabled = enable

        tblFilter.Visible = Not enable
        rgStatistik.Visible = Not enable
    End Sub

    Private Sub ShowStep2()
        FillGridStatistik(True)
       
        SelectFilterInDropDown()
        FillDropDowns()

        ToggleDatePickerOn(False)

        Session("Step") = 2
    End Sub

    ''' <summary>
    ''' Löscht die Vorbelegung der Daten bei der ersten Änderung
    ''' </summary>
    Private Sub ClearVorbelegungOnce()
        If CBool(Session("bClearVorbelegung")) Then
            txtDatFreiVon.Clear()
            txtDatFreiBis.Clear()

            Session("bClearVorbelegung") = False
        End If
    End Sub

    Private Sub UpdateFilter()
        _mTransportbeauftragung.strFilterCarport = GetFilterValue(rcbCarport)
        _mTransportbeauftragung.strFilterHersteller = GetFilterValue(rcbHersteller)
        _mTransportbeauftragung.strFilterVermietergruppe = GetFilterValue(rcbVermietgruppe)
        _mTransportbeauftragung.strFilterKraftstoffart = GetFilterValue(rcbKraftstoffart)
    End Sub

    Private Sub rcbCarport_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbCarport.SelectedIndexChanged
        _mTransportbeauftragung.strFilterCarport = GetFilterValue(rcbCarport)
        _mTransportbeauftragung.strFilterHersteller = ""
        _mTransportbeauftragung.strFilterVermietergruppe = ""
        _mTransportbeauftragung.strFilterKraftstoffart = ""

        _mTransportbeauftragung.RefreshLists()
        FillDropDowns()
    End Sub

    Protected Sub rcbHersteller_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbHersteller.SelectedIndexChanged
        UpdateFilter()
        _mTransportbeauftragung.RefreshLists()
        FillDropDowns()
    End Sub

    Protected Sub rcbVermietgruppe_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbVermietgruppe.SelectedIndexChanged
        UpdateFilter()
        _mTransportbeauftragung.RefreshLists()
        FillDropDowns()
    End Sub

    Protected Sub rcbKraftstoffart_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbKraftstoffart.SelectedIndexChanged
        UpdateFilter()
        _mTransportbeauftragung.RefreshLists()
        FillDropDowns()
    End Sub

    ''' <summary>
    ''' Selektiert die Dropdown-Werte anhand der gesetzten Filter 
    ''' </summary>
    Private Sub SelectFilterInDropDown()
        If _mTransportbeauftragung.strFilterCarport = "" AndAlso _mTransportbeauftragung.lstCarports.Count <> 0 Then
            _mTransportbeauftragung.strFilterCarport = _mTransportbeauftragung.lstCarports(0)
        End If

        rcbCarport.SelectedValue = _mTransportbeauftragung.strFilterCarport
        rcbHersteller.SelectedValue = _mTransportbeauftragung.strFilterHersteller
        rcbKraftstoffart.SelectedValue = _mTransportbeauftragung.strFilterKraftstoffart
        rcbVermietgruppe.SelectedValue = _mTransportbeauftragung.strFilterVermietergruppe

        _mTransportbeauftragung.RefreshLists()
    End Sub

End Class