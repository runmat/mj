Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports Telerik.Web.UI
Imports System.Globalization
Imports System.IO

Partial Public Class Report03
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private m_objExcel As DataTable

    'ACHTUNG Nur zum Testen auf true setzen
    'Die Dateien werden dann unter "D:\Dokumente\"  abgelegt
    Private useLocalTestPath As Boolean = False

    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation


#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ScriptManager.RegisterClientScriptBlock(Page, GetType(Report03), "async_upload", "function onUploadFailed(sender, args) { " & _
                                        "alert(args.get_message()); " & _
                                        "} " & _
                                        "function onFileUploaded(sender, args) { " & _
                                        "document.getElementById('" & submitFiles.ClientID & "').click(); }", True)


        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        m_App = New App(m_User)

        GridNavigation1.setGridElment(gvUeberf)

        lblError.Visible = False
        lblError.Text = ""

        Dim newCulture As CultureInfo = CultureInfo.CreateSpecificCulture("de-DE")


        Try


            If Not IsPostBack Then
                GetAppIDFromQueryString(Me)


                Dim uebf As New LogUeberf(m_User, m_App, Session("AppID").ToString, Session.SessionID, String.Empty)
                'Dim dv As DataView = uebf.readGroupData(Me.Page, m_User.KUNNR)
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
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            If trKUNNR.Visible And cmb_KundenNr.Items.Count = 0 Then
                cmb_KundenNr.Visible = False
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            lblError.Visible = True
        End Try
    End Sub

#End Region

