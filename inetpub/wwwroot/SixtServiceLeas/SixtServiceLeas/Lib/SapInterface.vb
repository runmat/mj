'Imports Microsoft.Data.SAPClient
Imports System.Configuration
Imports System.IO
Imports System.Diagnostics


Public Class SapInterface
    'Private m_strClassAndMethod As String
    'Private m_strSAPAppServerHost As String
    'Private m_shortSAPSystemNumber As Short
    'Private m_shortSAPClient As Short
    'Private m_strSAPUsername As String
    'Private m_strSAPPassword As String
    'Private m_BizTalkSapConnectionString As String


    'Private Sub MakeDestination()
    '    m_BizTalkSapConnectionString = "ASHOST=" & ConfigurationManager.AppSettings("SAPAppServerHost") & _
    '                                ";CLIENT=" & CShort(ConfigurationManager.AppSettings("SAPClient")) & _
    '                                ";SYSNR=" & CShort(ConfigurationManager.AppSettings("SAPSystemNumber")) & _
    '                                ";USER=" & ConfigurationManager.AppSettings("SAPUsername") & _
    '                                ";PASSWD=" & ConfigurationManager.AppSettings("SAPPassword") & _
    '                                ";LANG=DE"
    'End Sub

    Public Function InsertFreisetzungZul(ByVal SapTable As DataTable) As DataTable

        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        Dim intID As Int32 = -1
        Dim strKUNNR As String = "0000300997"
        'MakeDestination()
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        Try
            'strCom = "EXEC Z_M_IMP_SERVICE_AUFTR_001 @I_KUNNR='" & strKUNNR & "', " _
            '                                           & "@GT_Web=@pSAPTable, " _
            '                                           & "@GT_Web=@pExpSAPTable OUTPUT OPTION 'disabledatavalidation'"


            'cmd.Connection = con
            'cmd.CommandText = strCom

            'Dim pSAPTable As New SAPParameter("@pSAPTable")
            'Dim pExpSAPTable As New SAPParameter("@pExpSAPTable", ParameterDirection.Output)
            'pSAPTable.Value = SapTable

            'cmd.Parameters.Add(pSAPTable)
            'cmd.Parameters.Add(pExpSAPTable)

            'cmd.ExecuteNonQuery()

            S.AP.Init("Z_M_IMP_SERVICE_AUFTR_001", "I_KUNNR", strKUNNR)

            Dim tbl As DataTable = S.AP.GetImportTable("GT_WEB")
            tbl.Merge(SapTable)
            tbl.AcceptChanges()

            S.AP.Execute()

            Dim expTable As DataTable = S.AP.GetExportTable("GT_WEB") 'DirectCast(pExpSAPTable.Value, DataTable)
            Dim datRows() As DataRow
            Dim datrow As DataRow
            datRows = expTable.Select("ZFCODE<>''")
            Dim ErrTable As DataTable = Nothing
            If datRows.Length > 0 Then

                Dim Newrow As DataRow
                ErrTable = New DataTable
                ErrTable.Columns.Add("id", System.Type.GetType("System.String"))
                ErrTable.Columns.Add("Message", System.Type.GetType("System.String"))

                For Each datrow In datRows
                    Newrow = ErrTable.NewRow
                    Select Case datrow("ZFCODE").ToString
                        Case "001"
                            Newrow("id") = datrow("REFERENZ1").ToString
                            Newrow("Message") = "Es existiert kein Datensatz zu diesem Vertrag - keine Datenänderung /-löschung"
                            ErrTable.Rows.Add(Newrow)
                        Case "002"
                            Newrow("id") = datrow("REFERENZ1").ToString
                            Newrow("Message") = "Zum Vertrag existiert noch ein unerledigter Datensatz mit gleichem Auftragsgrund - keine Datenübernahme"
                            ErrTable.Rows.Add(Newrow)
                        Case "003"
                            Newrow("id") = datrow("REFERENZ1").ToString
                            Newrow("Message") = "Änderungskennzeichen unbekannt - keine Datenübernahme"
                            ErrTable.Rows.Add(Newrow)
                        Case "008"
                            Newrow("id") = datrow("REFERENZ1").ToString
                            Newrow("Message") = "Fehler bei Insert  - keine Datenübernahme"
                            ErrTable.Rows.Add(Newrow)
                    End Select
                Next
            End If
            Return ErrTable
        Catch
            Throw New Exception("WMInsertFreisetzung_Zul, Fehler beim Import:  " & Err.Number & ", " & Err.Description)
            'Finally
            '    con.Close()
            '    con.Dispose()
            '    cmd.Dispose()
        End Try
    End Function

    Public Function TabellenSpaltenZul() As DataTable
        Dim dt As DataTable = Nothing

        S.AP.Init("Z_M_IMP_SERVICE_AUFTR_001")
        dt = S.AP.GetImportTable("GT_WEB")

        Return dt
    End Function

    Public Function WMGetFreisetzungStatus() As String

        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        Dim intID As Int32 = -1
        'DirectCast(DirectCast(DirectCast(ex, Microsoft.Data.SAPClient.SAPException).InnerException, System.Exception), Microsoft.Adapters.SAP.RFCException).SapErrorMessage()
        Dim strKUNNR As String = "0000300997"
        Dim strTest As String = ConfigurationManager.AppSettings("ISTEST")

        'MakeDestination()
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        Try
            'strCom = "EXEC Z_M_STATUS_SIXT_LS_001 @I_KUNNR='" & strKUNNR & "', " _
            '                                           & "@I_TEST='" & strTest & "', " _
            '                                           & "@E_XML=@pSAPXML OUTPUT OPTION 'disabledatavalidation'"



            'Dim pSAPXML As New SAPParameter("@pSAPXML", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPXML)

            'cmd.Connection = con
            'cmd.CommandText = strCom

            'cmd.ExecuteNonQuery()

            S.AP.InitExecute("Z_M_STATUS_SIXT_LS_001", "I_KUNNR,I_TEST", strKUNNR, strTest)

            Return S.AP.GetExportParameter("E_XML") 'pSAPXML.Value

        Catch
            Select Case CastSapBizTalkErrorMessage(Err.Description)
                Case "NO_DATA"
                    Throw New Exception("WMInsertFreisetzung_Status, Fehler beim Export: Keine Daten gefunden!")
                    Return String.Empty
                Case Else
                    Throw New Exception("WMInsertFreisetzung_Status, Fehler beim Export:  " & Err.Number & ", " & Err.Description)
                    Return String.Empty
            End Select

            'Finally
            '    con.Close()
            '    con.Dispose()
            '    cmd.Dispose()
        End Try
    End Function

    Public Shared Function CastSapBizTalkErrorMessage(ByVal errorMessage As String) As String
        If errorMessage.Contains("SapErrorMessage") = True Then

            Return Mid(errorMessage, errorMessage.IndexOf("SapErrorMessage") + 17, _
                        errorMessage.Substring((errorMessage.IndexOf("SapErrorMessage") + 16)).IndexOf("."))

        Else
            Return errorMessage

        End If
    End Function
End Class