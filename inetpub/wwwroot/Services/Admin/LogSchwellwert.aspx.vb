Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services.PageElements


Partial Public Class LogSchwellwert
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation

    Private m_blnShowDetails() As Boolean
    Private mSeite As String
    Private mDatum As Date

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        AdminAuth(Me, m_User, AdminLevel.Organization)

        GetAppIDFromQueryString(Me)

        Try
            m_App = New App(m_User)

            Dim Datum As Date

            lblError.Text = ""

            If Not IsPostBack Then
                txtAbDatum.Text = Today.ToString("dd.MM.yyyy")
                txtBisDatum.Text = Today.ToString("dd.MM.yyyy")
            End If

            ReDim m_blnShowDetails(DataGrid1.PageSize)
            Dim i As Int32
            For i = 0 To DataGrid1.PageSize - 1
                m_blnShowDetails(i) = False
            Next

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "LogSchwellwert", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
        End Try
        GridNavigation1.setGridElment(grvLogSchwellwert)
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click

        DataGrid1.Visible = False
        HGZ.Visible = False

        If Not IsDate(txtAbDatum.Text) Then
            If Not IsStandardDate(txtAbDatum.Text) Then
                If Not IsSAPDate(txtAbDatum.Text) Then
                    lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                End If
            End If
        End If
        If Not IsDate(txtBisDatum.Text) Then
            If Not IsStandardDate(txtBisDatum.Text) Then
                If Not IsSAPDate(txtBisDatum.Text) Then
                    lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                End If
            End If
        End If

        'Wenn der Datentyp nicht stimmt, sollte hier ausgestiegen werden
        If lblError.Text <> String.Empty Then Exit Sub


        Dim datAb As Date = CDate(txtAbDatum.Text)
        Dim datBis As Date = CDate(txtBisDatum.Text)
        If datAb > datBis Then
            lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
        End If
        If lblError.Text.Length > 0 Then
            Exit Sub
        End If

        FillGridSchwellwerte(0, "")

    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As DataTable = Session("LogSchwellwerteResultExcel")

        Try

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub grvLogSchwellwert_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grvLogSchwellwert.PageIndexChanging
        FillGridSchwellwerte(e.NewPageIndex, "")
    End Sub

    Private Sub grvLogSchwellwert_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grvLogSchwellwert.RowCommand


        Dim index As Integer = Convert.ToInt32(e.CommandArgument)

        If e.CommandName = "Detail" Then
            Me.ViewState("mySort") = "[ASPX_ID]"

            Dim row As GridViewRow = grvLogSchwellwert.Rows(index)
            mSeite = row.Cells(0).Text
            mDatum = CDate(row.Cells(1).Text)
            FillDataGrid2(mSeite, 0)

            HGZ.Visible = True
        End If

    End Sub

    Private Sub HGZ_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HGZ.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "BAPI"
                e.TemplateFilename = "Templates\\BAPIData.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub

    Private Sub HGZ_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles HGZ.SortCommand
        FillDataGrid2(mSeite, HGZ.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub HGZ_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles HGZ.PageIndexChanged
        FillDataGrid2(mSeite, e.NewPageIndex)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Me.HGZ.Visible Then
            Dim item As DataGridItem
            Dim cell As TableCell
            Dim control As Control
            Dim image As System.Web.UI.WebControls.Image
            For Each item In HGZ.Items
                For Each cell In item.Cells
                    For Each control In cell.Controls
                        If TypeOf control Is System.Web.UI.WebControls.Image Then
                            image = CType(control, System.Web.UI.WebControls.Image)
                            image.Width = 16
                            image.Height = 16
                            If InStr(image.ImageUrl, "plus.gif") > 0 Then
                                image.ImageUrl = "/Services/Images/plus.gif"
                            End If
                            If InStr(image.ImageUrl, "minus.gif") > 0 Then
                                image.ImageUrl = "/Services/Images/minus.gif"
                            End If
                            cell.Width = 10
                        End If
                    Next
                Next
            Next
        End If
        GridNavigation1.setGridElment(grvLogSchwellwert)
        SetEndASPXAccess(Me)

    End Sub


    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    '----------------------------------------------------------------------
    ' Methode:      FillGridSchwellwerte
    ' Autor:        SFa
    ' Beschreibung: Ruft Daten aus der Tabelle LogSchwellwert ab und
    '               zeigt diese im Grid an
    ' Erstellt am:  03.03.2009
    ' ITA:          2633
    '----------------------------------------------------------------------

    Private Sub FillGridSchwellwerte(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not IsDate(Me.txtAbDatum.Text) Then
            Me.lblError.Text = "Bitte Datum/Startdatum übergeben."
            Exit Sub
        ElseIf Not IsDate(Me.txtBisDatum.Text) Then
            Me.lblError.Text = "Bitte Datum/Enddatum übergeben."
            Exit Sub
        Else
            Dim datTemp As Date
            datTemp = DateAdd(DateInterval.Day, 1, CDate(Me.txtBisDatum.Text))

            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT * " & _
                                                    "FROM LogSchwellwert " & _
                                                    "WHERE " & _
                                                    "     Datum BETWEEN @von AND @bis ", cn)
            With da.SelectCommand.Parameters
                .AddWithValue("@von", CDate(Me.txtAbDatum.Text))
                .AddWithValue("@bis", datTemp)
            End With

            Dim SchwellwerteTable As New DataTable

            da.Fill(SchwellwerteTable)

            If SchwellwerteTable.Rows.Count > 0 Then
                Session("LogSchwellwerteResultExcel") = SchwellwerteTable
                grvLogSchwellwert.PageIndex = intPageIndex
                grvLogSchwellwert.DataSource = SchwellwerteTable
                grvLogSchwellwert.DataBind()
                Result.Visible = True
            Else
                lblError.Text = "Keine Daten für diese Selektion."
            End If
            GridNavigation1.setGridElment(grvLogSchwellwert)
        End If
    End Sub

    Private Sub FillDataGrid2(ByVal Seite As String, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim dsLogData As New DataSet()
        Dim dt As New DataTable()
        Dim blnSortNew As Boolean = False

        lblError.Text = ""

        If strSort.Length > 0 Then
            strSort = "[" & strSort & "]"
            blnSortNew = True
            If CStr(Me.ViewState("mySort")) = strSort Then
                strSort &= " DESC"
            End If
        Else
            strSort = CStr(Me.ViewState("mySort"))
        End If
        Me.ViewState("mySort") = strSort

        Dim datTemp As Date
        datTemp = DateAdd(DateInterval.Day, 1, CDate(mDatum))

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        'Ermittele Daten für die Darstellung

        Dim da As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT " & _
                                                "     ASPX_ID, " & _
                                                "     Seite, " & _
                                                "     Anwendung, " & _
                                                "     [Start ASPX], " & _
                                                "     [Ende ASPX], " & _
                                                "     [Dauer ASPX], " & _
                                                "     Benutzer, " & _
                                                "     Kunnr, " & _
                                                "     Customername, " & _
                                                "     AccountingArea, " & _
                                                "     Testbenutzer, " & _
                                                "     [Dauer SAP], " & _
                                                "     [Zugriffe SAP] " & _
                                                "FROM dbo.vwASPXBAPISchwellwert_SUM " & _
                                                "WHERE " & _
                                                "     (Seite = @Seite) " & _
                                                "     AND ([Start ASPX] BETWEEN @von AND @bis) " & _
                                                "ORDER BY " & strSort, _
                                                cn)
        With da.SelectCommand.Parameters
            .AddWithValue("@Seite", Seite)
            .AddWithValue("@von", mDatum)
            .AddWithValue("@bis", datTemp)
        End With

        dt = New DataTable("ASPX")


        da.Fill(dt)

        Dim tmpRow As DataRow

        For Each tmpRow In dt.Rows
            If TypeOf tmpRow("Dauer ASPX") Is System.DBNull Then
                tmpRow("Dauer ASPX") = 0
            End If
            If TypeOf tmpRow("Dauer SAP") Is System.DBNull Then
                tmpRow("Dauer SAP") = 0
            End If
        Next
        dsLogData.Tables.Add(dt)

        Dim dt2 As New DataTable("BAPI")
        da = New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT " & _
                                                "     SAP_ID, " & _
                                                "     ASPX_ID, " & _
                                                "     BAPI, " & _
                                                "     Beschreibung, " & _
                                                "     [Start SAP], " & _
                                                "     [Ende SAP], " & _
                                                "     [Dauer SAP], " & _
                                                "     Erfolg, " & _
                                                "     Fehlermeldung " & _
                                                "FROM dbo.vwASPXBAPI_DETAIL " & _
                                                "WHERE " & _
                                                "      ([Start SAP] BETWEEN @von AND @bis) ", _
                                                cn)
        With da.SelectCommand.Parameters
            .AddWithValue("@von", mDatum)
            .AddWithValue("@bis", datTemp)
        End With
        da.Fill(dt2)
        dsLogData.Tables.Add(dt2)

        Dim dc1 As DataColumn
        Dim dc2 As DataColumn
        'Relation ASPX => BAPI
        dc1 = dsLogData.Tables("ASPX").Columns("ASPX_ID")
        dc2 = dsLogData.Tables("BAPI").Columns("ASPX_ID")
        Dim dr As DataRelation = New DataRelation("ASPX_BAPI", dc1, dc2, False)
        dsLogData.Relations.Add(dr)

        With Me.HGZ
            If dsLogData.Tables(0).Rows.Count > 0 Then
                .CurrentPageIndex = intPageIndex
                .DataSource = dsLogData
                .DataMember = "ASPX"

                .DataBind()
                .Visible = True
            Else
                lblError.Text = "Es wurden keine Einträge im SAP-Monitoring gefunden."
            End If

        End With

        'GridNavigation1.setGridElment(grvLogSchwellwert)
        divDetails.Visible = True
    End Sub

#End Region

End Class