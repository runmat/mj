Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports System.Web.UI
Imports Admin.CKG_Adapter

Partial Public Class Bapi2Report
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App

#Region " Declarations"
    Private werteTable As DataTable
    Private ApplicationsList As DataTable
    Private BapiList As DataTable

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
                Dim tmpDataView As DataView = werteTable.DefaultView

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
                lblError.Text = "Keine Daten für diese Selektion."
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try

    End Sub

    Private Sub FillLists()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM Application order by AppName, AppFriendlyName", cn)
            ApplicationsList = New DataTable
            da.Fill(ApplicationsList)

            Dim da2 As New SqlClient.SqlDataAdapter("SELECT * FROM BAPI order by BAPI", cn)
            BapiList = New DataTable
            da2.Fill(BapiList)

            DDL_ApplicationID.Items.Clear()

            For Each xrow In ApplicationsList.Rows
                Dim listitem As ListItem = New ListItem
                'listitem.Text = xrow("AppFriendlyName")
                'listitem.Text = Server.HtmlEncode(xrow("AppName") & " / " & xrow("AppFriendlyName"))
                listitem.Text = xrow("AppName") & " / " & xrow("AppFriendlyName")
                listitem.Value = xrow("AppID")
                'listitem.Attributes.Add("OptionGroup", xrow("AppName"))
                DDL_ApplicationID.Items.Add(listitem)
            Next

            Dim i As Integer = 0
            For Each xrow In ApplicationsList.Rows
                
                Dim listitem As New HtmlGenericControl("li")

                Dim objLink As New HyperLink()
                objLink.NavigateUrl = "javascript:void(0)"
                'objLink.Attributes.Add("onClick", "$('#" & DDL_ApplicationID.ClientID & "').val($('#" & DDL_ApplicationID.ClientID & " option:contains(\'" & xrow("AppName") & " / " & xrow("AppFriendlyName") & "\')').attr('value')) ")
                objLink.Attributes.Add("onClick", "$('#" & DDL_ApplicationID.ClientID & "').val(" & xrow("AppID") & ")")
                objLink.ID = "ul_li_" & i.ToString()
                objLink.Text = Server.HtmlEncode(xrow("AppName") & " / " & xrow("AppFriendlyName"))
                listitem.Controls.Add(objLink)

                UL_ApplicationID.Controls.Add(listitem)
                i += 1
            Next


            DDL_BapiID.Items.Clear()
            For Each xrow In BapiList.Rows
                Dim listitem As ListItem = New ListItem
                listitem.Text = xrow("BAPI")
                listitem.Value = xrow("ID")
                'listitem.Attributes.Add("OptionGroup", xrow("AppName"))
                DDL_BapiID.Items.Add(listitem)
            Next

            'FilterNew.Attributes.Add("onChange", "$('#" & DDL_ApplicationID.ClientID & "').filterByText($(this).val()); ")
            'FilterNew.Attributes.Add("onClick", "alert('go') ")
            Dim csName As [String] = "FilterScript"
            Dim csType As Type = Me.[GetType]()

            ' Get a ClientScriptManager reference from the Page class. 
            Dim cs As ClientScriptManager = Page.ClientScript

            If Not cs.IsClientScriptBlockRegistered(csType, csName) Then
                Dim csText As New StringBuilder()
                csText.Append("<script type=""text/javascript"">")
                csText.Append("$('#" & DDL_ApplicationID.ClientID & "').selectfilter(); </")
                csText.Append("script>")
                cs.RegisterClientScriptBlock(csType, csName, csText.ToString())
            End If


            Dim csName2 As [String] = "FilterScript2"
            Dim csType2 As Type = Me.[GetType]()

            ' Get a ClientScriptManager reference from the Page class. 
            Dim cs2 As ClientScriptManager = Page.ClientScript

            Dim javascript As String = "" & vbCrLf
            javascript += "(function ($) {" & vbCrLf
            javascript += "var wto; " & vbCrLf
            javascript += "  jQuery.expr[':'].Contains = function(a,i,m){" & vbCrLf
            javascript += "      return (a.textContent || a.innerText || '').toUpperCase().indexOf(m[3].toUpperCase())>=0;" & vbCrLf
            javascript += "  };" & vbCrLf
            javascript += "  function listFilter(header, list) { " & vbCrLf
            javascript += "    $('#" & FilterUL.ClientID & "')" & vbCrLf

            javascript += "      .change( function () {" & vbCrLf

            javascript += "        var filter = $(this).val();" & vbCrLf

            'delay
            javascript += "         clearTimeout(wto); " & vbCrLf
            javascript += "         wto = setTimeout(function() { " & vbCrLf



            javascript += "        if(filter) {" & vbCrLf
            javascript += "             if(filter.length > 3) {" & vbCrLf
            javascript += "                 // this finds all links in a list that contain the input," & vbCrLf
            javascript += "                 // and hide the ones not containing the input while showing the ones that do" & vbCrLf
            javascript += "                 $(list).find('a:not(:Contains(' + filter + '))').parent().slideUp(2000);" & vbCrLf
            javascript += "                 $(list).find('a:Contains(' + filter + ')').parent().slideDown(2000);" & vbCrLf
            javascript += "                 $('#" & FilterULError.ClientID & "').slideUp(2000).html('');" & vbCrLf
            javascript += "             } else {" & vbCrLf
            javascript += "                 $(list).find('li').slideUp(3000);" & vbCrLf
            javascript += "                 $('#" & FilterULError.ClientID & "').html('Bitte geben Sie mehr als drei Zeichen an!<br /><br />').slideDown(2000);" & vbCrLf

            javascript += "             }" & vbCrLf
            javascript += "        } else {" & vbCrLf
            javascript += "          $(list).find('li').slideUp(3000);" & vbCrLf
            javascript += "        }" & vbCrLf
            javascript += "        return false;" & vbCrLf

            'ende delay
            javascript += "         }, 1000); " & vbCrLf

            javascript += "      })" & vbCrLf
            javascript += "    .keyup( function () {" & vbCrLf
            javascript += "        // fire the above change event after every letter" & vbCrLf
            javascript += "        $(this).change();" & vbCrLf
            javascript += "    });" & vbCrLf
            javascript += "  }" & vbCrLf
            javascript += "  //ondomready" & vbCrLf
            javascript += "  $(function () {" & vbCrLf
            javascript += "     listFilter($('#FilterHeader'), $('#" & UL_ApplicationID.ClientID & "'));" & vbCrLf
            'javascript += "                 $($('#" & UL_ApplicationID.ClientID & "')).find('li').slideUp(100);" & vbCrLf
            javascript += "     $('#" & UL_ApplicationID.ClientID & "').find('a:not(:Contains(\'startup\'))').parent().slideUp(0);" & vbCrLf
            javascript += "  });" & vbCrLf
            javascript += "}(jQuery));" & vbCrLf

            FilterUL.Attributes.Add("onClick", "$('#" & UL_ApplicationID.ClientID & "').show()")

            If Not cs2.IsClientScriptBlockRegistered(csType2, csName2) Then
                Dim csText2 As New StringBuilder()
                csText2.Append("<script type=""text/javascript"">")
                csText2.Append(javascript & " </")
                csText2.Append("script>")
                cs2.RegisterClientScriptBlock(csType2, csName2, csText2.ToString())
            End If




        Catch ex As Exception
            Me.lblError.Text = "Bitte Selektionsparameter auswählen."
        Finally
            cn.Dispose()
            cn.Close()
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

            Dim da As New SqlClient.SqlDataAdapter("SELECT ApplicationID, BapiID, LastUsed from ApplicationBAPI inner join BAPI on ApplicationBAPI.BapiID = BAPI.ID inner join Application on ApplicationBAPI.ApplicationID = Application.AppID WHERE BAPI.BAPI LIKE @BapiName AND AppName LIKE @Technischer_Name AND AppFriendlyName LIKE @Freundlicher_Name ", cn)

            da.SelectCommand.Parameters.AddWithValue("@Freundlicher_Name", Replace(txtFriendName.Text.Trim, "*", "%"))
            da.SelectCommand.Parameters.AddWithValue("@Technischer_Name", Replace(txtTechName.Text.Trim, "*", "%"))
            da.SelectCommand.Parameters.AddWithValue("@BapiName", Replace(txtBapiName.Text.Trim, "*", "%"))



            If txtDateVon.Text.Length > 0 Then
                Dim datAb As Date = CDate(txtDateVon.Text)
                da.SelectCommand.CommandText &= "AND LastUsed >= @LastuseVon "
                da.SelectCommand.Parameters.AddWithValue("@LastuseVon", datAb)
            End If
            If txtDateBis.Text.Length > 0 Then
                Dim datBis As Date = CDate(txtDateBis.Text & " 23:59:59")
                da.SelectCommand.CommandText &= "AND LastUsed <= @LastuseBis "
                da.SelectCommand.Parameters.AddWithValue("@LastuseBis", datBis)
            End If
            'If iKundenr > 0 Then
            '    da.SelectCommand.CommandText &= "AND CustomerID = @CustomerID "
            '    da.SelectCommand.Parameters.AddWithValue("@CustomerID", iKundenr)
            'End If

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

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                FillForm()

            ElseIf Not Session("LogwerteResult") Is Nothing Then
                werteTable = Session("LogwerteResult")
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Bapi2Report", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
        End Try

        If Not IsPostBack Then
            FillLists()
        End If

    End Sub
    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        FillGrid(gvResult.PageIndex, e.SortExpression)
    End Sub

    Private Sub gvResult_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If ApplicationsList Is Nothing Or BapiList Is Nothing Then
            FillLists()
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each xrow In ApplicationsList.Rows
                Dim listitem As ListItem = New ListItem
                listitem.Text = xrow("AppFriendlyName")
                listitem.Value = xrow("AppID")
                listitem.Attributes.Add("OptionGroup", xrow("AppName"))
                CType(e.Row.FindControl("Grid_DDL_ApplicationID"), GroupedDropDownListAdapter).Items.Add(listitem)
            Next
            CType(e.Row.FindControl("Grid_DDL_ApplicationID"), GroupedDropDownListAdapter).SelectedValue = CType(e.Row.FindControl("Hidden_ApplicationID"), HiddenField).Value
            CType(e.Row.FindControl("Grid_DDL_ApplicationID"), GroupedDropDownListAdapter).Attributes.Add("onClick", "$('#" & CType(e.Row.FindControl("Hidden_New_ApplicationID"), TextBox).ClientID & "').val($(this).val());")

            For Each xrow In BapiList.Rows
                Dim listitem As ListItem = New ListItem
                listitem.Text = xrow("BAPI")
                listitem.Value = xrow("ID")
                'listitem.Attributes.Add("OptionGroup", xrow("AppName"))
                'DDL_BapiID.Items.Add(listitem)
                CType(e.Row.FindControl("DDL_BapiID"), GroupedDropDownListAdapter).Items.Add(listitem)
            Next
            CType(e.Row.FindControl("DDL_BapiID"), GroupedDropDownListAdapter).SelectedValue = CType(e.Row.FindControl("Hidden_BapiID"), HiddenField).Value
            CType(e.Row.FindControl("DDL_BapiID"), GroupedDropDownListAdapter).Attributes.Add("onClick", "$('#" & CType(e.Row.FindControl("Hidden_New_BapiID"), TextBox).ClientID & "').val($(this).val());")

            'löschen
            CType(e.Row.FindControl("IMG_delete"), Image).Attributes.Add("onClick", "$('#" & CType(e.Row.FindControl("Hidden_New_BapiID"), TextBox).ClientID & "').val('0');$('#" & CType(e.Row.FindControl("Hidden_New_ApplicationID"), TextBox).ClientID & "').val('0');$('#" & CType(e.Row.FindControl("Grid_DDL_ApplicationID"), GroupedDropDownListAdapter).ClientID & "').fadeOut();$('#" & CType(e.Row.FindControl("DDL_BapiID"), GroupedDropDownListAdapter).ClientID & "').fadeOut();$(this).fadeOut();")


        End If
    End Sub


    Protected Sub imgbInsert_click(ByVal sender As Object, ByVal e As EventArgs) Handles imgbInsert.Click
        Try
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim cmdInsert As New SqlClient.SqlCommand("INSERT INTO ApplicationBAPI(ApplicationID,BapiID) Values(@ApplicationID,@BapiID)", cn)

            With cmdInsert.Parameters
                .AddWithValue("@ApplicationID", DDL_ApplicationID.SelectedValue)
                .AddWithValue("@BapiID", DDL_BapiID.SelectedValue)
            End With
            cmdInsert.ExecuteNonQuery()

            cn.Close()
            cn.Dispose()
        Catch ex As Exception
            lblError.Text += "Fehler: " & ex.Message & "<br />"
        End Try
        DoSubmit()

    End Sub


    Protected Sub gvResult_Speichern_click(ByVal sender As Object, ByVal e As EventArgs) Handles gvResult_Speichern.Click

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        For Each xrow As GridViewRow In gvResult.Rows
            Try
                If ((CType(xrow.FindControl("Grid_DDL_ApplicationID"), GroupedDropDownListAdapter).SelectedValue <> CType(xrow.FindControl("Hidden_ApplicationID"), HiddenField).Value) Or CType(xrow.FindControl("Hidden_New_ApplicationID"), TextBox).Text = "0") Then
                    Dim cmdUpdate As New SqlClient.SqlCommand()
                    If CType(xrow.FindControl("Hidden_New_ApplicationID"), TextBox).Text = "0" Then
                        cmdUpdate = New SqlClient.SqlCommand("delete from ApplicationBAPI where ApplicationID = '" & CType(xrow.FindControl("Hidden_ApplicationID"), HiddenField).Value & "' and BapiID = '" & CType(xrow.FindControl("Hidden_BapiID"), HiddenField).Value & "'", cn)
                    Else
                        cmdUpdate = New SqlClient.SqlCommand("update ApplicationBAPI set ApplicationID = '" & CType(xrow.FindControl("Hidden_New_ApplicationID"), TextBox).Text & "' ,BapiID = '" & CType(xrow.FindControl("Hidden_New_BapiID"), TextBox).Text & "' where ApplicationID = '" & CType(xrow.FindControl("Hidden_ApplicationID"), HiddenField).Value & "' and BapiID = '" & CType(xrow.FindControl("Hidden_BapiID"), HiddenField).Value & "'", cn)
                    End If
                    'lblError.Visible = True
                    'lblError.Text += cmdUpdate.CommandText() & "<br>"
                    'With cmdUpdate.Parameters
                    '.AddWithValue("@ApplicationID", DDL_ApplicationID.SelectedValue)
                    '.AddWithValue("@BapiID", DDL_BapiID.SelectedValue)
                    'End With
                    cmdUpdate.ExecuteNonQuery()

                End If
            Catch ex As Exception
                lblError.Text += "Fehler: " & ex.Message & "<br />"
            End Try
        Next
        cn.Close()
        cn.Dispose()

        DoSubmit()
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