Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class fin_18
    REM § Lese-/Schreibfunktion, Kunde: allg. Banken App, 
    REM § Show - BAPI: Z_M_Abweich_abrufgrund,
    REM § Change - BAPI: Z_M_save_zabwvergrund
    Inherits BankBase

#Region " Declarations"

    Dim strEQUNR As String
    Dim strErledigt As String
    Dim strMemo As String
    Dim strDATAUS As String
    Dim strSuchKennzeichen As String
    Dim dtDetails As New DataTable()

#End Region

#Region " Properties"
    Public Property Equipment() As String
        Get
            Return strEQUNR
        End Get
        Set(ByVal Value As String)
            strEQUNR = Value
        End Set
    End Property

    Public Property SuchKennzeichen() As String
        Get
            Return strSuchKennzeichen
        End Get
        Set(ByVal Value As String)
            strSuchKennzeichen = Value
        End Set
    End Property

    Public Property Erledigt() As String
        Get
            Return strErledigt
        End Get
        Set(ByVal Value As String)
            strErledigt = Value
        End Set
    End Property

    Public Property DetailsTable() As DataTable
        Get
            Return dtDetails
        End Get
        Set(ByVal Value As DataTable)
            dtDetails = Value
        End Set
    End Property

    Public Property Memo() As String
        Get
            Return strMemo
        End Get
        Set(ByVal Value As String)
            strMemo = Value
        End Set
    End Property

    Public Property Dataus() As String
        Get
            Return strDATAUS
        End Get
        Set(ByVal Value As String)
            strDATAUS = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String,
                    ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "fin_18.Show"

        'alte Resulttabelle Clearen, da bei einer exception noch daten darin enthalten bleiben JJ2008.1.14
        If Not Result Is Nothing Then
            Result.Clear()
        End If

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        m_intIDSAP = -1

        Try
            ClearError()

            S.AP.InitExecute("Z_M_Abweich_abrufgrund", "I_AG, I_ABWEICHUNG",
                             Right("0000000000" & m_objUser.Customer.KUNNR, 10), SuchKennzeichen)

            Dim tmpTable As DataTable = S.AP.GetExportTable("GT_OUT")
            tmpTable.Columns.Add(New DataColumn("Halter Anschrift", System.Type.GetType("System.String"), ""))
            tmpTable.Columns.Add(New DataColumn("Halter Anschrift Abweichung", System.Type.GetType("System.String"), ""))

            Dim tmprow As DataRow
            For Each tmprow In tmpTable.Rows
                tmprow("Halter Anschrift Abweichung") = CStr(tmprow("ZH_NEU_NAME1")) & " " & CStr(tmprow("ZH_NEU_NAME2")) & " " & CStr(tmprow("ZH_NEU_NAME3")) & "<br>" & CStr(tmprow("ZH_NEU_STREET")) & " " & CStr(tmprow("ZH_NEU_HOUSE_NUM1")) & "<br>" & CStr(tmprow("ZH_NEU_POST_CODE1")) & " " & CStr(tmprow("ZH_NEU_CITY1"))
                tmprow("Halter Anschrift") = CStr(tmprow("ZH_ALT_NAME1")) & " " & CStr(tmprow("ZH_ALT_NAME2")) & " " & CStr(tmprow("ZH_ALT_NAME3")) & "<br>" & CStr(tmprow("ZH_ALT_STREET")) & " " & CStr(tmprow("ZH_ALT_HOUSE_NUM1")) & "<br>" & CStr(tmprow("ZH_ALT_POST_CODE1")) & " " & CStr(tmprow("ZH_ALT_CITY1"))
            Next
            tmpTable.AcceptChanges()

            Result = CreateOutPut(tmpTable, AppID)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    RaiseError("-8711", "Keine Daten vorhanden")
                Case Else
                    RaiseError("-9999", ex.Message)
            End Select
        End Try
    End Sub

    Private Function getAbrufgrund(ByVal kuerzel As String) As String
        Dim cn As New SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim dr As SqlClient.SqlDataReader
        Dim sAbrufgrund As String = ""

        Try

            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()
            cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                            "[WebBezeichnung]" & _
                                            "FROM CustomerAbrufgruende " & _
                                            "WHERE " & _
                                            "CustomerID =' " & m_objUser.Customer.CustomerId.ToString & "' AND " & _
                                            "SapWert='" & kuerzel & "'", cn)
            cmdAg.CommandType = CommandType.Text
            dr = cmdAg.ExecuteReader()

            If dr.Read() = True Then
                If dr.IsDBNull(0) Then
                    sAbrufgrund = (String.Empty)
                Else
                    sAbrufgrund = (CStr(dr.Item("WebBezeichnung")))
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
        End Try

        Return sAbrufgrund
    End Function

    Public Sub fillDetailsTable(ByVal equinr As String)
        Try
            Dim tmpRows As DataRow()

            tmpRows = Result.Select("EQUNR='" & equinr & "'")

            If Not tmpRows.Length = 1 Then
                Throw New Exception("Fehler, es konnte keine Detailansicht generiert werden, es steht mehr als ein DS in der Selektion")
            End If

            Dim tmpRow As DataRow
            tmpRow = tmpRows(0)

            'vorherige Detailstabelle Freigeben und neu erstellen
            DetailsTable.Dispose()
            DetailsTable = New DataTable()

            'feststellen ob Unterschiede vorhanden

            Dim tmpObj(tmpRow.ItemArray.Length - 1) As Object
            Dim i As Int32 = 0


            If tmpRow.Table.Columns.Contains("Versandgrund Abweichung") Then
                If Not CStr(tmpRow.Item("Versandgrund Abweichung")) = CStr(tmpRow.Item("Versandgrund")) Then
                    DetailsTable.Columns.Add("Versandgrund Abweichung", System.Type.GetType("System.String"), "")
                    DetailsTable.Columns.Add("Versandgrund", System.Type.GetType("System.String"), "")
                    tmpObj(i) = getAbrufgrund(CStr(tmpRow.Item("Versandgrund Abweichung")))
                    i = i + 1
                    tmpObj(i) = getAbrufgrund(CStr(tmpRow.Item("Versandgrund")))
                    i = i + 1
                End If
            End If

            If tmpRow.Table.Columns.Contains("Halter Anschrift Abweichung") Then
                If Not CStr(tmpRow.Item("Halter Anschrift Abweichung")) = CStr(tmpRow.Item("Halter Anschrift")) Then
                    DetailsTable.Columns.Add("Halter Anschrift Abweichung", System.Type.GetType("System.String"), "")
                    DetailsTable.Columns.Add("Halter Anschrift", System.Type.GetType("System.String"), "")
                    tmpObj(i) = tmpRow.Item("Halter Anschrift Abweichung")
                    i = i + 1
                    tmpObj(i) = tmpRow.Item("Halter Anschrift")
                    i = i + 1
                End If
            End If

            If tmpRow.Table.Columns.Contains("Kennzeichen") Then
                If Not CStr(tmpRow.Item("Kennzeichen")) = CStr(tmpRow.Item("Kennzeichen Abweichung")) Then
                    DetailsTable.Columns.Add("Kennzeichen Abweichung", System.Type.GetType("System.String"), "")
                    DetailsTable.Columns.Add("Kennzeichen", System.Type.GetType("System.String"), "")
                    tmpObj(i) = tmpRow.Item("Kennzeichen Abweichung")
                    i = i + 1
                    tmpObj(i) = tmpRow.Item("Kennzeichen")
                    i = i + 1
                End If
            End If

            If tmpRow.Table.Columns.Contains("Nummer ZB2") Then
                If Not CStr(tmpRow.Item("Nummer ZB2")) = CStr(tmpRow.Item("Nummer ZB2 Abweichung")) Then
                    DetailsTable.Columns.Add("Nummer ZB2 Abweichung", System.Type.GetType("System.String"), "")
                    DetailsTable.Columns.Add("Nummer ZB2", System.Type.GetType("System.String"), "")
                    tmpObj(i) = tmpRow.Item("Nummer ZB2 Abweichung")
                    i = i + 1
                    tmpObj(i) = tmpRow.Item("Nummer ZB2")
                    i = i + 1
                End If
            End If


            If tmpRow.Table.Columns.Contains("Halternummer") Then
                If Not CStr(tmpRow.Item("Halternummer")) = CStr(tmpRow.Item("Halternummer Abweichung")) Then
                    DetailsTable.Columns.Add("Halternummer Abweichung", System.Type.GetType("System.String"), "")
                    DetailsTable.Columns.Add("Halternummer", System.Type.GetType("System.String"), "")
                    tmpObj(i) = tmpRow.Item("Halternummer Abweichung")
                    i = i + 1
                    tmpObj(i) = tmpRow.Item("Halternummer")
                    i = i + 1
                End If
            End If

            If tmpRow.Table.Columns.Contains("COC") Then
                If Not CStr(tmpRow.Item("COC")) = CStr(tmpRow.Item("COC Abweichung")) Then
                    DetailsTable.Columns.Add("COC Abweichung", System.Type.GetType("System.String"), "")
                    DetailsTable.Columns.Add("COC", System.Type.GetType("System.String"), "")
                    If CStr(tmpRow.Item("COC Abweichung")) = "X" Then
                        tmpObj(i) = "vorhanden"
                        i = i + 1
                    Else
                        tmpObj(i) = "nicht vorhanden"
                        i = i + 1
                    End If

                    If CStr(tmpRow.Item("COC")) = "X" Then
                        tmpObj(i) = "vorhanden"
                        i = i + 1
                    Else
                        tmpObj(i) = "-"
                        i = i + 1
                    End If
                End If
            End If


            DetailsTable.AcceptChanges()
            ReDim Preserve tmpObj(i - 1)
            DetailsTable.Rows.Add(tmpObj)
            DetailsTable.AcceptChanges()
        Catch ex As Exception
            m_strMessage = ex.Message
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Public Overrides Sub Change()
        If m_objLogApp Is Nothing Then
            m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        m_intIDSAP = -1

        Try
            ClearError()

            S.AP.InitExecute("Z_M_Save_ZABWVERSGRUND", "IMP_KUNNR, IMP_EQUNR, IMP_DATAUS, IMP_MEMO, IMP_ERLEDIGT",
                             Right("0000000000" & m_objUser.KUNNR, 10), Equipment, Dataus, Memo, Erledigt)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_UPDATE_EQUI"
                    RaiseError("-8703", "Fehler bei der Datenspeicherung (EQUI-UPDATE)")
                Case "NO_DATA"
                    RaiseError("-8704", "Keine Equipmentdaten gefunden")
                Case Else
                    RaiseError("-9999", ex.Message)
            End Select
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: fin_18.vb $
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 22.06.09   Time: 11:33
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITa 2918 nachbesserungen Z_M_SAVEABWVERSGRUND, Z_M_ABWEICH_ABRUFGRUND
' 
' *****************  Version 7  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 15:49
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA - 2918 .Net Connector Umstellung
' 
' Bapis:
' Z_M_Brief_Ohne_Daten
' Z_M_Daten_Einz_Report_001
' 
' *****************  Version 6  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 14:29
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 - .Net Connector Umstellung
' 
' Bapis:
' Z_M_Abweich_abrufgrund
' Z_M_Save_ZABWVERGRUND
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.06.08    Time: 10:31
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 1954
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.04.08   Time: 14:45
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 15.04.08   Time: 15:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Bugfix Abweichungen
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 6.02.08    Time: 16:53
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 6.02.08    Time: 11:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' unfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 4.02.08    Time: 17:53
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' AKF Änderungen Unfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 4.02.08    Time: 16:40
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Abweichungen
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 2.02.08    Time: 10:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 14.01.08   Time: 11:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Warte Auf Bapi Änderung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 11.01.08   Time: 15:33
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1477 erstellung Grid
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 11.01.08   Time: 9:25
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1477 kompilierfähig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 10.01.08   Time: 16:37
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1477 Torso hinzugefügt
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 10.01.08   Time: 16:07
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1477 Torso
'' ************************************************
