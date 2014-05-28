Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Change01
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable
    Private m_objExcel As DataTable

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Info As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHidden As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    'Private schmal As String
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cmdweiter As System.Web.UI.WebControls.LinkButton
    Dim mChange As Nacherfassung

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
        m_App = New Base.Kernel.Security.App(m_User)

        Try


            If Not IsPostBack Then
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                mChange = New Nacherfassung(m_User, m_App, "")
                mChange.Fill(Session("AppID").ToString, Session.SessionID)
                If Not mChange.Status = 0 Then
                    lblError.Text = mChange.Message
                    lblError.Visible = True
                    Exit Sub
                End If

                m_objExcel = mChange.Result.Copy
                m_objExcel.Columns.Remove("EQUNR")
                For Each tmpRow As DataRow In m_objExcel.Rows
                    tmpRow("BRANDING") = getBranding(tmpRow("BRANDING").ToString)
                Next
                m_objExcel.AcceptChanges()
                Session.Add("ExcelTabelle", m_objExcel)

                mChange.ResultTable = mChange.Result
                mChange.ResultTable.Columns.Add("Zuordnen", System.Type.GetType("System.Boolean"))
                mChange.ResultTable.AcceptChanges()
                m_objTable = mChange.ResultTable
                Session("ObjNacherfassungSession") = mChange
                Session("ResultTable") = m_objTable


                If Not Session("ApplblInfoText") Is Nothing Then
                    If lbl_Info.Text.Length = 0 Then
                        lbl_Info.Text = CStr(Session("ApplblInfoText"))
                    Else
                        lbl_Info.Text &= "<br>" & CStr(Session("ApplblInfoText"))
                    End If
                End If

                FillGrid(0)

            Else  'wenn Postback
                If mChange Is Nothing Then
                    mChange = CType(Session("ObjNacherfassungSession"), Nacherfassung)
                End If
                If m_objTable Is Nothing Then
                    m_objTable = CType(Session("ResultTable"), DataTable)
                End If
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub doNoDataAktion()
        lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
        cmdweiter.Visible = False
        lnkCreateExcel.Visible = False
        lblError.Visible = True
        DataGrid1.Visible = False
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If mChange Is Nothing Then
            mChange = CType(Session("ObjNacherfassungSession"), Nacherfassung)
        End If

        m_objTable = mChange.ResultTable

        If m_objTable.Rows.Count = 0 Then
            doNoDataAktion()
        Else


            DataGrid1.Visible = True
            lblNoData.Visible = False

            Dim tmpDataView As DataView = m_objTable.DefaultView

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

            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."

            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim label As Label
            Dim control As Control

            For Each item In DataGrid1.Items                    'Zeilen des DataGrids durchgehen...
                Dim datEingang As Date
                Dim daysBack As Long
                cell = item.Cells(5) 'Erste Spalte holen = CheckBox (nicht sichtbar)
                For Each control In cell.Controls
                    Dim control1 = TryCast(control, Label)
                    If (control1 IsNot Nothing) Then
                        label = control1
                        If label.ID = "Label6" Then
                            If IsDate(label.Text) Then
                                datEingang = CDate(label.Text)
                                label.Text = datEingang.ToShortDateString
                                daysBack = DateDiff(DateInterval.Day, datEingang, Now)
                                If daysBack >= 14 Then
                                    Dim i As Integer
                                    For i = 0 To item.Cells.Count - 1
                                        cell = item.Cells(i)
                                        cell.ForeColor = Drawing.Color.Red
                                    Next
                                End If
                            End If
                        End If
                    End If
                Next

            Next

        End If
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

    Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click
        Try

            If m_objExcel Is Nothing Then
                m_objExcel = CType(Session("ExcelTabelle"), DataTable)
            End If
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName



            excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_objExcel, Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub cmdweiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdweiter.Click
        Dim item As DataGridItem
        Dim tmpRows() As DataRow
        Dim bCheck As Boolean
        Dim sEQUNR As String
        Dim TempTable As DataTable = CType(Session("ResultTable"), DataTable)

        For Each item In DataGrid1.Items
            bCheck = CType(item.FindControl("chk_Order"), CheckBox).Checked
            If bCheck = True Then
                sEQUNR = item.Cells(0).Text
                tmpRows = TempTable.Select("EQUNR='" & sEQUNR & "'")
                tmpRows(0).BeginEdit()
                tmpRows(0).Item("Zuordnen") = True
                tmpRows(0).EndEdit()
                TempTable.AcceptChanges()
            End If

        Next

        tmpRows = TempTable.Select("Zuordnen=True")
        If Not CType(tmpRows.Length, Integer) > 0 Then
            lblError.Text = "Keine Dokumente ausgewählt!"
        Else
            Session("ResultTable") = TempTable
            Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Public Function getBranding(ByVal kuerzel As String) As String
        If Not kuerzel Is String.Empty AndAlso Not kuerzel = "" Then
            If mChange Is Nothing Then
                mChange = CType(Session("ObjNacherfassungSession"), Nacherfassung)
            End If
            If Not mChange.Brandings.Select("ZZLABEL='" & kuerzel & "'").Length = 0 Then
                Return mChange.Brandings.Select("ZZLABEL='" & kuerzel & "'")(0)("BRANDING").ToString
            Else
                Return ""
            End If
        End If
        Return String.Empty
    End Function

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub
End Class
' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:09
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Warnungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 26.11.08   Time: 12:57
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2427 testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 24.09.08   Time: 17:46
' Updated in $/CKAG/Applications/AppBPLG/Forms
' doppeltes PageLoad entfernt
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 11.08.08   Time: 17:54
' Updated in $/CKAG/Applications/AppBPLG/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 18.07.08   Time: 10:36
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2069 fertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 17.07.08   Time: 15:52
' Updated in $/CKAG/Applications/AppBPLG/Forms
' killAllDBNullValuesInDataTable methode hinzugefügt
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.07.08   Time: 12:33
' Created in $/CKAG/Applications/AppBPLG/Forms
' Erstellung ITA 2069
' 
' ************************************************
