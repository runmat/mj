Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business
Imports CKG


Namespace AppF2
    <Serializable()> Public Class Search

#Region " Definitions"
        Private m_strHaendlerReferenzNummer As String
        Private m_strHaendlerName As String
        Private m_strHaendlerOrt As String
        Private m_tblHaendler As DataTable
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
        Private m_strSapInterneHaendlerReferenzNummer As String

        Private m_strREFERENZ As String

        Private m_blnGestartet As Boolean
        Private m_tblSearchResult As DataTable

        'für sucheHaendler Control
        '---------------------------------
        Private aStrHaendlernummer() As String
        Private aStrName1() As String
        Private aStrName2() As String
        Private aStrOrt() As String
        Private aStrPLZ() As String
        Private aStrStrasse() As String
        Private inthaendlerTreffer As Int32
        Private strZeigeAlle As String = ""
        Private strSucheHaendlerNr As String
        Private strSuchePLZ As String
        Private strSucheOrt As String
        Private strSucheName1 As String
        Private strSucheName2 As String
        '---------------------------------


        <NonSerialized()> Private m_strSessionID As String
        <NonSerialized()> Private m_strAppID As String
        <NonSerialized()> Private m_vwHaendler As DataView
        '<NonSerialized()> Protected WithEvents m_objSAPDestination As SAP.Connector.Destination
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


        Public ReadOnly Property aHaendlernummer() As String()
            Get
                Return aStrHaendlernummer
            End Get
        End Property


        Public ReadOnly Property aName1() As String()
            Get
                Return aStrName1
            End Get
        End Property

        Public ReadOnly Property aName2() As String()
            Get
                Return aStrName2
            End Get
        End Property


        Public ReadOnly Property aOrt() As String()
            Get
                Return aStrOrt
            End Get
        End Property


        Public ReadOnly Property aPLZ() As String()
            Get
                Return aStrPLZ
            End Get
        End Property

        Public ReadOnly Property aStrasse() As String()
            Get
                Return aStrStrasse
            End Get
        End Property

        Public ReadOnly Property anzahlHaendlerTreffer() As Integer
            Get
                Return inthaendlerTreffer
            End Get
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


        Public Property sucheHaendlerNr() As String
            Get
                Return strSucheHaendlerNr
            End Get
            Set(ByVal Value As String)
                strSucheHaendlerNr = Value
            End Set
        End Property

        Public Property suchePLZ() As String
            Get
                Return strSuchePLZ
            End Get
            Set(ByVal Value As String)
                strSuchePLZ = Value
            End Set
        End Property

        Public Property sucheOrt() As String
            Get
                Return strSucheOrt
            End Get
            Set(ByVal Value As String)
                strSucheOrt = Value
            End Set
        End Property



        Public Property sucheName1() As String
            Get
                Return strSucheName1
            End Get
            Set(ByVal Value As String)
                strSucheName1 = Value
            End Set
        End Property

        Public Property sucheName2() As String
            Get
                Return strSucheName2
            End Get
            Set(ByVal Value As String)
                strSucheName2 = Value
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


        Public Property zeigeAlle() As Boolean
            Get
                If strZeigeAlle = "X" Then
                    Return True
                Else
                    Return False
                End If

            End Get
            Set(ByVal Value As Boolean)

                If Value Then
                    strZeigeAlle = "X"
                Else
                    strZeigeAlle = ""
                End If
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

        Public ReadOnly Property Haendler() As DataView
            Get
                Return m_vwHaendler
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

        Public ReadOnly Property SapInterneHaendlerReferenzNummer As String
            Get
                Return m_strSapInterneHaendlerReferenzNummer
            End Get
        End Property

        Public Property Status As Integer
        Public Property Kontingente As DataTable
        Public Property Message As String

#End Region

