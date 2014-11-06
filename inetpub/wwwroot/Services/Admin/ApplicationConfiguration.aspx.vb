Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class ApplicationConfiguration
    Inherits Page

#Region "Properties"

    Private Property Refferer() As String
        Get
            Return ViewState.Item("refferer").ToString
        End Get
        Set(ByVal value As String)
            ViewState.Item("refferer") = value
        End Set
    End Property

#End Region

#Region " Membervariables "

    Private m_User As User
    Private m_App As App
    Protected WithEvents GridNavigation1 As CKG.Services.GridNavigation

#End Region

#Region " Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        lblHead.Text = "Konfigurationseinstellungen für"
        AdminAuth(Me, m_User, AdminLevel.Master)
        GridNavigation1.setGridElment(dgSearchResult)

        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                If Request.UrlReferrer IsNot Nothing Then
                    Refferer = Request.UrlReferrer.ToString()
                Else
                    Refferer = "Selection.aspx"
                End If

                If m_User.Customer.AccountingArea = -1 Then
                    lnkAppManagement.Visible = True
                    lbtnNew.Enabled = True
                End If

                hiddenAppID.Value = CStr(Request.QueryString("AppID"))

                If String.IsNullOrEmpty(hiddenAppID.Value) Then
                    lbtnNew.Visible = False
                Else
                    Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
                        cn.Open()
                        FillCustomerDropdown(cn)
                        FillGroupDropdown(cn)
                        cn.Close()
                    End Using

                    FillForm()
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString()
            m_App.WriteErrorText(1, m_User.UserName, "ApplicationConfiguration", "PageLoad", lblError.Text)
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        If Input.Visible Then
            lnkBack.Visible = False
        Else
            lnkBack.Visible = True
            If ddlCustomer.SelectedValue = "1" And ddlGroup.SelectedValue = "0" Then
                For Each item As GridViewRow In dgSearchResult.Rows
                    Dim cell As TableCell = item.Cells(item.Cells.Count - 1)
                    For Each control As Control In cell.Controls
                        If TypeOf control Is LinkButton Then
                            Dim lnkButton As LinkButton = CType(control, LinkButton)
                            lnkButton.Enabled = False
                        End If
                    Next
                Next
            End If
        End If
    End Sub

    Private Sub ddlCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCustomer.SelectedIndexChanged
        'Gruppenauswahl aktualisieren
        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()
            FillGroupDropdown(cn)
            cn.Close()
        End Using

        FillForm()
    End Sub

    Private Sub ddlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGroup.SelectedIndexChanged
        FillForm()
    End Sub

    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        dgSearchResult.PageIndex = PageIndex
        FillDataGrid()
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillDataGrid()
    End Sub

    Private Sub dgSearchResult_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles dgSearchResult.Sorting
        Dim strSort As String = e.SortExpression
        If Not ViewState("ResultSort") Is Nothing AndAlso ViewState("ResultSort").ToString = strSort Then
            strSort &= " DESC"
        End If
        ViewState("ResultSort") = strSort
        FillDataGrid(strSort)
    End Sub

    Private Sub dgSearchResult_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles dgSearchResult.RowCommand

        If e.CommandName <> "Sort" Then

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = dgSearchResult.Rows(index)
            Dim CtrlConfigID As Label = row.Cells(0).FindControl("lblConfigID")

            Select Case e.CommandName

                Case "Create"
                    hiddenConfigIDSave.Value = CtrlConfigID.Text
                    Dim CtrlConfigType As Label = row.Cells(1).FindControl("lblConfigType")
                    Dim CtrlConfigKey As Label = row.Cells(2).FindControl("lblConfigKey")
                    EditCreateMode(CtrlConfigType.Text, CtrlConfigKey.Text)

                Case "Edit"
                    EditEditMode(CInt(CtrlConfigID.Text))

                Case "Del"
                    EditDeleteMode(CInt(CtrlConfigID.Text))

            End Select

            dgSearchResult.SelectedIndex = row.RowIndex

        End If

    End Sub

    Private Sub dgSearchResult_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles dgSearchResult.RowEditing
        'Dummy
    End Sub

    Private Sub rbConfigType_CheckedChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles rbBool.CheckedChanged, rbString.CheckedChanged
        If rbBool.Checked Then
            cbxBoolValue.Visible = True
            txtStringValue.Text = ""
            txtStringValue.Visible = False
        Else
            cbxBoolValue.Checked = False
            cbxBoolValue.Visible = False
            txtStringValue.Visible = True
        End If
    End Sub

    Private Sub lnkBack_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lnkBack.Click
        Response.Redirect(Refferer)
    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnCancel.Click
        Search(False, , True)
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnNew.Click
        SearchMode(False)
        ClearEdit(True)
    End Sub

    Private Sub lbtnSave_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnSave.Click
        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Try
                Dim intConfigID As Integer = CInt(hiddenConfigID.Value)
                Dim strLogMsg As String = "Anwendungseinstellungen anlegen"
                If Not (intConfigID = -1) Then
                    strLogMsg = "Anwendungseinstellungen ändern"
                End If

                Dim strConfigType As String
                If rbBool.Checked Then
                    strConfigType = "bool"
                Else
                    strConfigType = "string"
                End If

                Dim strConfigValue As String
                If cbxBoolValue.Visible Then
                    If cbxBoolValue.Checked Then
                        strConfigValue = "true"
                    Else
                        strConfigValue = "false"
                    End If
                Else
                    strConfigValue = txtStringValue.Text
                End If

                Dim _AppConfig As New Kernel.AppConfiguration(intConfigID, _
                                                    CInt(hiddenAppID.Value), _
                                                    strConfigType, _
                                                    txtConfigKey.Text, _
                                                    strConfigValue, _
                                                    txtDescription.Text, _
                                                    CInt(ddlCustomer.SelectedValue), _
                                                    CInt(ddlGroup.SelectedValue))
                _AppConfig.Save(cn)
                Dim tblLogParameter As DataTable = SetNewLogParameters()
                Log(_AppConfig.ConfigID.ToString, strLogMsg, tblLogParameter)
                Search(False, True, True, , True)
                lblMessage.Text = "Die Änderungen wurden gespeichert."
            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "AppConfiguration", "lbtnSave_Click", ex.ToString)

                lblError.Text = ex.Message
                If Not ex.InnerException Is Nothing Then
                    lblError.Text &= ": " & ex.InnerException.Message
                End If
                Log(hiddenConfigID.Value, lblError.Text, New DataTable(), "ERR")
            End Try

            cn.Close()
        End Using
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles lbtnDelete.Click
        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            Try

                Dim _AppConfig As New Kernel.AppConfiguration(CInt(hiddenConfigID.Value), cn)
                Dim tblLogParameter As DataTable = SetOldLogParameters(CInt(hiddenConfigID.Value))
                _AppConfig.Delete(cn)
                Log(_AppConfig.ConfigID.ToString, "Anwendungseinstellungen löschen", tblLogParameter)
                Search(False, True, True, True, True)
                lblMessage.Text = "Die Anwendungseinstellung wurde gelöscht."
            Catch ex As Exception
                m_App.WriteErrorText(1, m_User.UserName, "AppConfiguration", "lbtnDelete_Click", ex.ToString)

                lblError.Text = ex.Message
                If Not ex.InnerException Is Nothing Then
                    lblError.Text &= ": " & ex.InnerException.Message
                End If
                Log(hiddenConfigID.Value, lblError.Text, New DataTable(), "ERR")
            End Try

            cn.Close()
        End Using
    End Sub

