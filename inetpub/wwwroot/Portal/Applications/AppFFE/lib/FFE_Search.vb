Option Explicit On
Option Strict On

Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

'Imports System
'Imports CKG.Base.Kernel
<Serializable()> Public Class FFE_Search
#Region " Definitions"
    Private m_strHaendlerReferenzNummer As String
    Private m_strHaendlerName As String
    Private m_strHaendlerOrt As String
    Private m_strHaendlerFiliale As String
    Private m_tblHaendler As DataTable
    Private m_tblFilialen As DataTable
    Private m_districtTable As DataTable
    Private m_strErrorMessage As String

    Private m_objApp As Base.Kernel.Security.App
    Private m_objUser As Base.Kernel.Security.User

    Private m_strCUSTOMER As String
    Private m_strNAME As String
    Private m_strNAME_2 As String
    Private m_strCOUNTRYISO As String
    Private m_strPOSTL_CODE As String
    Private m_strCITY As String
    Private m_strSTREET As String

    Private m_strREFERENZ As String
    Private m_strFILIALE As String

    Private m_blnGestartet As Boolean
    Private m_tblSearchResult As DataTable

    Protected m_intStatus As Int32


    <NonSerialized()> Private m_strSessionID As String
    <NonSerialized()> Private m_strAppID As String
    <NonSerialized()> Private m_vwHaendler As DataView
    <NonSerialized()> Private m_districtView As DataView
    '<NonSerialized()> Private m_vwFilialen As DataView
    <NonSerialized()> Protected m_objLogApp As Base.Kernel.Logging.Trace
    <NonSerialized()> Protected m_intIDSAP As Int32
#End Region

#Region " Public Properties"
    Public Property SessionID() As String
        Get
            Return m_strSessionID
        End Get
        Set(ByVal Value As String)
            m_strSessionID = Value
        End Set
    End Property

    Public ReadOnly Property IDSAP() As Int32
        Get
            Return m_intIDSAP
        End Get
    End Property

    Public ReadOnly Property SearchResult() As DataTable
        Get
            Return m_tblSearchResult
        End Get
    End Property

    Public ReadOnly Property Gestartet() As Boolean
        Get
            Return m_blnGestartet
        End Get
    End Property

    Public Property HaendlerReferenzNummer() As String
        Get
            Return m_strHaendlerReferenzNummer
        End Get
        Set(ByVal Value As String)
            m_strHaendlerReferenzNummer = Value
        End Set
    End Property

    Public Property HaendlerName() As String
        Get
            Return m_strHaendlerName
        End Get
        Set(ByVal Value As String)
            m_strHaendlerName = Value
        End Set
    End Property

    Public Property HaendlerOrt() As String
        Get
            Return m_strHaendlerOrt
        End Get
        Set(ByVal Value As String)
            m_strHaendlerOrt = Value
        End Set
    End Property

    Public Property HaendlerFiliale() As String
        Get
            Return m_strHaendlerFiliale
        End Get
        Set(ByVal Value As String)
            m_strHaendlerFiliale = Value
        End Set
    End Property

    Public ReadOnly Property Haendler() As DataView
        Get
            Return m_vwHaendler
        End Get
    End Property

    Public ReadOnly Property Filialen() As DataView
        Get
            Return m_tblFilialen.DefaultView
        End Get
    End Property

    Public ReadOnly Property District() As DataView
        Get
            Return m_districtTable.DefaultView
        End Get
    End Property

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return m_strErrorMessage
        End Get
    End Property

    Public ReadOnly Property CUSTOMER() As String
        Get
            Return m_strCUSTOMER.TrimStart("0"c)
        End Get
    End Property

    Public ReadOnly Property NAME() As String
        Get
            Return m_strNAME
        End Get
    End Property

    Public ReadOnly Property NAME_2() As String
        Get
            Return m_strNAME_2
        End Get
    End Property

    Public ReadOnly Property COUNTRYISO() As String
        Get
            Return m_strCOUNTRYISO
        End Get
    End Property

    Public ReadOnly Property POSTL_CODE() As String
        Get
            Return m_strPOSTL_CODE
        End Get
    End Property

    Public ReadOnly Property CITY() As String
        Get
            Return m_strCITY
        End Get
    End Property

    Public ReadOnly Property STREET() As String
        Get
            Return m_strSTREET
        End Get
    End Property

    Public ReadOnly Property REFERENZ() As String
        Get
            Return m_strREFERENZ
        End Get
    End Property
