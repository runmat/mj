Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.Web.UI
Imports System.Net
Imports System.IO

Partial Public Class UploadUeberf
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    '    Private dv As DataView
    Private ExcelPrint As DataTable
    Protected WithEvents drpRegulierer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents drpRechnungsempf As System.Web.UI.WebControls.DropDownList
    Private clsUeberf As UeberfgStandard_01

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        If Request.UrlReferrer.ToString.IndexOf("Selection.aspx") = 0 Then
            Session("App_Ueberf") = Nothing
            clsUeberf = Nothing
        ElseIf Not IsPostBack Then
            If Session("App_Ueberf") Is Nothing Then
                clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
                clsUeberf.FillData()
                If Not Session("App_SelData") Is Nothing Then
                    FillChecked()
                End If
                If Not Session("App_SelUser") Is Nothing Then
                    clsUeberf.selectedUser = Session("App_SelUser")
                    RadioButtonList1.SelectedValue = clsUeberf.selectedUser
                    If clsUeberf.selectedUser = "X" Then
                        RadioButtonList1.Items(1).Selected = True
                    Else
                        RadioButtonList1.Items(0).Selected = True
                    End If
                Else
                    RadioButtonList1.Items(1).Selected = True
                End If
            Else
                clsUeberf = Session("App_Ueberf")
                If Not Session("App_SelUser") Is Nothing Then
                    clsUeberf.selectedUser = Session("App_SelUser")
                    RadioButtonList1.SelectedValue = clsUeberf.selectedUser
                    If clsUeberf.selectedUser = "X" Then
                        RadioButtonList1.Items(1).Selected = True
                    Else
                        RadioButtonList1.Items(0).Selected = True
                    End If

                End If
            End If
            If Not Session("App_PageIndex") Is Nothing Then
                FillGrid(Session("App_PageIndex"))
            Else
                FillGrid(0)
            End If

        Else
            clsUeberf = Session("App_Ueberf")
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim table As DataTable
        If Not (clsUeberf.TabUpload Is Nothing) Then
            table = clsUeberf.TabUpload
            Session("App_Ueberf") = clsUeberf
        Else
            table = Nothing
        End If

        If (table Is Nothing) OrElse (table.Rows.Count = 0) Then
            DataGrid1.Visible = False
            lbl_Filter.Visible = False
            RadioButtonList1.Visible = False
            lblError.Visible = True
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            cmdSend0.Visible = False
        Else
            DataGrid1.Visible = True
            lblError.Visible = False
            cmdSend0.Visible = True
            Dim datarow As DataRow

            For Each datarow In table.Rows
                datarow("Url") = "UploadUeberf_01.aspx?AppID=" & Session("AppID").ToString
            Next

            Dim tmpDataView As DataView = table.DefaultView
            If clsUeberf.SelUser = "X" Then
                tmpDataView.RowFilter = "User='" & m_User.UserName & "'"
            Else
                tmpDataView.RowFilter = ""
            End If
            If tmpDataView.Count > 0 Then

                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
                    strTempSort = strSort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If strDirection = "asc" Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    ViewState("Sort") = strTempSort
                    ViewState("Direction") = strDirection
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        strTempSort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "asc"
                            ViewState("Direction") = strDirection
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If

                DataGrid1.CurrentPageIndex = intTempPageIndex
                DataGrid1.DataSource = tmpDataView

                DataGrid1.DataBind()

                If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                    lblError.Text = CStr(Session("ShowOtherString"))
                    txt_Protokoll.Text = CStr(Session("ShowOtherString"))
                Else
                    txt_Protokoll.Text = "Es wurden " & tmpDataView.Count.ToString & " Aufträge für die  Nachbearbeitung gefunden." & vbCrLf
                End If

                If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                    'lnkKreditlimit.Text = "Zurück"
                    'lnkKreditlimit.NavigateUrl = "javascript:history.back()"
                End If

                lblError.Visible = True
                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If
            Else
                DataGrid1.Visible = False
                lblError.Visible = True
                lblError.Text = "Keine Daten zur Anzeige gefunden."
                cmdSend0.Visible = False
            End If
        End If
    End Sub

    Private Sub FillGrid2(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim table As DataTable
        Try
            If Not (clsUeberf.TabUpload Is Nothing) OrElse (clsUeberf.TabUpload.Rows.Count > 0) Then
                table = clsUeberf.TabUpload
                Session("App_Ueberf") = clsUeberf
            Else
                table = Nothing
            End If

            If (table Is Nothing) OrElse (table.Rows.Count = 0) Then
                lbl_Filter.Visible = False
                RadioButtonList1.Visible = False
                DataGrid1.Visible = False
                lblError.Visible = True
                lblError.Text = "Keine Daten zur Anzeige gefunden."
                cmdSend0.Visible = False
            Else
                DataGrid1.Visible = True
                lblError.Visible = False

                Dim datarow As DataRow

                For Each datarow In table.Rows
                    datarow("Url") = "UploadUeberf_01.aspx?AppID=" & Session("AppID").ToString
                Next

                Dim tmpDataView As DataView = table.DefaultView
                If clsUeberf.SelUser = "X" Then
                    table.DefaultView.RowFilter = "Vbeln <>'' AND User='" & m_User.UserName & "'"
                Else
                    table.DefaultView.RowFilter = "Vbeln <>''"
                End If

                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
                    strTempSort = strSort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If strDirection = "asc" Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    ViewState("Sort") = strTempSort
                    ViewState("Direction") = strDirection
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        strTempSort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "asc"
                            ViewState("Direction") = strDirection
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If

                DataGrid1.CurrentPageIndex = intTempPageIndex
                DataGrid1.DataSource = tmpDataView

                DataGrid1.DataBind()
                DataGrid1.CurrentPageIndex = intTempPageIndex
                DataGrid1.DataSource = tmpDataView

                DataGrid1.DataBind()

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If
            End If

            DataGrid1.Columns(0).Visible = False
            DataGrid1.Columns(12).Visible = False
            DataGrid1.Columns(11).Visible = False
            DataGrid1.Columns(13).Visible = False
            DataGrid1.Columns(14).Visible = True
            DataGrid1.Columns(15).Visible = False
            cmdSend0.Visible = False
            cmdBack0.Visible = True
            lbl_Filter.Visible = False
            RadioButtonList1.Visible = False
            table.DefaultView.RowFilter = "Vbeln = 'gelöscht'"
            If table.DefaultView.Count > 0 Then
                txt_Protokoll.Text = txt_Protokoll.Text & "Es wurden " & table.DefaultView.Count & " Aufträge gelöscht." & vbCrLf
            End If
            table.DefaultView.RowFilter = "Vbeln <> 'gelöscht' AND Vbeln <> ''"
            If table.DefaultView.Count > 0 Then
                txt_Protokoll.Text = txt_Protokoll.Text & "Es wurden " & table.DefaultView.Count & " Aufträge in unserem System erfasst." & vbCrLf
            End If
            FillExcel(clsUeberf)
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = ex.Message
        End Try
    End Sub
    
    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        Dim cell As TableCell
        Dim strAufID As String = ""
        If e.CommandName = "Edit" Then

            cell = e.Item.Cells(1)
            strAufID = cell.Text

            Try
                clsUeberf.SelAufID = strAufID
                SelAuftrag(strAufID)
                FillPartnerClass(clsUeberf)
                FillAuftragClass(clsUeberf)

                clsUeberf.HoldData = True
                Checkgrid()
                
                Response.Redirect("UploadEdit01.aspx?AppID=" & Session("AppID").ToString)
            Catch ex As Exception
                lblError.Text = "Fehler beim Laden der Daten in die Detailansicht"
            End Try
        End If

    End Sub

    '----------------------------------------------------------------------
    ' Methode: Checkgrid
    ' Autor: O.Rudolph
    ' Beschreibung: -   es wird geprüft welche Einträgen auf OK oder Löschen
    '                   gesetzt wurden
    '               -   anschließend in die Tabelle geschrieben                    
    ' Erstellt am: 26.09.08
    ' ITA: 2197
    '----------------------------------------------------------------------

    Private Sub Checkgrid()

        Dim dataRows() As DataRow
        Dim cell As TableCell
        Dim cellChecked As TableCell
        Dim strAufID As String = ""
        Dim Control As Control
        Dim checkbox As CheckBox
        Dim Item As DataGridItem
        'Checkbox-Einträge merken
        For Each Item In DataGrid1.Items
            cell = Item.Cells(1)
            strAufID = cell.Text.ToString
            cellChecked = Item.Cells(11)

            With clsUeberf
                dataRows = .TabUpload.Select("AUF_ID='" & strAufID & "'")
                For Each Control In cellChecked.Controls
                    If TypeOf Control Is CheckBox Then
                        CheckBox = CType(Control, CheckBox)
                        If CheckBox.ID = "rb_ok" AndAlso CheckBox.Checked Then
                            If dataRows.Length > 0 Then
                                dataRows(0)("OK") = True
                                dataRows(0)("Del") = False
                                dataRows(0)("NoSel") = False
                            End If
                            .TabUpload.AcceptChanges()
                        End If
                    End If
                Next
                cellChecked = Item.Cells(12)
                For Each Control In cellChecked.Controls
                    Dim box = TryCast(Control, CheckBox)
                    If (box IsNot Nothing) Then
                        checkbox = box
                        If checkbox.ID = "rb_del" AndAlso checkbox.Checked Then
                            If dataRows.Length > 0 Then
                                dataRows(0)("OK") = False
                                dataRows(0)("Del") = True
                                dataRows(0)("NoSel") = False
                            End If
                            .TabUpload.AcceptChanges()
                        End If
                    End If
                Next
                cellChecked = Item.Cells(13)
                For Each Control In cellChecked.Controls
                    Dim box = TryCast(Control, CheckBox)
                    If (box IsNot Nothing) Then
                        checkbox = box
                        If checkbox.ID = "rb_Deselect" AndAlso checkbox.Checked Then
                            If dataRows.Length > 0 Then
                                dataRows(0)("OK") = False
                                dataRows(0)("Del") = False
                                dataRows(0)("NoSel") = True
                            End If
                            .TabUpload.AcceptChanges()
                        End If
                    End If
                Next
            End With
        Next
        Session("App_Ueberf") = clsUeberf
    End Sub
    
    Private Sub FillExcel(ByRef ueberf As UeberfgStandard_01)

        With clsUeberf
            ExcelPrint = New DataTable
            ExcelPrint.Columns.Add("Angel. am", System.Type.GetType("System.String"))
            ExcelPrint.Columns.Add("Überf. am", System.Type.GetType("System.String"))
            ExcelPrint.Columns.Add("Referenz", System.Type.GetType("System.String"))
            ExcelPrint.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
            ExcelPrint.Columns.Add("Fahrzeugtyp", System.Type.GetType("System.String"))
            ExcelPrint.Columns.Add("Startort", System.Type.GetType("System.String"))
            ExcelPrint.Columns.Add("Zielort", System.Type.GetType("System.String"))
            ExcelPrint.Columns.Add("Warenempf. Zielort", System.Type.GetType("System.String"))
            ExcelPrint.Columns.Add("Rueckort", System.Type.GetType("System.String"))
            ExcelPrint.Columns.Add("Auftragsnummer", System.Type.GetType("System.String"))

            Dim dataRows() As DataRow
            Dim dataRow As DataRow
            Dim NewRow As DataRow
            dataRows = .TabUpload.Select("VBeln <>''")

            For Each dataRow In dataRows
                NewRow = ExcelPrint.NewRow
                NewRow("Angel. am") = dataRow("Datum")
                NewRow("Überf. am") = dataRow("UeberfDatum")
                NewRow("Referenz") = dataRow("Referenz")
                NewRow("Kennzeichen") = dataRow("Kennzeichen")
                NewRow("Fahrzeugtyp") = dataRow("Fahrzeugtyp")
                NewRow("Startort") = dataRow("Startort")
                NewRow("Zielort") = dataRow("Zielort")
                NewRow("Warenempf. Zielort") = dataRow("WEZielort")
                NewRow("Rueckort") = dataRow("Rueckort")
                NewRow("Auftragsnummer") = dataRow("Vbeln")
                ExcelPrint.Rows.Add(NewRow)
            Next
            ExcelPrint.AcceptChanges()
            Session("App_Excel") = ExcelPrint
        End With
        tdExcel.Visible = True
    End Sub

    Private Sub FillPartnerClass(ByRef ueberf As UeberfgStandard_01)
        'Dim strAufID As String = ""
        Dim dataRows() As DataRow
        Dim dataRow As DataRow


        With clsUeberf

            'Auftraggeber
            dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'AG'")
            If dataRows.Length > 0 Then

                If Not IsDBNull(dataRows(0)("NAME")) Then
                    .KundeName = dataRows(0)("NAME")
                End If
                If Not IsDBNull(dataRows(0)("STREET")) Then
                    .KundeStrasse = dataRows(0)("STREET")
                End If
                If Not IsDBNull(dataRows(0)("POSTL_CODE")) AndAlso Not IsDBNull(dataRows(0)("CITY")) Then
                    .KundeOrt = dataRows(0)("POSTL_CODE") & " " & dataRows(0)("CITY")
                End If

                If Not IsDBNull(dataRows(0)("NAME_2")) Then
                    .KundeAnsprechpartner = dataRows(0)("NAME_2")
                End If

            Else
                dataRow = .TabPartnerSel.NewRow
                dataRow("Partn_Role") = "AG"
                dataRow("AUF_ID") = .SelAufID
                dataRow("Kunnr_AG") = Right("0000000000" & m_User.KUNNR, 10)
                dataRow("Itm_Number") = "000000"
                dataRow("Partn_Numb") = Right("0000000000" & m_User.KUNNR, 10)
                .TabPartnerSel.Rows.Add(dataRow)
            End If
            'Regulierer
            dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'RG'")

            If dataRows.Length > 0 Then
                clsUeberf.SelRegulierer = dataRows(0)("PARTN_NUMB")

                If Not IsDBNull(dataRows(0)("NAME_2")) Then
                    .AbAnsprechpartner = dataRows(0)("NAME_2")
                End If

                If Not IsDBNull(dataRows(0)("TELEPHONE")) Then
                    .AbTelefon = dataRows(0)("TELEPHONE")
                End If
                If Not IsDBNull(dataRows(0)("NAME")) Then
                    .AbName = dataRows(0)("NAME")
                End If

                If Not IsDBNull(dataRows(0)("CITY")) Then
                    .AbOrt = dataRows(0)("CITY")
                End If

                If Not IsDBNull(dataRows(0)("POSTL_CODE")) Then
                    .AbPlz = dataRows(0)("POSTL_CODE")
                End If

                If Not IsDBNull(dataRows(0)("STREET")) Then
                    .AbStrasse = dataRows(0)("STREET")
                End If
            Else
                dataRow = .TabPartnerSel.NewRow
                dataRow("Partn_Role") = "RG"
                dataRow("AUF_ID") = .SelAufID
                dataRow("Kunnr_AG") = Right("0000000000" & m_User.KUNNR, 10)
                dataRow("Itm_Number") = "000000"
                .TabPartnerSel.Rows.Add(dataRow)
            End If

            dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'RE'")
            'Rechnungsempfänger
            If dataRows.Length > 0 Then

                .SelRechnungsempf = dataRows(0)("PARTN_NUMB")

                If Not IsDBNull(dataRows(0)("NAME_2")) Then
                    .AnAnsprechpartner = dataRows(0)("NAME_2")
                End If
                If Not IsDBNull(dataRows(0)("NAME")) Then
                    .AnName = dataRows(0)("NAME")
                End If

                If Not IsDBNull(dataRows(0)("CITY")) Then
                    .AnOrt = dataRows(0)("CITY")
                End If

                If Not IsDBNull(dataRows(0)("POSTL_CODE")) Then
                    .AnPlz = dataRows(0)("POSTL_CODE")
                End If

                If Not IsDBNull(dataRows(0)("STREET")) Then
                    .AnStrasse = dataRows(0)("STREET")
                End If

                If Not IsDBNull(dataRows(0)("TELEPHONE")) Then
                    .AnTelefon = dataRows(0)("TELEPHONE")
                End If
            Else
                dataRow = .TabPartnerSel.NewRow
                dataRow("Partn_Role") = "RE"
                dataRow("AUF_ID") = .SelAufID
                dataRow("Kunnr_AG") = Right("0000000000" & m_User.KUNNR, 10)
                dataRow("Itm_Number") = "000000"
                .TabPartnerSel.Rows.Add(dataRow)
            End If

            dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'ZB'")
            If dataRows.Length > 0 Then
                For Each dataRow In dataRows
                    'Abholadresse
                    If Not IsDBNull(dataRows(0)("PARTN_NUMB")) Then
                        .SelAbholung = dataRows(0)("PARTN_NUMB")
                    End If

                    If Not IsDBNull(dataRows(0)("NAME_2")) Then
                        .AbAnsprechpartner = dataRows(0)("NAME_2")
                    End If

                    If Not IsDBNull(dataRows(0)("TELEPHONE")) Then
                        .AbTelefon = dataRows(0)("TELEPHONE")
                    End If
                    If Not IsDBNull(dataRows(0)("NAME")) Then
                        .AbName = dataRows(0)("NAME")
                    End If
                    If Not IsDBNull(dataRows(0)("CITY")) Then
                        .AbOrt = dataRows(0)("CITY")
                    End If

                    If Not IsDBNull(dataRows(0)("POSTL_CODE")) Then
                        .AbPlz = dataRows(0)("POSTL_CODE")
                    End If

                    If Not IsDBNull(dataRows(0)("STREET")) Then
                        .AbStrasse = dataRows(0)("STREET")
                    End If

                    If Not IsDBNull(dataRows(0)("TELEPHONE2")) Then

                        .AbTelefon2 = dataRows(0)("TELEPHONE2")
                    End If
                    If Not IsDBNull(dataRows(0)("FAX_NUMBER")) Then
                        .AbFax = dataRows(0)("FAX_NUMBER")
                    End If

                Next
            Else
                dataRow = .TabPartnerSel.NewRow
                dataRow("Partn_Role") = "ZB"
                dataRow("AUF_ID") = .SelAufID
                dataRow("Kunnr_AG") = Right("0000000000" & m_User.KUNNR, 10)
                dataRow("Itm_Number") = "000000"
                .TabPartnerSel.Rows.Add(dataRow)
            End If
            'Anlieferadresse

            dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'WE'")
            If dataRows.Length > 0 Then
                For Each dataRow In dataRows
                    If Not IsDBNull(dataRows(0)("PARTN_NUMB")) Then
                        clsUeberf.SelAnlieferung = dataRows(0)("PARTN_NUMB")
                    End If

                    If Not IsDBNull(dataRows(0)("NAME_2")) Then
                        .AnAnsprechpartner = dataRows(0)("NAME_2")
                    End If
                    If Not IsDBNull(dataRows(0)("NAME")) Then
                        .AnName = dataRows(0)("NAME")
                    End If

                    If Not IsDBNull(dataRows(0)("CITY")) Then
                        .AnOrt = dataRows(0)("CITY")
                    End If

                    If Not IsDBNull(dataRows(0)("POSTL_CODE")) Then
                        .AnPlz = dataRows(0)("POSTL_CODE")
                    End If

                    If Not IsDBNull(dataRows(0)("STREET")) Then
                        .AnStrasse = dataRows(0)("STREET")
                    End If

                    If Not IsDBNull(dataRows(0)("TELEPHONE")) Then
                        .AnTelefon = dataRows(0)("TELEPHONE")
                    End If
                    If Not IsDBNull(dataRows(0)("TELEPHONE2")) Then
                        .AnTelefon2 = dataRows(0)("TELEPHONE2")
                    End If
                    If Not IsDBNull(dataRows(0)("FAX_NUMBER")) Then
                        .AnFax = dataRows(0)("FAX_NUMBER")
                    End If
                Next
            Else
                dataRow = .TabPartnerSel.NewRow
                dataRow("Partn_Role") = "WE"
                dataRow("AUF_ID") = .SelAufID
                dataRow("Kunnr_AG") = Right("0000000000" & m_User.KUNNR, 10)
                dataRow("Itm_Number") = "000000"
                .TabPartnerSel.Rows.Add(dataRow)
            End If

            dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'ZR'")
            If dataRows.Length > 0 Then
                For Each dataRow In dataRows
                    If Not IsDBNull(dataRows(0)("PARTN_NUMB")) Then
                        clsUeberf.SelRetour = dataRows(0)("PARTN_NUMB")
                        clsUeberf.Anschluss = True
                    End If

                    If Not IsDBNull(dataRows(0)("NAME_2")) Then
                        .ReAnsprechpartner = dataRows(0)("NAME_2")
                    End If
                    If Not IsDBNull(dataRows(0)("NAME")) Then
                        .ReName = dataRows(0)("NAME")
                    End If

                    If Not IsDBNull(dataRows(0)("CITY")) Then
                        .ReOrt = dataRows(0)("CITY")
                    End If

                    If Not IsDBNull(dataRows(0)("POSTL_CODE")) Then
                        .RePlz = dataRows(0)("POSTL_CODE")
                    End If

                    If Not IsDBNull(dataRows(0)("STREET")) Then
                        .ReStrasse = dataRows(0)("STREET")
                    End If

                    If Not IsDBNull(dataRows(0)("TELEPHONE")) Then
                        .ReTelefon = dataRows(0)("TELEPHONE")
                    End If
                    If Not IsDBNull(dataRows(0)("TELEPHONE2")) Then
                        .ReTelefon2 = dataRows(0)("TELEPHONE2")
                    End If
                    If Not IsDBNull(dataRows(0)("FAX_NUMBER")) Then
                        .ReFax = dataRows(0)("FAX_NUMBER")
                    End If
                Next
            End If
        End With
        Session("App_Ueberf") = clsUeberf
    End Sub

    Private Sub FillAuftragClass(ByRef ueberf As UeberfgStandard_01)
        Dim dataRows() As DataRow
        Dim dataRow As DataRow

        With clsUeberf
            dataRows = .TabUeberf.Select("AUF_ID='" & clsUeberf.SelAufID & "'")
            For Each dataRow In dataRows

                If Not IsDBNull(dataRows(0)("ZZFAHRZGTYP")) Then
                    .Herst = dataRows(0)("ZZFAHRZGTYP")
                End If

                If Not IsDBNull(dataRows(0)("ZZFAHRZGTYPRUCK")) Then
                    .ReHerst = dataRows(0)("ZZFAHRZGTYPRUCK")
                End If

                If Not IsDBNull(dataRows(0)("ZZKENN")) Then
                    Dim tempKennz() As String
                    tempKennz = Split(dataRows(0)("ZZKENN").ToString, "-")
                    If tempKennz.Length = 2 Then
                        .Kenn1 = tempKennz(0)
                        .Kenn2 = tempKennz(1)
                    End If
                End If

                If Not IsDBNull(dataRows(0)("ZZKENNRUCK")) Then
                    Dim tempKennz() As String
                    tempKennz = Split(dataRows(0)("ZZKENNRUCK").ToString, "-")
                    If tempKennz.Length = 2 Then
                        .ReKenn1 = tempKennz(0)
                        .ReKenn2 = tempKennz(1)
                    End If
                End If

                If Not IsDBNull(dataRows(0)("ZZFAHRG")) Then
                    .Vin = dataRows(0)("ZZFAHRG")
                End If
                If Not IsDBNull(dataRows(0)("ZZREFNR")) Then
                    .Ref = dataRows(0)("ZZREFNR")
                End If

                If Not IsDBNull(dataRows(0)("ZZFAHRGRUCK")) Then
                    .ReVin = dataRows(0)("ZZFAHRGRUCK")
                End If

                If Not IsDBNull(dataRows(0)("ZZREFNRRUCK")) Then
                    .ReRef = dataRows(0)("ZZREFNRRUCK")
                End If

                If Not IsDBNull(dataRows(0)("BEMERKUNG02")) Then
                    .Bemerkung = dataRows(0)("BEMERKUNG02")
                End If

                If Not IsDBNull(dataRows(0)("VDATU")) AndAlso IsDate(dataRows(0)("VDATU")) AndAlso CType(dataRows(0)("VDATU"), DateTime).Year > 1900 Then
                    .DatumUeberf = dataRows(0)("VDATU")
                End If

                If Not IsDBNull(dataRows(0)("BEMERKUNG")) Then
                    .Bemerkung = dataRows(0)("BEMERKUNG")
                End If

                If Not IsDBNull(dataRows(0)("EINW")) Then
                    If dataRows(0)("EINW").ToString.ToLower = "x" Then
                        .FzgEinweisung = True
                    End If
                End If

                If Not IsDBNull(dataRows(0)("TANKE")) Then
                    If dataRows(0)("TANKE").ToString.ToLower = "x" Then
                        .Tanken = True
                    End If
                End If

                If Not IsDBNull(dataRows(0)("WASCHEN")) Then
                    If dataRows(0)("WASCHEN").ToString.ToLower = "x" Then
                        .Waesche = True
                    End If
                End If

                If Not IsDBNull(dataRows(0)("ROTKENN")) Then
                    If dataRows(0)("ROTKENN").ToString.ToLower = "x" Then
                        .RotKenn = True
                    End If
                End If

                If Not IsDBNull(dataRows(0)("ZULGE")) Then
                    If dataRows(0)("ZULGE").ToString.ToLower = "x" Then
                        .FzgZugelassen = "Ja"
                    ElseIf dataRows(0)("ZULGE") = "n" Then
                        .FzgZugelassen = "Nein"
                    End If
                End If

                If Not IsDBNull(dataRows(0)("ZULGRUCK")) Then
                    If dataRows(0)("ZULGRUCK").ToString.ToLower = "x" Then
                        .ReFzgZugelassen = "Ja"
                    ElseIf dataRows(0)("ZULGRUCK") = "n" Then
                        .ReFzgZugelassen = "Nein"
                    End If
                End If

                If Not IsDBNull(dataRows(0)("SOWIHIN")) Then
                    If dataRows(0)("SOWIHIN").ToString.ToLower = "x" Then
                        .SomWin = "Winter"
                    ElseIf dataRows(0)("SOWIHIN") = "G" Then
                        .SomWin = "Ganzjahresreifen"
                    ElseIf dataRows(0)("SOWIHIN") = "S" Then
                        .SomWin = "Sommer"
                    End If
                End If

                If Not IsDBNull(dataRows(0)("SOWIRUCK")) Then
                    If dataRows(0)("SOWIRUCK").ToString.ToLower = "x" Then
                        .ReSomWin = "Winter"
                    ElseIf dataRows(0)("SOWIRUCK") = "G" Then
                        .ReSomWin = "Ganzjahresreifen"
                    ElseIf dataRows(0)("SOWIRUCK") = "S" Then
                        .ReSomWin = "Sommer"
                    End If
                End If

                If Not IsDBNull(dataRows(0)("HIN_ZUL_KCL")) Then
                    If dataRows(0)("HIN_ZUL_KCL").ToString.ToLower = "x" Then
                        .Hin_KCL_Zulassen = "Ja"
                    ElseIf dataRows(0)("HIN_ZUL_KCL") = "N" Then
                        .Hin_KCL_Zulassen = "Nein"
                    End If
                End If

                If Not IsDBNull(dataRows(0)("AUGRU")) Then
                    .SelFahrzeugwert = dataRows(0)("AUGRU")
                End If

                If Not IsDBNull(dataRows(0)("KFZ_KL")) Then
                    .Fahrzeugklasse = dataRows(0)("KFZ_KL")
                End If

                If Not IsDBNull(dataRows(0)("KFZ_KLR")) Then
                    .ReFahrzeugklasse = dataRows(0)("KFZ_KLR")
                End If

                If Not IsDBNull(dataRows(0)("EXPRESS_VERSAND")) Then
                    If dataRows(0)("EXPRESS_VERSAND").ToString.ToLower = "x" Then
                        .Express = "Ja"
                    ElseIf dataRows(0)("EXPRESS_VERSAND") = "n" Then
                        .Express = "Nein"
                    End If
                End If

            Next
        End With
        Session("App_Ueberf") = clsUeberf
    End Sub

    Protected Sub cmdSend0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSend0.Click
        Dim strAufID As String = ""
        Dim angelegt As Boolean
        Dim geloescht As Boolean
        Dim dataRows() As DataRow
        Dim dataRow As DataRow
        Dim gridRow As DataRow
        Checkgrid()
        Try
            For Each gridRow In clsUeberf.TabUpload.Rows
                strAufID = gridRow("AUF_ID").ToString
                If gridRow("OK") = True Then
                    With clsUeberf
                        SelAuftrag(strAufID)
                        .CleanClass()
                        FillPartnerClass(clsUeberf)
                        FillAuftragClass(clsUeberf)
                        .SaveUploaded()
                        .AuftragsStatus = "A"
                        .Update("B")
                        dataRows = .TabUpload.Select("AUF_ID='" & strAufID & "'")

                        For Each dataRow In dataRows
                            If .Message.Length = 0 Then
                                dataRow("Vbeln") = .Vbeln.TrimStart("0"c)
                            Else
                                dataRow("Vbeln") = .Message
                            End If
                        Next

                        .TabUpload.AcceptChanges()
                        angelegt = True
                        ' txt_Protokoll.Text = "1 Auftrag " & .Vbeln.TrimStart("0"c) & " erfolgreich angelegt. " & vbCrLf
                    End With
                ElseIf gridRow("del") = True Then
                    With clsUeberf
                        .CleanClass()
                        SelAuftrag(strAufID)
                        FillPartnerClass(clsUeberf)
                        FillAuftragClass(clsUeberf)
                        .AuftragsStatus = "L"
                        .Update()
                        geloescht = True
                        dataRows = .TabUpload.Select("AUF_ID='" & strAufID & "'")
                        For Each dataRow In dataRows
                            If .Message.Length = 0 Then
                                dataRow("Vbeln") = "gelöscht"
                            Else
                                dataRow("Vbeln") = .Message
                            End If
                        Next
                        'txt_Protokoll.Text = "1 Auftrag erfolgreich gelöscht." & vbCrLf
                    End With
                End If
            Next
            If angelegt = True Or geloescht = True Then
                Session("App_Ueberf") = clsUeberf
                FillGrid2(0)
                tdExcel.Visible = True
                cmdSend0.Visible = False
                cmdBack0.Visible = True
            Else
                FillGrid(0)
                lblError.Text = "Kein Auftrag ausgewählt!"
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Daten in die Detailansicht"
        End Try
    End Sub


    Private Sub SelAuftrag(ByVal strAufID As String)
        Dim i2 As Integer
        Dim dataRows() As DataRow
        Dim dataRow As DataRow
        Dim NewRow As DataRow

        With clsUeberf
            .CleanClass()
            .SelAufID = strAufID
            dataRows = .TabUeberf.Select("AUF_ID='" & strAufID & "'")
            If dataRows.Length > 0 Then
                .TabUeberfSel = .TabUeberf.Clone
                i2 = 0
                For Each dataRow In dataRows
                    NewRow = .TabUeberfSel.NewRow
                    For i2 = 0 To .TabUeberf.Columns.Count - 1
                        NewRow(i2) = dataRow(i2)
                    Next
                    .TabUeberfSel.Rows.Add(NewRow)
                Next
                .TabUeberfSel.AcceptChanges()
            End If
            dataRows = .TabPartner.Select("AUF_ID='" & strAufID & "'")
            If dataRows.Length > 0 Then
                .TabPartnerSel = .TabPartner.Clone
                i2 = 0
                For Each dataRow In dataRows
                    NewRow = .TabPartnerSel.NewRow
                    For i2 = 0 To .TabPartner.Columns.Count - 1
                        NewRow(i2) = dataRow(i2)
                    Next
                    .TabPartnerSel.Rows.Add(NewRow)
                Next
            End If
            .TabPartnerSel.AcceptChanges()
        End With
    End Sub

    'Protected Sub cmdUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpload.Click
    '    Dim fname As String
    '    Dim fnameNew As String
    '    Dim path As String
    '    Dim Extension As String
    '    Dim File As HttpPostedFile
    '    lblError.Text = String.Empty

    '    If Not (upFile.PostedFile.FileName = String.Empty) Then

    '        'Dateigröße prüfen
    '        Try
    '            fname = upFile.PostedFile.FileName

    '            If (upFile.PostedFile.ContentLength > CType(ConfigurationManager.AppSettings("MaxUploadSizeUeberf"), Integer)) Then
    '                lblError.Text = "Datei '" & Right(fname, fname.Length - fname.LastIndexOf("\") - 1).ToUpper & "' ist zu gross (>200 KB)."
    '                Exit Sub
    '            End If
    '            ''------------------
    '            Extension = Right(upFile.PostedFile.FileName, 4)
    '            'If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".PDF" Then
    '            '    lblError.Text = "Es können nur Dateien im PDF-Format verarbeitet werden."
    '            '    Exit Sub
    '            'End If

    '            path = ConfigurationManager.AppSettings("UploadPathLocal") '"\\192.168.10.79\Datawizard\Upload\Ueberfuehrungen\"
    '            File = CType(upFile.PostedFile, System.Web.HttpPostedFile)
    '            fnameNew = Now.ToShortDateString & "_" & Replace(Now.ToShortTimeString, ":", "-") & "_" & m_User.KUNNR & "-" & m_User.UserName
    '            File.SaveAs(path & fnameNew & Extension)
    '            'http://192.168.10.194/datawizard/request/Ueberfuerung?Parameter_1=Start
    '            Dim params As String = "?Parameter_1=Start"

    '            Dim request As WebRequest = WebRequest.Create("http://192.168.10.194:80/dw/request/Ueberfuehrung?Parameter_1=Start") '
    '            request.Method = "POST"
    '            ' Create POST data and convert it to a byte array.

    '            'Dim datareader As New StreamReader(path & fnameNew & Extension)
    '            Dim postData As String = "This is a test that posts this string to a Web server."
    '            'datareader.BaseStream.ToString
    '            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)

    '            ' Set the ContentType property of the WebRequest.
    '            request.ContentType = ""
    '            ' Set the ContentLength property of the WebRequest.
    '            request.ContentLength = byteArray.Length

    '            ' Get the request stream.
    '            Dim dataStream As Stream = request.GetRequestStream()
    '            ' Write the data to the request stream.
    '            dataStream.Write(byteArray, 0, byteArray.Length)
    '            '' Close the Stream object.
    '            dataStream.Close()
    '            '' Get the response.
    '            Dim response As WebResponse = request.GetResponse()
    '            '' Display the status.
    '            Console.WriteLine(CType(Response, HttpWebResponse).StatusDescription)
    '            '' Get the stream containing content returned by the server.
    '            dataStream = Response.GetResponseStream()
    '            ' Open the stream using a StreamReader for easy access.
    '            Dim reader As New StreamReader(dataStream)
    '            ' Read the content.
    '            Dim responseFromServer As String = reader.ReadToEnd()
    '            ' Display the content.
    '            Console.WriteLine(responseFromServer)
    '            ' Clean up the streams.
    '            reader.Close()
    '            dataStream.Close()
    '            response.Close()

    '            txt_Protokoll.Text = "Datei erfolgreich bearbeitet" & vbCrLf
    '            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
    '            clsUeberf.FillData()
    '            FillGrid(0)
    '            Session("App_Ueberf") = clsUeberf
    '        Catch ex As Exception
    '            lblError.Text = ex.Message
    '        End Try


    '    Else
    '        lblError.Text = "Keine Datei ausgewählt."
    '    End If
    'End Sub
    Protected Sub cmdBack0_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack0.Click
        Session("App_UebTexte") = Nothing
        Session("App_Ueberf") = Nothing
        Response.Redirect("UploadUeberf.aspx?AppID=" & Session("AppID").ToString)
    End Sub


    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try

            Dim tblTemp As New DataTable()
            If Not Session("App_Excel") Is Nothing Then

                tblTemp = Session("App_Excel")
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
                excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
            Else
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        Dim strAufID As String = ""

        Session("App_PageIndex") = e.NewPageIndex
        Checkgrid()
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub


    '----------------------------------------------------------------------
    ' Methode: cmdUpload0_Click
    ' Autor: O.Rudolph
    ' Beschreibung: Upload einer Datei zum Lobster-Webserver zu Weiter-
    '               verarbeitung im Datawizard
    ' Erstellt am: 28.08.2008
    ' ITA: 2197
    '----------------------------------------------------------------------

    Protected Sub cmdUpload0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpload0.Click
        Dim fname As String
        Dim fnameNew As String
        Dim path As String
        Dim Extension As String
        Dim File As HttpPostedFile
        lblError.Text = String.Empty

        If Not (upFile.PostedFile.FileName = String.Empty) Then

            Try

                fname = upFile.PostedFile.FileName
                'Dateigröße prüfen
                If (upFile.PostedFile.ContentLength > CType(ConfigurationManager.AppSettings("MaxUploadSizeUeberf"), Integer)) Then
                    lblError.Text = "Datei '" & Right(fname, fname.Length - fname.LastIndexOf("\") - 1).ToUpper & "' ist zu gross (>200 KB)."
                    Exit Sub
                End If
                ''------------------
                Extension = Right(upFile.PostedFile.FileName, 4)
                path = ConfigurationManager.AppSettings("UploadPathLocal") '"\\192.168.10.79\Datawizard\Upload\Ueberfuehrungen\"
                File = CType(upFile.PostedFile, System.Web.HttpPostedFile)
                fnameNew = Now.ToShortDateString & "_" & Replace(Now.ToShortTimeString, ":", "-") & "_" & m_User.KUNNR & "-" & m_User.UserName
                File.SaveAs(path & fnameNew & Extension)
                Dim sUrl As String = ConfigurationManager.AppSettings("DataWizard")
                sUrl = sUrl & "Ueberfuehrung/" & Session("AppID").ToString & "_UploadUeberf/" & m_User.KUNNR & "?param1=value1&param2=" & m_User.UserName & ""
                Dim request As WebRequest = WebRequest.Create(sUrl)


                Dim stream As New FileStream(path & fnameNew & Extension, FileMode.Open)
                Dim reader As New StreamReader(File.InputStream)
                Dim lBytes As Long = stream.Length
                Dim byte1(lBytes - 1) As Byte
                'Lesen der Datei in ein Bytearray
                stream.Read(byte1, 0, lBytes)
                stream.Close()

                request.Method = WebRequestMethods.Http.Post
                request.ContentType = File.ContentType
                request.ContentLength = byte1.Length

                'Schreiben des Bytearray in den erwateten Stream des Zielservers
                Dim newStream As Stream = request.GetRequestStream()

                newStream.Write(byte1, 0, byte1.Length)

                newStream.Close()

                Dim response As WebResponse = request.GetResponse()
                Dim responseStream As Stream = response.GetResponseStream()

                Dim reader2 As New StreamReader(responseStream)
                '' Antwort lesen
                Dim responseFromServer As String = reader2.ReadToEnd()
                '' Streams schließen.
                reader.Close()
                response.Close()
                If responseFromServer = "Error" Then
                    lblError.Text = "Fehler beim verarbeiten Ihter Datei!"
                    txt_Protokoll.Text = "Fehler beim verarbeiten Ihter Datei!" & vbCrLf
                Else
                    txt_Protokoll.Text = "Datei erfolgreich bearbeitet" & vbCrLf
                    clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
                    clsUeberf.FillData()
                    FillGrid(0)
                    Session("App_Ueberf") = clsUeberf
                End If
                IO.File.Delete(path & fnameNew & Extension)
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try


        Else
            lblError.Text = "Keine Datei ausgewählt."
        End If
    End Sub

    Private Sub FillChecked()
        Dim Drow As DataRow
        Dim Nrow As DataRow
        Dim DataTblChecked As DataTable = Session("App_SelData")

        For Each Drow In DataTblChecked.Rows
            For Each Nrow In clsUeberf.TabUpload.Rows
                If Nrow("Auf_ID") = Drow("Auf_ID") Then
                    Nrow("Ok") = Drow("Ok")
                    Nrow("Del") = Drow("Del")
                    Nrow("NoSel") = Drow("NoSel")
                End If
            Next
        Next
        clsUeberf.TabUpload.AcceptChanges()
        Session("App_SelData") = DataTblChecked
    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        If RadioButtonList1.SelectedItem.Value = "Eigene" Then
            clsUeberf.selectedUser = "X"
            FillGrid(0)
        Else
            clsUeberf.selectedUser = ""
            FillGrid(0)
        End If
        Session("App_SelUser") = clsUeberf.selectedUser
    End Sub

    Protected Sub cmdNEW_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNEW.Click
        Checkgrid()

        Try
            If Session("App_Ueberf") Is Nothing Then
                clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
                Session("App_Ueberf") = clsUeberf
            Else
                Session("App_Ueberf") = clsUeberf
            End If
            clsUeberf.NewDataSet = True

            Response.Redirect("UploadEdit01.aspx?AppID=" & Session("AppID").ToString)
        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Daten in die Detailansicht"
        End Try

    End Sub
End Class
' ************************************************
' $History: UploadUeberf.aspx.vb $
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 27.11.08   Time: 13:27
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197 Nachbearbeitung
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 26.11.08   Time: 11:33
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 13.11.08   Time: 11:33
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 21.10.08   Time: 9:01
' Updated in $/CKAG/Applications/AppUeberf/Forms
' Bugfix leeres Datum abgefangen!OR
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 26.09.08   Time: 14:47
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 18.09.08   Time: 9:28
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 10.09.08   Time: 17:58
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 8.09.08    Time: 11:51
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 5.09.08    Time: 9:55
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 3.09.08    Time: 10:10
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2132
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 25.08.08   Time: 8:28
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' ************************************************
