Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Text.RegularExpressions


Public Class Report01_2
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

    'Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable
    Private m_objExcel As DataTable

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblHidden As System.Web.UI.WebControls.Label
    Protected WithEvents lnkCreateExcel As System.Web.UI.WebControls.ImageButton
    Private legende As String
    Private csv As String
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkShowCSV As System.Web.UI.WebControls.HyperLink
    'Private schmal As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        If (Session("ResultTable") Is Nothing) Then
            Response.Redirect(Request.UrlReferrer.ToString)
        Else
            m_objTable = CType(Session("ResultTable"), DataTable)
        End If

        If (Session("ExcelTable") Is Nothing) Then
            m_objExcel = CType(Session("ResultTable"), DataTable)
        Else
            m_objExcel = CType(Session("ExcelTable"), DataTable)
        End If

        Try
            legende = Request.QueryString.Item("legende")
            If (legende = "Ja") Then
                lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
            End If
            If (legende = "Report201") Then
                lblInfo.Text = "<br>'X' = vorhanden"
            End If
            If (legende = "Report203") Then
                lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung, 'X' = vorhanden"
            End If

            BuildTextForLabelHead()

            'm_App = New Base.Kernel.Security.App(m_User)

            legende = Request.QueryString.Item("legende")
            If (legende = "Ja") Then
                lblInfo.Text = "<br>*(V)ollmacht, (H)andelsregistereintrag, (P)ersonalausweis, (G)ewerbeanmeldung, (E)inzugsermächtigung"
            End If

            If Not IsPostBack Then
                lnkBack.NavigateUrl = Request.UrlReferrer.ToString()

                csv = Request.QueryString.Item("csv")
                If csv = "Ja" Then
                    lnkShowCSV.Visible = True
                    lblDownloadTip.Visible = True
                    lnkCreateExcel.Visible = False

                    Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".csv"

                    excelFactory.CreateDocumentAndWriteToFilesystem(ConfigurationManager.AppSettings("ExcelPath") & strFileName, m_objExcel, Me, , , , , False)
                    lnkShowCSV.NavigateUrl = "/Portal/Temp/Excel/" & strFileName
                Else
                    lnkShowCSV.Visible = False
                    lblDownloadTip.Visible = False
                    lnkCreateExcel.Visible = True
                End If

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                If Not Session("ApplblInfoText") Is Nothing Then
                    If lblInfo.Text.Length = 0 Then
                        lblInfo.Text = CStr(Session("ApplblInfoText"))
                    Else
                        lblInfo.Text &= "<br>" & CStr(Session("ApplblInfoText"))
                    End If
                End If

                FillGrid(0)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        '##
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_objTable.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False

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

            DataGrid1.CurrentPageIndex = intTempPageIndex
            DataGrid1.DataSource = tmpDataView

            DataGrid1.DataBind()

            Dim k As Int32
            Dim l As Int32
            For l = 0 To DataGrid1.Items.Count - 1
                For k = 0 To m_objTable.Columns.Count - 1
                    If m_objTable.Columns(k).ExtendedProperties.Count > 0 Then
                        Select Case m_objTable.Columns(k).ExtendedProperties("Alignment").ToString
                            Case "Right"
                                DataGrid1.Items(l).Cells(k).HorizontalAlign = HorizontalAlign.Right
                            Case "Center"
                                DataGrid1.Items(l).Cells(k).HorizontalAlign = HorizontalAlign.Center
                            Case Else
                                DataGrid1.Items(l).Cells(k).HorizontalAlign = HorizontalAlign.Left
                        End Select
                    End If

                Next
            Next

            If (Not Session("ShowOtherString") Is Nothing) AndAlso CStr(Session("ShowOtherString")).Length > 0 Then
                lblNoData.Text = CStr(Session("ShowOtherString"))
            Else
                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Einträge zu """ & m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString & """ gefunden."
            End If
            'If (Not Session("BackLink") Is Nothing) AndAlso CStr(Session("BackLink")) = "HistoryBack" Then
            '    lnkKreditlimit.Text = "Zurück"
            '    lnkKreditlimit.NavigateUrl = "javascript:history.back()"
            'End If
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

    Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
        Dim intItem As Int32

        For intItem = 0 To m_objTable.Columns.Count - 1
            If m_objTable.Columns(intItem).DataType Is System.Type.GetType("System.DateTime") Then
                If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Cells(intItem).Text = DataBinder.Eval(e.Item.DataItem, m_objTable.Columns(intItem).ColumnName, "{0:dd.MM.yyyy}")
                End If
            End If
        Next
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Function StripQueryStringFromUrl(ByVal pUrl As String) As String
        Return Regex.Replace(pUrl, "\?.*$", String.Empty)
    End Function

    Private Function GetUrlReferrerForCmdBack() As System.Uri
        Dim aUri As System.Uri
        Dim aString As String

        aUri = Request.UrlReferrer
        aString = Request.QueryString("legende")
        If aString = "AppVFS-ADR" Or aString = "AppVFS-KZL" Then
            aUri = New Uri(lblHidden.Text)
        End If
        Return aUri
    End Function

    Private Sub BuildTextForLabelHead()

        'Wenn die Anzeige aus AppVFS Kennzeichenbestand aufgerufen wurde,
        'soll nicht der Friendly Name angezeigt werden.
        ucStyles.TitleText = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If (legende = "AppVFS-ADR") Then
            lblHead.Text = "Adressen"
            lblPageTitle.Text = String.Empty
            ucStyles.TitleText &= " - " & lblHead.Text
        ElseIf (legende = "AppVFS-KZL") Then
            lblHead.Text = "Kennzeichenliste"
            lblPageTitle.Text = String.Empty
            ucStyles.TitleText &= " - " & lblHead.Text
        Else
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lnkCreateExcel_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles lnkCreateExcel.Click
        Try
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

            excelFactory.CreateDocumentAndSendAsResponse(strFileName, Me.m_objExcel, Me.Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
        End Try
    End Sub
End Class

' ************************************************
' $History: Report01_2.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 26.11.08   Time: 17:11
' Updated in $/CKAG/portal/Shared
' ITA 2317 unfertig
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 29.10.08   Time: 13:41
' Updated in $/CKAG/portal/Shared
' ITA: 2311
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 29.10.08   Time: 11:02
' Updated in $/CKAG/portal/Shared
' Änderung Linkbutton zu ImageButton download excel
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 14.10.08   Time: 15:07
' Updated in $/CKAG/portal/Shared
' Imagebutton mit Excel Image hinzugefügt
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 14.05.08   Time: 15:21
' Updated in $/CKAG/portal/Shared
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/portal/Shared
' Migration OR
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/portal/Shared
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:21
' Created in $/CKAG/portal/shared
' 
' *****************  Version 15  *****************
' User: Uha          Date: 11.12.07   Time: 16:31
' Updated in $/CKG/Portal/Shared
' ITA 1468/1500 Testversion
' 
' *****************  Version 14  *****************
' User: Uha          Date: 16.08.07   Time: 11:46
' Updated in $/CKG/Portal/Shared
' Link CSV-Datei nennt sich jetzt auch so. CSV-Datei wird nicht jedesmal
' neu erzeugt.
' 
' *****************  Version 13  *****************
' User: Uha          Date: 13.08.07   Time: 14:37
' Updated in $/CKG/Portal/Shared
' In Report01_2 kann sich jetzt die ExcelTabelle von der Tabelle im
' HTML-Datagrid unterscheiden.
' 
' *****************  Version 12  *****************
' User: Uha          Date: 8.08.07    Time: 17:22
' Updated in $/CKG/Portal/Shared
' Bugfixing CSV-Ausgabe
' 
' *****************  Version 11  *****************
' User: Uha          Date: 8.08.07    Time: 15:39
' Updated in $/CKG/Portal/Shared
' Report01_2 kann jetzt auch CSV-Dateien ausgeben
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 21.06.07   Time: 17:09
' Updated in $/CKG/Portal/Shared
' lnkCreateExcel_Click:Excel Filename war auf Report gesetzt -> jetzt
' wieder Datum und Username
' 
' *****************  Version 9  *****************
' User: Uha          Date: 20.06.07   Time: 18:55
' Updated in $/CKG/Portal/Shared
' Reparaturen im Standardreport
' 
' *****************  Version 8  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Portal/Shared
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 15.05.07   Time: 15:29
' Updated in $/CKG/Portal/Shared
' Änderungen aus StartApplication vom 11.05.2007
' 
' *****************  Version 6  *****************
' User: Uha          Date: 3.05.07    Time: 18:27
' Updated in $/CKG/Portal/Shared
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
