Imports CKG.Base.Business
Imports CKG.Base.Common
Imports GeneralTools.Models

<Serializable()> Public Class Briefversand
    Inherits Base.Business.BankBase

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strAppId As String, ByVal strSessionId As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strAppId, strSessionId, strFilename)
    End Sub

#Region " Declarations"

    Private _mESubrc As String
    Private _mEMessage As String
    Private _mStrFahrgestellnr As String
    Private _mStrKennzeichen As String
    Private _mStrZbiiNr As String
    Private _mStrLVnr As String
    Private _mStrRef1 As String
    Private _mStrRef2 As String
    Private _mStrVersandArt As String
    Private _mTblFahrzeuge As DataTable
    Private _mTblFahrzeugePrint As DataTable
    Private _mTblFahrzeugeFehler As DataTable
    Private _mTblZulStellen As DataTable
    Private _mTblAdressen As DataTable
    Private _mTblVersandgruende As DataTable
    Private _mTblVersandOptions As DataTable
    Private _mKbanr As String 'Zulassungsdienst-Nummer
    Private _mBoolNewAdress As Boolean = False
    Private _mStrAnrede As String
    Private _mStrName1 As String
    Private _mStrName2 As String
    Private _mStrCity As String
    Private _mStrPostcode As String
    Private _mStrStreet As String
    Private _mStrHouseNum As String
    Private _mTblLaender As DataTable
    Private _mStrLaenderKuerzel As String
    Private _mMaterial As String
    Private _mVersandadrZe As String
    Private _mVersandadrZs As String
    Private _mVersandadrtext As String
    Private _mVersgrund As String
    Private _mVersgrundText As String
    Private _mVersartText As String
    Private _strAuftragsstatus As String
    Private _strAuftragsnummer As String
    Private _mAutLevel As String
    Private _mTblUpload As DataTable
    Private _mStrReferenceforAut As String
    Private _mSachbearbeiter As String
    Private _mVersandOhneAbmeldung As String
    Private _mTblVersandError As DataTable
    Private _mEQuiTyp As String
    Private _mBriefversand As String
    Private _mSchluesselVersand As String
    Private _mOptionFlag As String
    Private _mHalter As String
    Private _mBemerkung As String
    Private _mEqTyp As String
    Private _mAdressart As Integer
    Private _mAdressartText As String
    Private _mVersandoptionenText As String
    Private _mAutorisierungText As String
    Private _mBeauftragungsdatum As String
    Private _mTblStueckliste As DataTable
    Private _mBrieflieferanten As DataTable

#End Region

    Public Enum Adressarten
        TempSuche = 1
        TempZulassungsstelle = 2
        TempManuell = 3
        EndSuche = 4
        EndZulassungsstelle = 5
        EndManuell = 6
    End Enum

