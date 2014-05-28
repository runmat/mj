Option Explicit On
Option Infer On
Option Strict On

Imports CKG.Base.Common
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security

Namespace Business
    <Serializable()> Public Class Search
        REM § Lese-/Schreibfunktion, Kunde: FFD, 
        REM § LeseHaendlerSAP_Einzeln, LeseHaendlerSAP, LeseFilialenSAP - BAPI: Z_M_Adressdaten.

#Region " Definitions"
        Private _haendlerReferenzNummer As String
        Private _haendlerName As String
        Private _haendlerOrt As String
        Private _haendlerFiliale As String
        Private _haendler As DataTable
        Private _filialen As DataTable
        Private _districts As DataTable
        Private _errorMessage As String

        Private _app As App
        Private _user As User

        Private _CUSTOMER As String
        Private _NAME As String
        Private _NAME_2 As String
        Private _COUNTRYISO As String
        Private _POSTL_CODE As String
        Private _CITY As String
        Private _STREET As String

        Private _REFERENZ As String
        Private _FILIALE As String

        Private _gestartet As Boolean
        Private _searchResult As DataTable

        <NonSerialized()> Private _sessionID As String
        <NonSerialized()> Private _appID As String
        <NonSerialized()> Private _vwHaendler As DataView
        <NonSerialized()> Private _districtView As DataView
        <NonSerialized()> Private _vwFilialen As DataView
        <NonSerialized()> Protected LogApp As Logging.Trace
        <NonSerialized()> Protected IDSAP As Int32
#End Region

#Region " Public Properties"
        Public Property SessionID() As String
            Get
                Return _sessionID
            End Get
            Set(ByVal value As String)
                _sessionID = value
            End Set
        End Property

        'Public ReadOnly Property IDSAP() As Int32
        '    Get
        '        Return IDSAP
        '    End Get
        'End Property

        Public ReadOnly Property SearchResult() As DataTable
            Get
                Return _searchResult
            End Get
        End Property

        Public ReadOnly Property Gestartet() As Boolean
            Get
                Return _gestartet
            End Get
        End Property

        Public Property HaendlerReferenzNummer() As String
            Get
                Return _haendlerReferenzNummer
            End Get
            Set(ByVal value As String)
                _haendlerReferenzNummer = value
            End Set
        End Property

        Public Property HaendlerName() As String
            Get
                Return _haendlerName
            End Get
            Set(ByVal value As String)
                _haendlerName = value
            End Set
        End Property

        Public Property HaendlerOrt() As String
            Get
                Return _haendlerOrt
            End Get
            Set(ByVal value As String)
                _haendlerOrt = value
            End Set
        End Property

        Public Property HaendlerFiliale() As String
            Get
                Return _haendlerFiliale
            End Get
            Set(ByVal value As String)
                _haendlerFiliale = value
            End Set
        End Property

        Public ReadOnly Property Haendler() As DataView
            Get
                Return _vwHaendler
            End Get
        End Property

        Public ReadOnly Property Filialen() As DataView
            Get
                Return _filialen.DefaultView
            End Get
        End Property

        Public ReadOnly Property District() As DataView
            Get
                Return _districts.DefaultView
            End Get
        End Property

        Public ReadOnly Property ErrorMessage() As String
            Get
                Return _errorMessage
            End Get
        End Property

        Public ReadOnly Property CUSTOMER() As String
            Get
                Return _CUSTOMER.TrimStart("0"c)
            End Get
        End Property

        Public ReadOnly Property NAME() As String
            Get
                Return _NAME
            End Get
        End Property

        Public ReadOnly Property NAME_2() As String
            Get
                Return _NAME_2
            End Get
        End Property

        Public ReadOnly Property COUNTRYISO() As String
            Get
                Return _COUNTRYISO
            End Get
        End Property

        Public ReadOnly Property POSTL_CODE() As String
            Get
                Return _POSTL_CODE
            End Get
        End Property

        Public ReadOnly Property CITY() As String
            Get
                Return _CITY
            End Get
        End Property

        Public ReadOnly Property STREET() As String
            Get
                Return _STREET
            End Get
        End Property

        Public ReadOnly Property REFERENZ() As String
            Get
                Return _REFERENZ
            End Get
        End Property
#End Region

