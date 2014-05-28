Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports Microsoft.VisualBasic

Public Class Report04
    Inherits System.Web.UI.Page


#Region "Events"
    Public Event SortCommand As DataGridSortCommandEventHandler

#End Region
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
#Region "Members"

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_Report As VW_08
    Private m_DataSource As DataTable

#End Region

    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trSelectDropdown As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtIKZNummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVorhabennummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUebergabedatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUebergabedatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRechnungsnummern As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdSelection As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents RangeValidatorX As System.Web.UI.WebControls.RangeValidator
    Protected WithEvents GridPanel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected m_ErrorText As String

#Region "Properties"
    
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Session("ShowLink") = "False"
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

        'initial Datumsbelegung auf heute bzw -1Monat
        If Page.IsPostBack = False Then
            txtUebergabedatumBis.Text = Now.ToShortDateString()
            txtUebergabedatumVon.Text = Date.Now.AddMonths(-1).ToShortDateString()
        End If
    End Sub

    Private Sub createExcel()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim resultTable As New DataTable()
            resultTable = CType(Session("Source"), DataTable)

            If Not resultTable.Rows.Count <= 0 Then

                Try
                    resultTable = formatAllColumsToStringForExcelReport(resultTable)

                    Excel.ExcelExport.WriteExcel(resultTable, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch
                End Try
                Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    Private Sub cmdSelection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelection.Click

        Dim inputIsValid As Boolean = False
        If txtVorhabennummer.Text = "" And txtIKZNummer.Text = "" And txtRechnungsnummern.Text = "" Then
            If HelpProcedures.checkDate(txtUebergabedatumVon, txtUebergabedatumBis, m_ErrorText, False, 1) Then
                inputIsValid = True
            End If
        Else

            inputIsValid = HelpProcedures.checkDate(txtUebergabedatumVon, txtUebergabedatumBis, m_ErrorText, True, 0)

        End If

        If inputIsValid Then
            calVon.Visible = False
            calBis.Visible = False

            Dim alSelectionCriteria As New ArrayList()
            With alSelectionCriteria
                .Add(txtVorhabennummer.Text)
                .Add(txtIKZNummer.Text)
                .Add(txtUebergabedatumVon.Text)
                .Add(txtUebergabedatumBis.Text)
                .Add(txtRechnungsnummern.Text)
            End With


            m_Report = New VW_08(m_User, m_App, "")

            m_DataSource = m_Report.fillRechnungsPruefungsGrid(alSelectionCriteria)
            lblError.Text = m_Report.Message

            If Not m_DataSource Is Nothing Then

                Session("Source") = m_DataSource
                m_Report.CreateOutPut(CType(Session("Source"), DataTable), Session("AppID"))
                Session("Source") = m_Report.Result
                DataGrid1.DataSource = Session("Source")
                DataGrid1.DataBind()
            Else
                disposeGrid(DataGrid1)
            End If
        Else
            lblError.Text = m_ErrorText
            disposeGrid(DataGrid1)
        End If


        If DataGrid1.Items.Count > 0 Then
            createExcel()

            If (Not Session("lnkExcel") Is Nothing) AndAlso (Not Session("lnkExcel").ToString.Length = 0) Then
                lnkExcel.NavigateUrl = Session("lnkExcel").ToString
                lblDownloadTip.Visible = True
                lnkExcel.Visible = True
            End If
        Else
            lblDownloadTip.Visible = False
            lnkExcel.Visible = False
        End If



    End Sub

    Private Sub disposeGrid(ByRef grid As DataGrid)
        grid.DataSource = Nothing
        grid.DataBind()
    End Sub

    Private Function formatAllColumsToStringForExcelReport(ByVal tmpTable As DataTable) As DataTable

        Dim tmpColumn As DataColumn
        Dim tmpTable2 As New DataTable()
        Dim tmpRow2 As DataRow
        Dim tmpRow As DataRow
        Dim i As Int32 = 0



        Try

            For Each tmpColumn In tmpTable.Columns
                If Not tmpColumn.DataType.Name = "String" Then
                    tmpTable2.Columns.Add(tmpColumn.ColumnName, System.Type.GetType("System.String"), tmpColumn.Expression)

                Else
                    tmpTable2.Columns.Add(tmpColumn.ColumnName, System.Type.GetType("System.String"), tmpColumn.Expression)
                End If
                tmpTable2.AcceptChanges()
            Next


            For Each tmpRow In tmpTable.Rows
                tmpRow2 = tmpTable2.NewRow
                For i = 0 To tmpRow.ItemArray.Length - 1

                    tmpRow2.BeginEdit()
                    If tmpRow.Item(i).GetType.Equals(System.Type.GetType("System.DateTime")) = True Then
                        tmpRow2.Item(i) = CDate(tmpRow.Item(i)).ToShortDateString
                    Else
                        tmpRow2.Item(i) = tmpRow.Item(i)
                    End If
                    tmpRow2.EndEdit()

                Next i
                tmpTable2.Rows.Add(tmpRow2)
            Next
        Catch e As Exception
            System.Diagnostics.Debug.Write(e.ToString)
        End Try

        Return tmpTable2
    End Function


    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand

        ' Create a DataView from the DataTable.
        Dim dv As DataView = New DataView(CType(Session("Source"), DataTable))


        If Viewstate.Item("exSortExpression") = Nothing Then
            Viewstate.Add("exSortExpression", e.SortExpression)
            Viewstate.Add("Sort", "asc")
        Else
            If Viewstate.Item("exSortExpression") = e.SortExpression Then
                If Viewstate.Item("Sort") = "asc" Then
                    Viewstate.Item("Sort") = "desc"
                ElseIf Viewstate.Item("Sort") = "desc" Then
                    Viewstate.Item("Sort") = "asc"
                End If
            Else
                Viewstate("exSortExpression") = e.SortExpression
                Viewstate("Sort") = "asc"
            End If
        End If


        dv.Sort = e.SortExpression & " " & ViewState.Item("Sort")
        'System.Diagnostics.Debug.WriteLine(dv.Sort)

        ' Rebind the data source and specify that it should be sorted
        ' by the field specified in the SortExpression property.
        DataGrid1.DataSource = dv
        DataGrid1.DataBind()

    End Sub




    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim m_objTable As New DataTable()
        m_objTable = CType(Session("Source"), DataTable)

        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub




    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtUebergabedatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtUebergabedatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub
End Class

' ************************************************
' $History: Report04.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:48
' Created in $/CKAG/Applications/appvw/Forms
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 31.10.07   Time: 10:43
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 31.10.07   Time: 10:26
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 30.10.07   Time: 14:52
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 30.10.07   Time: 13:56
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 30.10.07   Time: 13:33
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 30.10.07   Time: 12:55
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 30.10.07   Time: 12:39
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 5.10.07    Time: 10:08
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 4.10.07    Time: 17:32
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 4.10.07    Time: 13:36
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 2.10.07    Time: 17:51
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 2.10.07    Time: 8:12
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 28.09.07   Time: 15:50
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 28.09.07   Time: 12:23
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 27.09.07   Time: 17:51
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.09.07   Time: 15:45
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 25.09.07   Time: 16:14
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 25.09.07   Time: 13:14
' Created in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 


