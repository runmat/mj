Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Change_03
    REM § Lese-/Schreibfunktion, Kunde: DAD, 
    REM § Show - BAPI: Z_M_Flc_Anz_Auftr_003,
    REM § Change - BAPI: Z_M_Flc_Change_Auftr_003.

    Inherits BankBase

#Region " Declarations"
    Private m_strVbeln As String = String.Empty

    Private dataArray As ArrayList
    Private rowToChange As DataRow

    Private m_strStatus_I As String

    Private m_tblPositionen As DataTable
#End Region

#Region " Properties"
    Public Property Positionen() As DataTable
        Get
            If m_tblPositionen Is Nothing Then
                ReNewPositions()
            End If
            Return m_tblPositionen
        End Get
        Set(ByVal Value As DataTable)
            m_tblPositionen = Value
        End Set
    End Property

    Public Property Status_I() As String
        Get
            Return m_strStatus_I
        End Get
        Set(ByVal Value As String)
            m_strStatus_I = Value
        End Set
    End Property

    Public Property rowChange() As DataRow
        Get
            Return rowToChange
        End Get
        Set(ByVal Value As DataRow)
            rowToChange = Value
        End Set
    End Property

    Public Property objData() As ArrayList
        Get
            Return dataArray
        End Get
        Set(ByVal Value As ArrayList)
            dataArray = Value
        End Set
    End Property

    Public Property Vbeln() As String
        Get
            Return m_strVbeln
        End Get
        Set(ByVal Value As String)
            m_strVbeln = Value
        End Set
    End Property

    Public Property FahrzeugeAenderung() As DataTable
        Get
            Return m_tblResult
        End Get
        Set(ByVal Value As DataTable)
            m_tblResult = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub Show()
        Dim tableFahrzeuge As New DataTable()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ComCommon.SAPProxy_ComCommon()
            Dim intID As Int32 = -1

            Try
                Dim tblAuftragskoepfeOut As New SAPProxy_ComCommon.ZDAD_M_WEB_FLC_AUFTR_KOPF_003Table()

                MakeDestination()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Flc_Anz_Auftr_003", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_M_Flc_Anz_Auftr_003(Right("0000000000" & m_objUser.KUNNR, 10), m_strStatus_I, tblAuftragskoepfeOut)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If
                m_tblResult = tblAuftragskoepfeOut.ToADODataTable

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    'Umschreiben zur Datumskonvertierung -
                    'das kann die Column-Übersetzung nicht!
                    Dim rowTemp As DataRow
                    Dim tblTemp2 As New DataTable()
                    tblTemp2.Columns.Add("Vbeln", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Wldat", System.Type.GetType("System.DateTime"))
                    tblTemp2.Columns.Add("Gebiet", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Kunnr", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Name1", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Name2", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Street", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Post_code1", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("City1", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Txt30", System.Type.GetType("System.String"))
                    Dim newRow As DataRow
                    For Each rowTemp In m_tblResult.Rows
                        newRow = tblTemp2.NewRow
                        newRow("Vbeln") = CStr(rowTemp("Vbeln")).TrimStart("0"c)
                        newRow("Wldat") = MakeDateStandard(CStr(rowTemp("Wldat")))
                        newRow("Gebiet") = CStr(rowTemp("Gebiet"))
                        newRow("Kunnr") = CStr(rowTemp("Kunnr")).TrimStart("0"c)
                        newRow("Name1") = CStr(rowTemp("Name1"))
                        newRow("Name2") = CStr(rowTemp("Name2"))
                        newRow("Street") = CStr(CStr(rowTemp("Street")) & " " & CStr(rowTemp("House_num1"))).Trim(" "c)
                        newRow("Post_code1") = CStr(rowTemp("Post_code1"))
                        newRow("City1") = CStr(rowTemp("City1"))
                        newRow("Txt30") = CStr(rowTemp("Txt30"))

                        tblTemp2.Rows.Add(newRow)
                    Next
                    m_tblResult = tblTemp2.Copy

                    m_intStatus = 0
                End If


                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -3331
                        m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ComCommon.SAPProxy_ComCommon()

            Try

                Dim tblPositionen As New SAPProxy_ComCommon.ZDAD_M_WEB_FLC_AUFTR_POS_003Table()
                Dim rowPosition As SAPProxy_ComCommon.ZDAD_M_WEB_FLC_AUFTR_POS_003
                Dim i As Integer

                Dim rowTemp As DataRow
                For Each rowTemp In m_tblPositionen.Rows
                    rowPosition = New SAPProxy_ComCommon.ZDAD_M_WEB_FLC_AUFTR_POS_003()
                    For i = 0 To m_tblPositionen.Columns.Count - 2
                        If Not TypeOf rowTemp(i) Is System.DBNull Then
                            rowPosition.Item(i) = rowTemp(i)
                        End If
                    Next
                    tblPositionen.Add(rowPosition)
                Next

                MakeDestination()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                m_intStatus = 0
                m_strMessage = ""

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Flc_Change_Auftr_003", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)


                Dim strDaten As String = "KUNNR=" & m_objUser.KUNNR

                objSAP.Z_M_Flc_Change_Auftr_003(Right("0000000000" & m_objUser.KUNNR, 10), Right("0000000000" & m_strVbeln, 10), tblPositionen)
                objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True, Left(strDaten, 500))
                End If

                m_tblPositionen = tblPositionen.ToADODataTable
                FillAdditionalColumn()
                Dim row As DataRow
                For Each row In m_tblPositionen.Rows
                    row("Pruefungsart") = CStr(row("Art"))
                    If TypeOf row("Fehler") Is System.DBNull OrElse CStr(row("Fehler")).Trim(" "c) = "" Then
                        row("Fehler") = "Status OK"
                    End If
                Next

                m_tblPositionen.Columns.RemoveAt(m_tblPositionen.Columns.Count - 1)
                m_tblPositionen.Columns(0).ColumnName = "Art"
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -9997
                        m_strMessage = "Auftrag nicht  vorhanden."
                    Case "NO_CHANGE"
                        m_intStatus = -9998
                        m_strMessage = "Keine Auftragsänderung."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden. (" & ex.Message & ")"
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
            Finally
                If m_intIDsap > -1 Then
                    m_objLogApp.LogStandardIdentity = m_intStandardLogID
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub FillAdditionalColumn()
        If Not m_tblPositionen Is Nothing AndAlso m_tblPositionen.Rows.Count > 0 Then
            m_tblPositionen.Columns.Add("Art", System.Type.GetType("System.String"))
            Dim tmpRow As DataRow

            For Each tmpRow In m_tblPositionen.Rows
                Select Case CStr(tmpRow("Pruefungsart"))
                    Case "1"
                        tmpRow("Art") = "Buchprüfung"
                    Case "2"
                        tmpRow("Art") = "Körperliche Prüfung"
                    Case "3"
                        tmpRow("Art") = "Vollprüfung"
                End Select
            Next
        End If
    End Sub

    Public Sub ReNewPositions()
        Dim tmpPositionen As New SAPProxy_ComCommon.ZDAD_M_WEB_FLC_AUFTR_POS_003Table()
        m_tblPositionen = tmpPositionen.ToADODataTable
    End Sub
#End Region
End Class

' ************************************************
' $History: Change_03.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Kernel
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Kernel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Kernel
' 
' *****************  Version 2  *****************
' User: Uha          Date: 25.09.07   Time: 17:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1124 hinzugefügt (Change03/Change03_2) und allgemeines Bugfix
' Floorcheck
' 
' *****************  Version 1  *****************
' User: Uha          Date: 24.09.07   Time: 18:07
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1124: Upload Prüflisten via WEB - Nicht lauffähige Vorversion
' 
' ************************************************
