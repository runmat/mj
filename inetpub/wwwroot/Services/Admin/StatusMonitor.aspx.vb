Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services.PageElements

Partial Public Class StatusMonitor


    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App

    Public WithEvents ddlPlatzierung As DropDownList
    Private dtStatus As DataTable

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Master)

        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
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


            kundenname = dtStatus.Select("ID='" & e.CommandArgument & "'")(0).Item("Abteilung")

            For Each tmpGVRow In gv.Rows
                If dtStatus.Rows(tmpGVRow.DataItemIndex).Item("ID") = e.CommandArgument Then
                    tmpgvRowindex = tmpGVRow.RowIndex
                    Exit For
                End If
            Next

            If e.CommandName = "save" Then
                Dim text As String
                Dim tmpTXT As TextBox
                tmpTXT = gv.Rows(tmpgvRowindex).FindControl("txtLogoPfad")
                text = tmpTXT.Text


                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try


                    cn.Open()
                    cmdCommand = New SqlClient.SqlCommand("Update StatusMonitorDAD Set Logo='" & text & "' where ID=" & e.CommandArgument)
                    cmdCommand.Connection = cn
                    cmdCommand.CommandType = CommandType.Text
                    cmdCommand.ExecuteNonQuery()
                    lblMessage.Text = "Kunde - " & kundenname & " - erfolgreich aktuallisiert"


                Catch ex As Exception
                    Throw New Exception("es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message)

                Finally
                    cn.Close()
                End Try

            ElseIf e.CommandName = "saveReferenz" Then
                Dim text As String
                Dim tmpTXT As TextBox
                tmpTXT = gv.Rows(tmpgvRowindex).FindControl("txtBerechtigungReferenz")
                text = tmpTXT.Text


                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try


                    cn.Open()
                    cmdCommand = New SqlClient.SqlCommand("Update StatusMonitorDAD Set Benutzerreferenz='" & text & "' where ID=" & e.CommandArgument)
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
            cmdCommand = New SqlClient.SqlCommand("Update StatusMonitorDAD Set Platzierung='" & ddlTemp.SelectedValue.Split(":")(0) & "' where ID=" & ddlTemp.SelectedValue.Split(":")(1))
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

    Protected Sub lbHinzufuegen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbhinzufuegen.Click
        Dim cmdCommand As SqlClient.SqlCommand
        Dim cn As SqlClient.SqlConnection

        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try


            cn.Open()
            cmdCommand = New SqlClient.SqlCommand("INSERT INTO StatusMonitorDAD (Abteilung,Platzierung,Logo,Status,Benutzerreferenz,InfoText)" & _
                    "Values" & _
                    "( '" & txtKundenName.Text & "'," & _
                                   "'R'," & _
                                    "''," & _
                                    "'0'," & _
                                    "'999'," & _
                                    "'');")


            cmdCommand.Connection = cn
            cmdCommand.CommandType = CommandType.Text
            cmdCommand.ExecuteNonQuery()
            lblMessage.Text = "Kunde - " & txtKundenName.Text & " - erfolgreich hinzugefügt"
            getDataSource()
            fillGrid()

        Catch ex As Exception
            Throw New Exception("es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message)

        Finally
            cn.Close()
        End Try

    End Sub
#End Region

#Region "Methods"
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


            cmdAg = New SqlClient.SqlCommand("SELECT * FROM StatusmonitorDAD ", cn)
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
#End Region
    'Protected Sub lbZurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbback.Click
    '    Response.Redirect("../Start/Selection.aspx")
    'End Sub
End Class