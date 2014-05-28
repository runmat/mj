Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Services.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports CKG
Partial Public Class Change205_2
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    'Private objSuche As Search
    Private objHaendler As Arval_1
    Private versandart As String
    Private upload As Boolean

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        GridNavigation1.setGridElment(DataGrid1)

        versandart = Request.QueryString.Item("art").ToString
        upload = False
        If Not (Request.QueryString.Item("file") Is Nothing) Then
            upload = CType(Request.QueryString.Item("file").ToString, Boolean)
        End If

        lnkKreditlimit.NavigateUrl = "Change205.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change205.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHaendler = CType(Session("objHaendler"), Arval_1)

            If Not IsPostBack Then

                FillGrid(0, , True)
            Else
                CheckGrid()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        CheckGrid()

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT = '99'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
            FillGrid(DataGrid1.CurrentPageIndex)
        Else
            Session("objHaendler") = objHaendler
            Response.Redirect("Change205_3.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
        End If
    End Sub

    Private Function CheckGrid() As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chbox As CheckBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intReturn As Int32 = 0
        Dim tmpRows As DataRow()

        For Each item In DataGrid1.Items

            Dim strZZFAHRG As String = ""
            For Each cell In item.Cells

                strZZFAHRG = "EQUNR = '" & item.Cells(0).Text & "'"

                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chbox = CType(control, CheckBox)

                        tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                        If (tmpRows.Length > 0) Then
                            Select Case chbox.ID
                                Case "chk0000"
                                    tmpRows(0).BeginEdit()
                                    If Not chbox.Visible Then
                                        tmpRows(0).Item("MANDT") = "77"         'in Autorisierung
                                    Else
                                        If chbox.Checked Then                   'anfordern
                                            tmpRows(0).Item("MANDT") = "99"
                                        Else
                                            tmpRows(0).Item("MANDT") = "11"     'nicht anfordern
                                        End If
                                    End If
                                    tmpRows(0).EndEdit()
                                    objHaendler.Fahrzeuge.AcceptChanges()
                            End Select
                        End If
                    End If
                Next
            Next
        Next
        Session("objHaendler") = objHaendler
        Return intReturn
    End Function

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal fill As Boolean = False)
        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView
        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            GridNavigation1.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            'ShowScript.Visible = False
        Else
            GridNavigation1.Visible = True
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


            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If

            'Controls füllen...

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim chkBox As CheckBox
            Dim control As Control
            Dim strZZFAHRG As String
            Dim tmpRows As DataRow()

            For Each item In DataGrid1.Items
                For Each cell In item.Cells

                    strZZFAHRG = "EQUNR = '" & item.Cells(0).Text & "'"

                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            If chkBox.ID = "chk0000" Then

                                tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                                If tmpRows.Length > 0 Then
                                    If tmpRows(0)("MANDT").ToString = "77" Then   'MANDT = 77, d.h. in Autorisierung
                                        chkBox.Visible = False
                                        chkBox.Checked = False
                                        item.ForeColor = System.Drawing.Color.Gray
                                        item.Font.Bold = True
                                        item.Font.Italic = True
                                    Else
                                        If (tmpRows(0)("MANDT").ToString = "99") AndAlso (upload = True) Then
                                            chkBox.Checked = True                   'Nur, wenn Dateiupload!!!
                                        End If
                                        If tmpRows(0)("MANDT").ToString = "11" Then
                                            chkBox.Checked = False
                                        End If
                                    End If
                                Else
                                    chkBox.Visible = False
                                    chkBox.Checked = False
                                    item.Cells(item.Cells.Count - 1).ForeColor = System.Drawing.Color.Red
                                End If
                            End If
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub PagerChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        FillGrid(pageindex)
    End Sub


    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        'CheckGrid()
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change205_2.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 17.04.09   Time: 14:40
' Created in $/CKAG2/Applications/AppArval/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************