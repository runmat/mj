Imports CKG.Base.Kernel.Admin
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business

Partial Public Class Contact
    Inherits System.Web.UI.Page


#Region " Membervariables "
    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As Global.Admin.GridNavigation
    Private m_Rights As DataTable
    Private m_Districts As DataTable
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)

        lblHead.Text = "Ansprechpartner verwalten"
        AdminAuth(Me, m_User, AdminLevel.FirstLevel)
        GridNavigation1.setGridElment(dgSearchResult)
        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                FillHierarchy()
                FillForm()
                FillAccountingArea(m_User.Customer.CustomerId, True)
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Contact", "Page_Load", ex.ToString)

            lblError.Text = ex.ToString
        End Try
    End Sub

    Private Sub FillForm()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            FillCustomer(cn) 'DropDowns fuer Customer fuellen
            FillGroups(CInt(ddlFilterCustomer.SelectedItem.Value), cn) 'DropDowns fuer Group fuellen

            trEmployee02.Visible = False
            trEmployee03.Visible = False
            trEmployee04.Visible = False
            trEmployee05.Visible = False
            trEmployee07.Visible = False

            If m_User.HighestAdminLevel > AdminLevel.Customer Then
                'Wenn SuperUser:
                lblCustomer.Visible = False 'Label mit Customer-Namen ausblenden
                ddlFilterCustomer.Visible = True 'DropDown zur Customer-Auswahl einblenden
                'wenn SuperUser und übergeordnete Firma
                If m_User.Customer.AccountingArea = -1 Then
                    lnkAppManagement.Visible = True
                    trMandant.Visible = True
                End If

                trEmployee02.Visible = True
                trEmployee03.Visible = True
                trEmployee04.Visible = True
                trEmployee05.Visible = True
                trEmployee07.Visible = True
                
            Else
                'Wenn nicht SuperUser:
                lnkArchivManagement.Visible = False
                lnkCustomerManagement.Visible = False 'Link fuer die Kundenverwaltung ausblenden
                lblCustomer.Text = m_User.Customer.CustomerName 'Customer des angemeldeten Benutzers
                dgSearchResult.Columns(6).Visible = False 'Spalte "Test-Zugang" ausblenden
                trTestUser.Visible = False '"Test-Zugang" aus dem Edit-Bereich ausblenden
                dgSearchResult.Columns(8).Visible = False 'Spalte "Passwort läuft nie ab" ausblenden
                dgSearchResult.Columns(5).Visible = False 'Spalte "Customer-Admin" ausblenden
                lnkAppManagement.Visible = False 'Link fuer die Anwendungsverwaltung ausblenden


                If Not m_User.Customer.ShowOrganization Then
                    lnkOrganizationManagement.Visible = False
                    dgSearchResult.Columns(4).Visible = False 'Spalte "Organisation" ausblenden
                End If

                If Not m_User.IsCustomerAdmin Then
                    'Wenn nicht Customer-Admin:
                    lnkOrganizationManagement.Visible = False
                    lnkGroupManagement.Visible = False
                    dgSearchResult.Columns(4).Visible = False 'Spalte "Organisation" ausblenden
                    If m_User.Groups.Count > 0 Then lblGroup.Text = m_User.Groups(0).GroupName 'Gruppe des angemeldeten Benutzers
                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Sub FillCustomer(ByVal cn As SqlClient.SqlConnection)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn)

        dtCustomers.AddAllNone(True, True)
        With ddlFilterCustomer
            Dim dv As DataView = dtCustomers.DefaultView
            dv.Sort = "Customername"
            .DataSource = dv
            .DataTextField = "Customername"
            .DataValueField = "CustomerID"
            .DataBind()
            If m_User.HighestAdminLevel = AdminLevel.Master Or m_User.HighestAdminLevel = AdminLevel.FirstLevel Then
                .Items.FindByValue("0").Selected = True
            Else
                .Enabled = False
                .Items.FindByValue(m_User.Customer.CustomerId.ToString).Selected = True
            End If

        End With
    End Sub

    Private Sub FillGroups(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
        Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
        FillGroup(ddlFilterGroup, True, dtGroups)
        If ddlFilterGroup.Items.Count = 0 Then
            ddlFilterGroup.Enabled = False
            btnSuche.Enabled = False
        End If
    End Sub

    Private Sub FillGroup(ByVal ddl As DropDownList, ByVal blnAllNone As Boolean, ByVal dtgroups As Kernel.GroupList)
        If blnAllNone Then dtgroups.AddAllNone(True, True)
        With ddl
            .Items.Clear()
            Dim dv As DataView = dtgroups.DefaultView
            dv.Sort = "GroupName"
            If m_User.HighestAdminLevel = AdminLevel.Organization AndAlso m_User.Customer.OrgAdminRestrictToCustomerGroup Then
                dv.RowFilter = "IsCustomerGroup=1"
            End If
            .DataSource = dv
            .DataTextField = "GroupName"
            .DataValueField = "GroupID"
            .DataBind()
            If m_User.Groups.HasGroups Then
                Dim _li As ListItem
                _li = .Items.FindByValue(m_User.Groups(0).GroupId.ToString)
                If Not _li Is Nothing Then _li.Selected = True
                If ddl.ID = "ddlGroups" Then
                    _li = .Items.FindByValue("-1")
                    If Not _li Is Nothing Then
                        ddl.ClearSelection()
                        _li.Selected = True
                    End If
                End If
            Else
                If .Items.Count <> 0 Then
                    If blnAllNone Then
                        Dim _li As ListItem = .Items.FindByValue("-1")
                        If Not _li Is Nothing Then
                            _li.Selected = True
                        End If
                    End If
                End If
            End If
        End With
    End Sub

    Protected Sub lbtnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnNew.Click

        Session("ContactID") = Nothing
        SearchMode(False)
        ClearEdit()

        ibtSetGroup.Enabled = False
        trEmployee05.Visible = False
        trEmployee07.Visible = False

    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True, Optional ByVal blnNewSearch As Boolean = False, Optional ByVal blnShowGroupArea As Boolean = False)
        'gewünschte Expand/Collapse-Stati für die Seitenabschnitte in hidden fields setzen, werden dann von JQuery ausgewertet
        If blnSearchMode Then
            If blnNewSearch Then
                ihExpandstatusSearchFilterArea.Value = "1"
                ihExpandstatusSearchResultArea.Value = "0"
            Else
                ihExpandstatusSearchFilterArea.Value = "0"
                ihExpandstatusSearchResultArea.Value = "1"
            End If
            ihExpandStatusInputArea.Value = "0"
            ihExpandStatusInputGroupArea.Value = "0"
        Else
            ihExpandstatusSearchFilterArea.Value = "0"
            ihExpandstatusSearchResultArea.Value = "0"
            If blnShowGroupArea Then
                ihExpandStatusInputArea.Value = "0"
                ihExpandStatusInputGroupArea.Value = "1"
            Else
                ihExpandStatusInputArea.Value = "1"
                ihExpandStatusInputGroupArea.Value = "0"
            End If
        End If
    End Sub

    Private Sub ClearEdit()

        'texboxen
        '----------------------------------------
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtDepartment.Text = ""
        txtPosition.Text = ""
        txtMobil.Text = ""
        txtFax.Text = ""
        txtMail.Text = ""
        txtPhone.Text = ""


        'labels
        '----------------------------------------
        lblPictureName.Text = ""
        '----------------------------------------

        'linkbuttons
        '----------------------------------------
        lbtnSave.Visible = True
        lbtnDelete.Visible = False
        '----------------------------------------

        'dropDownListen
        '----------------------------------------
        ddlTitle.SelectedIndex = 0
        ddlHierarchy.SelectedValue = "1"
        '----------------------------------------


        LockEdit(False)

    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim enabled As Boolean = Not blnLock
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If
        Dim backColor As System.Drawing.Color = System.Drawing.Color.FromName(strBackColor)

        ddlAccountingArea.Enabled = enabled
        ddlAccountingArea.BackColor = backColor
        btnUpload.Enabled = enabled
        btnRemove.Enabled = enabled
        ddlTitle.Enabled = enabled
        ddlTitle.BackColor = backColor
        txtFirstName.Enabled = enabled
        txtFirstName.BackColor = backColor
        txtLastName.Enabled = enabled
        txtLastName.BackColor = backColor
        txtMail.Enabled = enabled
        txtMail.BackColor = backColor
        txtPhone.Enabled = enabled
        txtPhone.BackColor = backColor
        txtMobil.Enabled = enabled
        txtMobil.BackColor = backColor
        ddlHierarchy.Enabled = enabled
        ddlHierarchy.BackColor = backColor
        txtDepartment.Enabled = enabled
        txtDepartment.BackColor = backColor
        txtPosition.Enabled = enabled
        txtPosition.BackColor = backColor
        txtFax.Enabled = enabled
        txtFax.BackColor = backColor
    End Sub

    Protected Sub lbtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSave.Click
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim cmdTable As SqlClient.SqlCommand
        Dim intTemp As Int32

        If ValidateContact() = True Then
            lblError.Text = "Es wurden nicht alle Pflichtfelder gefüllt."
            Exit Sub
        End If


        Dim Mandant As String
        Dim SQL As String

        If Session("ContactID") Is Nothing Then
            SQL = "SELECT Contact.ID FROM Contact where Name1=@Name1 and Name2=@Name2"
            Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand(SQL, cn)

            Dim tblReturn As New DataTable

            cmd.Parameters.AddWithValue("Name1", Trim(txtFirstName.Text))
            cmd.Parameters.AddWithValue("Name2", Trim(txtLastName.Text))

            Dim adContact As New SqlClient.SqlDataAdapter(cmd)

            adContact.Fill(tblReturn)

            If tblReturn.Rows.Count > 0 Then

                lblError.Text = "Ansprechpartner existiert bereits!"
                Exit Sub
            End If
        End If
       
        If trMandant.Visible = True Then
            Mandant = ddlAccountingArea.SelectedValue
        Else
            Mandant = m_User.Customer.AccountingArea
        End If


        Try

            If Session("ContactID") Is Nothing Then
                SQL = "INSERT INTO Contact (Mandant,Anrede,Name1,Name2,Telefon,Fax,Mobile,Mail,Hierarchie,Abteilung,Position,PictureName) VALUES (@Mandant,@Anrede,@Name1,@Name2,@Telefon,@Fax,@Mobile,@Mail,@Hierarchie,@Abteilung,@Position,@PictureName);SELECT SCOPE_IDENTITY() AS 'Identity'"
            Else
                SQL = "Update Contact set Mandant=@Mandant,Anrede=@Anrede,Name1=@Name1,Name2=@Name2,Telefon=@Telefon,Fax=@Fax,Mobile=@Mobile,Mail=@Mail,Hierarchie=@Hierarchie,Abteilung=@Abteilung,Position=@Position,PictureName=@PictureName where ID=@ContactID"
            End If



            cmdTable = New SqlClient.SqlCommand(SQL, cn)
            cmdTable.Parameters.AddWithValue("@Mandant", Mandant)
            cmdTable.Parameters.AddWithValue("@Anrede", ddlTitle.SelectedValue)
            cmdTable.Parameters.AddWithValue("@Name1", txtFirstName.Text)
            cmdTable.Parameters.AddWithValue("@Name2", txtLastName.Text)
            cmdTable.Parameters.AddWithValue("@Telefon", txtPhone.Text)
            cmdTable.Parameters.AddWithValue("@Fax", txtFax.Text)
            cmdTable.Parameters.AddWithValue("@Mobile", txtMobil.Text)
            cmdTable.Parameters.AddWithValue("@Mail", txtMail.Text)
            cmdTable.Parameters.AddWithValue("@Hierarchie", ddlHierarchy.SelectedValue)
            cmdTable.Parameters.AddWithValue("@Abteilung", txtDepartment.Text)
            cmdTable.Parameters.AddWithValue("@Position", txtPosition.Text)

            cn.Open()
            cmdTable.CommandType = CommandType.Text

            If Session("ContactID") Is Nothing Then
                cmdTable.Parameters.AddWithValue("@PictureName", System.DBNull.Value)
                intTemp = CInt(cmdTable.ExecuteScalar)

                Session("ContactID") = intTemp.ToString

            Else
                cmdTable.Parameters.AddWithValue("@PictureName", lblPictureName.Text)
                cmdTable.Parameters.AddWithValue("@ContactID", Session("ContactID"))
                cmdTable.ExecuteNonQuery()

            End If


            If String.IsNullOrEmpty(lblPictureName.Text) = False Then
                Image1.ImageUrl = Replace(System.Configuration.ConfigurationManager.AppSettings("UploadPathContacts"), "\", "/") & "responsible/" & lblPictureName.Text
            End If



            ibtSetGroup.Enabled = True
            trEmployee05.Visible = True
            trEmployee07.Visible = True

            lblMessage.Text = "Speichern erfolgreich."

        Catch ex As Exception
            lblError.Text = "Die Daten konnten nicht gespeichert werden."

        Finally
            cn.Close()
        End Try

    End Sub

    Private Function ValidateContact() As Boolean

        Dim booError As Boolean = False

        ClearBorder()

        If CheckEmpty(txtFirstName) = True Then SetBorder(txtFirstName, Drawing.Color.Red) : booError = True
        If CheckEmpty(txtLastName) = True Then SetBorder(txtLastName, Drawing.Color.Red) : booError = True
        If CheckEmpty(txtPhone) = True Then SetBorder(txtPhone, Drawing.Color.Red) : booError = True
        If CheckEmpty(txtMail) = True Then SetBorder(txtMail, Drawing.Color.Red) : booError = True


        Return booError

    End Function

    Private Sub ClearBorder()
        SetBorder(txtFirstName, Drawing.Color.Empty)
        SetBorder(txtLastName, Drawing.Color.Empty)
        SetBorder(txtPhone, Drawing.Color.Empty)
        SetBorder(txtMail, Drawing.Color.Empty)
    End Sub

    Private Function CheckEmpty(ByVal txt As TextBox) As Boolean

        Dim booEmpty As Boolean = False

        If Trim(txt.Text).Length = 0 Then
            booEmpty = True
        End If

        Return booEmpty

    End Function

    Private Sub SetBorder(ByVal ctrl As Control, ByVal SetColor As System.Drawing.Color)

        If TypeOf (ctrl) Is TextBox Then
            CType(ctrl, TextBox).BorderColor = SetColor
        End If

    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try

            If Session("ContactID") = Nothing Then
                lblError.Text = "Bitte Speichern Sie zunächst den Ansprechpartner ab."
                Exit Sub
            End If

            If Not (upFile.PostedFile.FileName = String.Empty) Then
                Dim fname As String = upFile.PostedFile.FileName
                If (upFile.PostedFile.ContentLength > CType(System.Configuration.ConfigurationManager.AppSettings("MaxUploadSize"), Integer)) Then
                    lblError.Text = "Datei '" & Right(fname, fname.Length - fname.LastIndexOf("\") - 1).ToUpper & "' ist zu gross (>300 KB)."
                    Exit Sub
                End If
                '------------------
                If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".JPG" Then
                    lblError.Text = "Es können nur Bilddateien im JPG - Format verarbeitet werden."
                    Exit Sub
                End If

                'upFile.PostedFile
                If Not (upFile.PostedFile Is Nothing) Then
                    Dim info As System.IO.FileInfo
                    Dim uFile As System.Web.HttpPostedFile = upFile.PostedFile
                    Dim fnameOld As String = ""
                    If lblPictureName.Text.Length > 0 Then
                        fnameOld = System.Configuration.ConfigurationManager.AppSettings("UploadPathContactsLocal") & "responsible\" & lblPictureName.Text
                        info = New System.IO.FileInfo(fnameOld)
                        If (info.Exists) Then
                            System.IO.File.Delete(fnameOld)
                        End If
                    End If

                    Dim TimeAppend As String

                    TimeAppend = DateTime.Now.Hour & DateTime.Now.Minute & DateTime.Now.Second


                    Dim fnameNew As String = System.Configuration.ConfigurationManager.AppSettings("UploadPathContactsLocal") & "responsible\" & Session("ContactID") & TimeAppend & ".jpg"

                    uFile.SaveAs(fnameNew)
                    info = New System.IO.FileInfo(fnameNew)
                    If Not (info.Exists) Then
                        lblError.Text = "Fehler beim Speichern."
                    End If

                    Image1.ImageUrl = Replace(System.Configuration.ConfigurationManager.AppSettings("UploadPathContacts"), "\", "/") & "responsible/" & Session("ContactID") & TimeAppend & ".jpg"
                    lblPictureName.Text = Session("ContactID") & TimeAppend & ".jpg"


                    Dim cmdTable As SqlClient.SqlCommand
                    cn.Open()

                    cmdTable = New SqlClient.SqlCommand("Update Contact set PictureName = @PictureName Where id = @id", cn)

                    cmdTable.Parameters.AddWithValue("@PictureName", Session("ContactID") & TimeAppend & ".jpg")
                    cmdTable.Parameters.AddWithValue("@id", Session("ContactID"))


                    cmdTable.CommandType = CommandType.Text

                    cmdTable.ExecuteNonQuery()



                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim Hochladen. (" & ex.ToString & ")"
        End Try

    End Sub

    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnEmpty.Click
        btnSuche_Click(sender, e)
    End Sub

    Protected Sub btnSuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSuche.Click
        LoadContacts()
        If dgSearchResult.Rows.Count = 0 Then
            lblError.Text = "Keine Datensätze gefunden."
        End If
    End Sub

    Private Sub LoadContacts()
        Dim SQL As String
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim tblReturn As New DataTable()
        Dim SQLWhere As String = ""
        Dim Mandant As String = ""
        Dim Kunde As String = ""
        Dim Gruppe As String = ""
        Dim Name1 As String = ""
        Dim Name2 As String = ""
        Dim SQLFields As String = ""


        If m_User.Customer.AccountingArea <> -1 Then
            Mandant = " Mandant = " & m_User.Customer.AccountingArea
            SQLFields = Mandant
        End If


        If ddlFilterGroup.SelectedValue > 0 Then
            Gruppe = " AND GroupID = " & ddlFilterGroup.SelectedValue
        End If


        If ddlFilterCustomer.SelectedValue > 0 Then
            Kunde = " Contact.ID in (Select Distinct ContactID from ContactGroups Where CustomerID =  " & ddlFilterCustomer.SelectedValue & Gruppe & ")"
            If SQLFields.Length > 0 Then
                SQLFields &= " AND " & Kunde
            Else
                SQLFields = Kunde
            End If

        End If

        Dim strName1 As String = txtFilterName1.Text.Replace("*", "")
        If strName1.Length > 0 Then
            'Name1 = txtFilterName1.Text
            'If Name1.Contains("*") Then
            '    Name1 = " Name1 like '" & Replace(Name1, "*", "%") & "'"
            'Else
            '    Name1 = " Name1 = '" & Name1 & "'"
            'End If
            Name1 = " Name1 like '%" & strName1 & "%'"

            If SQLFields.Length > 0 Then
                SQLFields &= " AND " & Name1
            Else
                SQLFields = Name1
            End If

        End If

        Dim strName2 As String = txtFilterName2.Text.Replace("*", "")
        If strName2.Length > 0 Then
            'Name2 = txtFilterName2.Text
            'If Name2.Contains("*") Then
            '    Name2 = " Name2 like '" & Replace(Name2, "*", "%") & "'"
            'Else
            '    Name2 = " Name2 = '" & Name2 & "'"
            'End If
            Name2 = " Name2 like '%" & strName2 & "%'"

            If SQLFields.Length > 0 Then
                SQLFields &= " AND " & Name2
            Else
                SQLFields = Name2
            End If

        End If

        If (Trim(Mandant & Kunde & Name1 & Name2)).Length > 0 Then
            SQLWhere = "WHERE " & SQLFields
        End If


        SQL = "SELECT Contact.ID, dbo.Contact.Anrede, dbo.Contact.Name1, dbo.Contact.Name2, dbo.Contact.Telefon, dbo.Contact.Fax, dbo.Contact.Mobile, dbo.Contact.Mail, " & _
                      "dbo.Hierarchy.[Level] AS Hierarchie, dbo.Contact.Abteilung, dbo.Contact.Position " & _
                      "FROM dbo.Contact INNER JOIN " & _
                      "dbo.Hierarchy ON dbo.Contact.Hierarchie = dbo.Hierarchy.ID " & SQLWhere



        Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand(SQL, cn)

        Dim adContact As New SqlClient.SqlDataAdapter(cmd)

        adContact.Fill(tblReturn)


        tblReturn.Columns.Add("Ansprechpartner", GetType(System.String))
        tblReturn.AcceptChanges()
        Dim Seperator As String

        For Each dr As DataRow In tblReturn.Rows

            If String.IsNullOrEmpty(dr("Name2")) = False AndAlso String.IsNullOrEmpty(dr("Name1")) = False Then
                Seperator = ", "
            Else
                Seperator = ""
            End If


            dr("Ansprechpartner") = dr("Name2") & Seperator & dr("Name1")

        Next


        Session("ContactTable") = tblReturn

        FillGrid(0)
    End Sub

    Private Sub FillAccountingArea(ByVal intCustomerId As Int32, Optional ByVal neuanlage As Boolean = False)
        'AccountingArea
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _Customer As New Customer(intCustomerId, cn)
            Dim _AccountingAreaList As Kernel.AccountingAreaList
            _AccountingAreaList = New Kernel.AccountingAreaList(cn)
            Dim vwAccountingAreaList As DataView = _AccountingAreaList.DefaultView
            vwAccountingAreaList.Sort = "Area"
            ddlAccountingArea.DataSource = vwAccountingAreaList
            ddlAccountingArea.DataValueField = "Area"
            ddlAccountingArea.DataTextField = "Description"
            ddlAccountingArea.DataBind()
            ddlAccountingArea.ClearSelection()




            If m_User.Customer.AccountingArea = -1 Then
                'User gehört der Übergeordneten Firma an, Grundlegend BK änderbar
                ddlAccountingArea.Enabled = True
            Else
                'kein Übergeordneter user, Grundlegend BK nicht änderbar
                ddlAccountingArea.Enabled = False
            End If


            If Not ddlAccountingArea.Items.FindByValue(_Customer.AccountingArea.ToString) Is Nothing Then
                ddlAccountingArea.Items.FindByValue(_Customer.AccountingArea.ToString).Selected = True
            Else
                If _Customer.AccountingArea = -1 Then 'es wurde schon eine Vorselektion der Kunden nach BK vorgenommen
                    'wenn firma 1 aufgerufen wird, soll der buchungskreis nicht änderbar sein,
                    'wenn aber ein User der Firma 1 eine Neuanlage tätigt, soll der Buchungskreis auswählbar sein
                    'daher optionaler Parameter "Neuanlage", außerdem soll keine Übergeordnete Firma angelegt werden
                    If neuanlage Then
                        ddlAccountingArea.Enabled = True
                    Else
                        Dim newItem As New System.Web.UI.WebControls.ListItem("Übergeordnet", "-1")
                        ddlAccountingArea.Items.Add(newItem)
                        ddlAccountingArea.Items.FindByValue("-1").Selected = True
                        ddlAccountingArea.Enabled = False

                    End If
                Else
                    'der Buchungskreis wurde nicht gefunden ist aber auch keine Übergeordnete Firma mit -1? gibts nicht
                    Throw New Exception("Der Buchungskreis: " & _Customer.AccountingArea & " ist nicht bekannt!")
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim Befüllen der Accountingareas: " & ex.Message
        End Try

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()

        Dim tblTemp As DataTable = CType(Session("ContactTable"), DataTable)

        If tblTemp Is Nothing Then
            SearchMode(, True)
            Exit Sub
        End If

        tmpDataView = tblTemp.DefaultView


        If tmpDataView.Count = 0 Then
            SearchMode(, True)
            lblError.Text = "Zu Ihrer Selektion konnten keine Werte ermittelt werden"
            Exit Sub
        Else
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Dim intTempPageIndex As Int32 = intPageIndex

            If strSort.Trim(" "c).Length > 0 Then
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

            dgSearchResult.PageIndex = intTempPageIndex
            dgSearchResult.DataSource = tmpDataView
            dgSearchResult.DataBind()

            If dgSearchResult.Rows.Count > 0 Then
                SearchMode()
            Else
                SearchMode(, True)
            End If

        End If
    End Sub

    Private Sub Contact_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        HelpProcedures.FixedGridViewCols(dgSearchResult)
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgSearchResult.RowCommand

        If IsNumeric(e.CommandArgument) = False Then Exit Sub

        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = dgSearchResult.Rows(index)

        Dim CtrlLabel As Label
        CtrlLabel = row.Cells(0).FindControl("lblID")

        If e.CommandName = "Edit" Then

            
            Dim lblContactName As Label = row.Cells(0).FindControl("lblGridAnsprechpartner")

            lblAnsprechpartner.Text = lblContactName.Text

            SearchMode(False)

            FillContact(CtrlLabel.Text)

            Session("ContactID") = CtrlLabel.Text


            LockEdit(False)

            trEmployee05.Visible = True
            trEmployee07.Visible = True

        ElseIf e.CommandName = "Del" Then

            Session("ContactID") = CtrlLabel.Text
            dgSearchResult.SelectedIndex = row.RowIndex

            SearchMode(False)

            FillContact(CtrlLabel.Text)

            ibtSetGroup.Enabled = False
            Session("ContactID") = CtrlLabel.Text
            LockEdit(True)
            lbtnDelete.Visible = True
            lbtnSave.Visible = False

        ElseIf e.CommandName = "Group" Then

            Session("ContactID") = CtrlLabel.Text
            FillGridCustomer()

            SetSelectedCustomer()

            lblAnsprechpartner.Text = CType(row.Cells(0).FindControl("lblGridAnsprechpartner"), Label).Text

            SearchMode(False, , True)

            lbtnBack.Visible = False
            lbtnCancel.Visible = False
            lbtnSave.Visible = False
            lbtnBackToSearch.Visible = True
            trEmployee05.Visible = True
            trEmployee07.Visible = True

        End If
    End Sub

    Private Sub FillContact(ByVal id As String)
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim tblReturn As New DataTable()
        Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("Select * from Contact where ID = " & id, cn)
        Dim adContact As New SqlClient.SqlDataAdapter(cmd)

        adContact.Fill(tblReturn)

        Dim dr As DataRow = tblReturn.Rows(0)

        If String.IsNullOrEmpty(dr("Anrede")) = False Then
            ddlTitle.SelectedValue = dr("Anrede")
        End If

        If trMandant.Visible = True Then
            ddlAccountingArea.SelectedValue = dr("Mandant")
        End If

        txtFirstName.Text = dr("Name1")
        txtLastName.Text = dr("Name2")
        txtPhone.Text = dr("Telefon")
        txtMobil.Text = dr("Mobile")
        txtFax.Text = dr("Fax")
        txtMail.Text = dr("Mail")
        txtPosition.Text = dr("Position")
        txtDepartment.Text = dr("Abteilung")
        ddlHierarchy.SelectedValue = dr("Hierarchie")

        If String.IsNullOrEmpty(dr("PictureName").ToString) = False Then
            Image1.ImageUrl = Replace(System.Configuration.ConfigurationManager.AppSettings("UploadPathContacts"), "\", "/") & "responsible/" & dr("PictureName")
            lblPictureName.Text = dr("PictureName").ToString
        End If



    End Sub

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles dgSearchResult.RowEditing

    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgSearchResult.Sorting
        FillGrid(0, e.SortExpression)
    End Sub

    Protected Sub ddlFilterCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFilterCustomer.SelectedIndexChanged
        Dim intCustomerID As Integer = CInt(ddlFilterCustomer.SelectedItem.Value)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()
        If intCustomerID > 0 Then
            FillGroups(intCustomerID, cn)
        Else
            Dim dtGroups As New Kernel.GroupList(intCustomerID, cn, m_User.Customer.AccountingArea)
            FillGroup(ddlFilterGroup, True, dtGroups)
        End If
    End Sub

    Private Sub FillHierarchy()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()
            Dim _HierarchyList As New Kernel.HierarchyList(cn)
            Dim vwHierarchy As DataView = _HierarchyList.DefaultView
            ddlHierarchy.DataSource = vwHierarchy
            ddlHierarchy.DataValueField = "ID"
            ddlHierarchy.DataTextField = "Level"
            ddlHierarchy.DataBind()
            ddlHierarchy.ClearSelection()
            ddlHierarchy.Items.FindByValue("1").Selected = True

        Catch ex As Exception
            Throw
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Protected Sub ibtSetGroup_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSetGroup.Click
        

        FillGridCustomer()

        SetSelectedCustomer()

        lblAnsprechpartner.Text = txtLastName.Text & ", " & txtFirstName.Text

        SearchMode(False, , True)

    End Sub

    Private Sub SetSelectedCustomer()

        cblGroups.Items.Clear()

        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim tblReturn As New DataTable()
        Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("Select Distinct CustomerID from ContactGroups where ContactID=@ContactID", cn)

        cmd.Parameters.AddWithValue("@ContactID", Session("ContactID"))

        Dim adContact As New SqlClient.SqlDataAdapter(cmd)

        Try
            cn.Open()
            adContact.Fill(tblReturn)

            For Each dr As GridViewRow In gvCustomer.Rows

                tblReturn.DefaultView.RowFilter = ""

                tblReturn.DefaultView.RowFilter = "CustomerID=" & CType(dr.FindControl("lblCustID"), Label).Text

                If tblReturn.DefaultView.Count > 0 Then
                    CType(dr.FindControl("cbxCustomer"), CheckBox).Checked = True
                End If

            Next

            lbtnCancel.Visible = False
            lbtnDelete.Visible = False
            lbtnSave.Visible = False
            lbtnBack.Visible = True

        Catch ex As Exception

        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub FillGridCustomer()
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Dim dtCustomers As Kernel.CustomerList
        dtCustomers = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn)


        Dim dv As DataView = dtCustomers.DefaultView
        dv.Sort = "Customername"

        With gvCustomer
            .DataSource = dv
            .DataBind()
        End With
    End Sub

    Private Sub gvCustomer_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustomer.RowCommand
        If e.CommandName = "Edit" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvCustomer.Rows(index)

            For Each dr As GridViewRow In gvCustomer.Rows

                dr.BackColor = Drawing.Color.Empty

            Next

            row.BackColor = Drawing.Color.LightGreen

            cblGroups.Items.Clear()

            Dim CtrlLabel As Label
            CtrlLabel = row.FindControl("lblCustID")

            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            Dim dtGroups As New Kernel.GroupList(CtrlLabel.Text, cn, m_User.Customer.AccountingArea)

            If dtGroups.Rows.Count > 0 Then

                cblGroups.DataSource = dtGroups.DefaultView
                cblGroups.DataValueField = "GroupID"
                cblGroups.DataTextField = "GroupName"
                cblGroups.DataBind()

            End If

            Dim dtContact As New DataTable
            Dim SQL As String
            SQL = "SELECT GroupID, CustomerID from ContactGroups where ContactID = @ContactID"



            Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand(SQL, cn)

            cmd.Parameters.AddWithValue("@ContactID", Session("ContactID"))


            Dim adContact As New SqlClient.SqlDataAdapter(cmd)

            adContact.Fill(dtContact)

            If dtContact.Rows.Count > 0 Then

                For Each litem As ListItem In cblGroups.Items

                    dtContact.DefaultView.RowFilter = "GroupID = " & litem.Value

                    If dtContact.DefaultView.Count > 0 Then
                        litem.Selected = True

                    End If

                Next

            End If

        End If
    End Sub

    Private Sub gvCustomer_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCustomer.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("btnCustomer"), Button).Attributes.Add("onmouseover", "this.style.cursor='hand'")
            End If
        Catch Ex As Exception
            'report error... 
            Return
        End Try
    End Sub

    Private Sub gvCustomer_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvCustomer.RowDeleting

    End Sub
  
    Private Sub gvCustomer_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvCustomer.RowEditing

    End Sub

    Protected Sub cblGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cblGroups.SelectedIndexChanged

        Dim cbx As CheckBoxList = CType(sender, CheckBoxList)

        Dim booSelected As Boolean = False
        Dim GroupID As String = ""
        Dim CustomerID As String = ""


        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

        cn.Open()

        Dim cmdTable As SqlClient.SqlCommand


        Try
            Dim SQL As String

            'Ist wenigstens eine Gruppe ausgewählt?
            For Each litem As ListItem In cblGroups.Items

                If litem.Selected = True Then
                    booSelected = True
                    Exit For
                End If
            Next

            For Each dr As GridViewRow In gvCustomer.Rows

                If dr.BackColor = Drawing.Color.LightGreen Then
                    CType(dr.FindControl("cbxCustomer"), CheckBox).Checked = booSelected
                    CustomerID = CType(dr.FindControl("lblCustID"), Label).Text
                    Exit For
                End If

            Next


            SQL = "Delete from ContactGroups where ContactID=@ContactID and CustomerID=@CustomerID"


            cmdTable = New SqlClient.SqlCommand(SQL, cn)
            cmdTable.Parameters.AddWithValue("@ContactID", Session("ContactID"))
            cmdTable.Parameters.AddWithValue("@CustomerID", CustomerID)

            cmdTable.CommandType = CommandType.Text

            cmdTable.ExecuteNonQuery()


            SQL = "Insert Into ContactGroups(ContactID,CustomerID,GroupID)values(@ContactID,@CustomerID,@GroupID)"
            cmdTable.CommandText = SQL
            cmdTable.Parameters.Add("@GroupID", SqlDbType.Int)


            For Each litem As ListItem In cblGroups.Items

                If litem.Selected = True Then

                    GroupID = litem.Value

                    cmdTable.Parameters("@GroupID").Value = GroupID

                    cmdTable.ExecuteNonQuery()

                End If
            Next


        Catch ex As Exception

        Finally
            cn.Close()
        End Try


    End Sub

    Protected Sub lbtnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnBack.Click
        LockEdit(False)

        lbtnCancel.Visible = True
        lbtnSave.Visible = True
        lbtnBack.Visible = False

        If dgSearchResult.Rows.Count = 0 Then
            SearchMode(, True)
        Else
            SearchMode()
        End If

        Session("SetFilter") = Nothing

        Image1.ImageUrl = "images/placeholder.png"
        lblPictureName.Text = ""

    End Sub

    Protected Sub lbtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnCancel.Click

        If dgSearchResult.Rows.Count > 0 Then
            SearchMode()
        Else
            SearchMode(, True)
        End If

        Image1.ImageUrl = "images/placeholder.png"
        lblPictureName.Text = ""


    End Sub

    Protected Sub lbtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnDelete.Click
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try

            Dim info As System.IO.FileInfo

            Dim fnameNew As String = System.Configuration.ConfigurationManager.AppSettings("UploadPathContactsLocal") & "responsible\" & Session("ContactID") & ".jpg"
            info = New System.IO.FileInfo(fnameNew)
            If (info.Exists) Then
                System.IO.File.Delete(fnameNew)
            End If

            lblPictureName.Text = ""

            cn.Open()

            Dim cmdTable As SqlClient.SqlCommand
            cmdTable = New SqlClient.SqlCommand("Delete Contact where ID=@ContactID", cn)
            cmdTable.Parameters.AddWithValue("@ContactID", Session("ContactID"))
            cmdTable.ExecuteNonQuery()

            lbtnCancel.Visible = False
            lbtnDelete.Visible = False
            lbtnBack.Visible = True

            'Neu laden
            LoadContacts()
            lblMessage.Text = "Das Löschen war erfolgreich."
        Catch ex As Exception
            lblError.Text = "Fehler beim Löschen."
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemove.Click
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try

            Dim info As System.IO.FileInfo

            Dim fnameNew As String = System.Configuration.ConfigurationManager.AppSettings("UploadPathContactsLocal") & "responsible\" & Session("ContactID") & ".jpg"
            info = New System.IO.FileInfo(fnameNew)
            If (info.Exists) Then
                System.IO.File.Delete(fnameNew)
            End If

            Image1.ImageUrl = "images/placeholder.png"
            lblPictureName.Text = ""

            Dim cmdTable As SqlClient.SqlCommand
            cmdTable = New SqlClient.SqlCommand("Update Contact set PictureName=@PictureName where ID=@ContactID", cn)
            cmdTable.Parameters.AddWithValue("@PictureName", System.DBNull.Value)
            cmdTable.Parameters.AddWithValue("@ContactID", Session("ContactID"))

            If cn.State <> ConnectionState.Open Then
                cn.Open()
            End If

            cmdTable.ExecuteNonQuery()

        Catch ex As Exception
            lblError.Text = "Fehler beim Löschen. (" & ex.ToString & ")"
        Finally
            cn.Close()
        End Try
    End Sub

    Protected Sub ibtnFilter_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFilter.Click

        If Session("SetFilter") Is Nothing Then
            Session("SetFilter") = True
            FilterGridCustomer()
        Else

            If CBool(Session("SetFilter")) = True Then
                FillGridCustomer()

                SetSelectedCustomer()
                Session("SetFilter") = False

            Else
                Session("SetFilter") = True
                FilterGridCustomer()


            End If
        End If

    End Sub

    Private Sub FilterGridCustomer()

        Dim TempTable As New DataTable

        TempTable.Columns.Add("CustomerID", GetType(System.String))
        TempTable.Columns.Add("CustomerName", GetType(System.String))

        TempTable.AcceptChanges()

        Dim NewRow As DataRow

        For Each dr As GridViewRow In gvCustomer.Rows

            If CType(dr.FindControl("cbxCustomer"), CheckBox).Checked = True Then

                NewRow = TempTable.NewRow

                NewRow("CustomerID") = CType(dr.FindControl("lblCustID"), Label).Text
                NewRow("CustomerName") = CType(dr.FindControl("btnCustomer"), Button).Text

                TempTable.Rows.Add(NewRow)

            End If


        Next



        Dim dv As DataView = TempTable.DefaultView
        dv.Sort = "Customername"

        With gvCustomer
            .DataSource = dv
            .DataBind()
        End With

        For Each dr As GridViewRow In gvCustomer.Rows
            CType(dr.FindControl("cbxCustomer"), CheckBox).Checked = True
        Next


    End Sub

    Protected Sub lbtnBackToSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnBackToSearch.Click

        lbtnBackToSearch.Visible = False
        SearchMode(True)

        Session("SetFilter") = Nothing

    End Sub
End Class