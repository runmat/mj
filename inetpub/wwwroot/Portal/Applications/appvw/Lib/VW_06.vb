Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class VW_06
    REM § Lese-/Schreibfunktion, Kunde: VW, 
    REM § Show - BAPI: Z_M_Exp_Fin_001,
    REM § Change - BAPI: Z_M_Imp_Fin_001.

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_strVorhaben As String = String.Empty

    Private m_tblFahrzeugeSuche As DataTable

    Private dataArray As ArrayList
    Private rowToChange As DataRow
#End Region

#Region " Properties"
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

    Public Property Vorhaben() As String
        Get
            Return m_strVorhaben
        End Get
        Set(ByVal Value As String)
            m_strVorhaben = Value
        End Set
    End Property

    Public Property FahrzeugeSuche() As DataTable
        Get
            Return m_tblFahrzeugeSuche
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeugeSuche = Value
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

            'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()
            Dim intID As Int32 = -1

            Try
                'Dim tblFahrzeugeIn As New SAPProxy_VW.ZDAD_WEB_IMP_FIN_001Table()
                Dim tblIn As DataTable = m_tblFahrzeugeSuche.Copy
                tblIn.Columns.RemoveAt(2)
                'tblFahrzeugeIn.FromADODataTable(tblIn)

                'Dim tblFahrzeugeOut As New SAPProxy_VW.ZDAD_WEB_EXP_FIN_001Table()

                'MakeDestination()
                'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                'objSAP.Connection.Open()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Exp_Fin_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim tblFahrzeugeIn As DataTable = S.AP.GetImportTableWithInit("Z_M_Exp_Fin_001.GT_WEB_IN", "I_KUNNR, I_REFERENZ2", m_objUser.KUNNR.ToSapKunnr, m_strVorhaben)
                tblIn.Copy(tblFahrzeugeIn)
                m_tblResult = S.AP.GetExportTableWithExecute("GT_WEB_OUT")

                'objSAP.Z_M_Exp_Fin_001(Right("0000000000" & m_objUser.KUNNR, 10), m_strVorhaben, tblFahrzeugeIn, tblFahrzeugeOut)
                'objSAP.CommitWork()

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'm_tblResult = tblFahrzeugeOut.ToADODataTable

                m_tblResult.Columns.Add("CHASSIS_NUM1", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("CHASSIS_NUM2", System.Type.GetType("System.String"))
                m_tblResult.Columns.Add("WAEHLBAR", System.Type.GetType("System.Boolean"))
                m_tblResult.Columns.Add("AUSGEWAEHLT", System.Type.GetType("System.Boolean"))

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    Dim tmpRow As DataRow
                    Dim arrayRows() As DataRow
                    For Each tmpRow In m_tblResult.Rows
                        Dim strTemp As String = ""

                        tmpRow("AUSGEWAEHLT") = False
                        tmpRow("CHASSIS_NUM1") = ""
                        If Not TypeOf tmpRow("REFERENZ1") Is System.DBNull Then
                            If m_tblFahrzeugeSuche.Rows.Count > 0 Then
                                arrayRows = m_tblFahrzeugeSuche.Select("REFERENZ1='" & CStr(tmpRow("REFERENZ1")) & "'")
                                If arrayRows.Length > 0 Then
                                    If Not TypeOf arrayRows(0)("CHASSIS_NUM_2") Is System.DBNull Then
                                        tmpRow("CHASSIS_NUM2") = arrayRows(0)("CHASSIS_NUM_2")
                                    End If
                                    If Not TypeOf arrayRows(0)("CHASSIS_NUM") Is System.DBNull Then
                                        strTemp = Left(CStr(arrayRows(0)("CHASSIS_NUM")), 11)
                                    End If
                                    tmpRow("CHASSIS_NUM1") = strTemp
                                    m_tblFahrzeugeSuche.Rows.Remove(arrayRows(0))
                                End If
                            End If
                        End If

                        If CStr(tmpRow("CHASSIS_NUM1")).Length = 0 Then
                            If Not TypeOf tmpRow("CHASSIS_NUM") Is System.DBNull Then
                                tmpRow("CHASSIS_NUM1") = Left(CStr(tmpRow("CHASSIS_NUM")), 11)
                            End If
                        End If

                        strTemp = ""
                        If Not TypeOf tmpRow("FCODE") Is System.DBNull Then
                            strTemp = CStr(tmpRow("FCODE"))
                        End If
                        Select Case strTemp
                            Case ""
                                tmpRow("FCODE") = "Suche erfolgreich"
                                tmpRow("WAEHLBAR") = True
                            Case "000"
                                tmpRow("FCODE") = "Suche erfolgreich"
                                tmpRow("WAEHLBAR") = True
                            Case "001"
                                tmpRow("FCODE") = "IKZ nicht zuordenbar"
                                tmpRow("WAEHLBAR") = False
                            Case "010"
                                tmpRow("FCODE") = "FIN ist bereits zugeordnet (" & CStr(tmpRow("CHASSIS_NUM")) & ")"
                                tmpRow("WAEHLBAR") = False
                            Case "011"
                                tmpRow("FCODE") = "Keine Übereinstimmung FIN (Zeichen 1 bis 11) - Übergeben: " & CStr(tmpRow("CHASSIS_NUM1")) & ", Erwartet: " & Left(CStr(tmpRow("CHASSIS_NUM")), 11)
                                tmpRow("WAEHLBAR") = False
                            Case Else
                                tmpRow("FCODE") = "Unbekannter Fehler"
                                tmpRow("WAEHLBAR") = False
                        End Select

                        Dim blnCheckDoubleVIN As Boolean = True
                        If TypeOf tmpRow("CHASSIS_NUM") Is System.DBNull Then
                            blnCheckDoubleVIN = False
                        Else
                            strTemp = "CHASSIS_NUM='" & CStr(tmpRow("CHASSIS_NUM")) & "' AND "
                        End If
                        If TypeOf tmpRow("CHASSIS_NUM2") Is System.DBNull Then
                            blnCheckDoubleVIN = False
                        Else
                            strTemp &= "CHASSIS_NUM2='" & CStr(tmpRow("CHASSIS_NUM2")) & "'"
                        End If

                        If blnCheckDoubleVIN Then
                            Dim tmpRows() As DataRow = m_tblResult.Select(strTemp)
                            If tmpRows.Length > 1 Then
                                Dim intLoop As Integer
                                For intLoop = 1 To tmpRows.Length - 1
                                    tmpRows(intLoop)("FCODE") = "FIN doppelt übermittelt"
                                    tmpRows(intLoop)("WAEHLBAR") = False
                                Next
                            End If
                        End If
                    Next

                    m_intStatus = 0
                End If


                WriteLogEntry(True, "Vorhaben=" & m_strVorhaben & ", KUNNR=" & m_objUser.KUNNR, m_tblResult)

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
                WriteLogEntry(False, "Vorhaben=" & m_strVorhaben & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                'objSAP.Connection.Close()
                'objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()

            Try
                Dim i As Integer
                For i = m_tblResult.Rows.Count - 1 To 0 Step -1
                    If CBool(m_tblResult.Rows(i)("AUSGEWAEHLT")) = False Then
                        'Überzählige Zeilen löschen
                        m_tblResult.Rows.RemoveAt(i)
                    Else
                        'Werte zur Übergabe vorbereiten
                        m_tblResult.Rows(i)("CHASSIS_NUM") = CStr(m_tblResult.Rows(i)("CHASSIS_NUM1")) & CStr(m_tblResult.Rows(i)("CHASSIS_NUM2"))
                        m_tblResult.Rows(i)("FCODE") = ""
                    End If
                Next
                'Überzählige Spalten löschen
                m_tblResult.Columns.Remove("CHASSIS_NUM1")
                m_tblResult.Columns.Remove("CHASSIS_NUM2")
                m_tblResult.Columns.Remove("WAEHLBAR")
                m_tblResult.Columns.Remove("AUSGEWAEHLT")

                'Dim tblFahrzeuge As New SAPProxy_VW.ZDAD_WEB_EXP_FIN_001Table()
                'tblFahrzeuge.FromADODataTable(m_tblResult)

                'MakeDestination()
                'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                'objSAP.Connection.Open()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                m_intStatus = 0
                m_strMessage = ""

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Imp_Fin_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)


                Dim tblFahrzeuge As DataTable = S.AP.GetImportTableWithInit("Z_M_Imp_Fin_001.GT_WEB_IN", "I_KUNNR", m_objUser.KUNNR.ToSapKunnr)
                m_tblResult.Copy(tblFahrzeuge)
                m_tblResult = S.AP.GetExportTableWithExecute("GT_WEB_IN")

                'objSAP.Z_M_Imp_Fin_001(Right("0000000000" & m_objUser.KUNNR, 10), tblFahrzeuge)
                'objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    Dim strDaten As String = "Vorhaben=" & m_strVorhaben & ", KUNNR=" & m_objUser.KUNNR
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True, Left(strDaten, 500))
                End If

                'm_tblResult = tblFahrzeuge.ToADODataTable

                Dim tmpRow As DataRow
                For Each tmpRow In m_tblResult.Rows
                    Dim strTemp As String = ""
                    If Not TypeOf tmpRow("FCODE") Is System.DBNull Then
                        strTemp = CStr(tmpRow("FCODE"))
                    End If
                    Select Case strTemp
                        Case ""
                            tmpRow("FCODE") = "Vorgang O.K."
                        Case "000"
                            tmpRow("FCODE") = "Vorgang O.K."
                        Case "001"
                            tmpRow("FCODE") = "IKZ nicht zuordenbar"
                        Case "012"
                            tmpRow("FCODE") = "FIN unzulässig"
                        Case "013"
                            tmpRow("FCODE") = "Fehler bei Update"
                        Case Else
                            tmpRow("FCODE") = "Unbekannter Fehler"
                    End Select
                Next
                m_tblResult.Columns.Add("CHASSIS_NUM1", System.Type.GetType("System.String"), "")
                m_tblResult.Columns.Add("CHASSIS_NUM2", System.Type.GetType("System.String"), "")
                m_tblResult.Columns.Add("WAEHLBAR", System.Type.GetType("System.Boolean"), "True")
                m_tblResult.Columns.Add("AUSGEWAEHLT", System.Type.GetType("System.Boolean"), "True")
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden. (" & ex.Message & ")"
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
            Finally
                If m_intIDsap > -1 Then
                    m_objLogApp.LogStandardIdentity = m_intStandardLogID
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                End If

                'objSAP.Connection.Close()
                'objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: VW_06.vb $
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
' *****************  Version 4  *****************
' User: Uha          Date: 27.08.07   Time: 9:17
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Bugfix in Werkstattzuordnungsliste II: Leerer SAP-Rückgabewert für
' CHASSIS_NUM2 verursachte Fehler
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.08.07   Time: 17:52
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Bugfixing ITA 1120 und 1177
' 
' *****************  Version 2  *****************
' User: Uha          Date: 15.08.07   Time: 11:16
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' ITA 1177 "Werkstattzuordnungsliste II" - testfähige Version
' 
' *****************  Version 1  *****************
' User: Uha          Date: 14.08.07   Time: 13:42
' Created in $/CKG/Applications/AppVW/AppVWWeb/Lib
' ITA 1177 "Werkstattzuordnungsliste II" - kompilierfähige Rohversion
' hinzugefügt
' 
' ************************************************
