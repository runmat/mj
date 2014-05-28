Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class FFE_Bank_4
    Inherits FFE_BankBasis

#Region " Declarations"
    Private m_strHaendler As String
    Private m_tblAuftragsUebersicht As DataTable
    Private m_tblAuftraegeAlle As DataTable
    Private m_tblAuftraege As DataTable
    Private m_tblRaw As DataTable
    Private m_blnChangeMemo As Boolean
    Private m_blnChangeFaelligkeit As Boolean
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

    Public Property Haendler() As String
        Get
            Return m_strHaendler
        End Get
        Set(ByVal Value As String)
            m_strHaendler = Right(Value, 6).TrimStart("0"c)
            m_tblAuftraege = m_tblAuftraegeAlle.Copy
            Dim i As Int32
            For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1
                If Not Right(m_tblAuftraege.Rows(i)("Händler").ToString, 5).TrimStart("0"c) = m_strHaendler Then
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
                rowTemp.Item("Händler") = Right(rowTemp.Item("Händler").ToString, 5).TrimStart("0"c)
                rowTemp("Memo alt") = rowTemp("Memo").ToString
                If rowTemp("Kontingentart").ToString = "0004" Or rowTemp("Kontingentart").ToString = "0003" _
                            Or rowTemp("Kontingentart").ToString = "0006" Then
                    If Not rowTemp("fällig am").ToString = String.Empty Then
                        rowTemp("Faelligkeit") = True
                        rowTemp("FaelligkeitString") = "Ändern"
                    Else
                        rowTemp("Faelligkeit") = False
                        rowTemp("FaelligkeitString") = "Erfassen"
                    End If
                Else
                    rowTemp("Faelligkeit") = False
                    rowTemp("FaelligkeitString") = "ZZZZZZZ"
                End If
                Select Case rowTemp("Kontingentart").ToString
                    Case "0001"
                        rowTemp("Kontingentart") = "Standard temporär"
                        rowTemp("DelayedPayment") = False
                    Case "0002"
                        rowTemp("Kontingentart") = "Standard endgültig"
                        rowTemp("DelayedPayment") = False
                    Case "0003"
                        rowTemp("Kontingentart") = "Retail"
                        rowTemp("DelayedPayment") = False
                    Case "0004"
                        rowTemp("Kontingentart") = "Erweitertes Zahlungsziel<br>(Delayed Payment)<br>endgültig"
                        rowTemp("DelayedPayment") = True
                        rowTemp("faellig am alt") = rowTemp("fällig am")
                    Case "0006"
                        rowTemp("Kontingentart") = "KF/KL"
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
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_strHaendler = ""
        m_blnChangeFaelligkeit = False
        m_blnChangeMemo = False
    End Sub

    Public Sub Show_Retail() ' ITA: 1067
        m_strClassAndMethod = "FDD_Bank_4.Show_Retail"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            Dim tblAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VORTable()
            Dim rowAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VOR()
            Dim tblAuftraegeSAP As New SAPProxy_FFE.ZDAD_M_WEB_EQUIDATENTable()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim i As Int32
            Dim rowTemp As DataRow

            Try
                m_intStatus = 0
                m_strMessage = ""

                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Faellige_Equipments", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Faellige_Equis_Fce(" ", "0003", m_strFiliale, m_strCustomer, "", "1510", tblAuftragsUebersichtSAP, tblAuftraegeSAP)           'TODOHEZ
                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_tblAuftragsUebersicht = New DataTable()

                m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Retail", System.Type.GetType("System.Int32"))

                For i = 0 To tblAuftragsUebersichtSAP.Count - 1
                    rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Item(i)
                    If IsNumeric(rowAuftragsUebersichtSAP.Zaehler_03) Then
                        If CInt(rowAuftragsUebersichtSAP.Zaehler_03) > 0 Then
                            rowTemp = m_tblAuftragsUebersicht.NewRow
                            rowTemp("Händlernummer") = Right(rowAuftragsUebersichtSAP.Kunnr, 6).TrimStart("0"c)
                            rowTemp("Händlername") = rowAuftragsUebersichtSAP.Name1_Zf
                            rowTemp("Anzahl Retail") = CInt(rowAuftragsUebersichtSAP.Zaehler_03)
                            m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                        End If
                    End If
                Next

                m_tblRaw = tblAuftraegeSAP.ToADODataTable

                Dim j As Integer

                For j = m_tblRaw.Rows.Count - 1 To 0 Step -1
                    If (TypeOf (m_tblRaw.Rows(j)("ZZTMPDT")) Is System.DBNull) OrElse (CType(m_tblRaw.Rows(j)("ZZTMPDT"), String) = String.Empty) OrElse (CType(m_tblRaw.Rows(j)("ZZTMPDT"), String) = "00000000") Then
                        m_tblRaw.Rows.RemoveAt(j)
                    End If
                Next

                m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)

                If m_strHaendler.Length > 0 Then
                    Haendler = m_strHaendler
                End If

                WriteLogEntry(True, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=", m_tblAuftragsUebersicht)
            Catch ex As Exception
                Select Case ex.Message
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
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftragsUebersicht)
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
    Public Overrides Sub Show()
        m_strClassAndMethod = "FDD_Bank_4.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            Dim tblAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VORTable()
            Dim rowAuftragsUebersichtSAP As New SAPProxy_FFE.ZDAD_M_WEB_AUFTRAEGE_ANZ_VOR()
            Dim tblAuftraegeSAP As New SAPProxy_FFE.ZDAD_M_WEB_EQUIDATENTable()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim i As Int32
            Dim rowTemp As DataRow

            Try
                m_intStatus = 0
                m_strMessage = ""
                Dim strKKBER As String = ""

                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Faellige_Equipments", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Faellige_Equis_Fce(" ", strKKBER, m_strFiliale, m_strCustomer, "", "1510", tblAuftragsUebersichtSAP, tblAuftraegeSAP)    'TODOHEZ
                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_tblAuftragsUebersicht = New DataTable()

                m_tblAuftragsUebersicht.Columns.Add("Händlernummer", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Händlername", System.Type.GetType("System.String"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard endgültig", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Standard temporär", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl Flottengeschäft", System.Type.GetType("System.Int32"))
                m_tblAuftragsUebersicht.Columns.Add("Anzahl KF/KL", System.Type.GetType("System.Int32"))

                For i = 0 To tblAuftragsUebersichtSAP.Count - 1
                    rowAuftragsUebersichtSAP = tblAuftragsUebersichtSAP.Item(i)
                    If IsNumeric(rowAuftragsUebersichtSAP.Zaehler_01) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_02) And IsNumeric(rowAuftragsUebersichtSAP.Zaehler_04) Then
                        If CInt(rowAuftragsUebersichtSAP.Zaehler_01) + CInt(rowAuftragsUebersichtSAP.Zaehler_02) + CInt(rowAuftragsUebersichtSAP.Zaehler_04) > 0 Then
                            rowTemp = m_tblAuftragsUebersicht.NewRow
                            rowTemp("Händlernummer") = Right(rowAuftragsUebersichtSAP.Kunnr, 6).TrimStart("0"c)
                            rowTemp("Händlername") = rowAuftragsUebersichtSAP.Name1_Zf
                            rowTemp("Anzahl Standard endgültig") = CInt(rowAuftragsUebersichtSAP.Zaehler_02)
                            rowTemp("Anzahl Standard temporär") = CInt(rowAuftragsUebersichtSAP.Zaehler_01)
                            rowTemp("Anzahl Flottengeschäft") = CInt(rowAuftragsUebersichtSAP.Zaehler_04)
                            rowTemp("Anzahl KF/KL") = CInt(rowAuftragsUebersichtSAP.Zaehler_06)
                            m_tblAuftragsUebersicht.Rows.Add(rowTemp)
                        End If
                    End If
                Next

                m_tblRaw = tblAuftraegeSAP.ToADODataTable

                Dim j As Integer

                For j = m_tblRaw.Rows.Count - 1 To 0 Step -1
                    If (TypeOf (m_tblRaw.Rows(j)("ZZTMPDT")) Is System.DBNull) OrElse (CType(m_tblRaw.Rows(j)("ZZTMPDT"), String) = String.Empty) OrElse (CType(m_tblRaw.Rows(j)("ZZTMPDT"), String) = "00000000") Then
                        m_tblRaw.Rows.RemoveAt(j)
                    End If
                Next

                m_tblAuftraegeAlle = CreateOutPut(m_tblRaw, m_strAppID)

                If m_strHaendler.Length > 0 Then
                    Haendler = m_strHaendler
                End If

                WriteLogEntry(True, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=", m_tblAuftragsUebersicht)
            Catch ex As Exception
                Select Case ex.Message
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
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), m_tblAuftragsUebersicht)
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
    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                MakeDestination()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

                Dim strfaellig_am_alt As String
                Dim strfaellig_am As String
                Dim strMemo_alt As String
                Dim strMemo As String
                Dim strEQUNR As String

                Dim tmpRow As DataRow
                For Each tmpRow In m_tblAuftraege.Rows
                    tmpRow.AcceptChanges()
                    strEQUNR = tmpRow("EQUNR").ToString

                    If tmpRow("fällig am").ToString = tmpRow("faellig am alt").ToString Then
                        strfaellig_am_alt = tmpRow("faellig am alt").ToString
                        strfaellig_am = strfaellig_am_alt
                        m_blnChangeFaelligkeit = False
                    Else
                        strfaellig_am_alt = tmpRow("faellig am alt").ToString
                        strfaellig_am = tmpRow("fällig am").ToString
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
                        m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Faellige_Equipments_Update", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                        objSAP.Z_M_Faellige_Equis_Update_Fce(strEQUNR, strMemo, strTempDate)
                        objSAP.CommitWork()

                        If m_intIDSAP > -1 Then
                            m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                        End If
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
                        tmpRow("faellig am alt") = tmpRow("fällig am")
                        If tmpRow("faellig am alt").ToString.Length = 0 Then
                            tmpRow("FaelligkeitString") = "Erfassen"
                        Else
                            tmpRow("FaelligkeitString") = "Ändern"
                        End If
                    End If
                    tmpRow.AcceptChanges()
                Next
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_UPDATE"
                        m_intStatus = -1401
                        m_strMessage = "Kein EQUI-UPDATE."
                    Case "NO_TEXTAENDERUNG"
                        m_intStatus = -1402
                        m_strMessage = "Fehler bei der Textänderung."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region
End Class
