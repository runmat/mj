Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business

<Serializable()> Public Class ALD_2
    REM § Lese-/Schreibfunktion, Kunde: ALD, 
    REM § Show - BAPI: Z_M_ZULF_FZGE_ALD,
    REM § Change - BAPI: ?.

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_strSachbearbeiterNummer As String

    Private m_strSucheFahrgestellNr As String
    Private m_strSucheKennzeichen As String
    Private m_strSucheLeasingvertragsNr As String
    Private m_kbanr As String
    Private m_zulkz As String
    Private m_Fahrzeuge As Int32
    Private m_liznr As String

    Private m_tblSachbearbeiter As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property Sachbearbeiter() As DataTable
        Get
            If m_tblSachbearbeiter Is Nothing Then
                LeseSachbearbeiter()
            End If
            Return m_tblSachbearbeiter
        End Get
    End Property

    Public Property SachbearbeiterNummer() As String
        Get
            Return m_strSachbearbeiterNummer
        End Get
        Set(ByVal Value As String)
            m_strSachbearbeiterNummer = Value
        End Set
    End Property

    Public Property LizenzNr() As String
        Get
            Return m_liznr
        End Get
        Set(ByVal Value As String)
            m_liznr = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblResult
        End Get
        Set(ByVal Value As DataTable)
            m_tblResult = Value
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

    Public Property SucheLeasingvertragsNr() As String
        Get
            Return m_strSucheLeasingvertragsNr
        End Get
        Set(ByVal Value As String)
            m_strSucheLeasingvertragsNr = Value
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
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub Show()
        Dim tableHide As New DataTable()
        Dim status As String = ""
        Dim row As DataRow
        Dim rowResult As DataRow()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ALD.SAPProxy_ALD()
            '§§§ JVE 17.01.2006 Tabellenname hat sich geändert. Wieso???
            'Dim tblFahrzeuge As New SAPProxy_ALD.ZDAD_M_WEB_EQUIDATENTable()
            Dim tblFahrzeuge As New SAPProxy_ALD.ZDAD_M_WEB_ALDTable()
            Dim tblGrund As New SAPProxy_ALD.ZVERSAND_GRUNDTable()
            '---------------------------------------------------------------
            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Dim intID As Int32 = -1

            Try

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unabgemeldet_Aldbp", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_M_Unabgemeldet_Aldbp(m_strSucheFahrgestellNr, "0000000000", m_strSucheKennzeichen, m_strSucheLeasingvertragsNr, tblGrund, tblFahrzeuge)
                'Right("0000000000" & m_objUser.KUNNR, 10)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                m_tblResult = tblFahrzeuge.ToADODataTable
                m_tblResult.Columns.Add("STATUS", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("SACHBEARBEITER", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("SB_CHECK", System.Type.GetType("System.Boolean"))
                m_intStatus = 0

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    'Zur Autorisierung gespeicherte Daten entfernen!
                    readAllAuthorizationSets(tableHide, status) 'Daten, die zur Autorisierung gespeichert wurden nicht anzeigen!
                    If tableHide.Rows.Count > 0 Then
                        For Each row In tableHide.Rows
                            rowResult = m_tblResult.Select("EQUNR = '" & row("EQUIPMENT").ToString & "'")
                            If Not (rowResult.Length = 0) Then
                                m_tblResult.Rows.Remove(rowResult(0))
                            End If
                        Next
                    End If

                    For Each row In m_tblResult.Rows
                        row("STATUS") = ""
                        row("SACHBEARBEITER") = ""
                        row("SB_CHECK") = True
                    Next
                End If

                WriteLogEntry(True, "FahrgestellNr=" & m_strSucheFahrgestellNr & ", LVNr.=" & m_strSucheLeasingvertragsNr & ", KfzKz.=" & m_strSucheKennzeichen & ", KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -3331
                        m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -3332
                        m_strMessage = "Keine oder falsche Haendlernummer."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "FahrgestellNr=" & m_strSucheFahrgestellNr & ", LVNr.=" & m_strSucheLeasingvertragsNr & ", KfzKz.=" & m_strSucheKennzeichen & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ALD.SAPProxy_ALD()

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

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abmeldung_Aldbp", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim strDaten As String = _
                    ", strSucheFahrgestellNr=" & m_strSucheFahrgestellNr & _
                    ", KUNNR=" & KUNNR & _
                    ", liznr=" & m_liznr
                objSAP.Z_M_Abmeldung_Aldbp(RemoveSingleSpace(m_strSucheFahrgestellNr), KUNNR, CStr(m_tblSachbearbeiter.Select("SORTL='" & m_strSachbearbeiterNummer & "'")(0)("KUNNR_ZK")), RemoveSingleSpace(m_liznr), m_strSachbearbeiterNummer)
                objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True, strDaten)
                End If
                m_strMessage = "Vorgang OK"
            Catch ex As Exception
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                Select Case ex.Message
                    Case "NO_UPDATE_EQUI"
                        m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                        m_intStatus = -3311
                    Case "NO_BRIEFANFORDERUNG"
                        m_strMessage = "Brief bereits angefordert"
                        m_intStatus = -3312
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                        m_intStatus = -3313
                    Case "NO_AUFTRAG"
                        m_strMessage = "Kein Auftrag angelegt"
                        m_intStatus = -3314
                    Case "NO_ABMELDUNG"
                        m_strMessage = "Brief bereits in Abmeldung."
                        m_intStatus = -3315
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

    Public Function GiveResultStructure() As DataTable
        Dim tblTemp As DataTable
        '§§§ JVE 17.01.2006 Tabellenname hat sich geändert. Wieso???
        'Dim tblTempSAP As New SAPProxy_ALD.ZDAD_M_WEB_EQUIDATENTable()
        Dim tblTempSAP As New SAPProxy_ALD.ZDAD_M_WEB_ALDTable()
        '--------------------------------------------------------------
        tblTemp = tblTempSAP.ToADODataTable
        tblTemp.Columns.Add("STATUS", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("SACHBEARBEITER", System.Type.GetType("System.String"))
        tblTemp.Columns.Add("SB_CHECK", System.Type.GetType("System.Boolean"))
        Return tblTemp
    End Function

    Public Function CheckAgainstAuthorizationTable(ByVal strEQUIPMENT As String) As Boolean
        Dim tableHide As New DataTable()
        Dim status As String = ""
        Dim rowResult As DataRow()
        Dim blnReturn As Boolean = False

        Try
            readAllAuthorizationSets(tableHide, status) 'Daten, die zur Autorisierung gespeichert wurden nicht anzeigen!

            rowResult = tableHide.Select("EQUIPMENT = '" & strEQUIPMENT & "'")
            If Not (rowResult.Length = 0) Then
                blnReturn = True
            End If
        Catch ex As Exception

        End Try
        Return blnReturn
    End Function

    Public Sub readAllAuthorizationSets(ByRef resultTable As DataTable, ByRef status As String)
        Dim connection As SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim sqlInsert As String
        connection = New SqlClient.SqlConnection
        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            sqlInsert = "SELECT * FROM AuthorizationALD"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            connection.Open()
            adapter.SelectCommand = command
            adapter.Fill(resultTable)

            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Private Sub LeseSachbearbeiter()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ALD.SAPProxy_ALD()
            Dim tblSAP As New SAPProxy_ALD.ZDAD_M_WEB_SACHBEARBEITERTable()

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

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Sachbearbeiter_Aldp", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_M_Sachbearbeiter_Aldbp(KUNNR, tblSAP)
                objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If
                m_tblSachbearbeiter = tblSAP.ToADODataTable

                Dim row As DataRow
                For Each row In m_tblSachbearbeiter.Rows
                    'row("SORTL") = CStr(row("SORTL")).TrimStart("0"c)
                    'row("KUNNR_ZK") = CStr(row("KUNNR_ZK")).TrimStart("0"c)
                    row("NAME1") = CStr(row("SORTL")) & " - " & CStr(row("NAME1"))
                Next

            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Sachbearbeiter gefunden."
                        m_intStatus = -3111
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

    Private Function RemoveSingleSpace(ByVal strIn As String) As String
        Dim strReturn As String = ""
        Try
            Dim strOut As String = strIn.Trim(" "c)
            If strOut = "&nbsp;" Then
                strOut = ""
            End If
            strReturn = strOut
        Catch
        End Try
        Return strReturn
    End Function
#End Region
End Class

' ************************************************
' $History: ALD_2.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 6.12.07    Time: 13:15
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' ITA: 1440
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 10  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' 
' ************************************************
