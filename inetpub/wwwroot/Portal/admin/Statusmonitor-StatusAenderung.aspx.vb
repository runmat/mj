
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Partial Public Class Statusmonitor_StatusAenderung



    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Public WithEvents ddlPlatzierung As DropDownList
    Private dtStatus As DataTable
    Private tblServiceCenter As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "Verwaltung Login - Nachricht"
        AdminAuth(Me, m_User, AdminLevel.None)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""
                SetRadioButton()
                getDataSource()
                fillGrid()
            Else
                If dtStatus Is Nothing Then
                    dtStatus = Session("StatusMonitorDT")
                End If
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try

    End Sub


    Private Sub fillGrid(Optional ByVal strSort As String = "")

        Dim tmpView As New DataView
        'nach neuladen aus der datenbank ist natürlich die sortierung wieder neu, daher view in session speichern 
        tmpView = dtStatus.DefaultView
        If Not ViewState("Direction") Is Nothing And Not ViewState("SortExpression") Is Nothing Then
            tmpView.Sort = ViewState("SortExpression") & " " & ViewState("Direction")
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
            tmpView.Sort = strSort & " " & strDirection
        End If
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
            If m_User.Reference = "999" And Not m_User.Reference.Trim(" "c) Is String.Empty Then ' Masteradmin
                cmdAg = New SqlClient.SqlCommand("SELECT * FROM vwStatusmonitorDAD Where ServiceCenterID = " & rbServiceCenter.SelectedValue, cn)
            ElseIf m_User.Reference = "99" And Not m_User.Reference.Trim(" "c) Is String.Empty Then ' Servicecenteradmin
                cmdAg = New SqlClient.SqlCommand("SELECT * FROM vwStatusmonitorDAD WHERE BENUTZERREFERENZ='" & m_User.Reference & _
                                                "' And ServiceCenterID = " & m_User.Organization.OrganizationReference, cn)
            Else ' Abteilungsadmin
                cmdAg = New SqlClient.SqlCommand("SELECT * FROM vwStatusmonitorDAD WHERE BENUTZERREFERENZ='" & m_User.Reference & _
                                               "' And ServiceCenterID = " & rbServiceCenter.SelectedValue, cn)
            End If

            cmdAg.CommandType = CommandType.Text
            ad.SelectCommand = cmdAg
            ad.Fill(ds, "StatusTabelle")

            If ds.Tables("StatusTabelle") Is Nothing Then
                Throw New Exception("Es wurden keine Datensätze gefunden")
            End If
            dtStatus = ds.Tables("StatusTabelle")

            Session.Add("StatusMonitorDT", dtStatus)


        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
        End Try

    End Sub

    Private Sub SetRadioButton()

        Dim cn As SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim ds As DataSet
        Dim ad As SqlClient.SqlDataAdapter
        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try

            cn.Open()

            ds = New DataSet()

            ad = New SqlClient.SqlDataAdapter()


            cmdAg = New SqlClient.SqlCommand("SELECT * FROM ServiceCenter ", cn)
            cmdAg.CommandType = CommandType.Text
            ad.SelectCommand = cmdAg
            ad.Fill(ds, "ServiceCenterTabelle")

            If ds.Tables("ServiceCenterTabelle") Is Nothing Then
                gv.Visible = False
                Throw New Exception("Es wurden keine Datensätze gefunden")
            End If
            tblServiceCenter = ds.Tables("ServiceCenterTabelle")

            Session.Add("ServiceCenterTabelle", tblServiceCenter)

            For xServiceCenter As Integer = 0 To tblServiceCenter.Rows.Count - 1
                Dim rdvitem As New ListItem
                rdvitem.Text = tblServiceCenter.Rows(xServiceCenter)("ServiceCenter").ToString
                rdvitem.Value = tblServiceCenter.Rows(xServiceCenter)("ServiceCenterID").ToString
                rbServiceCenter.Items.Add(rdvitem)
            Next
            If m_User.Reference = "999" Then
                rbServiceCenter.Items(0).Selected = True
            Else
                rbServiceCenter.Items(CInt(m_User.Organization.OrganizationReference) - 1).Selected = True
                lblServiceCenter.Text = rbServiceCenter.SelectedItem.Text
                lblServiceCenter.Visible = True
                rbServiceCenter.Visible = False
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True

        Finally
            cn.Close()
        End Try




    End Sub

    Private Sub gv_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv.RowCommand


        If Not e.CommandName = "sort" Then
            Dim kundenname As String
            Dim tmpGVRow As GridViewRow
            Dim tmpgvRowindex As Int32
            Dim cmdCommand As SqlClient.SqlCommand
            Dim cn As SqlClient.SqlConnection


            If dtStatus Is Nothing Then
                dtStatus = Session("StatusMonitorDT")
            End If


            For Each tmpGVRow In gv.Rows
                If dtStatus.Rows(tmpGVRow.DataItemIndex).Item("ABT_ID") = e.CommandArgument Then
                    tmpgvRowindex = tmpGVRow.RowIndex
                    Exit For
                End If
            Next


            kundenname = dtStatus.Select("ABT_ID='" & e.CommandArgument & "'")(0).Item("Abteilung")

            If e.CommandName = "change" Then
                Dim aenderung As Int32


                If dtStatus.Select("ABT_ID='" & e.CommandArgument & "'")(0).Item("StatusNeu") = 0 Then

                    aenderung = 1
                Else

                    aenderung = 0
                End If

                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try


                    cn.Open()
                    cmdCommand = New SqlClient.SqlCommand("Update SC_Abteilungen Set Status=" & aenderung & " where ID=" & e.CommandArgument)
                    cmdCommand.Connection = cn
                    cmdCommand.CommandType = CommandType.Text
                    cmdCommand.ExecuteNonQuery()
                    lblMessage.Text = "Kunde - " & kundenname & " - erfolgreich aktuallisiert"

                Catch ex As Exception
                    Throw New Exception("es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message)

                Finally
                    cn.Close()
                End Try

            ElseIf e.CommandName = "saveInfoText" Then
                Dim text As String
                Dim tmpTXT As TextBox
                tmpTXT = gv.Rows(tmpgvRowindex).FindControl("txtInfoText")
                text = tmpTXT.Text


                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try


                    cn.Open()
                    cmdCommand = New SqlClient.SqlCommand("Update StatusMonitor Set InfoText='" & text & "' where ABT_ID=" & e.CommandArgument)
                    cmdCommand.Connection = cn
                    cmdCommand.CommandType = CommandType.Text
                    cmdCommand.ExecuteNonQuery()
                    lblMessage.Text = "Kunde - " & kundenname & " - erfolgreich aktuallisiert"


                Catch ex As Exception
                    Throw New Exception("es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message)

                Finally
                    cn.Close()
                End Try

            End If
            getDataSource()
            fillGrid()
        End If
    End Sub




    Public Function PlatzierungChanged(ByVal sender As Object, ByVal value As System.EventArgs) As Boolean Handles ddlPlatzierung.SelectedIndexChanged
        Dim kundenname As String
        Dim ddlTemp As DropDownList = CType(sender, DropDownList)
        Dim cmdCommand As SqlClient.SqlCommand
        Dim cn As SqlClient.SqlConnection

        '  Dim id As String = ddlTemp.SelectedValue.Split(":")(0)

        kundenname = dtStatus.Select("ID=" & ddlTemp.SelectedValue.Split(":")(1) & "")(0).Item("Abteilung")

        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try
            cn.Open()
            cmdCommand = New SqlClient.SqlCommand("Update StatusMonitor Set Platzierung='" & ddlTemp.SelectedValue.Split(":")(0) & "' where ID=" & ddlTemp.SelectedValue.Split(":")(1))
            cmdCommand.Connection = cn
            cmdCommand.CommandType = CommandType.Text
            cmdCommand.ExecuteNonQuery()
            lblMessage.Text = "Kunde - " & kundenname & " - erfolgreich aktuallisiert"


        Catch ex As Exception
            Throw New Exception("es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message)
        Finally
            cn.Close()
        End Try
        getDataSource()
        Return True
    End Function
    Private Sub gv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv.RowDataBound

        Dim tmpddl As DropDownList
        tmpddl = e.Row.FindControl("ddlPlatzierung")

        If Not tmpddl Is Nothing Then

            Dim tmpItem As ListItem

            For Each tmpItem In tmpddl.Items
                tmpddl.SelectedValue = e.Row.DataItem.item("Platzierung")
                tmpItem.Value = tmpItem.Value & ":" & e.Row.DataItem.item("ID")
            Next

            If m_User.Reference = "999" And Not m_User.Reference.Trim(" "c) Is String.Empty Then
                tmpddl.Enabled = True
            ElseIf m_User.Reference = "99" And Not m_User.Reference.Trim(" "c) Is String.Empty Then
                tmpddl.Enabled = True
            Else
                tmpddl.Enabled = False
            End If

        End If

    End Sub


    Private Sub Grid1_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv.RowCreated
        If e.Row.RowType = ListItemType.Item Then
            Dim tmpDDL As DropDownList
            tmpDDL = CType(e.Row.FindControl("ddlPlatzierung"), DropDownList)
            If Not tmpDDL Is Nothing Then
                AddHandler tmpDDL.SelectedIndexChanged, AddressOf PlatzierungChanged
            End If
        End If
    End Sub

    Private Sub gv_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv.Sorting
        fillGrid(e.SortExpression)
    End Sub

    Protected Sub lbZurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbZurueck.Click
        Response.Redirect("../Start/Selection.aspx")
    End Sub

    Protected Sub rbServiceCenter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbServiceCenter.SelectedIndexChanged
        getDataSource()
        fillGrid()
    End Sub

End Class