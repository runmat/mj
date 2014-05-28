Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Briefanforderung
    REM § Lese-/Schreibfunktion, Kunde: BPLG, 
    REM § GiveCars - BAPI: Z_M_Unangefordert_002,
    REM § Anfordern - BAPI: Z_M_Briefanforderung_002.

    Inherits BankBaseCredit

#Region " Declarations"

    Private m_tblAbrufgruende As DataTable
    Private m_strSucheTIDNR As String = ""
    Private m_strSucheZZREFERENZ1 As String = ""
    Private m_strSucheFahrgestellNr As String = String.Empty
    Private m_strName1 As String
    Private m_strName2 As String
    Private m_strName3 As String
    Private m_strCity As String
    Private m_strPostcode As String
    Private m_strStreet As String
    Private m_strHouseNum As String
    Private m_tblFahrzeuge As DataTable
    Private m_tblZulStelle As DataTable
    Private m_strZZFAHRG As String
    Private mVersandAdressValue As String
    Private mVersandAdressText As String
    Private m_strAuftragsnummer As String
    Private m_strAuftragsstatus As String
    Private m_strAuftragsstatus2 As String
    Private m_strKreditkontrollBereich As String
    Private m_strMaterialNummer As String 'Versandart
    Private mTextZumMaterial As String 'Beschreibungs Text der Versandart
    Private m_preis As Decimal 'HEZ
    Private m_kbanr As String 'Zulassungsdienst-Nummer
    Private m_strLIZNR As String = ""
    Private m_tblLaender As DataTable
    Private m_strLaenderKuerzel As String
    Private mVersandAdressen As DataTable
    Private mEndkundennummer As String
    Private mEndkundenAdresse As String
    Private mEndkundenFullName As String
    Private mVersandKuerzel As String

#End Region

