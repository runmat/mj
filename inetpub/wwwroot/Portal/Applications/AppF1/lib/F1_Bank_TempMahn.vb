Imports CKG
Imports CKG.Base.Kernel

Public Class F1_Bank_TempMahn

    REM § Mahn-Report, Kunde: FFE, BAPI: Z_M_Zu_Mahnende_Fahrzeuge_Fce,
    REM § Ausgabetabelle per Zuordnung in Web-DB.
    Inherits F1_BankBase

#Region " Declarations"
    Private m_Result As DataTable
    Private m_ResultSumme As DataTable
    Private m_iStatus As Int32
    Private m_sMessage As Int32
    Private m_DatumVon As Date
    Private m_HaendlerEx As String = String.Empty
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

    Public Property MahnSumme() As DataTable
        Get
            Return m_ResultSumme
        End Get
        Set(ByVal value As DataTable)
            m_ResultSumme = value
        End Set
    End Property

    Public Property HaendlerEx() As String
        Get
            Return m_HaendlerEx
        End Get
        Set(ByVal value As String)
            m_HaendlerEx = value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFilename)
    End Sub

    Public Overloads Sub fill(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: fill
        ' Autor: JJU
        ' Beschreibung: gibt alle zu Mahnenden Versendungen für einen Händler zurück
        ' Erstellt am: 25.03.2009
        ' ITA: 2670
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZU_MAHNENDE_FAHRZEUGE_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim Haendler As String

            If m_HaendlerEx = String.Empty Then
                Haendler = m_objUser.Reference
            Else
                Haendler = HaendlerEx
            End If

            'myProxy.setImportParameter("I_HAENDLER_EX", Haendler)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_ZU_MAHNENDE_FAHRZEUGE_STD", "I_AG,I_HAENDLER_EX", Right("0000000000" & m_objUser.KUNNR, 10), Haendler)

            m_Result = S.AP.GetExportTable("GT_MAHN") 'myProxy.getExportTable("GT_MAHN")

            m_Result.Columns.Add("MahnArtText", String.Empty.GetType)
            m_Result.Columns.Add("Abrufgrund", String.Empty.GetType)

            For Each tmpRow As DataRow In m_Result.Rows
                With tmpRow
                    Select Case .Item("MAHNART").ToString
                        Case "WE"
                            .Item("MahnArtText") = "Wiedereingang"
                        Case "ZE"
                            .Item("MahnArtText") = "Zahlungseingang"
                        Case Else
                            .Item("MahnArtText") = "unbekannt"
                    End Select
                End With

                tmpRow("Abrufgrund") = getAbrufgrund(tmpRow("AUGRU").ToString)
                
            Next
            m_Result.AcceptChanges()


        Catch ex As Exception
            m_Result = Nothing
            m_intStatus = -9999
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try


    End Sub

    Public Overloads Sub fillMahnungenSumme(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: fillMahnungenSumme
        ' Autor: SFa
        ' Beschreibung: gibt alle Mahnungen summiert zurück
        ' Erstellt am: 25.09.2009
        ' ITA: 3125
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZU_MAHNENDE_FAHRZEUGE_STD", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_ZU_MAHNENDE_FAHRZEUGE_STD", "I_AG,I_HAENDLER_EX", m_objUser.KUNNR.PadLeft(10, CChar("0")), m_objUser.Reference)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER_EX", m_objUser.Reference)

            If CStr(m_DatumVon) <> "00:00:00" Then
                'myProxy.setImportParameter("I_BANKMELDUNG", CStr(m_DatumVon))
                S.AP.SetImportParameter("I_BANKMELDUNG", m_DatumVon)
            End If

            'myProxy.callBapi()
            S.AP.Execute()

            m_ResultSumme = S.AP.GetExportTable("GT_SUM") 'myProxy.getExportTable("GT_SUM")

            'Mit Nullen auffüllen, um eine nachher korrekte Sortierung zu gewährleisten
            For Each Row As DataRow In m_ResultSumme.Rows

                Row("COUNT_WE") = Right("000" & Row("COUNT_WE").ToString, 3)
                Row("COUNT_ZE") = Right("000" & Row("COUNT_ZE").ToString, 3)

            Next

            m_ResultSumme.AcceptChanges()

        Catch ex As Exception
            m_ResultSumme = Nothing
            m_intStatus = -9999
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try

    End Sub

    Private Function getAbrufgrund(ByVal kuerzel As String) As String
        Dim cn As New SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim dsAg As DataSet
        Dim adAg As SqlClient.SqlDataAdapter
        Dim dr As SqlClient.SqlDataReader
        Dim sReturn As String = ""

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
                    sReturn = String.Empty
                Else
                    sReturn = CStr(dr.Item("WebBezeichnung"))
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
        End Try

        Return sReturn
    End Function

#End Region

End Class
' ************************************************
' $History: F1_Bank_TempMahn.vb $
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 30.09.09   Time: 13:49
' Updated in $/CKAG/Applications/AppF1/lib
' 3125
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 29.09.09   Time: 13:54
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 30.04.09   Time: 11:39
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2837
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.03.09   Time: 16:50
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2670 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.03.09   Time: 14:15
' Updated in $/CKAG/Applications/AppF1/lib
' ITa 2670
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 25.03.09   Time: 10:21
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2741, 2670
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 25.03.09   Time: 8:35
' Created in $/CKAG/Applications/AppF1/lib
' ITA 2670
' 
' ************************************************