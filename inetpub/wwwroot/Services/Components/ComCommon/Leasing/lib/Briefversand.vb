Imports CKG.Base.Business
Imports CKG.Base.Common
Imports System.Data.SqlClient

<Serializable()> Public Class Briefversand
    Inherits Base.Business.BankBase

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFilename)
    End Sub

#Region " Declarations"

    Private mE_SUBRC As String
    Private mE_MESSAGE As String
    Private m_strFahrgestellnr As String
    Private m_strKennzeichen As String
    Private m_strZBIINr As String
    Private m_strLVnr As String
    Private m_strRef1 As String
    Private m_strRef2 As String
    Private m_strVersandArt As String
    Private m_tblFahrzeuge As DataTable
    Private m_tblFahrzeugePrint As DataTable
    Private m_tblFahrzeugeFehler As DataTable
    Private m_tblZulStellen As DataTable
    Private m_tblAdressen As DataTable
    Private m_tblVersandgruende As DataTable
    Private m_tblVersandOptions As DataTable
    Private m_kbanr As String 'Zulassungsdienst-Nummer
    Private m_boolNewAdress As Boolean = False
    Private m_strAnrede As String
    Private m_strName1 As String
    Private m_strName2 As String
    Private m_strCity As String
    Private m_strPostcode As String
    Private m_strStreet As String
    Private m_strHouseNum As String
    Private m_tblLaender As DataTable
    Private m_strLaenderKuerzel As String
    Private m_material As String
    Private m_versandadr_ZE As String
    Private m_versandadr_ZS As String
    Private m_versandadrtext As String
    Private m_versgrund As String
    Private m_versgrundText As String
    Private m_versartText As String
    Private strAuftragsstatus As String
    Private strAuftragsnummer As String
    Private m_AutLevel As String
    Private m_tblUpload As DataTable
    Private m_strReferenceforAut As String
    Private m_Sachbearbeiter As String
    Private m_VersohneAbeld As String
    Private m_tblVersandError As DataTable
    Private m_EQuiTyp As String
    Private m_Briefversand As String
    Private m_SchluesselVersand As String
    Private m_OptionFlag As String
    Private mHalter As String
    Private mBemerkung As String
    Private mEqTyp As String
    Private mAdressart As Integer
    Private mAdressartText As String
    Private mVersandoptionenText As String
    Private mAutorisierungText As String
    Private mBeauftragungsdatum As String
    Private m_tblStueckliste As DataTable
    Private m_Brieflieferanten As DataTable

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
            Return m_OptionFlag
        End Get
        Set(ByVal value As String)
            m_OptionFlag = value
        End Set
    End Property

    Public Property Briefversand() As String
        Get
            Return m_Briefversand
        End Get
        Set(ByVal value As String)
            m_Briefversand = value
        End Set
    End Property

    Public Property SchluesselVersand() As String
        Get
            Return m_SchluesselVersand
        End Get
        Set(ByVal value As String)
            m_SchluesselVersand = value
        End Set
    End Property

    Public Property VersartText() As String
        Get
            Return m_versartText
        End Get
        Set(ByVal value As String)
            m_versartText = value
        End Set
    End Property

    Public Property VersgrundText() As String
        Get
            Return m_versgrundText
        End Get
        Set(ByVal value As String)
            m_versgrundText = value
        End Set
    End Property

    Public Property ReferenceforAut() As String
        Get
            Return m_strReferenceforAut
        End Get
        Set(ByVal value As String)
            m_strReferenceforAut = value
        End Set
    End Property

    Public Property AutLevel() As String
        Get
            Return m_AutLevel
        End Get
        Set(ByVal Value As String)
            m_AutLevel = Value
        End Set
    End Property

    Public Property kbanr() As String
        Get
            Return m_kbanr
        End Get
        Set(ByVal Value As String)
            m_kbanr = Value
        End Set
    End Property

    Public Property neueAdresse() As Boolean
        Get
            Return m_boolNewAdress
        End Get
        Set(ByVal Value As Boolean)
            m_boolNewAdress = Value
        End Set

    End Property

    Public Property VersandArt() As String
        Get
            Return m_strVersandArt
        End Get
        Set(ByVal Value As String)
            m_strVersandArt = Value
        End Set
    End Property

    Public Property Versandgruende() As DataTable
        Get
            Return m_tblVersandgruende
        End Get
        Set(ByVal value As DataTable)
            m_tblVersandgruende = value
        End Set
    End Property

    Public Property VersandOptionen() As DataTable
        Get
            Return m_tblVersandOptions
        End Get
        Set(ByVal value As DataTable)
            m_tblVersandOptions = value
        End Set
    End Property

    Public Property Stueckliste As DataTable
        Get
            Return m_tblStueckliste
        End Get
        Set(value As DataTable)
            m_tblStueckliste = value
        End Set
    End Property

    Public Property Adressen() As DataTable
        Get
            Return m_tblAdressen
        End Get
        Set(ByVal value As DataTable)
            m_tblAdressen = value
        End Set
    End Property

    Public Property ZulStellen() As DataTable
        Get
            Return m_tblZulStellen
        End Get
        Set(ByVal value As DataTable)
            m_tblZulStellen = value
        End Set
    End Property

    Public ReadOnly Property Laender() As DataTable
        Get
            If m_tblLaender Is Nothing Then
                getLaender(New Page)
            End If
            Return m_tblLaender
        End Get
    End Property

    Public ReadOnly Property Brieflieferanten() As DataTable
        Get
            If m_Brieflieferanten Is Nothing Then
                getBrieflieferanten(New Page)
            End If
            Return m_Brieflieferanten
        End Get
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal value As DataTable)
            m_tblFahrzeuge = value
        End Set
    End Property

    Public Property FahrzeugePrint() As DataTable
        Get
            Return m_tblFahrzeugePrint
        End Get
        Set(ByVal value As DataTable)
            m_tblFahrzeugePrint = value
        End Set
    End Property

    Public Property FahrzeugeFehler() As DataTable
        Get
            Return m_tblFahrzeugeFehler
        End Get
        Set(ByVal value As DataTable)
            m_tblFahrzeugeFehler = value
        End Set
    End Property

    Public Property E_SUBRC() As String
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As String)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
        End Set
    End Property

    Public Property Fahrgestellnr() As String
        Get
            Return m_strFahrgestellnr
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnr = Value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return m_strKennzeichen
        End Get
        Set(ByVal value As String)
            m_strKennzeichen = value
        End Set
    End Property

    Public Property ZBIINr() As String
        Get
            Return m_strZBIINr
        End Get
        Set(ByVal value As String)
            m_strZBIINr = value
        End Set
    End Property

    Public Property LVnr() As String
        Get
            Return m_strLVnr
        End Get
        Set(ByVal value As String)
            m_strLVnr = value
        End Set
    End Property

    Public Property Ref1() As String
        Get
            Return m_strRef1
        End Get
        Set(ByVal value As String)
            m_strRef1 = value
        End Set
    End Property

    Public Property Ref2() As String
        Get
            Return m_strRef2
        End Get
        Set(ByVal value As String)
            m_strRef2 = value
        End Set
    End Property

    Public Property Anrede() As String
        Get
            Return m_strAnrede
        End Get
        Set(ByVal Value As String)
            m_strAnrede = Value
        End Set

    End Property

    Public Property Name1() As String
        Get
            Return m_strName1
        End Get
        Set(ByVal Value As String)
            m_strName1 = Value
        End Set

    End Property

    Public Property Name2() As String
        Get
            Return m_strName2
        End Get
        Set(ByVal Value As String)
            m_strName2 = Value
        End Set

    End Property

    Public Property City() As String
        Get
            Return m_strCity
        End Get
        Set(ByVal Value As String)
            m_strCity = Value
        End Set
    End Property

    Public Property PostCode() As String
        Get
            Return m_strPostcode
        End Get
        Set(ByVal Value As String)
            m_strPostcode = Value
        End Set
    End Property

    Public Property Street() As String
        Get
            Return m_strStreet
        End Get
        Set(ByVal Value As String)
            m_strStreet = Value
        End Set
    End Property

    Public Property HouseNum() As String
        Get
            Return m_strHouseNum
        End Get
        Set(ByVal Value As String)
            m_strHouseNum = Value
        End Set
    End Property

    Public Property laenderKuerzel() As String
        Get
            Return m_strLaenderKuerzel
        End Get
        Set(ByVal Value As String)
            m_strLaenderKuerzel = Value
        End Set

    End Property

    Public Property Materialnummer() As String
        Get
            Return m_material
        End Get
        Set(ByVal Value As String)
            m_material = Value
        End Set
    End Property

    Public Property VersandAdresse_ZE() As String
        Get
            Return m_versandadr_ZE
        End Get
        Set(ByVal Value As String)
            m_versandadr_ZE = Value
        End Set
    End Property

    Public Property VersandAdresse_ZS() As String
        Get
            Return m_versandadr_ZS
        End Get
        Set(ByVal Value As String)
            m_versandadr_ZS = Value
        End Set
    End Property

    Public Property VersandAdresseText() As String
        Get
            Return m_versandadrtext
        End Get
        Set(ByVal Value As String)
            m_versandadrtext = Value
        End Set
    End Property

    Public Property VersandGrund() As String
        Get
            Return m_versgrund
        End Get
        Set(ByVal Value As String)
            m_versgrund = Value
        End Set
    End Property

    Public Property Auftragsstatus() As String
        Get
            Return strAuftragsstatus
        End Get
        Set(ByVal Value As String)
            strAuftragsstatus = Value
        End Set
    End Property

    Public Property Auftragsnummer() As String
        Get
            Return strAuftragsnummer
        End Get
        Set(ByVal Value As String)
            strAuftragsnummer = Value
        End Set
    End Property

    Public Property Sachbearbeiter() As String
        Get
            Return m_Sachbearbeiter
        End Get
        Set(ByVal Value As String)
            m_Sachbearbeiter = Value
        End Set
    End Property

    Public Property tblUpload() As DataTable
        Get
            Return m_tblUpload
        End Get
        Set(ByVal value As DataTable)
            m_tblUpload = value
        End Set
    End Property

    Public Property VersohneAbeld() As String
        Get
            Return m_VersohneAbeld
        End Get
        Set(ByVal Value As String)
            m_VersohneAbeld = Value
        End Set
    End Property

    Public Property EQuiTyp() As String
        Get
            Return m_EQuiTyp
        End Get
        Set(ByVal Value As String)
            m_EQuiTyp = Value
        End Set
    End Property

    Public Property Halter() As String
        Get
            Return mHalter
        End Get
        Set(ByVal Value As String)
            mHalter = Value
        End Set
    End Property

    Public Property Bemerkung() As String
        Get
            Return mBemerkung
        End Get
        Set(ByVal Value As String)
            mBemerkung = Value
        End Set
    End Property

    Public Property EqTyp() As String
        Get
            Return mEqTyp
        End Get
        Set(ByVal Value As String)
            mEqTyp = Value
        End Set
    End Property

    Public Property Adressart() As Integer
        Get
            Return mAdressart
        End Get
        Set(ByVal Value As Integer)
            mAdressart = Value
        End Set
    End Property

    Public Property AdressartText() As String
        Get
            Return mAdressartText
        End Get
        Set(ByVal Value As String)
            mAdressartText = Value
        End Set
    End Property

    Public Property VersandoptionenText() As String
        Get
            Return mVersandoptionenText
        End Get
        Set(ByVal Value As String)
            mVersandoptionenText = Value
        End Set
    End Property

    Public Property AutorisierungText() As String
        Get
            Return mAutorisierungText
        End Get
        Set(ByVal Value As String)
            mAutorisierungText = Value
        End Set
    End Property

    Public Property Beauftragungsdatum() As String
        Get
            Return mBeauftragungsdatum
        End Get
        Set(ByVal Value As String)
            mBeauftragungsdatum = Value
        End Set
    End Property

    Public Property sReferenz As String
    Public Property sName1 As String
    Public Property sName2 As String
    Public Property sStrasse As String
    Public Property sPLZ As String
    Public Property sOrt As String

    Public Property BrieflieferantNr As String
    Public Property Restlaufzeit As String
    Public Property AbmeldedatumVon As String
    Public Property AbmeldedatumBis As String
    Public Property AbmeldeauftragVon As String
    Public Property AbmeldeauftragBis As String
    Public Property Abgemeldet As String
    Public Property ZulassungsdatumVon As String
    Public Property ZulassungsdatumBis As String

