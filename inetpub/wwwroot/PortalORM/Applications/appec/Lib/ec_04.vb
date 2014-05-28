Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common
Public Class ec_04

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datErfassungsdatumVon As DateTime
    Private m_tblHistory As DataTable
    Private m_strFilename2 As String
#End Region

#Region " Properties"
    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return m_strFilename2
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        'wiso bekomme ich den filename den ich beim erzeugen des Reportobjektes übergeben musste nicht wieder zurück? Property Eingebaut in EC_04 JJ2007.11.21
        m_strFilename2 = strFilename
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub


    Public Sub FillHersteller(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        m_strAppID = AppID
        m_strSessionID = SessionID
        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Herstellergroup", m_objApp, m_objUser, Page)
            'myProxy.callBapi()
            'm_tblResult = myProxy.getExportTable("T_HERST")

            S.AP.InitExecute("Z_M_Herstellergroup")
            m_tblResult = S.AP.GetExportTable("T_HERST")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Hersteller gefunden"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strErfassungsdatumVon As String, ByVal strErfassungsdatumBis As String, ByVal strHerstellerID As String, ByVal page As Page)
        m_strClassAndMethod = "EC_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Zulauf", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("ZUL_DAT", strErfassungsdatumVon)
            'myProxy.setImportParameter("ZUL_BIS", strErfassungsdatumBis)
            'myProxy.setImportParameter("I_HERSTNR", strHerstellerID)

            'myProxy.callBapi()
            'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

            S.AP.InitExecute("Z_M_Ec_Avm_Zulauf", "ZUL_DAT,ZUL_BIS,I_HERSTNR", strErfassungsdatumVon, strErfassungsdatumBis, strHerstellerID)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
            CreateOutPut(tblTemp2, strAppID)

            Dim tblOutput As New DataTable()
            Dim rowNew As DataRow
            Dim vwResult As DataView
            Dim intIndex As Integer

            vwResult = m_tblResult.DefaultView
            vwResult.Sort = "Hersteller, Modellcode, Fahrgestellnummer"     'Sortierung

            With tblOutput.Columns
                .Add("Datum Zulauf", GetType(System.DateTime))
                .Add("Uhrzeit Zulauf", GetType(System.String))
                .Add("Eingang ZBII", GetType(System.DateTime))
                .Add("Eingang PDI", GetType(System.DateTime))
                .Add("Hersteller", GetType(System.String))
                .Add("Modellcode", GetType(System.String))
                .Add("Modellbezeichnung", GetType(System.String))
                .Add("Fahrgestellnummer", GetType(System.String))
                .Add("Auftragsnummer", GetType(System.String))
                .Add("Unit-Nummer", GetType(System.String))
            End With

            For intIndex = 0 To vwResult.Count - 1                          'Ausgabetabelle aufbauen
                rowNew = tblOutput.NewRow

                rowNew("Datum Zulauf") = CDate(vwResult.Item(intIndex)("Datum Zulauf"))
                If Not (CStr(vwResult.Item(intIndex)("Uhrzeit Zulauf")).TrimStart("0"c).Trim = String.Empty) Then
                    rowNew("Uhrzeit Zulauf") = Left(CStr(vwResult.Item(intIndex)("Uhrzeit Zulauf")), 2) & ":" & CStr(vwResult.Item(intIndex)("Uhrzeit Zulauf")).Substring(2, 2)   '§§§ JVE 02.10.2006
                Else
                    rowNew("Uhrzeit Zulauf") = System.DBNull.Value
                End If
                rowNew("Eingang ZBII") = vwResult.Item(intIndex)("Eingang ZBII")
                rowNew("Eingang PDI") = vwResult.Item(intIndex)("Eingang PDI")
                rowNew("Hersteller") = CStr(vwResult.Item(intIndex)("Hersteller"))
                rowNew("Modellcode") = CStr(vwResult.Item(intIndex)("Modellcode"))
                rowNew("Modellbezeichnung") = CStr(vwResult.Item(intIndex)("Modellbezeichnung"))
                rowNew("Fahrgestellnummer") = CStr(vwResult.Item(intIndex)("Fahrgestellnummer"))
                rowNew("Unit-Nummer") = CStr(vwResult.Item(intIndex)("Unit-Nummer")) & CStr(vwResult.Item(intIndex)("Prüfziffer"))
                rowNew("Auftragsnummer") = CStr(vwResult.Item(intIndex)("Auftragsnummer"))
                tblOutput.Rows.Add(rowNew)
            Next
            tblOutput.AcceptChanges()

            m_tblResult = tblOutput

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & strErfassungsdatumVon, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_FLEET"
                    m_strMessage = "Keine Fleet Daten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & strErfassungsdatumVon & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Sub FillPDIZulauf(ByVal strAppID As String, ByVal strSessionID As String, ByVal strErfassungsdatumVon As String, ByVal strErfassungsdatumBis As String, ByVal strHerstellerID As String, ByVal page As Page)
        m_strClassAndMethod = "EC_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_EC_AVM_PDI_ZULAUF_01", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, "0"c))
            'myProxy.setImportParameter("I_EING_VON", strErfassungsdatumVon)
            'myProxy.setImportParameter("I_EING_BIS", strErfassungsdatumBis)
            'myProxy.setImportParameter("I_HERST_SCHL", strHerstellerID)

            'myProxy.callBapi()
            'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

            S.AP.InitExecute("Z_DPM_EC_AVM_PDI_ZULAUF_01", "I_KUNNR,I_EING_VON,I_EING_BIS,I_HERST_SCHL", _
                             m_objUser.KUNNR.PadLeft(10, "0"c), strErfassungsdatumVon, strErfassungsdatumBis, strHerstellerID)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
            CreateOutPut(tblTemp2, strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & strErfassungsdatumVon, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "ERR_ZEITR"
                    m_strMessage = "Zeitraum unzulässig."
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & strErfassungsdatumVon & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try


    End Sub
   
#End Region
End Class

' ************************************************
' $History: ec_04.vb $
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 1.03.11    Time: 11:39
' Updated in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 26.06.09   Time: 14:53
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918, Z_M_Ec_Avm_Zulauf, Z_V_FAHRZEUG_STATUS_001,
' Z_M_EC_AVM_STATUS_BESTAND, Z_M_ABMBEREIT_LAUFAEN,
' Z_M_ABMBEREIT_LAUFZEIT, Z_M_Brief_Temp_Vers_Mahn_001,
' Z_M_SCHLUE_SET_MAHNSP_001, Z_M_SCHLUESSELDIFFERENZEN,
' Z_M_SCHLUESSELVERLOREN, Z_M_SCHLUE_TEMP_VERS_MAHN_001,
' Z_M_Ec_Avm_Status_Zul,  Z_M_ECA_TAB_BESTAND
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 25.06.09   Time: 17:20
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 tests und anpassungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.06.09   Time: 16:04
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 Z_M_Herstellergroup, Z_M_EC_AVM_BATCH_update,
' Z_M_EC_AVM_HERST_VWZWECK_MODID, Z_M_EC_AVM_BATCH_INSERT,
' Z_M_EC_AVM_BATCH_ANSICHT
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Lib
' Warnungen entfernt!
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 19.05.08   Time: 13:39
' Updated in $/CKAG/Applications/appec/Lib
' ITA 1920
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 21.11.07   Time: 9:49
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 20.11.07   Time: 15:34
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 10  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
