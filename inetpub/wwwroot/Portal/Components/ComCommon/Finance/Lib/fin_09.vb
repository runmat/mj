Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

<Serializable()> Public Class fin_09
    REM § Lese-/Schreibfunktion,, 
    REM § Change BAPI:Z_M_Abckennzeichen
    REM § Report BAPI:Z_M_Haendlerbestand
    REM § Show  BAPI:Z_M_Faellige_Fahrzdok
    REM § FILL BAPI: Z_Dad_Daten_Einaus_Report_003
    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_tblFahrzeuge As DataTable
    Private m_tblFahrzeugeExcel As DataTable
    Private m_strFahrgestellnummer As String
    Private m_strFahrgestellnummerSuche As String
    Private m_strVertragsnummer As String
    Private m_strObjektnummer As String
    Private m_strOrdernummer As String
    Private m_strVorgaenge As String
    Private m_tblAuftragsUebersicht As DataTable
    Private strPersonennummer As String
    Private m_strEQUNR As String
    Private m_strAbrufgrund As String
    Private mLastschrifterzeugung As String = "N"

#End Region

#Region " Properties"
    Public Property Lastschrifterzeugung() As String
        Get
            Return mLastschrifterzeugung
        End Get
        Set(ByVal value As String)
            mLastschrifterzeugung = value
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

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub

    Public Overrides Sub Change()
        'Änderung Versandstatus - Change47
        m_strClassAndMethod = "fin_09.Change"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                ClearError()

                S.AP.Init("Z_M_Abckennzeichen", "I_EQUNR, I_KUNNR, I_LEV",
                                 EQUNR, Right("0000000000" & m_objUser.KUNNR, 10), Lastschrifterzeugung)

                Dim userName As String = m_objUser.UserName
                If (Len(userName) > 12) Then
                    userName = Left(userName, 12)
                End If
                S.AP.SetImportParameter("I_WEBUSER", userName)

                S.AP.Execute()

            Catch ex As Exception
                Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

                If errormessage.Contains("NOT_FOUND") Then
                    RaiseError("-1", "Equipment nicht gefunden.")
                ElseIf errormessage.Contains("NO_UPDATE") Then
                    RaiseError("-2", "Equipment konnte nicht geändert werden.")
                ElseIf errormessage.Contains("NO_MESSAGE") Then
                    RaiseError("-3", "CS-Meldung konnte nicht generiert werden.")
                Else
                    RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                End If

                WriteLogEntry(False, "CHASSIS_NUM=" & m_strFahrgestellnummerSuche & ", KNRZE=, KONZS=" & m_objUser.KUNNR & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strVertragsnummer & ", ZZREFERENZ1=" & m_strOrdernummer & " , " & Replace(m_strMessage, "<br>", " "), m_tblFahrzeugeExcel)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Report()
        m_strClassAndMethod = "fin_09.Report"

        Dim tblFahrzeugeEquiSap As DataTable
        Dim sapRow As DataRow

        Try
            ClearError()

            S.AP.InitExecute("Z_M_Haendlerbestand", "I_AG, I_HAENDLER",
                             Right("0000000000" & m_objUser.KUNNR, 10), Right("0000000000" & strPersonennummer, 10))

            tblFahrzeugeEquiSap = S.AP.GetExportTable("GT_WEB")
            tblFahrzeugeEquiSap.Columns("Zzkkber").MaxLength = 25
            For Each sapRow In tblFahrzeugeEquiSap.Rows

                Select Case CStr(sapRow.Item("Zzkkber"))
                    Case "0001"
                        sapRow.Item("Zzkkber") = "Standard temporär"
                    Case "0002"
                        sapRow.Item("Zzkkber") = "Standard endgültig"
                    Case "0005"
                        sapRow.Item("Zzkkber") = "Händler Zulassung"
                End Select
            Next

            m_tblFahrzeuge = CreateOutPut(tblFahrzeugeEquiSap, AppID)
            m_tblFahrzeugeExcel = CreateOutPut(tblFahrzeugeEquiSap, AppID)

            WriteLogEntry(True, "CHASSIS_NUM=" & m_strFahrgestellnummerSuche & ", KNRZE=, KONZS=" & m_objUser.KUNNR & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strVertragsnummer & ", ZZREFERENZ1=" & m_strOrdernummer, m_tblFahrzeugeExcel)

        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NO_DATA") Then
                RaiseError("-1", "Keine Eingabedaten vorhanden.")
            ElseIf errormessage.Contains("NO_HAENDLER") Then
                RaiseError("-2", "Händler nicht vorhanden.")
            Else
                RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End If

            WriteLogEntry(False, "CHASSIS_NUM=" & m_strFahrgestellnummerSuche & ", KNRZE=, KONZS=" & m_objUser.KUNNR & ", KUNNR=" & m_strCustomer & ", LIZNR=" & m_strVertragsnummer & ", ZZREFERENZ1=" & m_strOrdernummer & " , " & Replace(m_strMessage, "<br>", " "), m_tblFahrzeugeExcel)
        End Try
    End Sub

    Public Overrides Sub Show() 'Muss bleiben da von der Sub abgeleitet wird

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        'Änderung Versandstatus - Change47 
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        ClearError()

        Try
            'weil bei bankbase führende nullen angehängt werden, verträgt das bapi aber nicht wenn ohne Händlernummer gesucht wird.
            Dim tmpCustomer As String
            If m_strCustomer = "0000000000" Then
                tmpCustomer = ""
            Else
                tmpCustomer = Right("0000000000" & m_strCustomer, 10)
            End If

            S.AP.InitExecute("Z_Dad_Daten_Einaus_Report_003", "AG, HAENDLER, ABCKZ, CHASSIS_NUM, LIZNR",
                             Right("0000000000" & m_objUser.KUNNR, 10), tmpCustomer, "1", m_strFahrgestellnummer, m_strVertragsnummer)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("EINNEU")
            tblTemp2.Columns("ABCKZ").MaxLength = 25
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
            Next

            tblTemp2.Columns("ZZVGRUND").MaxLength = 150
            tblTemp2.AcceptChanges()
            For Each rowTemp In tblTemp2.Rows
                Dim strgrund As String
                If CStr(rowTemp("ZZVGRUND")).Length > 0 Then
                    strgrund = Abrufgrund(CStr(rowTemp("ZZVGRUND")))
                Else
                    strgrund = "keine Auswahl"
                End If
                rowTemp("ZZVGRUND") = strgrund
            Next
            tblTemp2.Columns.Add("Adresse", System.Type.GetType("System.String"))
            For Each rowTemp In tblTemp2.Rows
                rowTemp("Adresse") = rowTemp("Name1").ToString & ", " & rowTemp("Post_Code1").ToString & " " & rowTemp("City1").ToString & ", " & rowTemp("Street").ToString & " " & rowTemp("House_NUM1").ToString
            Next
            If Not m_strObjektnummer Is Nothing AndAlso m_strObjektnummer.Length > 0 Then
                Dim objCol As DataColumn
                m_tblFahrzeuge = New DataTable()

                For Each objCol In tblTemp2.Columns
                    m_tblFahrzeuge.Columns.Add(objCol.ColumnName, objCol.DataType)
                Next

                Dim objDataRows() As DataRow = tblTemp2.Select("ZZREFERENZ1='" & m_strObjektnummer & "'")
                If objDataRows.Length > 0 Then
                    Dim objRow As DataRow = objDataRows(0)
                    Dim myRow As DataRow = m_tblFahrzeuge.NewRow()
                    Dim i As Integer

                    For i = 0 To tblTemp2.Columns.Count - 1
                        myRow(i) = objRow(i)
                    Next

                    m_tblFahrzeuge.Rows.Add(myRow)
                End If
            Else
                m_tblFahrzeuge = tblTemp2.Copy
            End If
            'für Grid anzeige
            m_tblFahrzeuge = CreateOutPut(m_tblFahrzeuge, AppID)

            'customer setzen wenn keiner vorhanden, wenn mehrere Händler pech gehabt falsche selektion in sap
            If tblTemp2.Rows.Count > 0 Then
                m_strCustomer = tblTemp2.Rows(0)("Kunnr").ToString
            Else
                RaiseError("-1113", "Keine Daten gefunden!")
            End If

        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NO_DATA") Then
                RaiseError("-1110", "Die Selektion ergab keine Treffer.")
            ElseIf errormessage.Contains("NO_HAENDLER") Then
                RaiseError("-1111", "Der Händler wurde nicht gefunden.")
            Else
                RaiseError("-1112", "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")")
            End If

        End Try
    End Sub

