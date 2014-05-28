Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts


'#################################################################
' Klasse für das Lesen der Avis-Grundaten#
' Reports : Eingang Fahrzeugbriefe (Report01)
'           Eingang ZBII ohne Vorlage von Grunddaten (Report02)
'           Eingang ZBII ohne Eingang Carport (Report02?Appl=Carport)
'#################################################################
Public Class Avis01
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strFlag_Zulge As String
    Private m_tblExcel As DataTable
    Private m_strEingZBIIbis As String
#End Region

#Region " Properties"
    Public Property Flag_Zulge() As String
        Get
            Return m_strFlag_Zulge
        End Get
        Set(ByVal value As String)
            m_strFlag_Zulge = value
        End Set
    End Property
    Public Property ExcelResult() As DataTable
        Get
            Return m_tblExcel
        End Get
        Set(ByVal value As DataTable)
            m_tblExcel = value
        End Set
    End Property
    Public Property EingZBIIbis() As String
        Get
            Return m_strEingZBIIbis
        End Get
        Set(ByVal value As String)
            m_strEingZBIIbis = value
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
    ' Beschreibung: füllen der Ausgabetabelle für Eingang Fahrzeugbriefe (Report01)
    ' Erstellt am: 24.10.2008
    ' ITA: 2310
    '----------------------------------------------------------------------

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, _
                              ByVal datAbmeldedatumVon As DateTime, ByVal datAbmeldedatumBis As DateTime)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        Dim strKunnr As String = ""
        m_strClassAndMethod = "Avis01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1

        Try
            Dim strDatTempVon As String
            If IsDate(datAbmeldedatumVon) Then
                'strDatTempVon = MakeDateSAP(datAbmeldedatumVon)
                'If strDatTempVon = "10101" Then
                '    strDatTempVon = "|"
                'End If
            Else
                strDatTempVon = "|"
            End If
            Dim strDatTempBis As String
            If IsDate(datAbmeldedatumBis) Then
                'strDatTempBis = MakeDateSAP(datAbmeldedatumBis)
                'If strDatTempBis = "10101" Then
                '    strDatTempBis = "|"
                'End If
            Else
                strDatTempBis = "|"
            End If

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_READ_GRUNDDAT_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            S.AP.InitExecute("Z_M_READ_GRUNDDAT_001",
                        "I_KUNNR_AG, I_EING_DAT_VON, I_EING_DAT_BIS, I_ZUGELASSEN",
                        strKunnr, datAbmeldedatumVon, datAbmeldedatumBis, m_strFlag_Zulge)

            'strCom = "EXEC Z_M_READ_GRUNDDAT_001 @I_KUNNR_AG='" & strKunnr & "', " _
            '                                       & "@I_EING_DAT_VON='" & strDatTempVon & "', " _
            '                                       & "@I_EING_DAT_VON='" & strDatTempBis & "', " _
            '                                       & "@I_EING_DAT_VON='" & m_strFlag_Zulge & "', " _
            '                                       & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)

            'cmd.ExecuteNonQuery()

            Dim tblTemp2 As DataTable

            'tblTemp2 = DirectCast(pSAPTableAUFTRAG.Value, DataTable)
            tblTemp2 = S.AP.GetExportTable("GT_WEB")

            Dim row As DataRow
            For Each row In tblTemp2.Rows
                row("Name1") = row("Name1").ToString & " " & row("ORT01").ToString
            Next
            tblTemp2.AcceptChanges()

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            CreateOutPut(tblTemp2, strAppID)
            m_tblExcel = m_tblResult.Copy
            With m_tblResult
                .Columns.Remove("Lieferant")
                .Columns.Remove("Händlername")
            End With

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_EING_DAT_VON=" & datAbmeldedatumVon.ToShortDateString & ", I_EING_DAT_BIS=" & datAbmeldedatumBis.ToShortDateString, m_tblResult, False)
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
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_EING_DAT_VON=" & datAbmeldedatumVon.ToShortDateString & ", I_EING_DAT_BIS=" & datAbmeldedatumBis.ToShortDateString & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
    'Fill_ZBIIVorlage
    Public Overloads Sub Fill_ZBIIVorlage(ByVal strAppID As String, ByVal strSessionID As String)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        Dim strKunnr As String = ""
        m_strClassAndMethod = "Avis01.Fill_ZBIIVorlage"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1

        Try

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_READ_GRUNDDAT_002", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            S.AP.InitExecute("Z_M_READ_GRUNDDAT_002", "I_KUNNR_AG", strKunnr)

            'strCom = "EXEC Z_M_READ_GRUNDDAT_002 @I_KUNNR_AG='" & strKunnr & "', " _
            '                                       & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)

            'cmd.ExecuteNonQuery()

            Dim tblTemp2 As DataTable

            tblTemp2 = S.AP.GetExportTable("GT_WEB")
            'tblTemp2 = DirectCast(pSAPTableAUFTRAG.Value, DataTable)

            Dim row As DataRow
            For Each row In tblTemp2.Rows
                row("Name1") = row("Name1").ToString & " " & row("ORT01").ToString
            Next
            tblTemp2.AcceptChanges()

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            CreateOutPut(tblTemp2, strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            'Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
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

    Public Overloads Sub Fill_ZBIICarport(ByVal strAppID As String, ByVal strSessionID As String)

        Dim strKunnr As String = ""
        m_strClassAndMethod = "Avis01.Fill_ZBIICarport"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String

        'If m_strEingZBIIbis = "" Then
        '    m_strEingZBIIbis = "00000000"
        'Else
        '    m_strEingZBIIbis = MakeDateSAP(m_strEingZBIIbis)
        'End If

        Dim intID As Int32 = -1

        Try

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_READ_GRUNDDAT_002", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            S.AP.InitExecute("Z_M_READ_GRUNDDAT_003",
                        "I_KUNNR_AG, I_DAT_EING_ZBII_BIS",
                        strKunnr, m_strEingZBIIbis)

            'strCom = "EXEC Z_M_READ_GRUNDDAT_003 @I_KUNNR_AG='" & strKunnr & "', " _
            '                                       & "@I_DAT_EING_ZBII_BIS='" & m_strEingZBIIbis & "', " _
            '                                       & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)

            'cmd.ExecuteNonQuery()

            Dim tblTemp2 As DataTable
            'tblTemp2 = DirectCast(pSAPTableAUFTRAG.Value, DataTable)
            tblTemp2 = S.AP.GetExportTable("GT_WEB")
            Dim row As DataRow
            For Each row In tblTemp2.Rows
                row("Name1") = row("Name1").ToString & " " & row("ORT01").ToString
            Next
            tblTemp2.AcceptChanges()

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            CreateOutPut(tblTemp2, strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            'Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
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
#End Region
End Class
' ************************************************
' $History: Avis01.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 1.12.08    Time: 16:13
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA: 2359 & kleinere Anpassungen
' 
' ************************************************
