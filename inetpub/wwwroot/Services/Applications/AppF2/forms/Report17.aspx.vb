Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder

Public Class Report17
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private Const ResultSessionKey = "MahnstopData"

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        Common.GetAppIDFromQueryString(Me)

        m_App = New App(m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        lblError.Text = ""
        ErrorGrid.InnerText = ""
        Success.InnerText = ""

        If Session("MahnTabelle") Is Nothing Then
            GetSAPTable()
        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(e As EventArgs)
        MyBase.OnUnload(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub SetError(ByVal text As String)

        lblError.Text += text

        If Not String.IsNullOrEmpty(lblError.Text.Trim()) Then
            lblError.Visible = True
        End If

    End Sub

    Protected Sub rgMahnstopNeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles rgMahnstop.NeedDataSource
        LoadData(False)
    End Sub

    Private Sub SaveChanges()
        Dim dt As DataTable = Session("MahnTabelle")

        For Each xItem In rgMahnstop.Items
            If xItem("CHANGED").Controls(1).Checked Then
                Dim rows As DataRow() = dt.Select("CHASSIS_NUM='" & xItem("CHASSIS_NUM").Text & "' AND MATNR='" & xItem("MATNR").Text & "'")
                Dim row As DataRow

                If rows.GetLength(0) = 0 Then
                    row = dt.NewRow()
                Else
                    row = rows(0)
                End If

                row.SetField("CHASSIS_NUM", xItem("CHASSIS_NUM").Text)
                row.SetField("MATNR", xItem("MATNR").Controls(3).Text) 'xItem("MATNR").Text

                Dim dat As New DateTime()
                If DateTime.TryParse(xItem("MAHNDATUM_AB").Controls(1).Text, dat) Then
                    row.SetField("MAHNDATUM_AB", dat)
                Else
                    row("MAHNDATUM_AB") = DBNull.Value
                End If

                row.SetField("BEM", xItem("BEM").Controls(1).Text)
                row.SetField("MAHNSP_SETZEN", "X")

                If rows.GetLength(0) = 0 Then
                    dt.Rows.Add(row)
                End If

            End If
        Next

        dt.AcceptChanges()

        Session("MahnTabelle") = dt

    End Sub

    Private Sub GetSAPTable()
        Dim proxy = DynSapProxy.getProxy("Z_DPM_SAVE_MAHN_EQSTL_01", m_App, m_User, Page)

        proxy.setImportParameter("I_AG", Right("0000000000" & m_User.Customer.KUNNR, 10))
        proxy.setImportParameter("I_USER", m_User.UserName)

        Dim table As DataTable = proxy.getImportTable("GT_IN")
        Session("MahnTabelle") = table
    End Sub

    Private Function SendChangesToSAP() As Boolean
        Try
            Dim proxy = DynSapProxy.getProxy("Z_DPM_SAVE_MAHN_EQSTL_01", m_App, m_User, Page)

            proxy.setImportParameter("I_AG", Right("0000000000" & m_User.Customer.KUNNR, 10))
            proxy.setImportParameter("I_USER", m_User.UserName)

            Dim table As DataTable = proxy.getImportTable("GT_IN")

            table.Merge(Session("MahnTabelle"))
            'Dim Mahntable As DataTable = Session("MahnTabelle")

            'For Each row As DataRow In Mahntable.Rows
            '    Dim SAPRow As DataRow = table.NewRow()

            '    SAPRow("CHASSIS_NUM") = row("CHASSIS_NUM")
            '    SAPRow("MATNR") = row("MATNR")
            '    SAPRow("MAHNDATUM_AB") = row("MAHNDATUM_AB")
            '    SAPRow("BEM") = row("BEM")
            '    SAPRow("MAHNSP_SETZEN") = row("MAHNSP_SETZEN")

            '    table.Rows.Add(SAPRow)
            'Next

            table.AcceptChanges()

            proxy.callBapi()
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Private Sub LoadData(Optional ByVal rebind As Boolean = True)


        If rebind OrElse Session("TempData") Is Nothing Then
            Try
                Dim proxy = DynSapProxy.getProxy("Z_DPM_READ_MAHN_EQSTL_02", m_App, m_User, Page)

                proxy.setImportParameter("I_AG", Right("0000000000" & m_User.Customer.KUNNR, 10))
                proxy.setImportParameter("I_PAID", txt_FIN.Text)
                proxy.setImportParameter("I_KONTONR", txt_Kontonr.Text)
                proxy.setImportParameter("I_STOPDAT_VON", txt_MahnstopVon.Text)
                proxy.setImportParameter("I_STOPDAT_BIS", txt_MahnstopBis.Text)

                proxy.callBapi()

                Dim dtresult As DataTable = proxy.getExportTable("GT_OUT")
                dtresult.Columns.Add("CHANGED", Type.GetType("System.Boolean")).DefaultValue = False
                For Each row In dtresult.Rows
                    row("CHANGED") = False
                Next
                dtresult.AcceptChanges()

                Session("TempData") = dtresult
            Catch ex As Exception

            End Try
        End If

        Dim table As DataTable = Session("TempData")

        If Not table Is Nothing Then
            rgMahnstop.DataSource = table.DefaultView

            divSelection.Visible = False
            PnResult.Visible = True
            ibtNewSearch.ImageUrl = "../../../Images/queryArrow.gif"
            lb_Execute.Visible = False
            btnSave.Visible = True
        Else
            rgMahnstop.DataSource = Nothing
            lblError.Text = "Es wurden keine Daten gefunden!"

            divSelection.Visible = True
            PnResult.Visible = False
            ibtNewSearch.ImageUrl = "../../../Images/queryArrowUp.gif"
            lb_Execute.Visible = True
            btnSave.Visible = False
        End If

        If rebind Then rgMahnstop.Rebind()
    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Session("TempData") = Nothing
        Session("MahnTabelle") = Nothing
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        SaveChanges()
        If SendChangesToSAP() Then
            Session("TempData") = Nothing
            Session("MahnTabelle") = Nothing
            LoadData()
            Success.InnerText = "Die Daten wurden gespeichert!"
        Else
            ErrorGrid.InnerText = "Fehler beim Speichern der Daten!"
        End If
    End Sub

    Private Sub rgMahnstop_PageIndexChanged(sender As Object, e As Telerik.Web.UI.GridPageChangedEventArgs) Handles rgMahnstop.PageIndexChanged
        SaveChanges()
    End Sub

    Private Sub rgMahnstop_PageSizeChanged(sender As Object, e As Telerik.Web.UI.GridPageSizeChangedEventArgs) Handles rgMahnstop.PageSizeChanged
        SaveChanges()
    End Sub

    Private Sub rgMahnstop_SortCommand(sender As Object, e As Telerik.Web.UI.GridSortCommandEventArgs) Handles rgMahnstop.SortCommand
        SaveChanges()
    End Sub

    Protected Sub ExecuteClick(sender As Object, e As EventArgs) Handles lb_Execute.Click
        Dim dateFrom As Date
        Dim dateTo As Date
        Dim von As String = txt_MahnstopVon.Text.Trim()
        Dim bis As String = txt_MahnstopBis.Text.Trim()


        lblError.Text = String.Empty

        If Not String.IsNullOrEmpty(von) And String.IsNullOrEmpty(bis) Then
            bis = von
        ElseIf String.IsNullOrEmpty(von) And Not String.IsNullOrEmpty(bis) Then
            SetError("Geben Sie 'Mahnstop von' an!")
            Return
        ElseIf String.IsNullOrEmpty(von) And String.IsNullOrEmpty(bis) Then
            von = Date.Now.ToShortDateString()
            bis = von
        End If

        If Not Date.TryParse(von, dateFrom) Then
            SetError("'Mahnstop von' ist ungültig!")
            Return
        End If

        If Not Date.TryParse(bis, dateTo) Then
            SetError("'Mahnstop bis' ist ungültig!")
            Return
        End If

        If dateFrom > dateTo Then
            SetError("'Mahnstop bis' ist kleiner 'Mahnstop von'.")
            Return
        End If

        Dim dateDiv As Integer = (dateTo - dateFrom).Days


        LoadData()
    End Sub

    Protected Sub ibtNewSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtNewSearch.Click
        divSelection.Visible = (Not divSelection.Visible)
        lb_Execute.Visible = divSelection.Visible

        If ibtNewSearch.ImageUrl = "../../../Images/queryArrow.gif" Then
            ibtNewSearch.ImageUrl = "../../../Images/queryArrowUp.gif"
        Else
            ibtNewSearch.ImageUrl = "../../../Images/queryArrow.gif"
        End If
    End Sub

End Class