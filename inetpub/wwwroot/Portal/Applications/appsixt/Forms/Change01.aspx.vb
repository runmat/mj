Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change01
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objAbmeldUnterlagen As Sixt_AbmeldUnterlagen

    Protected WithEvents ucStyles As Global.CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As Global.CKG.Portal.PageElements.Header

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
                lblCarportID.Text = m_User.Reference
                txtDemontagedatum.Text = Now.ToShortDateString
                txtAnzahlKennz.Text = "2"
                lblName.Text = m_User.FirstName & " " & m_User.LastName
                SetFocus(txtKennz)
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.SelectedIndex = 1
                Session("objAbmeldUnterlagen") = Nothing
            Else
                objAbmeldUnterlagen = CType(Session("objAbmeldUnterlagen"), Sixt_AbmeldUnterlagen)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lb_Weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Weiter.Click
        lblError.Text = ""
        If ProofInput() Then
            cmdSend.Visible = False
            If objAbmeldUnterlagen Is Nothing Then
                objAbmeldUnterlagen = New Sixt_AbmeldUnterlagen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                With objAbmeldUnterlagen
                    .CreatePosTable()

                    Dim DataNewRow As DataRow
                    DataNewRow = .Positionen.NewRow
                    DataNewRow("KUNNR_AG") = Right("0000000000" & m_User.KUNNR, 10)
                    DataNewRow("LSNUMMER") = ""
                    DataNewRow("CARPORT_ID") = ""
                    DataNewRow("LICENSE_NUM") = txtKennz.Text
                    DataNewRow("CHASSIS_NUM") = ""
                    DataNewRow("ZZFABRIKNAME") = ""
                    DataNewRow("ZZB1_IN_VERW_DAD") = ""
                    DataNewRow("DAT_DEMONT") = txtDemontagedatum.Text
                    DataNewRow("ANZ_KENNZ_CPL") = txtAnzahlKennz.Text.Trim
                    DataNewRow("VORLAGE_ZB1_CPL") = rblZBI.SelectedValue
                    DataNewRow("VORLAGE_ZB1_CPL_Text") = rblZBI.SelectedItem.Text
                    DataNewRow("WEB_USER") = m_User.UserName
                    .Positionen.Rows.Add(DataNewRow)
                End With

                FillGrid(0)
                txtKennz.Text = ""
                SetFocus(txtKennz)
                txtDemontagedatum.Enabled = False
                rblZBI.SelectedItem.Value = 2
                Session("objAbmeldUnterlagen") = objAbmeldUnterlagen
            Else
                With objAbmeldUnterlagen
                    If .Positionen.Select("LICENSE_NUM='" & txtKennz.Text.Trim & "'").Length = 0 Then
                        Dim DataNewRow As DataRow
                        DataNewRow = .Positionen.NewRow
                        DataNewRow("KUNNR_AG") = Right("0000000000" & m_User.KUNNR, 10)
                        DataNewRow("LSNUMMER") = ""
                        DataNewRow("CARPORT_ID") = ""
                        DataNewRow("LICENSE_NUM") = txtKennz.Text.Trim
                        DataNewRow("CHASSIS_NUM") = ""
                        DataNewRow("ZZFABRIKNAME") = ""
                        DataNewRow("ZZB1_IN_VERW_DAD") = ""
                        DataNewRow("DAT_DEMONT") = txtDemontagedatum.Text
                        DataNewRow("ANZ_KENNZ_CPL") = txtAnzahlKennz.Text
                        DataNewRow("VORLAGE_ZB1_CPL") = rblZBI.SelectedValue
                        DataNewRow("VORLAGE_ZB1_CPL_Text") = rblZBI.SelectedItem.Text
                        DataNewRow("WEB_USER") = m_User.UserName

                        .Positionen.Rows.Add(DataNewRow)
                    Else
                        lblError.Text = "Kennzeichen bereits erfasst!"
                        Exit Sub
                    End If

                End With
                FillGrid(0)
                txtKennz.Text = ""
                rblZBI.SelectedItem.Value = 2
                txtDemontagedatum.Enabled = False
                Session("objAbmeldUnterlagen") = objAbmeldUnterlagen
                SetFocus(txtKennz)
            End If

        End If


    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objAbmeldUnterlagen.Positionen.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            tblResult.Visible = False
        Else
            ddlPageSize.Visible = True
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            GridView1.Visible = True
            cmdGetData.Visible = True
            tblResult.Visible = True
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

            GridView1.PageIndex = intTempPageIndex
            GridView1.PagerStyle.CssClass = "PagerStyle"
            GridView1.DataSource = tmpDataView
            GridView1.DataBind()

            If GridView1.PageCount > 1 Then
                GridView1.PagerSettings.Visible = True
            Else
                GridView1.PagerSettings.Visible = False
            End If
        End If
    End Sub

    Private Function ProofInput() As Boolean
        If txtKennz.Text.Length = 0 Then
            lblError.Text = "Bitte füllen Sie alle Pflichfelder!"
            Return False
        ElseIf txtAnzahlKennz.Text.Length = 0 Then
            lblError.Text = "Bitte füllen Sie alle Pflichfelder!"
            Return False
        ElseIf CDate(txtDemontagedatum.Text) > Now.ToShortDateString Then
            lblError.Text = "Das Demontagedatumdarf nicht in der Zukunft liegen!"
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub DoSubmit()

        lblError.Text = ""
        objAbmeldUnterlagen.GetData(Session("AppID").ToString, Session.SessionID.ToString, Me)

        Session("objAbmeldUnterlagen") = objAbmeldUnterlagen

        If Not objAbmeldUnterlagen.Status = 0 Then
            lblError.Text = "Fehler: " & objAbmeldUnterlagen.Message
        Else
            If objAbmeldUnterlagen.Positionen.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                If objAbmeldUnterlagen.Positionen.Select("CHASSIS_NUM = 'unbekannt - prüfen'").Length > 0 Then
                    lblError.Text = "Achtung:  Es liegen unbekannte Fahrzeugpositionen vor!"
                    cmdSend.Visible = False
                Else
                    cmdSend.Visible = True
                End If
                FillGrid(0)
            End If
        End If

    End Sub

    Private Sub DoSubmit2()

        lblError.Text = ""
        objAbmeldUnterlagen.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)

        Session("objAbmeldUnterlagen") = objAbmeldUnterlagen

        If Not objAbmeldUnterlagen.Status = 0 Then
            lblError.Text = "Fehler: " & objAbmeldUnterlagen.Message
        Else
            If objAbmeldUnterlagen.Positionen.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                If objAbmeldUnterlagen.Positionen.Select("LSNUMMER = ''").Length <> 0 Then
                    lblError.Text = "Achtung:  Fehler beim Senden der Daten!(Keine Lieferscheinnr. erzeugt))"
                Else
                    lblNoData.Visible = True
                    cmdSend.Visible = False
                    lblNoData.Text = "Daten erfolgreich gesendet!"
                    lblNoData.ForeColor = Color.MediumSeaGreen
                    trPrint.Visible = True
                    cmdEdit.Visible = False
                    cmdNewList.Visible = True
                End If

            End If
        End If

    End Sub
    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        lb_Weiter_Click(sender, e)
    End Sub

    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        lblError.Text = ""
        Dim Datakey As String = GridView1.DataKeys(e.RowIndex).Value.ToString()
        With objAbmeldUnterlagen
            If .Positionen.Select("LICENSE_NUM='" & Datakey.Trim & "'").Length = 1 Then
                Dim DataRowPos As DataRow
                DataRowPos = .Positionen.Select("LICENSE_NUM='" & Datakey.Trim & "'")(0)
                .Positionen.Rows.Remove(DataRowPos)
                .Positionen.AcceptChanges()
                If .Positionen.Rows.Count = 0 Then txtDemontagedatum.Enabled = True
                Session("objAbmeldUnterlagen") = objAbmeldUnterlagen
                FillGrid(GridView1.PageIndex)
            End If
        End With

        'Prüfen. ob der Absenden-Button wieder eingeblendet werden kann.
        If objAbmeldUnterlagen.Positionen.Rows.Count > 0 Then 'Gibt es noch Positionen?
            If objAbmeldUnterlagen.Positionen.Select("CHASSIS_NUM = 'unbekannt - prüfen'").Length = 0 Then 'Sind noch Positionen unbekannt?

                If objAbmeldUnterlagen.Positionen.Select("CHASSIS_NUM = ''").Length = 0 Then cmdSend.Visible = True

            End If
        End If


        


    End Sub

    Protected Sub cmdGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdGetData.Click
        DoSubmit()
    End Sub

    Protected Sub cmdSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSend.Click

        If tbl_Query.Visible = False Then
            DoSubmit2()

        Else
            lblNoData.Text = "Übersicht Datenerfassung"
            lblNoData.Visible = True
            tbl_Query.Visible = False
            cmdEdit.Visible = True
            cmdGetData.Visible = False
        End If

    End Sub
    Private Function CreateExcelTable() As DataTable

        Dim tblExcel As New DataTable
        With tblExcel
            .Columns.Add("Kennzeichen", GetType(System.String))
            .Columns.Add("Fahrgestellnummer", GetType(System.String))
            .Columns.Add("Hersteller", GetType(System.String))
            .Columns.Add("Demontagedatum", GetType(System.String))
            .Columns.Add("Vorlage ZBI", GetType(System.String))
            .Columns.Add("Anzahl Kennzeichen", GetType(System.String))
            .Columns.Add("Web User", GetType(System.String))
            .Columns.Add("Carport ID", GetType(System.String))
            .Columns.Add("Erfassungsdatum", GetType(System.String))
        End With

        Return tblExcel

    End Function
    Protected Sub cmdEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEdit.Click
        tbl_Query.Visible = True
        cmdEdit.Visible = False
        cmdGetData.Visible = True
    End Sub

    Protected Sub lnkCreatePDF1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreatePDF1.Click
        Try
            Dim tblExcel As DataTable = CreateExcelTable()
            Dim sLiefNummer As String = ""
            Dim excelRow As DataRow
            tblExcel.TableName = "Lieferschein"
            tblExcel.Columns.Add("Nr", GetType(System.String))
            Dim tmpTabelle As DataTable = HelpProcedures.DataTableAlphabeticSort(objAbmeldUnterlagen.Positionen, "LICENSE_NUM", 0)
            Dim i As Integer = 1
            For Each drow As DataRow In tmpTabelle.Rows
                excelRow = tblExcel.NewRow
                excelRow("Nr") = i
                excelRow("Kennzeichen") = drow("LICENSE_NUM")
                excelRow("Fahrgestellnummer") = drow("CHASSIS_NUM")
                excelRow("Hersteller") = drow("ZZFABRIKNAME")
                excelRow("Demontagedatum") = CDate(drow("DAT_DEMONT")).ToShortDateString
                excelRow("Vorlage ZBI") = drow("VORLAGE_ZB1_CPL_Text")
                excelRow("Anzahl Kennzeichen") = drow("ANZ_KENNZ_CPL")
                excelRow("Web User") = drow("WEB_USER")
                excelRow("Carport ID") = drow("CARPORT_ID")
                excelRow("Erfassungsdatum") = Now.ToShortDateString
                tblExcel.Rows.Add(excelRow)
                i += 1
            Next

            Dim headTable As New DataTable("Kopf")
            headTable.Columns.Add("CarportID", GetType(System.String))
            headTable.Columns.Add("Name1", GetType(System.String))
            headTable.Columns.Add("Name2", GetType(System.String))
            headTable.Columns.Add("LieferscheinNummer", GetType(System.String))

            Dim tmpSAPRow As DataRow
            For Each drow As DataRow In objAbmeldUnterlagen.Positionen.Rows
                tmpSAPRow = headTable.NewRow
                tmpSAPRow("CarportID") = drow("CARPORT_ID")
                tmpSAPRow("Name1") = m_User.LastName
                tmpSAPRow("Name2") = m_User.FirstName
                tmpSAPRow("LieferscheinNummer") = drow("LSNUMMER")
                sLiefNummer = drow("LSNUMMER").ToString.Trim
                headTable.Rows.Add(tmpSAPRow)
                Exit For
            Next

            Dim ms As New IO.MemoryStream
            ms = HelpProcedures.CreateBarcode(sLiefNummer)
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", m_User.Customer.LogoImage)
            imageHt.Add("Logo2", m_User.Customer.LogoImage2)
            imageHt.Add("Logo3", ms)

            Dim docFactory As New DocumentGeneration.WordDocumentFactory(tblExcel, imageHt)
            docFactory.CreateDocumentTable("Lieferschein", Me.Page, "\Applications\AppSixt\Documents\Bestellung.doc", headTable)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try


    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim tblExcel As DataTable = CreateExcelTable()

        Dim excelRow As DataRow

        Dim tmpTabelle As DataTable = HelpProcedures.DataTableAlphabeticSort(objAbmeldUnterlagen.Positionen, "LICENSE_NUM", 0)

        For Each drow As DataRow In tmpTabelle.Rows
            excelRow = tblExcel.NewRow
            excelRow("Kennzeichen") = drow("LICENSE_NUM")
            excelRow("Fahrgestellnummer") = drow("CHASSIS_NUM")
            excelRow("Hersteller") = drow("ZZFABRIKNAME")
            excelRow("Demontagedatum") = CDate(drow("DAT_DEMONT")).ToShortDateString
            excelRow("Vorlage ZBI") = drow("VORLAGE_ZB1_CPL_Text")
            excelRow("Anzahl Kennzeichen") = drow("ANZ_KENNZ_CPL")
            excelRow("Web User") = drow("WEB_USER")
            excelRow("Carport ID") = drow("CARPORT_ID")
            excelRow("Erfassungsdatum") = Now.ToShortDateString
            tblExcel.Rows.Add(excelRow)
        Next

        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub cmdNewList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNewList.Click
        tbl_Query.Visible = True
        trPrint.Visible = False
        cmdEdit.Visible = False
        cmdGetData.Visible = False
        cmdNewList.Visible = False
        lblNoData.Text = ""
        lblError.Text = ""
        GridView1.DataSource = Nothing
        GridView1.DataBind()
        tblResult.Visible = False
        objAbmeldUnterlagen = Nothing
        txtDemontagedatum.Enabled = True
        Session("objAbmeldUnterlagen") = Nothing
    End Sub

    Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
        GridView1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub
End Class