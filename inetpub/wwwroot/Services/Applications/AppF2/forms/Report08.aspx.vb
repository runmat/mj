Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business

Partial Public Class Report08
    Inherits System.Web.UI.Page
#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
    Private m_report As Mahnstufe
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

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If Not e.CommandName = "Sort" Then
            Dim TempTable As DataTable = CType(Session("Result"), DataTable)
            Dim index As Integer = CType(e.CommandArgument, Integer)
            Dim row As GridViewRow = GridView1.Rows(index)
            Dim lblEqunr As Label
            Dim tblRows() As DataRow
            lblEqunr = CType(row.FindControl("lblEqunr"), Label)
            tblRows = TempTable.Select("EQUNR='" & lblEqunr.Text & "'")

            If e.CommandName = "Freigabe" Then
                If tblRows.Length = 1 Then
                    Dim objTempzuEnd As New TempZuEndg(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    objTempzuEnd.EQUNR = lblEqunr.Text
                    objTempzuEnd.Vertragsnummer = row.Cells(2).Text
                    objTempzuEnd.Kennzeichen = row.Cells(3).Text
                    objTempzuEnd.Versand = row.Cells(4).Text
                    Session("objTempzuEnd") = objTempzuEnd

                    lblAbfrage.Text = "Soll die Position mit der Vertragsnummer " & row.Cells(2).Text & "  wirklich auf ""endgültig"" umgesetzt werden?"
                    ModalPopupExtender2.Show()
                End If
            End If
        End If
    End Sub
    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim objTempzuEnd As TempZuEndg
        objTempzuEnd = CType(Session("objTempzuEnd"), TempZuEndg)
        Session("objTempzuEnd") = objTempzuEnd
        objTempzuEnd.Customer = m_User.KUNNR

        objTempzuEnd.Change(Session("AppID").ToString, Session.SessionID, Me)

        Dim sMsg As String = ""
        If objTempzuEnd.Message.Length > 0 Then
            sMsg = objTempzuEnd.Message
            lblError.Text = "Fehler: " & objTempzuEnd.Message
        Else
            sMsg = "Vorgang OK"
            'logging
            Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.CollectDetails("Vertragsnummer", objTempzuEnd.Vertragsnummer, True)
            logApp.CollectDetails("Kennzeichen", objTempzuEnd.Kennzeichen)
            logApp.CollectDetails("Versanddatum", objTempzuEnd.Versand)
            logApp.CollectDetails("Kommentar", CType(sMsg, Object))

            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

            'Aktualisieren
            m_report = New Mahnstufe(m_User, m_App, "")
            m_report.Fill(Session("AppID").ToString, Session.SessionID, Me)
            If m_report.Status < 0 And Not m_report.Status = -12 Then '-12=no-data
                lblError.Text = "Fehler: " & m_report.Message
            Else
                If m_report.Result Is Nothing OrElse m_report.Result.Rows.Count = 0 Then
                    lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
                Else
                    Session("Result") = m_report.Result
                    Result.Visible = True
                    FillGrid(0)
                End If
            End If

        End If


    End Sub
    Private Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing

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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        ModalPopupExtender2.Show()
    End Sub
#End Region


#Region "Methods"

    Private Sub doSubmit()


        m_report = New Mahnstufe(m_User, m_App, "")
        m_report.Fill(Session("AppID").ToString, Session.SessionID, Me)
        If m_report.Status < 0 And Not m_report.Status = -12 Then '-12=no-data
            lblError.Text = "Fehler: " & m_report.Message
        Else
            If m_report.Result Is Nothing OrElse m_report.Result.Rows.Count = 0 Then
                lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
            Else
                If Session("Result") Is Nothing Then
                    Session.Add("Result", m_report.Result)
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
' $History: Report08.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Dittbernerc  Date: 5.04.11    Time: 16:30
' Updated in $/CKAG2/Applications/AppF2/forms
' FixGridViewCols:
' 
' Elemente wie Textboxen, Dropdownlisten, und Checkboxen werden in der
' Berechnung mit ber�cksichtigt.
' Die Headrow wird nur noch einmalig abgefragt nicht pro abgefragter Row
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 20.01.10   Time: 11:37
' Updated in $/CKAG2/Applications/AppF2/forms
' Ita: 3446, 3463
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 19.08.09   Time: 14:48
' Updated in $/CKAG2/Applications/AppF2/forms
' ITA: 3000
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 19.08.09   Time: 9:07
' Created in $/CKAG2/Applications/AppF2/forms
' ITA: 3000
' 