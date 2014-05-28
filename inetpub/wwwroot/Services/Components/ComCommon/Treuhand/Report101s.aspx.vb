Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common

Namespace Treuhand
    Partial Public Class Report101s
        Inherits System.Web.UI.Page

        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App

        Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

        Private m_report As Treuhandbestand
        Private m_IsTG As Boolean


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            Dim appID = CStr(Session("AppID"))

            lblHead.Text = m_User.Applications.Select("AppID = '" & appID & "'")(0)("AppFriendlyName").ToString
            GridNavigation1.setGridElment(GridView1)

            m_IsTG = Not Request.QueryString.Item("ART") Is Nothing AndAlso Request.QueryString.Item("ART") = "TG"

            If Not Page.IsPostBack Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                Dim sf = New SperreFreigabe(m_User, m_App, appID, Me.Session.SessionID, String.Empty)
                If m_IsTG = False Then sf.alleTreugeber = "X"
                sf.GetCustomer(Me, appID, Me.Session.SessionID)
                FillPreselectDropDown(sf.Result)

                m_report = New Treuhandbestand(m_User, m_App, strFileName)
                m_report.AppID = appID
                m_report.SessionID = Me.Session.SessionID
                m_report.Aktion = IIf(m_IsTG, "TG", "TN")
                Session.Add("objReport", m_report)
            ElseIf Not Session("objReport") Is Nothing Then
                m_report = CType(Session("objReport"), Treuhandbestand)
            End If
        End Sub

        Private Sub FillPreselectDropDown(ByVal data As DataTable)
            ddlPreselect.Items.Clear()
            lbl_SelPreselect.Visible = False
            ddlPreselect.Visible = False

            If Not data Is Nothing AndAlso data.Rows.Count > 0 Then
                ddlPreselect.Items.Add(New ListItem("-- alle --", "--"))

                For Each row As DataRow In data.Rows
                    Dim isTG = row("ZSELECT") = "TG"
                    If isTG <> m_IsTG Then Continue For

                    Dim valueCol = IIf(isTG, "AG", "TREU")
                    Dim textCol = IIf(isTG, "NAME1_AG", "NAME1_TG")
                    ddlPreselect.Items.Add(New ListItem(row(textCol), row(valueCol)))
                Next

                If ddlPreselect.Items.Count = 1 Then Exit Sub

                lbl_SelPreselect.Text = IIf(m_IsTG, "Auftraggeber", "Treuhandgeber")
                lbl_SelPreselect.Visible = True
                ddlPreselect.Visible = True
            End If
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
            Dim preselect = IIf(ddlPreselect.SelectedIndex <= 0, String.Empty, ddlPreselect.SelectedValue)
            m_report.Fill(Me, preselect, txtKennz.Text, txtFin.Text, txtRef2.Text)
            hField.Value = "0"

            If m_report.Status < 0 Then
                lblError.Visible = True
                lblError.Text = m_report.Message
                GridView1.Visible = False
                Result.Visible = False
            Else
                If m_report.Result Is Nothing OrElse m_report.Result.Rows.Count = 0 Then
                    lblError.Visible = True
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    GridView1.Visible = False
                    Result.Visible = False
                Else
                    lblError.Visible = False
                    Session("ResultTable") = m_report.Result
                    FillGrid(0)
                End If
            End If

        End Sub

        Protected Sub NewSearch_Click(sender As Object, e As ImageClickEventArgs)
            NeueSuche()
        End Sub

        Private Sub NeueSuche()
            NewSearch.Visible = False
            NewSearchUp.Visible = True
            cmdSearch.Visible = True
            tab1.Visible = True
            Queryfooter.Visible = True
            FillGrid(GridView1.PageIndex, "")
        End Sub

        Protected Sub NewSearchUp_Click(sender As Object, e As ImageClickEventArgs)
            NewSearch.Visible = True
            NewSearchUp.Visible = False
            cmdSearch.Visible = False
            tab1.Visible = False
            Queryfooter.Visible = False
            FillGrid(GridView1.PageIndex, "")
        End Sub

        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

            Dim ResultTable As DataTable

            ResultTable = CType(Session("ResultTable"), DataTable)

            If Not ResultTable Is Nothing Then

                If ResultTable.Rows.Count = 0 Then
                    NeueSuche()
                    lblError.Visible = True
                    lblError.Text = "Keine Daten zur Anzeige gefunden."
                    GridView1.Visible = False
                    Result.Visible = False
                Else
                    lblError.Visible = False
                    GridView1.Visible = True
                    lnkCreateExcel.Visible = True
                    Result.Visible = True
                    lblNoData.Visible = False

                    If hField.Value = "0" Then
                        lblNoData.Visible = False
                        cmdSearch.Visible = False
                        tab1.Visible = False
                        Queryfooter.Visible = False
                    End If

                    hField.Value = "1"

                    If Not tab1.Visible Then
                        NewSearch.Visible = True
                        lblNewSearch.Visible = True
                        NewSearchUp.Visible = False
                    Else
                        NewSearch.Visible = False
                        lblNewSearch.Visible = False
                        NewSearchUp.Visible = True
                    End If

                    Dim tmpDataView As New DataView(ResultTable)

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

                    GridView1.PageIndex = intTempPageIndex

                    GridView1.DataSource = tmpDataView
                    GridView1.DataBind()
                End If
            Else
                lblError.Text = "Keine Daten zur Anzeige gefunden."
                lblError.Visible = True
            End If
        End Sub

        Private Sub lnkCreateExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateExcel.Click

            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As DataTable = CType(Session("ResultTable"), DataTable).Copy
            Dim AppURL As String
            Dim col As DataControlField
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""

            'If tblTemp.Columns.Contains("BELNR") Then
            '    tblTemp.Columns.Remove("BELNR")
            'End If
            Dim tmpRow As DataRow

            For Each tmpRow In tblTemp.Rows
                If tmpRow("Versandadresse").ToString.Length > 0 Then
                    tmpRow("Versandadresse") = Replace(tmpRow("Versandadresse").ToString, "<br/>", ", ")
                End If
            Next

            AppURL = Replace(Me.Request.Url.LocalPath, "/Services", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            For Each col In GridView1.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper Then
                        sColName = TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                Next
                tblTemp.AcceptChanges()
            Next

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

            'm_report.ResultTable = CType(Session("ResultTable"), DataTable)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
            HelpProcedures.FixedGridViewCols(GridView1)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub


        Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
            FillGrid(e.NewPageIndex)
        End Sub

        Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
            GridView1.PageIndex = PageIndex
            FillGrid(PageIndex)
        End Sub

        Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
            FillGrid(0)
        End Sub
        Private Sub Gridview1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
            FillGrid(GridView1.PageIndex, e.SortExpression)
        End Sub

        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Session("objReport") = Nothing
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub
    End Class
End Namespace
