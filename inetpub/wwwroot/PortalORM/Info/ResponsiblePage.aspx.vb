Imports CKG.Base.Kernel


Namespace Info
    Public Class ResponsiblePage
        Inherits System.Web.UI.Page
        Protected WithEvents ucStyles As PageElements.Styles

#Region " Vom Web Form Designer generierter Code "
        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
        Protected WithEvents Repeater1 As System.Web.UI.WebControls.Repeater
        Protected WithEvents ucHeader As PageElements.Header
        Protected WithEvents lnkAnsprech As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lnkFirmenPost As System.Web.UI.WebControls.HyperLink
        Protected WithEvents lblSupportAdress As System.Web.UI.WebControls.Label
        Protected WithEvents FormDiv As System.Web.UI.HtmlControls.HtmlGenericControl
        Protected WithEvents divInfo As System.Web.UI.HtmlControls.HtmlGenericControl
        Protected WithEvents btnCancel As System.Web.UI.WebControls.Button
        'Dieser Aufruf ist für den Web Form-Designer erforderlich.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
            'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
            InitializeComponent()
        End Sub

#End Region

#Region " Membervariables "
        Private m_User As Security.User
#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            ' Hier Benutzercode zur Seiteninitialisierung einfügen
            Try
                If Not Session("objUser") Is Nothing Then
                    m_User = CType(Session("objUser"), Security.User)
                End If
                'InitHeader.InitUser(m_User)

                ucHeader.InitUser(m_User)
                ucStyles.TitleText = "Ansprechpartner"
                If Not m_User Is Nothing AndAlso Not m_User.Customer Is Nothing AndAlso Not m_User.Groups.Count = 0 Then

                    Dim cn As SqlClient.SqlConnection
                    cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                    'Dim _EmployeeAssigned As New Admin.EmployeeList(m_User.GroupID, m_User.Customer.AccountingArea, cn)
                    '_EmployeeAssigned.GetAssigned()
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
                            For Each row As DataRow In result.Rows
                                row("mail") = m_User.Customer.CustomerContact.Kundenpostfach.Trim
                            Next
                        End If

                        If m_User.Customer.CustomerContact.Kundenhotline.Trim.Length > 0 Then
                            For Each row As DataRow In result.Rows
                                row("Telefon") = m_User.Customer.CustomerContact.Kundenhotline.Trim
                            Next
                        End If

                        If m_User.Customer.CustomerContact.Kundenfax.Trim.Length > 0 Then
                            For Each row As DataRow In result.Rows
                                row("fax") = m_User.Customer.CustomerContact.Kundenfax.Trim
                            Next
                        End If

                        Repeater1.DataSource = result.DefaultView
                        'If _EmployeeAssigned.DefaultView.Count > 0 Then
                        '    _EmployeeAssigned.DefaultView.Sort = "employeeHierarchy"
                        'Repeater1.DataSource = _EmployeeAssigned.DefaultView
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
                    End If
                Else
                    Repeater1.Visible = False
                End If

            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
        End Sub

        Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            lnkAnsprech.Text = CType(sender, LinkButton).Text
            lnkAnsprech.NavigateUrl = "mailto:" & lnkAnsprech.Text
            lnkFirmenPost.Text = Trim(m_User.Customer.CustomerContact.Kundenpostfach)
            lnkFirmenPost.NavigateUrl = "mailto:" & Trim(m_User.Customer.CustomerContact.Kundenpostfach)
            'lblSupportAdress.Text = m_User.Customer.CustomerContact.Kundenpostfach

            FormDiv.Attributes.Add("class", "transbox")
            divInfo.Attributes.Add("style", "display:block; position: absolute; z-index:100;border-color: #000000;" & _
                                   "border-style: solid; border-width: 1px;  background-color: #dfdfdf; " & _
                                   " left:33%; top:33%; padding: 5px")
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            FormDiv.Attributes.Remove("class")
            divInfo.Attributes.Add("style", "display:none;")
        End Sub
    End Class
End Namespace

' ************************************************
' $History: ResponsiblePage.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Dittbernerc  Date: 27.05.11   Time: 13:48
' Updated in $/CKAG/PortalORM/Info
' 
' *****************  Version 5  *****************
' User: Dittbernerc  Date: 19.05.11   Time: 14:38
' Updated in $/CKAG/PortalORM/Info
' 
' *****************  Version 4  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 13:39
' Updated in $/CKAG/PortalORM/Info
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 27.10.09   Time: 14:33
' Updated in $/CKAG/PortalORM/Info
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 26.10.09   Time: 11:44
' Updated in $/CKAG/PortalORM/Info
' 
' *****************  Version 1  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Created in $/CKAG/PortalORM/Info
' ITA 2152 und 2158
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 17:17
' Created in $/CKAG/PortalORM/Info
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/PortalORM/Info
' 
' ************************************************