#End Region

#Region "Methods"

    Public Overrides Sub Change()


    End Sub

    Public Overrides Sub Show()


    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Page, _
                            Optional ByVal Upload As Boolean = False)
        m_strClassAndMethod = "Briefversand.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_UNANGEF_ALLG_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_EQTYP", m_EQuiTyp)

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


                Dim SapTable As DataTable = myProxy.getImportTable("GT_IN")

                Dim SapRow As DataRow


                If Upload = False Then

                    If Len(m_strFahrgestellnr & m_strKennzeichen & m_strLVnr & m_strZBIINr & m_strRef1 & m_strRef2) > 0 Then
                        SapRow = SapTable.NewRow
                        SapRow("CHASSIS_NUM") = m_strFahrgestellnr
                        SapRow("LICENSE_NUM") = m_strKennzeichen
                        SapRow("LIZNR") = m_strLVnr
                        SapRow("TIDNR") = m_strZBIINr
                        SapRow("ZZREFERENZ1") = m_strRef1
                        SapRow("ZZREFERENZ2") = m_strRef2
                        SapTable.Rows.Add(SapRow)
                    End If

                Else
                    Dim uploadRow As DataRow
                    For Each uploadRow In m_tblUpload.Rows
                        SapRow = SapTable.NewRow
                        SapRow("CHASSIS_NUM") = uploadRow("CHASSIS_NUM").ToString
                        SapRow("LICENSE_NUM") = uploadRow("LICENSE_NUM").ToString
                        SapRow("LIZNR") = uploadRow("LIZNR").ToString
                        SapRow("TIDNR") = uploadRow("TIDNR").ToString
                        SapRow("ZZREFERENZ1") = uploadRow("ZZREFERENZ1").ToString
                        SapRow("ZZREFERENZ2") = uploadRow("ZZREFERENZ2").ToString
                        SapTable.Rows.Add(SapRow)
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

                    If strKUNNR = "0010026883" Then
                        getVertragsdaten(page, rowTemp)
                    End If

                    tblTemp2.AcceptChanges()

                Next

                m_tblFahrzeuge = CreateOutPut(tblTemp2, strAppID)


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

                    If strKUNNR = "0010026883" Then
                        getVertragsdaten(page, rowTemp)
                    End If

                    tblTemp.AcceptChanges()
                Next

                m_tblFahrzeugeFehler = CreateOutPut(tblTemp, strAppID)

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

    Public Sub GetAdressenandZulStellen(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetAdressenandZulStellen"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_ADRESSPOOL_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_EQTYP", EqTyp)

                myProxy.callBapi()

                Dim SapTableZul As DataTable = myProxy.getExportTable("GT_ZULAST")
                Dim SapTableAdressen As DataTable = myProxy.getExportTable("GT_ADRS")

                Dim SapRow As DataRow

                m_tblZulStellen = SapTableZul.Clone
                m_tblZulStellen.Columns.Add("DISPLAY", GetType(System.String))

                SapTableZul.DefaultView.RowFilter = "LIFNR <> ''"

                SapTableZul = SapTableZul.DefaultView.ToTable

                For Each row In SapTableZul.Rows
                    SapRow = m_tblZulStellen.NewRow

                    SapRow("DISPLAY") = row("PSTLZ").ToString & " - " & row("ORT01").ToString & " - " & row("STRAS").ToString
                    SapRow("LIFNR") = row("LIFNR").ToString
                    SapRow("ORT01") = row("ORT01").ToString
                    SapRow("PSTLZ") = row("PSTLZ").ToString
                    SapRow("STRAS") = row("STRAS").ToString
                    SapRow("ZKFZKZ") = row("ZKFZKZ").ToString
                    SapRow("NAME1") = row("NAME1").ToString
                    SapRow("NAME2") = row("NAME2").ToString
                    m_tblZulStellen.Rows.Add(SapRow)
                Next

                m_tblAdressen = SapTableAdressen.Clone
                m_tblAdressen.Columns.Add("DISPLAY", GetType(System.String))

                For Each row In SapTableAdressen.Rows
                    SapRow = m_tblAdressen.NewRow
                    SapRow("IDENT") = row("IDENT").ToString
                    SapRow("DISPLAY") = row("NAME1").ToString & " " & row("NAME2").ToString & " - " & row("STREET").ToString & ", " & row("CITY1").ToString
                    SapRow("KUNNR") = row("KUNNR").ToString
                    SapRow("NAME1") = row("NAME1").ToString
                    SapRow("NAME2") = row("NAME2").ToString
                    SapRow("STREET") = row("STREET").ToString
                    SapRow("HOUSE_NUM1") = row("HOUSE_NUM1").ToString
                    SapRow("POST_CODE1") = row("POST_CODE1").ToString
                    SapRow("CITY1") = row("CITY1").ToString
                    SapRow("COUNTRY") = row("COUNTRY").ToString
                    SapRow("NAME1") = row("NAME1").ToString
                    SapRow("NAME2") = row("NAME2").ToString
                    m_tblAdressen.Rows.Add(SapRow)
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblAdressen)
                getLaender(page)

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

    Public Sub GetAdressen(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetAdressen"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_ADRESSPOOL_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_EQTYP", EqTyp)
                myProxy.setImportParameter("I_POS_TEXT", sReferenz)
                myProxy.setImportParameter("I_NAME1", sName1)
                myProxy.setImportParameter("I_NAME2", sName2)
                myProxy.setImportParameter("I_STRAS", sStrasse)
                myProxy.setImportParameter("I_PSTLZ", sPLZ)
                myProxy.setImportParameter("I_ORT01", sOrt)

                myProxy.callBapi()

                Dim SapTableAdressen As DataTable = myProxy.getExportTable("GT_ADRS")

                Dim SapRow As DataRow

                m_tblAdressen = SapTableAdressen.Clone
                m_tblAdressen.Columns.Add("DISPLAY", GetType(System.String))

                For Each row In SapTableAdressen.Rows
                    SapRow = m_tblAdressen.NewRow
                    SapRow("IDENT") = row("IDENT").ToString
                    SapRow("DISPLAY") = row("NAME1").ToString & " " & row("NAME2").ToString & " - " & row("STREET").ToString & ", " & row("CITY1").ToString
                    SapRow("KUNNR") = row("KUNNR").ToString
                    SapRow("NAME1") = row("NAME1").ToString
                    SapRow("NAME2") = row("NAME2").ToString
                    SapRow("STREET") = row("STREET").ToString
                    SapRow("HOUSE_NUM1") = row("HOUSE_NUM1").ToString
                    SapRow("POST_CODE1") = row("POST_CODE1").ToString
                    SapRow("CITY1") = row("CITY1").ToString
                    SapRow("COUNTRY") = row("COUNTRY").ToString
                    SapRow("SAPNR") = row("SAPNR").ToString
                    SapRow("NAME1") = row("NAME1").ToString
                    SapRow("NAME2") = row("NAME2").ToString
                    m_tblAdressen.Rows.Add(SapRow)
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblAdressen)

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

    Public Sub GetAbrufgrund(ByVal strAppID As String, _
                   ByVal strSessionID As String, _
                   ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetAbrufgrund"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_VERS_GRUND_KUN_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_ABCKZ", m_strVersandArt)
                myProxy.setImportParameter("I_GRUPPE", "")


                myProxy.callBapi()


                m_tblVersandgruende = myProxy.getExportTable("GT_OUT")


                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblVersandgruende)
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

    Public Sub GetVersandOptions(ByVal strAppID As String, _
                       ByVal strSessionID As String, _
                       ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetVersandOptions"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_LV_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_VWAG", "X")

                Dim SapTable As DataTable = myProxy.getImportTable("GT_IN_AG")

                Dim SapRow As DataRow

                SapRow = SapTable.NewRow

                SapRow("AG") = strKUNNR

                SapTable.Rows.Add(SapRow)

                Dim SapProzess As DataTable = myProxy.getImportTable("GT_IN_PROZESS")

                Dim RowProz As DataRow

                RowProz = SapProzess.NewRow

                RowProz("SORT1") = m_OptionFlag

                SapProzess.Rows.Add(RowProz)

                myProxy.callBapi()

                m_tblVersandOptions = myProxy.getExportTable("GT_OUT_DL")
                m_tblVersandOptions.Columns.Add("Selected", GetType(System.String))
                m_tblVersandOptions.Columns.Add("Description", GetType(System.String))
                m_tblVersandOptions.Columns("Description").DefaultValue = ""

                m_tblVersandOptions.AcceptChanges()

                For Each dr As DataRow In m_tblVersandOptions.Rows
                    dr("Description") = ""
                Next


                For Each VersandRow As DataRow In m_tblVersandOptions.Rows
                    If VersandRow("VW_AG") = "X" Then
                        VersandRow("Selected") = "1"
                    End If
                Next


                Dim LangTextTable As DataTable = myProxy.getExportTable("GT_OUT_ESLL_LTXT")

                If LangTextTable.Rows.Count > 0 Then

                    Dim strText As String

                    For Each dr As DataRow In m_tblVersandOptions.Rows

                        strText = ""

                        LangTextTable.DefaultView.RowFilter = "SRVPOS = '" & dr("ASNUM").ToString.PadLeft(18, "0"c) & "'"

                        If LangTextTable.DefaultView.Count > 0 Then

                            For i = 0 To LangTextTable.DefaultView.Count - 1

                                strText &= LangTextTable.DefaultView.Item(i)("TDLINE").ToString & " "

                            Next

                        End If

                        dr("Description") = strText

                    Next

                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblVersandOptions)

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

    Private Sub getLaender(ByVal page As Page)
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

            m_tblLaender = myProxy.getExportTable("GT_WEB")


            m_tblLaender.Columns.Add("Beschreibung", Type.GetType("System.String"))
            m_tblLaender.Columns.Add("FullDesc", Type.GetType("System.String"))
            Dim rowTemp As DataRow
            For Each rowTemp In m_tblLaender.Rows
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

    Sub GetStueckliste(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Briefversand.GetStueckliste"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim strKUNNR = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim myProxy = DynSapProxy.getProxy("Z_DPM_READ_EQUI_STL_01", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_AG", strKUNNR)
                myProxy.setImportParameter("I_EQTYP", "T")
                myProxy.setImportParameter("I_STATUS", "L;T;V") ' lagernd, temporär versandt, Versand angefordert

                Dim gt_in = myProxy.getImportTable("GT_IN")
                Fahrzeuge.Select("Selected = '1'").ToList().ForEach(Sub(r)
                                                                        Dim new_r = gt_in.NewRow
                                                                        new_r("CHASSIS_NUM") = r("Fahrgestellnummer")
                                                                        new_r("EQUNR") = r("EQUNR")
                                                                        gt_in.Rows.Add(new_r)
                                                                    End Sub)
                gt_in.AcceptChanges()

                myProxy.callBapi()

                Dim gt_out = myProxy.getExportTable("GT_OUT")

                Dim col = New DataColumn("Selected", GetType(String))
                col.DefaultValue = String.Empty
                gt_out.Columns.Add(col)
                gt_out.AcceptChanges()
                m_tblStueckliste = gt_out

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblStueckliste)
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

    Private Sub getBrieflieferanten(ByVal page As Page)

        m_intStatus = 0
        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_BRIEFLIEFERANTEN_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, "0"c))

            myProxy.callBapi()

            m_Brieflieferanten = myProxy.getExportTable("GT_OUT")


            m_Brieflieferanten.Columns.Add("Adresse", Type.GetType("System.String"))

            Dim rowTemp As DataRow
            For Each rowTemp In m_Brieflieferanten.Rows
                rowTemp("Adresse") = rowTemp("NAME1")

                If String.IsNullOrEmpty(rowTemp("NAME2")) = False Then
                    rowTemp("Adresse") &= " " & rowTemp("NAME2")
                End If

                rowTemp("Adresse") &= ", " & rowTemp("CITY1")

            Next

            Dim NewRow As DataRow = m_Brieflieferanten.NewRow

            NewRow("KUNNR") = "0"
            NewRow("Adresse") = "--- Auswahl ---"

            m_Brieflieferanten.Rows.Add(NewRow)

            m_Brieflieferanten.DefaultView.Sort = "Adresse"

            m_Brieflieferanten = m_Brieflieferanten.DefaultView.ToTable


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

    Public Sub Anfordern(ByVal strAppID As String, _
                      ByVal strSessionID As String, _
                      ByVal page As Page)
        m_strClassAndMethod = "Briefversand.Anfordern"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim myProxy = DynSapProxy.getProxy("Z_DPM_FILL_VERSAUFTR", m_objApp, m_objUser, page)
                myProxy.setImportParameter("KUNNR_AG", strKUNNR)

                Dim SapTable = myProxy.getImportTable("GT_IN")

                Dim selectedRows = Fahrzeuge.Select("Selected = '1'")
                If selectedRows.Length > 0 Then
                    For Each Fahrzeugrow In selectedRows

                        If Not m_tblStueckliste Is Nothing Then

                            Dim selectedParts = Stueckliste.Select(String.Format("Selected='1' and EQUNR='{0}'", Fahrzeugrow("EQUNR")))
                            If selectedParts.Length > 0 Then
                                For Each partRow As DataRow In selectedParts
                                    Dim SapRow As DataRow = SapTable.NewRow
                                    FillSapRow(SapRow, Fahrzeugrow, strKUNNR, partRow)
                                    SapTable.Rows.Add(SapRow)
                                Next
                            Else
                                Dim SapRow As DataRow = SapTable.NewRow
                                FillSapRow(SapRow, Fahrzeugrow, strKUNNR)
                                SapTable.Rows.Add(SapRow)
                            End If
                        Else
                            Dim SapRow As DataRow = SapTable.NewRow
                            FillSapRow(SapRow, Fahrzeugrow, strKUNNR)
                            SapTable.Rows.Add(SapRow)

                        End If


                    Next

                    myProxy.callBapi()

                    m_tblVersandError = myProxy.getExportTable("GT_ERR")


                    mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                    mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

                    If IsNumeric(mE_SUBRC) Then
                        m_intStatus = CInt(mE_SUBRC)
                        If mE_MESSAGE.Length > 0 Then m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    End If
                    If m_tblVersandError.Rows.Count > 0 Then
                        m_strMessage = "Eine oder mehrere Anforderungen konnten im System nicht erstellt werden."
                    End If

                    For Each tmpRow As DataRow In m_tblFahrzeuge.Select("Selected = '1'")
                        selectedRows = m_tblVersandError.Select("CHASSIS_NUM = '" & tmpRow("Fahrgestellnummer").ToString & "'")
                        If selectedRows.Length > 0 Then
                            'tmpRow("Bemerkung") = "Fehler: " & String.Join(", ", selectedRows.Select(Function(r) r("Bemerkung")).ToArray)
                            Dim objArray As Object() = selectedRows.Select(Function(r) r("Bemerkung")).ToArray
                            Dim strArray(objArray.GetUpperBound(0)) As String
                            For i = 0 To objArray.GetUpperBound(0)
                                strArray(i) = CStr(objArray(i))
                            Next
                            tmpRow("Bemerkung") = String.Join(", ", strArray)   '"Fehler: " & 
                            WriteLogEntry(False, "KUNNR=" + m_objUser.KUNNR + "; FIN = " + tmpRow("Fahrgestellnummer").ToString, m_tblFahrzeuge)
                        Else
                            tmpRow("Bemerkung") = "erfolgreich"
                            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR + "; FIN = " + tmpRow("Fahrgestellnummer").ToString, m_tblFahrzeuge)
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
        row("ZZBRFVERS") = m_Briefversand
        row("ZZSCHLVERS") = m_SchluesselVersand
        row("ZZABMELD") = m_VersohneAbeld
        row("ABCKZ") = m_strVersandArt
        row("MATNR") = m_material
        row("ZZVGRUND") = m_versgrund
        row("ZZANFDT") = Now.ToShortDateString
        row("ZZBETREFF") = mBemerkung
        row("ZZNAME_ZH") = mHalter

        'PartnerNr
        row("ZZKUNNR_ZS") = m_versandadr_ZS

        'Zulassungsstelle
        row("ZZADRNR_ZS") = m_versandadr_ZE

        If VersandArt = "1" Then
            If m_versandadr_ZE.Length > 0 Then
                row("ZZ_MAHNA") = "0001"
            Else
                row("ZZ_MAHNA") = "0002"
            End If
        End If

        row("ZZKONZS_ZS") = ""
        ' Freie Adresse
        row("ZZNAME1_ZS") = m_strName1
        row("ZZNAME2_ZS") = m_strName2
        row("ZZSTRAS_ZS") = m_strStreet
        row("ZZHAUSNR_ZS") = m_strHouseNum
        row("ZZPSTLZ_ZS") = m_strPostcode
        row("ZZORT01_ZS") = m_strCity
        row("COUNTRY_ZS") = m_strLaenderKuerzel
        row("ZZLAND_ZS") = m_strLaenderKuerzel
        row("ERNAM") = Left(m_objUser.UserName, 12)
        row("LIZNR") = fzgRow("Leasingnummer").ToString

        If Not partRow Is Nothing Then
            row("IDNRK") = partRow("IDNRK")
        End If
    End Sub

    Public Sub AnfordernAusAutorisierung(ByVal strAppID As String, _
                  ByVal strSessionID As String, _
                  ByVal page As Page)

        m_strClassAndMethod = "Briefversand.Anfordern"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_FILL_VERSAUFTR", m_objApp, m_objUser, page)
                myProxy.setImportParameter("KUNNR_AG", strKUNNR)
                Dim SapTable As DataTable = myProxy.getImportTable("GT_IN")

                Dim SelectedRow() As DataRow
                Dim Fahrzeugrow As DataRow

                SelectedRow = m_tblFahrzeuge.Select("Selected = '1' AND EQUNR='" & m_strReferenceforAut & "'")
                If SelectedRow.Length > 0 Then
                    For Each Fahrzeugrow In SelectedRow

                        Dim SapRow As DataRow = SapTable.NewRow

                        SapRow("ZZKUNNR_AG") = strKUNNR
                        SapRow("LICENSE_NUM") = Fahrzeugrow("Kennzeichen").ToString
                        SapRow("CHASSIS_NUM") = Fahrzeugrow("Fahrgestellnummer").ToString
                        SapRow("ZZBRFVERS") = m_Briefversand
                        SapRow("ZZSCHLVERS") = m_SchluesselVersand
                        SapRow("ZZABMELD") = m_VersohneAbeld
                        SapRow("ABCKZ") = m_strVersandArt
                        SapRow("MATNR") = m_material
                        SapRow("ZZVGRUND") = m_versgrund
                        SapRow("ZZANFDT") = Now.ToShortDateString
                        'PartnerNr

                        SapRow("ZZKUNNR_ZS") = m_versandadr_ZS

                        'Zulassungsstelle
                        SapRow("ZZADRNR_ZS") = m_versandadr_ZE

                        If VersandArt = "1" Then
                            If m_versandadr_ZE.Length > 0 Then
                                SapRow("ZZ_MAHNA") = "0001"
                            Else
                                SapRow("ZZ_MAHNA") = "0002"
                            End If
                        End If

                        SapRow("ZZKONZS_ZS") = ""
                        ' Freie Adresse
                        SapRow("ZZNAME1_ZS") = m_strName1
                        SapRow("ZZNAME2_ZS") = m_strName2
                        SapRow("ZZSTRAS_ZS") = m_strStreet
                        SapRow("ZZHAUSNR_ZS") = m_strHouseNum
                        SapRow("ZZPSTLZ_ZS") = m_strPostcode
                        SapRow("ZZORT01_ZS") = m_strCity
                        SapRow("COUNTRY_ZS") = m_strLaenderKuerzel

                        SapRow("ERNAM") = Left(m_objUser.UserName, 12)
                        SapRow("LIZNR") = Fahrzeugrow("Leasingnummer").ToString

                        SapTable.Rows.Add(SapRow)

                    Next

                    myProxy.callBapi()

                    m_tblVersandError = myProxy.getExportTable("GT_ERR")


                    mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                    mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")
                    If IsNumeric(mE_SUBRC) Then
                        m_intStatus = CInt(mE_SUBRC)
                        If mE_MESSAGE.Length > 0 Then m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    End If
                    If m_tblVersandError.Rows.Count > 0 Then
                        m_strMessage = "Eine oder mehrere Anforderungen konnten im System nicht erstellt werden."
                    End If



                    For Each tmpRow As DataRow In m_tblFahrzeuge.Select("Selected = '1' AND EQUNR='" & m_strReferenceforAut & "'")
                        SelectedRow = m_tblVersandError.Select("CHASSIS_NUM = '" & tmpRow("Fahrgestellnummer").ToString & "'")
                        If SelectedRow.Length > 0 Then
                            m_intStatus = -2100
                            m_strMessage = "Fehler: " & SelectedRow(0)("Bemerkung").ToString
                            WriteLogEntry(False, "KUNNR=" + m_objUser.KUNNR + "; FIN = " + tmpRow("Fahrgestellnummer").ToString, m_tblFahrzeuge)
                        Else
                            m_strMessage = "Ihre Anforderung wurde erfolgreich in unserem System übernommen."
                            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR + "; FIN = " + tmpRow("Fahrgestellnummer").ToString, m_tblFahrzeuge)
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

    Public Sub Anfordern2(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Briefversand.Anfordern2"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim myProxy = DynSapProxy.getProxy("Z_DPM_IMP_VERS_BEAUFTR_01", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)

                Dim SapTable = myProxy.getImportTable("GT_IN")

                Dim selectedRows = Fahrzeuge.Select("Selected = '1'")
                If selectedRows.Length > 0 Then
                    For Each Fahrzeugrow In selectedRows

                        Dim SapRow As DataRow = SapTable.NewRow
                        FillSapRow2(SapRow, Fahrzeugrow)
                        SapTable.Rows.Add(SapRow)

                    Next

                    myProxy.callBapi()

                    m_tblVersandError = myProxy.getExportTable("GT_OUT")

                    If m_tblVersandError.Select("ABLAUFSTATUS='FEHLERHAFT'").Length > 0 Then
                        m_strMessage = "Eine oder mehrere Anforderungen konnten im System nicht erstellt werden."
                    End If

                    For Each tmpRow As DataRow In m_tblFahrzeuge.Select("Selected = '1'")
                        selectedRows = m_tblVersandError.Select("LEISTUNGSIDCMS = '" & tmpRow("Leasingnummer").ToString & "'")
                        If selectedRows.Length > 0 AndAlso selectedRows(0)("ABLAUFSTATUS").ToString() = "FEHLERHAFT" Then
                            tmpRow("Bemerkung") = selectedRows(0)("FEHLERART").ToString()
                            WriteLogEntry(False, "KUNNR=" + m_objUser.KUNNR + "; LIZNR = " + tmpRow("Leasingnummer").ToString, m_tblFahrzeuge)
                        Else
                            tmpRow("Bemerkung") = "erfolgreich"
                            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR + "; LIZNR = " + tmpRow("Leasingnummer").ToString, m_tblFahrzeuge)
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
        row("ANF_ART") = m_strVersandArt
        row("VERS_ART") = m_material
        row("VERS_GRUND") = m_versgrund
        Select Case mAdressart
            Case 1, 4
                row("EMPF_ART") = "DRITTE"
            Case 2, 5
                row("EMPF_ART") = "ZULASSUNGSSTELLE"
            Case Else
                row("EMPF_ART") = ""
        End Select
        row("NAME_EMPF") = m_strName1
        row("VORNAME_EMPF") = ""
        row("STRASSE_EMPF") = m_strStreet
        row("HNR_EMPF") = m_strHouseNum
        row("PLZ_EMPF") = m_strPostcode
        row("ORT_EMPF") = m_strCity
        row("LAND_EMPF") = m_strLaenderKuerzel
        row("SYSTEMKENNZ") = ""
        row("AUFTRAGGEBERID") = m_objUser.UserName
        row("NAME_ANSP") = m_strName2
        row("NAME_ANF") = ""
        row("VORNAME_ANF") = ""
        row("STRASSE_ANF") = ""
        row("HNR_ANF") = ""
        row("PLZ_ANF") = ""
        row("ORT_ANF") = ""
        row("LAND_ANF") = ""
        row("CLIENTLD") = ""

    End Sub

    Public Sub AnfordernAusAutorisierung2(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        m_strClassAndMethod = "Briefversand.AnfordernAusAutorisierung2"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_IMP_VERS_BEAUFTR_01", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                Dim SapTable As DataTable = myProxy.getImportTable("GT_IN")

                Dim SelectedRow() As DataRow
                Dim Fahrzeugrow As DataRow

                SelectedRow = m_tblFahrzeuge.Select("Selected = '1' AND EQUNR='" & m_strReferenceforAut & "'")
                If SelectedRow.Length > 0 Then
                    For Each Fahrzeugrow In SelectedRow

                        Dim SapRow As DataRow = SapTable.NewRow

                        SapRow("LEISTUNGSIDCMS") = Fahrzeugrow("Leasingnummer").ToString
                        SapRow("SICHERHEITSIDCMS") = ""
                        SapRow("ZBRIEF") = Fahrzeugrow("NummerZBII").ToString
                        SapRow("ANF_ART") = m_strVersandArt
                        SapRow("VERS_ART") = m_material
                        SapRow("VERS_GRUND") = m_versgrund
                        Select Case mAdressart
                            Case 1, 4
                                SapRow("EMPF_ART") = "DRITTE"
                            Case 2, 5
                                SapRow("EMPF_ART") = "ZULASSUNGSSTELLE"
                            Case Else
                                SapRow("EMPF_ART") = ""
                        End Select
                        SapRow("NAME_EMPF") = m_strName1
                        SapRow("VORNAME_EMPF") = ""
                        SapRow("STRASSE_EMPF") = m_strStreet
                        SapRow("HNR_EMPF") = m_strHouseNum
                        SapRow("PLZ_EMPF") = m_strPostcode
                        SapRow("ORT_EMPF") = m_strCity
                        SapRow("LAND_EMPF") = m_strLaenderKuerzel
                        SapRow("SYSTEMKENNZ") = ""
                        SapRow("AUFTRAGGEBERID") = m_objUser.UserName
                        SapRow("NAME_ANSP") = m_strName2
                        SapRow("NAME_ANF") = ""
                        SapRow("VORNAME_ANF") = ""
                        SapRow("STRASSE_ANF") = ""
                        SapRow("HNR_ANF") = ""
                        SapRow("PLZ_ANF") = ""
                        SapRow("ORT_ANF") = ""
                        SapRow("LAND_ANF") = ""
                        SapRow("CLIENTLD") = ""

                        SapTable.Rows.Add(SapRow)

                    Next

                    myProxy.callBapi()

                    m_tblVersandError = myProxy.getExportTable("GT_OUT")

                    If m_tblVersandError.Select("ABLAUFSTATUS='FEHLERHAFT'").Length > 0 Then
                        m_strMessage = "Eine oder mehrere Anforderungen konnten im System nicht erstellt werden."
                    End If

                    For Each tmpRow As DataRow In m_tblFahrzeuge.Select("Selected = '1' AND EQUNR='" & m_strReferenceforAut & "'")
                        SelectedRow = m_tblVersandError.Select("LEISTUNGSIDCMS = '" & tmpRow("Leasingnummer").ToString & "'")
                        If SelectedRow.Length > 0 AndAlso SelectedRow(0)("ABLAUFSTATUS").ToString() = "FEHLERHAFT" Then
                            m_intStatus = -2100
                            m_strMessage = "Fehler: " & SelectedRow(0)("FEHLERART").ToString()
                            WriteLogEntry(False, "KUNNR=" + m_objUser.KUNNR + "; LIZNR = " + tmpRow("Leasingnummer").ToString, m_tblFahrzeuge)
                        Else
                            m_strMessage = "Ihre Anforderung wurde erfolgreich in unserem System übernommen."
                            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR + "; LIZNR = " + tmpRow("Leasingnummer").ToString, m_tblFahrzeuge)
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
        m_tblUpload = New DataTable
        With m_tblUpload
            .Columns.Add("CHASSIS_NUM", GetType(System.String))
            .Columns.Add("LICENSE_NUM", GetType(System.String))
            .Columns.Add("LIZNR", GetType(System.String))
            .Columns.Add("TIDNR", GetType(System.String))
            .Columns.Add("ZZREFERENZ1", GetType(System.String))
            .Columns.Add("ZZREFERENZ2", GetType(System.String))
        End With
    End Sub

    Private Sub getVertragsdaten(ByVal page As Page, ByRef row As DataRow)

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

    Public Function ShowStilllegungsdatumPopup(ByVal strAppID As String) As Boolean
        Dim strWert As String = Base.Kernel.Common.Common.GetApplicationConfigValue("PopupStilllegungsdatumAnzeigen", strAppID, m_objUser.Customer.CustomerId, m_objUser.GroupID)
        Dim blnShowPopup = (Not String.IsNullOrEmpty(strWert) AndAlso strWert.ToUpper() = "TRUE")

        Dim blnAufAbmeldungWarten = VersandOptionen.Select("EXTGROUP='" & VersandArt & "' AND EAN11 = 'ZZABMELD_INVERTED' AND Selected = '1'").Any()

        Return (Adressart = Adressarten.EndManuell AndAlso Fahrzeuge.Select("Selected = '1' AND Abmeldedatum IS NULL").Any() AndAlso blnShowPopup AndAlso Not blnAufAbmeldungWarten)
    End Function

#End Region

End Class
