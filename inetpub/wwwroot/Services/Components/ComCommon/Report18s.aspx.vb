Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Partial Public Class Report18s
    Inherits System.Web.UI.Page

#Region "Declarations"

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private m_App As App
    Private m_User As User

#End Region


#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(GridView1)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
        NewSearch.ImageUrl = String.Format("/Services/Images/queryArrow{0}.gif", IIf(cmdcreate.Visible, "Up", ""))
        NewSearch2.ImageUrl = NewSearch.ImageUrl

        MyBase.OnPreRender(e)
    End Sub

    Protected Sub cmdcreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdcreate.Click
        DoSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


#End Region

#Region "Methods"

    Private Sub DoSubmit()
        Try
            lblError.Text = ""
            If Not IsDate(txtAbDatum.Text) Then
                lblError.Text = "Geben Sie bitte ein gültiges Startdatum ein!<br>"
                Exit Sub
            End If

            If Not IsDate(txtBisDatum.Text) Then
                lblError.Text = "Geben Sie bitte ein gültiges Enddatum ein!<br>"
                Exit Sub
            End If

            If txtAbDatum.Text.Length = 0 OrElse txtBisDatum.Text.Length = 0 Then
                lblError.Text = "Geben Sie bitte ein Start- und ein Enddatum ein!<br>"
                Exit Sub
            End If


            Dim datAb As Date = CDate(txtAbDatum.Text)
            Dim datBis As Date = CDate(txtBisDatum.Text)

            If datAb > datBis Then
                lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
            End If

            Dim strAction As String = "NEU"
            If rbAusgang.Checked Then
                strAction = "AUS"
            End If

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New EinAusgang(m_User, m_App, datAb, datBis, strAction, strFileName)


            If rbAusgang.Checked Then
                m_Report.ABC = rblAusgangZusatz.SelectedValue
            End If

            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)



            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.ResultTable.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Session("ResultTable") = m_Report.ResultTable
                    FillGrid(0)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = CType(Session("ResultTable"), DataTable).DefaultView

        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            Result.Visible = False
        Else
            Result.Visible = True
            cmdcreate.Visible = False
            tab1.Visible = False
            Queryfooter.Visible = False
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Dim intTempPageIndex As Int32 = intPageIndex

            If strSort.Trim(" "c).Length > 0 Then
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

    End Sub




#End Region



    Protected Sub rbAusgang_CheckedChanged(sender As Object, e As EventArgs) Handles rbAusgang.CheckedChanged
        trAusgangZusatz.Visible = rbAusgang.Checked
    End Sub

    Private Sub rbEingang_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbEingang.CheckedChanged
        trAusgangZusatz.Visible = rbAusgang.Checked
    End Sub


    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub



    Private Sub GridView1_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Protected Sub NewSearch_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        cmdcreate.Visible = Not cmdcreate.Visible
        tab1.Visible = Not tab1.Visible
        Queryfooter.Visible = Not Queryfooter.Visible
        Result.Visible = Not Result.Visible
    End Sub

    Protected Sub lnkCreateExcel1_Click(sender As Object, e As EventArgs) Handles lnkCreateExcel1.Click

        Dim control As New Control()
        Dim tblTranslations As New DataTable()
        Dim tblTemp As DataTable = CType(Session("ResultTable"), DataTable).Copy
        Dim AppURL As String = Nothing
        Dim col2 As DataColumn = Nothing
        Dim bVisibility As Integer = 0
        Dim i As Integer = 0
        Dim sColName As String = ""
        Dim gefunden As Boolean
        AppURL = Me.Request.Url.LocalPath.Replace("/Services", "..")
        tblTranslations = DirectCast(Me.Session(AppURL), DataTable)

        ' Adressfeld zusammensetzen
        For Each zeile As DataRow In tblTemp.Rows
            zeile("POST_CODE1") = zeile("NAME1").ToString() & ", " & zeile("NAME2").ToString() & _
                ", " & zeile("STREET").ToString() & " " & zeile("HOUSE_NUM1").ToString() & _
                ", " & zeile("POST_CODE1").ToString() & " " & zeile("CITY1").ToString()
        Next

        ' Nur die Spalten in Excel-Export übernehmen, die auch angezeigt werden
        For i = tblTemp.Columns.Count - 1 To 0 Step -1
            gefunden = False
            bVisibility = 0
            col2 = tblTemp.Columns(i)

            For Each col As DataControlField In GridView1.Columns
                If col2.ColumnName.ToUpper() = col.SortExpression.ToUpper() Then
                    gefunden = True
                    sColName = TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)

                    If bVisibility = 0 Then
                        tblTemp.Columns.Remove(col2)
                    ElseIf sColName.Length > 0 Then
                        col2.ColumnName = sColName
                    End If
                End If
            Next
            If Not gefunden Then
                tblTemp.Columns.Remove(col2)
            End If
        Next

        'For Each col As DataControlField In GridView1.Columns
        '    i = tblTemp.Columns.Count - 1
        '    While i >= 0
        '        bVisibility = 0
        '        col2 = tblTemp.Columns(i)
        '        If col2.ColumnName.ToUpper() = col.SortExpression.ToUpper() Then


        '            sColName = CKG.Base.Kernel.Common.Common.TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
        '            If bVisibility = 0 Then
        '                tblTemp.Columns.Remove(col2)
        '            ElseIf sColName.Length > 0 Then
        '                col2.ColumnName = sColName
        '            End If
        '        End If
        '        i += -1
        '    End While

        '    tblTemp.AcceptChanges()
        'Next

        'i = tblTemp.Columns.Count - 1
        'While i >= 0

        '    Dim colFound As [Boolean] = False
        '    Dim colTempName As String = tblTemp.Columns(i).ColumnName

        '    For Each dr As DataRow In tblTranslations.Rows
        '        If colTempName = dr(4).ToString() Then
        '            colFound = True
        '            Exit For
        '        End If
        '    Next

        '    If colFound = False Then
        '        tblTemp.Columns.Remove(colTempName)
        '    End If
        '    i += -1
        'End While

        tblTemp.AcceptChanges()

        Dim excelFactory As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim filename As String = [String].Format("{0:yyyyMMdd_HHmmss_}", System.DateTime.Now) + m_User.UserName
        excelFactory.CreateDocumentAndSendAsResponse(filename, tblTemp, Me.Page, False, Nothing, 0, 0)

    End Sub
End Class