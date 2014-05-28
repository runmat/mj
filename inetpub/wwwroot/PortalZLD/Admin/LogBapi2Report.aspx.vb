Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel

Partial Public Class LogBapi2Report
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App
#Region " Declarations"
    Private werteTable As DataTable
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        FillCustomer(cn)
    End Sub
    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn, True, False)

        Dim dv As DataView = dtCustomers.DefaultView
        dv.Sort = "Customername"
        ' m_context.Cache.Insert("myCustomerListView", dv, Nothing, DateTime.Now.AddMinutes(20), TimeSpan.Zero)
        Session.Add("myCustomerListView", dv)

        With ddlFilterCustomer
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
        End With
        ddlFilterCustomer.SelectedIndex = 0
    End Sub
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Try


            If werteTable.Rows.Count > 0 Then

                Result.Visible = True
                lnkCreateExcel.Visible = True
                Dim tmpDataView As New DataView()
                tmpDataView = werteTable.DefaultView

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
                gvResult.Visible = True

            Else
                Result.Visible = False
                lblError.Visible = True
                lblError.Text = "Keine Daten für diese Selektion."
            End If
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = "Fehler: " & ex.Message
        End Try

    End Sub

    Private Sub DoSubmit()
        Dim iKundenr As Integer
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            If txtFriendName.Text.Length = 0 Then txtFriendName.Text = "*"
            If txtTechName.Text.Length = 0 Then txtTechName.Text = "*"
            If txtBapiName.Text.Length = 0 Then txtBapiName.Text = "*"

            iKundenr = CInt(ddlFilterCustomer.SelectedItem.Value)

            cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT * " & _
                                                    "FROM BapiToReport " & _
                                                    " WHERE BAPI LIKE @BapiName " & _
                                                    " AND Technischer_Name LIKE @Technischer_Name " & _
                                                    " AND Freundlicher_Name LIKE @Freundlicher_Name ", cn)

            da.SelectCommand.Parameters.AddWithValue("@Freundlicher_Name", Replace(txtFriendName.Text.Trim, "*", "%"))
            da.SelectCommand.Parameters.AddWithValue("@Technischer_Name", Replace(txtTechName.Text.Trim, "*", "%"))
            da.SelectCommand.Parameters.AddWithValue("@BapiName", Replace(txtBapiName.Text.Trim, "*", "%"))



            If txtDateVon.Text.Length > 0 Then
                Dim datAb As Date = CDate(txtDateVon.Text)
                da.SelectCommand.CommandText &= "AND Lastuse >= @LastuseVon "
                da.SelectCommand.Parameters.AddWithValue("@LastuseVon", datAb)
            End If
            If txtDateBis.Text.Length > 0 Then
                Dim datBis As Date = CDate(txtDateBis.Text & " 23:59:59")
                da.SelectCommand.CommandText &= "AND Lastuse <= @LastuseBis "
                da.SelectCommand.Parameters.AddWithValue("@LastuseBis", datBis)
            End If
            If iKundenr > 0 Then
                da.SelectCommand.CommandText &= "AND CustomerID = @CustomerID "
                da.SelectCommand.Parameters.AddWithValue("@CustomerID", iKundenr)
            End If

            werteTable = New DataTable
            da.Fill(werteTable)
            Session("LogwerteResult") = werteTable
            Session("LogwerteResultExcel") = werteTable
            FillGrid(0)

        Catch ex As Exception
            Me.lblError.Text = "Bitte Selektionsparameter auswählen."
        Finally
            cn.Dispose()
            cn.Close()
        End Try

    End Sub

    Protected Sub btnSuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSuche.Click
        Dim BoolDateVon As Boolean = False
        Dim BoolDateBis As Boolean = False
        If txtDateVon.Text.Length > 0 Then
            If Not IsDate(txtDateVon.Text) Then
                If Not IsStandardDate(txtDateVon.Text) Then
                    lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                    Exit Sub
                Else
                    BoolDateVon = True
                End If
            Else
                BoolDateVon = True
            End If
        End If

        If txtDateBis.Text.Length > 0 Then
            If Not IsDate(txtDateBis.Text) Then
                If Not IsStandardDate(txtDateBis.Text) Then
                    lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                    Exit Sub
                Else
                    BoolDateBis = True
                End If
            Else
                BoolDateBis = True
            End If
        End If

        If BoolDateBis And BoolDateVon Then
            Dim datAb As Date = CDate(txtDateVon.Text)
            Dim datBis As Date = CDate(txtDateBis.Text)
            If datAb > datBis Then
                lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
                Exit Sub
            End If
        End If
        DoSubmit()
    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Dim ExcelTable As DataTable
        If Not (Session("LogwerteResultExcel") Is Nothing) Then

            ExcelTable = Session("LogwerteResultExcel")
            If ExcelTable.Columns.CanRemove(ExcelTable.Columns("ID")) Then ExcelTable.Columns.Remove("ID")
            If ExcelTable.Columns.CanRemove(ExcelTable.Columns("CustomerID")) Then ExcelTable.Columns.Remove("CustomerID")
            If ExcelTable.Columns.CanRemove(ExcelTable.Columns("Pfad")) Then ExcelTable.Columns.Remove("Pfad")

            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

                excelFactory.CreateDocumentAndSendAsResponse(strFileName, ExcelTable, Me.Page)

            Catch ex As Exception
                lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
            End Try
        End If
    End Sub

#End Region

#Region " Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Master)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvResult)

        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        Try

            If Not IsPostBack Then
                FillForm()

            ElseIf Not Session("LogwerteResult") Is Nothing Then
                werteTable = Session("LogwerteResult")
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Bapi2Report", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try
    End Sub
    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        FillGrid(gvResult.PageIndex, e.SortExpression)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
#End Region


    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        tab1.Visible = Not tab1.Visible
        btnSuche.Visible = Not btnSuche.Visible
        Queryfooter.Visible = Not Queryfooter.Visible
    End Sub
End Class