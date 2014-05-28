Imports System.Configuration
Imports System.IO
Imports System.Diagnostics
'Imports Microsoft.Data.SAPClient

Public Class Sixt1

    Private m_strClassAndMethod As String
    Private m_strSAPAppServerHost As String
    Private m_shortSAPSystemNumber As Short
    Private m_shortSAPClient As Short
    Private m_strSAPUsername As String
    Private m_strSAPPassword As String
    Private m_BizTalkSapConnectionString As String

    'Protected WithEvents m_objSAPDestination As SAP.Connector.Destination

    Public Function GetBriefdaten(ByVal VIN As String, ByVal Date_From As String, ByVal date_to As String) As String

        m_strClassAndMethod = "Sixt1.GetBriefdaten"
        'Dim objSAP As New SAPProxy_SixtService.SAPProxy_SixtService()

        'MakeDestination()

        'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        'objSAP.Connection.Open()
        Dim strXml As String

        Try

            'objSAP.Z_M_Exp_Briefdaten(date_to, Date_From, VIN, "0000312680", strXml)
            'objSAP.CommitWork()

            S.AP.InitExecute("Z_M_Exp_Briefdaten", "IMP_KUNNR,IMP_FAHRG,IMP_ERDATV,IMP_ERDATB", "0000312680", VIN, Date_From, date_to)

            strXml = S.AP.GetExportParameter("EXP_XML_STRING")

        Catch ex As Exception
            If ex.Message = "NO_DATA" Then
                Throw ex
            Else
                Throw New Exception("WMGetFahrzeug, Fehler beim Ermitteln der Daten.")
            End If

            'Finally
            'If IsNothing(objSAP.Connection) = False Then
            '    objSAP.Connection.Close()
            '    objSAP.Dispose()
            'End If
        End Try

        Return strXml


    End Function

    'Public Sub MakeDestination()
    '    m_objSAPDestination = New SAP.Connector.Destination()
    '    m_objSAPDestination.AppServerHost = ConfigurationManager.AppSettings("SAPAppServerHost")
    '    m_objSAPDestination.SystemNumber = CShort(ConfigurationManager.AppSettings("SAPSystemNumber"))
    '    m_objSAPDestination.Client = CShort(ConfigurationManager.AppSettings("SAPClient"))
    '    m_objSAPDestination.Username = ConfigurationManager.AppSettings("SAPUsername")
    '    m_objSAPDestination.Password = ConfigurationManager.AppSettings("SAPPassword")
    'End Sub

    'Private Sub MakeBizTalkDestination()
    '    m_BizTalkSapConnectionString = "ASHOST=" & ConfigurationManager.AppSettings("SAPAppServerHost") & _
    '                        ";CLIENT=" & CShort(ConfigurationManager.AppSettings("SAPClient")) & _
    '                        ";SYSNR=" & CShort(ConfigurationManager.AppSettings("SAPSystemNumber")) & _
    '                        ";USER=" & ConfigurationManager.AppSettings("SAPUsername") & _
    '                        ";PASSWD=" & ConfigurationManager.AppSettings("SAPPassword") & _
    '                        ";LANG=DE"
    'End Sub

    Public Function GetZulAuftrTable() As DataTable

        Dim SapTable As DataTable

        Try
            S.AP.Init("Z_M_Imp_Zul_Auftr_1", "I_KUNNR", "0000312680")
            SapTable = S.AP.GetImportTable("GT_WEB")
        Catch
            EventLog.WriteEntry("SixtService", "WMInsertFreisetzung, Fehler in GetZulAuftrTable(): " & Err.Description, EventLogEntryType.Warning)
            Throw New Exception("WMInsertFreisetzung, Fehler in GetZulAuftrTable():  " & Err.Number & ", " & Err.Description)
        End Try

        Return SapTable
    End Function

    Public Sub SetZulAuftr(ByRef SapTable As DataTable) 'SAPProxy_SixtService.ZDAD_ZUL_AUFTR_1Table)
        'Zulassungsauftrag
        m_strClassAndMethod = "Sixt1.SetZulAuftr"

        'Dim objSAP As New SAPProxy_SixtService.SAPProxy_SixtService()

        Try

            'MakeDestination()

            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()



            'objSAP.Z_M_Imp_Zul_Auftr_1("0000312680", SapTable)

            'objSAP.CommitWork()

            S.AP.Init("Z_M_Imp_Zul_Auftr_1", "I_KUNNR", "0000312680")
            Dim tbl As DataTable = S.AP.GetImportTable("GT_WEB")
            tbl.Merge(SapTable)
            tbl.AcceptChanges()

            S.AP.Execute()
        Catch
            EventLog.WriteEntry("SixtService", "WMInsertFreisetzung, Fehler beim Import(Modul - SetZulAuftr): " & Err.Description, EventLogEntryType.Warning)
            Throw New Exception("WMInsertFreisetzung, Fehler beim Import:  " & Err.Number & ", " & Err.Description)
            'Finally
            '    If IsNothing(objSAP.Connection) = False Then
            '        objSAP.Connection.Close()
            '        objSAP.Dispose()
            '    End If
        End Try

    End Sub

    Public Function GetVehicleChanges(ByVal strXML As String) As String
        'Änderungsauftrag
        m_strClassAndMethod = "Sixt1.GetVehicleChanges"

        'Dim objSAP As New SAPProxy_SixtService.SAPProxy_SixtService()

        Try


            'XML-Datei erstellen
            Dim strFile As String = ConfigurationManager.AppSettings("TempPathXML") & "XML" & Format(DateTime.Now, "ddMMyyyyHHmmss") & ".xml"

            Dim SWriter As StreamWriter

            Dim filExport As FileInfo

            filExport = New FileInfo(strFile)

            'Das korrekte Encoding auswählen!
            SWriter = New StreamWriter(filExport.CreateText.BaseStream, System.Text.Encoding.Default)

            'Übergabestring in die XML-Datei schreiben
            SWriter.Write(strXML)

            SWriter.Close()

            'Dataset
            Dim dsVIN As New DataSet("VIN")

            'XML-Datei in das Dataset einlesen
            dsVIN.ReadXml(strFile)

            'Die XML-Datei kann gelöscht werden
            System.IO.File.Delete(strFile)

            S.AP.Init("Z_M_Read_Tab_Equi_And_001", "I_KUNNR", "0000312680")

            Dim SapTable As DataTable = S.AP.GetImportTable("TAB_IN") 'New SAPProxy_SixtService.ZDAD_TAB_EQUI_AND_001Table()
            Dim dr As DataRow 'New SAPProxy_SixtService.ZDAD_TAB_EQUI_AND_001()
            Dim tr As DataRow


            'Dataset-Tabelle durchlaufen und in die Sap-Tabelle schreiben
            For Each tr In dsVIN.Tables("VehicleID").Rows

                dr = SapTable.NewRow 'New SAPProxy_SixtService.ZDAD_TAB_EQUI_AND_001()
                With dr

                    dr("Chassis_Num") = tr("VEHICLEID_Text")

                End With

                SapTable.Rows.Add(dr)
            Next

            SapTable.AcceptChanges()

            'MakeDestination()

            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            strXML = String.Empty

            'objSAP.Z_M_Read_Tab_Equi_And_001("0000312680", strXML, SapTable)

            'objSAP.CommitWork()

            S.AP.Execute()

            strXML = S.AP.GetExportParameter("E_XML")

        Catch ex As Exception
            If ex.Message = "NO_DATA" Then
                Throw ex
            Else
                Throw New Exception("WMUpdateFahrzeug, Fehler beim Ermitteln der Daten.")
            End If

            'Finally
            '    If IsNothing(objSAP.Connection) = False Then
            '        objSAP.Connection.Close()
            '        objSAP.Dispose()
            '    End If
        End Try

        Return strXML

    End Function

    Public Function GetZulFahrzeuge(ByVal Date_From As String) As String

        m_strClassAndMethod = "Sixt1.GetZulFahrzeuge"
        'Dim objSAP As New SAPProxy_SixtService.SAPProxy_SixtService()

        'MakeDestination()

        'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        'objSAP.Connection.Open()
        Dim strXml As String

        Try
            'objSAP.Z_M_Export_Zul_001(Date_From, "0000312680", strXml)
            'objSAP.CommitWork()

            S.AP.InitExecute("Z_M_Export_Zul_001", "I_KUNNR,I_ERDAT", "0000312680", Date_From)

            strXml = S.AP.GetExportParameter("E_XML")

        Catch ex As Exception
            If ex.Message = "NO_DATA" Then
                Throw ex
            Else
                Throw New Exception("GetZulFahrzeuge, Fehler beim Ermitteln der Daten.")
            End If

            'Finally
            '    If IsNothing(objSAP.Connection) = False Then
            '        objSAP.Connection.Close()
            '        objSAP.Dispose()
            '    End If
        End Try

        Return strXml

    End Function

    Public Function GetAbmeldungen(ByVal DatVon As String, ByVal DatBis As String) As VehicleDocuments

        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        Dim intID As Int32 = -1

        Dim KUNNR As String = "0000312680"
        'MakeBizTalkDestination()
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        Try

            'con.Open()
            'strCom = "EXEC Z_DPM_EXPORT_ABM_001 @I_AG='" & KUNNR & "', " _
            '                                           & "@I_ERDAT_VON='" & DatVon & "', " _
            '                                           & "@I_ERDAT_BIS='" & DatBis & "', " _
            '                                           & "@E_XML=@pSAPXML OUTPUT, @E_SUBRC=@pSUBRC OUTPUT, @E_MESSAGE=@pMessage OUTPUT OPTION 'disabledatavalidation'"



            'Dim pSAPXML As New SAPParameter("@pSAPXML", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPXML)



            'Dim pMessage As New SAPParameter("@pMessage", ParameterDirection.Output)
            'cmd.Parameters.Add(pMessage)

            'Dim pSubrc As New SAPParameter("@pSUBRC", ParameterDirection.Output)
            'cmd.Parameters.Add(pSubrc)


            'cmd.Connection = con
            'cmd.CommandText = strCom

            'cmd.ExecuteNonQuery()

            S.AP.InitExecute("Z_DPM_EXPORT_ABM_001", "I_AG,I_ERDAT_VON,I_ERDAT_BIS", KUNNR, DatVon, DatBis)

            Dim E_MESSAGE As String = ""
            Dim E_SUBRC As String = ""

            'If Not IsDBNull(pSubrc.Value) Then
            If Not IsDBNull(S.AP.GetExportParameter("E_SUBRC")) Then
                E_SUBRC = DirectCast(S.AP.GetExportParameter("E_SUBRC"), String)
            End If
            'If Not IsDBNull(pMessage.Value) Then
            If Not IsDBNull(S.AP.GetExportParameter("E_MESSAGE")) Then
                E_MESSAGE = DirectCast(S.AP.GetExportParameter("E_MESSAGE"), String)
            End If

            If E_MESSAGE.Length > 0 Then
                Throw New Exception("WMGetAbmeldung_Status, Fehler beim Export:  " & E_SUBRC & ", " & E_MESSAGE)
            End If

            'XML-Datei erstellen
            Dim strFile As String = ConfigurationManager.AppSettings("TempPathXML") & "XML" & Format(DateTime.Now, "ddMMyyyyHHmmss") & ".xml"

            Dim SWriter As StreamWriter

            Dim filExport As FileInfo

            filExport = New FileInfo(strFile)

            'Das korrekte Encoding auswählen!
            SWriter = New StreamWriter(filExport.CreateText.BaseStream, System.Text.Encoding.UTF8)

            'Übergabestring in die XML-Datei schreiben
            SWriter.Write(S.AP.GetExportParameter("E_XML")) 'pSAPXML

            SWriter.Close()

            'Dataset
            Dim dsVIN As New DataSet("VIN")

            'XML-Datei in das Dataset einlesen
            dsVIN.ReadXml(strFile)

            'Die XML-Datei kann gelöscht werden
            System.IO.File.Delete(strFile)


            Dim TempTable As New DataTable

            Dim vd As New VehicleDocuments

            If dsVIN.Tables.Count > 0 Then

                TempTable = dsVIN.Tables(0)

                For Each dr As DataRow In TempTable.Rows
                    Dim VDOC As New VehicleDocument
                    VDOC.fahrgestellNr = dr("Fahrgestellnummer")
                    VDOC.amtKennz = dr("KENNZEICHEN")
                    VDOC.abmeldungDatum = dr("ABMELDEDATUM")
                    vd.Add(VDOC)
                Next

            End If

            Return vd

        Catch
            Select Case CastSapBizTalkErrorMessage(Err.Description)

                Case "NO_DATA"
                    Throw New Exception("WMGetAbmeldung_Status, Fehler beim Export: Keine Daten gefunden!")

                Case Else
                    Throw New Exception("WMGetAbmeldung_Status, Fehler beim Export:  " & Err.Number & ", " & CastSapBizTalkErrorMessage(Err.Description))

            End Select

            'Finally
            '    con.Close()
            '    con.Dispose()
            '    cmd.Dispose()
        End Try

    End Function

    ''' <summary>
    ''' Holt die Importtabelle für SaveBrieffreigabe aus SAP
    ''' </summary>
    ''' <returns>GT_LIST</returns>
    ''' <remarks></remarks>
    Public Function GetSaveBrieffreigabeTable() As DataTable
        Dim SAPTable As DataTable

        Try
            S.AP.Init("Z_DPM_IMP_VERS_ABM_001")

            SAPTable = S.AP.GetImportTable("GT_LIST")

        Catch ex As Exception
            Throw New Exception("WMInsertBrieffreigabe, Fehler beim Abrufen der Import-Tabelle: " & CastSapBizTalkErrorMessage(ex.Message))
        End Try

        Return SAPTable
    End Function

    Public Sub SaveBrieffreigabe(ByRef SapTable As DataTable)

        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        Dim intID As Int32 = -1

        Dim strKUNNR As String = "0000312680"
        'MakeBizTalkDestination()
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        Try
            'con.Open()
            'strCom = "EXEC Z_DPM_IMP_VERS_ABM_001 @I_KUNNR_AG ='" & strKUNNR & "', " _
            '                                           & "@GT_LIST=@pSAPTable, " _
            '                                           & "@E_SUBRC=@pSUBRC OUTPUT, @E_MESSAGE=@pMessage OUTPUT OPTION 'disabledatavalidation'"


            'cmd.Connection = con
            'cmd.CommandText = strCom

            'Dim pSAPTable As New SAPParameter("@pSAPTable")
            'pSAPTable.Value = SapTable

            'cmd.Parameters.Add(pSAPTable)

            'Dim pMessage As New SAPParameter("@pMessage", ParameterDirection.Output)
            'cmd.Parameters.Add(pMessage)

            'Dim pSubrc As New SAPParameter("@pSUBRC", ParameterDirection.Output)
            'cmd.Parameters.Add(pSubrc)


            'cmd.ExecuteNonQuery()

            S.AP.Init("Z_DPM_IMP_VERS_ABM_001", "I_KUNNR_AG", strKUNNR)

            Dim tbl As DataTable = S.AP.GetImportTable("GT_LIST")
            tbl.Merge(SapTable)
            tbl.AcceptChanges()

            S.AP.Execute()

            Dim E_MESSAGE As String = ""
            Dim E_SUBRC As String = ""

            'If Not IsDBNull(pSubrc.Value) Then
            If Not IsDBNull(S.AP.GetExportParameter("E_SUBRC")) Then
                E_SUBRC = DirectCast(S.AP.GetExportParameter("E_SUBRC"), String)
            End If
            'If Not IsDBNull(pMessage.Value) Then
            If Not IsDBNull(S.AP.GetExportParameter("E_MESSAGE")) Then
                E_MESSAGE = DirectCast(S.AP.GetExportParameter("E_MESSAGE"), String)
            End If
            If E_MESSAGE.Length > 0 Then
                Throw New Exception("WMInsertBrieffreigabe, Fehler beim Import:  " & E_SUBRC & ", " & E_MESSAGE)
            End If

        Catch ex As Exception
            Throw New Exception("WMInsertBrieffreigabe, Fehler beim Import: " & CastSapBizTalkErrorMessage(ex.Message))
            'Finally
            '    con.Close()
            '    con.Dispose()
            '    cmd.Dispose()
        End Try
    End Sub


    Public Shared Function CastSapBizTalkErrorMessage(ByVal errorMessage As String) As String
        If errorMessage.Contains("SapErrorMessage") = True Then

            Return Mid(errorMessage, errorMessage.IndexOf("SapErrorMessage") + 17, _
                        errorMessage.Substring((errorMessage.IndexOf("SapErrorMessage") + 16)).IndexOf("."))

        Else
            Return errorMessage

        End If
    End Function

    Public Function MakeDateSAP(ByVal datInput As Date) As String
        REM $ Formt Date-Input in String YYYYMMDD um
        Return Year(datInput) & Right("0" & Month(datInput), 2) & Right("0" & Day(datInput), 2)
    End Function

End Class
