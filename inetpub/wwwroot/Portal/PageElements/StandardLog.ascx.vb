Imports CKG.Base.Kernel

Namespace PageElements
    Public MustInherit Class StandardLog
        Inherits System.Web.UI.UserControl

#Region "Declarations"
        Private m_context As HttpContext = HttpContext.Current
        Private m_strSessionID As String
        Private m_strUser As String
        Private m_blnShowDetails() As Boolean
        Private m_strConnectionString As String
        Private m_strHeader As String
        Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
        Protected WithEvents chkSession As System.Web.UI.WebControls.RadioButton
        Protected WithEvents Calendar1 As System.Web.UI.WebControls.Calendar
        Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
        Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
        Protected WithEvents lblHead As System.Web.UI.WebControls.Label
        Protected WithEvents chkUser As System.Web.UI.WebControls.RadioButton
        Private m_objTrace As Logging.Trace

#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

#Region "Properties"
        Public Property Header() As String
            Get
                Return m_strHeader
            End Get
            Set(ByVal Value As String)
                m_strHeader = Value
                lblHead.Text = m_strHeader
            End Set
        End Property

        Public Property SessionID() As String
            Get
                Return m_strSessionID
            End Get
            Set(ByVal Value As String)
                m_strSessionID = Value
            End Set
        End Property

        Public Property User() As String
            Get
                Return m_strUser
            End Get
            Set(ByVal Value As String)
                m_strUser = Value
            End Set
        End Property

        Public Property ConnectionString() As String
            Get
                Return m_strConnectionString
            End Get
            Set(ByVal Value As String)
                m_strConnectionString = Value
            End Set
        End Property
#End Region

