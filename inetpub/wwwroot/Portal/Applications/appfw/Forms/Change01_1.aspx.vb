Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb

Public Class Change01_1
    Inherits System.Web.UI.Page

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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objHandler As Versandbeauftragung

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents cmdNeueVIN As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtVIN As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblExcelfile As System.Web.UI.WebControls.Label
    Protected WithEvents cmdContinue As System.Web.UI.WebControls.LinkButton
    Protected WithEvents tblUpload As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents cmdUpload As System.Web.UI.WebControls.LinkButton
    Protected WithEvents upFile As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents lbDateiHinzufuegen As LinkButton



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkKreditlimit.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHandler") Is Nothing Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHandler = CType(Session("objHandler"), Versandbeauftragung)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("250")
                ddlPageSize.SelectedIndex = 2

                'DataGrid1.BackColor = System.Drawing.Color.DarkSeaGreen
                FillGrid(0, , True)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Try
            Dim tmpDataView As New DataView()
            tmpDataView = objHandler.Fahrzeuge.DefaultView

            tmpDataView.RowFilter = ""
            Dim intFahrzeugBriefe As Int32 = tmpDataView.Count

            If intFahrzeugBriefe = 0 Then
                lblError.Text = "Bitte erfassen Sie zunächst Fahrgestellnummern zur Beauftragung."
                FillGrid(DataGrid1.CurrentPageIndex)
            Else
                Session("objHandler") = objHandler
                Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Aufbau der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()
        tmpDataView = objHandler.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            cmdSave.Enabled = False
            ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
        Else
            DataGrid1.Visible = True
            cmdSave.Enabled = True
            ddlPageSize.Visible = True
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String

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

            lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " Fahrgestellnummer(n) erfasst."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        Try
            DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
            FillGrid(0)
        Catch ex As Exception
            lblError.Text = "Beim Aufbau der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        Try
            FillGrid(e.NewPageIndex)
        Catch ex As Exception
            lblError.Text = "Beim Aufbau der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        Try
            FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
        Catch ex As Exception
            lblError.Text = "Beim Aufbau der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub addFahrgestellnummernRangePerUpload()
        'Prüfe Fehlerbedingung
        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            lblExcelfile.Text = upFile.PostedFile.FileName
            If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                Exit Sub
            End If
        Else
            lblError.Text = "Keine Datei ausgewählt"
            Exit Sub
        End If
        'Lade Datei
        upload(upFile.PostedFile)
    End Sub



    Private Function getDataTableFromExcel(ByVal filepath As String, ByVal filename As String) As DataTable
        '----------------------------------------------------------------------
        ' Methode: GetDataTable
        ' Autor: JJU 
        ' Beschreibung: extrahiert die Daten aus dem ersten Exceltabellen-Blatt in eine Datatable
        ' Erstellt am: 2008.09.22
        ' ITA: 1844
        '----------------------------------------------------------------------

        Dim objDataset1 As New DataSet()
        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                         "Data Source=" & filepath & filename & ";Extended Properties=""Excel 8.0;HDR=No"""
        Dim objConn As New OleDbConnection(sConnectionString)
        objConn.Open()

        Dim schemaTable As DataTable
        Dim tmpObj() As Object = {Nothing, Nothing, Nothing, "Table"}
        schemaTable = objConn.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, tmpObj)

        For Each sheet As DataRow In schemaTable.Rows
            Dim tableName As String = sheet("Table_Name").ToString
            Dim objCmdSelect As New OleDbCommand("SELECT * FROM [" & tableName & "]", objConn)
            Dim objAdapter1 As New OleDbDataAdapter(objCmdSelect)
            objAdapter1.Fill(objDataset1, tableName)
        Next
        Dim tblTemp As DataTable = objDataset1.Tables(0)
        objConn.Close()
        Return tblTemp
    End Function

    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)
        Try
            Dim filepath As String = ConfigurationSettings.AppSettings("UploadpathLocal")
            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationSettings.AppSettings("UploadpathLocal") & filename)
                uFile = Nothing
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    lblError.Text = "Fehler beim Speichern."
                    Exit Sub
                End If
                'Datei gespeichert -> Auswertung
                Dim tblTemp As DataTable = getDataTableFromExcel(filepath, filename)
                If Not tblTemp Is Nothing AndAlso Not tblTemp.Rows.Count = 0 Then
                    For Each tmpRow As DataRow In tblTemp.Rows
                        addOneFahrgestellnummerToTable(tmpRow(0).ToString)
                    Next
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message & " / " & ex.StackTrace
        Finally
        End Try
    End Sub

    Private Sub cmdNeueVIN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNeueVIN.Click
        Try
            txtVIN.Text = generateFullChassisNum(txtVIN.Text)
            If Not txtVIN.Text.Trim(" "c).Length = 17 Then
                lblError.Text = "Bitte geben Sie eine 17-stellige Fahrgestellnummer ein."
            Else
                addOneFahrgestellnummerToTable(txtVIN.Text)
                txtVIN.Text = "WF0_XX"
            End If
        Catch ex As Exception
            lblError.Text = "Beim Hinzufügen der Fahrgestellnummer ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function generateFullChassisNum(ByVal FGSNR As String) As String
        'Durch der Platzhalter _ soll durch die 10. Stelle einer kompletten Fahrgestelltnumemr ersetzt werden. JJ2007.11.28

        If FGSNR.Trim(" "c).Length = 17 AndAlso Not FGSNR.IndexOf("_") = -1 AndAlso FGSNR.IndexOf("_") = 3 Then
            FGSNR = FGSNR.Replace("_", FGSNR.Chars(9))
        End If

        Return FGSNR

    End Function

    Private Sub addOneFahrgestellnummerToTable(ByVal fahrgestellnummer As String)
        Try
            Dim tmpRows() As DataRow = objHandler.Fahrzeuge.Select("Chassis_Num='" & Left(fahrgestellnummer.Trim(" "c), 17) & "'")

            If tmpRows.Length = 1 Then
                Throw New Exception("Fahrgestellnummer wurde bereits erfasst. " & fahrgestellnummer)
            Else
                Dim rowNew As DataRow = objHandler.Fahrzeuge.NewRow
                rowNew("Chassis_Num") = Left(fahrgestellnummer.Trim(" "c), 17)
                rowNew("Ernam") = Left(m_User.UserName, 12)
                rowNew("KUNNR_ZS") = objHandler.SucheHaendlernummer
                rowNew("Name1") = objHandler.EmpfaengerName
                rowNew("City1") = objHandler.EmpfaengerOrt
                rowNew("Post_Code1") = objHandler.EmpfaengerPLZ
                rowNew("Street") = objHandler.EmpfaengerStrasse
                rowNew("VERMARKT") = objHandler.Vermarktung
                rowNew("EIGENTUEMER") = objHandler.Eigentuemer
                rowNew("CODE_VERSANDART") = objHandler.Versandart
                objHandler.Fahrzeuge.Rows.Add(rowNew)
                objHandler.Fahrzeuge.AcceptChanges()
                FillGrid(0)
                Session("objHandler") = objHandler
            End If
        Catch ex As Exception
            lblError.Text = "Beim Hinzufügen der Fahrgestellnummer ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Try
            Select Case e.CommandName
                Case "Delete"
                    Dim cell As TableCell = e.Item.Cells(0)
                    objHandler.Fahrzeuge.Select("CHASSIS_NUM='" & cell.Text & "'")(0).Delete()
                    objHandler.Fahrzeuge.AcceptChanges()
                    FillGrid(0)
                    Session("objHandler") = objHandler
                Case Else
                    'Tu nix
            End Select
        Catch ex As Exception
            lblError.Text = "Beim Löschen der Fahrgestellnummer ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lbDateiHinzufuegen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbDateiHinzufuegen.Click
        addFahrgestellnummernRangePerUpload()
    End Sub
End Class
' ************************************************
' $History: Change01_1.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.09.08   Time: 9:45
' Updated in $/CKAG/Applications/appfw/Forms
' ITA 1844 Excel upload statt csv
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 19.09.08   Time: 15:43
' Updated in $/CKAG/Applications/appfw/Forms
' ITA 1844 anpassungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 9.09.08    Time: 11:27
' Updated in $/CKAG/Applications/appfw/Forms
' ITA 1844
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 8.09.08    Time: 17:39
' Updated in $/CKAG/Applications/appfw/Forms
' ITA 1844 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 3.09.08    Time: 10:01
' Updated in $/CKAG/Applications/appfw/Forms
' ITa 1844 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 1.09.08    Time: 17:40
' Created in $/CKAG/Applications/appfw/Forms
' ITa 1844 Compilierfähig
' 
' ************************************************