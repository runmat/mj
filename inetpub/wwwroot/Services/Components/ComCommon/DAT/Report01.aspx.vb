Option Strict On
Option Explicit On

Imports CKG.Components.ComCommon.SilverDAT
Imports DAT_BankenLinie_Connector

Namespace DAT
    Public Class Report01
        Inherits Page

        Private _user As Base.Kernel.Security.User
        Private _app As Base.Kernel.Security.App

        Private m_DAT As DATBankenlinieConnector

#Region "Events"

        Protected Overrides Sub OnLoad(e As EventArgs)
            MyBase.OnLoad(e)

            _user = Base.Kernel.Common.Common.GetUser(Me)

            Base.Kernel.Common.Common.FormAuth(Me, _user)
            Base.Kernel.Common.Common.GetAppIDFromQueryString(Me)

            If Session("objDAT") IsNot Nothing Then
                m_DAT = CType(Session("objDAT"), DATBankenlinieConnector)
            Else
                m_DAT = New DATBankenlinieConnector(_user)
                Session("objDAT") = m_DAT
            End If

            lblHead.Text = _user.Applications.Select("AppID = '" & Session("AppID").ToString() & "'")(0)("AppFriendlyName").ToString()
            _app = New Base.Kernel.Security.App(_user)

            If Not IsPostBack Then
                GetVersion()
                GetVehicleTypes()
            End If
        End Sub

        Protected Sub VehicleTypesSelectedIndexChanged(sender As Object, e As EventArgs)
            ResetControls(ddlManufacturers, _
                                ddlBaseModels, _
                                ddlSubModels, _
                                lblEuropaCodeDisplay, _
                                trEngineOptions, _
                                trBodyOptions, _
                                trGearingOptions, _
                                trEquipmentLineOptions, _
                                ddlConstructionYear, _
                                txtMileage, _
                                lbCreate)

            If Not String.IsNullOrEmpty(ddlVehicleTypes.SelectedValue) Then
                GetManufacturers()
            End If

            Result.Visible = False
        End Sub

        Protected Sub ManufacturersSelectedIndexChanged(sender As Object, e As EventArgs)
            ResetControls(ddlBaseModels, _
                                ddlSubModels, _
                                lblEuropaCodeDisplay, _
                                trEngineOptions, _
                                trBodyOptions, _
                                trGearingOptions, _
                                trEquipmentLineOptions, _
                                ddlConstructionYear, _
                                txtMileage, _
                                lbCreate)

            If Not String.IsNullOrEmpty(ddlManufacturers.SelectedValue) Then
                GetBaseModels()
            End If

            Result.Visible = False
        End Sub

        Protected Sub BaseModelsSelectedIndexChanged(sender As Object, e As EventArgs)
            ResetControls(ddlSubModels, _
                                lblEuropaCodeDisplay, _
                                trEngineOptions, _
                                trBodyOptions, _
                                trGearingOptions, _
                                trEquipmentLineOptions, _
                                ddlConstructionYear, _
                                txtMileage, _
                                lbCreate)

            If Not String.IsNullOrEmpty(ddlBaseModels.SelectedValue) Then
                GetSubModels()
            End If

            Result.Visible = False
        End Sub

        Protected Sub SubModelsSelectedIndexChanged(sender As Object, e As EventArgs)
            ResetControls(lblEuropaCodeDisplay, _
                                trEngineOptions, _
                                trBodyOptions, _
                                trGearingOptions, _
                                trEquipmentLineOptions, _
                                ddlConstructionYear, _
                                txtMileage, _
                                lbCreate)

            If Not String.IsNullOrEmpty(ddlSubModels.SelectedValue) Then
                ShowExtendedOptions()
            End If

            Result.Visible = False
        End Sub

        Protected Sub ExtendedOptionsChanged(sender As Object, e As EventArgs)
            If Not String.IsNullOrEmpty(CType(sender, DropDownList).SelectedValue) Then
                ShowExtendedOptions()
            End If
            updatePage()
        End Sub

        Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
            Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Protected Sub SuchenClicked(sender As Object, e As EventArgs)
            If String.IsNullOrEmpty(txtMileage.Text) Then
                lblError.Text = "Bitte Kilometerstand angeben!"
                Exit Sub
            End If
            Dim hep As String = ""
            Dim hvp As String = ""
            lblError.Text += m_DAT.PreiseHolen(lblEuropaCodeDisplay.Text, "", Integer.Parse(txtMileage.Text), Integer.Parse(ddlConstructionYear.SelectedValue), hep, hvp)
            lblDealerPurchasePriceDisplay.Text = hep
            lblDealerSalesPriceDisplay.Text = hvp
            Result.Visible = True
        End Sub

#End Region

