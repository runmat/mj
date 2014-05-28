
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class jve_LogMessage2
    Inherits System.Web.UI.Page
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As Global.CKG.PortalZLD.GridNavigation

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        lblHead.Text = "Nachrichten"
        AdminAuth(Me, m_User, AdminLevel.Organization)
        GridNavigation1.setGridElment(dgSearchResult)

        Try
            m_App = New App(m_User)

            If Not IsPostBack Then
                lblError.Text = ""

                go()
                'TblSearch.Visible = False
                'TblLog.Visible = True
                'btnSave.Visible = False
                'btnDelete.Visible = False
                'btnNew.Visible = True
                'btnCancel.Visible = False
                'btnDelete2.Visible = False
            End If
        Catch ex As Exception
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

        ' txtServerzeit.Text = System.DateTime.Now.ToString

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

        Catch ex As Exception
            lblError.Text = "Fehler beim Lesen der Datenbank."
            Exit Sub
        End Try

        If (anzahl = 0) Then
            lblError.Text = "Keine Nachrichten vorhanden."

        Else
            FillDataGrid(False)
        End If
    End Sub
    Private Sub FillDataGrid(ByVal blnForceNew As Boolean, Optional ByVal intPageIndex As Int32 = 0, Optional ByVal strSort As String = "")
        Dim table As DataTable
        Dim sql As String

        Result.Visible = False
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
            m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "FillDataGrid", ex.ToString)

            lblError.Text = "Fehler beim Lesen der Datenbank."
            Exit Sub
        End Try

        Dim text As String
        Dim row As DataRow

        If (table.Rows.Count > 0) Then
            For Each row In table.Rows
                text = CType(row.Item("titleText"), String)
                Dim betreff() As String
                betreff = text.Split("}")
                betreff = betreff(1).Split("{")
                row.Item("titleText") = betreff(0)
            Next
            table.AcceptChanges()

            Result.Visible = True
            If strSort.Length > 0 Then
                If CStr(ViewState("mySort")) = strSort Then
                    strSort &= " DESC"
                End If
            Else
                strSort = CStr(ViewState("mySort"))
            End If

            ViewState("mySort") = strSort

            table.DefaultView.Sort = strSort
            With dgSearchResult
                .PageIndex = intPageIndex
                .DataSource = table
                .DataBind()
                .Visible = True
            End With

        End If
    End Sub
    Private Sub clearForm()
        txtDatumVon.Text = ""
        txtDatumBis.Text = ""
        txtZeitVon.Text = ""
        txtZeitBis.Text = ""
        txtBetreff.Text = ""
        txtMessage.Text = ""
        ddlTime.Items(0).Selected = True
        'ddlKunde.Items(0).Selected = True
        cbxActive.Checked = False
        cbxActiveOld.Checked = False
        cbxAll.Checked = False
        cbxAlterTime.Checked = False
        cbxAny.Checked = False
        cbxLogin.Checked = False
        cbxProd.Checked = False
        cbxTest.Checked = False
    End Sub
    Protected Sub dgSearchResult_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand
        Dim id As Integer

        If e.CommandName = "Select" Then
            id = CType(e.CommandArgument, Integer)
            edit(id)
            Input.Visible = True
            Result.Visible = False
            ddlTime.Visible = True
            lbtnSave.Visible = True
            lbtnDelete.Visible = True
            lbtnCancel.Visible = True
            lbtn_New.Visible = False
            lbtnDelete2.Visible = False
        End If
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
            txtDatumVon.Text = CType(table.Rows(0).Item("activeDateFrom"), String)
            txtDatumBis.Text = CType(table.Rows(0).Item("activeDateTo"), String)
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
            m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "edit", ex.ToString)
            lblError.Text = "Fehler beim Lesen der Datenbank."
            Exit Sub
        Finally
            conn.Close()
            conn.Dispose()
            da.Dispose()
        End Try
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
            m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "edit", ex.ToString)

            lblError.Text = "Fehler beim Lesen der Datenbank."
            Exit Sub
        End Try

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
            .AddWithValue("@activeDateFrom", txtDatumVon.Text)
            .AddWithValue("@activeDateTo", txtDatumBis.Text)
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
            Result.Visible = True
            Input.Visible = False
            lbtnSave.Visible = False
            lbtnDelete.Visible = False
            lbtnDelete2.Visible = False
            lbtnCancel.Visible = False
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "saveInsert", ex.ToString)

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
            .AddWithValue("@activeDateFrom", txtDatumVon.Text)
            .AddWithValue("@activeDateTo", txtDatumBis.Text)
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
            Result.Visible = True
            Input.Visible = False
            lbtnSave.Visible = False
            lbtnDelete.Visible = False
            lbtnDelete2.Visible = False
            lbtnCancel.Visible = False
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "saveUpdate", ex.ToString)

            lblError.Text = "Fehler beim Schreiben in die Datenbank!"
        End Try
    End Sub
    Private Function checkData(ByVal sqltype As Boolean) As Boolean
        Dim command As SqlClient.SqlCommand

        'Prüfen, ob Datumsformat
        If Not IsDate(txtDatumVon.Text) Then
            lblError.Text = "Falsches Datumsformat."
            Return False
        End If

        If Not IsDate(txtDatumBis.Text) Then
            lblError.Text = "Falsches Datumsformat."
            Return False
        End If

        'Prüfen, ob DatumVon <= DatumBis
        If Not (CType(txtDatumVon.Text, Date) <= CType(txtDatumBis.Text, Date)) Then
            lblError.Text = "Anfangsdatum muß kleiner oder gleich Enddatum sein."
            Return False
        End If

        'Prüfen, ob DatumBis >=Heute
        If Not (CType(txtDatumBis.Text, Date) >= CType(Format(Now, "d"), Date)) Then
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
            If (CType(txtDatumVon.Text, Date) = CType(txtDatumBis.Text, Date)) Then
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
    Protected Sub lbtnSave_Click(sender As Object, e As EventArgs) Handles lbtnSave.Click
        If (txtid.Text = "") Then
            saveInsert()
        Else
            saveUpdate()
        End If
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

    Protected Sub lbtn_New_Click(sender As Object, e As EventArgs) Handles lbtn_New.Click
        clearForm()
        Input.Visible = True
        Result.Visible = False
        ddlTime.Visible = True
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        lbtnDelete2.Visible = False
        lbtnCancel.Visible = True
        lbtn_New.Visible = False
    End Sub

    Protected Sub lbtnCancel_Click(sender As Object, e As EventArgs) Handles lbtnCancel.Click
        Input.Visible = False
        Result.Visible = True
        ddlTime.Visible = False
        lbtnSave.Visible = False
        lbtnDelete.Visible = False
        lbtnDelete2.Visible = False
        lbtnCancel.Visible = False
        lbtn_New.Visible = True
        disableInput(False)
    End Sub

    Protected Sub lbtnDelete2_Click(sender As Object, e As EventArgs) Handles lbtnDelete2.Click
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
            Input.Visible = False
            Result.Visible = True
            ddlTime.Visible = False
            lbtnSave.Visible = False
            lbtnDelete.Visible = False
            lbtnDelete2.Visible = False
            lbtnCancel.Visible = False
            lbtn_New.Visible = True
            disableInput(False)

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "jve_LogMessage2", "btnDelete2_Click", ex.ToString)

            lblError.Text = "Fehler beim Schreiben der Datenbank."
            Exit Sub
        End Try
    End Sub
    Private Sub disableInput(ByVal b As Boolean)
        txtDatumVon.Enabled = Not b
        txtDatumBis.Enabled = Not b
        txtZeitVon.Enabled = Not b
        txtZeitBis.Enabled = Not b
        txtBetreff.Enabled = Not b
        txtMessage.Enabled = Not b
        ddlTime.Enabled = Not b
        cbxActive.Enabled = Not b
        cbxLogin.Enabled = Not b
    End Sub

    Protected Sub lbtnDelete_Click(sender As Object, e As EventArgs) Handles lbtnDelete.Click
        lbtnDelete.Visible = False
        lbtnDelete2.Visible = True
        lbtnCancel.Visible = True
        lbtnSave.Visible = False
        disableInput(True)
        lblError.Text = "Löschen Sie die Nachricht durch erneutes Klicken auf 'Löschen'."
    End Sub
End Class