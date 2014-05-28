Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports System.Data.OleDb
Imports CKG.Base.Kernel.Common

''' <summary>
''' Business Logic für Report28
''' </summary>
Public Class ec_28
    Inherits Base.Business.DatenimportBase

#Region " Declarations "

    Private m_EingangZB2_von As DateTime = DateTime.MinValue
    Private m_EingangZB2_bis As DateTime = DateTime.MinValue
    Private m_EingangFzg_von As DateTime = DateTime.MinValue
    Private m_EingangFzg_bis As DateTime = DateTime.MinValue
    Private m_BereitmFzg_von As DateTime = DateTime.MinValue
    Private m_BereitmFzg_bis As DateTime = DateTime.MinValue
    Private m_ZulDat_von As DateTime = DateTime.MinValue
    Private m_ZulDat_bis As DateTime = DateTime.MinValue
    Private m_tblSelFahrzeuge As DataTable
    Private m_tblHersteller As DataTable
    Private m_tblPDIStandorte As DataTable
    Private m_tblStati As DataTable
    Private m_strExceldatei As String

#End Region

#Region " Properties "

    Public Property EingangZB2_von As DateTime
        Get
            Return m_EingangZB2_von
        End Get
        Set(value As DateTime)
            m_EingangZB2_von = value
        End Set
    End Property

    Public Property EingangZB2_bis As DateTime
        Get
            Return m_EingangZB2_bis
        End Get
        Set(value As DateTime)
            m_EingangZB2_bis = value
        End Set
    End Property

    Public Property EingangFzg_von As DateTime
        Get
            Return m_EingangFzg_von
        End Get
        Set(value As DateTime)
            m_EingangFzg_von = value
        End Set
    End Property

    Public Property EingangFzg_bis As DateTime
        Get
            Return m_EingangFzg_bis
        End Get
        Set(value As DateTime)
            m_EingangFzg_bis = value
        End Set
    End Property

    Public Property BereitmFzg_von As DateTime
        Get
            Return m_BereitmFzg_von
        End Get
        Set(value As DateTime)
            m_BereitmFzg_von = value
        End Set
    End Property

    Public Property BereitmFzg_bis As DateTime
        Get
            Return m_BereitmFzg_bis
        End Get
        Set(value As DateTime)
            m_BereitmFzg_bis = value
        End Set
    End Property

    Public Property ZulDat_von As DateTime
        Get
            Return m_ZulDat_von
        End Get
        Set(value As DateTime)
            m_ZulDat_von = value
        End Set
    End Property

    Public Property ZulDat_bis As DateTime
        Get
            Return m_ZulDat_bis
        End Get
        Set(value As DateTime)
            m_ZulDat_bis = value
        End Set
    End Property

    Public Property SelFahrzeuge As DataTable
        Get
            Return m_tblSelFahrzeuge
        End Get
        Set(value As DataTable)
            m_tblSelFahrzeuge = value
        End Set
    End Property

    Public Property Hersteller As DataTable
        Get
            Return m_tblHersteller
        End Get
        Set(value As DataTable)
            m_tblHersteller = value
        End Set
    End Property

    Public Property PDIStandorte As DataTable
        Get
            Return m_tblPDIStandorte
        End Get
        Set(value As DataTable)
            m_tblPDIStandorte = value
        End Set
    End Property

    Public Property Stati As DataTable
        Get
            Return m_tblStati
        End Get
        Set(value As DataTable)
            m_tblStati = value
        End Set
    End Property

    Public Property ExcelDatei As String
        Get
            Return m_strExceldatei
        End Get
        Set(value As String)
            m_strExceldatei = value
        End Set
    End Property

#End Region

#Region " Methods "

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        InitFahrzeugTable()
    End Sub

    ''' <summary>
    ''' Spalten der Fahrzeugtabelle anlegen
    ''' </summary>
    Private Sub InitFahrzeugTable()
        SelFahrzeuge = S.AP.GetImportTableWithInit("Z_DPM_LIST_POOLS_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))

        'SelFahrzeuge = New DataTable()
        'SelFahrzeuge.Columns.Add("Fahrgestellnummer", GetType(String))
        'SelFahrzeuge.Columns.Add("Kennzeichen", GetType(String))
        'SelFahrzeuge.Columns.Add("ZB2Nummer", GetType(String))
        'SelFahrzeuge.Columns.Add("ModelId", GetType(String))
        'SelFahrzeuge.Columns.Add("Unitnummer", GetType(String))
        'SelFahrzeuge.Columns.Add("Auftragsnummer", GetType(String))
        'SelFahrzeuge.Columns.Add("BatchId", GetType(String))
        'SelFahrzeuge.Columns.Add("SippCode", GetType(String))
        'SelFahrzeuge.Columns.Add("Hersteller", GetType(String))
        'SelFahrzeuge.Columns.Add("Fahrzeugart", GetType(String))
        'SelFahrzeuge.Columns.Add("PdiStandort", GetType(String))
        'SelFahrzeuge.Columns.Add("Status", GetType(String))
        'SelFahrzeuge.AcceptChanges()
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub

    ''' <summary>
    ''' Bestandsdaten laden
    ''' </summary>
    ''' <param name="strAppID"></param>
    ''' <param name="strSessionID"></param>
    ''' <param name="page"></param>
    ''' <remarks>Z_DPM_LIST_POOLS_001</remarks>
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "EC_28.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            S.AP.Init("Z_DPM_LIST_POOLS_001", "I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))

            If EingangZB2_von > DateTime.MinValue AndAlso EingangZB2_bis > DateTime.MinValue Then
                S.AP.SetImportParameter("I_ERDAT_EQUI_VON", EingangZB2_von)
                S.AP.SetImportParameter("I_ERDAT_EQUI_BIS", EingangZB2_bis)
            End If
            If EingangFzg_von > DateTime.MinValue AndAlso EingangFzg_bis > DateTime.MinValue Then
                S.AP.SetImportParameter("I_ZZDAT_EIN_VON", EingangFzg_von)
                S.AP.SetImportParameter("I_ZZDAT_EIN_BIS", EingangFzg_bis)
            End If
            If BereitmFzg_von > DateTime.MinValue AndAlso BereitmFzg_bis > DateTime.MinValue Then
                S.AP.SetImportParameter("I_ZZDAT_BER_VON", BereitmFzg_von)
                S.AP.SetImportParameter("I_ZZDAT_BER_BIS", BereitmFzg_bis)
            End If
            If ZulDat_von > DateTime.MinValue AndAlso ZulDat_bis > DateTime.MinValue Then
                S.AP.SetImportParameter("I_REPLA_DATE_VON", ZulDat_von)
                S.AP.SetImportParameter("I_REPLA_DATE_BIS", ZulDat_bis)
            End If

            'Dim tblInput As DataTable = S.AP.GetImportTable("GT_WEB")
            S.AP.SetImportTable("GT_WEB", m_tblSelFahrzeuge)

            'For i As Integer = 0 To (m_tblSelFahrzeuge.Rows.Count - 1)
            '    Dim dRow As DataRow = m_tblSelFahrzeuge.Rows(i)
            '    Dim newRow As DataRow = tblInput.NewRow()

            '    newRow("CHASSIS_NUM") = dRow("Fahrgestellnummer")
            '    newRow("LICENSE_NUM") = dRow("Kennzeichen")
            '    newRow("ZZREFERENZ1") = dRow("Unitnummer")
            '    newRow("LIZNR") = dRow("Auftragsnummer")
            '    newRow("ZUNIT_NR_BIS") = dRow("BatchId")
            '    newRow("ZZSIPP") = dRow("SippCode")
            '    newRow("ZZMODELL") = dRow("ModelId")
            '    newRow("ZZHERST_TEXT") = dRow("Hersteller")
            '    newRow("TIDNR") = dRow("ZB2Nummer")
            '    newRow("KUNPDI") = dRow("PdiStandort")
            '    newRow("STATUS_TEXT") = dRow("Status")

            '    tblInput.Rows.Add(newRow)
            'Next

            'tblInput.AcceptChanges()

            S.AP.Execute()

            Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB")

            'CreateOutPut(tblTemp, strAppID)
            m_tblResult = DataTableHelper.TranslateDataColumns(tblTemp, strAppID, m_objApp)
            
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").TrimEnd()
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try
    End Sub

    ''' <summary>
    ''' Excel-Datei auslesen
    ''' </summary>
    ''' <param name="tblName"></param>
    ''' <remarks></remarks>
    Public Sub LoadExcelData(ByVal tblName As String)
        Dim rowData As DataRow
        Dim newRow As DataRow
        Dim strFahrgestellnummer As String             'Spalte 1 : Fahrgestellnummer
        Dim strKennzeichen As String = ""          'Spalte 2 : Kennzeichen
        Dim strZB2Nummer As String = ""   'Spalte 3 : ZB2Nummer
        Dim strModelId As String = ""     'Spalte 4 : ModelId
        Dim strUnitnummer As String = ""     'Spalte 5 : Unitnummer
        Dim strAuftragsnummer As String = ""     'Spalte 6 : Auftragsnummer
        Dim strBatchId As String = ""     'Spalte 7 : BatchId
        Dim strSippCode As String = ""     'Spalte 8 : SippCode

        Try
            m_intStatus = 0

            Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                     "Data Source=" & m_strExceldatei & ";" & _
                     "Extended Properties=""Excel 8.0;HDR=NO;"""

            Dim objConn As New OleDbConnection(sConnectionString)
            objConn.Open()

            Dim objCmdSelect As New OleDbCommand("SELECT * FROM  [" & tblName & "$]", objConn)

            Dim objAdapter1 As New OleDbDataAdapter()
            objAdapter1.SelectCommand = objCmdSelect

            Dim objDataset1 As New DataSet()
            objAdapter1.Fill(objDataset1, "XLData")

            For i As Integer = 1 To (objDataset1.Tables(0).Rows.Count - 1)
                rowData = objDataset1.Tables(0).Rows(i)

                If Not (TypeOf rowData(0) Is System.DBNull) Then

                    strFahrgestellnummer = CStr(rowData(0))
                    If Not (TypeOf rowData(1) Is System.DBNull) Then
                        strKennzeichen = CStr(rowData(1))
                    End If
                    If Not (TypeOf rowData(2) Is System.DBNull) Then
                        strZB2Nummer = CStr(rowData(2))
                    End If
                    If Not (TypeOf rowData(3) Is System.DBNull) Then
                        strModelId = CStr(rowData(3))
                    End If
                    If Not (TypeOf rowData(4) Is System.DBNull) Then
                        strUnitnummer = CStr(rowData(4))
                    End If
                    If Not (TypeOf rowData(5) Is System.DBNull) Then
                        strAuftragsnummer = CStr(rowData(5))
                    End If
                    If Not (TypeOf rowData(6) Is System.DBNull) Then
                        strBatchId = CStr(rowData(6))
                    End If
                    If Not (TypeOf rowData(7) Is System.DBNull) Then
                        strSippCode = CStr(rowData(7))
                    End If

                    newRow = m_tblSelFahrzeuge.NewRow()

                    'newRow("Fahrgestellnummer") = strFahrgestellnummer
                    'newRow("Kennzeichen") = strKennzeichen
                    'newRow("ZB2Nummer") = strZB2Nummer
                    'newRow("ModelId") = strModelId
                    'newRow("Unitnummer") = strUnitnummer
                    'newRow("Auftragsnummer") = strAuftragsnummer
                    'newRow("BatchId") = strBatchId
                    'newRow("SippCode") = strSippCode

                    newRow("CHASSIS_NUM") = strFahrgestellnummer
                    newRow("LICENSE_NUM") = strKennzeichen
                    newRow("ZZREFERENZ1") = strUnitnummer
                    newRow("LIZNR") = strAuftragsnummer
                    newRow("ZUNIT_NR_BIS") = strBatchId
                    newRow("ZZSIPP") = strSippCode
                    newRow("ZZMODELL") = strModelId
                    newRow("TIDNR") = strZB2Nummer
                   
                    m_tblSelFahrzeuge.Rows.Add(newRow)

                End If
            Next

            objConn.Close()

        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = ex.Message
        End Try

    End Sub

    ''' <summary>
    ''' Herstellerauswahl laden
    ''' </summary>
    ''' <param name="strAppID"></param>
    ''' <param name="strSessionID"></param>
    ''' <remarks>Z_M_HERSTELLERGROUP</remarks>
    Public Overloads Sub FillHersteller(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "EC_28.FillHersteller"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            S.AP.InitExecute("Z_M_HERSTELLERGROUP")

            m_tblHersteller = S.AP.GetExportTable("T_HERST")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblHersteller, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblHersteller, False)

        End Try
    End Sub

    ''' <summary>
    ''' PDI-Standortauswahl laden
    ''' </summary>
    ''' <param name="strAppID"></param>
    ''' <param name="strSessionID"></param>
    ''' <remarks>Z_DPM_LIST_PDI_001</remarks>
    Public Overloads Sub FillPDIStandorte(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "EC_28.FillPDIStandorte"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            S.AP.InitExecute("Z_DPM_LIST_PDI_001", "I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))

            m_tblPDIStandorte = S.AP.GetExportTable("GT_WEB")

            m_tblPDIStandorte.Columns.Add("KUNPDINR", GetType(System.Int32))

            For Each dRow As DataRow In m_tblPDIStandorte.Rows
                'Für nummerische Sortierung nach PDI-Nr.
                Dim intKunPdiNr As Integer = 0
                Int32.TryParse(dRow("KUNPDI").ToString().Replace("PDI", ""), intKunPdiNr)
                dRow("KUNPDINR") = intKunPdiNr
            Next

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblPDIStandorte, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblPDIStandorte, False)

        End Try
    End Sub

    ''' <summary>
    ''' Statusauswahl laden
    ''' </summary>
    ''' <param name="strAppID"></param>
    ''' <param name="strSessionID"></param>
    ''' <remarks>Z_DPM_READ_AUFTR_006</remarks>
    Public Overloads Sub FillStati(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "EC_28.FillStati"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            S.AP.InitExecute("Z_DPM_READ_AUFTR_006", "I_KUNNR,I_KENNUNG", m_objUser.KUNNR.PadLeft(10, "0"c), "STATUS")

            m_tblStati = S.AP.GetExportTable("GT_OUT")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblStati, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblStati, False)

        End Try
    End Sub

#End Region

End Class


