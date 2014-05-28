Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Partial Public Class Change03s
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private objKlaerfaelle As Klaerfaelle
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(GridView1)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New App(m_User)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)
            If Not IsPostBack Then
                rbAktion.SelectedValue = "1"
            Else
                objKlaerfaelle = CType(Session("App_objKlaerfaelle"), Klaerfaelle)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        txtKlaertext.Attributes.Add("style", "overflow :hidden")
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        DoSubmit()
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(0, e.SortExpression)
    End Sub

    Protected Sub lb_Weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Weiter.Click

        lblError.Text = ""

        If tr_Kunde.Visible = True Then
            If CheckKunde() = True Then
                lblError.Text = "Bitte wählen Sie einen Kunden aus."
                Exit Sub
            End If
        End If

        objKlaerfaelle.Modus = "S"
        lblNoData.Text = ""

        If txtKlaertext.Text.Length > 359 Then
            lblError.Text = "Der Klärfalltext darf nicht mehr als 360 Zeichen enthalten."
            Exit Sub
        End If

        Dim dataRow() As DataRow = objKlaerfaelle.ResultPDIs.Select("LICENSE_NUM = '" & lblKennzeichen.Text & "'")
        If dataRow.Length = 1 Then
            If txtKlaertext.Text <> dataRow(0)("TEXT_KLAERF_CARP").ToString Then
                objKlaerfaelle.Klaerfalltext = txtKlaertext.Text
                objKlaerfaelle.SelectedKennz = lblKennzeichen.Text
                objKlaerfaelle.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)

                If Not objKlaerfaelle.Status = 0 Then
                    lblError.Text = objKlaerfaelle.E_MESSAGE
                    Result.Visible = False
                Else
                    objKlaerfaelle.Modus = Me.rbAktion.SelectedItem.Value
                    objKlaerfaelle.FILL(Session("AppID").ToString, Session.SessionID.ToString, Me)

                    If Not objKlaerfaelle.Status = 0 Then
                        lblError.Text = objKlaerfaelle.E_MESSAGE
                        Result.Visible = False
                    Else
                        FillGrid(0)
                        btnConfirm.Visible = True
                        tbl_Query.Visible = False
                        lb_Weiter.Visible = False
                        lb_Cancel.Visible = False
                        lblNoData.ForeColor = Drawing.Color.LimeGreen
                        lblNoData.Visible = True
                        lblNoData.Text = "Daten erfolgreich gespeichert!"
                    End If
                End If
            Else
                    lblError.Text = "Keine Änderungen vorgenommen!"
                End If
        End If
    End Sub

    Protected Sub lb_Cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Cancel.Click
        FillGrid(0)

        btnConfirm.Visible = True
        Result.Visible = True
        tbl_Query.Visible = False
        lb_Weiter.Visible = False
        lb_Cancel.Visible = False

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim tblExcel As DataTable = CreateExcelTable()

        Dim excelRow As DataRow

        Dim tmpTabelle As DataTable = HelpProcedures.DataTableAlphabeticSort(objKlaerfaelle.ResultPDIs, "LICENSE_NUM", 0)

        For Each drow As DataRow In tmpTabelle.Rows
            excelRow = tblExcel.NewRow
            excelRow("Carport ID") = drow("CARPORT_ID")
            excelRow("Lieferscheinnummer") = drow("LIEFNR")
            excelRow("Kennzeichen") = drow("LICENSE_NUM")
            excelRow("Fahrgestellnummer") = drow("CHASSIS_NUM")
            If IsDate(drow("DAT_IMP").ToString) Then excelRow("Meldungsdatum") = CDate(drow("DAT_IMP")).ToShortDateString
            If IsDate(drow("DAT_DEMONT").ToString) Then excelRow("Demontagedatum") = CDate(drow("DAT_DEMONT")).ToShortDateString
            excelRow("Anzahl Kennzeichen") = drow("ANZ_KENNZ_CPL")
            Select Case drow("VORLAGE_ZB1_CPL").ToString
                Case "0"
                    excelRow("Vorlage ZBI") = "Nein"
                Case "1"
                    excelRow("Vorlage ZBI") = "Ja"
                Case "2"
                    excelRow("Vorlage ZBI") = "Kopie"
            End Select
            If IsDate(drow("DAT_ANLAGE_ABW").ToString) Then excelRow("Anlage Abweichung") = CDate(drow("DAT_ANLAGE_ABW")).ToShortDateString
            excelRow("Klärfalltext DAD") = drow("TEXT_KLAERF_DAD")
            If IsDate(drow("BEARB_KLAER_CARP").ToString) Then excelRow("Bearbeitung durch Carport am") = CDate(drow("BEARB_KLAER_CARP")).ToShortDateString
            excelRow("Klärfalltext Carport") = drow("TEXT_KLAERF_CARP")
            If IsDate(drow("ABSCHL_KLAER_DAD").ToString) Then excelRow("abgeschlossen durch DAD am") = CDate(drow("ABSCHL_KLAER_DAD")).ToShortDateString
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

    Protected Sub rbAktion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbAktion.SelectedIndexChanged
        If rbAktion.SelectedValue = "E" Then
            trKennzeichen.Visible = True
            trFahrgestellnummer.Visible = True
            trLieferscheinnummer.Visible = True
        Else
            trKennzeichen.Visible = False
            trFahrgestellnummer.Visible = False
            trLieferscheinnummer.Visible = False
            txtFahrgestellnummer.Text = ""
            txtKennz.Text = ""
            txtLieferscheinnummer.Text = ""
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        If Not IsPostBack Then
            If tr_Kunde.Visible = True Then FillKunden()
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    Private Sub DoSubmit()
        lblError.Text = ""

        If tr_Kunde.Visible = True Then
            If CheckKunde() = True Then
                lblError.Text = "Bitte wählen Sie einen Kunden aus."
                Exit Sub
            End If
        End If

        If objKlaerfaelle Is Nothing Then
            objKlaerfaelle = New Klaerfaelle(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        End If

        If Me.rbAktion.SelectedItem.Value = "E" Then
            If txtFahrgestellnummer.Text.Trim.Length + txtKennz.Text.Trim.Length + txtLieferscheinnummer.Text.Trim.Length = 0 Then
                lblError.Text = "Bitte geben Sie ein Suchkriterium ein!"
                Exit Sub
            End If
            objKlaerfaelle.Fahrgestnr = txtFahrgestellnummer.Text.Trim
            objKlaerfaelle.Kennz = txtKennz.Text.Trim.ToUpper
            objKlaerfaelle.Liefernr = txtLieferscheinnummer.Text.Trim
        End If

        objKlaerfaelle.Modus = Me.rbAktion.SelectedItem.Value
        objKlaerfaelle.CarportID = m_User.Reference
        objKlaerfaelle.FILL(Session("AppID").ToString, Session.SessionID.ToString, Me)

        If Not objKlaerfaelle.Status = 0 Then
            lblError.Text = objKlaerfaelle.E_MESSAGE
            Result.Visible = False
        Else
            FillGrid(0)
        End If

        Session("App_objKlaerfaelle") = objKlaerfaelle

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal strFilter As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objKlaerfaelle.ResultPDIs.DefaultView
        tmpDataView.RowFilter = strFilter

        If tmpDataView.Count = 0 Then
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lblError.Visible = True
            Result.Visible = False
        Else

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            lblNoData.Text = ""
            lblNoData.Visible = False

            Result.Visible = True
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

    Private Sub AddMaxLength()

        Dim lengthFunction As String = "function isMaxLength(txtBox) {"
        lengthFunction += " if(txtBox) { "
        lengthFunction += "     return ( txtBox.value.length <=" & 359 & ");"
        lengthFunction += " }"
        lengthFunction += "}"

        Me.txtKlaertext.Attributes.Add("onkeypress", "return isMaxLength(this);")

        ClientScript.RegisterClientScriptBlock(Me.[GetType](), "txtKlaertext", lengthFunction, True)

    End Sub

#End Region

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Show" Then
            Dim strKey As String
            strKey = e.CommandArgument

            Dim dataRow() As DataRow = objKlaerfaelle.ResultPDIs.Select("LICENSE_NUM = '" & strKey & "'")
            If dataRow.Length = 1 Then
                lblCarportID.Text = dataRow(0)("CARPORT_ID").ToString
                lblLiefnr.Text = dataRow(0)("LIEFNR").ToString
                lblFin.Text = dataRow(0)("CHASSIS_NUM").ToString
                lblKennzeichen.Text = dataRow(0)("LICENSE_NUM").ToString
                lblKlarDAD.Text = dataRow(0)("TEXT_KLAERF_DAD").ToString
                txtKlaertext.Text = dataRow(0)("TEXT_KLAERF_CARP").ToString
                lblEndDat.Text = dataRow(0)("ABSCHL_KLAER_DAD").ToString
                lblEditCarport.Text = dataRow(0)("BEARB_KLAER_CARP").ToString
                If IsDate(dataRow(0)("BEARB_KLAER_CARP").ToString) Then
                    lblEditCarport.Text = CDate(dataRow(0)("BEARB_KLAER_CARP").ToString).ToShortDateString
                End If

                If dataRow(0)("DAT_ANLAGE_ABW").ToString.Length > 0 AndAlso lblEndDat.Text.Length = 0 Then
                    txtKlaertext.Enabled = True
                    lb_Weiter.Visible = True
                Else
                    txtKlaertext.Enabled = False
                    lb_Weiter.Visible = False
                End If
                FillGrid(0, "", "LICENSE_NUM = '" & strKey & "'")

                btnConfirm.Visible = False
                lb_Cancel.Visible = True
                'plSearch.Visible = False
                'ddlPageSize.Visible = False

                tbl_Query.Visible = True

                'Result.Visible = False
                Select Case rbAktion.SelectedValue
                    Case "1"
                        txtKlaertext.Enabled = True
                        lb_Weiter.Visible = True
                    Case "2"
                        txtKlaertext.Enabled = True
                        lb_Weiter.Visible = True
                    Case "3"
                        txtKlaertext.Enabled = False
                        lb_Weiter.Visible = False
                    Case "A"
                        txtKlaertext.Enabled = False
                        lb_Weiter.Visible = False
                    Case "E"
                        If dataRow(0)("DAT_ANLAGE_ABW").ToString.Length > 0 AndAlso lblEndDat.Text.Length = 0 Then
                            txtKlaertext.Enabled = True
                            lb_Weiter.Visible = True
                        Else
                            txtKlaertext.Enabled = False
                            lb_Weiter.Visible = False
                        End If
                End Select
            End If


        End If
    End Sub
   
    Private Function CreateExcelTable() As DataTable

        Dim tblExcel As New DataTable
        With tblExcel
            .Columns.Add("Carport ID", GetType(System.String))
            .Columns.Add("Lieferscheinnummer", GetType(System.String))
            .Columns.Add("Fahrgestellnummer", GetType(System.String))
            .Columns.Add("Kennzeichen", GetType(System.String))
            .Columns.Add("Meldungsdatum", GetType(System.String))
            .Columns.Add("Demontagedatum", GetType(System.String))
            .Columns.Add("Anzahl Kennzeichen", GetType(System.String))
            .Columns.Add("Vorlage ZBI", GetType(System.String))
            .Columns.Add("Anlage Abweichung", GetType(System.String))
            .Columns.Add("Klärfalltext DAD", GetType(System.String))
            .Columns.Add("Bearbeitung durch Carport am", GetType(System.String))
            .Columns.Add("Klärfalltext Carport", GetType(System.String))
            .Columns.Add("abgeschlossen durch DAD am", GetType(System.String))
        End With

        Return tblExcel

    End Function

    Private Sub FillKunden()

        If objKlaerfaelle Is Nothing Then
            objKlaerfaelle = New Klaerfaelle(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        End If
        objKlaerfaelle.CarportID = m_User.Reference

        objKlaerfaelle.FillCustomerToPDI(Session("AppID").ToString, Session.SessionID.ToString, Me)

        If Not objKlaerfaelle.Status = 0 Then
            lblError.Text = objKlaerfaelle.E_MESSAGE
        End If

        Dim dItem As New ListItem

        dItem.Value = "0"
        dItem.Text = "Bitte wählen Sie einen Kunden aus."

        ddlKunde.Items.Add(dItem)

        If objKlaerfaelle.Kunden IsNot Nothing AndAlso objKlaerfaelle.Kunden.Rows.Count > 0 Then
            For Each dRow As DataRow In objKlaerfaelle.Kunden.Rows
                ddlKunde.Items.Add(New ListItem(dRow("NAME1_AG").ToString, dRow("KUNNR_AG").ToString))
            Next
        End If

        objKlaerfaelle = Nothing

    End Sub

    Private Function CheckKunde() As Boolean

        Dim booErr As Boolean = False

        If ddlKunde.SelectedValue = "0" Then
            booErr = True
        End If

        Return booErr

    End Function

    Protected Sub ddlKunde_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlKunde.SelectedIndexChanged

        objKlaerfaelle = CType(Session("App_objKlaerfaelle"), Klaerfaelle)

        objKlaerfaelle.Kundennummer = ddlKunde.SelectedItem.Value
        Session("App_objKlaerfaelle") = objKlaerfaelle

    End Sub

End Class