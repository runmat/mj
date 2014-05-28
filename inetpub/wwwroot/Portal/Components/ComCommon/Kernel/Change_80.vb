Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Change_80
    REM § Lese-/Schreibfunktion, Kunde: DAD, 
    REM § Show - BAPI: Z_M_Flc_Anz_Auftr_002,
    REM § Change - BAPI: Hier inaktiv.

    Inherits BankBase

#Region " Declarations"
    Private m_strGebiet As String
    Private m_strHaendlerNr As String
    Private m_strPruefdat_von As String
    Private m_strPruefdat_bis As String
    Private m_strStatusInput As String
    Private m_tblResultPositionen As DataTable
    Private m_tblResultSummary As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultSummary() As DataTable
        Get
            Return m_tblResultSummary
        End Get
    End Property

    Public ReadOnly Property ResultPositionen() As DataTable
        Get
            Return m_tblResultPositionen
        End Get
    End Property

    Public Property StatusInput() As String
        Get
            Return m_strStatusInput
        End Get
        Set(ByVal Value As String)
            m_strStatusInput = Value
        End Set
    End Property

    Public Property Pruefdat_von() As String
        Get
            Return m_strPruefdat_von
        End Get
        Set(ByVal Value As String)
            m_strPruefdat_von = Value
        End Set
    End Property

    Public Property Pruefdat_bis() As String
        Get
            Return m_strPruefdat_bis
        End Get
        Set(ByVal Value As String)
            m_strPruefdat_bis = Value
        End Set
    End Property

    Public Property HaendlerNr() As String
        Get
            Return m_strHaendlerNr
        End Get
        Set(ByVal Value As String)
            m_strHaendlerNr = Value
        End Set
    End Property

    Public Property Gebiet() As String
        Get
            Return m_strGebiet
        End Get
        Set(ByVal Value As String)
            m_strGebiet = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub Show()
        Dim tableFahrzeuge As New DataTable()
        Dim row As DataRow

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ComCommon.SAPProxy_ComCommon()
            Dim intID As Int32 = -1

            Try
                Dim tblText As New SAPProxy_ComCommon.ZDAD_M_FLC_TEXT_001Table()
                Dim tblAuftrKopf As New SAPProxy_ComCommon.ZDAD_M_WEB_FLC_AUFTR_KOPF_002Table()
                Dim tblAuftrPos As New SAPProxy_ComCommon.ZDAD_M_WEB_FLC_AUFTR_POS_002Table()

                MakeDestination()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Flc_Anz_Auftr_002", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim tmpVon As String = ""
                If IsDate(m_strPruefdat_von) Then
                    tmpVon = MakeDateSAP(CDate(m_strPruefdat_von))
                End If
                Dim tmpBis As String = ""
                If IsDate(m_strPruefdat_bis) Then
                    tmpBis = MakeDateSAP(CDate(m_strPruefdat_bis))
                End If
                objSAP.Z_M_Flc_Anz_Auftr_002( _
                                    m_strGebiet, _
                                    Right("0000000000" & m_objUser.KUNNR, 10), _
                                    m_strHaendlerNr, _
                                    tmpBis, _
                                    tmpVon, _
                                    m_strStatusInput, _
                                    tblText, _
                                    tblAuftrKopf, _
                                    tblAuftrPos)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If
                Dim tblTemp As DataTable = tblAuftrKopf.ToADODataTable
                If (tblTemp Is Nothing) OrElse (tblTemp.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    Dim tblTexte As DataTable = tblText.ToADODataTable
                    tblTemp.Columns.Add("Bemerkung", System.Type.GetType("System.String"))

                    Dim rowTemp As DataRow
                    Dim rowTemp2() As DataRow
                    Dim i As Integer

                    For Each rowTemp In tblTemp.Rows
                        rowTemp("Street") = CStr(rowTemp("Street")) & " " & CStr(rowTemp("House_num1"))
                        rowTemp("Bemerkung") = ""

                        If Not tblTexte Is Nothing AndAlso tblTexte.Rows.Count > 0 Then
                            rowTemp2 = tblTexte.Select("VBELN = '" & CStr(rowTemp("VBELN")) & "'")
                            If rowTemp2.Length > 0 Then
                                For i = 0 To rowTemp2.Length - 1
                                    rowTemp("Bemerkung") = CStr(rowTemp("Bemerkung")) & CStr(rowTemp2(i)("Tdline"))
                                Next
                            End If

                            rowTemp("Bemerkung") = CStr(rowTemp("Bemerkung")).TrimStart(" "c)
                            rowTemp("Vbeln") = CStr(rowTemp("Vbeln")).TrimStart("0"c)
                            rowTemp("Kunnr") = CStr(rowTemp("Kunnr")).TrimStart("0"c)
                        End If
                    Next

                    Dim tmpView As DataView = tblTemp.DefaultView
                    tmpView.Sort = "Gebiet, Kunnr"
                    Dim iLoop As Integer
                    Dim iCount As Integer = 1
                    Dim strGebiet As String = "XXXX"
                    Dim strKunnr As String = "XXXX"

                    Dim tmpSummary As DataTable = tblTemp.Clone
                    tmpSummary.Columns.RemoveAt(13)
                    tmpSummary.Columns.RemoveAt(12)
                    tmpSummary.Columns.RemoveAt(11)
                    tmpSummary.Columns.RemoveAt(10)
                    tmpSummary.Columns.RemoveAt(7)
                    tmpSummary.Columns.RemoveAt(1)
                    tmpSummary.Columns.RemoveAt(0)
                    tmpSummary.Columns.Add("Anzahl", System.Type.GetType("System.Int32"))

                    For iLoop = 0 To tmpView.Count - 1
                        If (Not strGebiet = CStr(tmpView(iLoop)("Gebiet"))) Or (Not strKunnr = CStr(tmpView(iLoop)("Kunnr"))) Then
                            If Not strGebiet = "XXXX" Then
                                'Neue Zeile hinzufügen
                                rowTemp = tmpSummary.NewRow
                                rowTemp("Gebiet") = CStr(tmpView(iLoop - 1)("Gebiet"))
                                rowTemp("Kunnr") = CStr(tmpView(iLoop - 1)("Kunnr"))
                                rowTemp("Name1") = CStr(tmpView(iLoop - 1)("Name1"))
                                rowTemp("Name2") = CStr(tmpView(iLoop - 1)("Name2"))
                                rowTemp("Street") = CStr(tmpView(iLoop - 1)("Street"))
                                rowTemp("Post_code1") = CStr(tmpView(iLoop - 1)("Post_code1"))
                                rowTemp("City1") = CStr(tmpView(iLoop - 1)("City1"))
                                rowTemp("Anzahl") = iCount
                                tmpSummary.Rows.Add(rowTemp)
                                'Zähler zurücksetzen
                                iCount = 1
                            End If
                            strGebiet = CStr(tmpView(iLoop)("Gebiet"))
                            strKunnr = CStr(tmpView(iLoop)("Kunnr"))
                        Else
                            'Hochzählen
                            iCount += 1
                        End If
                    Next
                    If Not strGebiet = "XXXX" Then
                        'Neue Zeile hinzufügen
                        rowTemp = tmpSummary.NewRow
                        rowTemp("Gebiet") = CStr(tmpView(tmpView.Count - 1)("Gebiet"))
                        rowTemp("Kunnr") = CStr(tmpView(tmpView.Count - 1)("Kunnr"))
                        rowTemp("Name1") = CStr(tmpView(tmpView.Count - 1)("Name1"))
                        rowTemp("Name2") = CStr(tmpView(tmpView.Count - 1)("Name2"))
                        rowTemp("Street") = CStr(tmpView(tmpView.Count - 1)("Street"))
                        rowTemp("Post_code1") = CStr(tmpView(tmpView.Count - 1)("Post_code1"))
                        rowTemp("City1") = CStr(tmpView(tmpView.Count - 1)("City1"))
                        rowTemp("Anzahl") = iCount
                        tmpSummary.Rows.Add(rowTemp)
                    End If

                    m_tblResultSummary = tmpSummary.Copy

                    'Umschreiben zur Datumskonvertierung -
                    'das kann die Column-Übersetzung nicht!
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
                    tblTemp2.Columns.Add("Zzsend0", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Zzsend1", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Zzsend2", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Txt30", System.Type.GetType("System.String"))
                    tblTemp2.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
                    Dim newRow As DataRow
                    For Each rowTemp In tblTemp.Rows
                        newRow = tblTemp2.NewRow
                        newRow("Vbeln") = CStr(rowTemp("Vbeln"))
                        newRow("Wldat") = MakeDateStandard(CStr(rowTemp("Wldat")))
                        newRow("Gebiet") = CStr(rowTemp("Gebiet"))
                        newRow("Kunnr") = CStr(rowTemp("Kunnr"))
                        newRow("Name1") = CStr(rowTemp("Name1"))
                        newRow("Name2") = CStr(rowTemp("Name2"))
                        newRow("Street") = CStr(rowTemp("Street"))
                        newRow("Post_code1") = CStr(rowTemp("Post_code1"))
                        newRow("City1") = CStr(rowTemp("City1"))
                        newRow("Zzsend0") = CStr(rowTemp("Zzsend0"))
                        newRow("Zzsend1") = CStr(rowTemp("Zzsend1"))
                        newRow("Zzsend2") = CStr(rowTemp("Zzsend2"))
                        newRow("Txt30") = CStr(rowTemp("Txt30"))
                        newRow("Bemerkung") = CStr(rowTemp("Bemerkung"))

                        tblTemp2.Rows.Add(newRow)
                    Next
                    m_tblResult = tblTemp2.Copy

                    tblTemp = tblAuftrPos.ToADODataTable
                    For Each row In tblTemp.Rows
                        row("Vbeln") = CStr(row("Vbeln")).TrimStart("0"c)
                        row("Posnr") = CStr(row("Posnr")).TrimStart("0"c)
                    Next
                    m_tblResultPositionen = tblTemp.Copy

                    m_intStatus = 0
                End If


                WriteLogEntry(True, "Gebiet=" & m_strGebiet & ", KUNNR=" & m_objUser.KUNNR, m_tblResult)

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
                WriteLogEntry(False, "Gebiet=" & m_strGebiet & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
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
        'If Not m_blnGestartet Then
        '    m_blnGestartet = True

        '    Dim objSAP As New SAPProxy_ComCommon.SAPProxy_ComCommon()

        '    Try
        '        Dim i As Integer
        '        For i = m_tblResult.Rows.Count - 1 To 0 Step -1
        '            If CBool(m_tblResult.Rows(i)("AUSGEWAEHLT")) = False Then
        '                'Überzählige Zeilen löschen
        '                m_tblResult.Rows.RemoveAt(i)
        '            Else
        '                'Werte zur Übergabe vorbereiten
        '                m_tblResult.Rows(i)("CHASSIS_NUM") = CStr(m_tblResult.Rows(i)("CHASSIS_NUM1")) & CStr(m_tblResult.Rows(i)("CHASSIS_NUM2"))
        '                m_tblResult.Rows(i)("FCODE") = ""
        '            End If
        '        Next
        '        'Überzählige Spalten löschen
        '        m_tblResult.Columns.Remove("CHASSIS_NUM1")
        '        m_tblResult.Columns.Remove("CHASSIS_NUM2")
        '        m_tblResult.Columns.Remove("WAEHLBAR")
        '        m_tblResult.Columns.Remove("AUSGEWAEHLT")

        '        Dim tblFahrzeuge As New SAPProxy_VW.ZDAD_WEB_EXP_FIN_001Table()
        '        tblFahrzeuge.FromADODataTable(m_tblResult)

        '        MakeDestination()
        '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        '        objSAP.Connection.Open()

        '        If m_objLogApp Is Nothing Then
        '            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '        End If
        '        m_intIDSAP = -1

        '        m_intStatus = 0
        '        m_strMessage = ""

        '        m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Imp_Fin_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)


        '        Dim strDaten As String = "Vorhaben=" & m_strVorhaben & _
        '            ", KUNNR=" & m_objUser.KUNNR

        '        objSAP.Z_M_Imp_Fin_001(Right("0000000000" & m_objUser.KUNNR, 10), tblFahrzeuge)
        '        objSAP.CommitWork()

        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True, Left(strDaten, 500))
        '        End If

        '        m_tblResult = tblFahrzeuge.ToADODataTable

        '        Dim tmpRow As DataRow
        '        For Each tmpRow In m_tblResult.Rows
        '            Dim strTemp As String = ""
        '            If Not TypeOf tmpRow("FCODE") Is System.DBNull Then
        '                strTemp = CStr(tmpRow("FCODE"))
        '            End If
        '            Select Case strTemp
        '                Case ""
        '                    tmpRow("FCODE") = "Vorgang O.K."
        '                Case "000"
        '                    tmpRow("FCODE") = "Vorgang O.K."
        '                Case "001"
        '                    tmpRow("FCODE") = "IKZ nicht zuordenbar"
        '                Case "012"
        '                    tmpRow("FCODE") = "FIN unzulässig"
        '                Case "013"
        '                    tmpRow("FCODE") = "Fehler bei Update"
        '                Case Else
        '                    tmpRow("FCODE") = "Unbekannter Fehler"
        '            End Select
        '        Next
        '        m_tblResult.Columns.Add("CHASSIS_NUM1", System.Type.GetType("System.String"), "")
        '        m_tblResult.Columns.Add("CHASSIS_NUM2", System.Type.GetType("System.String"), "")
        '        m_tblResult.Columns.Add("WAEHLBAR", System.Type.GetType("System.Boolean"), "True")
        '        m_tblResult.Columns.Add("AUSGEWAEHLT", System.Type.GetType("System.Boolean"), "True")
        '    Catch ex As Exception
        '        m_intStatus = -9999
        '        m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden. (" & ex.Message & ")"
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
        '        End If
        '    Finally
        '        If m_intIDsap > -1 Then
        '            m_objLogApp.LogStandardIdentity = m_intStandardLogID
        '            m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
        '        End If

        '        objSAP.Connection.Close()
        '        objSAP.Dispose()

        '        m_blnGestartet = False
        '    End Try
        'End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Change_80.vb $
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
' User: Uha          Date: 20.09.07   Time: 16:34
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1181: Testversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 19.09.07   Time: 17:29
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1181: Funktionslose Rohversion
' 
' ************************************************
