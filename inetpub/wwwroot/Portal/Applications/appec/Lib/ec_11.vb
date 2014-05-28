Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class ec_11
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Zugelassene_Fahrzeuge,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strHersteller As String
    Private m_strPDI As String
#End Region

#Region " Properties"
    Public Property PEingangsdatumVon() As DateTime
        Get
            Return m_datEingangsdatumVon
        End Get
        Set(ByVal Value As DateTime)
            m_datEingangsdatumVon = Value
        End Set
    End Property

    Public Property PEingangsdatumBis() As DateTime
        Get
            Return m_datEingangsdatumBis
        End Get
        Set(ByVal Value As DateTime)
            m_datEingangsdatumBis = Value
        End Set
    End Property

    Public Property PHersteller() As String
        Get
            Return m_strHersteller
        End Get
        Set(ByVal Value As String)
            m_strHersteller = Value
        End Set
    End Property

    Public Property PPDI() As String
        Get
            Return m_strPDI
        End Get
        Set(ByVal Value As String)
            m_strPDI = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "ec_11.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
     
        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Zulassungen_2", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_PDI", m_strPDI)
            'myProxy.setImportParameter("I_ZULDAT_VON", m_datEingangsdatumVon.ToShortDateString)
            'myProxy.setImportParameter("I_ZULDAT_BIS", m_datEingangsdatumBis.ToShortDateString)
            'myProxy.setImportParameter("I_MAKE", m_strHersteller)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Ec_Avm_Zulassungen_2", "I_KUNNR,I_PDI,I_ZULDAT_VON,I_ZULDAT_BIS,I_MAKE", Right("0000000000" & m_objUser.KUNNR, 10), _
                             m_strPDI, m_datEingangsdatumVon.ToShortDateString, m_datEingangsdatumBis.ToShortDateString, m_strHersteller)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            CreateOutPut(tblTemp2, strAppID)

            WriteLogEntry(True, "VDATU_VON=" & m_datEingangsdatumVon.ToShortDateString & ", VDATU_BIS=" & m_datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZCARPORT=" & m_strPDI & ", I_MAKE=" & m_strHersteller, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_PARAMETER"
                    m_strMessage = "Kein Suchkriterium gefüllt."
                Case "NO_DATA"
                    m_strMessage = "Keine Ausgabedaten gefunden."
                Case "NO_KORREKTPARAMETER"
                    m_strMessage = "Wenn nach Fahrgestellnummer gesucht wird, kann nicht gleichzeitig nach PDI und/oder Zulassungsdatum gesucht werden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "VDATU_VON=" & m_datEingangsdatumVon.ToShortDateString & ", VDATU_BIS=" & m_datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZCARPORT=" & m_strPDI & ", I_MAKE=" & m_strHersteller & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

#End Region
End Class

' ************************************************
' $History: ec_11.vb $
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
