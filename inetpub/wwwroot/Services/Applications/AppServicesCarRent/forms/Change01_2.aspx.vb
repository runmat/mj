Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change01_2
    Inherits System.Web.UI.Page
    Private m_User As Security.User
    Private m_App As Security.App
    Private objSuche As CarRent01

    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Security.App(m_User)
        GridNavigation1.setGridElment(gvFahrzeuge)
        objSuche = CType(Session("objSuche"), CarRent01)
        Try

            If Not IsPostBack Then
                FillGrid(0)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            btnConfirm.Enabled = False
        End Try
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objSuche.Status = 0 OrElse objSuche.Status = -1111 Then
            If objSuche.Result.Rows.Count = 0 Then
                Result.Visible = True
            Else
                Result.Visible = True

                Dim tmpDataView As New DataView()
                tmpDataView = objSuche.Result.DefaultView
                tmpDataView.RowFilter = "SelectedEinzel=True AND Art='CAR'"

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

                gvFahrzeuge.PageIndex = intTempPageIndex

                gvFahrzeuge.DataSource = tmpDataView
                gvFahrzeuge.DataBind()
                lblMessage.Text = "Überprüfen Sie die Daten und Klicken Sie auf ""Absenden"" um den Vorgang abzuschliessen."
            End If
        Else
            gvFahrzeuge.Visible = False
            lblError.Text = objSuche.Message
        End If
    End Sub

    Private Sub gvFahrzeuge_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        FillGrid(pageindex)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(gvFahrzeuge.PageIndex)
    End Sub

    Private Sub gvFahrzeuge_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvFahrzeuge.Sorting
        FillGrid(gvFahrzeuge.PageIndex, e.SortExpression)
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim objExcelExport As New Excel.ExcelExport()


        objSuche = CType(Session("objSuche"), CarRent01)

        'If objSuche.Zentral = True Then
        '    objSuche.setZulassung(Session("AppID").ToString, Session.SessionID, Me)
        'Else
        '    objSuche.setZulassungDezentral(Session("AppID").ToString, Session.SessionID, Me)
        'End If

        objSuche.setZulassung(Session("AppID").ToString, Session.SessionID, Me)


        If objSuche.Status = 0 Then
            objSuche.setResultRowClear()
            Session("AppResultExcel") = objSuche.ResultExcel
            ExcelDiv.Visible = True
            FillGrid(0)
            btnConfirm.Visible = False
            gvFahrzeuge.Columns(gvFahrzeuge.Columns.Count - 1).Visible = True
            lblMessage.Text = "Es wurden folgende Vorgänge abgeschlossen!"
            lbBack.Visible = True
        End If


    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As DataTable = Session("AppResultExcel")

        Try

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString & "&Back=1", False)
    End Sub
End Class