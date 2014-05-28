Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class fin_05
    REM § Lese-/Schreibfunktion, 
    REM § Show - BAPI: Z_M_Faellige_Fahrzdok
    REM § Change - BAPI: Z_M_Faellige_Equipments_Update

    Inherits Base.Business.BankBase

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
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String,
                    ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        m_strHaendler = ""
        m_blnChangeFaelligkeit = False
        m_blnChangeMemo = False
    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String)
        Dim tblAuftragsUebersichtSAP As DataTable
        Dim rowAuftragsUebersichtSAP As DataRow
        Dim tblAuftraegeSAP As DataTable

        Dim i As Int32
        Dim rowTemp As DataRow

        Try
            ClearError()

            S.AP.InitExecute("Z_M_Faellige_Fahrzdok", "I_AG, I_HAENDLER, I_VKORG",
                             Right("0000000000" & m_objUser.KUNNR, 10), personenennummer, "1510")

            tblAuftragsUebersichtSAP = S.AP.GetExportTable("GT_ANZ")
            tblAuftraegeSAP = S.AP.GetExportTable("GT_WEB")

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

                If (TypeOf (m_tblRaw.Rows(j)("ZZTMPDT")) Is System.DBNull) OrElse (m_tblRaw.Rows(j)("ZZTMPDT") Is Nothing) Then
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
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NO_WEB") Then
                RaiseError("-1401", "Keine Web-Tabelle erstellt.")
            ElseIf errormessage.Contains("NO_DATA") Then
                RaiseError("0", "Keine Daten gefunden.")
            ElseIf errormessage.Contains("NO_HAENDLER") Then
                RaiseError("-1402", "Händler nicht vorhanden.")
            Else
                RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End If
        End Try
    End Sub

    Public Overrides Sub Show()
    End Sub

    Public Function getAbrufgrund(ByVal kuerzel As String) As String
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

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String)

        Try
            ClearError()

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

                    S.AP.InitExecute("Z_M_Faellige_Equipments_Update", "I_EQUNR, I_ZZFAEDT, I_TEXT200",
                                     strEQUNR, strTempDate, Left(strMemo, 200))
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
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NO_UPDATE") Then
                RaiseError("-1401", "Kein EQUI-UPDATE.")
            ElseIf errormessage.Contains("NO_TEXTAENDERUNG") Then
                RaiseError("-1402", "Fehler bei der Textänderung.")
            Else
                RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End If
        End Try
    End Sub

    Public Overrides Sub Change()

    End Sub

#End Region

End Class

' ************************************************
' $History: fin_05.vb $
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 4.09.09    Time: 9:55
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA: 3050
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 24.06.09   Time: 16:00
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 finalisierung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 22.06.09   Time: 16:02
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_M_Faellige_Fahrzdok, Z_M_Faellige_Equipments_Update
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.04.08   Time: 14:45
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 9.04.08    Time: 13:29
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' bugfix fällige vorgänge
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.03.08    Time: 18:08
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' AKF Änderungen 1733
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 2.02.08    Time: 15:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' akf änderungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 7.01.08    Time: 14:32
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1499 Verbesserungen Change41_X
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 13.12.07   Time: 12:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.12.07   Time: 11:17
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.12.07   Time: 10:32
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.12.07   Time: 14:10
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1466/1499 (Fällige Vorgänge) Kompilierfähig = unfertig eingefügt
' 
' ************************************************
