Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Partial Public Class Report01
    Inherits System.Web.UI.Page
#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private m_report As DatenOhneZB2
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        m_App = New App(m_User)

        GridNavigation1.setGridElment(GridView1)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

    End Sub

    Private Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        doSubmit()
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub gridview1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(0, e.SortExpression)
    End Sub

    Protected Sub lbCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreateExcel.Click

        Dim TempTable As DataTable = CType(Session("Result"), DataTable)
        Dim excelFactory As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

        excelFactory.CreateDocumentAndSendAsResponse(strFileName, TempTable, Me.Page, , , , )
    End Sub

    Protected Sub lb_Loeschen_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim gvRow As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)

        Dim index As Integer = gvRow.RowIndex

        Dim row As GridViewRow = GridView1.Rows(index)

        Dim strTidNR As String


        m_report = New DatenOhneZB2(m_User, m_App, "")
        Session.Add("objReport", m_report)
        m_report.SessionID = Me.Session.SessionID
        m_report.AppID = CStr(Session("AppID"))
        strTidNR = row.Cells(1).Text

        m_report.DelDocuments(Session("AppID").ToString, Session.SessionID, strTidNR, Me)
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.CollectDetails("Vertragsnummer", CType(row.Cells(0).Text.TrimStart("0"c), Object), True)
        logApp.CollectDetails("Briefnummer", CType((strTidNR), Object))
        logApp.CollectDetails("Fahrgestellnummer", CType(row.Cells(2).Text.TrimStart("0"c), Object))
        logApp.CollectDetails("Erfassungsdatum", CType(row.Cells(3).Text.TrimStart("0"c), Object))

        If Not m_report.Status = 0 Then

            lblError.Text = m_report.Message
            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, strTidNR, "Fehler beim Löschen von Daten ohne Dokumente mit ZB2-Nummer" & strTidNR & ". (Fehler: " & m_report.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10, logApp.InputDetails)
        Else
            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, strTidNR, "Daten ohne Dokumente mit ZB2-Nummer" & strTidNR & " erfolgreich gelöscht.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
        End If

        Session("Result") = Nothing
        doSubmit()


        'End If
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region


#Region "Methods"

    Private Sub doSubmit()


        m_report = New DatenOhneZB2(m_User, m_App, "")
        m_report.Fill(Session("AppID").ToString, Session.SessionID, Me)
        If m_report.Status < 0 And Not m_report.Status = -12 Then '-12=no-data
            lblError.Text = "Fehler: " & m_report.Message
        Else
            If m_report.Result Is Nothing OrElse m_report.Result.Rows.Count = 0 Then
                lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
            Else

                If Session("Result") Is Nothing Then
                    Session.Add("Result", m_report.Result)
                Else
                    Session("Result") = m_report.Result
                End If


                Result.Visible = True
                FillGrid(0)



            End If
        End If
    End Sub


    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim TempTable As DataTable = CType(Session("Result"), DataTable)

        If TempTable.Rows.Count = 0 Then
            GridView1.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
        Else
            GridView1.Visible = True
            DivPlaceholder.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = TempTable.DefaultView

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
    End Sub




#End Region


End Class


' ************************************************
' $History: Report01.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 6.04.11    Time: 16:41
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 7.08.09    Time: 13:15
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 6.08.09    Time: 14:12
' Updated in $/CKAG2/Applications/AppF2/forms
' 