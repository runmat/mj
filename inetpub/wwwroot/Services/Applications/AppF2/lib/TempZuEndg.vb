Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG

Public Class TempZuEndg
    Inherits BankBase


#Region " Declarations"
    Private m_tblFahrzeuge As DataTable
    Private m_tblFahrzeugeExcel As DataTable
    Private m_strFahrgestellnummer As String
    Private m_strKennzeichen As String
    Private m_strVersand As String
    Private m_strFahrgestellnummerSuche As String
    Private m_strVertragsnummer As String
    Private m_strObjektnummer As String
    Private m_strOrdernummer As String
    Private m_strVorgaenge As String
    Private m_tblAuftragsUebersicht As DataTable
    Private strPersonennummer As String
    Private m_strEQUNR As String
    Private m_strFleet20 As String
    Private m_strFleet21 As String
    Private m_strAbrufgrund As String
    Private m_AnBank As Boolean
    Private m_DatumVon As Date
    Private m_DatumBis As Date
#End Region

#Region " Properties"


    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeuge = Value
        End Set
    End Property

    Public Property FahrzeugeExcel() As DataTable
        Get
            Return m_tblFahrzeugeExcel
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeugeExcel = Value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property

    Public Property FahrgestellnummerSuche() As String
        Get
            Return m_strFahrgestellnummerSuche
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummerSuche = Value
        End Set
    End Property

    Public Property Ordernummer() As String
        Get
            Return m_strOrdernummer
        End Get
        Set(ByVal Value As String)
            m_strOrdernummer = Value
        End Set
    End Property

    Public Property EQUNR() As String
        Get
            Return m_strEQUNR
        End Get
        Set(ByVal Value As String)
            m_strEQUNR = Value
        End Set
    End Property

    Public Property Vertragsnummer() As String
        Get
            Return m_strVertragsnummer
        End Get
        Set(ByVal Value As String)
            m_strVertragsnummer = Value
        End Set
    End Property

    Public Property Objektnummer() As String
        Get
            Return m_strObjektnummer
        End Get
        Set(ByVal Value As String)
            m_strObjektnummer = Value
        End Set
    End Property

    Public Property Vorgaenge() As String
        Get
            Return m_strVorgaenge
        End Get
        Set(ByVal Value As String)
            m_strVorgaenge = Value
        End Set
    End Property

    Public ReadOnly Property AuftragsUebersicht() As DataTable
        Get
            Return m_tblAuftragsUebersicht
        End Get
    End Property

    Public Property personenennummer() As String
        Get
            Return strPersonennummer
        End Get
        Set(ByVal value As String)
            'personennummer immer mit führender 60 =FALSCH SAP=WEB!
            strPersonennummer = Right("0000000000" & value, 10)
        End Set
    End Property
    Public ReadOnly Property Abrufgrund(ByVal IDGrund As String) As String
        Get
            Dim cn As New SqlClient.SqlConnection
            Dim cmdAg As SqlClient.SqlCommand
            Dim dsAg As DataSet
            Dim adAg As SqlClient.SqlDataAdapter
            m_strAbrufgrund = ""
            Try
                cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
                cn.Open()

                dsAg = New DataSet()

                adAg = New SqlClient.SqlDataAdapter()

                cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                                "[WebBezeichnung]" & _
                                                "FROM CustomerAbrufgruende " & _
                                                "WHERE " & _
                                                "CustomerID = " & m_objUser.Customer.CustomerId.ToString & _
                                                " AND SAPWert = '" & IDGrund & "'", cn)
                cmdAg.CommandType = CommandType.Text
                'AbrufTyp: 'temp' oder 'endg'
                Dim dr As SqlClient.SqlDataReader
                dr = cmdAg.ExecuteReader
                While dr.Read
                    m_strAbrufgrund = dr("WebBezeichnung").ToString
                End While
            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try
            Return m_strAbrufgrund
        End Get
    End Property
    Public Property Fleet20() As String
        Get
            Return m_strFleet20
        End Get
        Set(ByVal Value As String)
            m_strFleet20 = Value
        End Set
    End Property
    Public Property Fleet21() As String
        Get
            Return m_strFleet21
        End Get
        Set(ByVal Value As String)
            m_strFleet21 = Value
        End Set
    End Property
    Public Property AnBank() As Boolean
        Get
            Return m_AnBank
        End Get
        Set(ByVal Value As Boolean)
            m_AnBank = Value
        End Set
    End Property

    Public Property DatumVon() As Date
        Get
            Return Me.m_DatumVon
        End Get
        Set(ByVal value As System.DateTime)
            Me.m_DatumVon = value
        End Set
    End Property
    Public Property DatumBis() As Date
        Get
            Return Me.m_DatumBis
        End Get
        Set(ByVal value As System.DateTime)
            Me.m_DatumBis = value
        End Set
    End Property
    Public Property Versand() As String
        Get
            Return m_strVersand
        End Get
        Set(ByVal Value As String)
            m_strVersand = Value
        End Set
    End Property
    Public Property Kennzeichen() As String
        Get
            Return m_strKennzeichen
        End Get
        Set(ByVal Value As String)
            m_strKennzeichen = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub
    Public Overrides Sub Change()

    End Sub
    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Änderung Versandstatus - Change47
        m_strClassAndMethod = "TempZuEndg.Change"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim tmpCustomer As String
            'If m_strCustomer = "0000000000" Then
            '    tmpCustomer = ""
            'Else
            '    tmpCustomer = Right("0000000000" & m_strCustomer, 10)
            'End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Abckennzeichen", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_EQUNR", m_strEQUNR)
                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_WEBUSER", Left(m_objUser.UserName, 12))
                myProxy.setImportParameter("I_ZBE20", m_strFleet20)
                myProxy.setImportParameter("I_ZBE21", m_strFleet21)

                myProxy.callBapi()



            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NOT_FOUND"
                        m_intStatus = -1
                        m_strMessage = "Equipment nicht gefunden."
                    Case "NO_UPDATE"
                        m_intStatus = -2
                        m_strMessage = "Equipment konnte nicht geändert werden."
                    Case "NO_MESSAGE"
                        m_intStatus = -3
                        m_strMessage = "CS-Meldung konnte nicht generiert werden."
                    Case Else
                        m_intStatus = -1112
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
    Public Sub ChangeBank(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        m_strClassAndMethod = "TempZuEndg.ChangeDez"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim tmpCustomer As String
            If m_strCustomer = "0000000000" Then
                tmpCustomer = ""
            Else
                tmpCustomer = Right("0000000000" & m_strCustomer, 10)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ENDG_VERS_AN_BANK_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_EQUNR", m_strEQUNR)
                myProxy.setImportParameter("I_AG", m_strKUNNR)
                myProxy.setImportParameter("I_WEBUSER", Left(m_objUser.UserName, 12))
                myProxy.setImportParameter("I_ZBE20", m_strFleet20)
                myProxy.setImportParameter("I_ZBE21", m_strFleet21)

                myProxy.callBapi()



            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NOT_FOUND"
                        m_intStatus = -1
                        m_strMessage = "Equipment nicht gefunden."
                    Case "NO_UPDATE"
                        m_intStatus = -2
                        m_strMessage = "Equipment konnte nicht geändert werden."
                    Case "NO_MESSAGE"
                        m_intStatus = -3
                        m_strMessage = "CS-Meldung konnte nicht generiert werden."
                    Case Else
                        m_intStatus = -1112
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try

        End If
    End Sub
    Public Sub UpdateFleet(ByVal Page As Page)
        m_strClassAndMethod = "TempZuEndg.UpdateFleet"

        Try
            m_intStatus = 0
            m_strMessage = ""

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_SAVE_BEMERKUNG_001", m_objApp, m_objUser, Page)

            myProxy.setImportParameter("I_EQUNR", m_strEQUNR)
            myProxy.setImportParameter("I_ZBE20", m_strFleet20)
            myProxy.setImportParameter("I_ZBE21", m_strFleet21)

            myProxy.callBapi()


        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "EQUI_NOT_FOUND"
                    m_intStatus = -1
                    m_strMessage = "Fehler beim Speichern."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select


        End Try

    End Sub

    Public Sub Report(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "TempZuEndg.Report"

        Dim tblFahrzeugeEquiSAP As DataTable
        Dim SapRow As DataRow


        Try
            m_intStatus = 0
            m_strMessage = ""


            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_NACHTR_ENDG_001", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            If strPersonennummer <> "0000000000" Then
                myProxy.setImportParameter("I_HAENDLER", Right("0000000000" & strPersonennummer, 10))
            End If


            If CStr(DatumVon) <> "00:00:00" AndAlso CStr(DatumBis) <> "00:00:00" Then
                myProxy.setImportParameter("I_VON", MakeDateSAP(m_DatumVon.ToShortDateString))
                myProxy.setImportParameter("I_BIS", MakeDateSAP(m_DatumBis.ToShortDateString))


            End If


            myProxy.callBapi()

            tblFahrzeugeEquiSAP = myProxy.getExportTable("GT_WEB")
            tblFahrzeugeEquiSAP.Columns.Add("Adresse", System.Type.GetType("System.String"))
            For Each SapRow In tblFahrzeugeEquiSAP.Rows
                SapRow("Adresse") = SapRow("Name1").ToString & ", " & SapRow("Post_Code1").ToString & " " & SapRow("City1").ToString & ", " & SapRow("Street").ToString & " " & SapRow("House_NUM1").ToString
                SapRow("KNRZE") = Right(SapRow("KNRZE").ToString, 2)
            Next


            m_tblFahrzeuge = CreateOutPut(tblFahrzeugeEquiSAP, strAppID)
            m_tblFahrzeugeExcel = CreateOutPut(tblFahrzeugeEquiSAP, Me.AppID)

            WriteLogEntry(True, "CHASSIS_NUM=" & m_strFahrgestellnummerSuche & ", KNRZE=, KONZS=" & m_objUser.KUNNR & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strVertragsnummer & ", ZZREFERENZ1=" & m_strOrdernummer, m_tblFahrzeugeExcel)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case "NO_HAENDLER"
                    m_intStatus = -2
                    m_strMessage = "Händler nicht vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select

            WriteLogEntry(False, "CHASSIS_NUM=" & m_strFahrgestellnummerSuche & ", KNRZE=, KONZS=" & m_objUser.KUNNR & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strVertragsnummer & ", ZZREFERENZ1=" & m_strOrdernummer & " , " & Replace(m_strMessage, "<br>", " "), m_tblFahrzeugeExcel)
        End Try
    End Sub

    Public Overrides Sub Show() 'Muss bleiben da von der Sub abgeleitet wird

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Änderung Versandstatus - Change47 
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        m_intStatus = 0
        m_strMessage = String.Empty

        Try

            'weil bei bankbase führende nullen angehängt werden, verträgt das bapi aber nicht wenn ohne Händlernummer gesucht wird.
            Dim tmpCustomer As String
            If m_strCustomer = "0000000000" Then
                tmpCustomer = ""
            Else
                tmpCustomer = Right("0000000000" & m_strCustomer, 10)
            End If

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Dad_Daten_Einaus_Report_003", m_objApp, m_objUser, page)

            myProxy.setImportParameter("AG", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("HAENDLER", tmpCustomer)
            myProxy.setImportParameter("ABCKZ", "1")
            myProxy.setImportParameter("CHASSIS_NUM", m_strFahrgestellnummer)
            myProxy.setImportParameter("LIZNR", m_strVertragsnummer)

            myProxy.callBapi()

            Dim tblTemp2 As DataTable = myProxy.getExportTable("EINNEU")
            tblTemp2.Columns("ABCKZ").MaxLength = 25
            tblTemp2.Columns("ZZVGRUND").MaxLength = 150
            tblTemp2.Columns.Add("Adresse", System.Type.GetType("System.String"))
            tblTemp2.AcceptChanges()
            Dim rowTemp As DataRow
            For Each rowTemp In tblTemp2.Rows
                Select Case CStr(rowTemp("ABCKZ"))
                    Case "1"
                        rowTemp("ABCKZ") = "temporär"
                    Case "2"
                        rowTemp("ABCKZ") = "endgültig"
                    Case "5"
                        rowTemp("ABCKZ") = "Händler Zulassung"
                End Select
                Dim strgrund As String
                If CStr(rowTemp("ZZVGRUND")).Length > 0 Then
                    strgrund = Abrufgrund(CStr(rowTemp("ZZVGRUND")))
                Else
                    strgrund = "keine Auswahl"
                End If
                rowTemp("ZZVGRUND") = strgrund
                rowTemp("Adresse") = rowTemp("Name1").ToString & ", <br />" & _
                                        rowTemp("Post_Code1").ToString & " " & rowTemp("City1").ToString & ", <br />" & _
                                        rowTemp("Street").ToString & " " & rowTemp("House_NUM1").ToString
                rowTemp("KNRZE") = Right(rowTemp("KNRZE").ToString, 2)
            Next


            If Not m_strObjektnummer Is Nothing AndAlso m_strObjektnummer.Length > 0 Then
                Dim objCol As DataColumn
                m_tblFahrzeuge = New DataTable()
                For Each objCol In tblTemp2.Columns
                    m_tblFahrzeuge.Columns.Add(objCol.ColumnName, objCol.DataType)
                Next
                Dim objRow As DataRow
                Dim objDataRows() As DataRow
                Dim myRow As DataRow
                'objRow = tblTemp2.Select("ZZREFERENZ1=" & m_strObjektnummer)
                objDataRows = tblTemp2.Select("ZZREFERENZ1='" & m_strObjektnummer & "'")
                If objDataRows.Length > 0 Then
                    objRow = objDataRows(0)
                    myRow = m_tblFahrzeuge.NewRow()
                    Dim i As Integer
                    For i = 0 To tblTemp2.Columns.Count - 1
                        myRow(i) = objRow(i)
                    Next
                    m_tblFahrzeuge.Rows.Add(myRow)
                End If
            Else
                m_tblFahrzeuge = tblTemp2.Copy
            End If

            m_tblFahrzeuge.Columns.Add("Status", System.Type.GetType("System.String"))
            m_tblFahrzeuge.Columns.Add("Fleet20", System.Type.GetType("System.String"))
            m_tblFahrzeuge.Columns.Add("Fleet21", System.Type.GetType("System.String"))
            'für Grid anzeige
            m_tblFahrzeuge = CreateOutPut(m_tblFahrzeuge, AppID)

            'customer setzen wenn keiner vorhanden, wenn mehrere Händler pech gehabt falsche selektion in sap
            m_strCustomer = tblTemp2.Rows(0)("Kunnr").ToString

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1110
                    m_strMessage = "Die Selektion ergab keine Treffer."
                Case "HAENDLER_NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Der Händler wurde nicht gefunden."
                Case Else
                    m_intStatus = -1112
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
        End Try

    End Sub
#End Region
End Class
