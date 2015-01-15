Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG.EasyAccess

<CLSCompliant(False)> Public Class _Report03
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    'Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatensatz As System.Web.UI.WebControls.Label
    Protected WithEvents lblMsg As System.Web.UI.WebControls.Label
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents btnBearb As System.Web.UI.WebControls.LinkButton

    'Private zulDaten As Zulassung
    Private m_App As Security.App
    Private m_User As Security.User
    Private showCheckbox As Boolean
    'Protected WithEvents lblNoData As System.Web.UI.WebControls.Label

    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Table10 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblSearch As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tRow As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tCell As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents tdSearch As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents cblArchive As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents rblTypes As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cbx1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents btnSuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtShowAll As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Private appl As String
    Private ELN As String = String.Empty
    Private sRefField As String = String.Empty


    Private fieldName As String
    Private fieldID As String
    Private fieldValue As String
    Private queryString As String = String.Empty
    Public Delegate Sub callBackFromSearchAttributesDelegate()
    Private myColumsToshow As New ArrayList

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

        'der unterschied zu Report00 soweit ich das jetzt überblicken kann, ist das hier ein sperrkriterium im optischen archiv existieren muss, 
        'wenn dies nicht der fall ist, rücksprung auf start/selection.aspx
        'JJU2008.05.27

        Dim searchfields As ArrayList

        Session.Add("AppID", Request.QueryString("AppID"))

        If Not Request.QueryString("ELN") Is Nothing Then
            ELN = Request.QueryString("ELN")
        End If

        m_User = GetUser(Me)

        ucHeader.InitUser(m_User)

        'Benutzer aus KUNDEN-Gruppe mit leerem Referenzeintrag fliegt hier schon raus
        FormAuth(Me, m_User, True)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text


        If Not Request.QueryString("RefField") Is Nothing Then
            sRefField = Request.QueryString("RefField")
        End If

        'Diese Anwendung benötigt zwingend die Bezeichnung des Filterkriteriums im Archiv
        If sRefField.Length = 0 Then
            Response.Redirect("/Portal/(S(" & Session.SessionID.ToString & "))/Start/Selection.aspx")
        End If

        Try
            m_App = New Security.App(m_User)
            Literal1.Text = String.Empty

            If Not IsPostBack Then
                Dim logApp As New Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Optisches Archiv")
                loadForm(False)
            Else
                searchfields = CType(Session("EasySearchFields"), ArrayList)
                addSearchFields(searchfields)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub loadForm(ByVal postback As Boolean)
        Dim item As ListItem
        Dim status As String
        Dim easy As New EasyAccess.EasyAccess(m_User, ELN)
        Dim archives As EasyAccess.EasyArchive = easy.getArchives()
        Dim arc As Archive

        Session.Add("EasyAccess", easy)

        If (archives.count = 0) Then
            lblError.Text = "Keine Archive verfügbar!"

            'btnFinish.Disabled = True
            btnSuche.Enabled = False
            Exit Sub
        End If

        Dim intCounter As Integer

        '--- Typliste füllen ---
        intCounter = 0
        While archives.hasNext

            arc = archives.nextArchive

            item = rblTypes.Items.FindByText(arc.getArcType())

            If item Is Nothing Then
                item = New ListItem()
                item.Value = CType(arc.getId(), String)
                item.Text = arc.getArcType().ToString
                rblTypes.Items.Add(item)
                intCounter = intCounter + 1
            End If

            If (Not postback) AndAlso (intCounter = 1) Then
                item.Selected = True
            End If
        End While

        '--- Archivliste füllen ---
        archives.resetCounter() 'Internen Zähler für neuen Durchlauf zurücksetzen
        cblArchive.Items.Clear()
        intCounter = 0
        While archives.hasNext

            arc = archives.nextArchive

            item = New ListItem()
            item.Value = CType(arc.getId(), String)
            item.Text = arc.getTitleName().ToString

            If arc.getArcType = rblTypes.SelectedItem.Text Then
                cblArchive.Items.Add(item)
                intCounter = intCounter + 1
                If (intCounter < 3) Then
                    item.Selected = True
                End If
            End If
        End While

        status = String.Empty
        easy.init(status)
        If (status <> String.Empty) Then
            lblError.Text = "Fehler:" & status
            Exit Sub
        End If
        initSearchFields()
    End Sub

    Private Sub addSearchFields(ByVal fields As ArrayList)
        'Liest die Felder des Archivs (ohne '.' am Anfang) und fügt sie incl. Eingabebox in die Seite ein
        Dim lit As Literal
        Dim box As TextBox
        Dim boxHidden As HtmlInputHidden = Nothing
        Dim cbx As CheckBox
        Dim i As Integer
        Dim field As EasyResultField
        Dim easy As EasyAccess.EasyAccess
        Dim defaultQuery As String
        Dim defaultID As String = ""
        Dim defaultValue As String = ""

        'Standard-Query holen
        easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)
        defaultQuery = easy.getCurrentArchive.getDefaultQuery()

        If (defaultQuery <> String.Empty) Then
            If defaultQuery.IndexOf("=") >= 0 Then
                defaultID = defaultQuery.Split("="c)(0).TrimStart("."c)
                defaultValue = defaultQuery.Split("="c)(1)
            End If
        End If

        tCell.Controls.Clear()
        Dim bRefrenceFieldFound As Boolean = False
        For i = 0 To fields.Count - 1

            field = CType(fields(i), EasyResultField)

            lit = New Literal()
            lit.Text = field.Name.ToUpper & ":<br>"

            box = New TextBox()                         'Normale Textbox

            If field.Id = defaultID Then
                box.Text = defaultValue
                box.Enabled = False

                boxHidden = New HtmlInputHidden()       'Hidden Field
                boxHidden.ID = field.Id & "?" & field.Name.ToUpper
                boxHidden.Value = defaultValue
            ElseIf field.Name.ToUpper = UCase(sRefField) Then
                bRefrenceFieldFound = True
                If m_User.Reference.Length > 0 Then
                    box.Text = m_User.Reference
                    box.Enabled = False

                    boxHidden = New HtmlInputHidden()       'Hidden Field
                    boxHidden.ID = field.Id & "?" & field.Name.ToUpper
                    boxHidden.Value = m_User.Reference
                Else
                    box.ID = field.Id & "?" & field.Name.ToUpper
                End If
            Else
                box.ID = field.Id & "?" & field.Name.ToUpper
            End If

            '############################################################################

            '§§§ JVE 27.07.2006: Wenn weniger als 7 Suchfelder, keine Checkboxen einblenden!
            txtShowAll.Value = "X"
            Table2.Visible = False
            If fields.Count > 6 Then
                txtShowAll.Value = String.Empty  'Nicht alle anzeigen
                Table2.Visible = True

                cbx = New CheckBox()
                cbx.ID = field.Id & "§" & field.Name.ToUpper
                '§§§ JVE 21.02.2006 Die ersten 6 Felder vorbelegen!
                If (i < 6) Then
                    cbx.Checked = True
                End If
                tCell.Controls.Add(cbx)
            End If

            '############################################################################
            tCell.Controls.Add(lit)
            tCell.Controls.Add(box)
            If Not (boxHidden Is Nothing) Then
                tCell.Controls.Add(boxHidden)
            End If

            lit = New Literal()
            lit.Text = "<br>"
            tCell.Controls.Add(lit)
        Next

        If Not bRefrenceFieldFound Then
            'Suchfeld wurde in Archiv nicht gefunden
            '=> Sperrkriterium konnte nicht geprüft werden = Abbruch
            Response.Redirect("/Portal/(S(" & Session.SessionID.ToString & "))/Start/Selection.aspx")
        End If
    End Sub


    Private Sub addTheResultColumnsToShow(Optional ByRef resultArray As ArrayList = Nothing)
        If resultArray Is Nothing Then
            myColumsToshow.Add(New EasyResultField(fieldName, CType(fieldID, Integer), 0))
        Else 'füllen des public arrays beendet, zurückgebene des lokalen arrays, das die alte ablauf struktur nicht verändert werden muss JJU2008.05.27
            resultArray = myColumsToshow.Clone
            myColumsToshow.Clear()
        End If
    End Sub


    Private Function getColumnsToshow(ByVal blnAll As Boolean) As ArrayList
        'Fügt anzuzeigenden Felder (Checkbox gesetzt) in eine Arraylist
        Dim result As New ArrayList()
        Dim theDelegateObject As New callBackFromSearchAttributesDelegate(AddressOf addTheResultColumnsToShow)

        If blnAll Then
            'Alle Felder anzeigen: §§§ JVE 27.07.2006
            getTheInputFieldAttributes(tblSearch, theDelegateObject)
            'aufruf um resultarray byref zu füllen JJU2008.05.27
            addTheResultColumnsToShow(result)
        Else
            'Nur die ausgewählten Felder anzeigen
            getTheInputFieldAttributes(tblSearch, theDelegateObject)
            'aufruf um resultarray byref zu füllen JJU2008.05.27
            addTheResultColumnsToShow(result)
        End If

        Return result
    End Function

    Private Sub getQueryString(Optional ByVal objDefaultQuery As Object = Nothing)
        'Fügt den Suchstring anhand gefüllter Suchfelder Zusammen
        'Fügt den Suchstring anhand gefüllter Suchfelder Zusammen

        Dim theDelegateObject As New callBackFromSearchAttributesDelegate(AddressOf verketteQueryString)

        getTheInputFieldAttributes(tblSearch, theDelegateObject)
        If queryString <> String.Empty Then
            queryString = queryString.TrimEnd(" "c)
            If queryString.LastIndexOf("&") = queryString.Length - 1 Then
                queryString = queryString.Remove(queryString.Length - 1, 1)
                queryString = queryString.TrimEnd(" "c)
            End If
        End If
    End Sub

    Private Sub verketteQueryString()
        If Not fieldValue Is String.Empty Then
            queryString &= "." & fieldID & "=" & fieldValue & " & "
        End If
    End Sub


    Private Sub getTheInputFieldAttributes(ByVal MotherControl As Control, Optional ByVal del As [Delegate] = Nothing)
        'mothercontrol = tblSearch
        Dim controlX As Web.UI.Control
        If MotherControl.ID = "tblSearch" Then
            MotherControl = tblSearch
        End If
        Dim box As New TextBox
        For Each controlX In MotherControl.Controls
            If controlX.HasControls = True Then
                getTheInputFieldAttributes(controlX, del)
            Else
                If InStr(controlX.ID, "?") > 0 Then
                    If Not TypeOf (box) Is TextBox Then
                        Throw New Exception("ein Suchfeld ist keine Textbox: " & controlX.ID)
                    End If
                    box = CType(controlX, TextBox)
                    fieldID = box.ID.Substring(0, box.ID.IndexOf("?")).ToUpper
                    fieldValue = box.Text
                    fieldName = box.ID.Substring(box.ID.IndexOf("?") + 1, box.ID.Length - box.ID.IndexOf("?") - 1).ToUpper
                    del.Method.Invoke(Me, Nothing) 'da callbackmethoden subs sind ohne parameter. JJ2008.05.27
                End If
            End If
        Next
    End Sub

    Private Sub DoSubmit()
        Dim easy As EasyAccess.EasyAccess
        Dim result As DataTable
        Dim archive As Archive
        Dim status As String
        Dim queryString As String
        Dim strArchiveList As ArrayList
        Dim item As ListItem

        easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)
        status = String.Empty

        strArchiveList = New ArrayList()
        For Each item In cblArchive.Items
            If item.Selected Then
                archive = easy.getArchives().getArchive(CType(item.Value, Long))
                strArchiveList.Add(archive)
            End If
        Next

        If (strArchiveList.Count = 0) Then
            lblError.Text = "Kein(e) Archiv(e) ausgewählt."
            Exit Sub
        End If

        lblError.Text = String.Empty
        'queryString = getQueryString(archive.getDefaultQuery)
        queryString = String.Empty
        getQueryString()

        If (queryString = String.Empty) Then
            lblError.Text = "Keine Suchkriterien eingegeben."
            Exit Sub
        End If

        '§§§ JVE 27.07.2006: cbxShowAll.checked -> es werden alle Suchfelder auch in der Trefferliste angezeigt (bei max. 5 Suchfeldern)
        If (txtShowAll.Value = String.Empty) Then
            easy.getResult.hitTblHeader = getColumnsToshow(False)
        Else
            easy.getResult.hitTblHeader = getColumnsToshow(True)
        End If

        If (easy.getResult.hitTblHeader.Count = 0) Then
            lblError.Text = "Keine Felder zur Anzeige ausgewählt."
            Exit Sub
        End If
        '###############################################################################
        If (easy.getResult.hitTblHeader.Count > 6) Then
            lblError.Text = "Es können maximal 6 Felder zur Anzeige ausgewählt werden."
            Exit Sub
        End If
        '###############################################################################

        easy.query(strArchiveList, queryString, status) '...und los!
        If (status = String.Empty) Then
            result = easy.getResult().getHitTable()
            FillGrid(0)
        Else
            lblError.Text = "Fehler:" & status
            Exit Sub
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        FillGrid(e.NewPageIndex)
    End Sub

    'Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
    '    FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    'End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal direction As String = "")
        Dim easy As EasyAccess.EasyAccess
        Dim m_objTable As DataTable

        easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)
        m_objTable = easy.getResult.getHitTable

        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblError.Visible = True
            lblError.Text = "Es wurden keine Treffer gefunden."
            'ShowScript.Visible = False
        Else
            DataGrid1.Visible = True

            Dim tmpDataView As New DataView()
            tmpDataView = m_objTable.DefaultView

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
                If easy.totalHits > tmpDataView.Count Then
                    lblNoData.Text = "Es werden " & tmpDataView.Count.ToString & " Dokument(e) von " & easy.totalHits.ToString & " vorhandenen angezeigt."
                Else
                    lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Dokument(e) gefunden."
                End If
            End If
            If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
                lnkKreditlimit.Text = "Zurück"
                lnkKreditlimit.NavigateUrl = "javascript:history.back()"
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

    'Private Sub ddlKunnr_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    initSearchFields()
    'End Sub

    Private Sub initSearchFields()
        Dim easy As EasyAccess.EasyAccess
        Dim status As String
        Dim searchFields As ArrayList
        Dim archive As Archive
        Dim strSearchFields As String = ""
        Dim objSearchFields As ArrayList
        Dim item As ListItem
        Dim itemSelected As ListItem = Nothing

        easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)
        status = String.Empty
        '##################################################################################
        '##################################################################################
        '    Vorbereitung für Multiarchivsuche!!!
        '##################################################################################
        '##################################################################################
        'archive = easy.getArchives().getArchive(1)
        '##################################################################################
        '##################################################################################

        '§§§ JVE 10.05.2006: Alle markierten Archive ziehen

        objSearchFields = New ArrayList()
        For Each item In cblArchive.Items
            If item.Selected Then
                archive = easy.getArchives().getArchive(CType(item.Value, Long))
                searchFields = easy.getSearchFields(archive, strSearchFields, status)
                objSearchFields.Add(searchFields)
                If itemSelected Is Nothing Then
                    itemSelected = item 'Erstes markierte Archiv merken
                End If
            End If
        Next

        If objSearchFields.Count = 0 Then
            lblError.Text = "Kein(e) Archiv(e) ausgewählt!"
            Exit Sub
        End If

        archive = easy.getArchives().getArchive(CType(itemSelected.Value, Long))

        'txtAusdruck.Text = archive.getIndexName()

        If status <> String.Empty Then
            lblError.Text = status
            Exit Sub
        End If

        'Suchfelder holen
        status = String.Empty

        searchFields = easy.getSearchFields(archive, strSearchFields, status)
        'easy.getResult.hitTblSearch = strSearchFields

        If status <> String.Empty Then
            lblError.Text = status
            Exit Sub
        End If

        addSearchFields(searchFields)

        If (Session("EasySearchFields") Is Nothing) Then
            Session.Add("EasySearchFields", searchFields)
        Else
            Session("EasySearchFields") = searchFields
        End If

        easy.getResult.clear()
        'FillGrid(0)
    End Sub

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        e.Item.Cells(3).Visible = False 'ID
        e.Item.Cells(4).Visible = False 'Version
        e.Item.Cells(5).Visible = False
        e.Item.Cells(6).Visible = False
        e.Item.Cells(7).Visible = False
        e.Item.Cells(8).Visible = False
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim doc_id As String
        Dim doc_ver As String
        Dim doc_loc As String
        Dim doc_arc As String
        Dim status As String
        Dim strQuery As String
        Dim easy As EasyAccess.EasyAccess
        Dim result As DataTable

        easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)
        status = String.Empty

        If e.CommandName = "ansicht" Then
            doc_loc = CType(e.Item.Cells(3).Text(), String)
            doc_arc = CType(e.Item.Cells(4).Text(), String)
            doc_id = CType(e.Item.Cells(5).Text(), String)
            doc_ver = CType(e.Item.Cells(6).Text(), String)

            easy.getPics(doc_loc, doc_arc, doc_id, doc_ver, status)

            If (status <> String.Empty) Then
                lblError.Text = status
                Exit Sub
            End If

            result = easy.getResult.getHitTable
            'FillGrid(0, "Bilder", "desc")
            FillGrid(0)

            Dim tmpRows As DataRow() = result.Select("doc_id = '" & doc_id & "' AND doc_version = '" & doc_ver & "'")

            Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
            Literal1.Text &= "						  <!-- //" & vbCrLf
            Literal1.Text &= "                          window.open(""" & Replace(tmpRows(0)("Bilder").ToString, "\", "/") & """, ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & vbCrLf
            Literal1.Text &= "						    window.document.location.href = ""#" & doc_id & """;" & vbCrLf
            Literal1.Text &= "						  //-->" & vbCrLf
            Literal1.Text &= "						</script>" & vbCrLf
        End If

        If e.CommandName = "Details" Then
            doc_loc = CType(e.Item.Cells(3).Text(), String)
            doc_arc = CType(e.Item.Cells(4).Text(), String)
            doc_id = CType(e.Item.Cells(5).Text(), String)
            doc_ver = CType(e.Item.Cells(6).Text(), String)
            strQuery = doc_loc & "." & doc_arc & "." & doc_id & "." & doc_ver

            Literal1.Text = "						<script language=""Javascript"">" & vbCrLf
            Literal1.Text &= "						  <!-- //" & vbCrLf
            Literal1.Text &= "                          window.open(""_Report01.aspx?I=" & strQuery & """, ""_blank"", ""left=0,top=0,scrollbars=NO"");" & vbCrLf
            Literal1.Text &= "						  //-->" & vbCrLf
            Literal1.Text &= "						</script>" & vbCrLf
        End If
        status = String.Empty
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub rblTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rblTypes.SelectedIndexChanged
        Dim easy As EasyAccess.EasyAccess

        easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)
        easy.getResult.clear()
        Session("EasyAccess") = easy

        FillGrid(0)
        loadForm(True)
    End Sub

    Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
        DoSubmit()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: _Report03.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:22
' Updated in $/CKAG/Components/ComArchive
' ITA:2837
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 27.05.08   Time: 13:08
' Updated in $/CKAG/Components/ComArchive
' ITA 1950
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:29
' Created in $/CKAG/Components/ComArchive
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:03
' Created in $/CKAG/Components/ComArchive/inetpub/wwwroot/Portal/Components/ComArchive
' 
' *****************  Version 1  *****************
' User: Uha          Date: 20.12.07   Time: 15:31
' Created in $/CKG/Components/ComArchive/ComArchiveWeb
' ITA 1507 (RTFS) / 1480 (AKF) Optisches Archiv mit Einschränkung auf
' User-Referenz zum Test
' 
' ************************************************
