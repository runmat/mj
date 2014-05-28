Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Configuration
Imports CKG.Base.Business
Imports CKG.Base.Kernel

<Serializable()> Public Class LT_01c

    Inherits BankBase

#Region " Declarations"
    Private m_strHaendlernummer As String
    Private m_strHalterNummer As String
    Private m_strStandortNummer As String
    Private m_strZielFirma As String
    Private m_strZielFirma2 As String
    Private m_strZielStrasse As String
    Private m_strZielHNr As String
    Private m_strZielPLZ As String
    Private m_strZielOrt As String
    Private m_strZielLand As String
    Private m_blnArvalZulassung As Boolean

    Private m_strSucheFgstNr As String
    Private m_strSucheLvNr As String
    Private m_strSucheKunnrZL As String
    Private m_strSucheKunnrZF As String
    Private m_strSucheKennz As String

    Private m_tableGrund As DataTable
    Private m_tableLeasingnehmer As DataTable
    Private m_tableLieferant As DataTable
    Private m_tableZulst As DataTable
    Private m_versandadr As String
    Private m_versandadrtext As String
    Private m_versicherung As String
    Private m_material As String
    Private m_schein As String
    Private m_abckz As String
    Private m_equ As String
    Private m_kennz As String
    Private m_tidnr As String
    Private m_liznr As String
    Private m_versgrund As String
    Private m_versgrundText As String
    Private strAuftragsstatus As String
    Private strAuftragsnummer As String
    Private dataArray As ArrayList
    Private m_blnAbwVersandadresse As Boolean
    Private rowToChange As DataRow
    Private m_strBemerkung As String
    Private m_strListenart As String
    Private m_strListenartText As String
#End Region

