Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Partial Public Class Report06
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation
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
        btnZuordnen.Visible = True
        btnSave.Visible = False
        GridView1.Columns(4).Visible = True
        For i = 5 To 8
            GridView1.Columns(i).Visible = False
        Next
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        If GridView1.Columns(4).Visible = False Then
            UpdateDataTableSave()
        Else
            UpdateDataTableZuordnen()
        End If
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        If GridView1.Columns(4).Visible = False Then
            UpdateDataTableSave()
        Else
            UpdateDataTableZuordnen()
        End If
        FillGrid(0)
    End Sub

    Private Sub gridview1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        If GridView1.Columns(4).Visible = False Then
            UpdateDataTableSave()
        Else
            UpdateDataTableZuordnen()
        End If

        FillGrid(0, e.SortExpression)
    End Sub

    Protected Sub lbCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreateExcel.Click

        Dim TempTable As DataTable = CType(Session("Result"), DataTable).Copy
        Dim excelFactory As New CKG.Base.Kernel.DocumentGeneration.ExcelDocumentFactory()
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName


        Dim i As Integer = TempTable.Columns.Count

        Do Until i = 4

            TempTable.Columns.RemoveAt(TempTable.Columns.Count - 1)
            TempTable.AcceptChanges()

            i = TempTable.Columns.Count
        Loop

        excelFactory.CreateDocumentAndSendAsResponse(strFileName, TempTable, Me.Page, , , , )
    End Sub

    Private Sub btnZuordnen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnZuordnen.Click
        UpdateDataTableZuordnen()
        Dim TempTable As DataTable = Session("Result")
        Dim tmpRows() As DataRow

        tmpRows = TempTable.Select("Zuordnen=True")

        If CType(tmpRows.Length, Integer) > 0 Then
            btnZuordnen.Visible = False
            btnSave.Visible = True

            For i = 5 To 8

                GridView1.Columns(i).Visible = True

            Next
            FillGrid(0, Zugeordnet:=True)
            GridView1.Columns(4).Visible = False


        Else
            lblError.Text = "Keine Dokumente ausgewählt!"
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        UpdateDataTableSave()
        Dim TempTable As DataTable = Session("Result")


        Dim tmpRows() As DataRow
        Dim Status As String = String.Empty
        tmpRows = TempTable.Select("Zuordnen=True")

        Dim m_Report As ZB2OhneDaten

        m_Report = New ZB2OhneDaten(m_User, m_App, "")
        m_Report.Change(TempTable, Status, Session("AppID").ToString, Session.SessionID, Me)

        For i = 5 To 8

            GridView1.Columns(i).Visible = False

        Next

        GridView1.Columns(9).Visible = True
        btnSave.Visible = False

        FillGrid(0, Zugeordnet:=True)

        If Status.Length > 0 Then
            lblError.Text = Status
        End If


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

        Session("Result") = Nothing

        Dim m_Report As ZB2OhneDaten

        m_Report = New ZB2OhneDaten(m_User, m_App, "")

        m_Report.Fill(Session("AppID").ToString, Session.SessionID, Me)
        If m_Report.Status < 0 And Not m_Report.Status = -12 Then '-12=no-data
            lblError.Text = "Fehler: " & m_Report.Message
        Else
            If m_Report.Result Is Nothing OrElse m_Report.Result.Rows.Count = 0 Then
                lblError.Text = "Es wurden keine Daten zur Anzeige gefunden."
            Else

                m_Report.Result.Columns.Add("Zuordnen", System.Type.GetType("System.Boolean"))
                m_Report.Result.Columns.Add("Haendlernummer", System.Type.GetType("System.String"))
                m_Report.Result.Columns.Add("Lizenznr", System.Type.GetType("System.String"))
                m_Report.Result.Columns.Add("Finart", System.Type.GetType("System.String"))
                m_Report.Result.Columns.Add("Label", System.Type.GetType("System.String"))
                m_Report.Result.Columns.Add("Status", System.Type.GetType("System.String"))

                m_Report.Result.AcceptChanges()

                Dim Row As DataRow

                For Each Row In m_Report.Result.Rows
                    Row("Zuordnen") = False
                Next


                Session.Add("Result", m_Report.Result)

                Result.Visible = True
                FillGrid(0)

            End If
        End If
    End Sub


    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal Zugeordnet As Boolean = False)


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

            If Zugeordnet = True Then
                tmpDataView.RowFilter = "Zuordnen = True"
            End If

            GridView1.PageIndex = intTempPageIndex
            GridView1.DataSource = tmpDataView

            GridView1.DataBind()

        End If
    End Sub

    Private Sub UpdateDataTableZuordnen()


        Dim TempTable As DataTable = Session("Result")
        Dim tmpRows() As DataRow
        Dim row As GridViewRow
        Dim booChecked As Boolean = False
        Dim label As Label
        Dim chkBox As CheckBox

        For Each row In GridView1.Rows

            chkBox = CType(row.Cells(4).FindControl("chkAdresseAnzeigen"), CheckBox)
            label = CType(row.Cells(2).FindControl("lblFahrgestellnummer"), Label)

            If chkBox.Checked = True Then

                tmpRows = TempTable.Select("Fahrgestellnummer='" & label.Text & "'")
                tmpRows(0).BeginEdit()
                tmpRows(0).Item("Zuordnen") = True
                tmpRows(0).EndEdit()
                TempTable.AcceptChanges()

            End If

        Next


        Session("Result") = TempTable


    End Sub
    Private Sub UpdateDataTableSave()


        Dim TempTable As DataTable = Session("Result")
        Dim tmpRows() As DataRow
        Dim row As GridViewRow
        Dim booChecked As Boolean = False
        Dim label As Label
        Dim Haendlernummer As String
        Dim Lizenznr As String
        Dim Finart As String
        Dim sLabel As String
        Dim txt As TextBox
        Dim ddl As DropDownList

        For Each row In GridView1.Rows

            txt = CType(row.Cells(5).FindControl("txtHaendlernummer"), TextBox)
            Haendlernummer = txt.Text
            txt = CType(row.Cells(6).FindControl("txtLizenznr"), TextBox)
            Lizenznr = txt.Text
            ddl = CType(row.Cells(7).FindControl("ddlFinart"), DropDownList)
            Finart = ddl.SelectedValue
            txt = CType(row.Cells(8).FindControl("txtLabel"), TextBox)
            sLabel = txt.Text
            label = CType(row.Cells(2).FindControl("lblFahrgestellnummer"), Label)
            tmpRows = TempTable.Select("Fahrgestellnummer='" & label.Text & "'")
            tmpRows(0).BeginEdit()
            tmpRows(0).Item("Zuordnen") = True
            tmpRows(0).Item("Haendlernummer") = Haendlernummer
            tmpRows(0).Item("Lizenznr") = Lizenznr
            tmpRows(0).Item("Finart") = Finart
            tmpRows(0).Item("Label") = sLabel
            tmpRows(0).EndEdit()
            TempTable.AcceptChanges()


        Next


        Session("Result") = TempTable


    End Sub


#End Region




End Class
' ************************************************
' $History: Report06.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 6.04.11    Time: 16:41
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 12.01.11   Time: 15:54
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 12.01.11   Time: 13:15
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 1.02.10    Time: 10:40
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 28.08.09   Time: 12:53
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 11.08.09   Time: 14:36
' Updated in $/CKAG2/Applications/AppF2/forms
' ITA: 3069
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 7.08.09    Time: 16:25
' Updated in $/CKAG2/Applications/AppF2/forms
' ITA: 3069
' 