#Region " Public Methods"
        Public Sub New(ByRef objApp As App, ByRef objUser As User, ByVal sessionID As String, ByVal appID As String)
            _app = objApp
            _user = objUser


            _haendlerReferenzNummer = ""
            _haendlerName = ""
            _haendlerOrt = ""
            _haendlerFiliale = ""

            _sessionID = sessionID
            _appID = appID

            ResetFilialenTabelle()
        End Sub

        'Public Sub ReNewSAPDestination(ByVal sessionID As String, ByVal appID As String)

        '    _sessionID = sessionID
        '    _appID = appID
        'End Sub

        Public Function LeseHaendlerSAP_Einzeln(ByVal appID As String, ByVal sessionID As String, ByVal inputReferenz As String) As Boolean
            If Not _gestartet Then
                _gestartet = True

                _CUSTOMER = ""
                _NAME = ""
                _NAME_2 = ""
                _COUNTRYISO = ""
                _POSTL_CODE = ""
                _CITY = ""
                _STREET = ""
                _REFERENZ = ""
                _FILIALE = ""

                Dim blnReturn As Boolean = False

                If LogApp Is Nothing Then
                    LogApp = New Logging.Trace(_app.Connectionstring, _app.SaveLogAccessSAP, _app.LogLevel)
                End If
                IDSAP = -1

                Try
                    _errorMessage = ""

                    IDSAP = LogApp.WriteStartDataAccessSAP(_user.UserName, _user.IsTestUser, "Z_M_Adressdaten", appID, sessionID, _user.CurrentLogAccessASPXID)

                    Dim proxy = DynSapProxy.getProxy("Z_M_ADRESSDATEN", _app, _user, PageHelper.GetCurrentPage())

                    proxy.setImportParameter("I_KNRZE", "")
                    proxy.setImportParameter("I_KONZS", Right("0000000000" & _user.Customer.KUNNR, 10))
                    proxy.setImportParameter("I_KUNNR", inputReferenz)
                    proxy.setImportParameter("I_VKORG", "1510")

                    proxy.callBapi()

                    If IDSAP > -1 Then
                        LogApp.WriteEndDataAccessSAP(IDSAP, True)
                    End If

                    Dim returnTable = proxy.getExportTable("GT_WEB")

                    Select Case returnTable.Rows.Count
                        Case 0
                            _errorMessage = "Kein Suchergebnis."
                            If IDSAP > -1 Then
                                LogApp.WriteEndDataAccessSAP(IDSAP, False, _errorMessage)
                            End If

                            blnReturn = False
                        Case 1
                            Dim row As DataRow = returnTable.Rows(0)

                            _CUSTOMER = CStr(row("KONZS")).TrimStart("0"c)
                            _REFERENZ = Right(CStr(row("KUNNR")), 6).TrimStart("0"c)
                            _FILIALE = Right(CStr(row("KNRZE")), 6).TrimStart("0"c)
                            _NAME = CStr(row("NAME1"))
                            _NAME_2 = CStr(row("NAME2"))
                            _CITY = CStr(row("ORT01"))
                            _POSTL_CODE = CStr(row("PSTLZ"))
                            _STREET = CStr(row("STRAS"))
                            _COUNTRYISO = CStr(row("LAND1"))

                            blnReturn = True
                        Case Else
                            _errorMessage = "Suchergebnis nicht eindeutig."
                            If IDSAP > -1 Then
                                LogApp.WriteEndDataAccessSAP(IDSAP, False, _errorMessage)
                            End If

                            blnReturn = False
                    End Select

                    If _errorMessage.Length = 0 Then
                        WriteLogEntry(True, "Suche nach Händler """ & inputReferenz & """ erfolgreich.")
                    Else
                        WriteLogEntry(False, "Suche nach Händler """ & inputReferenz & """ nicht erfolgreich. (" & _errorMessage & ")")
                    End If
                Catch ex As Exception
                    Select Case ex.Message
                        Case "NO_WEB"
                            _errorMessage = "Keine Web-Tabelle erstellt."
                        Case "NO_DATA"
                            _errorMessage = "Keine Eingabedaten gefunden."
                        Case Else
                            _errorMessage = ex.Message
                    End Select
                    If IDSAP > -1 Then
                        LogApp.WriteEndDataAccessSAP(IDSAP, False, _errorMessage)
                    End If
                    blnReturn = False
                    WriteLogEntry(False, "Suche nach Händler """ & inputReferenz & """ nicht erfolgreich. (" & _errorMessage & ")")
                Finally
                    If IDSAP > -1 Then
                        LogApp.WriteStandardDataAccessSAP(IDSAP)
                    End If

                    _gestartet = False
                End Try

                Return blnReturn
            End If
        End Function

        Public Function LeseFilialenSAP(Optional ByVal inputFiliale As String = "") As Int32
            ResetFilialenTabelle()
            ResetHaendlerTabelle()

            Dim tempFiliale = inputFiliale
            Dim cn As New SqlClient.SqlConnection(_user.App.Connectionstring)
            Dim intReturn As Int32

            Try
                If inputFiliale.Length = 0 Then
                    tempFiliale = _haendlerFiliale
                End If

                cn.Open()
                If tempFiliale.Trim(" "c).Length = 0 Then
                    Dim da As New SqlClient.SqlDataAdapter("SELECT OrganizationReference AS FILIALE, OrganizationName AS DISPLAY_FILIALE FROM Organization WHERE OrganizationReference <> '' AND OrganizationReference <> '999' AND CustomerID=@CustomerID", cn)
                    da.SelectCommand.Parameters.AddWithValue("@CustomerID", _user.Customer.CustomerId)
                    da.Fill(_filialen)
                Else
                    Dim da As New SqlClient.SqlDataAdapter("SELECT OrganizationReference AS FILIALE, OrganizationName AS DISPLAY_FILIALE FROM Organization WHERE OrganizationReference=@OrganizationReference AND CustomerID=@CustomerID", cn)
                    da.SelectCommand.Parameters.AddWithValue("@CustomerID", _user.Customer.CustomerId)
                    da.SelectCommand.Parameters.AddWithValue("@OrganizationReference", tempFiliale)
                    da.Fill(_filialen)
                End If

                intReturn = _filialen.Rows.Count
            Catch ex As Exception
                _errorMessage = "Keine Filialen für diesen Kunden <br>(" & ex.Message & ")."
                intReturn = 0
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
            Return intReturn
        End Function

        Public Function ReadDistrictSAP(ByVal appID As String, ByVal sessionID As String) As Int32
            If Not _gestartet Then
                _gestartet = True

                ResetDistrictTable()

                If LogApp Is Nothing Then
                    LogApp = New Logging.Trace(_app.Connectionstring, _app.SaveLogAccessSAP, _app.LogLevel)
                End If
                IDSAP = -1

                Dim intReturn As Int32
                Try
                    _errorMessage = ""

                    IDSAP = LogApp.WriteStartDataAccessSAP(_user.UserName, _user.IsTestUser, "Z_Berechtigung_Distrikte", appID, sessionID, _user.CurrentLogAccessASPXID)

                    Dim proxy = DynSapProxy.getProxy("Z_BERECHTIGUNG_DISTRIKTE", _app, _user, PageHelper.GetCurrentPage())

                    proxy.setImportParameter("ANWENDUNG", appID)
                    proxy.setImportParameter("BENGRP", _user.UserID.ToString)
                    proxy.setImportParameter("KUNNR", Right("0000000000" & _user.Customer.KUNNR, 10))

                    proxy.callBapi()

                    If IDSAP > -1 Then
                        LogApp.WriteEndDataAccessSAP(IDSAP, True)
                    End If

                    Dim returnTable = proxy.getExportTable("OUT_TAB")

                    intReturn = returnTable.Rows.Count
                    If intReturn = 0 Then
                        _errorMessage = "Kein Suchergebnis."
                        'Suche ohne Ergebnis
                        If IDSAP > -1 Then
                            LogApp.WriteEndDataAccessSAP(IDSAP, False, _errorMessage)
                        End If
                    Else
                        Dim newRow As DataRow

                        For Each row As DataRow In returnTable.Rows
                            newRow = _districts.NewRow()
                            newRow("DISTRIKT") = Right(CStr(row("DISTRIKT")), 6).TrimStart("0"c)
                            newRow("VORBELEGT") = CStr(row("VORBELEGT"))
                            newRow("NAME1") = CStr(row("NAME1"))
                            _districts.Rows.Add(newRow)
                        Next
                        _districtView = _districts.DefaultView
                        _districtView.Sort = "NAME1"
                    End If

                    If _errorMessage.Length = 0 Then
                        WriteLogEntry(True, "Suche nach Distrikten erfolgreich.")
                    Else
                        WriteLogEntry(False, "Suche nach Distrikten nicht erfolgreich. (" & _errorMessage & ")")
                    End If
                Catch ex As Exception
                    Select Case ex.Message
                        Case "NO_WEB"
                            _errorMessage = "Keine Web-Tabelle erstellt."
                            intReturn = 0
                        Case "NO_DATA"
                            _errorMessage = "Keine Eingabedaten gefunden."
                            intReturn = 0
                        Case Else
                            _errorMessage = ex.Message
                            intReturn = 0
                    End Select
                    If IDSAP > -1 Then
                        LogApp.WriteEndDataAccessSAP(IDSAP, False, _errorMessage)
                    End If
                    WriteLogEntry(False, "Suche nach Distrikten nicht erfolgreich. (" & _errorMessage & ")")
                Finally
                    If IDSAP > -1 Then
                        LogApp.WriteStandardDataAccessSAP(IDSAP)
                    End If

                    _gestartet = False
                End Try
                Return intReturn
            End If
        End Function

        Public Function LeseHaendlerSAP(ByVal appID As String, ByVal sessionID As String, Optional ByVal inputReferenz As String = "", Optional ByVal inputFiliale As String = "") As Int32
            If Not _gestartet Then
                _gestartet = True

                ResetHaendlerTabelle()

                If LogApp Is Nothing Then
                    LogApp = New Logging.Trace(_app.Connectionstring, _app.SaveLogAccessSAP, _app.LogLevel)
                End If
                IDSAP = -1
                Dim tempFiliale As String = inputFiliale
                Dim tempReferenz As String = inputReferenz
                Dim tempName As String = ""
                Dim ort As String = ""
                Dim intReturn As Int32

                Try
                    _errorMessage = ""

                    If InputFiliale.Length = 0 And InputReferenz.Length = 0 Then
                        tempFiliale = _haendlerFiliale
                        tempReferenz = _haendlerReferenzNummer
                    End If
                    '§§§JVE <14.11.2005 drittes Kriterium eingefügt...>
                    If ((tempFiliale.Length = 0 And tempReferenz.Length = 0 And _haendlerName.Length = 0 And _haendlerOrt.Length = 0) And (_user.Organization.AllOrganizations = False)) Then
                        _errorMessage = "Händler- oder Filialnummer müssen gefüllt sein."
                        intReturn = -1
                    Else
                        If tempFiliale = "ALL" Then  ' FFD Regionalzuordnung jeder solle alle Händler sehen 
                            tempFiliale = ""         ' lt. Braasch 29.02.2008
                        End If
                        IDSAP = LogApp.WriteStartDataAccessSAP(_user.UserName, _user.IsTestUser, "Z_M_Adressdaten", appID, sessionID, _user.CurrentLogAccessASPXID)

                        Dim proxy = DynSapProxy.getProxy("Z_M_ADRESSDATEN", _app, _user, PageHelper.GetCurrentPage())
                        proxy.setImportParameter("I_KNRZE", tempFiliale)
                        proxy.setImportParameter("I_KONZS", Right("0000000000" & _user.Customer.KUNNR, 10))
                        proxy.setImportParameter("I_KUNNR", tempReferenz)
                        proxy.setImportParameter("I_VKORG", "1510")

                        proxy.callBapi()

                        If IDSAP > -1 Then
                            LogApp.WriteEndDataAccessSAP(IDSAP, True)
                        End If

                        Dim returnTable = proxy.getExportTable("GT_WEB")

                        intReturn = returnTable.Rows.Count
                        If intReturn = 0 Then
                            _errorMessage = "Kein Suchergebnis."
                            'Suche ohne Ergebnis
                            If IDSAP > -1 Then
                                LogApp.WriteEndDataAccessSAP(IDSAP, False, _errorMessage)
                            End If
                        Else

                            For Each row As DataRow In returnTable.Rows
                                Dim dealerRow = _haendler.NewRow()
                                dealerRow("CUSTOMER") = CStr(row("Konzs")).TrimStart("0"c)

                                dealerRow("REFERENZ") = Right(CStr(row("Kunnr")), 6).TrimStart("0"c)
                                dealerRow("FILIALE") = Right(CStr(row("Knrze")), 6).TrimStart("0"c)

                                dealerRow("NAME") = CStr(row("Name1"))
                                dealerRow("NAME_2") = CStr(row("Name2"))
                                dealerRow("CITY") = CStr(row("Ort01"))
                                dealerRow("POSTL_CODE") = CStr(row("Pstlz"))
                                dealerRow("STREET") = CStr(row("Stras"))
                                dealerRow("COUNTRYISO") = CStr(row("Land1"))
                                dealerRow("DISPLAY") = String.Format("{0} - {1}, {2}", Right(CStr(row("Kunnr")), 6).TrimStart("0"c), CStr(row("Name1")), CStr(row("Ort01")))
                                dealerRow("DISPLAY_ADDRESS") = CStr(row("Name1"))
                                If Not CStr(row("Name2")).Trim(" "c).Length = 0 Then
                                    dealerRow("DISPLAY_ADDRESS") = String.Format("{0}, {1}, {2}-{3} {4}, {5}", dealerRow("DISPLAY_ADDRESS").ToString, CStr(row("Name2")), CStr(row("Land1")), CStr(row("Pstlz")), CStr(row("Ort01")), CStr(row("Stras")))
                                Else
                                    dealerRow("DISPLAY_ADDRESS") = String.Format("{0}, {1}-{2} {3}, {4}", dealerRow("DISPLAY_ADDRESS").ToString, CStr(row("Land1")), CStr(row("Pstlz")), CStr(row("Ort01")), CStr(row("Stras")))
                                End If

                                _haendler.Rows.Add(dealerRow)
                            Next
                            _vwHaendler = _haendler.DefaultView
                            If returnTable.Rows.Count > 1 Then
                                'Weitere Auswahl entspechend Name und/oder Ort
                                Dim filterExp As String = ""
                                If _haendlerReferenzNummer.Trim(" "c).Length = 0 Then
                                    If (Len(Trim(_haendlerName)) > 0) And (Len(Trim(_haendlerOrt)) > 0) Then
                                        filterExp = "NAME like '" & Replace(_haendlerName, "'", "''") & "' AND CITY like '" & Replace(_haendlerOrt, "'", "''") & "'"
                                    ElseIf Len(Trim(_haendlerName)) > 0 Then
                                        filterExp = "NAME like '" & Replace(_haendlerName, "'", "''") & "'"
                                    ElseIf Len(Trim(_haendlerOrt)) > 0 Then
                                        filterExp = "CITY like '" & Replace(_haendlerOrt, "'", "''") & "'"
                                    End If
                                End If

                                If filterExp.Length > 0 Then
                                    filterExp = Replace(filterExp, "*", "%")
                                    _vwHaendler.RowFilter = filterExp
                                    intReturn = _vwHaendler.Count
                                End If
                                'Suche mit mehreren Ergebnissen
                            End If
                            _vwHaendler.Sort = "NAME"
                        End If

                    End If

                    If _errorMessage.Length = 0 Then
                        WriteLogEntry(True, "Suche nach Filiale """ & tempFiliale & """ bzw. Händler """ & tempReferenz & """ erfolgreich.")
                    Else
                        WriteLogEntry(False, "Suche nach Filiale """ & tempFiliale & """ bzw. Händler """ & tempReferenz & """ nicht erfolgreich. (" & _errorMessage & ")")
                    End If

                Catch ex As Exception
                    Select Case ex.Message
                        Case "NO_WEB"
                            _errorMessage = "Keine Web-Tabelle erstellt."
                            intReturn = 0
                        Case "NO_DATA"
                            _errorMessage = "Keine Eingabedaten gefunden."
                            intReturn = 0
                        Case Else
                            _errorMessage = ex.Message
                            intReturn = 0
                    End Select
                    If IDSAP > -1 Then
                        LogApp.WriteEndDataAccessSAP(IDSAP, False, _errorMessage)
                    End If
                    WriteLogEntry(False, "Suche nach Filiale """ & tempFiliale & """ bzw. Händler """ & tempReferenz & """ nicht erfolgreich. (" & _errorMessage & ")")
                Finally
                    If IDSAP > -1 Then
                        LogApp.WriteStandardDataAccessSAP(IDSAP)
                    End If

                    _gestartet = False
                End Try
                Return intReturn
            End If
        End Function

        Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal comment As String)
            Try
                If LogApp Is Nothing Then
                    LogApp = New Logging.Trace(_app.Connectionstring, _app.SaveLogAccessSAP, _app.LogLevel)
                End If

                Dim type = If(blnSuccess, "DBG", "ERR")
                LogApp.WriteEntry(type, _user.UserName, _sessionID, CInt(_appID), _user.Applications.Select("AppID = '" & _appID & "'")(0)("AppFriendlyName").ToString, "Report", comment, _user.CustomerName, _user.Customer.CustomerId, _user.IsTestUser, 0)
            Catch ex As Exception
                _app.WriteErrorText(1, _user.UserName, "DADReports", "WriteLogEntry", ex.ToString)
            End Try
        End Sub

        Private Sub ResetHaendlerTabelle()
            _haendler = New DataTable()

            _haendler.Columns.Add("CUSTOMER", GetType(String))
            _haendler.Columns.Add("REFERENZ", GetType(String))
            _haendler.Columns.Add("FILIALE", GetType(String))
            _haendler.Columns.Add("NAME", GetType(String))
            _haendler.Columns.Add("NAME_2", GetType(String))
            _haendler.Columns.Add("CITY", GetType(String))
            _haendler.Columns.Add("POSTL_CODE", GetType(String))
            _haendler.Columns.Add("STREET", GetType(String))
            _haendler.Columns.Add("COUNTRYISO", GetType(String))
            _haendler.Columns.Add("DISPLAY", GetType(String))
            _haendler.Columns.Add("DISPLAY_ADDRESS", GetType(String))
        End Sub

        Private Sub ResetDistrictTable()
            _districts = New DataTable()

            _districts.Columns.Add("DISTRIKT", GetType(String))
            _districts.Columns.Add("VORBELEGT", GetType(String))
            _districts.Columns.Add("NAME1", GetType(String))
        End Sub

        Private Sub ResetFilialenTabelle()
            _filialen = New DataTable()

            _filialen.Columns.Add("CUSTOMER", GetType(String))
            _filialen.Columns.Add("FILIALE", GetType(String))
            _filialen.Columns.Add("NAME", GetType(String))
            _filialen.Columns.Add("NAME_2", GetType(String))
            _filialen.Columns.Add("CITY", GetType(String))
            _filialen.Columns.Add("POSTL_CODE", GetType(String))
            _filialen.Columns.Add("STREET", GetType(String))
            _filialen.Columns.Add("COUNTRYISO", GetType(String))
            _filialen.Columns.Add("DISPLAY_FILIALE", GetType(String))
        End Sub

        'Public Function LeseAdressenSAP(ByVal strParentNode As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32 'HEZ: "B" oder "C"
        '    Dim tblTemp As DataTable
        '    _searchResult = New DataTable()
        '    _searchResult.Columns.Add("ADDRESSNUMBER", GetType(String))
        '    _searchResult.Columns.Add("DISPLAY_ADDRESS", GetType(String))

        '    _searchResult.Columns.Add("POSTL_CODE", GetType(String))
        '    _searchResult.Columns.Add("STREET", GetType(String))
        '    _searchResult.Columns.Add("COUNTRYISO", GetType(String))
        '    _searchResult.Columns.Add("CITY", GetType(String))
        '    _searchResult.Columns.Add("NAME", GetType(String))
        '    _searchResult.Columns.Add("NAME_2", GetType(String))

        '    Dim currentPage = PageHelper.GetCurrentPage()
        '    Dim proxy = DynSapProxy.getProxy("BAPI_CUSTOMER_GET_CHILDREN", _app, _user, currentPage)

        '    proxy.setImportParameter("CUSTHITYP", adressart)
        '    proxy.setImportParameter("CUSTOMERNO", Right("0000000060" & strParentNode, 10))
        '    proxy.setImportParameter("NODE_LEVEL", nodelevel)
        '    proxy.setImportParameter("VALID_ON", DateTime.Now.ToShortDateString())

        '    proxy.callBapi()

        '    tblTemp = proxy.getExportTable("NODE_LIST")

        '    Dim SAPTableRow As DataRow

        '    Dim newDealerDetailRow As DataRow

        '    For Each SAPTableRow In tblTemp.Rows
        '        'Der Händler soll sich nicht selbst zur Auswahl bekommen!!!
        '        If SAPTableRow("NODE_LEVEL").ToString.TrimStart("0"c) = "1" Then

        '            Dim innerProxy = DynSapProxy.getProxy("BAPI_CUSTOMER_GETDETAIL2", _app, _user, currentPage)
        '            innerProxy.setImportParameter("COMPANYCODE", "")
        '            innerProxy.setImportParameter("CUSTOMERNO", CStr(SAPTableRow("CUSTOMER")))
        '            innerProxy.callBapi()

        '            Dim SAPCustomerAdress = innerProxy.getExportParameter("CUSTOMERADDRESS")
        '            Dim SAPCustomerCompanyDetail = innerProxy.getExportParameter("CUSTOMERCOMPANYDETAIL")
        '            Dim SAPCustomerDetail = innerProxy.getExportParameter("CUSTOMERGENERALDETAIL")
        '            Dim SAPBapiRet1 = innerProxy.getExportParameter("RETURN")

        '            Dim SAPCustomerBankDetail = innerProxy.getExportTable("CUSTOMERBANKDETAIL")

        '            'Die Detaildaten zu den Händlern in die Tabelle _haendler schreiben
        '            'objSAP.Bapi_Customer_Getdetail2("", SAPTableRow("CUSTOMER").ToString, SAPCustomerAdress, SAPCustomerCompanyDetail, SAPCustomerDetail, SAPBapiRet1, SAPCustomerBankDetail)

        '            'If SAPBapiRet1.Type.Trim(" "c) = "" Or SAPBapiRet1.Type = "S" Or SAPBapiRet1.Type = "I" Then
        '            '    If (Not SAPCustomerDetail.Groupkey = _user.Reference) Or (SAPCustomerDetail.Groupkey.Length = 0 And _user.Reference.Length = 0) Then
        '            '        newDealerDetailRow = _searchResult.NewRow

        '            '        Dim strTemp As String = SAPCustomerAdress.Name
        '            '        If SAPCustomerAdress.Name_2.Length > 0 Then
        '            '            strTemp &= ", " & SAPCustomerAdress.Name_2
        '            '        End If
        '            '        If SAPCustomerAdress.Name_3.Length > 0 Then
        '            '            strTemp &= ", " & SAPCustomerAdress.Name_3
        '            '        End If
        '            '        If SAPCustomerAdress.Name_4.Length > 0 Then
        '            '            strTemp &= ", " & SAPCustomerAdress.Name_4
        '            '        End If

        '            '        newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPCustomerAdress.Countryiso & " - " & SAPCustomerAdress.Postl_Code & " " & SAPCustomerAdress.City & ", " & SAPCustomerAdress.Street
        '            '        newDealerDetailRow("ADDRESSNUMBER") = SAPCustomerDetail.Customer
        '            '        If SAPCustomerAdress.Postl_Code.Length > 0 Then
        '            '            newDealerDetailRow("POSTL_CODE") = SAPCustomerAdress.Postl_Code
        '            '        End If
        '            '        If SAPCustomerAdress.Street.Length > 0 Then
        '            '            newDealerDetailRow("STREET") = SAPCustomerAdress.Street
        '            '        End If
        '            '        If SAPCustomerAdress.Countryiso.Length > 0 Then
        '            '            newDealerDetailRow("COUNTRYISO") = SAPCustomerAdress.Countryiso
        '            '        End If
        '            '        If SAPCustomerAdress.City.Length > 0 Then
        '            '            newDealerDetailRow("CITY") = SAPCustomerAdress.City
        '            '        End If
        '            '        If SAPCustomerAdress.Name.Length > 0 Then
        '            '            newDealerDetailRow("NAME") = SAPCustomerAdress.Name
        '            '        End If
        '            '        If SAPCustomerAdress.Name_2.Length > 0 Then
        '            '            newDealerDetailRow("NAME_2") = SAPCustomerAdress.Name_2
        '            '        End If

        '            '        _searchResult.Rows.Add(newDealerDetailRow)
        '            '    End If
        '            'End If
        '        End If
        '    Next

        '    Return _searchResult.Rows.Count
        'End Function
#End Region
    End Class
End Namespace

' ************************************************
' $History: Search.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Business
' Migration OR
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Base/Business
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 10.03.08   Time: 10:15
' Updated in $/CKG/Base/Base/Business
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 14.02.08   Time: 13:54
' Updated in $/CKG/Base/Base/Business
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Business
' ITA:1440
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Business
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Base/Base/Business
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
