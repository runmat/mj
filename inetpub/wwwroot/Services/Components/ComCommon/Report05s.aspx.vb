Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports System.Drawing

Partial Public Class Report05s
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Private m_objExcel As DataTable
#End Region
    Protected widestData As Integer


#Region "Events"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvBestand)
        widestData = 0

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        checkDatum()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        CKG.Base.Business.HelpProcedures.FixedGridViewCols(gvBestand)

        NewSearch.ImageUrl = String.Format("/Services/Images/queryArrow{0}.gif", IIf(divSelection.Visible, "Up", ""))
        NewSearch2.ImageUrl = NewSearch.ImageUrl
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    'Private Sub gvBestand_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBestand.RowDataBound
    '    For i As Integer = 0 To gvBestand.Columns.Count - 1
    '        Dim iColHeaderWidth As Integer = 0
    '        Dim iColItemWidth As Integer = 0
    '        e.Row.Cells(i).Attributes.Add("style", "white-space:nowrap; ")
    '        For Each ctrl As Control In e.Row.Cells(i).Controls
    '            If TypeOf (ctrl) Is LinkButton Then
    '                Dim lnkbtn As LinkButton = CType(ctrl, LinkButton)
    '                iColHeaderWidth = lnkbtn.Text.Length * 10 / 2
    '                e.Row.Cells(i).Attributes.Add("style", "white-space:nowrap;width: " & iColHeaderWidth & "px;")
    '            End If
    '            If TypeOf (ctrl) Is Label Then
    '                Dim lable As Label = CType(ctrl, Label)
    '                iColItemWidth = lable.Text.Length * 10 / 2
    '                e.Row.Cells(i).Attributes.Add("style", "white-space:nowrap;width: " & iColItemWidth & "px;")
    '            End If
    '        Next
    '    Next
    'End Sub

    Private Sub gvBestand_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvBestand.Sorting
        FillGrid(0, e.SortExpression)
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Try
            Dim excelFactory As New Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            m_objExcel = CType(Session("ResultTable"), DataTable)

            For Each tmpRow As DataRow In m_objExcel.Rows
                tmpRow("Anschrift_ZH") = tmpRow("Anschrift_ZH").ToString.Replace("<br>", " ")
                tmpRow("Anschrift_ZE") = tmpRow("Anschrift_ZE").ToString.Replace("<br>", " ")
                tmpRow("Anschrift_ZP") = tmpRow("Anschrift_ZP").ToString.Replace("<br>", " ")
            Next
            m_objExcel.AcceptChanges()

            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim AppURL As String
            Dim col As DataControlField
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String = ""
            AppURL = Replace(Me.Request.Url.LocalPath, "/Services", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            For Each col In gvBestand.Columns
                For i = m_objExcel.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = m_objExcel.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper Then
                        sColName = TranslateColLbtn(gvBestand, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            m_objExcel.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                Next
                m_objExcel.AcceptChanges()
            Next

            excelFactory.CreateDocumentAndSendAsResponse(strFileName, Me.m_objExcel, Me.Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            lblError.Visible = True
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub DoSubmit()
        Try
            lblError.Text = ""

            Dim datAb As Date = CDate(txtAbDatum.Text)
            Dim datBis As Date = CDate(txtBisDatum.Text)


            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New ZugelasseneFahrz(m_User, m_App, strFileName)

            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me, datAb, datBis)

            Session("ResultTable") = m_Report.ResultTable

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.ResultTable.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Session("ShowOtherString") = "Es wurden " & m_Report.ResultTable.Rows.Count.ToString & " Einträge zu ""Eingänge"" gefunden."

                    'Dim objExcelExport As New Excel.ExcelExport()
                    'Try
                    '    Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    'Catch
                    'End Try
                    'Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName

                    FillGrid(0)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try
    End Sub


    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()

        Dim tblTemp As DataTable = CType(Session("ResultTable"), DataTable)

        tmpDataView = tblTemp.DefaultView


        If tmpDataView.Count = 0 Then
            Result.Visible = False
            lblError.Text = "zu Ihrer Selektion konnten keine Werte ermittelt werden"
        Else
            Result.Visible = True

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

            gvBestand.PageIndex = intTempPageIndex
            gvBestand.DataSource = tmpDataView
            gvBestand.DataBind()

            Result.Visible = True
            lbCreate.Visible = False
            divSelection.Visible = False
            'lblNewSearch.Visible = True
            'ibtNewSearch.Visible = True


        End If
    End Sub



    Private Sub checkDatum()

        If IsDate(txtBisDatum.Text) = True And IsDate(txtAbDatum.Text) = True Then
            Dim datAb As Date = CDate(txtAbDatum.Text)
            Dim datBis As Date = CDate(txtBisDatum.Text)

            If (datBis - datAb).TotalDays > 31 Then
                lblError.Visible = True
                lblError.Text = "Datumsbereich darf nicht größer als 1 Monat sein."
                Exit Sub
            ElseIf (datBis - datAb) < TimeSpan.FromDays(0) Then
                lblError.Visible = True
                lblError.Text = "Datum ""Bis"" darf nicht vor ""Von"" liegen."
                Exit Sub
            Else
                DoSubmit()
            End If
        Else
            lblError.Visible = True
            lblError.Text = "Ungültiger Datumswert"
            Exit Sub
        End If

    End Sub

#End Region


    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub

    Protected Sub NewSearch_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        divSelection.Visible = Not divSelection.Visible
        lbCreate.Visible = Not lbCreate.Visible
    End Sub
End Class

' ************************************************
' $History: Report05s.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 11.05.11   Time: 20:48
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 7.12.10    Time: 17:16
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 13.08.10   Time: 16:41
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA 4041 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 13.08.10   Time: 16:31
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA 4041 Testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 13.08.10   Time: 15:58
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA 4041
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 13.08.10   Time: 12:43
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA 4041
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.08.10   Time: 11:42
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA 4041 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.08.10   Time: 10:20
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA 4041 unfertig
' 
' ************************************************