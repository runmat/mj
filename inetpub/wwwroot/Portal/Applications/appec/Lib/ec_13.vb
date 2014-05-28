Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class ec_13

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datErfassungsdatumVon As DateTime
    Private m_datErfassungsdatumBis As DateTime
    Private m_tblHistory As DataTable
#End Region

#Region " Properties"
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


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Dim row As DataRow

        m_strClassAndMethod = "ec_13.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Offene_Zahlungen", m_objApp, m_objUser, page)
            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()
            S.AP.InitExecute("Z_M_Ec_Avm_Offene_Zahlungen", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            CreateOutPut(tblTemp2, strAppID)

            m_tblResult.Columns.Add("Status", GetType(System.String))

            For Each row In m_tblResult.Rows
                row("Status") = String.Empty
            Next

            m_tblResult.AcceptChanges()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        End Try
    End Sub


#End Region
End Class

' ************************************************
' $History: ec_13.vb $
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
