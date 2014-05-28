Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports System.Data.OleDb
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Partial Public Class Change02s1
    Inherits System.Web.UI.Page

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objAbmeldUnterlagen As CarportErfassung
    Private booError As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        GridNavigation1.setGridElment(GridView1)


        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                lblCarportID.Text = m_User.Reference
                txtDateDemontage.Text = Now.ToShortDateString
                txtAnzahlKennz.Text = "2"
                lblName.Text = m_User.FirstName & " " & m_User.LastName
                Session("objAbmeldUnterlagen") = Nothing
                'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "pageLoad", "")
                SetFocus(txtKennz)
            Else
                objAbmeldUnterlagen = CType(Session("objAbmeldUnterlagen"), CarportErfassung)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lb_Weiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Weiter.Click
        lblError.Text = ""

        If tr_Kunde.Visible = True Then
            If CheckKunde() = True Then
                lblError.Text = "Bitte wählen Sie einen Kunden aus."
                Exit Sub
            End If
        End If


        If ProofInput() Then



            If objAbmeldUnterlagen Is Nothing Then
                objAbmeldUnterlagen = New CarportErfassung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                With objAbmeldUnterlagen
                    .CreatePosTable()

                    Dim DataNewRow As DataRow
                    DataNewRow = .Positionen.NewRow

                    If tr_Kunde.Visible = True Then
                        DataNewRow("KUNNR_AG") = Right("0000000000" & ddlKunde.SelectedValue, 10)
                    Else
                        DataNewRow("KUNNR_AG") = Right("0000000000" & m_User.KUNNR, 10)
                    End If

                    DataNewRow("ID") = .Positionen.Rows.Count + 1
                    DataNewRow("LSNUMMER") = ""
                    DataNewRow("CARPORT_ID") = ""
                    DataNewRow("LICENSE_NUM") = txtKennz.Text
                    DataNewRow("CHASSIS_NUM") = ""
                    DataNewRow("ZZFABRIKNAME") = ""
                    DataNewRow("ZZB1_IN_VERW_DAD") = ""
                    DataNewRow("DAT_DEMONT") = txtDateDemontage.Text
                    DataNewRow("ANZ_KENNZ_CPL") = txtAnzahlKennz.Text.Trim
                    DataNewRow("VORLAGE_ZB1_CPL") = rblZBI.SelectedValue
                    DataNewRow("VORLAGE_ZB1_CPL_Text") = rblZBI.SelectedItem.Text
                    DataNewRow("WEB_USER") = m_User.UserName
                    DataNewRow("FLAG_FOUND") = "X"
                    .Positionen.Rows.Add(DataNewRow)
                End With

                FillGrid(0)
                txtKennz.Text = ""
                SetFocus(txtKennz)
                txtDateDemontage.Enabled = False

                Session("objAbmeldUnterlagen") = objAbmeldUnterlagen
            Else
                With objAbmeldUnterlagen
                    If .Positionen.Select("LICENSE_NUM='" & txtKennz.Text.Trim & "'").Length = 0 Then
                        Dim DataNewRow As DataRow
                        DataNewRow = .Positionen.NewRow
                        If tr_Kunde.Visible = True Then
                            DataNewRow("KUNNR_AG") = Right("0000000000" & ddlKunde.SelectedValue, 10)
                        Else
                            DataNewRow("KUNNR_AG") = Right("0000000000" & m_User.KUNNR, 10)
                        End If
                        DataNewRow("ID") = .Positionen.Rows.Count + 1
                        DataNewRow("LSNUMMER") = ""
                        DataNewRow("CARPORT_ID") = ""
                        DataNewRow("LICENSE_NUM") = txtKennz.Text.Trim
                        DataNewRow("CHASSIS_NUM") = ""
                        DataNewRow("ZZFABRIKNAME") = ""
                        DataNewRow("ZZB1_IN_VERW_DAD") = ""
                        DataNewRow("DAT_DEMONT") = txtDateDemontage.Text
                        DataNewRow("ANZ_KENNZ_CPL") = txtAnzahlKennz.Text
                        DataNewRow("VORLAGE_ZB1_CPL") = rblZBI.SelectedValue
                        DataNewRow("VORLAGE_ZB1_CPL_Text") = rblZBI.SelectedItem.Text
                        DataNewRow("WEB_USER") = m_User.UserName
                        DataNewRow("FLAG_FOUND") = "X"

                        .Positionen.Rows.Add(DataNewRow)
                    Else
                        lblError.Text = "Kennzeichen bereits erfasst!"
                        Exit Sub
                    End If

                End With

                GridView1.Columns(0).Visible = False

                For Each dr As DataRow In objAbmeldUnterlagen.Positionen.Rows

                    dr("FLAG_FOUND") = "X"


                Next

                objAbmeldUnterlagen.Positionen.AcceptChanges()

                FillGrid(0)
                txtKennz.Text = ""
                txtDateDemontage.Enabled = False
                Session("objAbmeldUnterlagen") = objAbmeldUnterlagen
                SetFocus(txtKennz)

            End If
            cmdSend.Visible = False
        End If
    End Sub

    Protected Sub rblZBI_changed(ByVal sender As Object, ByVal e As EventArgs)Handles rblZBI.SelectedIndexChanged
        Dim tst As String
        tst = cstr(rblZBI.SelectedItem.Text)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objAbmeldUnterlagen.Positionen.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            Result.Visible = False
            cmdGetData.Visible = False
        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            GridView1.Visible = True
            cmdGetData.Visible = True
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

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()
            GridView1.PageIndex = intTempPageIndex




            Dim txt As TextBox
            Dim lbl As Label

            For Each gvRow As GridViewRow In GridView1.Rows

                txt = CType(gvRow.FindControl("txtGridFahrgestellnummer"), TextBox)
                lbl = CType(gvRow.FindControl("lblFlag"), Label)

                If lbl.Text <> "X" Then
                    'GridView1.Columns(0).Visible = True
                    txt.Enabled = True
                    'CType(gvRow.FindControl("imgOk"), Image).Visible = False
                    'CType(gvRow.FindControl("imgError"), Image).Visible = True
                    CType(gvRow.FindControl("txtGridKennzeichen"), TextBox).Enabled = True
                    CType(gvRow.FindControl("txtGridVertragsnummer"), TextBox).Enabled = True
                Else
                    txt.Enabled = False
                    CType(gvRow.FindControl("txtGridKennzeichen"), TextBox).Enabled = False
                    CType(gvRow.FindControl("txtGridVertragsnummer"), TextBox).Enabled = False
                End If


            Next

        End If
    End Sub

    Private Function ProofInput() As Boolean
        If txtKennz.Text.Length = 0 AndAlso trKennzeichen.Visible = True Then
            lblError.Text = "Bitte füllen Sie alle Pflichfelder!"
            Return False
        ElseIf txtAnzahlKennz.Text.Length = 0 Then
            lblError.Text = "Bitte füllen Sie alle Pflichfelder!"
            Return False
        ElseIf CDate(txtDateDemontage.Text) > CDate(Now.ToShortDateString) Then
            lblError.Text = "Das Demontagedatumdarf nicht in der Zukunft liegen!"
            Return False
        ElseIf rblZBI.SelectedValue = "" Then
            lblError.Text = "Bitte wählen Sie eine Vorlage für die ZB I aus!"
        Else
            Return True
        End If
    End Function
    Private Sub DoSubmit()

        lblError.Text = ""

        If tr_Kunde.Visible = True Then
            If CheckKunde() = True Then
                lblError.Text = "Bitte wählen Sie einen Kunden aus."
                Exit Sub
            Else
                objAbmeldUnterlagen.Kundennummer = ddlKunde.SelectedValue
                objAbmeldUnterlagen.Kundenname = ddlKunde.SelectedItem.Text
            End If
        
        End If


        objAbmeldUnterlagen.GetData(Session("AppID").ToString, Session.SessionID.ToString, Me)

        Session("objAbmeldUnterlagen") = objAbmeldUnterlagen

        If Not objAbmeldUnterlagen.Status = 0 Then
            lblError.Text = "Fehler: " & objAbmeldUnterlagen.Message
        Else
            If objAbmeldUnterlagen.Positionen.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                DivPrint.Visible = True
                imgPDF.Visible = False
                lnkCreatePDF1.Visible = False

                If objAbmeldUnterlagen.Positionen.Select("FLAG_FOUND <> 'X'").Length > 0 Then
                    lblError.Text = "Achtung:  Es liegen unbekannte Fahrzeugpositionen vor!"

                    GridView1.Columns(0).Visible = True

                    'Dim txt As TextBox


                    'For Each gvRow As GridViewRow In GridView1.Rows

                    '    txt = CType(gvRow.FindControl("txtGridFahrgestellnummer"), TextBox)

                    '    If txt.Text = "unbekannt - prüfen" Then

                    '        txt.Enabled = True
                    '        CType(gvRow.FindControl("imgOk"), Image).Visible = False
                    '        CType(gvRow.FindControl("imgError"), Image).Visible = True
                    '        CType(gvRow.FindControl("txtGridKennzeichen"), TextBox).Enabled = True

                    '    Else
                    '        txt.Enabled = False
                    '        CType(gvRow.FindControl("txtGridKennzeichen"), TextBox).Enabled = False
                    '    End If


                    'Next


                    cmdSend.Visible = True

                Else
                    GridView1.Columns(0).Visible = True
                    cmdSend.Visible = True
                End If
                FillGrid(0)
            End If
        End If

    End Sub

    Private Sub DoSubmit2()

        booError = False

        lblError.Text = ""

        If tr_Kunde.Visible = True Then
            If CheckKunde() = True Then
                lblError.Text = "Bitte wählen Sie einen Kunden aus."
                Exit Sub
            End If
        Else
            objAbmeldUnterlagen.Kundennummer = ddlKunde.SelectedValue
        End If


        For Each gvRow As GridViewRow In GridView1.Rows

            objAbmeldUnterlagen.Positionen.Select("LICENSE_NUM = '" & CType(gvRow.FindControl("txtGridKennzeichen"), TextBox).Text & "'")(0)("CHASSIS_NUM") = CType(gvRow.FindControl("txtGridFahrgestellnummer"), TextBox).Text

            If String.IsNullOrEmpty(CType(gvRow.FindControl("txtGridVertragsnummer"), TextBox).Text) = False Then
                objAbmeldUnterlagen.Positionen.Select("LICENSE_NUM = '" & CType(gvRow.FindControl("txtGridKennzeichen"), TextBox).Text & "'")(0)("LIZNR") = CType(gvRow.FindControl("txtGridVertragsnummer"), TextBox).Text
            End If
        Next



        objAbmeldUnterlagen.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)

        Session("objAbmeldUnterlagen") = objAbmeldUnterlagen

        For Each row As DataRow In objAbmeldUnterlagen.Positionen.Rows


            For Each gvRow As GridViewRow In GridView1.Rows

                If row("LICENSE_NUM").ToString = CType(gvRow.FindControl("txtGridKennzeichen"), TextBox).Text Then
                    CType(gvRow.FindControl("imgError"), Image).ToolTip = row("FEHLERTEXT").ToString

                    If String.IsNullOrEmpty(row("FEHLERTEXT")) = False Then
                        CType(gvRow.FindControl("imgError"), Image).Visible = True
                        CType(gvRow.FindControl("imgOk"), Image).Visible = False
                        booError = True
                        lblError.Text = "Beim Speichern sind Fehler aufgetreten."

                    Else
                        CType(gvRow.FindControl("imgError"), Image).Visible = False
                        CType(gvRow.FindControl("imgOk"), Image).Visible = True
                    End If

                    If row("FEHLERTEXT").ToString = "Vertragsnummer fehlt" Then
                        CType(gvRow.FindControl("txtGridVertragsnummer"), TextBox).BorderColor = Drawing.Color.Red
                        CType(gvRow.FindControl("txtGridVertragsnummer"), TextBox).Enabled = True
                    End If

                End If

            Next

        Next




        If Not objAbmeldUnterlagen.Status = 0 OrElse booError = True Then
            lblError.Text = "Beim Speichern sind Fehler aufgetreten. "

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
                    lblNoData.ForeColor = Drawing.Color.MediumSeaGreen
                    DivPrint.Visible = True
                    imgPDF.Visible = True
                    lnkCreatePDF1.Visible = True
                    cmdEdit.Visible = False
                    lb_Upload.Visible = False
                    cmdNewList.Visible = True
                    GridView1.Enabled = False

                    CType(GridNavigation1.FindControl("ddlPageSize"), DropDownList).Enabled = False

                    Dim ctrl As LinkButton

                    For Each Row As GridViewRow In GridView1.Rows

                        ctrl = CType(Row.FindControl("lbDelete"), LinkButton)
                        ctrl.Enabled = False
                        CType(Row.FindControl("imgError"), Image).Visible = False
                        CType(Row.FindControl("imgOk"), Image).Visible = True
                    Next


                    CreatePdf()

                End If

            End If
        End If

    End Sub
    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        lb_Weiter_Click(sender, e)
    End Sub

   

    Private Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        UpdateTable()

        lblError.Text = ""
        Dim Datakey As String = GridView1.DataKeys(e.RowIndex).Value.ToString()
        With objAbmeldUnterlagen
            If .Positionen.Select("LICENSE_NUM='" & Datakey.Trim & "'").Length = 1 Then
                Dim DataRowPos As DataRow
                DataRowPos = .Positionen.Select("LICENSE_NUM='" & Datakey.Trim & "'")(0)
                .Positionen.Rows.Remove(DataRowPos)
                .Positionen.AcceptChanges()
                If .Positionen.Rows.Count = 0 Then txtDateDemontage.Enabled = True
                Session("objAbmeldUnterlagen") = objAbmeldUnterlagen
                FillGrid(GridView1.PageIndex)
            End If
        End With
    End Sub

    Protected Sub cmdGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdGetData.Click
        UpdateTable()
        DoSubmit()
    End Sub

    Protected Sub cmdSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSend.Click

        ClearGrid()

        If CheckGrid() = False Then Exit Sub

        If Panel1.Visible = False Then
            DoSubmit2()

        Else
            lblNoData.Text = "Übersicht Datenerfassung"
            lblNoData.Visible = True
            Panel1.Visible = False
            btnEmpty.Visible = False
            lb_Weiter.Visible = False
            lb_Upload.Visible = False
            cmdEdit.Visible = True
            cmdGetData.Visible = False
            cmdSend.Text = "Absenden"
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
        Panel1.Visible = True
        btnEmpty.Visible = True

        If rdbEinzel.Checked = True Then
            lb_Weiter.Visible = True
        Else
            lb_Upload.Visible = True
        End If


        cmdEdit.Visible = False
        cmdGetData.Visible = True
        cmdSend.Text = "Weiter"
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
        'Panel1.Visible = True
        'btnEmpty.Visible = True
        'lb_Weiter.Visible = True
        'DivPrint.Visible = False

        'cmdEdit.Visible = False
        'cmdGetData.Visible = False
        'cmdNewList.Visible = False
        'lblNoData.Text = ""
        'lblError.Text = ""
        'rblZBI.SelectedIndex = -1
        'GridView1.DataSource = Nothing
        'GridView1.DataBind()
        'Result.Visible = False
        'cmdGetData.Visible = False
        'objAbmeldUnterlagen = Nothing
        'txtDateDemontage.Enabled = True
        'Session("objAbmeldUnterlagen") = Nothing
        'cmdSend.Text = "Weiter"
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "pageLoad", "")
        Response.Redirect("Change02s.aspx?AppID=" & Session("AppID").ToString)


    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        If Not IsPostBack Then
            If tr_Kunde.Visible = True Then FillKunden()
        End If
        'HelpProcedures.FixedGridViewCols(GridView1)

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        UpdateTable()
        FillGrid(GridView1.PageIndex, e.SortExpression)
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
            headTable.Columns.Add("Kundenname", GetType(System.String))

            Dim tmpSAPRow As DataRow
            For Each drow As DataRow In objAbmeldUnterlagen.Positionen.Rows
                tmpSAPRow = headTable.NewRow
                tmpSAPRow("CarportID") = drow("CARPORT_ID")
                tmpSAPRow("Name1") = m_User.LastName
                tmpSAPRow("Name2") = m_User.FirstName
                tmpSAPRow("LieferscheinNummer") = drow("LSNUMMER")
                sLiefNummer = drow("LSNUMMER").ToString.Trim
                tmpSAPRow("Kundenname") = objAbmeldUnterlagen.Kundenname
                headTable.Rows.Add(tmpSAPRow)
                Exit For
            Next

            Dim imageHt = GetImageTable(sLiefNummer)

            Dim docFactory As New DocumentGeneration.WordDocumentFactory(tblExcel, imageHt)
            docFactory.CreateDocumentTable("Lieferschein", Me.Page, "\Components\ComCommon\Leasing\Documents\Bestellung.doc", headTable)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try

    End Sub

    Private Function GetImageTable(ByVal liefNummer As String) As Hashtable
        Dim imageHt As New Hashtable()
        Try
            imageHt.Add("Logo", m_User.Customer.LogoImage)
        Catch ex As Exception
            ' LogoPath am Customer nicht (korrekt) gepflegt - hier: ignorieren
        End Try
        Try
            imageHt.Add("Logo2", m_User.Customer.LogoImage2)
        Catch ex As Exception
            ' LogoPath2 am Customer nicht (korrekt) gepflegt - hier: ignorieren
        End Try
        Dim ms As New IO.MemoryStream
        ms = HelpProcedures.CreateBarcode(liefNummer)
        imageHt.Add("Logo3", ms)

        Return imageHt
    End Function

    Private Sub CreatePdf()
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
            headTable.Columns.Add("Kundenname", GetType(System.String))

            Dim tmpSAPRow As DataRow
            For Each drow As DataRow In objAbmeldUnterlagen.Positionen.Rows
                tmpSAPRow = headTable.NewRow
                tmpSAPRow("CarportID") = drow("CARPORT_ID")
                tmpSAPRow("Name1") = m_User.LastName
                tmpSAPRow("Name2") = m_User.FirstName
                tmpSAPRow("LieferscheinNummer") = drow("LSNUMMER")
                tmpSAPRow("Kundenname") = objAbmeldUnterlagen.Kundenname
                sLiefNummer = drow("LSNUMMER").ToString.Trim
                headTable.Rows.Add(tmpSAPRow)
                Exit For
            Next

            Dim imageHt = GetImageTable(sLiefNummer)
            Dim docFactory As New DocumentGeneration.WordDocumentFactory(tblExcel, imageHt)

            Dim Path As String = ConfigurationManager.AppSettings("ExcelPath") & "Lieferschein_" & Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName


            docFactory.CreateDocumentTableAndSave(Path, Me.Page, "\Components\ComCommon\Leasing\Documents\Bestellung.doc", headTable)

            Dim ScriptText As String

            ScriptText = "						<script language=""Javascript"">" & vbCrLf
            ScriptText &= "						  <!-- //" & vbCrLf
            ScriptText &= "                          function pageLoad() {window.open(""Change02s_1.aspx?AppID=" & Session("AppID").ToString & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");}" & vbCrLf
            ScriptText &= "						  //-->" & vbCrLf
            ScriptText &= "						</script>" & vbCrLf


            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "pageLoad", ScriptText)

            Session("App_Filepath") = Path

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub FillKunden()

        If objAbmeldUnterlagen Is Nothing Then
            objAbmeldUnterlagen = New CarportErfassung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        End If


        objAbmeldUnterlagen.FillCustomerToPDI(Session("AppID").ToString, Session.SessionID.ToString, Me)

        If objAbmeldUnterlagen.E_MESSAGE.Length > 0 Then
            lblError.Text = objAbmeldUnterlagen.E_MESSAGE
        End If

        Dim dItem As New ListItem

        dItem.Value = "0"
        dItem.Text = "Bitte wählen Sie einen Kunden aus."

        ddlKunde.Items.Add(dItem)

        If Not objAbmeldUnterlagen.Kunden Is Nothing Then
            For Each dRow As DataRow In objAbmeldUnterlagen.Kunden.Rows

                dItem = New ListItem
                dItem.Value = dRow("KUNNR_AG").ToString
                dItem.Text = dRow("NAME1_AG").ToString

                ddlKunde.Items.Add(dItem)

            Next
        Else
            lblError.Text = "Keine Kunden mit gewähltem Carport verknüpft."
        End If


        objAbmeldUnterlagen = Nothing


    End Sub

    Private Function CheckKunde() As Boolean

        Dim booErr As Boolean = False

        If ddlKunde.SelectedValue = "0" Then
            booErr = True
        End If


        Return booErr

    End Function

    Protected Sub rdbEinzel_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbEinzel.CheckedChanged, rdbUpload.CheckedChanged

        Session("objAbmeldUnterlagen") = Nothing

        lb_Weiter.Visible = rdbEinzel.Checked
        trKennzeichen.Visible = rdbEinzel.Checked

        tblUpload.Visible = rdbUpload.Checked
        lb_Upload.Visible = rdbUpload.Checked

        cmdGetData.Visible = False
        cmdSend.Visible = False

        Result.Visible = False

    End Sub

   

    Protected Sub lb_Upload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Upload.Click
        'Prüfe Fehlerbedingung
        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            If (Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" AndAlso Right(upFile.PostedFile.FileName.ToUpper, 5) <> ".XLSX") Then

                lblError.Text = "Es können nur Dateien im .XLS oder .XLSX-Format verarbeitet werden."
                Exit Sub
            End If
        Else

            lblError.Text = "Keine Datei ausgewählt!"
            Exit Sub
        End If


        upload(upFile.PostedFile)

    End Sub

    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)

        Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
        Dim filename As String = ""
        Dim info As System.IO.FileInfo

        'Dateiname: User_yyyyMMddhhmmss.xls
        If Right(upFile.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"
        End If
        If Right(upFile.PostedFile.FileName.ToUpper, 5) = ".XLSX" Then
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xlsx"
        End If

        Try
            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If

                'Datei gespeichert -> Auswertung
                Dim sConnectionString As String = ""
                If Right(upFile.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
                    sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                     "Data Source=" & filepath & filename & ";" & _
                     "Extended Properties=""Excel 8.0;HDR=YES;"""
                Else
                    sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + filename + _
                    ";Extended Properties=""Excel 12.0 Xml;HDR=YES"""
                End If


                Dim objConn As New OleDbConnection(sConnectionString)
                objConn.Open()

                Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

                Dim objAdapter1 As New OleDbDataAdapter()
                objAdapter1.SelectCommand = objCmdSelect

                Dim objDataset1 As New DataSet()
                objAdapter1.Fill(objDataset1, "XLData")


                Dim TempTable As DataTable = objDataset1.Tables(0)

                objConn.Close()


                lblError.Text = ""

                If tr_Kunde.Visible = True Then
                    If CheckKunde() = True Then
                        lblError.Text = "Bitte wählen Sie einen Kunden aus."
                        Exit Sub
                    End If
                End If

                objAbmeldUnterlagen = Nothing

                If ProofInput() Then

                    objAbmeldUnterlagen = New CarportErfassung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    With objAbmeldUnterlagen
                        .CreatePosTable()

                        Dim DataNewRow As DataRow
                        DataNewRow = .Positionen.NewRow

                        If tr_Kunde.Visible = True Then
                            DataNewRow("KUNNR_AG") = Right("0000000000" & ddlKunde.SelectedValue, 10)
                        Else
                            DataNewRow("KUNNR_AG") = Right("0000000000" & m_User.KUNNR, 10)
                        End If


                        Dim i As Integer = 0

                        For Each dr As DataRow In TempTable.Rows

                            Dim strKennzeichen As String = dr(1).ToString()
                            Dim strFahrgestellnummer As String = dr(0).ToString()

                            If Not String.IsNullOrEmpty(strKennzeichen) AndAlso Not String.IsNullOrEmpty(strFahrgestellnummer) Then
                                DataNewRow = .Positionen.NewRow

                                If tr_Kunde.Visible = True Then
                                    DataNewRow("KUNNR_AG") = Right("0000000000" & ddlKunde.SelectedValue, 10)
                                Else
                                    DataNewRow("KUNNR_AG") = Right("0000000000" & m_User.KUNNR, 10)
                                End If
                                DataNewRow("ID") = i + 1
                                DataNewRow("LSNUMMER") = ""
                                DataNewRow("CARPORT_ID") = ""
                                DataNewRow("LICENSE_NUM") = strKennzeichen
                                DataNewRow("CHASSIS_NUM") = strFahrgestellnummer
                                DataNewRow("ZZFABRIKNAME") = ""
                                DataNewRow("ZZB1_IN_VERW_DAD") = ""
                                DataNewRow("DAT_DEMONT") = txtDateDemontage.Text
                                DataNewRow("ANZ_KENNZ_CPL") = txtAnzahlKennz.Text.Trim
                                DataNewRow("VORLAGE_ZB1_CPL") = rblZBI.SelectedValue
                                DataNewRow("VORLAGE_ZB1_CPL_Text") = rblZBI.SelectedItem.Text
                                DataNewRow("WEB_USER") = m_User.UserName
                                DataNewRow("FLAG_FOUND") = "X"
                                .Positionen.Rows.Add(DataNewRow)
                            End If

                        Next

                    End With

                    FillGrid(0)
                    txtKennz.Text = ""
                    SetFocus(txtKennz)
                    txtDateDemontage.Enabled = False

                    Session("objAbmeldUnterlagen") = objAbmeldUnterlagen

                End If

            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim hochladen der Datei! " & ex.Message
        End Try

    End Sub


    Private Sub UpdateTable()

        objAbmeldUnterlagen = Session("objAbmeldUnterlagen")

        If Not objAbmeldUnterlagen.Positionen Is Nothing Then

            Dim lbl As Label
            Dim img As Image


            For Each dr As GridViewRow In GridView1.Rows

                lbl = CType(dr.FindControl("lblID"), Label)
                img = CType(dr.FindControl("imgError"), Image)

                If Not img Is Nothing Then

                    If img.Visible = True Then

                        objAbmeldUnterlagen.Positionen.Select("ID = '" & lbl.Text & "'")(0)("CHASSIS_NUM") = CType(dr.FindControl("txtGridFahrgestellnummer"), TextBox).Text
                        objAbmeldUnterlagen.Positionen.Select("ID = '" & lbl.Text & "'")(0)("LICENSE_NUM") = CType(dr.FindControl("txtGridKennzeichen"), TextBox).Text
                        objAbmeldUnterlagen.Positionen.Select("ID = '" & lbl.Text & "'")(0)("LIZNR") = CType(dr.FindControl("txtGridVertragsnummer"), TextBox).Text

                        objAbmeldUnterlagen.Positionen.AcceptChanges()

                    End If


                End If

            Next


        End If


        Session("objAbmeldUnterlagen") = objAbmeldUnterlagen

    End Sub

   
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        UpdateTable()
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        UpdateTable()
        FillGrid(0)
    End Sub

    Private Function CheckGrid() As Boolean

        lblError.Text = ""

        Dim bChecked As Boolean = True

        For Each gvRow As GridViewRow In GridView1.Rows

            If CType(gvRow.FindControl("txtGridFahrgestellnummer"), TextBox).Enabled = True Then

                If CType(gvRow.FindControl("txtGridFahrgestellnummer"), TextBox).Text.Length <> 17 Then

                    CType(gvRow.FindControl("txtGridFahrgestellnummer"), TextBox).BorderColor = Drawing.Color.Red

                    bChecked = False
                    lblError.Text = "Fahrgestellnummer nicht 17-stellig."
                End If

            End If

        Next





        Return bChecked
    End Function

    Private Sub ClearGrid()
        For Each gvRow As GridViewRow In GridView1.Rows

            CType(gvRow.FindControl("txtGridFahrgestellnummer"), TextBox).BorderColor = Drawing.Color.Empty
            CType(gvRow.FindControl("txtGridVertragsnummer"), TextBox).BorderColor = Drawing.Color.Empty

        Next
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

End Class

