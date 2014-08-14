Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel

Partial Public Class LogMessage
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.CKG.PortalZLD.GridNavigation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Master)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gridMain)

        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        Try
            If Not IsPostBack Then
                lblError.Text = ""

                go()
                TblSearch.Visible = False
                ButtonPanel.Visible = False
                Result.Visible = True
                btnNew.Visible = True
                btnDelete2.Visible = False
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogMessage", "Page_Load", ex.ToString)
            lblError.Text = ex.ToString
            lblError.Visible = True
        End Try

    End Sub
    Private Sub go()
        Dim anzahl As Integer

        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

        With ddlTime.Items
            .Add("00:00")
            .Add("01:00")
            .Add("02:00")
            .Add("03:00")
            .Add("04:00")
            .Add("05:00")
            .Add("06:00")
            .Add("07:00")
            .Add("08:00")
            .Add("09:00")
            .Add("10:00")
            .Add("11:00")
            .Add("12:00")
            .Add("13:00")
            .Add("14:00")
            .Add("15:00")
            .Add("16:00")
            .Add("17:00")
            .Add("18:00")
            .Add("19:00")
            .Add("20:00")
            .Add("21:00")
            .Add("22:00")
            .Add("23:00")
        End With

        txtServerzeit.Text = System.DateTime.Now.ToString

        Try
            '#NEU
            'Dim conn As New SqlClient.SqlConnection()
            Dim command As New SqlClient.SqlCommand("select count(id) from LoginMessage", conn)
            conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            conn.Open()
            anzahl = CType(command.ExecuteScalar(), Integer)
            conn.Close()
            conn.Dispose()
            command.Dispose()
            '#ALT
            'anzahl = CType(DBManager.Execute.Scalar("select count(id) from LoginMessage"), Integer)
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogMessage", "go", ex.ToString)
            lblError.Text = "Fehler beim Lesen der Datenbank."
            Exit Sub
        End Try

        If (anzahl = 0) Then
            lblMessage.Text = "Keine Nachrichten vorhanden."

        Else
            FillDataGrid(False)
        End If
    End Sub

    Private Sub FillDataGrid(ByVal blnForceNew As Boolean, Optional ByVal intPageIndex As Int32 = 0, Optional ByVal strSort As String = "")
        Dim table As DataTable
        Dim sql As String

        gridMain.Visible = False
        'Sortierung nach aktiv und aktuelles Datum/Zeit im gespeicherten Zeitfenster!
        sql = "SELECT *" & _
                       " FROM LoginMessage" & _
                       " ORDER BY active DESC, activeDateTo DESC, activeTimeTo DESC "
        Try
            Dim conn As New SqlClient.SqlConnection()
            Dim da As New SqlClient.SqlDataAdapter(sql, conn)
            table = New DataTable()
            conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            conn.Open()
            da.Fill(table)
            conn.Close()
            conn.Dispose()
            da.Dispose()
            table.Columns.Remove("messageText")
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogMessage", "FillDataGrid", ex.ToString)

            lblError.Text = "Fehler beim Lesen der Datenbank."
            Exit Sub
        End Try

        Dim text As String
        Dim row As DataRow

        If (table.Rows.Count > 0) Then
            For Each row In table.Rows
                text = CType(row.Item("titleText"), String)
                text = text.Replace("{", "<")
                text = text.Replace("}", ">")
                row.Item("titleText") = text
            Next
            table.AcceptChanges()

            'lblInfo.Text = table.Rows.Count & " Nachricht(en) gefunden."
            gridMain.Visible = True
            If strSort.Length > 0 Then
                If CStr(ViewState("mySort")) = strSort Then
                    strSort &= " DESC"
                End If
            Else
                strSort = CStr(ViewState("mySort"))
            End If

            ViewState("mySort") = strSort

            table.DefaultView.Sort = strSort
            With gridMain
                .PageIndex = intPageIndex
                .DataSource = table
                .DataBind()
                .Visible = True
            End With

        End If
    End Sub

    Private Sub clearForm()
        txtDateVon.Text = ""
        txtDateBis.Text = ""
        txtZeitVon.Text = ""
        txtZeitBis.Text = ""
        txtBetreff.Text = ""
        txtMessage.Text = ""
        txtid.Text = ""
        ddlTime.Items(0).Selected = True
        'ddlKunde.Items(0).Selected = True
        cbxActive.Checked = False
        cbxActiveOld.Checked = False
        cbxAll.Checked = False
        cbxAlter.Checked = False
        cbxAlterTime.Checked = False
        cbxAny.Checked = False
        cbxLogin.Checked = False
        cbxProd.Checked = False
        cbxTest.Checked = False
    End Sub

    Private Sub saveInsert()
        Dim sql As String
        Dim command As New SqlClient.SqlCommand()
        Dim customer As Integer

        If (checkData(True) = False) Then
            'Prüfen, ob Eingabe korrekt
            Exit Sub
        End If

        If CType(ddlKunde.SelectedItem.Value, String) = String.Empty Then 'Gewählter Kunde
            customer = 0
        Else
            customer = CType(ddlKunde.SelectedItem.Value, Integer)
        End If
        sql = "INSERT INTO LoginMessage (activeDateFrom, " & _
                                            "activeDateTo,activeTimeFrom,activeTimeTo,messageText, messageColor," & _
                                            "titleText,active,enableLogin,onlyTEST,onlyPROD) " & _
                                            "VALUES (@activeDateFrom,@activeDateTo,@activeTimeFrom,@activeTimeTo," & _
                                            "@messageText,@alleSeiten,@titleText,@active,@enableLogin,@onlyTest,@onlyProd)"

        command.CommandText = sql

        With command.Parameters
            .AddWithValue("@activeDateFrom", txtDateBis.Text)
            .AddWithValue("@activeDateTo", txtDateBis.Text)
            .AddWithValue("@activeTimeFrom", txtZeitVon.Text)
            .AddWithValue("@activeTimeTo", txtZeitBis.Text)
            .AddWithValue("@messageText", txtMessage.Text)
            .AddWithValue("@alleSeiten", cbxAny.Checked)
            .AddWithValue("@titleText", txtBetreff.Text)
            .AddWithValue("@active", cbxActive.Checked)
            .AddWithValue("@enableLogin", customer)
            .AddWithValue("@onlyTest", cbxTest.Checked)
            .AddWithValue("@onlyProd", cbxProd.Checked)
        End With
        Try
            Dim conn As New SqlClient.SqlConnection()
            conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            command.Connection = conn
            conn.Open()
            command.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
            command.Dispose()

            FillDataGrid(False, 0)
            clearForm()

            lblError.Text = ""
            TblSearch.Visible = False
            ButtonPanel.Visible = False
            Result.Visible = True
            ddlTime.Visible = False
            btnNew.Visible = True
            btnDelete2.Visible = False
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogMessage", "saveInsert", ex.ToString)

            lblError.Text = "Fehler beim Schreiben in die Datenbank!"
        End Try
    End Sub

    Private Sub saveUpdate()
        Dim sql As String
        Dim command As New SqlClient.SqlCommand()
        Dim customer As Integer

        If (checkData(False) = False) Then
            'Prüfen, ob Eingaben Korrekt.
            Exit Sub
        End If

        If CType(ddlKunde.SelectedItem.Value, String) = String.Empty Then 'Gewählter Kunde
            customer = 0
        Else
            customer = CType(ddlKunde.SelectedItem.Value, Integer)
        End If

        sql = "UPDATE LoginMessage SET " & _
                                    "creationDate = @currentDate," & _
                                    "activeDateFrom = @activeDateFrom," & _
                                    "activeDateTo = @activeDateTo," & _
                                    "activeTimeFrom = @activeTimeFrom," & _
                                    "activeTimeTo = @activeTimeTo," & _
                                    "messageText = @messageText, " & _
                                    "messageColor = @alleSeiten," & _
                                    "titleText = @titleText," & _
                                    "active = @active," & _
                                    "enableLogin = @customer," & _
                                    "onlyTEST = @onlyTest, " & _
                                    "onlyPROD = @onlyProd " & _
                                    "WHERE id = @id"

        command.CommandText = sql

        With command.Parameters
            .AddWithValue("@currentDate", Now)
            .AddWithValue("@activeDateFrom", txtDateVon.Text)
            .AddWithValue("@activeDateTo", txtDateBis.Text)
            .AddWithValue("@activeTimeFrom", txtZeitVon.Text)
            .AddWithValue("@activeTimeTo", txtZeitBis.Text)
            .AddWithValue("@messageText", txtMessage.Text)
            .AddWithValue("@alleSeiten", cbxAny.Checked)
            .AddWithValue("@titleText", txtBetreff.Text)
            .AddWithValue("@active", cbxActive.Checked)
            .AddWithValue("@customer", customer)
            .AddWithValue("@onlyTest", cbxTest.Checked)
            .AddWithValue("@onlyProd", cbxProd.Checked)
            .AddWithValue("@id", txtid.Text)
        End With
        Try

            Dim conn As New SqlClient.SqlConnection()
            conn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            command.Connection = conn
            conn.Open()
            command.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
            command.Dispose()

            FillDataGrid(False, 0)
            lblError.Text = ""
            TblSearch.Visible = False
            ButtonPanel.Visible = False
            Result.Visible = True
            btnNew.Visible = True
            ddlTime.Visible = False
            btnDelete2.Visible = False
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogMessage", "saveUpdate", ex.ToString)

            lblError.Text = "Fehler beim Schreiben in die Datenbank!"
        End Try
    End Sub

    Private Function checkData(ByVal sqltype As Boolean) As Boolean
        Dim command As SqlClient.SqlCommand

        'Prüfen, ob Datumsformat
        If Not IsDate(txtDateVon.Text) Then
            lblError.Text = "Falsches Datumsformat."
            Return False
        End If

        If Not IsDate(txtDateBis.Text) Then
            lblError.Text = "Falsches Datumsformat."
            Return False
        End If

        'Prüfen, ob DatumVon <= DatumBis
        If Not (CType(txtDateVon.Text, Date) <= CType(txtDateBis.Text, Date)) Then
            lblError.Text = "Anfangsdatum muß kleiner oder gleich Enddatum sein."
            Return False
        End If

        'Prüfen, ob DatumBis >=Heute
        If Not (CType(txtDateBis.Text, Date) >= CType(Format(Now, "d"), Date)) Then
            lblError.Text = "Mindestens das Enddatum muß größer oder gleich Heute sein."
            Return False
        End If

        'Prüfen, ob Zeiformat
        If Not IsDate(Format(txtZeitVon.Text, "Short Time")) Then
            lblError.Text = "Falsches Zeitformat."
            Return False
        End If
        If Not IsDate(Format(txtZeitBis.Text, "Short Time")) Then
            lblError.Text = "Falsches Zeitformat."
            Return False
        End If

        'Prüfen, ob ZeitVon < ZeitBis 
        If Not (CType(txtZeitVon.Text, Date) < (CType(txtZeitBis.Text, Date))) Then
            If (CType(txtDateVon.Text, Date) = CType(txtDateBis.Text, Date)) Then
                lblError.Text = "Die Endzeit muß größer sein als die Startzeit."
                Return False
            End If
        End If

        'Prüfen, ob Nachricht > 500 Zeichen
        If (txtMessage.Text.Length > 500) Then
            lblError.Text = "Die Länge der Nachricht übertrifft 500 Zeichen."
            Return False
        End If

        'Prüfen, ob mehr als 1 aktive Nachricht pro Kunde, die bei jedem Seitenwechsel angezeigt wird
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim anz As Integer

        If Not CType(ddlKunde.SelectedItem.Value, String) = String.Empty Then
            command = New SqlClient.SqlCommand("SELECT Count(id) FROM LoginMessage WHERE messageColor <> 0 and enableLogin = " & CType(ddlKunde.SelectedItem.Value, Integer), conn)
            Try
                conn.Open()
                anz = CType(command.ExecuteScalar(), Integer)
            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "checkData", ex.ToString)
                lblError.Text = "Fehler beim Lesen aus der Datenbank!"
                conn.Dispose()
                Return False
            Finally
                conn.Close()
            End Try

            If (anz > 0) And (sqltype = True) Then 'INSERT
                lblError.Text = "Es darf nicht mehr als 1 Fenter-Nachricht pro Kunde angelegt werden."
                Return False
            End If
        End If
        'Prüfen, ob mehr als 5 aktive Login - Nachrichten
        Dim gleich5 As Integer
        command = New SqlClient.SqlCommand("SELECT Count(id) FROM LoginMessage WHERE active <> 0 AND messageColor = 0", conn)
        Try
            conn.Open()
            gleich5 = (CType(command.ExecuteScalar(), Integer))
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "checkData", ex.ToString)
            lblError.Text = "Fehler beim Lesen aus der Datenbank!"
            conn.Dispose()
            Return False
        Finally
            conn.Close()
        End Try

        If (sqltype = True) Then    'INSERT
            If (gleich5 > 4) And (cbxActive.Checked = True) Then
                lblError.Text = "Es dürfen nicht mehr als 5 Login-Nachrichten aktiv sein."
                Return False
            End If
        Else
            If (((cbxActiveOld.Checked = False) And (cbxActive.Checked = True) And (cbxAny.Checked = False) And (gleich5 > 4))) Then 'UPDATE
                lblError.Text = "Es dürfen nicht mehr als 5 Login-Nachrichten aktiv sein."
                Return False
            End If
            If (((cbxActiveOld.Checked = True) And (cbxActive.Checked = True) And (cbxAny.Checked = False) And (gleich5 > 4))) Then 'UPDATE
                lblError.Text = "Es dürfen nicht mehr als 5 Login-Nachrichten aktiv sein."
                Return False
            End If
        End If
        conn.Dispose()
        Return True 'Sonst ok
    End Function

    Private Sub disableInput(ByVal b As Boolean)
        txtDateVon.Enabled = Not b
        txtDateBis.Enabled = Not b
        txtZeitVon.Enabled = Not b
        txtZeitBis.Enabled = Not b
        txtBetreff.Enabled = Not b
        txtMessage.Enabled = Not b
        ddlTime.Enabled = Not b
        cbxActive.Enabled = Not b
        cbxLogin.Enabled = Not b
    End Sub


    Private Sub fillCustomer()
        Dim sqltable As New DataTable()
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim comm As New SqlClient.SqlCommand()

        Try
            comm.CommandText = "select customerid, customername from customer"
            comm.Connection = conn
            conn.Open()
            'Kundenliste füllen
            Dim da As New SqlClient.SqlDataAdapter(comm)
            da.Fill(sqltable)
            da.Dispose()
            conn.Close()

            Dim r As Data.DataRow
            r = sqltable.NewRow()
            r("CustomerID") = System.DBNull.Value
            r("Customername") = "(ALLE)"
            sqltable.Rows.Add(r)

            Dim dv As DataView = sqltable.DefaultView
            dv.Sort = "Customername"

            With ddlKunde
                .DataSource = dv
                .DataTextField = "Customername"
                .DataValueField = "CustomerID"
                .DataBind()
            End With
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogMessage", "edit", ex.ToString)

            lblError.Text = "Fehler beim Lesen der Datenbank."
            Exit Sub
        End Try

    End Sub

    Private Sub edit(ByVal idMessage As Integer)
        Dim table As DataTable
        Dim sqltable As New DataTable()
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim comm As New SqlClient.SqlCommand()
        Dim da As New SqlClient.SqlDataAdapter(comm)

        fillCustomer()

        Try
            da = New SqlClient.SqlDataAdapter("SELECT * FROM LoginMessage WHERE id = " & idMessage, conn)
            conn.Open()
            table = New DataTable()
            da.Fill(table)

            lblError.Text = ""
            txtid.Text = CType(table.Rows(0).Item("id"), String)
            txtDateVon.Text = CType(table.Rows(0).Item("activeDateFrom"), String)
            txtDateBis.Text = CType(table.Rows(0).Item("activeDateTo"), String)
            txtZeitVon.Text = Format(CType(table.Rows(0).Item("activeTimeFrom"), String), "Short Time")
            txtZeitBis.Text = Format(CType(table.Rows(0).Item("activeTimeTo"), String), "Short Time")
            txtBetreff.Text = CType(table.Rows(0).Item("titleText"), String)
            txtMessage.Text = CType(table.Rows(0).Item("messageText"), String)
            cbxActive.Checked = CType(table.Rows(0).Item("active"), Boolean)
            cbxActiveOld.Checked = CType(table.Rows(0).Item("active"), Boolean)
            cbxLogin.Checked = CType(table.Rows(0).Item("enableLogin"), Boolean)
            cbxTest.Checked = CType(table.Rows(0).Item("onlyTest"), Boolean)
            cbxProd.Checked = CType(table.Rows(0).Item("onlyProd"), Boolean)
            cbxAny.Checked = CType(table.Rows(0).Item("messageColor"), Boolean)
            ddlKunde.ClearSelection()
            Dim liItem As ListItem
            Dim sQuery As String
            If CType(table.Rows(0).Item("enableLogin"), String) <> 0 Then
                'ddlKunde.Items.FindByValue(CType(table.Rows(0).Item("id"), String))
                sQuery = CType(table.Rows(0).Item("enableLogin"), String)
                liItem = ddlKunde.Items.FindByValue(sQuery.Trim)
                liItem.Selected = True
            Else
                ddlKunde.Items(0).Selected = True
            End If

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogMessage", "edit", ex.ToString)
            lblError.Text = "Fehler beim Lesen der Datenbank."
            Exit Sub
        Finally
            conn.Close()
            conn.Dispose()
            da.Dispose()
        End Try
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        gridMain.PageIndex = PageIndex
        FillDataGrid(False)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid(False)
    End Sub

    Private Sub gridMain_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridMain.RowCommand
        Dim id As Integer
        Dim index As Integer
        Dim row As GridViewRow
        Dim lblID As Label

        If e.CommandName = "Select" Then
            index = Convert.ToInt32(e.CommandArgument)
            row = gridMain.Rows(index)
            lblID = CType(row.Cells(0).FindControl("lblID"), Label)
            id = CInt(lblID.Text)
            edit(id)
            TblSearch.Visible = True
            ButtonPanel.Visible = True
            Result.Visible = False
            btnNew.Visible = False
            ddlTime.Visible = True
            btnDelete2.Visible = False
        End If
    End Sub

    Private Sub gridMain_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridMain.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then


            Dim addButton As ImageButton = CType(e.Row.Cells(11).Controls(0).FindControl("btnSelect"), ImageButton)
            addButton.CommandArgument = e.Row.RowIndex.ToString()

        End If

    End Sub


    Private Sub gridMain_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gridMain.Sorting
        FillDataGrid(False, 0, e.SortExpression)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        clearForm()
        lblError.Text = ""
        TblSearch.Visible = True
        ButtonPanel.Visible = True
        Result.Visible = False
        ddlTime.Visible = True
        btnSave.Visible = True
        btnNew.Visible = False
        btnDelete2.Visible = False
        disableInput(False)
        fillCustomer()
    End Sub

    Private Sub btnDelete2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete2.Click
        Dim command As New SqlClient.SqlCommand()

        command.CommandText = "DELETE FROM LoginMessage WHERE id = @id"
        command.Parameters.AddWithValue("@id", txtid.Text)
        Try
            '#NEU
            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            command.Connection = conn
            conn.Open()
            command.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
            '#ALT
            'DBManager.Execute.NonQuery(command)
            FillDataGrid(False, 0)

            lblError.Text = ""
            TblSearch.Visible = False
            ButtonPanel.Visible = False
            Result.Visible = True
            btnDelete2.Visible = False
            btnNew.Visible = True
            disableInput(False)
            btnDelete.Visible = True
            btnDelete2.Visible = False
            btnCancel.Visible = True
            btnSave.Visible = True

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogMessage", "btnDelete2_Click", ex.ToString)

            lblError.Text = "Fehler beim Schreiben der Datenbank."
            Exit Sub
        End Try
    End Sub

    Private Sub ddlTime_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTime.SelectedIndexChanged
        If (cbxAlterTime.Checked) Then
            txtZeitBis.Text = ddlTime.Items(ddlTime.SelectedIndex).ToString
            cbxAlterTime.Checked = False
        Else
            txtZeitVon.Text = ddlTime.Items(ddlTime.SelectedIndex).ToString
            cbxAlterTime.Checked = True
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        TblSearch.Visible = False
        ButtonPanel.Visible = False
        btnDelete2.Visible = False
        Result.Visible = True
        btnNew.Visible = True
        lblError.Text = ""
        disableInput(False)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        txtServerzeit.Text = System.DateTime.Now.ToString
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (txtid.Text = "") Then
            saveInsert()
        Else
            saveUpdate()
        End If
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        btnDelete.Visible = False
        btnDelete2.Visible = True
        btnCancel.Visible = True
        btnSave.Visible = False
        disableInput(True)
        lblError.Text = "Löschen Sie die Nachricht durch erneutes Klicken auf 'Löschen'."
    End Sub
End Class