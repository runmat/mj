Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Partial Public Class Report10
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)



        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


            If Not IsPostBack Then
                FillPageSize()
                FillSpediteur()
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        If (checkInput()) Then
            DoSubmit()
        End If
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Rechnungsdatenreport(m_User, m_App, "")

            lblError.Text = ""

            If checkDate() Then
                Dim strRechnungsnummer As String
                Dim strRechnungsnummerBis As String
                Dim strFahrgestellnummer As String
                Dim strAusgabeart As String
                Dim strRechnungsdatumVon As String
                Dim strRechnungsdatumBis As String
                Dim strLeistungsdatumVon As String
                Dim strLeistungsdatumBis As String
                Dim strLeistungscodeVon As String
                Dim strLeistungscodeBis As String
                Dim strAbgearbeitet As String
                Dim strGrunddatenvorlage As String
                Dim strLeistungsart As String
                Dim strSpediteur As String

                If txtRechnNr.Text.Length = 0 Then
                    strRechnungsnummer = ""
                Else
                    txtRechnNr.Text = Trim(txtRechnNr.Text)
                    strRechnungsnummer = txtRechnNr.Text
                End If

                If txtRechnNrBis.Text.Length = 0 Then
                    strRechnungsnummerBis = ""
                Else
                    txtRechnNrBis.Text = Trim(txtRechnNrBis.Text)
                    strRechnungsnummerBis = txtRechnNrBis.Text
                End If


                If txtFahrgestNr.Text.Length = 0 Then
                    strFahrgestellnummer = ""
                    strAusgabeart = ""
                Else
                    txtFahrgestNr.Text = Trim(txtFahrgestNr.Text)
                    strFahrgestellnummer = txtFahrgestNr.Text
                    'Ausgabeart nur relevant, wenn Fahrgestellnr. angegeben
                    'If cbRechnungsausgabe.Checked Then
                    '    strAusgabeart = "R"
                    'Else
                    '    strAusgabeart = "F"
                    'End If
                End If
                If txtRechnDatVon.Text.Length = 0 Then
                    strRechnungsdatumVon = ""
                Else
                    strRechnungsdatumVon = txtRechnDatVon.Text
                End If
                If txtRechnDatBis.Text.Length = 0 Then
                    strRechnungsdatumBis = ""
                Else
                    strRechnungsdatumBis = txtRechnDatBis.Text
                End If
                If txtLeistDatVon.Text.Length = 0 Then
                    strLeistungsdatumVon = ""
                Else
                    strLeistungsdatumVon = txtLeistDatVon.Text
                End If
                If txtLeistDatBis.Text.Length = 0 Then
                    strLeistungsdatumBis = ""
                Else
                    strLeistungsdatumBis = txtLeistDatBis.Text
                End If

                If txtLeistungscodeVon.Text.Length = 0 Then
                    strLeistungscodeVon = ""
                Else
                    strLeistungscodeVon = Trim(txtLeistungscodeVon.Text)
                End If
                If txtLeistungscodeBis.Text.Length = 0 Then
                    strLeistungscodeBis = ""
                Else
                    strLeistungscodeBis = Trim(txtLeistungscodeBis.Text)
                End If

                If txtLeistArt.Text.Length = 0 Then
                    strLeistungsart = ""
                Else
                    txtLeistArt.Text = Trim(txtLeistArt.Text)
                    strLeistungsart = txtLeistArt.Text
                End If

                If drpSpediteur.SelectedItem.Text = "Alle" Then
                    strSpediteur = ""
                Else
                    strSpediteur = drpSpediteur.SelectedItem.Text
                End If


                strAbgearbeitet = rblAbgearbeitet.SelectedItem.Value

                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me, strRechnungsnummer, strRechnungsnummerBis,
                              strFahrgestellnummer, strAusgabeart, strRechnungsdatumVon, strRechnungsdatumBis, strLeistungsdatumVon,
                              strLeistungsdatumBis, strLeistungscodeVon, strLeistungscodeBis, strAbgearbeitet, strGrunddatenvorlage, strSpediteur)

                Session("ResultTable") = m_Report.Result
                Session("ExcelTable") = m_Report.ExcelResult
                Session("SapTableKum") = m_Report.SapTableKum
                Session("SapTable") = m_Report.SapTable

                If Not m_Report.Status = 0 Then
                    lblError.Text = m_Report.Message
                ElseIf m_Report.SapTableKum.Rows.Count < 1 Then
                    lblError.Text = "Es konnten keine Rechnungsdaten ermittelt werden."
                Else
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Rechnungsdatenreport")
                    FillGridRechKum(0)
                    'Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
            
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGridRechKum(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tblAusgabe As DataTable = CType(Session("SapTableKum"), DataTable)

        If tblAusgabe.Rows.Count = 0 Then
            tblKum.Visible = False
            dgKum.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."

        Else
            tblKum.Visible = True
            dgKum.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = tblAusgabe.DefaultView

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

            dgKum.CurrentPageIndex = intTempPageIndex

            dgKum.DataSource = tmpDataView
            dgKum.DataBind()

            If dgKum.PageCount > 1 Then
                dgKum.PagerStyle.CssClass = "PagerStyle"
                dgKum.DataBind()
                dgKum.PagerStyle.Visible = True
            Else
                dgKum.PagerStyle.Visible = False
            End If

            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Rechnungen gefunden."
            lblNoData.Visible = True

        End If




    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tblAusgabe As DataTable = CType(Session("ResultTable"), DataTable)

        If tblAusgabe.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoDataDetail.Visible = True
            lblNoDataDetail.Text = "Keine Daten zur Anzeige gefunden."

        Else
            DataGrid1.Visible = True
            lblNoDataDetail.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = tblAusgabe.DefaultView



            If String.IsNullOrEmpty(Session("FilterDetail").ToString()) = False Then
                tmpDataView.RowFilter = "Rechnungsnummer = '" & Session("FilterDetail").ToString() & "'"
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

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoDataDetail.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoDataDetail.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
            End If

            lblNoDataDetail.Visible = True

            If DataGrid1.PageCount > 1 Then
                'DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub


    Private Function checkDate() As Boolean
        Dim tmpbool As Boolean = True
        For Each ctrl As BaseValidator In Me.Validators
            If ctrl.IsValid = False Then
                tmpbool = False
            End If
        Next
        Return tmpbool
    End Function

    Private Function checkInput() As Boolean
        Dim DateVon As Date
        Dim DateBis As Date

        If (txtRechnNr.Text.Length + txtFahrgestNr.Text.Length + txtRechnDatVon.Text.Length + txtRechnDatBis.Text.Length + txtLeistDatVon.Text.Length + _
            txtLeistDatBis.Text.Length + txtLeistArt.Text.Length = 0 + txtLeistungscodeVon.Text.Length + txtLeistungscodeBis.Text.Length) AndAlso (drpSpediteur.SelectedItem.Text = "Alle") Then
            lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
            Return False
        End If

        'wenn nicht Filter nach Rechn-/Fahrgest-Nr, mind. eine Datumseingrenzung erforderlich
        If txtRechnNr.Text.Length = 0 AndAlso txtFahrgestNr.Text.Length = 0 Then

            If txtRechnDatVon.Text.Length + txtRechnDatBis.Text.Length + txtLeistDatVon.Text.Length + txtLeistDatBis.Text.Length = 0 AndAlso (txtLeistungscodeVon.Text.Length + txtLeistungscodeBis.Text.Length = 0) Then


                If txtRechnDatVon.Text.Length + txtRechnDatBis.Text.Length + txtLeistDatVon.Text.Length + txtLeistDatBis.Text.Length = 0 Then
                    lblError.Text = "Es muss mindestens eine der beiden möglichen Datumseingrenzungen(von-bis) oder die Eingabe des Leistungscodes(von-bis)erfolgen."
                    Return False
                End If
            End If

            
        End If

        If IsDate(txtRechnDatVon.Text) AndAlso IsDate(txtRechnDatBis.Text) Then
            DateVon = CType(txtRechnDatVon.Text, Date)
            DateBis = CType(txtRechnDatBis.Text, Date)
            If DateDiff(DateInterval.Day, DateVon, DateBis) > 90 Then
                lblError.Text = "Ausgewählter Zeitraum des Rechnungsdatums zu groß. Maximal 3 Monate."
                Return False
            End If
        ElseIf IsDate(txtRechnDatVon.Text) AndAlso Not IsDate(txtRechnDatBis.Text) Then
            lblError.Text = "Rechnungsdatum bis muss gefüllt sein!"
            Return False
        ElseIf Not IsDate(txtRechnDatVon.Text) AndAlso IsDate(txtRechnDatBis.Text) Then
            lblError.Text = "Rechnungsdatum von muss gefüllt sein!"
            Return False
        End If
        If IsDate(txtLeistDatVon.Text) AndAlso IsDate(txtLeistDatBis.Text) Then
            DateVon = CType(txtLeistDatVon.Text, Date)
            DateBis = CType(txtLeistDatBis.Text, Date)
            If DateDiff(DateInterval.Day, DateVon, DateBis) > 90 Then
                lblError.Text = "Ausgewählter Zeitraum des Leistungsdatums zu groß. Maximal 3 Monate."
                Return False
            End If
        ElseIf IsDate(txtLeistDatVon.Text) AndAlso Not IsDate(txtLeistDatBis.Text) Then
            lblError.Text = "Leistungsdatum bis muss gefüllt sein!"
            Return False
        ElseIf Not IsDate(txtLeistDatVon.Text) AndAlso IsDate(txtLeistDatBis.Text) Then
            lblError.Text = "Leistungsdatum von muss gefüllt sein!"
            Return False
        End If

        If txtLeistungscodeVon.Text.Length + txtLeistungscodeBis.Text.Length > 0 Then
            If txtLeistungscodeVon.Text.Length = 0 Then
                lblError.Text = "Leistungscode von muss gefüllt sein!"
                Return False
            ElseIf txtLeistungscodeVon.Text.Length = 0 Then
                lblError.Text = "Leistungscode bis muss gefüllt sein!"
                Return False
            End If
        End If


        Return True
    End Function

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Session("ResultTable") = Nothing
        Session("ExcelTable") = Nothing
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub dgKum_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgKum.PageIndexChanged
        FillGridRechKum(e.NewPageIndex)
    End Sub

    Private Sub dgKum_SortCommand(source As Object, e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgKum.SortCommand
        FillGridRechKum(dgKum.CurrentPageIndex, e.SortExpression)
    End Sub
    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        dgKum.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGridRechKum(0)
    End Sub

    Protected Sub dgKum_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgKum.ItemCommand
        If e.CommandName = "Rechnr" Then
            Session("FilterDetail") = CType(e.CommandSource, LinkButton).Text
            FillGrid(0)
            SetVisibility()
        End If
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        UpdateTables()
        Dim m_Report As New Rechnungsdatenreport(m_User, m_App, "")
        m_Report.Save(Session("AppID").ToString, Session.SessionID.ToString, CType(Session("SapTableKum"), DataTable), CType(Session("SapTable"), DataTable))

        If m_Report.Status = 0 Then
            lblSuccess.Visible = True
        Else
            lblSaveError.Text = m_Report.Message
        End If
    End Sub

    Private Sub UpdateTables()

        Dim lb As LinkButton
        Dim cbx As CheckBox

        'Dim SapTable As DataTable = CType(Session("SapTable"), DataTable)
        Dim SapTableKum As DataTable = CType(Session("SapTableKum"), DataTable)
        Dim userName As String = m_User.UserName

        If userName.Length > 30 Then
            userName = Left(userName, 30)
        End If


        'DataGridzeilen durchlaufen.
        For Each item As DataGridItem In dgKum.Items

            For Each cell As TableCell In item.Cells

                For Each c As Control In cell.Controls
                    If TypeOf c Is CheckBox Then

                        If CType(c, CheckBox).ID = "cbxVerarbeitet" Then
                            cbx = CType(c, CheckBox)
                        End If
                    ElseIf TypeOf c Is LinkButton Then
                        If CType(c, LinkButton).ID = "lbRechnungsnummer" Then
                            lb = CType(c, LinkButton)
                        End If

                    End If

                Next

            Next

            'Daten in der gruppierten Tabelle aktualisieren
            SapTableKum.Select("RECH_NR ='" & lb.Text & "'")(0)("ABGEARB_FLAG") = IIf(cbx.Checked, "X", "")
            SapTableKum.Select("RECH_NR ='" & lb.Text & "'")(0)("ABGEARB_USER") = IIf(cbx.Checked, userName, "")

            'For Each nRow As DataRow In SapTable.Rows

            '    If nRow("RECH_NR").ToString() = lb.Text Then
            '        nRow("ABGEARB_FLAG") = IIf(cbx.Checked, "X", "")
            '        nRow("ABGEARB_USER") = IIf(cbx.Checked, userName, "")
            '    End If


            'Next
        Next


        'Session("SapTable") = SapTable
        Session("SapTableKum") = SapTableKum


    End Sub

    Private Sub FillPageSize()

        ddlPageSize.Items.Add("10")
        ddlPageSize.Items.Add("20")
        ddlPageSize.Items.Add("50")
        ddlPageSize.Items.Add("100")
        ddlPageSize.Items.Add("200")
        ddlPageSize.Items.Add("500")
        ddlPageSize.Items.Add("1000")
        ddlPageSize.SelectedIndex = 2

        ddlPageSizeDetail.Items.Add("10")
        ddlPageSizeDetail.Items.Add("20")
        ddlPageSizeDetail.Items.Add("50")
        ddlPageSizeDetail.Items.Add("100")
        ddlPageSizeDetail.Items.Add("200")
        ddlPageSizeDetail.Items.Add("500")
        ddlPageSizeDetail.Items.Add("1000")
        ddlPageSizeDetail.SelectedIndex = 2


    End Sub

    Private Sub FillSpediteur()
        Dim m_Report As New Rechnungsdatenreport(m_User, m_App, "")

        m_Report.FillSpediteur(Session("AppID").ToString, Session.SessionID.ToString, Me)

        If m_Report.TableSpediteur.Rows.Count > 0 Then

            Dim lItem As ListItem

            For i = 0 To m_Report.TableSpediteur.Rows.Count - 1
                lItem = New ListItem()

                lItem.Value = (i + 1).ToString()
                lItem.Text = m_Report.TableSpediteur.Rows(i)("POS_KURZTEXT").ToString()


                drpSpediteur.Items.Add(lItem)

            Next

        Else
            lblError.Text = "Zur Auswahl des Spediteurs konnten leider keine Daten ermittelt werden."
        End If


    End Sub
   
 Protected Sub DataGrid1_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Protected Sub DataGrid1_SortCommand(source As Object, e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Protected Sub ddlPageSizeDetail_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPageSizeDetail.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSizeDetail.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        tblDetail.Visible = False
        tblKum.Visible = True
        tblSelection.Visible = True
        cmdCreate.Visible = True
        cmdBack.Visible = True
        lbBack.Visible = False
    End Sub

 
    Protected Sub cmdShowAll_Click(sender As Object, e As EventArgs) Handles cmdShowAll.Click
        Session("FilterDetail") = ""
        FillGrid(0)
        SetVisibility()
    End Sub

    Private Sub SetVisibility()
        tblDetail.Visible = True
        tblKum.Visible = False
        tblSelection.Visible = False
        cmdCreate.Visible = False
        cmdBack.Visible = False
        lbBack.Visible = True
    End Sub


    Protected Sub lnkCreateExcel_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles lnkCreateExcel.Click
        If Not (Session("ExcelTable") Is Nothing) Then
            Dim excelTable = CType(Session("ExcelTable"), DataTable)

            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, excelTable, Me.Page, False, Nothing, 0, 0)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub
End Class