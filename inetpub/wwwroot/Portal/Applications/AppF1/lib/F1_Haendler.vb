Option Explicit On 
Option Strict On

Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports System.Configuration
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class F1_Haendler
    Inherits F1_BankBase

#Region " Declarations"
    Private m_tblAbrufgruende As DataTable
    Private m_strSucheVertragsNr As String
    Private m_strSucheOrderNr As String
    Private m_strSucheBriefNr As String
    Private m_strSucheFahrgestellNr As String
    Private m_strSucheHaendlernummer As String
    Private mMassenAnforderung As DataTable
    Private mPageIndex As Integer


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
    Private m_Dienstleister As DataTable
    Private m_DienstleisterDetail As String
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
                                                    "[AbrufTyp] , " & _
                                                    "[Eingeschraenkt] " & _
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

    Public Property MassenAnforderung() As DataTable
        Get
            Return mMassenAnforderung
        End Get
        Set(ByVal Value As DataTable)
            If Not Value Is Nothing Then
                If Not Value.Rows.Count = 0 Then
                    Value.Columns(0).ColumnName = "Fahrgestellnummer"
                    Value.AcceptChanges()
                    mMassenAnforderung = Value
                End If
            Else
                mMassenAnforderung = Nothing
            End If

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
                m_strAdresse = Right(Value, 10)
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


    Public Property SucheHaendlernummer() As String
        Get
            Return m_strSucheHaendlernummer
        End Get
        Set(ByVal Value As String)
            m_strSucheHaendlernummer = Value
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
            If Value Is Nothing Then
                m_strSucheBriefNr = ""
            Else
                m_strSucheBriefNr = Value
            End If

        End Set
    End Property

    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellNr
        End Get
        Set(ByVal Value As String)
            If Value Is Nothing Then
                m_strSucheFahrgestellNr = ""
            Else
                m_strSucheFahrgestellNr = Value
            End If

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
            Return m_tblLaender
        End Get
    End Property


    Public Property Dienstleister() As DataTable
        Get
            Return m_Dienstleister
        End Get
        Set(ByVal Value As DataTable)
            m_Dienstleister = Value
        End Set
    End Property
    Public Property DienstleisterDetail() As String
        Get
            Return m_DienstleisterDetail
        End Get
        Set(ByVal Value As String)
            m_DienstleisterDetail = Value
        End Set
    End Property

    Public Property PageIndex() As Integer
        Get
            Return mPageIndex
        End Get
        Set(ByVal Value As Integer)
            mPageIndex = Value
        End Set
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
        m_strSucheBriefNr = ""
        m_hez = hez
        m_ZzLaufzeit = ""
    End Sub

    Public Function isHEZ() As Boolean
        Return m_hez
    End Function

    Public Sub GiveCars(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: GiveCars
        ' Autor: JJU
        ' Beschreibung: gibt die anforderbaren briefe zurück (briefanforderung)
        ' Erstellt am: 04.03.2009
        ' ITA: 2661
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unangefordert_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER_EX", m_strSucheHaendlernummer)
            'myProxy.setImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr.ToUpper)
            'myProxy.setImportParameter("I_ZB2", m_strSucheBriefNr.ToUpper)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Unangefordert_STD", "I_AG,I_HAENDLER_EX,I_CHASSIS_NUM,I_ZB2",
                             m_objUser.KUNNR.PadLeft(10, CChar("0")), m_strSucheHaendlernummer, m_strSucheFahrgestellNr.ToUpper, m_strSucheBriefNr.ToUpper)

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
                .Add("KOPFTEXT", System.Type.GetType("System.String"))
                .Add("POSITIONSTEXT", System.Type.GetType("System.String"))
                .Add("ZZFINART", System.Type.GetType("System.String"))
                .Add("ZZLAUFZEIT", System.Type.GetType("System.String"))
                .Add("ZZBOOLAUFZEIT", System.Type.GetType("System.Boolean"))
                .Add("TEXT300", System.Type.GetType("System.String"))
                .Add("AUGRU", System.Type.GetType("System.String"))
                .Add("AUGRU_Klartext", System.Type.GetType("System.String"))
            End With


            Dim rowFahrzeugSAP As DataRow
            For Each rowFahrzeugSAP In S.AP.GetExportTable("GT_WEB").Rows 'myProxy.getExportTable("GT_WEB").Rows

                Dim rowFahrzeug As DataRow
                rowFahrzeug = m_tblFahrzeuge.NewRow
                rowFahrzeug("MANDT") = "0" 'nicht anfordern

                'alle Fahrzeuge die nicht im excelupload vorhanden sind, nicht hinzufügen
                If Not MassenAnforderung Is Nothing Then
                    If MassenAnforderung.Select("Fahrgestellnummer='" & rowFahrzeugSAP("Chassis_Num").ToString & "'").Count = 0 Then
                        GoTo NextRow
                    End If
                    rowFahrzeug("MANDT") = "2" 'endgültig anfordern
                End If


                rowFahrzeug("ZZFAHRG") = rowFahrzeugSAP("Chassis_Num")

                rowFahrzeug("EQUNR") = rowFahrzeugSAP("Equnr")
                rowFahrzeug("LIZNR") = rowFahrzeugSAP("Liznr")
                rowFahrzeug("TIDNR") = rowFahrzeugSAP("Tidnr")
                rowFahrzeug("LICENSE_NUM") = rowFahrzeugSAP("License_Num")
                rowFahrzeug("ZZREFERENZ1") = rowFahrzeugSAP("Zzreferenz1")
                rowFahrzeug("ZZLABEL") = rowFahrzeugSAP("Zzlabel")

                If UCase(rowFahrzeugSAP("Zzcockz").ToString) = "X" Then
                    rowFahrzeug("ZZCOCKZ") = True
                Else
                    rowFahrzeug("ZZCOCKZ") = False
                End If

                If UCase(rowFahrzeugSAP("Zzbezahlt").ToString) = "X" Then
                    rowFahrzeug("ZZBEZAHLT") = True
                Else
                    rowFahrzeug("ZZBEZAHLT") = False
                End If

                rowFahrzeug("VBELN") = ""
                rowFahrzeug("COMMENT") = ""
                rowFahrzeug("ZZFINART") = rowFahrzeugSAP("Zzfinart")
                rowFahrzeug("AUGRU") = String.Empty
                rowFahrzeug("AUGRU_Klartext") = String.Empty

                If Not rowFahrzeugSAP("Zzlaufzeit").ToString = "000" Or _
                    Not rowFahrzeugSAP("Zzabmdat").ToString = "00000000" Then
                    rowFahrzeug("ZZBOOLAUFZEIT") = True
                Else
                    rowFahrzeug("ZZBOOLAUFZEIT") = False
                End If

                rowFahrzeug("KOPFTEXT") = String.Empty
                rowFahrzeug("POSITIONSTEXT") = String.Empty

                m_tblFahrzeuge.Rows.Add(rowFahrzeug)
