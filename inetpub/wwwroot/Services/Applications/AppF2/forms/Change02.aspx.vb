Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports System.Drawing

Partial Public Class Change02
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

    Dim NewDatagrid As GridView
    Dim NewLiteral As Literal
    Dim m_change As AbweichAbrufgrund
    Dim strEQUINR As String
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        GridNavigation1.setGridElment(GridView1)
        m_App = New App(m_User)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Page.IsPostBack = False Then

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            m_change = New AbweichAbrufgrund(m_User, m_App, CStr(Session("AppID")), Me.Session.SessionID, strFileName)
            Session.Add("objChange", m_change)

        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal strFilter As String = "")

        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), AbweichAbrufgrund)
        End If


        If m_change.Result Is Nothing OrElse m_change.Result.Rows.Count = 0 Then
            GridView1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
        Else
            GridView1.Visible = True
            lblNoData.Visible = False
            Result.Visible = True

            Dim tmpDataView As New DataView()
            tmpDataView = m_change.Result.DefaultView
            If strFilter.Length > 0 Then
                tmpDataView.RowFilter = strFilter
                ViewState("Filter") = strFilter
            ElseIf Not ViewState("Filter") Is Nothing Then
                strFilter = ViewState("Filter")
                tmpDataView.RowFilter = strFilter
            End If

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
                If strTempSort.Length = 0 Then
                    strTempSort = "Haendlernummer"
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

            GridView1.PageIndex = intTempPageIndex

            GridView1.DataSource = tmpDataView
            GridView1.DataBind()


            'Hinzufügen der verlinkung zur fahrzeughistorie JJU2008.08.20 ITA 2188
            '----------------------------------------------------
            For Each item As GridViewRow In GridView1.Rows
                If Not item.FindControl("lnkHistorie") Is Nothing Then
                    CType(item.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "Report02.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report02'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkHistorie"), HyperLink).Text & "&cw=true"
                End If
            Next
            '----------------------------------------------------

        End If

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        ModalPopupExtender2.Show()
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub


