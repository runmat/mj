Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Public Class EFA_IPVerwaltung
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App
    Private dtStatus As DataTable
    Private objCustomer As DataTable
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Organization)
        m_App = New App(m_User)
        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(gv)
        Try

            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                Dim tempDataSet As DataSet
                tempDataSet = GetCustomer()
                If Not tempDataSet Is Nothing Then
                    objCustomer = tempDataSet.Tables(0)
                End If
                Session("objCustomer") = objCustomer
                getDataSource()
                fillGrid(0)

            Else
                If dtStatus Is Nothing Then
                    dtStatus = CType(Session("KBS_EFADT"), DataTable)
                End If
                If objCustomer Is Nothing Then
                    objCustomer = CType(Session("objCustomer"), DataTable)
                End If
            End If

        Catch ex As Exception
            lblError.Text = ex.ToString
        End Try

    End Sub


    Private Sub fillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal strSearchString As String = "")


        Dim tmpView As New DataView
        'nach neuladen aus der datenbank ist natürlich die sortierung wieder neu, daher view in session speichern 
        tmpView = dtStatus.DefaultView
        tmpView.RowFilter = strSearchString
        If Not ViewState("SearchString") Is Nothing Then
            tmpView.RowFilter = ViewState("SearchString").ToString
        End If

        If Not ViewState("Direction") Is Nothing And Not ViewState("SortExpression") Is Nothing Then
            tmpView.Sort = ViewState("SortExpression").ToString & " " & ViewState("Direction").ToString
        End If

        If Not strSort.Trim(" "c).Length = 0 Then
            Dim strDirection As String
            If ViewState("Direction") Is Nothing Then
                strDirection = "desc"
            Else
                strDirection = ViewState("Direction").ToString
            End If

            If strDirection = "asc" Then
                strDirection = "desc"
            Else
                strDirection = "asc"
            End If

            ViewState("SortExpression") = strSort
            ViewState("Direction") = strDirection
            ViewState("SearchString") = strSearchString
            tmpView.Sort = strSort & " " & strDirection
        End If
        gv.PageIndex = intPageIndex
        gv.DataSource = tmpView
        gv.DataBind()


    End Sub
    Private Sub getDataSource()
        Dim cn As SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim ds As DataSet
        Dim ad As SqlClient.SqlDataAdapter
        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try

            cn.Open()
            ds = New DataSet()
            ad = New SqlClient.SqlDataAdapter()

            cmdAg = New SqlClient.SqlCommand("SELECT * FROM KBS_EFA ", cn)
            cmdAg.CommandType = CommandType.Text
            ad.SelectCommand = cmdAg
            ad.Fill(ds, "StatusTabelle")

            If ds.Tables("StatusTabelle") Is Nothing Then
                lblError.Text = "Es wurden keine Datensätze gefunden"
            End If
            dtStatus = ds.Tables("StatusTabelle")

            Session.Add("KBS_EFADT", dtStatus)


        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
        End Try

    End Sub


    Private Function GetCustomer() As DataSet
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try
            cn.Open()

            Dim daCustomers As SqlClient.SqlDataAdapter
            daCustomers = New SqlClient.SqlDataAdapter("SELECT CustomerID, Customername, KUNNR  " & _
                                                            "FROM vwCustomer where Customername ='Kroschke' OR Customername ='KBS' OR Customername ='nicht Kroschke' OR Customername ='Kroschke Partner' ORDER By Customername", cn)

            Dim dsCustomers As New DataSet
            daCustomers.Fill(dsCustomers)
            Return dsCustomers

        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Kunden!"
            Return Nothing
        End Try


    End Function

    Protected Sub ibtnSearch_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearch.Click
        Dim strSearch As String = ""
        If txtSuche.Text.Length > 0 Then
            strSearch = ddlSearch.SelectedValue + " = '" + txtSuche.Text + "'"
        End If

        fillGrid(0, "", strSearch)
        ibtnNoFilter.Visible = True
    End Sub

    Protected Sub ibtnNoFilter_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtnNoFilter.Click
        fillGrid(0)
        ibtnNoFilter.Visible = False
    End Sub

    Private Sub gv_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv.RowCommand


        If Not e.CommandName = "sort" Then
            Dim KassenIP As String


            Dim tmpGVRow As GridViewRow
            Dim tmpgvRowindex As Int32
            Dim cmdCommand As SqlClient.SqlCommand
            Dim cn As SqlClient.SqlConnection


            If dtStatus Is Nothing Then
                dtStatus = CType(Session("KBS_EFADT"), DataTable)
            End If


            KassenIP = e.CommandArgument.ToString

            For Each tmpGVRow In gv.Rows
                If gv.DataKeys(tmpGVRow.RowIndex)("IP").ToString = e.CommandArgument.ToString Then
                    tmpgvRowindex = tmpGVRow.RowIndex
                    Exit For
                End If
            Next

            If e.CommandName = "save" Then
                Dim textIP As String
                Dim textLGORT As String
                Dim textWERK As String
                Dim textMaster As String = "0"
                Dim textFirma As String
                Dim textCustID As String
                Dim textKunnr As String
                Dim txtBox As TextBox
                Dim chkMaster As CheckBox
                Dim ddlFirma As DropDownList
                txtBox = CType(gv.Rows(tmpgvRowindex).FindControl("txtIP"), TextBox)
                textIP = txtBox.Text
                txtBox = CType(gv.Rows(tmpgvRowindex).FindControl("txtLGORT"), TextBox)
                textLGORT = txtBox.Text
                txtBox = CType(gv.Rows(tmpgvRowindex).FindControl("txtWERKS"), TextBox)
                textWERK = txtBox.Text
                chkMaster = CType(gv.Rows(tmpgvRowindex).FindControl("chkMaster"), CheckBox)
                If chkMaster.Checked Then textMaster = "1"
                ddlFirma = CType(gv.Rows(tmpgvRowindex).FindControl("ddlFirma"), DropDownList)
                textFirma = ddlFirma.SelectedItem.Text
                textCustID = ddlFirma.SelectedValue.Split(":"c)(0)
                textKunnr = ddlFirma.SelectedValue.Split(":"c)(1)

                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try


                    cn.Open()
                    cmdCommand = New SqlClient.SqlCommand("Update KBS_EFA Set IP='" & textIP &
                                                          "', WERKS ='" & textWERK &
                                                          "', LGORT ='" & textLGORT &
                                                          "', Firma ='" & textFirma &
                                                          "', Kunnr ='" & textKunnr &
                                                          "', CustomerID ='" & textCustID &
                                                          "', Master =" & textMaster &
                                                          "  where IP='" & e.CommandArgument.ToString() & "'")
                    cmdCommand.Connection = cn
                    cmdCommand.CommandType = CommandType.Text
                    cmdCommand.ExecuteNonQuery()
                    lblMessage.Text = "Kasse - " & KassenIP & " - erfolgreich aktualisiert"


                Catch ex As Exception
                    lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message

                Finally
                    cn.Close()
                End Try

                'ElseIf e.CommandName = "save" Then
                '    Dim text As String
                '    Dim tmpTXT As TextBox
                '    tmpTXT = CType(gv.Rows(tmpgvRowindex).FindControl("txtWERKS"), TextBox)
                '    text = tmpTXT.Text


                '    cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                '    Try


                '        cn.Open()
                '        cmdCommand = New SqlClient.SqlCommand("Update KBS_EFA Set WERKS='" & text & "' where IP='" & e.CommandArgument.ToString & "'")
                '        cmdCommand.Connection = cn
                '        cmdCommand.CommandType = CommandType.Text
                '        cmdCommand.ExecuteNonQuery()
                '        lblMessage.Text = "Kasse - " & KassenIP & " - erfolgreich aktualisiert"


                '    Catch ex As Exception
                '        lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message

                '    Finally
                '        cn.Close()
                '    End Try

            ElseIf e.CommandName = "entfernen" Then


                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try


                    cn.Open()
                    cmdCommand = New SqlClient.SqlCommand("Delete FROM KBS_EFA where IP='" & e.CommandArgument.ToString & "'")
                    cmdCommand.Connection = cn
                    cmdCommand.CommandType = CommandType.Text
                    cmdCommand.ExecuteNonQuery()
                    lblMessage.Text = "Kasse - " & KassenIP & " - erfolgreich entfernt"


                Catch ex As Exception
                    lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message

                Finally
                    cn.Close()
                End Try


            End If
            getDataSource()
            Dim strSearch As String = ""
            If txtSuche.Text.Length > 0 Then
                strSearch = ddlSearch.SelectedValue + " = '" + txtSuche.Text + "'"
            End If

            fillGrid(0, "", strSearch)
        End If
    End Sub

    Private Sub gv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = ListItemType.Item Then
            Dim tmpDDL As DropDownList
            tmpDDL = CType(e.Row.FindControl("ddlFirma"), DropDownList)
            If Not tmpDDL Is Nothing Then
                For Each drow As DataRow In objCustomer.Rows
                    Dim tmpListItem As New ListItem
                    tmpListItem.Value = drow("CustomerID").ToString & ":" & drow("KUNNR").ToString & ":" & e.Row.DataItem.item("IP")
                    tmpListItem.Text = drow("Customername").ToString
                    tmpDDL.Items.Add(tmpListItem)

                    If drow("CustomerID").ToString = e.Row.DataItem("CustomerID").ToString Then
                        tmpDDL.SelectedValue = tmpListItem.Value
                    End If

                Next
            End If
        End If
    End Sub
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        gv.PageIndex = PageIndex
        Dim strSearch As String = ""
        If txtSuche.Text.Length > 0 Then
            strSearch = ddlSearch.SelectedValue + " = '" + txtSuche.Text + "'"
        End If
        fillGrid(PageIndex, "", strSearch)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        Dim strSearch As String = ""
        If txtSuche.Text.Length > 0 Then
            strSearch = ddlSearch.SelectedValue + " = '" + txtSuche.Text + "'"
        End If
        fillGrid(0, "", strSearch)
    End Sub
    Protected Sub lbHinzufuegen_Click(sender As Object, e As EventArgs) Handles lbHinzufuegen.Click
        Dim cmdCommand As SqlClient.SqlCommand
        Dim cn As SqlClient.SqlConnection

        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try


            cn.Open()
            cmdCommand = New SqlClient.SqlCommand("INSERT INTO KBS_EFA (IP,WERKS ,LGORT,Firma,Kunnr,CustomerID, Master)" & _
                    "Values" & _
                    "( '" & txtNeuKasseIP.Text & "'," & _
                                   "'XXXX'," & _
                                    "'XXXX'," & _
                                    "'XXXX'," & _
                                    "'XXXX'," & _
                                    "'XXXX'," & _
                                    "0)", cn)
            cmdCommand.CommandType = CommandType.Text
            cmdCommand.ExecuteNonQuery()
            lblMessage.Text = "Kasse - " & txtNeuKasseIP.Text & " - erfolgreich hinzugefügt"
            getDataSource()
            Dim strSearch As String
            strSearch = ddlSearch.SelectedValue + " = '" + txtNeuKasseIP.Text + "'"
            fillGrid(0, "", strSearch)

        Catch ex As Exception
            lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message
        Finally
            cn.Close()
        End Try
    End Sub
End Class