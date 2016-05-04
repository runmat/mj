Option Explicit On
Option Infer On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security

Public Class VFS03
    REM ß Lese-/Schreibfunktion, Kunde: VFS,
    REM ß Show - BAPI: --keine--,
    REM ß Change - BAPI: z_M_Offeneanforderungen_storno

    Inherits BankBase

#Region " Declarations"
    Private _vorname As String
    Private _name As String
    Private _strasse As String
    Private _telefon As String
    Private _hausnummer As String
    Private _postleitzahl As String
    Private _ort As String
    Private _offenBest As String
    Private _bestGesamt As String
    Private _agenturnummer As String
    Private _anzahlKennzeichen As Integer
    Private _express As Boolean
    Private _confirm As Boolean
    Private _complete As Boolean
    Private _emailAdresse As String
    Private _keineEmailAdresse As Boolean
    Private _anrede As String
    Private _mehrfachBestellung As Boolean = False

    Private _weAnrede As String = ""
    Private _weVorname As String = ""
    Private _weName As String = ""
    Private _weStrasse As String = ""
    Private _weTelefon As String = ""
    Private _weHausnummer As String = ""
    Private _wePostleitzahl As String = ""
    Private _weOrt As String = ""
    Private _lastOrder As Nullable(Of Date)

    Private _lastOrderAdress As DataTable
    Private _freiLoeschListe As DataTable
    Private _geoCodeDaten As DataTable
    Private _minMax As Boolean = False
    Private _auftragsnummer As String
    Private _sperreGrund As String
    Private _errMessage As String

    Private _anzahlAAP As Integer
    Private _eSubrc As String
    Private _eMessage As String
    Private _datumVon As Date
    Private _datumBis As Date
    Private _status As String
#End Region

