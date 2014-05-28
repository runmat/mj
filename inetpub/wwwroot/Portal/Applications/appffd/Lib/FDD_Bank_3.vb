Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class FDD_Bank_3
    REM § Lese-/Schreibfunktion, Kunde: FFD, 
    REM § Show/Report - BAPI: Zz_Unbezahlt_Equipment,
    REM § Change - BAPI: Als Internetfunktionalität abgeschaltet (Zz_Unbezahlt_Equi_Change).

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_tblFahrzeuge As DataTable
    Private m_tblFahrzeugeExcel As DataTable
    Private m_strFahrgestellnummer As String
    Private m_strFahrgestellnummerSuche As String
    Private m_strVertragsnummer As String
    Private m_strOrdernummer As String
    Private m_strVorgaenge As String
#End Region

#Region " Properties"
    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeuge = Value
        End Set
    End Property

    Public Property FahrzeugeExcel() As DataTable
        Get
            Return m_tblFahrzeugeExcel
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeugeExcel = Value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property

    Public Property FahrgestellnummerSuche() As String
        Get
            Return m_strFahrgestellnummerSuche
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummerSuche = Value
        End Set
    End Property

    Public Property Ordernummer() As String
        Get
            Return m_strOrdernummer
        End Get
        Set(ByVal Value As String)
            m_strOrdernummer = Value
        End Set
    End Property

    Public Property Vertragsnummer() As String
        Get
            Return m_strVertragsnummer
        End Get
        Set(ByVal Value As String)
            m_strVertragsnummer = Value
        End Set
    End Property

    Public Property Vorgaenge() As String
        Get
            Return m_strVorgaenge
        End Get
        Set(ByVal Value As String)
            m_strVorgaenge = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_strVertragsnummer = ""
        m_strOrdernummer = ""
        m_strFahrgestellnummer = ""
        m_strVorgaenge = "alle"
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Sub Report(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FDD_Bank_3.Report"

        Try
            m_intStatus = 0

            If CheckCustomerData() Then
                m_strMessage = ""
                Dim strKKBER As String = ""

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unbezahlte_Equipments", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("00000" & m_strCustomer, 5))
                'myProxy.setImportParameter("I_KNRZE", "")
                'myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_CHASSIS_NUM", m_strFahrgestellnummerSuche)
                'myProxy.setImportParameter("I_LIZNR", m_strVertragsnummer)
                'myProxy.setImportParameter("I_ZZREFERENZ1", m_strOrdernummer)
                'myProxy.setImportParameter("I_VKORG", "1510")
                'myProxy.setImportParameter("I_ZB2", "")

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Unbezahlte_Equipments", "I_KUNNR,I_KNRZE,I_KONZS,I_CHASSIS_NUM,I_LIZNR,I_ZZREFERENZ1,I_VKORG,I_ZB2",
                                 Right("00000" & m_strCustomer, 5), "", Right("0000000000" & m_objUser.KUNNR, 10), m_strFahrgestellnummerSuche,
                                 m_strVertragsnummer, m_strOrdernummer, "1510", "")

                m_tblFahrzeuge = New DataTable()
                m_tblFahrzeuge.Columns.Add("Vertragsnummer", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Finanzierungsart", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Ordernummer", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("COC", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Erfassung Fahrzeug", System.Type.GetType("System.DateTime"))
                m_tblFahrzeuge.Columns.Add("Angefordert am", System.Type.GetType("System.DateTime"))
                m_tblFahrzeuge.Columns.Add("Angefordert um", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Gesperrt", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Freigegeben am", System.Type.GetType("System.DateTime"))
                m_tblFahrzeuge.Columns.Add("Freigegeben um", System.Type.GetType("System.String"))
                m_tblFahrzeuge.Columns.Add("Versendet am", System.Type.GetType("System.DateTime"))

                m_tblFahrzeugeExcel = New DataTable()
                m_tblFahrzeugeExcel.Columns.Add("Vertragsnummer", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Finanzierungsart", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Ordernummer", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("COC", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Erfassung Fahrzeug", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Angefordert am", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Angefordert um", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Gesperrt", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Freigegeben am", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Freigegeben um", System.Type.GetType("System.String"))
                m_tblFahrzeugeExcel.Columns.Add("Versendet am", System.Type.GetType("System.String"))

                Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
                Dim rowTemp As DataRow

                For Each rowTemp In tblTemp.Rows
                    Dim blnAddRow As Boolean = False
                    If m_strVorgaenge = "alle" Then
                        blnAddRow = True
                    Else
                        If m_strVorgaenge = "angefordert" And rowTemp("ZZANFDT").ToString.Length > 0 Then
                            blnAddRow = True
                        Else
                            If m_strVorgaenge = "nichtangefordert" And rowTemp("ZZANFDT").ToString.Length = 0 Then
                                blnAddRow = True
                            End If
                        End If
                    End If

                    If blnAddRow Then
                        Dim rowNew As DataRow = m_tblFahrzeuge.NewRow
                        Dim datTemp As DateTime
                        Dim strTime As String = ""

                        Dim rowNewExcel As DataRow = m_tblFahrzeugeExcel.NewRow
                        rowNew("Vertragsnummer") = rowTemp("LIZNR")
                        rowNewExcel("Vertragsnummer") = rowTemp("LIZNR")

                        rowNew("Fahrgestellnummer") = rowTemp("CHASSIS_NUM")
                        rowNewExcel("Fahrgestellnummer") = rowTemp("CHASSIS_NUM")

                        rowNew("Finanzierungsart") = rowTemp("ZZFINART")
                        rowNewExcel("Finanzierungsart") = rowTemp("ZZFINART")

                        rowNew("Ordernummer") = rowTemp("ZZREFERENZ1")
                        rowNewExcel("Ordernummer") = rowTemp("ZZREFERENZ1")

                        rowNew("COC") = rowTemp("ZZCOCKZ")
                        rowNewExcel("COC") = rowTemp("ZZCOCKZ")

                        If IsDate(rowTemp("ERDAT").ToString) Then
                            datTemp = CDate(rowTemp("ERDAT").ToString)
                            rowNew("Erfassung Fahrzeug") = datTemp
                            rowNewExcel("Erfassung Fahrzeug") = Format(datTemp, "dd.MM.yyyy")
                        End If

                        If IsDate(rowTemp("ZZANFDT").ToString) Then
                            datTemp = CDate(rowTemp("ZZANFDT").ToString)
                            rowNew("Angefordert am") = datTemp
                            rowNewExcel("Angefordert am") = Format(datTemp, "dd.MM.yyyy")
                        End If

                        strTime = CStr(HelpProcedures.MakeTimeStandard(rowTemp("ZZANFZT").ToString))
                        If strTime <> "00:00:00" Then
                            rowNew("Angefordert um") = strTime
                            rowNewExcel("Angefordert um") = strTime
                        End If

                        Dim strTemp As String = ""
                        Select Case rowTemp("ZZKKBER").ToString
                            Case "0001"
                                strTemp = "Standard temporär"
                            Case "0002"
                                strTemp = "Standard endgültig"
                            Case "0003"
                                strTemp = "Retail"
                            Case "0004"
                                strTemp = "Delayed Payment"
                            Case "0005"
                                strTemp = "Händlereigene Zulassung (HEZ)"
                        End Select

                        If strTemp.Length > 0 Then
                            rowNew("Kontingentart") = strTemp
                            rowNewExcel("Kontingentart") = strTemp
                        End If

                        If rowTemp("CMGST").ToString = "B" Then
                            rowNew("Gesperrt") = "X"
                            rowNewExcel("Gesperrt") = "X"
                        Else
                            rowNew("Gesperrt") = ""
                            rowNewExcel("Gesperrt") = ""
                        End If

                        If IsDate(rowTemp("ZZFREIDT").ToString) Then
                            datTemp = CDate(rowTemp("ZZFREIDT").ToString)
                            rowNew("Freigegeben am") = datTemp
                            rowNewExcel("Freigegeben am") = Format(datTemp, "dd.MM.yyyy")
                        End If

                        strTime = CStr(HelpProcedures.MakeTimeStandard(rowTemp("ZZFREIZT").ToString))
                        If strTime <> "00:00:00" Then
                            rowNew("Freigegeben um") = strTime
                            rowNewExcel("Freigegeben um") = strTime
                        End If

                        If IsDate(rowTemp("ZZTMPDT").ToString) Then
                            datTemp = CDate(rowTemp("ZZTMPDT").ToString)
                            rowNew("Versendet am") = datTemp
                            rowNewExcel("Versendet am") = Format(datTemp, "dd.MM.yyyy")
                        End If


                        m_tblFahrzeuge.Rows.Add(rowNew)
                        m_tblFahrzeugeExcel.Rows.Add(rowNewExcel)
                    End If
                Next

                WriteLogEntry(True, "CHASSIS_NUM=" & m_strFahrgestellnummerSuche & ", KNRZE=, KONZS=" & m_objUser.KUNNR & ", KUNNR=" & Right("00000" & m_strCustomer, 5) & ", LIZNR=" & m_strVertragsnummer & ", ZZREFERENZ1=" & m_strOrdernummer, m_tblFahrzeugeExcel)
            End If
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_intStatus = -1
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case "NO_HAENDLER"
                    m_intStatus = -2
                    m_strMessage = "Händler nicht vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select

            WriteLogEntry(False, "CHASSIS_NUM=" & m_strFahrgestellnummerSuche & ", KNRZE=, KONZS=" & m_objUser.KUNNR & ", KUNNR=" & Right("00000" & m_strCustomer, 5) & ", LIZNR=" & m_strVertragsnummer & ", ZZREFERENZ1=" & m_strOrdernummer & " , " & Replace(m_strMessage, "<br>", " "), m_tblFahrzeugeExcel)
        End Try

    End Sub

    Public Overrides Sub Change()
       
    End Sub
#End Region
End Class

' ************************************************
' $History: FDD_Bank_3.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 10.07.09   Time: 9:58
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 9.07.09    Time: 11:18
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 19.06.09   Time: 16:48
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 14.06.07   Time: 15:09
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.06.07   Time: 17:03
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Portal - Startapplication 13.06.2005
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' ************************************************
