Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class change_06

    Inherits Base.Business.BankBase

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


    Public Overloads Sub Show(ByVal page As Page)
        m_strClassAndMethod = "Change_06.Show"

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
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
            End With

            m_intStatus = 0
            m_strMessage = ""


            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Schlue_Temp_Vers_Mahn_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Schlue_Temp_Vers_Mahn_001", "I_KUNNR", m_strKUNNR)

            Dim CarDetail As DataRow
            'Dim CarDetailTable As DataTable = myProxy.getExportTable("GT_WEB")
            Dim CarDetailTable As DataTable = S.AP.GetExportTable("GT_WEB")

            If CarDetailTable.Rows.Count > 0 Then
                Dim rowNew As DataRow
                For Each CarDetail In CarDetailTable.Rows
                    'If CarDetail.Liznr.Length > 0 Then
                    rowNew = m_tblFahrzeuge.NewRow
                    rowNew("Equipmentnummer") = CarDetail("Equnr")
                    rowNew("Fahrgestellnummer") = CarDetail("Chassis_Num")
                    rowNew("Versandanschrift") = CarDetail("Name1").ToString & "," & CarDetail("Street").ToString & "," & CarDetail("Post_Code1").ToString & "," & CarDetail("City1").ToString
                    rowNew("Kennzeichen") = CarDetail("License_Num")
                    Dim datTemp As DateTime
                    'Versanddatum
                    If Not CarDetail("Zztmpdt") Is DBNull.Value AndAlso IsNumeric(CarDetail("Zztmpdt")) Then
                        datTemp = CDate(CarDetail("Zztmpdt"))
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
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Unbekannter Fehler."
            End Select
            WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & " , " & m_strMessage, m_tblFahrzeuge)
        End Try
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)


        m_intStatus = 0
        m_strMessage = ""

        Dim tmpDataView As DataView
        tmpDataView = m_tblFahrzeuge.DefaultView

        tmpDataView.RowFilter = "ActionNOTHING = 0"

        Dim i As Int32


        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Schlue_Set_Mahnsp_001", m_objApp, m_objUser, page)

        For i = 0 To tmpDataView.Count - 1
            Dim strBemerkung As String = "Vorgang erfolgreich"
            Dim strActionText As String = "Löschen"
            Try

                Dim strEQU As String
                strEQU = CType(tmpDataView.Item(i)("Equipmentnummer"), String)
                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_EQUNR", CType(tmpDataView.Item(i)("Equipmentnummer"), String))
                'myProxy.setImportParameter("I_ZZMANSP", "X")
                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Schlue_Set_Mahnsp_001", "I_KUNNR,I_EQUNR,I_ZZMANSP", m_strKUNNR, CType(tmpDataView.Item(i)("Equipmentnummer"), String), "X")
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
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

    End Sub

#End Region
End Class

' ************************************************
' $History: change_06.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 29.06.09   Time: 9:23
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 Z_M_Fehlend_Unfall_001, Z_M_Schlue_Set_Mahnsp_001
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.06.09   Time: 14:53
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918, Z_M_Ec_Avm_Zulauf, Z_V_FAHRZEUG_STATUS_001,
' Z_M_EC_AVM_STATUS_BESTAND, Z_M_ABMBEREIT_LAUFAEN,
' Z_M_ABMBEREIT_LAUFZEIT, Z_M_Brief_Temp_Vers_Mahn_001,
' Z_M_SCHLUE_SET_MAHNSP_001, Z_M_SCHLUESSELDIFFERENZEN,
' Z_M_SCHLUESSELVERLOREN, Z_M_SCHLUE_TEMP_VERS_MAHN_001,
' Z_M_Ec_Avm_Status_Zul,  Z_M_ECA_TAB_BESTAND
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Lib
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