#Region " Properties"
    Public Property KeineEmailAdresse() As Boolean
        Get
            Return _keineEmailAdresse
        End Get
        Set(ByVal value As Boolean)
            _keineEmailAdresse = Value
        End Set
    End Property

    Public Property EmailAdresse() As String
        Get
            Return _EmailAdresse
        End Get
        Set(ByVal value As String)
            _EmailAdresse = Value
        End Set
    End Property

    Public Property Mehrfachbestellung() As Boolean
        Get
            Return _mehrfachBestellung
        End Get
        Set(ByVal value As Boolean)
            _mehrfachBestellung = value
        End Set
    End Property

    Public Property Anrede() As String
        Get
            Return _anrede
        End Get
        Set(ByVal value As String)
            _anrede = Value
        End Set
    End Property

    Public Property Confirm() As Boolean
        Get
            Return _confirm
        End Get
        Set(ByVal value As Boolean)
            _confirm = Value
        End Set
    End Property

    Public Property Complete() As Boolean
        Get
            Return _complete
        End Get
        Set(ByVal value As Boolean)
            _complete = Value
        End Set
    End Property

    Public Property Vorname() As String
        Get
            Return _vorname
        End Get
        Set(ByVal value As String)
            _vorname = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = Value
        End Set
    End Property

    Public Property Strasse() As String
        Get
            Return _strasse
        End Get
        Set(ByVal value As String)
            _strasse = Value
        End Set
    End Property

    Public Property Telefon() As String
        Get
            Return _telefon
        End Get
        Set(ByVal value As String)
            _telefon = Value
        End Set
    End Property

    Public Property Hausnummer() As String
        Get
            Return _Hausnummer
        End Get
        Set(ByVal value As String)
            _Hausnummer = Value
        End Set
    End Property

    Public Property Postleitzahl() As String
        Get
            Return _Postleitzahl
        End Get
        Set(ByVal value As String)
            _Postleitzahl = Value
        End Set
    End Property

    Public Property Ort() As String
        Get
            Return _Ort
        End Get
        Set(ByVal value As String)
            _Ort = Value
        End Set
    End Property

    Public Property OffenBest() As String
        Get
            Return _offenBest
        End Get
        Set(ByVal value As String)
            _offenBest = value
        End Set
    End Property

    Public Property BestGesamt() As String
        Get
            Return _BestGesamt
        End Get
        Set(ByVal value As String)
            _BestGesamt = Value
        End Set
    End Property

    Public Property Agenturnummer() As String
        Get
            Return _Agenturnummer
        End Get
        Set(ByVal value As String)
            _Agenturnummer = Value
        End Set
    End Property

    Public Property AnzahlKennzeichen() As Integer
        Get
            Return _anzahlKennzeichen
        End Get
        Set(ByVal value As Integer)
            _anzahlKennzeichen = Value
        End Set
    End Property

    Public Property Express() As Boolean
        Get
            Return _express
        End Get
        Set(ByVal value As Boolean)
            _express = Value
        End Set
    End Property


    Public Property WEAnrede() As String
        Get
            Return _WEAnrede
        End Get
        Set(ByVal value As String)
            _WEAnrede = Value
        End Set
    End Property

    Public Property WEVorname() As String
        Get
            Return _WEVorname
        End Get
        Set(ByVal value As String)
            _WEVorname = Value
        End Set
    End Property

    Public Property WEName() As String
        Get
            Return _WEName
        End Get
        Set(ByVal value As String)
            _WEName = Value
        End Set
    End Property

    Public Property WEStrasse() As String
        Get
            Return _WEStrasse
        End Get
        Set(ByVal value As String)
            _WEStrasse = Value
        End Set
    End Property

    Public Property WETelefon() As String
        Get
            Return _WETelefon
        End Get
        Set(ByVal value As String)
            _WETelefon = Value
        End Set
    End Property

    Public Property WEHausnummer() As String
        Get
            Return _WEHausnummer
        End Get
        Set(ByVal value As String)
            _WEHausnummer = Value
        End Set
    End Property

    Public Property WEPostleitzahl() As String
        Get
            Return _WEPostleitzahl
        End Get
        Set(ByVal value As String)
            _WEPostleitzahl = Value
        End Set
    End Property

    Public Property WEOrt() As String
        Get
            Return _WEOrt
        End Get
        Set(ByVal value As String)
            _WEOrt = Value
        End Set
    End Property
    Public Property LastOrderAdress() As DataTable
        Get
            Return _lastOrderAdress
        End Get
        Set(ByVal value As DataTable)
            _lastOrderAdress = Value
        End Set
    End Property
    Public Property GeoCodeDaten() As DataTable
        Get
            Return _geoCodeDaten
        End Get
        Set(ByVal value As DataTable)
            _geoCodeDaten = Value
        End Set
    End Property

    Public Property LastOrder() As Nullable(Of Date)
        Get
            Return _LastOrder
        End Get
        Set(ByVal value As Nullable(Of Date))
            _LastOrder = value
        End Set
    End Property
    Public Property MinMax() As Boolean
        Get
            Return _minMax
        End Get
        Set(ByVal value As Boolean)
            _minMax = Value
        End Set
    End Property
    Public Property Auftragsnummer() As String
        Get
            Return _auftragsnummer
        End Get
        Set(ByVal value As String)
            _auftragsnummer = Value
        End Set
    End Property

    Public Property ErrMessage() As String
        Get
            Return _errMessage
        End Get
        Set(ByVal value As String)
            _errMessage = Value
        End Set
    End Property

    Public Property AnzahlAAP() As Integer
        Get
            Return _anzahlAAP
        End Get
        Set(ByVal value As Integer)
            _anzahlAAP = Value
        End Set
    End Property

    Public Property SperreGrund() As String
        Get
            Return _SperreGrund
        End Get
        Set(ByVal value As String)
            _SperreGrund = Value
        End Set
    End Property
    Public Property DatumVon() As Date
        Get
            Return _datumVon
        End Get
        Set(ByVal value As DateTime)
            _datumVon = value
        End Set
    End Property
    Public Property DatumBis() As Date
        Get
            Return _datumBis
        End Get
        Set(ByVal value As DateTime)
            _datumBis = value
        End Set
    End Property
    Public Property StatusFilter() As String
        Get
            Return _status
        End Get
        Set(ByVal value As System.String)
            _status = value
        End Set
    End Property
    Public Property Frei_Loesch_Liste() As DataTable
        Get
            Return _freiLoeschListe
        End Get
        Set(ByVal value As DataTable)
            _freiLoeschListe = Value
        End Set
    End Property

    Public Property Versicherungsjahr As String


#End Region