#Region " Properties"

    Public ReadOnly Property Abrufgruende() As DataTable
        Get
            If m_tblAbrufgruende Is Nothing Then
                Dim cn As SqlClient.SqlConnection
                Dim cmdAg As SqlClient.SqlCommand
                Dim dsAg As DataSet
                Dim adAg As SqlClient.SqlDataAdapter
                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
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
                                                    " AND GroupID = " & m_objUser.GroupID.ToString _
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

    Public Property kbanr() As String
        Get
            Return m_kbanr
        End Get
        Set(ByVal Value As String)
            m_kbanr = Value
        End Set
    End Property

    Public Property VersandKuerzel() As String
        Get
            Return mVersandKuerzel
        End Get
        Set(ByVal Value As String)
            mVersandKuerzel = Value
        End Set
    End Property

    Public Property MaterialText() As String
        Get
            Return mTextZumMaterial
        End Get
        Set(ByVal value As String)
            mTextZumMaterial = value
        End Set
    End Property

    Public ReadOnly Property Endkundennummer() As String
        Get
            Return mEndkundennummer
        End Get
    End Property

    Public ReadOnly Property EndkundenAdresse() As String
        Get
            Return mEndkundenAdresse
        End Get
    End Property

    Public ReadOnly Property EndkundeFullName() As String
        Get
            Return mEndkundenFullName
        End Get
    End Property

    Public Property laenderKuerzel() As String
        Get
            Return m_strLaenderKuerzel
        End Get
        Set(ByVal Value As String)
            m_strLaenderKuerzel = Value
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

    Public Property sucheLIZNR() As String
        Get
            Return m_strLIZNR
        End Get
        Set(ByVal Value As String)
            If Not m_strLIZNR Is Nothing Then
                m_strLIZNR = Value.Replace(" ", "")
            Else
                m_strLIZNR = ""
            End If

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

    Public ReadOnly Property Auftragsstatus2() As String
        Get
            Return m_strAuftragsstatus2
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

    Public Property VersandAdressValue() As String
        Get
            Return mVersandAdressValue
        End Get
        Set(ByVal Value As String)
            mVersandAdressValue = Value
        End Set
    End Property

    Public Property VersandAdressText() As String
        Get
            Return mVersandAdressText
        End Get
        Set(ByVal Value As String)
            mVersandAdressText = Value
        End Set
    End Property

    Public Property SucheTIDNR() As String
        Get
            Return m_strSucheTIDNR
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                m_strSucheTIDNR = Value.Replace(" ", "")
            Else
                m_strSucheTIDNR = ""
            End If

        End Set
    End Property

    Public Property SucheZZREFERENZ1() As String
        Get
            Return m_strSucheZZREFERENZ1
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                m_strSucheZZREFERENZ1 = Value.Replace(" ", "")
            Else
                m_strSucheZZREFERENZ1 = ""
            End If
        End Set
    End Property

    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellNr
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                m_strSucheFahrgestellNr = Value.Replace(" ", "")
            Else
                m_strSucheFahrgestellNr = ""
            End If
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

    Public Property Versandadressen() As DataTable
        Get
            Return mVersandAdressen
        End Get
        Set(ByVal Value As DataTable)
            mVersandAdressen = Value
        End Set
    End Property

    Public ReadOnly Property Laender() As DataTable
        Get
            If m_tblLaender Is Nothing Then
                getLaender()
            End If
            Return m_tblLaender
        End Get
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, ByVal strCustomer As String, ByVal strKunnr As String, Optional ByVal strCreditControlArea As String = "ZDAD", Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        Customer = strCustomer
        CreditControlArea = strCreditControlArea
        m_intStatus = 0
        m_strMessage = ""
        m_strAuftragsnummer = ""
        m_strAuftragsstatus = ""
        m_strSucheZZREFERENZ1 = ""
        m_strSucheTIDNR = ""
        m_strSucheFahrgestellNr = ""
        m_hez = hez
        Show()
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

    Public Sub GiveCars()
        m_strClassAndMethod = "Briefanforderung.GiveCars"
        ClearError()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
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
                    .Add("ANFNR", System.Type.GetType("System.String"))
                    .Add("AUGRU", System.Type.GetType("System.String"))
                    .Add("AUGRU_Klartext", System.Type.GetType("System.String"))
                    'ZS=Endkundennummer/ZF=Händlernummer
                    .Add("EX_KUNNR_ZF", System.Type.GetType("System.String"))
                    .Add("EX_KUNNR_ZS", System.Type.GetType("System.String"))
                End With

                S.AP.Init("Z_M_Unangefordert_003")

                S.AP.SetImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_HAENDLER", "")
                S.AP.SetImportParameter("I_CHASSIS_NUM", Left(SucheFahrgestellNr, 30))
                S.AP.SetImportParameter("I_LIZNR", sucheLIZNR)
                S.AP.SetImportParameter("I_ZZREFERENZ1", m_strSucheZZREFERENZ1)
                S.AP.SetImportParameter("I_VKORG", "1510")
                S.AP.SetImportParameter("I_TIDNR", SucheTIDNR)

                S.AP.Execute()

                Dim tblFahrzeugeSap As DataTable = S.AP.GetExportTable("GT_WEB")
                mVersandAdressen = S.AP.GetExportTable("GT_PARTNER") 'versandadressen tabelle sichern

                For Each row As DataRow In tblFahrzeugeSap.Rows
                    Dim rowFahrzeug As DataRow

                    rowFahrzeug = m_tblFahrzeuge.NewRow
                    rowFahrzeug("ZZFAHRG") = row("Chassis_Num")
                    rowFahrzeug("MANDT") = "0"
                    rowFahrzeug("LIZNR") = row("Liznr")
                    rowFahrzeug("EQUNR") = row("Equnr")
                    rowFahrzeug("TIDNR") = row("Tidnr")
                    rowFahrzeug("Zzreferenz1") = row("Zzreferenz1")
                    rowFahrzeug("LICENSE_NUM") = row("License_Num")
                    rowFahrzeug("EX_KUNNR_ZF") = row("EX_KUNNR_ZF")
                    rowFahrzeug("EX_KUNNR_ZS") = row("EX_KUNNR_ZS")
                    If UCase(row("Zzcockz").ToString) = "X" Then
                        rowFahrzeug("ZZCOCKZ") = True
                    Else
                        rowFahrzeug("ZZCOCKZ") = False
                    End If
                    rowFahrzeug("VBELN") = ""
                    rowFahrzeug("COMMENT") = ""
                    rowFahrzeug("ZZFINART") = row("Zzfinart")
                    rowFahrzeug("KOPFTEXT") = ""
                    rowFahrzeug("POSITIONSTEXT") = ""

                    rowFahrzeug("text50") = ""
                    rowFahrzeug("AUGRU") = ""

                    m_tblFahrzeuge.Rows.Add(rowFahrzeug)
                    m_tblFahrzeuge.AcceptChanges()
                Next

                'Hier Immer Endkundennummer aus adressetabelle anzeigen!
                'endkunden aus adressetabelle herausziehen. 
                'ZS=Endkundennummer/ZF=Händlernummer
                Dim tmpRows() As DataRow
                tmpRows = mVersandAdressen.Select("ADDRTYP='ZS'")

                If tmpRows.Count = 0 Then
                    mEndkundennummer = ""
                    mEndkundenFullName = ""
                    mEndkundenAdresse = ""
                ElseIf tmpRows.Count = 1 Then
                    'Adressdaten für Kopfdaten befüllen befüllen
                    mEndkundennummer = tmpRows(0).Item("EX_KUNNR").ToString
                    mEndkundenFullName = tmpRows(0).Item("NAME1").ToString & " " & tmpRows(0).Item("NAME2").ToString
                    mEndkundenAdresse = tmpRows(0).Item("COUNTRY").ToString & " - " & tmpRows(0).Item("POST_CODE1").ToString & " " & tmpRows(0).Item("CITY1").ToString & _
                    "<br>" & tmpRows(0).Item("STREET").ToString & " " & tmpRows(0).Item("HOUSE_NUM1").ToString

                ElseIf tmpRows.Count > 1 Then
                    Throw New Exception("Es ist Mehr als ein Endkunde in der Adresstabelle vorhanden")
                End If

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -2501
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -2502
                        m_strMessage = "Händler nicht vorhanden."
                    Case "TEMP_VERSAND"
                        m_intStatus = -2503
                        m_strMessage = "Brief bereits Temporär versendet!"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

    Public Sub GetZulStelle(ByVal plz As String, ByVal ort As String, ByRef status As String)

        ClearError()
        Try

            S.AP.InitExecute("Z_GET_ZULST_BY_PLZ", "I_PLZ,I_ORT", plz, ort)

            Dim table As DataTable = S.AP.GetExportTable("T_ZULST") 'DirectCast(pET_ZULST.Value, DataTable)

            If (table.Rows.Count > 1) Then
                'Mehr als ein Eintrag gefunden! Darf nicht sein!
                status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            m_tblZulStelle = table.Copy

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                Case "ERR_INV_PLZ"
                    RaiseError("-1118", "Ungültige Postleitzahl.")
                Case Else
                    RaiseError("-9999", "Unbekannter Fehler.")
            End Select
            status = m_strMessage
            m_kbanr = ""

        End Try
    End Sub

    Private Sub getLaender()

        ClearError()

        Try

            S.AP.InitExecute("Z_M_Land_Plz_001")

            m_tblLaender = S.AP.GetExportTable("GT_WEB")

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
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
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

        ClearError()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.Init("Z_M_Briefanforderung_004")

                Dim VersandTabelle As DataTable = S.AP.GetImportTable("IMP")
                
                m_strAuftragsnummer = ""
                m_strAuftragsstatus = ""

                Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("ZZFAHRG = '" & m_strZZFAHRG & "'")

                'Hier wird selektiert welche Versandadresseingabe Ausgewählt wurde und Je nach dem die Attribute befüllt.
                Select Case mVersandKuerzel
                    Case "MAEI" 'Manuelle Eingabe
                        'die Attribute sind schon gefüllt
                        m_kbanr = ""
                    Case "ZUST" 'Zulassungsstelle
                        'die Adressattribute müssen geleert werden
                        m_strCity = ""
                        m_strHouseNum = ""
                        m_strLaenderKuerzel = ""
                        m_strName1 = ""
                        m_strName2 = ""
                        m_strPostcode = ""
                        m_strStreet = ""
                        m_strName3 = ""
                        m_kbanr = Right("0000000" & VersandAdressValue, 7)
                    Case "ZWST" 'Zweigstelle
                        m_kbanr = ""
                        'die Adressdaten aus Partnertabelle lesen
                        'ZS=Endkundennummer/ZF=Händlernummer

                        Dim tmpRows() As DataRow = mVersandAdressen.Select("EX_KUNNR='" & VersandAdressValue & "'")
                        If tmpRows.Count = 0 Then
                            Throw New Exception("Der VersandadressValue wurde in der Partnertabelle nicht gefunden: " & VersandAdressValue)
                        ElseIf tmpRows.Count = 1 Then
                            m_strName1 = tmpRows(0).Item("NAME1").ToString
                            m_strName2 = tmpRows(0).Item("NAME2").ToString
                            m_strCity = tmpRows(0).Item("CITY1").ToString
                            m_strHouseNum = tmpRows(0).Item("HOUSE_NUM1").ToString
                            m_strStreet = tmpRows(0).Item("STREET").ToString
                            m_strPostcode = tmpRows(0).Item("POST_CODE1").ToString
                            m_strLaenderKuerzel = tmpRows(0).Item("COUNTRY").ToString
                        ElseIf tmpRows.Count > 1 Then
                            m_strName1 = tmpRows(0).Item("NAME1").ToString
                            m_strName2 = tmpRows(0).Item("NAME2").ToString
                            m_strCity = tmpRows(0).Item("CITY1").ToString
                            m_strHouseNum = tmpRows(0).Item("HOUSE_NUM1").ToString
                            m_strStreet = tmpRows(0).Item("STREET").ToString
                            m_strPostcode = tmpRows(0).Item("POST_CODE1").ToString
                            m_strLaenderKuerzel = tmpRows(0).Item("COUNTRY").ToString
                        End If
                    Case Else
                        Throw New Exception("Das Versandkürzel ist unbekannt: " & mVersandKuerzel)
                End Select

                Dim tmpRow As DataRow = VersandTabelle.NewRow

                With tmpRow
                    .Item("Anfnr") = rowFahrzeug(0)("ANFNR").ToString
                    .Item("Augru") = rowFahrzeug(0)("AUGRU").ToString
                    .Item("City1") = m_strCity
                    .Item("Equnr") = rowFahrzeug(0)("EQUNR").ToString
                    If m_objUser.UserName.Length > 12 Then
                        .Item("Ernam") = Left(m_objUser.UserName, 12)
                    Else
                        .Item("Ernam") = m_objUser.UserName
                    End If
                    'autorisierungslevel der Gruppe schreiben, +1 weil in sap das feld nur von 1-4 geht
                    .Item("Kvgr3") = CStr(m_objUser.Groups.ItemByID(m_objUser.GroupID).Authorizationright + 1)
                    .Item("Chassis_Num") = m_strZZFAHRG
                    .Item("House_Num1") = m_strHouseNum
                    .Item("Konzs") = Right("0000000000" & m_strKUNNR, 10)
                    .Item("License_Num") = rowFahrzeug(0)("LICENSE_NUM").ToString
                    .Item("Liznr") = rowFahrzeug(0)("LIZNR").ToString
                    .Item("Matnr") = Right("000000000000000000" & m_strMaterialNummer, 18)
                    .Item("Name1") = m_strName1
                    .Item("Name2") = m_strName2
                    .Item("Name3") = m_strName3
                    .Item("Post_Code1") = m_strPostcode
                    .Item("Street") = m_strStreet
                    .Item("Text50") = rowFahrzeug(0)("TEXT50").ToString
                    .Item("Tidnr") = rowFahrzeug(0)("TIDNR").ToString
                    .Item("Txt_Kopf") = rowFahrzeug(0)("KOPFTEXT").ToString
                    .Item("Txt_Pos") = rowFahrzeug(0)("POSITIONSTEXT").ToString
                    .Item("Zulst") = m_kbanr
                    .Item("Zzkkber") = m_strKreditkontrollBereich
                    .Item("Zzlabel") = rowFahrzeug(0)("ZZLABEL").ToString
                    .Item("Zzlaufzeit") = rowFahrzeug(0)("ZZLAUFZEIT").ToString
                    .Item("Country") = laenderKuerzel
                End With

                VersandTabelle.Rows.Add(tmpRow)
                VersandTabelle.AcceptChanges()

                S.AP.Execute()

                m_strAuftragsnummer = S.AP.GetExportParameter("E_VBELN")
                m_strAuftragsstatus = S.AP.GetExportParameter("E_CMGST")

                Select Case UCase(m_strAuftragsstatus)
                    Case ""
                        m_strAuftragsstatus = "Kreditprüfung nicht durchgeführt"
                        m_strAuftragsstatus2 = m_strAuftragsstatus
                    Case "A"
                        m_strAuftragsstatus = "Vorgang OK"
                        m_strAuftragsstatus2 = m_strAuftragsstatus
                    Case "B"
                        m_strAuftragsstatus = "Freigabe erforderlich"
                        m_strAuftragsstatus2 = "in Bearbeitung Bank"
                    Case "C"
                        m_strAuftragsstatus = "Freigabe erforderlich"
                        m_strAuftragsstatus2 = "in Bearbeitung Bank"
                    Case "D"
                        m_strAuftragsstatus = "Freigegeben"
                        m_strAuftragsstatus2 = "in Bearbeitung Bank "
                    Case Else
                        m_strAuftragsstatus = "Unbekannt"
                        m_strAuftragsstatus2 = "Unbekannt"
                End Select

                If String.IsNullOrEmpty(m_strAuftragsnummer) Then
                    m_intStatus = -2100
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                End If
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        RaiseError("-1111", "System ausgelastet. Bitte clicken Sie erneut auf ""Absenden"".")
                    Case "NO_UPDATE_EQUI"
                        RaiseError("-1112", "Fehler bei der Datenspeicherung (EQUI-UPDATE)")
                    Case "NO_AUFTRAG"
                        RaiseError("-1113", "Kein Auftrag angelegt")
                    Case "NO_ZDADVERSAND"
                        RaiseError("-1114", "Keine Einträge für die Versanddatei erstellt")
                    Case "NO_MODIFY_ILOA"
                        RaiseError("-1115", "ILOA-MODIFY-Fehler")
                    Case "NO_BRIEFANFORDERUNG"
                        RaiseError("-1116", "ZB-II bereits angefordert")
                    Case "NO_EQUZ"
                        RaiseError("-1117", "Kein ZB-II vorhanden (EQUZ)")
                    Case "NO_ILOA"
                        RaiseError("-1118", "Kein Brief vorhanden (ILOA)")
                    Case "EQUI_SPERRE"
                        RaiseError("-1119", "Vorgang in Bearbeitung")
                    Case "ERR_INV_ZS_ABWADR"
                        RaiseError("-1119", "Fehlende Information zu abw. Adresse für Empfänger ZB-II!")
                    Case "NO_TEXTAENDERUNG"
                        RaiseError("-1120", "Fehler bei der Textänderung!")
                    Case "ERR_INV_KUNNR"
                        RaiseError("-1121", "Fehlende Information zu abw. Adresse für Händler!")
                    Case "ERR_INV_ZF"
                        RaiseError("-1122", "Ungültige Kundennummer für Händler!")
                    Case "ERR_INV_ZF_ABWADR"
                        RaiseError("-1123", "Kein Zulassungsdatum angegeben!")
                    Case "ERR_INV_ZH"
                        RaiseError("-1124", "Ungültige Kundennummer für Halter!")
                    Case "ERR_INV_ZH_ABWADR"
                        RaiseError("-1125", "Fehlende Information zu abw. Adresse für Halter!")
                    Case "ERR_INV_ZS"
                        RaiseError("-1126", "Ungültige Kundennummer für Empfänger ZB-II!")
                    Case "ERR_INV_ZS_ABWADR"
                        RaiseError("-1127", "Fehlende Information zu abw. Adresse für Empfänger ZB-II!")
                    Case "ERR_INV_ZS_ABWADR"
                        RaiseError("-1128", "Fehlende Information zu abw. Adresse für Empfänger ZB-II!")
                    Case "ERR_INV_ZE"
                        RaiseError("-1129", "Ungültige Kundennummer für Empänger Schein & Schilder!")
                    Case "ERR_INV_ZE_ABWADR"
                        RaiseError("-1130", "Fehlende Information zu abw. Adresse für Empfänger Schein & Schilder!")
                    Case "ERR_INV_ZA"
                        RaiseError("-1131", "Ungültige Kundennummer für Standort!")
                    Case "ERR_INV_ZA_ABWADR"
                        RaiseError("-1132", "Fehlende Information zu abw. Adresse für Standort!")
                    Case "ERR_SAVE"
                        RaiseError("-1133", "Fehler beim Speichern!")
                    Case "ERR_NO_ZF"
                        RaiseError("-1134", "Kein Händler angegeben!")
                    Case "ERR_NO_ZH"
                        RaiseError("-1135", "Kein Händler angegeben!")
                    Case "ERR_NO_ZS"
                        RaiseError("-1136", "Kein Empfänger Brief angegeben!")
                    Case "ERR_NO_ZE"
                        RaiseError("-1137", "Kein Empfänger Schein & Schilder angegeben!")
                    Case "ERR_NO_EQUI"
                        RaiseError("-1138", "Kein Equipment zu Fahrgestell-Nr. gefunden!")
                    Case "ERR_NO_RE"
                        RaiseError("-1139", "Kein Rechnungsempfänger angegeben!")
                    Case "ERR_NO_RG"
                        RaiseError("-1140", "Kein Regulierer angegeben!")
                    Case "ERR_INV_RE"
                        RaiseError("-1141", "Ungültiger Rechnungsempfänger!")
                    Case "ERR_INV_RG"
                        RaiseError("-1142", "Ungültiger Regulierer!")
                    Case "HAENDLER_NOT_FOUND"
                        RaiseError("-1143", "Händler konnte nicht gefunden werden!")
                    Case "HAENDLER_NO_ZDADCREDITLIMIT"
                        RaiseError("-1144", "Händler hat keinen Eintrag in der ZDADCREDITLIMIT!")
                    Case "KUNDE_NICHT_ANGELEGT"
                        RaiseError("-1145", "Kunde konnte nicht angelegt werden!")
                    Case "AG_CUSTOMIZING_NICHT_ANGELEGT"
                        RaiseError("-1146", "AG_CUSTOMIZING_NICHT_ANGELEGT!")
                    Case "NO_CS_MESSAGE"
                        RaiseError("-1147", "CS-Meldung konnt nicht angelegt werden!")
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: Briefanforderung.vb $
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:09
' Updated in $/CKAG/Applications/AppBPLG/Lib
' Warnungen
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 21.10.08   Time: 10:01
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2287,2244
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 24.09.08   Time: 16:31
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ergänzung sap logging
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 24.09.08   Time: 16:29
' Updated in $/CKAG/Applications/AppBPLG/Lib
' SAP Logging ergänzt
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 24.09.08   Time: 14:37
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA:2243
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 9.09.08    Time: 13:21
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2194  
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 11.08.08   Time: 17:54
' Updated in $/CKAG/Applications/AppBPLG/Lib
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 5.08.08    Time: 8:59
' Updated in $/CKAG/Applications/AppBPLG/Lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 29.07.08   Time: 14:29
' Updated in $/CKAG/Applications/AppBPLG/Lib
' BPLG Test Nachbesserungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 25.07.08   Time: 14:32
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2070 nachbesserungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 23.07.08   Time: 11:57
' Updated in $/CKAG/Applications/AppBPLG/Lib
' To Upper Funktion bei Import parametern entfernt ITA 2070
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.07.08   Time: 9:57
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2070 nachbesserungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.07.08   Time: 12:49
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2070
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.07.08   Time: 14:19
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2070 rohversion
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 18.07.08   Time: 12:51
' Created in $/CKAG/Applications/AppBPLG/Lib
' Klassen erstellt
' ************************************************