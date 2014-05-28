Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Services

Public Class SingleUserHistory
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Organization)
        GridNavigation1.setGridElment(dgSearchResult)

        Try
            m_App = New App(m_User)

            lblError.Text = ""

            If Not IsPostBack Then
                Dim tmpUser As New User(CInt(Request.QueryString("UserID")), m_User.App.Connectionstring)
                lblUsername.Text = tmpUser.UserName

                If Not m_User.Customer.AccountingArea = -1 Then
                    If m_User.Customer.AccountingArea = tmpUser.Customer.AccountingArea Then
                        If (m_User.HighestAdminLevel < AdminLevel.Master) AndAlso (Not (m_User.Customer.CustomerId = tmpUser.Customer.CustomerId)) Then
                            Throw New Exception("Sie dürfen auf den Benutzer nicht zugreifen.")
                        End If
                    Else
                        Throw New Exception("Sie dürfen auf den Benutzer nicht zugreifen.")
                    End If
                End If

                ceTxtDatumVon.SelectedDate = Today.AddDays(-30)
                txtDatumVon.Text = ceTxtDatumVon.SelectedDate.Value.ToShortDateString()

                ceTxtDatumBis.SelectedDate = Today
                txtDatumBis.Text = ceTxtDatumBis.SelectedDate.Value.ToShortDateString()
            Else

                If IsDate(txtDatumVon.Text) Then
                    ceTxtDatumVon.SelectedDate = txtDatumVon.Text
                End If

                If IsDate(txtDatumBis.Text) Then
                    ceTxtDatumBis.SelectedDate = txtDatumBis.Text
                End If


            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "SingleUserHistory", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString()
        End Try
    End Sub

    Private Sub FilldgSearchResult(ByVal blnForceNew As Boolean, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim dt As New DataTable()
        If Not blnForceNew AndAlso (Not Session("UserResult") Is Nothing) Then
            dt = CType(Session("UserResult"), DataTable)
        Else
            If Not IsDate(txtDatumVon.Text) Then
                lblError.Text = "Bitte gültiges Startdatum übergeben."
                Exit Sub
            ElseIf Not IsDate(txtDatumBis.Text) Then
                lblError.Text = "Bitte gültiges Enddatum übergeben."
                Exit Sub
            ElseIf CDate(txtDatumVon.Text) > CDate(txtDatumBis.Text) Then
                lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!"
                Exit Sub
            Else
                Dim strTemp As String
                strTemp = CStr(CDate(txtDatumBis.Text).AddDays(1))

                Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()
                Dim da As New SqlClient.SqlDataAdapter("SELECT ID, [Action] AS Aktion, LastChangedBy AS [Änderer], OldValue AS [Alter Wert], NewValue AS [Neuer Wert], InsertDate AS Zeitpunkt, SaveType AS Typ FROM AdminHistory_User WHERE (Username = @Username) AND (InsertDate BETWEEN CONVERT ( Datetime , '" & txtDatumVon.Text & "' , 104 ) AND CONVERT ( Datetime , '" & strTemp & "' , 104 )) ", cn)
                With da.SelectCommand.Parameters
                    .AddWithValue("@Username", lblUsername.Text)
                End With
                da.Fill(dt)
                Session("UserResult") = dt
            End If
        End If

        If dt.Rows.Count = 0 Then
            lblError.Text = "Keine Ergebnisse für den genannten Zeitraum."
            Exit Sub
        End If
        If strSort.Length > 0 Then
            If CStr(ViewState("mySort")) = strSort Then
                strSort &= " DESC"
            End If
        Else
            If CStr(ViewState("mySort")).Length = 0 Then
                strSort = "ID DESC"
            Else
                strSort = CStr(ViewState("mySort"))
            End If
        End If
        ViewState("mySort") = strSort

        dt.DefaultView.Sort = strSort
        With dgSearchResult
            .CurrentPageIndex = intPageIndex
            .DataSource = dt
            .DataBind()
            .Visible = True
        End With
        Result.Visible = True
    End Sub

    Private Sub dgSearchResult_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgSearchResult.SortCommand
        FilldgSearchResult(False, dgSearchResult.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub dgSearchResult_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSearchResult.PageIndexChanged
        FilldgSearchResult(False, e.NewPageIndex)
    End Sub

    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        Result.Visible = False

        ViewState("mySort") = ""
        FilldgSearchResult(True, 0)
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As New DataTable()
        Dim dvUser As DataView

        dvUser = CType(Session("UserResult"), DataTable).DefaultView

        If dvUser.Count > 0 Then
            reportExcel = dvUser.Table

            Try
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
                excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try

        End If
    End Sub

    Private Sub GridNavigation1_PagerChanged(PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FilldgSearchResult(False, PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FilldgSearchResult(False, 0)
    End Sub
End Class