Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Partial Public Class Report02
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private m_objExcel As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvUeberf)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        Try


            If Not IsPostBack Then

                Dim uebf As New LogUeberf(m_User, m_App, Session("AppID").ToString, Session.SessionID, String.Empty)
                Dim dv As DataView = uebf.readGroupData(Me.Page, getKundennummer())
                If Not dv Is Nothing Then
                    If dv.Table.Rows.Count > 0 Then
                        With cmb_KundenNr
                            .DataSource = dv
                            .DataValueField = "KUNNR"
                            .DataTextField = "Anzeige"
                            .DataBind()
                        End With
                    Else
                        trKUNNR.Visible = False
                    End If
                End If
            End If
            If trKUNNR.Visible And cmb_KundenNr.Items.Count = 0 Then
                cmb_KundenNr.Visible = False
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            Dim uebf As New LogUeberf(m_User, m_App, Session("AppID").ToString, Session.SessionID, strFileName)
            Dim table As DataTable

            Dim kunnr As String = String.Empty
            If Not cmb_KundenNr.SelectedItem Is Nothing Then
                kunnr = cmb_KundenNr.SelectedItem.Value
                Session("kunnr") = kunnr
            Else
                Session("kunnr") = m_User.KUNNR
            End If

            uebf.readSAPAuftraege2(txtAuftrag.Text, txtAuftragdatum.Text, txtAuftragdatumBis.Text, _
                                   txtReferenz.Text, txtKennzeichen.Text, txtUeberfuehrungdatumVon.Text, _
                                   txtUeberfuehrungdatumBis.Text, rbAuftragart.SelectedItem.Value, kunnr, _
                                   txt_Leasinggesellschaft.Text, txt_Leasingkunde.Text, _
                                   Session("AppID").ToString, Session.SessionID.ToString, Me.Page, getKundennummer())
            table = uebf.Result

            If (uebf.Status = 0 AndAlso table.Rows.Count > 0) Then
                Session("ResultTable") = table


                Try
                    m_objExcel = New DataTable()
                    m_objExcel.Columns.Add("Referenz", table.Columns("Zzrefnr").DataType)
                    m_objExcel.Columns.Add("Auftrag", table.Columns("Aufnr").DataType)
                    m_objExcel.Columns.Add("Auftragsdatum", table.Columns("Wadat").DataType)
                    m_objExcel.Columns.Add("Fahrt", table.Columns("Fahrtnr").DataType)
                    m_objExcel.Columns.Add("Kennzeichen", table.Columns("Zzkenn").DataType)
                    m_objExcel.Columns.Add("Typ", table.Columns("Zzbezei").DataType)
                    m_objExcel.Columns.Add("Leistungsdatum", table.Columns("VDATU").DataType)
                    m_objExcel.Columns.Add("Abgabedatum", table.Columns("wadat_ist").DataType)
                    m_objExcel.Columns.Add("Von", table.Columns("Fahrtvon").DataType)
                    m_objExcel.Columns.Add("Nach", table.Columns("Fahrtnach").DataType)
                    m_objExcel.Columns.Add("Km", table.Columns("Gef_Km").DataType)
                    m_objExcel.Columns.Add("Klärfall", table.Columns("KFTEXT").DataType)

                    Dim tmpRow As DataRow
                    Dim tmpNew As DataRow
                    For Each tmpRow In table.Rows
                        tmpNew = m_objExcel.NewRow

                        tmpNew("Referenz") = tmpRow("Zzrefnr")

                        tmpNew("Auftrag") = tmpRow("Aufnr")
                        tmpNew("Auftragsdatum") = tmpRow("Wadat")
                        tmpNew("Fahrt") = tmpRow("Fahrtnr")
                        tmpNew("Kennzeichen") = tmpRow("Zzkenn")
                        tmpNew("Typ") = tmpRow("Zzbezei")
                        tmpNew("Leistungsdatum") = tmpRow("VDATU")
                        tmpNew("Abgabedatum") = tmpRow("wadat_ist")
                        tmpNew("Von") = tmpRow("Fahrtvon")
                        tmpNew("Nach") = tmpRow("Fahrtnach")
                        tmpNew("Km") = tmpRow("Gef_Km")
                        If CStr(tmpRow("KFTEXT")).Trim(" "c) <> String.Empty Then
                            tmpNew("Klärfall") = "X"
                        Else
                            tmpNew("Klärfall") = ""
                        End If

                        m_objExcel.Rows.Add(tmpNew)
                    Next


                Catch
                End Try
                Session("AppTableExcel") = m_objExcel
                FillGrid(0)
            Else
                Session("ResultTable") = Nothing
                FillGrid(0)
                lblError.Text = uebf.Message
            End If


        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler im Report <Optisches Archiv (Recherche Überführungen)>. Fehler: " & ex.Message)
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim table As DataTable

        table = CType(Session("ResultTable"), DataTable)

        If (table Is Nothing) OrElse (table.Rows.Count = 0) Then
            Result.Visible = False
            lblError.Visible = True
            Panel1.Visible = True
            lbCreate.Visible = True
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            'ShowScript.Visible = False
        Else
            Result.Visible = True
            lblError.Visible = False
            Panel1.Visible = False
            lbCreate.Visible = False
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
            End If

            gvUeberf.PageIndex = intTempPageIndex
            gvUeberf.DataSource = tmpDataView

            gvUeberf.DataBind()

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblError.Text = CStr(Session("ShowOtherString"))
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                'lnkKreditlimit.Text = "Zurück"
                'lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            End If
            lblError.Visible = True

        End If
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Try
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            m_objExcel = CType(Session("AppTableExcel"), DataTable)
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, Me.m_objExcel, Me.Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            lblError.Visible = True
        End Try
    End Sub

    Private Sub gvUeberf_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUeberf.RowCommand
        'Response.Redirect(e.CommandArgument.ToString & "?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        Panel1.Visible = Not Panel1.Visible
        lbCreate.Visible = Not lbCreate.Visible
    End Sub


    Private Sub gvUeberf_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        gvUeberf.EditIndex = -1
        FillGrid(pageindex)
    End Sub

    Private Sub gvUeberf_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvUeberf.Sorting
        FillGrid(0, e.SortExpression)
    End Sub
    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Function getKundennummer() As String

        Dim Kundennummer As String = m_User.KUNNR

        If m_User.Store = "AUTOHAUS" AndAlso m_User.Reference.Trim(" "c).Length > 0 AndAlso m_User.KUNNR = "261510" Then
            Kundennummer = m_User.Reference
        End If

        Return Kundennummer

    End Function



End Class