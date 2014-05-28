Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class AppF2_05
    REM § Lese-/Schreibfunktion, 
    REM § Show - BAPI: Z_M_Faellige_Fahrzdok
    REM § Change - BAPI: Z_M_Faellige_Equipments_Update

    Inherits BankBase

#Region " Declarations"
    Private m_strHaendler As String
    Private m_tblAuftragsUebersicht As DataTable
    Private m_tblAuftraegeAlle As DataTable
    Private m_tblAuftraege As DataTable
    Private m_tblRaw As DataTable
    Private m_blnChangeMemo As Boolean
    Private m_blnChangeFaelligkeit As Boolean
    Private strPersonennummer As String = ""
    Private strZZREFERENZ1 As String
#End Region

#Region " Properties"
    Public ReadOnly Property AuftraegeAlle() As DataTable
        Get
            Return m_tblAuftraegeAlle
        End Get
    End Property

    Public ReadOnly Property AuftragsUebersicht() As DataTable
        Get
            Return m_tblAuftragsUebersicht
        End Get
    End Property

    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return m_tblAuftraege
        End Get
    End Property

    Public Property personenennummer() As String
        Get
            Return strPersonennummer
        End Get
        Set(ByVal value As String)
            strPersonennummer = value
        End Set
    End Property

    Public Property zzreferenz1() As String
        Get
            Return strZZREFERENZ1
        End Get
        Set(ByVal value As String)
            strZZREFERENZ1 = value.Trim(" "c)
        End Set
    End Property

    Public Property Haendler() As String
        Get
            Return m_strHaendler
        End Get
        Set(ByVal Value As String)
            m_strHaendler = Value
            m_tblAuftraege = m_tblAuftraegeAlle.Copy
            Dim i As Int32
            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1
                If Not m_tblAuftraege.Rows(i)("Händler").ToString = m_strHaendler Then
                    m_tblAuftraege.Rows(i).Delete()
                End If
            Next
            m_tblResultExcel = m_tblAuftraege.Copy
            For i = 0 To m_tblResultExcel.Columns.Count - 1
                Dim s As String
                s = m_tblResultExcel.Columns.Item(i).Caption
            Next
            m_tblResultExcel.Columns.Remove("EQUNR")

            m_tblAuftraege.Columns.Add("DelayedPayment", System.Type.GetType("System.Boolean"))
            m_tblAuftraege.Columns.Add("Faelligkeit", System.Type.GetType("System.Boolean"))
            m_tblAuftraege.Columns.Add("faellig am alt", System.Type.GetType("System.DateTime"))
            m_tblAuftraege.Columns.Add("FaelligkeitString", System.Type.GetType("System.String"))
            m_tblAuftraege.Columns.Add("Memo alt", System.Type.GetType("System.String"))
            m_tblAuftraege.Columns.Add("MemoString", System.Type.GetType("System.String"))
            m_tblAuftraege.Columns.Add("DatumAendern", System.Type.GetType("System.Boolean"))

            Dim rowTemp As DataRow

            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1
                rowTemp = m_tblAuftraege.Rows(i)
                rowTemp.Item("Händler") = rowTemp.Item("Händler").ToString
                rowTemp("Memo alt") = rowTemp("Memo").ToString

                rowTemp("Faelligkeit") = False
                rowTemp("FaelligkeitString") = "ZZZZZZZ"

                Select Case rowTemp("Kontingentart").ToString
                    Case "0001"
                        rowTemp("Kontingentart") = "Standard temporär"
                        rowTemp("DelayedPayment") = False
                    Case "0002"
                        rowTemp("Kontingentart") = "Standard endgültig"
                        rowTemp("DelayedPayment") = False

                End Select
                If Not rowTemp("Memo").ToString = String.Empty Then
                    rowTemp("MemoString") = "Ändern"
                Else
                    rowTemp("MemoString") = "Erfassen"
                End If
                rowTemp("DatumAendern") = False
            Next
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Security.User, ByRef objApp As Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_strHaendler = ""
        m_blnChangeFaelligkeit = False
        m_blnChangeMemo = False
    End Sub


    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)


        Dim tblAuftragsUebersichtSAP As DataTable
        Dim rowAuftragsUebersichtSAP As DataRow
        Dim tblAuftraegeSAP As New DataTable

        Dim i As Int32
        Dim rowTemp As DataRow

        Try
            m_intStatus = 0
            m_strMessage = ""

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Faellige_Fahrzdok", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("I_HAENDLER", personenennummer)
            myProxy.setImportParameter("I_VKORG", "1510")

            myProxy.callBapi()

            tblAuftragsUebersichtSAP = myProxy.getExportTable("GT_ANZ")
            tblAuftraegeSAP = myProxy.getExportTable("GT_WEB")

            'keine Spaltenübersetzung, da Zahlenwerte die dann auch sortiert werden müssten
            'm_tblAuftragsUebersicht = CreateOutPut(m_tblAuftragsUebersicht, Me.AppID)

            m_tblAuftragsUebersicht = New DataTable()

            m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
            m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
            m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard endgültig", System.Type.GetType("System.Int32"))
            m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard temporär", System.Type.GetType("System.Int32"))
            m_tblAuftragsUebersicht.Columns.Add("Anzahl Flottengeschäft", System.Type.GetType("System.Int32"))
            m_tblAuftragsUebersicht.Columns.Add("Ort", System.Type.GetType("System.String"))
            m_tblAuftragsUebersicht.Columns.Add("ZZREFERENZ1", System.Type.GetType("System.String"))

            For i = 0 To tblAuftragsUebersichtSAP.Rows.Count - 1
                rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Rows(i)
                If IsNumeric(rowAuftragsUebersichtSAP("Zaehler_01")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_02")) And IsNumeric(rowAuftragsUebersichtSAP("Zaehler_04")) Then
                    If CInt(rowAuftragsUebersichtSAP("Zaehler_01")) + CInt(rowAuftragsUebersichtSAP("Zaehler_02")) + CInt(rowAuftragsUebersichtSAP("Zaehler_04")) > 0 Then
                        rowTemp = m_tblAuftragsUebersicht.NewRow
                        rowTemp("Händlernummer") = CStr(rowAuftragsUebersichtSAP("Kunnr"))
                        rowTemp("Händlername") = rowAuftragsUebersichtSAP("Name1_Zf")
                        rowTemp("Anzahl Standard endgültig") = CInt(rowAuftragsUebersichtSAP("Zaehler_02"))
                        rowTemp("Anzahl Standard temporär") = CInt(rowAuftragsUebersichtSAP("Zaehler_01"))
                        rowTemp("Anzahl Flottengeschäft") = CInt(rowAuftragsUebersichtSAP("Zaehler_04"))
                        rowTemp("Ort") = rowAuftragsUebersichtSAP("Ort01_Zf")
                        rowTemp("ZZREFERENZ1") = rowAuftragsUebersichtSAP("Zzreferenz1")
                        m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                    End If
                End If
            Next

            m_tblRaw = tblAuftraegeSAP

            m_tblRaw.Columns.Add("AbrufGrundX", GetType(System.String), "")


            Dim j As Integer

            For j = m_tblRaw.Rows.Count - 1 To 0 Step -1

                If (TypeOf (m_tblRaw.Rows(j)("ZZTMPDT")) Is System.DBNull) OrElse (CType(m_tblRaw.Rows(j)("ZZTMPDT"), String) = String.Empty) OrElse (CType(m_tblRaw.Rows(j)("ZZTMPDT"), String) = "00000000") Then
                    m_tblRaw.Rows.RemoveAt(j)
                End If
            Next

            Dim rowTemp2 As DataRow
            For Each rowTemp2 In m_tblRaw.Rows
                If rowTemp2("AUGRU") Is DBNull.Value Then
                    rowTemp2("AUGRU") = String.Empty
                End If
                rowTemp2("AbrufGrundX") = CStr(getAbrufgrund(CStr(rowTemp2("AUGRU"))))
            Next
            m_tblRaw.AcceptChanges()

            m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)

            If m_strHaendler.Length > 0 Then
                Haendler = m_strHaendler
            End If


        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_WEB"
                    m_intStatus = -1401
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case "NO_DATA"
                    m_intStatus = 0
                    m_strMessage = "Keine Daten gefunden."
                Case "NO_HAENDLER"
                    m_intStatus = -1402
                    m_strMessage = "Händler nicht vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try
    End Sub

    Public Overrides Sub Show()
    End Sub

    'Public Overrides Sub Show()
    '    m_strClassAndMethod = "fin_05.Show"
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_ComCommon_Finance.SAPProxy_ComCommon_Finance()

    '        Dim tblAuftragsUebersichtSAP As New SAPProxy_ComCommon_Finance.ZDAD_FAELLIGE_FAHRZDOK_ANZTable()
    '        Dim rowAuftragsUebersichtSAP As New SAPProxy_ComCommon_Finance.ZDAD_FAELLIGE_FAHRZDOK_ANZ()
    '        Dim tblAuftraegeSAP As New SAPProxy_ComCommon_Finance.ZDAD_FAELLIGE_FAHRZDOKTable()

    '        MakeDestination()
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        If m_objLogApp Is Nothing Then
    '            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
    '        End If
    '        m_intIDSAP = -1
    '        Dim i As Int32
    '        Dim rowTemp As DataRow

    '        Try
    '            m_intStatus = 0
    '            m_strMessage = ""


    '            m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Faellige_Fahrzdok", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
    '            objSAP.Z_M_Faellige_Fahrzdok(Right("0000000000" & m_objUser.KUNNR, 10), personenennummer, "", "1510", tblAuftragsUebersichtSAP, tblAuftraegeSAP)           'TODOHEZ
    '            objSAP.CommitWork()
    '            If m_intIDSAP > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
    '            End If

    '            'keine Spaltenübersetzung, da Zahlenwerte die dann auch sortiert werden müssten
    '            'm_tblAuftragsUebersicht = CreateOutPut(m_tblAuftragsUebersicht, Me.AppID)

    '            m_tblAuftragsUebersicht = New DataTable()

    '            m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
    '            m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
    '            m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard endgültig", System.Type.GetType("System.Int32"))
    '            m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard temporär", System.Type.GetType("System.Int32"))
    '            m_tblAuftragsUebersicht.Columns.Add("Anzahl Flottengeschäft", System.Type.GetType("System.Int32"))
    '            m_tblAuftragsUebersicht.Columns.Add("Ort", System.Type.GetType("System.String"))
    '            m_tblAuftragsUebersicht.Columns.Add("ZZREFERENZ1", System.Type.GetType("System.String"))

    '            For i = 0 To tblAuftragsUebersichtSAP.Count - 1
    '                rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Item(i)
    '                If IsNumeric(rowAuftragsUebersichtSAP.Zaehler_01) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_02) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_04) Then
    '                    If CInt(rowAuftragsUebersichtSAP.Zaehler_01) + CInt(rowAuftragsUebersichtSAP.Zaehler_02) + CInt(rowAuftragsUebersichtSAP.Zaehler_04) > 0 Then
    '                        rowTemp = m_tblAuftragsUebersicht.NewRow
    '                        rowTemp("Händlernummer") = CStr(rowAuftragsUebersichtSAP.Kunnr)
    '                        rowTemp("Händlername") = rowAuftragsUebersichtSAP.Name1_Zf
    '                        rowTemp("Anzahl Standard endgültig") = CInt(rowAuftragsUebersichtSAP.Zaehler_02)
    '                        rowTemp("Anzahl Standard temporär") = CInt(rowAuftragsUebersichtSAP.Zaehler_01)
    '                        rowTemp("Anzahl Flottengeschäft") = CInt(rowAuftragsUebersichtSAP.Zaehler_04)
    '                        rowTemp("Ort") = rowAuftragsUebersichtSAP.Ort01_Zf
    '                        rowTemp("ZZREFERENZ1") = rowAuftragsUebersichtSAP.Zzreferenz1
    '                        m_tblAuftragsUebersicht.Rows.Add(rowTemp)
    '                    End If
    '                End If
    '            Next





    '            m_tblRaw = tblAuftraegeSAP.ToADODataTable


    '            m_tblRaw.Columns.Add("AbrufGrundX", GetType(System.String), "")


    '            Dim j As Integer

    '            For j = m_tblRaw.Rows.Count - 1 To 0 Step -1

    '                If (TypeOf (m_tblRaw.Rows(j)("ZZTMPDT")) Is System.DBNull) OrElse (CType(m_tblRaw.Rows(j)("ZZTMPDT"), String) = String.Empty) OrElse (CType(m_tblRaw.Rows(j)("ZZTMPDT"), String) = "00000000") Then
    '                    m_tblRaw.Rows.RemoveAt(j)
    '                End If
    '            Next

    '            Dim rowTemp2 As DataRow
    '            For Each rowTemp2 In m_tblRaw.Rows
    '                If rowTemp2("AUGRU") Is DBNull.Value Then
    '                    rowTemp2("AUGRU") = String.Empty
    '                End If
    '                rowTemp2("AbrufGrundX") = CStr(getAbrufgrund(CStr(rowTemp2("AUGRU"))))
    '            Next
    '            m_tblRaw.AcceptChanges()

    '            m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)

    '            If m_strHaendler.Length > 0 Then
    '                Haendler = m_strHaendler
    '            End If


    '        Catch ex As Exception
    '            Select Case ex.Message
    '                Case "NO_WEB"
    '                    m_intStatus = -1401
    '                    m_strMessage = "Keine Web-Tabelle erstellt."
    '                Case "NO_DATA"
    '                    m_intStatus = 0
    '                    m_strMessage = "Keine Daten gefunden."
    '                Case "NO_HAENDLER"
    '                    m_intStatus = -1402
    '                    m_strMessage = "Händler nicht vorhanden."
    '                Case Else
    '                    m_intStatus = -9999
    '                    m_strMessage = ex.Message
    '            End Select
    '            If m_intIDSAP > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
    '            End If

    '        Finally
    '            If m_intIDsap > -1 Then
    '                m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
    '            End If

    '            objSAP.Connection.Close()
    '            objSAP.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub

    Private Function getAbrufgrund(ByVal kuerzel As String) As String
        Dim cn As New SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim dsAg As DataSet
        Dim adAg As SqlClient.SqlDataAdapter
        Dim dr As SqlClient.SqlDataReader
        Dim sAbrufgrund As String = ""
        Try

            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()
            dsAg = New DataSet()
            adAg = New SqlClient.SqlDataAdapter()
            cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                            "[WebBezeichnung]" & _
                                            "FROM CustomerAbrufgruende " & _
                                            "WHERE " & _
                                            "CustomerID =' " & m_objUser.Customer.CustomerId.ToString & "' AND " & _
                                            "SapWert='" & kuerzel & "'" & _
                                            " AND GroupID = " & m_objUser.GroupID.ToString _
                                               , cn)
            cmdAg.CommandType = CommandType.Text
            dr = cmdAg.ExecuteReader()

            If dr.Read() = True Then
                If dr.IsDBNull(0) Then
                    sAbrufgrund = String.Empty
                Else
                    sAbrufgrund = CStr(dr.Item("WebBezeichnung"))
                End If
            End If
        Catch ex As Exception

            Throw ex

        Finally
            cn.Close()
        End Try
        Return sAbrufgrund
    End Function

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        Try
            Dim strfaellig_am_alt As String
            Dim strfaellig_am As String
            Dim strMemo_alt As String
            Dim strMemo As String
            Dim strEQUNR As String

            Dim tmpRow As DataRow
            For Each tmpRow In m_tblAuftraege.Rows
                tmpRow.AcceptChanges()
                strEQUNR = tmpRow("EQUNR").ToString

                If tmpRow("faellig am").ToString = tmpRow("faellig am alt").ToString Then
                    strfaellig_am_alt = tmpRow("faellig am alt").ToString
                    strfaellig_am = strfaellig_am_alt
                    m_blnChangeFaelligkeit = False
                Else
                    strfaellig_am_alt = tmpRow("faellig am alt").ToString
                    strfaellig_am = tmpRow("faellig am").ToString
                    m_blnChangeFaelligkeit = True
                End If

                If tmpRow("Memo alt").ToString = tmpRow("Memo").ToString Then
                    strMemo_alt = tmpRow("Memo alt").ToString
                    strMemo = strMemo_alt
                    m_blnChangeMemo = False
                Else
                    strMemo_alt = tmpRow("Memo alt").ToString
                    strMemo = tmpRow("Memo").ToString
                    m_blnChangeMemo = True
                End If

                If m_blnChangeFaelligkeit Or m_blnChangeMemo Then
                    Dim strTempDate As String = ""
                    If IsDate(strfaellig_am) Then
                        strTempDate = Format(CDate(strfaellig_am), "yyyyMMdd")
                    End If

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Faellige_Equipments_Update", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_EQUNR", strEQUNR)
                    myProxy.setImportParameter("I_ZZFAEDT", strTempDate)
                    myProxy.setImportParameter("I_TEXT200", Left(strMemo, 200))

                    myProxy.callBapi()

                End If

                If m_blnChangeMemo Then
                    tmpRow("Memo alt") = tmpRow("Memo")
                    If tmpRow("Memo alt").ToString.Length = 0 Then
                        tmpRow("MemoString") = "Erfassen"
                    Else
                        tmpRow("MemoString") = "Ändern"
                    End If
                End If

                If m_blnChangeFaelligkeit Then
                    tmpRow("faellig am alt") = tmpRow("faellig am")
                    If tmpRow("faellig am alt").ToString.Length = 0 Then
                        tmpRow("FaelligkeitString") = "Erfassen"
                    Else
                        tmpRow("FaelligkeitString") = "Ändern"
                    End If
                End If
                tmpRow.AcceptChanges()
            Next
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_UPDATE"
                    m_intStatus = -1401
                    m_strMessage = "Kein EQUI-UPDATE."
                Case "NO_TEXTAENDERUNG"
                    m_intStatus = -1402
                    m_strMessage = "Fehler bei der Textänderung."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try
    End Sub
    Public Overrides Sub Change()

    End Sub

#End Region
End Class
