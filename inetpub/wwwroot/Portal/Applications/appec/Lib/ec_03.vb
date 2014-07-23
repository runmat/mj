Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common
Public Class ec_03

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strHersteller As String
    Private m_tblHersteller As DataTable
#End Region

#Region " Properties"
    Public Property PHersteller() As String
        Get
            Return m_strHersteller
        End Get
        Set(ByVal Value As String)
            m_strHersteller = Value
        End Set
    End Property

    Public ReadOnly Property PHerstellerListe() As DataTable
        Get
            Return m_tblHersteller
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub



    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "EC_03.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
      
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Nur_Brief_Vorh", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

            S.AP.InitExecute("Z_M_Ec_Avm_Nur_Brief_Vorh", "I_KUNNR, I_HERST", Right("0000000000" & m_objUser.KUNNR, 10), m_strHersteller)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

            CreateOutPut(tblTemp2, strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", HERSTELLER=" & m_strHersteller, m_tblResult, False)
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
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", HERSTELLER=" & m_strHersteller & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Sub getHersteller(ByVal page As Web.UI.Page, ByVal strAppID As String, ByVal strSessionID As String)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Domaene_Herst", m_objApp, m_objUser, page)

                'myProxy.callBapi()

                'm_tblHersteller = myProxy.getExportTable("GT_HERST")

                S.AP.InitExecute("Z_M_Domaene_Herst")

                m_tblHersteller = S.AP.GetExportTable("GT_HERST")

                m_tblHersteller.Columns.Add("View", GetType(System.String))

                Dim row As DataRow
                For Each row In m_tblHersteller.Rows
                    row("View") = CStr(row("DOMVALUE_L")) & " - " & CStr(row("ZHERST"))
                Next
                m_tblHersteller.AcceptChanges()

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -1234
                        m_strMessage = "Fehler: Keine Daten gefunden!"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region
End Class

' ************************************************
' $History: ec_03.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 9.03.10    Time: 10:56
' Updated in $/CKAG/Applications/appec/Lib
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 26.06.09   Time: 15:28
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 unfertig, nicht kompilierbar
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