#End Region

#Region " Public Methods"
    Public Sub New(ByRef objApp As Base.Kernel.Security.App, ByRef objUser As Base.Kernel.Security.User, ByVal strSessionID As String, ByVal strAppID As String)
        m_objApp = objApp
        m_objUser = objUser

        m_strHaendlerReferenzNummer = ""
        m_strHaendlerName = ""
        m_strHaendlerOrt = ""
        m_strHaendlerFiliale = ""

        m_strSessionID = strSessionID
        m_strAppID = strAppID

        ResetFilialenTabelle()
    End Sub

    Public Sub ReNewSAPDestination(ByVal strSessionID As String, ByVal strAppID As String)

        m_strSessionID = strSessionID
        m_strAppID = strAppID
    End Sub

    Public Function LeseHaendlerSAP_Einzeln(ByVal strAppID As String, ByVal strSessionID As String, ByVal InputReferenz As String) As Boolean
        
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_strCUSTOMER = ""
            m_strNAME = ""
            m_strNAME_2 = ""
            m_strCOUNTRYISO = ""
            m_strPOSTL_CODE = ""
            m_strCITY = ""
            m_strSTREET = ""
            m_strREFERENZ = ""
            m_strFILIALE = ""

            Dim blnReturn As Boolean = False

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_strErrorMessage = ""

                S.AP.Init("Z_M_Adressdaten_Fce")

                S.AP.SetImportParameter("I_KUNNR", InputReferenz)
                S.AP.SetImportParameter("I_KNRZE", "")
                S.AP.SetImportParameter("I_KONZS", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                S.AP.SetImportParameter("I_VKORG", "1510")

                S.AP.Execute()

                Dim SAPReturnTable As DataTable = S.AP.GetExportTable("GT_WEB")

                Select Case SAPReturnTable.Rows.Count
                    Case 0
                        m_strErrorMessage = "Kein Suchergebnis."

                        blnReturn = False
                    Case 1
                        Dim dr As DataRow = SAPReturnTable.Rows(0)

                        m_strCUSTOMER = dr("Konzs").ToString.TrimStart("0"c)
                        m_strREFERENZ = Right(dr("Kunnr").ToString, 6).TrimStart("0"c)
                        m_strFILIALE = Right(dr("Knrze").ToString, 6).TrimStart("0"c)
                        m_strNAME = dr("Name1").ToString
                        m_strNAME_2 = dr("Name2").ToString
                        m_strCITY = dr("Ort01").ToString
                        m_strPOSTL_CODE = dr("Pstlz").ToString
                        m_strSTREET = dr("Stras").ToString
                        m_strCOUNTRYISO = dr("Land1").ToString

                        blnReturn = True
                    Case Else
                        ' SAP-Performance Logging
                        m_strErrorMessage = "Suchergebnis nicht eindeutig."

                        blnReturn = False
                End Select

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_WEB"
                        m_strErrorMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_strErrorMessage = "Keine Eingabedaten gefunden."
                    Case Else
                        m_strErrorMessage = ex.Message
                End Select

                'Rückgabewert setzen
                blnReturn = False
                ' Fehler Logging
                WriteLogEntry(False, "Suche nach Händler """ & InputReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
            Finally

                m_blnGestartet = False
            End Try

            Return blnReturn
        End If

        Return False
    End Function

    Public Function LeseFilialenSAP(Optional ByVal InputFiliale As String = "") As Int32
        ResetFilialenTabelle()
        ResetHaendlerTabelle()

        Dim strTempFiliale As String = InputFiliale
        Dim cn As New SqlClient.SqlConnection(m_objUser.App.Connectionstring)
        Dim intReturn As Int32

        Try
            If InputFiliale.Length = 0 Then
                strTempFiliale = m_strHaendlerFiliale
            End If

            cn.Open()
            If strTempFiliale.Trim(" "c).Length = 0 Then
                Dim da As New SqlClient.SqlDataAdapter("SELECT OrganizationReference AS FILIALE, OrganizationName AS DISPLAY_FILIALE FROM Organization WHERE OrganizationReference <> '' AND OrganizationReference <> '999' AND CustomerID=@CustomerID", cn)
                da.SelectCommand.Parameters.AddWithValue("@CustomerID", m_objUser.Customer.CustomerId)
                da.Fill(m_tblFilialen)
            Else
                Dim da As New SqlClient.SqlDataAdapter("SELECT OrganizationReference AS FILIALE, OrganizationName AS DISPLAY_FILIALE FROM Organization WHERE OrganizationReference=@OrganizationReference AND CustomerID=@CustomerID", cn)
                da.SelectCommand.Parameters.AddWithValue("@CustomerID", m_objUser.Customer.CustomerId)
                da.SelectCommand.Parameters.AddWithValue("@OrganizationReference", strTempFiliale)
                da.Fill(m_tblFilialen)
            End If

            intReturn = m_tblFilialen.Rows.Count
        Catch ex As Exception
            m_strErrorMessage = "Keine Filialen für diesen Kunden <br>(" & ex.Message & ")."
            intReturn = 0
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
        Return intReturn
    End Function

    Public Function ReadDistrictSAP(ByVal appID As String, ByVal sessionID As String) As Int32
        If Not m_blnGestartet Then
            m_blnGestartet = True


            ResetDistrictTable()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Dim intReturn As Int32

            Try
                m_strErrorMessage = ""

                S.AP.Init("Z_Berechtigung_Distrikte")

                S.AP.SetImportParameter("KUNNR", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                S.AP.SetImportParameter("BENGRP", m_objUser.UserID.ToString)
                S.AP.SetImportParameter("ANWENDUNG", appID)

                S.AP.Execute()

                Dim SAPReturnTable As DataTable = S.AP.GetExportTable("OUT_TAB")

                intReturn = SAPReturnTable.Rows.Count
                If SAPReturnTable.Rows.Count = 0 Then
                    m_strErrorMessage = "Kein Suchergebnis."
                Else
                    For Each dr As DataRow In SAPReturnTable.Rows

                        Dim newRow As DataRow = m_districtTable.NewRow()

                        newRow("DISTRIKT") = Right(dr("Distrikt").ToString, 6).TrimStart("0"c)
                        newRow("VORBELEGT") = dr("Vorbelegt")
                        newRow("NAME1") = dr("Name1")

                        m_districtTable.Rows.Add(newRow)
                    Next

                    m_districtView = m_districtTable.DefaultView
                    m_districtView.Sort = "NAME1"
                End If

                If m_strErrorMessage.Length = 0 Then
                    WriteLogEntry(True, "Suche nach Distrikten erfolgreich.")
                Else
                    WriteLogEntry(False, "Suche nach Distrikten nicht erfolgreich. (" & m_strErrorMessage & ")")
                End If

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_WEB"
                        m_strErrorMessage = "Keine Web-Tabelle erstellt."
                        intReturn = 0
                    Case "NO_DATA"
                        m_strErrorMessage = "Keine Eingabedaten gefunden."
                        intReturn = 0
                    Case Else
                        m_strErrorMessage = ex.Message
                        intReturn = 0
                End Select
                WriteLogEntry(False, "Suche nach Distrikten nicht erfolgreich. (" & m_strErrorMessage & ")")
            Finally
                m_blnGestartet = False
            End Try

            Return intReturn
        End If

        Return -1
    End Function

    Public Function LeseHaendlerSAP(ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal InputReferenz As String = "",
                                    Optional ByVal InputFiliale As String = "") As Int32
        If Not m_blnGestartet Then
            m_blnGestartet = True

            ResetHaendlerTabelle()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim I As Int32
            Dim strTempFiliale As String = InputFiliale
            Dim strTempReferenz As String = InputReferenz
            Dim intReturn As Int32

            Try
                m_strErrorMessage = ""

                If InputFiliale.Length = 0 And InputReferenz.Length = 0 Then
                    strTempFiliale = m_strHaendlerFiliale
                    strTempReferenz = m_strHaendlerReferenzNummer
                End If

                If ((strTempFiliale.Length = 0 And
                     strTempReferenz.Length = 0 And
                     m_strHaendlerName.Length = 0 And
                     m_strHaendlerOrt.Length = 0) And
                    (m_objUser.Organization.AllOrganizations = False)) Then

                    m_strErrorMessage = "Händler- oder Filialnummer müssen gefüllt sein."
                    intReturn = -1
                Else

                    If strTempFiliale = "ALL" Then  ' FFD Regionalzuordnung jeder solle alle Händler sehen 
                        strTempFiliale = ""         ' lt. Braasch 29.02.2008
                    End If

                    S.AP.Init("Z_M_Adressdaten_Fce")

                    S.AP.SetImportParameter("I_KUNNR", strTempReferenz)
                    S.AP.SetImportParameter("I_KNRZE", strTempFiliale)
                    S.AP.SetImportParameter("I_KONZS", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                    S.AP.SetImportParameter("I_VKORG", "1510")

                    S.AP.Execute()

                    Dim SAPReturnTable As DataTable = S.AP.GetExportTable("GT_WEB")

                    intReturn = SAPReturnTable.Rows.Count
                    If intReturn = 0 Then
                        m_strErrorMessage = "Kein Suchergebnis."
                    Else
                        Dim newDealerDetailRow As DataRow
                        Dim dr As DataRow

                        For I = 0 To SAPReturnTable.Rows.Count - 1
                            dr = SAPReturnTable.Rows(I)

                            newDealerDetailRow = m_tblHaendler.NewRow()
                            newDealerDetailRow("CUSTOMER") = dr("Konzs").ToString.TrimStart("0"c)

                            newDealerDetailRow("REFERENZ") = Right(dr("Kunnr").ToString, 6).TrimStart("0"c)
                            newDealerDetailRow("FILIALE") = Right(dr("Knrze").ToString, 6).TrimStart("0"c)

                            newDealerDetailRow("NAME") = dr("Name1").ToString
                            newDealerDetailRow("NAME_2") = dr("Name2").ToString
                            newDealerDetailRow("CITY") = dr("Ort01").ToString
                            newDealerDetailRow("POSTL_CODE") = dr("Pstlz").ToString
                            newDealerDetailRow("STREET") = dr("Stras").ToString
                            newDealerDetailRow("COUNTRYISO") = dr("Land1").ToString
                            newDealerDetailRow("DISPLAY") = Right(dr("Kunnr").ToString, 6).TrimStart("0"c) & " - " & dr("Name1").ToString & ", " & dr("Ort01").ToString
                            newDealerDetailRow("DISPLAY_ADDRESS") = dr("Name1").ToString
                            If Not dr("Name2").ToString.Trim(" "c).Length = 0 Then
                                newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & dr("Name2").ToString & ", " &
                                    dr("Land1").ToString & "-" & dr("Pstlz").ToString & " " & dr("Ort01").ToString & ", " & dr("Stras").ToString
                            Else
                                newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & dr("Land1").ToString & "-" &
                                    dr("Pstlz").ToString & " " & dr("Ort01").ToString & ", " & dr("Stras").ToString
                            End If

                            m_tblHaendler.Rows.Add(newDealerDetailRow)
                        Next

                        m_vwHaendler = m_tblHaendler.DefaultView

                        If SAPReturnTable.Rows.Count > 1 Then
                            'Weitere Auswahl entspechend Name und/oder Ort
                            Dim filterExp As String = ""
                            If m_strHaendlerReferenzNummer.Trim(" "c).Length = 0 Then
                                If (Len(Trim(m_strHaendlerName)) > 0) And (Len(Trim(m_strHaendlerOrt)) > 0) Then
                                    filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "' AND CITY like '" &
                                        Replace(m_strHaendlerOrt, "'", "''") & "'"
                                ElseIf Len(Trim(m_strHaendlerName)) > 0 Then
                                    filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "'"
                                ElseIf Len(Trim(m_strHaendlerOrt)) > 0 Then
                                    filterExp = "CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
                                End If
                            End If

                            If filterExp.Length > 0 Then
                                filterExp = Replace(filterExp, "*", "%")
                                m_vwHaendler.RowFilter = filterExp
                                intReturn = m_vwHaendler.Count
                            End If
                            'Suche mit mehreren Ergebnissen
                        End If
                        m_vwHaendler.Sort = "NAME"
                    End If

                End If

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_WEB"
                        m_strErrorMessage = "Keine Web-Tabelle erstellt."
                        intReturn = 0
                    Case "NO_DATA"
                        m_strErrorMessage = "Keine Eingabedaten gefunden."
                        intReturn = 0
                    Case Else
                        m_strErrorMessage = ex.Message
                        intReturn = 0
                End Select
                ' Fehler-Login
                WriteLogEntry(False, "Suche nach Filiale """ & strTempFiliale & """ bzw. Händler """ & strTempReferenz &
                              """ nicht erfolgreich. (" & m_strErrorMessage & ")")
            Finally
                m_blnGestartet = False
            End Try

            Return intReturn
        End If

        Return -1
    End Function

    Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal strComment As String)
        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            Dim p_strType As String = "ERR"
            Dim p_strComment As String = strComment

            If blnSuccess Then
                p_strType = "DBG"
            End If

            m_objLogApp.WriteEntry(p_strType, m_objUser.UserName, m_strSessionID, CInt(m_strAppID),
                                   m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString,
                                   "Report", p_strComment, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)
        Catch ex As Exception
            m_objApp.WriteErrorText(1, m_objUser.UserName, "DADReports", "WriteLogEntry", ex.ToString)
        End Try
    End Sub

    Private Sub ResetHaendlerTabelle()
        m_tblHaendler = New DataTable()

        m_tblHaendler.Columns.Add("CUSTOMER", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("REFERENZ", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("FILIALE", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("NAME", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("NAME_2", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("CITY", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("STREET", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("DISPLAY", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))
    End Sub

    Private Sub ResetDistrictTable()
        m_districtTable = New DataTable()

        m_districtTable.Columns.Add("DISTRIKT", System.Type.GetType("System.String"))
        m_districtTable.Columns.Add("VORBELEGT", System.Type.GetType("System.String"))
        m_districtTable.Columns.Add("NAME1", System.Type.GetType("System.String"))
    End Sub

    Private Sub ResetFilialenTabelle()
        m_tblFilialen = New DataTable()

        m_tblFilialen.Columns.Add("CUSTOMER", System.Type.GetType("System.String"))
        m_tblFilialen.Columns.Add("FILIALE", System.Type.GetType("System.String"))
        m_tblFilialen.Columns.Add("NAME", System.Type.GetType("System.String"))
        m_tblFilialen.Columns.Add("NAME_2", System.Type.GetType("System.String"))
        m_tblFilialen.Columns.Add("CITY", System.Type.GetType("System.String"))
        m_tblFilialen.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
        m_tblFilialen.Columns.Add("STREET", System.Type.GetType("System.String"))
        m_tblFilialen.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
        m_tblFilialen.Columns.Add("DISPLAY_FILIALE", System.Type.GetType("System.String"))
    End Sub

    Public Function LeseAdressenSAP(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Web.UI.Page, ByVal strParentNode As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32 'HEZ: "B" oder "C"
        
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            m_tblSearchResult = New DataTable()
            m_tblSearchResult.Columns.Add("ADDRESSNUMBER", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))

            m_tblSearchResult.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("STREET", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("CITY", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("NAME", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("NAME_2", System.Type.GetType("System.String"))

            S.AP.Init("Bapi_Customer_Get_Children")

            S.AP.SetImportParameter("VALID_ON", Today.ToShortDateString)
            S.AP.SetImportParameter("NODE_LEVEL", nodelevel)
            S.AP.SetImportParameter("CUSTOMERNO", Right("0000000000" & strParentNode, 10))
            S.AP.SetImportParameter("CUSTHITYP", adressart)

            S.AP.Execute()

            Dim SAPReturnTableNODE_LIST As DataTable = S.AP.GetExportTable("NODE_LIST")

            For Each tmpRow1 As DataRow In SAPReturnTableNODE_LIST.Rows
                If tmpRow1("NODE_LEVEL").ToString.TrimStart("0"c) = "1" Then

                    'Die Detaildaten zu den Händlern in die Tabelle m_tblHaendler schreiben

                    S.AP.InitExecute("Bapi_Customer_Getdetail2", "CUSTOMERNO", tmpRow1("CUSTOMER").ToString)

                    Dim SAPReturnValue As DataTable = S.AP.GetExportTable("RETURN")
                    Dim SAPReturnCustomerDetail As DataTable = S.AP.GetExportTable("CUSTOMERGENERALDETAIL")
                    Dim SAPReturnCustomerAdress As DataTable = S.AP.GetExportTable("CUSTOMERADDRESS")

                    If SAPReturnValue.Rows(0)("TYPE").ToString.Trim(" "c) = "" Or
                        SAPReturnValue.Rows(0)("TYPE").ToString = "S" Or
                        SAPReturnValue.Rows(0)("TYPE").ToString = "I" Then

                        If (Not SAPReturnCustomerDetail.Rows(0)("Groupkey").ToString = m_objUser.Reference) Or
                            (SAPReturnCustomerDetail.Rows(0)("Groupkey").ToString.Length = 0 And m_objUser.Reference.Length = 0) Then

                            Dim newDealerDetailRow As DataRow = m_tblSearchResult.NewRow
                            Dim strTemp As String = SAPReturnCustomerAdress.Rows(0)("Name").ToString

                            If SAPReturnCustomerAdress.Rows(0)("Name_2").ToString.Length > 0 Then
                                strTemp &= ", " & SAPReturnCustomerAdress.Rows(0)("Name_2").ToString
                            End If

                            If SAPReturnCustomerAdress.Rows(0)("Name_3").ToString.Length > 0 Then
                                strTemp &= ", " & SAPReturnCustomerAdress.Rows(0)("Name_3").ToString
                            End If

                            If SAPReturnCustomerAdress.Rows(0)("Name_4").ToString.Length > 0 Then
                                strTemp &= ", " & SAPReturnCustomerAdress.Rows(0)("Name_4").ToString
                            End If

                            newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPReturnCustomerAdress.Rows(0)("Countryiso").ToString & " - " & SAPReturnCustomerAdress.Rows(0)("Postl_Code").ToString & " " & SAPReturnCustomerAdress.Rows(0)("City").ToString & ", " & SAPReturnCustomerAdress.Rows(0)("Street").ToString
                            newDealerDetailRow("ADDRESSNUMBER") = SAPReturnCustomerAdress.Rows(0)("Customer").ToString

                            If SAPReturnCustomerAdress.Rows(0)("Postl_Code").ToString.Length > 0 Then
                                newDealerDetailRow("POSTL_CODE") = SAPReturnCustomerAdress.Rows(0)("Postl_Code").ToString
                            End If

                            If SAPReturnCustomerAdress.Rows(0)("Street").ToString.Length > 0 Then
                                newDealerDetailRow("STREET") = SAPReturnCustomerAdress.Rows(0)("Street").ToString
                            End If

                            If SAPReturnCustomerAdress.Rows(0)("Countryiso").ToString.Length > 0 Then
                                newDealerDetailRow("COUNTRYISO") = SAPReturnCustomerAdress.Rows(0)("Countryiso").ToString
                            End If

                            If SAPReturnCustomerAdress.Rows(0)("City").ToString.Length > 0 Then
                                newDealerDetailRow("CITY") = SAPReturnCustomerAdress.Rows(0)("City").ToString
                            End If

                            If SAPReturnCustomerAdress.Rows(0)("Name").ToString.Length > 0 Then
                                newDealerDetailRow("NAME") = SAPReturnCustomerAdress.Rows(0)("Name").ToString
                            End If

                            If SAPReturnCustomerAdress.Rows(0)("Name_2").ToString.Length > 0 Then
                                newDealerDetailRow("NAME_2") = SAPReturnCustomerAdress.Rows(0)("Name_2").ToString
                            End If

                            m_tblSearchResult.Rows.Add(newDealerDetailRow)
                        End If
                    End If
                End If
            Next

            Return m_tblSearchResult.Rows.Count
        Catch ex As Exception
            Select Case ex.Message
                Case Else
                    m_strErrorMessage = "Es ist ein Fehler aufgetreten: " & ex.Message
                    m_intStatus = -3
            End Select
        End Try

        Return 0
    End Function
    
#End Region

End Class
' ************************************************
' $History: FFE_Search.vb $
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 10.03.10   Time: 14:25
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA: 2918
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 3.03.10    Time: 15:08
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 7.08.08    Time: 14:34
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA:2058
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 24.07.08   Time: 14:52
' Updated in $/CKAG/Applications/AppFFE/lib
' SAP connection Closed verbessert/hinzugefgt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugefgt
' 
' ************************************************
