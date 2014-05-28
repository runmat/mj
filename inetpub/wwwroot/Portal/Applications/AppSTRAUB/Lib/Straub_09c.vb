Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class Straub_09c
    REM § Lese-/Schreibfunktion, Kunde: FFD, 
    REM § GiveCars - BAPI: Z_M_Unangefordert,
    REM § Anfordern - BAPI: Z_M_Briefanforderung.

    Inherits Base.Business.BankBaseCredit

#Region " Declarations"
    Private m_strSucheLiznr As String
    Private m_strSucheKennzeichen As String
    Private m_strSucheFahrgestellnr As String
    Private m_strSucheEquityp As String
    Private m_tblFahrzeuge As DataTable
    Private m_strZZFAHRG As String
    'Private m_strAdresse As String
    Private m_strAuftragsnummer As String
    Private m_strAuftragsstatus As String
    'Private m_strKreditkontrollBereich As String
    Private m_strMaterialNummer As String 'Versandart
    'Private m_strAdresseHalter As String    'HEZ
    'Private m_strAdresseEmpf As String    'HEZ
    'Private m_preis As Decimal 'HEZ
    'Private m_datumRange As DataTable       'HEZ:Hier Datumswerte auslesen...
    'Private m_kbanr As String 'Zulassungsdienst-Nummer
    Private m_strName As String
    Private m_strName1 As String
    Private m_strStrasse As String
    Private m_strNr As String
    Private m_strPlz As String
    Private m_strOrt As String
#End Region