#End Region

#Region " Data and Function "

    Private Sub FillCustomerDropdown(ByVal cn As SqlClient.SqlConnection)

        Dim _CustomerList As New Kernel.CustomerList(m_User.Customer.AccountingArea, cn)
        Dim vwCustomer As DataView = _CustomerList.DefaultView
        vwCustomer.Sort = "Customername"
        ddlCustomer.DataSource = vwCustomer
        ddlCustomer.DataValueField = "CustomerID"
        ddlCustomer.DataTextField = "Customername"
        ddlCustomer.DataBind()
        'ggf. Firma 1 ergänzen (enthält die Defaultwerte)
        If ddlCustomer.Items.FindByValue("1") Is Nothing Then
            ddlCustomer.Items.Add(New ListItem("Firma1", "1"))
        End If
        ddlCustomer.SelectedValue = "1"

    End Sub

    Private Sub FillGroupDropdown(ByVal cn As SqlClient.SqlConnection)

        Dim _GroupList As New Kernel.GroupList(CInt(ddlCustomer.SelectedValue), cn, m_User.Customer.AccountingArea, True)
        Dim vwGroup As DataView = _GroupList.DefaultView
        ddlGroup.DataSource = vwGroup
        ddlGroup.DataValueField = "GroupID"
        ddlGroup.DataTextField = "GroupName"
        ddlGroup.DataBind()
        ddlGroup.SelectedValue = "0"

    End Sub

    Private Sub EnableRadioButtons(ByVal blnValue As Boolean)
        rbBool.Enabled = blnValue
        rbString.Enabled = blnValue
    End Sub

    Private Sub FillForm()
        Input.Visible = False
        Result.Visible = False
        Search(False, True, True, True, True)
    End Sub

    Private Sub FillDataGrid()
        Dim strSort As String = "ConfigKey"
        If Not ViewState("ResultSort") Is Nothing Then
            strSort = ViewState("ResultSort").ToString
        End If
        FillDataGrid(strSort)
    End Sub

    Private Sub FillDataGrid(ByVal strSort As String)
        Result.Visible = True
        Dim dvAppConfiguration As DataView

        If Session("myAppConfigurationView") IsNot Nothing Then
            dvAppConfiguration = CType(Session("myAppConfigurationView"), DataView)
        Else
            Dim dtAppConfiguration As New Kernel.AppConfigurationList(CInt(hiddenAppID.Value), CInt(ddlCustomer.SelectedValue), CInt(ddlGroup.SelectedValue), m_User.App.Connectionstring)
            dvAppConfiguration = dtAppConfiguration.DefaultView
            Session("myAppConfigurationView") = dvAppConfiguration
        End If
        dvAppConfiguration.Sort = strSort

        With dgSearchResult
            .DataSource = dvAppConfiguration
            .DataBind()
        End With
        If Not m_User.Customer.AccountingArea = -1 Then
            If ddlCustomer.SelectedValue = "1" Then
                'alle Ändern Buttons deaktivieren wenn es kein SuperAdmin und es Firma1, also default ist
                dgSearchResult.Columns(6).Visible = False
            End If
        End If
        For Each tmpItem As GridViewRow In dgSearchResult.Rows
            Dim lbl As Label = CType(tmpItem.FindControl("lblConfigID"), Label)
            tmpItem.Cells(5).Visible = (lbl.Text = "-1")
            tmpItem.Cells(6).Visible = (lbl.Text <> "-1")
        Next
    End Sub

    Private Sub EditCreateMode(ByVal strConfigType As String, ByVal strConfigKey As String)
        Dim strCustomerID As String = ddlCustomer.SelectedValue
        Dim strGroupID As String = ddlGroup.SelectedValue
        If Not FillEdit(CInt(hiddenAppID.Value), strConfigType, strConfigKey, 1, 0) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            hiddenConfigID.Value = -1
            ddlCustomer.SelectedValue = strCustomerID
            ddlGroup.SelectedValue = strGroupID
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditEditMode(ByVal intConfigID As Integer)
        If Not FillEdit(intConfigID) Then
            LockEdit(True)
            lbtnSave.Enabled = False
        Else
            lbtnSave.Enabled = True
        End If
        lbtnCancel.Text = "Verwerfen"
    End Sub

    Private Sub EditDeleteMode(ByVal intConfigID As Integer)
        If Not FillEdit(intConfigID) Then
            lbtnDelete.Enabled = False
        Else
            lblMessage.Text = "Möchten Sie die Einstellung wirklich löschen?"
            lbtnDelete.Enabled = True
        End If
        LockEdit(True)
        lbtnCancel.Text = "Abbrechen"
        lbtnSave.Visible = False
        lbtnDelete.Visible = True
    End Sub

    Private Function FillEdit(ByVal intAppID As Integer, ByVal strConfigType As String, ByVal strConfigKey As String, ByVal intCustomerID As Integer, ByVal intGroupID As Integer) As Boolean
        SearchMode(False)

        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            lblError.Text = ""

            Try
                Dim _AppConfig As New Kernel.AppConfiguration(intAppID, strConfigType, strConfigKey, intCustomerID, intGroupID, cn)
                SetInput(_AppConfig)
            Catch ex As Exception
                Dim intTempCustomerID As Integer = CInt(ddlCustomer.SelectedValue)
                Dim intTempGroupID As Integer = CInt(ddlGroup.SelectedValue)
                FillEdit(CInt(hiddenConfigID.Value))
                lblError.Text = "Einstellung existiert noch nicht."

                hiddenConfigID.Value = "-1"
                ddlCustomer.SelectedValue = intTempCustomerID.ToString()
                ddlGroup.SelectedValue = intTempGroupID.ToString()
                txtConfigKey.Text = ""
                rbBool.Checked = True
                rbString.Checked = False
                cbxBoolValue.Visible = True
                cbxBoolValue.Checked = False
                txtStringValue.Visible = False
                txtDescription.Text = ""
            End Try

            cn.Close()
        End Using

        Return True
    End Function

    Private Function FillEdit(ByVal intConfigID As Integer, Optional ByVal blnAvoidExceptionLoop As Boolean = False) As Boolean
        SearchMode(False)

        Using cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
            cn.Open()

            lblError.Text = ""

            Try
                Dim _AppConfig As New Kernel.AppConfiguration(intConfigID, cn)
                SetInput(_AppConfig)
            Catch ex As Exception
                Dim intTempCustomerID As Integer = CInt(ddlCustomer.SelectedValue)
                Dim intTempGroupID As Integer = CInt(ddlGroup.SelectedValue)
                'ggf. Stack-Overflow durch ExceptionLoop verhindern
                If Not blnAvoidExceptionLoop Then
                    FillEdit(CInt(hiddenConfigIDSave.Value), True)
                End If
                lblError.Text = "Einstellung existiert noch nicht."

                hiddenConfigID.Value = "-1"
                ddlCustomer.SelectedValue = intTempCustomerID.ToString()
                ddlGroup.SelectedValue = intTempGroupID.ToString()
                txtConfigKey.Text = ""
                rbBool.Checked = True
                rbString.Checked = False
                cbxBoolValue.Visible = True
                cbxBoolValue.Checked = False
                txtStringValue.Visible = False
                txtDescription.Text = ""
            End Try

            cn.Close()
        End Using

        Return True
    End Function

    Private Sub SetInput(ByVal appConfig As Kernel.AppConfiguration)
        hiddenConfigID.Value = appConfig.ConfigID.ToString()
        lblConfigKey.Text = appConfig.ConfigKey
        txtConfigKey.Text = appConfig.ConfigKey

        ddlCustomer.SelectedValue = appConfig.CustomerID
        ddlGroup.SelectedValue = appConfig.GroupID

        If appConfig.ConfigType.ToLower() = "bool" Then
            rbBool.Checked = True
            rbString.Checked = False
            cbxBoolValue.Visible = True
            cbxBoolValue.Checked = (appConfig.ConfigValue.ToLower() = "true")
            txtStringValue.Visible = False
        Else
            rbString.Checked = True
            rbBool.Checked = False
            txtStringValue.Visible = True
            txtStringValue.Text = appConfig.ConfigValue
            cbxBoolValue.Visible = False
        End If

        txtDescription.Text = appConfig.Description
    End Sub

    Private Sub ClearEdit(ByVal blnClearDdlSelection As Boolean)
        hiddenConfigID.Value = "-1"
        hiddenConfigIDSave.Value = "-1"
        lblConfigKey.Text = ""
        txtConfigKey.Text = ""
        rbBool.Checked = True
        rbString.Checked = False
        cbxBoolValue.Visible = True
        cbxBoolValue.Checked = False
        txtStringValue.Visible = False
        txtDescription.Text = ""

        lbtnSave.Visible = True
        lbtnDelete.Visible = False

        If blnClearDdlSelection Then
            ddlCustomer.SelectedValue = "1"
            ddlGroup.SelectedValue = "0"
        End If

        LockEdit(False)
    End Sub

    Private Sub LockEdit(ByVal blnLock As Boolean)
        Dim strBackColor As String = "White"
        If blnLock Then
            strBackColor = "LightGray"
        End If

        ddlCustomer.Enabled = Not blnLock
        ddlCustomer.BackColor = Drawing.Color.FromName(strBackColor)
        ddlGroup.Enabled = Not blnLock
        ddlGroup.BackColor = Drawing.Color.FromName(strBackColor)
        cbxBoolValue.Enabled = Not blnLock
        cbxBoolValue.BackColor = Drawing.Color.FromName(strBackColor)
        txtStringValue.Enabled = Not blnLock
        txtStringValue.BackColor = Drawing.Color.FromName(strBackColor)

        'Einstellungstyp, -name und -beschreibung nur für Firma 1 und "alle Gruppen" änderbar
        If blnLock OrElse Not ddlCustomer.SelectedValue = "1" OrElse Not ddlGroup.SelectedValue = "0" Then
            rbBool.Enabled = False
            rbBool.BackColor = Drawing.Color.FromName("LightGray")
            rbString.Enabled = False
            rbString.BackColor = Drawing.Color.FromName("LightGray")
            txtConfigKey.Enabled = False
            txtConfigKey.BackColor = Drawing.Color.FromName("LightGray")
            txtDescription.Enabled = False
            txtDescription.BackColor = Drawing.Color.FromName("LightGray")
        Else
            rbBool.Enabled = True
            rbBool.BackColor = Drawing.Color.FromName("White")
            rbString.Enabled = True
            rbString.BackColor = Drawing.Color.FromName("White")
            txtConfigKey.Enabled = True
            txtConfigKey.BackColor = Drawing.Color.FromName("White")
            txtDescription.Enabled = True
            txtDescription.BackColor = Drawing.Color.FromName("White")
        End If
    End Sub

    Private Sub SearchMode(Optional ByVal blnSearchMode As Boolean = True)
        Input.Visible = Not blnSearchMode
        ddlCustomer.Enabled = Not Input.Visible
        ddlGroup.Enabled = Not Input.Visible
        Result.Visible = blnSearchMode
        lbtnSave.Visible = Not blnSearchMode
        lbtnCancel.Visible = Not blnSearchMode
        lbtnNew.Visible = blnSearchMode
    End Sub

    Private Sub Search(ByVal blnClearDdlSelection As Boolean, Optional ByVal blnRefillDataGrid As Boolean = False, Optional ByVal blnResetSelectedIndex As Boolean = False, Optional ByVal blnResetPageIndex As Boolean = False, Optional ByVal blnClearCache As Boolean = False)
        'bei Initalload sind Parameter False,True,True,True
        ClearEdit(blnClearDdlSelection)
        If blnClearCache Then
            Session.Remove("myAppConfigurationView")
        End If
        If blnResetSelectedIndex Then dgSearchResult.SelectedIndex = -1
        If blnResetPageIndex Then dgSearchResult.PageIndex = 0
        SearchMode()
        If blnRefillDataGrid Then FillDataGrid()
    End Sub

    Private Sub Log(ByVal strIdentification As String, ByVal strDescription As String, ByVal tblParameters As DataTable, Optional ByVal strCategory As String = "APP")
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_User.App.Connectionstring, m_User.App.SaveLogAccessSAP, m_User.App.LogLevel)

        Dim strUserName As String = m_User.UserName
        Dim strSessionID As String = Session.SessionID
        Dim intSource As Integer = 0
        Dim strTask As String = "Admin - Anwendungskonfiguration"
        Dim strCustomerName As String = m_User.CustomerName
        Dim blnIsTestUser As Boolean = m_User.IsTestUser
        Dim intSeverity As Integer = 0

        logApp.WriteEntry(strCategory, strUserName, strSessionID, intSource, strTask, strIdentification, strDescription, strCustomerName, m_User.Customer.CustomerId, blnIsTestUser, intSeverity, tblParameters)
    End Sub

    Private Function SetOldLogParameters(ByVal intConfigID As Int32) As DataTable
        Dim cn As SqlClient.SqlConnection
        cn = New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try

            cn.Open()
            Dim _AppConfig As New Kernel.AppConfiguration(intConfigID, cn)

            Dim tblPar = CreateLogTableStructure()

            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Alt"
                .Rows(.Rows.Count - 1)("ConfigID") = _AppConfig.ConfigID
                .Rows(.Rows.Count - 1)("AppID") = _AppConfig.AppID
                .Rows(.Rows.Count - 1)("CustomerID") = _AppConfig.CustomerID
                .Rows(.Rows.Count - 1)("GroupID") = _AppConfig.GroupID
                .Rows(.Rows.Count - 1)("ConfigType") = _AppConfig.ConfigType
                .Rows(.Rows.Count - 1)("ConfigKey") = _AppConfig.ConfigKey
                .Rows(.Rows.Count - 1)("ConfigValue") = _AppConfig.ConfigValue
                .Rows(.Rows.Count - 1)("Description") = _AppConfig.Description
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AppConfiguration", "SetOldLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
            Return dt
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Function

    Private Function SetNewLogParameters() As DataTable

        Try
            Dim tblPar = CreateLogTableStructure()

            With tblPar
                .Rows.Add(.NewRow)
                .Rows(.Rows.Count - 1)("Status") = "Neu"
                .Rows(.Rows.Count - 1)("ConfigID") = CInt(hiddenConfigID.Value)
                .Rows(.Rows.Count - 1)("AppID") = CInt(hiddenAppID.Value)
                .Rows(.Rows.Count - 1)("CustomerID") = CInt(ddlCustomer.SelectedValue)
                .Rows(.Rows.Count - 1)("GroupID") = CInt(ddlGroup.SelectedValue)
                If rbBool.Checked Then
                    .Rows(.Rows.Count - 1)("ConfigType") = "bool"
                Else
                    .Rows(.Rows.Count - 1)("ConfigType") = "string"
                End If
                .Rows(.Rows.Count - 1)("ConfigKey") = txtConfigKey.Text
                If cbxBoolValue.Visible Then
                    If cbxBoolValue.Checked Then
                        .Rows(.Rows.Count - 1)("ConfigValue") = "true"
                    Else
                        .Rows(.Rows.Count - 1)("ConfigValue") = "false"
                    End If
                Else
                    .Rows(.Rows.Count - 1)("ConfigValue") = txtStringValue.Text
                End If
                .Rows(.Rows.Count - 1)("Description") = txtDescription.Text
            End With
            Return tblPar
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AppConfiguration", "SetNewLogParameters", ex.ToString)

            Dim dt As New DataTable()
            dt.Columns.Add("Fehler beim Erstellen der Log-Parameter", Type.GetType("System.String"))
            dt.Rows.Add(dt.NewRow)
            Dim str As String = ex.Message
            If Not ex.InnerException Is Nothing Then
                str &= ": " & ex.InnerException.Message
            End If
            dt.Rows(0)("Fehler beim Erstellen der Log-Parameter") = str
            Return dt
        End Try
    End Function

    Private Function CreateLogTableStructure() As DataTable
        Dim tblPar As New DataTable()
        With tblPar
            .Columns.Add("Status", Type.GetType("System.String"))
            .Columns.Add("ConfigID", Type.GetType("System.Integer"))
            .Columns.Add("AppID", Type.GetType("System.Integer"))
            .Columns.Add("CustomerID", Type.GetType("System.Integer"))
            .Columns.Add("GroupID", Type.GetType("System.Integer"))
            .Columns.Add("ConfigType", Type.GetType("System.String"))
            .Columns.Add("ConfigKey", Type.GetType("System.String"))
            .Columns.Add("ConfigValue", Type.GetType("System.String"))
            .Columns.Add("Description", Type.GetType("System.String"))
        End With
        Return tblPar
    End Function

#End Region

End Class