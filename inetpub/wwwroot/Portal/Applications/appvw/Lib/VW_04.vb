Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Imports SapORM.Contracts
Imports SapORM.Models

<Serializable()> _
Public Class VW_04
    Inherits Base.Business.BankBase
    REM § Lese-/Schreibfunktion, Kunde: VW,
    REM § Show - BAPI: Z_M_IMP_ZUL_HAEND_001,
    REM § Change - BAPI: Z_M_IMP_ZUL_HAEND_002.

#Region "Declarations"
    Private m_strVorhaben As String
    Private m_strStatusBearbeitung As String
    Private m_strSchluessel As String
    Private m_tblHaendler As DataTable
    Private m_tblReturnChanged As DataTable
    Private m_strKonzBearb As String
#End Region

#Region "Properties"
    Public Property KonzBearb() As String
        Get
            Return m_strKonzBearb
        End Get
        Set(ByVal Value As String)
            m_strKonzBearb = Value
        End Set
    End Property

    Public ReadOnly Property ReturnChanged() As DataTable
        Get
            Return m_tblReturnChanged
        End Get
    End Property

    Public Property Schluessel() As String
        Get
            Return m_strSchluessel
        End Get
        Set(ByVal Value As String)
            m_strSchluessel = Value
        End Set
    End Property

    Public Property StatusBearbeitung() As String
        Get
            Return m_strStatusBearbeitung
        End Get
        Set(ByVal Value As String)
            m_strStatusBearbeitung = Value
        End Set
    End Property

    Public Property Vorhaben() As String
        Get
            Return m_strVorhaben
        End Get
        Set(ByVal Value As String)
            m_strVorhaben = Value
        End Set
    End Property

    Public Property Haendler() As DataTable
        Get
            Return m_tblHaendler
        End Get
        Set(ByVal Value As DataTable)
            m_tblHaendler = Value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        'DataTable anlegen
        Me.m_tblHaendler = New DataTable()
        With Me.m_tblHaendler.Columns
            .Add("Schluessel", System.Type.GetType("System.String"))
            .Add("Vorhaben", System.Type.GetType("System.String"))
            .Add("HaendlerID", System.Type.GetType("System.String"))
            .Add("HaendlerNr", System.Type.GetType("System.String"))
            .Add("HaendlerName", System.Type.GetType("System.String"))
            .Add("HaendlerAdresse", System.Type.GetType("System.String"))
            .Add("Anzahl", System.Type.GetType("System.Int64"))
            .Add("Zielbahnhof", System.Type.GetType("System.String"))
            .Add("Status", System.Type.GetType("System.String"))
            .Add("HaendlerName1", System.Type.GetType("System.String"))
            .Add("HaendlerName2", System.Type.GetType("System.String"))
            .Add("HaendlerStrasse", System.Type.GetType("System.String"))
            .Add("HaendlerHausnummer", System.Type.GetType("System.String"))
            .Add("HaendlerPLZ", System.Type.GetType("System.String"))
            .Add("HaendlerOrt", System.Type.GetType("System.String"))
        End With

        Customer = objUser.KUNNR
    End Sub

#End Region