#Region "Methods"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                lblError.Text = ""

                ReDim m_blnShowDetails(DataGrid1.PageSize)
                Dim i As Int32
                For i = 0 To DataGrid1.PageSize - 1
                    m_blnShowDetails(i) = False
                Next


                If Not IsPostBack Then
                    m_objTrace = New Logging.Trace(m_strConnectionString, False)
                    If m_objTrace.SessionData(m_strUser, m_strSessionID) Then
                        FillGrid(0)
                    Else
                        lblError.Text = "Fehler:<br>" & m_objTrace.ErrorMessage
                    End If
                    Session("m_objTrace") = m_objTrace
                    'm_context.Cache.Insert("m_objTrace", m_objTrace, New System.Web.Caching.CacheDependency(Server.MapPath("StandardLog.ascx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                Else
                    If Not Session("m_objTrace") Is Nothing Then
                        m_objTrace = CType(Session("m_objTrace"), Logging.Trace)
                        If chkSession.Checked And Calendar1.Visible Then
                            If m_objTrace.SessionData(m_strUser, m_strSessionID) Then
                                FillGrid(0)
                            Else
                                lblError.Text = "Fehler:<br>" & m_objTrace.ErrorMessage
                            End If
                            Session("m_objTrace") = m_objTrace
                            ' m_context.Cache.Insert("m_objTrace", m_objTrace, New System.Web.Caching.CacheDependency(Server.MapPath("StandardLog.ascx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                            Calendar1.Visible = False
                        ElseIf chkUser.Checked And (Not Calendar1.Visible) Then
                            Calendar1.SelectedDate = Today
                            If m_objTrace.UserData(m_strUser, Calendar1.SelectedDate.ToShortDateString) Then
                                FillGrid(0)
                            Else
                                lblError.Text = "Fehler:<br>" & m_objTrace.ErrorMessage
                            End If
                            Session("m_objTrace") = m_objTrace
                            'm_context.Cache.Insert("m_objTrace", m_objTrace, New System.Web.Caching.CacheDependency(Server.MapPath("StandardLog.ascx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
                            Calendar1.Visible = True

                        End If
                    Else
                        Dim strLinkPrefix As String = "/" & System.Configuration.ConfigurationManager.AppSettings("WebAppPath") & "/"
                        Response.Redirect(strLinkPrefix & "(S(" & Session.SessionID & "))/Start/Selection.aspx")
                    End If
                End If
            Catch ex As Exception
                lblError.Text = "Fehler.<br>(" & ex.Message & ")"
            End Try
        End Sub

        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
            If m_objTrace.StandardLog.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblError.Text = "Keine Daten zur Anzeige gefunden."
            Else
                DataGrid1.Visible = True

                Dim tmpDataView As New DataView()
                tmpDataView = m_objTrace.StandardLog.DefaultView

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

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.Visible = True
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    If DataGrid1.CurrentPageIndex = DataGrid1.PageCount - 1 Then
                        DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                    Else
                        DataGrid1.PagerStyle.NextPageText = "<img border=""0"" src=""/Portal/Images/arrow_right.gif"" width=""12"" height=""11"">"
                    End If

                    If DataGrid1.CurrentPageIndex = 0 Then
                        DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/empty.gif"" width=""12"" height=""11"">"
                    Else
                        DataGrid1.PagerStyle.PrevPageText = "<img border=""0"" src=""/Portal/Images/arrow_left.gif"" width=""12"" height=""11"">"
                    End If
                    DataGrid1.DataBind()
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim label As label
                Dim checkbox As checkbox
                Dim button As ImageButton
                Dim control As control
                Dim tblDetails As New DataTable

                For Each item In DataGrid1.Items
                    Dim blnDetailsExist As Boolean = False
                    cell = item.Cells(0)

                    For Each control In cell.Controls
                        If TypeOf control Is label Then
                            label = CType(control, label)
                            tblDetails = m_objTrace.LogDetails(CInt(label.Text))
                            If Not tblDetails Is Nothing Then
                                If Not tblDetails.Rows.Count = 0 Then
                                    blnDetailsExist = True
                                End If
                            End If
                        End If
                    Next

                    If blnDetailsExist Then
                        For Each control In cell.Controls
                            If TypeOf control Is checkbox Then
                                checkbox = CType(control, checkbox)
                                checkbox.Checked = m_blnShowDetails(item.ItemIndex)
                                If checkbox.Checked Then
                                    Dim NewLiteral As New Literal()
                                    NewLiteral.Text = "</TD></TR><TR><TD>&nbsp;</TD><TD colspan=""" & item.Cells.Count - 1 & """>"
                                    item.Cells(item.Cells.Count - 1).Controls.Add(NewLiteral)

                                    Dim NewDatagrid As New DataGrid()
                                    NewDatagrid.Width = New Unit("100%")
                                    NewDatagrid.AlternatingItemStyle.CssClass = "GridTableAlternate"
                                    NewDatagrid.HeaderStyle.CssClass = "GridTableHeadSub"
                                    NewDatagrid.DataSource = tblDetails
                                    NewDatagrid.DataBind()
                                    Dim newItem As DataGridItem
                                    Dim newCell As TableCell
                                    For Each newItem In NewDatagrid.Items
                                        For Each newCell In newItem.Cells
                                            If UCase(newCell.Text) = "FALSE" Then
                                                newCell.Text = "Nein"
                                            End If
                                            If UCase(newCell.Text) = "TRUE" Then
                                                newCell.Text = "Ja"
                                            End If
                                        Next
                                    Next
                                    item.Cells(item.Cells.Count - 1).Controls.Add(NewDatagrid)
                                End If
                            End If
                        Next
                        For Each control In cell.Controls
                            If TypeOf control Is ImageButton Then
                                button = CType(control, ImageButton)
                                If Not m_blnShowDetails(item.ItemIndex) Then
                                    button.ImageUrl = "../Images/plus.gif"
                                Else
                                    button.ImageUrl = "../Images/minus.gif"
                                End If
                            End If
                        Next
                    Else
                        For Each control In cell.Controls
                            If TypeOf control Is ImageButton Then
                                button = CType(control, ImageButton)
                                button.Visible = False
                            End If
                        Next
                    End If
                Next
            End If
        End Sub

        Private Sub ShrinkGrid()
            Dim item As DataGridItem
            Dim cell As TableCell
            Dim checkbox As checkbox
            Dim control As control

            For Each item In DataGrid1.Items
                cell = item.Cells(0)
                For Each control In cell.Controls
                    If TypeOf control Is checkbox Then
                        checkbox = CType(control, checkbox)
                        checkbox.Checked = False
                    End If
                Next
            Next
        End Sub

        Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
            ShrinkGrid()
            FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
        End Sub

        Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
            ShrinkGrid()
            FillGrid(e.NewPageIndex)
        End Sub

        Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
            If e.CommandSource.ToString = "System.Web.UI.WebControls.ImageButton" Then
                Dim item As DataGridItem
                Dim cell As TableCell
                Dim checkbox As checkbox
                Dim control As control

                For Each item In DataGrid1.Items
                    cell = item.Cells(0)
                    For Each control In cell.Controls
                        If TypeOf control Is checkbox Then
                            checkbox = CType(control, checkbox)
                            If checkbox.Checked Then
                                m_blnShowDetails(item.ItemIndex) = checkbox.Checked
                            End If
                        End If
                    Next
                Next

                m_blnShowDetails(e.Item.ItemIndex) = Not m_blnShowDetails(e.Item.ItemIndex)
                FillGrid(DataGrid1.CurrentPageIndex)
            End If
        End Sub

        Private Sub Calendar1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
            If m_objTrace.UserData(m_strUser, Calendar1.SelectedDate.ToShortDateString) Then
                FillGrid(0)
            Else
                lblError.Text = "Fehler:<br>" & m_objTrace.ErrorMessage
            End If
            Session("m_objTrace") = m_objTrace
            'm_context.Cache.Insert("m_objTrace", m_objTrace, New System.Web.Caching.CacheDependency(Server.MapPath("StandardLog.ascx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: StandardLog.ascx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 9.06.09    Time: 13:19
' Updated in $/CKAG/portal/PageElements
' Bugfix: Server-Cache nicht mehr benutzen
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 1.07.08    Time: 14:58
' Updated in $/CKAG/portal/PageElements
' ITA: 2014
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/portal/PageElements
' Migration OR
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:05
' Updated in $/CKAG/portal/PageElements
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:19
' Created in $/CKAG/portal/PageElements
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 31.07.07   Time: 10:48
' Updated in $/CKG/Portal/PageElements
' Pfad zur Selection.aspx  in StandardLog.aspx geändert.
' 
' *****************  Version 5  *****************
' User: Uha          Date: 15.03.07   Time: 11:36
' Updated in $/CKG/Portal/PageElements
' Imagepfade korrigiert
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal/PageElements
' 
' ************************************************