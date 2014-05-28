Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports EasyAccess_ALDBP

<CLSCompliant(False)> Public Class _Report00
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatensatz As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents btnBearb As System.Web.UI.WebControls.LinkButton

    Private m_TotalHitCount As Int32
    Private m_TotalResult As DataTable
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private showCheckbox As Boolean
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents btnFinish As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents tblSearch As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents ddlArchive As System.Web.UI.WebControls.DropDownList
    Private appl As String
    Private querydetails(7, 2) As String
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Private logApp As Base.Kernel.Logging.Trace

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

        Dim searchfields As ArrayList

        GetAppIDFromQueryString(Me)

        m_User = GetUser(Me)

        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        Try
            lblError.Text = String.Empty
            m_App = New Base.Kernel.Security.App(m_User)
            If Not IsPostBack Then
                logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Optisches Archiv")

                Session("m_TotalResult") = Nothing
                loadForm()
            Else
                If Not Session("m_TotalResult") Is Nothing Then
                    m_TotalResult = CType(Session("m_TotalResult"), DataTable)
                End If
                searchfields = CType(Session("EasySearchFields"), ArrayList)
                addSearchFields(searchfields)
            End If
        Catch ex As Exception
            'lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.ToString & ")"
        End Try
    End Sub

    Private Sub loadForm()
        Dim item As ListItem
        Dim status As String

        Dim easy As New EasyAccess_ALDBP.EasyAccess(m_User)
        Dim archives As EasyAccess_ALDBP.EasyArchive = easy.getArchives()
        Dim arc As Archive

        Session.Add("EasyAccess", easy)     'Merken, in Global.asax Freigeben! 
        While archives.hasNext
            item = New ListItem()

            arc = archives.nextArchive

            item.Value = CType(arc.getId(), String)
            'UH 13.06.2005:
            item.Text = arc.getTitleName().ToString
            'item.Text = arc.getName().ToString

            ddlArchive.Items.Add(item)
        End While

        status = String.Empty
        easy.init(status)
        If (status <> String.Empty) Then
            lblError.Text = "Fehler:" & status
            Exit Sub
        End If

        '###############################################################        
        'logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        'logApp.UpdateEntry("APP", Session("AppID").ToString, _
        '    "DBG_04, strremotehost = " & easy.remotehost & _
        '          ", lngremoteport = " & easy.remoteport.ToString & _
        '          ", lngrequesttimeout = " & easy.requesttimeout.ToString & _
        '          ", strSessionId = " & easy.SessionId & _
        '          ", strblobpathlocal = " & easy.blobpathlocal & _
        '          ", strblobpathremote = " & easy.blobpathremote & _
        '          ", strEasyUser = " & easy.EasyUser & _
        '          ", strEasyPwd = " & easy.EasyPwd)
        '###############################################################        

        initSearchFields()
    End Sub

    Private Sub addSearchFields(ByVal fields As ArrayList)
        'Liest die Felder des Archivs (ohne '.' am Anfang) und fügt sie incl. Eingabebox in die Seite ein
        Dim tblRow As System.Web.UI.HtmlControls.HtmlTableRow
        Dim tblCell As System.Web.UI.HtmlControls.HtmlTableCell
        Dim label As Label
        Dim literal0 As Literal
        Dim literal As Literal
        Dim literal2 As Literal
        Dim box As TextBox

        Dim i As Integer
        Dim field As EasyResultField

        If fields.Count > 0 Then
            'Überschrift
            tblRow = New System.Web.UI.HtmlControls.HtmlTableRow()

            tblCell = New System.Web.UI.HtmlControls.HtmlTableCell()
            label = New Label()
            label.Text = "Suchfelder"
            label.Font.Bold = True
            tblCell.Controls.Add(label)
            tblRow.Cells.Add(tblCell)

            'tblCell = New System.Web.UI.HtmlControls.HtmlTableCell()
            'label = New Label()
            'label.Text = "Anzeige"
            'label.Font.Bold = True
            'tblCell.Controls.Add(label)
            'tblRow.Cells.Add(tblCell)

            tblSearch.Rows.Add(tblRow)

            For i = 0 To fields.Count - 1
                'Zeile
                field = CType(fields(i), EasyResultField)

                If (Not field.Name.ToUpper = "AUFTRAGSNR") And _
                   (Not field.Name.ToUpper = "LAGERORT") And _
                   (Not field.Name.ToUpper = "ARCDATOLD") Then
                    tblRow = New System.Web.UI.HtmlControls.HtmlTableRow()

                    tblCell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    literal0 = New Literal()
                    literal0.Text = "&nbsp;&nbsp;&nbsp;"
                    tblCell.Controls.Add(literal0)
                    literal = New Literal()
                    literal.Text = field.Name.ToUpper & ":<br>&nbsp;&nbsp;&nbsp;"
                    tblCell.Controls.Add(literal)
                    box = New TextBox()
                    box.ID = field.Id & "$" & field.Name.ToUpper
                    box.Width = System.Web.UI.WebControls.Unit.Pixel(120)
                    'box.Width = System.Web.UI.WebControls.Unit.Pixel(100)
                    tblCell.Controls.Add(box)

                    literal2 = New Literal()
                    literal2.Text = "<input type=""hidden"" name=""" & field.Id & "§" & field.Name.ToUpper & """ id=""" & field.Id & "§" & field.Name.ToUpper & """ value=""ON"">"
                    tblCell.Controls.Add(literal2)

                    tblCell.Attributes.Add("Class", "TextLarge")
                    tblRow.Cells.Add(tblCell)

                    'tblCell = New System.Web.UI.HtmlControls.HtmlTableCell()
                    'cbx = New CheckBox()
                    'cbx.ID = field.Id & "§" & field.Name.ToUpper
                    'cbx.Checked = True
                    'tblCell.Controls.Add(cbx)
                    'tblRow.Cells.Add(tblCell)

                    tblSearch.Rows.Add(tblRow)
                End If
            Next
        End If
    End Sub

    Private Function getColumnsToshow() As ArrayList
        'Fügt anzuzeigenden Felder (Checkbox gesetzt) in eine Arraylist
        Dim j As Integer
        Dim field As String
        Dim value As String
        Dim result As New ArrayList()
        Dim id As String

        For j = 0 To Request.Form.Count - 1
            field = Request.Form.Keys.Item(j)
            If InStr(field, "§") > 0 Then
                value = Request.Form.Item(j).ToString()
                id = field.Substring(0, field.IndexOf("§"))

                If value <> String.Empty Then
                    'index = field.Substring(field.IndexOf("%") + 1, field.Length - field.IndexOf("%") - 1)
                    field = field.Substring(field.IndexOf("§") + 1, field.Length - field.IndexOf("§") - 1).ToUpper

                    result.Add(New EasyResultField(field, CType(id, Integer), 0))
                    'field = field.Replace("$.", "")
                    'field = field.Substring(field.IndexOf("."), field.Length - field.IndexOf("."))
                    'queryString &= field & "=" & value & " & "
                End If
            End If
        Next

        'If (queryString <> String.Empty) AndAlso (queryString.Substring(queryString.Length - 3, 3) = " & ") Then
        '    queryString = Left(queryString, queryString.Length - 3)
        'End If

        Return result
    End Function

    Private Function getQueryString() As String
        'Fügt den Suchstring anhand gefüllter Suchfelder Zusammen
        Dim queryString As String
        Dim j As Integer
        Dim field As String
        Dim value As String
        Dim id As String
        Dim i As Integer

        For i = 0 To 6
            querydetails(i, 0) = String.Empty
            querydetails(i, 1) = "0"
        Next
        queryString = String.Empty

        For j = 0 To Request.Form.Count - 1
            field = Request.Form.Keys.Item(j)
            Dim intPos As Integer = 0
            Dim intBeginOrEnd As Integer = 1

            If InStr(field, "$") > 0 Then
                id = field.Substring(0, field.IndexOf("$"))
                value = Request.Form.Item(j).ToString()

                If (Not value = String.Empty) And (Not value = "*") Then
                    querydetails(CInt(id) - 1001, 0) = value
                    If (InStr(value, "-") > 0) And (InStr(value, "*") > 0) Then
                        intPos = InStr(value, "-")
                    Else
                        If (InStr(value, "/") > 0) And (InStr(value, "*") > 0) Then
                            intPos = InStr(value, "/")
                        End If
                    End If

                    If intPos > 0 Then
                        If Left(value, 1) = "*" Then
                            intBeginOrEnd = -1
                        End If
                    End If

                    querydetails(CInt(id) - 1001, 1) = CStr(intPos * intBeginOrEnd)
                    Select Case intPos * intBeginOrEnd
                        Case Is > 0
                            queryString &= "." & id & "=" & Left(value, intPos - 1) & "* & "
                        Case Is < 0
                            queryString &= "." & id & "=*" & Right(value, Len(value) - intPos) & " & "
                        Case Else
                            queryString &= "." & id & "=" & value & " & "
                    End Select
                End If
            End If
        Next

        If (queryString <> String.Empty) AndAlso (queryString.Substring(queryString.Length - 3, 3) = " & ") Then
            queryString = Left(queryString, queryString.Length - 3)
        End If

        Return queryString
    End Function

    Private Sub btnFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFinish.Click
        Dim easy As EasyAccess_ALDBP.EasyAccess
        Dim result As DataTable
        Dim archive As Archive
        Dim status As String
        Dim queryString As String
        Dim listitem As ListItem

        Dim rowNew As DataRow
        Dim intColumns As Int32
        Dim intResultRow As Int32

        lblError.Text = String.Empty
        queryString = getQueryString()

        If (queryString = String.Empty) Then
            lblError.Text = "Keine Suchkriterien eingegeben."
            Exit Sub
        End If

        status = String.Empty
        easy = CType(Session("EasyAccess"), EasyAccess_ALDBP.EasyAccess)

        status = String.Empty
        m_TotalHitCount = 0
        m_TotalResult = Nothing

        For Each listitem In ddlArchive.Items
            easy.getResult.hitTblHeader = getColumnsToshow()

            If (easy.getResult.hitTblHeader.Count = 0) Then
                lblError.Text = "Keine Felder zur Anzeige ausgewählt."
                Exit Sub
            End If

            archive = easy.getArchives().getArchive(CType(listitem.Value, Long))
            easy.query(archive, queryString, status) '...und los!
            If (status = String.Empty) Then
                result = easy.getResult().getHitTable()
                If m_TotalResult Is Nothing Then
                    m_TotalResult = result.Clone
                    m_TotalResult.Columns.Add("Archiv", System.Type.GetType("System.String"))
                End If
                m_TotalHitCount += easy.total_hits
                For intResultRow = 0 To result.Rows.Count - 1
                    If m_TotalResult.Rows.Count = 100 Then Exit For
                    rowNew = m_TotalResult.NewRow
                    For intColumns = 0 To result.Columns.Count - 1
                        Dim strTemp As String = ""
                        If intColumns > 5 And intColumns < 13 Then
                            Select Case CInt(querydetails(intColumns - 6, 1))
                                Case Is > 0
                                    strTemp = querydetails(intColumns - 6, 0).Trim("*"c)
                                    If strTemp = Left(CStr(result.Rows(intResultRow)(intColumns)), Len(strTemp)) Then
                                        rowNew(intColumns) = result.Rows(intResultRow)(intColumns)
                                    Else
                                        rowNew = Nothing
                                        m_TotalHitCount -= 1
                                        GoTo LeaveFor
                                    End If
                                Case Is < 0
                                    strTemp = querydetails(intColumns - 6, 0).Trim("*"c)
                                    If strTemp = Right(CStr(result.Rows(intResultRow)(intColumns)), Len(strTemp)) Then
                                        rowNew(intColumns) = result.Rows(intResultRow)(intColumns)
                                    Else
                                        rowNew = Nothing
                                        m_TotalHitCount -= 1
                                        GoTo LeaveFor
                                    End If
                                Case Else
                                    rowNew(intColumns) = result.Rows(intResultRow)(intColumns)
                            End Select
                        Else
                            rowNew(intColumns) = result.Rows(intResultRow)(intColumns)
                        End If
                    Next
                    'Strange behaviour!!!
                    'Manchmal fliegt Exit For aus dem äußeren For
                    'Darum: Eine archaische Sprungmarke!
LeaveFor:
                    If Not rowNew Is Nothing Then
                        rowNew("Archiv") = listitem.Value
                        m_TotalResult.Rows.Add(rowNew)
                    End If
                Next
            Else
                lblError.Text = "Fehler:" & status
                Exit Sub
            End If
        Next

        Session("m_TotalResult") = m_TotalResult
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal direction As String = "")

        If m_TotalResult.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = CStr(Session("ShowOtherString"))
        Else
            DataGrid1.Visible = True

            Dim tmpDataView As New DataView()
            tmpDataView = m_TotalResult.DefaultView

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

            If (direction.Length > 0) Then
                tmpDataView.Sort = strTempSort & " " & direction
            End If

            DataGrid1.CurrentPageIndex = intTempPageIndex
            DataGrid1.DataSource = tmpDataView

            DataGrid1.DataBind()

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                If m_TotalHitCount > m_TotalResult.Rows.Count Then
                    lblNoData.Text = "Es werden " & tmpDataView.Count.ToString & " Dokumente (von " & m_TotalHitCount.ToString & " gefundenen) angezeigt."
                Else
                    lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Dokument(e) gefunden."
                End If
            End If
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

    Private Sub initSearchFields()
        Dim easy As EasyAccess_ALDBP.EasyAccess
        Dim status As String
        Dim searchFields As ArrayList
        Dim archive As Archive
        Dim strSearchFields As String = ""

        easy = CType(Session("EasyAccess"), EasyAccess_ALDBP.EasyAccess)
        status = String.Empty

        '###############################################################        
        'logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        'logApp.UpdateEntry("APP", Session("AppID").ToString, "DBG_05")
        '###############################################################        

        archive = easy.getArchives().getArchive(CType(ddlArchive.SelectedItem.Value, Long))

        If status <> String.Empty Then
            lblError.Text = status
            Exit Sub
        End If

        'Suchfelder holen
        status = String.Empty

        '###############################################################        
        'logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        'logApp.UpdateEntry("APP", Session("AppID").ToString, "DBG_06")
        '###############################################################        

        searchFields = easy.getSearchFields(archive, strSearchFields, status)
        'easy.getResult.hitTblSearch = strSearchFields

        If status <> String.Empty Then
            lblError.Text = status
            Exit Sub
        End If

        '###############################################################        
        'logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        'logApp.UpdateEntry("APP", Session("AppID").ToString, "DBG_07")
        '###############################################################        

        addSearchFields(searchFields)

        If (Session("EasySearchFields") Is Nothing) Then
            Session.Add("EasySearchFields", searchFields)
        Else
            Session("EasySearchFields") = searchFields
        End If

        easy.getResult.clear()

        'UH 02.12.2005
        'Hier werden nur Archive desselben Formats bertrachtet.
        '=> Initialisierung nur am Anfang
        'FillGrid(0)
        DataGrid1.Visible = False
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        e.Item.Cells(3).Visible = False 'ID
        e.Item.Cells(4).Visible = False 'Version
        e.Item.Cells(5).Visible = False
        e.Item.Cells(6).Visible = False
        e.Item.Cells(7).Visible = False
        e.Item.Cells(8).Visible = False
        e.Item.Cells(e.Item.Cells.Count - 1).Visible = False
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim doc_id As String
        Dim doc_ver As String
        Dim status As String
        Dim easy As EasyAccess_ALDBP.EasyAccess
        Dim strLink As String = String.Empty

        easy = CType(Session("EasyAccess"), EasyAccess_ALDBP.EasyAccess)
        status = String.Empty

        'If e.CommandName = "ansicht" Then
        doc_id = CType(e.Item.Cells(3).Text(), String)
        doc_ver = CType(e.Item.Cells(4).Text(), String)
        easy.getPics(easy.getArchives().getArchive(CType(e.Item.Cells(e.Item.Cells.Count - 1).Text(), Long)), doc_id, doc_ver, status, strLink)

        Dim tmpRows() As DataRow = m_TotalResult.Select("DOC_ID = " & doc_id & " AND DOC_VERSION = " & doc_ver)
        tmpRows(0)("Bilder") = strLink
        m_TotalResult.AcceptChanges()
        Session("m_TotalResult") = m_TotalResult

        FillGrid(0)

        If status <> String.Empty Then
            lblError.Text = status
        End If

        Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
        Literal1.Text &= "						  <!-- //" & vbCrLf
        Literal1.Text &= "                          window.open(""" & Replace(strLink, "\", "/") & """, ""_blank"", ""left=0,top=0,scrollbars=NO"");" & vbCrLf
        Literal1.Text &= "						    window.document.location.href = ""#" & doc_id & """;" & vbCrLf
        Literal1.Text &= "						  //-->" & vbCrLf
        Literal1.Text &= "						</script>" & vbCrLf
        'End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim easy As EasyAccess_ALDBP.EasyAccess

        easy = CType(Session("EasyAccess"), EasyAccess_ALDBP.EasyAccess)
        easy.setPics(easy.getCurrentArchive, Session.SessionID)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: _Report00.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Forms
' 
' *****************  Version 15  *****************
' User: Uha          Date: 21.06.07   Time: 11:32
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 19.06.07   Time: 15:57
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' ITA: 1071 - Maximierbarkeit des PDF-Dokumentes
' 
' *****************  Version 13  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' 
' ************************************************
