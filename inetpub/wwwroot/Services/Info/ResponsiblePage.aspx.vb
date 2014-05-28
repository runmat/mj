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
            'InitHeader.InitUser(m_User)

            lblHead.Text = "Ansprechpartner"
            Me.Title = "Ansprechpartner"

            If Not m_User Is Nothing AndAlso Not m_User.Customer Is Nothing AndAlso Not m_User.Groups.Count = 0 Then

                Dim cn As SqlConnection
                cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                'Dim _EmployeeAssigned As New Admin.EmployeeList(m_User.GroupID, m_User.Customer.AccountingArea, cn)
                '_EmployeeAssigned.GetAssigned()

                'Dim cmd As New SqlClient.SqlCommand()
                'Dim adapter As New SqlClient.SqlDataAdapter()
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
                    'result.DefaultView.Sort = "employeeHierarchy"

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

                    'For Each RepItem As RepeaterItem In Repeater1.Controls
                    '    Dim Ctrl As Control
                    '    Dim Ctrl2 As Control
                    '    Ctrl = RepItem.FindControl("divMail")
                    '    Ctrl2 = RepItem.FindControl("divMailPartner")

                    '    If m_User.Customer.CustomerContact.Kundenpostfach.Trim.Length > 0 Then
                    '        Ctrl.Visible = False
                    '        Ctrl2.Visible = True
                    '    Else
                    '        Ctrl.Visible = True
                    '        Ctrl2.Visible = False
                    '    End If

                    'Next
                Else
                    Repeater1.Visible = False
                    TableRepeater.Height = "200px"
                End If
                'getCustomerNews()
            Else
                Repeater1.Visible = False
                'imgNews1.Visible = False
                'imgNews2.Visible = False
                'lblNews1.Visible = False
                'lblNews2.Visible = False
                TableRepeater.Height = "200px"
                'TableNews.Height = "200px"
            End If
            'If Not m_User Is Nothing AndAlso Not m_User.Customer Is Nothing AndAlso Not m_User.Groups.Count = 0 Then

            '    Dim cn As SqlClient.SqlConnection
            '    cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
            '    Dim _EmployeeAssigned As New Admin.EmployeeList(m_User.GroupID, m_User.Customer.AccountingArea, cn)
            '    _EmployeeAssigned.GetAssigned()
            '    If _EmployeeAssigned.DefaultView.Count > 0 Then
            '        _EmployeeAssigned.DefaultView.Sort = "employeeHierarchy"
            '        Repeater1.DataSource = _EmployeeAssigned.DefaultView
            '        Repeater1.DataBind()
            '        Repeater1.Visible = True
            '        For Each RepItem As RepeaterItem In Repeater1.Controls
            '            Dim Ctrl As Control
            '            Dim Ctrl2 As Control
            '            Ctrl = RepItem.FindControl("divMail")
            '            Ctrl2 = RepItem.FindControl("divMailPartner")
            '            If m_User.Customer.CustomerContact.Kundenpostfach.Trim.Length > 0 Then
            '                Ctrl.Visible = False
            '                Ctrl2.Visible = True
            '            Else
            '                Ctrl.Visible = True
            '                Ctrl2.Visible = False
            '            End If

            '        Next
            '    Else
            '        Repeater1.Visible = False
            '    End If
            'Else
            '    Repeater1.Visible = False
            'End If

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    lnkAnsprech.Text = CType(sender, LinkButton).Text
    '    lnkAnsprech.NavigateUrl = "mailto:" & lnkAnsprech.Text
    '    lnkFirmenPost.Text = m_User.Customer.CustomerContact.Kundenpostfach
    '    lnkFirmenPost.NavigateUrl = "mailto:" & m_User.Customer.CustomerContact.Kundenpostfach

    '    ModalPopupExtender2.Show()
    'End Sub

End Class