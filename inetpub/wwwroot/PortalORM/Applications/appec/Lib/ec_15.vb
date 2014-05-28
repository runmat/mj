Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class ec_15
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Briefeohnefzg2 (Fill),
    REM § BAPI: Z_M_Brieflebenslauf (FillHistory),
    REM § Ausgabetabelle per Zuordnung in Web-DB (Fill), Direkte Rückgabe (FillHistory).

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strBriefnummer As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strHaendlerID As String
    Private m_tblHistory As DataTable
    Private m_tblResultFahrzeuge As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultFahrzeuge() As DataTable
        Get
            Return m_tblResultFahrzeuge
        End Get
    End Property

    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, _
                            ByVal strFahrgestellnummer As String)
        m_strClassAndMethod = "ec_15.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)
            Dim strZZFAHRG As String = strFahrgestellnummer

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEFLEBENSLAUF_TUETE", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_VKORG", "1510")
                'myProxy.setImportParameter("I_ZZFAHRG", strZZFAHRG)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_BRIEFLEBENSLAUF_TUETE", "I_KUNNR,I_VKORG,I_ZZFAHRG", strKUNNR, "1510", strZZFAHRG)

                m_tblResult = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZFAHRG=" & strZZFAHRG, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZFAHRG=" & strZZFAHRG & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub


    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String, ByVal page As Page)
        m_strClassAndMethod = "ec_15.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Brieflebenslauf", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_VKORG", "1510")
            'myProxy.setImportParameter("I_ZZKENN", UCase(strAmtlKennzeichen))
            'myProxy.setImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer))
            'myProxy.setImportParameter("I_ZZBRIEF", UCase(strBriefnummer))
            'myProxy.setImportParameter("I_ZZREF1", UCase(strOrdernummer))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Ec_Avm_Brieflebenslauf", "I_KUNNR,I_VKORG,I_ZZKENN,I_ZZFAHRG,I_ZZBRIEF,I_ZZREF1", Right("0000000000" & m_objUser.KUNNR, 10), _
                             "1510", UCase(strAmtlKennzeichen), UCase(strFahrgestellnummer), UCase(strBriefnummer), UCase(strOrdernummer))

            m_tblHistory = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            m_tblResultFahrzeuge = S.AP.GetExportTable("GT_FAHRG") 'myProxy.getExportTable("GT_FAHRG")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen), m_tblHistory, False)

        Catch ex As Exception
            m_intStatus = -2222
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ZZBRIEF=" & UCase(strBriefnummer) & ", ZFAHRG=" & UCase(strFahrgestellnummer) & ", ZZREF1=" & UCase(strOrdernummer) & ", ZZKENN=" & UCase(strAmtlKennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblHistory, False)
        End Try
    End Sub


#End Region
End Class

' ************************************************
' $History: ec_15.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 29.03.10   Time: 13:24
' Updated in $/CKAG/Applications/appec/Lib
' ITA: 3552
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 9.03.10    Time: 14:00
' Updated in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 26.06.09   Time: 11:29
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918
' Z_M_EC_AVM_BRIEFLEBENSLAUF,Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul,Z_M_EC_AVM_FZG_OH
' NE_BRIEF,Z_M_Ec_Avm_Fzg_Ohne_Unitnr,Z_M_Ec_Avm_Nur_Brief_Vorh,
' Z_M_EC_AVM_OFFENE_ZAHLUNGEN,  Z_M_EC_AVM_PDI_BESTAND,
' Z_M_EC_AVM_STATUS_EINSTEUERUNG,  Z_M_EC_AVM_STATUS_GREENWAY,
' Z_M_Ec_Avm_Status_Zul, Z_M_EC_AVM_ZULASSUNGEN, Z_M_Ec_Avm_Zulassungen_2
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