NextRow:
            Next

            Dim col As DataColumn
            For Each col In m_tblFahrzeuge.Columns
                col.ReadOnly = False
            Next

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
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
        End Try

    End Sub

    Public Sub getZulStelle(ByVal plz As String, ByVal ort As String, ByRef status As String, ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: getZulStelle
        ' Autor: JJU
        ' Beschreibung: gibt die zulassungsstellen anhand von parameter plz, ort 
        ' Erstellt am: 06.03.2009
        ' ITA: 2661
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_PLZ", plz)
            'myProxy.setImportParameter("I_ORT", ort)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_Get_Zulst_By_Plz", "I_PLZ,I_ORT", plz, ort)

            'If (myProxy.getExportTable("T_ZULST").Rows.Count > 1) Then
            If (S.AP.GetExportTable("T_ZULST").Rows.Count > 1) Then
                status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            m_tblZulStelle = S.AP.GetExportTable("T_ZULST") 'myProxy.getExportTable("T_ZULST")

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
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

    Public Sub getLaender(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: getLaender
        ' Autor: JJU
        ' Beschreibung: gibt alle Länder zurück
        ' Erstellt am: 06.03.2009
        ' ITA: 2661
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Land_Plz_001", m_objApp, m_objUser, page)

            'myProxy.callBapi()

            m_tblLaender = S.AP.GetExportTableWithInitExecute("Z_M_Land_Plz_001.GT_WEB", "")

            'm_tblLaender = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")


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
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        End Try

    End Sub

    Public Sub Anfordern(ByVal strAppID As String, ByVal strSessionID As String)

        '----------------------------------------------------------------------
        ' Methode: Anfordern
        ' Autor: JJU
        ' Beschreibung: forder briefe für händler und bank an
        ' Erstellt am: 09.03.2009
        ' ITA: 2664
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEFANFORDERUNG_STD", m_objApp, m_objUser, page)

            'Dim sapimporttable = myProxy.getImportTable("IMP")

            Dim sapimporttable As DataTable = S.AP.GetImportTableWithInit("Z_M_BRIEFANFORDERUNG_STD.IMP")

            m_intStatus = 0
            m_strMessage = ""
            m_strAuftragsnummer = ""
            m_strAuftragsstatus = ""

            Dim strAuftragsnummer As String = ""
            Dim strAuftragsstatus As String = ""

            If CheckCustomerData() Then
                Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("ZZFAHRG = '" & m_strZZFAHRG & "'")
                Dim newSapRow As DataRow = sapimporttable.NewRow

                If m_bNewAdress = True Then
                    m_strAdresse = String.Empty
                End If

                If String.IsNullOrEmpty(ZLST) = False Then
                    If ZLST = "X" Then
                        'myProxy.setImportParameter("I_ZULST", ZLST)
                        S.AP.SetImportParameter("I_ZULST", ZLST)
                    End If
                End If


                newSapRow("AG") = m_objUser.KUNNR.PadLeft(10, CChar("0"))
                newSapRow("HAENDLER_EX") = Right(m_strCustomer, 10)


                If Not m_strAdresse.Trim(" "c).Length = 0 Then
                    newSapRow("Kunnr_Zs") = Right("0000000000" & m_strAdresse, 10)
                End If

                newSapRow("Equnr") = rowFahrzeug(0)("EQUNR").ToString
                newSapRow("Ernam") = Right(m_strInternetUser, 12)
                newSapRow("Zzlabel") = rowFahrzeug(0)("ZZLABEL").ToString
                newSapRow("Zzkkber") = m_strKreditkontrollBereich
                newSapRow("Matnr") = m_strMaterialNummer
                newSapRow("Text50") = rowFahrzeug(0)("TEXT50").ToString
                newSapRow("Preis") = 0
                'Kunnr_ZH?
                newSapRow("Kunnr_Ze") = ""
                newSapRow("Datum") = Today
                newSapRow("Zulst") = m_kbanr
                newSapRow("VERSANDWEG") = DienstleisterDetail
                newSapRow("Txt_Kopf") = rowFahrzeug(0)("KOPFTEXT").ToString
                newSapRow("Txt_Pos") = rowFahrzeug(0)("POSITIONSTEXT").ToString

                If m_objUser.Reference.Trim(" "c).Length = 0 Then
                    newSapRow("Kdgrp") = "" 'bedeutet das die Briefanforderung gesperrt angelegt wird, ITA 2199 JJU 2008.08.22
                Else
                    newSapRow("Kdgrp") = "X" 'bedeutet das die Briefanforderung nicht gesperrt angelegt wird, ITA 2199 JJU 2008.08.22
                End If


                newSapRow("Zzdien1") = ""
                newSapRow("Augru") = rowFahrzeug(0)("Augru").ToString
                newSapRow("Zzlaufzeit") = rowFahrzeug(0)("ZZLAUFZEIT").ToString
                newSapRow("Anfnr") = rowFahrzeug(0)("TEXT300").ToString
                newSapRow("Name1") = m_strName1
                newSapRow("Name2") = m_strName2
                newSapRow("Name3") = m_strName3
                newSapRow("Street") = m_strStreet
                newSapRow("House_Num1") = m_strHouseNum
                newSapRow("Post_Code1") = m_strPostcode
                newSapRow("City1") = m_strCity

                sapimporttable.Rows.Add(newSapRow)

                'myProxy.callBapi()
                S.AP.Execute()

                m_strAuftragsnummer = S.AP.GetExportParameter("E_VBELN").TrimStart("0"c) 'myProxy.getExportParameter("E_VBELN").TrimStart("0"c)
                strAuftragsstatus = S.AP.GetExportParameter("E_CMGST").TrimStart("0"c) 'myProxy.getExportParameter("E_CMGST").TrimStart("0"c)
                m_strAuftragsstatus = S.AP.GetExportParameter("E_CMGST").TrimStart("0"c) 'myProxy.getExportParameter("E_CMGST").TrimStart("0"c)

                Select Case UCase(strAuftragsstatus)
                    Case ""
                        m_strAuftragsstatus = "Kreditprüfung nicht durchgeführt"
                    Case "A"
                        m_strAuftragsstatus = "Vorgang OK"
                    Case "B"
                        m_strAuftragsstatus = "Zahlung offen"
                    Case "C"
                        m_strAuftragsstatus = "Zahlung offen"
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
            Select Case ex.Message.Replace("Execution failed", "").Trim()
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
                Case "NO_ANF_ABRUFGRUND"
                    m_strMessage = "Abrufgrund ungültig."
                    m_intStatus = -1119
                Case Else
                    m_strMessage = ex.Message
                    m_intStatus = -9999
            End Select
        End Try
    End Sub

    Public Sub GetDienstleister(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: getZulStelle
        ' Autor: JJU
        ' Beschreibung: gibt die zulassungsstellen anhand von parameter plz, ort 
        ' Erstellt am: 06.03.2009
        ' ITA: 2661
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_VERSWEGE_01", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            S.AP.Init("Z_DPM_READ_VERSWEGE_01", "I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, CChar("0")))

            If m_objUser.Reference.Length > 0 Then
                'myProxy.setImportParameter("I_KUNNR_ZF", m_objUser.Reference)
                S.AP.SetImportParameter("I_KUNNR_ZF", m_objUser.Reference)
            End If

            'myProxy.callBapi()
            S.AP.Execute()

            Dienstleister = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

        Catch ex As Exception

        End Try
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

    Public Function GeteingeschraenkteAbrufgruende(ByVal SapWert As String) As Integer
        Dim rows() As DataRow

        rows = Abrufgruende.Select("SapWert='" & SapWert & "'")
        If rows.Length > 0 Then
            If Not rows(0)("Eingeschraenkt") Is DBNull.Value Then
                Return CInt(rows(0)("Eingeschraenkt"))
            Else
                Return 0
            End If

        Else
            Return 0
        End If
    End Function

#End Region

End Class
' ************************************************
' $History: F1_Haendler.vb $
' 
' *****************  Version 17  *****************
' User: Fassbenders  Date: 4.01.11    Time: 14:33
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 6.10.10    Time: 14:20
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 12.03.10   Time: 16:36
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 16.11.09   Time: 15:47
' Updated in $/CKAG/Applications/AppF1/lib
' ITA:3298
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 28.04.09   Time: 13:18
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2823 testfertig
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 20.04.09   Time: 16:04
' Updated in $/CKAG/Applications/AppF1/lib
' bugfix briefanforderung country entfernt
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 16.04.09   Time: 13:38
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 9.04.09    Time: 14:45
' Updated in $/CKAG/Applications/AppF1/lib
' Nachbesserungen dokumentenversand
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 30.03.09   Time: 9:10
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2664 nachbesserungen
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 26.03.09   Time: 8:16
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2741 unfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 25.03.09   Time: 17:35
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2741 
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 20.03.09   Time: 12:34
' Updated in $/CKAG/Applications/AppF1/lib
' auskommentierungen entfernt
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 9.03.09    Time: 14:51
' Updated in $/CKAG/Applications/AppF1/lib
' 2664 testfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.03.09    Time: 15:25
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2664 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.03.09    Time: 15:52
' Updated in $/CKAG/Applications/AppF1/lib
' ita 2664
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.03.09    Time: 17:30
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 4.03.09    Time: 11:12
' Created in $/CKAG/Applications/AppF1/lib
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