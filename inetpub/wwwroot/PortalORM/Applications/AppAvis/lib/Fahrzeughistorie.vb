Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts


Public Class Fahrzeughistorie
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private tblHistorie As DataTable
    Private tblGrunddaten As DataTable
    Private tblAdressen As DataTable
    Private tblLastchange As DataTable
    Private tblEquidaten As DataTable
#End Region

#Region " Properties"

    Public SapSourceORM As Boolean

    Property Historie() As DataTable
        Get
            Return tblHistorie
        End Get
        Set(ByVal value As DataTable)
            tblHistorie = value
        End Set
    End Property
    Property Grunddaten() As DataTable
        Get
            Return tblGrunddaten
        End Get
        Set(ByVal value As DataTable)
            tblGrunddaten = value
        End Set
    End Property
    Property Adressen() As DataTable
        Get
            Return tblAdressen
        End Get
        Set(ByVal value As DataTable)
            tblAdressen = value
        End Set
    End Property
    Property LastChange() As DataTable
        Get
            Return tblAdressen
        End Get
        Set(ByVal value As DataTable)
            tblAdressen = value
        End Set
    End Property
    Property Equidaten() As DataTable
        Get
            Return tblEquidaten
        End Get
        Set(ByVal value As DataTable)
            tblEquidaten = value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    '----------------------------------------------------------------------
    ' Methode: Fill
    ' Autor: O.Rudolph
    ' Beschreibung: füllen der Ausgabetabelle für Fahrzeughistorie (Report07)
    ' Erstellt am: 30.12.2008
    ' ITA: 2389
    '----------------------------------------------------------------------

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, _
                              ByVal strAmtlKennzeichen As String, _
                              ByVal strFahrgestellnummer As String, _
                              ByVal strBriefnummer As String, ByVal strMVAnummer As String, _
                              ByVal mProduktionskennziffer As String)

        'If (SapSourceORM) Then
        Fill_ORM(strAppID, strSessionID, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strMVAnummer, mProduktionskennziffer)
        'Else
        'Fill_BizTalk(strAppID, strSessionID, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strMVAnummer, mProduktionskennziffer)
        'End If

    End Sub

    Public Sub Fill_ORM(ByVal strAppID As String, ByVal strSessionID As String, _
                              ByVal strAmtlKennzeichen As String, _
                              ByVal strFahrgestellnummer As String, _
                              ByVal strBriefnummer As String, ByVal strMVAnummer As String, _
                              ByVal mProduktionskennziffer As String)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        Dim strKunnr As String = ""
        m_strClassAndMethod = "Fahrzeughistorie.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1
        'con.Open()
        Try


            S.AP.InitExecute("Z_M_BRIEFLEBENSLAUF_002",
                        "I_KUNNR, I_ZZKENN, I_ZZFAHRG, I_ZZBRIEF, I_PROD_KENNZIFFER, I_ZZREF1, I_EQUNR, I_MVA_NUMMER",
                        strKunnr, strAmtlKennzeichen.ToUpper, strFahrgestellnummer.ToUpper, strBriefnummer.ToUpper, mProduktionskennziffer.ToUpper, "", "", strMVAnummer)


            'strCom = "EXEC Z_M_BRIEFLEBENSLAUF_002 @I_KUNNR='" & strKunnr & "', " _
            '                                       & "@I_ZZKENN='" & strAmtlKennzeichen.ToUpper & "', " _
            '                                       & "@I_ZZFAHRG='" & strFahrgestellnummer.ToUpper & "', " _
            '                                       & "@I_ZZBRIEF='" & strBriefnummer.ToUpper & "', " _
            '                                       & "@I_PROD_KENNZIFFER='" & mProduktionskennziffer.ToUpper & "', " _
            '                                       & "@I_ZZREF1='', " _
            '                                       & "@I_EQUNR='', " _
            '                                       & "@I_MVA_NUMMER='" & strMVAnummer & "', " _
            '                                       & "@GT_QMMA=@pSAPTableLastChange OUTPUT, " _
            '                                       & "@GT_QMEL=@pSAPTableMel OUTPUT, " _
            '                                       & "@GT_ADDR=@pSAPTableAdr OUTPUT, " _
            '                                       & "@GT_EQUI=@pSAPTableEqui OUTPUT, " _
            '                                       & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableGrunddaten As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'Dim pSAPTableAdressdaten As New SAPParameter("@pSAPTableAdr", ParameterDirection.Output)
            'Dim pSAPTableLebenslauf As New SAPParameter("@pSAPTableMel", ParameterDirection.Output)
            'Dim pSAPTableEquidaten As New SAPParameter("@pSAPTableEqui", ParameterDirection.Output)
            'Dim pSAPTableLastChange As New SAPParameter("@pSAPTableLastChange", ParameterDirection.Output)

            'cmd.Parameters.Add(pSAPTableGrunddaten)
            'cmd.Parameters.Add(pSAPTableAdressdaten)
            'cmd.Parameters.Add(pSAPTableLebenslauf)
            'cmd.Parameters.Add(pSAPTableEquidaten)
            'cmd.Parameters.Add(pSAPTableLastChange)

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_BRIEFLEBENSLAUF_002", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            'cmd.ExecuteNonQuery()
            Dim tblTemp2 As DataTable

            'tblTemp2 = DirectCast(pSAPTableGrunddaten.Value, DataTable)
            tblTemp2 = S.AP.GetExportTable("GT_WEB")
            If Not tblTemp2 Is Nothing Then
                'HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
                tblGrunddaten = tblTemp2
            End If


            'tblTemp2 = DirectCast(pSAPTableAdressdaten.Value, DataTable)
            tblTemp2 = S.AP.GetExportTable("GT_ADDR")
            If Not tblTemp2 Is Nothing Then
                'HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
                tblAdressen = tblTemp2
            End If

            'tblTemp2 = DirectCast(pSAPTableLebenslauf.Value, DataTable)
            tblTemp2 = S.AP.GetExportTable("GT_QMEL")
            If Not tblTemp2 Is Nothing Then
                'HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
                tblHistorie = tblTemp2

                'Dim r2 As DataRow = tblTemp2.Rows(0)
                'Dim dt2 As DateTime = CDate(r2("STRMN"))

            End If

            'tblTemp2 = DirectCast(pSAPTableEquidaten.Value, DataTable)
            tblTemp2 = S.AP.GetExportTable("GT_EQUI")
            If Not tblTemp2 Is Nothing Then
                'HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
                tblEquidaten = tblTemp2
            End If


            'tblTemp2 = DirectCast(pSAPTableLastChange.Value, DataTable)
            tblTemp2 = S.AP.GetExportTable("GT_QMMA")
            If Not tblTemp2 Is Nothing Then
                'HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
                tblLastchange = tblTemp2
            End If


            Dim row As DataRow

            For Each row In tblHistorie.Rows
                'If Not row("STRMN") Is Nothing Then
                '    row("STRMN") = row("STRMN").ToString.TrimStart("0"c)
                '    If row("STRMN").ToString <> String.Empty Then
                '        row("STRMN") = Left(MakeDateStandard(row("STRMN").ToString).ToString, 10)
                '    End If
                'End If
                'If Not TypeOf row("ERDAT") Is System.DBNull Then
                '    row("ERDAT") = row("ERDAT").ToString.TrimStart("0"c)
                '    If row("ERDAT").ToString <> String.Empty Then
                '        row("ERDAT") = Left(MakeDateStandard(row("ERDAT").ToString).ToString, 10)
                '    End If
                'End If
            Next

            For Each row In tblLastchange.Rows
                'If Not row("ZZUEBER") Is Nothing Then
                '    row("ZZUEBER") = row("ZZUEBER").ToString.TrimStart("0"c)
                '    If row("ZZUEBER").ToString <> String.Empty Then
                '        row("ZZUEBER") = Left(MakeDateStandard(row("ZZUEBER").ToString).ToString, 10)
                '    End If
                'End If
                'If Not TypeOf row("PSTER") Is System.DBNull Then
                '    row("PSTER") = row("PSTER").ToString.TrimStart("0"c)
                '    If row("PSTER").ToString <> String.Empty Then
                '        row("PSTER") = Left(MakeDateStandard(row("PSTER").ToString).ToString, 10)
                '    End If
                'End If
                'If Not TypeOf row("AEDAT") Is System.DBNull Then
                '    row("AEDAT") = row("AEDAT").ToString.TrimStart("0"c)
                '    If row("AEDAT").ToString <> String.Empty Then
                '        row("AEDAT") = Left(MakeDateStandard(row("AEDAT").ToString).ToString, 10)
                '    End If
                'End If
                'If Not TypeOf row("AEZEIT") Is System.DBNull Then
                '    row("AEZEIT") = row("AEZEIT").ToString.TrimStart("0"c)
                '    If row("AEZEIT").ToString <> String.Empty Then
                '        row("AEZEIT") = Left(row("AEZEIT").ToString, 2) & ":" & row("AEZEIT").ToString.Substring(2, 2)
                '    End If
                'End If

                row("AEZEIT") = row("AEZEIT").ToString.ToTimeString()
            Next


            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            'CreateOutPut(DirectCast(pSAPTableAUFTRAG.Value, DataTable), strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_ZZKENN=" & strAmtlKennzeichen & ", I_ZZFAHRG=" & strFahrgestellnummer, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            'Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_ZZKENN=" & strAmtlKennzeichen & ", I_ZZFAHRG=" & strFahrgestellnummer & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'cmd.Dispose()
            'con.Close()
            'con.Dispose()
            m_blnGestartet = False
        End Try
    End Sub

    'Public Sub Fill_BizTalk(ByVal strAppID As String, ByVal strSessionID As String, _
    '                          ByVal strAmtlKennzeichen As String, _
    '                          ByVal strFahrgestellnummer As String, _
    '                          ByVal strBriefnummer As String, ByVal strMVAnummer As String, _
    '                          ByVal mProduktionskennziffer As String)
    '    Dim con As New SAPConnection(m_BizTalkSapConnectionString)

    '    Dim cmd As New SAPCommand()
    '    Dim strCom As String
    '    Dim strKunnr As String = ""
    '    m_strClassAndMethod = "Fahrzeughistorie.FILL"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID
    '    strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

    '    Dim intID As Int32 = -1
    '    con.Open()
    '    Try

    '        strCom = "EXEC Z_M_BRIEFLEBENSLAUF_002 @I_KUNNR='" & strKunnr & "', " _
    '                                               & "@I_ZZKENN='" & strAmtlKennzeichen.ToUpper & "', " _
    '                                               & "@I_ZZFAHRG='" & strFahrgestellnummer.ToUpper & "', " _
    '                                               & "@I_ZZBRIEF='" & strBriefnummer.ToUpper & "', " _
    '                                               & "@I_PROD_KENNZIFFER='" & mProduktionskennziffer.ToUpper & "', " _
    '                                               & "@I_ZZREF1='', " _
    '                                               & "@I_EQUNR='', " _
    '                                               & "@I_MVA_NUMMER='" & strMVAnummer & "', " _
    '                                               & "@GT_QMMA=@pSAPTableLastChange OUTPUT, " _
    '                                               & "@GT_QMEL=@pSAPTableMel OUTPUT, " _
    '                                               & "@GT_ADDR=@pSAPTableAdr OUTPUT, " _
    '                                               & "@GT_EQUI=@pSAPTableEqui OUTPUT, " _
    '                                               & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

    '        cmd.Connection = con
    '        cmd.CommandText = strCom


    '        Dim pSAPTableGrunddaten As New SAPParameter("@pSAPTable", ParameterDirection.Output)
    '        Dim pSAPTableAdressdaten As New SAPParameter("@pSAPTableAdr", ParameterDirection.Output)
    '        Dim pSAPTableLebenslauf As New SAPParameter("@pSAPTableMel", ParameterDirection.Output)
    '        Dim pSAPTableEquidaten As New SAPParameter("@pSAPTableEqui", ParameterDirection.Output)
    '        Dim pSAPTableLastChange As New SAPParameter("@pSAPTableLastChange", ParameterDirection.Output)

    '        cmd.Parameters.Add(pSAPTableGrunddaten)
    '        cmd.Parameters.Add(pSAPTableAdressdaten)
    '        cmd.Parameters.Add(pSAPTableLebenslauf)
    '        cmd.Parameters.Add(pSAPTableEquidaten)
    '        cmd.Parameters.Add(pSAPTableLastChange)

    '        intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_BRIEFLEBENSLAUF_002", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

    '        cmd.ExecuteNonQuery()
    '        Dim tblTemp2 As DataTable

    '        tblTemp2 = DirectCast(pSAPTableGrunddaten.Value, DataTable)

    '        If Not tblTemp2 Is Nothing Then
    '            HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
    '            tblGrunddaten = tblTemp2
    '        End If


    '        tblTemp2 = DirectCast(pSAPTableAdressdaten.Value, DataTable)
    '        If Not tblTemp2 Is Nothing Then
    '            HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
    '            tblAdressen = tblTemp2
    '        End If

    '        tblTemp2 = DirectCast(pSAPTableLebenslauf.Value, DataTable)
    '        If Not tblTemp2 Is Nothing Then
    '            HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
    '            tblHistorie = tblTemp2

    '        End If

    '        tblTemp2 = DirectCast(pSAPTableEquidaten.Value, DataTable)
    '        If Not tblTemp2 Is Nothing Then
    '            HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
    '            tblEquidaten = tblTemp2
    '        End If


    '        tblTemp2 = DirectCast(pSAPTableLastChange.Value, DataTable)
    '        If Not tblTemp2 Is Nothing Then
    '            HelpProcedures.killAllDBNullValuesInDataTable(tblTemp2)
    '            tblLastchange = tblTemp2
    '        End If


    '        Dim row As DataRow

    '        For Each row In tblHistorie.Rows
    '            If Not row("STRMN") Is Nothing Then
    '                row("STRMN") = row("STRMN").ToString.TrimStart("0"c)
    '                If row("STRMN").ToString <> String.Empty Then
    '                    row("STRMN") = Left(MakeDateStandard(row("STRMN").ToString).ToString, 10)
    '                End If
    '            End If
    '            If Not TypeOf row("ERDAT") Is System.DBNull Then
    '                row("ERDAT") = row("ERDAT").ToString.TrimStart("0"c)
    '                If row("ERDAT").ToString <> String.Empty Then
    '                    row("ERDAT") = Left(MakeDateStandard(row("ERDAT").ToString).ToString, 10)
    '                End If
    '            End If
    '        Next

    '        For Each row In tblLastchange.Rows
    '            If Not row("ZZUEBER") Is Nothing Then
    '                row("ZZUEBER") = row("ZZUEBER").ToString.TrimStart("0"c)
    '                If row("ZZUEBER").ToString <> String.Empty Then
    '                    row("ZZUEBER") = Left(MakeDateStandard(row("ZZUEBER").ToString).ToString, 10)
    '                End If
    '            End If
    '            If Not TypeOf row("PSTER") Is System.DBNull Then
    '                row("PSTER") = row("PSTER").ToString.TrimStart("0"c)
    '                If row("PSTER").ToString <> String.Empty Then
    '                    row("PSTER") = Left(MakeDateStandard(row("PSTER").ToString).ToString, 10)
    '                End If
    '            End If
    '            If Not TypeOf row("AEDAT") Is System.DBNull Then
    '                row("AEDAT") = row("AEDAT").ToString.TrimStart("0"c)
    '                If row("AEDAT").ToString <> String.Empty Then
    '                    row("AEDAT") = Left(MakeDateStandard(row("AEDAT").ToString).ToString, 10)
    '                End If
    '            End If
    '            If Not TypeOf row("AEZEIT") Is System.DBNull Then
    '                row("AEZEIT") = row("AEZEIT").ToString.TrimStart("0"c)
    '                If row("AEZEIT").ToString <> String.Empty Then
    '                    row("AEZEIT") = Left(row("AEZEIT").ToString, 2) & ":" & row("AEZEIT").ToString.Substring(2, 2)
    '                End If
    '            End If
    '        Next


    '        If intID > -1 Then
    '            m_objLogApp.WriteEndDataAccessSAP(intID, True)
    '        End If

    '        'CreateOutPut(DirectCast(pSAPTableAUFTRAG.Value, DataTable), strAppID)
    '        WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_ZZKENN=" & strAmtlKennzeichen & ", I_ZZFAHRG=" & strFahrgestellnummer, m_tblResult, False)
    '    Catch ex As Exception
    '        m_intStatus = -5555
    '        Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
    '            Case "NO_DATA"
    '                m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
    '            Case Else
    '                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '        End Select
    '        If intID > -1 Then
    '            m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
    '        End If
    '        WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_ZZKENN=" & strAmtlKennzeichen & ", I_ZZFAHRG=" & strFahrgestellnummer & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
    '    Finally
    '        If intID > -1 Then
    '            m_objLogApp.WriteStandardDataAccessSAP(intID)
    '        End If
    '        cmd.Dispose()
    '        con.Close()
    '        con.Dispose()
    '        m_blnGestartet = False
    '    End Try
    'End Sub
#End Region
End Class
' ************************************************
' $History: Fahrzeughistorie.vb $
' 
' *****************  Version 3  *****************
' User: Dittbernerc  Date: 9.06.09    Time: 17:17
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 5.01.09    Time: 17:07
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA: 2389
' 
' ************************************************
