Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change_03
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

            Dim objSAP As New SAPProxy_STRAUB.SAPProxy_STRAUB()

            Dim CarDetail As SAPProxy_STRAUB.ZDAD_M_ST_AVM_MAHN_001
            Dim CarDetailTable As New SAPProxy_STRAUB.ZDAD_M_ST_AVM_MAHN_001Table()

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
                    .Add("Equipmentnummer", System.Type.GetType("System.String"))
                    .Add("Kennzeichen", System.Type.GetType("System.String"))
                    .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                    .Add("Versandanschrift", System.Type.GetType("System.String"))
                    .Add("Versanddatum", System.Type.GetType("System.String"))
                    .Add("ActionNOTHING", System.Type.GetType("System.Boolean"))
                    .Add("ActionDELE", System.Type.GetType("System.Boolean"))
                    .Add("Action", System.Type.GetType("System.String"))
                    .Add("Bemerkung", System.Type.GetType("System.String"))
                    .Add("Anforderer", System.Type.GetType("System.String"))
                End With

                m_intStatus = 0
                m_strMessage = ""

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Schlue_Temp_Vers_Mahn_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Schlue_Temp_Vers_Mahn_001(Right("0000000000" & m_objUser.KUNNR, 10), CarDetailTable)
                objSAP.CommitWork()
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If

                If CarDetailTable.Count > 0 Then
                    Dim rowNew As DataRow
                    For Each CarDetail In CarDetailTable
                        'If CarDetail.Liznr.Length > 0 Then
                        rowNew = m_tblFahrzeuge.NewRow
                        rowNew("Equipmentnummer") = CarDetail.Equnr
                        rowNew("Fahrgestellnummer") = CarDetail.Chassis_Num
                        'rowNew("Versandanschirft") = CarDetail.Name1 & "," & CarDetail.Street & "," & CarDetail.Post_Code1 & "," & CarDetail.City1
                        rowNew("Versandanschrift") = CarDetail.Name1 & "," & CarDetail.Street & "," & CarDetail.Post_Code1 & "," & CarDetail.City1
                        rowNew("Kennzeichen") = CarDetail.License_Num
                        rowNew("Anforderer") = CarDetail.Ernam
                        Dim datTemp As DateTime
                        'Versanddatum
                        If Not CarDetail.Zztmpdt Is Nothing AndAlso IsNumeric(CarDetail.Zztmpdt) Then
                            datTemp = HelpProcedures.MakeDateStandard(CarDetail.Zztmpdt)
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Versanddatum") = Left(CStr(datTemp), 10)
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
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1234
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Unbekannter Fehler."
                End Select

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

                    m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Schlue_Set_Mahnsp_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                    Dim strEQU As String
                    strEQU = CType(tmpDataView.Item(i)("Equipmentnummer"), String)

                    objSAP.Z_M_Schlue_Set_Mahnsp_001(strEQU, Right("0000000000" & m_objUser.KUNNR, 10), "X")
                    objSAP.CommitWork()
                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                    End If

                Catch ex As Exception
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_intStatus = -1234
                            m_strMessage = "Equipment nicht gefunden."
                        Case "NO_UPDATE"
                            m_intStatus = -4554
                            m_strMessage = "Fehler bei der Aktualisierung."
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Unbekannter Fehler."
                    End Select
                    strBemerkung = m_strMessage
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
' $History: Change_03.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.07.07    Time: 9:51
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Lib
' 
' ************************************************