#Region "Methods"

    Public Overrides Sub Show()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            'MakeDestination()

            'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()
            'Dim tblSAP As New SAPProxy_VW.ZDAD_WEB_IMP_ZUL_001Table()
            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Imp_Zul_Haend_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                m_tblResult = S.AP.GetExportTableWithInitExecute("Z_M_Imp_Zul_Haend_001.GT_WEB",
                                                                                  "I_KUNNR, I_VORHABEN, I_STAT_BEARB",
                                                                                  m_objUser.KUNNR.ToSapKunnr, m_strVorhaben, m_strStatusBearbeitung)

                'objSAP.Z_M_Imp_Zul_Haend_001(m_strCustomer, m_strStatusBearbeitung, m_strVorhaben, tblSAP)
                'objSAP.CommitWork()

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                'm_tblResult = tblSAP.ToADODataTable

                m_tblResult.AcceptChanges()
                m_tblResult.Columns.Add("HaendlerAdresse", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("Status", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("Selected", System.Type.GetType("System.Boolean"))
                m_tblResult.Columns.Add("NewAddress", System.Type.GetType("System.Boolean"))
                Dim row As DataRow
                For Each row In m_tblResult.Rows
                    row("Selected") = False
                    row("NewAddress") = False
                    row("HaendlerAdresse") = CStr(row("Name1_sus")) & " " & _
                                            CStr(row("Name2_sus")) & ", " & _
                                            CStr(row("Strasse_sus")) & " " & _
                                            CStr(row("Hausnr_sus")) & ", " & _
                                            CStr(row("Pstlz_sus")) & " " & _
                                            CStr(row("Ort_sus"))

                    Dim s1 As String = CStr(row("Stat_sus_bearb"))
                    Dim s2 As String = s1.Trim(" "c)

                    Select Case CStr(row("Stat_sus_bearb")).Trim(" "c)
                        Case ""
                            row("Status") = "U"
                        Case "1"
                            row("Status") = "F"
                        Case "9"
                            row("Status") = "S"
                    End Select
                Next
                m_tblResult.AcceptChanges()

                Dim tmpVw As DataView = m_tblResult.DefaultView()
                tmpVw.Sort = "Nummer_sus,Referenz2,Status"

                'DataTable leeren (falls gefüllt)
                Me.m_tblHaendler.Clear()

                Dim intCount As Integer = 0
                Dim strHaendlernummer As String = "XXXXX"
                Dim strVorhabennummer As String = "XXXXX"
                Dim strStatus As String = "XXXXX"
                Dim strHaendlerID As String = ""
                Dim strHaendlerName As String = ""
                Dim strHaendlerAdresse As String = ""
                Dim strZielbahnhof As String = ""

                Dim strHaendlerName1 As String = ""
                Dim strHaendlerName2 As String = ""
                Dim strHaendlerStrasse As String = ""
                Dim strHaendlerHausnummer As String = ""
                Dim strHaendlerPLZ As String = ""
                Dim strHaendlerOrt As String = ""


                Dim intLoop As Integer
                For intLoop = 0 To tmpVw.Count - 1
                    If strHaendlernummer = CStr(tmpVw(intLoop)("Nummer_sus")) And strVorhabennummer = CStr(tmpVw(intLoop)("Referenz2")) And strStatus = CStr(tmpVw(intLoop)("Status")) Then
                        intCount += 1
                    Else
                        If intCount > 0 Then
                            'Neue Row hinzufügen
                            row = Me.m_tblHaendler.NewRow()
                            row("Schluessel") = strVorhabennummer & "/" & strHaendlernummer & "/" & strStatus
                            row("Vorhaben") = strVorhabennummer
                            row("HaendlerID") = strHaendlerID
                            row("HaendlerNr") = strHaendlernummer
                            row("HaendlerName") = strHaendlerName
                            row("HaendlerAdresse") = strHaendlerAdresse
                            row("Zielbahnhof") = strZielbahnhof

                            row("HaendlerName1") = strHaendlerName1
                            row("HaendlerName2") = strHaendlerName2
                            row("HaendlerStrasse") = strHaendlerStrasse
                            row("HaendlerHausnummer") = strHaendlerHausnummer
                            row("HaendlerPLZ") = strHaendlerPLZ
                            row("HaendlerOrt") = strHaendlerOrt
                            row("Status") = strStatus
                            row("Anzahl") = intCount
                            m_tblHaendler.Rows.Add(row)
                        End If
                        strVorhabennummer = CStr(tmpVw(intLoop)("Referenz2"))
                        strHaendlernummer = CStr(tmpVw(intLoop)("Nummer_sus"))
                        strHaendlerName = CStr(tmpVw(intLoop)("Name1_sus")) & " " & CStr(tmpVw(intLoop)("Name2_sus"))
                        strHaendlerAdresse = CStr(tmpVw(intLoop)("HaendlerAdresse"))
                        strHaendlerID = CStr(tmpVw(intLoop)("Referenz1"))
                        strZielbahnhof = CStr(tmpVw(intLoop)("Zielbahnhof"))

                        strHaendlerName1 = CStr(tmpVw(intLoop)("Name1_sus"))
                        strHaendlerName2 = CStr(tmpVw(intLoop)("Name2_sus"))
                        strHaendlerStrasse = CStr(tmpVw(intLoop)("Strasse_sus"))
                        strHaendlerHausnummer = CStr(tmpVw(intLoop)("Hausnr_sus"))
                        strHaendlerPLZ = CStr(tmpVw(intLoop)("Pstlz_sus"))
                        strHaendlerOrt = CStr(tmpVw(intLoop)("Ort_sus"))

                        strStatus = CStr(tmpVw(intLoop)("Status"))
                        intCount = 1
                    End If
                Next
                If Not strHaendlernummer = "XXXXX" Then
                    'Neue Row hinzufügen
                    row = Me.m_tblHaendler.NewRow()
                    row("Schluessel") = strVorhabennummer & "/" & strHaendlernummer & "/" & strStatus
                    row("Vorhaben") = strVorhabennummer
                    row("HaendlerID") = strHaendlerID
                    row("HaendlerNr") = strHaendlernummer
                    row("HaendlerName") = strHaendlerName
                    row("HaendlerAdresse") = strHaendlerAdresse
                    row("Zielbahnhof") = strZielbahnhof

                    row("HaendlerName1") = strHaendlerName1
                    row("HaendlerName2") = strHaendlerName2
                    row("HaendlerStrasse") = strHaendlerStrasse
                    row("HaendlerHausnummer") = strHaendlerHausnummer
                    row("HaendlerPLZ") = strHaendlerPLZ
                    row("HaendlerOrt") = strHaendlerOrt
                    row("Status") = strStatus
                    row("Anzahl") = intCount
                    m_tblHaendler.Rows.Add(row)
                End If

                m_tblResultExcel = m_tblResult.Copy
                m_tblResultExcel.Columns.Remove("HaendlerAdresse")
                m_tblResultExcel.Columns.Remove("Selected")
                m_tblResultExcel.Columns.Remove("NewAddress")

                m_tblResultExcel.Columns(0).ColumnName = "Vorhaben"
                m_tblResultExcel.Columns(1).ColumnName = "Vorhaben Teil"
                m_tblResultExcel.Columns(2).ColumnName = "IKZ"
                m_tblResultExcel.Columns(3).ColumnName = "Niederlassung DPAG"
                m_tblResultExcel.Columns(4).ColumnName = "Straße"
                m_tblResultExcel.Columns(5).ColumnName = "PLZ"
                m_tblResultExcel.Columns(6).ColumnName = "Ort"
                m_tblResultExcel.Columns(7).ColumnName = "Händlernummer"
                m_tblResultExcel.Columns(8).ColumnName = "Händlername 1"
                m_tblResultExcel.Columns(9).ColumnName = "Händlername 2"
                m_tblResultExcel.Columns(10).ColumnName = "Händlerstraße"
                m_tblResultExcel.Columns(11).ColumnName = "Händlerhausnummer"
                m_tblResultExcel.Columns(12).ColumnName = "Händler-PLZ"
                m_tblResultExcel.Columns(13).ColumnName = "Händlerort"
                m_tblResultExcel.Columns(14).ColumnName = "Zielbahnhof"
                m_tblResultExcel.Columns(15).ColumnName = "Bemerkung Händler"
                m_tblResultExcel.Columns(16).ColumnName = "Status Händler"
                m_tblResultExcel.Columns(17).ColumnName = "Hersteller"
                m_tblResultExcel.Columns(18).ColumnName = "Fahrzeugtyp"
                m_tblResultExcel.Columns(19).ColumnName = "Variante"

                For Each row In m_tblResultExcel.Rows
                    row("Status Händler") = CStr(row("Status"))
                Next

                m_tblResultExcel.Columns.Remove("Status")

                m_tblResult.Columns.Add("Name1Input", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("Name2Input", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("StrasseInput", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("HausnummerInput", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("PLZInput", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("OrtInput", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("ZielbahnhofInput", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("BemerkungInput", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("StatusInput", System.Type.GetType("System.String"))

                WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c), Nothing)
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ",   " & Replace(m_strMessage, "<br>", " "), Nothing)

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

            Finally
                'objSAP.Connection.Close()
                'objSAP.Dispose()
                m_blnGestartet = False
            End Try

        End If

    End Sub

    Public Overrides Sub Change()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            'MakeDestination()
            'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()
            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                Dim intLoop As Integer
                m_intStatus = 0
                m_strMessage = ""

                For intLoop = m_tblResult.Rows.Count - 1 To 0 Step -1
                    If Not CBool(m_tblResult.Rows(intLoop)("Selected")) Then
                        m_tblResult.Rows.RemoveAt(intLoop)
                    End If
                Next

                If m_tblResult.Rows.Count = 0 Then
                    Throw New Exception("Es sind keine Daten zum Speichern vorhanden.")
                End If

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Imp_Zul_Haend_002", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim listSAP As Generic.List(Of Z_M_Imp_Zul_Haend_002.GT_WEB) _
                    = Z_M_Imp_Zul_Haend_002.GT_WEB.GetImportListWithInit(S.AP, "I_KUNNR, I_NAME_SUS_BEARB, I_KONZ_BEARB", m_objUser.KUNNR.ToSapKunnr, m_objUser.UserName, m_strKonzBearb)

                Dim rowSAP As Z_M_Imp_Zul_Haend_002.GT_WEB
                For intLoop = 0 To m_tblResult.Rows.Count - 1
                    rowSAP = New Z_M_Imp_Zul_Haend_002.GT_WEB()
                    rowSAP.REFERENZ1 = CStr(m_tblResult.Rows(intLoop)("Referenz1"))
                    rowSAP.Nummer_Sus = CStr(m_tblResult.Rows(intLoop)("Nummer_Sus"))
                    rowSAP.Stat_Sus_Bearb = CStr(m_tblResult.Rows(intLoop)("StatusInput"))

                    If CBool(m_tblResult.Rows(intLoop)("NewAddress")) Then
                        rowSAP.Name1_Sus = CStr(m_tblResult.Rows(intLoop)("Name1Input"))
                        rowSAP.Name2_Sus = CStr(m_tblResult.Rows(intLoop)("Name2Input"))
                        rowSAP.Strasse_Sus = CStr(m_tblResult.Rows(intLoop)("StrasseInput"))
                        rowSAP.Hausnr_Sus = CStr(m_tblResult.Rows(intLoop)("HausnummerInput"))
                        rowSAP.Pstlz_Sus = CStr(m_tblResult.Rows(intLoop)("PLZInput"))
                        rowSAP.Ort_Sus = CStr(m_tblResult.Rows(intLoop)("OrtInput"))
                        rowSAP.ZIELBAHNHOF = CStr(m_tblResult.Rows(intLoop)("ZielbahnhofInput"))
                        rowSAP.Bem_Sus_Bearb = CStr(m_tblResult.Rows(intLoop)("BemerkungInput"))
                    Else
                        rowSAP.Name1_Sus = CStr(m_tblResult.Rows(intLoop)("Name1_Sus"))
                        rowSAP.Name2_Sus = CStr(m_tblResult.Rows(intLoop)("Name2_Sus"))
                        rowSAP.Strasse_Sus = CStr(m_tblResult.Rows(intLoop)("Strasse_Sus"))
                        rowSAP.Hausnr_Sus = CStr(m_tblResult.Rows(intLoop)("Hausnr_Sus"))
                        rowSAP.Pstlz_Sus = CStr(m_tblResult.Rows(intLoop)("Pstlz_Sus"))
                        rowSAP.Ort_Sus = CStr(m_tblResult.Rows(intLoop)("Ort_Sus"))
                        rowSAP.ZIELBAHNHOF = CStr(m_tblResult.Rows(intLoop)("Zielbahnhof"))
                        rowSAP.Bem_Sus_Bearb = CStr(m_tblResult.Rows(intLoop)("Bem_Sus_Bearb"))
                    End If

                    listSAP.Add(rowSAP)
                Next

                S.AP.ApplyImport(listSAP)
                S.AP.Execute()

                'Dim tblSAP As New SAPProxy_VW.ZDAD_WEB_IMP_ZUL_002Table()
                'Dim rowSAP As SAPProxy_VW.ZDAD_WEB_IMP_ZUL_002
                'For intLoop = 0 To m_tblResult.Rows.Count - 1
                '    rowSAP = New SAPProxy_VW.ZDAD_WEB_IMP_ZUL_002()
                '    rowSAP.Referenz1 = CStr(m_tblResult.Rows(intLoop)("Referenz1"))
                '    rowSAP.Nummer_Sus = CStr(m_tblResult.Rows(intLoop)("Nummer_Sus"))
                '    rowSAP.Stat_Sus_Bearb = CStr(m_tblResult.Rows(intLoop)("StatusInput"))

                '    If CBool(m_tblResult.Rows(intLoop)("NewAddress")) Then
                '        rowSAP.Name1_Sus = CStr(m_tblResult.Rows(intLoop)("Name1Input"))
                '        rowSAP.Name2_Sus = CStr(m_tblResult.Rows(intLoop)("Name2Input"))
                '        rowSAP.Strasse_Sus = CStr(m_tblResult.Rows(intLoop)("StrasseInput"))
                '        rowSAP.Hausnr_Sus = CStr(m_tblResult.Rows(intLoop)("HausnummerInput"))
                '        rowSAP.Pstlz_Sus = CStr(m_tblResult.Rows(intLoop)("PLZInput"))
                '        rowSAP.Ort_Sus = CStr(m_tblResult.Rows(intLoop)("OrtInput"))
                '        rowSAP.Zielbahnhof = CStr(m_tblResult.Rows(intLoop)("ZielbahnhofInput"))
                '        rowSAP.Bem_Sus_Bearb = CStr(m_tblResult.Rows(intLoop)("BemerkungInput"))
                '    Else
                '        rowSAP.Name1_Sus = CStr(m_tblResult.Rows(intLoop)("Name1_Sus"))
                '        rowSAP.Name2_Sus = CStr(m_tblResult.Rows(intLoop)("Name2_Sus"))
                '        rowSAP.Strasse_Sus = CStr(m_tblResult.Rows(intLoop)("Strasse_Sus"))
                '        rowSAP.Hausnr_Sus = CStr(m_tblResult.Rows(intLoop)("Hausnr_Sus"))
                '        rowSAP.Pstlz_Sus = CStr(m_tblResult.Rows(intLoop)("Pstlz_Sus"))
                '        rowSAP.Ort_Sus = CStr(m_tblResult.Rows(intLoop)("Ort_Sus"))
                '        rowSAP.Zielbahnhof = CStr(m_tblResult.Rows(intLoop)("Zielbahnhof"))
                '        rowSAP.Bem_Sus_Bearb = CStr(m_tblResult.Rows(intLoop)("Bem_Sus_Bearb"))
                '    End If

                '    tblSAP.Add(rowSAP)
                'Next

                'objSAP.Z_M_Imp_Zul_Haend_002(Me.m_strKonzBearb, Me.m_strCustomer, m_objUser.UserName, tblSAP)
                'objSAP.CommitWork()


                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                'm_tblReturnChanged = tblSAP.ToADODataTable
                m_tblReturnChanged = S.AP.GetExportTable("GT_WEB")

                m_tblReturnChanged.Columns(0).ColumnName = "Fahrzeugreferenz"
                m_tblReturnChanged.Columns(1).ColumnName = "Haendlernummer"
                m_tblReturnChanged.Columns(2).ColumnName = "Name 1"
                m_tblReturnChanged.Columns(3).ColumnName = "Name 2"
                m_tblReturnChanged.Columns(4).ColumnName = "Strasse"
                m_tblReturnChanged.Columns(5).ColumnName = "Hausnummer"
                m_tblReturnChanged.Columns(6).ColumnName = "PLZ"
                m_tblReturnChanged.Columns(7).ColumnName = "Ort"
                m_tblReturnChanged.Columns(8).ColumnName = "Zielbahnhof"
                m_tblReturnChanged.Columns(9).ColumnName = "Bemerkung"
                m_tblReturnChanged.Columns(10).ColumnName = "Status"
                m_tblReturnChanged.Columns(11).ColumnName = "Rückkehrcode"

                Dim row As DataRow
                For Each row In m_tblReturnChanged.Rows

                    Dim s1 As String = CStr(row("Status"))
                    Dim s2 As String = s1.Trim(" "c)

                    Select Case CStr(row("Status")).Trim(" "c)
                        Case ""
                            row("Status") = "U"
                        Case "1"
                            row("Status") = "F"
                        Case "9"
                            row("Status") = "S"
                    End Select
                Next
                m_tblReturnChanged.AcceptChanges()
            Catch ex As Exception
                Select Case ex.Message
                    Case "ERROR"
                        m_intStatus = -9998
                        m_strMessage = "Nicht alle Sätze konnten verarbeitet werden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                'objSAP.Connection.Close()
                'objSAP.Dispose()
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: VW_04.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:49
' Created in $/CKAG/Applications/appvw/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 18.07.07   Time: 13:38
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Report "Workflow Werkstattzuordnungsliste" für VW und DP zum Testen
' fertig
' 
' *****************  Version 4  *****************
' User: Uha          Date: 17.07.07   Time: 18:51
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 3. teilfunktionaler Zwischenstand von Change01
' 
' *****************  Version 3  *****************
' User: Uha          Date: 16.07.07   Time: 17:22
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 2. teilfunktionaler Zwischenstand von Change01
' 
' *****************  Version 2  *****************
' User: Uha          Date: 16.07.07   Time: 14:21
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Teilfunktionaler Zwischenstand von Change01
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.07.07   Time: 16:58
' Created in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Report "Workflow Werkstattzuordnungsliste 1" roh hinzugefügt = Keine
' Komplierfehler aber nicht lauffähig
' 
' ************************************************
