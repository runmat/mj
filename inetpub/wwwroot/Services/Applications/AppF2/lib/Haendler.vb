Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports System.Data.SqlClient
Imports CKG.Base.Common

Public Class Haendler
    Inherits AppF2BankBaseCredit

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
    Private m_strAdresse As String
    Private m_strAuftragsnummer As String
    Private m_strAuftragsstatus As String
    Private m_strAuftragsstatus2 As String
    Private m_strKreditkontrollBereich As String
    Private m_strMaterialNummer As String
    'Versandart
    Private m_strAdresseHalter As String
    'HEZ
    Private m_strAdresseEmpf As String
    'HEZ
    Private m_preis As Decimal
    'HEZ
    Private m_datumRange As DataTable
    'HEZ:Hier Datumswerte auslesen...
    Private m_kbanr As String
    'Zulassungsdienst-Nummer
    Private m_strLIZNR As String = ""
    Private m_boolNewAdress As Boolean = False
    Private m_tblLaender As DataTable
    Private m_strLaenderKuerzel As String
    Private mVersandArtText As String
    Private mVersandAdressArt As String
    Private mUploadTable As DataTable
    Private mE_SUBRC As String
    Private mE_MESSAGE As String
    Private m_boolUploadFehler As Boolean = False
    Private m_FlagVersand As String

#End Region

