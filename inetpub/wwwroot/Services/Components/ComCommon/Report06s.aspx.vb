Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Partial Public Class Report06s
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvBestand)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)


            lblError.Text = ""

            If IsPostBack = False Then

                FillDropdowns()
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Try
            Dim TempAusgabeTable As DataTable = CType(Session("UserDataTable"), DataTable).Clone
            Dim AusgabeTable As DataTable = CType(Session("UserDataTable"), DataTable).Copy
            Dim col2 As DataColumn
            Dim sColName As String = ""

            'Zugeordnete Anwendungen
            Dim Checked As Boolean = True
            Dim TempTable As New DataTable
            Dim GetData As New UserGroupApp
            Dim Row As DataRow

            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Select Case Checked
                Case rdbAnwendung.Checked

                    TempTable.Columns.Add("Anwendungen", System.Type.GetType("System.String"))

                    Row = TempTable.NewRow

                    Row("Anwendungen") = ddlAnwendung.SelectedItem.Text

                    TempTable.Rows.Add(Row)

                    TempTable.AcceptChanges()

                Case rdbGruppe.Checked
                    TempTable = GetData.GetApplicationsPerGroup(CInt(ddlGruppe.SelectedValue), cn)
                    TempTable.Columns.Remove("AppID")
                    TempTable.Columns("AppFriendlyName").ColumnName = "Anwendungen"
                    TempTable.AcceptChanges()

                Case rdbAlle.Checked
                    TempTable = GetData.GetApplications(m_User.Customer.CustomerId, cn)

                    TempTable.Columns.Remove("AppID")
                    TempTable.Columns("AppFriendlyName").ColumnName = "Anwendungen"
                    TempTable.AcceptChanges()

            End Select


            Dim FieldControl As DataControlField
            Dim Found As Boolean = False

            AusgabeTable.Columns.Add("Temp", System.Type.GetType("System.String"))

            For Each Row In AusgabeTable.Rows

                If CBool(Row("AccountIsLockedOut")) = False Then
                    Row("Temp") = "nein"
                Else
                    Row("Temp") = "ja"
                End If

            Next

            AusgabeTable.Columns.Remove("AccountIsLockedOut")
            AusgabeTable.Columns("Temp").ColumnName = "AccountIsLockedOut"

            AusgabeTable.AcceptChanges()

            For Each col2 In TempAusgabeTable.Columns

                Found = False


                For Each FieldControl In gvBestand.Columns

                    If col2.ColumnName.ToUpper = FieldControl.SortExpression.ToUpper Then
                        Found = True

                        sColName = FieldControl.HeaderText

                        AusgabeTable.Columns(col2.ColumnName).ColumnName = sColName
                        If FieldControl.Visible = False Then Found = False

                        Exit For

                    End If


                Next

                If Found = False Then
                    AusgabeTable.Columns.Remove(AusgabeTable.Columns(col2.ColumnName))

                End If

                AusgabeTable.AcceptChanges()
            Next


            Dim DataSetUser As New DataSet

            AusgabeTable.TableName = "User"
            TempTable.TableName = "Anwendungen"

            DataSetUser.Tables.Add(AusgabeTable)
            DataSetUser.Tables.Add(TempTable)

            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, DataSetUser, Me.Page)


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        If rdbAnwendung.Checked = False And rdbGruppe.Checked = False And rdbAlle.Checked = False Then
            lblError.Text = "Bitte wählen: Anwendung, Gruppe oder Alle."
        Else
            Dim Checked As Boolean = True
            Dim TempTable As New DataTable
            Dim GetData As New UserGroupApp
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Select Case Checked
                Case rdbAnwendung.Checked
                    TempTable = GetData.GetUserByAppID(m_User.Customer.CustomerId, CInt(ddlAnwendung.SelectedValue), cn)
                Case rdbGruppe.Checked
                    TempTable = GetData.GetUserByGroup(m_User.Customer.CustomerId, CInt(ddlGruppe.SelectedValue), cn)
                Case rdbAlle.Checked
                    TempTable = GetData.GetUserByCustomerID(m_User.Customer.CustomerId, cn)
            End Select

            If TempTable.Rows.Count > 0 Then
                If Session("UserDataTable") Is Nothing Then
                    Session.Add("UserDataTable", TempTable)
                Else
                    Session("UserDataTable") = TempTable
                End If

                FillGrid(0)

            Else
                lblError.Text = "Zu dieser Auswahl konnten keine Daten ermittelt werden."
            End If

        End If
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub


    Protected Sub rdbAnwendung_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbAnwendung.CheckedChanged
        If rdbAnwendung.Checked = True Then
            ddlAnwendung.Enabled = True
            rdbGruppe.Checked = False
            rdbAlle.Checked = False
            ddlGruppe.Enabled = False
        Else
            ddlAnwendung.Enabled = False
        End If

        ResetGrid()
    End Sub

    Protected Sub rdbGruppe_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbGruppe.CheckedChanged
        If rdbGruppe.Checked = True Then
            ddlGruppe.Enabled = True
            rdbAnwendung.Checked = False
            rdbAlle.Checked = False
            ddlAnwendung.Enabled = False
        Else
            ddlGruppe.Enabled = False
        End If

        ResetGrid()
    End Sub

    Protected Sub rdbAlle_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbAlle.CheckedChanged
        If rdbAlle.Checked = True Then
            ddlAnwendung.Enabled = False
            ddlGruppe.Enabled = False
            rdbGruppe.Checked = False
            rdbAnwendung.Checked = False
        End If

        ResetGrid()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        ModalPopupExtender2.Show()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        gvBestand.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub gvBestand_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvBestand.RowEditing
        Dim row As GridViewRow = gvBestand.Rows(e.NewEditIndex)

        Dim lbl As Label

        lbl = CType(row.FindControl("lblGroup"), Label)


        lblGruppe.Text = "Anwendungen der Gruppe " & lbl.Text

        lbl = CType(row.FindControl("lblGroupID"), Label)

        Dim GetData As New UserGroupApp

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()


        Dim TempTable As DataTable = GetData.GetApplicationsPerGroup(CInt(lbl.Text), cn)


        grvAnwendungen.DataSource = TempTable.DefaultView
        grvAnwendungen.DataBind()
    End Sub

    Private Sub gvBestand_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvBestand.Sorting
        FillGrid(gvBestand.PageIndex, e.SortExpression)
    End Sub

