Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements



Partial Public Class StatusMonitor



    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Protected WithEvents lblMessage As Label
    Protected WithEvents lblError As Label
    Public WithEvents ddlPlatzierung As DropDownList
    Private dtStatus As DataTable
    Private tblServiceCenter As DataTable
    Private tblSC_Abteilungen As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = "StatusMonitor - Administration"
        AdminAuth(Me, m_User, AdminLevel.Master)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""
                SetRadioButton()
                Filldropdown()
                getDataSource()
                fillGrid()
            Else
                If dtStatus Is Nothing Then
                    dtStatus = Session("StatusMonitorDT")
                End If
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName,  "StatusMonitor - Administration", "Page_Load", ex.ToString)
            lblError.Text = ex.Message
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


            cmdAg = New SqlClient.SqlCommand("SELECT  dbo.Statusmonitor.ID, dbo.SC_Abteilungen.Abteilung, dbo.Statusmonitor.Platzierung, dbo.Statusmonitor.Logo, dbo.Statusmonitor.Status," & _
            " dbo.Statusmonitor.Benutzerreferenz, dbo.Statusmonitor.Infotext " & _
                       " FROM         dbo.SC_Abteilungen INNER JOIN " & _
                      "dbo.Relation_SC_Abteilung ON dbo.SC_Abteilungen.ID = dbo.Relation_SC_Abteilung.AbteilungID INNER JOIN " & _
                      "dbo.ServiceCenter ON dbo.Relation_SC_Abteilung.ServiceCenterID = dbo.ServiceCenter.ServiceCenterID INNER JOIN " & _
                     "dbo.Statusmonitor ON dbo.ServiceCenter.ServiceCenterID = dbo.Statusmonitor.ServiceCenterID AND dbo.SC_Abteilungen.ID = dbo.Statusmonitor.ABT_ID " & _
                     "Where dbo.ServiceCenter.ServiceCenterID=" & rbServiceCenter.SelectedValue, cn)

            cmdAg.CommandType = CommandType.Text
            ad.SelectCommand = cmdAg
            ad.Fill(ds, "StatusTabelle")

            If ds.Tables("StatusTabelle") Is Nothing Then
                gv.Visible = False
                Throw New Exception("Es wurden keine Datensätze gefunden")
            End If
            dtStatus = ds.Tables("StatusTabelle")
            Session.Add("StatusMonitorDT", dtStatus)
            If dtStatus.Rows.Count=0 Then
                gv.Visible = False
                Throw New Exception("Es wurden keine Datensätze gefunden")
            End If
            gv.Visible = True
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
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
            Dim rdvitem As New ListItem

            For xServiceCenter As Integer = 0 To tblServiceCenter.Rows.Count - 1
                rdvitem = New ListItem
                rdvitem.Text = tblServiceCenter.Rows(xServiceCenter)("ServiceCenter").ToString
                rdvitem.Value = tblServiceCenter.Rows(xServiceCenter)("ServiceCenterID").ToString
                rbServiceCenter.Items.Add(rdvitem)
            Next
            rbServiceCenter.Items(0).Selected = True
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True

        Finally
            cn.Close()
        End Try




    End Sub
   

    Private Sub Filldropdown()

        Dim cn As SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim ds As DataSet
        Dim ad As SqlClient.SqlDataAdapter
        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try
            Dim tmpTbl As New DataTable
            cn.Open()

            ds = New DataSet()

            ad = New SqlClient.SqlDataAdapter()

            ' Sind schon Zuweisungen gemacht wurden?
            cmdAg = New SqlClient.SqlCommand("SELECT     dbo.SC_Abteilungen.ID, dbo.SC_Abteilungen.Abteilung " & _
                                             "FROM       dbo.Relation_SC_Abteilung INNER JOIN " & _
                                             "  dbo.SC_Abteilungen ON dbo.Relation_SC_Abteilung.AbteilungID = dbo.SC_Abteilungen.ID " & _
                                             "WHERE     dbo.Relation_SC_Abteilung.ServiceCenterID = " & rbServiceCenter.SelectedValue & _
                                             " ORDER BY dbo.SC_Abteilungen.Abteilung ", cn)
            cmdAg.CommandType = CommandType.Text
            ad.SelectCommand = cmdAg

            ad.Fill(ds, "AbteilungsTabelle")

            If ds.Tables("AbteilungsTabelle") Is Nothing Then
                gv.Visible = False
                Throw New Exception("Es wurden keine Datensätze gefunden")
            End If
            tmpTbl = ds.Tables("AbteilungsTabelle")

            ' wenn nicht alle SC-Abteilungen laden

            ds = New DataSet()

            ad = New SqlClient.SqlDataAdapter()

            cmdAg = New SqlClient.SqlCommand("SELECT dbo.SC_Abteilungen.ID, dbo.SC_Abteilungen.Abteilung " & _
                                             "FROM dbo.Relation_SC_Abteilung RIGHT OUTER JOIN " & _
                                             "dbo.SC_Abteilungen ON dbo.Relation_SC_Abteilung.AbteilungID = dbo.SC_Abteilungen.ID " & _
                                             "WHERE (dbo.Relation_SC_Abteilung.ServiceCenterID IS NULL) OR " & _
                                             "(dbo.Relation_SC_Abteilung.ServiceCenterID <> " & rbServiceCenter.SelectedValue & ") " & _
                                             " GROUP BY dbo.SC_Abteilungen.ID, dbo.SC_Abteilungen.Abteilung " & _
                                             " ORDER BY dbo.SC_Abteilungen.Abteilung", cn)

            cmdAg.CommandType = CommandType.Text
            ad.SelectCommand = cmdAg

            ad.Fill(ds, "AbteilungsTabelle")

            If ds.Tables("AbteilungsTabelle") Is Nothing Then
                gv.Visible = False
                Throw New Exception("Es wurden keine Datensätze gefunden")
            End If
            tblSC_Abteilungen = ds.Tables("AbteilungsTabelle")




            For iCount As Integer = tblSC_Abteilungen.Rows.Count - 1 To 0 Step -1
                For Each Delrow As DataRow In tmpTbl.Rows
                    If tblSC_Abteilungen.Rows(iCount).Item(0) = Delrow("ID") Then
                        tblSC_Abteilungen.Rows.RemoveAt(iCount)
                    End If
                Next
            Next


            Session.Add("AbteilungsTabelle", tblSC_Abteilungen)

         

            For xServiceCenter As Integer = 0 To tblSC_Abteilungen.Rows.Count - 1
                Dim rdvitem As New ListItem
                rdvitem.Text = tblSC_Abteilungen.Rows(xServiceCenter)("Abteilung").ToString
                rdvitem.Value = tblSC_Abteilungen.Rows(xServiceCenter)("ID").ToString
                ddlAbteilung.Items.Add(rdvitem)
            Next
            ddlAbteilung.Items(0).Selected = True
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
                    cmdCommand = New SqlClient.SqlCommand("Update StatusMonitor Set Logo='" & text & "' where ID=" & e.CommandArgument)
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
                    cmdCommand = New SqlClient.SqlCommand("Update StatusMonitor Set Benutzerreferenz='" & text & "' where ID=" & e.CommandArgument)
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

    Protected Sub lbHinzufuegen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbHinzufuegen.Click
        Dim cmdCommand As SqlClient.SqlCommand
        Dim cn As SqlClient.SqlConnection

        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try
            cn.Open()
            Dim AbteilungID As Integer
            If rbAuswahl.Checked = True Then

                cmdCommand = New SqlClient.SqlCommand("INSERT INTO dbo.Relation_SC_Abteilung (ServiceCenterID,AbteilungID)" & _
                        "Values" & _
                        "( " & rbServiceCenter.SelectedValue & ", " & ddlAbteilung.SelectedValue & ");")

                AbteilungID = ddlAbteilung.SelectedValue

                cmdCommand.Connection = cn
                cmdCommand.CommandType = CommandType.Text
                cmdCommand.ExecuteNonQuery()
            ElseIf rbNeu.Checked Then
                cmdCommand = New SqlClient.SqlCommand("INSERT INTO dbo.SC_Abteilungen (Abteilung, Status)" & _
                        "Values" & _
                        "( '" & txtKundenName.Text.Trim & "' , 0 );" & _
                           "SELECT SCOPE_IDENTITY()")
                cmdCommand.Connection = cn
                cmdCommand.CommandType = CommandType.Text

                AbteilungID = cmdCommand.ExecuteScalar()

                cmdCommand = New SqlClient.SqlCommand("INSERT INTO dbo.Relation_SC_Abteilung (ServiceCenterID,AbteilungID)" & _
                        "Values" & _
                        "( " & rbServiceCenter.SelectedValue & ", " & AbteilungID & ");")

                cmdCommand.Connection = cn
                cmdCommand.CommandType = CommandType.Text
                cmdCommand.ExecuteNonQuery()
            End If

            cmdCommand = New SqlClient.SqlCommand("INSERT INTO StatusMonitor (ABT_ID,Platzierung,Logo,Status,Benutzerreferenz,InfoText,ServiceCenterID)" & _
                    "Values" & _
                    "( " & AbteilungID & "," & _
                                   "'R'," & _
                                    "''," & _
                                    "'0'," & _
                                    "'999'," & _
                                    "''," & rbServiceCenter.SelectedValue & ");")


            cmdCommand.Connection = cn
            cmdCommand.CommandType = CommandType.Text
            cmdCommand.ExecuteNonQuery()
            lblMessage.Text = "Kunde - " & txtKundenName.Text & " - erfolgreich hinzugefügt"
            ddlAbteilung.Items.Clear()
            Filldropdown()
            getDataSource()
            fillGrid()
            txtKundenName.Text = ""
        Catch ex As Exception
            Throw New Exception("es ist ein Fehler beim updaten der Datenbank aufgetreten: " & ex.Message)

        Finally
            cn.Close()
        End Try

    End Sub

    Protected Sub lbZurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbZurueck.Click
        Response.Redirect("../Start/Selection.aspx")
    End Sub

    Protected Sub rbServiceCenter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbServiceCenter.SelectedIndexChanged
        lblError.Text = ""
        lblError.Visible = False
        ddlAbteilung.Items.Clear()
        Filldropdown()
        getDataSource()
        fillGrid()
    End Sub

    Protected Sub rbAuswahl_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbAuswahl.CheckedChanged

        txtKundenName.Enabled = Not rbAuswahl.Checked
        ddlAbteilung.Enabled = rbAuswahl.Checked
 
    End Sub

    Protected Sub rbNeu_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rbNeu.CheckedChanged
        txtKundenName.Enabled = rbNeu.Checked
        ddlAbteilung.Enabled = Not rbNeu.Checked
    End Sub
End Class