#Region "Control Events"

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
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

    Private Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click

        If Panel1.Visible Then

            Panel1.Visible = False
            lbCreate.Visible = False
            NewSearch.ImageUrl = "../../../Images/queryArrowUp.gif"
            Result.Visible = True

        Else

            Panel1.Visible = True
            lbCreate.Visible = True
            NewSearch.ImageUrl = "../../../Images/queryArrow.gif"
            Result.Visible = False

        End If




    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        lblFahrt.Text = ""
        lblFahrtnr.Text = ""
        lblAuftragsnr.Text = ""
        Panel1.Visible = True
        Result.Visible = True
        lbCreate.Visible = True
        Panel3.Visible = True
        Panel2.Visible = False
        FillGrid(gvUeberf.PageIndex)
    End Sub

    Protected Sub AsyncFileUpload1_UploadedComplete2(ByVal sender As Object, ByVal e As AjaxControlToolkit.AsyncFileUploadEventArgs)
        Dim FileUpload As AjaxControlToolkit.AsyncFileUpload = sender
        If Not FileUpload Is Nothing Then
            If FileUpload.HasFile = True Then
                If FileUpload.ContentType = "application/pdf" Then
                    Dim uebf As LogUeberf = CType(Session("LogUeberf"), LogUeberf)
                    Dim grvrow As GridViewRow = grvProtokollUpload1.Rows(CInt(FileUpload.ToolTip))
                    Dim lblFahrt As Label = CType(grvrow.Cells(1).FindControl("lblFahrt"), Label)
                    Dim Fahrt As String = lblFahrt.Text
                    Dim lfdNr As String = grvrow.Cells(0).Text
                    Dim DokArt As String = grvrow.Cells(2).Text
                    Dim sVBELN As String = Right("0000000000" & lblAuftragsnr.Text, 10)
                    Dim lblKategorie As Label = CType(grvrow.Cells(5).FindControl("lblKategorie"), Label)
                    Dim sKategorie As String = lblKategorie.Text
                    Dim ProtokollRow As DataRow = uebf.UploadedProtokolle.Select("ID='" & lfdNr & "'")(0)
                    Dim filename As String = System.IO.Path.GetFileName(FileUpload.FileName)
                    'alt'Dim filepath As String = ConfigurationManager.AppSettings("UploadPathSambaArchive") + m_User.KUNNR.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    'alt'Dim filepath As String = "D:\Dokumente\" + m_User.KUNNR.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"

                    Dim filepath As String
                    If useLocalTestPath Then
                        filepath = "D:\Dokumente\" + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    Else
                        filepath = ConfigurationManager.AppSettings("UploadPathSambaArchive") + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    End If

                    filename = sVBELN + "_" + Fahrt + "_" + sKategorie + "_" + DokArt + ".pdf"
                    If Not System.IO.Directory.Exists(filepath) Then
                        System.IO.Directory.CreateDirectory(filepath)
                    End If
                    FileUpload.SaveAs(filepath + "\" + filename)

                    ProtokollRow("Filename") = filename
                    ProtokollRow("Filepath") = filepath & "/" & filename

                    If ProtokollRow("LVORM").ToString = "A" Then
                        ProtokollRow("LVORM") = "U"
                    Else
                        ProtokollRow("LVORM") = "N"
                        saveProtokolls(uebf, ProtokollRow)
                        ProtokollRow("LVORM") = "A"
                    End If

                    Dim table As DataTable = uebf.Result
                    Dim auftraegeRow As DataRow

                    'alt'Dim fpath As String = "D:\Dokumente\" + m_User.KUNNR.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    'alt'Dim fpath As String = ConfigurationManager.AppSettings("UploadPathSambaArchive") + m_User.KUNNR.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"


                    Dim fpath As String
                    If useLocalTestPath Then
                        fpath = "D:\Dokumente\" + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    Else
                        fpath = ConfigurationManager.AppSettings("UploadPathSambaArchive") + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    End If

                    Dim files() As String = System.IO.Directory.GetFiles(fpath, sVBELN & "_" & Fahrt & "*")

                    If table.Select("Aufnr='" & lblAuftragsnr.Text & "' AND Fahrtnr='" & Fahrt & "'").Length > 0 Then
                        auftraegeRow = table.Select(" Aufnr='" & lblAuftragsnr.Text & "' AND Fahrtnr='" & Fahrt & "'")(0)
                        Select Case files.Length
                            Case 1
                                auftraegeRow("Dokumente") = "1 Dokument vorhanden"
                            Case Is > 1
                                auftraegeRow("Dokumente") = files.Length & " Dokument vorhanden"
                            Case 0
                                auftraegeRow("Dokumente") = "keine Dokumente vorhanden"
                        End Select

                    End If
                    Session("LogUeberf") = uebf
                    uebf = Nothing
                End If
            End If

        End If
    End Sub

    Protected Sub UploadedComplete(ByVal sender As Object, ByVal e As FileUploadedEventArgs)
        'Dim FileUpload As AjaxControlToolkit.AsyncFileUpload = sender
        Dim FileUpload = DirectCast(sender, RadAsyncUpload)

        If Not FileUpload Is Nothing Then

            If FileUpload.UploadedFiles.Count > 0 Then
                If e.File.ContentType = "application/pdf" Then
                    Dim uebf As LogUeberf = CType(Session("LogUeberf"), LogUeberf)

                    'Dim grvrow As GridViewRow = grvProtokollUpload1.Rows(CInt(FileUpload.ToolTip))
                    Dim grvrow As GridViewRow = DirectCast(FileUpload.BindingContainer, GridViewRow)

                    Dim lblFahrt As Label = CType(grvrow.Cells(1).FindControl("lblFahrt"), Label)
                    Dim Fahrt As String = lblFahrt.Text
                    Dim lfdNr As String = grvrow.Cells(0).Text
                    Dim DokArt As String = grvrow.Cells(2).Text
                    Dim sVBELN As String = Right("0000000000" & lblAuftragsnr.Text, 10)
                    Dim lblKategorie As Label = CType(grvrow.Cells(5).FindControl("lblKategorie"), Label)
                    Dim sKategorie As String = lblKategorie.Text
                    Dim ProtokollRow As DataRow = uebf.UploadedProtokolle.Select("ID='" & lfdNr & "'")(0)
                    Dim filename As String = System.IO.Path.GetFileName(e.File.FileName)

                    Dim filepath As String = String.Empty
                    If useLocalTestPath Then
                        filepath = "D:\Dokumente\" + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    Else
                        filepath = ConfigurationManager.AppSettings("UploadPathSambaArchive") + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    End If

                    filename = sVBELN + "_" + Fahrt + "_" + sKategorie + "_" + DokArt + ".pdf"
                    If Not System.IO.Directory.Exists(filepath) Then
                        System.IO.Directory.CreateDirectory(filepath)
                    End If

                    e.File.SaveAs(filepath + "\" + filename)

                    Dim lbltmp As Label = grvrow.FindControl("lblUplFile")
                    lbltmp.Text = filename
                    lbltmp.Visible = True
                    grvrow.FindControl("ibtnDelUploadFile1").Visible = True
                    grvrow.FindControl("radUpload1").Visible = False

                    ProtokollRow("Filename") = filename
                    ProtokollRow("Filepath") = filepath & "/" & filename

                    If ProtokollRow("LVORM").ToString = "A" Then
                        ProtokollRow("LVORM") = "U"
                    Else
                        ProtokollRow("LVORM") = "N"
                        saveProtokolls(uebf, ProtokollRow)
                        ProtokollRow("LVORM") = "A"
                    End If

                    Dim table As DataTable = uebf.Result
                    Dim auftraegeRow As DataRow

                    Dim fpath As String = String.Empty

                    If useLocalTestPath Then
                        fpath = "D:\Dokumente\" + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    Else
                        fpath = ConfigurationManager.AppSettings("UploadPathSambaArchive") + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                    End If

                    Dim files() As String = System.IO.Directory.GetFiles(fpath, sVBELN & "_" & Fahrt & "*")

                    If table.Select("Aufnr='" & lblAuftragsnr.Text & "' AND Fahrtnr='" & Fahrt & "'").Length > 0 Then
                        auftraegeRow = table.Select(" Aufnr='" & lblAuftragsnr.Text & "' AND Fahrtnr='" & Fahrt & "'")(0)
                        Select Case files.Length
                            Case 1
                                auftraegeRow("Dokumente") = "1 Dokument vorhanden"
                            Case Is > 1
                                auftraegeRow("Dokumente") = files.Length & " Dokument vorhanden"
                            Case 0
                                auftraegeRow("Dokumente") = "keine Dokumente vorhanden"
                        End Select

                    End If
                    Session("LogUeberf") = uebf
                    uebf = Nothing

                End If
            End If

        End If
    End Sub

#End Region

#Region "GridView Events"

    Private Sub gvUeberf_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUeberf.RowCommand

        If e.CommandName = "UploadShow" Then

            Dim uebf As LogUeberf = CType(Session("LogUeberf"), LogUeberf)

            If Not uebf.UploadedProtokolle Is Nothing Then
                If Not uebf.UploadedProtokolle.Rows.Count = 0 Then
                    Dim dRows() As DataRow = uebf.Result.Select("Counter='" & e.CommandArgument & "'")
                    If dRows.Length > 0 Then
                        FillGridProtokolle(uebf.UploadedProtokolle, dRows(0)("Aufnr").ToString, dRows(0)("Fahrtnr").ToString)
                        If dRows(0)("Fahrtnr").ToString = "1" Then
                            lblFahrt.Text = "Hinfahrt"
                        ElseIf dRows(0)("Fahrtnr").ToString = "2" Then
                            lblFahrt.Text = "Rückfahrt"
                        End If

                        lblFahrtnr.Text = dRows(0)("Fahrtnr").ToString
                        lblAuftragsnr.Text = dRows(0)("Aufnr").ToString

                        Panel1.Visible = False
                        Result.Visible = False
                        lbCreate.Visible = False
                        Panel3.Visible = False
                        Panel2.Visible = True

                    End If

                End If
            End If

        End If
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

    Private Sub grvProtokollUpload1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvProtokollUpload1.RowCommand

        Dim uebf As LogUeberf = CType(Session("LogUeberf"), LogUeberf)
        Dim fullPath As String = ""

        If e.CommandName = "Loeschen" Then
            Try
                Dim lfdNr As String = e.CommandArgument
                Dim ProtokollRow As DataRow = uebf.UploadedProtokolle.Select("ID='" & lfdNr & "'")(0)
                fullPath = ProtokollRow("Filepath").ToString
                If System.IO.File.Exists(fullPath) Then
                    System.IO.File.Delete(fullPath)
                End If

                ProtokollRow("Filepath") = ""
                ProtokollRow("Filename") = ""
                If Not ProtokollRow("LVORM") = "N" Then
                    ProtokollRow("LVORM") = "X"
                    saveProtokolls(uebf, ProtokollRow)
                End If
                Dim sVBELN As String = Right("0000000000" & lblAuftragsnr.Text, 10)
                Dim Fahrt As String = lblFahrtnr.Text

                uebf.UploadedProtokolle.AcceptChanges()
                Dim table As DataTable = uebf.Result
                Dim auftraegeRow As DataRow

                'alt'Dim fpath As String = "D:\Dokumente\" + m_User.KUNNR.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                'alt'Dim fpath As String = ConfigurationManager.AppSettings("UploadPathSambaArchive") + m_User.KUNNR.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"

                Dim fpath As String = String.Empty
                If useLocalTestPath Then
                    fpath = "D:\Dokumente\" + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                Else
                    fpath = ConfigurationManager.AppSettings("UploadPathSambaArchive") + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                End If

                Dim files() As String = System.IO.Directory.GetFiles(fpath, sVBELN & "_" & Fahrt & "*")

                If table.Select("Aufnr='" & lblAuftragsnr.Text & "' AND Fahrtnr='" & Fahrt & "'").Length > 0 Then
                    auftraegeRow = table.Select(" Aufnr='" & lblAuftragsnr.Text & "' AND Fahrtnr='" & Fahrt & "'")(0)
                    Select Case files.Length
                        Case 1
                            auftraegeRow("Dokumente") = "1 Dokument vorhanden"
                        Case Is > 1
                            auftraegeRow("Dokumente") = files.Length & " Dokument vorhanden"
                        Case 0
                            auftraegeRow("Dokumente") = "keine Dokumente vorhanden"
                    End Select
                End If

                Session("LogUeberf") = uebf
                FillGridProtokolle(uebf.UploadedProtokolle, lblAuftragsnr.Text, lblFahrtnr.Text)
                'End If
            Catch ex As Exception
                lblUploadMessage1.Text = "Fehler beim Löschen der Datei: " & fullPath
            End Try

        End If
    End Sub


#End Region

#Region "Methods"

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

            If String.IsNullOrEmpty(txtAuftrag.Text.Trim()) And String.IsNullOrEmpty(txtAuftragdatum.Text.Trim()) And _
                String.IsNullOrEmpty(txtReferenz.Text.Trim()) And String.IsNullOrEmpty(txtKennzeichen.Text.Trim()) And _
               String.IsNullOrEmpty(txtAuftragdatumBis.Text.Trim()) And String.IsNullOrEmpty(txtUeberfuehrungdatumVon.Text.Trim()) And _
               String.IsNullOrEmpty(txtUeberfuehrungdatumBis.Text.Trim()) And _
               String.IsNullOrEmpty(txt_Leasinggesellschaft.Text.Trim()) And String.IsNullOrEmpty(txt_Leasingkunde.Text.Trim()) Then

                lblError.Text = "Bitte erst ein Filterwert eingeben."
                lblError.Visible = True

                Return

            End If


            uebf.readSAPAuftraege2(txtAuftrag.Text, txtAuftragdatum.Text, txtAuftragdatumBis.Text, _
                                   txtReferenz.Text, txtKennzeichen.Text, txtUeberfuehrungdatumVon.Text, _
                                   txtUeberfuehrungdatumBis.Text, "O", kunnr, _
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
                'uebf.getProtokollarten(m_User, Me)
                uebf.getProtokollarten(getKundennummer(), Me)
                Dim boolCanUpload As Boolean = False
                If Not uebf.ProtokollArten Is Nothing Then
                    If uebf.ProtokollArten.Rows.Count > 0 Then
                        boolCanUpload = True
                        For Each AuftragsRow In table.Rows
                            For Each Row As DataRow In uebf.ProtokollArten.Rows
                                Dim UploadRow As DataRow = uebf.UploadedProtokolle.NewRow
                                UploadRow("ZZKUNNR") = Row("ZZKUNNR")
                                UploadRow("ZZKATEGORIE") = Row("ZZKATEGORIE")
                                UploadRow("ZZPROTOKOLLART") = Row("ZZPROTOKOLLART")
                                UploadRow("ID") = uebf.UploadedProtokolle.Rows.Count + 1
                                UploadRow("Filename") = Row("Filename")
                                UploadRow("Filepath") = Row("Filepath")
                                UploadRow("Fahrt") = AuftragsRow("Fahrtnr")
                                UploadRow("LVORM") = ""
                                UploadRow("Auftr") = Right("0000000000" & AuftragsRow("AUFNR").ToString, 10)
                                uebf.UploadedProtokolle.Rows.Add(UploadRow)
                            Next
                        Next

                    End If
                End If
                If boolCanUpload = True Then
                    Dim files As String()
                    Dim info As System.IO.FileInfo
                    Dim fname As String

                    Dim sSub() As String
                    For Each auftraegeRow As DataRow In table.Rows
                        Dim sVBELN As String = Right("0000000000" & auftraegeRow("AUFNR").ToString, 10)
                        'alt' Dim fpath As String = "D:\Dokumente\" + m_User.KUNNR.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                        'alt'Dim fpath As String = ConfigurationManager.AppSettings("UploadPathSambaArchive") + m_User.KUNNR.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"

                        Dim fpath As String = String.Empty
                        If useLocalTestPath Then
                            fpath = "D:\Dokumente\" + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                        Else
                            fpath = ConfigurationManager.AppSettings("UploadPathSambaArchive") + getKundennummer().PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                        End If

                        Dim sFahrtnr As String = auftraegeRow("Fahrtnr").ToString

                        If System.IO.Directory.Exists(fpath) Then
                            files = System.IO.Directory.GetFiles(fpath, sVBELN & "_" & sFahrtnr & "*")

                            For i = 0 To files.Length - 1
                                info = New System.IO.FileInfo(files.GetValue(i).ToString)
                                fname = info.Name

                                sSub = Split(fname, "_")
                                If Not sSub(1) Is Nothing Then
                                    Select Case files.Length
                                        Case 1
                                            If auftraegeRow("Fahrtnr") = sSub(1) Then
                                                auftraegeRow("Dokumente") = "1 Dokument vorhanden"
                                                Dim UploadRow() As DataRow = uebf.UploadedProtokolle.Select("Auftr = '" & sVBELN & "' AND Fahrt='" & sFahrtnr & "'")

                                                For Each tmpRow As DataRow In UploadRow
                                                    'If tmpRow("ZZKATEGORIE") = sSub(2).ToString Then
                                                    If tmpRow("ZZPROTOKOLLART") = sSub(3).ToString.ToString.Replace(".pdf", "").Replace(".PDF", "") Then
                                                        tmpRow("Filename") = fname
                                                        tmpRow("Filepath") = info.FullName
                                                        tmpRow("LVORM") = "A"
                                                        uebf.UploadedProtokolle.AcceptChanges()
                                                    End If
                                                Next
                                            Else
                                                auftraegeRow("Dokumente") = "Keine Dokumente vorhanden"
                                            End If

                                        Case Is > 1
                                            If auftraegeRow("Fahrtnr") = sSub(1) Then
                                                auftraegeRow("Dokumente") = files.Length & " Dokumente vorhanden"
                                                Dim UploadRow() As DataRow = uebf.UploadedProtokolle.Select("Auftr = '" & sVBELN & "' AND Fahrt='" & sFahrtnr & "'")

                                                For Each tmpRow As DataRow In UploadRow


                                                    'If tmpRow("ZZKATEGORIE") = sSub(2).ToString Then
                                                    If tmpRow("ZZPROTOKOLLART") = sSub(3).ToString.Replace(".pdf", "").Replace(".PDF", "") Then
                                                        tmpRow("Filename") = fname
                                                        tmpRow("Filepath") = info.FullName
                                                        tmpRow("LVORM") = "A"
                                                        uebf.UploadedProtokolle.AcceptChanges()

                                                    End If
                                                Next

                                            Else
                                                auftraegeRow("Dokumente") = "Keine Dokumente vorhanden"
                                            End If

                                    End Select
                                End If

                            Next
                            If files.Length = 0 Then
                                auftraegeRow("Dokumente") = "Keine Dokumente vorhanden"
                            End If
                        Else
                            auftraegeRow("Dokumente") = "Keine Dokumente vorhanden"
                        End If
                    Next
                    Session("LogUeberf") = uebf
                    Session("ResultTable") = table
                    FillGrid(0)
                Else
                    Session("ResultTable") = Nothing
                    FillGrid(0)
                    lblError.Text = uebf.Message
                    lblError.Visible = True
                End If


                Session("LogUeberf") = uebf
                Session("ResultTable") = table
                FillGrid(0)

            Else

                lblError.Text = "Keine Daten gefunden."
                lblError.Visible = True

            End If


        Catch ex As Exception
            lblError.Visible = True
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
            lblError.Visible = True
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

            Dim Status As String

            For Each dr As GridViewRow In gvUeberf.Rows

                Status = table.Select("Aufnr = '" & dr.Cells(3).Text & "'")(0)("FAHRER_STATUS").ToString

                If String.IsNullOrEmpty(Status) = False Then

                    If Status = "OK" Then
                        For i As Int16 = 0 To dr.Cells.Count - 1
                            dr.Cells(i).ForeColor = Drawing.Color.Red
                        Next


                    End If


                End If

            Next


            lblError.Visible = True

        End If
    End Sub

    Private Sub FillGridProtokolle(ByVal tblProtokolle As DataTable, ByVal sAuftr As String, ByVal sFahrt As String)
        Dim tmpDataView As New DataView()
        tmpDataView = tblProtokolle.DefaultView
        tmpDataView.RowFilter = "Fahrt='" & sFahrt & "' And Auftr='" & Right("0000000000" & sAuftr, 10) & "'"
        If tmpDataView.Count = 0 Then
            grvProtokollUpload1.Visible = False
        Else
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            grvProtokollUpload1.Visible = True

            grvProtokollUpload1.DataSource = tmpDataView
            grvProtokollUpload1.DataBind()

            Dim table As DataTable
            table = CType(Session("ResultTable"), DataTable)

            Dim Status As String
            Dim ibt As ImageButton
            Dim afu As AjaxControlToolkit.AsyncFileUpload

            For Each dr As GridViewRow In grvProtokollUpload1.Rows

                Status = table.Select("Aufnr = '" & sAuftr & "'")(0)("FAHRER_STATUS").ToString

                If String.IsNullOrEmpty(Status) = False Then

                    If Status = "OK" Then
                        ibt = CType(dr.FindControl("ibtnDelUploadFile1"), ImageButton)
                        afu = CType(dr.FindControl("AsyncFileUpload1"), AjaxControlToolkit.AsyncFileUpload)
                        ibt.Visible = False
                        afu.Visible = False
                    End If


                End If

            Next



        End If
    End Sub

    Private Function saveProtokolls(ByVal uebf As LogUeberf, ByVal UpdateRow As DataRow) As Boolean

        uebf.UpdateProtokollarten(m_User, Me, UpdateRow)
        If uebf.Status <> 0 Then
            lblUploadMessage1.Text = uebf.Message
            Return False
        Else
            Return True
        End If
    End Function

    Private Function getKundennummer() As String

        Dim Kundennummer As String = m_User.KUNNR

        If m_User.Store = "AUTOHAUS" AndAlso m_User.Reference.Trim(" "c).Length > 0 AndAlso m_User.KUNNR = "261510" Then
            Kundennummer = m_User.Reference
        End If

        Return Kundennummer

    End Function

#End Region


End Class