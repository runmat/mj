Option Strict Off
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements




Partial Public Class EFA_Change01

    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private dtStatus As DataTable
    Private objCustomer As DataTable
    Protected WithEvents ddlFirma As DropDownList
    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)


        Try

            m_App = New App(m_User)
            lblError.Text = ""

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
            lblError.Visible = True
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
                If dtStatus.Rows(tmpGVRow.DataItemIndex).Item("IP").ToString = e.CommandArgument.ToString Then
                    tmpgvRowindex = tmpGVRow.RowIndex
                    Exit For
                End If
            Next

            If e.CommandName = "saveIP" Then
                Dim text As String
                Dim tmpTXT As TextBox
                tmpTXT = CType(gv.Rows(tmpgvRowindex).FindControl("txtIP"), TextBox)
                text = tmpTXT.Text


                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try


                    cn.Open()
                    cmdCommand = New SqlClient.SqlCommand("Update KBS_EFA Set IP='" & text & "' where IP='" & e.CommandArgument.ToString() & "'")
                    cmdCommand.Connection = cn
                    cmdCommand.CommandType = CommandType.Text
                    cmdCommand.ExecuteNonQuery()
                    lblMessage.Text = "Kasse - " & KassenIP & " - erfolgreich aktuallisiert"


                Catch ex As Exception
                    lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message

                Finally
                    cn.Close()
                End Try

            ElseIf e.CommandName = "saveLGORT" Then
                Dim text As String
                Dim tmpTXT As TextBox
                tmpTXT = CType(gv.Rows(tmpgvRowindex).FindControl("txtLGORT"), TextBox)
                text = tmpTXT.Text


                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try


                    cn.Open()
                    cmdCommand = New SqlClient.SqlCommand("Update KBS_EFA Set LGORT='" & text & "' where IP='" & e.CommandArgument.ToString & "'")
                    cmdCommand.Connection = cn
                    cmdCommand.CommandType = CommandType.Text
                    cmdCommand.ExecuteNonQuery()
                    lblMessage.Text = "Kasse - " & KassenIP & " - erfolgreich aktuallisiert"


                Catch ex As Exception
                    lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message

                Finally
                    cn.Close()
                End Try


            ElseIf e.CommandName = "saveWERKS" Then
                Dim text As String
                Dim tmpTXT As TextBox
                tmpTXT = CType(gv.Rows(tmpgvRowindex).FindControl("txtWERKS"), TextBox)
                text = tmpTXT.Text


                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try


                    cn.Open()
                    cmdCommand = New SqlClient.SqlCommand("Update KBS_EFA Set WERKS='" & text & "' where IP='" & e.CommandArgument.ToString & "'")
                    cmdCommand.Connection = cn
                    cmdCommand.CommandType = CommandType.Text
                    cmdCommand.ExecuteNonQuery()
                    lblMessage.Text = "Kasse - " & KassenIP & " - erfolgreich aktuallisiert"


                Catch ex As Exception
                    lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message

                Finally
                    cn.Close()
                End Try

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
            fillGrid(0)
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


    Private Sub Grid1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv.RowCreated

    End Sub

    Private Sub gv_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv.Sorting
        fillGrid(0, e.SortExpression)
    End Sub

    Protected Sub lbHinzufuegen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbHinzufuegen.Click
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
            fillGrid(0)

        Catch ex As Exception
            lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message
        Finally
            cn.Close()
        End Try

    End Sub

    Protected Sub lbZurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbZurueck.Click
        Response.Redirect("../../../Start/Selection.aspx")
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

    Protected Sub ddlFirma_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim KassenIP As String
        Dim ddlTemp As DropDownList = CType(sender, DropDownList)
        Dim cmdCommand As SqlClient.SqlCommand
        Dim cn As SqlClient.SqlConnection

        'Dim id As String = ddlTemp.SelectedValue.Split(":")(0)

        KassenIP = ddlTemp.SelectedValue.Split(":"c)(2)

        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try
            cn.Open()
            cmdCommand = New SqlClient.SqlCommand("Update KBS_EFA Set Firma='" & ddlTemp.SelectedItem.Text & _
                                                  "', CustomerID='" & ddlTemp.SelectedValue.Split(":"c)(0) & _
                                                  "', Kunnr='" & ddlTemp.SelectedValue.Split(":"c)(1) & _
                                                  "' where IP='" & ddlTemp.SelectedValue.Split(":"c)(2) & "'")
            cmdCommand.Connection = cn
            cmdCommand.CommandType = CommandType.Text
            cmdCommand.ExecuteNonQuery()
            lblMessage.Text = "Kasse - " & KassenIP & " - erfolgreich aktuallisiert"


        Catch ex As Exception
            lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message
        Finally
            cn.Close()
        End Try
        getDataSource()
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        Dim strSearch As String

        strSearch = ddlSearch.SelectedValue + " = '" + txtSuche.Text + "'"

        fillGrid(0, "", strSearch)
    End Sub

    Protected Sub cmdnoSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdnoSearch.Click
        fillGrid(0)
    End Sub

    Protected Sub chkMaster_CheckedChanged(sender As Object, e As EventArgs)
        Dim checkbox As CheckBox = CType(sender, CheckBox)
        Dim row As GridViewRow = CType(checkbox.NamingContainer, GridViewRow)
        Dim Box As TextBox = CType(row.FindControl("txtIP"), TextBox)


        Dim KassenIP As String = Box.Text
        Dim cmdCommand As SqlClient.SqlCommand
        Dim cn As SqlClient.SqlConnection

        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try
            cn.Open()
            cmdCommand = New SqlClient.SqlCommand("Update KBS_EFA Set Master=" & IIf(checkbox.Checked, 1, 0) & _
                                                  " where IP='" & KassenIP & "'")
            cmdCommand.Connection = cn
            cmdCommand.CommandType = CommandType.Text
            cmdCommand.ExecuteNonQuery()
            lblMessage.Text = "Kasse - " & KassenIP & " - erfolgreich aktuallisiert"


        Catch ex As Exception
            lblError.Text = "es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message
        Finally
            cn.Close()
        End Try
        getDataSource()

    End Sub

    Protected Sub gv_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv.PageIndexChanging
        fillGrid(e.NewPageIndex, "", "")
    End Sub
End Class

' ************************************************
' $History: EFA_Change01.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 28.03.11   Time: 14:00
' Updated in $/CKAG/Applications/AppKBS/forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 1.11.10    Time: 17:11
' Updated in $/CKAG/Applications/AppKBS/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 7.04.10    Time: 17:39
' Updated in $/CKAG/Applications/AppKBS/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 12.02.10   Time: 16:25
' Updated in $/CKAG/Applications/AppKBS/forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 12.05.09   Time: 16:12
' Updated in $/CKAG/Applications/AppKBS/forms
' ITa 2808
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 12.05.09   Time: 16:04
' Updated in $/CKAG/Applications/AppKBS/forms
' ITA 2808
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.05.09   Time: 15:59
' Created in $/CKAG/Applications/AppKBS/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 6.05.09    Time: 16:56
' Created in $/CKAG/Applications/AppKroschke/Forms
' EFA KassenAdministration ITA 2808
' 
' ************************************************
