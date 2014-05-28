Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports Microsoft.Data.SAPClient

Public Class Versandbeauftragung
    Inherits BankBase


#Region " Declarations"
    Private m_strSucheHaendlernummer As String

    Private m_strEmpfaengerName As String
    Private m_strEmpfaengerOrt As String
    Private m_strEmpfaengerPLZ As String
    Private m_strEmpfaengerStrasse As String
    Private m_strDatum As String

    Private m_strVersandcode As String
    Private mVersandArt As String
    Private mVermarkt As String
    Private mEigentuemer As String



    Private m_tblFahrzeuge As DataTable
    Private mSapTable As DataTable

#End Region

#Region " Properties"

    Public Property SucheHaendlernummer() As String
        Get
            Return m_strSucheHaendlernummer
        End Get
        Set(ByVal Value As String)
            m_strSucheHaendlernummer = Value
        End Set
    End Property

    Public Property Eigentuemer() As String
        Get
            Return mEigentuemer
        End Get
        Set(ByVal value As String)
            mEigentuemer = value
        End Set
    End Property

    Public Property Vermarktung() As String
        Get
            Return mVermarkt
        End Get
        Set(ByVal value As String)
            mVermarkt = value
        End Set
    End Property
    Public Property Versandart() As String
        Get
            Return mVersandArt
        End Get
        Set(ByVal Value As String)
            mVersandArt = Value
        End Set
    End Property

    Public Property EmpfaengerName() As String
        Get
            Return m_strEmpfaengerName
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerName = Value
        End Set
    End Property

    Public Property EmpfaengerOrt() As String
        Get
            Return m_strEmpfaengerOrt
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerOrt = Value
        End Set
    End Property

    Public Property EmpfaengerPLZ() As String
        Get
            Return m_strEmpfaengerPLZ
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerPLZ = Value
        End Set
    End Property

    Public Property EmpfaengerStrasse() As String
        Get
            Return m_strEmpfaengerStrasse
        End Get
        Set(ByVal Value As String)
            m_strEmpfaengerStrasse = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeuge = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        m_strSucheHaendlernummer = ""
        m_strEmpfaengerName = ""
        m_strEmpfaengerOrt = ""
        m_strEmpfaengerPLZ = ""
        m_strEmpfaengerStrasse = ""
        m_strVersandcode = ""
        mVersandArt = ""

        NewTblFahrzeuge()
    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "Versandbeauftragung.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1
            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()
            Try
                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_DAD_CS_MDR_KUNDATEN_VERSAND @GT_KUNDATEN_VERSAND=@pI_GT_KUNDATEN_VERSAND,"
                strCom = strCom & "@GT_KUNDATEN_VERSAND=@pE_GT_KUNDATEN_VERSAND  OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                'importparameter
                Dim pI_GT_KUNDATEN_VERSAND As New SAPParameter("@pI_GT_KUNDATEN_VERSAND", New DataTable)

                'outputparameter
                Dim pE_GT_KUNDATEN_VERSAND As New SAPParameter("@pE_GT_KUNDATEN_VERSAND", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_GT_KUNDATEN_VERSAND)

                'exportparameter hinzufügen
                cmd.Parameters.Add(pE_GT_KUNDATEN_VERSAND)

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_DAD_CS_MDR_KUNDATEN_VERSAND", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                cmd.ExecuteNonQuery()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                If Not pE_GT_KUNDATEN_VERSAND.Value Is Nothing Then
                    m_tblResult = (DirectCast(pE_GT_KUNDATEN_VERSAND.Value, DataTable))
                    HelpProcedures.killAllDBNullValuesInDataTable(m_tblResult)
                Else
                    m_intStatus = -3331
                    m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Es ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If
                con.Close()
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Sub createSAPTable(ByRef sapTable As DataTable)
        sapTable = New DataTable
        With sapTable
            .Columns.Add("KUNNR_ZS", System.Type.GetType("System.String"))
            .Columns.Add("KONZS", System.Type.GetType("System.String"))
            .Columns.Add("NAME1", System.Type.GetType("System.String"))
            .Columns.Add("STREET", System.Type.GetType("System.String"))
            .Columns.Add("POST_CODE1", System.Type.GetType("System.String"))
            .Columns.Add("CITY1", System.Type.GetType("System.String"))
            .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
            .Columns.Add("LICENSE_NUM", System.Type.GetType("System.String"))
            .Columns.Add("EIGENTUEMER", System.Type.GetType("System.String"))
            .Columns.Add("VERMARKT", System.Type.GetType("System.String"))
            .Columns.Add("CODE_VERSANDART", System.Type.GetType("System.String"))
            .Columns.Add("ERNAM", System.Type.GetType("System.String"))
            .Columns.Add("ERDAT", System.Type.GetType("System.String"))
            .Columns.Add("ZFCODE", System.Type.GetType("System.String"))
        End With
        sapTable.AcceptChanges()

    End Sub


    Public Overrides Sub Change()
        m_strClassAndMethod = "Versandbeauftragung.Change"
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1
            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()
            Try
                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_DAD_FW_BEAUFTRAGUNG_001 @I_KUNNR_AG=@pI_KUNNR_AG, @GT_ZDAD_FW_BEAUFTRAGUNG_001=@pI_GT_ZDAD_FW_BEAUFTRAGUNG_001,"
                strCom = strCom & "@GT_ZDAD_FW_BEAUFTRAGUNG_001=@pE_GT_ZDAD_FW_BEAUFTRAGUNG_001 OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                'importparameter
                Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)
                Dim pI_GT_ZDAD_FW_BEAUFTRAGUNG_001 As New SAPParameter("@pI_GT_ZDAD_FW_BEAUFTRAGUNG_001", New DataTable)

                'outputparameter
                Dim pE_GT_ZDAD_FW_BEAUFTRAGUNG_001 As New SAPParameter("@pE_GT_ZDAD_FW_BEAUFTRAGUNG_001", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_KUNNR_AG)
                cmd.Parameters.Add(pI_GT_ZDAD_FW_BEAUFTRAGUNG_001)

                'exportparameter hinzufügen
                cmd.Parameters.Add(pE_GT_ZDAD_FW_BEAUFTRAGUNG_001)

                'befüllen der Importparameter
                pI_KUNNR_AG.Value = m_strKUNNR


                'Beauftragungstabelle füllen
                If mSapTable Is Nothing Then
                    createSAPTable(mSapTable)
                Else
                    mSapTable.Clear()
                End If



                Dim strDaten As String = "KUNNR=" & KUNNR
                Dim tmpRow As DataRow


                Dim rowBeauftragung As DataRow
                For Each tmpRow In m_tblFahrzeuge.Rows
                    rowBeauftragung = mSapTable.NewRow
                    rowBeauftragung("KONZS") = "" 'immer leer nur bei neuanlage von Händler zu füllen, nicht in der applikation!
                    rowBeauftragung("Chassis_Num") = Left(CStr(tmpRow("Chassis_Num")), 30)
                    rowBeauftragung("Ernam") = Left(CStr(tmpRow("Ernam")), 12)
                    If Not TypeOf tmpRow("KUNNR_ZS") Is System.DBNull Then
                        'sap erwartet bei einem Händer der im SAP schon vorhanden ist die SAP-Nummer 
                        'vergleich mit Adresstabelle 

                        Dim vorhandenRows() As DataRow
                        'so hier soll schlussendlich noch einmal geprüft werden ob der Händler im SAP schon vorhanden ist und wenn ja 
                        'soll die SAP eigene nummer übergeben werden, absolut bescheuert. JJU2008.09.22
                        vorhandenRows = m_tblResult.Select("KONZS='" & tmpRow("KUNNR_ZS").ToString & "'")
                        If Not vorhandenRows.Length = 0 Then
                            rowBeauftragung("KUNNR_ZS") = Right("0000000000" & vorhandenRows(0)("KUNNR").ToString, 10)
                        Else
                            rowBeauftragung("KUNNR_ZS") = Left(CStr(tmpRow("KUNNR_ZS")).ToUpper, 10)
                        End If
                    End If
                    If Not TypeOf tmpRow("Name1") Is System.DBNull Then
                        rowBeauftragung("Name1") = Left(CStr(tmpRow("Name1")), 40)
                    End If
                    If Not TypeOf tmpRow("City1") Is System.DBNull Then
                        rowBeauftragung("City1") = Left(CStr(tmpRow("City1")), 40)
                    End If
                    If Not TypeOf tmpRow("Post_Code1") Is System.DBNull Then
                        rowBeauftragung("Post_Code1") = Left(CStr(tmpRow("Post_Code1")), 10)
                    End If
                    If Not TypeOf tmpRow("Street") Is System.DBNull Then
                        rowBeauftragung("Street") = Left(CStr(tmpRow("Street")), 60)
                    End If
                    rowBeauftragung("VERMARKT") = Left(CStr(tmpRow("VERMARKT")), 10)
                    rowBeauftragung("CODE_VERSANDART") = Left(CStr(tmpRow("CODE_VERSANDART")), 32)
                    rowBeauftragung("EIGENTUEMER") = Left(CStr(tmpRow("EIGENTUEMER")), 1)
                    mSapTable.Rows.Add(rowBeauftragung)
                Next
                HelpProcedures.killAllDBNullValuesInDataTable(mSapTable)
                pI_GT_ZDAD_FW_BEAUFTRAGUNG_001.Value = mSapTable

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_DAD_FW_BEAUFTRAGUNG_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                cmd.ExecuteNonQuery()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                mSapTable = DirectCast(pE_GT_ZDAD_FW_BEAUFTRAGUNG_001.Value, DataTable)
                HelpProcedures.killAllDBNullValuesInDataTable(mSapTable)
                m_tblFahrzeuge = mSapTable

                Dim strTemp As String
                For Each tmpRow In m_tblFahrzeuge.Rows
                    strTemp = ""
                    If Not TypeOf tmpRow("ZFCODE") Is System.DBNull Then
                        strTemp = CStr(tmpRow("ZFCODE"))
                        strTemp = strTemp.Trim(" "c)
                    End If
                    Select Case strTemp
                        Case "0"
                            tmpRow("ZFCODE") = "angelegt"
                        Case "1"
                            tmpRow("ZFCODE") = "Händler unbekannt"
                        Case "2"
                            tmpRow("ZFCODE") = "ZBII bereits beauftragt"
                        Case "3"
                            tmpRow("ZFCODE") = "ZBII bereits versendet"
                        Case "4"
                            tmpRow("ZFCODE") = "keine Grunddaten vorhanden"
                        Case "5"
                            tmpRow("ZFCODE") = "keine Abmeldung"
                        Case "6"
                            tmpRow("ZFCODE") = "ZBII liegt noch nicht vor"
                        Case Else
                            tmpRow("ZFCODE") = "Unbekannter Fehler (Kennzeichen: " & strTemp & ")"
                    End Select
                    'soo jetzt haben wir sap die sap eigene nummer übermittelt, dann bitte wieder umwandeln 
                    'so dass der kunde seine Händernummer sieht...... JJU20080924
                    If Not TypeOf tmpRow("KUNNR_ZS") Is System.DBNull Then
                        Dim vorhandenRows() As DataRow
                        vorhandenRows = m_tblResult.Select("KUNNR='" & tmpRow("KUNNR_ZS").ToString & "'")
                        If Not vorhandenRows.Length = 0 Then
                            tmpRow("KUNNR_ZS") = vorhandenRows(0)("KONZS").ToString
                        End If
                    End If

                Next

                'Datum Formatieren für GridAusgabe, Spalte in DateUmwandeln JJ2007.11.13
                Dim alDatums As New ArrayList()
                alDatums.Add("ERDAT")
                Fahrzeuge = HelpProcedures.FormatGridDateColumn(alDatums, Fahrzeuge)


                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True, Left(strDaten, 500))
                End If
            Catch ex As Exception
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden. (" & ex.Message & ")"
                m_intStatus = -9999
                'End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblFahrzeuge)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If
                con.Close()

                
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Function SuggestionDay() As DateTime
        Dim datTemp As DateTime = Now
        Dim intAddDays As Int32 = 0
        Do While datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday Or intAddDays < 3
            datTemp = datTemp.AddDays(1)
            If Not (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday) Then
                intAddDays += 1
            End If
        Loop
        Return datTemp
    End Function

    Public Sub ClearTblFahrzeuge()
        m_tblFahrzeuge = Nothing
    End Sub

    Private Sub NewTblFahrzeuge()
        createSAPTable(m_tblFahrzeuge)
    End Sub

    Private Function RemoveSingleSpace(ByVal strIn As String) As String
        Dim strReturn As String = ""
        Try
            Dim strOut As String = strIn.Trim(" "c)
            If strOut = "&nbsp;" Then
                strOut = ""
            End If
            strReturn = strOut
        Catch
        End Try
        Return strReturn
    End Function
#End Region
End Class

' ************************************************
' $History: Versandbeauftragung.vb $
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 22.10.08   Time: 16:39
' Updated in $/CKAG/Applications/appfw/Lib
' BugFix Versandbeauftragung per CVS Gagelmann 22102008 mail
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 24.09.08   Time: 8:32
' Updated in $/CKAG/Applications/appfw/Lib
' ITA 1844 vollendung
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.09.08   Time: 10:44
' Updated in $/CKAG/Applications/appfw/Lib
' ITA 1844 sonderlocken für SAP eingefügt. 
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 19.09.08   Time: 15:43
' Updated in $/CKAG/Applications/appfw/Lib
' ITA 1844 anpassungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 9.09.08    Time: 11:27
' Updated in $/CKAG/Applications/appfw/Lib
' ITA 1844
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 8.09.08    Time: 17:39
' Updated in $/CKAG/Applications/appfw/Lib
' ITA 1844 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 3.09.08    Time: 10:01
' Updated in $/CKAG/Applications/appfw/Lib
' ITa 1844 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 1.09.08    Time: 17:40
' Created in $/CKAG/Applications/appfw/Lib
' ITa 1844 Compilierfähig
' 
' ************************************************