#End Region

#Region "Methods"

    Private Sub FillDropdowns()
        Dim GetData As New UserGroupApp

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        Dim TempTable As DataTable = GetData.GetApplications(m_User.Customer.CustomerId, cn)

        ddlAnwendung.DataSource = TempTable.DefaultView

        ddlAnwendung.DataValueField = "AppID"
        ddlAnwendung.DataTextField = "AppFriendlyName"

        ddlAnwendung.DataBind()

        TempTable = GetData.GetGroups(m_User.Customer.CustomerId, cn)

        ddlGruppe.DataSource = TempTable.DefaultView

        ddlGruppe.DataValueField = "GroupID"
        ddlGruppe.DataTextField = "GroupName"

        ddlGruppe.DataBind()

    End Sub


    Private Sub FillGrid(ByVal PageIndex As Int32, Optional ByVal Sort As String = "")


        Dim UserDataTable As New DataTable
        Dim Direction As String = String.Empty

        If IsNothing(Session("UserDataTable")) = False Then

            UserDataTable = CType(Session("UserDataTable"), DataTable)

            If UserDataTable.Rows.Count > 0 Then


                gvBestand.Visible = True
                'ddlPageSize.Visible = True

                'tdExcel.Visible = True

                Result.Visible = True

                Dim TempPageIndex As Int32 = PageIndex

                If Sort.Trim(" "c).Length > 0 Then
                    PageIndex = 0
                    Sort = Sort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = Sort) Then
                        If ViewState("Direction") Is Nothing Then
                            Direction = "desc"
                        Else
                            Direction = ViewState("Direction").ToString
                        End If
                    Else
                        Direction = "desc"
                    End If

                    If Direction = "asc" Then
                        Direction = "desc"
                    Else
                        Direction = "asc"
                    End If

                    ViewState("Sort") = Sort
                    ViewState("Direction") = Direction
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        Sort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            Direction = "asc"
                            ViewState("Direction") = Direction
                        Else
                            Direction = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not Sort.Length = 0 Then
                    UserDataTable.DefaultView.Sort = Sort & " " & Direction
                End If

                gvBestand.PageIndex = PageIndex

                gvBestand.DataSource = UserDataTable.DefaultView
                gvBestand.DataBind()


                lnkCreateExcel1.Visible = True

                lblMessage.Text = "Es wurden " & UserDataTable.Rows.Count & " Datensätze gefunden."
                lblMessage.Visible = True

            Else
                lblMessage.Visible = True
                lblMessage.Text = "Es wurden keine Datensätze gefunden."
                lbCreate.Visible = True

            End If
        Else
            lblMessage.Visible = True
        End If

    End Sub

    Private Sub ResetGrid()

        Result.Visible = False

    End Sub


#End Region

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        'lbCreate.Visible = True
        'tab1.Visible = True
        'Queryfooter.Visible = True
        'Result.Visible = False
    End Sub
End Class