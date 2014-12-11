Imports CKG.Base.Kernel
Imports System.Data.SqlClient

Partial Public Class ResponsiblePage
    Inherits System.Web.UI.Page

#Region " Membervariables "
    Private m_User As Security.User
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Session("objUser") Is Nothing Then
                m_User = CType(Session("objUser"), Security.User)
            End If

            lblHead.Text = "Ansprechpartner"
            Me.Title = "Ansprechpartner"

            If Not m_User Is Nothing AndAlso Not m_User.Customer Is Nothing AndAlso Not m_User.Groups.Count = 0 Then

                Dim cn As SqlConnection
                cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)

                Dim result As New DataTable()

                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If

                Dim daApp As New SqlClient.SqlDataAdapter("SELECT *, Name2 + ',' + Name1 + ' (' + [position] + ')' AS EmployeeName" & _
                                                          " FROM Contact INNER JOIN ContactGroups ON Contact.id = ContactGroups.ContactID " & _
                                                          " WHERE     (ContactGroups.CustomerID = @CustomerID) AND (ContactGroups.GroupID = @GroupID)", cn)

                daApp.SelectCommand.Parameters.AddWithValue("@CustomerID", m_User.Customer.CustomerId)
                daApp.SelectCommand.Parameters.AddWithValue("@GroupID", m_User.GroupID)
                daApp.Fill(result)

                If result.DefaultView.Count > 0 Then

                    If m_User.Customer.CustomerContact.Kundenpostfach.Trim.Length > 0 Then
                        For Each row In result.Rows
                            row("mail") = m_User.Customer.CustomerContact.Kundenpostfach.Trim
                        Next
                    End If

                    If m_User.Customer.CustomerContact.Kundenhotline.Trim.Length > 0 Then
                        For Each row In result.Rows
                            row("Telefon") = m_User.Customer.CustomerContact.Kundenhotline.Trim
                        Next
                    End If

                    If m_User.Customer.CustomerContact.Kundenfax.Trim.Length > 0 Then
                        For Each row In result.Rows
                            row("fax") = m_User.Customer.CustomerContact.Kundenfax.Trim
                        Next
                    End If

                    Repeater1.DataSource = result.DefaultView
                    Repeater1.DataBind()
                    Repeater1.Visible = True

                Else
                    Repeater1.Visible = False
                    TableRepeater.Height = "200px"
                End If

            Else
                Repeater1.Visible = False
                TableRepeater.Height = "200px"
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

End Class