#Region "Methods"

        Private Shared Sub ResetControls(ParamArray controls As Control())
            Dim li As ListItem

            For Each control As Control In controls
                Dim wc As WebControl = TryCast(control, WebControl)
                Dim dropdown As DropDownList = TryCast(control, DropDownList)
                Dim txtControl As ITextControl = TryCast(control, ITextControl)
                Dim tr As HtmlTableRow = TryCast(control, HtmlTableRow)

                If dropdown IsNot Nothing Then
                    li = dropdown.Items(0)
                    dropdown.Items.Clear()
                    dropdown.Items.Add(li)
                ElseIf txtControl IsNot Nothing Then
                    txtControl.Text = String.Empty
                ElseIf tr IsNot Nothing Then
                    tr.Visible = False
                    Dim ddl As Control = tr.FindControl(String.Concat("ddl", tr.ID.Substring(2)))
                    ResetControls(ddl)
                End If

                If wc IsNot Nothing Then
                    wc.Enabled = False
                End If
            Next
        End Sub

        Private Sub GetVersion()
            lbVersion.Text = m_DAT.GetDATVersion()
        End Sub

        Private Sub GetVehicleTypes()
            ddlVehicleTypes.DataSource = m_DAT.ddlVehicleTypes_DataTable()
            ddlVehicleTypes.DataBind()
        End Sub

        Private Sub GetManufacturers()
            ddlManufacturers.DataSource = m_DAT.ddlManufacturers_DataTable(Integer.Parse(ddlVehicleTypes.SelectedValue))
            ddlManufacturers.DataBind()
            ddlManufacturers.Enabled = True
        End Sub

        Private Sub GetBaseModels()
            Try
                lblError.Text = lblError.Text.Replace("Es sind leider keine Modelle verfügbar", "")
                ddlBaseModels.DataSource = m_DAT.ddlBaseModels_DataTable(Integer.Parse(ddlVehicleTypes.SelectedValue), Integer.Parse(ddlManufacturers.SelectedValue))
                ddlBaseModels.DataBind()
                ddlBaseModels.Enabled = True
            Catch
                lblError.Text += "Es sind leider keine Modelle verfügbar"
            End Try
        End Sub

        Private Sub GetSubModels()
            ddlSubModels.DataSource = m_DAT.ddlSubModels_DataTable(Integer.Parse(ddlVehicleTypes.SelectedValue), Integer.Parse(ddlManufacturers.SelectedValue), Integer.Parse(ddlBaseModels.SelectedValue))
            ddlSubModels.DataBind()
            ddlSubModels.Enabled = True
        End Sub

        Private Sub GetEngineOptions()
            ddlEngineOptions.DataSource = m_DAT.ddlEngineOptions_DataTable(Integer.Parse(ddlVehicleTypes.SelectedValue), Integer.Parse(ddlManufacturers.SelectedValue), Integer.Parse(ddlBaseModels.SelectedValue), Integer.Parse(ddlSubModels.SelectedValue))
            ddlEngineOptions.DataBind()
            ddlEngineOptions.Enabled = True

            If ddlEngineOptions.Items.Count = 2 Then
                ddlEngineOptions.SelectedIndex = 1
                ExtendedOptionsChanged(ddlEngineOptions, New EventArgs())
            End If
        End Sub

        Private Sub GetBodyOptions()
            Try
                ddlBodyOptions.DataSource = m_DAT.ddlBodyOptions_DataTable(Integer.Parse(ddlVehicleTypes.SelectedValue), Integer.Parse(ddlManufacturers.SelectedValue), Integer.Parse(ddlBaseModels.SelectedValue), Integer.Parse(ddlSubModels.SelectedValue))
                ddlBodyOptions.DataBind()
                ddlBodyOptions.Enabled = True
            Catch
            End Try
            If ddlBodyOptions.Items.Count = 2 Then
                ddlBodyOptions.SelectedIndex = 1
                ExtendedOptionsChanged(ddlBodyOptions, New EventArgs())
            End If
        End Sub

        Private Sub GetGearingOptions()
            If ddlVehicleTypes.SelectedValue <> "3" And ddlVehicleTypes.SelectedValue <> "4" And ddlVehicleTypes.SelectedValue <> "5" Then
                ddlGearingOptions.DataSource = m_DAT.ddlGearingOptions_DataTable(Integer.Parse(ddlVehicleTypes.SelectedValue), Integer.Parse(ddlManufacturers.SelectedValue), Integer.Parse(ddlBaseModels.SelectedValue), Integer.Parse(ddlSubModels.SelectedValue))
                ddlGearingOptions.DataBind()
                ddlGearingOptions.Enabled = True
            End If
            If ddlGearingOptions.Items.Count = 2 Then
                ddlGearingOptions.SelectedIndex = 1
                ExtendedOptionsChanged(ddlGearingOptions, New EventArgs())
            End If
        End Sub

        Private Sub GetEquipmentLineOptions()
            Try
                ddlEquipmentLineOptions.DataSource = m_DAT.ddlEquipmentLineOptions_DataTable(Integer.Parse(ddlVehicleTypes.SelectedValue), Integer.Parse(ddlManufacturers.SelectedValue), Integer.Parse(ddlBaseModels.SelectedValue), Integer.Parse(ddlSubModels.SelectedValue))
                ddlEquipmentLineOptions.DataBind()
                ddlEquipmentLineOptions.Enabled = True
            Catch
            End Try
            If ddlEquipmentLineOptions.Items.Count = 2 Then
                ddlEquipmentLineOptions.SelectedIndex = 1
                ExtendedOptionsChanged(ddlEquipmentLineOptions, New EventArgs())
            End If
        End Sub

        Private Sub CompileDatECode()
            Try
                lblEuropaCodeDisplay.Text = GetDatECode()

                lbCreate.Enabled = True
            Catch ex As DatException
                If ex.ErrorCode = 11006 Then
                    ShowExtendedOptions()
                Else
                    Throw
                End If
            End Try
        End Sub

        Private Sub GetPriceDiscoveryConstructionYears()
            Try
                lblError.Text = lblError.Text.Replace("Es sind leider keine Baujahre verfügbar.", "")
                ResetControls(ddlConstructionYear)
                ddlConstructionYear.DataSource = m_DAT.ddlConstructionYear_DataTable(lblEuropaCodeDisplay.Text)
                ddlConstructionYear.DataBind()
                ddlConstructionYear.Enabled = True
            Catch
                lblError.Text += "Es sind leider keine Baujahre verfügbar."

            End Try
        End Sub

        Private Function GetDatECode() As String
            Dim selectedEngineOptions As String = ddlEngineOptions.SelectedValue
            Dim selectedBodyOptions As String = ddlBodyOptions.SelectedValue
            Dim selectedGearingOptions As String = ddlGearingOptions.SelectedValue
            Dim selectedEquipmentLineOptions As String = ddlEquipmentLineOptions.SelectedValue

            Dim additionalOptions As New List(Of Integer)()

            If Not String.IsNullOrEmpty(selectedEngineOptions) Then
                additionalOptions.Add(Convert.ToInt32(selectedEngineOptions))
            End If

            If Not String.IsNullOrEmpty(selectedBodyOptions) And ddlVehicleTypes.SelectedValue <> "4" And ddlVehicleTypes.SelectedValue <> "5" Then
                additionalOptions.Add(Convert.ToInt32(selectedBodyOptions))
            End If

            If Not String.IsNullOrEmpty(selectedGearingOptions) And ddlVehicleTypes.SelectedValue <> "3" And ddlVehicleTypes.SelectedValue <> "4" And ddlVehicleTypes.SelectedValue <> "5" Then
                additionalOptions.Add(Convert.ToInt32(selectedGearingOptions))
            End If

            If Not String.IsNullOrEmpty(selectedEquipmentLineOptions) Then
                additionalOptions.Add(Convert.ToInt32(selectedEquipmentLineOptions))
            End If

            Dim ErrorMessage As String = ""

            Dim year As Integer = Date.Now.Year
            Integer.TryParse(ddlConstructionYear.SelectedValue, year)

            Dim mileage As Integer = 0
            Integer.TryParse(lblMileage.Text, mileage)
            
            Dim VehicleTypes As Integer = 0
            Integer.TryParse(ddlVehicleTypes.SelectedValue, VehicleTypes)
            
            Dim Manufacturers As Integer = 0
            Integer.TryParse(ddlManufacturers.SelectedValue, Manufacturers)

            Dim BaseModels As Integer = 0
            Integer.TryParse(ddlBaseModels.SelectedValue, BaseModels)

            Dim SubModels As Integer = 0
            Integer.TryParse(ddlSubModels.SelectedValue, SubModels)

            Dim bMx As String = Get_Daten.GetECode(m_DAT.DATcustomerLogin, m_DAT.DATcustomerNumber, m_DAT.DATcustomerSignature, m_DAT.DATinterfacePartnerNumber, m_DAT.DATinterfacePartnerSignature, mileage, year, "", ErrorMessage, additionalOptions.ToArray(), VehicleTypes, Manufacturers, BaseModels, SubModels)

            Return bMx

        End Function

        Private Sub ShowExtendedOptions()
            If Not trEngineOptions.Visible Then
                If Not String.IsNullOrEmpty(ddlSubModels.SelectedValue) Then
                    trEngineOptions.Visible = True
                    GetEngineOptions()
                End If
            ElseIf Not trBodyOptions.Visible Then
                If Not String.IsNullOrEmpty(ddlEngineOptions.SelectedValue) Then
                    trBodyOptions.Visible = True
                    GetBodyOptions()
                End If
            ElseIf Not trGearingOptions.Visible Then
                If Not String.IsNullOrEmpty(ddlBodyOptions.SelectedValue) Then
                    trGearingOptions.Visible = True
                    GetGearingOptions()
                End If
            ElseIf Not trEquipmentLineOptions.Visible Then
                If Not String.IsNullOrEmpty(ddlGearingOptions.SelectedValue) Then
                    trEquipmentLineOptions.Visible = True
                    GetEquipmentLineOptions()
                End If
            End If

        End Sub

        Private Sub updatePage()
            CompileDatECode()
            If Not String.IsNullOrEmpty(lblEuropaCodeDisplay.Text) Then
                GetPriceDiscoveryConstructionYears()
                ddlConstructionYear.Enabled = True
                txtMileage.Enabled = True
            End If 
        End Sub

#End Region

    End Class

End Namespace