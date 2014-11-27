Imports DAT_BankenLinie_Connector

Public Class DATBankenlinieConnector

    Private customerNumber As String = ""
    Private customerLogin As String = ""
    Private customerSignature As String = ""
    Private interfacePartnerNumber As String = ""
    Private interfacePartnerSignature As String = ""

    Public ReadOnly Property DATcustomerNumber() As String
        Get
            Return customerNumber
        End Get
    End Property
    Public ReadOnly Property DATcustomerLogin() As String
        Get
            Return customerLogin
        End Get
    End Property
    Public ReadOnly Property DATcustomerSignature() As String
        Get
            Return customerSignature
        End Get
    End Property
    Public ReadOnly Property DATinterfacePartnerNumber() As String
        Get
            Return interfacePartnerNumber
        End Get
    End Property
    Public ReadOnly Property DATinterfacePartnerSignature() As String
        Get
            Return interfacePartnerSignature
        End Get
    End Property

    Public Sub New(ByVal usr As Base.Kernel.Security.User)
        GetSDCredentials(usr)
    End Sub

    Private Sub GetSDCredentials(ByVal user As Base.Kernel.Security.User)
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        cn.Open()
        Dim cmdSDCredentials As New SqlClient.SqlCommand("SELECT Kundennummer, LoginNameBankenLinie, SignaturBankenLinie, Signatur2BankenLinie FROM CustomerDAT WHERE CustomerID=@CustomerID", cn)
        cmdSDCredentials.Parameters.AddWithValue("@CustomerID", user.Customer.CustomerId)

        Using dr As SqlClient.SqlDataReader = cmdSDCredentials.ExecuteReader()
            If dr.HasRows Then
                dr.Read()
                customerNumber = dr.GetString(0)
                interfacePartnerNumber = customerNumber
                customerLogin = dr.GetString(1)
                customerSignature = dr.GetString(2)
                interfacePartnerSignature = dr.GetString(3)
            End If
        End Using
        cn.Close()
    End Sub

    Public Function GetDATVersion() As String
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.basicSelectionRequest = New de.dat.gold.basicSelectionRequest()
        vs.sessionID = sessionID
        vs.restriction = de.dat.gold.releaseRestriction.ALL

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        Dim version As String = viSelection.getVersion(vs)

        vi.doLogout(False, False)

        Return version

    End Function

    Public Function ddlVehicleTypes_DataTable() As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.basicSelectionRequest = New de.dat.gold.basicSelectionRequest()
        vs.sessionID = sessionID
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getVehicleTypes(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function ddlManufacturers_DataTable(ByVal vehicleType As Integer) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.manufacturerSelectionRequest = New de.dat.gold.manufacturerSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getManufacturers(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function ddlErstzulassungMonat_DataTable() As DataTable
        Dim tblMonate As New DataTable
        tblMonate.Columns.Add("key", GetType(String))
        tblMonate.Columns.Add("value", GetType(String))

        For i As Integer = 1 To 12
            Dim newRow As DataRow = tblMonate.NewRow()
            newRow("key") = i.ToString()
            newRow("value") = i.ToString()
            tblMonate.Rows.Add(newRow)
        Next

        Return tblMonate
    End Function

    Public Function ddlErstzulassungJahr_DataTable() As DataTable
        Dim tblJahre As New DataTable
        tblJahre.Columns.Add("key", GetType(String))
        tblJahre.Columns.Add("value", GetType(String))

        For i As Integer = 1970 To DateTime.Today.Year
            Dim newRow As DataRow = tblJahre.NewRow()
            newRow("key") = i.ToString()
            newRow("value") = i.ToString()
            tblJahre.Rows.Add(newRow)
        Next

        Return tblJahre
    End Function

    Public Function ddlBaseModels_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.baseModelSelectionRequest = New de.dat.gold.baseModelSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getBaseModels(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function ddlSubModels_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subModelSelectionRequest = New de.dat.gold.subModelSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getSubModels(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function ddlEngineOptions_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getEngineOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function ddlBodyOptions_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getCarBodyOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function ddlGearingOptions_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getGearingOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function ddlEquipmentLineOptions_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getEquipmentLineOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    'Transporter & LKW
    Public Function ddlWheelbase_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getWheelBaseOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    'Transporter 
    Public Function ddlTypeOfDrive_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getTypeOfDriveOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    'LKW 
    Public Function ddlConstructionOptions_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getConstructionOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    'LKW 
    Public Function ddlNumberOfAxleOptions_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getNumberOfAxleOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    'LKW 
    Public Function ddlDriversCabOptions_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getDriversCabOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    'LKW 
    Public Function ddlGrossVehicleWeightOptions_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getGrossVehicleWeightOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    'LKW 
    Public Function ddlSuspensionOptions_DataTable(ByVal vehicleType As Integer, ByVal manufacturer As Integer, ByVal baseModel As Integer, ByVal subModel As Integer, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.subTypeVariantEquipmentSelectionRequest = New de.dat.gold.subTypeVariantEquipmentSelectionRequest()
        vs.sessionID = sessionID
        vs.vehicleType = vehicleType
        vs.manufacturer = manufacturer
        vs.baseModel = baseModel
        vs.subModel = subModel
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vs.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vs.constructionTimeToSpecified = True
        End If

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As de.dat.gold.integerStringPair = viSelection.getSuspensionOptions(vs)
        For Each bm As de.dat.gold.integerStringPair In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.key
            table1_Row("Value") = bm.value
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function ddlContainer_DataTable(ByVal europaCode As String, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim localePeriod As de.dat.gold.locale = New de.dat.gold.locale()
        localePeriod.country = "DE"
        localePeriod.datCountryIndicator = "DE"
        localePeriod.language = "de"

        Dim vsContainer As de.dat.gold.priceFocusSelectionRequest = New de.dat.gold.priceFocusSelectionRequest()
        vsContainer.sessionID = sessionID
        vsContainer.datECode = europaCode
        vsContainer.locale = localePeriod
        vsContainer.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        If ezJahr > 0 Then
            vsContainer.constructionTimeTo = CalculateConstructionTime(ezMonat, ezJahr)
            vsContainer.constructionTimeToSpecified = True
        End If

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)
        Dim table1_Row As DataRow

        Try
            Dim bMx() As de.dat.gold.stringStringPair = viSelection.getPriceFocusCases(vsContainer)
            For Each bm As de.dat.gold.stringStringPair In bMx
                table1_Row = table1.NewRow()
                table1_Row("Key") = bm.key
                table1_Row("Value") = bm.value
                table1.Rows.Add(table1_Row)
            Next
        Catch
        End Try

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function ddlConstructionYear_DataTable(ByVal europaCode As String) As DataTable
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim vs As de.dat.gold.priceFocusSelectionRequest = New de.dat.gold.priceFocusSelectionRequest()
        vs.sessionID = sessionID
        vs.datECode = europaCode
        vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL

        Dim locale As de.dat.gold.locale = New de.dat.gold.locale()
        locale.country = "DE"
        locale.datCountryIndicator = "DE"
        locale.language = "de"
        vs.locale = locale

        'Fill Datatable Models
        Dim table1 As DataTable = New DataTable()

        Dim table1HEP As DataColumn = New DataColumn()
        Dim table1HVP As DataColumn = New DataColumn()
        table1HEP.Caption = "Key"
        table1HEP.ColumnName = "Key"
        table1HVP.Caption = "Value"
        table1HVP.ColumnName = "Value"

        table1.Columns.Add(table1HEP)
        table1.Columns.Add(table1HVP)

        Dim table1_Row As DataRow

        Dim bMx() As Integer = viSelection.getPriceFocusConstructionYears(vs)
        For Each bm As Integer In bMx
            table1_Row = table1.NewRow()
            table1_Row("Key") = bm.ToString()
            table1_Row("Value") = bm.ToString()
            table1.Rows.Add(table1_Row)
        Next

        vi.doLogout(False, False)

        Return table1

    End Function

    Public Function GetConstructionTimeMin(ByVal europaCode As String, ByVal container As String) As Integer
        Dim vi As New de.dat.gold.authentication.Authentication()
        Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

        Dim viSelection As New de.dat.gold.VehicleSelectionService()
        viSelection.CookieContainer = vi.CookieContainer

        Dim localePeriod As de.dat.gold.locale = New de.dat.gold.locale()
        localePeriod.country = "DE"
        localePeriod.datCountryIndicator = "DE"
        localePeriod.language = "de"

        Dim vsPeriod As de.dat.gold.constructionPeriodSelectionRequest = New de.dat.gold.constructionPeriodSelectionRequest()
        vsPeriod.sessionID = sessionID
        vsPeriod.datECode = europaCode
        vsPeriod.restriction = de.dat.gold.releaseRestriction.APPRAISAL
        vsPeriod.locale = localePeriod
        If Not String.IsNullOrEmpty(container) Then
            vsPeriod.container = container
        End If

        Dim ConstPeriod As de.dat.gold.constructionTimePeriod = viSelection.getConstructionPeriods(vsPeriod)

        vi.doLogout(False, False)

        Return ConstPeriod.constructionTimeMin
    End Function

    Public Function PreiseHolen(ByVal europaCode As String, ByVal container As String, ByVal mileage As Integer, ByVal constructionYear As Integer, ByRef hep As String, ByRef hvp As String, Optional ByVal ezMonat As Integer = 0, Optional ByVal ezJahr As Integer = 0, Optional ByVal minConstructionTime As Integer = 0) As String
        Dim returnMessage As String = ""

        Try
            'Setzen der Container
            Dim vi As New de.dat.gold.authentication.Authentication()
            Dim sessionID As String = vi.Login(customerLogin, customerNumber, customerSignature, interfacePartnerNumber, interfacePartnerSignature)

            Dim viSelection As New de.dat.gold.VehicleSelectionService()
            viSelection.CookieContainer = vi.CookieContainer

            Dim iConstructionTime As Integer = CalculateConstructionTime(1, constructionYear)

            If minConstructionTime = 0 Then
                Dim localePeriod As de.dat.gold.locale = New de.dat.gold.locale()
                localePeriod.country = "DE"
                localePeriod.datCountryIndicator = "DE"
                localePeriod.language = "de"

                Dim vsPeriod As de.dat.gold.constructionPeriodSelectionRequest = New de.dat.gold.constructionPeriodSelectionRequest()
                vsPeriod.sessionID = sessionID
                vsPeriod.datECode = europaCode
                vsPeriod.restriction = de.dat.gold.releaseRestriction.APPRAISAL
                vsPeriod.locale = localePeriod
                If Not String.IsNullOrEmpty(container) Then
                    vsPeriod.container = container
                End If

                Dim ConstPeriod As de.dat.gold.constructionTimePeriod = viSelection.getConstructionPeriods(vsPeriod)
                minConstructionTime = ConstPeriod.constructionTimeMin
            End If

            Dim viEvaluation As New de.dat.gold.evaluation.Evaluation()
            viEvaluation.CookieContainer = vi.CookieContainer

            Dim vs As de.dat.gold.evaluation.vehicleEvaluationRequest = New de.dat.gold.evaluation.vehicleEvaluationRequest()
            vs.sessionID = sessionID
            vs.datECode = europaCode
            Try
                vs.mileage = mileage
            Catch
                vs.mileage = 1
            End Try
            If minConstructionTime > iConstructionTime Then
                vs.constructionTime = minConstructionTime
            Else
                vs.constructionTime = iConstructionTime
            End If
            vs.restriction = de.dat.gold.releaseRestriction.APPRAISAL

            Dim locale As de.dat.gold.evaluation.locale = New de.dat.gold.evaluation.locale()
            locale.country = "DE"
            locale.datCountryIndicator = "DE"
            locale.language = "de"
            vs.locale = locale
            If ezJahr > 0 Then
                vs.registrationDate = ezJahr.ToString().PadLeft(4, "0"c) & "-" & ezMonat.ToString().PadLeft(2, "0"c) & "-01"
            Else
                vs.registrationDate = constructionYear.ToString().PadLeft(4, "0"c) & "-06-06"
            End If
            vs.save = "false"
            vs.coverage = "SIMPLE"
            vs.vatType = "R"
            If Not String.IsNullOrEmpty(container) Then
                vs.container = container
            End If

            Try
                Dim bMx As de.dat.gold.evaluation.getVehicleEvaluationResponseVXS = viEvaluation.getVehicleEvaluation(vs)
                hep = bMx.Dossier(0).Valuation.PurchasePriceGross.Value.ToString()
                hvp = bMx.Dossier(0).Valuation.SalesPriceGross.Value.ToString()
            Catch e As Exception
                returnMessage = "Leider ist ein Fehler beim berechnen der Preise aufgetreten.<br>" & ReplaceErrorMessages(e.Message)
                hep = ""
                hvp = ""
            End Try

            vi.doLogout(False, False)

        Catch
            returnMessage = "Mit den gewählten Optionen ist leider keine Preiskalkulation möglich. Bitte ändern Sie Ihre Auswahl und versuchen Sie es erneut.<br>"
        End Try

        Return returnMessage

    End Function

    Private Function ReplaceErrorMessages(EMessage As String) As String
        EMessage = EMessage.Replace("Vehicle evaluation failed: de.dat.sphinx.global.exception.ApplicationException: NOT_EVALUABLE invalid mileage/+/0/", "Bitte geben Sie einen Kilometerstand zwischen 0 und " & " an.")
        EMessage = EMessage.Replace("Mileage is either wrong or missing.", "Die Bewertung kann für die angegebene Laufleistung nicht automatisch durchgeführt werden.")
        EMessage = EMessage.Replace("ECode not found for this construction time", "Die Bewertung kann für das angegebene Baujahr nicht automatisch durchgeführt werden.")
        Return EMessage

    End Function

    Public Function CalculateConstructionTime(ByVal monat As Integer, ByVal jahr As Integer) As Integer
        'Berechnung: Ausgangspunkt ist Januar 1970 = 10, ab da für jeden Monat +10
        Return (jahr - 1970) * 120 + monat * 10
    End Function

End Class