#Region " Properties "

    Public Property OptionFlag() As String
        Get
            Return _mOptionFlag
        End Get
        Set(ByVal value As String)
            _mOptionFlag = value
        End Set
    End Property

    Public Property Briefversand() As String
        Get
            Return _mBriefversand
        End Get
        Set(ByVal value As String)
            _mBriefversand = value
        End Set
    End Property

    Public Property SchluesselVersand() As String
        Get
            Return _mSchluesselVersand
        End Get
        Set(ByVal value As String)
            _mSchluesselVersand = value
        End Set
    End Property

    Public Property VersartText() As String
        Get
            Return _mVersartText
        End Get
        Set(ByVal value As String)
            _mVersartText = value
        End Set
    End Property

    Public Property VersgrundText() As String
        Get
            Return _mVersgrundText
        End Get
        Set(ByVal value As String)
            _mVersgrundText = value
        End Set
    End Property

    Public Property ReferenceforAut() As String
        Get
            Return _mStrReferenceforAut
        End Get
        Set(ByVal value As String)
            _mStrReferenceforAut = value
        End Set
    End Property

    Public Property AutLevel() As String
        Get
            Return _mAutLevel
        End Get
        Set(ByVal value As String)
            _mAutLevel = value
        End Set
    End Property

    Public Property Kbanr() As String
        Get
            Return _mKbanr
        End Get
        Set(ByVal value As String)
            _mKbanr = value
        End Set
    End Property

    Public Property NeueAdresse() As Boolean
        Get
            Return _mBoolNewAdress
        End Get
        Set(ByVal value As Boolean)
            _mBoolNewAdress = value
        End Set

    End Property

    Public Property VersandArt() As String
        Get
            Return _mStrVersandArt
        End Get
        Set(ByVal value As String)
            _mStrVersandArt = value
        End Set
    End Property

    Public Property Versandgruende() As DataTable
        Get
            Return _mTblVersandgruende
        End Get
        Set(ByVal value As DataTable)
            _mTblVersandgruende = value
        End Set
    End Property

    Public Property VersandOptionen() As DataTable
        Get
            Return _mTblVersandOptions
        End Get
        Set(ByVal value As DataTable)
            _mTblVersandOptions = value
        End Set
    End Property

    Public Property Stueckliste As DataTable
        Get
            Return _mTblStueckliste
        End Get
        Set(value As DataTable)
            _mTblStueckliste = value
        End Set
    End Property

    Public Property Adressen() As DataTable
        Get
            Return _mTblAdressen
        End Get
        Set(ByVal value As DataTable)
            _mTblAdressen = value
        End Set
    End Property

    Public Property ZulStellen() As DataTable
        Get
            Return _mTblZulStellen
        End Get
        Set(ByVal value As DataTable)
            _mTblZulStellen = value
        End Set
    End Property

    Public ReadOnly Property Laender() As DataTable
        Get
            If _mTblLaender Is Nothing Then
                GetLaender(New Page)
            End If
            Return _mTblLaender
        End Get
    End Property

    Public ReadOnly Property Brieflieferanten() As DataTable
        Get
            If _mBrieflieferanten Is Nothing Then
                GetBrieflieferanten(New Page)
            End If
            Return _mBrieflieferanten
        End Get
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return _mTblFahrzeuge
        End Get
        Set(ByVal value As DataTable)
            _mTblFahrzeuge = value
        End Set
    End Property

    Public Property FahrzeugePrint() As DataTable
        Get
            Return _mTblFahrzeugePrint
        End Get
        Set(ByVal value As DataTable)
            _mTblFahrzeugePrint = value
        End Set
    End Property

    Public Property FahrzeugeFehler() As DataTable
        Get
            Return _mTblFahrzeugeFehler
        End Get
        Set(ByVal value As DataTable)
            _mTblFahrzeugeFehler = value
        End Set
    End Property

    Public Property ESubrc() As String
        Get
            Return _mESubrc
        End Get
        Set(ByVal value As String)
            _mESubrc = value
        End Set
    End Property

    Public Property EMessage() As String
        Get
            Return _mEMessage
        End Get
        Set(ByVal value As String)
            _mEMessage = value
        End Set
    End Property

    Public Property Fahrgestellnr() As String
        Get
            Return _mStrFahrgestellnr
        End Get
        Set(ByVal value As String)
            _mStrFahrgestellnr = value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return _mStrKennzeichen
        End Get
        Set(ByVal value As String)
            _mStrKennzeichen = value
        End Set
    End Property

    Public Property Zb2Nr() As String
        Get
            Return _mStrZbiiNr
        End Get
        Set(ByVal value As String)
            _mStrZbiiNr = value
        End Set
    End Property

    Public Property LVnr() As String
        Get
            Return _mStrLVnr
        End Get
        Set(ByVal value As String)
            _mStrLVnr = value
        End Set
    End Property

    Public Property Ref1() As String
        Get
            Return _mStrRef1
        End Get
        Set(ByVal value As String)
            _mStrRef1 = value
        End Set
    End Property

    Public Property Ref2() As String
        Get
            Return _mStrRef2
        End Get
        Set(ByVal value As String)
            _mStrRef2 = value
        End Set
    End Property

    Public Property Anrede() As String
        Get
            Return _mStrAnrede
        End Get
        Set(ByVal value As String)
            _mStrAnrede = value
        End Set

    End Property

    Public Property Name1() As String
        Get
            Return _mStrName1
        End Get
        Set(ByVal value As String)
            _mStrName1 = value
        End Set

    End Property

    Public Property Name2() As String
        Get
            Return _mStrName2
        End Get
        Set(ByVal value As String)
            _mStrName2 = value
        End Set

    End Property

    Public Property City() As String
        Get
            Return _mStrCity
        End Get
        Set(ByVal value As String)
            _mStrCity = value
        End Set
    End Property

    Public Property PostCode() As String
        Get
            Return _mStrPostcode
        End Get
        Set(ByVal value As String)
            _mStrPostcode = value
        End Set
    End Property

    Public Property Street() As String
        Get
            Return _mStrStreet
        End Get
        Set(ByVal value As String)
            _mStrStreet = value
        End Set
    End Property

    Public Property HouseNum() As String
        Get
            Return _mStrHouseNum
        End Get
        Set(ByVal value As String)
            _mStrHouseNum = value
        End Set
    End Property

    Public Property LaenderKuerzel() As String
        Get
            Return _mStrLaenderKuerzel
        End Get
        Set(ByVal value As String)
            _mStrLaenderKuerzel = value
        End Set

    End Property

    Public Property Materialnummer() As String
        Get
            Return _mMaterial
        End Get
        Set(ByVal value As String)
            _mMaterial = value
        End Set
    End Property

    Public Property VersandAdresseZe() As String
        Get
            Return _mVersandadrZe
        End Get
        Set(ByVal value As String)
            _mVersandadrZe = value
        End Set
    End Property

    Public Property VersandAdresseZs() As String
        Get
            Return _mVersandadrZs
        End Get
        Set(ByVal value As String)
            _mVersandadrZs = value
        End Set
    End Property

    Public Property VersandAdresseText() As String
        Get
            Return _mVersandadrtext
        End Get
        Set(ByVal value As String)
            _mVersandadrtext = value
        End Set
    End Property

    Public Property VersandGrund() As String
        Get
            Return _mVersgrund
        End Get
        Set(ByVal value As String)
            _mVersgrund = value
        End Set
    End Property

    Public Property Auftragsstatus() As String
        Get
            Return _strAuftragsstatus
        End Get
        Set(ByVal value As String)
            _strAuftragsstatus = value
        End Set
    End Property

    Public Property Auftragsnummer() As String
        Get
            Return _strAuftragsnummer
        End Get
        Set(ByVal value As String)
            _strAuftragsnummer = value
        End Set
    End Property

    Public Property Sachbearbeiter() As String
        Get
            Return _mSachbearbeiter
        End Get
        Set(ByVal value As String)
            _mSachbearbeiter = value
        End Set
    End Property

    Public Property TblUpload() As DataTable
        Get
            Return _mTblUpload
        End Get
        Set(ByVal value As DataTable)
            _mTblUpload = value
        End Set
    End Property

    Public Property VersandOhneAbmeldung() As String
        Get
            Return _mVersandOhneAbmeldung
        End Get
        Set(ByVal value As String)
            _mVersandOhneAbmeldung = value
        End Set
    End Property

    Public Property EQuiTyp() As String
        Get
            Return _mEQuiTyp
        End Get
        Set(ByVal value As String)
            _mEQuiTyp = value
        End Set
    End Property

    Public Property Halter() As String
        Get
            Return _mHalter
        End Get
        Set(ByVal value As String)
            _mHalter = value
        End Set
    End Property

    Public Property Bemerkung() As String
        Get
            Return _mBemerkung
        End Get
        Set(ByVal value As String)
            _mBemerkung = value
        End Set
    End Property

    Public Property EqTyp() As String
        Get
            Return _mEqTyp
        End Get
        Set(ByVal value As String)
            _mEqTyp = value
        End Set
    End Property

    Public Property Adressart() As Integer
        Get
            Return _mAdressart
        End Get
        Set(ByVal value As Integer)
            _mAdressart = value
        End Set
    End Property

    Public Property AdressartText() As String
        Get
            Return _mAdressartText
        End Get
        Set(ByVal value As String)
            _mAdressartText = value
        End Set
    End Property

    Public Property VersandoptionenText() As String
        Get
            Return _mVersandoptionenText
        End Get
        Set(ByVal value As String)
            _mVersandoptionenText = value
        End Set
    End Property

    Public Property AutorisierungText() As String
        Get
            Return _mAutorisierungText
        End Get
        Set(ByVal value As String)
            _mAutorisierungText = value
        End Set
    End Property

    Public Property Beauftragungsdatum() As String
        Get
            Return _mBeauftragungsdatum
        End Get
        Set(ByVal value As String)
            _mBeauftragungsdatum = value
        End Set
    End Property

    Public Property SReferenz As String
    Public Property SName1 As String
    Public Property SName2 As String
    Public Property SStrasse As String
    Public Property SPlz As String
    Public Property SOrt As String
    Public Property BrieflieferantNr As String
    Public Property Restlaufzeit As String
    Public Property AbmeldedatumVon As String
    Public Property AbmeldedatumBis As String
    Public Property AbmeldeauftragVon As String
    Public Property AbmeldeauftragBis As String
    Public Property Abgemeldet As String
    Public Property ZulassungsdatumVon As String
    Public Property ZulassungsdatumBis As String
    Public Property AufAbmeldungWarten As Boolean

