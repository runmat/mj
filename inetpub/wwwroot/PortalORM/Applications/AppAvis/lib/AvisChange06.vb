Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class AvisChange06
    Inherits CKG.Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strAvisID As String
    Private m_strName As String
    Private m_strPLZ As String
    Private m_strOrt As String
    Private m_strMail As String
    Private m_strLoesch As String
#End Region

#Region " Properties"
    Public ReadOnly Property Kennung() As String
        Get
            Return "EINBAUFIRMA"
        End Get
    End Property
    Public Property Name() As String
        Get
            Return m_strName
        End Get
        Set(ByVal Value As String)
            m_strName = Value
        End Set
    End Property
    Public Property AvisID() As String
        Get
            Return m_strAvisID
        End Get
        Set(ByVal Value As String)
            m_strAvisID = Value
        End Set
    End Property
    Public Property PLZ() As String
        Get
            Return m_strPLZ
        End Get
        Set(ByVal Value As String)
            m_strPLZ = Value
        End Set
    End Property
    Public Property Ort() As String
        Get
            Return m_strOrt
        End Get
        Set(ByVal Value As String)
            m_strOrt = Value
        End Set
    End Property
    Public Property Mail() As String
        Get
            Return m_strMail
        End Get
        Set(ByVal Value As String)
            m_strMail = Value
        End Set
    End Property
    Public Property Loesch() As String
        Get
            Return m_strLoesch
        End Get
        Set(ByVal Value As String)
            m_strLoesch = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Function SaveFirmenDat() As DataTable

        Dim SAPTable As DataTable
        Dim SAPTableEx As DataTable
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        Dim intID As Int32 = -1

        Try
            S.AP.Init("Z_M_SAVE_AUFTRDAT_006", "I_KUNNR", m_objUser.KUNNR.ToSapKunnr())

            SAPTable = S.AP.GetImportTable("GT_WEB")

            'strCom = "EXEC Z_M_SAVE_AUFTRDAT_006 @I_KUNNR='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@GT_WEB=@pSAPTable,@GT_WEB=@SAPTableExport OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'SAPTable = CreateTableForSap()
            Dim row As DataRow = SAPTable.NewRow

            row("KENNUNG") = Kennung
            row("POS_KURZTEXT") = m_strAvisID
            row("POS_TEXT") = m_strName & ";" & m_strOrt
            row("EMAIL") = m_strMail
            row("LOEVM") = m_strLoesch
            row("AENDT") = Now '.ToString.TruncateToShortDateString
            row("AENUS") = m_objUser.UserName
            SAPTable.Rows.Add(row)
            SAPTable.AcceptChanges()

            ''Importparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", SAPTable)
            'cmd.Parameters.Add(pSAPTable)

            ''Exportparameter
            'Dim SAPTableExport As New SAPParameter("@SAPTableExport", ParameterDirection.Output)
            'cmd.Parameters.Add(SAPTableExport)

            'cmd.ExecuteNonQuery()
            S.AP.Execute()

            'Dim SAPTableEx As DataTable = DirectCast(SAPTableExport.Value, DataTable)
            SAPTableEx = S.AP.GetExportTable("GT_WEB")


            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Speichern.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            'Throw New Exception(m_strMessage)

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'con.Close()
            'con.Dispose()

            'cmd.Dispose()

        End Try

        Return m_tblResult

    End Function

    'Private Function CreateTableForSap() As DataTable

    '    Dim tblTemp As New DataTable

    '    With tblTemp.Columns
    '        .Add("KENNUNG", System.Type.GetType("System.String"))
    '        .Add("POS_KURZTEXT", System.Type.GetType("System.String"))
    '        .Add("POS_TEXT", System.Type.GetType("System.String"))
    '        .Add("EMAIL", System.Type.GetType("System.String"))
    '        .Add("LOEVM", System.Type.GetType("System.String"))
    '        .Add("AENDT", System.Type.GetType("System.String"))
    '        .Add("AENUS", System.Type.GetType("System.String"))
    '        .Add("RETUR_BEM", System.Type.GetType("System.String"))
    '    End With

    '    Return tblTemp

    'End Function

    Private Function CreateTableForOutput() As DataTable

        Dim tblTemp As New DataTable

        With tblTemp.Columns
            .Add("ID", System.Type.GetType("System.String"))
            .Add("Name", System.Type.GetType("System.String"))
            .Add("Ort", System.Type.GetType("System.String"))
            .Add("Mail1", System.Type.GetType("System.String"))
            .Add("Mail2", System.Type.GetType("System.String"))
            .Add("Mail3", System.Type.GetType("System.String"))
        End With

        Return tblTemp

    End Function

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        Dim strKunnr As String = ""
        m_strClassAndMethod = "AvisChange06.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1

        Try
            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_READ_AUFTRDAT_006", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            'strCom = "EXEC Z_M_READ_AUFTRDAT_006 @I_KUNNR='" & strKunnr & "', " _
            '                                       & "@I_POS_KURZTEXT='EINBAUFIRMA', " _
            '                                       & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)

            'cmd.ExecuteNonQuery()

            Dim tblTemp2 As DataTable
            'tblTemp2 = DirectCast(pSAPTableAUFTRAG.Value, DataTable)
            tblTemp2 = S.AP.GetExportTableWithInitExecute("Z_M_READ_AUFTRDAT_006.GT_WEB",
                                                            "I_KUNNR, I_POS_KURZTEXT",
                                                            m_objUser.KUNNR.ToSapKunnr(), Kennung)

            Dim row As DataRow
            Dim NewRow As DataRow
            m_tblResult = CreateTableForOutput()
            For Each row In tblTemp2.Rows
                NewRow = m_tblResult.NewRow

                NewRow("ID") = row("POS_KURZTEXT").ToString

                Dim splitarr() As String

                splitarr = Split(row("POS_TEXT").ToString, ";")
                Dim i As Integer
                For i = 0 To splitarr.Length - 1
                    If i = 0 Then NewRow("Name") = splitarr(i).ToString
                    If i = 1 Then NewRow("Ort") = splitarr(i).ToString
                Next
                splitarr = Split(row("EMAIL").ToString, ";")
                For i = 0 To splitarr.Length - 1
                    If i = 0 Then NewRow("Mail1") = splitarr(i).ToString
                    If i = 1 Then NewRow("Mail2") = splitarr(i).ToString
                    If i = 2 Then NewRow("Mail3") = splitarr(i).ToString
                Next
                m_tblResult.Rows.Add(NewRow)
            Next
            m_tblResult.AcceptChanges()

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If



            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
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
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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


    Private Function MakeStandardDate(ByVal strInput As String) As String
        Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)

        If IsDate(strTemp) Then
            Return strTemp
        Else
            Return String.Empty
        End If
    End Function

#End Region
End Class
