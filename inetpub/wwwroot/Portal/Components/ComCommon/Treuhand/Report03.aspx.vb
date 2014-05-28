Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Namespace Treuhand
    Partial Public Class Report03
        Inherits System.Web.UI.Page

        Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles
        Protected WithEvents ucHeader As CKG.Portal.PageElements.Header

        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private BestandVon As String

        Dim m_report As Treuhandbestand

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            BestandVon = ""
            If Not Request.QueryString.Item("ART") Is Nothing Then
                BestandVon = Request.QueryString.Item("ART").ToString
            End If


            m_report = CType(Session("m_report"), Treuhandbestand)
            If Not IsPostBack Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                m_report = New Treuhandbestand(m_User, m_App, strFileName)
                Session.Add("objReport", m_report)
                m_report.SessionID = Me.Session.SessionID
                m_report.AppID = CStr(Session("AppID"))
                doSubmit()
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("250")
            End If
        End Sub

        Private Sub doSubmit()
            If BestandVon = "TG" Then
                m_report.Aktion = "TG"
            Else
                m_report.Aktion = "TN"
            End If
            m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString)
            If m_report.Status < 0 Then
                lblError.Text = m_report.Message
            Else
                If m_report.ResultTable.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    GridView1.Visible = False
                Else
                    Session("ResultTable") = m_report.ResultTable
                    FillGrid(0)
                End If
            End If

        End Sub

        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

            Dim ResultTable As DataTable

            ResultTable = CType(Session("ResultTable"), DataTable)

            If Not ResultTable Is Nothing Then

                If ResultTable.Rows.Count = 0 Then
                    lblError.Visible = True
                    lblError.Text = "Keine Daten zur Anzeige gefunden."
                    GridView1.Visible = False
                Else
                    lblError.Visible = False
                    GridView1.Visible = True
                    lnkCreateExcel.Visible = True
                    lblNoData.Visible = True
                    lblNoData.Text = ResultTable.Rows.Count & " Vorgänge gefunden!"

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
                    'If GridView1.PageCount > 1 Then
                    '    GridView1.PagerSettings.Visible = True
                    'Else
                    '    GridView1.PagerSettings.Visible = False
                    'End If
                End If
            Else
                lblError.Text = "Keine Daten zur Anzeige gefunden."
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


            Dim tmpRow As DataRow

            For Each tmpRow In tblTemp.Rows
                If tmpRow("Versandadresse").ToString <> "<br/><br/>" Then
                    tmpRow("Versandadresse") = Replace(tmpRow("Versandadresse").ToString, "<br/>", ", ")
                End If
            Next

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
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

            m_report.ResultTable = CType(Session("ResultTable"), DataTable)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub
        Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
            FillGrid(e.NewPageIndex)
        End Sub

        Protected Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPageSize.SelectedIndexChanged
            GridView1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
            FillGrid(0)
        End Sub

        Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
            FillGrid(0, e.SortExpression)
        End Sub
    End Class
End Namespace