#Region " Properties"

    Public Property Bemerkung() As String
        Get
            Return m_strBemerkung
        End Get
        Set(ByVal Value As String)
            m_strBemerkung = Value
        End Set
    End Property

    Public Property Listenart() As String
        Get
            Return m_strListenart
        End Get
        Set(ByVal Value As String)
            m_strListenart = Value
        End Set
    End Property

    Public Property ListenartText() As String
        Get
            Return m_strListenartText
        End Get
        Set(ByVal Value As String)
            m_strListenartText = Value
        End Set
    End Property

    Public Property Versicherung() As String
        Get
            Return m_versicherung
        End Get
        Set(ByVal Value As String)
            m_versicherung = Value
        End Set
    End Property

    Public Property AbwVersandadresse() As Boolean
        Get
            Return m_blnAbwVersandadresse
        End Get
        Set(ByVal Value As Boolean)
            m_blnAbwVersandadresse = Value
        End Set
    End Property

    Public Property VersandGrundText() As String
        Get
            Return m_versgrundText
        End Get
        Set(ByVal Value As String)
            m_versgrundText = Value
        End Set
    End Property

    Public Property rowChange() As DataRow
        Get
            Return rowToChange
        End Get
        Set(ByVal Value As DataRow)
            rowToChange = Value
        End Set
    End Property

    Public Property objData() As ArrayList
        Get
            Return dataArray
        End Get
        Set(ByVal Value As ArrayList)
            dataArray = Value
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

    Public Property VersandGrund() As String
        Get
            Return m_versgrund
        End Get
        Set(ByVal Value As String)
            m_versgrund = Value
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

    Public Property TIDNr() As String
        Get
            Return m_tidnr
        End Get
        Set(ByVal Value As String)
            m_tidnr = Value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return m_kennz
        End Get
        Set(ByVal Value As String)
            m_kennz = Value
        End Set
    End Property

    Public Property Equimpent() As String
        Get
            Return m_equ
        End Get
        Set(ByVal Value As String)
            m_equ = Value
        End Set
    End Property

    Public Property Versandart() As String
        Get
            Return m_abckz
        End Get
        Set(ByVal Value As String)
            m_abckz = Value
        End Set
    End Property

    Public Property ScheinSchildernummer() As String
        Get
            Return m_schein
        End Get
        Set(ByVal Value As String)
            m_schein = Value
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

    Public Property VersandAdresse() As String
        Get
            Return m_versandadr
        End Get
        Set(ByVal Value As String)
            m_versandadr = Value
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

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblResult
        End Get
        Set(ByVal Value As DataTable)
            m_tblResult = Value
        End Set
    End Property

    Public ReadOnly Property GrundTabelle() As DataTable
        Get
            Return m_tableGrund
        End Get
    End Property

    Public ReadOnly Property LeasingnehmerTabelle() As DataTable
        Get
            Return m_tableLeasingnehmer
        End Get
    End Property

    Public ReadOnly Property LieferantTabelle() As DataTable
        Get
            Return m_tableLieferant
        End Get
    End Property

    Public ReadOnly Property ZulstTabelle() As DataTable
        Get
            Return m_tableZulst
        End Get
    End Property

    Public Property SucheLvNr() As String
        Get
            Return m_strSucheLvNr
        End Get
        Set(ByVal Value As String)
            m_strSucheLvNr = Value
        End Set
    End Property

    Public Property SucheKunnrZL() As String
        Get
            Return m_strSucheKunnrZL
        End Get
        Set(ByVal Value As String)
            m_strSucheKunnrZL = Value
        End Set
    End Property

    Public Property SucheKunnrZF() As String
        Get
            Return m_strSucheKunnrZF
        End Get
        Set(ByVal Value As String)
            m_strSucheKunnrZF = Value
        End Set
    End Property

    Public Property SucheFgstNr() As String
        Get
            Return m_strSucheFgstNr
        End Get
        Set(ByVal Value As String)
            m_strSucheFgstNr = Value
        End Set
    End Property

    Public Property SucheKennz() As String
        Get
            Return m_strSucheKennz
        End Get
        Set(ByVal Value As String)
            m_strSucheKennz = Value
        End Set
    End Property

    Public Property ArvalZulassung() As Boolean
        Get
            Return m_blnArvalZulassung
        End Get
        Set(ByVal Value As Boolean)
            m_blnArvalZulassung = Value
        End Set
    End Property

    Public Property ZielLand() As String
        Get
            Return m_strZielLand
        End Get
        Set(ByVal Value As String)
            m_strZielLand = Value
        End Set
    End Property

    Public Property ZielOrt() As String
        Get
            Return m_strZielOrt
        End Get
        Set(ByVal Value As String)
            m_strZielOrt = Value
        End Set
    End Property

    Public Property ZielPLZ() As String
        Get
            Return m_strZielPLZ
        End Get
        Set(ByVal Value As String)
            m_strZielPLZ = Value
        End Set
    End Property

    Public Property ZielHNr() As String
        Get
            Return m_strZielHNr
        End Get
        Set(ByVal Value As String)
            m_strZielHNr = Value
        End Set
    End Property

    Public Property ZielStrasse() As String
        Get
            Return m_strZielStrasse
        End Get
        Set(ByVal Value As String)
            m_strZielStrasse = Value
        End Set
    End Property

    Public Property ZielFirma2() As String
        Get
            Return m_strZielFirma2
        End Get
        Set(ByVal Value As String)
            m_strZielFirma2 = Value
        End Set
    End Property

    Public Property ZielFirma() As String
        Get
            Return m_strZielFirma
        End Get
        Set(ByVal Value As String)
            m_strZielFirma = Value
        End Set
    End Property

    Public Property Haendlernummer() As String
        Get
            Return m_strHaendlernummer
        End Get
        Set(ByVal Value As String)
            m_strHaendlernummer = Value
        End Set
    End Property

    Public Property HalterNummer() As String
        Get
            Return m_strHalterNummer
        End Get
        Set(ByVal Value As String)
            m_strHalterNummer = Value
        End Set
    End Property

    Public Property StandortNummer() As String
        Get
            Return m_strStandortNummer
        End Get
        Set(ByVal Value As String)
            m_strStandortNummer = Value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_blnArvalZulassung = False

        m_strZielLand = "DE"
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Sub ClearResultTable()
        m_tblResult = Nothing
    End Sub

    Public Function giveResultStructure(ByVal page As Page) As DataTable
        Dim tbl As DataTable

        S.AP.Init("Z_M_Unangefordert_Lt")

        tbl = S.AP.GetImportTable("GT_WEB").Clone()

        tbl.Columns.Add("Leasingnehmer", GetType(System.String))
        tbl.Columns.Add("Lieferant", GetType(System.String))
        tbl.Columns.Add("STATUS", GetType(System.String))
        tbl.Columns.Add("ART", GetType(System.String))

        Return tbl

    End Function

    Public Sub GiveCars(ByVal page As Page)
        Dim row As DataRow

        Try

            S.AP.Init("Z_M_Unangefordert_Lt")

            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_LICENSE_NUM", m_strSucheKennz)
            S.AP.SetImportParameter("I_LIZNR", SucheLvNr)
            S.AP.SetImportParameter("I_CHASSIS_NUM", m_strSucheFgstNr)
            S.AP.SetImportParameter("I_KUNNR_ZL", SucheKunnrZL)
            S.AP.SetImportParameter("I_KUNNR_ZF", SucheKunnrZF)
            S.AP.SetImportParameter("I_KONZS_ZK", m_objUser.Reference)

            S.AP.Execute()

            m_tableGrund = S.AP.GetExportTable("GT_GRU")
            m_tblResult = S.AP.GetExportTable("GT_WEB")
            m_tblResult.Columns.Add("STATUS")

            m_tblResult.Columns.Add("Leasingnehmer")
            m_tblResult.Columns.Add("Lieferant")

            m_intStatus = 0

            For Each row In m_tblResult.Rows
                row("Leasingnehmer") = CStr(row("KONZS_ZL")) & " " & CStr(row("NAME1_ZL"))
                row("Lieferant") = CStr(row("KONZS_ZF")) & " " & CStr(row("NAME1_ZF"))
            Next

            If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                m_intStatus = -3331
                m_strMessage = "Keine Daten gefunden."
            End If

            WriteLogEntry(True, "HAENDLER=" & m_strHaendlernummer & ", LVNr.=" & m_strSucheLvNr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                Case "NO_HAENDLER"
                    m_intStatus = -3332
                    m_strMessage = "Keine oder falsche Haendlernummer."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            WriteLogEntry(False, "HAENDLER=" & m_strHaendlernummer & ", LVNr.=" & m_strSucheLvNr & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Sub getZulassungsdienste(ByRef page As Web.UI.Page, ByRef tblSTVA As DataTable, ByRef status As String)

        Try

            S.AP.InitExecute("Z_Get_Zulst_By_Plz", "I_PLZ, I_ORT", "", "")

            Dim table As DataTable = S.AP.GetExportTable("T_ZULST")

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
            End If

            tblSTVA = table.Copy

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
        End Try

    End Sub

    Public Sub Fill_Zulst_Leasingnehmer_Lieferant(ByVal page As Page)

        Try
            S.AP.InitExecute("Z_M_Kundendaten_Lt", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            m_intStatus = 0

            m_tableLeasingnehmer = S.AP.GetExportTable("GT_KUNDEN_ZL")
            m_tableLieferant = S.AP.GetExportTable("GT_KUNDEN_ZF")
            m_tableZulst = S.AP.GetExportTable("GT_ZULST")

        Catch ex As Exception

            m_intStatus = -9999
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
        End Try
    End Sub

    Public Overloads Sub Anfordern(ByVal page As Page)

        Try
            ClearError()

            S.AP.Init("Z_M_Briefanforderung_Allg")

            S.AP.SetImportParameter("I_ABCKZ", m_abckz)
            S.AP.SetImportParameter("I_CHASSIS_NUM", m_strSucheFgstNr)
            S.AP.SetImportParameter("I_EQUNR", m_equ)

            S.AP.SetImportParameter("I_ERNAM", Left(m_objUser.UserName, 12))
            S.AP.SetImportParameter("I_HOUSE", m_strZielHNr)
            S.AP.SetImportParameter("I_EQUNR", m_equ)

            S.AP.SetImportParameter("I_KUNNR_AG", KUNNR)
            S.AP.SetImportParameter("I_LAND1", m_strZielLand)
            S.AP.SetImportParameter("I_LICENSE_NUM", m_kennz)

            S.AP.SetImportParameter("I_LISTENART", m_strListenart)
            S.AP.SetImportParameter("I_LIZNR", m_liznr)
            S.AP.SetImportParameter("I_MATNR", Right("000000000000000000" & m_material, 18))

            S.AP.SetImportParameter("I_NAME1", m_strZielFirma)
            S.AP.SetImportParameter("I_NAME2", Replace(m_strZielFirma2, "&nbsp;", ""))
            S.AP.SetImportParameter("I_ORT01", m_strZielOrt)

            S.AP.SetImportParameter("I_PSTLZ", m_strZielPLZ)
            S.AP.SetImportParameter("I_STR01", m_strZielStrasse)
            S.AP.SetImportParameter("I_TIDNR", m_tidnr)

            S.AP.SetImportParameter("I_ZZNAME_ZH", m_strBemerkung)
            S.AP.SetImportParameter("I_ZZVGRUND", m_versgrund)

            S.AP.Execute()

            strAuftragsnummer = S.AP.GetExportParameter("E_VBELN").TrimStart("0"c)
            strAuftragsstatus = "Vorgang OK"
            If strAuftragsnummer.Length = 0 Then
                m_intStatus = -2100
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                strAuftragsstatus = "Fehler: Anforderung konnte nicht erstellt werden."
            End If
        Catch ex As Exception
            strAuftragsstatus = "Fehler"
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
                Case Else
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    m_intStatus = -9999
            End Select
        End Try
    End Sub

    Private Sub addResultTableHeader()
        If (m_tblResult Is Nothing) Then
            m_tblResult = New DataTable()
        End If
        With m_tblResult.Columns
            .Add("id")
            .Add("Erstellt")
            .Add("Benutzer")
            .Add("Equipment")
            .Add("Fahrgestellnr")
            .Add("Versandadresse")
            .Add("VersandadresseName1")
            .Add("VersandadresseName2")
            .Add("VersandadresseStr")
            .Add("VersandadresseNr")
            .Add("VersandadressePlz")
            .Add("VersandadresseOrt")
            .Add("Lvnr")
            .Add("Haendlernummer")
            .Add("Versandart")
            .Add("Kennzeichen")
            .Add("TIDNr")
            .Add("LIZNr")
            .Add("Materialnummer")
            .Add("VersandartShow")
            .Add("Status")
        End With

    End Sub

    Private Sub addResultTableRow(ByVal id As String, ByVal tstamp As String, ByVal equi As String, ByVal user As String, ByVal objData As ArrayList)
        Dim row As DataRow

        row = m_tblResult.NewRow
        row("id") = id
        row("Erstellt") = tstamp
        row("Benutzer") = user
        row("Equipment") = equi
        If objData(11) Is Nothing Then
            row("Lvnr") = ""
        Else
            row("Lvnr") = objData(11)
        End If
        If objData(10) Is Nothing Then
            row("Fahrgestellnr") = ""
        Else
            row("Fahrgestellnr") = objData(10)
        End If
        If objData(3) Is Nothing Then
            row("VersandadresseName1") = ""
            row("Versandadresse") = ""
        Else
            row("VersandadresseName1") = objData(3)
            row("Versandadresse") = objData(3).ToString
        End If
        If objData(4) Is Nothing Then
            row("VersandadresseName2") = ""
        Else
            row("VersandadresseName2") = objData(4)
        End If
        If objData(5) Is Nothing Then
            row("VersandadresseStr") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & "<br>" & " "
        Else
            row("VersandadresseStr") = objData(5)
            row("Versandadresse") = CStr(row("Versandadresse")) & "<br>" & objData(5).ToString
        End If
        If objData(6) Is Nothing Then
            row("VersandadresseNr") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & " "
        Else
            row("VersandadresseNr") = objData(6)
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & objData(6).ToString
        End If
        If objData(7) Is Nothing Then
            row("VersandadressePlz") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & ", " & " "
        Else
            row("VersandadressePlz") = objData(7)
            row("Versandadresse") = CStr(row("Versandadresse")) & ", " & objData(7).ToString
        End If
        If objData(8) Is Nothing Then
            row("VersandadresseOrt") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & " "
        Else
            row("VersandadresseOrt") = objData(8)
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & objData(8).ToString
        End If

        If objData(0) Is Nothing Then
            row("Haendlernummer") = ""
        Else
            row("Haendlernummer") = objData(0)
        End If
        If objData(22) Is Nothing Then
            row("Kennzeichen") = ""
        Else
            row("Kennzeichen") = objData(22)
        End If
        If objData(23) Is Nothing Then
            row("TIDNr") = ""
        Else
            row("TIDNr") = objData(23)
        End If
        row("LIZNr") = objData(24)
        If objData(18) Is Nothing Then
            row("Materialnummer") = "1391"
        Else
            row("Materialnummer") = objData(18)
        End If
        Select Case CStr(row("Materialnummer"))
            Case "1391"
                row("VersandartShow") = "innerhalb von 24 bis 48 h"
            Case "1385"
                row("VersandartShow") = "vor 9:00 Uhr"
            Case "1389"
                row("VersandartShow") = "vor 10:00 Uhr"
            Case "1390"
                row("VersandartShow") = "vor 12:00 Uhr"
        End Select
        row("Versandart") = "2"                 'Endgültig
        m_tblResult.Rows.Add(row)
        m_tblResult.AcceptChanges()
    End Sub

    Public Sub getAutorizationData(ByRef status As String)
        Dim formatter As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim cn As New SqlClient.SqlConnection
        Dim cmdOutPut As SqlClient.SqlCommand
        Dim objData As ArrayList
        Dim ms As New MemoryStream()
        Dim drAppData As SqlClient.SqlDataReader
        Dim intI As Int32 = 4

        addResultTableHeader()

        Try
            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()

            cmdOutPut = New SqlClient.SqlCommand()
            cmdOutPut.Connection = cn
            cmdOutPut.CommandText = "SELECT * FROM AuthorizationARVAL"
            drAppData = cmdOutPut.ExecuteReader

            While drAppData.Read()
                If Not drAppData Is Nothing Then
                    If Not TypeOf drAppData(intI) Is System.DBNull Then
                        ' 1. Daten als bytearray aus der DB lesen:
                        Dim bytData(CInt(drAppData.GetBytes(intI, 0, Nothing, 0, Integer.MaxValue - 1) - 1)) As Byte
                        drAppData.GetBytes(intI, 0, bytData, 0, bytData.Length)
                        ' Dataset über einen Memory Stream aus dem ByteArray erzeugen:
                        Dim stmAppData As MemoryStream
                        stmAppData = New MemoryStream(bytData)
                        ms = stmAppData
                        formatter = New BinaryFormatter()
                        objData = New ArrayList()
                        objData = DirectCast(formatter.Deserialize(ms), ArrayList)
                        addResultTableRow(CType(drAppData("id"), String), CType(drAppData("tstamp"), String), CType(drAppData("equipment"), String), CType(drAppData("username"), String), objData)
                    Else
                        ms = Nothing
                    End If
                Else
                    ms = Nothing
                End If
            End While
        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Public Sub clearDB(ByVal id As Int32, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String

        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            sqlInsert = "DELETE FROM AuthorizationARVAL WHERE id = @id"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            With command.Parameters
                .AddWithValue("@id", id)
            End With
            connection.Open()
            command.ExecuteScalar()
            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Public Sub readAllAuthorizationSets(ByRef resultTable As DataTable, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim sqlInsert As String

        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            sqlInsert = "SELECT * FROM AuthorizationARVAL"

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

#End Region

End Class

' ************************************************
' $History: LT_01c.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 5.03.10    Time: 10:30
' Updated in $/CKAG/Applications/AppLeaseTrend/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.06.09   Time: 16:47
' Updated in $/CKAG/Applications/AppLeaseTrend/Lib
' ITA 2918 Z_M_Fehlende_Coc_Leasetr
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.06.09   Time: 14:58
' Updated in $/CKAG/Applications/AppLeaseTrend/Lib
' ITA 2918  Z_M_FEHLENDE_VERTRAGSNUMMERN, Z_M_BRIEFLEBENSLAUF_LT,
' Z_M_BRIEFERSTEINGANG_LEASETR, Z_M_BRIEFANFORDERUNG_ALLG,
' Z_M_UNANGEFORDERT_L, Z_M_KUNDENDATEN_LT, Z_M_VERSENDETE_ZB2_ENDG_LT,
' Z_M_VERSENDETE_ZB2_TEMP_LT, Z_M_BRIEF_TEMP_VERS_MAHN_002,
' Z_M_DATEN_OHNE_ZB2_LT
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:12
' Updated in $/CKAG/Applications/AppLeaseTrend/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 15:07
' Created in $/CKAG/Applications/AppLeaseTrend/Lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:22
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Lib
' ITA: 1440
' 
' *****************  Version 12  *****************
' User: Uha          Date: 2.07.07    Time: 18:14
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 11  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Lib
' 
' ************************************************
