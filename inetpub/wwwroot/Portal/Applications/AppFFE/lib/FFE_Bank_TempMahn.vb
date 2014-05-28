Public Class FFE_Bank_TempMahn

    REM § Mahn-Report, Kunde: FFE, BAPI: Z_M_Zu_Mahnende_Fahrzeuge_Fce,
    REM § Ausgabetabelle per Zuordnung in Web-DB.
    Inherits FFE_BankBase

#Region " Declarations"
    Private m_Result As DataTable
    Private m_iStatus As Int32
    Private m_sMessage As Int32
#End Region

#Region " Properties"
    Public Property NewResultTable() As DataTable
        Get
            Return m_Result
        End Get
        Set(ByVal value As DataTable)
            m_Result = value
        End Set
    End Property

    Public ReadOnly Property ReportStatus() As Int32
        Get
            Return m_iStatus
        End Get
    End Property
    Public ReadOnly Property ReportMessage() As Int32
        Get
            Return m_sMessage
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "FFE_Bank_TempMahn.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            MakeDestination()

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()

            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1
            Dim strKUNNR As String = m_objUser.Reference ' Right("0000000000" & m_objUser.KUNNR, 10)
            Dim strKNRZE As String = m_strFiliale
            Dim strKONZS As String = Right("0000000000" & m_objUser.KUNNR, 10)
            Dim strHaendlernr As String = ""
            Try
                Dim SAPTable As New SAPProxy_FFE.ZDAD_ZU_MAHNENDE_FAHRZEUGE_FCETable()
                Dim strVKORG As String = "1510"
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Temporaer_Zu_Mahnen", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Zu_Mahnende_Fahrzeuge_Fce(strKONZS, strKUNNR, strVKORG, strKNRZE, SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                m_Result = New DataTable()
                m_Result.Columns.Add("HAENDLER", System.Type.GetType("System.String"))
                m_Result.Columns.Add("ZZKKBER", System.Type.GetType("System.String"))
                m_Result.Columns.Add("ZZTMPDT", System.Type.GetType("System.String"))
                m_Result.Columns.Add("TIDNR", System.Type.GetType("System.String"))
                m_Result.Columns.Add("LICENSE_NUM", System.Type.GetType("System.String"))
                m_Result.Columns.Add("LIZNR", System.Type.GetType("System.String"))
                m_Result.Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
                m_Result.Columns.Add("ZMELDBANK", System.Type.GetType("System.String"))
                m_Result.Columns.Add("ZZFRIST", System.Type.GetType("System.String"))

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                Dim rowTemp As DataRow

                For Each rowTemp In tblTemp2.Rows
                    Dim rowNew As DataRow = m_Result.NewRow
                    Select Case CStr(rowTemp("ZZKKBER"))
                        Case "0001"
                            rowTemp("ZZKKBER") = "Standard temporär"
                        Case "0002"
                            rowTemp("ZZKKBER") = "Standard endgültig"
                    End Select
                    rowTemp("ZZTMPDT") = FormatDateTime(MakeDateStandard(rowTemp("ZZTMPDT").ToString), DateFormat.ShortDate)
                    If rowTemp("ZMELDBANK").ToString = "00000000" Then
                        rowTemp("ZMELDBANK") = ""
                    Else
                        rowTemp("ZMELDBANK") = FormatDateTime(MakeDateStandard(rowTemp("ZMELDBANK").ToString), DateFormat.ShortDate)
                    End If

                    strHaendlernr = Right(rowTemp("HAENDLER").ToString.TrimStart("0"c), 5)
                    rowTemp("HAENDLER") = strHaendlernr


                    rowNew("HAENDLER") = rowTemp("HAENDLER")
                    rowNew("ZZKKBER") = rowTemp("ZZKKBER")
                    rowNew("ZZTMPDT") = rowTemp("ZZTMPDT")
                    rowNew("TIDNR") = rowTemp("TIDNR")
                    rowNew("LICENSE_NUM") = rowTemp("LICENSE_NUM")
                    rowNew("LIZNR") = rowTemp("LIZNR")
                    rowNew("CHASSIS_NUM") = rowTemp("CHASSIS_NUM")
                    rowNew("ZMELDBANK") = rowTemp("ZMELDBANK")
                    rowNew("ZZFRIST") = rowTemp("ZZFRIST")

                    m_Result.Rows.Add(rowNew)
                Next


                WriteLogEntry(True, "KNRZE=" & strKNRZE & ", KONZS=" & strKONZS & ", KUNNR=" & strKUNNR, m_tblResult)
            Catch ex As Exception
                m_iStatus = -9999
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                        m_iStatus = -1
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_HAENDLER"
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KNRZE=" & strKNRZE & ", KONZS=" & strKONZS & ", KUNNR=" & strKUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class
' ************************************************
' $History: FFE_Bank_TempMahn.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.07.08   Time: 10:52
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 21.05.08   Time: 16:34
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 15.05.08   Time: 16:59
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA:1865
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Applications/AppFFE/lib
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.04.08    Time: 13:32
' Created in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 3.04.08    Time: 11:19
' Created in $/CKG/Applications/AppFFE/AppFFEWeb/lib
' ITA 1790
' 
' ************************************************