Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Namespace Beauftragung2

    Partial Public Class Report55s
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_App As App
        Private m_User As User
        Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
        Private m_objExcel As DataTable
#End Region


#Region "Events"


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            FormAuth(Me, m_User)
            GetAppIDFromQueryString(Me)
            m_App = New App(m_User)
            GridNavigation1.setGridElment(gvBestand)

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

        Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
            Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
            HelpProcedures.FixedGridViewCols(gvBestand)
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

        Private Sub gvBestand_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvBestand.Sorting
            FillGrid(0, e.SortExpression)
        End Sub

#End Region

#Region "Methods"

        Private Sub DoSubmit()
            Try
                lblError.Text = ""

                Dim datAb As Date
                Dim datBis As Date

                If txtAbDatum.Text.Length > 0 AndAlso txtBisDatum.Text.Length > 0 Then
                    datAb = CDate(txtAbDatum.Text)
                    datBis = CDate(txtBisDatum.Text)
                Else
                    datAb = CDate("01.01.2000")
                    datBis = CDate("01.01.2050")
                End If


                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                Dim m_Report As New Beauftragung2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

                m_Report.FillZulassung(Session("AppID").ToString, Session.SessionID.ToString, Me, datAb, datBis)

                Session("ResultTable") = m_Report.ResultTable

                If Not m_Report.Status = 0 Then
                    lblError.Text = m_Report.Message
                Else
                    If m_Report.ResultTable.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else
                        Session("ShowOtherString") = "Es wurden " & m_Report.ResultTable.Rows.Count.ToString & " Datensätze gefunden."

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
                lblNewSearch.Visible = True
                ibtNewSearch.Visible = True


            End If
        End Sub



        Private Sub checkDatum()

            If txtAbDatum.Text.Length > 0 AndAlso txtBisDatum.Text.Length > 0 Then
                If IsDate(txtBisDatum.Text) = True And IsDate(txtAbDatum.Text) = True Then
                    Dim datAb As Date = CDate(txtAbDatum.Text)
                    Dim datBis As Date = CDate(txtBisDatum.Text)

                    If (datBis - datAb) < TimeSpan.FromDays(0) Then
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
            Else
                DoSubmit()
            End If



        End Sub

        Protected Sub ibtNewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtNewSearch.Click
            divSelection.Visible = Not divSelection.Visible
            lbCreate.Visible = Not lbCreate.Visible
        End Sub

#End Region


    End Class

End Namespace
' ************************************************
' $History: Report55s.aspx.vb $