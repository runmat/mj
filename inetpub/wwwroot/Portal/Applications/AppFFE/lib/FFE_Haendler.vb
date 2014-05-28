Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports System.Configuration
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class FFE_Haendler
    Inherits FFE_BankBase
#Region " Declarations"
    Private m_tblAbrufgruende As DataTable
    Private m_strSucheVertragsNr As String
    Private m_strSucheOrderNr As String
    Private m_strSucheBriefNr As String
    Private m_strSucheFahrgestellNr As String
    Private m_tblFahrzeuge As DataTable
    Private m_tblZulStelle As DataTable
    Private m_strZZFAHRG As String
    Private m_strAdresse As String
    Private m_strAuftragsnummer As String
    Private m_strAuftragsstatus As String
    Private m_strKreditkontrollBereich As String
    Private m_strMaterialNummer As String 'Versandart
    Private m_strAdresseHalter As String    'HEZ
    Private m_strAdresseEmpf As String    'HEZ
    Private m_preis As Decimal 'HEZ
    Private m_datumRange As DataTable       'HEZ:Hier Datumswerte auslesen...
    Private m_kbanr As String 'Zulassungsdienst-Nummer
    Private m_ZzLaufzeit As String
    Private m_strName1 As String
    Private m_strName2 As String
    Private m_strName3 As String
    Private m_strCity As String
    Private m_strPostcode As String
    Private m_strStreet As String
    Private m_strHouseNum As String
    Private m_bNewAdress As Boolean
    Private m_tblLaender As DataTable
#End Region

#Region " Properties"

    Public ReadOnly Property Abrufgruende() As DataTable
        Get
            If m_tblAbrufgruende Is Nothing Then
                Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim cmdAg As SqlClient.SqlCommand
                Dim dsAg As DataSet
                Dim adAg As SqlClient.SqlDataAdapter

                Try

                    cn.Open()

                    dsAg = New DataSet()

                    adAg = New SqlClient.SqlDataAdapter()

                    cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                                    "[WebBezeichnung]," & _
                                                    "[SapWert]," & _
                                                    "[MitZusatzText]," & _
                                                    "[Zusatzbemerkung], " & _
                                                    "[AbrufTyp] " & _
                                                    "FROM CustomerAbrufgruende " & _
                                                    "WHERE " & _
                                                    "CustomerID = " & m_objUser.Customer.CustomerId.ToString & _
                                                    " AND GroupID = " & m_objUser.GroupID.ToString & _
                                                    " Order by WebBezeichnung" _
                                                    , cn)
                    cmdAg.CommandType = CommandType.Text
                    'AbrufTyp: 'temp' oder 'endg'

                    adAg.SelectCommand = cmdAg
                    adAg.Fill(dsAg, "Abrufgruende")

                    If dsAg.Tables("Abrufgruende") Is Nothing OrElse dsAg.Tables("Abrufgruende").Rows.Count = 0 Then
                        Throw New Exception("Keine Abrufgründe für den Kunden hinterlegt.")
                    End If

                    m_tblAbrufgruende = dsAg.Tables("Abrufgruende")
                Catch ex As Exception
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End If

            Return m_tblAbrufgruende
        End Get
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

    Public Property Name3() As String
        Get
            Return m_strName3
        End Get
        Set(ByVal Value As String)
            m_strName3 = Value
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
    Public Property kbanr() As String
        Get
            Return m_kbanr
        End Get
        Set(ByVal Value As String)
            m_kbanr = Value
        End Set
    End Property

    Public Property preis() As Decimal
        Get
            Return m_preis
        End Get
        Set(ByVal Value As Decimal)
            m_preis = Value
        End Set

    End Property
    Public Property zuldatum() As Date
        Get
            Return m_zuldatum
        End Get
        Set(ByVal Value As Date)
            m_zuldatum = Value
        End Set
    End Property

    Public Property MaterialNummer() As String
        Get
            Return m_strMaterialNummer
        End Get
        Set(ByVal Value As String)
            m_strMaterialNummer = Value
        End Set
    End Property

    Public Property KreditkontrollBereich() As String
        Get
            Return m_strKreditkontrollBereich
        End Get
        Set(ByVal Value As String)
            m_strKreditkontrollBereich = Right("0000" & Value.Trim(" "c), 4)
        End Set
    End Property

    Public ReadOnly Property Auftragsnummer() As String
        Get
            Return m_strAuftragsnummer
        End Get
    End Property

    Public ReadOnly Property Auftragsstatus() As String
        Get
            Return m_strAuftragsstatus
        End Get
    End Property

    Public Property ZZFAHRG() As String
        Get
            Return m_strZZFAHRG
        End Get
        Set(ByVal Value As String)
            m_strZZFAHRG = Value
        End Set
    End Property

    Public Property Adresse() As String
        Get
            Return m_strAdresse
        End Get
        Set(ByVal Value As String)
            If Value.Length > 0 Then
                m_strAdresse = Right("00000000000" & Value, 10)
            Else
                m_strAdresse = ""
            End If
        End Set
    End Property

    Public Property AdresseHalter() As String
        Get
            Return m_strAdresseHalter
        End Get
        Set(ByVal Value As String)
            m_strAdresseHalter = Right("00000000000" & Value, 10)
        End Set
    End Property

    Public Property AdresseEmpf() As String
        Get
            Return m_strAdresseEmpf
        End Get
        Set(ByVal Value As String)
            m_strAdresseEmpf = Right("00000000000" & Value, 10)
        End Set
    End Property

    Public Property SucheVertragsNr() As String
        Get
            Return m_strSucheVertragsNr
        End Get
        Set(ByVal Value As String)
            m_strSucheVertragsNr = Value
        End Set
    End Property

    Public Property SucheOrderNr() As String
        Get
            Return m_strSucheOrderNr
        End Get
        Set(ByVal Value As String)
            m_strSucheOrderNr = Value
        End Set
    End Property

    Public Property SucheBriefNr() As String
        Get
            Return m_strSucheBriefNr
        End Get
        Set(ByVal Value As String)
            m_strSucheBriefNr = Value
        End Set
    End Property

    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellNr
        End Get
        Set(ByVal Value As String)
            m_strSucheFahrgestellNr = Value
        End Set
    End Property

    Public Property Laufzeit() As String
        Get
            Return m_ZzLaufzeit
        End Get
        Set(ByVal Value As String)
            m_ZzLaufzeit = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeuge = Value
        End Set
    End Property

    Public Property ZulStellen() As DataTable
        Get
            Return m_tblZulStelle
        End Get
        Set(ByVal Value As DataTable)
            m_tblZulStelle = Value
        End Set
    End Property

    Public Property NewAdress() As Boolean
        Get
            Return m_bNewAdress
        End Get
        Set(ByVal Value As Boolean)
            m_bNewAdress = Value
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