#Region " Methods"
    Public Sub New(ByRef user As User, ByRef app As CKG.Base.Kernel.Security.App, ByVal appID As String, ByVal sessionID As String, ByVal fileName As String)
        MyBase.New(user, app, appID, sessionID, fileName)

        _vorname = ""
        _name = ""
        _strasse = ""
        _hausnummer = ""
        _postleitzahl = ""
        _ort = ""
        _agenturnummer = ""
        _anzahlKennzeichen = 5
        _express = False
        _confirm = False
        _complete = False
        _emailAdresse = ""
        _keineEmailAdresse = False
        _auftragsnummer = ""
        _errMessage = ""
    End Sub

    Public Overloads Overrides Sub show()
        'nur wegen bankbase
    End Sub

    Public Overloads Sub Show(ByVal HEZ As String)
        'GIBT'S HIER NICHT!!!

        '_ClassAndMethod = "VFS03.Show"
        'If Not m_blnGestartet Then
        '    m_blnGestartet = True

        '    Dim objSAP As New SAPProxy_ComCommon.SAPProxy_ComCommon()

        '    Dim tblAuftraegeSAP As New SAPProxy_ComCommon.ZDAD_M_WEB_AUFTRAEGETable()

        '    MakeDestination()
        '    objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        '    objSAP.Connection.Open()

        '    If m_objLogApp Is Nothing Then
        '        m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '    End If
        '    m_intIDSAP = -1
        '    Dim i As Int32
        '    Dim rowTemp As DataRow

        '    Try
        '        m_intStatus = 0
        '        _Message = ""
        '        Dim strKKBER As String

        '        m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Offene_Anforderungen_001", _AppID, _SessionID, m_objUser.CurrentLogAccessASPXID)

        '        objSAP.Z_M_Offene_Anforderungen_001(Right("0000000000" & m_objUser.Customer.KUNNR, 10), _Haendler, HEZ, "", "1510", tblAuftraegeSAP)

        '        objSAP.CommitWork()
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
        '        End If

        '        m_tblRaw = tblAuftraegeSAP.ToADODataTable
        '        m_tblRaw.AcceptChanges()
        '        For Each rowTemp In m_tblRaw.Rows
        '            Select Case rowTemp("BSTZD")
        '                Case "0001"
        '                    rowTemp("BSTZD") = "Standard tempor‰r"
        '                Case "0002"
        '                    rowTemp("BSTZD") = "Standard endg¸ltig"
        '                Case "0005"
        '                    rowTemp("BSTZD") = "H‰ndlereigene Zulassung"
        '            End Select
        '            If CStr(rowTemp("CMGST")) = "B" Then
        '                rowTemp("CMGST") = "X"
        '            Else
        '                rowTemp("CMGST") = ""
        '            End If
        '        Next
        '        m_tblRaw.AcceptChanges()
        '        m_tblAuftraege = CreateOutPut(m_tblRaw, _AppID)
        '        m_tblResultExcel = m_tblAuftraege.Copy
        '        m_tblResultExcel.Columns.Remove("VBELN")
        '        m_tblResultExcel.Columns.Remove("EQUNR")


        '        If m_tblAuftraege.Rows.Count = 0 Then
        '            m_intStatus = 0
        '            _Message = "Keine Daten gefunden."
        '        End If

        '        If _Haendler.Length > 0 Then
        '            Haendler = _Haendler
        '        End If
        '    Catch ex As Exception
        '        Select Case ex.Message
        '            Case "NO_DATA"
        '                m_intStatus = 0
        '                If m_hez = True Then
        '                    m_intStatus = -2501
        '                End If
        '                _Message = "Keine Daten gefunden."
        '            Case Else
        '                m_intStatus = -9999
        '                _Message = ex.Message
        '        End Select
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, _Message)
        '        End If
        '    Finally
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
        '        End If

        '        objSAP.Connection.Close()
        '        objSAP.Dispose()

        '        m_blnGestartet = False
        '    End Try
        'End If
    End Sub


    Public Function checkVertriebsdirektion(ByVal code As String, ByVal page As Page) As Boolean
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_MOFA_VDPRUEFUNG_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("VD", code)

                myProxy.callBapi()

                Dim strResult = myProxy.getExportParameter("OK")
                If Not strResult = "X" Then
                    Return False
                Else
                    Return True
                End If

            Catch ex As Exception

                m_intStatus = -9999
                m_strMessage = ex.Message
                Return False

            Finally

                m_blnGestartet = False
            End Try
        End If
    End Function


    Public Sub GetLastOrder(ByVal vermittlernummer As String, ByVal appID As String, ByVal sessionID As String, ByVal page As Page)

        m_strClassAndMethod = "vfs03.GetLastOrder"
        m_strAppID = appID
        m_strSessionID = sessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERS_BESTELLPRUEF_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                myProxy.setImportParameter("I_KUN_EXT_VM", vermittlernummer)

                myProxy.callBapi()

                _lastOrderAdress = myProxy.getExportTable("GS_WEB")

                _lastOrder = CDate(myProxy.getExportParameter("E_DAT_BST"))

                If Not _lastOrderAdress Is Nothing Then

                    If _lastOrderAdress.Rows.Count > 0 Then

                        With _lastOrderAdress.Rows(0)
                            Anrede = .Item("ANRED").ToString
                            Vorname = .Item("NAME1").ToString
                            Name = .Item("NAME2").ToString
                            Strasse = .Item("STREET").ToString
                            Hausnummer = .Item("HOUSE_NUM1").ToString
                            Postleitzahl = .Item("POST_CODE1").ToString
                            Ort = .Item("CITY1").ToString
                            Telefon = .Item("TEL_NUMBER").ToString
                            EmailAdresse = .Item("SMTP_ADDR").ToString
                            _offenBest = .Item("ANZ_VERK").ToString
                            _bestGesamt = .Item("ANZ_GES").ToString
                        End With

                    End If

                End If

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If


    End Sub

    Public Sub Bestellen(ByVal page As Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True
            If m_objLogApp Is Nothing Then
                m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim strVersand As String
                If _express Then
                    strVersand = " "
                Else
                    strVersand = "X"
                End If

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERS_BESTELLUNG_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                myProxy.setImportParameter("I_ANREDE", _anrede)
                myProxy.setImportParameter("I_NAME1", _vorname)
                myProxy.setImportParameter("I_NAME2", _name)
                myProxy.setImportParameter("I_STREET", _strasse)
                myProxy.setImportParameter("I_HOUSE_NUM1", _Hausnummer)
                myProxy.setImportParameter("I_POST_CODE1", _Postleitzahl)
                myProxy.setImportParameter("I_CITY1", _Ort)
                myProxy.setImportParameter("I_AGENTUR", _Agenturnummer)
                myProxy.setImportParameter("I_ANZAHL", _anzahlKennzeichen.ToString)
                myProxy.setImportParameter("I_VERSART", strVersand)
                If _anzahlAAP >= 0 Then
                    myProxy.setImportParameter("I_ANZ_APVTG", _anzahlAAP.ToString)
                End If
                myProxy.setImportParameter("I_SMTP_ADDR", _EmailAdresse)
                myProxy.setImportParameter("I_TELF1", _telefon)

                If Mehrfachbestellung Then
                    myProxy.setImportParameter("I_BEST_ANL", "X")
                End If
                myProxy.setImportParameter("WE_ANREDE", _WEAnrede)
                myProxy.setImportParameter("WE_NAME1", _WEVorname)
                myProxy.setImportParameter("WE_NAME2", _WEName)
                myProxy.setImportParameter("WE_STREET", _WEStrasse)
                myProxy.setImportParameter("WE_HOUSE_NUM1", _WEHausnummer)
                myProxy.setImportParameter("WE_POST_CODE1", _WEPostleitzahl)
                myProxy.setImportParameter("WE_CITY1", _WEOrt)
                myProxy.setImportParameter("WE_TELF1", _WETelefon)

                myProxy.setImportParameter("I_VERS_JAHR", Versicherungsjahr)

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_VERS_BESTELLUNG_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)


                myProxy.callBapi()

                Dim bestVor = myProxy.getExportParameter("E_BEST_VORH")
                Auftragsnummer = myProxy.getExportParameter("E_SPERRE_WEB_ID")

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                'If Not BestVor Is Nothing Then
                '    If Not BestVor = "0" And Not BestVor = "X" Then
                '        m_intStatus = -2222
                '        m_strMessage = "heutige Bestellvorg‰nge: " & bestVor



                '        Exit Sub
                '    End If
                'End If
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        ' m_intStatus = -9999
                        m_intStatus = -2222
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub GetGesperrte(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        m_strClassAndMethod = "vfs03.Z_M_VERS_FREIGABE_CENTER_001"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERS_FREIGABE_CENTER_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("KUNNR", m_objUser.Customer.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("SPERRGRUND", SperreGrund)

                myProxy.callBapi()

                _lastOrderAdress = myProxy.getExportTable("O_OUTPUT")
                _eSubrc = myProxy.getExportParameter("E_SUBRC")
                _eMessage = myProxy.getExportParameter("E_MESSAGE")
                _lastOrderAdress.Columns("UZEIT_LETZT_BEST").MaxLength = 8
                _lastOrderAdress.Columns.Add("STATUS", GetType(System.String))

                For Each row As DataRow In _lastOrderAdress.Rows
                    Dim sAgentur = row("AGENTUR").ToString
                    sAgentur = Left(sAgentur, 4) & "-" & Mid(sAgentur, 5, 4) & "-" & Right(sAgentur, 1)
                    row("AGENTUR") = sAgentur

                    Dim sOrderTime = row("UZEIT_LETZT_BEST").ToString
                    If sOrderTime.Length = 6 AndAlso sOrderTime <> "000000" Then
                        sOrderTime = sOrderTime.Substring(0, 2) & ":" & sOrderTime.Substring(2, 2) & ":" & sOrderTime.Substring(4, 2)
                    Else
                        sOrderTime = ""
                    End If
                    row("UZEIT_LETZT_BEST") = sOrderTime
                    Dim sperre = row("SPERRE_BST").ToString
                    Dim loeschKZ = row("LOEKZ").ToString

                    If SperreGrund = "3" Then
                        sperre = row("SPERRE_WEB").ToString
                        If sperre.Trim = "X" Then
                            row("STATUS") = "Gesperrt"
                        End If
                        If loeschKZ.Trim = "X" Then
                            row("STATUS") = "Storniert"
                        End If
                    Else
                        If sperre.Trim = String.Empty Then
                            row("STATUS") = "Freigegeben"
                        End If
                        If sperre.Trim = "X" Then
                            row("STATUS") = "Gesperrt"
                        End If
                        If loeschKZ.Trim = "X" Then
                            row("STATUS") = "Storniert"
                        End If
                    End If
                Next

                ResultExcel = New DataTable

                With ResultExcel.Columns
                    .Add("Agenturnummer", GetType(String))
                    .Add("Verkehrsjahr", GetType(String))
                    .Add("Auftragsnr", GetType(String))
                    .Add("Anrede", GetType(String))
                    .Add("Name1", GetType(String))
                    .Add("Name2", GetType(String))
                    .Add("Straﬂe", GetType(String))
                    .Add("Nummer", GetType(String))
                    .Add("PLZ", GetType(String))
                    .Add("Ort", GetType(String))
                    .Add("Telefon", GetType(String))
                    .Add("E-Mail", GetType(String))
                    .Add("abw. Adr. Anrede", GetType(String))
                    .Add("abw. Adr. Name1", GetType(String))
                    .Add("abw. Adr. Name2", GetType(String))
                    .Add("abw. Adr. Straﬂe", GetType(String))
                    .Add("abw. Adr. Nummer", GetType(String))
                    .Add("abw. Adr. PLZ", GetType(String))
                    .Add("abw. Adr. Ort", GetType(String))
                    .Add("abw. Adr. Telefon", GetType(String))
                    .Add("Menge", GetType(String))
                    .Add("Anz. aap-Vertr‰ge", GetType(String))
                    .Add("Status", GetType(String))
                End With
                For Each orderRow As DataRow In _lastOrderAdress.Rows
                    Dim newRow = ResultExcel.NewRow
                    newRow("Agenturnummer") = orderRow("AGENTUR")
                    newRow("Verkehrsjahr") = orderRow("VERS_JAHR")
                    newRow("Auftragsnr") = orderRow("SPERRE_WEB_ID")
                    newRow("Anrede") = orderRow("ANRED_MEDI")
                    newRow("Name1") = orderRow("NAME1")
                    newRow("Name2") = orderRow("NAME2")
                    newRow("Straﬂe") = orderRow("STREET")
                    newRow("Nummer") = orderRow("HOUSE_NUM1")
                    newRow("PLZ") = orderRow("POST_CODE1")
                    newRow("Ort") = orderRow("CITY1")
                    newRow("Telefon") = orderRow("TELF1")
                    newRow("E-Mail") = orderRow("SMTP_ADDR")
                    newRow("abw. Adr. Anrede") = orderRow("WE_ANRED_MEDI")
                    newRow("abw. Adr. Name1") = orderRow("WE_NAME1")
                    newRow("abw. Adr. Name2") = orderRow("WE_NAME2")
                    newRow("abw. Adr. Straﬂe") = orderRow("WE_STREET")
                    newRow("abw. Adr. PLZ") = orderRow("WE_HOUSE_NUM1")
                    newRow("abw. Adr. Ort") = orderRow("WE_POST_CODE1")
                    newRow("abw. Adr. Telefon") = orderRow("WE_TELF1")
                    newRow("Menge") = orderRow("ANZAHL")
                    newRow("Anz. aap-Vertr‰ge") = orderRow("ANZ_APVTG")
                    newRow("Status") = orderRow("STATUS")
                    ResultExcel.Rows.Add(newRow)
                    ResultExcel.AcceptChanges()
                Next

                _lastOrderAdress.AcceptChanges()

                If _eSubrc <> "0" Then
                    m_intStatus = CInt(_eSubrc)
                    m_strMessage = _eMessage
                End If

            Catch ex As Exception
                m_strMessage = "Keine Daten gefunden."
            Finally
                m_blnGestartet = False
            End Try
        End If


    End Sub


    Public Sub GetFreig_Loesch_Liste(ByVal appID As String, ByVal sessionID As String, ByVal page As Page)

        m_strClassAndMethod = "vfs03.Z_M_VERS_FREIG_LOESCH_LIST_001"
        m_strAppID = appID
        m_strSessionID = sessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERS_FREIG_LOESCH_LIST_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("AG", m_objUser.Customer.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("AB_DAT", _datumVon.ToString)
                myProxy.setImportParameter("AB_DAT_BIS", _datumBis.ToString)
                myProxy.setImportParameter("STATUS", _status.ToString)

                myProxy.callBapi()

                Frei_Loesch_Liste = myProxy.getExportTable("GT_OUT")

                _eSubrc = myProxy.getExportParameter("E_SUBRC")
                _eMessage = myProxy.getExportParameter("E_MESSAGE")
                _freiLoeschListe.Columns("AEZEIT").MaxLength = 8

                For Each row As DataRow In _freiLoeschListe.Rows

                    Dim sAgentur As String = row("AGENTUR").ToString
                    sAgentur = Left(sAgentur, 4) & "-" & Mid(sAgentur, 5, 4) & "-" & Right(sAgentur, 1)
                    row("AGENTUR") = sAgentur

                    Dim sAendTime As String = row("AEZEIT").ToString
                    If sAendTime.Length = 6 AndAlso sAendTime <> "000000" Then
                        sAendTime = sAendTime.Substring(0, 2) & ":" & sAendTime.Substring(2, 2) & ":" & sAendTime.Substring(4, 2)
                    Else
                        sAendTime = ""
                    End If
                    row("AEZEIT") = sAendTime

                Next

                ResultExcel = New DataTable

                With ResultExcel.Columns
                    .Add("Agenturnummer", GetType(String))
                    .Add("Auftragsnr", GetType(String))
                    .Add("Anrede", GetType(String))
                    .Add("Name1", GetType(String))
                    .Add("Name2", GetType(String))
                    .Add("Straﬂe", GetType(String))
                    .Add("Nummer", GetType(String))
                    .Add("PLZ", GetType(String))
                    .Add("Ort", GetType(String))
                    .Add("Telefon", GetType(String))
                    .Add("E-Mail", GetType(String))
                    .Add("abw. Adr. Anrede", GetType(String))
                    .Add("abw. Adr. Name1", GetType(String))
                    .Add("abw. Adr. Name2", GetType(String))
                    .Add("abw. Adr. Straﬂe", GetType(String))
                    .Add("abw. Adr. Nummer", GetType(String))
                    .Add("abw. Adr. PLZ", GetType(String))
                    .Add("abw. Adr. Ort", GetType(String))
                    .Add("abw. Adr. Telefon", GetType(String))
                    .Add("Menge", GetType(String))
                    .Add("Anz. aap-Vertr‰ge", GetType(String))
                    .Add("bearbeitet von", GetType(String))
                    .Add("bearbeitet am", GetType(String))
                    .Add("bearbeitet um", GetType(String))
                    .Add("Status", GetType(String))
                End With
                For Each orderRow As DataRow In Frei_Loesch_Liste.Rows
                    Dim dataRow = ResultExcel.NewRow
                    dataRow("Agenturnummer") = orderRow("AGENTUR")
                    dataRow("Auftragsnr") = orderRow("SPERRE_WEB_ID")
                    dataRow("Anrede") = orderRow("ANRED_MEDI")

                    dataRow("Name1") = orderRow("NAME1")
                    dataRow("Name2") = orderRow("NAME2")
                    dataRow("Straﬂe") = orderRow("STREET")
                    dataRow("Nummer") = orderRow("HOUSE_NUM1")
                    dataRow("PLZ") = orderRow("POST_CODE1")
                    dataRow("Ort") = orderRow("CITY1")
                    dataRow("Telefon") = orderRow("TELF1")
                    dataRow("E-Mail") = orderRow("SMTP_ADDR")
                    dataRow("abw. Adr. Anrede") = orderRow("WE_ANRED_MEDI")
                    dataRow("abw. Adr. Name1") = orderRow("WE_NAME1")
                    dataRow("abw. Adr. Name2") = orderRow("WE_NAME2")
                    dataRow("abw. Adr. Straﬂe") = orderRow("WE_STREET")
                    dataRow("abw. Adr. PLZ") = orderRow("WE_HOUSE_NUM1")
                    dataRow("abw. Adr. Ort") = orderRow("WE_POST_CODE1")
                    dataRow("abw. Adr. Telefon") = orderRow("WE_TELF1")
                    dataRow("Menge") = orderRow("ANZAHL")
                    dataRow("Anz. aap-Vertr‰ge") = orderRow("ANZ_APVTG")
                    dataRow("bearbeitet von") = orderRow("AENAM")
                    Dim sAendDate = orderRow("AEDAT").ToString
                    If IsDate(sAendDate) Then
                        dataRow("bearbeitet am") = CDate(sAendDate).ToShortDateString
                    End If
                    dataRow("bearbeitet um") = orderRow("AEZEIT")
                    dataRow("Status") = orderRow("STATUS_TEXT")
                    ResultExcel.Rows.Add(dataRow)
                    ResultExcel.AcceptChanges()
                Next

                Frei_Loesch_Liste.AcceptChanges()

                If _eSubrc <> "0" Then
                    m_intStatus = CInt(_eSubrc)
                    m_strMessage = _eMessage
                End If

            Catch ex As Exception
                m_strMessage = "Keine Daten gefunden."
            Finally
                m_blnGestartet = False
            End Try
        End If


    End Sub

    Public Overrides Sub Change()

        If Not m_blnGestartet Then
            m_blnGestartet = True
            If m_objLogApp Is Nothing Then
                m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Mofa_Anford_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_MOFA_ANFORD_001", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                proxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_ANREDE", _anrede)
                proxy.setImportParameter("I_NAME1", _vorname)
                proxy.setImportParameter("I_NAME2", _name)
                proxy.setImportParameter("I_STREET", _strasse)
                proxy.setImportParameter("I_HOUSE_NUM1", _Hausnummer)
                proxy.setImportParameter("I_POST_CODE1", _Postleitzahl)
                proxy.setImportParameter("I_CITY1", _Ort)
                proxy.setImportParameter("I_KUN_VD", "")
                proxy.setImportParameter("I_AGENTUR", _Agenturnummer)
                proxy.setImportParameter("I_ANZAHL", _anzahlKennzeichen.ToString)
                proxy.setImportParameter("I_VERSART", If(_express, " ", "X"))
                proxy.setImportParameter("I_SMTP_ADDR", _EmailAdresse)
                proxy.setImportParameter("I_TELF1", _telefon)
                proxy.setImportParameter("I_BEST_ANL", If(Mehrfachbestellung, "X", ""))

                proxy.callBapi()

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                Dim E_BEST_VORH = proxy.getExportParameter("E_BEST_VORH")

                If Not String.IsNullOrEmpty(E_BEST_VORH) Then
                    If Not E_BEST_VORH = "0" And Not Mehrfachbestellung Then
                        m_intStatus = -2222
                        m_strMessage = String.Format("heutige Bestellvorg‰nge: {0}", E_BEST_VORH)
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        ' m_intStatus = -9999
                        m_intStatus = -2222
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    '----------------------------------------------------------------------
    ' Methode:      GeoAdressen
    ' Autor:        SFa
    ' Beschreibung: Pr¸ft die ¸bergebene Adresse auf Richtigkeit und liefert
    '               ggf. Alternativadresse zur¸ck
    ' Erstellt am:  17.09.2008
    ' ITA:          2150
    '----------------------------------------------------------------------
    Public Sub GeoAdressen(ByVal strAppID As String, _
                                ByVal strSessionID As String, _
                                ByVal page As Page, ByVal bWEAdresse As Boolean)

        m_strClassAndMethod = "vfs03.GeoAdressen"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_CHECK_ADRESS", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_LAND", "DE")
                If bWEAdresse = False Then
                    myProxy.setImportParameter("I_STRASSE", Strasse)
                    myProxy.setImportParameter("I_HAUSNR", Hausnummer)
                    myProxy.setImportParameter("I_POSTLTZ", Postleitzahl)
                    myProxy.setImportParameter("I_ORT", Ort)
                Else
                    myProxy.setImportParameter("I_STRASSE", WEStrasse)
                    myProxy.setImportParameter("I_HAUSNR", WEHausnummer)
                    myProxy.setImportParameter("I_POSTLTZ", WEPostleitzahl)
                    myProxy.setImportParameter("I_ORT", WEOrt)
                End If

                myProxy.callBapi()

                _geoCodeDaten = myProxy.getExportTable("GT_GEO")

            Catch ex As Exception
                _geoCodeDaten = Nothing
            Finally
                m_blnGestartet = False
            End Try
        End If


    End Sub

    Public Sub Storno(ByVal agentur As String, ByVal auftragsnummer As String, ByVal page As Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True
            If m_objLogApp Is Nothing Then
                m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERS_ABLEHNUNG_CENTER_001", m_objApp, m_objUser, page)

                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_AGENTUR", agentur)
                myProxy.setImportParameter("I_SPERRE_WEB_ID", auftragsnummer)
                myProxy.setImportParameter("I_AENAM", Left(m_objUser.UserName, 12))
                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_VERS_ABLEHNUNG_CENTER_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)


                myProxy.callBapi()

                m_strMessage = myProxy.getExportParameter("O_OK")


                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If


            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NOT_FOUND"
                        m_strMessage = "Datensatz nicht gefunden."
                    Case "NOT_CHANGED"
                        m_strMessage = "Datensatz konnte nicht ge‰ndert werden"
                    Case Else
                        ' m_intStatus = -9999
                        m_intStatus = -2222
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Freigabe(ByVal agentur As String, ByVal auftragsnummer As String, ByVal page As Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True
            If m_objLogApp Is Nothing Then
                m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERS_FREIGABE_BST_010", m_objApp, m_objUser, page)

                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_AGENTUR", agentur)
                myProxy.setImportParameter("I_SPERRE_WEB_ID", auftragsnummer)
                myProxy.setImportParameter("I_AENAM", Left(m_objUser.UserName, 12))
                myProxy.setImportParameter("I_ANZAHL", _anzahlKennzeichen.ToString)
                If _anzahlAAP >= 0 Then
                    myProxy.setImportParameter("I_ANZ_APVTG", _anzahlAAP.ToString)
                End If
                myProxy.setImportParameter("I_SMTP_ADDR", _EmailAdresse)
                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_VERS_FREIGABE_BST_010", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)


                myProxy.callBapi()

                m_strMessage = myProxy.getExportParameter("O_OK")


                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If


            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NOT_FOUND"
                        m_strMessage = "Datensatz nicht gefunden."
                    Case "NOT_CHANGED"
                        m_strMessage = "Datensatz konnte nicht ge‰ndert werden"
                    Case Else
                        ' m_intStatus = -9999
                        m_intStatus = -2222
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Function GenerateValidationLink(ByVal Vermittlernummer As String) As String

        Dim KeyVermittlernummer As String

        Dim Crypt As New CKG.Base.Kernel.Security.CryptNew

        KeyVermittlernummer = Crypt.psEncrypt(Vermittlernummer)


        Return KeyVermittlernummer


    End Function

#End Region
End Class
' ************************************************
' $History: vfs03.vb $
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 7.02.11    Time: 16:49
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 16.12.10   Time: 12:16
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 14.12.10   Time: 15:18
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 10.12.10   Time: 9:17
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 7.12.10    Time: 13:13
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 11.11.10   Time: 14:28
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 15.03.10   Time: 13:01
' Updated in $/CKAG2/Applications/AppInsurance/lib
' ITA: 2918
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 2.02.10    Time: 16:15
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 20.01.10   Time: 18:06
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 27.11.09   Time: 13:18
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 26.11.09   Time: 17:03
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 29.10.09   Time: 15:15
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 26.10.09   Time: 14:30
' Updated in $/CKAG2/Applications/AppInsurance/lib
' ITA: 3249, 3206
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 22.10.09   Time: 14:30
' Updated in $/CKAG2/Applications/AppInsurance/lib
' ITA: 3206
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 8.10.09    Time: 14:27
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 27.03.09   Time: 12:29
' Updated in $/CKAG2/Applications/AppGenerali/lib
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 12.01.09   Time: 13:40
' Created in $/CKAG2/Applications/AppGenerali/lib
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 12.01.09   Time: 13:28
' Created in $/CKG/Services/Applications/AppGenerali/lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 9.12.08    Time: 11:06
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2433 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.08.08   Time: 9:22
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2086 SapConnector Part entfernt
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 22.08.08   Time: 9:15
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2086 fertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:43
' Created in $/CKAG/Applications/appvfs/Lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.03.08   Time: 10:15
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 21.02.08   Time: 10:29
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA:1727
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 31.01.08   Time: 8:52
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1466
' 
' *****************  Version 4  *****************
' User: Uha          Date: 24.01.08   Time: 13:07
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1644: Oberfl‰chenkosmetik (Vertriebsdirektion entfernt) - BAPI nach
' wie vor funktionslos
' 
' *****************  Version 3  *****************
' User: Uha          Date: 23.01.08   Time: 12:36
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1644: Formular mit Pr¸fung (BAPI immer noch funktionslos)
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.01.08   Time: 14:52
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1644: Change01 und VFS03 - Vorversion, da BAPI derzeit nur H¸lle
' 
' *****************  Version 1  *****************
' User: Uha          Date: 22.01.08   Time: 13:16
' Created in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1644: Change01 und VFS03 (funktionslos) hinzugef¸gt
' 
' ************************************************