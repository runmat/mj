Option Explicit On 
Option Strict On

Imports System
Imports System.Configuration
Imports CKG.Base.Business
Imports CKG.Base.Kernel

<Serializable()> Public Class FMS_2
    REM § Lese-/Schreibfunktion, Kunde: FMS, 
    REM § Show - BAPI: Z_M_ZULF_FZGE_ALD,
    REM § Change - BAPI: ?.

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_strSachbearbeiterNummer As String

    Private m_strSucheFahrgestellNr As String
    Private m_strSucheKennzeichen As String
    Private m_strSucheLeasingvertragsNr As String
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

            m_intIDSAP = -1

            Try
                S.AP.Init("Z_M_Unabgemeldet_Aldbp")

                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_LICENSE_NUM", m_strSucheKennzeichen)
                S.AP.SetImportParameter("I_LIZNR", m_strSucheLeasingvertragsNr)
                S.AP.SetImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr)

                S.AP.Execute()

                m_tblResult = S.AP.GetExportTable("GT_WEB")

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

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
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
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                S.AP.Init("Z_M_Abmeldung_Aldbp")

                S.AP.SetImportParameter("I_KUNNR_AG", KUNNR)
                S.AP.SetImportParameter("I_KUNNR_ZK", CStr(m_tblSachbearbeiter.Select("SORTL='" & m_strSachbearbeiterNummer & "'")(0)("KUNNR_ZK")))
                S.AP.SetImportParameter("I_CHASSIS_NUM", RemoveSingleSpace(m_strSucheFahrgestellNr))
                S.AP.SetImportParameter("I_LIZNR", RemoveSingleSpace(m_liznr))
                S.AP.SetImportParameter("I_SORTL", m_strSachbearbeiterNummer)

                S.AP.Execute()

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
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Function GiveResultStructure() As DataTable

        S.AP.Init("Z_M_UNANGEFORDERT_BWFUHR")

        Dim tblTemp As DataTable = S.AP.GetImportTable("GT_WEB")

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
        Dim connection As New SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim sqlInsert As String

        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            sqlInsert = "SELECT * FROM AuthorizationFMS"

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
            connection.Dispose()
        End Try
    End Sub

    Private Sub LeseSachbearbeiter()

        '*********Das BAPI Z_M_Sachbearbeiter_Aldp existiert nicht mehr!
        '*********Die Anwendung um den SAP.Connector bereinigt werden
        '*********Es ist fraglich, ob die Anwendung noch mal verwendet werden wird

        Throw New ApplicationException("Das BAPI Z_M_Sachbearbeiter_Aldp existiert nicht")

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
' $History: FMS_2.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 15:58
' Updated in $/CKAG/Applications/appfms/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:44
' Created in $/CKAG/Applications/appfms/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 17:52
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 13:11
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' 
' ******************************************