#End Region

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand


        'sobald ein ItemCommand gefeuert wird, muss die aktuelle EQUNR für die Detailsansicht "herausgeholt werden" und für das festellen des richtigen Datensatzes
        strEQUINR = e.CommandArgument

        If e.CommandName = "Erledigt" Then

            If m_change Is Nothing Then
                m_change = CType(Session("objChange"), AbweichAbrufgrund)
            End If


            Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)



            logApp.CollectDetails("Hdl.-Nr.", CType(m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Haendlernummer"), Object), True)
            logApp.CollectDetails("Fahrgestellnummer", CType(m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Fahrgestellnummer"), Object))
            logApp.CollectDetails("Vertragsnummer", CType(m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Vertragsnummer"), Object))
            logApp.CollectDetails("Kommentar", CType("Erledigt", Object))

            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

            'dann erledigt Zeile schreiben
            m_change.Equipment = strEQUINR
            m_change.Memo = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Memo")
            m_change.Erledigt = "X"
            m_change.Dataus = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Datum Ausgang")
            m_change.Change(Me)

            If m_change.Status = 0 Then
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Abweichender Versandgrund", logApp.InputDetails)
            Else
                logApp.UpdateEntry("ERR", Session("AppID").ToString, "Abweichender Versandgrund: Speichern fehlgeschlagen", logApp.InputDetails)

            End If

            m_change.Show(Me)
            ' lblError.Text = m_change.Message
            GridView1.EditIndex = -1
            Me.FillGrid(GridView1.PageIndex)

        End If

        If e.CommandName = "Edit" Then
            Dim tblDetails As DataTable
            Dim blnDetailsExist As Boolean = False

            Dim strMemo As String = ""
            If m_change Is Nothing Then
                m_change = CType(Session("objChange"), AbweichAbrufgrund)
            End If


            If strEQUINR Is String.Empty Or strEQUINR Is Nothing Then
                'lblError.Text = "FEHLER: die Details Ansicht bekam keine EQUINR"
                Exit Sub
            End If

            strMemo = m_change.Result.Select("EQUNR = '" & strEQUINR & "'")(0)("Memo").ToString

            m_change.fillDetailsTable(strEQUINR)


            tblDetails = m_change.DetailsTable


            If Not tblDetails Is Nothing Then
                If Not tblDetails.Rows.Count = 0 Then
                    blnDetailsExist = True
                End If
            End If


            If blnDetailsExist Then
                txtMemo1.Text = strMemo
                lblEQUNR.Text = strEQUINR
                GridView2.DataSource = tblDetails
                GridView2.DataBind()
                ModalPopupExtender2.Show()
            End If

            strEQUINR = String.Empty
        End If

        If e.CommandName = "Filter" Then
            Dim strFilter As String = ""
            Dim LinkBut As LinkButton
            LinkBut = CType(e.CommandSource, LinkButton)
            strFilter = e.CommandArgument.ToString & "=" & LinkBut.Text
            Me.FillGrid(GridView1.PageIndex, strFilter:=strFilter)
        End If
    End Sub

    Private Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        Try
            Dim Fahrgestellnummer As String = ""

            If m_change Is Nothing Then
                m_change = CType(Session("objChange"), AbweichAbrufgrund)
            End If
            Dim lblEqui As Label
            lblEqui = CType(e.Row.Cells(12).FindControl("lblEQUNR"), Label)
            Fahrgestellnummer = m_change.Result.Select("EQUNR='" & lblEqui.Text & "'")(0).Item("Fahrgestellnummer")
            Dim myDeleteButton As ImageButton
            myDeleteButton = CType(e.Row.Cells(12).FindControl("lbErledigt"), ImageButton)
            myDeleteButton.Attributes.Add("onclick", "return confirm('Dokument mit Fahrgestellnummer \n" & Fahrgestellnummer & "\n als erledigt kennzeichnen?');")

        Catch
            'lblError.Text = "Vorsicht: Bestätigungsabfragen konnten nicht generiert werden"
        End Try
    End Sub

    Private Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing
        GridView1.EditIndex = e.NewEditIndex
        Me.FillGrid(GridView1.PageIndex)

    End Sub

    Private Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating


        If Not GridView1.EditIndex = -1 Then

            If m_change Is Nothing Then
                m_change = CType(Session("objChange"), AbweichAbrufgrund)
            End If

            Dim lblMemo As Label = CType(GridView1.Rows(GridView1.EditIndex).Cells(8).FindControl("lblMemo"), Label)


            If lblMemo.Text.Length > 120 Then
                Dim script As String
                script = "<" & "script language='javascript'>" & _
                          "alert('Es sind Maximal 120 Zeichen erlaubt, Ihr Text enthält " & lblMemo.Text.Length & " Zeichen' );" & _
                      "</" & "script>"
                Response.Write(script)
                Exit Sub
            End If
            m_change.Memo = lblMemo.Text
            m_change.Equipment = strEQUINR

            m_change.Erledigt = ""
            m_change.Dataus = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Datum Ausgang").ToString
            m_change.Change(Me)


            Dim lbl As Label
            Dim lbt As LinkButton
            Dim hyp As HyperLink

            lbl = CType(GridView1.Rows.Item(GridView1.EditIndex).FindControl("lblVertragsnummer"), Label)

            lbt = CType(GridView1.Rows.Item(GridView1.EditIndex).FindControl("lbtnHaendlernummer"), LinkButton)

            hyp = CType(GridView1.Rows.Item(GridView1.EditIndex).FindControl("lnkHistorie"), HyperLink)

            Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)


            logApp.CollectDetails("Hdl.-Nr.", CType(lbt.Text, Object), True)
            logApp.CollectDetails("Fahrgestellnummer", CType(hyp.Text, Object))
            logApp.CollectDetails("Vertragsnummer", CType(lbl.Text, Object))
            logApp.CollectDetails("Kommentar", CType(m_change.Memo, Object))

            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


            If m_change.Status = 0 Then
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Abweichender Versandgrund", logApp.InputDetails)
            Else
                logApp.UpdateEntry("ERR", Session("AppID").ToString, "Abweichender Versandgrund: Speichern fehlgeschlagen", logApp.InputDetails)

            End If

            m_change.Show(Me)

        End If

        GridView1.EditIndex = -1

        Me.FillGrid(GridView1.PageIndex)

    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConfirm.Click
        If Not Session("objChange") Is Nothing Then
            m_change = CType(Session("objChange"), AbweichAbrufgrund)
            If rb_Abrufgrund.Checked = True Then
                m_change.SuchKennzeichen = "AAG"
            ElseIf rb_COC.Checked = True Then
                m_change.SuchKennzeichen = "COC"
            ElseIf rb_ZBI.Checked = True Then
                m_change.SuchKennzeichen = "ZB1"
            End If

            m_change.Show(Me)
            ViewState("Filter") = Nothing
            FillGrid(0)
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click


        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), AbweichAbrufgrund)
        End If

        strEQUINR = lblEQUNR.Text
        m_change.Memo = txtMemo1.Text
        m_change.Dataus = m_change.Result.Select("EQUNR='" & strEQUINR & "'")(0).Item("Datum Ausgang")
        m_change.Equipment = strEQUINR
        m_change.Erledigt = ""
        m_change.Change(Me)
        m_change.Show(Me)
        Session("objChange") = m_change

        Me.FillGrid(GridView1.PageIndex)

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub
End Class
