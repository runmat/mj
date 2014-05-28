Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business


' Anfroderungsnummer 1067
' Erstellt am: 01.06.2007 - Sven Faﬂbender
<Serializable()> _
Public Class FDD_AbgleichRetail

    Inherits Base.Business.DatenimportBase



    '#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)

    End Sub


#Region "Declarations"

    Private m_dteBaseDate As Date

#End Region

#Region "Methods"

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)


        Dim strKUNNR As String = Right("0000000000" & Me.m_objUser.Customer.KUNNR, 10)
        Try

            m_intStatus = 0
            m_strMessage = ""
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Abgleich_Retail", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_Csc", "0000324616")
            'myProxy.setImportParameter("I_KUNNR_Rt", strKUNNR)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_V_Abgleich_Retail", "I_KUNNR_Csc,I_KUNNR_Rt", "0000324616", strKUNNR)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("OT_OUT") 'myProxy.getExportTable("OT_OUT")

            CreateOutPut(tblTemp2, strAppID)

            WriteLogEntry(True, "KUNNR=" & strKUNNR, m_tblResult, False)

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NRF"
                    m_intStatus = -1402
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -5555
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & strKUNNR & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try


    End Sub

#End Region


End Class

' ************************************************
' $History: FFD_AbgleichRetail.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.06.09   Time: 11:35
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 13.06.07   Time: 17:03
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Portal - Startapplication 13.06.2005
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Beyond Compare
' 
' ************************************************
