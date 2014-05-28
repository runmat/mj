Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change01_2
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_intLineCount As Int32
    Private m_datatable As DataTable
    Private m_report As Carport
    Private m_objExcel As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User)
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If m_datatable Is Nothing Then
                m_datatable = CType(Session("App_ResultTable"), DataTable)
            End If

            If m_report Is Nothing Then
                m_report = CType(Session("App_Report"), Carport)
            End If


            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2
                FillGrid(0)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        If m_datatable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        Else
            lnkCreateExcel.Visible = True
            DataGrid1.Visible = True
            lblNoData.Visible = False


            'prüfen ob alle temp. selektion angeklickt wurde, dann mahndatum und mahnstufe anzeigen, wenn in Column Translation eingetragen wurde JJU2008.03.03


            Dim tmpDataView As New DataView()
            tmpDataView = m_datatable.DefaultView

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

            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Fahrzeuge gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
            Dim cell As TableCell
            Dim cell6 As TableCell
            Dim chkBox As CheckBox
            Dim control As Control
            Dim bEnabled As Boolean
            For Each item As DataGridItem In DataGrid1.Items
                bEnabled = False
                cell6 = item.Cells(7)
                If cell6.Text = "&nbsp;" Then
                    bEnabled = True
                    cmdSave.Visible = True
                End If
                cell = item.Cells(11)
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        chkBox.Enabled = bEnabled
                        chkBox.Checked = Not bEnabled
                    End If
                Next

            Next
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click

        Dim TableToSave As DataTable = m_report.GetSaveTable


        'TableToSave.Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
        'TableToSave.Columns.Add("ZZDAT_BER", System.Type.GetType("System.String"))
        'TableToSave.Columns.Add("EQUNR", System.Type.GetType("System.String"))
        'TableToSave.Columns.Add("QMNUM", System.Type.GetType("System.String"))

        Dim saverow As DataRow
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim control As Control
        Dim ChassisNum As String = ""

        For Each item As DataGridItem In DataGrid1.Items
            cell = item.Cells(11)

            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    If chkBox.Enabled = True AndAlso chkBox.Checked = True Then
                        saverow = TableToSave.NewRow
                        saverow("CHASSIS_NUM") = item.Cells(5).Text
                        ChassisNum = item.Cells(5).Text
                        saverow("ZZDAT_BER") = Now
                        item.Cells(7).Text = Now.ToShortDateString
                        saverow("EQUNR") = item.Cells(12).Text
                        saverow("QMNUM") = item.Cells(13).Text
                        TableToSave.Rows.Add(saverow)
                        TableToSave.AcceptChanges()
                        chkBox.Enabled = False
                    End If
                End If
            Next
        Next

        m_report.Save(Session("AppID").ToString, Session.SessionID.ToString)

        If m_report.Message <> "" Then
            lblError.Text = "Daten konnten nicht gespeichert werden!"
        Else
            Dim rows() As DataRow
            Dim updaterow As DataRow
            For i = 0 To TableToSave.Rows.Count - 1
                ChassisNum = TableToSave.Rows(i)("CHASSIS_NUM").ToString
                rows = m_report.Result.Select("Fahrgestellnummer='" & ChassisNum & "'")
                For Each updaterow In rows
                    updaterow("Datum_Bereit") = Now.ToShortDateString
                Next
                m_report.Result.AcceptChanges()
            Next

            lblNoData.Visible = True
            lblNoData.Text = "Ihre Daten wurden im System gespeichert!"
            cmdSave.Visible = False
        End If
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        CreateExcel()
    End Sub

    Private Sub CreateExcel()
        Dim ExcelTable As DataTable
        If Not (Session("App_ResultTable") Is Nothing) Then
            m_report.FilterExcel()
            ExcelTable = m_report.ResultExcel
            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, ExcelTable, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

End Class
' ************************************************
' $History: Change01_2.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 4.12.08    Time: 17:03
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.12.08    Time: 16:43
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2359 kleinere Anpassung
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.12.08    Time: 13:42
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 1.12.08    Time: 16:13
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 2359 & kleinere Anpassungen
' 
' ************************************************
