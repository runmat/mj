Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Namespace Kernel.Common
    <Serializable()> Public Class Search
        REM § Lese-/Schreibfunktion, Kunde: FFD, 
        REM § LeseHaendlerSAP_Einzeln, LeseHaendlerSAP, LeseFilialenSAP - BAPI: Z_M_Adressdaten.


#Region " Definitions"
        Private m_strHaendlerReferenzNummer As String
        Private m_strHaendlerName As String
        Private m_strHaendlerOrt As String
        Private m_strHaendlerFiliale As String
        Private m_tblHaendler As DataTable
        Private m_tblFilialen As DataTable
        Private m_districtTable As DataTable
        Private m_strErrorMessage As String

        Protected m_intStatus As Int32
        Protected m_strMessage As String

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

        Dim tblRightsResult As DataTable
        Dim tblDistrictsResult As DataTable

        <NonSerialized()> Private m_strSessionID As String
        <NonSerialized()> Private m_strAppID As String
        <NonSerialized()> Private m_vwHaendler As DataView
        <NonSerialized()> Private m_districtView As DataView
        <NonSerialized()> Private m_vwFilialen As DataView
        <NonSerialized()> Protected m_objLogApp As Base.Kernel.Logging.Trace
        <NonSerialized()> Protected m_intIDSAP As Int32
        <NonSerialized()> Protected m_intStandardLogID As Int32
        <NonSerialized()> Protected m_strFileName As String
        <NonSerialized()> Protected m_strClassAndMethod As String

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
        Public Property Rights() As DataTable
            Get
                Return tblRightsResult
            End Get
            Set(ByVal Value As DataTable)
                tblRightsResult = Value
            End Set
        End Property

        Public ReadOnly Property Districts() As DataTable
            Get
                Return tblDistrictsResult
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

                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Adressdaten", m_objApp, m_objUser)

                    'm_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Adressdaten", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                    'myProxy.setImportParameter("I_KUNNR", InputReferenz)
                    'myProxy.setImportParameter("I_KNRZE", "")
                    'myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                    'myProxy.setImportParameter("I_VKORG", "1510")

                    'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                    S.AP.InitExecute("Z_M_Adressdaten", "I_KUNNR, I_KNRZE, I_KONZS, I_VKORG", InputReferenz, "", Right("0000000000" & m_objUser.Customer.KUNNR, 10), "1510")

                    'Dim SAPReturnTable As DataTable = myProxy.getExportTable("GT_WEB")
                    Dim SAPReturnTable As DataTable = S.AP.GetExportTable("GT_WEB")

                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    'End If

                    Select Case SAPReturnTable.Rows.Count
                        Case 0
                            ' SAP-Performance Logging
                            m_strErrorMessage = "Kein Suchergebnis."
                            'If m_intIDSAP > -1 Then
                            '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
                            'End If

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
                            'If m_intIDSAP > -1 Then
                            '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
                            'End If

                            blnReturn = False
                    End Select

                Catch ex As Exception
                    Select Case ex.Message
                        Case "NO_WEB"
                            m_strErrorMessage = "Keine Web-Tabelle erstellt."
                        Case "NO_DATA"
                            m_strErrorMessage = "Keine Eingabedaten gefunden."
                        Case Else
                            m_strErrorMessage = ex.Message
                    End Select

                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
                    'End If
                    'Rückgabewert setzen
                    blnReturn = False
                    ' Fehler Logging
                    WriteLogEntry(False, "Suche nach Händler """ & InputReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
                Finally
                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    'End If

                    m_blnGestartet = False
                End Try

                Return blnReturn
            End If
        End Function

        Public Function LeseFilialenSAP(Optional ByVal InputFiliale As String = "") As Int32
            ResetFilialenTabelle()
            ResetHaendlerTabelle()

            Dim strTempFiliale As String = InputFiliale

            Dim intReturn As Int32

            Try
                If InputFiliale.Length = 0 Then
                    strTempFiliale = m_strHaendlerFiliale
                End If
                Dim cn As New SqlClient.SqlConnection(m_objUser.App.Connectionstring)
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
                cn.Close()
                intReturn = m_tblFilialen.Rows.Count
            Catch ex As Exception
                m_strErrorMessage = "Keine Filialen für diesen Kunden <br>(" & ex.Message & ")."
                intReturn = 0
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
                Dim I As Int32

                Dim intReturn As Int32

                Try
                    m_strErrorMessage = ""

                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_Berechtigung_Distrikte", m_objApp, m_objUser)

                    'm_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_Berechtigung_Distrikte", appID, sessionID, m_objUser.CurrentLogAccessASPXID)

                    'myProxy.setImportParameter("ANWENDUNG", appID)
                    'myProxy.setImportParameter("BENGRP", m_objUser.UserID.ToString)
                    'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.Customer.KUNNR, 10))

                    'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    'End If

                    S.AP.InitExecute("Z_Berechtigung_Distrikte", "ANWENDUNG, BENGRP, KUNNR", appID, m_objUser.UserID.ToString, Right("0000000000" & m_objUser.Customer.KUNNR, 10))

                    'Dim SAPReturnTable As DataTable = myProxy.getExportTable("OUT_TAB")
                    Dim SAPReturnTable As DataTable = S.AP.GetExportTable("OUT_TAB")

                    intReturn = SAPReturnTable.Rows.Count
                    If SAPReturnTable.Rows.Count = 0 Then
                        m_strErrorMessage = "Kein Suchergebnis."
                        ' SAP-Performance Logging
                        'If m_intIDSAP > -1 Then
                        '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
                        'End If
                    Else
                        Dim newRow As DataRow
                        Dim dr As DataRow

                        For I = 0 To SAPReturnTable.Rows.Count - 1
                            dr = SAPReturnTable.Rows(I)

                            newRow = m_districtTable.NewRow()
                            newRow("DISTRIKT") = Right(dr("Distrikt").ToString, 6).TrimStart("0"c)
                            newRow("VORBELEGT") = dr("Vorbelegt").ToString
                            newRow("NAME1") = dr("Name1").ToString
                            m_districtTable.Rows.Add(newRow)
                        Next
                        m_districtView = m_districtTable.DefaultView
                        m_districtView.Sort = "NAME1"
                    End If

                Catch ex As Exception
                    Select Case ex.Message
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
                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
                    'End If
                    ' Fehler-Logging
                    WriteLogEntry(False, "Suche nach Distrikten nicht erfolgreich. (" & m_strErrorMessage & ")")
                Finally
                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    'End If

                    m_blnGestartet = False
                End Try
                Return intReturn
            End If
        End Function

        Public Function LeseHaendlerSAP(ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal InputReferenz As String = "", Optional ByVal InputFiliale As String = "") As Int32
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
                Dim strTempName As String = ""
                Dim strOrt As String = ""
                Dim intReturn As Int32

                Try
                    m_strErrorMessage = ""

                    If InputFiliale.Length = 0 And InputReferenz.Length = 0 Then
                        strTempFiliale = m_strHaendlerFiliale
                        strTempReferenz = m_strHaendlerReferenzNummer
                    End If

                    If ((strTempFiliale.Length = 0 And strTempReferenz.Length = 0 And m_strHaendlerName.Length = 0 And m_strHaendlerOrt.Length = 0) And (m_objUser.Organization.AllOrganizations = False)) Then
                        m_strErrorMessage = "Händler- oder Filialnummer müssen gefüllt sein."
                        intReturn = -1
                    Else

                        If strTempFiliale = "ALL" Then  ' FFD Regionalzuordnung jeder solle alle Händler sehen 
                            strTempFiliale = ""         ' lt. Braasch 29.02.2008
                        End If


                        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Adressdaten", m_objApp, m_objUser)

                        'myProxy.setImportParameter("I_KUNNR", strTempReferenz)
                        'myProxy.setImportParameter("I_KNRZE", strTempFiliale)
                        'myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                        'myProxy.setImportParameter("I_VKORG", "1510")

                        'm_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Adressdaten", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                        'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                        S.AP.InitExecute("Z_M_Adressdaten", "I_KUNNR, I_KNRZE, I_KONZS, I_VKORG",
                                         strTempReferenz, strTempFiliale, Right("0000000000" & m_objUser.Customer.KUNNR, 10), "1510")

                        'Dim SAPReturnTable As DataTable = myProxy.getExportTable("GT_WEB")
                        Dim SAPReturnTable As DataTable = S.AP.GetExportTable("GT_WEB")

                        ' SAP-Performance Logging
                        'If m_intIDSAP > -1 Then
                        '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                        'End If

                        intReturn = SAPReturnTable.Rows.Count
                        If intReturn = 0 Then
                            m_strErrorMessage = "Kein Suchergebnis."
                            ' SAP-Performance Logging
                            'If m_intIDSAP > -1 Then
                            '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
                            'End If
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
                                    newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & dr("Name2").ToString & ", " & dr("Land1").ToString & "-" & dr("Pstlz").ToString & " " & dr("Ort01").ToString & ", " & dr("Stras").ToString
                                Else
                                    newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & dr("Land1").ToString & "-" & dr("Pstlz").ToString & " " & dr("Ort01").ToString & ", " & dr("Stras").ToString
                                End If

                                m_tblHaendler.Rows.Add(newDealerDetailRow)
                            Next
                            m_vwHaendler = m_tblHaendler.DefaultView
                            If SAPReturnTable.Rows.Count > 1 Then
                                'Weitere Auswahl entspechend Name und/oder Ort
                                Dim filterExp As String = ""
                                If m_strHaendlerReferenzNummer.Trim(" "c).Length = 0 Then
                                    If (Len(Trim(m_strHaendlerName)) > 0) And (Len(Trim(m_strHaendlerOrt)) > 0) Then
                                        filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "' AND CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
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
                    Select Case ex.Message
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
                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
                    'End If
                    ' Fehler-Login
                    WriteLogEntry(False, "Suche nach Filiale """ & strTempFiliale & """ bzw. Händler """ & strTempReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
                Finally
                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    'End If

                    m_blnGestartet = False
                End Try
                Return intReturn
            End If
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
                m_objLogApp.WriteEntry(p_strType, m_objUser.UserName, m_strSessionID, CInt(m_strAppID), m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString, "Report", p_strComment, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)
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

        Public Function LeseAdressenSAP(ByVal strParentNode As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32 'HEZ: "B" oder "C"
            Try
                Dim tblTemp As DataTable
                m_tblSearchResult = New DataTable()
                m_tblSearchResult.Columns.Add("ADDRESSNUMBER", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))

                m_tblSearchResult.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("STREET", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("CITY", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("NAME", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("NAME_2", System.Type.GetType("System.String"))

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Bapi_Customer_Get_Children", m_objApp, m_objUser)

                'myProxy.setImportParameter("CUSTHITYP", adressart)
                'myProxy.setImportParameter("CUSTOMERNO", Right("0000000060" & strParentNode, 10))
                'myProxy.setImportParameter("NODE_LEVEL", nodelevel)
                'myProxy.setImportParameter("VALID_ON", Date.Today.ToString)

                'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                S.AP.InitExecute("Bapi_Customer_Get_Children", "CUSTHITYP, CUSTOMERNO, NODE_LEVEL, VALID_ON",
                                 adressart, Right("0000000060" & strParentNode, 10), nodelevel, Date.Today.ToString)

                'tblTemp = myProxy.getExportTable("NODE_LIST")
                tblTemp = S.AP.GetExportTable("NODE_LIST")

                Dim SAPTableRow As DataRow

                Dim newDealerDetailRow As DataRow

                Dim SAPCustomerAdress As DataTable
                Dim SAPCustomerCompanyDetail As DataTable
                Dim SAPCustomerDetail As DataTable
                Dim SAPBapiRet1 As DataTable
                Dim SAPCustomerBankDetail As DataTable
                Dim SAPBapiRet1Row As DataRow
                Dim SAPCustomerAdressRow As DataRow
                Dim SAPCustomerDetailRow As DataRow

                For Each SAPTableRow In tblTemp.Rows
                    'Der Händler soll sich nicht selbst zur Auswahl bekommen!!!
                    If SAPTableRow("NODE_LEVEL").ToString.TrimStart("0"c) = "1" Then

                        'Die Detaildaten zu den Händlern in die Tabelle m_tblHaendler schreiben
                        'myProxy = DynSapProxy.getProxyNoPage("Bapi_Customer_Getdetail2", m_objApp, m_objUser)

                        'myProxy.setImportParameter("CUSTOMERNO", SAPTableRow("CUSTOMER").ToString)
                        'myProxy.setImportParameter("COMPANYCODE", "")

                        'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                        S.AP.InitExecute("Bapi_Customer_Getdetail2", "CUSTOMERNO, COMPANYCODE", SAPTableRow("CUSTOMER").ToString, "")

                        'SAPCustomerAdress = myProxy.getExportTable("CUSTOMERADDRESS")
                        'SAPCustomerCompanyDetail = myProxy.getExportTable("CUSTOMERCOMPANYDETAIL")
                        'SAPCustomerDetail = myProxy.getExportTable("CUSTOMERGENERALDETAIL")
                        'SAPBapiRet1 = myProxy.getExportTable("RETURN")
                        'SAPCustomerBankDetail = myProxy.getExportTable("CUSTOMERBANKDETAIL")

                        SAPCustomerAdress = S.AP.GetExportTable("CUSTOMERADDRESS")
                        SAPCustomerCompanyDetail = S.AP.GetExportTable("CUSTOMERCOMPANYDETAIL")
                        SAPCustomerDetail = S.AP.GetExportTable("CUSTOMERGENERALDETAIL")
                        SAPBapiRet1 = S.AP.GetExportTable("RETURN")
                        SAPCustomerBankDetail = S.AP.GetExportTable("CUSTOMERBANKDETAIL")

                        SAPBapiRet1Row = SAPBapiRet1.Rows(0)
                        SAPCustomerAdressRow = SAPCustomerAdress.Rows(0)
                        SAPCustomerDetailRow = SAPCustomerDetail.Rows(0)

                        If SAPBapiRet1Row("Type").ToString.Trim(" "c) = "" Or SAPBapiRet1Row("Type").ToString = "S" Or SAPBapiRet1Row("Type").ToString = "I" Then
                            If (Not SAPCustomerDetailRow("Groupkey").ToString = m_objUser.Reference) Or (SAPCustomerDetailRow("Groupkey").ToString.Length = 0 And m_objUser.Reference.Length = 0) Then
                                newDealerDetailRow = m_tblSearchResult.NewRow

                                Dim strTemp As String = SAPCustomerAdressRow("Name").ToString
                                If SAPCustomerAdressRow("Name_2").ToString.Length > 0 Then
                                    strTemp &= ", " & SAPCustomerAdressRow("Name_2").ToString
                                End If
                                If SAPCustomerAdressRow("Name_3").ToString.Length > 0 Then
                                    strTemp &= ", " & SAPCustomerAdressRow("Name_3").ToString
                                End If
                                If SAPCustomerAdressRow("Name_4").ToString.Length > 0 Then
                                    strTemp &= ", " & SAPCustomerAdressRow("Name_4").ToString
                                End If

                                newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPCustomerAdressRow("Countryiso").ToString & " - " & SAPCustomerAdressRow("Postl_Code").ToString & " " & SAPCustomerAdressRow("City").ToString & ", " & SAPCustomerAdressRow("Street").ToString
                                newDealerDetailRow("ADDRESSNUMBER") = SAPCustomerDetailRow("Customer").ToString
                                If SAPCustomerAdressRow("Postl_Code").ToString.Length > 0 Then
                                    newDealerDetailRow("POSTL_CODE") = SAPCustomerAdressRow("Postl_Code").ToString
                                End If
                                If SAPCustomerAdressRow("Street").ToString.Length > 0 Then
                                    newDealerDetailRow("STREET") = SAPCustomerAdressRow("Street").ToString
                                End If
                                If SAPCustomerAdressRow("Countryiso").ToString.Length > 0 Then
                                    newDealerDetailRow("COUNTRYISO") = SAPCustomerAdressRow("Countryiso").ToString
                                End If
                                If SAPCustomerAdressRow("City").ToString.Length > 0 Then
                                    newDealerDetailRow("CITY") = SAPCustomerAdressRow("City").ToString
                                End If
                                If SAPCustomerAdressRow("Name").ToString.Length > 0 Then
                                    newDealerDetailRow("NAME") = SAPCustomerAdressRow("Name").ToString
                                End If
                                If SAPCustomerAdressRow("Name_2").ToString.Length > 0 Then
                                    newDealerDetailRow("NAME_2") = SAPCustomerAdressRow("Name_2").ToString
                                End If

                                m_tblSearchResult.Rows.Add(newDealerDetailRow)
                            End If
                        End If
                    End If
                Next

                Return m_tblSearchResult.Rows.Count
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function Show(ByVal sUserID As String, ByVal sKunnr As String) As Int32
            m_strClassAndMethod = "SAPProxy_Base.GetRights"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
                    m_intStatus = 0
                    m_strMessage = ""

                    'm_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_Berechtigung_Anzeigen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_Berechtigung_Anzeigen", m_objApp, m_objUser)

                    'myProxy.setImportParameter("KUNNR", sKunnr)
                    'myProxy.setImportParameter("BENGRP", sUserID)

                    'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    'End If

                    S.AP.InitExecute("Z_Berechtigung_Anzeigen", "KUNNR, BENGRP", sKunnr, sUserID)

                    'tblRightsResult = myProxy.getExportTable("OUTPUT")
                    'Dim count As Integer = tblRightsResult.Rows.Count
                    'tblDistrictsResult = myProxy.getExportTable("DISTRIKTE")

                    tblRightsResult = S.AP.GetExportTable("OUTPUT")
                    Dim count As Integer = tblRightsResult.Rows.Count
                    tblDistrictsResult = S.AP.GetExportTable("DISTRIKTE")

                    WriteLogEntry(True, "Berechtigungen eingelesen")
                Catch ex As Exception
                    Select Case ex.Message
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    'End If
                    WriteLogEntry(False, "Fehler beim Einlesen der Berechtigungen")
                Finally
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    'End If

                    m_blnGestartet = False
                End Try
            End If
        End Function


        Public Function Change() As Int32

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_Berechtigung_Anlegen", m_objApp, m_objUser)
                S.AP.Init("Z_Berechtigung_Anlegen")

                'Dim sapTable As DataTable = myProxy.getImportTable("INPUT")
                Dim sapTable As DataTable = S.AP.GetImportTable("INPUT")

                Dim sapTableRow As DataRow

                For Each row As DataRow In tblRightsResult.Rows
                    sapTableRow = sapTable.NewRow

                    sapTableRow("Anwendung") = CType(row.Item("Anwendung"), String)
                    sapTableRow("Bengrp") = row.Item("Bengrp").ToString
                    sapTableRow("Distrikt") = row.Item("Distrikt").ToString
                    sapTableRow("Kunnr") = row.Item("Kunnr").ToString
                    sapTableRow("Mandt") = row.Item("Mandt").ToString
                    sapTableRow("Vorbelegt") = row.Item("Vorbelegt").ToString
                    sapTableRow("Loekz") = row.Item("Loekz").ToString

                    sapTable.Rows.Add(sapTableRow)
                Next

                'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)
                S.AP.Execute()

                m_intStatus = 0
                m_strMessage = ""

                'If m_intIDSAP > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                'End If
            Catch ex As Exception
                Select Case ex.Message

                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                'If m_intIDSAP > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                'End If
            End Try
        End Function
#End Region

        Public Sub New()

        End Sub
    End Class
End Namespace

' ************************************************
' $History: Search.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Common
' Migration OR
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Base/Kernel/Common
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Kernel/Common
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Kernel/Common
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.05.07   Time: 15:29
' Created in $/CKG/Base/Base/Kernel/Common
' Änderungen aus StartApplication vom 11.05.2007
' 
' ************************************************