#Region " Properties"
    Public Property POrt() As String
        Get
            Return m_strOrt
        End Get
        Set(ByVal Value As String)
            m_strOrt = Value
        End Set
    End Property

    Public Property PPlz() As String
        Get
            Return m_strPlz
        End Get
        Set(ByVal Value As String)
            m_strPlz = Value
        End Set
    End Property

    Public Property PHausnummer() As String
        Get
            Return m_strNr
        End Get
        Set(ByVal Value As String)
            m_strNr = Value
        End Set
    End Property

    Public Property PStrasse() As String
        Get
            Return m_strStrasse
        End Get
        Set(ByVal Value As String)
            m_strStrasse = Value
        End Set
    End Property

    Public Property PName1() As String
        Get
            Return m_strName1
        End Get
        Set(ByVal Value As String)
            m_strName1 = Value
        End Set
    End Property

    Public Property PName() As String
        Get
            Return m_strName
        End Get
        Set(ByVal Value As String)
            m_strName = Value
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


    Public Property SucheVertragsNr() As String
        Get
            Return m_strSucheLiznr
        End Get
        Set(ByVal Value As String)
            m_strSucheLiznr = Value
        End Set
    End Property

    Public Property SucheKennzeichen() As String
        Get
            Return m_strSucheKennzeichen
        End Get
        Set(ByVal Value As String)
            m_strSucheKennzeichen = Value
        End Set
    End Property

    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellnr
        End Get
        Set(ByVal Value As String)
            m_strSucheFahrgestellnr = Value
        End Set
    End Property

    Public Property SucheEquiTyp() As String
        Get
            Return m_strSucheEquityp
        End Get
        Set(ByVal Value As String)
            m_strSucheEquityp = Value
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
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Sub GiveCars()
        m_strClassAndMethod = "Straub_09c.GiveCars"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_STRAUB2.SAPProxy_STRAUB2()

            Dim tblFahrzeugeSAP As New SAPProxy_STRAUB2.ZDAD_M_WEB_EQUIDATENTable() 'ZDAD_M_WEB_HAENDLERBESTANDTable() 'ZDAD_M_WEB_EQUIDATENTable()
            Dim rowFahrzeugSAP As SAPProxy_STRAUB2.ZDAD_M_WEB_EQUIDATEN ' .ZDAD_M_WEB_HAENDLERBESTAND 'ZDAD_M_WEB_EQUIDATEN
            Dim rowGrundSAP As SAPProxy_STRAUB2.ZVERSAND_GRUNDTable ' ZDAD_M_WEB_EQUIDATEN

            m_tblFahrzeuge = New DataTable()

            With m_tblFahrzeuge.Columns
                .Add("ZZFAHRG", System.Type.GetType("System.String"))
                .Add("MANDT", System.Type.GetType("System.String"))
                .Add("LIZNR", System.Type.GetType("System.String"))
                .Add("EQUNR", System.Type.GetType("System.String"))
                .Add("LICENSE_NUM", System.Type.GetType("System.String"))
                .Add("EQTYP", System.Type.GetType("System.String"))
                .Add("COMMENT", System.Type.GetType("System.String"))
                .Add("TEXT50", System.Type.GetType("System.String"))
                .Add("TEXT200", System.Type.GetType("System.String"))
                .Add("KOPFTEXT", System.Type.GetType("System.String"))
                .Add("POSITIONSTEXT", System.Type.GetType("System.String"))
                .Add("TIDNR", System.Type.GetType("System.String"))
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

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Unangefordert_001(m_strSucheFahrgestellnr, m_strSucheEquityp, m_strKUNNR, m_strSucheKennzeichen, m_strSucheLiznr, "1510", rowGrundSAP, tblFahrzeugeSAP)
                objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If

                For Each rowFahrzeugSAP In tblFahrzeugeSAP
                    Dim rowFahrzeug As DataRow
                    rowFahrzeug = m_tblFahrzeuge.NewRow
                    rowFahrzeug("ZZFAHRG") = rowFahrzeugSAP.Chassis_Num
                    rowFahrzeug("MANDT") = "0"
                    rowFahrzeug("EQUNR") = rowFahrzeugSAP.Equnr.TrimStart("0"c)
                    If rowFahrzeugSAP.Eqtyp = "B" Then
                        rowFahrzeug("EQTYP") = "Brief"
                    Else
                        rowFahrzeug("EQTYP") = "Schlüsseltüte"
                    End If
                    rowFahrzeug("LIZNR") = rowFahrzeugSAP.Liznr
                    rowFahrzeug("LICENSE_NUM") = rowFahrzeugSAP.License_Num
                    rowFahrzeug("COMMENT") = String.Empty
                    rowFahrzeug("TEXT50") = String.Empty
                    rowFahrzeug("TEXT200") = String.Empty
                    rowFahrzeug("COMMENT") = String.Empty
                    rowFahrzeug("KOPFTEXT") = String.Empty
                    rowFahrzeug("POSITIONSTEXT") = String.Empty
                    rowFahrzeug("TIDNR") = rowFahrzeugSAP.Tidnr
                    m_tblFahrzeuge.Rows.Add(rowFahrzeug)
                Next

                WriteLogEntry(True, "CHASSIS_NUM=" & m_strSucheFahrgestellnr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ", LIZNR=" & m_strSucheLiznr & ", CHASSIS_NUM=" & m_strSucheKennzeichen, m_tblFahrzeuge)
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
                        m_strMessage = "Fehler bei der Ermittlung der Equipmentdaten."
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If

                WriteLogEntry(False, "CHASSIS_NUM=" & m_strSucheFahrgestellnr & ", KONZS=" & m_strKUNNR.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=" & Right(m_strCustomer, 5) & ", LIZNR=" & m_strSucheLiznr & ", CHASSIS_NUM=" & m_strSucheKennzeichen & " , " & Replace(m_strMessage, "<br>", " "), m_tblFahrzeuge)
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

    Public Sub Anfordern(Optional ByVal hez As Boolean = False)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_STRAUB2.SAPProxy_STRAUB2() '.SAPProxy_PORSCHE()

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

                Dim rowFahrzeug() As DataRow = m_tblFahrzeuge.Select("EQUNR = '" & m_strZZFAHRG & "'")

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefanforderung", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Briefanforderung_001(rowFahrzeug(0)("MANDT").ToString, rowFahrzeug(0)("ZZFAHRG").ToString, Right("000000000000000000" & rowFahrzeug(0)("EQUNR").ToString, 18), m_strInternetUser, m_strNr, m_strKUNNR, "", "", "", "", "DE", rowFahrzeug(0)("LICENSE_NUM").ToString, rowFahrzeug(0)("LIZNR").ToString, Right("000000000000000000" & m_strMaterialNummer, 18), m_strName, m_strName1, m_strOrt, m_strPlz, m_strStrasse, rowFahrzeug(0)("TIDNR").ToString, "", "", strAuftragsstatus, strAuftragsnummer)
                objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If

                m_strAuftragsnummer = strAuftragsnummer.TrimStart("0"c)

                If m_strAuftragsnummer.Length = 0 Then
                    m_intStatus = -2100
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                Else
                    m_strMessage = "Vorgang OK (" & m_strAuftragsnummer & ")"
                End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_UPDATE_EQUI"
                        m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)."
                        m_intStatus = -1112
                    Case "NO_AUFTRAG"
                        m_strMessage = "Kein Auftrag angelegt."
                        m_intStatus = -1113
                    Case "NO_ZDADVERSAND"
                        m_strMessage = "Keine Einträge für die Versanddatei erstellt."
                        m_intStatus = -1114
                    Case "NO_UPDATE_ILOA"
                        m_strMessage = "ILOA-UPDATE-Fehler."
                        m_intStatus = -1115
                    Case "NO_BRIEFANFORDERUNG"
                        m_strMessage = "Brief bereits angefordert."
                        m_intStatus = -1116
                    Case "NO_EQUZ_ILOA"
                        m_strMessage = "Kein Brief vorhanden (EQUZ+ILOA)."
                        m_intStatus = -1117
                    Case "EQUI_SPERRE"
                        m_strMessage = "EQUI ist gesperrt."
                        m_intStatus = -1118
                    Case "NO_INSERT_ADRESSE"
                        m_strMessage = "Keine Adresse angelegt."
                        m_intStatus = -1119
                    Case Else
                        m_strMessage = ex.Message
                        m_intStatus = -9999
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
            Finally
                If m_intIDsap > -1 Then
                    m_objLogApp.LogStandardIdentity = m_intStandardLogID
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
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
' $History: Straub_09c.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.07.07    Time: 9:51
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' 
' ************************************************
