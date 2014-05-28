Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class AbweichAbrufgrund
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
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Show(ByVal page As Page)
        m_strClassAndMethod = "AbweichAbrufgrund.Show"

        'alte Resulttabelle Clearen, da bei einer exception noch daten darin enthalten bleiben JJ2008.1.14
        If Not Result Is Nothing Then
            Result.Clear()
        End If

        If m_objLogApp Is Nothing Then
            m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        m_intIDSAP = -1

        Try
            m_intStatus = 0
            m_strMessage = ""

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_ABWEICH_ABRUFGRUND_02", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
            myProxy.setImportParameter("I_ABWEICHUNG", strSuchKennzeichen)

            myProxy.callBapi()

            Dim sapTable As DataTable = myProxy.getExportTable("GT_OUT")

            Dim tmpTable As New DataTable()
            tmpTable = sapTable
            tmpTable.Columns.Add(New DataColumn("Halter Anschrift", System.Type.GetType("System.String"), ""))
            tmpTable.Columns.Add(New DataColumn("Halter Anschrift Abweichung", System.Type.GetType("System.String"), ""))


            Dim tmprow As DataRow
            For Each tmprow In tmpTable.Rows
                tmprow("Halter Anschrift Abweichung") = CStr(tmprow("ZH_NEU_NAME1")) & " " & CStr(tmprow("ZH_NEU_NAME2")) & " " & CStr(tmprow("ZH_NEU_NAME3")) & ", " & CStr(tmprow("ZH_NEU_STREET")) & " " & CStr(tmprow("ZH_NEU_HOUSE_NUM1")) & ", " & CStr(tmprow("ZH_NEU_POST_CODE1")) & " " & CStr(tmprow("ZH_NEU_CITY1"))
                tmprow("Halter Anschrift") = CStr(tmprow("ZH_ALT_NAME1")) & " " & CStr(tmprow("ZH_ALT_NAME2")) & " " & CStr(tmprow("ZH_ALT_NAME3")) & ", " & CStr(tmprow("ZH_ALT_STREET")) & " " & CStr(tmprow("ZH_ALT_HOUSE_NUM1")) & ", " & CStr(tmprow("ZH_ALT_POST_CODE1")) & " " & CStr(tmprow("ZH_ALT_CITY1"))
                tmprow("KNRZE") = Right(tmprow("KNRZE").ToString, 2)
            Next
            tmpTable.AcceptChanges()


            Result = CreateOutPut(tmpTable, Me.AppID)


            'WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", LICENSE_NUM=" & m_strINKennzeichen.ToUpper & ", LIZNR=" & m_strINVertragsnummer.ToUpper & ", CHASSIS_NUM=" & m_strINFahrgestellnummer.ToUpper, m_tblResult)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -8711
                    m_strMessage = "Keine Daten vorhanden"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If
        Finally
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
            End If
        End Try
    End Sub
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

    Public Sub fillDetailsTable(ByVal EQUINR As String)
        Try
            Dim tmpRows As DataRow()

            tmpRows = Result.Select("EQUNR='" & EQUINR & "'")



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

            If tmpRow.Table.Columns.Contains("Nummer ZBII") Then
                If Not CStr(tmpRow.Item("Nummer ZBII")) = CStr(tmpRow.Item("Nummer ZBII Abweichung")) Then
                    DetailsTable.Columns.Add("Nummer ZBII Abweichung", System.Type.GetType("System.String"), "")
                    DetailsTable.Columns.Add("Nummer ZBII", System.Type.GetType("System.String"), "")
                    tmpObj(i) = tmpRow.Item("Nummer ZBII Abweichung")
                    i = i + 1
                    tmpObj(i) = tmpRow.Item("Nummer ZBII")
                    i = i + 1
                End If
            End If

            If tmpRow.Table.Columns.Contains("Nummer ZBI") Then
                If Not CStr(tmpRow.Item("Nummer ZBI")) = CStr(tmpRow.Item("Nummer ZBI Abweichung")) Then
                    DetailsTable.Columns.Add("Nummer ZBI Abweichung", System.Type.GetType("System.String"), "")
                    DetailsTable.Columns.Add("Nummer ZBI", System.Type.GetType("System.String"), "")
                    tmpObj(i) = tmpRow.Item("Nummer ZBI Abweichung")
                    i = i + 1
                    tmpObj(i) = tmpRow.Item("Nummer ZBI")
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
    End Sub
    Public Overloads Sub Change(ByVal Page As Page)

        If m_objLogApp Is Nothing Then
            m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        m_intIDSAP = -1

        Try
            m_intStatus = 0
            m_strMessage = ""

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Save_ZABWVERSGRUND", m_objApp, m_objUser, Page)

            myProxy.setImportParameter("IMP_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("IMP_EQUNR", Equipment)
            myProxy.setImportParameter("IMP_DATAUS", Dataus)
            myProxy.setImportParameter("IMP_MEMO", Memo)
            myProxy.setImportParameter("IMP_ERLEDIGT", Erledigt)

            myProxy.callBapi()

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_UPDATE_EQUI"
                    m_intStatus = -8703
                    m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                Case "NO_DATA"
                    m_intStatus = -8704
                    m_strMessage = "Keine Equipmentdaten gefunden"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
        End Try
    End Sub
#End Region
End Class
