Option Explicit On 
Option Strict On

Imports System
Imports CKG
Imports CKG.Base.Kernel

Public Class Porsche_05
    REM § Lese-/Schreibfunktion, Kunde: FFD, 
    REM § GiveCars - BAPI: Z_M_Unangefordert,
    REM § Anfordern - BAPI: Z_M_Briefanforderung.

    Inherits CKG.Base.Business.BankBaseCredit

#Region " Declarations"
    Private m_strSucheVertragsNr As String
    Private m_strSucheOrderNr As String
    Private m_strSucheFahrgestellNr As String
    Private m_tblFahrzeuge As DataTable
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
#End Region

#Region " Properties"

    Public ReadOnly Property kbanr() As String
        Get
            Return m_kbanr
        End Get
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
            m_strAdresse = Right("00000000000" & Value, 10)
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

    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellNr
        End Get
        Set(ByVal Value As String)
            m_strSucheFahrgestellNr = Value
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
        ShowStandard()      'Standard-Methode für Porsche!
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

            Dim objSAP As New SAPProxy_PORSCHE.SAPProxy_PORSCHE()

            Dim tblFahrzeugeSAP As New SAPProxy_PORSCHE.ZDAD_M_WEB_HAENDLERBESTANDTable() 'ZDAD_M_WEB_EQUIDATENTable()
            Dim rowFahrzeugSAP As SAPProxy_PORSCHE.ZDAD_M_WEB_HAENDLERBESTAND 'ZDAD_M_WEB_EQUIDATEN

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
                    m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert_Porsche", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                    objSAP.Z_M_Unangefordert_Porsche(m_strSucheFahrgestellNr, m_strKUNNR, Right(m_strCustomer, 10), m_strSucheVertragsNr, "Change04.aspx", tblFahrzeugeSAP)
                    objSAP.CommitWork()

                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                    End If

                    For Each rowFahrzeugSAP In tblFahrzeugeSAP
                        Dim rowFahrzeug As DataRow
                        rowFahrzeug = m_tblFahrzeuge.NewRow
                        rowFahrzeug("ZZFAHRG") = rowFahrzeugSAP.Chassis_Num
                        rowFahrzeug("MANDT") = "0"
                        rowFahrzeug("LIZNR") = rowFahrzeugSAP.Liznr
                        rowFahrzeug("EQUNR") = rowFahrzeugSAP.Equnr
                        '###JVE 17.07.2006: Auskommentiert, da laut C.Rothe keine Verwendung...
                        'rowFahrzeug("LIZNR") = rowFahrzeugSAP.Liznr
                        rowFahrzeug("TIDNR") = rowFahrzeugSAP.Tidnr
                        rowFahrzeug("LICENSE_NUM") = rowFahrzeugSAP.License_Num
                        '###JVE 17.07.2006: Auskommentiert, da laut C.Rothe keine Verwendung...
                        'rowFahrzeug("ZZREFERENZ1") = rowFahrzeugSAP.Zzreferenz1
                        'rowFahrzeug("ZZLABEL") = rowFahrzeugSAP.Zzlabel
                        If UCase(rowFahrzeugSAP.Zzcockz) = "X" Then
                            rowFahrzeug("ZZCOCKZ") = True
                        Else
                            rowFahrzeug("ZZCOCKZ") = False
                        End If
                        '###JVE 17.07.2006: Auskommentiert, da laut C.Rothe keine Verwendung...
                        'If UCase(rowFahrzeugSAP.Zzbezahlt) = "X" Then
                        '    rowFahrzeug("ZZBEZAHLT") = True
                        'Else
                        '    rowFahrzeug("ZZBEZAHLT") = False
                        'End If

                        rowFahrzeug("VBELN") = ""
                        rowFahrzeug("COMMENT") = ""
                        '§§§JVE 19.09.2005 <begin>
                        'Spalte Finanzierungsart füllen...
                        rowFahrzeug("ZZFINART") = rowFahrzeugSAP.Zzfinart
                        '§§§JVE 19.09.2005 <end>
                        '§§§JVE 19.10.2005
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

    ' Public Function getPrices() As DataTable
    'Dim objsap As New SAPProxy_FFD.SAPProxy_FFD()
    'Dim table As New SAPProxy_FFD.KONMTable()
    'Dim tableResult As New DataTable()
    'Dim i As Integer

    'Try
    '    objsap.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '    objsap.Connection.Open()
    '    'objsap.Z_M_Kondition_Hez("V", "ZR03", m_strKUNNR, "000000000000002353", "1510", table)
    '    objsap.Z_M_Kondition_Hez("V", "ZR03", "0000324562", "000000000000002353", "1510", table)
    '    objsap.CommitWork()
    'Catch ex As Exception
    '    m_intStatus = -1234
    '    m_strMessage = "Fehler bei der Ermittlung der Preise."
    '    Return tableResult
    'Finally
    '    objsap.Connection.Close()
    '    objsap.Dispose()
    'End Try
    'tableResult = table.ToADODataTable
    'With tableResult.Columns
    '    .Remove("MANDT")
    '    .Remove("KNUMH")
    '    .Remove("KOPOS")
    '    .Remove("KLFN1")
    'End With
    'Return tableResult
    ' End Function

    Public Sub getZulStelle(ByVal plz As String, ByVal ort As String, ByRef status As String)
        Dim objSAP As New SAPProxy_Base.SAPProxy_Base() '  SAPProxy_FFD.SAPProxy_FFD() ' SAPProxy_FFD.SAPProxy_FFD()
        Dim objPLZ As New SAPProxy_Base.ZPOSTLEITZAHLENTable()
        Dim objZul As New SAPProxy_Base.ZULSTTable()
        Dim table As New DataTable()

        Try
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            objSAP.Z_Get_Zulst_By_Plz(ort, plz, objPLZ, objZul)
            objSAP.CommitWork()

            table = objZul.ToADODataTable
            If (table.Rows.Count > 1) Then
                'Mehr als ein Eintrag gefunden! Darf nicht sein!
                status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
            End If

            m_kbanr = table.Rows(0)("KBANR").ToString

        Catch ex As Exception
            Select Case ex.Message

                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
            status = m_strMessage
            m_kbanr = ""
        Finally
            If m_intIDSAP > -1 Then
                m_objLogApp.LogStandardIdentity = m_intStandardLogID
                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
            End If

            objSAP.Connection.Close()
            objSAP.Dispose()
        End Try
    End Sub

    Public Sub Anfordern(Optional ByVal hez As Boolean = False)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_PORSCHE.SAPProxy_PORSCHE()

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
                m_strAuftragsnummer = ""
                m_strAuftragsstatus = ""

                Dim strAuftragsnummer As String = ""
                Dim strAuftragsstatus As String = ""

                If CheckCustomerData() Then
                    Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("ZZFAHRG = '" & m_strZZFAHRG & "'")

                    m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefanforderung_Porsche", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                    objSAP.Z_M_Briefanforderung_Porsche("", rowFahrzeug(0)("ZZFAHRG").ToString, rowFahrzeug(0)("EQUNR").ToString, m_strInternetUser, m_strKUNNR, m_strCustomer, m_strAdresse, rowFahrzeug(0)("LICENSE_NUM").ToString, m_strMaterialNummer, rowFahrzeug(0)("TEXT50").ToString, rowFahrzeug(0)("TIDNR").ToString, rowFahrzeug(0)("KOPFTEXT").ToString, rowFahrzeug(0)("POSITIONSTEXT").ToString, "Change04.aspx", "", "", m_strKreditkontrollBereich, strAuftragsstatus, strAuftragsnummer)
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
                            m_strAuftragsstatus = "Vorgang gesperrt angelegt"
                        Case "C"
                            m_strAuftragsstatus = "Vorgang gesperrt angelegt"
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
#End Region

End Class

' ************************************************
' $History: Porsche_05.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 6.04.09    Time: 17:31
' Updated in $/CKAG2/Applications/AppPorsche/lib
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 3.04.09    Time: 16:42
' Created in $/CKAG2/Applications/AppPorsche/lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 18:24
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' 
' ************************************************