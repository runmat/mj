Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business

Namespace Logistik
    Partial Public Class Change01_2
        Inherits System.Web.UI.Page
        Private m_App As App
        Private m_User As User
        Private m_change As Logistik.Logistik1

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            FormAuth(Me, m_User)
            m_App = New App(m_User)
            GridNavigation1.setGridElment(gvResult)
            GetAppIDFromQueryString(Me)
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            lnkKreditlimit.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString

            If Session("AppChange") Is Nothing Then
                Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
            Else
                m_change = CType(Session("AppChange"), Logistik.Logistik1)
            End If

            If Not IsPostBack Then
                FillGrid(0)
            End If

        End Sub


        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

            Dim tmpDataView As New DataView()
            tmpDataView = m_change.Daten.DefaultView
            tmpDataView.RowFilter = ""
            If tmpDataView.Count = 0 Then
                gvResult.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
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
                gvResult.PageIndex = intTempPageIndex
                gvResult.DataSource = tmpDataView
                gvResult.DataBind()
                For Each item As GridViewRow In gvResult.Rows
                    If Not item.FindControl("lnkHistorie") Is Nothing Then
                        If Not m_User.Applications.Select("AppName = 'Report01'").Count = 0 Then
                            CType(item.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "Report01.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report01'")(0)("AppID").ToString & "&LinkedApp=" & Replace(Me.Page.AppRelativeVirtualPath, "~", "..") & "?AppID=" & Session("AppID").ToString & " &VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text
                        End If
                    End If
                Next
            End If
        End Sub
        Private Function CheckGrid() As Int32
            Dim item As GridViewRow
            Dim cell As TableCell
            Dim rbAuswahl As RadioButton
            Dim lbl As Label
            Dim control As Control
            Dim blnScriptFound As Boolean = False
            Dim intZaehl As Int32
            Dim intReturn As Int32 = 0

            Dim tmpRows As DataRow()

            For Each item In gvResult.Rows
                intZaehl = 1
                Dim strKennz As String = ""

                cell = item.Cells(0)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        lbl = CType(control, Label)
                        If lbl.ID = "LabelKennz" Then
                            strKennz = "LICENSE_NUM = '" & lbl.Text & "'"
                        End If
                    End If
                Next

                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is RadioButton Then
                            rbAuswahl = CType(control, RadioButton)
                            If rbAuswahl.ID = "rbExpress" Then
                                If rbAuswahl.Checked Then
                                    m_change.Daten.AcceptChanges()
                                    tmpRows = m_change.Daten.Select(strKennz)
                                    tmpRows(0).BeginEdit()
                                    tmpRows(0).Item("Express") = "X"
                                    tmpRows(0).Item("Standard") = ""
                                    tmpRows(0).EndEdit()
                                    m_change.Daten.AcceptChanges()
                                End If
                            End If
                            If rbAuswahl.ID = "rbStandard" Then
                                If rbAuswahl.Checked Then
                                    m_change.Daten.AcceptChanges()
                                    tmpRows = m_change.Daten.Select(strKennz)
                                    tmpRows(0).BeginEdit()
                                    tmpRows(0).Item("Standard") = "X"
                                    tmpRows(0).Item("Express") = ""
                                    tmpRows(0).EndEdit()
                                    m_change.Daten.AcceptChanges()
                                End If
                            End If
                        End If
                    Next
                    intZaehl += 1
                Next
            Next
            Session("AppChange") = m_change
            Return intReturn
        End Function

        Private Sub gvResult_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
            gvResult.EditIndex = -1
            FillGrid(pageindex)
        End Sub



        Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
            FillGrid(gvResult.PageIndex)
        End Sub
        Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
            FillGrid(0)
        End Sub


        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
            DoSubmit()
        End Sub

        Private Sub DoSubmit()
            CheckGrid()

            Dim tmpDataView As New DataView()
            tmpDataView = m_change.Daten.DefaultView

            tmpDataView.RowFilter = "Express = 'X' OR Standard = 'X'"
            Dim iAuswahlCount As Int32 = tmpDataView.Count
            tmpDataView.RowFilter = ""

            If iAuswahlCount = 0 Then
                lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
                FillGrid(gvResult.PageIndex)

            Else
                Session("AppHaendler") = m_change
                Response.Redirect("Change01_3.aspx?AppID=" & Session("AppID").ToString)

            End If
        End Sub
    End Class
End Namespace
