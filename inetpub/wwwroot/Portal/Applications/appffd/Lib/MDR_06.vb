Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class MDR_06
    REM § Lese-/Schreibfunktion, Kunde: MDR, 
    REM § Show - BAPI: Z_Dad_Cs_Mdr_Kundaten_Versand,
    REM § Change - BAPI: Z_Dad_Cs_Mdr_Beauftragung

    Inherits BankBase


#Region " Declarations"
    Private m_strSucheHaendlernummer As String

    Private m_strEmpfaengerName As String
    Private m_strEmpfaengerOrt As String
    Private m_strEmpfaengerPLZ As String
    Private m_strEmpfaengerStrasse As String
    Private m_strDatum As String

    Private m_strVersandcode As String
    Private mVersandart As String

    Private m_tblFahrzeuge As DataTable
#End Region

#Region " Properties"
    Public Property Versandart() As String
        Get
            Return mVersandart
        End Get
        Set(ByVal value As String)
            mVersandart = value
        End Set
    End Property

    Public Property SucheHaendlernummer() As String
        Get
            Return m_strSucheHaendlernummer
        End Get
        Set(ByVal Value As String)
            m_strSucheHaendlernummer = Value
        End Set
    End Property

    Public Property Versandcode() As String
        Get
            Return m_strVersandcode
        End Get
        Set(ByVal Value As String)
            m_strVersandcode = Value
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
        mVersandart = ""

        NewTblFahrzeuge()
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True
            m_intIDSAP = -1

            Dim intID As Int32 = -1

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Dad_Cs_Mdr_Kundaten_Versand", m_objApp, m_objUser, Page)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_Dad_Cs_Mdr_Kundaten_Versand")

                m_tblResult = S.AP.GetExportTable("GT_KUNDATEN_VERSAND") 'myProxy.getExportTable("GT_KUNDATEN_VERSAND")
                m_intStatus = 0

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -3331
                        m_strMessage = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim tblBeauftragung As DataTable
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Dad_Cs_Mdr_Beauftragung", m_objApp, m_objUser, page)
                S.AP.Init("Z_Dad_Cs_Mdr_Beauftragung")

                tblBeauftragung = S.AP.GetImportTable("GT_ZDAD_MDR_BEAUFTRAGUNG") 'myProxy.getImportTable("GT_ZDAD_MDR_BEAUFTRAGUNG")

                Dim strDaten As String = "KUNNR=" & KUNNR
                Dim tmpRow As DataRow
                Dim tmpNewRow As DataRow
                'Beauftragungstabelle füllen

                For Each tmpRow In m_tblFahrzeuge.Rows
                    tmpNewRow = tblBeauftragung.NewRow
                    tmpNewRow("Chassis_Num") = Left(CStr(tmpRow("Chassis_Num")), 30)
                    tmpNewRow("Ernam") = Left(CStr(tmpRow("Ernam")), 12)

                    If Not TypeOf tmpRow("Konzs") Is System.DBNull Then
                        tmpNewRow("Konzs") = Left(CStr(tmpRow("Konzs")).ToUpper, 10)
                    End If

                    If Not TypeOf tmpRow("Name1") Is System.DBNull Then
                        tmpNewRow("Name1") = Left(CStr(tmpRow("Name1")), 40)
                    End If

                    If Not TypeOf tmpRow("City1") Is System.DBNull Then
                        tmpNewRow("City1") = Left(CStr(tmpRow("City1")), 40)
                    End If

                    If Not TypeOf tmpRow("Post_Code1") Is System.DBNull Then
                        tmpNewRow("Post_Code1") = Left(CStr(tmpRow("Post_Code1")), 10)
                    End If

                    If Not TypeOf tmpRow("Street") Is System.DBNull Then
                        tmpNewRow("Street") = Left(CStr(tmpRow("Street")), 60)
                    End If

                    tmpNewRow("Vermarktungscode") = Left(CStr(tmpRow("Vermarktungscode")), 10)
                    tmpNewRow("Versandart") = Left(Umschluesselung(CStr(tmpRow("Versandart"))), 32)

                    tblBeauftragung.Rows.Add(tmpNewRow)
                Next

                S.AP.Execute() 'myProxy.callBapi()

                m_tblFahrzeuge = S.AP.GetExportTable("GT_ZDAD_MDR_BEAUFTRAGUNG") 'myProxy.getExportTable("GT_ZDAD_MDR_BEAUFTRAGUNG")

                Dim strTemp As String
                For Each tmpRow In m_tblFahrzeuge.Rows
                    strTemp = ""
                    If Not TypeOf tmpRow("FEHLER_KZ") Is System.DBNull AndAlso Not tmpRow("FEHLER_KZ") Is String.Empty Then
                        strTemp = CStr(tmpRow("FEHLER_KZ"))
                        strTemp = strTemp.Trim(" "c)
                    End If

                    m_tblFahrzeuge.Columns("FEHLER_KZ").MaxLength = 100
                    m_tblFahrzeuge.Columns("Versandart").MaxLength = 100

                    Select Case strTemp
                        Case "0"
                            tmpRow("FEHLER_KZ") = "Angelegt"
                        Case "1"
                            tmpRow("FEHLER_KZ") = "Händler fehlt"
                        Case "2"
                            tmpRow("FEHLER_KZ") = "Fahrgestellnummer schon vorhanden"
                        Case "3"
                            tmpRow("FEHLER_KZ") = "Händler schon vorhanden"
                        Case "4"
                            tmpRow("FEHLER_KZ") = "Fehler bei Adressanlage"
                        Case "5"
                            tmpRow("FEHLER_KZ") = "Versandauftragssatz schon vorhanden"
                        Case "6"
                            tmpRow("FEHLER_KZ") = "ZBII nicht im Bestand"
                        Case "7"
                            tmpRow("FEHLER_KZ") = "Angelegt"
                        Case "8"
                            tmpRow("FEHLER_KZ") = "Angelegt ZBII Bestand AV"
                        Case "9"
                            tmpRow("FEHLER_KZ") = "Angelegt ZBII in Abmeldung"
                        Case "A"
                            tmpRow("FEHLER_KZ") = "Angelegt ZBII nicht im Bestand"
                        Case Else
                            tmpRow("FEHLER_KZ") = "Unbekannter Fehler (Kennzeichen: " & strTemp & ")"
                    End Select
                    tmpRow("Versandart") = Umschluesselung(CStr(tmpRow("Versandart")))

                Next

                m_tblFahrzeuge.AcceptChanges()

            Catch ex As Exception
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden. (" & ex.Message & ")"
                m_intStatus = -9999

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                Dim alDatums As New ArrayList()
                alDatums.Add("ERDAT")

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
        m_tblFahrzeuge = New DataTable
        m_tblFahrzeuge.Columns.Add("KONZS", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("NAME1", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("STREET", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("POST_CODE1", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("CITY1", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("VERSANDART", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("VERMARKTUNGSCODE", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("ERNAM", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("FEHLER_KZ", System.Type.GetType("System.String"))
        m_tblFahrzeuge.Columns.Add("ERDAT", System.Type.GetType("System.DateTime"))
    End Sub

    Public Function Umschluesselung(ByVal wert As String) As String

        Select Case wert.TrimStart(" "c).TrimEnd(" "c)
            Case "1391"
                Return "Versand Standardbrief Deutsche Post AG"
            Case "5530"
                Return "Standardversand mit Sendungsverfolgung und Bestätigung vom Empfänger"
            Case "1390"
                Return "Expressversand mit Zeitoption vor 12:00 Uhr"
            Case "1389"
                Return "Expressversand mit Zeitoption vor 10:00 Uhr"
            Case "1385"
                Return "Expressversand mit Zeitoption vor 09:00 Uhr"
            Case "9999"
                Return "Selbstabholer beim DAD Ahrensburg"
            Case "Versand Standardbrief Deutsche Post AG"
                Return "1391"
            Case "Standardversand mit Sendungsverfolgung und Bestätigung vom Empfänger"
                Return "5530"
            Case "Expressversand mit Zeitoption vor 12:00 Uhr"
                Return "1390"
            Case "Expressversand mit Zeitoption vor 10:00 Uhr"
                Return "1389"
            Case "Expressversand mit Zeitoption vor 09:00 Uhr"
                Return "1385"
            Case "Selbstabholer beim DAD Ahrensburg"
                Return "9999"
            Case Else
                Throw New Exception("Materialnummer/Versandart nicht erkannt: " & wert)
        End Select
    End Function

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
' $History: MDR_06.vb $
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 7.04.10    Time: 13:55
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 6.04.10    Time: 14:30
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 3610
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 4.03.10    Time: 11:27
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.11.08   Time: 10:28
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 2382 testfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 21.05.08   Time: 17:00
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.04.08   Time: 14:03
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 15.04.08   Time: 12:48
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 1825
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 15.04.08   Time: 10:32
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA 1825
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 13.11.07   Time: 17:19
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 13.11.07   Time: 16:06
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 13.11.07   Time: 15:50
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 13.11.07   Time: 14:57
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.08.07   Time: 13:23
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA 1208: Bugfixing 1
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.08.07   Time: 12:30
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA 1208 Testversion
' 
' *****************  Version 1  *****************
' User: Uha          Date: 21.08.07   Time: 17:37
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA 1208: Kompilierfähige Vorversion mit Teilfunktionalität
' 
' ************************************************
