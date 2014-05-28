Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Vorerf02
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents cbxShowAll As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtDatumfilter As System.Web.UI.WebControls.TextBox
    Protected WithEvents cbxShowColumn As System.Web.UI.WebControls.CheckBox
    Private ve As String = "Vorerfassung"
    Private zulDaten As clsVorerf01
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private table As DataTable
    Private showCheckbox As Boolean
    Private appl As String
    Private highlightID As String
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtOrtsKzOld As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtFree2 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Literal2 As System.Web.UI.WebControls.Literal
    Protected WithEvents btnSaveSAP As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblFilter As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatensatz As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblTableTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnzahl As System.Web.UI.WebControls.Label
    Protected WithEvents dataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents btnErfassen As System.Web.UI.WebControls.LinkButton
    Private pflicht As String
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkPopUp As System.Web.UI.HtmlControls.HtmlAnchor
    Private ne As String = "Nacherfassung"

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        zulDaten = CType(Session("ZulDaten"), clsVorerf01)

        If (zulDaten.Vorerfassung = True) Then
            appl = ve
        Else
            appl = ne
        End If

        If Not IsPostBack Then
            Try
                'Seitenparameter laden
                highlightID = Request.QueryString("HR")
                'Header initialisieren
                lblPageTitle.Text = appl.ToUpper & " der Zulassungsdaten"
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text

                btnErfassen.Enabled = True

                If appl = ve Then
                    loadRecords()
                Else
                    btnErfassen.Enabled = False
                    loadRecordsNach()
                End If
                If noRecords() = False Then
                    FillGrid(0)
                Else
                    lblError.Text = "Keine Daten gefunden."
                    lblTableTitle.Visible = False
                    lblAnzahl.Visible = False
                End If
            Catch ex As Exception
            End Try
        Else
          
        End If
    End Sub

    Private Sub insertScript()
        If Not (highlightID Is Nothing) Then
            Literal2.Text = "						<script language=""Javascript"">" & vbCrLf
            Literal2.Text &= "						  <!-- //" & vbCrLf
            Literal2.Text &= "						    window.document.location.href = ""#" & highlightID & """;" & vbCrLf
            Literal2.Text &= "						  //-->" & vbCrLf
            Literal2.Text &= "						</script>" & vbCrLf
        End If
    End Sub

    Public Function noRecords() As Boolean
        Return ((table Is Nothing) OrElse (table.Rows.Count = 0))
    End Function


    Private Sub loadRecordsNach()
        'Nacherfassung()        
        Dim message As String = String.Empty
        Dim inserted As Integer

        'UH - 14.11.2005
        'Die Daten sollen nur beim ersten Mal vom SAP geholt werden.
        If Session("ReloadNacherfassung") = "1" Then
            'UH - 14.11.2005
            'Es kann der Fall eintreten, dass ein Datensatz z.B. zu einem anderen Datum
            'umgesetzt wurde.
            'Damit wird kein Gegenstück im SAP mehr gefunden
            'Es wird also:
            'a) Eine Suche im SAP ausgeführt.
            'b) Schon im SQL-Server vorhandene Ergebnisse aus der SAP-Trefferliste entfernt
            'c) Die übrigen SAP-Treffer dem SQL-Server hinzugefügt
            inserted = zulDaten.getSAPDatenVorerfassung(zulDaten.Kundentabelle, zulDaten.Materialtabelle, message, zulDaten.VKOrg, zulDaten.VKBur, zulDaten.FilterSTVA,
                                                        zulDaten.FilterID, HelpProcedures.MakeDateSAP(clsVorerf01.toShortDateStr(zulDaten.FilterDatum)),
                                                        zulDaten.FilterKunde)
            Session("ReloadNacherfassung") = "0"
        End If
        'd) In jedem Fall wird der SQL-Server auf Ergebnisse geprüft.
        loadRecords()

        If noRecords() Then
            lblError.Text = "Keine Daten gefunden."
        End If
    End Sub

    Private Sub loadRecords()
        Dim message As String = String.Empty

        If (appl = ve) Then 'Vorerfassung
            table = zulDaten.getSQLDaten(message, m_User.UserID, "ALLE", False)
        End If
        If (appl = ne) Then 'Nacherfassung: mit Filter STVA, KUNNR, Datensatz-ID, Endabgerechnet
            table = zulDaten.getSQLDaten(message, zulDaten.FilterSTVA, zulDaten.FilterKunde, zulDaten.FilterID, clsVorerf01.toShortDateStr(zulDaten.FilterDatum), True)
        End If
        If message <> String.Empty Then
            lblError.Text = "Fehler beim Laden der Daten."
            Exit Sub
        End If

        If Not table.Columns.Contains("status") Then
            table.Columns.Add("status", System.Type.GetType("System.String"))   'Spalte für Status einfügen
        End If


        zulDaten.updateKunde(zulDaten.VKOrg, zulDaten.VKBur, table)

        If (Session("ResultTable") Is Nothing) Then
            Session.Add("ResultTable", table)
        Else
            Session("ResultTable") = table
        End If

        Session("DatensatzAnzahl") = table.Rows.Count

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        table = CType(Session("ResultTable"), DataTable)

        If (table Is Nothing) OrElse (table.Rows.Count = 0) Then
            dataGrid.Visible = False
        Else
            lblAnzahl.Text = table.Rows.Count

            dataGrid.Visible = True
            Dim tmpDataView As New DataView()
            tmpDataView = table.DefaultView

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
            Else
                tmpDataView.Sort = "kundenname, haltername, str_wunschkennz, id"
            End If

            dataGrid.CurrentPageIndex = intTempPageIndex
            dataGrid.DataSource = tmpDataView
            dataGrid.DataBind()

            If dataGrid.PageCount > 1 Then
                dataGrid.PagerStyle.CssClass = "PagerStyle"
                dataGrid.DataBind()
                dataGrid.PagerStyle.Visible = True
            Else
                dataGrid.PagerStyle.Visible = False
            End If

            'Zuletzt beqrbeitete Zeile markieren...
            Dim item As DataGridItem

            If Not highlightID Is Nothing Then
                For Each item In dataGrid.Items
                    item.Font.Bold = False

                    If item.Cells(1).Text = highlightID Then
                        item.Font.Bold = True
                    End If
                Next
                insertScript()
            End If

            'Zeilen mit Gebühren = 0 markieren... (für Nacherfassung)

            zulDaten = CType(Session("ZulDaten"), clsVorerf01)
            Dim dienstleistung As String
            Dim row As DataRow()

            If (appl = ne) Then ' AndAlso (pflicht <> String.Empty) Then
                For Each item In dataGrid.Items
                    dienstleistung = item.Cells(4).Text
                    row = zulDaten.Materialtabelle.Select("MAKTX = '" & dienstleistung & "'")

                    If Not (row.Length = 1) Then
                        lblError.Text = "Dienstleistung konnte nicht gelesen werden."
                    Else
                        pflicht = CType(row(0)("ZZGEBPFLICHT"), String)
                        If (item.Cells(6).Text = "0") AndAlso (pflicht <> String.Empty) Then
                            item.Cells(6).CssClass = "Background_01"
                            'item.Cells(6).Font.Bold = True
                            lblError.Text = "Hinweis: Es wurde nicht für jeden Datensatz eine Kundengebühr eingetragen."
                        End If
                    End If
                Next
            End If

        End If
    End Sub

    Private Sub btnSaveSAP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSAP.Click
        Dim found As Boolean
        Dim status As String
        Dim deleteID As String
        Dim row As DataRow

        table = CType(Session("ResultTable"), DataTable)
        For Each row In table.Rows
            row("toSave") = True
        Next
        table.AcceptChanges()

        found = True
        status = String.Empty

        If (found = False) Then
            lblError.Text = "Keine Datensätze ausgewählt."
        Else
            deleteID = String.Empty
            If (appl = ve) Then '*** Vorerfassung
                zulDaten.writeZTable(table, status, m_User.UserID.ToString, deleteID)     'In SAP speichern (ZTabelle)
                For Each row In table.Rows
                    If (row("toDelete") = "L") Then
                        zulDaten.DeleteRecordVE(row("id").ToString)
                        row("status") = "Vorgang OK"
                    End If
                Next
            End If
            If (appl = ne) Then '*** Nacherfassung
                zulDaten.writeSDTable(table, status, m_User.UserID.ToString, deleteID, "", Me)   'In SAP speichern (SD-Auftrag)
            End If
            If Not (status = String.Empty) Then
                lblError.Text = "Fehler beim Speichern der Daten: " & status
                Exit Sub
            End If
            zulDaten.dbClear(deleteID, status)
            If Not (status = String.Empty) Then
                lblError.Text = "Fehler beim löschen des Datenpuffers."
                Exit Sub
            End If

            'Spalten mit "Vorgang OK" entfernen....
            Dim tblTemp As New DataTable()

            tblTemp = table.Copy()
            For Each row In tblTemp.Rows
                If (CType(row("status"), String) = "Vorgang OK") Or CType(row("toDelete"), String) <> "L" Then
                    table.Rows.Remove(table.Select("id_sap=" & row("id_sap").ToString)(0))
                End If
            Next
            table.AcceptChanges()
            Session("ResultTable") = table

            FillGrid(0)

            btnSaveSAP.Enabled = False
            'tblSumme.Visible = False

            If (table.Rows.Count > 0) Then
                lblMsg.Text = "Es konnten nicht alle Datensätze gespeichert werden!"
                lblMsg.ForeColor = System.Drawing.Color.Red
                lblTableTitle.Visible = False
            Else
                lblMsg.Text = "Ihre Datensätze sind erfolgreich bei uns eingegangen. Es sind keine Fehler aufgetreten."
                lblMsg.ForeColor = System.Drawing.Color.Black
                lblTableTitle.Visible = False
            End If


            Session("ReportTable") = tblTemp
            Dim tblExcel As DataTable = tblTemp.Copy
            With tblExcel.Columns
                .Remove("id_sap")
                .Remove("id_user")
                .Remove("id_session")
                .Remove("abgerechnet")
                .Remove("stvanr")
                .Remove("str_wunschkennz")
                .Remove("dienstleistungnr")
                .Remove("preis_stva")
                .Remove("preis_zulassung")
                .Remove("preis_kennz")
                .Remove("preis_pauschal")
                .Remove("saved")
                .Remove("toSave")
                .Remove("toDelete")
                .Remove("check2")
                .Remove("check3")
                .Remove("free2")
                .Remove("free3")
                .Remove("kundennr_alt")
                .Remove("tour")
                .Remove("fremdbestand")
                .Remove("barkunde")
                .Remove("sonst_dienst")
                .Remove("preis_sonstdienst")
                .Remove("vorerfassung")
                .Remove("testuser")
                .Remove("preis_kasse")
                .Remove("zzsteuer")
            End With
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim objExcelExport As New Excel.ExcelExport()
            Try
                Excel.ExcelExport.WriteExcel(tblExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                lnkExcel.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                lnkExcel.Visible = True
                lblDownloadTip.Visible = True
            Catch
            End Try

            Dim popupBuilder As New System.Text.StringBuilder()
            With popupBuilder
                .Append("<script languange=""Javascript""><!--")
                .Append(ControlChars.CrLf)
                .Append("function openPopUp() {")
                .Append(ControlChars.CrLf)
                .Append("window.open('Vorerf02_Print.aspx', 'POPUP', 'dependent=yes,location=no,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no');")
                .Append(ControlChars.CrLf)
                .Append("}")
                .Append(ControlChars.CrLf)
                .Append("--></script>")
                .Append(ControlChars.CrLf)
            End With
            ClientScript.RegisterClientScriptBlock(Me.GetType, "POPUP", popupBuilder.ToString)
            lnkPopUp.Visible = True

        End If
    End Sub

    Private Sub dataGrid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dataGrid.ItemCommand
        Dim id_Recordset As Int32
        Dim redirectPage As String = ""

        If e.CommandName = "Select" Then
            id_Recordset = CType(e.Item.Cells(1).Text(), Int32)
            If appl = ve Then
                redirectPage = "Vorerf01.aspx?AppID=" & Session("AppID").ToString & "&Appl=Vorerfassung&ID=" & id_Recordset & "&B=2346"
            Else
                'redirectPage = "Input_004_011.aspx?AppID=" & Session("AppID").ToString & "&Appl=Nacherfassung&ID=" & id_Recordset & "&B=2345"
            End If
            Response.Redirect(redirectPage)
        End If
    End Sub

    Private Sub dataGrid_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid.DeleteCommand
        Dim str_id_Recordset As String
        Dim str_toDelete As String

        If e.CommandName = "Delete" Then
            str_id_Recordset = e.Item.Cells(0).Text

            str_toDelete = e.Item.Cells(2).Text
            '--- &nbsp; in '' umwandeln...
            If (str_toDelete <> "L") Then
                str_toDelete = String.Empty
            End If
            zulDaten.DeleteRecordNE(str_id_Recordset, str_toDelete)

        End If
        loadRecords()
        FillGrid(0)
        If (appl = ne) Then
            If Not (highlightID Is Nothing) Then
                Literal2.Text = "						<script language=""Javascript"">" & vbCrLf
                Literal2.Text &= "						  <!-- //" & vbCrLf
                Literal2.Text &= "						    window.document.location.href = ""#" & highlightID & """;" & vbCrLf
                Literal2.Text &= "						  //-->" & vbCrLf
                Literal2.Text &= "						</script>" & vbCrLf
            End If
        End If
    End Sub

    Private Sub dataGrid_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid.SortCommand
        FillGrid(dataGrid.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub btnErfassen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnErfassen.Click
        Dim redirectpage As String = ""

        If appl = ve Then
            redirectpage = "Vorerf01.aspx?AppID=" & Session("AppID").ToString & "&Appl=Vorerfassung" '&Kunnr=" & kunnr & "&STVA=" & stva  '& "&ID=" & id_Recordset & "&Zuldatum=" & zulDatumFilter & "&B=2346"
        End If
        Response.Redirect(redirectpage)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Vorerf02.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 29.04.09   Time: 13:46
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 14.10.08   Time: 13:07
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA: 2301 & Warnungen bereinigt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 25.09.08   Time: 17:14
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA 2213
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 6.07.07    Time: 14:29
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' ITA: 1130
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 13:07
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 9:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' ************************************************
