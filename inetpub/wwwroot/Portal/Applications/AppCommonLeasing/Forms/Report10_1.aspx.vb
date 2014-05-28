Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Partial Public Class Report10_1
    Inherits System.Web.UI.Page

    Public WithEvents ucStyles As CKG.Portal.PageElements.Styles
    Public WithEvents ucHeader As CKG.Portal.PageElements.Header

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User

    Private m_BriefeundCOC As BriefeundCOC
#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""

            If Not IsPostBack Then
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If

                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

            If m_BriefeundCOC Is Nothing Then
                If Session("AppTypenscheinohneCOC") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    m_BriefeundCOC = CType(Session("AppTypenscheinohneCOC"), BriefeundCOC)

                End If
            End If

            If Not IsPostBack Then

                'seitenspeziefische Aktionen

                'dropDownListe füllen
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 1
                DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
                FillGrid(0)
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_BriefeundCOC.Status = 0 Then

            If IsNothing(m_BriefeundCOC.ResultTable) OrElse m_BriefeundCOC.ResultTable.Rows.Count = 0 Then

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
                lblInfo.Text = "Anzahl: 0"
            Else


                If m_BriefeundCOC.ResultTable.Columns("Bezahlt") Is Nothing Then
                    m_BriefeundCOC.ResultTable.Columns.Add("Bezahlt", GetType(System.String))
                    Session("AppTypenscheinohneCOC") = m_BriefeundCOC
                End If

                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView(m_BriefeundCOC.ResultTable)


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

                lblInfo.Text = "Anzahl: " & tmpDataView.Count

                DataGrid1.CurrentPageIndex = intTempPageIndex

                DataGrid1.DataSource = tmpDataView

                DataGrid1.DataBind()
                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.Visible = True
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If
                For Each item As DataGridItem In DataGrid1.Items
                    If Not item.FindControl("lnkFahrgestellnummer") Is Nothing Then
                        If m_User.Applications.Select("AppName = 'Report46'").Length > 0 Then
                            CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
                        End If
                    End If
                Next
            End If
        Else
            lblError.Text = m_BriefeundCOC.Message
            Exit Sub
        End If

    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        ChangeTable()
        DataGrid1.EditItemIndex = -1
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        ChangeTable()
        DataGrid1.EditItemIndex = -1
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        ChangeTable()
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        DataGrid1.EditItemIndex = -1
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_zurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub



    Protected Sub imgbExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As New DataTable()
            Dim AppURL As String
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            tblTemp = m_BriefeundCOC.Result.Copy
            For Each col In DataGrid1.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                    'EQUNR nicht mit in Excel ausgeben, wird meist nur als boundcolumn versteckt als schlüssel verwendet JJU2008.10.23
                    If col2.ColumnName.ToUpper = "EQUNR" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lb_Save_Click(sender As Object, e As EventArgs) Handles lb_Save.Click

        Dim cbx As CheckBox
        Dim lbl As Label

        For Each Item As DataGridItem In DataGrid1.Items

            cbx = CType(Item.FindControl("chkBezahlt"), CheckBox)
            lbl = CType(Item.FindControl("lblEQUNR"), Label)

            If cbx.Checked = True Then
                m_BriefeundCOC.ResultTable.Select("EQUNR='" & lbl.Text & "'")(0)("Bezahlt") = "X"
            Else
                m_BriefeundCOC.ResultTable.Select("EQUNR='" & lbl.Text & "'")(0)("Bezahlt") = System.DBNull.Value
            End If


        Next



        m_BriefeundCOC.ResultTable.DefaultView.RowFilter = "Bezahlt = 'X'"

        If m_BriefeundCOC.ResultTable.DefaultView.Count < 1 Then
            lblError.Visible = True
            lblError.Text = "Es wurden keine Datensätze ausgewählt."
            Exit Sub
        End If

        m_BriefeundCOC.SetBezahlt(Session("AppID").ToString, Me.Session.SessionID)

        If m_BriefeundCOC.Status <> -9999 Then
            m_BriefeundCOC.ResultTable.DefaultView.RowFilter = ""
            m_BriefeundCOC.ResultTable.DefaultView.RowFilter = "Bezahlt Is Null"
            m_BriefeundCOC.ResultTable = m_BriefeundCOC.ResultTable.DefaultView.ToTable
            lblMessage.Text = "Daten gesichert."
            FillGrid(0)
        Else
            lblError.Visible = True
            lblError.Text = "Beim Speichern der Daten ist ein Fehler aufgetreten."

        End If


    End Sub

    Private Sub ChangeTable()
        Dim cbx As CheckBox
        Dim lbl As Label

        For Each Item As DataGridItem In DataGrid1.Items

            cbx = CType(Item.FindControl("chkBezahlt"), CheckBox)
            lbl = CType(Item.FindControl("lblEQUNR"), Label)

            If cbx.Checked = True Then
                m_BriefeundCOC.ResultTable.Select("EQUNR='" & lbl.Text & "'")(0)("Bezahlt") = "X"
            Else
                m_BriefeundCOC.ResultTable.Select("EQUNR='" & lbl.Text & "'")(0)("Bezahlt") = System.DBNull.Value
            End If


        Next
    End Sub

End Class