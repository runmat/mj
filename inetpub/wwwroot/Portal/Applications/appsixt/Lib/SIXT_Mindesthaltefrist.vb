Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class SIXT_Mindesthaltefrist
    REM § Lese-/Schreibfunktion, Kunde: SIXT, 
    REM § Show - BAPI: Z_M_ABMBEREIT_LAUFZEIT,
    REM § Change - BAPI: Z_M_ABMBEREIT_LAUFAEN.

    Inherits Base.Business.BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_tblFahrzeuge As DataTable
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
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub Show()
 
    End Sub
    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "SIXT_Mindesthaltefrist.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABMBEREIT_LAUFZEIT", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("KUNNR", m_strKUNNR)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_ABMBEREIT_LAUFZEIT", "KUNNR", m_strKUNNR)

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("AUSGABE")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                If tblTemp2.Rows.Count > 0 Then

                    m_tblFahrzeuge = New DataTable()
                    m_tblFahrzeuge.Columns.Add("KUNPDI", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Equipmentnummer", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Briefnummer", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Abmeldedatum", System.Type.GetType("System.DateTime"))
                    m_tblFahrzeuge.Columns.Add("ActionNOTHING", System.Type.GetType("System.Boolean"))
                    m_tblFahrzeuge.Columns.Add("ActionDELE", System.Type.GetType("System.Boolean"))
                    m_tblFahrzeuge.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
                    m_tblFahrzeuge.Columns.Add("Action", System.Type.GetType("System.String"))

                    Dim col As DataColumn
                    For Each col In m_tblFahrzeuge.Columns
                        col.ReadOnly = False
                    Next


                    Dim rowNew As DataRow
                    Dim CarDetail As DataRow
                    For Each CarDetail In tblTemp2.Rows
                        'If CarDetail.Liznr.Length > 0 Then
                        rowNew = m_tblFahrzeuge.NewRow
                        rowNew("KUNPDI") = CarDetail("Kunpdi")
                        rowNew("Equipmentnummer") = CarDetail("Equnr")
                        rowNew("Fahrgestellnummer") = CarDetail("Zzfahrg")
                        rowNew("Briefnummer") = CarDetail("Zzbrief")
                        rowNew("Kennzeichen") = CarDetail("Zzkenn")
                        Dim datTemp As DateTime
                        If IsDate(CarDetail("Repla_Date")) Then
                            datTemp = CDate(CarDetail("Repla_Date"))
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Abmeldedatum") = datTemp.ToShortDateString
                            End If
                        End If
                        datTemp = Nothing
                        rowNew("Bemerkung") = ""
                        rowNew("Action") = ""
                        rowNew("ActionNOTHING") = True
                        rowNew("ActionDELE") = False
                        m_tblFahrzeuge.Rows.Add(rowNew)
                        'End If
                    Next
                Else
                    m_intStatus = -2202
                    m_strMessage = "Keine Vorgangsinformationen vorhanden."
                End If


                WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c), m_tblFahrzeuge)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & " , " & m_strMessage, m_tblFahrzeuge)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
    Public Overrides Sub Change()

    End Sub
    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "SIXT_Mindesthaltefrist.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Dim tmpDataView As DataView
            tmpDataView = m_tblFahrzeuge.DefaultView

            tmpDataView.RowFilter = "ActionNOTHING = 0"


            m_intIDSAP = -1
            Dim i As Int32

            For i = 0 To tmpDataView.Count - 1
                Dim strBemerkung As String = "Vorgang erfolgreich"
                Dim strActionText As String = ""
                Try
                    Dim strAction As String = ""
                    strActionText = "Löschen"

                    Dim strKUNPDI As String = CType(tmpDataView.Item(i)("KUNPDI"), String)
                    Dim strZZFAHRG As String = CType(tmpDataView.Item(i)("Fahrgestellnummer"), String)
                    Dim strZZKENN As String = CType(tmpDataView.Item(i)("Kennzeichen"), String)

                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABMBEREIT_LAUFAEN", m_objApp, m_objUser, page)

                    'myProxy.setImportParameter("KUNNR", m_strKUNNR)
                    'myProxy.setImportParameter("ZZKENN", strZZKENN)
                    'myProxy.setImportParameter("ZZFAHRG", strZZFAHRG)
                    'myProxy.setImportParameter("KUNPDI", strKUNPDI)

                    'myProxy.callBapi()

                    S.AP.InitExecute("Z_M_ABMBEREIT_LAUFAEN", "KUNNR, ZZKENN, ZZFAHRG, KUNPDI", m_strKUNNR, strZZKENN, strZZFAHRG, strKUNPDI)

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                    strBemerkung = ex.Message
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                Finally
                    m_blnGestartet = False
                End Try

                m_tblFahrzeuge.AcceptChanges()
                Dim tmpRows As DataRow()
                tmpRows = m_tblFahrzeuge.Select("Fahrgestellnummer = '" & CType(tmpDataView.Item(i)("Fahrgestellnummer"), String) & "'")
                If tmpRows.Length > 0 Then
                    tmpRows(0).BeginEdit()
                    tmpRows(0).Item("Action") = strActionText
                    tmpRows(0).Item("Bemerkung") = strBemerkung
                    tmpRows(0).EndEdit()
                    m_tblFahrzeuge.AcceptChanges()
                End If

            Next
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: SIXT_Mindesthaltefrist.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 3.03.11    Time: 14:57
' Updated in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 18.03.10   Time: 14:21
' Updated in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:38
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