#Region " Public Methods"
        Public Sub New(ByRef objApp As Base.Kernel.Security.App, ByRef objUser As Base.Kernel.Security.User, ByVal strSessionID As String, ByVal strAppID As String)
            m_objApp = objApp
            m_objUser = objUser

            'm_objSAPDestination = New SAP.Connector.Destination()
            'm_objSAPDestination.AppServerHost = m_objApp.SAPAppServerHost
            'm_objSAPDestination.SystemNumber = m_objApp.SAPSystemNumber
            'm_objSAPDestination.Client = m_objApp.SAPClient
            'm_objSAPDestination.Username = m_objApp.SAPUsername
            'm_objSAPDestination.Password = m_objApp.SAPPassword

            m_strHaendlerReferenzNummer = ""
            m_strHaendlerName = ""
            m_strHaendlerOrt = ""

            m_strSessionID = strSessionID
            m_strAppID = strAppID
        End Sub

        Public Sub ReNewSAPDestination(ByVal strSessionID As String, ByVal strAppID As String)
            'm_objSAPDestination = New SAP.Connector.Destination()
            'm_objSAPDestination.AppServerHost = m_objApp.SAPAppServerHost
            'm_objSAPDestination.SystemNumber = m_objApp.SAPSystemNumber
            'm_objSAPDestination.Client = m_objApp.SAPClient
            'm_objSAPDestination.Username = m_objApp.SAPUsername
            'm_objSAPDestination.Password = m_objApp.SAPPassword

            m_strSessionID = strSessionID
            m_strAppID = strAppID
        End Sub


        Public Function LeseHaendlerSAP_Einzeln(ByVal strAppID As String, ByVal strSessionID As String, ByVal InputReferenz As String, ByRef page As Page) As Boolean
            '----------------------------------------------------------------------
            ' Methode: LeseHaendlerSAP_Einzeln
            ' Autor: JJU
            ' Beschreibung: Umstellung auf dynproxy Z_M_Adressdaten_001
            ' Erstellt am: 19.6.2009
            ' ITA: 2928
            '----------------------------------------------------------------------

            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_strErrorMessage = ""
            Dim blnReturn As Boolean = False

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Adressdaten_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", Right("0000000000" & InputReferenz, 10))
                myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_VKORG", "1510")

                myProxy.callBapi()

                m_strCUSTOMER = ""
                m_strNAME = ""
                m_strNAME_2 = ""
                m_strCOUNTRYISO = ""
                m_strPOSTL_CODE = ""
                m_strCITY = ""
                m_strSTREET = ""
                m_strREFERENZ = ""


                Dim SAPReturnTable As DataTable = myProxy.getExportTable("GT_WEB")



                Select Case SAPReturnTable.Rows.Count
                    Case 0
                        m_strErrorMessage = "Kein Suchergebnis."
                        If m_intIDSAP > -1 Then
                            m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
                        End If

                        blnReturn = False
                    Case 1

                        Dim SAPReturnTableRow As DataRow = SAPReturnTable.Rows(0)

                        m_strCUSTOMER = SAPReturnTableRow("Konzs").ToString.TrimStart("0"c)
                        m_strREFERENZ = SAPReturnTableRow("Kunnr").ToString
                        m_strNAME = SAPReturnTableRow("Name1").ToString
                        m_strNAME_2 = SAPReturnTableRow("Name2").ToString
                        m_strCITY = SAPReturnTableRow("Ort01").ToString
                        m_strPOSTL_CODE = SAPReturnTableRow("Pstlz").ToString
                        m_strSTREET = SAPReturnTableRow("Stras").ToString
                        m_strCOUNTRYISO = SAPReturnTableRow("Land1").ToString

                        blnReturn = True
                    Case Else
                        m_strErrorMessage = "Suchergebnis nicht eindeutig."
                        If m_intIDSAP > -1 Then
                            m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
                        End If

                        blnReturn = False
                End Select

                If m_strErrorMessage.Length = 0 Then
                    WriteLogEntry(True, "Suche nach Händler """ & InputReferenz & """ erfolgreich.")
                Else
                    WriteLogEntry(False, "Suche nach Händler """ & InputReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
                End If
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_WEB"
                        m_strErrorMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_strErrorMessage = "Keine Eingabedaten gefunden."
                    Case Else
                        m_strErrorMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
                blnReturn = False
                WriteLogEntry(False, "Suche nach Händler """ & InputReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
            End Try
            Return blnReturn

        End Function







        Public Function LeseHaendlerSAP(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page, Optional ByVal InputReferenz As String = "") As Int32
            '----------------------------------------------------------------------
            ' Methode: LeseHaendlerSAP
            ' Autor: JJU
            ' Beschreibung: Umstellung auf dynproxy Z_M_Adressdaten_001
            ' Erstellt am: 19.6.2009
            ' ITA: 2928
            '----------------------------------------------------------------------


            ResetHaendlerTabelle()
            Dim strTempReferenz As String = InputReferenz
            Dim intReturn As Int32

            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Adressdaten_001", m_objApp, m_objUser, page)

                If InputReferenz.Length = 0 Then
                    strTempReferenz = m_strHaendlerReferenzNummer
                End If

                myProxy.setImportParameter("I_KUNNR", Right("0000000000" & InputReferenz, 10))
                myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                myProxy.setImportParameter("I_VKORG", "1510")

                myProxy.callBapi()

                Dim SAPReturnTable As DataTable = myProxy.getExportTable("GT_WEB")

                intReturn = SAPReturnTable.Rows.Count
                If SAPReturnTable.Rows.Count = 0 Then
                    m_strErrorMessage = "Kein Suchergebnis."
                Else

                    Dim newDealerDetailRow As DataRow

                    newDealerDetailRow = m_tblHaendler.NewRow()
                    newDealerDetailRow("REFERENZ") = "000000"
                    newDealerDetailRow("DISPLAY") = " - keine Auswahl - "
                    m_tblHaendler.Rows.Add(newDealerDetailRow)

                    Dim SAPReturnTableRow As DataRow

                    For I As Integer = 0 To SAPReturnTable.Rows.Count - 1
                        SAPReturnTableRow = SAPReturnTable.Rows(I)

                        newDealerDetailRow = m_tblHaendler.NewRow()
                        newDealerDetailRow("CUSTOMER") = SAPReturnTableRow("Konzs").ToString.TrimStart("0"c)

                        newDealerDetailRow("REFERENZ") = SAPReturnTableRow("Kunnr").ToString.TrimStart("0"c)

                        newDealerDetailRow("NAME") = SAPReturnTableRow("Name1")
                        newDealerDetailRow("NAME_2") = SAPReturnTableRow("Name2")
                        newDealerDetailRow("CITY") = SAPReturnTableRow("Ort01")
                        newDealerDetailRow("POSTL_CODE") = SAPReturnTableRow("Pstlz")
                        newDealerDetailRow("STREET") = SAPReturnTableRow("Stras")
                        newDealerDetailRow("COUNTRYISO") = SAPReturnTableRow("Land1")
                        '-------------------------------------------------------
                        'newDealerDetailRow("DISPLAY") = SAPReturnTableRow.Kunnr.TrimStart("0"c) & " - " & SAPReturnTableRow.Name1 & ", " & SAPReturnTableRow.Ort01
                        'es soll mit tastendruck ein händler aus der dll ausgewählt werden, das geht nur wenn der 1. wert in der DDL das suchkriterium ist, hier der name des Händlers-
                        'Händler nummer soll in anzeige komplett verschwinden> JJU//Rothe 2008.03.03
                        'Neuer Aufbau laut Rothe 2008.03.04: Name1,Name2,Str, Ort,.
                        '-------------------------------------------------------
                        newDealerDetailRow("DISPLAY") = SAPReturnTableRow("Name1").ToString & " " & SAPReturnTableRow("Name2").ToString & "  -  " & SAPReturnTableRow("Stras").ToString & ", " & SAPReturnTableRow("Ort01").ToString

                        newDealerDetailRow("DISPLAY_ADDRESS") = SAPReturnTableRow("Name1")
                        If Not SAPReturnTableRow("Name2").ToString.Trim(" "c).Length = 0 Then
                            newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow("Name2").ToString & ", " & SAPReturnTableRow("Land1").ToString & "-" & SAPReturnTableRow("Pstlz").ToString & " " & SAPReturnTableRow("Ort01").ToString & ", " & SAPReturnTableRow("Stras").ToString
                        Else
                            newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow("Land1").ToString & "-" & SAPReturnTableRow("Pstlz").ToString & " " & SAPReturnTableRow("Ort01").ToString & ", " & SAPReturnTableRow("Stras").ToString
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

                If m_strErrorMessage.Length = 0 Then
                    WriteLogEntry(True, "Suche nach Händler """ & strTempReferenz & """ erfolgreich.")
                Else
                    WriteLogEntry(False, "Suche nach Händler """ & strTempReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
                End If

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_WEB"
                        m_strErrorMessage = "Keine Web-Tabelle erstellt."
                        intReturn = 0
                    Case "NO_DATA"
                        m_strErrorMessage = "Keine Eingabedaten gefunden."
                        intReturn = 0
                    Case Else
                        m_strErrorMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        intReturn = 0
                End Select
                WriteLogEntry(False, "Suche nach Händler """ & strTempReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
            End Try
            Return intReturn
        End Function

        'Public Function LeseHaendlerSAP(ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal InputReferenz As String = "") As Int32
        '    If Not m_blnGestartet Then
        '        m_blnGestartet = True

        '        Dim objSAP As New SAPProxy_ComCommon_Finance.SAPProxy_ComCommon_Finance()

        '        ResetHaendlerTabelle()

        '        If m_objLogApp Is Nothing Then
        '            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '        End If
        '        m_intIDSAP = -1
        '        Dim I As Int32
        '        Dim strTempReferenz As String = InputReferenz

        '        Dim intReturn As Int32

        '        Try
        '            m_strErrorMessage = ""

        '            If InputReferenz.Length = 0 Then
        '                strTempReferenz = m_strHaendlerReferenzNummer
        '            End If

        '            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)


        '            Dim SAPReturnTable As New SAPProxy_ComCommon_Finance.ZDAD_M_WEB_ADRESSEN_002Table()
        '            Dim SAPReturnTableRow As New SAPProxy_ComCommon_Finance.ZDAD_M_WEB_ADRESSEN_002()

        '            m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Adressdaten_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
        '            objSAP.Z_M_Adressdaten_001(Right("0000000000" & m_objUser.Customer.KUNNR, 10), strTempReferenz, "1510", SAPReturnTable)
        '            If m_intIDSAP > -1 Then
        '                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
        '            End If

        '            intReturn = SAPReturnTable.Count
        '            If SAPReturnTable.Count = 0 Then
        '                m_strErrorMessage = "Kein Suchergebnis."
        '                'Suche ohne Ergebnis
        '                If m_intIDSAP > -1 Then
        '                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
        '                End If
        '            Else

        '                Dim newDealerDetailRow As DataRow

        '                newDealerDetailRow = m_tblHaendler.NewRow()
        '                newDealerDetailRow("REFERENZ") = "000000"
        '                newDealerDetailRow("DISPLAY") = " - keine Auswahl - "
        '                m_tblHaendler.Rows.Add(newDealerDetailRow)

        '                For I = 0 To SAPReturnTable.Count - 1
        '                    SAPReturnTableRow = SAPReturnTable.Item(I)

        '                    newDealerDetailRow = m_tblHaendler.NewRow()
        '                    newDealerDetailRow("CUSTOMER") = SAPReturnTableRow.Konzs.TrimStart("0"c)

        '                    newDealerDetailRow("REFERENZ") = SAPReturnTableRow.Kunnr.TrimStart("0"c)

        '                    newDealerDetailRow("NAME") = SAPReturnTableRow.Name1
        '                    newDealerDetailRow("NAME_2") = SAPReturnTableRow.Name2
        '                    newDealerDetailRow("CITY") = SAPReturnTableRow.Ort01
        '                    newDealerDetailRow("POSTL_CODE") = SAPReturnTableRow.Pstlz
        '                    newDealerDetailRow("STREET") = SAPReturnTableRow.Stras
        '                    newDealerDetailRow("COUNTRYISO") = SAPReturnTableRow.Land1
        '                    '-------------------------------------------------------
        '                    'newDealerDetailRow("DISPLAY") = SAPReturnTableRow.Kunnr.TrimStart("0"c) & " - " & SAPReturnTableRow.Name1 & ", " & SAPReturnTableRow.Ort01
        '                    'es soll mit tastendruck ein händler aus der dll ausgewählt werden, das geht nur wenn der 1. wert in der DDL das suchkriterium ist, hier der name des Händlers-
        '                    'Händler nummer soll in anzeige komplett verschwinden> JJU//Rothe 2008.03.03
        '                    'Neuer Aufbau laut Rothe 2008.03.04: Name1,Name2,Str, Ort,.
        '                    '-------------------------------------------------------
        '                    newDealerDetailRow("DISPLAY") = SAPReturnTableRow.Name1 & " " & SAPReturnTableRow.Name2 & "  -  " & SAPReturnTableRow.Stras & ", " & SAPReturnTableRow.Ort01

        '                    newDealerDetailRow("DISPLAY_ADDRESS") = SAPReturnTableRow.Name1
        '                    If Not SAPReturnTableRow.Name2.Trim(" "c).Length = 0 Then
        '                        newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow.Name2 & ", " & SAPReturnTableRow.Land1 & "-" & SAPReturnTableRow.Pstlz & " " & SAPReturnTableRow.Ort01 & ", " & SAPReturnTableRow.Stras
        '                    Else
        '                        newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow.Land1 & "-" & SAPReturnTableRow.Pstlz & " " & SAPReturnTableRow.Ort01 & ", " & SAPReturnTableRow.Stras
        '                    End If

        '                    m_tblHaendler.Rows.Add(newDealerDetailRow)
        '                Next
        '                m_vwHaendler = m_tblHaendler.DefaultView
        '                If SAPReturnTable.Count > 1 Then
        '                    'Weitere Auswahl entspechend Name und/oder Ort
        '                    Dim filterExp As String = ""

        '                    If m_strHaendlerReferenzNummer.Trim(" "c).Length = 0 Then
        '                        If (Len(Trim(m_strHaendlerName)) > 0) And (Len(Trim(m_strHaendlerOrt)) > 0) Then
        '                            filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "' AND CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
        '                        ElseIf Len(Trim(m_strHaendlerName)) > 0 Then
        '                            filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "'"
        '                        ElseIf Len(Trim(m_strHaendlerOrt)) > 0 Then
        '                            filterExp = "CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
        '                        End If
        '                    End If

        '                    If filterExp.Length > 0 Then
        '                        filterExp = Replace(filterExp, "*", "%")
        '                        m_vwHaendler.RowFilter = filterExp
        '                        intReturn = m_vwHaendler.Count
        '                    End If
        '                    'Suche mit mehreren Ergebnissen
        '                End If
        '                m_vwHaendler.Sort = "NAME"
        '            End If

        '            If m_strErrorMessage.Length = 0 Then
        '                WriteLogEntry(True, "Suche nach Händler """ & strTempReferenz & """ erfolgreich.")
        '            Else
        '                WriteLogEntry(False, "Suche nach Händler """ & strTempReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
        '            End If

        '        Catch ex As Exception
        '            Select Case ex.Message
        '                Case "NO_WEB"
        '                    m_strErrorMessage = "Keine Web-Tabelle erstellt."
        '                    intReturn = 0
        '                Case "NO_DATA"
        '                    m_strErrorMessage = "Keine Eingabedaten gefunden."
        '                    intReturn = 0
        '                Case Else
        '                    m_strErrorMessage = ex.Message
        '                    intReturn = 0
        '            End Select
        '            If m_intIDSAP > -1 Then
        '                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
        '            End If
        '            WriteLogEntry(False, "Suche nach Händler """ & strTempReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
        '        Finally
        '            If m_intIDSAP > -1 Then
        '                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
        '            End If

        '            objSAP.Connection.Close()
        '            objSAP.Dispose()

        '            m_blnGestartet = False
        '        End Try
        '        Return intReturn
        '    End If
        'End Function


        Public Function LeseHaendlerForSucheHaendlerControl(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, Optional ByVal InputReferenz As String = "") As Int32
            '----------------------------------------------------------------------
            ' Methode: LeseHaendlerForSucheHaendlerControl
            ' Autor: JJU
            ' Beschreibung: Umstellung auf dynproxy Z_M_Adressdaten_002
            ' Erstellt am: 19.6.2009
            ' ITA: 2928
            '----------------------------------------------------------------------


            Dim blnReturn As Boolean = False
            Dim I As Int32
            Dim intReturn As Int32 = 0
            m_strErrorMessage = ""

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Adressdaten_002", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", sucheHaendlerNr)
                myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                myProxy.setImportParameter("I_VKORG", "1510")
                myProxy.setImportParameter("I_ALL", strZeigeAlle)
                myProxy.setImportParameter("I_MAX", "200")
                myProxy.setImportParameter("I_NAME1", sucheName1)
                myProxy.setImportParameter("I_NAME2", sucheName2)
                myProxy.setImportParameter("I_ORT", sucheOrt)
                myProxy.setImportParameter("I_PSTLZ", suchePLZ)

                ResetHaendlerTabelle()
                myProxy.callBapi()

                If IsNumeric(myProxy.getExportParameter("E_REC_ANZ")) Then
                    inthaendlerTreffer = CInt(myProxy.getExportParameter("E_REC_ANZ"))
                End If
                Dim SAPReturnTable As DataTable = myProxy.getExportTable("GT_WEB")
                Dim SAPReturnTableRow As DataRow


                If SAPReturnTable.Rows.Count = 0 Then
                    m_strErrorMessage = "Kein Suchergebnis."
                Else

                    Dim newDealerDetailRow As DataRow

                    'arrays Dimensionieren
                    ReDim aStrHaendlernummer(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrName1(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrName2(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrOrt(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrPLZ(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrStrasse(SAPReturnTable.Rows.Count - 1)

                    'nur für testzwecke!! 
                    For I = 0 To SAPReturnTable.Rows.Count - 1

                        SAPReturnTableRow = SAPReturnTable.Rows(I)


                        'ArraysBefüllen
                        '-------------------------------------
                        aStrHaendlernummer(I) = stringZeichenEntfernen(SAPReturnTableRow("Kunnr").ToString)
                        aStrName1(I) = stringZeichenEntfernen(SAPReturnTableRow("Name1").ToString)
                        aStrName2(I) = stringZeichenEntfernen(SAPReturnTableRow("Name2").ToString)
                        aStrOrt(I) = stringZeichenEntfernen(SAPReturnTableRow("Ort01").ToString)
                        aStrPLZ(I) = stringZeichenEntfernen(SAPReturnTableRow("Pstlz").ToString)
                        aStrStrasse(I) = stringZeichenEntfernen(SAPReturnTableRow("Stras").ToString)
                        '-------------------------------------



                        newDealerDetailRow = m_tblHaendler.NewRow()
                        newDealerDetailRow("CUSTOMER") = SAPReturnTableRow("Konzs").ToString.TrimStart("0"c)

                        newDealerDetailRow("REFERENZ") = SAPReturnTableRow("Kunnr").ToString.TrimStart("0"c)

                        newDealerDetailRow("NAME") = SAPReturnTableRow("Name1").ToString
                        newDealerDetailRow("NAME_2") = SAPReturnTableRow("Name2").ToString
                        newDealerDetailRow("CITY") = SAPReturnTableRow("Ort01").ToString
                        newDealerDetailRow("POSTL_CODE") = SAPReturnTableRow("Pstlz").ToString
                        newDealerDetailRow("STREET") = SAPReturnTableRow("Stras").ToString
                        newDealerDetailRow("COUNTRYISO") = SAPReturnTableRow("Land1").ToString
                        '-------------------------------------------------------
                        'newDealerDetailRow("DISPLAY") = SAPReturnTableRow.Kunnr.TrimStart("0"c) & " - " & SAPReturnTableRow.Name1 & ", " & SAPReturnTableRow.Ort01
                        'es soll mit tastendruck ein händler aus der dll ausgewählt werden, das geht nur wenn der 1. wert in der DDL das suchkriterium ist, hier der name des Händlers-
                        'Händler nummer soll in anzeige komplett verschwinden> JJU//Rothe 2008.03.03
                        'Neuer Aufbau laut Rothe 2008.03.04: Name1,Name2,Str, Ort,.
                        '-------------------------------------------------------
                        newDealerDetailRow("DISPLAY") = SAPReturnTableRow("Name1").ToString & " " & SAPReturnTableRow("Name2").ToString & "  -  " & SAPReturnTableRow("Stras").ToString & ", " & SAPReturnTableRow("Ort01").ToString

                        newDealerDetailRow("DISPLAY_ADDRESS") = SAPReturnTableRow("Name1")
                        If Not SAPReturnTableRow("Name2").ToString.Trim(" "c).Length = 0 Then
                            newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow("Name2").ToString & ", " & SAPReturnTableRow("Land1").ToString & "-" & SAPReturnTableRow("Pstlz").ToString & " " & SAPReturnTableRow("Ort01").ToString & ", " & SAPReturnTableRow("Stras").ToString
                        Else
                            newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow("Land1").ToString & "-" & SAPReturnTableRow("Pstlz").ToString & " " & SAPReturnTableRow("Ort01").ToString & ", " & SAPReturnTableRow("Stras").ToString
                        End If

                        m_tblHaendler.Rows.Add(newDealerDetailRow)
                    Next
                    m_vwHaendler = m_tblHaendler.DefaultView
                    'If SAPReturnTable.Rows.Count > 1 Then
                    '    'Weitere Auswahl entspechend Name und/oder Ort
                    '    Dim filterExp As String = ""

                    '    If m_strHaendlerReferenzNummer.Trim(" "c).Length = 0 Then
                    '        If (Len(Trim(m_strHaendlerName)) > 0) And (Len(Trim(m_strHaendlerOrt)) > 0) Then
                    '            filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "' AND CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
                    '        ElseIf Len(Trim(m_strHaendlerName)) > 0 Then
                    '            filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "'"
                    '        ElseIf Len(Trim(m_strHaendlerOrt)) > 0 Then
                    '            filterExp = "CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
                    '        End If
                    '    End If

                    '    If filterExp.Length > 0 Then
                    '        filterExp = Replace(filterExp, "*", "%")
                    '        m_vwHaendler.RowFilter = filterExp
                    '    End If
                    'End If
                    m_vwHaendler.Sort = "NAME"
                End If

                If m_strErrorMessage.Length = 0 Then
                    WriteLogEntry(True, "Suche nach Händler  erfolgreich.")
                Else
                    WriteLogEntry(False, "Suche nach Händler nicht erfolgreich.  (" & m_strErrorMessage & ")")
                End If

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_WEB"
                        m_strErrorMessage = "Keine Web-Tabelle erstellt."
                        intReturn = -1
                    Case "NO_DATA"
                        m_strErrorMessage = "Keine Eingabedaten gefunden."
                        intReturn = -2
                    Case Else
                        m_strErrorMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        intReturn = -2
                End Select
                WriteLogEntry(False, "Suche nach Händler nicht erfolgreich. (" & m_strErrorMessage & ")")
            End Try
            Return intReturn
        End Function


        'Public Function LeseHaendlerForSucheHaendlerControl(ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal InputReferenz As String = "") As Int32
        '    If Not m_blnGestartet Then
        '        m_blnGestartet = True

        '        Dim objSAP As New SAPProxy_ComCommon_Finance.SAPProxy_ComCommon_Finance()

        '        ResetHaendlerTabelle()

        '        If m_objLogApp Is Nothing Then
        '            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '        End If
        '        m_intIDSAP = -1
        '        Dim I As Int32

        '        Dim intReturn As Int32 = 0

        '        Try
        '            m_strErrorMessage = ""


        '            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)

        '            Dim SAPReturnTable As New SAPProxy_ComCommon_Finance.ZDAD_M_WEB_ADRESSEN_002Table()
        '            Dim SAPReturnTableRow As New SAPProxy_ComCommon_Finance.ZDAD_M_WEB_ADRESSEN_002()

        '            m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Adressdaten_002", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
        '            objSAP.Z_M_Adressdaten_002(strZeigeAlle, Right("0000000000" & m_objUser.Customer.KUNNR, 10), sucheHaendlerNr, 200, sucheName1, sucheName2, sucheOrt, suchePLZ, "1510", inthaendlerTreffer, SAPReturnTable)
        '            If m_intIDSAP > -1 Then
        '                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
        '            End If

        '            If SAPReturnTable.Count = 0 Then
        '                m_strErrorMessage = "Kein Suchergebnis."

        '                'Suche ohne Ergebnis
        '                If m_intIDSAP > -1 Then
        '                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
        '                End If
        '            Else

        '                Dim newDealerDetailRow As DataRow

        '                'arrays Dimensionieren
        '                ReDim aStrHaendlernummer(SAPReturnTable.Count - 1)
        '                ReDim aStrName1(SAPReturnTable.Count - 1)
        '                ReDim aStrName2(SAPReturnTable.Count - 1)
        '                ReDim aStrOrt(SAPReturnTable.Count - 1)
        '                ReDim aStrPLZ(SAPReturnTable.Count - 1)
        '                ReDim aStrStrasse(SAPReturnTable.Count - 1)

        '                'nur für testzwecke!! 
        '                For I = 0 To SAPReturnTable.Count - 1

        '                    SAPReturnTableRow = SAPReturnTable.Item(I)


        '                    'ArraysBefüllen
        '                    '-------------------------------------
        '                    aStrHaendlernummer(I) = stringZeichenEntfernen(SAPReturnTableRow.Kunnr)
        '                    aStrName1(I) = stringZeichenEntfernen(SAPReturnTableRow.Name1)
        '                    aStrName2(I) = stringZeichenEntfernen(SAPReturnTableRow.Name2)
        '                    aStrOrt(I) = stringZeichenEntfernen(SAPReturnTableRow.Ort01)
        '                    aStrPLZ(I) = stringZeichenEntfernen(SAPReturnTableRow.Pstlz)
        '                    aStrStrasse(I) = stringZeichenEntfernen(SAPReturnTableRow.Stras)
        '                    '-------------------------------------



        '                    newDealerDetailRow = m_tblHaendler.NewRow()
        '                    newDealerDetailRow("CUSTOMER") = SAPReturnTableRow.Konzs.TrimStart("0"c)

        '                    newDealerDetailRow("REFERENZ") = SAPReturnTableRow.Kunnr.TrimStart("0"c)

        '                    newDealerDetailRow("NAME") = SAPReturnTableRow.Name1
        '                    newDealerDetailRow("NAME_2") = SAPReturnTableRow.Name2
        '                    newDealerDetailRow("CITY") = SAPReturnTableRow.Ort01
        '                    newDealerDetailRow("POSTL_CODE") = SAPReturnTableRow.Pstlz
        '                    newDealerDetailRow("STREET") = SAPReturnTableRow.Stras
        '                    newDealerDetailRow("COUNTRYISO") = SAPReturnTableRow.Land1
        '                    '-------------------------------------------------------
        '                    'newDealerDetailRow("DISPLAY") = SAPReturnTableRow.Kunnr.TrimStart("0"c) & " - " & SAPReturnTableRow.Name1 & ", " & SAPReturnTableRow.Ort01
        '                    'es soll mit tastendruck ein händler aus der dll ausgewählt werden, das geht nur wenn der 1. wert in der DDL das suchkriterium ist, hier der name des Händlers-
        '                    'Händler nummer soll in anzeige komplett verschwinden> JJU//Rothe 2008.03.03
        '                    'Neuer Aufbau laut Rothe 2008.03.04: Name1,Name2,Str, Ort,.
        '                    '-------------------------------------------------------
        '                    newDealerDetailRow("DISPLAY") = SAPReturnTableRow.Name1 & " " & SAPReturnTableRow.Name2 & "  -  " & SAPReturnTableRow.Stras & ", " & SAPReturnTableRow.Ort01

        '                    newDealerDetailRow("DISPLAY_ADDRESS") = SAPReturnTableRow.Name1
        '                    If Not SAPReturnTableRow.Name2.Trim(" "c).Length = 0 Then
        '                        newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow.Name2 & ", " & SAPReturnTableRow.Land1 & "-" & SAPReturnTableRow.Pstlz & " " & SAPReturnTableRow.Ort01 & ", " & SAPReturnTableRow.Stras
        '                    Else
        '                        newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow.Land1 & "-" & SAPReturnTableRow.Pstlz & " " & SAPReturnTableRow.Ort01 & ", " & SAPReturnTableRow.Stras
        '                    End If

        '                    m_tblHaendler.Rows.Add(newDealerDetailRow)
        '                Next
        '                m_vwHaendler = m_tblHaendler.DefaultView
        '                If SAPReturnTable.Count > 1 Then
        '                    'Weitere Auswahl entspechend Name und/oder Ort
        '                    Dim filterExp As String = ""

        '                    If m_strHaendlerReferenzNummer.Trim(" "c).Length = 0 Then
        '                        If (Len(Trim(m_strHaendlerName)) > 0) And (Len(Trim(m_strHaendlerOrt)) > 0) Then
        '                            filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "' AND CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
        '                        ElseIf Len(Trim(m_strHaendlerName)) > 0 Then
        '                            filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "'"
        '                        ElseIf Len(Trim(m_strHaendlerOrt)) > 0 Then
        '                            filterExp = "CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
        '                        End If
        '                    End If

        '                    If filterExp.Length > 0 Then
        '                        filterExp = Replace(filterExp, "*", "%")
        '                        m_vwHaendler.RowFilter = filterExp
        '                    End If
        '                End If
        '                m_vwHaendler.Sort = "NAME"
        '            End If

        '            If m_strErrorMessage.Length = 0 Then
        '                WriteLogEntry(True, "Suche nach Händler  erfolgreich.")
        '            Else
        '                WriteLogEntry(False, "Suche nach Händler nicht erfolgreich.  (" & m_strErrorMessage & ")")
        '            End If

        '        Catch ex As Exception
        '            Select Case ex.Message
        '                Case "NO_WEB"
        '                    m_strErrorMessage = "Keine Web-Tabelle erstellt."
        '                    intReturn = -1
        '                Case "NO_DATA"
        '                    m_strErrorMessage = "Keine Eingabedaten gefunden."
        '                    intReturn = -2
        '                Case Else
        '                    m_strErrorMessage = ex.Message
        '                    intReturn = -2
        '            End Select
        '            If m_intIDSAP > -1 Then
        '                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strErrorMessage)
        '            End If
        '            WriteLogEntry(False, "Suche nach Händler nicht erfolgreich. (" & m_strErrorMessage & ")")
        '        Finally
        '            If m_intIDSAP > -1 Then
        '                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
        '            End If

        '            objSAP.Connection.Close()
        '            objSAP.Dispose()

        '            m_blnGestartet = False
        '        End Try
        '        Return intReturn
        '    End If


        'End Function

        Private Function stringZeichenEntfernen(ByVal zeichenkette As String) As String
            If zeichenkette Is Nothing OrElse zeichenkette Is String.Empty Then
                zeichenkette = "-"
            Else
                If Not zeichenkette.IndexOf("'") = -1 Then
                    zeichenkette = zeichenkette.Replace("'", "")
                End If
                If Not zeichenkette.IndexOf("""") = -1 Then
                    zeichenkette = zeichenkette.Replace("""", "")
                End If
            End If
            Return zeichenkette
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
            m_vwHaendler = Nothing

            m_tblHaendler.Columns.Add("CUSTOMER", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("REFERENZ", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("NAME", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("NAME_2", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("CITY", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("STREET", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("DISPLAY", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))
        End Sub

        Public Function LeseAdressenSAP(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, ByVal strParentNode As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32 'HEZ: "B" oder "C"

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
                m_tblSearchResult.Columns.Add("HAENDLER", System.Type.GetType("System.String"))

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Customer_Get_Children", m_objApp, m_objUser, page)

                myProxy.setImportParameter("AG", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("VALID_ON", Now.ToShortDateString)
                myProxy.setImportParameter("CUSTHITYP", adressart)
                myProxy.setImportParameter("NODE_LEVEL", nodelevel)
                myProxy.setImportParameter("CUSTOMERNO", Right("0000000000" & strParentNode, 10))
                'myProxy.setImportParameter("WEB_GID", m_objUser.GroupID.ToString)
                'myProxy.setImportParameter("WEB_OID", m_objUser.Organization.OrganizationId.ToString)
                myProxy.setImportParameter("WEB_GID", "0")
                myProxy.setImportParameter("WEB_OID", "0")
                myProxy.callBapi()

                Dim SAPSalesTable As DataTable = myProxy.getExportTable("SALES_AREA")
                Dim SAPReturnTable As DataTable = myProxy.getExportTable("RETURN")
                Dim SAPAdressenTable As DataTable = myProxy.getExportTable("GT_ADRESSEN")
                Dim SAPTableRow As DataRow

                Dim newDealerDetailRow As DataRow
                Dim firstStep As Boolean = True

                For Each SAPTableRow In SAPAdressenTable.Rows
                    If SAPAdressenTable.Rows.Count > 0 Then

                        newDealerDetailRow = m_tblSearchResult.NewRow

                        Dim strTemp As String = SAPTableRow("Name1").ToString
                        If SAPTableRow("Name2").ToString.Length > 0 Then
                            strTemp &= ", " & SAPTableRow("Name2").ToString
                        End If
                        If SAPTableRow("Name3").ToString.Length > 0 Then
                            strTemp &= ", " & SAPTableRow("Name3").ToString
                        End If
                        If SAPTableRow("Name4").ToString.Length > 0 Then
                            strTemp &= ", " & SAPTableRow("Name4").ToString
                        End If

                        newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPTableRow("Countryiso").ToString & " - " & SAPTableRow("Post_Code1").ToString & " " & SAPTableRow("City1").ToString & ", " & SAPTableRow("Street").ToString
                        newDealerDetailRow("ADDRESSNUMBER") = SAPTableRow("Haendler").ToString
                        If SAPTableRow("Post_Code1").ToString.Length > 0 Then
                            newDealerDetailRow("POSTL_CODE") = SAPTableRow("Post_Code1")
                        End If
                        If SAPTableRow("Street").ToString.Length > 0 Then
                            newDealerDetailRow("STREET") = SAPTableRow("Street")
                        End If
                        If SAPTableRow("Countryiso").ToString.Length > 0 Then
                            newDealerDetailRow("COUNTRYISO") = SAPTableRow("Countryiso")
                        End If
                        If SAPTableRow("City1").ToString.Length > 0 Then
                            newDealerDetailRow("CITY") = SAPTableRow("City1")
                        End If
                        If SAPTableRow("Name1").ToString.Length > 0 Then
                            newDealerDetailRow("NAME") = SAPTableRow("Name1")
                        End If
                        If SAPTableRow("Name2").ToString.Length > 0 Then
                            newDealerDetailRow("NAME_2") = SAPTableRow("Name2")
                        End If
                        If SAPTableRow("HAENDLER").ToString.Length > 0 Then
                            newDealerDetailRow("HAENDLER") = SAPTableRow("HAENDLER_AG")
                        End If
                        m_tblSearchResult.Rows.Add(newDealerDetailRow)
                    End If

                Next
                Return m_tblSearchResult.Rows.Count
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub fillHaendlerData(ByVal strAppID As String, ByVal strSessionID As String, ByVal Haendlernummer As String, ByVal page As Web.UI.Page)
            '----------------------------------------------------------------------
            ' Methode: fillHaendlerData
            ' Autor: JJU
            ' Beschreibung: gibt für einen Händler das Kontingent und dessen adresse zurück
            ' Erstellt am: 04.03.2009
            ' ITA: 2661
            '----------------------------------------------------------------------

            m_strAppID = strAppID
            m_strSessionID = strSessionID
            Status = 0
            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_HAENDLER_KONTINGENT_STD", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_HAENDLER_EX", Haendlernummer)


                myProxy.callBapi()

                Kontingente = myProxy.getExportTable("GT_LIMIT")
                Dim tblAdresseTemp As DataTable = myProxy.getExportTable("EX_ADRS")
                'mFahrzeuge.Columns.Add("Anfordern", Type.GetType("System.String"))

                For Each tmpRow As DataRow In Kontingente.Rows
                    'translate Kontingentarten
                    Select Case tmpRow.Item("RECART").ToString
                        Case "G"
                            tmpRow.Item("RECART") = "Gruppe"
                        Case "H"
                            tmpRow.Item("RECART") = "Händler"
                        Case "S"
                            tmpRow.Item("RECART") = "Summe"
                            'bei summe nur frei als summe ausgeben
                            tmpRow.Item("KLIMK") = DBNull.Value
                            tmpRow.Item("SKFOR") = DBNull.Value
                        Case Else
                            tmpRow.Item("") = "unbekannte Kontingentart"
                    End Select
                Next


                Kontingente.AcceptChanges()
                m_strCUSTOMER = tblAdresseTemp(0)("AG").ToString
                m_strREFERENZ = tblAdresseTemp(0)("HAENDLER_EX").ToString
                m_strNAME = tblAdresseTemp(0)("NAME1").ToString
                m_strNAME_2 = tblAdresseTemp(0)("NAME2").ToString
                m_strCITY = tblAdresseTemp(0)("ORT01").ToString
                m_strPOSTL_CODE = tblAdresseTemp(0)("PSTLZ").ToString
                m_strSTREET = tblAdresseTemp(0)("STRAS").ToString
                m_strCOUNTRYISO = tblAdresseTemp(0)("LAND1").ToString
                m_strSapInterneHaendlerReferenzNummer = tblAdresseTemp(0)("HAENDLER").ToString



            Catch ex As Exception
                Status = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        Message = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
            End Try
        End Sub



        'Public Function LeseAdressenSAP(ByVal strParentNode As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32 'HEZ: "B" oder "C"
        '    'Dim objSAP As New SAPProxy_Base.SAPProxy_Base
        '    Dim objSAP2 As New SAPProxy_ComCommon_Finance.SAPProxy_ComCommon_Finance

        '    Try
        '        'Dim tblTemp As DataTable
        '        m_tblSearchResult = New DataTable()
        '        m_tblSearchResult.Columns.Add("ADDRESSNUMBER", System.Type.GetType("System.String"))
        '        m_tblSearchResult.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))

        '        m_tblSearchResult.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
        '        m_tblSearchResult.Columns.Add("STREET", System.Type.GetType("System.String"))
        '        m_tblSearchResult.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
        '        m_tblSearchResult.Columns.Add("CITY", System.Type.GetType("System.String"))
        '        m_tblSearchResult.Columns.Add("NAME", System.Type.GetType("System.String"))
        '        m_tblSearchResult.Columns.Add("NAME_2", System.Type.GetType("System.String"))

        '        'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        '        objSAP2.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)

        '        ' Dim SAPTable As New SAPProxy_ComCommon_Finance.BAPIKNA1_KNVHTable()
        '        Dim SAPSalesTable As New SAPProxy_ComCommon_Finance.BAPI_SDVTBERTable()
        '        Dim SAPReturnTable As New SAPProxy_ComCommon_Finance.BAPIRET2()
        '        Dim SAPAdressenTable As New SAPProxy_ComCommon_Finance.ZDAD_CUSTOMER_GET_CHILDRENTable


        '        'Dim SAPBapiRet1 As New SAPProxy_Base.BAPIRET1
        '        'Dim SAPCustomerDetail As New SAPProxy_Base.BAPICUSTOMER_KNA1()
        '        'Dim SAPCustomerAdress As New SAPProxy_Base.BAPICUSTOMER_04()
        '        'Dim SAPCustomerCompanyDetail As New SAPProxy_Base.BAPICUSTOMER_05()
        '        'Dim SAPCustomerBankDetail As New SAPProxy_Base.BAPICUSTOMER_02Table()


        '        objSAP2.Z_M_Customer_Get_Children(Right("0000000000" & m_objUser.KUNNR, 10), adressart, Right("0000000000" & strParentNode, 10), nodelevel, Format(Now, "yyyyMMdd"), m_objUser.GroupID, m_objUser.Organization.OrganizationId, SAPReturnTable, SAPAdressenTable, SAPSalesTable)



        '        'tblTemp = SAPAdressenTable.ToADODataTable


        '        Dim SAPTableRow As New SAPProxy_ComCommon_Finance.ZDAD_CUSTOMER_GET_CHILDREN

        '        Dim newDealerDetailRow As DataRow
        '        Dim firstStep As Boolean = True

        '        For Each SAPTableRow In SAPAdressenTable
        '            If SAPAdressenTable.Count > 0 Then


        '                newDealerDetailRow = m_tblSearchResult.NewRow

        '                Dim strTemp As String = SAPTableRow.Name1
        '                If SAPTableRow.Name2.Length > 0 Then
        '                    strTemp &= ", " & SAPTableRow.Name2
        '                End If
        '                If SAPTableRow.Name3.Length > 0 Then
        '                    strTemp &= ", " & SAPTableRow.Name3
        '                End If
        '                If SAPTableRow.Name4.Length > 0 Then
        '                    strTemp &= ", " & SAPTableRow.Name4
        '                End If

        '                newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPTableRow.Countryiso & " - " & SAPTableRow.Post_Code1 & " " & SAPTableRow.City1 & ", " & SAPTableRow.Street
        '                newDealerDetailRow("ADDRESSNUMBER") = SAPTableRow.Haendler
        '                If SAPTableRow.Post_Code1.Length > 0 Then
        '                    newDealerDetailRow("POSTL_CODE") = SAPTableRow.Post_Code1
        '                End If
        '                If SAPTableRow.Street.Length > 0 Then
        '                    newDealerDetailRow("STREET") = SAPTableRow.Street
        '                End If
        '                If SAPTableRow.Countryiso.Length > 0 Then
        '                    newDealerDetailRow("COUNTRYISO") = SAPTableRow.Countryiso
        '                End If
        '                If SAPTableRow.City1.Length > 0 Then
        '                    newDealerDetailRow("CITY") = SAPTableRow.City1
        '                End If
        '                If SAPTableRow.Name1.Length > 0 Then
        '                    newDealerDetailRow("NAME") = SAPTableRow.Name1
        '                End If
        '                If SAPTableRow.Name2.Length > 0 Then
        '                    newDealerDetailRow("NAME_2") = SAPTableRow.Name2
        '                End If

        '                m_tblSearchResult.Rows.Add(newDealerDetailRow)
        '            End If

        '        Next



        '        'For Each SAPTableRow In tblTemp.Rows
        '        '    'Die Detaildaten zu den Händlern in die Tabelle m_tblHaendler schreiben
        '        '    objSAP.Bapi_Customer_Getdetail2("", Right("0000000000" & SAPTableRow("CUSTOMER").ToString, 10), SAPCustomerAdress, SAPCustomerCompanyDetail, SAPCustomerDetail, SAPBapiRet1, SAPCustomerBankDetail)

        '        '    If SAPBapiRet1.Type.Trim(" "c) = "" Or SAPBapiRet1.Type = "S" Or SAPBapiRet1.Type = "I" Then
        '        '        newDealerDetailRow = m_tblSearchResult.NewRow

        '        '        Dim strTemp As String = SAPCustomerAdress.Name
        '        '        If SAPCustomerAdress.Name_2.Length > 0 Then
        '        '            strTemp &= ", " & SAPCustomerAdress.Name_2
        '        '        End If
        '        '        If SAPCustomerAdress.Name_3.Length > 0 Then
        '        '            strTemp &= ", " & SAPCustomerAdress.Name_3
        '        '        End If
        '        '        If SAPCustomerAdress.Name_4.Length > 0 Then
        '        '            strTemp &= ", " & SAPCustomerAdress.Name_4
        '        '        End If

        '        '        newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPCustomerAdress.Countryiso & " - " & SAPCustomerAdress.Postl_Code & " " & SAPCustomerAdress.City & ", " & SAPCustomerAdress.Street
        '        '        newDealerDetailRow("ADDRESSNUMBER") = SAPCustomerDetail.Customer
        '        '        If SAPCustomerAdress.Postl_Code.Length > 0 Then
        '        '            newDealerDetailRow("POSTL_CODE") = SAPCustomerAdress.Postl_Code
        '        '        End If
        '        '        If SAPCustomerAdress.Street.Length > 0 Then
        '        '            newDealerDetailRow("STREET") = SAPCustomerAdress.Street
        '        '        End If
        '        '        If SAPCustomerAdress.Countryiso.Length > 0 Then
        '        '            newDealerDetailRow("COUNTRYISO") = SAPCustomerAdress.Countryiso
        '        '        End If
        '        '        If SAPCustomerAdress.City.Length > 0 Then
        '        '            newDealerDetailRow("CITY") = SAPCustomerAdress.City
        '        '        End If
        '        '        If SAPCustomerAdress.Name.Length > 0 Then
        '        '            newDealerDetailRow("NAME") = SAPCustomerAdress.Name
        '        '        End If
        '        '        If SAPCustomerAdress.Name_2.Length > 0 Then
        '        '            newDealerDetailRow("NAME_2") = SAPCustomerAdress.Name_2
        '        '        End If

        '        '        m_tblSearchResult.Rows.Add(newDealerDetailRow)
        '        '    End If

        '        'Next         

        '        Return m_tblSearchResult.Rows.Count
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        objSAP2.Connection.Close()
        '        objSAP2.Dispose()
        '    End Try
        'End Function
#End Region
    End Class
End Namespace