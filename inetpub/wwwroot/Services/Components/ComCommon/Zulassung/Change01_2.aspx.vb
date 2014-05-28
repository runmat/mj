Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Namespace Zulassung
    Partial Public Class Change01_2
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private objZulassung As Zulassung1
        Private booError As Boolean
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            FormAuth(Me, m_User, True)
            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Security.App(m_User)
            GridNavigation1.setGridElment(GridView1)

            If IsNothing(Session("objZulassung")) Then Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)

            lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
            objZulassung = CType(Session("objZulassung"), Zulassung1)
            Try
                If Not IsPostBack Then
                    FillGrid(0)
                End If
            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
            End Try
        End Sub


        Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
            If objZulassung.Status = 0 OrElse objZulassung.Status = 102 Then
                If objZulassung.Fahrzeuge.Rows.Count = 0 Then
                    Result.Visible = True
                Else
                    Result.Visible = True

                    Dim tmpDataView As New DataView()
                    tmpDataView = objZulassung.Fahrzeuge.DefaultView

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
                    lblNoData.Visible = True
                    lblNoData.Text = "Wählen Sie die Fahrzeuge zur Zulassung aus und klicken Sie auf ""Weiter""."
                End If
            Else
                GridView1.Visible = False
                lblError.Text = objZulassung.Message
            End If
        End Sub

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

                            tmpRows = objZulassung.Fahrzeuge.Select(strZZFAHRG)
                            If (tmpRows.Length > 0) Then
                                tmpRows(0).BeginEdit()
                                Select Case chbox.ID
                                    Case "chk0000"
                                        If chbox.Checked Then           'anfordern
                                            tmpRows(0).Item("AUSWAHL") = "99"
                                            intReturn += 1
                                        Else
                                            tmpRows(0).Item("AUSWAHL") = ""
                                        End If
                                End Select
                                tmpRows(0).EndEdit()
                                objZulassung.Fahrzeuge.AcceptChanges()
                            End If
                        End If
                    Next
                Next
            Next
            Session("objZulassung") = objZulassung
            Return intReturn
        End Function

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub
        Private Sub gvFahrzeuge_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
            CheckGrid()
            FillGrid(pageindex)
        End Sub

        Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
            CheckGrid()
            FillGrid(GridView1.PageIndex)
        End Sub

        Private Sub gvFahrzeuge_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
            CheckGrid()
            FillGrid(GridView1.PageIndex, e.SortExpression)
        End Sub

        Protected Sub cmdContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdContinue.Click
            CheckGrid()

            Dim tmpDataView As New DataView()
            tmpDataView = objZulassung.Fahrzeuge.DefaultView

            tmpDataView.RowFilter = "AUSWAHL = '99'"
            Dim intFahrzeugBriefe As Int32 = tmpDataView.Count
            tmpDataView.RowFilter = ""

            If intFahrzeugBriefe = 0 Then
                intFahrzeugBriefe = tmpDataView.Count
                tmpDataView.RowFilter = ""

                If intFahrzeugBriefe = 0 Then
                    lblError.Text = "Bitte wählen Sie erst Fahrzeuge zur Anforderung aus."
                    FillGrid(GridView1.PageIndex)
                End If
            Else
                Session("objZulassung") = objZulassung
                Response.Redirect("Change01_3.aspx?AppID=" & Session("AppID").ToString)
            End If
        End Sub
        Protected Sub ibtAuswahl_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
            lblMessagePopUp.Text = "Sie sind im Begriff " & objZulassung.Fahrzeuge.Select("Bem=''").Length & " Fahrzeuge zu selektieren."
            lblMessagePopUp.Text &= "Wollen Sie fortfahren?<br />"
            ModalPopupExtender2.Show()
        End Sub
        Protected Sub ibtnAbwahl_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
            Dim chk As CheckBox

            For Each dr As GridViewRow In GridView1.Rows
                chk = CType(dr.FindControl("chk0000"), CheckBox)
                chk.Checked = False
            Next

            For Each tmpRows As DataRow In objZulassung.Fahrzeuge.Rows
                tmpRows("AUSWAHL") = ""
            Next
        End Sub

        Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOK.Click
            Dim chk As CheckBox

            For Each dr As GridViewRow In GridView1.Rows
                chk = CType(dr.FindControl("chk0000"), CheckBox)
                chk.Checked = True
            Next

            For Each tmpRows As DataRow In objZulassung.Fahrzeuge.Rows
                tmpRows("AUSWAHL") = "99"
            Next
            objZulassung.Fahrzeuge.AcceptChanges()
            Session("objZulassung") = objZulassung
        End Sub
    End Class
End Namespace