Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change_02
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
        m_strClassAndMethod = "Change_02.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_STRAUB.SAPProxy_STRAUB() ' SAPProxy_SIXT.SAPProxy_SIXT()

            Dim CarDetail As SAPProxy_STRAUB.ZDAD_M_WEB_REPORTING_SIXT
            Dim CarDetailTable As New SAPProxy_STRAUB.ZDAD_M_WEB_REPORTING_SIXTTable()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_tblFahrzeuge = New DataTable()
                With m_tblFahrzeuge.Columns
                    .Add("KUNPDI", System.Type.GetType("System.String"))
                    .Add("Equipmentnummer", System.Type.GetType("System.String"))
                    .Add("Hersteller", System.Type.GetType("System.String"))
                    .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                    .Add("Kennzeichen", System.Type.GetType("System.String"))
                    .Add("Abmeldedatum", System.Type.GetType("System.String"))
                    .Add("Erstzulassungsdatum", System.Type.GetType("System.String"))
                    .Add("Eingangsdatum", System.Type.GetType("System.String"))
                    .Add("ActionNOTHING", System.Type.GetType("System.Boolean"))
                    .Add("ActionDELE", System.Type.GetType("System.Boolean"))
                    .Add("Action", System.Type.GetType("System.String"))
                    .Add("Bemerkung", System.Type.GetType("System.String"))
                    .Add("Typcode", System.Type.GetType("System.String"))
                End With

                m_intStatus = 0
                m_strMessage = ""

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abmbereit_Laufzeit", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Abmbereit_Laufzeit(m_strCustomer, CarDetailTable)
                objSAP.CommitWork()
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If

                If CarDetailTable.Count > 0 Then
                    Dim rowNew As DataRow
                    For Each CarDetail In CarDetailTable
                        'If CarDetail.Liznr.Length > 0 Then
                        rowNew = m_tblFahrzeuge.NewRow
                        rowNew("KUNPDI") = CarDetail.Kunpdi
                        rowNew("Equipmentnummer") = CarDetail.Equnr
                        rowNew("Hersteller") = CarDetail.Zzherst_Text
                        rowNew("Fahrgestellnummer") = CarDetail.Zzfahrg
                        rowNew("Kennzeichen") = CarDetail.Zzkenn
                        rowNew("Typcode") = CarDetail.Zzmodell
                        Dim datTemp As DateTime
                        'Abmeldedatum
                        If Not CarDetail.Repla_Date Is Nothing AndAlso IsNumeric(CarDetail.Repla_Date) Then
                            datTemp = HelpProcedures.MakeDateStandard(CarDetail.Repla_Date)
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Abmeldedatum") = Left(CStr(datTemp), 10)
                            End If
                        End If
                        'Erstzulassungsdatum
                        If Not CarDetail.Vdatu Is Nothing AndAlso IsNumeric(CarDetail.Vdatu) Then
                            datTemp = HelpProcedures.MakeDateStandard(CarDetail.Vdatu)
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Erstzulassungsdatum") = Left(CStr(datTemp), 10)
                            End If
                        End If
                        'Eingangsdatum Abmeldeunterlagen
                        If Not CarDetail.Erdat Is Nothing AndAlso IsNumeric(CarDetail.Erdat) Then
                            datTemp = HelpProcedures.MakeDateStandard(CarDetail.Erdat)
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Eingangsdatum") = Left(CStr(datTemp), 10)
                            End If
                        End If
                        datTemp = Nothing
                        rowNew("Action") = ""
                        rowNew("ActionNOTHING") = True
                        rowNew("ActionDELE") = False
                        rowNew("Bemerkung") = ""
                        m_tblFahrzeuge.Rows.Add(rowNew)
                        'End If
                    Next
                Else
                    m_intStatus = -2202
                    m_strMessage = "Keine Vorgangsinformationen vorhanden."
                End If

                Dim col As DataColumn
                For Each col In m_tblFahrzeuge.Columns
                    col.ReadOnly = False
                Next

                WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c), m_tblFahrzeuge)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & " , " & m_strMessage, m_tblFahrzeuge)
            Finally
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
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

            Dim objSAP As New SAPProxy_STRAUB.SAPProxy_STRAUB()

            m_intStatus = 0
            m_strMessage = ""

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim tmpDataView As DataView
            tmpDataView = m_tblFahrzeuge.DefaultView

            tmpDataView.RowFilter = "ActionNOTHING = 0"

            Dim i As Int32
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            For i = 0 To tmpDataView.Count - 1
                Dim strBemerkung As String = "Vorgang erfolgreich"
                Dim strActionText As String
                Try
                    Dim strAction As String
                    strActionText = "Löschen"

                    m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abmbereit_Laufaen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                    Dim strKUNNR As String = m_strCustomer
                    Dim strKUNPDI As String = CType(tmpDataView.Item(i)("KUNPDI"), String)
                    Dim strZZFAHRG As String = CType(tmpDataView.Item(i)("Fahrgestellnummer"), String)
                    Dim strZZKENN As String = CType(tmpDataView.Item(i)("Kennzeichen"), String)
                    objSAP.Z_M_Abmbereit_Laufaen(strKUNNR, strKUNPDI, strZZFAHRG, strZZKENN)
                    objSAP.CommitWork()
                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                    End If

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                    strBemerkung = ex.Message
                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                    End If
                Finally
                    m_objLogApp.LogStandardIdentity = m_intStandardLogID
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End Try

                m_tblFahrzeuge.AcceptChanges()
                Dim tmpRows As DataRow()
                Dim tmpRow As DataRow
                tmpRows = m_tblFahrzeuge.Select("Fahrgestellnummer = '" & CType(tmpDataView.Item(i)("Fahrgestellnummer"), String) & "'")
                If tmpRows.Length > 0 Then
                    tmpRows(0).BeginEdit()
                    tmpRows(0).Item("Action") = strActionText
                    tmpRows(0).Item("Bemerkung") = strBemerkung
                    tmpRows(0).EndEdit()
                    m_tblFahrzeuge.AcceptChanges()
                End If
            Next

            objSAP.Connection.Close()
            objSAP.Dispose()

            m_blnGestartet = False
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Change_02.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.07.07    Time: 9:51
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' 
' ************************************************