#End Region

End Class


' ************************************************
' $History: fin_09.vb $
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 23.12.09   Time: 16:36
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 13.08.09   Time: 16:09
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 12.08.09   Time: 17:49
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 15.07.09   Time: 11:29
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' bugfix change47 ZZVGRUND maxlength
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 24.06.09   Time: 13:11
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_Dad_Daten_Einaus_Report_003
' 
' *****************  Version 9  *****************
' User: Dittbernerc  Date: 18.06.09   Time: 15:39
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 - abschaltung .net connector
' 
' BAPIS:
' 
' Z_M_Haendlerbestand
' Z_M_Faellige_Fahrzdok
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 17.06.09   Time: 16:45
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' .net Connector Umstellung
' 
' Bapi:
' Z_M_Haendlerbestand - in Bearbeitung
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 17.09.08   Time: 12:39
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2242 fertig
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 3.06.08    Time: 16:51
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 8.05.08    Time: 16:20
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Bugfix bei Abfrage von Abrufgründen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.04.08   Time: 14:45
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 22  *****************
' User: Jungj        Date: 11.04.08   Time: 8:17
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1842
' 
' *****************  Version 21  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 20  *****************
' User: Jungj        Date: 5.03.08    Time: 15:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' akf anpassungen 1733
' 
' *****************  Version 19  *****************
' User: Jungj        Date: 5.03.08    Time: 14:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' akf Änderungen 1733
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 1.03.08    Time: 14:50
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' AKF ÄNDERUNGEN X
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 1.03.08    Time: 13:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Änderung der Händlernummer auf 10 stellen mit führenden 0 
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 26.02.08   Time: 16:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1733
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 12.02.08   Time: 10:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' akf anpassungen
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 12.02.08   Time: 9:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 12.02.08   Time: 8:53
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 4.02.08    Time: 13:22
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA: 1677
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 2.02.08    Time: 15:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' akf änderungen
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 2.02.08    Time: 13:23
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 22.01.08   Time: 9:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Doku
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 17.01.08   Time: 12:38
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA: 1617
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 8.01.08    Time: 17:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' BugFIX RTFS
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 8.01.08    Time: 16:13
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Bugfix RTFS
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 8.01.08    Time: 12:38
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA: 1515
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 7.01.08    Time: 17:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 20.12.07   Time: 10:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.12.07   Time: 11:46
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1464/1498 Testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 18.12.07   Time: 8:25
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1464/1498  hinzugefügt, Proxy neu erstellt + Bapi Händlerbestand
' 
' ************************************************
