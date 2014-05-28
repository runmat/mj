Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class ec_16

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strPDI As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strModell As String
    Private m_tblResultPDIs As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultPDIs() As DataTable
        Get
            Return m_tblResultPDIs
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strPDI As String, ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal strFahrgestellnummer As String, ByVal strModell As String, ByVal page As Page)

        m_strClassAndMethod = "ec_16.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_PDI", strPDI)
            'myProxy.setImportParameter("I_ZZDAT_EIN_VON", datEingangsdatumVon.ToShortDateString)
            'myProxy.setImportParameter("I_ZZDAT_EIN_BIS", datEingangsdatumBis.ToShortDateString)
            'myProxy.setImportParameter("I_ZZFAHRG", strFahrgestellnummer)
            'myProxy.setImportParameter("I_ZMODELL", strModell)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul", "I_KUNNR,I_PDI,I_ZZDAT_EIN_VON,I_ZZDAT_EIN_BIS,I_ZZFAHRG,I_ZMODELL", Right("0000000000" & m_objUser.KUNNR, 10), _
                             strPDI, datEingangsdatumVon.ToShortDateString, datEingangsdatumBis.ToShortDateString, strFahrgestellnummer, strModell)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            CreateOutPut(tblTemp2, strAppID)
            m_tblResultPDIs = New DataTable()
            With m_tblResultPDIs
                .Columns.Add("PDINummer", System.Type.GetType("System.String"))
                .Columns.Add("PDIName", System.Type.GetType("System.String"))
                .Columns.Add("Fahrzeuge", System.Type.GetType("System.Int32"))
            End With

            Dim rowResult As DataRow
            Dim rowPDI As DataRow()
            Dim rowNew As DataRow
           
            For Each rowResult In m_tblResult.Rows
                rowPDI = m_tblResultPDIs.Select("PDINummer='" & CStr(rowResult("PDI")) & "'")
                If (rowPDI.Length = 0) Then
                    'PDI noch nicht gefunden
                    rowNew = m_tblResultPDIs.NewRow
                    rowNew("PDINummer") = CStr(rowResult("PDI"))
                    rowNew("PDIName") = CStr(rowResult("PDIName"))
                    rowNew("Fahrzeuge") = 1
                    m_tblResultPDIs.Rows.Add(rowNew)
                Else
                    'PDI gefunden, Anzahl hochsetzen
                    rowPDI(0)("Fahrzeuge") = 1 + CInt(rowPDI(0)("Fahrzeuge"))
                End If
            Next
            m_tblResultPDIs.AcceptChanges()
            WriteLogEntry(True, "ZZDAT_EIN_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZDAT_EIN_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZKUNPDI=" & strPDI & ", ZZFAHRG=" & strFahrgestellnummer & ", ZZBEZEI=" & strModell, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -4444
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "ZZDAT_EIN_VON=" & datEingangsdatumVon.ToShortDateString & ", ZZDAT_EIN_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZKUNPDI=" & strPDI & ", ZZFAHRG=" & strFahrgestellnummer & ", ZZBEZEI=" & strModell & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub


#End Region
End Class

' ************************************************
' $History: ec_16.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.06.09   Time: 11:29
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918
' Z_M_EC_AVM_BRIEFLEBENSLAUF,Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul,Z_M_EC_AVM_FZG_OH
' NE_BRIEF,Z_M_Ec_Avm_Fzg_Ohne_Unitnr,Z_M_Ec_Avm_Nur_Brief_Vorh,
' Z_M_EC_AVM_OFFENE_ZAHLUNGEN,  Z_M_EC_AVM_PDI_BESTAND,
' Z_M_EC_AVM_STATUS_EINSTEUERUNG,  Z_M_EC_AVM_STATUS_GREENWAY,
' Z_M_Ec_Avm_Status_Zul, Z_M_EC_AVM_ZULASSUNGEN, Z_M_Ec_Avm_Zulassungen_2
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
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
