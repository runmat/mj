Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change01s_2
    Inherits System.Web.UI.Page

#Region "Declarations"

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    'Private objSuche As Search
    Private objHaendler As Versand1

    Private versandart As String
    Private authentifizierung As String

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        GridNavigation1.setGridElment(GridView1)

        versandart = Request.QueryString.Item("art").ToString
        authentifizierung = Request.QueryString.Item("art2").ToString

        lnkKreditlimit.NavigateUrl = "Change01s.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If

        m_App = New Base.Kernel.Security.App(m_User)

        If Session("objHaendler") Is Nothing Then
            Response.Redirect("Change01s.aspx?AppID=" & Session("AppID").ToString)
        End If

        objHaendler = CType(Session("objHaendler"), Versand1)

        If Not IsPostBack Then
            If versandart = "TEMP" Then
                GridView1.BackColor = System.Drawing.Color.LightSkyBlue
            Else
                GridView1.BackColor = System.Drawing.Color.SandyBrown
            End If
            FillGrid(0, , True)
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        CheckGrid()

        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

        tmpDataView.RowFilter = "MANDT = '99'"
        Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
        tmpDataView.RowFilter = ""

        If intFahrzeugBriefe = 0 Then
            tmpDataView.RowFilter = "SWITCH = 1"
            intFahrzeugBriefe = tmpDataView.Count
            tmpDataView.RowFilter = ""

            If intFahrzeugBriefe = 0 Then
                lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
                FillGrid(GridView1.PageIndex)
            Else
                Session("objHaendler") = objHaendler
                Response.Redirect("Change01s_4.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung)
            End If
        Else
            Session("objHaendler") = objHaendler
            Response.Redirect("Change01s_3.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart & "&art2=" & authentifizierung)
        End If
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub GridView1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub

    Private Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If versandart = "TEMP" Then

            e.Row.Cells(7).Visible = False

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
    Private Function CheckGrid() As Int32
        Dim cell As TableCell
        Dim chbox As CheckBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intReturn As Int32 = 0
        Dim tmpRows As DataRow()

        Dim lbl As Label

        For Each Row As GridViewRow In GridView1.Rows
            Dim strZZFAHRG As String = ""

            lbl = CType(Row.Cells(0).FindControl("lblEqunr"), Label)


            strZZFAHRG = "EQUNR = '" & lbl.Text & "'"

            For Each cell In Row.Cells
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chbox = CType(control, CheckBox)

                        tmpRows = objHaendler.Fahrzeuge.Select(strZZFAHRG)
                        If (tmpRows.Length > 0) Then
                            tmpRows(0).BeginEdit()
                            Select Case chbox.ID
                                Case "chk0000"
                                    If chbox.Checked Then           'anfordern
                                        tmpRows(0).Item("MANDT") = "99"
                                        intReturn += 1
                                    Else
                                        If Not CStr(tmpRows(0).Item("MANDT")) = "11" Then
                                            tmpRows(0).Item("MANDT") = ""
                                        End If
                                    End If
                                Case "chkSWITCH"
                                    If chbox.Checked Then           'Versandart ändern
                                        tmpRows(0).Item("SWITCH") = True
                                        intReturn += 1
                                    Else
                                        tmpRows(0).Item("SWITCH") = False
                                    End If
                            End Select
                            tmpRows(0).EndEdit()
                            objHaendler.Fahrzeuge.AcceptChanges()
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

        Dim blnShowVersandartAendern As Boolean = False

        If versandart = "ENDG" Then
            Dim tmpRow As DataRow

            For Each tmpRow In objHaendler.Fahrzeuge.Rows
                If CStr(tmpRow("MANDT")) = "11" Then
                    blnShowVersandartAendern = True
                End If
            Next
        End If

        If tmpDataView.Count = 0 Then
            GridView1.Visible = False
            'ddlPageSize.Visible = False
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            lblNoData.Visible = True
            'ShowScript.Visible = False
        Else
            'ddlPageSize.Visible = True
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

            lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " KFZ-Brief(e) gefunden."
            lblNoData.Visible = True

            If GridView1.PageCount > 1 Then
                'GridView1.PagerStyle.CssClass = "PagerStyle"
                GridView1.DataBind()
                'GridView1.PagerStyle.Visible = True
            Else
                'GridView1.PagerStyle.Visible = False
            End If

            'Dim item As DataGridItem
            Dim cell As TableCell
            Dim hyperlink As HyperLink
            Dim control As Control

            Dim strHistoryLink As String = ""
            If m_User.Applications.Select("AppName = 'Report02'").Length > 0 Then
                strHistoryLink = "Report02.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report02'")(0)("AppID").ToString & "&VIN="
            End If

            For Each Row As GridViewRow In GridView1.Rows
                For Each cell In Row.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is HyperLink Then
                            hyperlink = CType(control, HyperLink)
                            Select Case hyperlink.ID
                                Case "VIN"
                                    If strHistoryLink.Length > 0 Then
                                        hyperlink.NavigateUrl = strHistoryLink & hyperlink.NavigateUrl
                                    Else
                                        hyperlink.NavigateUrl = ""
                                    End If
                            End Select
                        End If
                    Next
                Next
            Next

            If objHaendler.Fahrzeuge.Select("MANDT='11'").GetUpperBound(0) > -1 Then
                GridView1.Columns(GridView1.Columns.Count - 1).Visible = True
            Else
                GridView1.Columns(GridView1.Columns.Count - 1).Visible = False
            End If

            GridView1.Columns(2).Visible = blnShowVersandartAendern
        End If
    End Sub
#End Region

End Class
' ************************************************
' $History: Change01s_2.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 24.09.09   Time: 17:29
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 3112
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 24.09.09   Time: 10:44
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 3112
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 24.09.09   Time: 9:47
' Created in $/CKAG2/Services/Components/ComCommon
' 
