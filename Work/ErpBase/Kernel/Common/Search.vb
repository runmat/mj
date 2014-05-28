Option Explicit On
Option Infer On
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
        <NonSerialized()> Protected m_objLogApp As Logging.Trace
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
        Public Sub New(ByRef objApp As Security.App, ByRef objUser As Security.User, ByVal strSessionID As String, ByVal strAppID As String)
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

        'Public Function LeseHaendlerSAP_Einzeln(ByVal strAppID As String, ByVal strSessionID As String, ByVal InputReferenz As String) As Boolean
        '      If Not m_blnGestartet Then
        '          m_blnGestartet = True

        '          m_strCUSTOMER = ""
        '          m_strNAME = ""
        '          m_strNAME_2 = ""
        '          m_strCOUNTRYISO = ""
        '          m_strPOSTL_CODE = ""
        '          m_strCITY = ""
        '          m_strSTREET = ""
        '          m_strREFERENZ = ""
        '          m_strFILIALE = ""

        '          Dim blnReturn As Boolean = False

        '          If _logApp Is Nothing Then
        '              _logApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '          End If
        '          IDSAP = -1

        '          Try
        '              m_strErrorMessage = ""

        '              objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)

        '              Dim SAPReturnTable As New SAPProxy_Base.ZDAD_M_WEB_ADRESSENTable()
        '              Dim SAPReturnTableRow As New SAPProxy_Base.ZDAD_M_WEB_ADRESSEN()

        '              IDSAP = _logApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Adressdaten", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
        '              objSAP.Z_M_Adressdaten("", Right("0000000000" & m_objUser.Customer.KUNNR, 10), InputReferenz, "1510", SAPReturnTable)
        '              If IDSAP > -1 Then
        '                  _logApp.WriteEndDataAccessSAP(IDSAP, True)
        '              End If

        '              Select Case SAPReturnTable.Count
        '                  Case 0
        '                      m_strErrorMessage = "Kein Suchergebnis."
        '                      If IDSAP > -1 Then
        '                          _logApp.WriteEndDataAccessSAP(IDSAP, False, m_strErrorMessage)
        '                      End If

        '                      blnReturn = False
        '                  Case 1
        '                      SAPReturnTableRow = SAPReturnTable.Item(0)

        '                      m_strCUSTOMER = SAPReturnTableRow.Konzs.TrimStart("0"c)
        '                      m_strREFERENZ = Right(SAPReturnTableRow.Kunnr, 6).TrimStart("0"c)
        '                      m_strFILIALE = Right(SAPReturnTableRow.Knrze, 6).TrimStart("0"c)
        '                      m_strNAME = SAPReturnTableRow.Name1
        '                      m_strNAME_2 = SAPReturnTableRow.Name2
        '                      m_strCITY = SAPReturnTableRow.Ort01
        '                      m_strPOSTL_CODE = SAPReturnTableRow.Pstlz
        '                      m_strSTREET = SAPReturnTableRow.Stras
        '                      m_strCOUNTRYISO = SAPReturnTableRow.Land1

        '                      blnReturn = True
        '                  Case Else
        '                      m_strErrorMessage = "Suchergebnis nicht eindeutig."
        '                      If IDSAP > -1 Then
        '                          _logApp.WriteEndDataAccessSAP(IDSAP, False, m_strErrorMessage)
        '                      End If

        '                      blnReturn = False
        '              End Select

        '              If m_strErrorMessage.Length = 0 Then
        '                  WriteLogEntry(True, "Suche nach Händler """ & InputReferenz & """ erfolgreich.")
        '              Else
        '                  WriteLogEntry(False, "Suche nach Händler """ & InputReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
        '              End If
        '          Catch ex As Exception
        '              Select Case ex.Message
        '                  Case "NO_WEB"
        '                      m_strErrorMessage = "Keine Web-Tabelle erstellt."
        '                  Case "NO_DATA"
        '                      m_strErrorMessage = "Keine Eingabedaten gefunden."
        '                  Case Else
        '                      m_strErrorMessage = ex.Message
        '              End Select
        '              If IDSAP > -1 Then
        '                  _logApp.WriteEndDataAccessSAP(IDSAP, False, m_strErrorMessage)
        '              End If
        '              blnReturn = False
        '              WriteLogEntry(False, "Suche nach Händler """ & InputReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
        '          Finally
        '              If IDSAP > -1 Then
        '                  _logApp.WriteStandardDataAccessSAP(IDSAP)
        '              End If

        '              objSAP.Connection.Close()
        '              objSAP.Dispose()

        '              m_blnGestartet = False
        '          End Try

        '          Return blnReturn
        '      End If
        '  End Function

        '  Public Function LeseFilialenSAP(Optional ByVal InputFiliale As String = "") As Int32
        '      ResetFilialenTabelle()
        '      ResetHaendlerTabelle()

        '      Dim strTempFiliale As String = InputFiliale

        '      Dim intReturn As Int32

        '      Try
        '          If InputFiliale.Length = 0 Then
        '              strTempFiliale = m_strHaendlerFiliale
        '          End If
        '          Dim cn As New SqlClient.SqlConnection(m_objUser.App.Connectionstring)
        '          cn.Open()
        '          If strTempFiliale.Trim(" "c).Length = 0 Then
        '              Dim da As New SqlClient.SqlDataAdapter("SELECT OrganizationReference AS FILIALE, OrganizationName AS DISPLAY_FILIALE FROM Organization WHERE OrganizationReference <> '' AND OrganizationReference <> '999' AND CustomerID=@CustomerID", cn)
        '              da.SelectCommand.Parameters.AddWithValue("@CustomerID", m_objUser.Customer.CustomerId)
        '              da.Fill(m_tblFilialen)
        '          Else
        '              Dim da As New SqlClient.SqlDataAdapter("SELECT OrganizationReference AS FILIALE, OrganizationName AS DISPLAY_FILIALE FROM Organization WHERE OrganizationReference=@OrganizationReference AND CustomerID=@CustomerID", cn)
        '              da.SelectCommand.Parameters.AddWithValue("@CustomerID", m_objUser.Customer.CustomerId)
        '              da.SelectCommand.Parameters.AddWithValue("@OrganizationReference", strTempFiliale)
        '              da.Fill(m_tblFilialen)
        '          End If
        '          cn.Close()
        '          intReturn = m_tblFilialen.Rows.Count
        '      Catch ex As Exception
        '          m_strErrorMessage = "Keine Filialen für diesen Kunden <br>(" & ex.Message & ")."
        '          intReturn = 0
        '      End Try
        '      Return intReturn
        '  End Function

        '  Public Function ReadDistrictSAP(ByVal appID As String, ByVal sessionID As String) As Int32
        '      If Not m_blnGestartet Then
        '          m_blnGestartet = True

        '          Dim objSAP As New SAPProxy_Base.SAPProxy_Base()

        '          ResetDistrictTable()

        '          If _logApp Is Nothing Then
        '              _logApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '          End If
        '          IDSAP = -1
        '          Dim I As Int32

        '          Dim intReturn As Int32

        '          Try
        '              m_strErrorMessage = ""

        '              objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)

        '              Dim SAPReturnTable As New SAPProxy_Base.ZDADDISTRIKTE_STRUCTTable()
        '              Dim SAPReturnTableRow As New SAPProxy_Base.ZDADDISTRIKTE_STRUCT()

        '              IDSAP = _logApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_Berechtigung_Distrikte", appID, sessionID, m_objUser.CurrentLogAccessASPXID)
        '              objSAP.Z_Berechtigung_Distrikte(appID, m_objUser.UserID.ToString, Right("0000000000" & m_objUser.Customer.KUNNR, 10), SAPReturnTable)
        '              If IDSAP > -1 Then
        '                  _logApp.WriteEndDataAccessSAP(IDSAP, True)
        '              End If

        '              intReturn = SAPReturnTable.Count
        '              If SAPReturnTable.Count = 0 Then
        '                  m_strErrorMessage = "Kein Suchergebnis."
        '                  'Suche ohne Ergebnis
        '                  If IDSAP > -1 Then
        '                      _logApp.WriteEndDataAccessSAP(IDSAP, False, m_strErrorMessage)
        '                  End If
        '              Else
        '                  Dim newRow As DataRow

        '                  For I = 0 To SAPReturnTable.Count - 1
        '                      SAPReturnTableRow = SAPReturnTable.Item(I)

        '                      newRow = m_districtTable.NewRow()
        '                      newRow("DISTRIKT") = Right(SAPReturnTableRow.Distrikt, 6).TrimStart("0"c)
        '                      newRow("VORBELEGT") = SAPReturnTableRow.Vorbelegt
        '                      newRow("NAME1") = SAPReturnTableRow.Name1
        '                      m_districtTable.Rows.Add(newRow)
        '                  Next
        '                  m_districtView = m_districtTable.DefaultView
        '                  m_districtView.Sort = "NAME1"
        '              End If

        '              If m_strErrorMessage.Length = 0 Then
        '                  WriteLogEntry(True, "Suche nach Distrikten erfolgreich.")
        '              Else
        '                  WriteLogEntry(False, "Suche nach Distrikten nicht erfolgreich. (" & m_strErrorMessage & ")")
        '              End If


        '          Catch ex As Exception
        '              Select Case ex.Message
        '                  Case "NO_WEB"
        '                      m_strErrorMessage = "Keine Web-Tabelle erstellt."
        '                      intReturn = 0
        '                  Case "NO_DATA"
        '                      m_strErrorMessage = "Keine Eingabedaten gefunden."
        '                      intReturn = 0
        '                  Case Else
        '                      m_strErrorMessage = ex.Message
        '                      intReturn = 0
        '              End Select
        '              If IDSAP > -1 Then
        '                  _logApp.WriteEndDataAccessSAP(IDSAP, False, m_strErrorMessage)
        '              End If
        '              WriteLogEntry(False, "Suche nach Distrikten nicht erfolgreich. (" & m_strErrorMessage & ")")
        '          Finally
        '              If IDSAP > -1 Then
        '                  _logApp.WriteStandardDataAccessSAP(IDSAP)
        '              End If

        '              objSAP.Connection.Close()
        '              objSAP.Dispose()

        '              m_blnGestartet = False
        '          End Try
        '          Return intReturn
        '      End If
        '  End Function

        '  Public Function LeseHaendlerSAP(ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal InputReferenz As String = "", Optional ByVal InputFiliale As String = "") As Int32
        '      If Not m_blnGestartet Then
        '          m_blnGestartet = True

        '          Dim objSAP As New SAPProxy_Base.SAPProxy_Base()

        '          ResetHaendlerTabelle()

        '          If _logApp Is Nothing Then
        '              _logApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '          End If
        '          IDSAP = -1
        '          Dim I As Int32
        '          Dim strTempFiliale As String = InputFiliale
        '          Dim strTempReferenz As String = InputReferenz

        '          Dim intReturn As Int32

        '          Try
        '              m_strErrorMessage = ""

        '              If InputFiliale.Length = 0 And InputReferenz.Length = 0 Then
        '                  strTempFiliale = m_strHaendlerFiliale
        '                  strTempReferenz = m_strHaendlerReferenzNummer
        '              End If
        '              '§§§JVE <14.11.2005 drittes Kriterium eingefügt...>
        '              If ((strTempFiliale.Length = 0 And strTempReferenz.Length = 0) And (m_objUser.Organization.AllOrganizations = False)) Then
        '                  m_strErrorMessage = "Händler- oder Filialnummer müssen gefüllt sein."
        '                  intReturn = -1
        '              Else
        '                  objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)

        '                  Dim SAPReturnTable As New SAPProxy_Base.ZDAD_M_WEB_ADRESSENTable()
        '                  Dim SAPReturnTableRow As New SAPProxy_Base.ZDAD_M_WEB_ADRESSEN()

        '                  IDSAP = _logApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Adressdaten", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
        '                  objSAP.Z_M_Adressdaten(strTempFiliale, Right("0000000000" & m_objUser.Customer.KUNNR, 10), strTempReferenz, "1510", SAPReturnTable)
        '                  If IDSAP > -1 Then
        '                      _logApp.WriteEndDataAccessSAP(IDSAP, True)
        '                  End If

        '                  intReturn = SAPReturnTable.Count
        '                  If SAPReturnTable.Count = 0 Then
        '                      m_strErrorMessage = "Kein Suchergebnis."
        '                      'Suche ohne Ergebnis
        '                      If IDSAP > -1 Then
        '                          _logApp.WriteEndDataAccessSAP(IDSAP, False, m_strErrorMessage)
        '                      End If
        '                  Else
        '                      Dim newDealerDetailRow As DataRow

        '                      For I = 0 To SAPReturnTable.Count - 1
        '                          SAPReturnTableRow = SAPReturnTable.Item(I)

        '                          newDealerDetailRow = m_tblHaendler.NewRow()
        '                          newDealerDetailRow("CUSTOMER") = SAPReturnTableRow.Konzs.TrimStart("0"c)

        '                          newDealerDetailRow("REFERENZ") = Right(SAPReturnTableRow.Kunnr, 6).TrimStart("0"c)
        '                          newDealerDetailRow("FILIALE") = Right(SAPReturnTableRow.Knrze, 6).TrimStart("0"c)

        '                          newDealerDetailRow("NAME") = SAPReturnTableRow.Name1
        '                          newDealerDetailRow("NAME_2") = SAPReturnTableRow.Name2
        '                          newDealerDetailRow("CITY") = SAPReturnTableRow.Ort01
        '                          newDealerDetailRow("POSTL_CODE") = SAPReturnTableRow.Pstlz
        '                          newDealerDetailRow("STREET") = SAPReturnTableRow.Stras
        '                          newDealerDetailRow("COUNTRYISO") = SAPReturnTableRow.Land1
        '                          newDealerDetailRow("DISPLAY") = Right(SAPReturnTableRow.Kunnr, 6).TrimStart("0"c) & " - " & SAPReturnTableRow.Name1 & ", " & SAPReturnTableRow.Ort01
        '                          newDealerDetailRow("DISPLAY_ADDRESS") = SAPReturnTableRow.Name1
        '                          If Not SAPReturnTableRow.Name2.Trim(" "c).Length = 0 Then
        '                              newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow.Name2 & ", " & SAPReturnTableRow.Land1 & "-" & SAPReturnTableRow.Pstlz & " " & SAPReturnTableRow.Ort01 & ", " & SAPReturnTableRow.Stras
        '                          Else
        '                              newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow.Land1 & "-" & SAPReturnTableRow.Pstlz & " " & SAPReturnTableRow.Ort01 & ", " & SAPReturnTableRow.Stras
        '                          End If

        '                          m_tblHaendler.Rows.Add(newDealerDetailRow)
        '                      Next
        '                      m_vwHaendler = m_tblHaendler.DefaultView
        '                      If SAPReturnTable.Count > 1 Then
        '                          'Weitere Auswahl entspechend Name und/oder Ort
        '                          Dim filterExp As String = ""

        '                          If m_strHaendlerReferenzNummer.Trim(" "c).Length = 0 Then
        '                              If (Len(Trim(m_strHaendlerName)) > 0) And (Len(Trim(m_strHaendlerOrt)) > 0) Then
        '                                  filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "' AND CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
        '                              ElseIf Len(Trim(m_strHaendlerName)) > 0 Then
        '                                  filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "'"
        '                              ElseIf Len(Trim(m_strHaendlerOrt)) > 0 Then
        '                                  filterExp = "CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
        '                              End If
        '                          End If

        '                          If filterExp.Length > 0 Then
        '                              filterExp = Replace(filterExp, "*", "%")
        '                              m_vwHaendler.RowFilter = filterExp
        '                              intReturn = m_vwHaendler.Count
        '                          End If
        '                          'Suche mit mehreren Ergebnissen
        '                      End If
        '                      m_vwHaendler.Sort = "NAME"
        '                  End If

        '              End If

        '              If m_strErrorMessage.Length = 0 Then
        '                  WriteLogEntry(True, "Suche nach Filiale """ & strTempFiliale & """ bzw. Händler """ & strTempReferenz & """ erfolgreich.")
        '              Else
        '                  WriteLogEntry(False, "Suche nach Filiale """ & strTempFiliale & """ bzw. Händler """ & strTempReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
        '              End If

        '          Catch ex As Exception
        '              Select Case ex.Message
        '                  Case "NO_WEB"
        '                      m_strErrorMessage = "Keine Web-Tabelle erstellt."
        '                      intReturn = 0
        '                  Case "NO_DATA"
        '                      m_strErrorMessage = "Keine Eingabedaten gefunden."
        '                      intReturn = 0
        '                  Case Else
        '                      m_strErrorMessage = ex.Message
        '                      intReturn = 0
        '              End Select
        '              If IDSAP > -1 Then
        '                  _logApp.WriteEndDataAccessSAP(IDSAP, False, m_strErrorMessage)
        '              End If
        '              WriteLogEntry(False, "Suche nach Filiale """ & strTempFiliale & """ bzw. Händler """ & strTempReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
        '          Finally
        '              If IDSAP > -1 Then
        '                  _logApp.WriteStandardDataAccessSAP(IDSAP)
        '              End If

        '              objSAP.Connection.Close()
        '              objSAP.Dispose()

        '              m_blnGestartet = False
        '          End Try
        '          Return intReturn
        '      End If
        '  End Function

        Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal strComment As String)
            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
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

            m_tblHaendler.Columns.Add("CUSTOMER", GetType(String))
            m_tblHaendler.Columns.Add("REFERENZ", GetType(String))
            m_tblHaendler.Columns.Add("FILIALE", GetType(String))
            m_tblHaendler.Columns.Add("NAME", GetType(String))
            m_tblHaendler.Columns.Add("NAME_2", GetType(String))
            m_tblHaendler.Columns.Add("CITY", GetType(String))
            m_tblHaendler.Columns.Add("POSTL_CODE", GetType(String))
            m_tblHaendler.Columns.Add("STREET", GetType(String))
            m_tblHaendler.Columns.Add("COUNTRYISO", GetType(String))
            m_tblHaendler.Columns.Add("DISPLAY", GetType(String))
            m_tblHaendler.Columns.Add("DISPLAY_ADDRESS", GetType(String))
        End Sub

        Private Sub ResetDistrictTable()
            m_districtTable = New DataTable()

            m_districtTable.Columns.Add("DISTRIKT", GetType(String))
            m_districtTable.Columns.Add("VORBELEGT", GetType(String))
            m_districtTable.Columns.Add("NAME1", GetType(String))
        End Sub

        Private Sub ResetFilialenTabelle()
            m_tblFilialen = New DataTable()

            m_tblFilialen.Columns.Add("CUSTOMER", GetType(String))
            m_tblFilialen.Columns.Add("FILIALE", GetType(String))
            m_tblFilialen.Columns.Add("NAME", GetType(String))
            m_tblFilialen.Columns.Add("NAME_2", GetType(String))
            m_tblFilialen.Columns.Add("CITY", GetType(String))
            m_tblFilialen.Columns.Add("POSTL_CODE", GetType(String))
            m_tblFilialen.Columns.Add("STREET", GetType(String))
            m_tblFilialen.Columns.Add("COUNTRYISO", GetType(String))
            m_tblFilialen.Columns.Add("DISPLAY_FILIALE", GetType(String))
        End Sub

        'Public Function LeseAdressenSAP(ByVal strParentNode As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32 'HEZ: "B" oder "C"
        '    Dim objSAP As New SAPProxy_Base.SAPProxy_Base()

        '    Try
        '        Dim tblTemp As DataTable
        '        m_tblSearchResult = New DataTable()
        '        m_tblSearchResult.Columns.Add("ADDRESSNUMBER", GetType(String))
        '        m_tblSearchResult.Columns.Add("DISPLAY_ADDRESS", GetType(String))

        '        m_tblSearchResult.Columns.Add("POSTL_CODE", GetType(String))
        '        m_tblSearchResult.Columns.Add("STREET", GetType(String))
        '        m_tblSearchResult.Columns.Add("COUNTRYISO", GetType(String))
        '        m_tblSearchResult.Columns.Add("CITY", GetType(String))
        '        m_tblSearchResult.Columns.Add("NAME", GetType(String))
        '        m_tblSearchResult.Columns.Add("NAME_2", GetType(String))

        '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)

        '        Dim SAPTable As New SAPProxy_Base.BAPIKNA1_KNVHTable()
        '        Dim SAPReturnTable As New SAPProxy_Base.BAPIRET2()
        '        Dim SAPSalesTable As New SAPProxy_Base.BAPI_SDVTBERTable()
        '        Dim SAPCustomerDetail As New SAPProxy_Base.BAPICUSTOMER_KNA1()
        '        Dim SAPCustomerAdress As New SAPProxy_Base.BAPICUSTOMER_04()
        '        Dim SAPCustomerCompanyDetail As New SAPProxy_Base.BAPICUSTOMER_05()
        '        Dim SAPBapiRet1 As New SAPProxy_Base.BAPIRET1()
        '        Dim SAPCustomerBankDetail As New SAPProxy_Base.BAPICUSTOMER_02Table()

        '        objSAP.Bapi_Customer_Get_Children(adressart, Right("0000000060" & strParentNode, 10), nodelevel, Format(Now, "yyyyMMdd"), SAPReturnTable, SAPTable, SAPSalesTable)

        '        tblTemp = SAPTable.ToADODataTable

        '        Dim SAPTableRow As DataRow

        '        Dim newDealerDetailRow As DataRow

        '        For Each SAPTableRow In tblTemp.Rows
        '            'Der Händler soll sich nicht selbst zur Auswahl bekommen!!!
        '            If SAPTableRow("NODE_LEVEL").ToString.TrimStart("0"c) = "1" Then
        '                'If Not SAPTableRow("CUSTOMER").ToString.TrimStart("0"c) = strParentNode.TrimStart("0"c) Then

        '                'Die Detaildaten zu den Händlern in die Tabelle m_tblHaendler schreiben
        '                objSAP.Bapi_Customer_Getdetail2("", SAPTableRow("CUSTOMER").ToString, SAPCustomerAdress, SAPCustomerCompanyDetail, SAPCustomerDetail, SAPBapiRet1, SAPCustomerBankDetail)

        '                If SAPBapiRet1.Type.Trim(" "c) = "" Or SAPBapiRet1.Type = "S" Or SAPBapiRet1.Type = "I" Then
        '                    If (Not SAPCustomerDetail.Groupkey = m_objUser.Reference) Or (SAPCustomerDetail.Groupkey.Length = 0 And m_objUser.Reference.Length = 0) Then
        '                        newDealerDetailRow = m_tblSearchResult.NewRow

        '                        Dim strTemp As String = SAPCustomerAdress.Name
        '                        If SAPCustomerAdress.Name_2.Length > 0 Then
        '                            strTemp &= ", " & SAPCustomerAdress.Name_2
        '                        End If
        '                        If SAPCustomerAdress.Name_3.Length > 0 Then
        '                            strTemp &= ", " & SAPCustomerAdress.Name_3
        '                        End If
        '                        If SAPCustomerAdress.Name_4.Length > 0 Then
        '                            strTemp &= ", " & SAPCustomerAdress.Name_4
        '                        End If

        '                        newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPCustomerAdress.Countryiso & " - " & SAPCustomerAdress.Postl_Code & " " & SAPCustomerAdress.City & ", " & SAPCustomerAdress.Street
        '                        newDealerDetailRow("ADDRESSNUMBER") = SAPCustomerDetail.Customer
        '                        If SAPCustomerAdress.Postl_Code.Length > 0 Then
        '                            newDealerDetailRow("POSTL_CODE") = SAPCustomerAdress.Postl_Code
        '                        End If
        '                        If SAPCustomerAdress.Street.Length > 0 Then
        '                            newDealerDetailRow("STREET") = SAPCustomerAdress.Street
        '                        End If
        '                        If SAPCustomerAdress.Countryiso.Length > 0 Then
        '                            newDealerDetailRow("COUNTRYISO") = SAPCustomerAdress.Countryiso
        '                        End If
        '                        If SAPCustomerAdress.City.Length > 0 Then
        '                            newDealerDetailRow("CITY") = SAPCustomerAdress.City
        '                        End If
        '                        If SAPCustomerAdress.Name.Length > 0 Then
        '                            newDealerDetailRow("NAME") = SAPCustomerAdress.Name
        '                        End If
        '                        If SAPCustomerAdress.Name_2.Length > 0 Then
        '                            newDealerDetailRow("NAME_2") = SAPCustomerAdress.Name_2
        '                        End If

        '                        m_tblSearchResult.Rows.Add(newDealerDetailRow)
        '                    End If
        '                End If
        '            End If
        '        Next
        '        '§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§
        '        'Löschen:
        '        'newDealerDetailRow = m_tblSearchResult.NewRow
        '        'newDealerDetailRow("DISPLAY_ADDRESS") = "### Ich bin ein Dummy -> nimm mich! ###"
        '        'newDealerDetailRow("ADDRESSNUMBER") = "0200000002"
        '        'newDealerDetailRow("POSTL_CODE") = "12345"
        '        'newDealerDetailRow("STREET") = "Street"
        '        'newDealerDetailRow("COUNTRYISO") = "DE"
        '        'newDealerDetailRow("CITY") = "City"
        '        'newDealerDetailRow("NAME") = "Name"
        '        'newDealerDetailRow("NAME_2") = "Name_2"
        '        'm_tblSearchResult.Rows.Add(newDealerDetailRow)
        '        '§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§§

        '        Return m_tblSearchResult.Rows.Count
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        objSAP.Connection.Close()
        '        objSAP.Dispose()
        '    End Try
        'End Function

        Public Function Show(ByVal sUserID As String, ByVal sKunnr As String) As Int32
            m_strClassAndMethod = "SAPProxy_Base.GetRights"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
                    m_intStatus = 0
                    m_strMessage = ""

                    m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_Berechtigung_Anzeigen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    Dim proxy = DynSapProxy.getProxy("Z_BERECHTIGUNG_ANZEIGEN", m_objApp, m_objUser, PageHelper.GetCurrentPage())
                    proxy.setImportParameter("BENGRP", sUserID)
                    proxy.setImportParameter("KUNNR", sKunnr)

                    proxy.callBapi()
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    End If

                    tblRightsResult = proxy.getExportTable("OUTPUT")
                    tblDistrictsResult = proxy.getExportTable("DISTRIKTE")

                    WriteLogEntry(True, "Berechtigungen eingelesen")
                Catch ex As Exception
                    Select Case ex.Message
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "Fehler beim Einlesen der Berechtigungen")
                Finally
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    End If

                    m_blnGestartet = False
                End Try
            End If
        End Function


        Public Function Change() As Int32

            Dim proxy = DynSapProxy.getProxy("Z_BERECHTIGUNG_ANLEGEN", m_objApp, m_objUser, PageHelper.GetCurrentPage())
            Dim sapTable = proxy.getImportTable("INPUT")


            Dim count As Integer = tblRightsResult.Rows.Count

            For Each row As DataRow In tblRightsResult.Rows

                Dim sapTableRow = sapTable.NewRow()
                sapTableRow("Anwendung") = CType(row.Item("Anwendung"), String)
                sapTableRow("Bengrp") = row.Item("Bengrp").ToString
                sapTableRow("Distrikt") = row.Item("Distrikt").ToString
                sapTableRow("Kunnr") = row.Item("Kunnr").ToString
                sapTableRow("Mandt") = row.Item("Mandt").ToString
                sapTableRow("Vorbelegt") = row.Item("Vorbelegt").ToString
                sapTableRow("Loekz") = row.Item("Loekz").ToString

                sapTable.Rows.Add(sapTableRow)
            Next

            count = sapTable.Rows.Count

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                'objSAP.Z_Berechtigung_Anlegen(sapTable)
                'objSAP.CommitWork()
                proxy.callBapi()

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If
            Catch ex As Exception
                Select Case ex.Message

                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                'm_blnGestartet = False
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