#End Region
#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, ByVal strCustomer As String, Optional ByVal strCreditControlArea As String = "ZDAD", Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        Customer = strCustomer
        CreditControlArea = strCreditControlArea
        m_intStatus = 0
        m_strMessage = ""
        m_strAuftragsnummer = ""
        m_strAuftragsstatus = ""
        m_strSucheOrderNr = ""
        m_strSucheVertragsNr = ""
        m_strSucheFahrgestellNr = ""
        m_hez = hez
        m_ZzLaufzeit = ""
        Show()
    End Sub

    Public Function isHEZ() As Boolean
        Return m_hez
    End Function

    Public Function checkInputHEZ(ByVal zulDatum As String, ByRef message As String) As Boolean 'HEZ
        Dim datum As Date
        Dim row As DataRow()
        '§§§JVE 23.06.2005 <begin>
        'Dim drow As DataRow()

        'Datum?
        If Not IsDate(zulDatum) Then
            message = "Falsches Datumsformat."
            Return False
        End If
        'Hier SAP-Datumsfelder durchhühnern...
        datum = CType(zulDatum, Date)
        zulDatum = MakeDateSAP(datum).ToString

        row = m_datumRange.Select("Low = '" & zulDatum & "'")

        If row.Length = 0 Then  'Kein Datum gefunden. Also ugültig.
            message = "Ungültiges Zulassungsdatum (" & MakeDateStandard(zulDatum) & ")"
            Return False
        End If

        'curTime = Date.Now.TimeOfDay


        'While ((row <= m_datumRange.Rows.Count - 1) AndAlso m_datumRange.Rows(row)("LOW").ToString <> zulDatum)
        '    row += 1
        'End While
        'If row > (m_datumRange.Rows.Count - 1) Then
        '    message = "Datum ungültig. Das frühstmögliche Zulassungsdatum ist der " & MakeDateStandard(m_datumRange.Rows(0)("LOW").ToString)
        '    Return False        'Datum nicht gefunden
        'End If

        '§§§ <end>
        Return True
    End Function

    Public Sub GiveCars()
        m_strClassAndMethod = "FDD_Haendler.GiveCars"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            Dim tblFahrzeugeSAP As New SAPProxy_FFE.ZDAD_M_WEB_EQUIDATENTable()
            Dim rowFahrzeugSAP As SAPProxy_FFE.ZDAD_M_WEB_EQUIDATEN

            m_tblFahrzeuge = New DataTable()

            With m_tblFahrzeuge.Columns
                .Add("ZZFAHRG", System.Type.GetType("System.String"))
                .Add("MANDT", System.Type.GetType("System.String"))
                .Add("LIZNR", System.Type.GetType("System.String"))
                .Add("EQUNR", System.Type.GetType("System.String"))
                .Add("TIDNR", System.Type.GetType("System.String"))
                .Add("LICENSE_NUM", System.Type.GetType("System.String"))
                .Add("ZZREFERENZ1", System.Type.GetType("System.String"))
                .Add("ZZBEZAHLT", System.Type.GetType("System.Boolean"))
                .Add("ZZCOCKZ", System.Type.GetType("System.Boolean"))
                .Add("ZZLABEL", System.Type.GetType("System.String"))
                .Add("VBELN", System.Type.GetType("System.String"))
                .Add("COMMENT", System.Type.GetType("System.String"))
                .Add("TEXT50", System.Type.GetType("System.String"))
                .Add("TEXT200", System.Type.GetType("System.String"))
                '§§§JVE 19.10.2005
                .Add("KOPFTEXT", System.Type.GetType("System.String"))
                .Add("POSITIONSTEXT", System.Type.GetType("System.String"))
                '§§§JVE 19.09.2005 <begin>
                'Spalte Finanzierungsart zusätzlich eingefügt...
                .Add("ZZFINART", System.Type.GetType("System.String"))
                '§§§JVE 19.09.2005 <end>
                'SFa 13.01.2006 (Freitag!) Feld für die Mindesthaltefrist hinzugefügt
                .Add("ZZLAUFZEIT", System.Type.GetType("System.String"))
                .Add("ZZBOOLAUFZEIT", System.Type.GetType("System.Boolean"))
                'Orudo 30.05.2007 Feld für die Anfragenummer hinzugefügt
                .Add("TEXT300", System.Type.GetType("System.String"))
                'Orudo 07.04.2008 Feld für die Anfragenummer hinzugefügt
                .Add("AUGRU", System.Type.GetType("System.String"))
                .Add("AUGRU_Klartext", System.Type.GetType("System.String"))
            End With

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                If CheckCustomerData() Then
                    If (m_HEZ = False) Then
                        m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                        objSAP.Z_M_Unangefordert_Fce(m_strSucheFahrgestellNr, m_strFiliale, m_strKUNNR, Right(m_strCustomer, 5), m_strSucheVertragsNr, "1510", m_strSucheBriefNr, m_strSucheOrderNr, tblFahrzeugeSAP)
                    Else
                        Dim sapDatumTabelle As New SAPProxy_FFE.DATUM_RANGETable()      'HEZ:Hier Datumswerte auslesen...

                        m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert_Hez", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                        objSAP.Z_M_Unangefordert_Hez(m_strSucheFahrgestellNr, m_strFiliale, m_strKUNNR, Right(m_strCustomer, 5), m_strSucheVertragsNr, "1510", m_strSucheOrderNr, tblFahrzeugeSAP, sapDatumTabelle) 'Für HEZ
                        m_datumRange = sapDatumTabelle.ToADODataTable
                    End If
                    objSAP.CommitWork()

                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                    End If

                    For Each rowFahrzeugSAP In tblFahrzeugeSAP
                        Dim rowFahrzeug As DataRow
                        rowFahrzeug = m_tblFahrzeuge.NewRow
                        rowFahrzeug("ZZFAHRG") = rowFahrzeugSAP.Chassis_Num
                        rowFahrzeug("MANDT") = "0"
                        rowFahrzeug("EQUNR") = rowFahrzeugSAP.Equnr
                        rowFahrzeug("LIZNR") = rowFahrzeugSAP.Liznr
                        rowFahrzeug("TIDNR") = rowFahrzeugSAP.Tidnr
                        rowFahrzeug("LICENSE_NUM") = rowFahrzeugSAP.License_Num
                        rowFahrzeug("ZZREFERENZ1") = rowFahrzeugSAP.Zzreferenz1
                        rowFahrzeug("ZZLABEL") = rowFahrzeugSAP.Zzlabel
                        If UCase(rowFahrzeugSAP.Zzcockz) = "X" Then
                            rowFahrzeug("ZZCOCKZ") = True
                        Else
                            rowFahrzeug("ZZCOCKZ") = False
                        End If
                        If UCase(rowFahrzeugSAP.Zzbezahlt) = "X" Then
                            rowFahrzeug("ZZBEZAHLT") = True
                        Else
                            rowFahrzeug("ZZBEZAHLT") = False
                        End If
                        rowFahrzeug("VBELN") = ""
                        rowFahrzeug("COMMENT") = ""
                        '§§§JVE 19.09.2005 <begin>
                        'Spalte Finanzierungsart füllen...
                        rowFahrzeug("ZZFINART") = rowFahrzeugSAP.Zzfinart
                        '§§§JVE 19.09.2005 <end>
                        '§§§JVE 19.10.2005
                        rowFahrzeug("AUGRU") = String.Empty
                        rowFahrzeug("AUGRU_Klartext") = String.Empty

                        If Not rowFahrzeugSAP.Zzlaufzeit = "000" Or _
                            Not rowFahrzeugSAP.Zzabmdat = "00000000" Then
                            rowFahrzeug("ZZBOOLAUFZEIT") = True
                        Else
                            rowFahrzeug("ZZBOOLAUFZEIT") = False
                        End If
                        rowFahrzeug("KOPFTEXT") = String.Empty
                        rowFahrzeug("POSITIONSTEXT") = String.Empty
                        m_tblFahrzeuge.Rows.Add(rowFahrzeug)
                    Next

                    Dim col As DataColumn
                    For Each col In m_tblFahrzeuge.Columns
                        col.ReadOnly = False
                    Next

                    WriteLogEntry(True, "CHASSIS_NUM=" & m_strSucheFahrgestellNr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ", LIZNR=" & m_strSucheVertragsNr & ", ZZREFERENZ1=" & m_strSucheOrderNr, m_tblFahrzeuge)
                End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -2501
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -2502
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If

                WriteLogEntry(False, "CHASSIS_NUM=" & m_strSucheFahrgestellNr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ", LIZNR=" & m_strSucheVertragsNr & ", ZZREFERENZ1=" & m_strSucheOrderNr & " , " & Replace(m_strMessage, "<br>", " "), m_tblFahrzeuge)
            Finally
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub getZulStelle(ByRef page As Web.UI.Page, ByVal plz As String, ByVal ort As String, ByRef status As String)

        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_PLZ", plz)
            myProxy.setImportParameter("I_ORT", ort)

            myProxy.callBapi()

            Dim table As DataTable = myProxy.getExportTable("T_ZULST")

            If (table.Rows.Count > 1) Then
                'Mehr als ein Eintrag gefunden! Darf nicht sein!
                status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
            End If

            m_tblZulStelle = table.Copy

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
            status = m_strMessage
            m_kbanr = ""

        End Try

    End Sub
    Private Sub getLaender(ByVal page As Page)
        m_intStatus = 0
        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Land_Plz_001", m_objApp, m_objUser, Page)

            myProxy.callBapi()

            m_tblLaender = myProxy.getExportTable("GT_WEB")


            m_tblLaender.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
            m_tblLaender.Columns.Add("FullDesc", System.Type.GetType("System.String"))
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
    Public Sub Anfordern(Optional ByVal hez As Boolean = False)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim SAPTable As New SAPProxy_FFE.ZDAD_BRIEFANFORDERUNG_S001()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""
                m_strAuftragsnummer = ""
                m_strAuftragsstatus = ""

                Dim strAuftragsnummer As String = ""
                Dim strAuftragsstatus As String = ""

                If CheckCustomerData() Then
                    Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("ZZFAHRG = '" & m_strZZFAHRG & "'")

                    m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefanforderung", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    If m_bNewAdress = True Then
                        m_strAdresse = String.Empty
                    End If

                    'wenn Kontingentart Retail(KKBER=0003 oder Delayed Payment(KKBER=0004) ausgewählt dann Abrufgrund mit 126/127 füllen JJU2008.08.21
                    If m_strKreditkontrollBereich = "0003" Then
                        rowFahrzeug(0)("Augru") = "127"
                    ElseIf m_strKreditkontrollBereich = "0004" Then
                        rowFahrzeug(0)("Augru") = "126"
                    End If


                    With SAPTable
                        .Anfnr = rowFahrzeug(0)("TEXT300").ToString
                        .Augru = rowFahrzeug(0)("Augru").ToString
                        .City1 = m_strCity
                        .Datum = MakeDateSAP(zuldatum).ToString
                        .Equnr = rowFahrzeug(0)("EQUNR").ToString
                        .Ernam = m_strInternetUser
                        If hez Then
                            .Hezkz = "X"
                        Else
                            .Hezkz = ""
                        End If
                        .Chassis_Num = m_strZZFAHRG
                        .House_Num1 = m_strHouseNum
                        .Kdgrp = "X" 'bedeutet das die Briefanforderung nicht gesperrt angelegt wird, ITA 2199 JJU 2008.08.22
                        .Kunnr = Right("0000000000" & m_strCustomer, 10)
                        .Konzs = Right("0000000000" & m_strKUNNR, 10)
                        .Kunnr_Ze = ""
                        If hez Then
                            .Kunnr_Zh = m_strAdresseHalter
                        Else
                            .Kunnr_Zh = ""
                        End If
                        .Kunnr_Zs = m_strAdresse
                        .Kvgr3 = ""
                        .License_Num = rowFahrzeug(0)("LICENSE_NUM").ToString
                        .Liznr = rowFahrzeug(0)("LIZNR").ToString
                        .Matnr = m_strMaterialNummer
                        .Name1 = m_strName1
                        .Name2 = m_strName2
                        .Name3 = m_strName3
                        .Post_Code1 = m_strPostcode
                        .Preis = 0
                        .Street = m_strStreet
                        .Text50 = rowFahrzeug(0)("TEXT50").ToString
                        .Tidnr = rowFahrzeug(0)("TIDNR").ToString
                        .Txt_Kopf = rowFahrzeug(0)("KOPFTEXT").ToString
                        .Txt_Pos = rowFahrzeug(0)("POSITIONSTEXT").ToString
                        .Zulst = m_kbanr
                        .Zzdien1 = ""
                        .Datum = MakeDateSAP(Today.ToShortDateString)
                        .Zzkkber = m_strKreditkontrollBereich
                        .Zzlabel = rowFahrzeug(0)("ZZLABEL").ToString
                        .Zzlaufzeit = rowFahrzeug(0)("ZZLAUFZEIT").ToString
                    End With

                    objSAP.Z_M_Briefanforderung_Fce(SAPTable, strAuftragsstatus, strAuftragsnummer)

                    objSAP.CommitWork()

                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    End If

                    m_strAuftragsnummer = strAuftragsnummer.TrimStart("0"c)
                    Select Case UCase(strAuftragsstatus)
                        Case ""
                            m_strAuftragsstatus = "Kreditprüfung nicht durchgeführt"
                        Case "A"
                            m_strAuftragsstatus = "Vorgang OK"
                        Case "B"
                            m_strAuftragsstatus = "In Bearbeitung " & m_objUser.CustomerName
                        Case "C"
                            m_strAuftragsstatus = "In Bearbeitung " & m_objUser.CustomerName
                        Case "D"
                            m_strAuftragsstatus = "Freigegeben"
                        Case Else
                            m_strAuftragsstatus = "Unbekannt"
                    End Select

                    If m_strAuftragsnummer.Length = 0 Then
                        m_intStatus = -2100
                        m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    End If
                End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        m_strMessage = "System ausgelastet. Bitte klicken Sie erneut auf ""Absenden""."
                        m_intStatus = -1111
                    Case "NO_UPDATE_EQUI"
                        m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                        m_intStatus = -1112
                    Case "NO_AUFTRAG"
                        m_strMessage = "Kein Auftrag angelegt"
                        m_intStatus = -1113
                    Case "NO_ZDADVERSAND"
                        m_strMessage = "Keine Einträge für die Versanddatei erstellt"
                        m_intStatus = -1114
                    Case "NO_MODIFY_ILOA"
                        m_strMessage = "ILOA-MODIFY-Fehler"
                        m_intStatus = -1115
                    Case "NO_BRIEFANFORDERUNG"
                        m_strMessage = "Brief bereits angefordert"
                        m_intStatus = -1116
                    Case "NO_EQUZ"
                        m_strMessage = "Kein Brief vorhanden (EQUZ)"
                        m_intStatus = -1117
                    Case "NO_ILOA"
                        m_strMessage = "Kein Brief vorhanden (ILOA)"
                        m_intStatus = -1118
                    Case Else
                        m_strMessage = ex.Message
                        m_intStatus = -9999
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.LogStandardIdentity = m_intStandardLogID
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Anfordern_Bank(Optional ByVal hez As Boolean = False)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim SAPTable As New SAPProxy_FFE.ZDAD_BRIEFANFORDERUNG_S001()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""
                m_strAuftragsnummer = ""
                m_strAuftragsstatus = ""

                Dim strAuftragsnummer As String = ""
                Dim strAuftragsstatus As String = ""
                If CheckCustomerData() Then
                    Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("ZZFAHRG = '" & m_strZZFAHRG & "'")

                    m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefanforderung_002", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    'wenn Kontingentart Retail(KKBER=0003 oder Delayed Payment(KKBER=0004) ausgewählt dann Abrufgrund mit 126/127 füllen JJU2008.08.21
                    If m_strKreditkontrollBereich = "0003" Then
                        rowFahrzeug(0)("Augru") = "127"
                    ElseIf m_strKreditkontrollBereich = "0004" Then
                        rowFahrzeug(0)("Augru") = "126"
                    End If


                    If m_bNewAdress = True Then
                        m_strAdresse = String.Empty
                    End If
                    With SAPTable
                        .Anfnr = rowFahrzeug(0)("TEXT300").ToString
                        .Augru = rowFahrzeug(0)("Augru").ToString
                        .City1 = m_strCity
                        .Datum = MakeDateSAP(zuldatum).ToString
                        .Equnr = rowFahrzeug(0)("EQUNR").ToString
                        .Ernam = m_strInternetUser
                        If hez Then
                            .Hezkz = "X"
                        Else
                            .Hezkz = ""
                        End If
                        .Chassis_Num = m_strZZFAHRG
                        .House_Num1 = m_strHouseNum
                        .Kdgrp = "" 'bedeutet das die Briefanforderung gesperrt angelegt wird, ITA 2199 JJU 2008.08.22
                        .Kunnr = Right("0000000000" & m_strCustomer, 10)
                        .Konzs = Right("0000000000" & m_strKUNNR, 10)
                        .Kunnr_Ze = ""
                        If hez Then
                            .Kunnr_Zh = m_strAdresseHalter
                        Else
                            .Kunnr_Zh = ""
                        End If
                        .Kunnr_Zs = m_strAdresse
                        .Kvgr3 = ""
                        .License_Num = rowFahrzeug(0)("LICENSE_NUM").ToString
                        .Liznr = rowFahrzeug(0)("LIZNR").ToString
                        .Matnr = m_strMaterialNummer
                        .Name1 = m_strName1
                        .Name2 = m_strName2
                        .Name3 = m_strName3
                        .Post_Code1 = m_strPostcode
                        .Preis = 0
                        .Street = m_strStreet
                        .Text50 = rowFahrzeug(0)("TEXT50").ToString
                        .Tidnr = rowFahrzeug(0)("TIDNR").ToString
                        .Txt_Kopf = rowFahrzeug(0)("KOPFTEXT").ToString
                        .Txt_Pos = rowFahrzeug(0)("POSITIONSTEXT").ToString
                        .Zulst = m_kbanr
                        .Zzdien1 = ""
                        .Datum = MakeDateSAP(Today.ToShortDateString)
                        .Zzkkber = m_strKreditkontrollBereich
                        .Zzlabel = rowFahrzeug(0)("ZZLABEL").ToString
                        .Zzlaufzeit = rowFahrzeug(0)("ZZLAUFZEIT").ToString
                    End With

                    objSAP.Z_M_Briefanforderung_Fce(SAPTable, strAuftragsstatus, strAuftragsnummer)
                Else
                End If
                objSAP.CommitWork()

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_strAuftragsnummer = strAuftragsnummer.TrimStart("0"c)
                Select Case UCase(strAuftragsstatus)
                    Case ""
                        m_strAuftragsstatus = "Kreditprüfung nicht durchgeführt"
                    Case "A"
                        m_strAuftragsstatus = "Vorgang OK"
                    Case "B"
                        m_strAuftragsstatus = "In Bearbeitung Mazda"
                    Case "C"
                        m_strAuftragsstatus = "In Bearbeitung Mazda"
                    Case "D"
                        m_strAuftragsstatus = "Freigegeben"
                    Case Else
                        m_strAuftragsstatus = "Unbekannt"
                End Select

                If m_strAuftragsnummer.Length = 0 Then
                    m_intStatus = -2100
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        m_strMessage = "System ausgelastet. Bitte clicken Sie erneut auf ""Absenden""."
                        m_intStatus = -1111
                    Case "NO_UPDATE_EQUI"
                        m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                        m_intStatus = -1112
                    Case "NO_AUFTRAG"
                        m_strMessage = "Kein Auftrag angelegt"
                        m_intStatus = -1113
                    Case "NO_ZDADVERSAND"
                        m_strMessage = "Keine Einträge für die Versanddatei erstellt"
                        m_intStatus = -1114
                    Case "NO_MODIFY_ILOA"
                        m_strMessage = "ILOA-MODIFY-Fehler"
                        m_intStatus = -1115
                    Case "NO_BRIEFANFORDERUNG"
                        m_strMessage = "Brief bereits angefordert"
                        m_intStatus = -1116
                    Case "NO_EQUZ"
                        m_strMessage = "Kein Brief vorhanden (EQUZ)"
                        m_intStatus = -1117
                    Case "NO_ILOA"
                        m_strMessage = "Kein Brief vorhanden (ILOA)"
                        m_intStatus = -1118
                    Case "EQUI_SPERRE"
                        m_strMessage = "Vorgang in Bearbeitung"
                        m_intStatus = -1119
                    Case Else
                        m_strMessage = ex.Message
                        m_intStatus = -9999
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.LogStandardIdentity = m_intStandardLogID
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
    Public Sub ErsetzeAbrufgruende()
        Dim tempRow As DataRow
        Dim rows() As DataRow
        For Each tempRow In m_tblFahrzeuge.Rows
            rows = Abrufgruende.Select("SapWert='" & CStr(tempRow("AUGRU")) & "'")
            If rows.Length > 0 Then
                tempRow("AUGRU_Klartext") = CStr(rows(0)("WebBezeichnung"))
            End If
        Next
    End Sub
#End Region
End Class
' ************************************************
' $History: FFE_Haendler.vb $
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 9.03.10    Time: 9:22
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA: 2918
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 1.07.09    Time: 14:19
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2918
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 22.08.08   Time: 13:46
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2199 fertig
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 21.08.08   Time: 11:49
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2138 fertig
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 18.06.08   Time: 17:35
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 6.06.08    Time: 9:21
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 15.05.08   Time: 16:59
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA:1865
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.05.08   Time: 16:41
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 5.05.08    Time: 17:09
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Applications/AppFFE/lib
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.04.08    Time: 13:32
' Created in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 9.04.08    Time: 13:06
' Updated in $/CKG/Applications/AppFFE/AppFFEWeb/lib
' ITA: 1790
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 3.04.08    Time: 11:19
' Created in $/CKG/Applications/AppFFE/AppFFEWeb/lib
' ITA 1790
' 
' ************************************************