#End Region

#Region "Methods"

    Public Overrides Sub Change()


    End Sub

    Public Overrides Sub Show()


    End Sub

    Public Overloads Sub Fill(ByVal strAppId As String, _
                            ByVal strSessionId As String, _
                            ByVal page As Page, _
                            Optional ByVal upload As Boolean = False)
        m_strClassAndMethod = "Briefversand.FILL"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_UNANGEF_ALLG_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKunnr)
                myProxy.setImportParameter("I_EQTYP", _mEQuiTyp)

                'Erweiterte Selektion
                myProxy.setImportParameter("I_KUNNR_ZP", BrieflieferantNr)
                myProxy.setImportParameter("I_RESTLAUFZEIT", Restlaufzeit)
                myProxy.setImportParameter("I_ABMDAT_VON", AbmeldedatumVon)
                myProxy.setImportParameter("I_ABMDAT_BIS", AbmeldedatumBis)
                myProxy.setImportParameter("I_ABMAUF_VON", AbmeldeauftragVon)
                myProxy.setImportParameter("I_ABMAUF_BIS", AbmeldeauftragBis)
                myProxy.setImportParameter("I_ABMELD", Abgemeldet)
                myProxy.setImportParameter("I_ZULASS_DAT_VON", ZulassungsdatumVon)
                myProxy.setImportParameter("I_ZULASS_DAT_BIS", ZulassungsdatumBis)


                Dim sapTable As DataTable = myProxy.getImportTable("GT_IN")

                Dim sapRow As DataRow


                If Not upload Then

                    If Len(_mStrFahrgestellnr & _mStrKennzeichen & _mStrLVnr & _mStrZbiiNr & _mStrRef1 & _mStrRef2) > 0 Then
                        sapRow = sapTable.NewRow
                        sapRow("CHASSIS_NUM") = _mStrFahrgestellnr
                        sapRow("LICENSE_NUM") = _mStrKennzeichen
                        sapRow("LIZNR") = _mStrLVnr
                        sapRow("TIDNR") = _mStrZbiiNr
                        sapRow("ZZREFERENZ1") = _mStrRef1
                        sapRow("ZZREFERENZ2") = _mStrRef2
                        sapTable.Rows.Add(sapRow)
                    End If

                Else
                    Dim uploadRow As DataRow
                    For Each uploadRow In _mTblUpload.Rows
                        sapRow = sapTable.NewRow
                        sapRow("CHASSIS_NUM") = uploadRow("CHASSIS_NUM").ToString
                        sapRow("LICENSE_NUM") = uploadRow("LICENSE_NUM").ToString
                        sapRow("LIZNR") = uploadRow("LIZNR").ToString
                        sapRow("TIDNR") = uploadRow("TIDNR").ToString
                        sapRow("ZZREFERENZ1") = uploadRow("ZZREFERENZ1").ToString
                        sapRow("ZZREFERENZ2") = uploadRow("ZZREFERENZ2").ToString
                        sapTable.Rows.Add(sapRow)
                    Next

                End If

                'Auswahl für ZZREFERENZ1 ggf. übersteuern, wenn im User entsprechend konfiguriert
                Dim strRef1 As String = m_objUser.GetUserReferenceValueByReferenceType(Referenzfeldtyp.ZZREFERENZ1)
                If Not String.IsNullOrEmpty(strRef1) Then
                    For Each dRow As DataRow In sapTable.Rows
                        dRow("ZZREFERENZ1") = strRef1
                    Next
                End If

                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_ABRUFBAR")

                tblTemp2.Columns.Add("ADRESSE", GetType(System.String))
                tblTemp2.Columns.Add("TREUSPERR", GetType(System.String))
                tblTemp2.Columns.Add("Selected", GetType(System.String))
                tblTemp2.Columns.Add("PAID", GetType(System.String))
                tblTemp2.Columns.Add("KONTONR", GetType(System.String))
                tblTemp2.Columns.Add("CIN", GetType(System.String))

                tblTemp2.Columns("ZZSTATUS_ZUG").MaxLength = 15

                tblTemp2.AcceptChanges()

                For Each rowTemp As DataRow In tblTemp2.Rows

                    Dim sAdresse As String
                    sAdresse = rowTemp("NAME1_ZS").ToString
                    If rowTemp("NAME2_ZS").ToString.Length > 0 Then
                        sAdresse += " " + rowTemp("NAME2_ZS").ToString
                    End If
                    sAdresse += "<br/>" + rowTemp("STREET_ZS").ToString + " " + rowTemp("HOUSE_NUM1_ZS").ToString
                    sAdresse += "<br/>" + rowTemp("POST_CODE1_ZS").ToString + " " + rowTemp("CITY1_ZS").ToString
                    rowTemp("ADRESSE") = sAdresse
                    rowTemp("TREUSPERR") = ""
                    If rowTemp("TREUH_YT").ToString.Length + rowTemp("TREUH_YA").ToString.Length + rowTemp("TREUH_YZ").ToString.Length + rowTemp("TREUH_YE").ToString.Length > 0 Then
                        rowTemp("TREUSPERR") = "X"
                    End If
                    rowTemp("Selected") = ""

                    'ZZSTATUS_ZUG wird in der Spaltenübersetzung mit Status übersetzt
                    'hier müssen aber noch die Status ZZSTATUS_IABG und ZZSTATUS_ABG mit eingtragen werden.
                    'Ein Status kann nur gesetzt werden.
                    If rowTemp("ZZSTATUS_ABG").ToString.ToUpper.Contains("X") Then
                        rowTemp("ZZSTATUS_ZUG") = "abgemeldet"
                    Else
                        If rowTemp("ZZSTATUS_IABG").ToString.ToUpper.Contains("X") Then
                            rowTemp("ZZSTATUS_ZUG") = "in Abmeldung"
                        Else
                            If rowTemp("ZZSTATUS_ZUG").ToString.ToUpper.Contains("X") Then
                                rowTemp("ZZSTATUS_ZUG") = "zugelassen"
                            End If
                        End If


                    End If

                    If strKunnr = "0010026883" Then
                        GetVertragsdaten(page, rowTemp)
                    End If

                    tblTemp2.AcceptChanges()

                Next

                _mTblFahrzeuge = CreateOutPut(tblTemp2, strAppId)


                Dim tblTemp As DataTable = myProxy.getExportTable("GT_FEHLER")

                tblTemp.Columns.Add("ADRESSE", GetType(System.String))
                tblTemp.Columns.Add("TREUSPERR", GetType(System.String))
                tblTemp.Columns.Add("PAID", GetType(System.String))
                tblTemp.Columns.Add("KONTONR", GetType(System.String))
                tblTemp.Columns.Add("CIN", GetType(System.String))

                For Each rowTemp As DataRow In tblTemp.Rows

                    Dim sAdresse As String
                    sAdresse = rowTemp("NAME1_ZS").ToString
                    If rowTemp("NAME2_ZS").ToString.Length > 0 Then
                        sAdresse += " " + rowTemp("NAME2_ZS").ToString
                    End If
                    sAdresse += "<br/>" + rowTemp("STREET_ZS").ToString + " " + rowTemp("HOUSE_NUM1_ZS").ToString
                    sAdresse += "<br/>" + rowTemp("POST_CODE1_ZS").ToString + " " + rowTemp("CITY1_ZS").ToString
                    rowTemp("ADRESSE") = sAdresse
                    rowTemp("TREUSPERR") = ""
                    If rowTemp("TREUH_YT").ToString.Length + rowTemp("TREUH_YA").ToString.Length + rowTemp("TREUH_YZ").ToString.Length + rowTemp("TREUH_YE").ToString.Length > 0 Then
                        rowTemp("TREUSPERR") = "X"
                    End If

                    If strKunnr = "0010026883" Then
                        GetVertragsdaten(page, rowTemp)
                    End If

                    tblTemp.AcceptChanges()
                Next

                _mTblFahrzeugeFehler = CreateOutPut(tblTemp, strAppId)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub GetAdressenandZulStellen(ByVal strAppId As String, ByVal strSessionId As String, ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetAdressenandZulStellen"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_ADRESSPOOL_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKunnr)
                myProxy.setImportParameter("I_EQTYP", EqTyp)

                myProxy.callBapi()

                Dim sapTableZul As DataTable = myProxy.getExportTable("GT_ZULAST")
                Dim sapTableAdressen As DataTable = myProxy.getExportTable("GT_ADRS")

                Dim sapRow As DataRow

                _mTblZulStellen = sapTableZul.Clone
                _mTblZulStellen.Columns.Add("DISPLAY", GetType(System.String))

                sapTableZul.DefaultView.RowFilter = "LIFNR <> ''"

                sapTableZul = sapTableZul.DefaultView.ToTable

                For Each row In sapTableZul.Rows
                    sapRow = _mTblZulStellen.NewRow

                    sapRow("DISPLAY") = row("PSTLZ").ToString & " - " & row("ORT01").ToString & " - " & row("STRAS").ToString
                    sapRow("LIFNR") = row("LIFNR").ToString
                    sapRow("ORT01") = row("ORT01").ToString
                    sapRow("PSTLZ") = row("PSTLZ").ToString
                    sapRow("STRAS") = row("STRAS").ToString
                    sapRow("ZKFZKZ") = row("ZKFZKZ").ToString
                    sapRow("NAME1") = row("NAME1").ToString
                    sapRow("NAME2") = row("NAME2").ToString
                    _mTblZulStellen.Rows.Add(sapRow)
                Next

                _mTblAdressen = sapTableAdressen.Clone
                _mTblAdressen.Columns.Add("DISPLAY", GetType(System.String))

                For Each row In sapTableAdressen.Rows
                    sapRow = _mTblAdressen.NewRow
                    sapRow("IDENT") = row("IDENT").ToString
                    sapRow("DISPLAY") = row("NAME1").ToString & " " & row("NAME2").ToString & " - " & row("STREET").ToString & ", " & row("CITY1").ToString
                    sapRow("KUNNR") = row("KUNNR").ToString
                    sapRow("NAME1") = row("NAME1").ToString
                    sapRow("NAME2") = row("NAME2").ToString
                    sapRow("STREET") = row("STREET").ToString
                    sapRow("HOUSE_NUM1") = row("HOUSE_NUM1").ToString
                    sapRow("POST_CODE1") = row("POST_CODE1").ToString
                    sapRow("CITY1") = row("CITY1").ToString
                    sapRow("COUNTRY") = row("COUNTRY").ToString
                    sapRow("NAME1") = row("NAME1").ToString
                    sapRow("NAME2") = row("NAME2").ToString
                    _mTblAdressen.Rows.Add(sapRow)
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, _mTblAdressen)
                GetLaender(page)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub GetAdressen(ByVal strAppId As String, ByVal strSessionId As String, ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetAdressen"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_ADRESSPOOL_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKunnr)
                myProxy.setImportParameter("I_EQTYP", EqTyp)
                myProxy.setImportParameter("I_POS_TEXT", SReferenz)
                myProxy.setImportParameter("I_NAME1", SName1)
                myProxy.setImportParameter("I_NAME2", SName2)
                myProxy.setImportParameter("I_STRAS", SStrasse)
                myProxy.setImportParameter("I_PSTLZ", SPlz)
                myProxy.setImportParameter("I_ORT01", SOrt)

                myProxy.callBapi()

                Dim sapTableAdressen As DataTable = myProxy.getExportTable("GT_ADRS")

                Dim sapRow As DataRow

                _mTblAdressen = sapTableAdressen.Clone
                _mTblAdressen.Columns.Add("DISPLAY", GetType(System.String))

                For Each row In sapTableAdressen.Rows
                    sapRow = _mTblAdressen.NewRow
                    sapRow("IDENT") = row("IDENT").ToString
                    sapRow("DISPLAY") = row("NAME1").ToString & " " & row("NAME2").ToString & " - " & row("STREET").ToString & ", " & row("CITY1").ToString
                    sapRow("KUNNR") = row("KUNNR").ToString
                    sapRow("NAME1") = row("NAME1").ToString
                    sapRow("NAME2") = row("NAME2").ToString
                    sapRow("STREET") = row("STREET").ToString
                    sapRow("HOUSE_NUM1") = row("HOUSE_NUM1").ToString
                    sapRow("POST_CODE1") = row("POST_CODE1").ToString
                    sapRow("CITY1") = row("CITY1").ToString
                    sapRow("COUNTRY") = row("COUNTRY").ToString
                    sapRow("SAPNR") = row("SAPNR").ToString
                    sapRow("NAME1") = row("NAME1").ToString
                    sapRow("NAME2") = row("NAME2").ToString
                    _mTblAdressen.Rows.Add(sapRow)
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, _mTblAdressen)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub GetAbrufgrund(ByVal strAppId As String, _
                   ByVal strSessionId As String, _
                   ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetAbrufgrund"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_VERS_GRUND_KUN_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKunnr)
                myProxy.setImportParameter("I_ABCKZ", _mStrVersandArt)
                myProxy.setImportParameter("I_GRUPPE", "")


                myProxy.callBapi()


                _mTblVersandgruende = myProxy.getExportTable("GT_OUT")


                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, _mTblVersandgruende)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub GetVersandOptions(ByVal strAppId As String, _
                       ByVal strSessionId As String, _
                       ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetVersandOptions"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_LV_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_VWAG", "X")

                Dim sapTable As DataTable = myProxy.getImportTable("GT_IN_AG")

                Dim sapRow As DataRow

                sapRow = sapTable.NewRow

                sapRow("AG") = strKunnr

                sapTable.Rows.Add(sapRow)

                Dim sapProzess As DataTable = myProxy.getImportTable("GT_IN_PROZESS")

                Dim rowProz As DataRow

                rowProz = sapProzess.NewRow

                rowProz("SORT1") = _mOptionFlag

                sapProzess.Rows.Add(rowProz)

                myProxy.callBapi()

                _mTblVersandOptions = myProxy.getExportTable("GT_OUT_DL")


                _mTblVersandOptions.Columns.Add("Selected", GetType(System.String))
                _mTblVersandOptions.Columns.Add("Description", GetType(System.String))
                _mTblVersandOptions.Columns("Description").DefaultValue = ""

                _mTblVersandOptions.AcceptChanges()

                For Each dr As DataRow In _mTblVersandOptions.Rows
                    dr("Description") = ""
                Next


                For Each versandRow As DataRow In _mTblVersandOptions.Rows
                    If versandRow("VW_AG") = "X" Then
                        versandRow("Selected") = "1"
                    End If
                Next


                Dim langTextTable As DataTable = myProxy.getExportTable("GT_OUT_ESLL_LTXT")

                If langTextTable.Rows.Count > 0 Then

                    Dim strText As String

                    For Each dr As DataRow In _mTblVersandOptions.Rows

                        strText = ""

                        langTextTable.DefaultView.RowFilter = "SRVPOS = '" & dr("ASNUM").ToString.PadLeft(18, "0"c) & "'"

                        If langTextTable.DefaultView.Count > 0 Then

                            For i = 0 To langTextTable.DefaultView.Count - 1

                                strText &= langTextTable.DefaultView.Item(i)("TDLINE").ToString & " "

                            Next

                        End If

                        dr("Description") = strText

                    Next

                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, _mTblVersandOptions)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Sub GetLaender(ByVal page As Page)
        '----------------------------------------------------------------------
        ' Methode: getLaender
        ' Autor: JJU
        ' Beschreibung: gibt alle Länder zurück
        ' Erstellt am: 06.03.2009
        ' ITA: 2918
        '----------------------------------------------------------------------


        m_intStatus = 0
        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Land_Plz_001", m_objApp, m_objUser, page)

            myProxy.callBapi()

            _mTblLaender = myProxy.getExportTable("GT_WEB")


            _mTblLaender.Columns.Add("Beschreibung", Type.GetType("System.String"))
            _mTblLaender.Columns.Add("FullDesc", Type.GetType("System.String"))
            Dim rowTemp As DataRow
            For Each rowTemp In _mTblLaender.Rows
                If CInt(rowTemp("LNPLZ")) > 0 Then
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx")) & " (" & CStr(CInt(rowTemp("LNPLZ"))) & ")"
                Else
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx"))
                End If
                rowTemp("FullDesc") = CStr(rowTemp("Land1")) & " " & CStr(rowTemp("Beschreibung"))

            Next

        Catch ex As Exception
            Select Case ex.Message
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        End Try

    End Sub

    Sub GetStueckliste(ByVal strAppId As String, ByVal strSessionId As String, ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetStueckliste"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim strKunnr = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim myProxy = DynSapProxy.getProxy("Z_DPM_READ_EQUI_STL_01", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_AG", strKunnr)
                myProxy.setImportParameter("I_EQTYP", "T")
                myProxy.setImportParameter("I_STATUS", "L;T;V") ' lagernd, temporär versandt, Versand angefordert

                Dim gtIn = myProxy.getImportTable("GT_IN")
                Fahrzeuge.Select("Selected = '1'").ToList().ForEach(Sub(r)
                                                                        Dim newR = gtIn.NewRow
                                                                        newR("CHASSIS_NUM") = r("Fahrgestellnummer")
                                                                        newR("EQUNR") = r("EQUNR")
                                                                        gtIn.Rows.Add(newR)
                                                                    End Sub)
                gtIn.AcceptChanges()

                myProxy.callBapi()

                Dim gtOut = myProxy.getExportTable("GT_OUT")

                Dim col = New DataColumn("Selected", GetType(String))
                col.DefaultValue = String.Empty
                gtOut.Columns.Add(col)
                gtOut.AcceptChanges()
                _mTblStueckliste = gtOut

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, _mTblStueckliste)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Sub GetBrieflieferanten(ByVal page As Page)

        m_intStatus = 0
        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_BRIEFLIEFERANTEN_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, "0"c))

            myProxy.callBapi()

            _mBrieflieferanten = myProxy.getExportTable("GT_OUT")


            _mBrieflieferanten.Columns.Add("Adresse", Type.GetType("System.String"))

            Dim rowTemp As DataRow
            For Each rowTemp In _mBrieflieferanten.Rows
                rowTemp("Adresse") = rowTemp("NAME1")

                If String.IsNullOrEmpty(rowTemp("NAME2")) = False Then
                    rowTemp("Adresse") &= " " & rowTemp("NAME2")
                End If

                rowTemp("Adresse") &= ", " & rowTemp("CITY1")

            Next

            Dim newRow As DataRow = _mBrieflieferanten.NewRow

            newRow("KUNNR") = "0"
            newRow("Adresse") = "--- Auswahl ---"

            _mBrieflieferanten.Rows.Add(newRow)

            _mBrieflieferanten.DefaultView.Sort = "Adresse"

            _mBrieflieferanten = _mBrieflieferanten.DefaultView.ToTable


        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        End Try

    End Sub

    Public Sub Anfordern(ByVal strAppId As String, _
                      ByVal strSessionId As String, _
                      ByVal page As Page)
        m_strClassAndMethod = "Briefversand.Anfordern"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim myProxy = DynSapProxy.getProxy("Z_DPM_FILL_VERSAUFTR", m_objApp, m_objUser, page)
                myProxy.setImportParameter("KUNNR_AG", strKunnr)

                Dim sapTable = myProxy.getImportTable("GT_IN")

                Dim selectedRows = Fahrzeuge.Select("Selected = '1'")
                If selectedRows.Length > 0 Then
                    For Each Fahrzeugrow In selectedRows

                        If Not _mTblStueckliste Is Nothing Then

                            Dim selectedParts = Stueckliste.Select(String.Format("Selected='1' and EQUNR='{0}'", Fahrzeugrow("EQUNR")))
                            If selectedParts.Length > 0 Then
                                For Each partRow As DataRow In selectedParts
                                    Dim sapRow As DataRow = sapTable.NewRow
                                    FillSapRow(sapRow, Fahrzeugrow, strKunnr, partRow)
                                    sapTable.Rows.Add(sapRow)
                                Next
                            Else
                                Dim sapRow As DataRow = sapTable.NewRow
                                FillSapRow(sapRow, Fahrzeugrow, strKunnr)
                                sapTable.Rows.Add(sapRow)
                            End If
                        Else
                            Dim sapRow As DataRow = sapTable.NewRow
                            FillSapRow(sapRow, Fahrzeugrow, strKunnr)
                            sapTable.Rows.Add(sapRow)

                        End If


                    Next

                    myProxy.callBapi()

                    _mTblVersandError = myProxy.getExportTable("GT_ERR")


                    _mESubrc = myProxy.getExportParameter("E_SUBRC")
                    _mEMessage = myProxy.getExportParameter("E_MESSAGE")

                    If IsNumeric(_mESubrc) Then
                        m_intStatus = CInt(_mESubrc)
                        If _mEMessage.Length > 0 Then m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    End If
                    If _mTblVersandError.Rows.Count > 0 Then
                        m_strMessage = "Eine oder mehrere Anforderungen konnten im System nicht erstellt werden."
                    End If

                    For Each tmpRow As DataRow In _mTblFahrzeuge.Select("Selected = '1'")
                        selectedRows = _mTblVersandError.Select("CHASSIS_NUM = '" & tmpRow("Fahrgestellnummer").ToString & "'")
                        If selectedRows.Length > 0 Then
                            'tmpRow("Bemerkung") = "Fehler: " & String.Join(", ", selectedRows.Select(Function(r) r("Bemerkung")).ToArray)
                            Dim objArray As Object() = selectedRows.Select(Function(r) r("Bemerkung")).ToArray
                            Dim strArray(objArray.GetUpperBound(0)) As String
                            For i = 0 To objArray.GetUpperBound(0)
                                strArray(i) = CStr(objArray(i))
                            Next
                            tmpRow("Bemerkung") = String.Join(", ", strArray)   '"Fehler: " & 
                            WriteLogEntry(False, "KUNNR=" + m_objUser.KUNNR + "; FIN = " + tmpRow("Fahrgestellnummer").ToString, _mTblFahrzeuge)
                        Else
                            tmpRow("Bemerkung") = "erfolgreich"
                            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR + "; FIN = " + tmpRow("Fahrgestellnummer").ToString, _mTblFahrzeuge)
                        End If
                    Next
                Else : Throw New Exception("Keine Fahrzeuge ausgewählt!")
                End If
            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Sub FillSapRow(ByVal row As DataRow, ByVal fzgRow As DataRow, ByVal kundennr As String, Optional ByVal partRow As DataRow = Nothing)
        row("ZZKUNNR_AG") = kundennr
        row("LICENSE_NUM") = fzgRow("Kennzeichen").ToString
        row("CHASSIS_NUM") = fzgRow("Fahrgestellnummer").ToString
        row("ZZBRFVERS") = _mBriefversand
        row("ZZSCHLVERS") = _mSchluesselVersand
        row("ZZABMELD") = _mVersandOhneAbmeldung
        row("ABCKZ") = _mStrVersandArt
        row("MATNR") = _mMaterial
        row("ZZVGRUND") = _mVersgrund
        row("ZZANFDT") = Now.ToShortDateString
        row("ZZBETREFF") = _mBemerkung
        row("ZZNAME_ZH") = _mHalter

        'PartnerNr
        row("ZZKUNNR_ZS") = _mVersandadrZs

        'Zulassungsstelle
        row("ZZADRNR_ZS") = _mVersandadrZe

        If VersandArt = "1" Then
            If _mVersandadrZe.Length > 0 Then
                row("ZZ_MAHNA") = "0001"
            Else
                row("ZZ_MAHNA") = "0002"
            End If
        End If

        row("ZZKONZS_ZS") = ""
        ' Freie Adresse
        row("ZZNAME1_ZS") = _mStrName1
        row("ZZNAME2_ZS") = _mStrName2
        row("ZZSTRAS_ZS") = _mStrStreet
        row("ZZHAUSNR_ZS") = _mStrHouseNum
        row("ZZPSTLZ_ZS") = _mStrPostcode
        row("ZZORT01_ZS") = _mStrCity
        row("COUNTRY_ZS") = _mStrLaenderKuerzel
        row("ZZLAND_ZS") = _mStrLaenderKuerzel
        row("ERNAM") = Left(m_objUser.UserName, 12)
        row("LIZNR") = fzgRow("Leasingnummer").ToString

        If Not partRow Is Nothing Then
            row("IDNRK") = partRow("IDNRK")
        End If
    End Sub

    Public Sub AnfordernAusAutorisierung(ByVal strAppId As String, _
                  ByVal strSessionId As String, _
                  ByVal page As Page)

        m_strClassAndMethod = "Briefversand.Anfordern"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_FILL_VERSAUFTR", m_objApp, m_objUser, page)
                myProxy.setImportParameter("KUNNR_AG", strKunnr)
                Dim sapTable As DataTable = myProxy.getImportTable("GT_IN")

                Dim selectedRow() As DataRow
                Dim fahrzeugrow As DataRow

                selectedRow = _mTblFahrzeuge.Select("Selected = '1' AND EQUNR='" & _mStrReferenceforAut & "'")
                If selectedRow.Length > 0 Then
                    For Each fahrzeugrow In selectedRow

                        Dim sapRow As DataRow = sapTable.NewRow

                        sapRow("ZZKUNNR_AG") = strKunnr
                        sapRow("LICENSE_NUM") = fahrzeugrow("Kennzeichen").ToString
                        sapRow("CHASSIS_NUM") = fahrzeugrow("Fahrgestellnummer").ToString
                        sapRow("ZZBRFVERS") = _mBriefversand
                        sapRow("ZZSCHLVERS") = _mSchluesselVersand
                        sapRow("ZZABMELD") = _mVersandOhneAbmeldung
                        sapRow("ABCKZ") = _mStrVersandArt
                        sapRow("MATNR") = _mMaterial
                        sapRow("ZZVGRUND") = _mVersgrund
                        sapRow("ZZANFDT") = Now.ToShortDateString
                        sapRow("ZZBETREFF") = _mBemerkung
                        sapRow("ZZNAME_ZH") = _mHalter

                        'PartnerNr
                        sapRow("ZZKUNNR_ZS") = _mVersandadrZs

                        'Zulassungsstelle
                        sapRow("ZZADRNR_ZS") = _mVersandadrZe

                        If VersandArt = "1" Then
                            If _mVersandadrZe.Length > 0 Then
                                sapRow("ZZ_MAHNA") = "0001"
                            Else
                                sapRow("ZZ_MAHNA") = "0002"
                            End If
                        End If

                        sapRow("ZZKONZS_ZS") = ""
                        ' Freie Adresse
                        sapRow("ZZNAME1_ZS") = _mStrName1
                        sapRow("ZZNAME2_ZS") = _mStrName2
                        sapRow("ZZSTRAS_ZS") = _mStrStreet
                        sapRow("ZZHAUSNR_ZS") = _mStrHouseNum
                        sapRow("ZZPSTLZ_ZS") = _mStrPostcode
                        sapRow("ZZORT01_ZS") = _mStrCity
                        sapRow("COUNTRY_ZS") = _mStrLaenderKuerzel

                        sapRow("ERNAM") = Left(m_objUser.UserName, 12)
                        sapRow("LIZNR") = fahrzeugrow("Leasingnummer").ToString

                        sapTable.Rows.Add(sapRow)

                    Next

                    myProxy.callBapi()

                    _mTblVersandError = myProxy.getExportTable("GT_ERR")


                    _mESubrc = myProxy.getExportParameter("E_SUBRC")
                    _mEMessage = myProxy.getExportParameter("E_MESSAGE")
                    If IsNumeric(_mESubrc) Then
                        m_intStatus = CInt(_mESubrc)
                        If _mEMessage.Length > 0 Then m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    End If
                    If _mTblVersandError.Rows.Count > 0 Then
                        m_strMessage = "Eine oder mehrere Anforderungen konnten im System nicht erstellt werden."
                    End If



                    For Each tmpRow As DataRow In _mTblFahrzeuge.Select("Selected = '1' AND EQUNR='" & _mStrReferenceforAut & "'")
                        selectedRow = _mTblVersandError.Select("CHASSIS_NUM = '" & tmpRow("Fahrgestellnummer").ToString & "'")
                        If selectedRow.Length > 0 Then
                            m_intStatus = -2100
                            m_strMessage = "Fehler: " & selectedRow(0)("Bemerkung").ToString
                            WriteLogEntry(False, "KUNNR=" + m_objUser.KUNNR + "; FIN = " + tmpRow("Fahrgestellnummer").ToString, _mTblFahrzeuge)
                        Else
                            m_strMessage = "Ihre Anforderung wurde erfolgreich in unserem System übernommen."
                            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR + "; FIN = " + tmpRow("Fahrgestellnummer").ToString, _mTblFahrzeuge)
                        End If
                    Next



                Else : Throw New Exception("Keine Fahrzeuge ausgewählt!")
                End If
            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Anfordern2(ByVal strAppId As String, ByVal strSessionId As String, ByVal page As Page)
        m_strClassAndMethod = "Briefversand.Anfordern2"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim myProxy = DynSapProxy.getProxy("Z_DPM_IMP_VERS_BEAUFTR_01", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_KUNNR_AG", strKunnr)

                Dim sapTable = myProxy.getImportTable("GT_IN")

                Dim selectedRows = Fahrzeuge.Select("Selected = '1'")
                If selectedRows.Length > 0 Then
                    For Each Fahrzeugrow In selectedRows

                        Dim sapRow As DataRow = sapTable.NewRow
                        FillSapRow2(sapRow, Fahrzeugrow)
                        sapTable.Rows.Add(sapRow)

                    Next

                    myProxy.callBapi()

                    _mTblVersandError = myProxy.getExportTable("GT_OUT")

                    If _mTblVersandError.Select("ABLAUFSTATUS='FEHLERHAFT'").Length > 0 Then
                        m_strMessage = "Eine oder mehrere Anforderungen konnten im System nicht erstellt werden."
                    End If

                    For Each tmpRow As DataRow In _mTblFahrzeuge.Select("Selected = '1'")
                        selectedRows = _mTblVersandError.Select("LEISTUNGSIDCMS = '" & tmpRow("Leasingnummer").ToString & "'")
                        If selectedRows.Length > 0 AndAlso selectedRows(0)("ABLAUFSTATUS").ToString() = "FEHLERHAFT" Then
                            tmpRow("Bemerkung") = selectedRows(0)("FEHLERART").ToString()
                            WriteLogEntry(False, "KUNNR=" + m_objUser.KUNNR + "; LIZNR = " + tmpRow("Leasingnummer").ToString, _mTblFahrzeuge)
                        Else
                            tmpRow("Bemerkung") = "erfolgreich"
                            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR + "; LIZNR = " + tmpRow("Leasingnummer").ToString, _mTblFahrzeuge)
                        End If
                    Next
                Else : Throw New Exception("Keine Fahrzeuge ausgewählt!")
                End If
            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Sub FillSapRow2(ByVal row As DataRow, ByVal fzgRow As DataRow)

        row("LEISTUNGSIDCMS") = fzgRow("Leasingnummer").ToString
        row("SICHERHEITSIDCMS") = ""
        row("ZBRIEF") = fzgRow("NummerZBII").ToString
        row("ANF_ART") = _mStrVersandArt
        row("VERS_ART") = _mMaterial
        row("VERS_GRUND") = _mVersgrund
        Select Case _mAdressart
            Case 1, 4
                row("EMPF_ART") = "DRITTE"
            Case 2, 5
                row("EMPF_ART") = "ZULASSUNGSSTELLE"
            Case Else
                row("EMPF_ART") = ""
        End Select
        row("NAME_EMPF") = _mStrName1
        row("VORNAME_EMPF") = ""
        row("STRASSE_EMPF") = _mStrStreet
        row("HNR_EMPF") = _mStrHouseNum
        row("PLZ_EMPF") = _mStrPostcode
        row("ORT_EMPF") = _mStrCity
        row("LAND_EMPF") = _mStrLaenderKuerzel
        row("SYSTEMKENNZ") = ""
        row("AUFTRAGGEBERID") = m_objUser.UserName
        row("NAME_ANSP") = _mStrName2
        row("NAME_ANF") = ""
        row("VORNAME_ANF") = ""
        row("STRASSE_ANF") = ""
        row("HNR_ANF") = ""
        row("PLZ_ANF") = ""
        row("ORT_ANF") = ""
        row("LAND_ANF") = ""
        row("CLIENTLD") = ""
        row("INFO_ANF") = _mHalter & ", " & _mBemerkung

    End Sub

    Public Sub AnfordernAusAutorisierung2(ByVal strAppId As String, ByVal strSessionId As String, ByVal page As Page, ByVal autUser As String)

        m_strClassAndMethod = "Briefversand.AnfordernAusAutorisierung2"
        m_strAppID = strAppId
        m_strSessionID = strSessionId
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_IMP_VERS_BEAUFTR_01", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_KUNNR_AG", strKunnr)
                Dim sapTable As DataTable = myProxy.getImportTable("GT_IN")

                Dim selectedRow() As DataRow
                Dim fahrzeugrow As DataRow

                selectedRow = _mTblFahrzeuge.Select("Selected = '1' AND EQUNR='" & _mStrReferenceforAut & "'")
                If selectedRow.Length > 0 Then
                    For Each fahrzeugrow In selectedRow

                        Dim sapRow As DataRow = sapTable.NewRow

                        sapRow("LEISTUNGSIDCMS") = fahrzeugrow("Leasingnummer").ToString
                        sapRow("SICHERHEITSIDCMS") = ""
                        sapRow("ZBRIEF") = fahrzeugrow("NummerZBII").ToString
                        sapRow("ANF_ART") = _mStrVersandArt
                        sapRow("VERS_ART") = _mMaterial
                        sapRow("VERS_GRUND") = _mVersgrund
                        Select Case _mAdressart
                            Case 1, 4
                                sapRow("EMPF_ART") = "DRITTE"
                            Case 2, 5
                                sapRow("EMPF_ART") = "ZULASSUNGSSTELLE"
                            Case Else
                                sapRow("EMPF_ART") = ""
                        End Select
                        sapRow("NAME_EMPF") = _mStrName1
                        sapRow("VORNAME_EMPF") = ""
                        sapRow("STRASSE_EMPF") = _mStrStreet
                        sapRow("HNR_EMPF") = _mStrHouseNum
                        sapRow("PLZ_EMPF") = _mStrPostcode
                        sapRow("ORT_EMPF") = _mStrCity
                        sapRow("LAND_EMPF") = _mStrLaenderKuerzel
                        sapRow("SYSTEMKENNZ") = ""
                        sapRow("AUFTRAGGEBERID") = _mSachbearbeiter
                        sapRow("NAME_ANSP") = _mStrName2
                        sapRow("NAME_ANF") = ""
                        sapRow("VORNAME_ANF") = ""
                        sapRow("STRASSE_ANF") = ""
                        sapRow("HNR_ANF") = ""
                        sapRow("PLZ_ANF") = ""
                        sapRow("ORT_ANF") = ""
                        sapRow("LAND_ANF") = ""
                        sapRow("CLIENTLD") = ""
                        sapRow("USER_AUTOR") = autUser
                        sapRow("INFO_ANF") = _mHalter & ", " & _mBemerkung

                        sapTable.Rows.Add(sapRow)

                    Next

                    myProxy.callBapi()

                    _mTblVersandError = myProxy.getExportTable("GT_OUT")

                    If _mTblVersandError.Select("ABLAUFSTATUS='FEHLERHAFT'").Length > 0 Then
                        m_strMessage = "Eine oder mehrere Anforderungen konnten im System nicht erstellt werden."
                    End If

                    For Each tmpRow As DataRow In _mTblFahrzeuge.Select("Selected = '1' AND EQUNR='" & _mStrReferenceforAut & "'")
                        selectedRow = _mTblVersandError.Select("LEISTUNGSIDCMS = '" & tmpRow("Leasingnummer").ToString & "'")
                        If selectedRow.Length > 0 AndAlso selectedRow(0)("ABLAUFSTATUS").ToString() = "FEHLERHAFT" Then
                            m_intStatus = -2100
                            m_strMessage = "Fehler: " & selectedRow(0)("FEHLERART").ToString()
                            WriteLogEntry(False, "KUNNR=" + m_objUser.KUNNR + "; LIZNR = " + tmpRow("Leasingnummer").ToString, _mTblFahrzeuge)
                        Else
                            m_strMessage = "Ihre Anforderung wurde erfolgreich in unserem System übernommen."
                            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR + "; LIZNR = " + tmpRow("Leasingnummer").ToString, _mTblFahrzeuge)
                        End If
                    Next

                Else : Throw New Exception("Keine Fahrzeuge ausgewählt!")
                End If
            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub CreateUploadTable()
        _mTblUpload = New DataTable
        With _mTblUpload
            .Columns.Add("CHASSIS_NUM", GetType(System.String))
            .Columns.Add("LICENSE_NUM", GetType(System.String))
            .Columns.Add("LIZNR", GetType(System.String))
            .Columns.Add("TIDNR", GetType(System.String))
            .Columns.Add("ZZREFERENZ1", GetType(System.String))
            .Columns.Add("ZZREFERENZ2", GetType(System.String))
        End With
    End Sub

    Private Sub GetVertragsdaten(ByVal page As Page, ByRef row As DataRow)

        m_intStatus = 0
        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_VERTRAGSBESTAND_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
            myProxy.setImportParameter("I_PAID", row("LIZNR"))

            myProxy.callBapi()

            Dim tblTemp As DataTable = myProxy.getExportTable("GT_OUT")

            If tblTemp.Rows.Count > 0 Then
                row("PAID") = tblTemp.Rows(0)("PAID")
                row("KONTONR") = tblTemp.Rows(0)("KONTONR")
                row("CIN") = tblTemp.Rows(0)("CIN")
            End If

        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        End Try

    End Sub

    Public Function ShowStilllegungsdatumPopup(ByVal strAppId As String) As Boolean
        Dim strWert As String = Base.Kernel.Common.Common.GetApplicationConfigValue("PopupStilllegungsdatumAnzeigen", strAppId, m_objUser.Customer.CustomerId, m_objUser.GroupID)
        Dim blnShowPopup = (Not String.IsNullOrEmpty(strWert) AndAlso strWert.ToUpper() = "TRUE")

        Return (Fahrzeuge.Select("Selected = '1' AND Abmeldedatum IS NULL").Any() AndAlso blnShowPopup AndAlso Not AufAbmeldungWarten)
    End Function

#End Region

End Class