#Region " Properties"

    Public ReadOnly Property Abrufgruende() As DataTable
        Get
            m_tblAbrufgruende = Nothing
            Dim cn = New SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Try

                cn.Open()

                Dim dsAg = New DataSet()

                Dim adAg = New SqlDataAdapter()

                Dim cmdAg = New SqlCommand("SELECT " &
                                           "[WebBezeichnung]," &
                                           "[SapWert]," &
                                           "[MitZusatzText]," &
                                           "[Zusatzbemerkung], " &
                                           "[AbrufTyp], " &
                                           "[VersandAdressArt] " &
                                           "FROM CustomerAbrufgruende " &
                                           "WHERE " &
                                           "CustomerID = " & m_objUser.Customer.CustomerId.ToString &
                                           " AND GroupID = " & m_objUser.GroupID.ToString & " " _
                                           , cn)

                If Not mVersandAdressArt = Nothing Then
                    If mVersandAdressArt.Length > 0 Then
                        cmdAg.CommandText = cmdAg.CommandText & mVersandAdressArt
                    End If
                End If
                cmdAg.CommandType = CommandType.Text
                'AbrufTyp: 'temp' oder 'endg'

                adAg.SelectCommand = cmdAg
                adAg.Fill(dsAg, "Abrufgruende")

                If dsAg.Tables("Abrufgruende") Is Nothing OrElse dsAg.Tables("Abrufgruende").Rows.Count = 0 Then
                    Throw New Exception("Keine Abrufgründe für den Kunden hinterlegt.")
                End If

                m_tblAbrufgruende = dsAg.Tables("Abrufgruende")
            Finally
                cn.Close()
            End Try

            Return m_tblAbrufgruende
        End Get
    End Property

    Public Property kbanr() As String
        Get
            Return m_kbanr
        End Get
        Set(ByVal value As String)
            m_kbanr = value
        End Set
    End Property

    Public Property VersandArtText() As String
        Get
            Return mVersandArtText
        End Get
        Set(ByVal value As String)
            mVersandArtText = value
        End Set
    End Property

    Public Property VersandAdressArt() As String
        Get
            Return mVersandAdressArt
        End Get
        Set(ByVal value As String)
            mVersandAdressArt = value
        End Set
    End Property

    Public Property laenderKuerzel() As String
        Get
            Return m_strLaenderKuerzel
        End Get
        Set(ByVal value As String)
            m_strLaenderKuerzel = value
        End Set
    End Property

    Public Property neueAdresse() As Boolean
        Get
            Return m_boolNewAdress
        End Get
        Set(ByVal value As Boolean)
            m_boolNewAdress = value
        End Set
    End Property

    Public Property Name1() As String
        Get
            Return m_strName1
        End Get
        Set(ByVal value As String)
            m_strName1 = value
        End Set
    End Property

    Public Property Name2() As String
        Get
            Return m_strName2
        End Get
        Set(ByVal value As String)
            m_strName2 = value
        End Set
    End Property

    Public Property Name3() As String
        Get
            Return m_strName3
        End Get
        Set(ByVal value As String)
            m_strName3 = value
        End Set
    End Property

    Public Property City() As String
        Get
            Return m_strCity
        End Get
        Set(ByVal value As String)
            m_strCity = value
        End Set
    End Property

    Public Property PostCode() As String
        Get
            Return m_strPostcode
        End Get
        Set(ByVal value As String)
            m_strPostcode = value
        End Set
    End Property

    Public Property Street() As String
        Get
            Return m_strStreet
        End Get
        Set(ByVal value As String)
            m_strStreet = value
        End Set
    End Property

    Public Property HouseNum() As String
        Get
            Return m_strHouseNum
        End Get
        Set(ByVal value As String)
            m_strHouseNum = value
        End Set
    End Property

    Public Property preis() As Decimal
        Get
            Return m_preis
        End Get
        Set(ByVal value As Decimal)
            m_preis = value
        End Set
    End Property

    Public Property zuldatum() As Date
        Get
            Return m_zuldatum
        End Get
        Set(ByVal value As Date)
            m_zuldatum = value
        End Set
    End Property

    Public Property MaterialNummer() As String
        Get
            Return m_strMaterialNummer
        End Get
        Set(ByVal value As String)
            m_strMaterialNummer = value
        End Set
    End Property

    Public Property sucheLIZNR() As String
        Get
            Return m_strLIZNR
        End Get
        Set(ByVal value As String)
            If Not m_strLIZNR Is Nothing Then
                m_strLIZNR = value.Replace(" ", "").ToUpper
            Else
                m_strLIZNR = ""
            End If
        End Set
    End Property

    Public Property KreditkontrollBereich() As String
        Get
            Return m_strKreditkontrollBereich
        End Get
        Set(ByVal value As String)
            m_strKreditkontrollBereich = Right("0000" & value.Trim(" "c), 4)
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
        Set(ByVal value As String)
            m_strZZFAHRG = value
        End Set
    End Property

    Public Property Adresse() As String
        Get
            Return m_strAdresse
        End Get
        Set(ByVal value As String)
            m_strAdresse = Right("00000000000" & value, 10)
        End Set
    End Property

    Public Property AdresseHalter() As String
        Get
            Return m_strAdresseHalter
        End Get
        Set(ByVal value As String)
            m_strAdresseHalter = Right("00000000000" & value, 10)
        End Set
    End Property

    Public Property AdresseEmpf() As String
        Get
            Return m_strAdresseEmpf
        End Get
        Set(ByVal value As String)
            m_strAdresseEmpf = Right("00000000000" & value, 10)
        End Set
    End Property

    Public Property SucheTIDNR() As String
        Get
            Return m_strSucheTIDNR
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                m_strSucheTIDNR = value.Replace(" ", "").ToUpper
            Else
                m_strSucheTIDNR = ""
            End If
        End Set
    End Property

    Public Property SucheZZREFERENZ1() As String
        Get
            Return m_strSucheZZREFERENZ1
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                m_strSucheZZREFERENZ1 = value.Replace(" ", "").ToUpper
            Else
                m_strSucheZZREFERENZ1 = ""
            End If
        End Set
    End Property


    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellNr
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                m_strSucheFahrgestellNr = value.Replace(" ", "").ToUpper
            Else
                m_strSucheFahrgestellNr = ""
            End If
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal value As DataTable)
            m_tblFahrzeuge = value
        End Set
    End Property

    Public Property ZulStellen() As DataTable
        Get
            Return m_tblZulStelle
        End Get
        Set(ByVal value As DataTable)
            m_tblZulStelle = value
        End Set
    End Property

    Public ReadOnly Property Laender() As DataTable
        Get
            If m_tblLaender Is Nothing Then
                getLaender(AppID, SessionID, New Page)
            End If
            Return m_tblLaender
        End Get
    End Property

    Public Property UploadTable() As DataTable
        Get
            Return mUploadTable
        End Get
        Set(ByVal value As DataTable)
            mUploadTable = value
        End Set
    End Property

    Public Property E_SUBRC() As String
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal value As String)
            mE_SUBRC = value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal value As String)
            mE_MESSAGE = value
        End Set
    End Property

    Public Property boolUpload() As Boolean
        Get
            Return m_boolUploadFehler
        End Get
        Set(ByVal value As Boolean)
            m_boolUploadFehler = value
        End Set
    End Property

    Public Property FlagVersand() As String
        Get
            Return m_FlagVersand
        End Get
        Set(ByVal value As String)
            m_FlagVersand = value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As User, ByRef objApp As App, ByVal strAppID As String, ByVal strSessionID As String,
                   ByVal strFileName As String, ByVal strCustomer As String, ByVal strKunnr As String,
                   Optional ByVal strCreditControlArea As String = "ZDAD", Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        Customer = strCustomer
        KUNNR = strKunnr
        CreditControlArea = strCreditControlArea
        m_intStatus = 0
        m_strMessage = ""
        m_strAuftragsnummer = ""
        m_strAuftragsstatus = ""
        m_strSucheZZREFERENZ1 = ""
        m_strSucheTIDNR = ""
        m_strSucheFahrgestellNr = ""
        m_hez = hez
        Show(AppID, SessionID, New Page)
    End Sub

    Public Sub ErsetzeAbrufgruende()

        mVersandAdressArt = Nothing

        Dim tempRow As DataRow
        Dim rows() As DataRow
        For Each tempRow In m_tblFahrzeuge.Rows
            rows = Abrufgruende.Select("SapWert='" & CStr(tempRow("AUGRU")) & "'")
            If rows.Length > 0 Then
                tempRow("AUGRU_Klartext") = CStr(rows(0)("WebBezeichnung"))
            End If
        Next
    End Sub

    Public Function isHEZ() As Boolean
        Return m_hez
    End Function

    Public Function checkInputHEZ(ByVal zulDatum As String, ByRef message As String) As Boolean 'HEZ
        Dim datum As Date
        Dim row As DataRow()

        'Datum?
        If Not IsDate(zulDatum) Then
            message = "Falsches Datumsformat."
            Return False
        End If
        'Hier SAP-Datumsfelder durchhühnern...
        datum = CType(zulDatum, Date)
        zulDatum = MakeDateSAP(datum).ToString

        row = m_datumRange.Select("Low = '" & zulDatum & "'")

        If row.Length = 0 Then 'Kein Datum gefunden. Also ugültig.
            message = "Ungültiges Zulassungsdatum (" & MakeDateStandard(zulDatum) & ")"
            Return False
        End If

        Return True
    End Function


    Public Overloads Sub GiveCarsUpload(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_tblFahrzeuge = New DataTable()
        m_boolUploadFehler = False
        With m_tblFahrzeuge.Columns
            .Add("ZZFAHRG", GetType(String))
            .Add("MANDT", GetType(String))
            .Add("LIZNR", GetType(String))
            .Add("EQUNR", GetType(String))
            .Add("TIDNR", GetType(String))
            .Add("LICENSE_NUM", GetType(String))
            .Add("ZZREFERENZ1", GetType(String))
            .Add("ZZBEZAHLT", GetType(Boolean))
            .Add("ZZCOCKZ", GetType(Boolean))
            .Add("ZZLABEL", GetType(String))
            .Add("VBELN", GetType(String))
            .Add("COMMENT", GetType(String))
            .Add("TEXT50", GetType(String))
            .Add("TEXT200", GetType(String))
            .Add("KOPFTEXT", GetType(String))
            .Add("POSITIONSTEXT", GetType(String))
            .Add("ZZFINART", GetType(String))
            .Add("ZZLAUFZEIT", GetType(String))
            .Add("ANFNR", GetType(String))
            .Add("AUGRU", GetType(String))
            .Add("AUGRU_Klartext", GetType(String))
            .Add("ZZZLDAT", GetType(String))
            .Add("ZZFINART_BEZ", GetType(String))
            .Add("ZZFINART_GRP", GetType(String))
            .Add("MESSAGE", GetType(String))
        End With

        Try
            m_intStatus = 0
            m_strMessage = ""
            'weil bei bankbase führende nullen angehängt werden, verträgt das bapi aber nicht wenn ohne Händlernummer gesucht wird.
            Dim tmpCustomer As String
            If m_strCustomer = "0000000000" Then
                tmpCustomer = ""
            Else
                tmpCustomer = m_strCustomer
            End If

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unangefordert_004", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", m_strKUNNR)
            Dim SAPTable As DataTable = myProxy.getImportTable("SEL_LIZNR")
            Dim rowUpload As DataRow
            For Each rowUpload In mUploadTable.Rows
                Dim tmpSAPRow As DataRow = SAPTable.NewRow

                With tmpSAPRow
                    tmpSAPRow("LIZNR") = rowUpload("F1").ToString
                End With

                SAPTable.Rows.Add(tmpSAPRow)
                SAPTable.AcceptChanges()
            Next


            myProxy.callBapi()


            mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
            mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")
            Dim tblFahrzeugeSAP As DataTable = myProxy.getExportTable("GT_WEB")
            Dim rowFahrzeugSAP As DataRow


            For Each rowFahrzeugSAP In tblFahrzeugeSAP.Rows
                Dim rowFahrzeug As DataRow

                rowFahrzeug = m_tblFahrzeuge.NewRow
                rowFahrzeug("ZZFAHRG") = rowFahrzeugSAP("ZZFAHRG")
                rowFahrzeug("MANDT") = "0"
                rowFahrzeug("LIZNR") = rowFahrzeugSAP("Liznr")
                rowFahrzeug("EQUNR") = rowFahrzeugSAP("Equnr")
                rowFahrzeug("TIDNR") = rowFahrzeugSAP("Tidnr")
                rowFahrzeug("Zzreferenz1") = rowFahrzeugSAP("Zzreferenz1")
                rowFahrzeug("LICENSE_NUM") = rowFahrzeugSAP("License_Num")
                If UCase(rowFahrzeugSAP("Zzcockz").ToString) = "X" Then
                    rowFahrzeug("ZZCOCKZ") = True
                Else
                    rowFahrzeug("ZZCOCKZ") = False
                End If
                rowFahrzeug("VBELN") = ""
                rowFahrzeug("COMMENT") = ""
                rowFahrzeug("ZZFINART") = rowFahrzeugSAP("Zzfinart")
                rowFahrzeug("KOPFTEXT") = String.Empty
                rowFahrzeug("POSITIONSTEXT") = String.Empty

                rowFahrzeug("text50") = String.Empty
                rowFahrzeug("AUGRU") = String.Empty
                rowFahrzeug("ZZZLDAT") = rowFahrzeugSAP("ZZZLDAT")
                rowFahrzeug("MESSAGE") = rowFahrzeugSAP("MESSAGE")
                rowFahrzeug("ZZFINART_BEZ") = rowFahrzeugSAP("ZZFINART_BEZ")
                rowFahrzeug("ZZFINART_GRP") = rowFahrzeugSAP("ZZFINART_GRP")
                If rowFahrzeug("MESSAGE").ToString <> "" Then
                    m_boolUploadFehler = True
                End If

                m_tblFahrzeuge.Rows.Add(rowFahrzeug)
                m_tblFahrzeuge.AcceptChanges()
            Next


            Dim col As DataColumn
            For Each col In m_tblFahrzeuge.Columns
                col.ReadOnly = False
            Next

            'Händlernummer Speichern falls bankanforderung und selektion über andere Parameter
            'wenn mehrere Händlernummern falsche selektion im SAP
            Customer = tblFahrzeugeSAP.Rows(0)("Kunnr").ToString
            If mE_SUBRC <> "0" Then
                m_intStatus = CInt(mE_SUBRC)
                m_strMessage = mE_MESSAGE
            End If
            WriteLogEntry(True,
                          "CHASSIS_NUM=" & m_strSucheFahrgestellNr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) &
                          ", KNRZE=" & m_strFiliale & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strSucheTIDNR &
                          ", ZZREFERENZ1=" & m_strSucheZZREFERENZ1, m_tblFahrzeuge)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -2501
                    m_strMessage = "Es wurden keine Daten gefunden."
                Case "NO_HAENDLER"
                    m_intStatus = -2502
                    m_strMessage = "Händler nicht vorhanden."
                Case "TEMP_VERSAND"
                    m_intStatus = -2503
                    m_strMessage = "Brief bereits Temporär versendet!"
                Case "HIST_CHECK"
                    m_intStatus = -2504
                    m_strMessage = "Bitte in Historie prüfen."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select

            WriteLogEntry(False,
                          "CHASSIS_NUM=" & m_strSucheFahrgestellNr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) &
                          ", KNRZE=" & m_strFiliale & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strSucheTIDNR &
                          ", ZZREFERENZ1=" & m_strSucheZZREFERENZ1 & " , " & Replace(m_strMessage, "<br>", " "),
                          m_tblFahrzeuge)
        End Try
    End Sub

    Public Overloads Sub GiveCars(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_tblFahrzeuge = New DataTable()

        With m_tblFahrzeuge.Columns
            .Add("ZZFAHRG", Type.GetType("System.String"))
            .Add("MANDT", Type.GetType("System.String"))
            .Add("LIZNR", Type.GetType("System.String"))
            .Add("EQUNR", Type.GetType("System.String"))
            .Add("TIDNR", Type.GetType("System.String"))
            .Add("LICENSE_NUM", Type.GetType("System.String"))
            .Add("ZZREFERENZ1", Type.GetType("System.String"))
            .Add("ZZBEZAHLT", Type.GetType("System.Boolean"))
            .Add("ZZCOCKZ", Type.GetType("System.Boolean"))
            .Add("ZZLABEL", Type.GetType("System.String"))
            .Add("VBELN", Type.GetType("System.String"))
            .Add("COMMENT", Type.GetType("System.String"))
            .Add("TEXT50", Type.GetType("System.String"))
            .Add("TEXT200", Type.GetType("System.String"))
            .Add("KOPFTEXT", Type.GetType("System.String"))
            .Add("POSITIONSTEXT", Type.GetType("System.String"))
            .Add("ZZFINART", Type.GetType("System.String"))
            .Add("ZZLAUFZEIT", Type.GetType("System.String"))
            .Add("ANFNR", Type.GetType("System.String"))
            .Add("AUGRU", Type.GetType("System.String"))
            .Add("AUGRU_Klartext", Type.GetType("System.String"))
            .Add("ZZZLDAT", Type.GetType("System.String"))
            .Add("ZZFINART_BEZ", Type.GetType("System.String"))
            .Add("ZZFINART_GRP", Type.GetType("System.String"))
            .Add("MESSAGE", Type.GetType("System.String"))
        End With

        Try
            m_intStatus = 0
            m_strMessage = ""
            'weil bei bankbase führende nullen angehängt werden, verträgt das bapi aber nicht wenn ohne Händlernummer gesucht wird.
            Dim tmpCustomer As String
            If m_strCustomer = "0000000000" Then
                tmpCustomer = ""
            Else
                tmpCustomer = m_strCustomer
            End If

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unangefordert_002", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", m_strKUNNR)
            myProxy.setImportParameter("I_HAENDLER", tmpCustomer)
            myProxy.setImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr.Replace("*", ""))
            myProxy.setImportParameter("I_LIZNR", m_strLIZNR.Replace("*", ""))
            myProxy.setImportParameter("I_ZZREFERENZ1", m_strSucheZZREFERENZ1.Replace("*", ""))
            myProxy.setImportParameter("I_TIDNR", SucheTIDNR.Replace("*", ""))

            myProxy.callBapi()


            mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
            mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")
            Dim tblFahrzeugeSAP As DataTable = myProxy.getExportTable("GT_WEB")
            Dim rowFahrzeugSAP As DataRow


            For Each rowFahrzeugSAP In tblFahrzeugeSAP.Rows
                Dim rowFahrzeug As DataRow

                rowFahrzeug = m_tblFahrzeuge.NewRow
                rowFahrzeug("ZZFAHRG") = rowFahrzeugSAP("ZZFAHRG")
                rowFahrzeug("MANDT") = "0"
                rowFahrzeug("LIZNR") = rowFahrzeugSAP("Liznr")
                rowFahrzeug("EQUNR") = rowFahrzeugSAP("Equnr")
                rowFahrzeug("TIDNR") = rowFahrzeugSAP("Tidnr")
                rowFahrzeug("Zzreferenz1") = rowFahrzeugSAP("Zzreferenz1")
                rowFahrzeug("LICENSE_NUM") = rowFahrzeugSAP("License_Num")
                If UCase(rowFahrzeugSAP("Zzcockz").ToString) = "X" Then
                    rowFahrzeug("ZZCOCKZ") = True
                Else
                    rowFahrzeug("ZZCOCKZ") = False
                End If
                rowFahrzeug("VBELN") = ""
                rowFahrzeug("COMMENT") = ""
                rowFahrzeug("ZZFINART") = rowFahrzeugSAP("Zzfinart")
                rowFahrzeug("KOPFTEXT") = String.Empty
                rowFahrzeug("POSITIONSTEXT") = String.Empty

                rowFahrzeug("text50") = String.Empty
                rowFahrzeug("AUGRU") = String.Empty
                rowFahrzeug("MESSAGE") = rowFahrzeugSAP("MESSAGE")
                If rowFahrzeug("MESSAGE").ToString <> "" Then
                    m_boolUploadFehler = True
                End If
                rowFahrzeug("ZZZLDAT") = rowFahrzeugSAP("ZZZLDAT")
                rowFahrzeug("ZZFINART_BEZ") = rowFahrzeugSAP("ZZFINART_BEZ")
                rowFahrzeug("ZZFINART_GRP") = rowFahrzeugSAP("ZZFINART_GRP")

                m_tblFahrzeuge.Rows.Add(rowFahrzeug)
                m_tblFahrzeuge.AcceptChanges()
            Next


            Dim col As DataColumn
            For Each col In m_tblFahrzeuge.Columns
                col.ReadOnly = False
            Next

            'Händlernummer Speichern falls bankanforderung und selektion über andere Parameter
            'wenn mehrere Händlernummern falsche selektion im SAP
            If m_tblFahrzeuge.Rows.Count > 0 Then
                Customer = tblFahrzeugeSAP.Rows(0)("Kunnr").ToString
            End If


            If mE_SUBRC <> "0" Then
                m_intStatus = CInt(mE_SUBRC)
                m_strMessage = mE_MESSAGE
            End If
            WriteLogEntry(True,
                          "CHASSIS_NUM=" & m_strSucheFahrgestellNr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) &
                          ", KNRZE=" & m_strFiliale & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strSucheTIDNR &
                          ", ZZREFERENZ1=" & m_strSucheZZREFERENZ1, m_tblFahrzeuge)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -2501
                    m_strMessage = "Es wurden keine Daten gefunden."
                Case "NO_HAENDLER"
                    m_intStatus = -2502
                    m_strMessage = "Händler nicht vorhanden."
                Case "TEMP_VERSAND"
                    m_intStatus = -2503
                    m_strMessage = "Brief bereits Temporär versendet!"
                Case "HIST_CHECK"
                    m_intStatus = -2504
                    m_strMessage = "Bitte in Historie prüfen."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select

            WriteLogEntry(False,
                          "CHASSIS_NUM=" & m_strSucheFahrgestellNr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) &
                          ", KNRZE=" & m_strFiliale & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strSucheTIDNR &
                          ", ZZREFERENZ1=" & m_strSucheZZREFERENZ1 & " , " & Replace(m_strMessage, "<br>", " "),
                          m_tblFahrzeuge)
        End Try
    End Sub


    Public Sub getZulStelle(ByRef page As Page, ByVal plz As String, ByVal ort As String, ByRef status As String)

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

            m_kbanr = table.Rows(0)("KBANR").ToString

            m_tblZulStelle = table

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


    Private Sub getLaender(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        '----------------------------------------------------------------------
        ' Methode: getLaender
        ' Autor: JJU
        ' Beschreibung: gibt alle Länder zurück
        ' Erstellt am: 06.03.2009
        ' ITA: 2918
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
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


    Public Sub Anfordern(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page,
                         Optional ByVal hez As Boolean = False)
        Try
            m_intStatus = 0
            m_strMessage = ""
            m_strAuftragsnummer = ""
            m_strAuftragsstatus = ""

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Briefanforderung_002", m_objApp, m_objUser, page)


            Dim SAPTable As DataTable = myProxy.getImportTable("IMP")

            If CheckCustomerData() Then
                Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("ZZFAHRG = '" & m_strZZFAHRG & "'")

                If neueAdresse Then
                    'wenn eine Eigene Adresse Angelegt wurde oder eine Zulassungsstelle ausgewählt wurde, muss Kunr_ZS blank sein, sonst legt SAP das bei dieser Händlernummer an JJU2008.02.29
                    m_strAdresse = String.Empty
                End If

                Dim tmpSAPRow As DataRow = SAPTable.NewRow

                With tmpSAPRow
                    .Item("Anfnr") = rowFahrzeug(0)("ANFNR").ToString
                    .Item("Augru") = rowFahrzeug(0)("AUGRU").ToString
                    .Item("City1") = m_strCity
                    .Item("Datum") = "12.07.2009"
                    .Item("Equnr") = rowFahrzeug(0)("EQUNR").ToString
                    .Item("Ernam") = Left(m_strInternetUser, 12)
                    'autorisierungslevel der Gruppe schreiben, +1 weil in sap das feld nur von 1-4 geht
                    .Item("Kvgr3") = CStr(m_objUser.Groups.ItemByID(m_objUser.GroupID).Authorizationright + 1)
                    If hez Then
                        .Item("Hezkz") = "X"
                    Else
                        .Item("Hezkz") = ""
                    End If
                    .Item("Chassis_Num") = m_strZZFAHRG
                    .Item("House_Num1") = m_strHouseNum
                    .Item("Kdgrp") = ""
                    .Item("Kunnr") = m_strCustomer.TrimStart("0"c)
                    .Item("Konzs") = Right("0000000000" & m_strKUNNR, 10)
                    .Item("Kunnr_Ze") = ""
                    If hez Then
                        .Item("Kunnr_Zh") = m_strAdresseHalter
                    Else
                        .Item("Kunnr_Zh") = ""
                    End If
                    .Item("Kunnr_Zs") = m_strAdresse.TrimStart("0"c)
                    .Item("License_Num") = rowFahrzeug(0)("LICENSE_NUM").ToString
                    .Item("Liznr") = rowFahrzeug(0)("LIZNR").ToString
                    .Item("Matnr") = m_strMaterialNummer
                    .Item("Name1") = m_strName1
                    .Item("Name2") = m_strName2
                    .Item("Name3") = m_strName3
                    .Item("Post_Code1") = m_strPostcode
                    .Item("Preis") = 0
                    .Item("Street") = m_strStreet
                    .Item("Text50") = rowFahrzeug(0)("TEXT50").ToString
                    .Item("Tidnr") = rowFahrzeug(0)("TIDNR").ToString
                    .Item("Txt_Kopf") = rowFahrzeug(0)("KOPFTEXT").ToString
                    .Item("Txt_Pos") = rowFahrzeug(0)("POSITIONSTEXT").ToString
                    .Item("Zulst") = m_kbanr
                    .Item("Zzdien1") = ""
                    .Item("Zzkkber") = m_strKreditkontrollBereich
                    .Item("Zzlabel") = rowFahrzeug(0)("ZZLABEL").ToString
                    .Item("Zzlaufzeit") = rowFahrzeug(0)("ZZLAUFZEIT").ToString
                    .Item("Country") = laenderKuerzel
                End With

                SAPTable.Rows.Add(tmpSAPRow)
                SAPTable.AcceptChanges()

                myProxy.callBapi()
            End If
            m_strAuftragsnummer = myProxy.getExportParameter("E_Vbeln").TrimStart("0"c)
            Select Case UCase(myProxy.getExportParameter("E_Cmgst"))
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

            If m_strAuftragsnummer.Length = 0 Then
                m_intStatus = -2100
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
            End If
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
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
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    m_intStatus = -9999
            End Select
        End Try
    End Sub


    Public Function getHaendlernummerByFin(ByVal Fahrgestellnummer As String) As String
        '----------------------------------------------------------------------
        ' Methode: getHaendlerNummerByFIN
        ' Autor: JJU
        ' Beschreibung: Liefert über das BAPI Z_M_GET_HAENDLER Die Händlernummer von einer Fahrgestellnummer
        ' Erstellt am: 20080828
        ' ITA: 2124
        '----------------------------------------------------------------------
        m_strClassAndMethod = "fin_06.getHaendlernummerByFin"
        Dim sHaendlerNo As String = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1
            Dim tblTmp As New DataTable
            m_intStatus = 0
            m_strMessage = ""
            Try
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_GET_HAENDLER",
                                                            m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_GET_HAENDLER", m_objApp, m_objUser,
                                                 CurrentPageHelper.GetCurrentPage())

                'befüllen der Importparameter
                proxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_CHASSIS_NUM", Fahrgestellnummer.ToUpper)
                proxy.setImportParameter("I_VKORG", "1510")

                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                tblTmp = proxy.getExportTable("GT_DATEN")
                HelpProcedures.killAllDBNullValuesInDataTable(tblTmp)

                If tblTmp.Rows.Count = 1 Then
                    sHaendlerNo = tblTmp.Rows(0)("EIKTO").ToString
                ElseIf tblTmp.Rows.Count = 0 Then
                    Throw New Exception("zur Fahrgstellnummer " & Fahrgestellnummer & " wurde kein Händler gefunden")
                Else
                    Throw _
                        New Exception("zur Fahrgstellnummer " & Fahrgestellnummer & " wurden mehrere Händler gefunden")
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, tblTmp)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -2222
                        m_strMessage = "Keine Händlernummer zu dem Fahrzeug gefunden"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), tblTmp)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If
                m_blnGestartet = False
            End Try
        End If
        Return sHaendlerNo
    End Function


    Public Function getNextWorkDay(ByVal fromDate As Date, ByVal strAppID As String, ByVal strSessionID As String,
                                   ByVal page As Page) As Date
        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_S_Add_Date", m_objApp, m_objUser, page)

            myProxy.setImportParameter("IN_DATUM", fromDate.ToShortDateString)
            myProxy.setImportParameter("FACTORY_CALENDAR_ID", "01")
            myProxy.setImportParameter("CORRECT_OPTION", "+")
            myProxy.setImportParameter("ANZAHL", "1")

            myProxy.callBapi()

            Return CDate(myProxy.getExportParameter("OUT_DATUM"))
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_strMessage = "Unbekannter Fehler. " & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    m_intStatus = -9999
            End Select
        End Try
    End Function


    'Public Function getNextWorkDay(ByVal fromDate As Date) As Date
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_ComCommon_Finance.SAPProxy_ComCommon_Finance()

    '        MakeDestination()
    '        Try
    '            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '            objSAP.Connection.Open()

    '            Dim returnDatum As String = ""
    '            objSAP.Z_S_Add_Date(1, "+", "01", MakeDateSAP(fromDate), returnDatum)
    '            objSAP.CommitWork()
    '            Return MakeDateStandard(returnDatum)

    '            If m_objLogApp Is Nothing Then
    '                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
    '            End If

    '        Catch ex As Exception
    '            Select Case ex.Message
    '                Case Else
    '                    m_strMessage = "Unbekannter Fehler. " & ex.Message
    '                    m_intStatus = -9999
    '            End Select

    '        Finally
    '            m_blnGestartet = False
    '            objSAP.Connection.Close()
    '            objSAP.Dispose()
    '        End Try
    '    End If
    'End Function

#End Region
End Class
